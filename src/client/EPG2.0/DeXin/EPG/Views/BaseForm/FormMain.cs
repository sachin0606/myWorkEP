using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Com.Cn.Tool;
using DeXin.EPG.BLL;
using DeXin.EPG.BLL.ApplicationManagement;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Resource.ResourcesSpecialFiles;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.LogManagement;
using DeXin.EPG.Views.ProgramManagement;
using DeXin.EPG.Views.SendControlManagement;
using DeXin.EPG.Views.SendGenerator;
using DeXin.EPG.Views.SystemManagment;
using DeXin.EPG.Views.TableDescriptor;
using DeXin.EPG.Views.UserManagement;
using EPG.Views;
using EPG.Views.TableDescriptor;

namespace DeXin.EPG.Views.BaseForm
{
	public class FormMain : Form
	{
		private IContainer components = null;

		private MenuStrip topMenuStrip;

		private ToolStripMenuItem tsmSystem;

		private ToolStripMenuItem tsmEditPassword;

		private ToolStripMenuItem tsmLoginOrSwitch;

		private ToolStripMenuItem tsmExit;

		private ToolStripMenuItem tsmLanguage;

		private ToolStripMenuItem tsnChinese;

		private ToolStripMenuItem tsmEnglish;

		private ToolStripMenuItem tsmPermissionManagement;

		private ToolStripMenuItem tsmUserInfo;

		private ToolStripMenuItem tsmPermissionSetting;

		private ToolStripMenuItem tsmUserGroup;

		private ToolStripMenuItem tsmProgramManagement;

		private ToolStripMenuItem tsmDiscriptorManagement;

		private ToolStripMenuItem tsmDesriptorTypeManagement;

		private ToolStripMenuItem tsmDescriptorManagement;

		private ToolStripMenuItem tsmSendGenerate;

		private ToolStripMenuItem tsmSendControl;

		private ToolStripMenuItem tsmSendSpeed;

		private ToolStripMenuItem tsmTableSendORNot;

		private ToolStripMenuItem tsmPIDMapping;

		private ToolStripMenuItem tsmPMT_PIDMapping;

		private ToolStripMenuItem MultiIPSetting;

		private ToolStripMenuItem tsmSingleIPSetting;

		private ToolStripMenuItem tsmHelp;

		private ToolStripMenuItem tsmContent;

		private ToolStripMenuItem tsmAbout;

		private StatusStrip BottomStatusStrip;

		private ToolStripMenuItem tsmNitTsPrgManagement;

		private ToolStripMenuItem tsmBATManagement;

		private ToolStripMenuItem tsmEITManagement;

		private ToolStripMenuItem tsmMultimediaManage;

		private ToolStripMenuItem tsmLogManagement;

		private ToolStripStatusLabel tsStatusCurrentUser;

		private ToolStripStatusLabel tsStatusConncetedIP;

		private ToolStripStatusLabel tsStatusUserID;

		private ToolStripStatusLabel tsStatusIPAddress;

		private HelpProvider helpProvider1;

		private ToolStripMenuItem englishUSToolStripMenuItem;

		private System.Windows.Forms.Timer timerCheckDog;

		private ToolStripMenuItem tsmLCN;

		private ToolStripMenuItem tsbEitRateingSetting;

		private ToolStripMenuItem tsbSystemSetting;

		private ToolStripMenuItem tsmImportFromFile;

