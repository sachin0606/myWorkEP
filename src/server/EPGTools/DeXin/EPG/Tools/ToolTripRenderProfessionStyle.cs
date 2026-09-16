using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace DeXin.EPG.Tools
{
	public class ToolTripRenderProfessionStyle : ToolStripProfessionalRenderer
	{
		private static Bitmap titleBarGripBmp;

		private static string titleBarGripEnc;

		private static Color titlebarColor1;

		private static Color titlebarColor2;

		private static Color titlebarColor3;

		private static Color titlebarColor4;

		private static Color titlebarColor5;

		private static Color titlebarColor6;

		private static Color titlebarColor7;

		private static Color borderColor;

		static ToolTripRenderProfessionStyle()
		{
			titleBarGripEnc = "Qk16AQAAAAAAADYAAAAoAAAAIwAAAAMAAAABABgAAAAAAAAAAADEDgAAxA4AAAAAAAAAAAAAuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5ANj+RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5ANj+RzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMANj+";
			titlebarColor1 = Color.FromArgb(89, 135, 214);
			titlebarColor2 = Color.FromArgb(76, 123, 204);
			titlebarColor3 = Color.FromArgb(63, 111, 194);
			titlebarColor4 = Color.FromArgb(50, 99, 184);
			titlebarColor5 = Color.FromArgb(38, 88, 174);
			titlebarColor6 = Color.FromArgb(25, 76, 164);
			titlebarColor7 = Color.FromArgb(12, 64, 154);
			borderColor = Color.FromArgb(0, 0, 128);
			titleBarGripBmp = DeserializeFromBase64(titleBarGripEnc);
		}

		internal static Bitmap DeserializeFromBase64(string data)
		{
			MemoryStream stream = new MemoryStream(Convert.FromBase64String(data));
			return new Bitmap(stream);
		}

		private void DrawTitleBar(Graphics g, Rectangle rect)
		{
		}

		protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
		{
			DrawTitleBar(e.Graphics, new Rectangle(0, 0, e.ToolStrip.Width, 7));
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			DrawTitleBar(e.Graphics, new Rectangle(0, 0, e.ToolStrip.Width, 7));
		}

		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			Graphics graphics = e.Graphics;
			Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
			Color color = Color.FromArgb(203, 225, 252);
			Color color2 = Color.FromArgb(125, 165, 224);
			ToolStripButton toolStripButton = e.Item as ToolStripButton;
			if (toolStripButton.Pressed || toolStripButton.Checked)
			{
				color = Color.FromArgb(254, 128, 62);
				color2 = Color.FromArgb(255, 223, 154);
			}
			else if (toolStripButton.Selected)
			{
				color = Color.FromArgb(255, 255, 222);
				color2 = Color.FromArgb(255, 203, 136);
			}
			using (Brush brush = new LinearGradientBrush(rect, color, color2, LinearGradientMode.Vertical))
			{
				graphics.FillRectangle(brush, rect);
			}
			e.Graphics.DrawRectangle(SystemPens.ControlDarkDark, rect);
			graphics.DrawLine(SystemPens.ControlDarkDark, rect.X, rect.Y, rect.Width - 1, rect.Y);
			graphics.DrawLine(SystemPens.ControlDarkDark, rect.X, rect.Y, rect.X, rect.Height - 1);
			ToolStrip owner = toolStripButton.Owner;
			ToolStripButton toolStripButton2 = toolStripButton.Owner.GetItemAt(toolStripButton.Bounds.X, toolStripButton.Bounds.Bottom + 1) as ToolStripButton;
			if (toolStripButton2 == null)
			{
				graphics.DrawLine(SystemPens.ControlDarkDark, rect.X, rect.Height - 1, rect.X + rect.Width - 1, rect.Height - 1);
			}
		}
	}
}
