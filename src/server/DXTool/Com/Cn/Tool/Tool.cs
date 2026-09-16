using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using Com.Cn.Tool.Model;
using Microsoft.Reporting.WinForms;

namespace Com.Cn.Tool
{
	public class Tool
	{
		private static ErrorProvider errorProvider;

		private static object locObject = new object();

		public static string AppPath = AppDomain.CurrentDomain.BaseDirectory;

		public static Enum GetEnum(Type type, string typekey)
		{
			return (Enum)Enum.Parse(type, typekey, true);
		}

		public static int GetValueOfEnumByName(Type type, string name)
		{
			int num = 0;
			return Convert.ToInt32(Enum.Parse(type, name, true));
		}

		public static string GetString(object obj)
		{
			if (obj != null)
			{
				return obj.ToString();
			}
			return "";
		}

		public static int GetLength(object obj)
		{
			return GetString(obj).Length;
		}

		public static int GetLength(object[] obj)
		{
			if (obj != null)
			{
				return obj.Length;
			}
			return 0;
		}

		public static byte GetByte(object obj)
		{
			byte b = 0;
			try
			{
				return Convert.ToByte(obj);
			}
			catch
			{
				return 0;
			}
		}

		public static byte[] GetBytes(object obj)
		{
			byte[] array = new byte[0];
			try
			{
				array = obj as byte[];
				if (array == null)
				{
					array = new byte[0];
				}
			}
			catch
			{
				array = new byte[0];
			}
			return array;
		}

		public static short GetInt16(object obj)
		{
			short num = 0;
			try
			{
				return Convert.ToInt16(obj);
			}
			catch
			{
				return 0;
			}
		}

		public static ushort GetUInt16(object obj)
		{
			ushort num = 0;
			try
			{
				return Convert.ToUInt16(obj);
			}
			catch
			{
				return 0;
			}
		}

		public static int GetInt32(object obj)
		{
			int num = 0;
			try
			{
				return Convert.ToInt32(obj);
			}
			catch
			{
				return 0;
			}
		}

		public static long GetInt64(object obj)
		{
			long num = 0L;
			try
			{
				return Convert.ToInt64(obj);
			}
			catch
			{
				return 0L;
			}
		}

		public static DateTime? GetDateTime1(object obj)
		{
			if (obj is DBNull || obj == null || GetLength(obj) == 0)
			{
				return null;
			}
			return Convert.ToDateTime(obj);
		}

		public static DateTime GetDateTime(object obj)
		{
			DateTime now = DateTime.Now;
			try
			{
				return Convert.ToDateTime(obj);
			}
			catch
			{
				return DateTime.MinValue;
			}
		}

		public static DateTime GetDate(object obj)
		{
			DateTime minValue = DateTime.MinValue;
			try
			{
				return Convert.ToDateTime(obj);
			}
			catch
			{
				return DateTime.MinValue;
			}
		}

		public static double GetDouble(object obj)
		{
			double num = 0.0;
			try
			{
				return Convert.ToDouble(obj);
			}
			catch
			{
				return 0.0;
			}
		}

		public static decimal GetDecimal(object obj)
		{
			decimal num = 0m;
			try
			{
				return Convert.ToDecimal(obj);
			}
			catch
			{
				return 0m;
			}
		}

		public static bool GetBoolean(object obj)
		{
			bool flag = false;
			try
			{
				return Convert.ToBoolean(obj);
			}
			catch
			{
				return false;
			}
		}

		public static DateTime FormatDateTimeStart(object dt)
		{
			DateTime result = DateTime.MinValue;
			try
			{
				if (dt != null)
				{
					DateTime dateTime = Convert.ToDateTime(dt);
					result = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				}
			}
			catch
			{
				result = DateTime.MinValue;
			}
			return result;
		}

		public static DateTime FormatDateTimeEnd(object dt)
		{
			DateTime result = DateTime.Now;
			try
			{
				if (dt != null)
				{
					DateTime dateTime = Convert.ToDateTime(dt);
					result = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
				}
			}
			catch
			{
				result = DateTime.Now;
			}
			return result;
		}

		public static string FormatDateTime(DateTime? dt, string formatString)
		{
			if (dt.HasValue)
			{
				return Convert.ToDateTime(dt).ToString(formatString);
			}
			return "";
		}

		public static string FormatDateTime(DateTime dt, string formatString)
		{
			return dt.ToString(formatString);
		}

		public static string FormatNumber(object obj, string FormatString)
		{
			string text = "";
			decimal num = 0m;
			try
			{
				num = Convert.ToDecimal(obj);
				return string.Format(FormatString, num);
			}
			catch
			{
				return "";
			}
		}

		public static string FormatNumber(decimal obj, string FormatString)
		{
			string text = "";
			try
			{
				return obj.ToString(FormatString);
			}
			catch
			{
				return "";
			}
		}

		public static string FormatNumber(byte obj, string FormatString)
		{
			string text = "";
			try
			{
				return obj.ToString(FormatString);
			}
			catch
			{
				return "";
			}
		}

		public static string FormatNumber(short obj, string FormatString)
		{
			string text = "";
			try
			{
				return obj.ToString(FormatString);
			}
			catch
			{
				return "";
			}
		}

		public static string FormatNumber(int obj, string FormatString)
		{
			string text = "";
			try
			{
				return obj.ToString(FormatString);
			}
			catch
			{
				return "";
			}
		}

		public static string FormatNumber(long obj, string FormatString)
		{
			string text = "";
			try
			{
				return obj.ToString(FormatString);
			}
			catch
			{
				return "";
			}
		}

		public static string FormatNumber1(object obj, string FormatString)
		{
			string text = "0";
			decimal num = 0m;
			try
			{
				num = Convert.ToDecimal(obj);
				return (num == 0m) ? "0" : num.ToString(FormatString);
			}
			catch
			{
				return "0";
			}
		}

