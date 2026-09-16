using System.Net;

namespace Dexin.Util.Common.ExtensionHelper
{
	public static class NumberHelpe
	{
		public static short ToNetWorkNumber(this short obj)
		{
			return IPAddress.HostToNetworkOrder(obj);
		}

		public static int ToNetWorkNumber(this int obj)
		{
			return IPAddress.HostToNetworkOrder(obj);
		}

		public static long ToNetWorkNumber(this long obj)
		{
			return IPAddress.HostToNetworkOrder(obj);
		}

		public static ushort ToNetWorkNumber(this ushort obj)
		{
			return (ushort)IPAddress.HostToNetworkOrder((short)obj);
		}

		public static uint ToNetWorkNumber(this uint obj)
		{
			return (uint)IPAddress.HostToNetworkOrder((int)obj);
		}

		public static ulong ToNetWorkNumber(this ulong obj)
		{
			return (ulong)IPAddress.HostToNetworkOrder((long)obj);
		}

		public static short ToHostNumber(this short obj)
		{
			return IPAddress.NetworkToHostOrder(obj);
		}

		public static int ToHostNumber(this int obj)
		{
			return IPAddress.NetworkToHostOrder(obj);
		}

		public static long ToHostNumber(this long obj)
		{
			return IPAddress.NetworkToHostOrder(obj);
		}

		public static ushort ToHostNumber(this ushort obj)
		{
			return (ushort)IPAddress.NetworkToHostOrder((short)obj);
		}

		public static uint ToHostNumber(this uint obj)
		{
			return (uint)IPAddress.NetworkToHostOrder((int)obj);
		}

		public static ulong ToHostNumber(this ulong obj)
		{
			return (ulong)IPAddress.NetworkToHostOrder((long)obj);
		}
	}
}
