using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Dexin.Util.Common.FFmepgHelper
{
	public class MediaBase
	{
		private static string _FFmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe");

		protected static string FFmpegPath
		{
			get
			{
				return _FFmpegPath;
			}
			set
			{
				_FFmpegPath = value;
			}
		}

		private static void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Data))
			{
				Console.WriteLine(e.Data);
			}
		}

		private static void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Data))
			{
				Console.WriteLine(e.Data);
			}
		}

		public static MediaInfo GetVideoInfo(string mediaFile)
		{
			string parameters = string.Format("-i \"{0}\"", mediaFile);
			string rawInfo = RunProcess(parameters);
			MediaInfo mediaInfo = new MediaInfo(mediaFile);
			mediaInfo.RawInfo = rawInfo;
			mediaInfo.Duration = ExtractDuration(mediaInfo.RawInfo);
			mediaInfo.BitRate = ExtractBitrate(mediaInfo.RawInfo);
			mediaInfo.RawAudioFormat = ExtractRawAudioFormat(mediaInfo.RawInfo);
			mediaInfo.AudioFormat = ExtractAudioFormat(mediaInfo.RawAudioFormat);
			mediaInfo.AudioSamplingRate = ExtractAudioSamplingRate(mediaInfo.RawAudioFormat);
			mediaInfo.RawVideoFormat = ExtractRawVideoFormat(mediaInfo.RawInfo);
			mediaInfo.VideoFormat = ExtractVideoFormat(mediaInfo.RawVideoFormat);
			mediaInfo.Width = ExtractVideoWidth(mediaInfo.RawInfo);
			mediaInfo.Height = ExtractVideoHeight(mediaInfo.RawInfo);
			mediaInfo.infoGathered = true;
			return mediaInfo;
		}

		private static TimeSpan ExtractDuration(string rawInfo)
		{
			TimeSpan result = new TimeSpan(0L);
			Regex regex = new Regex("[D|d]uration:.((\\d|:|\\.)*)", RegexOptions.Compiled);
			Match match = regex.Match(rawInfo);
			if (match.Success)
			{
				string value = match.Groups[1].Value;
				string[] array = value.Split(':', '.');
				if (array.Length == 4)
				{
					result = new TimeSpan(0, Convert.ToInt16(array[0]), Convert.ToInt16(array[1]), Convert.ToInt16(array[2]), Convert.ToInt16(array[3]));
				}
			}
			return result;
		}

		private static double ExtractBitrate(string rawInfo)
		{
			Regex regex = new Regex("[B|b]itrate:.((\\d|:)*)", RegexOptions.Compiled);
			Match match = regex.Match(rawInfo);
			double result = 0.0;
			if (match.Success)
			{
				double.TryParse(match.Groups[1].Value, out result);
			}
			return result;
		}

		private static string ExtractRawAudioFormat(string rawInfo)
		{
			string text = string.Empty;
			Regex regex = new Regex("[A|a]udio:.*", RegexOptions.Compiled);
			Match match = regex.Match(rawInfo);
			if (match.Success)
			{
				text = match.Value;
			}
			return text.Replace("Audio: ", "");
		}

		private static string ExtractAudioFormat(string rawAudioFormat)
		{
			string[] array = rawAudioFormat.Split(new string[1] { ", " }, StringSplitOptions.None);
			return array[0].Replace("Audio: ", "");
		}

		private static int ExtractAudioSamplingRate(string rawAudioFormat)
		{
			string[] array = rawAudioFormat.Split(new string[1] { ", " }, StringSplitOptions.None);
			if (array.Length > 1)
			{
				return int.Parse(array[1].Replace(" Hz", ""));
			}
			return 0;
		}

		private static string ExtractRawVideoFormat(string rawInfo)
		{
			string text = string.Empty;
			Regex regex = new Regex("[V|v]ideo:.*", RegexOptions.Compiled);
			Match match = regex.Match(rawInfo);
			if (match.Success)
			{
				text = match.Value;
			}
			return text.Replace("Video: ", "");
		}

		private static string ExtractVideoFormat(string rawVideoFormat)
		{
			string[] array = rawVideoFormat.Split(new string[1] { ", " }, StringSplitOptions.None);
			return array[0].Replace("Video: ", "");
		}

		private static int ExtractVideoWidth(string rawInfo)
		{
			int result = 0;
			Regex regex = new Regex("(\\d{2,4})x(\\d{2,4})", RegexOptions.Compiled);
			Match match = regex.Match(rawInfo);
			if (match.Success)
			{
				int.TryParse(match.Groups[1].Value, out result);
			}
			return result;
		}

		private static int ExtractVideoHeight(string rawInfo)
		{
			int result = 0;
			Regex regex = new Regex("(\\d{2,4})x(\\d{2,4})", RegexOptions.Compiled);
			Match match = regex.Match(rawInfo);
			if (match.Success)
			{
				int.TryParse(match.Groups[2].Value, out result);
			}
			return result;
		}

		public static string RunProcess(string Parameters)
		{
			if (!File.Exists(FFmpegPath))
			{
				throw new FileNotFoundException("ffmpeg file not found");
			}
			ProcessStartInfo processStartInfo = new ProcessStartInfo(FFmpegPath, Parameters);
			processStartInfo.UseShellExecute = false;
			processStartInfo.CreateNoWindow = true;
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.RedirectStandardError = true;
			StringBuilder stringBuilder = new StringBuilder();
			using (Process process = Process.Start(processStartInfo))
			{
				do
				{
					string value = process.StandardError.ReadToEnd();
					stringBuilder.Append(value);
				}
				while (!process.HasExited);
				process.WaitForExit();
			}
			return stringBuilder.ToString();
		}

		public static Process BeginRunProcess(string Parameters)
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo(FFmpegPath, Parameters);
			processStartInfo.UseShellExecute = false;
			processStartInfo.CreateNoWindow = true;
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.RedirectStandardError = true;
			Process process = Process.Start(processStartInfo);
			process.ErrorDataReceived += proc_ErrorDataReceived;
			process.OutputDataReceived += proc_OutputDataReceived;
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();
			return process;
		}
	}
}
