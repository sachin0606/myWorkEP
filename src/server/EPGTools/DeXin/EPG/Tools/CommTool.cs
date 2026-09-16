using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Resource;
using Dexin.Util.Common.Text;

namespace DeXin.EPG.Tools
{
	public class CommTool : Tool
	{
		public static readonly string DBVersion = "2.3";

		private static Color _CellBackColor = Color.Empty;

		public static string SinkPath = Tool.AppPath + "\\Skin\\DiamondBlue.ssk";

		public static int InnerICNumberLength = 8;

		public static int OuterICNumberLength = 16;

		public static int ContextLength = 511;

		public static int FiftenLength = 15;

		private static string encoder { get; set; }

		public static Color CellBackColor
		{
			get
			{
				if (_CellBackColor.IsEmpty)
				{
					_CellBackColor = Color.Gray;
				}
				return _CellBackColor;
			}
		}

		public static void SetEncoder(string _encoder)
		{
			encoder = _encoder;
		}

		public static string GetEncoder()
		{
			return encoder;
		}

		public static Encoding ConvertToEncoder(string encoder)
		{
			if (string.IsNullOrWhiteSpace(encoder))
			{
				return Encoding.Default;
			}
			Encoding encoding = null;
			switch (encoder)
			{
			case "UTF-8":
				encoding = Encoding.UTF8;
				break;
			case "GB2312":
				encoding = Encoding.GetEncoding("GB2312");
				break;
			case "UNICODE":
				encoding = Encoding.Unicode;
				break;
			case "ASCII":
				encoding = Encoding.ASCII;
				break;
			default:
				encoding = Encoding.Default;
				break;
			}
			return encoding;
		}

		public static bool CkeckEmpty(TextBox control, string msg)
		{
			string value = Tool.GetValue(control);
			if (value.Length == 0)
			{
				control.Text = "";
				control.Focus();
				Tool.ShowMessage(msg + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull]);
				return false;
			}
			return true;
		}

		public static bool CkeckEmpty(TextBox control, string msg, MessageBoxButtons btn)
		{
			string value = Tool.GetValue(control);
			if (value.Length == 0 && !Tool.ShowMessage(msg + ResourceManagement.GetLocationString("SureIsNull"), btn))
			{
				control.Text = "";
				control.Focus();
				return false;
			}
			return true;
		}

		public static short CheckTxtBoxInt16(TextBox txt, string lblHeader)
		{
			string text = "";
			short num = 0;
			string text2 = txt.Text.Trim();
			if (text2.Length == 0)
			{
				text = lblHeader + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
				throw new CustomException(text, MessageType.SHOW);
			}
			try
			{
				return Convert.ToInt16(text2);
			}
			catch (Exception ex)
			{
				text = ex.Message;
				text = lblHeader + LocationControl.Ins.LocationItems[LacationEnum.InputFormatError];
				throw new CustomException(text, MessageType.SHOW);
			}
		}

		public static int CheckTxtBoxInt32(TextBox txt, string lblHeader)
		{
			string text = "";
			int num = 0;
			string text2 = txt.Text.Trim();
			if (text2.Length == 0)
			{
				text = lblHeader + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
				throw new CustomException(text, MessageType.SHOW);
			}
			try
			{
				return Convert.ToInt32(text2);
			}
			catch (Exception ex)
			{
				text = ex.Message;
				text = lblHeader + LocationControl.Ins.LocationItems[LacationEnum.InputFormatError];
				throw new CustomException(text, MessageType.SHOW);
			}
		}

		public static long CheckTxtBoxInt64(TextBox txt, string lblHeader)
		{
			string text = "";
			long num = 0L;
			string text2 = txt.Text.Trim();
			if (text2.Length == 0)
			{
				text = lblHeader + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
				throw new CustomException(text, MessageType.SHOW);
			}
			try
			{
				return Convert.ToInt64(text2);
			}
			catch (Exception ex)
			{
				text = ex.Message;
				text = lblHeader + LocationControl.Ins.LocationItems[LacationEnum.InputFormatError];
				throw new CustomException(text, MessageType.SHOW);
			}
		}

