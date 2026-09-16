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

namespace DeXin.EPG.Views.SystemManagment
{
	public class FormEditPWD : FormDetailBase
	{
		private UserInfoManager pageManger = new UserInfoManager();

		private CryptManageBase cryptManager = new CryptManageBase();

		private LogBLL logBll = new LogBLL();

		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblUserID;

		private TextBox txtPassword3;

		private TextBox txtUserID;

		private TextBox txtPassword2;

		private Label lblConfirmNewPWD;

		private TextBox txtPassword;

		private Label lblPassword;

		private Label lblNewPWD;

		public FormEditPWD()
		{
			InitializeComponent();
			base.PageFlag = PageEnum.Add;
		}

		protected override void FormLoad()
		{
			txtUserID.Enabled = false;
			txtUserID.Text = LoginUser.Ins.UserIns.UserID;
		}

		protected void SetFormStyle()
		{
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.WindowState = FormWindowState.Normal;
			base.FormBorderStyle = FormBorderStyle.Fixed3D;
			base.StartPosition = FormStartPosition.CenterParent;
		}

		protected override bool FunSubmit()
		{
			bool result = false;
			string message = "";
			if (Ckeck(out message))
			{
				string value = Tool.GetValue(txtUserID);
				string value2 = Tool.GetValue(txtPassword);
				string value3 = Tool.GetValue(txtPassword2);
				UserInfo userByID = pageManger.GetUserByID(value);
				if (userByID == null)
				{
					message = ResourceManagement.GetLocationString("CustomerNotExist");
				}
				else if (!userByID.Password.Equals(cryptManager.EnCryptMD5(value2)))
				{
					message = ResourceManagement.GetLocationString("OriginalPWDIsWrong");
				}
				else
				{
					userByID.Password = cryptManager.EnCryptMD5(value3);
					if (pageManger.UndateUserInfo(userByID, out message))
					{
						message = ResourceManagement.GetLocationString("ModifyPWDSuccess");
						if (LoginUser.Ins.UserIns != null && value.Equals(LoginUser.Ins.UserIns.UserID, StringComparison.CurrentCultureIgnoreCase))
						{
							base.DialogResult = DialogResult.OK;
						}
						Close();
						result = true;
					}
					else
					{
						message = ResourceManagement.GetLocationString("ModifyPWDFailure");
					}
				}
			}
			Tool.ShowMessage(message);
			return result;
		}

		protected override void FunCancel()
		{
			base.FunCancel();
			Close();
		}

		private bool Ckeck(out string msg)
		{
			msg = "";
			string value = Tool.GetValue(txtUserID);
			string value2 = Tool.GetValue(txtPassword);
			string value3 = Tool.GetValue(txtPassword2);
			string value4 = Tool.GetValue(txtPassword3);
			if (value.Length == 0)
			{
				msg = lblUserID.Text + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
				return false;
			}
			if (value2.Length == 0)
			{
				msg = lblPassword.Text + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
				return false;
			}
			if (value3.Length == 0)
			{
				msg = lblNewPWD.Text + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
				return false;
			}
			if (value3.Equals(value2))
			{
				msg = ResourceManagement.GetLocationString("PWDIsSame");
				return false;
			}
			if (value4.Length == 0)
			{
				msg = lblConfirmNewPWD.Text + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
				return false;
			}
			if (!value4.Equals(value3))
			{
				msg = ResourceManagement.GetLocationString("PWDIsNotSameWithConfirmPWD");
				return false;
			}
			return true;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.SystemManagment.FormEditPWD));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblUserID = new System.Windows.Forms.Label();
			this.txtPassword3 = new System.Windows.Forms.TextBox();
			this.txtUserID = new System.Windows.Forms.TextBox();
			this.txtPassword2 = new System.Windows.Forms.TextBox();
			this.lblConfirmNewPWD = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.lblNewPWD = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblUserID, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtPassword3, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtUserID, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtPassword2, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblConfirmNewPWD, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtPassword, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblPassword, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblNewPWD, 0, 2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.lblUserID, "lblUserID");
			this.lblUserID.Name = "lblUserID";
			resources.ApplyResources(this.txtPassword3, "txtPassword3");
			this.txtPassword3.Name = "txtPassword3";
			resources.ApplyResources(this.txtUserID, "txtUserID");
			this.txtUserID.Name = "txtUserID";
			resources.ApplyResources(this.txtPassword2, "txtPassword2");
			this.txtPassword2.Name = "txtPassword2";
			resources.ApplyResources(this.lblConfirmNewPWD, "lblConfirmNewPWD");
			this.lblConfirmNewPWD.Name = "lblConfirmNewPWD";
			resources.ApplyResources(this.txtPassword, "txtPassword");
			this.txtPassword.Name = "txtPassword";
			resources.ApplyResources(this.lblPassword, "lblPassword");
			this.lblPassword.Name = "lblPassword";
			resources.ApplyResources(this.lblNewPWD, "lblNewPWD");
			this.lblNewPWD.Name = "lblNewPWD";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormEditPWD";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
