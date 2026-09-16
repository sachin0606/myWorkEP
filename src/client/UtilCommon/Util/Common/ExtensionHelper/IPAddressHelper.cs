using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Util.Common.ExtensionHelper
{
	public static class IPAddressHelper
	{
		private const string min = "224.0.0.255";

		private const string max = "239.255.255.255";

		public static long MIN_MULTICASE_ADDRESS
		{
			get
			{
				return "224.0.0.255".GetAddress();
			}
		}

		public static long MAX_MULTICASE_ADDRESS
		{
			get
			{
				return "239.255.255.255".GetAddress();
			}
		}

		public static List<IPAddress> Localips(IPTypeEnum ipType = IPTypeEnum.IPV4)
		{
			List<IPAddress> list = new List<IPAddress>();
			IPAddress[] hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
			foreach (IPAddress iPAddress in hostAddresses)
			{
				if (ipType == IPTypeEnum.IPV4 && iPAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					list.Add(iPAddress);
				}
				else if (ipType == IPTypeEnum.IPV6 && iPAddress.AddressFamily == AddressFamily.InterNetworkV6)
				{
					list.Add(iPAddress);
				}
				else if (ipType == IPTypeEnum.ALL && (iPAddress.AddressFamily == AddressFamily.InterNetwork || iPAddress.AddressFamily == AddressFamily.InterNetworkV6))
				{
					list.Add(iPAddress);
				}
			}
			return list;
		}

		public static List<KeyValuePair<string, string>> LocalKeyValuePairIps(bool isaddlocalip = false, IPTypeEnum ipType = IPTypeEnum.IPV4)
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			if (isaddlocalip)
			{
				list.Add(new KeyValuePair<string, string>("127.0.0.1", "127.0.0.1"));
			}
			foreach (IPAddress item in Localips(ipType))
			{
				list.Add(new KeyValuePair<string, string>(item.ToString(), item.ToString()));
			}
			return list;
		}

		public static bool IsIPAddress(this string IP)
		{
			if (string.IsNullOrWhiteSpace(IP))
			{
				return false;
			}
			string[] array = IP.Split('.');
			if (array.Length != 4)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				int result;
				if (!int.TryParse(array[i], out result))
				{
					return false;
				}
				if (result < 0 || result > 255)
				{
					return false;
				}
				if (result == 0 && i == 0)
				{
					return false;
				}
			}
			return true;
		}

		public static long GetAddress(this string IP)
		{
			if (!IP.IsIPAddress())
			{
				return 0L;
			}
			return IP.ToIPAdress().GetAddress();
		}

		public static long GetAddress(this IPAddress IP)
		{
			int host = BitConverter.ToInt32(IP.GetAddressBytes(), 0);
			uint num = (uint)IPAddress.HostToNetworkOrder(host);
			return num;
		}

		public static bool IsMulticaseAddress(this string IP)
		{
			if (!IP.IsIPAddress())
			{
				return false;
			}
			long address = IP.GetAddress();
			if (address > MIN_MULTICASE_ADDRESS)
			{
				return address <= MAX_MULTICASE_ADDRESS;
			}
			return false;
		}

		public static bool IsMask(this string msk)
		{
			if (string.IsNullOrWhiteSpace(msk))
			{
				return false;
			}
			string[] array = msk.Split('.');
			if (array.Length != 4)
			{
				return false;
			}
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				int result;
				if (!int.TryParse(array[i], out result))
				{
					return false;
				}
				if (result < 0 || result > 255)
				{
					return false;
				}
				if (flag)
				{
					if (result != 0)
					{
						return false;
					}
					continue;
				}
				for (int num = 7; num >= 0; num--)
				{
					if (((result >> num) & 1) == 0)
					{
						flag = true;
					}
					else if (flag)
					{
						return false;
					}
				}
			}
			return true;
		}

		public static bool Compare(string ip0, string ip1)
		{
			if (!ip0.IsIPAddress())
			{
				return false;
			}
			if (!ip1.IsIPAddress())
			{
				return false;
			}
			return IPAddress.Parse(ip0).Equals(IPAddress.Parse(ip1));
		}

		public static bool Compare(this IPAddress ip0, IPAddress ip1)
		{
			return ip0.Equals(ip1);
		}

		public static bool Compare(this IPAddress ip0, string ip1)
		{
			if (!ip1.IsIPAddress())
			{
				return false;
			}
			return ip0.Equals(IPAddress.Parse(ip1));
		}

		public static byte[] IPAddressTo4(string IP)
		{
			byte[] array = new byte[4];
			string[] array2 = IP.Split('.');
			for (int i = 0; i < 4; i++)
			{
				array[i] = array2[i].ToInt8(0);
			}
			return array;
		}
	}
}