		public static bool CheckNumber(TextBox control, out string msg)
		{
			msg = "";
			string value = Tool.GetValue(control);
			try
			{
				Convert.ToDecimal(value);
			}
			catch
			{
				control.Text = "";
				control.Focus();
				msg = LocationControl.Ins.LocationItems[LacationEnum.InputFormatError];
				return false;
			}
			return true;
		}

		public static bool CheckNumber(TextBox control, string msg, MessageBoxButtons btn)
		{
			if (CkeckEmpty(control, msg, MessageBoxButtons.OKCancel) && Tool.GetValue(control).Length == 0)
			{
				return true;
			}
			string value = Tool.GetValue(control);
			try
			{
				Convert.ToDecimal(value);
			}
			catch
			{
				control.Text = "";
				control.Focus();
				Tool.ShowMessage(msg + LocationControl.Ins.LocationItems[LacationEnum.InputFormatError]);
				return false;
			}
			return true;
		}

		public static bool IsValidNotNegativeIntByTxtLbl(TextBox txt, Label lbl, out string msg)
		{
			int result = 0;
			if (!int.TryParse(txt.Text.Trim(), out result))
			{
				msg = ShowInputFormatError(lbl);
				return false;
			}
			if (result < 0)
			{
				msg = ShowInputFormatError(lbl);
				return false;
			}
			if (txt.Text.Trim().Contains('-'))
			{
				msg = ShowInputFormatError(lbl);
				return false;
			}
			msg = "";
			return true;
		}

		public static bool IsValidNotNegativeInt(string str)
		{
			int result = 0;
			if (!int.TryParse(str, out result))
			{
				return false;
			}
			if (result < 0)
			{
				return false;
			}
			return true;
		}

		public static string ShowInputFormatError(Label lbl)
		{
			return lbl.Text + LocationControl.Ins.LocationItems[LacationEnum.InputFormatError];
		}

		public static string ShowIsNotNull(Label lbl)
		{
			return lbl.Text + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
		}

		public static byte[] GetAddress(string address)
		{
			byte[] array = new byte[7];
			if (address != null)
			{
				string[] array2 = address.Split(',');
				byte b = (byte)Tool.GetInt16(array2[0]);
				short number = (short)((array2.Length > 1) ? Tool.GetInt16(array2[1]) : 0);
				short number2 = (short)((array2.Length > 2) ? Tool.GetInt16(array2[2]) : 0);
				short number3 = (short)((array2.Length > 3) ? Tool.GetInt16(array2[3]) : 0);
				array[0] = b;
				ToByteArray(number, 1, ref array);
				ToByteArray(number2, 3, ref array);
				ToByteArray(number3, 5, ref array);
			}
			return array;
		}

		public static string ShowAddress(string addressID)
		{
			string result = "";
			try
			{
				if (addressID.Length > 0)
				{
					result = LeftClearAddress(addressID);
				}
			}
			catch
			{
				result = "";
			}
			return result;
		}

		private static string LeftClearAddress(string addressID)
		{
			string result = addressID;
			try
			{
				if (addressID != null && addressID.Length - 1 > 0 && addressID.LastIndexOf(',') == addressID.Length - 1)
				{
					result = LeftClearAddress(addressID.Substring(0, addressID.Length - 1));
				}
			}
			catch
			{
				result = "";
			}
			return result;
		}

		public static string GetAddress1(string addressID)
		{
			string text = "";
			string[] array = addressID.Split(',');
			for (int num = array.Length - 1; num >= 0; num--)
			{
				string text2 = array[num];
				if (text2.Length > 0 && num >= 0)
				{
					for (int i = 0; i <= num; i++)
					{
						text = ((i >= num) ? (text + array[i]) : (text + array[i] + ","));
					}
					break;
				}
			}
			return text;
		}

