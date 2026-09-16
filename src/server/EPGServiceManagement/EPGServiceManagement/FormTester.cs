using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Com.Cn.Tool;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXinEPGSoftDog;
using EPGServiceBLL.SendOut;
using EPGServiceBLL.SendOut.NetSocket;
using EPGServiceNotify;
using TsSenders;

namespace EPGServiceManagement
{
	public class FormTester : Form
	{
		public delegate void ThreadChangeForProgressBarHandller(ProgressBar progressBar, int value);

		private Thread TSend;

		private Thread TSpeed;

		private IContainer components = null;

		private GroupBox groupBox3;

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

		private GroupBox groupBox2;

		private TableLayoutPanel tableLayoutPanel1;

		private RadioButton rbtTSPlayer;

		private RadioButton rbtTCP;

		private FlowLayoutPanel pTSPlayerSetting;

		private Label label2;

		private TextBox txtTSRate;

		private Button btSetRate;

		private Button btTSPlayerTest;

		private RadioButton rbtFile;

		private RadioButton rbtUDP;

		private Panel pNetSettings;

		private Label label1;

		private TableLayoutPanel tableLayoutPanel3;

		private TextBox txtEventDays;

		private Label label4;

		private GroupBox groupBox4;

		private TableLayoutPanel tableLayoutPanel4;

		private Button btStop;

		private Button btSend;

		private Button btApplySet;

		private ProgressBar pbStop;

		private CheckedListBox cklAllIP;

		private Label label3;

		private Label lRate;

		private Button btStartSpeed;

		private FlowLayoutPanel flowLayoutPanel1;

		private CheckBox ckIsAddRate;

		private TextBox txtRateAdd;

		private FlowLayoutPanel flowLayoutPanel2;

		private Button btnOta;

		private FlowLayoutPanel flowLayoutPanel3;

		private TextBox txtPkgcnt;

		private Label label5;

		private Label label6;

		private TextBox txtNullPkgPID;

