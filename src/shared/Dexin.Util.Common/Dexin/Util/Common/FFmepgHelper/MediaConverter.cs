using System.Text;

namespace Dexin.Util.Common.FFmepgHelper
{
	public class MediaConverter : MediaBase
	{
		protected static string ConvertPara(string inputFile, int audioSamplingRate, int audioBitRate, string audioFormat, ushort nitID, ushort tsID, ushort serviceID, ushort pmtStartPID, ushort mpegtsStartPID, string serviceProvider, string serviceName, string outputFile)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("-i \"{0}\"", inputFile));
			if (audioSamplingRate > 0)
			{
				stringBuilder.Append(string.Format(" -ar {0}", audioSamplingRate));
			}
			if (audioBitRate > 0)
			{
				stringBuilder.Append(string.Format(" -ab {0}", audioBitRate * 1000));
			}
			if (!string.IsNullOrEmpty(audioFormat))
			{
				stringBuilder.Append(string.Format(" -acodec {0}", audioFormat));
			}
			if (audioSamplingRate <= 0 && audioBitRate <= 0 && string.IsNullOrEmpty(audioFormat))
			{
				stringBuilder.Append(" -c copy");
			}
			if (nitID > 0)
			{
				stringBuilder.Append(string.Format(" -mpegts_original_network_id {0}", nitID));
			}
			if (tsID > 0)
			{
				stringBuilder.Append(string.Format(" -mpegts_transport_stream_id {0}", tsID));
			}
			if (serviceID > 0)
			{
				stringBuilder.Append(string.Format(" -mpegts_service_id {0}", serviceID));
			}
			if (pmtStartPID > 0)
			{
				stringBuilder.Append(string.Format(" -mpegts_pmt_start_pid {0}", pmtStartPID));
			}
			if (mpegtsStartPID > 0)
			{
				stringBuilder.Append(string.Format(" -mpegts_start_pid {0}", mpegtsStartPID));
			}
			if (!string.IsNullOrEmpty(serviceProvider))
			{
				stringBuilder.Append(string.Format(" -metadata service_provider=\"{0}\"", serviceProvider));
			}
			if (!string.IsNullOrEmpty(serviceName))
			{
				stringBuilder.Append(string.Format(" -metadata service_name=\"{0}\"", serviceName));
			}
			if (!string.IsNullOrEmpty(outputFile))
			{
				stringBuilder.Append(string.Format(" -y \"{0}\"", outputFile));
			}
			return stringBuilder.ToString();
		}

		public static string Convert(string inputFile, int audioSamplingRate, int audioBitRate, string audioFormat, ushort nitID, ushort tsID, ushort serviceID, ushort pmtStartPID, ushort mpegtsStartPID, string serviceProvider, string serviceName, string outputFile)
		{
			string parameters = ConvertPara(inputFile, audioSamplingRate, audioBitRate, audioFormat, nitID, tsID, serviceID, pmtStartPID, mpegtsStartPID, serviceProvider, serviceName, outputFile);
			return MediaBase.RunProcess(parameters);
		}

		public static void BeginConvert(string inputFile, int audioSamplingRate, int audioBitRate, string audioFormat, ushort nitID, ushort tsID, ushort serviceID, ushort pmtStartPID, ushort mpegtsStartPID, string serviceProvider, string serviceName, string outputFile)
		{
			string parameters = ConvertPara(inputFile, audioSamplingRate, audioBitRate, audioFormat, nitID, tsID, serviceID, pmtStartPID, mpegtsStartPID, serviceProvider, serviceName, outputFile);
			MediaBase.BeginRunProcess(parameters);
		}
	}
}
