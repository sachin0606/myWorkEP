using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;

namespace Dexin.Util.Common.Excel
{
	public class ExcelHelper
	{
		private static string GenerateExcelConnectionString(string path, bool? hasHeader = null)
		{
			string empty = string.Empty;
			if (!hasHeader.HasValue)
			{
				return string.Format("Provider = Microsoft.Jet.OLEDB.4.0; Data Source ={0};Extended Properties=Excel 8.0", path);
			}
			if (path.ToLower().EndsWith(".xls"))
			{
				empty = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1};IMEX=1'", path, hasHeader.Value ? "YES" : "NO");
			}
			else
			{
				if (!path.ToLower().EndsWith(".xlsx"))
				{
					throw new NullReferenceException("Excel path is not legal!");
				}
				empty = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR={1};IMEX=1'", path, hasHeader.Value ? "YES" : "NO");
			}
			return empty;
		}

		public static DataSet GetdataFromExcel(string path, bool hasHeader)
		{
			DataSet dataSet = new DataSet();
			using (OleDbConnection oleDbConnection = new OleDbConnection(GenerateExcelConnectionString(path, hasHeader)))
			{
				if (oleDbConnection.State != ConnectionState.Open)
				{
					oleDbConnection.Open();
				}
				DataTable oleDbSchemaTable = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[4] { null, null, null, "TABLE" });
				foreach (DataRow row in oleDbSchemaTable.Rows)
				{
					string text = row["TABLE_NAME"].ToString();
					if (text.EndsWith("$"))
					{
						string selectCommandText = string.Format("select * from [{0}]", text);
						OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(selectCommandText, oleDbConnection);
						oleDbDataAdapter.Fill(dataSet);
					}
				}
			}
			return dataSet;
		}

		public static void SavedataToExcelOledb(string path, DataTable excelData)
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			using (OleDbConnection oleDbConnection = new OleDbConnection(GenerateExcelConnectionString(path)))
			{
				if (oleDbConnection.State != ConnectionState.Open)
				{
					oleDbConnection.Open();
				}
				StringBuilder stringBuilder = new StringBuilder("CREATE TABLE " + excelData.TableName + " (");
				foreach (DataColumn column in excelData.Columns)
				{
					stringBuilder.Append("[" + column.ColumnName + "] memo ,");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				stringBuilder.Append(")");
				new OleDbCommand(stringBuilder.ToString(), oleDbConnection).ExecuteNonQuery();
				OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter("select * from " + excelData.TableName, oleDbConnection);
				OleDbCommandBuilder oleDbCommandBuilder = new OleDbCommandBuilder(oleDbDataAdapter);
				oleDbCommandBuilder.QuotePrefix = "[";
				oleDbCommandBuilder.QuoteSuffix = "]";
				DataTable dataTable = new DataTable();
				oleDbDataAdapter.Fill(excelData);
				oleDbDataAdapter.Update(excelData);
			}
		}

		public static void SavedataToExcel(string path, DataTable excelData)
		{
			using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.Default))
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (DataColumn column in excelData.Columns)
				{
					stringBuilder.Append(column.ColumnName + "\t");
				}
				stringBuilder.Append(Environment.NewLine);
				foreach (DataRow row in excelData.Rows)
				{
					foreach (DataColumn column2 in excelData.Columns)
					{
						stringBuilder.Append("'" + row[column2.ColumnName].ToString() + "\t");
					}
					stringBuilder.Append(Environment.NewLine);
				}
				streamWriter.Write(stringBuilder.ToString());
				streamWriter.Flush();
			}
		}

		public static DataTable CreatDataTable(IList<string> columnNames)
		{
			if (columnNames == null)
			{
				throw new Exception("columnNames is not null");
			}
			DataTable dataTable = new DataTable();
			DataColumn dataColumn = null;
			foreach (string columnName in columnNames)
			{
				dataColumn = new DataColumn(columnName, typeof(string));
				dataTable.Columns.Add(dataColumn);
			}
			return dataTable;
		}
	}
}
