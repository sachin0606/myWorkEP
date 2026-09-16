using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CryptManage
{
	public class CryptManageBase
	{
		public delegate void PubKeyEventHandler(object OldValue, object NewValue);

		private readonly string DEFAULT_PUBKEY = "feisn";

		private byte[] _securityKey;

		private string _pubKey;

		protected byte[] SecurityKey
		{
			get
			{
				if (_securityKey == null || _securityKey.Length < 0)
				{
					_securityKey = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(PubKey));
				}
				return _securityKey;
			}
		}

		public string PubKey
		{
			get
			{
				return _pubKey;
			}
			set
			{
				string pubKey = PubKey;
				_pubKey = (string.IsNullOrEmpty(value) ? DEFAULT_PUBKEY : value);
				_securityKey = null;
				if (this.OnPubKeyChanged != null)
				{
					this.OnPubKeyChanged(pubKey, PubKey);
				}
			}
		}

		public event PubKeyEventHandler OnPubKeyChanged;

		public CryptManageBase()
		{
			PubKey = DEFAULT_PUBKEY;
		}

		public CryptManageBase(string PubKey)
		{
			if (string.IsNullOrEmpty(PubKey))
			{
				this.PubKey = DEFAULT_PUBKEY;
			}
			else
			{
				this.PubKey = PubKey;
			}
		}

		public string EnCryptMD5(string StringValue)
		{
			byte[] buffer = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(StringValue));
			return GetXString(buffer);
		}

		protected string GetXString(byte[] buffer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in buffer)
			{
				stringBuilder.Append(b.ToString("X2"));
			}
			return stringBuilder.ToString();
		}

		protected List<byte> GetBytesFromXString(string XString)
		{
			List<byte> list = new List<byte>(XString.Length / 2);
			for (int i = 0; i < XString.Length; i += 2)
			{
				string value = XString.Substring(i, 2);
				list.Add(Convert.ToByte(value, 16));
			}
			return list;
		}
	}
}