		public static string AddressTo16(string num)
		{
			string text = "";
			try
			{
				string[] array = num.Split(',');
				int num2 = ((array.Length > 5) ? 5 : num.Length);
				for (int i = 1; i < array.Length; i++)
				{
					text = ((i != 1) ? (text + Tool.GetInt16(array[i]).ToString(FormatString.Ins.X4)) : (text + "0x" + Tool.GetByte(array[i]).ToString(FormatString.Ins.X2)));
				}
				if (text.Length < 16)
				{
					text = text.PadRight(16, '0');
				}
			}
			catch (Exception ex)
			{
				CustomException ex2 = new CustomException(ex.Message, MessageType.NONE);
				Tool.ShowMessage(ex2);
				text = num;
			}
			return text;
		}

		public static void ToByteArray(DateTime dt, int startIndex, ref byte[] result)
		{
			ToByteArray(Convert.ToInt16(dt.Year), startIndex, ref result);
			startIndex += 2;
			result[startIndex] = Convert.ToByte(dt.Month);
			startIndex++;
			result[startIndex] = Convert.ToByte(dt.Day);
			startIndex++;
			result[startIndex] = Convert.ToByte(dt.Hour);
			startIndex++;
			result[startIndex] = Convert.ToByte(dt.Minute);
			startIndex++;
			result[startIndex] = Convert.ToByte(dt.Second);
		}

		public static void ToByteArray(string str, int startIndex, ref byte[] array)
		{
			string s = Tool.GetString(str);
			byte[] bytes = Encoding.Unicode.GetBytes(s);
			for (int i = 0; i < bytes.Length; i++)
			{
				array[i + startIndex] = bytes[i];
			}
		}

		public static void ToByteArray(string str, int size, int startIndex, ref byte[] array)
		{
			string s = Tool.GetString(str);
			byte[] bytes = Encoding.Unicode.GetBytes(s);
			byte[] array2 = new byte[size];
			int num = ((bytes.Length > size) ? size : bytes.Length);
			for (int i = 0; i < num; i++)
			{
				array2[i] = bytes[i];
			}
			for (int i = 0; i < array2.Length; i++)
			{
				array[i + startIndex] = array2[i];
			}
		}

