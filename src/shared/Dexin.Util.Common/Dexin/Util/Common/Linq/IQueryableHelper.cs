using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Dexin.Util.Common.Linq
{
	public static class IQueryableHelper
	{
		private static LambdaExpression GenerateSelector<TEntity>(string propertyName, out Type resultType)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), "Entity");
			PropertyInfo property;
			Expression expression;
			if (propertyName.Contains('.'))
			{
				string[] array = propertyName.Split('.');
				property = typeof(TEntity).GetProperty(array[0]);
				expression = Expression.MakeMemberAccess(parameterExpression, property);
				for (int i = 1; i < array.Length; i++)
				{
					property = property.PropertyType.GetProperty(array[i]);
					expression = Expression.MakeMemberAccess(expression, property);
				}
			}
			else
			{
				property = typeof(TEntity).GetProperty(propertyName);
				expression = Expression.MakeMemberAccess(parameterExpression, property);
			}
			resultType = property.PropertyType;
			return Expression.Lambda(expression, parameterExpression);
		}

		private static MethodCallExpression GenerateMethodCall<TEntity>(IQueryable<TEntity> source, string methodName, string fieldName) where TEntity : class
		{
			Type typeFromHandle = typeof(TEntity);
			Type resultType;
			LambdaExpression expression = GenerateSelector<TEntity>(fieldName, out resultType);
			return Expression.Call(typeof(Queryable), methodName, new Type[2] { typeFromHandle, resultType }, source.Expression, Expression.Quote(expression));
		}

		public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string fieldName) where TEntity : class
		{
			MethodCallExpression expression = GenerateMethodCall(source, "OrderBy", fieldName);
			return source.Provider.CreateQuery<TEntity>(expression) as IOrderedQueryable<TEntity>;
		}

		public static IOrderedQueryable<TEntity> OrderByDescending<TEntity>(this IQueryable<TEntity> source, string fieldName) where TEntity : class
		{
			MethodCallExpression expression = GenerateMethodCall(source, "OrderByDescending", fieldName);
			return source.Provider.CreateQuery<TEntity>(expression) as IOrderedQueryable<TEntity>;
		}

		public static IOrderedQueryable<TEntity> ThenBy<TEntity>(this IOrderedQueryable<TEntity> source, string fieldName) where TEntity : class
		{
			MethodCallExpression expression = GenerateMethodCall(source, "ThenBy", fieldName);
			return source.Provider.CreateQuery<TEntity>(expression) as IOrderedQueryable<TEntity>;
		}

		public static IOrderedQueryable<TEntity> ThenByDescending<TEntity>(this IOrderedQueryable<TEntity> source, string fieldName) where TEntity : class
		{
			MethodCallExpression expression = GenerateMethodCall(source, "ThenByDescending", fieldName);
			return source.Provider.CreateQuery<TEntity>(expression) as IOrderedQueryable<TEntity>;
		}

		public static IOrderedQueryable<TEntity> OrderUsingSortExpression<TEntity>(this IQueryable<TEntity> source, string sortExpression) where TEntity : class
		{
			string[] array = sortExpression.Split(',');
			IOrderedQueryable<TEntity> orderedQueryable = null;
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Trim().Split(' ');
				string fieldName = array2[0];
				orderedQueryable = ((array2.Length != 2 || !array2[1].Equals("DESC", StringComparison.OrdinalIgnoreCase)) ? ((i == 0) ? source.OrderBy(fieldName) : orderedQueryable.ThenBy(fieldName)) : ((i == 0) ? source.OrderByDescending(fieldName) : orderedQueryable.ThenByDescending(fieldName)));
			}
			return orderedQueryable;
		}

		public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortExpression, bool ascending)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(T), sortExpression);
			Type resultType;
			LambdaExpression expression = GenerateSelector<T>(sortExpression, out resultType);
			Expression expression2 = Expression.Call(typeArguments: new Type[2]
			{
				typeof(T),
				resultType
			}, type: typeof(Queryable), methodName: ascending ? "OrderBy" : "OrderByDescending", arguments: new Expression[2]
			{
				source.Expression,
				Expression.Quote(expression)
			});
			return source.Provider.CreateQuery<T>(expression2);
		}

		public static IList<T> Query<T>(this IQueryable<T> total, PagingInfo paging, bool skipOrder = false)
		{
			if (paging == null)
			{
				return total.ToList();
			}
			paging.TotalItems = total.Count();
			paging.ValidatePaging();
			if (!skipOrder && !string.IsNullOrEmpty(paging.OrderField))
			{
				total = total.OrderBy(paging.OrderField, paging.OrderDirection == OrderDirections.asc);
			}
			if (paging.NumPerPage > 0)
			{
				total = total.Skip((paging.PageNum - 1) * paging.NumPerPage).Take(paging.NumPerPage);
			}
			return total.ToList();
		}
	}
}