		private bool IsExit = false;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.BaseForm.FormMain));
			this.topMenuStrip = new System.Windows.Forms.MenuStrip();
			this.tsmSystem = new System.Windows.Forms.ToolStripMenuItem();
			this.tsbSystemSetting = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmEditPassword = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmLoginOrSwitch = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmExit = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmPermissionManagement = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmUserGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmUserInfo = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmPermissionSetting = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmLanguage = new System.Windows.Forms.ToolStripMenuItem();
			this.tsnChinese = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmEnglish = new System.Windows.Forms.ToolStripMenuItem();
			this.englishUSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmProgramManagement = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmNitTsPrgManagement = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmBATManagement = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmEITManagement = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmMultimediaManage = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmImportFromFile = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDiscriptorManagement = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDesriptorTypeManagement = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDescriptorManagement = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmLCN = new System.Windows.Forms.ToolStripMenuItem();
			this.tsbEitRateingSetting = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmSendGenerate = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmSendControl = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmSendSpeed = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmTableSendORNot = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmPIDMapping = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmPMT_PIDMapping = new System.Windows.Forms.ToolStripMenuItem();
			this.MultiIPSetting = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmSingleIPSetting = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmLogManagement = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmContent = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.BottomStatusStrip = new System.Windows.Forms.StatusStrip();
			this.tsStatusCurrentUser = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsStatusUserID = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsStatusConncetedIP = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsStatusIPAddress = new System.Windows.Forms.ToolStripStatusLabel();
			this.helpProvider1 = new System.Windows.Forms.HelpProvider();
			this.timerCheckDog = new System.Windows.Forms.Timer(this.components);
			this.topMenuStrip.SuspendLayout();
			this.BottomStatusStrip.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(this.topMenuStrip, "topMenuStrip");
			this.helpProvider1.SetHelpKeyword(this.topMenuStrip, resources.GetString("topMenuStrip.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.topMenuStrip, (System.Windows.Forms.HelpNavigator)resources.GetObject("topMenuStrip.HelpNavigator"));
			this.helpProvider1.SetHelpString(this.topMenuStrip, resources.GetString("topMenuStrip.HelpString"));
			this.topMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.tsmSystem, this.tsmPermissionManagement, this.tsmLanguage, this.tsmProgramManagement, this.tsmDiscriptorManagement, this.tsmSendGenerate, this.tsmSendControl, this.tsmLogManagement, this.tsmHelp });
			this.topMenuStrip.Name = "topMenuStrip";
			this.helpProvider1.SetShowHelp(this.topMenuStrip, (bool)resources.GetObject("topMenuStrip.ShowHelp"));
			resources.ApplyResources(this.tsmSystem, "tsmSystem");
			this.tsmSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.tsbSystemSetting, this.tsmEditPassword, this.tsmLoginOrSwitch, this.tsmExit });
			this.tsmSystem.Name = "tsmSystem";
			resources.ApplyResources(this.tsbSystemSetting, "tsbSystemSetting");
			this.tsbSystemSetting.Name = "tsbSystemSetting";
			this.tsbSystemSetting.Click += new System.EventHandler(tsbSystemSetting_Click);
			resources.ApplyResources(this.tsmEditPassword, "tsmEditPassword");
			this.tsmEditPassword.Name = "tsmEditPassword";
			this.tsmEditPassword.Click += new System.EventHandler(tsmEditPassword_Click);
			resources.ApplyResources(this.tsmLoginOrSwitch, "tsmLoginOrSwitch");
			this.tsmLoginOrSwitch.Name = "tsmLoginOrSwitch";
			this.tsmLoginOrSwitch.Click += new System.EventHandler(tsmLoginOrSwitch_Click);
			resources.ApplyResources(this.tsmExit, "tsmExit");
			this.tsmExit.Name = "tsmExit";
			this.tsmExit.Click += new System.EventHandler(tsmExit_Click);
			resources.ApplyResources(this.tsmPermissionManagement, "tsmPermissionManagement");
			this.tsmPermissionManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.tsmUserGroup, this.tsmUserInfo, this.tsmPermissionSetting });
			this.tsmPermissionManagement.Name = "tsmPermissionManagement";
			resources.ApplyResources(this.tsmUserGroup, "tsmUserGroup");
			this.tsmUserGroup.Name = "tsmUserGroup";
			this.tsmUserGroup.Click += new System.EventHandler(tsmUserGroup_Click);
			resources.ApplyResources(this.tsmUserInfo, "tsmUserInfo");
			this.tsmUserInfo.Name = "tsmUserInfo";
			this.tsmUserInfo.Click += new System.EventHandler(tsmUserInfo_Click);
			resources.ApplyResources(this.tsmPermissionSetting, "tsmPermissionSetting");
			this.tsmPermissionSetting.Name = "tsmPermissionSetting";
			this.tsmPermissionSetting.Click += new System.EventHandler(tsmPermissionSetting_Click);
			resources.ApplyResources(this.tsmLanguage, "tsmLanguage");
			this.tsmLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.tsnChinese, this.tsmEnglish, this.englishUSToolStripMenuItem });
			this.tsmLanguage.Name = "tsmLanguage";
			resources.ApplyResources(this.tsnChinese, "tsnChinese");
			this.tsnChinese.Name = "tsnChinese";
			this.tsnChinese.Click += new System.EventHandler(tsnChinese_Click);
			resources.ApplyResources(this.tsmEnglish, "tsmEnglish");
			this.tsmEnglish.Name = "tsmEnglish";
			this.tsmEnglish.Click += new System.EventHandler(tsmEnglish_Click);
			resources.ApplyResources(this.englishUSToolStripMenuItem, "englishUSToolStripMenuItem");
			this.englishUSToolStripMenuItem.Name = "englishUSToolStripMenuItem";
			this.englishUSToolStripMenuItem.Click += new System.EventHandler(englishUSToolStripMenuItem_Click);
			resources.ApplyResources(this.tsmProgramManagement, "tsmProgramManagement");
			this.tsmProgramManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.tsmNitTsPrgManagement, this.tsmBATManagement, this.tsmEITManagement, this.tsmMultimediaManage, this.tsmImportFromFile });
			this.tsmProgramManagement.Name = "tsmProgramManagement";
			resources.ApplyResources(this.tsmNitTsPrgManagement, "tsmNitTsPrgManagement");
			this.tsmNitTsPrgManagement.Name = "tsmNitTsPrgManagement";
			this.tsmNitTsPrgManagement.Click += new System.EventHandler(tsmNitTsPrgManagement_Click);
			resources.ApplyResources(this.tsmBATManagement, "tsmBATManagement");
			this.tsmBATManagement.Name = "tsmBATManagement";
			this.tsmBATManagement.Click += new System.EventHandler(tsmBATManagement_Click);
			resources.ApplyResources(this.tsmEITManagement, "tsmEITManagement");
			this.tsmEITManagement.Name = "tsmEITManagement";
			this.tsmEITManagement.Click += new System.EventHandler(tsmEITManagement_Click);
			resources.ApplyResources(this.tsmMultimediaManage, "tsmMultimediaManage");
			this.tsmMultimediaManage.Name = "tsmMultimediaManage";
			this.tsmMultimediaManage.Click += new System.EventHandler(tsmMultimediaManage_Click);
			resources.ApplyResources(this.tsmImportFromFile, "tsmImportFromFile");
			this.tsmImportFromFile.Name = "tsmImportFromFile";
			this.tsmImportFromFile.Click += new System.EventHandler(tsmImportFromFile_Click);
			resources.ApplyResources(this.tsmDiscriptorManagement, "tsmDiscriptorManagement");
			this.tsmDiscriptorManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.tsmDesriptorTypeManagement, this.tsmDescriptorManagement, this.tsmLCN, this.tsbEitRateingSetting });
			this.tsmDiscriptorManagement.Name = "tsmDiscriptorManagement";
			resources.ApplyResources(this.tsmDesriptorTypeManagement, "tsmDesriptorTypeManagement");
			this.tsmDesriptorTypeManagement.Name = "tsmDesriptorTypeManagement";
			this.tsmDesriptorTypeManagement.Click += new System.EventHandler(tsmDesriptorTypeManagement_Click);
			resources.ApplyResources(this.tsmDescriptorManagement, "tsmDescriptorManagement");
			this.tsmDescriptorManagement.Name = "tsmDescriptorManagement";
			this.tsmDescriptorManagement.Click += new System.EventHandler(tsmDescriptorManagement_Click);
			resources.ApplyResources(this.tsmLCN, "tsmLCN");
			this.tsmLCN.Name = "tsmLCN";
			this.tsmLCN.Click += new System.EventHandler(tsmLCN_Click);
			resources.ApplyResources(this.tsbEitRateingSetting, "tsbEitRateingSetting");
			this.tsbEitRateingSetting.Name = "tsbEitRateingSetting";
			this.tsbEitRateingSetting.Click += new System.EventHandler(tsbEitRateingSetting_Click);
			resources.ApplyResources(this.tsmSendGenerate, "tsmSendGenerate");
			this.tsmSendGenerate.Name = "tsmSendGenerate";
			this.tsmSendGenerate.Click += new System.EventHandler(tsmSendGenerate_Click);
			resources.ApplyResources(this.tsmSendControl, "tsmSendControl");
			this.tsmSendControl.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.tsmSendSpeed, this.tsmTableSendORNot, this.tsmPIDMapping, this.tsmPMT_PIDMapping, this.MultiIPSetting, this.tsmSingleIPSetting });
			this.tsmSendControl.Name = "tsmSendControl";
			resources.ApplyResources(this.tsmSendSpeed, "tsmSendSpeed");
			this.tsmSendSpeed.Name = "tsmSendSpeed";
			this.tsmSendSpeed.Click += new System.EventHandler(tsmSendSpeed_Click);
			resources.ApplyResources(this.tsmTableSendORNot, "tsmTableSendORNot");
			this.tsmTableSendORNot.Name = "tsmTableSendORNot";
			this.tsmTableSendORNot.Click += new System.EventHandler(tsmTableSendORNot_Click);
			resources.ApplyResources(this.tsmPIDMapping, "tsmPIDMapping");
			this.tsmPIDMapping.Name = "tsmPIDMapping";
			this.tsmPIDMapping.Click += new System.EventHandler(tsmPIDMapping_Click);
			resources.ApplyResources(this.tsmPMT_PIDMapping, "tsmPMT_PIDMapping");
			this.tsmPMT_PIDMapping.Name = "tsmPMT_PIDMapping";
			this.tsmPMT_PIDMapping.Click += new System.EventHandler(tsmPMT_PIDMapping_Click);
			resources.ApplyResources(this.MultiIPSetting, "MultiIPSetting");
			this.MultiIPSetting.Name = "MultiIPSetting";
			this.MultiIPSetting.Click += new System.EventHandler(MultiIPSetting_Click);
			resources.ApplyResources(this.tsmSingleIPSetting, "tsmSingleIPSetting");
			this.tsmSingleIPSetting.Name = "tsmSingleIPSetting";
			this.tsmSingleIPSetting.Click += new System.EventHandler(tsmSingleIPSetting_Click);
			resources.ApplyResources(this.tsmLogManagement, "tsmLogManagement");
			this.tsmLogManagement.Name = "tsmLogManagement";
			this.tsmLogManagement.Click += new System.EventHandler(tsmLogManagement_Click);
			resources.ApplyResources(this.tsmHelp, "tsmHelp");
			this.tsmHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmContent, this.tsmAbout });
			this.tsmHelp.Name = "tsmHelp";
			resources.ApplyResources(this.tsmContent, "tsmContent");
			this.tsmContent.Name = "tsmContent";
			this.tsmContent.Click += new System.EventHandler(tsmContent_Click);
			resources.ApplyResources(this.tsmAbout, "tsmAbout");
			this.tsmAbout.Name = "tsmAbout";
			this.tsmAbout.Click += new System.EventHandler(tsmAbout_Click);
			resources.ApplyResources(this.BottomStatusStrip, "BottomStatusStrip");
			this.helpProvider1.SetHelpKeyword(this.BottomStatusStrip, resources.GetString("BottomStatusStrip.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.BottomStatusStrip, (System.Windows.Forms.HelpNavigator)resources.GetObject("BottomStatusStrip.HelpNavigator"));
			this.helpProvider1.SetHelpString(this.BottomStatusStrip, resources.GetString("BottomStatusStrip.HelpString"));
			this.BottomStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.tsStatusCurrentUser, this.tsStatusUserID, this.tsStatusConncetedIP, this.tsStatusIPAddress });
			this.BottomStatusStrip.Name = "BottomStatusStrip";
			this.helpProvider1.SetShowHelp(this.BottomStatusStrip, (bool)resources.GetObject("BottomStatusStrip.ShowHelp"));
			resources.ApplyResources(this.tsStatusCurrentUser, "tsStatusCurrentUser");
			this.tsStatusCurrentUser.Name = "tsStatusCurrentUser";
			resources.ApplyResources(this.tsStatusUserID, "tsStatusUserID");
			this.tsStatusUserID.Name = "tsStatusUserID";
			resources.ApplyResources(this.tsStatusConncetedIP, "tsStatusConncetedIP");
			this.tsStatusConncetedIP.Name = "tsStatusConncetedIP";
			resources.ApplyResources(this.tsStatusIPAddress, "tsStatusIPAddress");
			this.tsStatusIPAddress.Name = "tsStatusIPAddress";
			resources.ApplyResources(this.helpProvider1, "helpProvider1");
			this.timerCheckDog.Tick += new System.EventHandler(timerCheckDog_Tick);
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.BottomStatusStrip);
			base.Controls.Add(this.topMenuStrip);
			this.helpProvider1.SetHelpKeyword(this, resources.GetString("$this.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this, (System.Windows.Forms.HelpNavigator)resources.GetObject("$this.HelpNavigator"));
			this.helpProvider1.SetHelpString(this, resources.GetString("$this.HelpString"));
			base.IsMdiContainer = true;
			base.MainMenuStrip = this.topMenuStrip;
			base.Name = "FormMain";
			this.helpProvider1.SetShowHelp(this, (bool)resources.GetObject("$this.ShowHelp"));
			base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			base.Load += new System.EventHandler(FormMain_Load);
			base.Shown += new System.EventHandler(FormMain_Shown);
			this.topMenuStrip.ResumeLayout(false);
			this.topMenuStrip.PerformLayout();
			this.BottomStatusStrip.ResumeLayout(false);
			this.BottomStatusStrip.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormMain()
		{
			InitializeComponent();
			base.FormClosing += FormMain_FormClosing;
			Text = ResourceSpecial.EditionEPG;
		}

		private void tsmAbout_Click(object sender, EventArgs e)
		{
			FormAboutBox formAboutBox = new FormAboutBox();
			formAboutBox.ShowDialog();
		}

		private void tsmLoginOrSwitch_Click(object sender, EventArgs e)
		{
			Login();
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			string dBConnectionString = EPGSettings.Instatce.DBConnectionString;
			bool flag = true;
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (IsExit || Tool.ShowMessage(ResourceManagement.GetLocationString("ComfirmExit"), MessageBoxButtons.OKCancel))
				{
					LogBLL logBLL = new LogBLL();
					if (LoginUser.Ins.UserIns != null)
					{
						logBLL.AddLog(EnumOSName.Logout, ResourceManagement.GetLocationString("LoginOutSuccessfully"));
					}
				}
				else
				{
					e.Cancel = true;
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void FormMain_Shown(object sender, EventArgs e)
		{
			Login();
			CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
			if (currentUICulture.Equals(new CultureInfo("zh-CN")))
			{
				CheckLanguage(tsnChinese);
			}
			else if (currentUICulture.Equals(new CultureInfo("hi-IN")))
			{
				CheckLanguage(tsmEnglish);
			}
			else if (currentUICulture.Equals(new CultureInfo("en-US")))
			{
				CheckLanguage(englishUSToolStripMenuItem);
			}
		}

		private void Login()
		{
			IsExit = false;
			try
			{
				FormLogin formLogin = new FormLogin();
				formLogin.StartPosition = FormStartPosition.CenterParent;
				if (formLogin.ShowDialog() == DialogResult.OK)
				{
					if (base.MdiChildren.Length == 1)
					{
						Form form = base.MdiChildren.First();
						FormBase fm = form.GetType().Assembly.CreateInstance(form.GetType().FullName, true) as FormBase;
						form.Close();
						ShowMdiForm(fm);
					}
					tsStatusUserID.Text = LoginUser.Ins.UserIns.UserID;
					SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(EPGSettings.Instatce.DBConnectionString);
					tsStatusIPAddress.Text = sqlConnectionStringBuilder.DataSource.ToString();
					SystemSetting systemSetting = new SystemSettingBLL().GetSystemSetting(SystemSettingCode.EPGEncoding);
					if (systemSetting == null)
					{
						CommTool.SetEncoder("DEFAULT");
					}
					else
					{
						CommTool.SetEncoder(systemSetting.Value);
					}
				}
				else if (LoginUser.Ins.UserIns == null)
				{
					IsExit = true;
					Application.Exit();
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void ShowMdiForm(FormBase fm)
		{
			if (base.MdiChildren.Length == 1)
			{
				base.MdiChildren.First().Close();
			}
			if (fm.FunctionName == EnumFunctionName.SysFuncAllFunctions || fm.permitGroup.CanQuery)
			{
				fm.ControlBox = false;
				fm.WindowState = FormWindowState.Maximized;
				fm.MdiParent = this;
				fm.Show();
			}
			else
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("NoPermit"));
			}
		}

		private void tsmUserInfo_Click(object sender, EventArgs e)
		{
			FormUserInofs fm = new FormUserInofs();
			ShowMdiForm(fm);
		}

		private void tsmNitTsPrgManagement_Click(object sender, EventArgs e)
		{
			FormProgramsInfo fm = new FormProgramsInfo();
			ShowMdiForm(fm);
		}

		private void tsmBATManagement_Click(object sender, EventArgs e)
		{
			FormBATManage fm = new FormBATManage();
			ShowMdiForm(fm);
		}

		private void tsmEITManagement_Click(object sender, EventArgs e)
		{
			FormEITManage fm = new FormEITManage();
			ShowMdiForm(fm);
		}

		private void tsmMultimediaManage_Click(object sender, EventArgs e)
		{
			FormMultimediaManage fm = new FormMultimediaManage();
			ShowMdiForm(fm);
		}

		private void tsmImportFromFile_Click(object sender, EventArgs e)
		{
			if (new FormImportTSFile().ShowDialog() == DialogResult.OK)
			{
				tsmNitTsPrgManagement_Click(sender, e);
			}
		}

		private void tsmDesriptorTypeManagement_Click(object sender, EventArgs e)
		{
			FormDescriptorType fm = new FormDescriptorType();
			ShowMdiForm(fm);
		}

		private void tsmUserGroup_Click(object sender, EventArgs e)
		{
			FormUserGroup fm = new FormUserGroup();
			ShowMdiForm(fm);
		}

		private void tsmPermissionSetting_Click(object sender, EventArgs e)
		{
			FormPermitGroupManagement fm = new FormPermitGroupManagement();
			ShowMdiForm(fm);
		}

		private void tsmEditPassword_Click(object sender, EventArgs e)
		{
			if (LoginUser.Ins.UserIns == null)
			{
				Tool.ShowMessage("LoginFirst");
				return;
			}
			FormEditPWD formEditPWD = new FormEditPWD();
			formEditPWD.ShowDialog();
		}

		private void tsmSendSpeed_Click(object sender, EventArgs e)
		{
			FormSendSpeed fm = new FormSendSpeed();
			ShowMdiForm(fm);
		}

		private void tsmTableSendORNot_Click(object sender, EventArgs e)
		{
			FormSendOrNot fm = new FormSendOrNot();
			ShowMdiForm(fm);
		}

		private void tsmPIDMapping_Click(object sender, EventArgs e)
		{
			FormReMappingPID fm = new FormReMappingPID();
			ShowMdiForm(fm);
		}

		private void tsmDBBackup_Click(object sender, EventArgs e)
		{
			FormDBBackup fm = new FormDBBackup();
			ShowMdiForm(fm);
		}

		private void tsmDBRestore_Click(object sender, EventArgs e)
		{
			FormDBRestore fm = new FormDBRestore();
			ShowMdiForm(fm);
		}

		private void tsnChinese_Click(object sender, EventArgs e)
		{
			try
			{
				if (base.MdiChildren.Length == 1)
				{
					base.MdiChildren.First().Close();
				}
				CheckLanguage(tsnChinese);
				LanguageSetting("zh-CN", "zh-CN");
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void tsmEnglish_Click(object sender, EventArgs e)
		{
			try
			{
				if (base.MdiChildren.Length == 1)
				{
					base.MdiChildren.First().Close();
				}
				CheckLanguage(tsmEnglish);
				LanguageSetting("en", "hi-IN");
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void englishUSToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (base.MdiChildren.Length == 1)
				{
					base.MdiChildren.First().Close();
				}
				CheckLanguage(englishUSToolStripMenuItem);
				LanguageSetting("en", "en-US");
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void CheckLanguage(ToolStripMenuItem tsmi)
		{
			IList<ToolStripMenuItem> list = new List<ToolStripMenuItem>();
			list.Add(englishUSToolStripMenuItem);
			list.Add(tsmEnglish);
			list.Add(tsnChinese);
			tsmi.Checked = true;
			if (!tsmi.Checked)
			{
				return;
			}
			list.Remove(tsmi);
			foreach (ToolStripMenuItem item in list)
			{
				item.Checked = false;
			}
		}

		private void LanguageSetting(string LanguageUI, string Language)
		{
			EPGConfigurationManger.Instance.LanguageUI = LanguageUI;
			EPGConfigurationManger.Instance.Language = Language;
			EPGConfigurationManger.Instance.conManger.Save();
			ConfigurationManager.RefreshSection("appSettings");
			ConfigurationManager.RefreshSection("connectionStrings");
			ThreadInfo.Ins.SetInfo();
			LocationControl.Ins.Init();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormMain));
			componentResourceManager.ApplyResources(tsmSystem, "tsmSystem");
			componentResourceManager.ApplyResources(tsmEditPassword, "tsmEditPassword");
			componentResourceManager.ApplyResources(tsmLoginOrSwitch, "tsmLoginOrSwitch");
			componentResourceManager.ApplyResources(tsmExit, "tsmExit");
			componentResourceManager.ApplyResources(tsmPermissionManagement, "tsmPermissionManagement");
			componentResourceManager.ApplyResources(tsmUserGroup, "tsmUserGroup");
			componentResourceManager.ApplyResources(tsmUserInfo, "tsmUserInfo");
			componentResourceManager.ApplyResources(tsmPermissionSetting, "tsmPermissionSetting");
			componentResourceManager.ApplyResources(tsmLanguage, "tsmLanguage");
			componentResourceManager.ApplyResources(tsnChinese, "tsnChinese");
			componentResourceManager.ApplyResources(tsmEnglish, "tsmEnglish");
			componentResourceManager.ApplyResources(tsmProgramManagement, "tsmProgramManagement");
			componentResourceManager.ApplyResources(tsmNitTsPrgManagement, "tsmNitTsPrgManagement");
			componentResourceManager.ApplyResources(tsmBATManagement, "tsmBATManagement");
			componentResourceManager.ApplyResources(tsmEITManagement, "tsmEITManagement");
			componentResourceManager.ApplyResources(tsmMultimediaManage, "tsmMultimediaManage");
			componentResourceManager.ApplyResources(tsmDiscriptorManagement, "tsmDiscriptorManagement");
			componentResourceManager.ApplyResources(tsmDesriptorTypeManagement, "tsmDesriptorTypeManagement");
			componentResourceManager.ApplyResources(tsmDescriptorManagement, "tsmDescriptorManagement");
			componentResourceManager.ApplyResources(tsmSendGenerate, "tsmSendGenerate");
			componentResourceManager.ApplyResources(tsmSendControl, "tsmSendControl");
			componentResourceManager.ApplyResources(tsmSendSpeed, "tsmSendSpeed");
			componentResourceManager.ApplyResources(tsmTableSendORNot, "tsmTableSendORNot");
			componentResourceManager.ApplyResources(tsmPIDMapping, "tsmPIDMapping");
			componentResourceManager.ApplyResources(tsmPMT_PIDMapping, "tsmPMT_PIDMapping");
			componentResourceManager.ApplyResources(MultiIPSetting, "MultiIPSetting");
			componentResourceManager.ApplyResources(tsmSingleIPSetting, "tsmSingleIPSetting");
			componentResourceManager.ApplyResources(tsmHelp, "tsmHelp");
			componentResourceManager.ApplyResources(tsmContent, "tsmContent");
			componentResourceManager.ApplyResources(tsmAbout, "tsmAbout");
			componentResourceManager.ApplyResources(tsmLogManagement, "tsmLogManagement");
			componentResourceManager.ApplyResources(BottomStatusStrip, "BottomStatusStrip");
			componentResourceManager.ApplyResources(tsStatusCurrentUser, "tsStatusCurrentUser");
			componentResourceManager.ApplyResources(tsStatusConncetedIP, "tsStatusConncetedIP");
			componentResourceManager.ApplyResources(tsbEitRateingSetting, "tsbEitRateingSetting");
			componentResourceManager.ApplyResources(tsbSystemSetting, "tsbSystemSetting");
			componentResourceManager.ApplyResources(tsmImportFromFile, "tsmImportFromFile");
			Text = ResourceManagement.GetLocationString("MainFormText");
		}

		private void tsmPMT_PIDMapping_Click(object sender, EventArgs e)
		{
			FormPMTPIDReMapping fm = new FormPMTPIDReMapping();
			ShowMdiForm(fm);
		}

		private void MultiIPSetting_Click(object sender, EventArgs e)
		{
			FormMultiIPOutput fm = new FormMultiIPOutput();
			ShowMdiForm(fm);
		}

		private void tsmSingleIPSetting_Click(object sender, EventArgs e)
		{
			FormSingleIPOutput fm = new FormSingleIPOutput();
			ShowMdiForm(fm);
		}

		private void tsmDescriptorManagement_Click(object sender, EventArgs e)
		{
			try
			{
				FormDescriptors fm = new FormDescriptors();
				ShowMdiForm(fm);
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void tsmSendGenerate_Click(object sender, EventArgs e)
		{
			FormTSDataGenerator fm = new FormTSDataGenerator();
			ShowMdiForm(fm);
		}

		private void tsmExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void tsmLogManagement_Click(object sender, EventArgs e)
		{
			FormLogManagement1 fm = new FormLogManagement1();
			ShowMdiForm(fm);
		}

		private void CheckHTBDog()
		{
		}

		private void timerCheckDog_Tick(object sender, EventArgs e)
		{
		}

		private void tsmContent_Click(object sender, EventArgs e)
		{
			helpProvider1.HelpNamespace = CommTool.GetHelpPath();
			Help.ShowHelp(this, helpProvider1.HelpNamespace);
		}

		private void tsmLCN_Click(object sender, EventArgs e)
		{
			FormLCNType fm = new FormLCNType();
			ShowMdiForm(fm);
		}

		private void tsbEitRateingSetting_Click(object sender, EventArgs e)
		{
			FormEitRatingSetting fm = new FormEitRatingSetting();
			ShowMdiForm(fm);
		}

		private void tsbSystemSetting_Click(object sender, EventArgs e)
		{
			FormSystemSetting fm = new FormSystemSetting();
			ShowMdiForm(fm);
		}
	}
}