		public static void ToByteArray(char[] charString, int startIndex, ref byte[] array)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(charString);
			for (int i = 0; i < bytes.Length; i++)
			{
				array[i + startIndex] = bytes[i];
			}
		}

		public static void ToByteArray(byte number, int startIndex, ref byte[] array)
		{
			array[startIndex] = number;
		}

		public static void ToByteArray(short number, int startIndex, ref byte[] array)
		{
			byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(number));
			for (int i = 0; i < bytes.Length; i++)
			{
				array[i + startIndex] = bytes[i];
			}
		}

		public static void ToByteArray(int number, int startIndex, ref byte[] array)
		{
			byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(number));
			for (int i = 0; i < bytes.Length; i++)
			{
				array[i + startIndex] = bytes[i];
			}
		}

		public static void ToByteArray(long number, int startIndex, ref byte[] array)
		{
			byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(number));
			for (int i = 0; i < bytes.Length; i++)
			{
				array[i + startIndex] = bytes[i];
			}
		}

		public static void ToByteArray(byte[] resource, int startIndex, ref byte[] array)
		{
			for (int i = 0; i < resource.Length; i++)
			{
				array[i + startIndex] = resource[i];
			}
		}

		public static string GetHeadNumber(string head, string number)
		{
			string result = "0";
			head = "";
			if (number.Length != 0 && !number.Equals("0"))
			{
				result = head + number.PadLeft(16, '0');
			}
			return result;
		}

		public static string ConvertToInternalNumber(string strExternalNumber)
		{
			if (strExternalNumber.Length != 16)
			{
				throw new CustomException(ResourceManagement.GetLocationString("LengthICInternalNumberMustbe"));
			}
			foreach (char c in strExternalNumber)
			{
				if (c < '0' || c > '9')
				{
					throw new CustomException(ResourceManagement.GetLocationString("ICExternalNumbeMustBeDigital"));
				}
			}
			return strExternalNumber.Substring(7, 8);
		}

		public static int ConvertToCardHead(string ICNumber)
		{
			if (ICNumber.Length != 16)
			{
				throw new CustomException(ResourceManagement.GetLocationString("LengthICInternalNumberMustbe"));
			}
			foreach (char c in ICNumber)
			{
				if (c < '0' || c > '9')
				{
					throw new CustomException(ResourceManagement.GetLocationString("ICExternalNumbeMustBeDigital"));
				}
			}
			return Tool.GetInt32(ICNumber.Substring(1, 4));
		}

		public static string GetHelpPath()
		{
			string text = Tool.AppPath + "EPGHelp\\EPG User Manual Chinese.pdf";
			string text2 = Thread.CurrentThread.CurrentUICulture.ToString();
			if (text2.Equals("zh-CN"))
			{
				return Tool.AppPath + "EPGHelp\\EPG User Manual Chinese.pdf";
			}
			return Tool.AppPath + "EPGHelp\\EPG User Manual English.pdf";
		}

		public static EnumPossibleLoaction GetLocatingEnum(object obj)
		{
			EnumPossibleLoaction enumPossibleLoaction = EnumPossibleLoaction.NIT;
			try
			{
				enumPossibleLoaction = (EnumPossibleLoaction)(object)Tool.GetEnum(typeof(EnumPossibleLoaction), obj.ToString());
			}
			catch (Exception ex)
			{
				enumPossibleLoaction = EnumPossibleLoaction.NIT;
				Tool.DoNoneException(ex);
			}
			return enumPossibleLoaction;
		}

		public static string GetTextBoxString(int leftLength, int rightLength, TextBox txt)
		{
			string text = "";
			string value = Tool.GetValue(txt);
			string text2 = "";
			string text3 = "";
			if (value.IndexOf('.') < 0)
			{
				text2 = value.PadLeft(leftLength, '0');
				text = text2.Substring(text2.Length - leftLength, leftLength);
				return text.PadRight(leftLength + rightLength, '0');
			}
			text2 = value.Substring(0, value.IndexOf('.'));
			text2 = text2.PadLeft(leftLength, '0');
			text2 = text2.Substring(text2.Length - leftLength, leftLength);
			text3 = value.Substring(value.IndexOf('.') + 1);
			text3 = text3.PadRight(rightLength, '0');
			text3 = text3.Substring(0, rightLength);
			return text2 + text3;
		}

		public static byte NumberToByte(int a, int b)
		{
			byte b2 = 0;
			b2 = (byte)((a & 0xF) << 4);
			return (byte)(b2 | (byte)(b & 0xF));
		}

		public static byte NumberToByte(string args1, string args2)
		{
			byte b = 0;
			args1 = ((Tool.GetLength(args1) % 2 == 0) ? args1 : ("0" + args1));
			args2 = ((Tool.GetLength(args2) % 2 == 0) ? args2 : ("0" + args2));
			byte b2 = Tool.StringToBytes(args1)[0];
			byte b3 = Tool.StringToBytes(args2)[0];
			b = (byte)((b2 & 0xF) << 4);
			return (byte)(b | (byte)(b3 & 0xF));
		}

		public static byte[] NumberToByteString(params Int_String[] items)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Int_String int_String in items)
			{
				if (int_String.Value.Length > int_String.Lengrh)
				{
					int_String.Value = int_String.Value.Substring(int_String.Value.Length - int_String.Lengrh, int_String.Lengrh);
				}
				else
				{
					int_String.Value = int_String.Value.PadLeft(int_String.Lengrh, '0');
				}
				stringBuilder.Append(int_String.Value);
			}
			return Tool.StringToBytes(stringBuilder.ToString());
		}

		public static void SetTextBoxInputNumber16(TextBox txt)
		{
			txt.KeyPress += delegate(object sender_keyPress, KeyPressEventArgs e_keyPress)
			{
				char keyChar = e_keyPress.KeyChar;
				switch (keyChar)
				{
				default:
					if ((keyChar < 'A' || keyChar > 'F') && (keyChar < 'a' || keyChar > 'f'))
					{
						break;
					}
					goto case '\b';
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
					e_keyPress.Handled = false;
					return;
				}
				e_keyPress.Handled = true;
			};
			txt.TextChanged += delegate
			{
				char[] array = txt.Text.ToCharArray();
				char[] array2 = array;
				foreach (char c in array2)
				{
					if ((c < '0' || c > '9') && (c < 'A' || c > 'F') && (c < 'a' || c > 'f'))
					{
						txt.Text = "";
						break;
					}
				}
			};
		}

		public static void SetTextBoxInputNumberPoint(TextBox txt)
		{
			txt.KeyPress += delegate(object sender_keyPress, KeyPressEventArgs e_keyPress)
			{
				char keyChar = e_keyPress.KeyChar;
				if (keyChar == '.' || keyChar == '\b' || (keyChar >= '0' && keyChar <= '9'))
				{
					e_keyPress.Handled = false;
				}
				else
				{
					e_keyPress.Handled = true;
				}
			};
			txt.TextChanged += delegate
			{
				char[] array = txt.Text.ToArray();
				char[] array2 = array;
				foreach (char c in array2)
				{
					if (c != '.' && c != '\b' && (c < '.' || c > '9'))
					{
						txt.Text = "";
					}
				}
			};
		}

		public static void BindComboBoxUsingTBaseKey(IList<TBaseKey<int, string>> tbks, ComboBox cmb)
		{
			cmb.DataSource = tbks.ToList();
			cmb.ValueMember = "TKey";
			cmb.DisplayMember = "TValue";
		}

		public static void BindComboBoxUsingTBaseKey(IList<TBaseKey<long, string>> tbks, ComboBox cmb)
		{
			cmb.DataSource = tbks.ToList();
			cmb.ValueMember = "TKey";
			cmb.DisplayMember = "TValue";
		}

		public static void BindComboBoxUsingTBaseKey(IList<TBaseKey<string, string>> tbks, ComboBox cmb)
		{
			cmb.DataSource = tbks.ToList();
			cmb.ValueMember = "TKey";
			cmb.DisplayMember = "TValue";
		}

		public static void BindComboBoxUsingTBaseKey(IList<TBaseKey<int, string>> tbks, DataGridViewComboBoxColumn dgvcmb)
		{
			dgvcmb.DataSource = tbks.ToList();
			dgvcmb.ValueMember = "TKey";
			dgvcmb.DisplayMember = "TValue";
		}

		public static void BindComboBoxUsingTBaseKey(IList<TBaseKey<long, string>> tbks, DataGridViewComboBoxColumn dgvcmb)
		{
			dgvcmb.DataSource = tbks.ToList();
			dgvcmb.ValueMember = "TKey";
			dgvcmb.DisplayMember = "TValue";
		}

		public static void BindComboBoxUsingTBaseKey(IList<TBaseKey<string, string>> tbks, DataGridViewComboBoxColumn dgvcmb)
		{
			dgvcmb.DataSource = tbks.ToList();
			dgvcmb.ValueMember = "TKey";
			dgvcmb.DisplayMember = "TValue";
		}

		public static byte[] TimeToMJDBuf(DateTime tm)
		{
			byte[] array = new byte[5];
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			num2 = tm.Year - 1900;
			num3 = tm.Month;
			num4 = tm.Day;
			num5 = tm.Hour;
			num6 = tm.Minute;
			num7 = tm.Second;
			num = ((num3 == 1 || num3 == 2) ? 1 : 0);
			num8 = 14956 + num4 + (int)((double)(num2 - num) * 365.25) + (int)((double)(num3 + 1 + num * 12) * 30.6001);
			array[0] = (byte)((num8 >> 8) & 0xFF);
			array[1] = (byte)(num8 & 0xFF);
			array[2] = (byte)(num5 / 10 * 16 + num5 % 10);
			array[3] = (byte)(num6 / 10 * 16 + num6 % 10);
			array[4] = (byte)(num7 / 10 * 16 + num7 % 10);
			return array;
		}

		public static DateTime MJDBuffToTime(byte[] mjdBuff)
		{
			DateTime dateTime = default(DateTime);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			byte[] array = new byte[2]
			{
				mjdBuff[0],
				mjdBuff[1]
			};
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(array);
			}
			ushort num8 = BitConverter.ToUInt16(array, 0);
			num2 = (ushort)(((double)(int)num8 - 15078.2) / 365.25);
			num3 = (int)(((double)(int)num8 - 14956.1 - (double)(int)((double)num2 * 365.25)) / 30.6001);
			num4 = num8 - 14956 - (int)((double)num2 * 365.25) - (int)((double)num3 * 30.6001);
			byte[] array2 = new byte[3]
			{
				mjdBuff[2],
				mjdBuff[3],
				mjdBuff[4]
			};
			string text = BitConverter.ToString(array2);
			string[] array3 = text.Split('-');
			int num9 = 0;
			string[] array4 = array3;
			foreach (string s in array4)
			{
				int num10 = int.Parse(s);
				array2[num9] = (byte)num10;
				num9++;
			}
			num5 = array2[0];
			num6 = array2[1];
			num7 = array2[2];
			num = ((num3 == 14 || num3 == 15) ? 1 : 0);
			num2 += 1900 + num;
			num3 = num3 - 1 - num * 12;
			return new DateTime(num2, num3, num4, num5, num6, num7);
		}

		private byte[] convertToHex(byte[] b)
		{
			byte[] array = new byte[3];
			string text = BitConverter.ToString(b);
			string[] array2 = text.Split('-');
			int num = 0;
			string[] array3 = array2;
			foreach (string value in array3)
			{
				int num2 = Convert.ToInt32(value, 16);
				array[num] = (byte)num2;
				num++;
			}
			return array;
		}

		public static byte[] DescriptorStringToBytes(string args)
		{
			if (string.IsNullOrWhiteSpace(args))
			{
				return new byte[0];
			}
			Encoding encoding = ConvertToEncoder(encoder);
			List<byte> list = new List<byte>();
			if (encoder == "UTF-8")
			{
				list.Add(21);
			}
			else if (encoder == "GB2312")
			{
				list.Add(19);
			}
			else if (encoder == "UNICODE")
			{
				list.Add(17);
			}
			else if (!(encoder == "ASCII"))
			{
			}
			list.AddRange(encoding.GetBytes(args));
			return list.ToArray();
		}

		public static string DescriptorBytesToString(byte[] args)
		{
			if (args.Length == 0)
			{
				return "";
			}
			Encoding encoding = ConvertToEncoder(encoder);
			List<byte> list = new List<byte>();
			int num = 0;
			if (encoder == "UTF-8")
			{
				num++;
			}
			else if (encoder == "GB2312")
			{
				num++;
			}
			else if (encoder == "UNICODE")
			{
				num++;
			}
			else if (!(encoder == "ASCII"))
			{
			}
			int num2 = args.Length - 1;
			byte[] array = new byte[num2];
			Array.Copy(args, num, array, 0, array.Length);
			return encoding.GetString(array);
		}

		public static string GetFaildedReportFolder()
		{
			DateTime now = DateTime.Now;
			string text = TextHelper.GetLogFolder() + "\\" + now.ToString("yyyyMM") + "\\" + now.ToString("yyyy-MM-dd") + "ImportingEventReport\\";
			return text + "FaildedReport\\";
		}

		public static string GetFolder(string xmlFile, bool isSucessFolder)
		{
			string text = "";
			FileInfo fileInfo = new FileInfo(xmlFile);
			return string.Format(arg2: (!fileInfo.Exists) ? "FileNotFound" : fileInfo.Name, format: "{0}\\{0}_{1}\\{2}", arg0: DateTime.Now.ToString("yyyy-MM-dd"), arg1: isSucessFolder ? "Succeeded" : "Failded");
		}

		public static string GetFileName(string xmlFile, bool isSucess)
		{
			string text = "";
			FileInfo fileInfo = new FileInfo(xmlFile);
			if (fileInfo.Exists)
			{
				return fileInfo.Name;
			}
			return "FileNotFound";
		}

		public static string GetLogFileName(string xmlFile, bool isSucess)
		{
			string text = "";
			FileInfo fileInfo = new FileInfo(xmlFile);
			text = ((!fileInfo.Exists) ? "FileNotFound" : fileInfo.Name);
			return text + "_log";
		}
	}
}
