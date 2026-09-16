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
	public class FormPermitGroupManagementDetail : FormDetailBase
	{
		private IContainer components = null;

		private Label lblGroup;

		private Label lblFunction;

		private Panel pnlPermit;

		private CheckBox chkbEdit;

		private CheckBox chkbPrint;

		private CheckBox chkbQuery;

		private CheckBox chkbDel;

		private CheckBox chkbAdd;

		private ComboBox cmbFunction;

		private ComboBox cmbGroup;

		private TableLayoutPanel tableLayoutPanel1;

		private PermitGroupManagementBLL permitBLL = new PermitGroupManagementBLL();

		private PermitGroup permit = new PermitGroup();

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.UserManagement.FormPermitGroupManagementDetail));
			this.lblGroup = new System.Windows.Forms.Label();
			this.lblFunction = new System.Windows.Forms.Label();
			this.pnlPermit = new System.Windows.Forms.Panel();
			this.chkbEdit = new System.Windows.Forms.CheckBox();
			this.chkbPrint = new System.Windows.Forms.CheckBox();
			this.chkbQuery = new System.Windows.Forms.CheckBox();
			this.chkbDel = new System.Windows.Forms.CheckBox();
			this.chkbAdd = new System.Windows.Forms.CheckBox();
			this.cmbGroup = new System.Windows.Forms.ComboBox();
			this.cmbFunction = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.pnlPermit.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.lblGroup, "lblGroup");
			this.lblGroup.Name = "lblGroup";
			resources.ApplyResources(this.lblFunction, "lblFunction");
			this.lblFunction.Name = "lblFunction";
			resources.ApplyResources(this.pnlPermit, "pnlPermit");
			this.tableLayoutPanel1.SetColumnSpan(this.pnlPermit, 3);
			this.pnlPermit.Controls.Add(this.chkbEdit);
			this.pnlPermit.Controls.Add(this.chkbPrint);
			this.pnlPermit.Controls.Add(this.chkbQuery);
			this.pnlPermit.Controls.Add(this.chkbDel);
			this.pnlPermit.Controls.Add(this.chkbAdd);
			this.pnlPermit.Name = "pnlPermit";
			resources.ApplyResources(this.chkbEdit, "chkbEdit");
			this.chkbEdit.Name = "chkbEdit";
			this.chkbEdit.UseVisualStyleBackColor = true;
			this.chkbEdit.Click += new System.EventHandler(chkbEdit_Click);
			resources.ApplyResources(this.chkbPrint, "chkbPrint");
			this.chkbPrint.Name = "chkbPrint";
			this.chkbPrint.UseVisualStyleBackColor = true;
			this.chkbPrint.Click += new System.EventHandler(chkbPrint_Click);
			resources.ApplyResources(this.chkbQuery, "chkbQuery");
			this.chkbQuery.Name = "chkbQuery";
			this.chkbQuery.UseVisualStyleBackColor = true;
			this.chkbQuery.Click += new System.EventHandler(chkbQuery_Click);
			resources.ApplyResources(this.chkbDel, "chkbDel");
			this.chkbDel.Name = "chkbDel";
			this.chkbDel.UseVisualStyleBackColor = true;
			this.chkbDel.Click += new System.EventHandler(chkbDel_Click);
			resources.ApplyResources(this.chkbAdd, "chkbAdd");
			this.chkbAdd.Name = "chkbAdd";
			this.chkbAdd.UseVisualStyleBackColor = true;
			this.chkbAdd.Click += new System.EventHandler(chkbAdd_Click);
			resources.ApplyResources(this.cmbGroup, "cmbGroup");
			this.cmbGroup.FormattingEnabled = true;
			this.cmbGroup.Name = "cmbGroup";
			resources.ApplyResources(this.cmbFunction, "cmbFunction");
			this.cmbFunction.FormattingEnabled = true;
			this.cmbFunction.Name = "cmbFunction";
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblGroup, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.pnlPermit, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.cmbFunction, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblFunction, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.cmbGroup, 1, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Name = "FormPermitGroupManagementDetail";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.pnlPermit.ResumeLayout(false);
			this.pnlPermit.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormPermitGroupManagementDetail()
		{
			InitializeComponent();
		}

		public FormPermitGroupManagementDetail(int groupID, string functionID, PageEnum pageEnum)
		{
			InitializeComponent();
			try
			{
				permit = permitBLL.GetDataByGroupIDAndFunctionID(groupID, functionID).FirstOrDefault();
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
			base.PageFlag = pageEnum;
		}

		protected override void FormLoad()
		{
			BindControl();
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
				BindToPermitgGroup();
				switch (base.PageFlag)
				{
				case PageEnum.Edit:
				{
					num = permitBLL.Update(permit);
					LogBLL logBLL = new LogBLL();
					LogInfo logInfo = new LogInfo();
					logInfo.LogID = null;
					logInfo.UserID = LoginUser.Ins.UserIns.UserID;
					logInfo.UserName = LoginUser.Ins.UserIns.UserName;
					logInfo.OSName = LocationControl.Ins.LocationItems[LacationEnum.tsbAdd];
					logInfo.Description = LocationControl.Ins.LocationItems[LacationEnum.tsbAdd];
					logInfo.OperateDateTime = null;
					logInfo.IPAddress = LoginUser.Ins.GetClientIP();
					break;
				}
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
			return true;
		}

		private void BindToPermitgGroup()
		{
			permit.GroupID = Convert.ToInt16(cmbGroup.SelectedValue);
			permit.FunctionID = cmbFunction.SelectedValue.ToString();
			permit.CanAdd = chkbAdd.Checked;
			permit.CanDelete = chkbDel.Checked;
			permit.CanEdit = chkbEdit.Checked;
			permit.CanPrint = chkbPrint.Checked;
			permit.CanQuery = chkbQuery.Checked;
			permit.Creator = LoginUser.Ins.UserIns.UserID;
			permit.CreateTime = LoginUser.Ins.GetNowTime();
			permit.LastEditor = LoginUser.Ins.UserIns.UserID;
			permit.LastEditTime = LoginUser.Ins.GetNowTime();
		}

		private bool CheckBeforeSave(out string msg)
		{
			if (cmbGroup.SelectedValue == null)
			{
				msg = ResourceManagement.GetLocationString("GroupForbid");
				return false;
			}
			msg = "";
			return true;
		}

		private void BindComboBox()
		{
			GroupInfoBLL groupInfoBLL = new GroupInfoBLL();
			cmbGroup.DataSource = (from @group in groupInfoBLL.GetAllData()
				where @group.Status.Equals(true)
				select @group).ToList();
			cmbGroup.DisplayMember = "GroupName";
			cmbGroup.ValueMember = "GroupID";
		}

		private void BindComboBoxFuc()
		{
			cmbFunction.DataSource = BaseData.Ins.FunctionItems.ToList();
			cmbFunction.DisplayMember = "LocationFunctionName";
			cmbFunction.ValueMember = "FunctionID";
		}

		private void BindControl()
		{
			BindComboBox();
			BindComboBoxFuc();
			cmbGroup.SelectedValue = permit.GroupID;
			cmbFunction.SelectedValue = Tool.GetInt32(permit.FunctionID);
			chkbAdd.Checked = permit.CanAdd;
			chkbDel.Checked = permit.CanDelete;
			chkbEdit.Checked = permit.CanEdit;
			chkbPrint.Checked = permit.CanPrint;
			chkbQuery.Checked = permit.CanQuery;
		}

		private void InitData()
		{
			switch (base.PageFlag)
			{
			case PageEnum.Show:
				Text = ResourceManagement.GetLocationString("FormPermitGroupManagement") + ResourceManagement.GetLocationString("tsbQuery");
				BindControl();
				break;
			case PageEnum.Edit:
				Text = ResourceManagement.GetLocationString("FormPermitGroupManagement") + ResourceManagement.GetLocationString("tsbEdit");
				BindControl();
				break;
			case PageEnum.Add:
				Text = ResourceManagement.GetLocationString("FormPermitGroupManagement") + ResourceManagement.GetLocationString("tsbAdd");
				break;
			}
			SetReadOnly(base.PageFlag);
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
		}

		private void SetStyleQuery()
		{
			pnlPermit.Enabled = false;
		}

		private void SetStyleEdit()
		{
		}

		private void chkbQuery_Click(object sender, EventArgs e)
		{
			switch (chkbQuery.CheckState)
			{
			case CheckState.Checked:
				break;
			case CheckState.Unchecked:
				chkbAdd.Checked = false;
				chkbDel.Checked = false;
				chkbEdit.Checked = false;
				chkbPrint.Checked = false;
				break;
			case CheckState.Indeterminate:
				break;
			}
		}

		private void chkbAdd_Click(object sender, EventArgs e)
		{
			chkbQuery.Checked = true;
		}

		private void chkbEdit_Click(object sender, EventArgs e)
		{
			chkbQuery.Checked = true;
		}

		private void chkbDel_Click(object sender, EventArgs e)
		{
			chkbQuery.Checked = true;
		}

		private void chkbPrint_Click(object sender, EventArgs e)
		{
			chkbQuery.Checked = true;
		}
	}
}
