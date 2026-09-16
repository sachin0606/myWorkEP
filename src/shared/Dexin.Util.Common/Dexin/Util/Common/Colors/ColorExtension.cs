using System.Drawing;

namespace Dexin.Util.Common.Colors
{
	public static class ColorExtension
	{
		public static Color Gradual(this Color color, double level)
		{
			int r = color.R;
			r += (int)((double)(255 - r) * level);
			int g = color.G;
			g += (int)((double)(255 - g) * level);
			int b = color.B;
			b += (int)((double)(255 - b) * level);
			return Color.FromArgb(r, g, b);
		}
	}
}
