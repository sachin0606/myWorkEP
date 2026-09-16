using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Resource.ResourcesSpecialFiles;
using DeXin.EPG.Tools;
using EPGServiceBLL.SendOut.NetSocket;
using Microsoft.Win32;

namespace EPGServiceManagement
{
	public class FormManagement : Form
	{
		private IContainer components = null;

		private ContextMenuStrip cmsAppMenu;

		private NotifyIcon notifyIconEPGService;

		private ServiceController serviceControllerEPG;

		private ToolStripMenuItem tsmShow;

		private ToolStripMenuItem tsmStart;

		private ToolStripMenuItem tsmStop;

		private ToolStripMenuItem tsmRestart;

		private ToolStripMenuItem tsmExit;

		private BackgroundWorker bgwServiceState;

		private MenuStrip menuStrip1;

		private ToolStripMenuItem tsmSystem;

		private ToolStripMenuItem tsmiExit;

		private ToolStripMenuItem tsmiServiceManage;

		private ToolStripMenuItem tsmiStart;

		private ToolStripMenuItem tsmiStop;

		private ToolStripMenuItem tsmiRestart;

		private ToolStripMenuItem tsmSetting;

		private ToolStripMenuItem tsmOperation;

		private ToolStripMenuItem tsmAutoRefreshNotice;

		private StatusStrip statusStrip1;

		private ToolStripStatusLabel tslblServiceState;

		private System.Windows.Forms.Timer timerRefreshNotice;

		private ToolStrip tsToolBar;

		private ToolStripButton tsbSetting;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripButton tsbStart;

		private ToolStripButton tsbStop;

		private ToolStripButton tsbRestart;

		private ToolStripSeparator toolStripSeparator2;

		private ToolStripButton tsbRefresh;

		private ToolStripMenuItem tsmShowToolBar;

		private DataGridView dgvSendNotices;

		private ToolStripMenuItem languageSettingToolStripMenuItem;

		private ToolStripMenuItem chineseToolStripMenuItem;

		private ToolStripMenuItem englishToolStripMenuItem;

		private ToolStripStatusLabel tlslblSplit;

		private DataGridViewTextBoxColumn dcId;

		private DataGridViewTextBoxColumn dcCode;

		private DataGridViewTextBoxColumn dcLocation;

		private DataGridViewTextBoxColumn dcOperation;

		private DataGridViewTextBoxColumn dcTime;

		private ToolStripStatusLabel tlslblServiceStauts;

		private System.Windows.Forms.Timer timerNotifyKiller;

		private ToolStripMenuItem helpSettingToolStripMenuItem;

		private ToolStripMenuItem aboutSettingToolStripMenuItem;

		private ToolStripStatusLabel tslRateName;

		private ToolStripStatusLabel tslRate;

		private ToolStripStatusLabel tslRateUnit;

		private ToolStripMenuItem tsmAutoRefreshRate;

		private ToolStripMenuItem tsmOtherFunc;

		private ToolStripMenuItem tsmOtherOTA;

		private ToolStripMenuItem tsmOtherClearEGP;

		private int timeoutballonTip = 5000;

