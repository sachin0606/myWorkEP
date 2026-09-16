using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DeXin.EPG.Views.BaseForm;
using EPG.Views.Properties;

namespace EPG.Views
{
	public class FormSplashScreen : Form
	{
		private IContainer components = null;

		private Timer timerSplashScreen;

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timerSplashScreen = new System.Windows.Forms.Timer(this.components);
			base.SuspendLayout();
			this.timerSplashScreen.Enabled = true;
			this.timerSplashScreen.Interval = 3000;
			this.timerSplashScreen.Tick += new System.EventHandler(timerSplashScreen_Tick);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackgroundImage = EPG.Views.Properties.Resources.Sharp;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			base.ClientSize = new System.Drawing.Size(499, 304);
			this.DoubleBuffered = true;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FormSplashScreen";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FormSplashScreen";
			base.ResumeLayout(false);
		}

		public FormSplashScreen()
		{
			InitializeComponent();
		}

		private void timerSplashScreen_Tick(object sender, EventArgs e)
		{
			timerSplashScreen.Enabled = false;
			base.Visible = false;
			FormMain formMain = new FormMain();
			formMain.FormClosed += fm_FormClosed;
			formMain.Disposed += fm_Disposed;
			formMain.Show();
		}

		private void fm_Disposed(object sender, EventArgs e)
		{
			Dispose();
		}

		private void fm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Close();
		}
	}
}