		public FormTester()
		{
			InitializeComponent();
			Control.CheckForIllegalCrossThreadCalls = false;
			ServiceExitEventHandeller value = delegate
			{
				btStop_Click(null, null);
			};
			TSOutputManager.ServiceExiting += value;
			List<string> dataSource = (from x in Dns.GetHostEntry(Dns.GetHostName()).AddressList
				where !x.IsIPv6LinkLocal
				select x.ToString()).ToList();
			cklAllIP.DataSource = dataSource;
			Control.CheckForIllegalCrossThreadCalls = false;
			TSOutputManager.ServerStopProcessChanged += delegate(int process)
			{
				try
				{
					pbStop.Invoke((ThreadChangeForProgressBarHandller)delegate(ProgressBar bar, int value2)
					{
						bar.Value = value2;
					}, pbStop, process);
				}
				catch
				{
				}
				if (process >= 100)
				{
					btSend.Enabled = true;
					RadioButton radioButton = rbtFile;
					RadioButton radioButton2 = rbtTCP;
					RadioButton radioButton3 = rbtTSPlayer;
					bool flag = (rbtUDP.Enabled = true);
					flag = (radioButton3.Enabled = flag);
					flag = (radioButton2.Enabled = flag);
					radioButton.Enabled = flag;
					if (TSend != null)
					{
						TSend.Abort();
					}
					MessageBox.Show("Stop service success", "Stop service", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			};
			TSOutputManager.ServiceExiting += delegate
			{
				btSend.Enabled = true;
				RadioButton radioButton = rbtFile;
				RadioButton radioButton2 = rbtTCP;
				RadioButton radioButton3 = rbtTSPlayer;
				bool flag = (rbtUDP.Enabled = true);
				flag = (radioButton3.Enabled = flag);
				flag = (radioButton2.Enabled = flag);
				radioButton.Enabled = flag;
				if (TSend != null)
				{
					TSend.Abort();
				}
			};
		}

		private void FormTester_Load(object sender, EventArgs e)
		{
			SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
			sqlConnectionStringBuilder.ConnectionString = EPGSettings.Instatce.DBConnectionString;
			txtIP.Text = sqlConnectionStringBuilder.DataSource;
			txtDataBase.Text = sqlConnectionStringBuilder.InitialCatalog;
			txtUid.Text = sqlConnectionStringBuilder.UserID;
			txtpwd.Text = sqlConnectionStringBuilder.Password;
			txtEventDays.Text = EPGSettings.Instatce.EventLoadeDays.ToString();
			txtPkgcnt.Text = EPGSettings.Instatce.Package_Count.ToString();
			switch (EPGSettings.Instatce.SendTSType)
			{
			case EnumSendType.NetworkTCP:
				rbtTCP.Checked = true;
				break;
			case EnumSendType.NetworkUDP:
				rbtUDP.Checked = true;
				break;
			case EnumSendType.TSPlayer:
				rbtTSPlayer.Checked = true;
				break;
			case EnumSendType.File:
				rbtFile.Checked = true;
				break;
			}
			txtTSRate.Text = EPGSettings.Instatce.TSPlayerRate.ToString();
			ckIsAddRate.Checked = EPGSettings.Instatce.IsAddEmptyPackage;
			txtRateAdd.Text = EPGSettings.Instatce.TargetRate.ToString();
			txtRateAdd.Enabled = ckIsAddRate.Checked;
			txtNullPkgPID.Text = EPGSettings.Instatce.NullPkgPID.ToString("X");
			List<string> localAdapterSettings = LocalAdapterSettingHelper.GetLocalAdapterSettings();
			for (int i = 0; i < cklAllIP.Items.Count; i++)
			{
				if (localAdapterSettings.Contains(cklAllIP.Items[i].ToString()))
				{
					cklAllIP.SetItemChecked(i, true);
				}
			}
		}

		private void btApplySet_Click(object sender, EventArgs e)
		{
			SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
			sqlConnectionStringBuilder.InitialCatalog = txtDataBase.Text.Trim();
			sqlConnectionStringBuilder.DataSource = txtIP.Text.Trim();
			sqlConnectionStringBuilder.UserID = txtUid.Text.Trim();
			sqlConnectionStringBuilder.Password = txtpwd.Text.Trim();
			EPGSettings.Instatce.DBConnectionString = sqlConnectionStringBuilder.ConnectionString;
			EPGSettings.Instatce.EventLoadeDays = Tool.GetInt32(txtEventDays.Text);
			EPGSettings.Instatce.TSPlayerRate = Tool.GetInt32(txtTSRate.Text);
			EPGSettings.Instatce.Package_Count = Tool.GetInt32(txtPkgcnt.Text);
			EPGSettings.Instatce.IsAddEmptyPackage = ckIsAddRate.Checked;
			EPGSettings.Instatce.TargetRate = Tool.GetDecimal(txtRateAdd.Text);
			LocalAdapterSettingHelper.SaveLocalAdapterSettings(cklAllIP.CheckedItems.Cast<string>().ToList());
			if (rbtTCP.Checked)
			{
				EPGSettings.Instatce.SendTSType = EnumSendType.NetworkTCP;
			}
			else if (rbtUDP.Checked)
			{
				EPGSettings.Instatce.SendTSType = EnumSendType.NetworkUDP;
			}
			else if (rbtTSPlayer.Checked)
			{
				EPGSettings.Instatce.SendTSType = EnumSendType.TSPlayer;
				StreamPlayerManagement.Ins.OpenDevice();
				StreamPlayerManagement.Ins.SetRate(EPGSettings.Instatce.TSPlayerRate);
			}
			else if (rbtFile.Checked)
			{
				EPGSettings.Instatce.SendTSType = EnumSendType.File;
			}
			EPGSettings.Instatce.NullPkgPID = ushort.Parse(txtNullPkgPID.Text, NumberStyles.AllowHexSpecifier);
			EPGSettings.Instatce.Save();
			if (((Button)sender).Name.Equals(btApplySet.Name))
			{
				BalloonInfo balloonInfo = new BalloonInfo();
				balloonInfo.BalloonType = ToolTipIcon.Warning;
				balloonInfo.Content = ResourceManagement.GetLocationString("OperateSucceedAfterServiceRestar");
				balloonInfo.Timeout = 10000;
				balloonInfo.Title = "EPG Service";
				NotifyFormManage.StartShowMessageUI(balloonInfo);
			}
		}

		private void btSend_Click(object sender, EventArgs e)
		{
			btApplySet_Click(sender, e);
			btSend.Enabled = false;
			RadioButton radioButton = rbtFile;
			RadioButton radioButton2 = rbtTCP;
			RadioButton radioButton3 = rbtTSPlayer;
			bool flag = (rbtUDP.Enabled = false);
			flag = (radioButton3.Enabled = flag);
			flag = (radioButton2.Enabled = flag);
			radioButton.Enabled = flag;
			TSend = new Thread(TSOutputManager.StartSendTransprotStream)
			{
				IsBackground = true
			};
			TSend.Start();
		}

		private void btStop_Click(object sender, EventArgs e)
		{
			new Thread(TSOutputManager.StopSendTransprotStream).Start();
			SoftDogDes.ReleaseDog();
			if (SoftDogDes.IniDog(4) != 0)
			{
				MessageBox.Show(SoftDogDes.GetExtendDataADV1().ToString());
			}
			else
			{
				MessageBox.Show("error");
			}
			SoftDogDes.ReleaseDog();
		}

		private void btTSPlayerTest_Click(object sender, EventArgs e)
		{
			MessageBox.Show(StreamPlayerManagement.Ins.OpenDevice().ToString());
			StreamPlayerManagement.Ins.CloseDevice();
		}

		private void btSetRate_Click(object sender, EventArgs e)
		{
			EPGSettings.Instatce.TSPlayerRate = Convert.ToInt32(txtTSRate.Text);
			EPGSettings.Instatce.Save();
			StreamPlayerManagement.Ins.SetRate(EPGSettings.Instatce.TSPlayerRate);
			BalloonInfo balloonInfo = new BalloonInfo();
			balloonInfo.BalloonType = ToolTipIcon.Info;
			balloonInfo.Content = ResourceManagement.GetLocationString("OperateSucceedNow");
			balloonInfo.Timeout = 10000;
			balloonInfo.Title = "EPG Service";
			NotifyFormManage.StartShowMessageUI(balloonInfo);
		}

		private void rbtSendType_CheckedChanged(object sender, EventArgs e)
		{
			pTSPlayerSetting.Enabled = rbtTSPlayer.Checked;
			pNetSettings.Enabled = rbtTCP.Checked || rbtUDP.Checked;
		}

		private void FormTest_FormClosing(object sender, FormClosingEventArgs e)
		{
			TSOutputManager.StopSendTransprotStream();
			if (TSend != null)
			{
				TSend.Abort();
			}
		}

		private void btStartSpeed_Click(object sender, EventArgs e)
		{
			if (TSpeed == null)
			{
				TSpeed = new Thread((ThreadStart)delegate
				{
					try
					{
						while (TSOutputManager.ServiceRunningState)
						{
							byte[] value = UdpClientManager.Recieve(IPAddress.Any, 9098);
							double num = BitConverter.ToDouble(value, 0);
							lRate.Text = num.ToString("N4", CultureInfo.CurrentCulture);
							Thread.Sleep(1000);
						}
					}
					catch (ThreadAbortException)
					{
					}
					catch (Exception ex2)
					{
						lRate.Text = "ERROR";
						MessageBox.Show(ex2.Message);
					}
				});
				TSpeed.Start();
			}
			else
			{
				MessageBox.Show("Running!");
			}
		}

		private void ckIsAddRate_CheckedChanged(object sender, EventArgs e)
		{
			txtRateAdd.Enabled = ckIsAddRate.Checked;
		}

		private void btnOta_Click(object sender, EventArgs e)
		{
			FormSender formSender = new FormSender();
			formSender.Owner = this;
			formSender.Show();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EPGServiceManagement.FormTester));
			this.groupBox3 = new System.Windows.Forms.GroupBox();
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
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.ckIsAddRate = new System.Windows.Forms.CheckBox();
			this.rbtTSPlayer = new System.Windows.Forms.RadioButton();
			this.rbtTCP = new System.Windows.Forms.RadioButton();
			this.pTSPlayerSetting = new System.Windows.Forms.FlowLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.txtTSRate = new System.Windows.Forms.TextBox();
			this.btSetRate = new System.Windows.Forms.Button();
			this.btTSPlayerTest = new System.Windows.Forms.Button();
			this.rbtFile = new System.Windows.Forms.RadioButton();
			this.rbtUDP = new System.Windows.Forms.RadioButton();
			this.pNetSettings = new System.Windows.Forms.Panel();
			this.cklAllIP = new System.Windows.Forms.CheckedListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.txtRateAdd = new System.Windows.Forms.TextBox();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.lRate = new System.Windows.Forms.Label();
			this.btStartSpeed = new System.Windows.Forms.Button();
			this.btnOta = new System.Windows.Forms.Button();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.txtPkgcnt = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.txtEventDays = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.btStop = new System.Windows.Forms.Button();
			this.btSend = new System.Windows.Forms.Button();
			this.btApplySet = new System.Windows.Forms.Button();
			this.pbStop = new System.Windows.Forms.ProgressBar();
			this.label6 = new System.Windows.Forms.Label();
			this.txtNullPkgPID = new System.Windows.Forms.TextBox();
			this.groupBox3.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.pTSPlayerSetting.SuspendLayout();
			this.pNetSettings.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			base.SuspendLayout();
			this.groupBox3.Controls.Add(this.tableLayoutPanel2);
			resources.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
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
			this.groupBox1.Controls.Add(this.groupBox2);
			this.groupBox1.Controls.Add(this.tableLayoutPanel3);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.groupBox2.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.ckIsAddRate, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.rbtTSPlayer, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.rbtTCP, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.pTSPlayerSetting, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.rbtFile, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.rbtUDP, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.pNetSettings, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel3, 1, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.ckIsAddRate, "ckIsAddRate");
			this.ckIsAddRate.Name = "ckIsAddRate";
			this.ckIsAddRate.UseVisualStyleBackColor = true;
			this.ckIsAddRate.CheckedChanged += new System.EventHandler(ckIsAddRate_CheckedChanged);
			resources.ApplyResources(this.rbtTSPlayer, "rbtTSPlayer");
			this.rbtTSPlayer.Name = "rbtTSPlayer";
			this.rbtTSPlayer.TabStop = true;
			this.rbtTSPlayer.UseVisualStyleBackColor = true;
			this.rbtTSPlayer.CheckedChanged += new System.EventHandler(rbtSendType_CheckedChanged);
			resources.ApplyResources(this.rbtTCP, "rbtTCP");
			this.rbtTCP.Name = "rbtTCP";
			this.rbtTCP.TabStop = true;
			this.rbtTCP.UseVisualStyleBackColor = true;
			this.rbtTCP.CheckedChanged += new System.EventHandler(rbtSendType_CheckedChanged);
			this.pTSPlayerSetting.Controls.Add(this.label2);
			this.pTSPlayerSetting.Controls.Add(this.txtTSRate);
			this.pTSPlayerSetting.Controls.Add(this.btSetRate);
			this.pTSPlayerSetting.Controls.Add(this.btTSPlayerTest);
			resources.ApplyResources(this.pTSPlayerSetting, "pTSPlayerSetting");
			this.pTSPlayerSetting.Name = "pTSPlayerSetting";
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			resources.ApplyResources(this.txtTSRate, "txtTSRate");
			this.txtTSRate.Name = "txtTSRate";
			resources.ApplyResources(this.btSetRate, "btSetRate");
			this.btSetRate.Name = "btSetRate";
			this.btSetRate.UseVisualStyleBackColor = true;
			this.btSetRate.Click += new System.EventHandler(btSetRate_Click);
			resources.ApplyResources(this.btTSPlayerTest, "btTSPlayerTest");
			this.btTSPlayerTest.Name = "btTSPlayerTest";
			this.btTSPlayerTest.UseVisualStyleBackColor = true;
			this.btTSPlayerTest.Click += new System.EventHandler(btTSPlayerTest_Click);
			resources.ApplyResources(this.rbtFile, "rbtFile");
			this.rbtFile.Name = "rbtFile";
			this.rbtFile.TabStop = true;
			this.rbtFile.UseVisualStyleBackColor = true;
			this.rbtFile.CheckedChanged += new System.EventHandler(rbtSendType_CheckedChanged);
			resources.ApplyResources(this.rbtUDP, "rbtUDP");
			this.rbtUDP.Checked = true;
			this.rbtUDP.Name = "rbtUDP";
			this.rbtUDP.TabStop = true;
			this.rbtUDP.UseVisualStyleBackColor = true;
			this.rbtUDP.CheckedChanged += new System.EventHandler(rbtSendType_CheckedChanged);
			this.pNetSettings.Controls.Add(this.cklAllIP);
			this.pNetSettings.Controls.Add(this.label1);
			resources.ApplyResources(this.pNetSettings, "pNetSettings");
			this.pNetSettings.Name = "pNetSettings";
			this.tableLayoutPanel1.SetRowSpan(this.pNetSettings, 2);
			resources.ApplyResources(this.cklAllIP, "cklAllIP");
			this.cklAllIP.FormattingEnabled = true;
			this.cklAllIP.Name = "cklAllIP";
			this.cklAllIP.Tag = "";
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.flowLayoutPanel1.Controls.Add(this.txtRateAdd);
			this.flowLayoutPanel1.Controls.Add(this.label6);
			this.flowLayoutPanel1.Controls.Add(this.txtNullPkgPID);
			resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			resources.ApplyResources(this.txtRateAdd, "txtRateAdd");
			this.txtRateAdd.Name = "txtRateAdd";
			this.flowLayoutPanel2.Controls.Add(this.lRate);
			this.flowLayoutPanel2.Controls.Add(this.btStartSpeed);
			this.flowLayoutPanel2.Controls.Add(this.btnOta);
			resources.ApplyResources(this.flowLayoutPanel2, "flowLayoutPanel2");
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			resources.ApplyResources(this.lRate, "lRate");
			this.lRate.Name = "lRate";
			resources.ApplyResources(this.btStartSpeed, "btStartSpeed");
			this.btStartSpeed.Name = "btStartSpeed";
			this.btStartSpeed.UseVisualStyleBackColor = true;
			this.btStartSpeed.Click += new System.EventHandler(btStartSpeed_Click);
			resources.ApplyResources(this.btnOta, "btnOta");
			this.btnOta.Name = "btnOta";
			this.btnOta.UseVisualStyleBackColor = true;
			this.btnOta.Click += new System.EventHandler(btnOta_Click);
			this.flowLayoutPanel3.Controls.Add(this.txtPkgcnt);
			this.flowLayoutPanel3.Controls.Add(this.label5);
			resources.ApplyResources(this.flowLayoutPanel3, "flowLayoutPanel3");
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			resources.ApplyResources(this.txtPkgcnt, "txtPkgcnt");
			this.txtPkgcnt.Name = "txtPkgcnt";
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
			this.tableLayoutPanel3.Controls.Add(this.txtEventDays, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.label4, 0, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			resources.ApplyResources(this.txtEventDays, "txtEventDays");
			this.txtEventDays.Name = "txtEventDays";
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			this.groupBox4.Controls.Add(this.tableLayoutPanel4);
			resources.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
			this.tableLayoutPanel4.Controls.Add(this.btStop, 3, 0);
			this.tableLayoutPanel4.Controls.Add(this.btSend, 2, 0);
			this.tableLayoutPanel4.Controls.Add(this.btApplySet, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.pbStop, 4, 0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			resources.ApplyResources(this.btStop, "btStop");
			this.btStop.Name = "btStop";
			this.btStop.UseVisualStyleBackColor = true;
			this.btStop.Click += new System.EventHandler(btStop_Click);
			resources.ApplyResources(this.btSend, "btSend");
			this.btSend.Name = "btSend";
			this.btSend.UseVisualStyleBackColor = true;
			this.btSend.Click += new System.EventHandler(btSend_Click);
			resources.ApplyResources(this.btApplySet, "btApplySet");
			this.btApplySet.Name = "btApplySet";
			this.btApplySet.UseVisualStyleBackColor = true;
			this.btApplySet.Click += new System.EventHandler(btApplySet_Click);
			resources.ApplyResources(this.pbStop, "pbStop");
			this.pbStop.Name = "pbStop";
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			resources.ApplyResources(this.txtNullPkgPID, "txtNullPkgPID");
			this.txtNullPkgPID.Name = "txtNullPkgPID";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.groupBox4);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.groupBox3);
			base.Name = "FormTester";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FormTest_FormClosing);
			base.Load += new System.EventHandler(FormTester_Load);
			this.groupBox3.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.pTSPlayerSetting.ResumeLayout(false);
			this.pTSPlayerSetting.PerformLayout();
			this.pNetSettings.ResumeLayout(false);
			this.pNetSettings.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