		public static string FormatMoney(object obj)
		{
			string text = ((obj == null) ? "" : obj.ToString());
			try
			{
				return Convert.ToDecimal(obj).ToString(FormatString.Ins.Number2);
			}
			catch
			{
				return (obj == null) ? "" : obj.ToString();
			}
		}

		public static decimal FormatMoneyToDecimal(object obj)
		{
			decimal num = 0m;
			try
			{
				string s = ((obj == null) ? "0" : obj.ToString());
				return decimal.Parse(s, NumberStyles.Currency);
			}
			catch
			{
				return 0m;
			}
		}

		public static string FormatBytes(byte[] args)
		{
			if (args != null)
			{
				return Encoding.Unicode.GetString(args);
			}
			return "";
		}

		public static byte[] FormatBytes(string args)
		{
			if (args != null)
			{
				return Encoding.Unicode.GetBytes(args);
			}
			return new byte[0];
		}

		public static byte ToNumberByte(string args, bool isThrowException)
		{
			byte result = 0;
			if (!byte.TryParse(args, NumberStyles.HexNumber, Thread.CurrentThread.CurrentCulture, out result))
			{
				if (isThrowException)
				{
					throw new CustomException("Convert Failure!");
				}
				result = 0;
			}
			return result;
		}

		public static byte ToNumberByte(string args)
		{
			byte result = 0;
			if (!byte.TryParse(args, NumberStyles.HexNumber, Thread.CurrentThread.CurrentCulture, out result))
			{
				result = 0;
			}
			return result;
		}

		public static short ToNumber16(string args, bool isThrowException)
		{
			short result = 0;
			if (!short.TryParse(args, NumberStyles.HexNumber, Thread.CurrentThread.CurrentCulture, out result))
			{
				if (isThrowException)
				{
					throw new CustomException("转化失败!");
				}
				result = 0;
			}
			return result;
		}

		public static short ToNumber16(string args)
		{
			short result = 0;
			if (!short.TryParse(args, NumberStyles.HexNumber, Thread.CurrentThread.CurrentCulture, out result))
			{
				result = 0;
			}
			return result;
		}

		public static int ToNumber32(string args, bool isThrowException)
		{
			int result = 0;
			if (!int.TryParse(args, NumberStyles.HexNumber, Thread.CurrentThread.CurrentCulture, out result))
			{
				if (isThrowException)
				{
					throw new CustomException("转化失败!");
				}
				result = 0;
			}
			return result;
		}

		public static int ToNumber32(string args)
		{
			int result = 0;
			if (!int.TryParse(args, NumberStyles.HexNumber, Thread.CurrentThread.CurrentCulture, out result))
			{
				result = 0;
			}
			return result;
		}

		public static long ToNumber64(string args, bool isThrowException)
		{
			long result = 0L;
			if (!long.TryParse(args, NumberStyles.HexNumber, Thread.CurrentThread.CurrentCulture, out result))
			{
				if (isThrowException)
				{
					throw new CustomException("转化失败!");
				}
				result = 0L;
			}
			return result;
		}

		public static long ToNumber64(string args)
		{
			long result = 0L;
			if (!long.TryParse(args, NumberStyles.HexNumber, Thread.CurrentThread.CurrentCulture, out result))
			{
				result = 0L;
			}
			return result;
		}

		public static void DoNoneException(Exception ex)
		{
		}

		public static void ShowMessage(string message)
		{
			ShowMsg(message);
		}

		public static void ShowMessage(Exception ex)
		{
			if (ex == null)
			{
				return;
			}
			if (ex is CustomException)
			{
				CustomException ex2 = ex as CustomException;
				switch (ex2.messageType)
				{
				case MessageType.LOG:
					WriteExceptionMessage(ex2.EX);
					break;
				case MessageType.SHOWANDLOG:
					WriteExceptionMessage(ex2.EX);
					ShowMsg(ex2.CusMessage);
					break;
				case MessageType.SHOW:
					ShowMsg(ex2.CusMessage);
					break;
				default:
					WriteExceptionMessage(ex2);
					break;
				}
			}
			else
			{
				WriteExceptionMessage(ex);
				ShowExceptionMessage(ex);
			}
		}

		protected static void WriteExceptionMessage(Exception ex)
		{
			LogLevel logLevel = ConfigManager.Ins.logLevel;
			string defaultMessage = ConfigManager.Ins.DefaultMessage;
			if (!(ex is SocketException) && !(ex is ThreadAbortException))
			{
				switch (logLevel)
				{
				case LogLevel.ERROR:
					WriteTXTLog(logLevel, ex.Message);
					break;
				case LogLevel.DEBUG:
					WriteTXTLog(logLevel, ex.Message + ex.StackTrace);
					break;
				default:
					WriteTXTLog(logLevel, ex.Message + ex.StackTrace);
					break;
				}
			}
		}

		protected static void ShowExceptionMessage(Exception ex)
		{
			string defaultMessage = ConfigManager.Ins.DefaultMessage;
			if (ex is SocketException || ex is ThreadAbortException)
			{
				defaultMessage = "";
			}
			else
			{
				ShowMsgError(defaultMessage);
			}
		}

		protected static void ShowMsg(string message)
		{
			MessageBox.Show(message, ConfigManager.Ins.SystemTitle);
		}

		protected static void ShowMsgError(string message)
		{
			MessageBox.Show(message, GetLocalizedString("Com.Cn.Tool.ResourceMsg", "ErrorWarning"));
		}

		public static void ShowErrorProviderMessage(Control control, string ErrorMessage)
		{
			errorProvider = ((errorProvider == null) ? new ErrorProvider() : errorProvider);
			errorProvider.SetError(control, ErrorMessage);
		}

