using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.BLL.ApplicationManagement;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.UserManagement
{
	public class FormGroupInfoDetail : FormDetailBase
	{
		private IContainer components = null;

		private RadioButton radNoUse;

		private RadioButton radUse;

		private Label lblStatus;

		private Label lblGroupName;

		private Label lblGroupID;

		private TextBox txtGroupID;

		private TextBox txtGroupName;

		private Panel panel1;

		private TableLayoutPanel tableLayoutPanel1;

		private LogBLL logBLL = new LogBLL();

		private GroupInfoBLL giBLL = new GroupInfoBLL();

		private GroupInfo groupinfo = new GroupInfo();

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.UserManagement.FormGroupInfoDetail));
			this.radNoUse = new System.Windows.Forms.RadioButton();
			this.radUse = new System.Windows.Forms.RadioButton();
			this.lblStatus = new System.Windows.Forms.Label();
			this.lblGroupName = new System.Windows.Forms.Label();
			this.lblGroupID = new System.Windows.Forms.Label();
			this.txtGroupID = new System.Windows.Forms.TextBox();
			this.txtGroupName = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(base.error, "error");
			resources.ApplyResources(base.grbText, "grbText");
			base.error.SetError(base.grbText, resources.GetString("grbText.Error"));
			base.error.SetIconAlignment(base.grbText, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("grbText.IconAlignment"));
			base.error.SetIconPadding(base.grbText, (int)resources.GetObject("grbText.IconPadding"));
			resources.ApplyResources(this.radNoUse, "radNoUse");
			base.error.SetError(this.radNoUse, resources.GetString("radNoUse.Error"));
			base.error.SetIconAlignment(this.radNoUse, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("radNoUse.IconAlignment"));
			base.error.SetIconPadding(this.radNoUse, (int)resources.GetObject("radNoUse.IconPadding"));
			this.radNoUse.Name = "radNoUse";
			this.radNoUse.TabStop = true;
			this.radNoUse.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.radUse, "radUse");
			base.error.SetError(this.radUse, resources.GetString("radUse.Error"));
			base.error.SetIconAlignment(this.radUse, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("radUse.IconAlignment"));
			base.error.SetIconPadding(this.radUse, (int)resources.GetObject("radUse.IconPadding"));
			this.radUse.Name = "radUse";
			this.radUse.TabStop = true;
			this.radUse.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.lblStatus, "lblStatus");
			base.error.SetError(this.lblStatus, resources.GetString("lblStatus.Error"));
			base.error.SetIconAlignment(this.lblStatus, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lblStatus.IconAlignment"));
			base.error.SetIconPadding(this.lblStatus, (int)resources.GetObject("lblStatus.IconPadding"));
			this.lblStatus.Name = "lblStatus";
			resources.ApplyResources(this.lblGroupName, "lblGroupName");
			base.error.SetError(this.lblGroupName, resources.GetString("lblGroupName.Error"));
			base.error.SetIconAlignment(this.lblGroupName, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lblGroupName.IconAlignment"));
			base.error.SetIconPadding(this.lblGroupName, (int)resources.GetObject("lblGroupName.IconPadding"));
			this.lblGroupName.Name = "lblGroupName";
			resources.ApplyResources(this.lblGroupID, "lblGroupID");
			base.error.SetError(this.lblGroupID, resources.GetString("lblGroupID.Error"));
			base.error.SetIconAlignment(this.lblGroupID, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lblGroupID.IconAlignment"));
			base.error.SetIconPadding(this.lblGroupID, (int)resources.GetObject("lblGroupID.IconPadding"));
			this.lblGroupID.Name = "lblGroupID";
			resources.ApplyResources(this.txtGroupID, "txtGroupID");
			base.error.SetError(this.txtGroupID, resources.GetString("txtGroupID.Error"));
			base.error.SetIconAlignment(this.txtGroupID, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("txtGroupID.IconAlignment"));
			base.error.SetIconPadding(this.txtGroupID, (int)resources.GetObject("txtGroupID.IconPadding"));
			this.txtGroupID.Name = "txtGroupID";
			this.txtGroupID.ReadOnly = true;
			resources.ApplyResources(this.txtGroupName, "txtGroupName");
			base.error.SetError(this.txtGroupName, resources.GetString("txtGroupName.Error"));
			base.error.SetIconAlignment(this.txtGroupName, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("txtGroupName.IconAlignment"));
			base.error.SetIconPadding(this.txtGroupName, (int)resources.GetObject("txtGroupName.IconPadding"));
			this.txtGroupName.Name = "txtGroupName";
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Controls.Add(this.radUse);
			this.panel1.Controls.Add(this.radNoUse);
			base.error.SetError(this.panel1, resources.GetString("panel1.Error"));
			base.error.SetIconAlignment(this.panel1, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("panel1.IconAlignment"));
			base.error.SetIconPadding(this.panel1, (int)resources.GetObject("panel1.IconPadding"));
			this.panel1.Name = "panel1";
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtGroupID, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblStatus, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtGroupName, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblGroupName, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblGroupID, 0, 0);
			base.error.SetError(this.tableLayoutPanel1, resources.GetString("tableLayoutPanel1.Error"));
			base.error.SetIconAlignment(this.tableLayoutPanel1, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("tableLayoutPanel1.IconAlignment"));
			base.error.SetIconPadding(this.tableLayoutPanel1, (int)resources.GetObject("tableLayoutPanel1.IconPadding"));
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.tableLayoutPanel1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Name = "FormGroupInfoDetail";
			base.Load += new System.EventHandler(FormGroupInfoDetail_Load);
			base.Controls.SetChildIndex(base.grbText, 0);
			base.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormGroupInfoDetail()
		{
			InitializeComponent();
		}

		public FormGroupInfoDetail(PageEnum pageEnum)
		{
			InitializeComponent();
			base.PageFlag = pageEnum;
		}

		public FormGroupInfoDetail(int GroupID, PageEnum pageEnum)
		{
			InitializeComponent();
			try
			{
				groupinfo = giBLL.GetDataByGroupID(GroupID).First();
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
			base.PageFlag = ((GroupID < 0) ? PageEnum.Add : pageEnum);
		}

		private void FormGroupInfoDetail_Load(object sender, EventArgs e)
		{
			InitData();
		}

		protected override bool FunSubmit()
		{
			int num = 0;
			string message = "";
			if (!CheckBeforeSave(out message))
			{
				Tool.ShowMessage(message);
				return false;
			}
			try
			{
				BindToGroupInfo();
				switch (base.PageFlag)
				{
				case PageEnum.Add:
					num = giBLL.Add(groupinfo);
					logBLL.AddLog(EnumFunctionName.SysFuncUSERGROUP, EnumOSName.Add, "[" + groupinfo.GroupID + "][" + groupinfo.GroupName + "]");
					break;
				case PageEnum.Edit:
					num = giBLL.Update(groupinfo);
					logBLL.AddLog(EnumFunctionName.SysFuncUSERGROUP, EnumOSName.Delete, "[" + groupinfo.GroupID + "][" + groupinfo.GroupName + "]");
					break;
				}
				if (num > 0)
				{
					Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.SaveSucceed]);
					base.DialogResult = DialogResult.OK;
				}
				else
				{
					base.DialogResult = DialogResult.No;
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
			if (num <= 0)
			{
			}
			return true;
		}

		private void BindToGroupInfo()
		{
			int result = 0;
			if (int.TryParse(txtGroupID.Text.Trim(), out result))
			{
				groupinfo.GroupID = result;
			}
			else
			{
				groupinfo.GroupID = 0;
			}
			groupinfo.GroupName = txtGroupName.Text.Trim();
			if (radUse.Checked)
			{
				groupinfo.Status = true;
			}
			else
			{
				groupinfo.Status = false;
			}
			groupinfo.Creator = LoginUser.Ins.UserIns.UserID;
			groupinfo.CreateTime = LoginUser.Ins.GetNowTime();
			groupinfo.LastEditor = LoginUser.Ins.UserIns.UserID;
			groupinfo.LastEditTime = LoginUser.Ins.GetNowTime();
		}

		private void BindToControl(GroupInfo group)
		{
			txtGroupID.Text = group.GroupID.ToString();
			txtGroupName.Text = group.GroupName;
			if (group.Status)
			{
				radUse.Checked = true;
			}
			else
			{
				radNoUse.Checked = true;
			}
		}

		private void InitData()
		{
			switch (base.PageFlag)
			{
			case PageEnum.Show:
				Text = ResourceManagement.GetLocationString("FormGroupInfoDetail") + "-" + LocationControl.Ins.LocationItems[LacationEnum.tsbQuer];
				BindToControl(groupinfo);
				break;
			case PageEnum.Edit:
				Text = ResourceManagement.GetLocationString("FormGroupInfoDetail") + "-" + LocationControl.Ins.LocationItems[LacationEnum.tsbEdit];
				BindToControl(groupinfo);
				break;
			case PageEnum.Add:
				Text = ResourceManagement.GetLocationString("FormGroupInfoDetail") + "-" + LocationControl.Ins.LocationItems[LacationEnum.tsbAdd];
				break;
			}
			SetReadOnly(base.PageFlag);
		}

		protected override void FunCancel()
		{
			txtGroupID.Text = "";
			txtGroupName.Text = "";
			radUse.Checked = true;
			radNoUse.Checked = false;
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
			txtGroupID.Text = "";
			txtGroupName.Text = "";
			radUse.Checked = true;
		}

		private void SetStyleQuery()
		{
			txtGroupID.ReadOnly = true;
			txtGroupName.ReadOnly = true;
			panel1.Enabled = false;
		}

		private void SetStyleEdit()
		{
		}

		private bool CheckBeforeSave(out string msg)
		{
			if (string.IsNullOrEmpty(txtGroupName.Text.Trim()))
			{
				msg = lblGroupName.Text + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
				txtGroupName.Focus();
				return false;
			}
			msg = LocationControl.Ins.LocationItems[LacationEnum.SaveSucceed];
			return true;
		}
	}
}
