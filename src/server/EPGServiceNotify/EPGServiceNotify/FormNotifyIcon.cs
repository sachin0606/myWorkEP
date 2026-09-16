using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EPGServiceNotify
{
	public class FormNotifyIcon : Form
	{
		private Desktop m_Desktop = new Desktop();

		private BalloonInfo balloonMessage;

		private IContainer components = null;

		private NotifyIcon niEPGService;

		private Timer timerFormKiller;

		private Timer timerDeadFormKiller;

		public BalloonInfo BalloonMessage
		{
			get
			{
				return balloonMessage;
			}
			set
			{
				balloonMessage = value;
			}
		}

		public FormNotifyIcon()
		{
			InitializeComponent();
			NotifyIcon notifyIcon = niEPGService;
			EventHandler value = delegate
			{
				Exit();
			};
			notifyIcon.BalloonTipClosed += value;
			niEPGService.BalloonTipClicked += delegate
			{
				Exit();
			};
			niEPGService.BalloonTipShown += delegate
			{
				timerFormKiller.Start();
			};
		}

		private void FormNotifyIcon_Load(object sender, EventArgs e)
		{
			Icon icon;
			switch (balloonMessage.BalloonType)
			{
			case ToolTipIcon.Info:
				icon = new Icon(SystemIcons.Information, 16, 16);
				break;
			case ToolTipIcon.Warning:
				icon = new Icon(SystemIcons.Warning, 16, 16);
				break;
			case ToolTipIcon.Error:
				icon = new Icon(SystemIcons.Error, 16, 16);
				break;
			default:
				icon = new Icon(SystemIcons.Application, 16, 16);
				break;
			}
			niEPGService.Icon = icon;
			base.Visible = false;
			niEPGService.Visible = true;
			niEPGService.ShowBalloonTip(balloonMessage.Timeout, balloonMessage.Title, balloonMessage.Content, balloonMessage.BalloonType);
			timerFormKiller.Interval = balloonMessage.Timeout;
			timerDeadFormKiller.Interval = balloonMessage.Timeout * 4;
			timerDeadFormKiller.Start();
		}

		public void ShowMessage(object message)
		{
			if (!m_Desktop.BeginInteraction())
			{
				Exit();
				return;
			}
			balloonMessage = message as BalloonInfo;
			Application.Run(this);
		}

		public void Exit()
		{
			Invoke((FormCloseEventHandeller)delegate(Form notifyForm)
			{
				notifyForm.Close();
			}, this);
		}

		protected override void OnClosed(EventArgs e)
		{
			niEPGService.Visible = false;
			niEPGService.Dispose();
			components.Dispose();
			Dispose();
		}

		private void timerFormKiller_Tick(object sender, EventArgs e)
		{
			timerFormKiller.Stop();
			Close();
		}

		private void timerDeadFormKiller_Tick(object sender, EventArgs e)
		{
			timerDeadFormKiller.Stop();
			Exit();
		}

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EPGServiceNotify.FormNotifyIcon));
			this.niEPGService = new System.Windows.Forms.NotifyIcon(this.components);
			this.timerFormKiller = new System.Windows.Forms.Timer(this.components);
			this.timerDeadFormKiller = new System.Windows.Forms.Timer(this.components);
			base.SuspendLayout();
			resources.ApplyResources(this.niEPGService, "niEPGService");
			this.timerFormKiller.Tick += new System.EventHandler(timerFormKiller_Tick);
			this.timerDeadFormKiller.Tick += new System.EventHandler(timerDeadFormKiller_Tick);
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormNotifyIcon";
			base.ShowInTaskbar = false;
			base.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			base.Load += new System.EventHandler(FormNotifyIcon_Load);
			base.ResumeLayout(false);
		}
	}
}