		public static void WriteTXTLog(LogLevel loglevel, string msg)
		{
			WriteTXTLog("   [" + loglevel.ToString() + "]   " + msg);
		}

		public static void WriteTXTLog(string msg)
		{
			lock (locObject)
			{
				DateTime now = DateTime.Now;
				try
				{
					using (StreamWriter streamWriter = new StreamWriter(GetLogPath(now), true, Encoding.UTF8))
					{
						streamWriter.WriteLine("[" + now.ToString(FormatString.Ins.FFF) + "] " + msg);
						streamWriter.AutoFlush = true;
					}
				}
				catch (Exception ex)
				{
					DoNoneException(ex);
					ConfigManager.Ins.CreateLogPath();
					using (StreamWriter streamWriter2 = new StreamWriter(GetLogPath(now), true, Encoding.UTF8))
					{
						streamWriter2.WriteLine("[" + now.ToString(FormatString.Ins.FFF) + "] " + msg);
						streamWriter2.AutoFlush = true;
					}
				}
			}
		}

		public static void WriteTXTLog(Exception ex)
		{
			if (ex != null)
			{
				if (ConfigManager.Ins.logLevel == LogLevel.ERROR)
				{
					WriteTXTLog(LogLevel.ERROR, ex.Message);
				}
				else
				{
					WriteTXTLog(LogLevel.ERROR, ex.Message + ex.StackTrace);
				}
			}
		}

		public static string GetLogPath(DateTime dt)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string value = dt.ToString(FormatString.Ins.DataShort) + ".txt";
			stringBuilder.Append(ConfigManager.Ins.LogPath);
			stringBuilder.Append(value);
			return stringBuilder.ToString();
		}

		public static bool ShowMessage(string msg, MessageBoxButtons btn)
		{
			DialogResult dialogResult = MessageBox.Show(msg, ConfigManager.Ins.SystemTitle, btn, MessageBoxIcon.Question);
			if (dialogResult != DialogResult.OK)
			{
				return dialogResult == DialogResult.Yes;
			}
			return true;
		}

		public static DialogResult ShowMessage(string msg, MessageBoxButtons btn, MessageBoxIcon icon)
		{
			return MessageBox.Show(msg, ConfigManager.Ins.SystemTitle, btn, icon);
		}

		public static DialogResult ShowMessage(Form form, string msg, MessageBoxButtons btn, MessageBoxIcon icon)
		{
			return MessageBox.Show(form, msg, ConfigManager.Ins.SystemTitle, btn, icon);
		}

		public static void SetDGVReadOnly(DataGridView dgv, bool flag, params string[] columnNames)
		{
			foreach (DataGridViewColumn column in dgv.Columns)
			{
				if (columnNames != null && columnNames.Contains(column.Name))
				{
					column.ReadOnly = false;
				}
				else
				{
					column.ReadOnly = flag;
				}
			}
		}

		public static void SetDGVNotSortMode(DataGridView dgv)
		{
			foreach (DataGridViewColumn column in dgv.Columns)
			{
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
			}
		}

		public static void SetDGVVisible(DataGridView dgv, bool flag, params string[] columnNames)
		{
			foreach (DataGridViewColumn column in dgv.Columns)
			{
				if (columnNames.Contains(column.Name))
				{
					column.Visible = false;
				}
				else
				{
					column.Visible = flag;
				}
			}
		}

