using System.Diagnostics;
using System.Text;

namespace Dexin.Util.Common.FFmepgHelper
{
	public class UDPBroadcast : MediaConverter
	{
		protected static string SendPara(int loops, string inputFile, int audioSamplingRate, int audioBitRate, string audioFormat, ushort nitID, ushort tsID, ushort serviceID, ushort pmtStartPID, ushort mpegtsStartPID, string serviceProvider, string serviceName, int sendRate, string udpIPAddress, int udpPort, int packetSize, string localIPAddress, int localPort)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("-re -stream_loop {0} ", loops));
			stringBuilder.Append(MediaConverter.ConvertPara(inputFile, audioSamplingRate, audioBitRate, audioFormat, nitID, tsID, serviceID, pmtStartPID, mpegtsStartPID, serviceProvider, serviceName, null));
			stringBuilder.Append(" -f mpegts");
			stringBuilder.Append(string.Format(" -pkt_size {0}", packetSize));
			if (sendRate > 0)
			{
				stringBuilder.Append(string.Format(" -muxrate {0}", sendRate * 1000));
			}
			stringBuilder.Append(string.Format(" udp://{0}:{1}", udpIPAddress, udpPort));
			if (!string.IsNullOrEmpty(localIPAddress))
			{
				stringBuilder.Append(string.Format("?localaddr={0}", localIPAddress));
				if (localPort > 0)
				{
					stringBuilder.Append(string.Format("&localport={0}", localPort));
				}
			}
			return stringBuilder.ToString();
		}

		public static Process BeginSend(int loops, string inputFile, ushort nitID, ushort tsID, ushort serviceID, ushort pmtStartPID, ushort mpegtsStartPID, string serviceProvider, string serviceName, string udpAddress, int udpPort, int packetSize, int sendRate = 0, int audioSamplingRate = 0, int audioBitRate = 0, string audioFormat = null, string localIP = null, int localPort = 0)
		{
			string parameters = SendPara(loops, inputFile, audioSamplingRate, audioBitRate, audioFormat, nitID, tsID, serviceID, pmtStartPID, mpegtsStartPID, serviceProvider, serviceName, sendRate, udpAddress, udpPort, packetSize, localIP, localPort);
			return MediaBase.BeginRunProcess(parameters);
		}
	}
}
