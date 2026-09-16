using System;
using System.ComponentModel;
using System.Drawing;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using DeXin.EPG.Tools;

namespace EPGServiceManagement
{
	public class FormServiceProcess : Form
	{
		private IContainer components = null;

		private ProgressBar prbServiceController;

		private BackgroundWorker bgwServiceController;

		private ServiceController scEPG;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EPGServiceManagement.FormServiceProcess));
			this.prbServiceController = new System.Windows.Forms.ProgressBar();
			this.bgwServiceController = new System.ComponentModel.BackgroundWorker();
			this.scEPG = new System.ServiceProcess.ServiceController();
			base.SuspendLayout();
			resources.ApplyResources(this.prbServiceController, "prbServiceController");
			this.prbServiceController.Name = "prbServiceController";
			this.bgwServiceController.WorkerReportsProgress = true;
			this.bgwServiceController.DoWork += new System.ComponentModel.DoWorkEventHandler(bgwServiceController_DoWork);
			this.bgwServiceController.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bgwServiceController_ProgressChanged);
			this.bgwServiceController.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bgwServiceController_RunWorkerCompleted);
			this.scEPG.ServiceName = "EPGService";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.prbServiceController);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FormServiceProcess";
			base.ShowInTaskbar = false;
			base.Shown += new System.EventHandler(FormServiceProcess_Shown);
			base.ResumeLayout(false);
		}

		public FormServiceProcess(ServiceControllModel ControlModel)
		{
			InitializeComponent();
			bgwServiceController.RunWorkerAsync(ControlModel);
		}

		private void FormServiceProcess_Shown(object sender, EventArgs e)
		{
			if (base.Owner != null && base.Owner.WindowState != FormWindowState.Minimized)
			{
				int num = base.Owner.Width - base.Width;
				int num2 = base.Owner.Height - base.Height;
				num = ((num > 0) ? (base.Owner.Location.X + num / 2) : base.Owner.Location.X);
				num2 = ((num2 > 0) ? (base.Owner.Location.Y + num2 / 2) : base.Owner.Location.Y);
				base.Location = new Point(num, num2);
			}
		}

		private void bgwServiceController_DoWork(object sender, DoWorkEventArgs e)
		{
			bgwServiceController.ReportProgress(10);
			Thread.Sleep(100);
			ServiceControllModel serviceControllModel = (ServiceControllModel)e.Argument;
			bgwServiceController.ReportProgress(20);
			switch (serviceControllModel)
			{
			case ServiceControllModel.Start:
			{
				scEPG.Start(new string[9]
				{
					EPGSettings.Instatce.DBConnectionString,
					EPGSettings.Instatce.EventLoadeDays.ToString(),
					((int)EPGSettings.Instatce.SendTSType).ToString(),
					EPGSettings.Instatce.TSPlayerRate.ToString(),
					EPGConfigurationManger.Instance.Language,
					EPGConfigurationManger.Instance.LanguageUI,
					EPGSettings.Instatce.IsAddEmptyPackage.ToString(),
					EPGSettings.Instatce.TargetRate.ToString(),
					EPGSettings.Instatce.Package_Count.ToString()
				});
				int num = 10;
				bgwServiceController.ReportProgress(num);
				while (scEPG.Status != ServiceControllerStatus.Running)
				{
					int millisecondsTimeout = 1000;
					scEPG.Refresh();
					if (num < 60)
					{
						num += 10;
					}
					else if (num < 90)
					{
						num++;
						millisecondsTimeout = 2000;
					}
					bgwServiceController.ReportProgress(num);
					Thread.Sleep(millisecondsTimeout);
				}
				break;
			}
			case ServiceControllModel.Stop:
			{
				scEPG.Stop();
				int num = 10;
				bgwServiceController.ReportProgress(num);
				while (scEPG.Status != ServiceControllerStatus.Stopped)
				{
					scEPG.Refresh();
					if (num < 80)
					{
						num += 10;
					}
					bgwServiceController.ReportProgress(num);
					Thread.Sleep(500);
				}
				break;
			}
			case ServiceControllModel.Restart:
			{
				scEPG.Stop();
				int num = 10;
				bgwServiceController.ReportProgress(num);
				while (scEPG.Status != ServiceControllerStatus.Stopped)
				{
					scEPG.Refresh();
					if (num < 30)
					{
						num += 10;
					}
					bgwServiceController.ReportProgress(num);
					Thread.Sleep(1000);
				}
				Thread.Sleep(1000);
				scEPG.Start(new string[9]
				{
					EPGSettings.Instatce.DBConnectionString,
					EPGSettings.Instatce.EventLoadeDays.ToString(),
					((int)EPGSettings.Instatce.SendTSType).ToString(),
					EPGSettings.Instatce.TSPlayerRate.ToString(),
					EPGConfigurationManger.Instance.Language,
					EPGConfigurationManger.Instance.LanguageUI,
					EPGSettings.Instatce.IsAddEmptyPackage.ToString(),
					EPGSettings.Instatce.TargetRate.ToString(),
					EPGSettings.Instatce.Package_Count.ToString()
				});
				bgwServiceController.ReportProgress(40);
				while (scEPG.Status != ServiceControllerStatus.Running)
				{
					int millisecondsTimeout = 1000;
					scEPG.Refresh();
					if (num < 70)
					{
						num += 10;
					}
					else if (num < 90)
					{
						num++;
						millisecondsTimeout = 2000;
					}
					bgwServiceController.ReportProgress(num);
					Thread.Sleep(millisecondsTimeout);
				}
				break;
			}
			}
			bgwServiceController.ReportProgress(100);
			Thread.Sleep(1000);
		}

		private void bgwServiceController_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			prbServiceController.Value = e.ProgressPercentage;
		}

		private void bgwServiceController_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Thread.Sleep(500);
			base.DialogResult = DialogResult.OK;
		}
	}
}