		public static string ShowDGVColumns(DataGridView dgv)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DataGridViewColumn column in dgv.Columns)
			{
				stringBuilder.Append("//" + column.HeaderText + "<br /> row.Cells[" + column.Name + ".Name].Value = CommTool.GetString();<br />");
			}
			return stringBuilder.ToString();
		}

		public static void DGVSelect(DataGridView DGV, DataGridViewColumn CKColumn, bool isSelect)
		{
			if (DGV == null || CKColumn == null)
			{
				return;
			}
			foreach (DataGridViewRow item in (IEnumerable)DGV.Rows)
			{
				if (!item.ReadOnly)
				{
					item.Cells[CKColumn.Name].Value = isSelect;
				}
			}
		}

		public static void DGVSelectRowNotReadOnly(DataGridView DGV, DataGridViewColumn CKColumn, bool isSelect)
		{
			if (DGV == null || CKColumn == null)
			{
				return;
			}
			foreach (DataGridViewRow item in (IEnumerable)DGV.Rows)
			{
				item.Cells[CKColumn.Name].Value = isSelect;
			}
		}

		public static void DoDataError(DataGridView dgv, DataGridViewDataErrorEventArgs e)
		{
			DataGridViewColumn dataGridViewColumn = dgv.Columns[e.ColumnIndex];
			ShowMessage(dataGridViewColumn.Name);
		}

		public static void MouseRightSelectRow(DataGridView dgv, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
			{
				return;
			}
			int rowIndex = dgv.HitTest(e.X, e.Y).RowIndex;
			if (rowIndex < 0)
			{
				return;
			}
			foreach (DataGridViewRow item in (IEnumerable)dgv.Rows)
			{
				if (item.Index == rowIndex)
				{
					item.Selected = true;
				}
				else
				{
					item.Selected = false;
				}
			}
		}

		public static void SetDGVAutoSizeMode(DataGridView dgv, params string[] columnNames)
		{
			foreach (DataGridViewColumn column in dgv.Columns)
			{
				if (columnNames.Contains(column.Name))
				{
					column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				}
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
				dataColumn = new DataColumn(columnName);
				dataTable.Columns.Add(dataColumn);
			}
			return dataTable;
		}

		public static string GetValue(TextBox txt)
		{
			return txt.Text.Trim();
		}

		public static string GetValue(ComboBox cmb)
		{
			string result = GetString(cmb.SelectedValue).Trim();
			BaseKey baseKey = cmb.SelectedItem as BaseKey;
			if (baseKey != null)
			{
				result = baseKey.BKKey;
			}
			return result;
		}

		public static void BindingComboBox(IList<BaseKey> lst, ComboBox cmb)
		{
			BindMember(cmb, lst, "BKValue", "BKKey");
		}

		public static void BindingComboBox(IList<BaseKey> lst, ComboBox cmb, string key, string value)
		{
			lst.Insert(0, new BaseKey(key, value, true));
			BindMember(cmb, lst, "BKValue", "BKKey");
		}

		public static void BindingComboBox(IList<TBaseKey<long, string>> resource, ComboBox cmb)
		{
			if (resource.Count > 0)
			{
				int num = 0;
				Label label = new Label();
				foreach (TBaseKey<long, string> item in resource)
				{
					label.Text = item.TValue;
					num = label.PreferredWidth + 17;
					if (num > cmb.DropDownWidth)
					{
						cmb.DropDownWidth = num;
					}
				}
				label.Dispose();
			}
			cmb.DisplayMember = "TValue";
			cmb.ValueMember = "TKey";
			cmb.DataSource = resource;
		}

		public static void BindingComboBox(IList<TBaseKey<int, string>> resource, ComboBox cmb)
		{
			if (resource.Count > 0)
			{
				int num = 0;
				Label label = new Label();
				foreach (TBaseKey<int, string> item in resource)
				{
					label.Text = item.TValue;
					num = label.PreferredWidth + 17;
					if (num > cmb.DropDownWidth)
					{
						cmb.DropDownWidth = num;
					}
				}
				label.Dispose();
			}
			cmb.DisplayMember = "TValue";
			cmb.ValueMember = "TKey";
			cmb.DataSource = resource;
		}

		public static void BindingComboBox(BindingList<TBaseKey<long, string>> resource, ComboBox cmb)
		{
			if (resource.Count > 0)
			{
				int num = 0;
				Label label = new Label();
				foreach (TBaseKey<long, string> item in resource)
				{
					label.Text = item.TValue;
					num = label.PreferredWidth + 17;
					if (num > cmb.DropDownWidth)
					{
						cmb.DropDownWidth = num;
					}
				}
				label.Dispose();
			}
			cmb.DisplayMember = "TValue";
			cmb.ValueMember = "TKey";
			cmb.DataSource = resource;
		}

		public static void BindingComboBox(BindingList<TBaseKey<string, string>> resource, ComboBox cmb)
		{
			if (resource.Count > 0)
			{
				int num = 0;
				Label label = new Label();
				foreach (TBaseKey<string, string> item in resource)
				{
					label.Text = item.TValue;
					num = label.PreferredWidth + 17;
					if (num > cmb.DropDownWidth)
					{
						cmb.DropDownWidth = num;
					}
				}
				label.Dispose();
			}
			cmb.DisplayMember = "TValue";
			cmb.ValueMember = "TKey";
			cmb.DataSource = resource;
		}

		public static void BindingComboBoxText(IList<BaseKey> resource, ComboBox cmb)
		{
			if (resource.Count > 0)
			{
				int num = 0;
				Label label = new Label();
				BaseKey baseKey = null;
				foreach (object item in cmb.Items)
				{
					if (item is BaseKey)
					{
						baseKey = item as BaseKey;
						label.Text = baseKey.Text;
						num = label.PreferredWidth + 17;
						if (num > cmb.DropDownWidth)
						{
							cmb.DropDownWidth = num;
						}
					}
				}
				label.Dispose();
				GC.Collect();
			}
			cmb.DisplayMember = "Text";
			cmb.ValueMember = "BKKey";
			cmb.DataSource = resource;
		}

		public static void BindMember(ComboBox cmb, IList<BaseKey> resource, string disPlayMember, string valueMember)
		{
			if (resource.Count > 0)
			{
				int num = 0;
				Label label = new Label();
				foreach (BaseKey item in resource)
				{
					label.Text = item.BKValue;
					num = label.PreferredWidth + 17;
					if (num > cmb.DropDownWidth)
					{
						cmb.DropDownWidth = num;
					}
				}
				label.Dispose();
				GC.Collect();
			}
			cmb.DisplayMember = disPlayMember;
			cmb.ValueMember = valueMember;
			cmb.DataSource = resource;
		}

		public static void BindingComboBox(IList<BaseKey> lst, DataGridViewComboBoxColumn cmb)
		{
			BindMember(cmb, "BKValue", "BKKey");
			cmb.DataSource = lst;
		}

		public static void BindingComboBoxText(IList<BaseKey> lst, DataGridViewComboBoxColumn cmb)
		{
			BindMember(cmb, "BKValue", "BKKey");
			cmb.DataSource = lst;
		}

		public static void BindingComboBox(IList<BaseKey> lst, DataGridViewComboBoxColumn cmb, string key, string value)
		{
			BindMember(cmb, "BKValue", "BKKey");
			lst.Insert(0, new BaseKey(key, value, true));
			cmb.DataSource = lst;
		}

		public static void BindMember(DataGridViewComboBoxColumn cmb, string disPlayMember, string valueMember)
		{
			cmb.DisplayMember = disPlayMember;
			cmb.ValueMember = valueMember;
		}

		public static void BindingComboBox(IList<TBaseKey<int, string>> lst, DataGridViewComboBoxColumn cmb)
		{
			TBindMember(cmb);
			cmb.DataSource = lst;
		}

		public static void BindingComboBox(IList<TBaseKey<string, string>> lst, DataGridViewComboBoxColumn cmb)
		{
			TBindMember(cmb);
			cmb.DataSource = lst;
		}

		public static void BindingComboBox(IList<TBaseKey<long, string>> lst, DataGridViewComboBoxColumn cmb)
		{
			TBindMember(cmb);
			cmb.DataSource = lst;
		}

		public static void TBindMember(DataGridViewComboBoxColumn cmb)
		{
			cmb.DisplayMember = "TValue";
			cmb.ValueMember = "TKey";
		}

		public static bool IsValidNumeric(string strIn, int integerLength, int fractionLength)
		{
			if (integerLength < 1 || integerLength > 18)
			{
				integerLength = 1;
			}
			if (fractionLength < 0 || fractionLength > 4)
			{
				fractionLength = 0;
			}
			if (fractionLength == 0)
			{
				return Regex.IsMatch(strIn, "^(([\\-]?[1-9]\\d{1," + (integerLength - 1) + "})|([\\-]?[0-9]{1}))$");
			}
			return Regex.IsMatch(strIn, "^(((([\\-]?[1-9]\\d{1," + (integerLength - 1) + "})|(\\d{1}))$)|((([\\-]?[1-9]\\d{0," + (integerLength - 1) + "})|(0))(\\.\\d{1," + fractionLength + "}){0,1}$)|(\\-0\\.\\d{1," + fractionLength + "}))");
		}

		public static bool IsValidNotNegativeNumeric(string strIn, int integerLength, int fractionLength)
		{
			if (integerLength < 1 || integerLength > 18)
			{
				integerLength = 1;
			}
			if (fractionLength < 0 || fractionLength > 4)
			{
				fractionLength = 0;
			}
			if (fractionLength == 0)
			{
				return Regex.IsMatch(strIn, "^(([1-9]\\d{1," + (integerLength - 1) + "})|([0-9]{1}))$");
			}
			return Regex.IsMatch(strIn, "^(((([1-9]\\d{1," + (integerLength - 1) + "})|(\\d{1}))$)|((([1-9]\\d{0," + (integerLength - 1) + "})|(0))(\\.\\d{1," + fractionLength + "}){0,1}$))");
		}

		public static void SetLocation(Control control, string text)
		{
			int width = control.Width;
			control.Text = text;
			control.Location = new Point(control.Location.X + width - control.Width, control.Location.Y);
		}

		public static string GetIP()
		{
			string result = "";
			IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
			IPAddress[] addressList = hostEntry.AddressList;
			foreach (IPAddress iPAddress in addressList)
			{
				if (!iPAddress.IsIPv6LinkLocal && iPAddress != null)
				{
					result = iPAddress.ToString();
					break;
				}
			}
			return result;
		}

		public static IPAddress GetIPAddress()
		{
			IPAddress result = null;
			IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
			IPAddress[] addressList = hostEntry.AddressList;
			foreach (IPAddress iPAddress in addressList)
			{
				if (!iPAddress.IsIPv6LinkLocal && iPAddress != null)
				{
					result = iPAddress;
					break;
				}
			}
			return result;
		}

		public static byte[] ResolveByte(byte[] source, int start, int length)
		{
			byte[] array = new byte[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = source[i + start];
			}
			return array;
		}

		public static byte[] ResolveByte(byte[] source, int length, ref int start)
		{
			byte[] array = new byte[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = source[start];
				start++;
			}
			return array;
		}

		public static byte ResolveByte(byte[] source)
		{
			byte b = 0;
			byte[] array = new byte[1];
			if (source.Length < array.Length)
			{
				return 0;
			}
			return source[0];
		}

		public static short ResolveInt16(byte[] source, NetWorkEnum flg)
		{
			short num = 0;
			byte[] array = new byte[2];
			if (source.Length < array.Length)
			{
				for (int i = 0; i < source.Length; i++)
				{
					array[i] = source[i];
				}
				num = BitConverter.ToInt16(array, 0);
			}
			else
			{
				num = BitConverter.ToInt16(source, 0);
			}
			switch (flg)
			{
			case NetWorkEnum.HostToNetWork:
				num = IPAddress.HostToNetworkOrder(num);
				break;
			case NetWorkEnum.NetWorkToHost:
				num = IPAddress.NetworkToHostOrder(num);
				break;
			}
			return num;
		}

		public static int ResolveInt32(byte[] source, NetWorkEnum flg)
		{
			int num = 0;
			byte[] array = new byte[4];
			if (source.Length < array.Length)
			{
				for (int i = 0; i < source.Length; i++)
				{
					array[i] = source[i];
				}
				num = BitConverter.ToInt32(array, 0);
			}
			else
			{
				num = BitConverter.ToInt32(source, 0);
			}
			switch (flg)
			{
			case NetWorkEnum.HostToNetWork:
				num = IPAddress.HostToNetworkOrder(num);
				break;
			case NetWorkEnum.NetWorkToHost:
				num = IPAddress.NetworkToHostOrder(num);
				break;
			}
			return num;
		}

		public static long ResolveInt64(byte[] source, NetWorkEnum flg)
		{
			long num = 0L;
			byte[] array = new byte[8];
			if (source.Length < array.Length)
			{
				for (int i = 0; i < source.Length; i++)
				{
					array[i] = source[i];
				}
				num = BitConverter.ToInt64(array, 0);
			}
			else
			{
				num = BitConverter.ToInt64(source, 0);
			}
			switch (flg)
			{
			case NetWorkEnum.HostToNetWork:
				num = IPAddress.HostToNetworkOrder(num);
				break;
			case NetWorkEnum.NetWorkToHost:
				num = IPAddress.NetworkToHostOrder(num);
				break;
			}
			return num;
		}

		public static void ToByteArray(DateTime dt, byte[] result, ref int startIndex)
		{
			ToByteArray(Convert.ToInt16(dt.Year), result, ref startIndex);
			ToByteArray(Convert.ToByte(dt.Month), result, ref startIndex);
			ToByteArray(Convert.ToByte(dt.Day), result, ref startIndex);
			ToByteArray(Convert.ToByte(dt.Hour), result, ref startIndex);
			ToByteArray(Convert.ToByte(dt.Minute), result, ref startIndex);
			ToByteArray(Convert.ToByte(dt.Second), result, ref startIndex);
		}

		public static void ToByteArray(string str, int length, byte[] array, ref int startIndex)
		{
			string s = GetString(str);
			byte[] bytes = Encoding.Unicode.GetBytes(s);
			byte[] array2 = new byte[length];
			int startIndex2 = 0;
			ToByteArray(bytes, array2, ref startIndex2);
			ToByteArray(array2, array, ref startIndex);
		}

		public static void ToByteArray(char[] charString, byte[] array, ref int startIndex)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(charString);
			ToByteArray(bytes, array, ref startIndex);
		}

		public static void ToByteArray(byte number, byte[] array, ref int startIndex)
		{
			array[startIndex] = number;
			startIndex++;
		}

		public static void ToByteArray(short number, byte[] array, ref int startIndex)
		{
			byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(number));
			ToByteArray(bytes, array, ref startIndex);
		}

		public static void ToByteArray(int number, byte[] array, ref int startIndex)
		{
			byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(number));
			ToByteArray(bytes, array, ref startIndex);
		}

		public static void ToByteArray(long number, byte[] array, ref int startIndex)
		{
			byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(number));
			ToByteArray(bytes, array, ref startIndex);
		}

		public static void ToByteArray(byte[] resource, byte[] array, ref int startIndex)
		{
			for (int i = 0; i < resource.Length; i++)
			{
				ToByteArray(resource[i], array, ref startIndex);
			}
		}

		public static void RefreshReportData(ReportViewer reportViewer1, string reportDataSourceName, BindingSource bindingsource)
		{
			if (bindingsource == null)
			{
				throw new CustomException("bindingsource is  null!");
			}
			if (bindingsource.DataSource == null)
			{
				throw new CustomException("Report DataSource is  null!");
			}
			LocalReport localReport = new LocalReport();
			localReport = reportViewer1.LocalReport;
			ReportDataSource reportDataSource = new ReportDataSource();
			reportDataSource.Name = reportDataSourceName;
			reportDataSource.Value = bindingsource;
			localReport.DataSources.Clear();
			localReport.DataSources.Add(reportDataSource);
			reportViewer1.RefreshReport();
		}

		public static void RefreshReportData(ReportViewer reportViewer1, params ReportDataSource[] reportDataSource)
		{
			LocalReport localReport = new LocalReport();
			localReport = reportViewer1.LocalReport;
			localReport.DataSources.Clear();
			foreach (ReportDataSource reportDataSource2 in reportDataSource)
			{
				if (string.IsNullOrEmpty(reportDataSource2.Name))
				{
					throw new NoNullAllowedException("reportDataSource name is not null");
				}
				if (reportDataSource2.Value == null)
				{
					throw new NoNullAllowedException("reportDataSource value is not null");
				}
				localReport.DataSources.Add(reportDataSource2);
			}
			reportViewer1.RefreshReport();
		}

		public static void PrintLocalReport(string reportPath, DeviceInfo deviceInfo, params ReportDataSource[] reportDataSource)
		{
			if (string.IsNullOrEmpty(reportPath))
			{
				throw new NoNullAllowedException("reportPath is null");
			}
			LocalReport localReport = new LocalReport();
			localReport.ReportPath = reportPath;
			foreach (ReportDataSource reportDataSource2 in reportDataSource)
			{
				if (string.IsNullOrEmpty(reportDataSource2.Name))
				{
					throw new NoNullAllowedException("reportDataSource name is not null");
				}
				if (reportDataSource2.Value == null)
				{
					throw new NoNullAllowedException("reportDataSource value is not null");
				}
				localReport.DataSources.Add(reportDataSource2);
			}
			PrintLocalReport printLocalReport = new PrintLocalReport();
			printLocalReport.PrintReport(localReport, deviceInfo);
			printLocalReport.Dispose();
		}

		public static void PrintLocalReport(string printerName, string reportPath, DeviceInfo deviceInfo, params ReportDataSource[] reportDataSource)
		{
			if (string.IsNullOrEmpty(printerName))
			{
				throw new NoNullAllowedException("printer Name is not null");
			}
			if (string.IsNullOrEmpty(reportPath))
			{
				throw new NoNullAllowedException("reportPath is null");
			}
			LocalReport localReport = new LocalReport();
			localReport.ReportPath = reportPath;
			foreach (ReportDataSource reportDataSource2 in reportDataSource)
			{
				if (string.IsNullOrEmpty(reportDataSource2.Name))
				{
					throw new NoNullAllowedException("reportDataSource name is not null");
				}
				if (reportDataSource2.Value == null)
				{
					throw new NoNullAllowedException("reportDataSource value is not null");
				}
				localReport.DataSources.Add(reportDataSource2);
			}
			PrintLocalReport printLocalReport = new PrintLocalReport();
			printLocalReport.PrintReport(localReport, printerName, deviceInfo);
			printLocalReport.Dispose();
		}

		public static void PrintPreviewLocalReport(string reportPath, DeviceInfo deviceInfo, params ReportDataSource[] reportDataSource)
		{
			if (string.IsNullOrEmpty(reportPath))
			{
				throw new NoNullAllowedException("reportPath is null");
			}
			LocalReport localReport = new LocalReport();
			localReport.ReportPath = reportPath;
			foreach (ReportDataSource reportDataSource2 in reportDataSource)
			{
				if (string.IsNullOrEmpty(reportDataSource2.Name))
				{
					throw new NoNullAllowedException("reportDataSource name is not null");
				}
				if (reportDataSource2.Value == null)
				{
					throw new NoNullAllowedException("reportDataSource value is not null");
				}
				localReport.DataSources.Add(reportDataSource2);
			}
			PrintLocalReport printLocalReport = new PrintLocalReport();
			printLocalReport.PrintPreviewReport(localReport, deviceInfo);
			printLocalReport.Dispose();
		}

		public static void PrintPreviewLocalReport(string printerName, string reportPath, DeviceInfo deviceInfo, params ReportDataSource[] reportDataSource)
		{
			if (string.IsNullOrEmpty(printerName))
			{
				throw new NoNullAllowedException("printer Name is not null");
			}
			if (string.IsNullOrEmpty(reportPath))
			{
				throw new NoNullAllowedException("reportPath is null");
			}
			LocalReport localReport = new LocalReport();
			localReport.ReportPath = reportPath;
			foreach (ReportDataSource reportDataSource2 in reportDataSource)
			{
				if (string.IsNullOrEmpty(reportDataSource2.Name))
				{
					throw new NoNullAllowedException("reportDataSource name is not null");
				}
				if (reportDataSource2.Value == null)
				{
					throw new NoNullAllowedException("reportDataSource value is not null");
				}
				localReport.DataSources.Add(reportDataSource2);
			}
			PrintLocalReport printLocalReport = new PrintLocalReport();
			printLocalReport.PrintPreviewReport(localReport, printerName, deviceInfo);
			printLocalReport.Dispose();
		}

		public static DeviceInfo GetRDLCDeviceInfo(string rdlcPath)
		{
			DeviceInfo deviceInfo = new DeviceInfo();
			XDocument xDocument = XDocument.Load(rdlcPath);
			XNamespace xNamespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition";
			IEnumerable<XElement> enumerable = from el in xDocument.Elements(xNamespace + "Report").Elements(xNamespace + "Page").Elements()
				select (el);
			foreach (XElement item in enumerable)
			{
				switch (item.Name.LocalName)
				{
				case "PageHeight":
					deviceInfo.PageHeight = decimal.Parse(item.Value.Replace("cm", ""));
					break;
				case "PageWidth":
					deviceInfo.PageWidth = decimal.Parse(item.Value.Replace("cm", ""));
					break;
				case "LeftMargin":
					deviceInfo.MarginLeft = decimal.Parse(item.Value.Replace("cm", ""));
					break;
				case "RightMargin":
					deviceInfo.MarginRight = decimal.Parse(item.Value.Replace("cm", ""));
					break;
				case "TopMargin":
					deviceInfo.MarginTop = decimal.Parse(item.Value.Replace("cm", ""));
					break;
				case "BottomMargin":
					deviceInfo.MarginBottom = decimal.Parse(item.Value.Replace("cm", ""));
					break;
				}
			}
			return deviceInfo;
		}

		public static void SetTextBoxInputNumber(TextBox txt)
		{
			txt.KeyPress += txt_KeyPress;
		}

		private static void txt_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '\b':
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				e.Handled = false;
				break;
			default:
				e.Handled = true;
				break;
			}
		}

		public static string BytesToString(byte[] buffer)
		{
			if (buffer == null)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in buffer)
			{
				stringBuilder.Append(b.ToString(FormatString.Ins.X2));
			}
			return stringBuilder.ToString();
		}

		public static byte[] StringToBytes(string args)
		{
			string text = GetString(args);
			byte[] array = new byte[text.Length / 2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ToNumberByte(text[2 * i].ToString() + text[2 * i + 1]);
			}
			return array;
		}

		public static void SetNodeChecked(TreeNode Node, bool setAllChildren)
		{
			if (Node.Nodes.Count == 0 || !setAllChildren)
			{
				return;
			}
			foreach (TreeNode node in Node.Nodes)
			{
				node.Checked = Node.Checked;
				SetNodeChecked(node, setAllChildren);
			}
		}

		public static IList<TreeNode> GetCheckedNodes(TreeNodeCollection Nodes, bool getAllChildren)
		{
			List<TreeNode> list = new List<TreeNode>();
			foreach (TreeNode Node in Nodes)
			{
				if (Node.Checked)
				{
					list.Add(Node);
				}
				if (getAllChildren)
				{
					list.AddRange(GetCheckedNodes(Node.Nodes, getAllChildren));
				}
			}
			return list;
		}

		public static void SetNodeParentChecked(TreeNode tn)
		{
			if (tn.Parent != null)
			{
				if (GetCheckedNodes(tn.Parent.Nodes, true).Count <= 0)
				{
					tn.Parent.Checked = false;
				}
				else
				{
					tn.Parent.Checked = true;
				}
			}
		}

		public static void ExpandParentNodes(TreeNode expandNode, bool expandAllNode)
		{
			if (expandNode.Parent != null && !expandNode.Parent.IsExpanded)
			{
				expandNode.Parent.Expand();
				if (expandAllNode)
				{
					ExpandParentNodes(expandNode.Parent, expandAllNode);
				}
			}
		}

		public static IList<TreeNode> GetTreeNodes(TreeView tv)
		{
			IList<TreeNode> list = new List<TreeNode>();
			foreach (TreeNode node in tv.Nodes)
			{
				Recursive(list, node);
			}
			return list;
		}

		private static void Recursive(IList<TreeNode> tns, TreeNode tn)
		{
			foreach (TreeNode node in tn.Nodes)
			{
				tns.Add(node);
				Recursive(tns, node);
			}
		}

		public static void SetCheckedListBoxAllChecked(CheckedListBox chk, bool chked)
		{
			for (int i = 0; i < chk.Items.Count; i++)
			{
				chk.SetItemChecked(i, chked);
			}
		}

		public static void SetCheckedListBoxAntiChecked(CheckedListBox chk)
		{
			for (int i = 0; i < chk.Items.Count; i++)
			{
				chk.SetItemChecked(i, !chk.GetItemChecked(i));
			}
		}

		public static int FindMinimumNotUsed(List<int> lst)
		{
			int num = 1;
			IEnumerable<int> source = FindMinimumListNotUsed(lst);
			return source.Min();
		}

		public static IEnumerable<int> FindMinimumListNotUsed(List<int> lst)
		{
			IEnumerable<int> enumerable = null;
			if (lst.Count == 0 || !lst.Contains(1))
			{
				List<int> list = new List<int>();
				list.Add(1);
				return list;
			}
			List<int> list2 = new List<int>();
			foreach (int item in lst)
			{
				list2.Add(item + 1);
			}
			return list2.Except(lst);
		}

		public static IList<string> FindAllPrinters()
		{
			IList<string> list = new List<string>();
			try
			{
				for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
				{
					list.Add(PrinterSettings.InstalledPrinters[i]);
				}
			}
			catch
			{
			}
			return list;
		}

		public static bool FindPriter(string PrinterName)
		{
			bool result = false;
			for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
			{
				if (PrinterName == PrinterSettings.InstalledPrinters[i])
				{
					return true;
				}
			}
			return result;
		}

		public static string GetLocalizedString(string resxFileName, string strKey)
		{
			string text = "";
			try
			{
				ResourceManager resourceManager = new ResourceManager(resxFileName, Assembly.GetExecutingAssembly());
				CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
				text = resourceManager.GetString(strKey, currentUICulture);
				text = ((text == null) ? "" : text.Replace("\\n", "\n"));
			}
			catch
			{
				return "";
			}
			if (string.IsNullOrEmpty(text))
			{
				text = "no localized";
			}
			return text;
		}

		public static decimal MillimetersToInch(decimal cm)
		{
			return cm / 25.4m;
		}

		public static decimal InchToMillimeters(decimal inch)
		{
			return inch * 25.4m;
		}
	}
	public class Tool<T>
	{
		public static T GetEnum(string args)
		{
			return (T)Enum.Parse(typeof(T), args, true);
		}

		public static T GetEnum(int args)
		{
			return (T)Enum.ToObject(typeof(T), args);
		}

		public static T ConvertInstance(object args)
		{
			if (args is T)
			{
				return (T)args;
			}
			return default(T);
		}

		public static T LoadInstance()
		{
			Assembly assembly = Assembly.GetAssembly(typeof(T));
			return ConvertInstance(assembly.CreateInstance(typeof(T).ToString(), true));
		}

		public static Assembly LoadInstance(string path)
		{
			return Assembly.LoadFile(path);
		}

		public static T GetValue(TextBox txt)
		{
			T val = default(T);
			string text = txt.Text.Trim();
			switch (typeof(T).ToString())
			{
			case "System.Int16":
				return ConvertInstance((text.Length != 0) ? Convert.ToInt16(text) : 0);
			case "System.Int32":
				return ConvertInstance((text.Length != 0) ? Convert.ToInt32(text) : 0);
			case "System.Int64":
				return ConvertInstance((text.Length == 0) ? 0 : Convert.ToInt64(text));
			case "System.DateTime":
				return ConvertInstance(Convert.ToDateTime(text));
			default:
				return ConvertInstance(text);
			}
		}
	}
	public class Tool<TKey, TValue>
	{
		public static TKey GetValue(ComboBox cmb)
		{
			TKey result = default(TKey);
			TBaseKey<TKey, TValue> tBaseKey = cmb.SelectedItem as TBaseKey<TKey, TValue>;
			if (tBaseKey != null)
			{
				return tBaseKey.TKey;
			}
			return result;
		}

		public static void BindingComboBox(IList<TBaseKey<TKey, TValue>> resource, ComboBox cmb)
		{
			BindingComboBox(resource, cmb, TBasekeyPrototype.TValue, TBasekeyPrototype.TKey);
		}

		public static void BindingComboBox(IList<TBaseKey<TKey, TValue>> resource, ComboBox cmb, TKey insertFistKey, TValue insertFirstValue)
		{
			resource.Insert(0, new TBaseKey<TKey, TValue>(insertFistKey, insertFirstValue));
			BindingComboBox(resource, cmb, TBasekeyPrototype.TValue, TBasekeyPrototype.TKey);
		}

		public static void BindingComboBox(IList<TBaseKey<TKey, TValue>> resource, ComboBox cmb, TBaseKey<TKey, TValue> firstItem)
		{
			resource.Insert(0, firstItem);
			BindingComboBox(resource, cmb, TBasekeyPrototype.TValue, TBasekeyPrototype.TKey);
		}

		private static void BindingComboBox(IList<TBaseKey<TKey, TValue>> resource, ComboBox cmb, TBasekeyPrototype disPlayMember, TBasekeyPrototype valueMember)
		{
			BindingComboBox(resource, cmb, disPlayMember.ToString(), valueMember.ToString());
		}

		public static void BindingComboBox(IList<TBaseKey<TKey, TValue>> resource, ComboBox cmb, string disPlayMember, string valueMember)
		{
			if (resource.Count > 0)
			{
				int num = 0;
				Label label = new Label();
				foreach (TBaseKey<TKey, TValue> item in resource)
				{
					label.Text = ((item.TValue == null) ? "" : item.TValue.ToString());
					num = label.PreferredWidth + 17;
					if (num > cmb.DropDownWidth)
					{
						cmb.DropDownWidth = num;
					}
				}
				label.Dispose();
				GC.Collect();
			}
			BindingComboBoxMember(cmb, disPlayMember, valueMember);
			cmb.DataSource = resource;
		}

		public static void BindingComboBoxMember(ComboBox cmb, string disPlayMember, string valueMember)
		{
			cmb.DisplayMember = disPlayMember;
			cmb.ValueMember = valueMember;
		}
	}
}
