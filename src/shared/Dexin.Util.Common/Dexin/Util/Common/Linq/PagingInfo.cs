using System;

namespace Dexin.Util.Common.Linq
{
	public class PagingInfo
	{
		private int _TotalItems;

		private string _OrderField = string.Empty;

		public int TotalItems
		{
			get
			{
				return _TotalItems;
			}
			set
			{
				_TotalItems = value;
				if (PageNum > TotalPages)
				{
					PageNum = TotalPages;
				}
			}
		}

		public int NumPerPage { get; set; }

		public int PageNum { get; set; }

		public int TotalPages
		{
			get
			{
				return (NumPerPage == 0) ? 1 : ((int)Math.Ceiling((decimal)TotalItems / (decimal)NumPerPage));
			}
		}

		public string OrderField
		{
			get
			{
				return _OrderField;
			}
			set
			{
				_OrderField = ((value == null) ? string.Empty : value);
			}
		}

		public OrderDirections OrderDirection { get; set; }

		public int[] numPerPageArray { get; set; }

		public PagingInfo()
		{
			TotalItems = 0;
			NumPerPage = 20;
			PageNum = 1;
			OrderField = string.Empty;
			OrderDirection = OrderDirections.asc;
			numPerPageArray = new int[5] { 1, 20, 50, 100, 200 };
		}

		public virtual void ValidatePaging()
		{
			if (PageNum > TotalPages)
			{
				PageNum = TotalPages;
			}
			if (OrderField == null)
			{
				OrderField = string.Empty;
			}
		}
	}
}
