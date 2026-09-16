using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Util.Common.ExtensionHelper;

namespace Util.Common
{
	public static class CryptoHelper
	{
		public static byte[] DefaultBIV
		{
			get
			{
				return new byte[1] { 1 };
			}
		}

		public static byte[] DefaultKey
		{
			get
			{
				return new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
			}
		}

		[Obsolete]
		public static byte[] Md5Encrypt(byte[] buff)
		{
			return MD5(buff);
		}

		public static byte[] Md5Encrypt(string text, Encoding encoding)
		{
			byte[] bytes = encoding.GetBytes(text);
			return MD5(bytes);
		}

		public static byte[] MD5(byte[] data)
		{
			byte[] array = new byte[0];
			using (MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider())
			{
				array = mD5CryptoServiceProvider.ComputeHash(data, 0, data.Length);
				mD5CryptoServiceProvider.Clear();
				return array;
			}
		}

		public static void BuilderKey(out string PublicKey, out string PrivateKey)
		{
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				PublicKey = rSACryptoServiceProvider.ToXmlString(false);
				PrivateKey = rSACryptoServiceProvider.ToXmlString(true);
			}
		}

		public static byte[] RSAEncryptDC(string keyContainerName, byte[] buff, bool fOAEP = false)
		{
			CspParameters cspParameters = new CspParameters();
			cspParameters.KeyContainerName = keyContainerName;
			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspParameters))
			{
				return Encrypt(rsa, buff, fOAEP);
			}
		}

		public static byte[] RSAEncryptDC(string keyContainerName, string text, Encoding encoding, bool fOAEP = false)
		{
			CspParameters cspParameters = new CspParameters();
			cspParameters.KeyContainerName = keyContainerName;
			byte[] bytes = encoding.GetBytes(text);
			return RSAEncryptDC(keyContainerName, bytes, fOAEP);
		}

		public static byte[] RSADecryptDC(string keyContainerName, byte[] buff, bool fOAEP = false)
		{
			CspParameters cspParameters = new CspParameters();
			cspParameters.KeyContainerName = keyContainerName;
			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspParameters))
			{
				return Decrypt(rsa, buff, fOAEP);
			}
		}

		public static string RSADecryptDC(string keyContainerName, byte[] buff, Encoding encoding, bool fOAEP = false)
		{
			byte[] bytes = RSADecryptDC(keyContainerName, buff, fOAEP);
			return encoding.GetString(bytes);
		}

		public static byte[] RSAEncrypt(string Key, byte[] buff, bool fOAEP = false)
		{
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				rSACryptoServiceProvider.FromXmlString(Key);
				return Encrypt(rSACryptoServiceProvider, buff, fOAEP);
			}
		}

		public static byte[] RSAEncrypt(string Key, string text, Encoding encoding, bool fOAEP = false)
		{
			byte[] bytes = encoding.GetBytes(text);
			return RSAEncrypt(Key, bytes, fOAEP);
		}

		public static byte[] RSADecrypt(string PrivateKey, byte[] buff, bool fOAEP = false)
		{
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				rSACryptoServiceProvider.FromXmlString(PrivateKey);
				return Decrypt(rSACryptoServiceProvider, buff, fOAEP);
			}
		}

		public static string RSADecrypt(string PrivateKey, byte[] buff, Encoding encoding, bool fOAEP = false)
		{
			byte[] bytes = RSADecrypt(PrivateKey, buff, fOAEP);
			return encoding.GetString(bytes);
		}

		private static byte[] Encrypt(RSACryptoServiceProvider rsa, byte[] buff, bool fOAEP)
		{
			MemoryStream memoryStream = new MemoryStream();
			int index = 0;
			int length = rsa.KeySize / 8 - 11;
			while (index < buff.Length)
			{
				byte[] array = rsa.Encrypt(SplitBuff(buff, length, ref index), fOAEP);
				memoryStream.Write(array, 0, array.Length);
			}
			return memoryStream.ToArray();
		}

		private static byte[] Decrypt(RSACryptoServiceProvider rsa, byte[] buff, bool fOAEP)
		{
			MemoryStream memoryStream = new MemoryStream();
			int index = 0;
			int length = rsa.KeySize / 8;
			while (index < buff.Length)
			{
				byte[] array = rsa.Decrypt(SplitBuff(buff, length, ref index), fOAEP);
				memoryStream.Write(array, 0, array.Length);
			}
			return memoryStream.ToArray();
		}

		private static byte[] SplitBuff(byte[] buff, int length, ref int index)
		{
			if (index + length > buff.Length)
			{
				length = buff.Length - index;
			}
			return buff.GetBytes(length, ref index);
		}

		public static byte[] AESEncrypt(byte[] data, byte[] key, int size = 16, PaddingMode paddingmode = PaddingMode.PKCS7)
		{
			byte[] array = new byte[size];
			int num = key.Length;
			if (num > array.Length)
			{
				num = array.Length;
			}
			Array.Copy(key, array, num);
			using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
			{
				rijndaelManaged.Mode = CipherMode.ECB;
				rijndaelManaged.Padding = paddingmode;
				rijndaelManaged.KeySize = 128;
				rijndaelManaged.Key = array;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(data, 0, data.Length);
						cryptoStream.FlushFinalBlock();
						return memoryStream.ToArray();
					}
				}
			}
		}

		public static byte[] AESDecrypt(byte[] data, byte[] key, int size = 16, PaddingMode paddingmode = PaddingMode.PKCS7)
		{
			byte[] array = new byte[size];
			int num = key.Length;
			if (num > array.Length)
			{
				num = array.Length;
			}
			Array.Copy(key, array, num);
			using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
			{
				rijndaelManaged.Mode = CipherMode.ECB;
				rijndaelManaged.Padding = paddingmode;
				rijndaelManaged.KeySize = 128;
				rijndaelManaged.Key = array;
				using (MemoryStream stream = new MemoryStream(data))
				{
					using (CryptoStream cryptoStream = new CryptoStream(stream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Read))
					{
						byte[] array2 = new byte[data.Length + 32];
						int num2 = cryptoStream.Read(array2, 0, data.Length + 32);
						byte[] array3 = new byte[num2];
						Array.Copy(array2, 0, array3, 0, num2);
						return array3;
					}
				}
			}
		}

		public static byte[] RijndaelEncrypt(byte[] buff, byte[] key)
		{
			byte[] result = new byte[0];
			try
			{
				using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
				{
					rijndaelManaged.Mode = CipherMode.ECB;
					rijndaelManaged.Padding = PaddingMode.PKCS7;
					rijndaelManaged.Key = key;
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
						{
							cryptoStream.Write(buff, 0, buff.Length);
							cryptoStream.FlushFinalBlock();
							result = memoryStream.ToArray();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return result;
		}

		public static byte[] RijndaelDecrypt(byte[] buff, byte[] key)
		{
			byte[] result = new byte[0];
			try
			{
				using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
				{
					rijndaelManaged.Mode = CipherMode.ECB;
					rijndaelManaged.Padding = PaddingMode.PKCS7;
					rijndaelManaged.Key = key;
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write))
						{
							cryptoStream.Write(buff, 0, buff.Length);
							cryptoStream.FlushFinalBlock();
							result = memoryStream.ToArray();
						}
					}
				}
			}
			catch
			{
			}
			return result;
		}

		public static byte[] DESEncrypt(byte[] buff, byte[] key, byte[] bIV)
		{
			if (key == null || key.Length != 8)
			{
				key = DefaultKey;
			}
			if (bIV == null)
			{
				bIV = DefaultBIV;
			}
			byte[] result = new byte[0];
			using (DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider())
			{
				try
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(key, bIV), CryptoStreamMode.Write))
						{
							cryptoStream.Write(buff, 0, buff.Length);
							cryptoStream.FlushFinalBlock();
							result = memoryStream.ToArray();
						}
					}
				}
				catch
				{
				}
			}
			return result;
		}

		public static byte[] DESDecrypt(this byte[] buff, byte[] key, byte[] bIV)
		{
			if (key == null || key.Length != 8)
			{
				key = DefaultKey;
			}
			if (bIV == null)
			{
				bIV = DefaultBIV;
			}
			byte[] result = new byte[0];
			using (DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider())
			{
				try
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(key, bIV), CryptoStreamMode.Write))
						{
							cryptoStream.Write(buff, 0, buff.Length);
							cryptoStream.FlushFinalBlock();
							result = memoryStream.ToArray();
						}
					}
				}
				catch
				{
				}
			}
			return result;
		}

		public static byte[] DESEncrypt(byte[] buff, byte[] key)
		{
			byte[] result = new byte[0];
			using (DES dES = DES.Create())
			{
				try
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						dES.Key = key;
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dES.CreateEncryptor(), CryptoStreamMode.Write))
						{
							cryptoStream.Write(buff, 0, buff.Length);
							cryptoStream.FlushFinalBlock();
							result = memoryStream.ToArray();
						}
					}
				}
				catch
				{
				}
			}
			return result;
		}

		public static byte[] DESDecrypt(this byte[] buff, byte[] key)
		{
			byte[] result = new byte[0];
			using (DES dES = DES.Create())
			{
				try
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						dES.Key = key;
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dES.CreateDecryptor(), CryptoStreamMode.Write))
						{
							cryptoStream.Write(buff, 0, buff.Length);
							cryptoStream.FlushFinalBlock();
							result = memoryStream.ToArray();
						}
					}
				}
				catch
				{
				}
			}
			return result;
		}

		public static string EncryptToSHA1(string str)
		{
			using (SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider())
			{
				byte[] bytes = Encoding.ASCII.GetBytes(str);
				byte[] buff = sHA1CryptoServiceProvider.ComputeHash(bytes);
				sHA1CryptoServiceProvider.Clear();
				return buff.BytesToString("", "X2");
			}
		}
	}
}
