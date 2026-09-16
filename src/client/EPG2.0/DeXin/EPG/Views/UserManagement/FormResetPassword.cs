using System;
using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using CryptManage;
using DeXin.EPG.BLL;
using DeXin.EPG.BLL.ApplicationManagement;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.UserManagement
{
	public class FormResetPassword : FormDetailBase
	{
		private IContainer components = null;

		private TextBox txtConfirmPwd;

		private TextBox txtUserID;

		private Label lblConfirmPWD;

		private Label lblName;

		private TextBox txtUserName;

		private Label lblPassword;

		private TextBox txtPassword;

		private Label lblUserID;

		private TableLayoutPanel tableLayoutPanel1;

		private UserInfo user = null;

		private UserInfoManager userManger = new UserInfoManager();

		private LogBLL logBll = new LogBLL();

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.UserManagement.FormResetPassword));
			this.txtConfirmPwd = new System.Windows.Forms.TextBox();
			this.txtUserID = new System.Windows.Forms.TextBox();
			this.lblConfirmPWD = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.lblUserID = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.txtConfirmPwd, "txtConfirmPwd");
			this.txtConfirmPwd.Name = "txtConfirmPwd";
			resources.ApplyResources(this.txtUserID, "txtUserID");
			this.txtUserID.Name = "txtUserID";
			resources.ApplyResources(this.lblConfirmPWD, "lblConfirmPWD");
			this.lblConfirmPWD.Name = "lblConfirmPWD";
			resources.ApplyResources(this.lblName, "lblName");
			this.lblName.Name = "lblName";
			resources.ApplyResources(this.txtUserName, "txtUserName");
			this.txtUserName.Name = "txtUserName";
			resources.ApplyResources(this.lblPassword, "lblPassword");
			this.lblPassword.Name = "lblPassword";
			resources.ApplyResources(this.txtPassword, "txtPassword");
			this.txtPassword.Name = "txtPassword";
			resources.ApplyResources(this.lblUserID, "lblUserID");
			this.lblUserID.Name = "lblUserID";
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblUserID, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtConfirmPwd, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtUserID, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblConfirmPWD, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblName, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtPassword, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblPassword, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtUserName, 1, 1);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormResetPassword";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormResetPassword()
		{
			InitializeComponent();
		}

		public FormResetPassword(PageEnum pageFlag)
		{
			InitializeComponent();
			base.PageFlag = pageFlag;
		}

		public FormResetPassword(string userID, PageEnum pageFlag)
		{
			InitializeComponent();
			try
			{
				user = userManger.GetUserByID(userID);
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
			base.PageFlag = ((user == null) ? PageEnum.Add : pageFlag);
		}

		protected override void FormLoad()
		{
			_FunctionName = EnumFunctionName.SysFuncResetPassword;
			BindUserData(user);
		}

		protected override bool FunSubmit()
		{
			if (LoginUser.Ins.UserIns.UserID.ToLower() != "admin")
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("AdminCanResetPassword"));
				return false;
			}
			if (string.IsNullOrEmpty(txtUserID.Text.Trim()))
			{
				Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.IsNotNull]);
				return false;
			}
			if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
			{
				Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.IsNotNull]);
				return false;
			}
			if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
			{
				Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.IsNotNull]);
				return false;
			}
			if (string.IsNullOrEmpty(txtConfirmPwd.Text.Trim()))
			{
				Tool.ShowMessage(CommTool.ShowInputFormatError(lblConfirmPWD));
				return false;
			}
			if (txtPassword.Text.Trim() != txtConfirmPwd.Text.Trim())
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("Inconsistent"));
				return false;
			}
			BindtoObject();
			msg = "";
			userManger.UndateUserInfo(user, out msg);
			Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.OperateSucceed]);
			base.DialogResult = DialogResult.OK;
			return true;
		}

		private void BindUserData(UserInfo tempUser)
		{
			txtUserID.Text = ((tempUser == null) ? "" : tempUser.UserID);
			txtUserName.Text = ((tempUser == null) ? "" : tempUser.UserName);
		}

		private void BindtoObject()
		{
			user.UserID = txtUserID.Text.Trim();
			user.UserName = txtUserName.Text.Trim();
			user.Password = CryptionManage.Instance.EnCryptMD5(txtPassword.Text.Trim());
		}
	}
}
