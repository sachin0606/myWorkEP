using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL.EventFileHelper;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using EPGServiceBLL.Configuration;
using EPGServiceBLL.SendOut.NetSocket;
using EPGServiceNotify;

namespace EPGServiceManagement
{
	public class FormSetting : Form
	{
		private IContainer components = null;

		private GroupBox gbxDBSettings;

		private TableLayoutPanel tableLayoutPanel2;

		private Label lblIPSMS;

		private TextBox txtIP;

		private TextBox txtpwd;

		private Label lblDatabaseName;

		private Label lblPWD;

		private TextBox txtDataBase;

		private TextBox txtUid;

		private Label lblUserID;

		private GroupBox groupBox1;

		private TableLayoutPanel tableLayoutPanel3;

		private GroupBox groupBox2;

		private RadioButton rbtTSPlayer;

		private NumericUpDown ntxtTSRate;

		private Label label3;

		private Button btSetRate;

		private TableLayoutPanel tableLayoutPanel4;

		private CheckedListBox cklAllIP;

		private TableLayoutPanel tableLayoutPanel1;

		private Button btCancel;

		private Button btOK;

		private FlowLayoutPanel flowLayoutPanel1;

		private Label label4;

		private CheckBox ckAddEmptyPackage;

		private TableLayoutPanel tpEITSettings;

		private ComboBox cbxDateFormat;

		private CheckBox ckbAutoEPG;

		private Label label1;

		private NumericUpDown ntxtEventDays;

		private Label label5;

		private Label label6;

		private TableLayoutPanel tpEPGFilePath;

		private Button btSelectEPGPath;

		private TextBox txtEITFilePath;

		private Label label7;

		private FolderBrowserDialog fbEPGFilePath;

		private Label label8;

		private NumericUpDown nudPkg_cnt;

		private RadioButton rbtUDP;

		private Panel panel1;

		private NumericUpDown txtTargetRate;

