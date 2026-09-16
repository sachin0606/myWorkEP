using System;

namespace Util.Common
{
	public class BDLocaltion
	{
		private static double pi = Math.PI;

		private static double a = 6378245.0;

		private static double ee = 0.006693421622965943;

		private static double x_pi = pi * 3000.0 / 180.0;

		public static double[] wgs2bd(double lat, double lon)
		{
			double[] array = wgs2gcj(lat, lon);
			return gcj2bd(array[0], array[1]);
		}

		public static double[] gcj2bd(double lat, double lon)
		{
			double num = Math.Sqrt(lon * lon + lat * lat) + 2E-05 * Math.Sin(lat * x_pi);
			double d = Math.Atan2(lat, lon) + 3E-06 * Math.Cos(lon * x_pi);
			double num2 = num * Math.Cos(d) + 0.0065;
			double num3 = num * Math.Sin(d) + 0.006;
			return new double[2] { num3, num2 };
		}

		public static double[] bd2gcj(double lat, double lon)
		{
			double num = lon - 0.0065;
			double num2 = lat - 0.006;
			double num3 = Math.Sqrt(num * num + num2 * num2) - 2E-05 * Math.Sin(num2 * x_pi);
			double d = Math.Atan2(num2, num) - 3E-06 * Math.Cos(num * x_pi);
			double num4 = num3 * Math.Cos(d);
			double num5 = num3 * Math.Sin(d);
			return new double[2] { num5, num4 };
		}

		private static double[] wgs2gcj(double lat, double lon)
		{
			double num = transformLat(lon - 105.0, lat - 35.0);
			double num2 = transformLon(lon - 105.0, lat - 35.0);
			double d = lat / 180.0 * pi;
			double num3 = Math.Sin(d);
			num3 = 1.0 - ee * num3 * num3;
			double num4 = Math.Sqrt(num3);
			num = num * 180.0 / (a * (1.0 - ee) / (num3 * num4) * pi);
			num2 = num2 * 180.0 / (a / num4 * Math.Cos(d) * pi);
			double num5 = lat + num;
			double num6 = lon + num2;
			return new double[2] { num5, num6 };
		}

		private static double transformLat(double lat, double lon)
		{
			double num = -100.0 + 2.0 * lat + 3.0 * lon + 0.2 * lon * lon + 0.1 * lat * lon + 0.2 * Math.Sqrt(Math.Abs(lat));
			num += (20.0 * Math.Sin(6.0 * lat * pi) + 20.0 * Math.Sin(2.0 * lat * pi)) * 2.0 / 3.0;
			num += (20.0 * Math.Sin(lon * pi) + 40.0 * Math.Sin(lon / 3.0 * pi)) * 2.0 / 3.0;
			return num + (160.0 * Math.Sin(lon / 12.0 * pi) + 320.0 * Math.Sin(lon * pi / 30.0)) * 2.0 / 3.0;
		}

		private static double transformLon(double lat, double lon)
		{
			double num = 300.0 + lat + 2.0 * lon + 0.1 * lat * lat + 0.1 * lat * lon + 0.1 * Math.Sqrt(Math.Abs(lat));
			num += (20.0 * Math.Sin(6.0 * lat * pi) + 20.0 * Math.Sin(2.0 * lat * pi)) * 2.0 / 3.0;
			num += (20.0 * Math.Sin(lat * pi) + 40.0 * Math.Sin(lat / 3.0 * pi)) * 2.0 / 3.0;
			return num + (150.0 * Math.Sin(lat / 12.0 * pi) + 300.0 * Math.Sin(lat / 30.0 * pi)) * 2.0 / 3.0;
		}
	}
}
