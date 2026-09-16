using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptManage
{
	public class CryptionManage : CryptManageBase, ICrypt
	{
		private static CryptionManage _instance;

		public static CryptionManage Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new CryptionManage();
				}
				return _instance;
			}
		}

		public CryptionManage this[string PubKey]
		{
			get
			{
				Instance.PubKey = PubKey;
				return Instance;
			}
		}

		public CryptionManage()
		{
		}

		public CryptionManage(string PubKey)
			: base(PubKey)
		{
		}

		public string EnCrpty(string UnCryptedString)
		{
			string text = "ERROR";
			List<int> list = new List<int>();
			if (UnCryptedString.Length >= base.SecurityKey.Length)
			{
				for (int i = 0; i < base.SecurityKey.Length; i++)
				{
					list.Add(i);
				}
			}
			else
			{
				for (int i = 0; i < UnCryptedString.Length; i++)
				{
					int item = new Random().Next(0, base.SecurityKey.Length);
					if (list.Contains(item))
					{
						i--;
					}
					else
					{
						list.Add(item);
					}
				}
			}
			StringBuilder stringBuilder = new StringBuilder(8);
			int num = ((UnCryptedString.Length > base.SecurityKey.Length) ? ((int)Math.Ceiling((double)UnCryptedString.Length / 8.0)) : ((int)Math.Ceiling((double)base.SecurityKey.Length / 8.0)));
			StringBuilder stringBuilder2 = new StringBuilder(num * 2 + 2);
			int num2 = ((UnCryptedString.Length > base.SecurityKey.Length) ? UnCryptedString.Length : base.SecurityKey.Length);
			for (int j = 0; j <= num2; j++)
			{
				if (j > 0 && j % 8 == 0)
				{
					string value = stringBuilder.ToString();
					stringBuilder2.Append(Convert.ToByte(value, 2).ToString("X2"));
					stringBuilder.Clear();
				}
				if (j == num2)
				{
					if (j % 8 != 0)
					{
						string value = stringBuilder.ToString();
						stringBuilder2.Append(Convert.ToByte(value, 2).ToString("X2"));
						stringBuilder.Clear();
					}
					break;
				}
				if (num2 > base.SecurityKey.Length)
				{
					stringBuilder.Append("1");
				}
				else if (list.Contains(j))
				{
					stringBuilder.Append("1");
				}
				else
				{
					stringBuilder.Append("0");
				}
			}
			byte[] bytes = Encoding.UTF8.GetBytes(UnCryptedString);
			List<int> list2 = list.ToList();
			for (int i = 0; i < bytes.Length; i++)
			{
				if (list2.Count == 0)
				{
					list2 = list.ToList();
				}
				int num3 = list2.Min();
				bytes[i] = (byte)((base.SecurityKey[num3] + bytes[i]) % 255);
				list2.Remove(num3);
			}
			stringBuilder2.Insert(0, ((num + Convert.ToByte(stringBuilder2.ToString(0, 2), 16)) % 255).ToString("X2"));
			text = stringBuilder2.ToString() + GetXString(bytes);
			if (text.Length < 32)
			{
				text += EnCryptMD5(Guid.NewGuid().ToString()).Substring(0, 32 - text.Length);
			}
			return text;
		}

		public string DeCrypt(string CryptedString)
		{
			string text = "ERROR";
			int num = Convert.ToInt16(CryptedString.Substring(0, 2), 16) - Convert.ToInt16(CryptedString.Substring(2, 2), 16);
			int num2 = ((num > 0) ? num : (num + 255));
			string text2 = CryptedString.Substring(2, num2 * 2);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < num2 * 2; i += 2)
			{
				string value = Convert.ToString(Convert.ToByte(CryptedString.Substring(i + 2, 2), 16), 2).PadLeft(8, '0');
				stringBuilder.Append(value);
			}
			string text3 = stringBuilder.ToString();
			int num3 = 0;
			List<int> list = new List<int>();
			for (int i = 0; i < text3.Length; i++)
			{
				if (text3[i].Equals('1'))
				{
					num3++;
					if (i < base.SecurityKey.Length)
					{
						list.Add(i);
					}
				}
			}
			string xString = CryptedString.Substring(num2 * 2 + 2, num3 * 2);
			List<byte> bytesFromXString = GetBytesFromXString(xString);
			List<int> list2 = list.ToList();
			int num4 = 0;
			for (int i = 0; i < bytesFromXString.Count; i++)
			{
				if (list2.Count == 0)
				{
					list2 = list.ToList();
				}
				int num5 = list2.Min();
				int num6 = bytesFromXString[i] - base.SecurityKey[num5];
				if (num6 < 0)
				{
					num6 += 255;
				}
				bytesFromXString[i] = (byte)num6;
				try
				{
					if (num4 == 3)
					{
						num4 = 0;
					}
					if (num6 > 127 && num4 == 0)
					{
						List<byte> bytesFromXString2 = GetBytesFromXString(CryptedString.Substring((num2 + bytesFromXString.Count + 1) * 2, 4));
						bytesFromXString.AddRange(bytesFromXString2);
					}
					if (num6 > 127)
					{
						num4++;
					}
				}
				catch (Exception ex)
				{
					throw new Exception("分析中文信息出错:" + ex.Message);
				}
				list2.Remove(num5);
			}
			return Encoding.UTF8.GetString(bytesFromXString.ToArray());
		}
	}
}
