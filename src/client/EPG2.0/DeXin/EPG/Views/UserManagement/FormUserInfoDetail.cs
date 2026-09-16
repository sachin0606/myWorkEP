using System;
using System.ComponentModel;
using System.Linq;
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
	public class FormUserInfoDetail : FormDetailBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private TextBox txtPhoneNumber;

		private Panel pnlStatus;

		private RadioButton radEnUse;

		private RadioButton radUse;

		private Label lblStatus;

		private TextBox txtPwd;

		private TextBox txtUserID;

		private Label lblConfirmPWD;

		private Label lblPhoneNumber;

		private Label lblSex;

		private Label lblName;

		private TextBox txtUserName;

		private Label lblPassword;

		private TextBox txtPassword;

		private Label lblGroup;

		private Panel pnlSex;

		private RadioButton radFemale;

		private RadioButton radMale;

		private Label lblUserID;

		private ComboBox cmbUserGroup;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.UserManagement.FormUserInfoDetail));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.txtPhoneNumber = new System.Windows.Forms.TextBox();
			this.pnlStatus = new System.Windows.Forms.Panel();
			this.radEnUse = new System.Windows.Forms.RadioButton();
			this.radUse = new System.Windows.Forms.RadioButton();
			this.lblStatus = new System.Windows.Forms.Label();
			this.txtPwd = new System.Windows.Forms.TextBox();
			this.txtUserID = new System.Windows.Forms.TextBox();
			this.lblConfirmPWD = new System.Windows.Forms.Label();
			this.lblPhoneNumber = new System.Windows.Forms.Label();
			this.lblSex = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.lblGroup = new System.Windows.Forms.Label();
			this.pnlSex = new System.Windows.Forms.Panel();
			this.radFemale = new System.Windows.Forms.RadioButton();
			this.radMale = new System.Windows.Forms.RadioButton();
			this.lblUserID = new System.Windows.Forms.Label();
			this.cmbUserGroup = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.pnlStatus.SuspendLayout();
			this.pnlSex.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.txtPhoneNumber, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.pnlStatus, 1, 7);
			this.tableLayoutPanel1.Controls.Add(this.lblStatus, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this.txtPwd, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtUserID, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblConfirmPWD, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblPhoneNumber, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.lblSex, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.lblName, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtUserName, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblPassword, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtPassword, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblGroup, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.pnlSex, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.lblUserID, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.cmbUserGroup, 1, 4);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.txtPhoneNumber, "txtPhoneNumber");
			this.txtPhoneNumber.Name = "txtPhoneNumber";
			resources.ApplyResources(this.pnlStatus, "pnlStatus");
			this.pnlStatus.Controls.Add(this.radEnUse);
			this.pnlStatus.Controls.Add(this.radUse);
			this.pnlStatus.Name = "pnlStatus";
			resources.ApplyResources(this.radEnUse, "radEnUse");
			this.radEnUse.Name = "radEnUse";
			this.radEnUse.TabStop = true;
			this.radEnUse.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.radUse, "radUse");
			this.radUse.Name = "radUse";
			this.radUse.TabStop = true;
			this.radUse.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.lblStatus, "lblStatus");
			this.lblStatus.Name = "lblStatus";
			resources.ApplyResources(this.txtPwd, "txtPwd");
			this.txtPwd.Name = "txtPwd";
			resources.ApplyResources(this.txtUserID, "txtUserID");
			this.txtUserID.Name = "txtUserID";
			resources.ApplyResources(this.lblConfirmPWD, "lblConfirmPWD");
			this.lblConfirmPWD.Name = "lblConfirmPWD";
			resources.ApplyResources(this.lblPhoneNumber, "lblPhoneNumber");
			this.lblPhoneNumber.Name = "lblPhoneNumber";
			resources.ApplyResources(this.lblSex, "lblSex");
			this.lblSex.Name = "lblSex";
			resources.ApplyResources(this.lblName, "lblName");
			this.lblName.Name = "lblName";
			resources.ApplyResources(this.txtUserName, "txtUserName");
			this.txtUserName.Name = "txtUserName";
			resources.ApplyResources(this.lblPassword, "lblPassword");
			this.lblPassword.Name = "lblPassword";
			resources.ApplyResources(this.txtPassword, "txtPassword");
			this.txtPassword.Name = "txtPassword";
			resources.ApplyResources(this.lblGroup, "lblGroup");
			this.lblGroup.Name = "lblGroup";
			resources.ApplyResources(this.pnlSex, "pnlSex");
			this.pnlSex.Controls.Add(this.radFemale);
			this.pnlSex.Controls.Add(this.radMale);
			this.pnlSex.Name = "pnlSex";
			resources.ApplyResources(this.radFemale, "radFemale");
			this.radFemale.Name = "radFemale";
			this.radFemale.TabStop = true;
			this.radFemale.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.radMale, "radMale");
			this.radMale.Name = "radMale";
			this.radMale.TabStop = true;
			this.radMale.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.lblUserID, "lblUserID");
			this.lblUserID.Name = "lblUserID";
			resources.ApplyResources(this.cmbUserGroup, "cmbUserGroup");
			this.cmbUserGroup.FormattingEnabled = true;
			this.cmbUserGroup.Name = "cmbUserGroup";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormUserInfoDetail";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.pnlStatus.ResumeLayout(false);
			this.pnlStatus.PerformLayout();
			this.pnlSex.ResumeLayout(false);
			this.pnlSex.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormUserInfoDetail()
		{
			InitializeComponent();
		}

		public FormUserInfoDetail(PageEnum pageFlag)
		{
			InitializeComponent();
			base.PageFlag = pageFlag;
		}

		public FormUserInfoDetail(string userID, PageEnum pageFlag)
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
			BindUserGroup();
			InitData();
		}

		private void BindUserGroup()
		{
			GroupInfoBLL groupInfoBLL = new GroupInfoBLL();
			cmbUserGroup.DataSource = (from p in groupInfoBLL.GetAllData()
				where p.Status
				select p).ToList();
			cmbUserGroup.ValueMember = "GroupID";
			cmbUserGroup.DisplayMember = "GroupName";
		}

		protected void SetFormStyle()
		{
			base.FormBorderStyle = FormBorderStyle.Fixed3D;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
		}

		protected override bool FunSubmit()
		{
			bool flag = false;
			string message = "";
			if (!CheckSubmit(out message))
			{
				Tool.ShowMessage(message);
				return false;
			}
			try
			{
				EnUserInfo();
				message = "";
				switch (base.PageFlag)
				{
				case PageEnum.Add:
					flag = userManger.AddUserInfo(user, out message);
					if (flag)
					{
						logBll.AddLog(EnumFunctionName.SysFuncUSERINFO, EnumOSName.Add, user.UserID);
					}
					break;
				case PageEnum.Edit:
					flag = userManger.UndateUserInfo(user, out message);
					if (flag)
					{
						if (LoginUser.Ins.UserIns.UserID.Equals(user.UserID))
						{
							LoginUser.Ins.UserIns = user;
						}
						LogBLL logBLL = new LogBLL();
						logBll.AddLog(EnumFunctionName.SysFuncUSERINFO, EnumOSName.Edit, user.UserID);
					}
					break;
				}
				if (message.Length > 0)
				{
					Tool.ShowMessage(message);
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
			if (flag)
			{
				Close();
			}
			return true;
		}

		protected override void FunCancel()
		{
			InitData();
		}

		private void InitData()
		{
			string locationString = ResourceManagement.GetLocationString("SysFuncOperatorManagement");
			switch (base.PageFlag)
			{
			case PageEnum.Show:
				Text = locationString + " " + LocationControl.Ins.LocationItems[LacationEnum.tsbQuer];
				BindUserData(user);
				break;
			case PageEnum.Edit:
				Text = locationString + " " + LocationControl.Ins.LocationItems[LacationEnum.tsbEdit];
				BindUserData(user);
				break;
			case PageEnum.Add:
				BindUserData(null);
				Text = locationString + " " + LocationControl.Ins.LocationItems[LacationEnum.tsbAdd];
				break;
			}
			SetReadOnly(base.PageFlag);
		}

		private void BindUserData(UserInfo tempUser)
		{
			txtUserID.Text = ((tempUser == null) ? "" : tempUser.UserID);
			txtUserName.Text = ((tempUser == null) ? "" : tempUser.UserName);
			TextBox textBox = txtPassword;
			string text = (txtPwd.Text = ((tempUser == null) ? "" : tempUser.Password));
			textBox.Text = text;
			if (tempUser == null)
			{
				radMale.Checked = true;
			}
			else if (tempUser.Sex)
			{
				radMale.Checked = true;
			}
			else
			{
				radFemale.Checked = true;
			}
			txtPhoneNumber.Text = ((tempUser == null) ? "" : tempUser.PhoneNumber);
			if (tempUser == null)
			{
				cmbUserGroup.SelectedIndex = 0;
			}
			else
			{
				cmbUserGroup.SelectedValue = Tool.GetInt32(tempUser.GroupID);
			}
			if (tempUser == null)
			{
				radUse.Checked = true;
			}
			else if (tempUser.Status)
			{
				radUse.Checked = true;
			}
			else
			{
				radEnUse.Checked = true;
			}
		}

		private void SetReadOnly(PageEnum flag)
		{
			switch (flag)
			{
			case PageEnum.Show:
				SetStyleQuery();
				break;
			case PageEnum.Add:
				SetStyleAdd();
				break;
			case PageEnum.Edit:
				SetStyleEdit();
				break;
			}
		}

		private void SetStyleAdd()
		{
			foreach (Control control in base.Controls)
			{
				TextBox textBox = control as TextBox;
				if (textBox != null)
				{
					textBox.Text = "";
				}
			}
			cmbUserGroup.Text = "";
			radMale.Checked = true;
			radUse.Checked = true;
		}

		private void SetStyleQuery()
		{
			TextBox textBox = txtUserID;
			TextBox textBox2 = txtUserName;
			TextBox textBox3 = txtPwd;
			TextBox textBox4 = txtPassword;
			bool flag = (txtPhoneNumber.ReadOnly = true);
			flag = (textBox4.ReadOnly = flag);
			flag = (textBox3.ReadOnly = flag);
			flag = (textBox2.ReadOnly = flag);
			textBox.ReadOnly = flag;
			pnlSex.Enabled = false;
			pnlStatus.Enabled = false;
			cmbUserGroup.Enabled = false;
		}

		private void SetStyleEdit()
		{
			txtUserID.ReadOnly = true;
			txtPassword.ReadOnly = true;
			txtPwd.ReadOnly = true;
			if (txtUserID.Text.Trim().Equals("Admin"))
			{
				cmbUserGroup.Enabled = false;
				pnlStatus.Enabled = false;
			}
		}

		private void EnUserInfo()
		{
			if (user == null)
			{
				user = new UserInfo();
				user.Creator = LoginUser.Ins.UserIns.UserID;
			}
			user.UserID = Tool.GetValue(txtUserID);
			user.UserName = Tool.GetValue(txtUserName);
			if (base.PageFlag == PageEnum.Add)
			{
				user.Password = CryptionManage.Instance.EnCryptMD5(txtPassword.Text.Trim());
			}
			else
			{
				user.Password = txtPassword.Text.Trim();
			}
			user.Sex = radMale.Checked;
			user.GroupID = Tool.GetValue(cmbUserGroup);
			user.PhoneNumber = Tool.GetValue(txtPhoneNumber);
			user.Status = radUse.Checked;
			user.LastEditor = LoginUser.Ins.UserIns.UserID;
		}

		private bool CheckSubmit(out string msg)
		{
			msg = "";
			if (Tool.GetValue(txtUserID).Length == 0)
			{
				msg = lblUserID.Text + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
				txtUserID.Text = "";
				txtUserID.Focus();
				return false;
			}
			if (Tool.GetValue(txtUserName).Length == 0)
			{
				msg = lblName.Text + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
				txtUserName.Text = "";
				txtUserName.Focus();
				return false;
			}
			if (Tool.GetValue(txtPassword).Length == 0)
			{
				msg = lblPassword.Text + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
				txtPassword.Text = "";
				txtPassword.Focus();
				return false;
			}
			if (Tool.GetValue(txtPwd).Length == 0)
			{
				msg = lblConfirmPWD.Text + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
				txtPwd.Text = "";
				txtPwd.Focus();
				return false;
			}
			if (!Tool.GetValue(txtPwd).Equals(Tool.GetValue(txtPassword)))
			{
				msg = ResourceManagement.GetLocationString("Inconsistent");
				txtPwd.Text = "";
				txtPwd.Focus();
				return false;
			}
			return true;
		}
	}
}