		private SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EPGServiceManagement.FormSetting));
			this.gbxDBSettings = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.lblIPSMS = new System.Windows.Forms.Label();
			this.txtIP = new System.Windows.Forms.TextBox();
			this.txtpwd = new System.Windows.Forms.TextBox();
			this.lblDatabaseName = new System.Windows.Forms.Label();
			this.lblPWD = new System.Windows.Forms.Label();
			this.txtDataBase = new System.Windows.Forms.TextBox();
			this.txtUid = new System.Windows.Forms.TextBox();
			this.lblUserID = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.tpEITSettings = new System.Windows.Forms.TableLayoutPanel();
			this.cbxDateFormat = new System.Windows.Forms.ComboBox();
			this.ckbAutoEPG = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ntxtEventDays = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.tpEPGFilePath = new System.Windows.Forms.TableLayoutPanel();
			this.btSelectEPGPath = new System.Windows.Forms.Button();
			this.txtEITFilePath = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btCancel = new System.Windows.Forms.Button();
			this.btOK = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.btSetRate = new System.Windows.Forms.Button();
			this.ntxtTSRate = new System.Windows.Forms.NumericUpDown();
			this.rbtTSPlayer = new System.Windows.Forms.RadioButton();
			this.nudPkg_cnt = new System.Windows.Forms.NumericUpDown();
			this.label8 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.rbtUDP = new System.Windows.Forms.RadioButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.cklAllIP = new System.Windows.Forms.CheckedListBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.ckAddEmptyPackage = new System.Windows.Forms.CheckBox();
			this.txtTargetRate = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.fbEPGFilePath = new System.Windows.Forms.FolderBrowserDialog();
			this.gbxDBSettings.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tpEITSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.ntxtEventDays).BeginInit();
			this.tpEPGFilePath.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.ntxtTSRate).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.nudPkg_cnt).BeginInit();
			this.panel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.txtTargetRate).BeginInit();
			base.SuspendLayout();
			this.gbxDBSettings.Controls.Add(this.tableLayoutPanel2);
			resources.ApplyResources(this.gbxDBSettings, "gbxDBSettings");
			this.gbxDBSettings.Name = "gbxDBSettings";
			this.gbxDBSettings.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.lblIPSMS, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.txtIP, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.txtpwd, 1, 4);
			this.tableLayoutPanel2.Controls.Add(this.lblDatabaseName, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.lblPWD, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.txtDataBase, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.txtUid, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.lblUserID, 0, 3);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			resources.ApplyResources(this.lblIPSMS, "lblIPSMS");
			this.lblIPSMS.Name = "lblIPSMS";
			resources.ApplyResources(this.txtIP, "txtIP");
			this.txtIP.Name = "txtIP";
			resources.ApplyResources(this.txtpwd, "txtpwd");
			this.txtpwd.Name = "txtpwd";
			resources.ApplyResources(this.lblDatabaseName, "lblDatabaseName");
			this.lblDatabaseName.Name = "lblDatabaseName";
			this.lblDatabaseName.Tag = "";
			resources.ApplyResources(this.lblPWD, "lblPWD");
			this.lblPWD.Name = "lblPWD";
			resources.ApplyResources(this.txtDataBase, "txtDataBase");
			this.txtDataBase.Name = "txtDataBase";
			resources.ApplyResources(this.txtUid, "txtUid");
			this.txtUid.Name = "txtUid";
			resources.ApplyResources(this.lblUserID, "lblUserID");
			this.lblUserID.Name = "lblUserID";
			this.groupBox1.Controls.Add(this.tableLayoutPanel3);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
			this.tableLayoutPanel3.Controls.Add(this.tpEITSettings, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 3);
			this.tableLayoutPanel3.Controls.Add(this.groupBox2, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 2);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			resources.ApplyResources(this.tpEITSettings, "tpEITSettings");
			this.tpEITSettings.Controls.Add(this.cbxDateFormat, 0, 3);
			this.tpEITSettings.Controls.Add(this.ckbAutoEPG, 1, 1);
			this.tpEITSettings.Controls.Add(this.label1, 0, 0);
			this.tpEITSettings.Controls.Add(this.ntxtEventDays, 1, 0);
			this.tpEITSettings.Controls.Add(this.label5, 0, 1);
			this.tpEITSettings.Controls.Add(this.label6, 0, 2);
			this.tpEITSettings.Controls.Add(this.tpEPGFilePath, 1, 2);
			this.tpEITSettings.Controls.Add(this.label7, 0, 3);
			this.tpEITSettings.Name = "tpEITSettings";
			resources.ApplyResources(this.cbxDateFormat, "cbxDateFormat");
			this.cbxDateFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxDateFormat.FormattingEnabled = true;
			this.cbxDateFormat.Name = "cbxDateFormat";
			resources.ApplyResources(this.ckbAutoEPG, "ckbAutoEPG");
			this.ckbAutoEPG.Name = "ckbAutoEPG";
			this.ckbAutoEPG.UseVisualStyleBackColor = true;
			this.ckbAutoEPG.CheckedChanged += new System.EventHandler(ckbAutoEPG_CheckedChanged);
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			resources.ApplyResources(this.ntxtEventDays, "ntxtEventDays");
			this.ntxtEventDays.Maximum = new decimal(new int[4] { 64, 0, 0, 0 });
			this.ntxtEventDays.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
			this.ntxtEventDays.Name = "ntxtEventDays";
			this.ntxtEventDays.Value = new decimal(new int[4] { 7, 0, 0, 0 });
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			resources.ApplyResources(this.tpEPGFilePath, "tpEPGFilePath");
			this.tpEPGFilePath.Controls.Add(this.btSelectEPGPath, 1, 0);
			this.tpEPGFilePath.Controls.Add(this.txtEITFilePath, 0, 0);
			this.tpEPGFilePath.Name = "tpEPGFilePath";
			resources.ApplyResources(this.btSelectEPGPath, "btSelectEPGPath");
			this.btSelectEPGPath.Name = "btSelectEPGPath";
			this.btSelectEPGPath.UseVisualStyleBackColor = true;
			this.btSelectEPGPath.Click += new System.EventHandler(btSelectEPGPath_Click);
			resources.ApplyResources(this.txtEITFilePath, "txtEITFilePath");
			this.txtEITFilePath.Name = "txtEITFilePath";
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.btCancel, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.btOK, 0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.btCancel, "btCancel");
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Name = "btCancel";
			this.btCancel.UseVisualStyleBackColor = true;
			this.btCancel.Click += new System.EventHandler(btCancel_Click);
			resources.ApplyResources(this.btOK, "btOK");
			this.btOK.Name = "btOK";
			this.btOK.UseVisualStyleBackColor = true;
			this.btOK.Click += new System.EventHandler(btOK_Click);
			this.groupBox2.Controls.Add(this.tableLayoutPanel4);
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
			this.tableLayoutPanel4.Controls.Add(this.btSetRate, 3, 2);
			this.tableLayoutPanel4.Controls.Add(this.ntxtTSRate, 1, 2);
			this.tableLayoutPanel4.Controls.Add(this.rbtTSPlayer, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.nudPkg_cnt, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.label8, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.label3, 2, 2);
			this.tableLayoutPanel4.Controls.Add(this.rbtUDP, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.panel1, 1, 1);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			resources.ApplyResources(this.btSetRate, "btSetRate");
			this.btSetRate.Name = "btSetRate";
			this.btSetRate.UseVisualStyleBackColor = true;
			this.btSetRate.Click += new System.EventHandler(btSetRate_Click);
			resources.ApplyResources(this.ntxtTSRate, "ntxtTSRate");
			this.ntxtTSRate.Maximum = new decimal(new int[4] { 150, 0, 0, 0 });
			this.ntxtTSRate.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
			this.ntxtTSRate.Name = "ntxtTSRate";
			this.ntxtTSRate.Value = new decimal(new int[4] { 10, 0, 0, 0 });
			resources.ApplyResources(this.rbtTSPlayer, "rbtTSPlayer");
			this.rbtTSPlayer.Name = "rbtTSPlayer";
			this.rbtTSPlayer.TabStop = true;
			this.rbtTSPlayer.UseVisualStyleBackColor = true;
			this.rbtTSPlayer.CheckedChanged += new System.EventHandler(rbtTSPlayer_CheckedChanged);
			resources.ApplyResources(this.nudPkg_cnt, "nudPkg_cnt");
			this.nudPkg_cnt.Maximum = new decimal(new int[4] { 10, 0, 0, 0 });
			this.nudPkg_cnt.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
			this.nudPkg_cnt.Name = "nudPkg_cnt";
			this.nudPkg_cnt.Value = new decimal(new int[4] { 7, 0, 0, 0 });
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			resources.ApplyResources(this.rbtUDP, "rbtUDP");
			this.rbtUDP.Checked = true;
			this.rbtUDP.Name = "rbtUDP";
			this.rbtUDP.TabStop = true;
			this.rbtUDP.UseVisualStyleBackColor = true;
			this.tableLayoutPanel4.SetColumnSpan(this.panel1, 3);
			this.panel1.Controls.Add(this.cklAllIP);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			resources.ApplyResources(this.cklAllIP, "cklAllIP");
			this.cklAllIP.FormattingEnabled = true;
			this.cklAllIP.Name = "cklAllIP";
			this.cklAllIP.Tag = "";
			this.flowLayoutPanel1.Controls.Add(this.ckAddEmptyPackage);
			this.flowLayoutPanel1.Controls.Add(this.txtTargetRate);
			this.flowLayoutPanel1.Controls.Add(this.label4);
			resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			resources.ApplyResources(this.ckAddEmptyPackage, "ckAddEmptyPackage");
			this.ckAddEmptyPackage.Name = "ckAddEmptyPackage";
			this.ckAddEmptyPackage.UseVisualStyleBackColor = true;
			this.ckAddEmptyPackage.CheckedChanged += new System.EventHandler(ckAddEmptyPackage_CheckedChanged);
			resources.ApplyResources(this.txtTargetRate, "txtTargetRate");
			this.txtTargetRate.Maximum = new decimal(new int[4] { 1024, 0, 0, 0 });
			this.txtTargetRate.Name = "txtTargetRate";
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			this.fbEPGFilePath.ShowNewFolderButton = false;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.gbxDBSettings);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.Name = "FormSetting";
			base.Load += new System.EventHandler(FormSetting_Load);
			this.gbxDBSettings.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tpEITSettings.ResumeLayout(false);
			this.tpEITSettings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.ntxtEventDays).EndInit();
			this.tpEPGFilePath.ResumeLayout(false);
			this.tpEPGFilePath.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.ntxtTSRate).EndInit();
			((System.ComponentModel.ISupportInitialize)this.nudPkg_cnt).EndInit();
			this.panel1.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.txtTargetRate).EndInit();
			base.ResumeLayout(false);
		}

		public FormSetting()
		{
			InitializeComponent();
			List<string> dataSource = (from x in Dns.GetHostEntry(Dns.GetHostName()).AddressList
				where !x.IsIPv6LinkLocal
				select x.ToString()).ToList();
			cklAllIP.DataSource = dataSource;
		}

		private void FormSetting_Load(object sender, EventArgs e)
		{
			bool flag = true;
			sqlBuilder.ConnectionString = EPGSettings.Instatce.DBConnectionString;
			txtIP.Text = sqlBuilder.DataSource;
			txtDataBase.Text = sqlBuilder.InitialCatalog;
			txtUid.Text = sqlBuilder.UserID;
			txtpwd.Text = sqlBuilder.Password;
			ntxtEventDays.Value = EPGSettings.Instatce.EventLoadeDays;
			switch (EPGSettings.Instatce.SendTSType)
			{
			case EnumSendType.NetworkUDP:
				rbtUDP.Checked = true;
				break;
			case EnumSendType.TSPlayer:
				rbtTSPlayer.Checked = true;
				break;
			}
			ntxtTSRate.Value = EPGSettings.Instatce.TSPlayerRate;
			AutoShowtimerConfiguration autoShowtimerConfiguration = AutoShowtimer.Get();
			ckbAutoEPG.Checked = autoShowtimerConfiguration.IsAuto;
			txtEITFilePath.Text = autoShowtimerConfiguration.ShowtimPath;
			List<BaseKey> list = new List<BaseKey>();
			foreach (EnumDateFormat value in Enum.GetValues(typeof(EnumDateFormat)))
			{
				list.Add(new BaseKey
				{
					BKKey = value.ToString(),
					BKValue = ResourceManagement.GetLocationString(value.ToString())
				});
			}
			Tool.BindingComboBox(list, cbxDateFormat);
			List<string> localAdapterSettings = LocalAdapterSettingHelper.GetLocalAdapterSettings();
			for (int i = 0; i < cklAllIP.Items.Count; i++)
			{
				if (localAdapterSettings.Contains(cklAllIP.Items[i].ToString()))
				{
					cklAllIP.SetItemChecked(i, true);
				}
			}
			ckAddEmptyPackage.Checked = EPGSettings.Instatce.IsAddEmptyPackage;
			txtTargetRate.Text = EPGSettings.Instatce.TargetRate.ToString("F4", CultureInfo.CurrentCulture);
		}

		private void btOK_Click(object sender, EventArgs e)
		{
			sqlBuilder.InitialCatalog = txtDataBase.Text.Trim();
			sqlBuilder.DataSource = txtIP.Text.Trim();
			sqlBuilder.UserID = txtUid.Text.Trim();
			sqlBuilder.Password = txtpwd.Text.Trim();
			EPGSettings.Instatce.DBConnectionString = sqlBuilder.ConnectionString;
			EPGSettings.Instatce.EventLoadeDays = (int)ntxtEventDays.Value;
			EPGSettings.Instatce.Package_Count = (int)nudPkg_cnt.Value;
			LocalAdapterSettingHelper.SaveLocalAdapterSettings(cklAllIP.CheckedItems.Cast<string>().ToList());
			AutoShowtimerConfiguration autoShowtimerConfiguration = new AutoShowtimerConfiguration();
			autoShowtimerConfiguration.IsAuto = ckbAutoEPG.Checked;
			autoShowtimerConfiguration.ShowtimPath = txtEITFilePath.Text.Trim();
			autoShowtimerConfiguration.DateFormat = (EnumDateFormat)(object)Tool.GetEnum(typeof(EnumDateFormat), Tool.GetValue(cbxDateFormat));
			AutoShowtimer.Set(autoShowtimerConfiguration);
			if (rbtUDP.Checked)
			{
				EPGSettings.Instatce.SendTSType = EnumSendType.NetworkUDP;
			}
			else if (rbtTSPlayer.Checked)
			{
				EPGSettings.Instatce.SendTSType = EnumSendType.TSPlayer;
				EPGSettings.Instatce.TSPlayerRate = (int)ntxtTSRate.Value;
			}
			EPGSettings.Instatce.IsAddEmptyPackage = ckAddEmptyPackage.Checked;
			decimal result = 1m;
			decimal.TryParse(txtTargetRate.Text.Trim(), out result);
			EPGSettings.Instatce.TargetRate = result;
			EPGSettings.Instatce.Save();
			if (rbtTSPlayer.Checked)
			{
				if (!StreamPlayerManagement.Ins.OpenDevice())
				{
					throw new CustomException(ResourceManagement.GetLocationString("OpenTSPlayerError"), MessageType.SHOW);
				}
				StreamPlayerManagement.Ins.SetRate(EPGSettings.Instatce.TSPlayerRate);
				StreamPlayerManagement.Ins.CloseDevice();
			}
			BalloonInfo balloonInfo = new BalloonInfo();
			balloonInfo.BalloonType = ToolTipIcon.Warning;
			balloonInfo.Content = ResourceManagement.GetLocationString("OperateSucceedAfterServiceRestar");
			balloonInfo.Timeout = 10000;
			balloonInfo.Title = "EPG Service";
			NotifyFormManage.StartShowMessageUI(balloonInfo);
			base.DialogResult = DialogResult.OK;
		}

		private void btCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
		}

		private void rbtTSPlayer_CheckedChanged(object sender, EventArgs e)
		{
			NumericUpDown numericUpDown = ntxtTSRate;
			bool enabled = (btSetRate.Enabled = rbtTSPlayer.Checked);
			numericUpDown.Enabled = enabled;
			cklAllIP.Enabled = !rbtTSPlayer.Checked;
		}

		private void btSetRate_Click(object sender, EventArgs e)
		{
			EPGSettings.Instatce.TSPlayerRate = (int)ntxtTSRate.Value;
			EPGSettings.Instatce.Save();
			StreamPlayerManagement.Ins.SetRate(EPGSettings.Instatce.TSPlayerRate);
			BalloonInfo balloonInfo = new BalloonInfo();
			balloonInfo.BalloonType = ToolTipIcon.Info;
			balloonInfo.Content = ResourceManagement.GetLocationString("OperateSucceedNow");
			balloonInfo.Timeout = 10000;
			balloonInfo.Title = "EPG Service";
			NotifyFormManage.StartShowMessageUI(balloonInfo);
		}

		private void ckAddEmptyPackage_CheckedChanged(object sender, EventArgs e)
		{
			txtTargetRate.Enabled = ckAddEmptyPackage.Checked;
		}

		private void ckbAutoEPG_CheckedChanged(object sender, EventArgs e)
		{
			TableLayoutPanel tableLayoutPanel = tpEPGFilePath;
			bool enabled = (cbxDateFormat.Enabled = ckbAutoEPG.Checked);
			tableLayoutPanel.Enabled = enabled;
		}

		private void btSelectEPGPath_Click(object sender, EventArgs e)
		{
			if (fbEPGFilePath.ShowDialog() == DialogResult.OK)
			{
				txtEITFilePath.Text = fbEPGFilePath.SelectedPath;
			}
		}
	}
}