		private Thread tSpeed;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EPGServiceManagement.FormManagement));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.cmsAppMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmShow = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmStart = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmStop = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmRestart = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmExit = new System.Windows.Forms.ToolStripMenuItem();
			this.notifyIconEPGService = new System.Windows.Forms.NotifyIcon(this.components);
			this.serviceControllerEPG = new System.ServiceProcess.ServiceController();
			this.bgwServiceState = new System.ComponentModel.BackgroundWorker();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.tsmSystem = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmSetting = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiServiceManage = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiStart = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiStop = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiRestart = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmOtherFunc = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmOtherOTA = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmOtherClearEGP = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmOperation = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmShowToolBar = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmAutoRefreshNotice = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmAutoRefreshRate = new System.Windows.Forms.ToolStripMenuItem();
			this.languageSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.chineseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.tlslblServiceStauts = new System.Windows.Forms.ToolStripStatusLabel();
			this.tslblServiceState = new System.Windows.Forms.ToolStripStatusLabel();
			this.tlslblSplit = new System.Windows.Forms.ToolStripStatusLabel();
			this.tslRateName = new System.Windows.Forms.ToolStripStatusLabel();
			this.tslRate = new System.Windows.Forms.ToolStripStatusLabel();
			this.tslRateUnit = new System.Windows.Forms.ToolStripStatusLabel();
			this.timerRefreshNotice = new System.Windows.Forms.Timer(this.components);
			this.tsToolBar = new System.Windows.Forms.ToolStrip();
			this.tsbSetting = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbStart = new System.Windows.Forms.ToolStripButton();
			this.tsbStop = new System.Windows.Forms.ToolStripButton();
			this.tsbRestart = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
			this.dgvSendNotices = new System.Windows.Forms.DataGridView();
			this.dcId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcOperation = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.timerNotifyKiller = new System.Windows.Forms.Timer(this.components);
			this.cmsAppMenu.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.tsToolBar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvSendNotices).BeginInit();
			base.SuspendLayout();
			resources.ApplyResources(this.cmsAppMenu, "cmsAppMenu");
			this.cmsAppMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.tsmShow, this.tsmStart, this.tsmStop, this.tsmRestart, this.tsmExit });
			this.cmsAppMenu.Name = "cmsAppMenu";
			resources.ApplyResources(this.tsmShow, "tsmShow");
			this.tsmShow.Name = "tsmShow";
			this.tsmShow.Click += new System.EventHandler(tsmShow_Click);
			resources.ApplyResources(this.tsmStart, "tsmStart");
			this.tsmStart.Name = "tsmStart";
			this.tsmStart.Click += new System.EventHandler(tsmStart_Click);
			resources.ApplyResources(this.tsmStop, "tsmStop");
			this.tsmStop.Name = "tsmStop";
			this.tsmStop.Click += new System.EventHandler(tsmStop_Click);
			resources.ApplyResources(this.tsmRestart, "tsmRestart");
			this.tsmRestart.Name = "tsmRestart";
			this.tsmRestart.Click += new System.EventHandler(tsmRestart_Click);
			resources.ApplyResources(this.tsmExit, "tsmExit");
			this.tsmExit.Name = "tsmExit";
			this.tsmExit.Click += new System.EventHandler(tsmExit_Click);
			resources.ApplyResources(this.notifyIconEPGService, "notifyIconEPGService");
			this.notifyIconEPGService.ContextMenuStrip = this.cmsAppMenu;
			this.notifyIconEPGService.DoubleClick += new System.EventHandler(notifyIcon1_DoubleClick);
			this.serviceControllerEPG.ServiceName = "EPGService";
			this.bgwServiceState.DoWork += new System.ComponentModel.DoWorkEventHandler(bgwServiceState_DoWork);
			resources.ApplyResources(this.menuStrip1, "menuStrip1");
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.tsmSystem, this.tsmiServiceManage, this.tsmOtherFunc, this.tsmOperation, this.languageSettingToolStripMenuItem, this.helpSettingToolStripMenuItem });
			this.menuStrip1.Name = "menuStrip1";
			resources.ApplyResources(this.tsmSystem, "tsmSystem");
			this.tsmSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmSetting, this.tsmiExit });
			this.tsmSystem.Name = "tsmSystem";
			resources.ApplyResources(this.tsmSetting, "tsmSetting");
			this.tsmSetting.Name = "tsmSetting";
			this.tsmSetting.Click += new System.EventHandler(tsmSetting_Click);
			resources.ApplyResources(this.tsmiExit, "tsmiExit");
			this.tsmiExit.Name = "tsmiExit";
			this.tsmiExit.Click += new System.EventHandler(tsmExit_Click);
			resources.ApplyResources(this.tsmiServiceManage, "tsmiServiceManage");
			this.tsmiServiceManage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.tsmiStart, this.tsmiStop, this.tsmiRestart });
			this.tsmiServiceManage.Name = "tsmiServiceManage";
			resources.ApplyResources(this.tsmiStart, "tsmiStart");
			this.tsmiStart.Name = "tsmiStart";
			this.tsmiStart.Click += new System.EventHandler(tsmStart_Click);
			resources.ApplyResources(this.tsmiStop, "tsmiStop");
			this.tsmiStop.Name = "tsmiStop";
			this.tsmiStop.Click += new System.EventHandler(tsmStop_Click);
			resources.ApplyResources(this.tsmiRestart, "tsmiRestart");
			this.tsmiRestart.Name = "tsmiRestart";
			this.tsmiRestart.Click += new System.EventHandler(tsmRestart_Click);
			resources.ApplyResources(this.tsmOtherFunc, "tsmOtherFunc");
			this.tsmOtherFunc.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmOtherOTA, this.tsmOtherClearEGP });
			this.tsmOtherFunc.Name = "tsmOtherFunc";
			resources.ApplyResources(this.tsmOtherOTA, "tsmOtherOTA");
			this.tsmOtherOTA.Name = "tsmOtherOTA";
			this.tsmOtherOTA.Click += new System.EventHandler(tsmOtherOTA_Click);
			resources.ApplyResources(this.tsmOtherClearEGP, "tsmOtherClearEGP");
			this.tsmOtherClearEGP.Name = "tsmOtherClearEGP";
			this.tsmOtherClearEGP.Click += new System.EventHandler(tsmOtherClearEGP_Click);
			resources.ApplyResources(this.tsmOperation, "tsmOperation");
			this.tsmOperation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.tsmShowToolBar, this.tsmAutoRefreshNotice, this.tsmAutoRefreshRate });
			this.tsmOperation.Name = "tsmOperation";
			resources.ApplyResources(this.tsmShowToolBar, "tsmShowToolBar");
			this.tsmShowToolBar.Checked = true;
			this.tsmShowToolBar.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsmShowToolBar.Name = "tsmShowToolBar";
			this.tsmShowToolBar.Click += new System.EventHandler(tsmToolBar_Click);
			resources.ApplyResources(this.tsmAutoRefreshNotice, "tsmAutoRefreshNotice");
			this.tsmAutoRefreshNotice.Name = "tsmAutoRefreshNotice";
			this.tsmAutoRefreshNotice.CheckedChanged += new System.EventHandler(tsmAutoRefreshNotice_CheckedChanged);
			this.tsmAutoRefreshNotice.Click += new System.EventHandler(tsmAutoRefreshNotice_Click);
			resources.ApplyResources(this.tsmAutoRefreshRate, "tsmAutoRefreshRate");
			this.tsmAutoRefreshRate.Checked = true;
			this.tsmAutoRefreshRate.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsmAutoRefreshRate.Name = "tsmAutoRefreshRate";
			this.tsmAutoRefreshRate.CheckedChanged += new System.EventHandler(tsmAutoRefreshRate_CheckedChanged);
			this.tsmAutoRefreshRate.Click += new System.EventHandler(tsmAutoRefreshRate_Click);
			resources.ApplyResources(this.languageSettingToolStripMenuItem, "languageSettingToolStripMenuItem");
			this.languageSettingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.chineseToolStripMenuItem, this.englishToolStripMenuItem });
			this.languageSettingToolStripMenuItem.Name = "languageSettingToolStripMenuItem";
			resources.ApplyResources(this.chineseToolStripMenuItem, "chineseToolStripMenuItem");
			this.chineseToolStripMenuItem.Name = "chineseToolStripMenuItem";
			this.chineseToolStripMenuItem.Click += new System.EventHandler(chineseToolStripMenuItem_Click);
			resources.ApplyResources(this.englishToolStripMenuItem, "englishToolStripMenuItem");
			this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
			this.englishToolStripMenuItem.Click += new System.EventHandler(englishToolStripMenuItem_Click);
			resources.ApplyResources(this.helpSettingToolStripMenuItem, "helpSettingToolStripMenuItem");
			this.helpSettingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.aboutSettingToolStripMenuItem });
			this.helpSettingToolStripMenuItem.Name = "helpSettingToolStripMenuItem";
			resources.ApplyResources(this.aboutSettingToolStripMenuItem, "aboutSettingToolStripMenuItem");
			this.aboutSettingToolStripMenuItem.Name = "aboutSettingToolStripMenuItem";
			this.aboutSettingToolStripMenuItem.Click += new System.EventHandler(aboutSettingToolStripMenuItem_Click);
			resources.ApplyResources(this.statusStrip1, "statusStrip1");
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.tlslblServiceStauts, this.tslblServiceState, this.tlslblSplit, this.tslRateName, this.tslRate, this.tslRateUnit });
			this.statusStrip1.Name = "statusStrip1";
			resources.ApplyResources(this.tlslblServiceStauts, "tlslblServiceStauts");
			this.tlslblServiceStauts.Name = "tlslblServiceStauts";
			resources.ApplyResources(this.tslblServiceState, "tslblServiceState");
			this.tslblServiceState.Name = "tslblServiceState";
			resources.ApplyResources(this.tlslblSplit, "tlslblSplit");
			this.tlslblSplit.Name = "tlslblSplit";
			resources.ApplyResources(this.tslRateName, "tslRateName");
			this.tslRateName.Name = "tslRateName";
			resources.ApplyResources(this.tslRate, "tslRate");
			this.tslRate.Name = "tslRate";
			resources.ApplyResources(this.tslRateUnit, "tslRateUnit");
			this.tslRateUnit.Name = "tslRateUnit";
			this.timerRefreshNotice.Interval = 2000;
			this.timerRefreshNotice.Tick += new System.EventHandler(timerRefreshNotice_Tick);
			resources.ApplyResources(this.tsToolBar, "tsToolBar");
			this.tsToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.tsbSetting, this.toolStripSeparator1, this.tsbStart, this.tsbStop, this.tsbRestart, this.toolStripSeparator2, this.tsbRefresh });
			this.tsToolBar.Name = "tsToolBar";
			resources.ApplyResources(this.tsbSetting, "tsbSetting");
			this.tsbSetting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbSetting.Name = "tsbSetting";
			this.tsbSetting.Click += new System.EventHandler(tsmSetting_Click);
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.tsbStart, "tsbStart");
			this.tsbStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbStart.Name = "tsbStart";
			this.tsbStart.Click += new System.EventHandler(tsmStart_Click);
			resources.ApplyResources(this.tsbStop, "tsbStop");
			this.tsbStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbStop.Name = "tsbStop";
			this.tsbStop.Click += new System.EventHandler(tsmStop_Click);
			resources.ApplyResources(this.tsbRestart, "tsbRestart");
			this.tsbRestart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbRestart.Name = "tsbRestart";
			this.tsbRestart.Click += new System.EventHandler(tsmRestart_Click);
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.tsbRefresh, "tsbRefresh");
			this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbRefresh.Name = "tsbRefresh";
			this.tsbRefresh.Click += new System.EventHandler(tsbRefresh_Click);
			resources.ApplyResources(this.dgvSendNotices, "dgvSendNotices");
			this.dgvSendNotices.AllowUserToAddRows = false;
			this.dgvSendNotices.AllowUserToDeleteRows = false;
			this.dgvSendNotices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
			dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSendNotices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvSendNotices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSendNotices.Columns.AddRange(this.dcId, this.dcCode, this.dcLocation, this.dcOperation, this.dcTime);
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSendNotices.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvSendNotices.Name = "dgvSendNotices";
			this.dgvSendNotices.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSendNotices.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvSendNotices.RowTemplate.Height = 23;
			this.dcId.DataPropertyName = "Id";
			resources.ApplyResources(this.dcId, "dcId");
			this.dcId.Name = "dcId";
			this.dcId.ReadOnly = true;
			this.dcCode.DataPropertyName = "Code";
			resources.ApplyResources(this.dcCode, "dcCode");
			this.dcCode.Name = "dcCode";
			this.dcCode.ReadOnly = true;
			this.dcLocation.DataPropertyName = "Location";
			resources.ApplyResources(this.dcLocation, "dcLocation");
			this.dcLocation.Name = "dcLocation";
			this.dcLocation.ReadOnly = true;
			this.dcOperation.DataPropertyName = "Operation";
			resources.ApplyResources(this.dcOperation, "dcOperation");
			this.dcOperation.Name = "dcOperation";
			this.dcOperation.ReadOnly = true;
			this.dcTime.DataPropertyName = "EditDate";
			resources.ApplyResources(this.dcTime, "dcTime");
			this.dcTime.Name = "dcTime";
			this.dcTime.ReadOnly = true;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.dgvSendNotices);
			base.Controls.Add(this.tsToolBar);
			base.Controls.Add(this.statusStrip1);
			base.Controls.Add(this.menuStrip1);
			base.MainMenuStrip = this.menuStrip1;
			base.Name = "FormManagement";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FormManagement_FormClosing);
			base.Load += new System.EventHandler(FormManagement_Load);
			base.SizeChanged += new System.EventHandler(FormManagement_SizeChanged);
			this.cmsAppMenu.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.tsToolBar.ResumeLayout(false);
			this.tsToolBar.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvSendNotices).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormManagement()
		{
			InitializeComponent();
			Control.CheckForIllegalCrossThreadCalls = false;
			Text = ResourceSpecial.EditionEPG;
			dgvSendNotices.AutoGenerateColumns = false;
			bgwServiceState.RunWorkerAsync(1000);
			timerNotifyKiller.Interval = timeoutballonTip;
			System.Windows.Forms.Timer timer = timerNotifyKiller;
			EventHandler value = delegate
			{
				timerNotifyKiller.Stop();
				notifyIconEPGService.Visible = false;
				notifyIconEPGService.Visible = true;
			};
			timer.Tick += value;
			notifyIconEPGService.BalloonTipShown += delegate
			{
				timerNotifyKiller.Start();
			};
			SystemEvents.SessionSwitch += delegate(object sender, SessionSwitchEventArgs e)
			{
				Tool.WriteTXTLog(string.Format("{0} by {1}", e.Reason.ToString(), Environment.UserName));
			};
		}

		private void FormManagement_Load(object sender, EventArgs e)
		{
			CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
			if (currentUICulture.Equals(new CultureInfo("zh-CN")))
			{
				chineseToolStripMenuItem.Checked = true;
			}
			else if (currentUICulture.Equals(new CultureInfo("en-US")))
			{
				englishToolStripMenuItem.Checked = true;
			}
			if (!EPGRegistor.CheckRegistServer())
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("SoftOutOfDate"));
				Application.Exit();
			}
			else if (tsmAutoRefreshRate.Checked)
			{
				tsmAutoRefreshRate_CheckedChanged(tsmAutoRefreshRate, e);
			}
		}

		private void FormManagement_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				base.WindowState = FormWindowState.Minimized;
				e.Cancel = true;
			}
			if (tSpeed != null)
			{
				tSpeed.Abort();
			}
		}

		private void FormManagement_SizeChanged(object sender, EventArgs e)
		{
			if (base.WindowState == FormWindowState.Minimized)
			{
				base.ShowInTaskbar = false;
				notifyIconEPGService.ShowBalloonTip(timeoutballonTip, "EPGServiceManagement", ResourceManagement.GetLocationString("ProgramRunningHere"), ToolTipIcon.Info);
			}
			else
			{
				base.ShowInTaskbar = true;
			}
		}

		private void notifyIcon1_DoubleClick(object sender, EventArgs e)
		{
			base.WindowState = ((base.WindowState != FormWindowState.Minimized) ? FormWindowState.Minimized : FormWindowState.Normal);
		}

		private void tsmSetting_Click(object sender, EventArgs e)
		{
			try
			{
				if (new FormSetting().ShowDialog() == DialogResult.OK)
				{
					GetSendNoticeData();
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void tsmExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void tsmShow_Click(object sender, EventArgs e)
		{
			base.WindowState = FormWindowState.Normal;
		}

		private void tsmStart_Click(object sender, EventArgs e)
		{
			if (serviceControllerEPG.Status != ServiceControllerStatus.Running)
			{
				new FormServiceProcess(ServiceControllModel.Start).ShowDialog(this);
			}
		}

		private void tsmStop_Click(object sender, EventArgs e)
		{
			if (serviceControllerEPG.Status != ServiceControllerStatus.Stopped)
			{
				new FormServiceProcess(ServiceControllModel.Stop).ShowDialog(this);
			}
		}

		private void tsmRestart_Click(object sender, EventArgs e)
		{
			if (serviceControllerEPG.Status == ServiceControllerStatus.Running)
			{
				new FormServiceProcess(ServiceControllModel.Restart).ShowDialog(this);
			}
		}

		private void tsmToolBar_Click(object sender, EventArgs e)
		{
			tsmShowToolBar.Checked = !tsmShowToolBar.Checked;
			tsToolBar.Visible = tsmShowToolBar.Checked;
		}

		private void tsbRefresh_Click(object sender, EventArgs e)
		{
			try
			{
				GetSendNoticeData();
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void tsmAutoRefreshNotice_Click(object sender, EventArgs e)
		{
			tsmAutoRefreshNotice.Checked = !tsmAutoRefreshNotice.Checked;
		}

		private void tsmAutoRefreshNotice_CheckedChanged(object sender, EventArgs e)
		{
			if (tsmAutoRefreshNotice.Checked)
			{
				timerRefreshNotice.Start();
			}
			else
			{
				timerRefreshNotice.Stop();
			}
		}

		private void timerRefreshNotice_Tick(object sender, EventArgs e)
		{
			try
			{
				GetSendNoticeData();
			}
			catch (Exception ex)
			{
				tsmAutoRefreshNotice.Checked = false;
				Tool.ShowMessage(ex);
			}
		}

		private void tsmAutoRefreshRate_Click(object sender, EventArgs e)
		{
			tsmAutoRefreshRate.Checked = !tsmAutoRefreshRate.Checked;
		}

		private void tsmAutoRefreshRate_CheckedChanged(object sender, EventArgs e)
		{
			if (tsmAutoRefreshRate.Checked)
			{
				tSpeed = new Thread(showServiceSpeed);
				tSpeed.Start();
			}
			else if (tSpeed != null)
			{
				tSpeed.Abort();
			}
		}

		private void showServiceSpeed()
		{
			try
			{
				while (true)
				{
					bool flag = true;
					if (serviceControllerEPG.Status != ServiceControllerStatus.Running)
					{
						tslRate.Text = "0.0000";
					}
					else
					{
						byte[] value = UdpClientManager.Recieve(IPAddress.Any, 9098);
						double num = BitConverter.ToDouble(value, 0);
						tslRate.Text = num.ToString("N4", CultureInfo.CurrentCulture);
					}
					Thread.Sleep(2000);
				}
			}
			catch (ThreadAbortException)
			{
			}
			catch (Exception ex2)
			{
				tslRate.Text = "0.0000";
				Tool.WriteTXTLog("get send speed failed! exception is below:");
				Tool.WriteTXTLog(ex2);
			}
		}

		private void bgwServiceState_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				while (true)
				{
					serviceControllerEPG.Refresh();
					switch (serviceControllerEPG.Status)
					{
					case ServiceControllerStatus.Running:
					{
						ToolStripMenuItem toolStripMenuItem5 = tsmRestart;
						bool enabled = (tsmStop.Enabled = true);
						toolStripMenuItem5.Enabled = enabled;
						tsmStart.Enabled = false;
						ToolStripMenuItem toolStripMenuItem6 = tsmiRestart;
						enabled = (tsmiStop.Enabled = true);
						toolStripMenuItem6.Enabled = enabled;
						tsmiStart.Enabled = false;
						ToolStripButton toolStripButton3 = tsbRestart;
						enabled = (tsbStop.Enabled = true);
						toolStripButton3.Enabled = enabled;
						tsbStart.Enabled = false;
						break;
					}
					case ServiceControllerStatus.Stopped:
					{
						ToolStripMenuItem toolStripMenuItem3 = tsmRestart;
						bool enabled = (tsmStop.Enabled = false);
						toolStripMenuItem3.Enabled = enabled;
						tsmStart.Enabled = true;
						ToolStripMenuItem toolStripMenuItem4 = tsmiRestart;
						enabled = (tsmiStop.Enabled = false);
						toolStripMenuItem4.Enabled = enabled;
						tsmiStart.Enabled = true;
						ToolStripButton toolStripButton2 = tsbRestart;
						enabled = (tsbStop.Enabled = false);
						toolStripButton2.Enabled = enabled;
						tsbStart.Enabled = true;
						break;
					}
					default:
					{
						ToolStripMenuItem toolStripMenuItem = tsmRestart;
						bool enabled = (tsmStop.Enabled = false);
						toolStripMenuItem.Enabled = enabled;
						tsmStart.Enabled = false;
						ToolStripMenuItem toolStripMenuItem2 = tsmiRestart;
						enabled = (tsmiStop.Enabled = false);
						toolStripMenuItem2.Enabled = enabled;
						tsmiStart.Enabled = false;
						ToolStripButton toolStripButton = tsbRestart;
						enabled = (tsbStop.Enabled = false);
						toolStripButton.Enabled = enabled;
						tsbStart.Enabled = false;
						break;
					}
					}
					tslblServiceState.Text = serviceControllerEPG.Status.ToString();
					Thread.Sleep((int)e.Argument);
				}
			}
			catch (Exception ex)
			{
				ToolStripMenuItem toolStripMenuItem7 = tsmRestart;
				bool enabled = (tsmStop.Enabled = false);
				toolStripMenuItem7.Enabled = enabled;
				tsmStart.Enabled = false;
				ToolStripMenuItem toolStripMenuItem8 = tsmiRestart;
				enabled = (tsmiStop.Enabled = false);
				toolStripMenuItem8.Enabled = enabled;
				tsmiStart.Enabled = false;
				ToolStripButton toolStripButton4 = tsbRestart;
				enabled = (tsbStop.Enabled = false);
				toolStripButton4.Enabled = enabled;
				tsbStart.Enabled = false;
				Tool.ShowMessage(new CustomException(ResourceManagement.GetLocationString("EPGServiceNotInstalledorDestroyed"), ex, MessageType.SHOWANDLOG));
			}
		}

		private void GetSendNoticeData()
		{
			try
			{
				IList<SendNotices> all = new SendNoticesManager().GetAll();
				dgvSendNotices.DataSource = all;
			}
			catch (Exception ex)
			{
				dgvSendNotices.DataSource = null;
				if (ex is SqlException)
				{
					tsmAutoRefreshNotice.Checked = false;
					Tool.ShowMessage(ResourceManagement.GetLocationString("DatabaseConnectionException"));
					tsmSetting_Click(ex, null);
					return;
				}
				throw ex;
			}
		}

		private void chineseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetLanguage("zh-CN", "zh-CN");
			chineseToolStripMenuItem.Checked = true;
			englishToolStripMenuItem.Checked = false;
		}

		private void englishToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetLanguage("en", "en-US");
			chineseToolStripMenuItem.Checked = false;
			englishToolStripMenuItem.Checked = true;
		}

		private void SetLanguage(string languageUI, string language)
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageUI);
			Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
			EPGConfigurationManger.Instance.Language = language;
			EPGConfigurationManger.Instance.LanguageUI = languageUI;
			EPGConfigurationManger.Instance.Save();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormManagement));
			componentResourceManager.ApplyResources(tsmShow, "tsmShow");
			componentResourceManager.ApplyResources(tsmStart, "tsmStart");
			componentResourceManager.ApplyResources(tsmStop, "tsmStop");
			componentResourceManager.ApplyResources(tsmRestart, "tsmRestart");
			componentResourceManager.ApplyResources(tsmExit, "tsmExit");
			componentResourceManager.ApplyResources(notifyIconEPGService, "notifyIconEPGService");
			componentResourceManager.ApplyResources(menuStrip1, "menuStrip1");
			componentResourceManager.ApplyResources(tsmSystem, "tsmSystem");
			componentResourceManager.ApplyResources(tsmSetting, "tsmSetting");
			componentResourceManager.ApplyResources(tsmiExit, "tsmiExit");
			componentResourceManager.ApplyResources(tsmiServiceManage, "tsmiServiceManage");
			componentResourceManager.ApplyResources(tsmiStart, "tsmiStart");
			componentResourceManager.ApplyResources(tsmiStop, "tsmiStop");
			componentResourceManager.ApplyResources(tsmiRestart, "tsmiRestart");
			componentResourceManager.ApplyResources(tsmOperation, "tsmOperation");
			componentResourceManager.ApplyResources(tsmShowToolBar, "tsmShowToolBar");
			componentResourceManager.ApplyResources(tsmAutoRefreshNotice, "tsmAutoRefreshNotice");
			componentResourceManager.ApplyResources(tsmAutoRefreshRate, "tsmAutoRefreshRate");
			componentResourceManager.ApplyResources(languageSettingToolStripMenuItem, "languageSettingToolStripMenuItem");
			componentResourceManager.ApplyResources(chineseToolStripMenuItem, "chineseToolStripMenuItem");
			componentResourceManager.ApplyResources(englishToolStripMenuItem, "englishToolStripMenuItem");
			componentResourceManager.ApplyResources(statusStrip1, "statusStrip1");
			componentResourceManager.ApplyResources(tslblServiceState, "tslblServiceState");
			componentResourceManager.ApplyResources(tsToolBar, "tsToolBar");
			componentResourceManager.ApplyResources(tsbSetting, "tsbSetting");
			componentResourceManager.ApplyResources(toolStripSeparator1, "toolStripSeparator1");
			componentResourceManager.ApplyResources(tsbStart, "tsbStart");
			componentResourceManager.ApplyResources(tsbStop, "tsbStop");
			componentResourceManager.ApplyResources(tsbRestart, "tsbRestart");
			componentResourceManager.ApplyResources(toolStripSeparator2, "toolStripSeparator2");
			componentResourceManager.ApplyResources(tsbRefresh, "tsbRefresh");
			componentResourceManager.ApplyResources(dgvSendNotices, "dgvSendNotices");
			componentResourceManager.ApplyResources(this, "$this");
			componentResourceManager.ApplyResources(tslRateName, "tslRateName");
			componentResourceManager.ApplyResources(tslRateUnit, "tslRateUnit");
			componentResourceManager.ApplyResources(dcId, "dcId");
			componentResourceManager.ApplyResources(dcCode, "dcCode");
			componentResourceManager.ApplyResources(dcLocation, "dcLocation");
			componentResourceManager.ApplyResources(dcOperation, "dcOperation");
			componentResourceManager.ApplyResources(dcTime, "dcTime");
			componentResourceManager.ApplyResources(tlslblServiceStauts, "tlslblServiceStauts");
			componentResourceManager.ApplyResources(helpSettingToolStripMenuItem, "helpSettingToolStripMenuItem");
			componentResourceManager.ApplyResources(aboutSettingToolStripMenuItem, "aboutSettingToolStripMenuItem");
		}

		private void aboutSettingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new FormAboutBox().ShowDialog();
		}

		private void tsmOtherOTA_Click(object sender, EventArgs e)
		{
			string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TsSenders.exe");
			Process.Start(fileName);
		}

		private void tsmOtherClearEGP_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(ResourceManagement.GetLocationString("ConfirmClearEGP"), ResourceManagement.GetLocationString("systitle"), MessageBoxButtons.OKCancel) != DialogResult.OK)
			{
				return;
			}
			try
			{
				new EITInfoManager().ClearEITInfos();
				MessageBox.Show(ResourceManagement.GetLocationString("OperateSucceed"));
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}
	}
}
