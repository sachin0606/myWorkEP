using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Net;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using CryptManage;
using DeXin.EPG.BLL;
using DeXin.EPG.BLL.ApplicationManagement;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Resource.ResourcesSpecialFiles;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views
{
	public class FormLogin : FormBase
	{
		private IContainer components = null;

		private Button btnSubmit;

		private Button btnCancel;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblName;

		private Label lblPassword;

		private TextBox txtName;

		private TextBox txtPassword;

		private Panel panel1;

		private Button btSettings;

		private GroupBox gbxSettings;

		private TableLayoutPanel tableLayoutPanel2;

		private Label lblIPSMS;

		private TextBox txtIP;

		private TextBox txtpwd;

		private Label lblDatabaseName;

		private Label lblPWD;

		private TextBox txtDataBase;

		private TextBox txtUid;

		private Label lblUserID;

		private UserInfoManager pageManager = new UserInfoManager();

		private SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.FormLogin));
			this.btnSubmit = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblName = new System.Windows.Forms.Label();
			this.lblPassword = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btSettings = new System.Windows.Forms.Button();
			this.gbxSettings = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.lblIPSMS = new System.Windows.Forms.Label();
			this.txtIP = new System.Windows.Forms.TextBox();
			this.txtpwd = new System.Windows.Forms.TextBox();
			this.lblDatabaseName = new System.Windows.Forms.Label();
			this.lblPWD = new System.Windows.Forms.Label();
			this.txtDataBase = new System.Windows.Forms.TextBox();
			this.txtUid = new System.Windows.Forms.TextBox();
			this.lblUserID = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.gbxSettings.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(this.btnSubmit, "btnSubmit");
			this.btnSubmit.Name = "btnSubmit";
			this.btnSubmit.UseVisualStyleBackColor = true;
			this.btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblName, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblPassword, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtName, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtPassword, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.gbxSettings, 0, 5);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.lblName, "lblName");
			this.lblName.Name = "lblName";
			resources.ApplyResources(this.lblPassword, "lblPassword");
			this.lblPassword.Name = "lblPassword";
			resources.ApplyResources(this.txtName, "txtName");
			this.txtName.Name = "txtName";
			resources.ApplyResources(this.txtPassword, "txtPassword");
			this.txtPassword.Name = "txtPassword";
			resources.ApplyResources(this.panel1, "panel1");
			this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
			this.panel1.Controls.Add(this.btSettings);
			this.panel1.Controls.Add(this.btnSubmit);
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Name = "panel1";
			resources.ApplyResources(this.btSettings, "btSettings");
			this.btSettings.Name = "btSettings";
			this.btSettings.UseVisualStyleBackColor = true;
			this.btSettings.Click += new System.EventHandler(btSettings_Click);
			resources.ApplyResources(this.gbxSettings, "gbxSettings");
			this.tableLayoutPanel1.SetColumnSpan(this.gbxSettings, 2);
			this.gbxSettings.Controls.Add(this.tableLayoutPanel2);
			this.gbxSettings.Name = "gbxSettings";
			this.gbxSettings.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.lblIPSMS, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.txtIP, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.txtpwd, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.lblDatabaseName, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.lblPWD, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.txtDataBase, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.txtUid, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.lblUserID, 0, 2);
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
			base.AcceptButton = this.btnSubmit;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.tableLayoutPanel1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FormLogin";
			base.ShowInTaskbar = false;
			base.Load += new System.EventHandler(FormLogin_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.gbxSettings.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			base.ResumeLayout(false);
		}

		public FormLogin()
		{
			InitializeComponent();
		}

		private void FormLogin_Load(object sender, EventArgs e)
		{
			btSettings_Click(null, null);
			builder.ConnectionString = EPGSettings.Instatce.DBConnectionString;
			txtIP.Text = builder.DataSource;
			txtDataBase.Text = builder.InitialCatalog;
			txtUid.Text = builder.UserID;
			txtpwd.Text = builder.Password;
		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			string message = "";
			string text = txtIP.Text;
			try
			{
				IPAddress.Parse(text);
			}
			catch (Exception ex)
			{
				Tool.DoNoneException(ex);
				throw new CustomException(lblIPSMS.Text.Trim() + LocationControl.Ins.LocationItems[LacationEnum.InputFormatError], MessageType.SHOW);
			}
			builder.InitialCatalog = txtDataBase.Text;
			builder.DataSource = text;
			builder.UserID = txtUid.Text;
			builder.Password = txtpwd.Text;
			EPGSettings.Instatce.DBConnectionString = builder.ConnectionString;
			EPGSettings.Instatce.Save();
			SystemSettingBLL systemSettingBLL = new SystemSettingBLL();
			SystemSetting systemSetting = systemSettingBLL.GetSystemSetting(SystemSettingCode.DBVERSION);
			string s = ((systemSetting.Value == null) ? "-1" : systemSetting.Value);
			if (systemSetting == null)
			{
				Tool.ShowMessage(ResourceSpecial.UpgradeDBFirst);
				return;
			}
			if (decimal.Parse(CommTool.DBVersion) > decimal.Parse(s))
			{
				Tool.ShowMessage(ResourceSpecial.UpgradeDBFirst);
				return;
			}
			if (decimal.Parse(CommTool.DBVersion) < decimal.Parse(s))
			{
				Tool.ShowMessage(ResourceSpecial.UpgradeEPGFirst);
				return;
			}
			string value = Tool.GetValue(txtName);
			string password = CryptionManage.Instance.EnCryptMD5(Tool.GetValue(txtPassword));
			UserInfo userInfo = new UserInfo();
			userInfo.UserID = value;
			userInfo.Password = password;
			userInfo = pageManager.CheckLogin(userInfo, out message);
			if (userInfo == null)
			{
				Tool.ShowMessage(message);
				return;
			}
			LoginUser.Ins.UserIns = userInfo;
			base.DialogResult = DialogResult.OK;
			LogBLL logBLL = new LogBLL();
			logBLL.AddLog(EnumOSName.Login, ResourceManagement.GetLocationString("LoginSuccessfully"));
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
		}

		private void btSettings_Click(object sender, EventArgs e)
		{
			if (gbxSettings.Visible)
			{
				gbxSettings.Hide();
				base.Height -= gbxSettings.Height;
				btSettings.Text = ResourceManagement.GetLocationString("Setting") + ">>";
			}
			else
			{
				base.Height += gbxSettings.Height;
				gbxSettings.Show();
				btSettings.Text = ResourceManagement.GetLocationString("Setting") + "<<";
			}
		}
	}
}
