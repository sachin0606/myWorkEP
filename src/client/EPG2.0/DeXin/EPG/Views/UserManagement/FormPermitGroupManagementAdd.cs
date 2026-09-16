using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
	public class FormPermitGroupManagementAdd : FormDetailBase
	{
		private IContainer components = null;

		private Label lblGroup;

		private Panel pnlPermit;

		private FlowLayoutPanel flowLayoutPanel1;

		private CheckBox chkbQuery;

		private CheckBox chkbAdd;

		private CheckBox chkbEdit;

		private CheckBox chkbDel;

		private CheckBox chkbPrint;

		private ComboBox cmbGroupInfo;

		private TreeView treeView1;

		private TableLayoutPanel tableLayoutPanel1;

		private PermitGroupManagementBLL permitBLL = new PermitGroupManagementBLL();

		private PermitGroup permitGrp = new PermitGroup();

		private IList<PermitGroup> permitGroups = null;

		private LogBLL logBLL = new LogBLL();

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.UserManagement.FormPermitGroupManagementAdd));
			this.lblGroup = new System.Windows.Forms.Label();
			this.pnlPermit = new System.Windows.Forms.Panel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.chkbQuery = new System.Windows.Forms.CheckBox();
			this.chkbAdd = new System.Windows.Forms.CheckBox();
			this.chkbEdit = new System.Windows.Forms.CheckBox();
			this.chkbDel = new System.Windows.Forms.CheckBox();
			this.chkbPrint = new System.Windows.Forms.CheckBox();
			this.cmbGroupInfo = new System.Windows.Forms.ComboBox();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.pnlPermit.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.lblGroup, "lblGroup");
			this.lblGroup.Name = "lblGroup";
			this.tableLayoutPanel1.SetColumnSpan(this.pnlPermit, 3);
			this.pnlPermit.Controls.Add(this.flowLayoutPanel1);
			resources.ApplyResources(this.pnlPermit, "pnlPermit");
			this.pnlPermit.Name = "pnlPermit";
			this.flowLayoutPanel1.Controls.Add(this.chkbQuery);
			this.flowLayoutPanel1.Controls.Add(this.chkbAdd);
			this.flowLayoutPanel1.Controls.Add(this.chkbEdit);
			this.flowLayoutPanel1.Controls.Add(this.chkbDel);
			this.flowLayoutPanel1.Controls.Add(this.chkbPrint);
			resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			resources.ApplyResources(this.chkbQuery, "chkbQuery");
			this.chkbQuery.Name = "chkbQuery";
			this.chkbQuery.UseVisualStyleBackColor = true;
			this.chkbQuery.Click += new System.EventHandler(chkbQuery_Click);
			resources.ApplyResources(this.chkbAdd, "chkbAdd");
			this.chkbAdd.Name = "chkbAdd";
			this.chkbAdd.UseVisualStyleBackColor = true;
			this.chkbAdd.Click += new System.EventHandler(chkbAdd_Click);
			resources.ApplyResources(this.chkbEdit, "chkbEdit");
			this.chkbEdit.Name = "chkbEdit";
			this.chkbEdit.UseVisualStyleBackColor = true;
			this.chkbEdit.Click += new System.EventHandler(chkbEdit_Click);
			resources.ApplyResources(this.chkbDel, "chkbDel");
			this.chkbDel.Name = "chkbDel";
			this.chkbDel.UseVisualStyleBackColor = true;
			this.chkbDel.Click += new System.EventHandler(chkbDel_Click);
			resources.ApplyResources(this.chkbPrint, "chkbPrint");
			this.chkbPrint.Name = "chkbPrint";
			this.chkbPrint.UseVisualStyleBackColor = true;
			this.chkbPrint.Click += new System.EventHandler(chkbPrint_Click);
			resources.ApplyResources(this.cmbGroupInfo, "cmbGroupInfo");
			this.cmbGroupInfo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbGroupInfo.FormattingEnabled = true;
			this.cmbGroupInfo.Name = "cmbGroupInfo";
			this.cmbGroupInfo.SelectedIndexChanged += new System.EventHandler(cmbGroupInfo_SelectedIndexChanged);
			resources.ApplyResources(this.treeView1, "treeView1");
			this.treeView1.Name = "treeView1";
			this.tableLayoutPanel1.SetRowSpan(this.treeView1, 3);
			this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(treeView1_AfterCheck);
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblGroup, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.cmbGroupInfo, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.pnlPermit, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.treeView1, 3, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormPermitGroupManagementAdd";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.pnlPermit.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormPermitGroupManagementAdd()
		{
			InitializeComponent();
			base.PageFlag = PageEnum.Add;
		}

		protected override void FormLoad()
		{
			InitData();
			BindComboBox();
			LoadFunctionItem();
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
				switch (base.PageFlag)
				{
				case PageEnum.Add:
				{
					IList<PermitGroup> list = new List<PermitGroup>();
					list = GetSelectedNodes(treeView1);
					num = permitBLL.AddWithSqlTrans(list, permitGroups);
					logBLL.AddLog(EnumFunctionName.SysFuncPermitGroupManagemnent, EnumOSName.Add, EnumOSName.Add.ToString());
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
					Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.SaveFailure]);
					base.DialogResult = DialogResult.No;
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
			return true;
		}

		private bool CheckBeforeSave(out string msg)
		{
			if (!chkbAdd.Checked && !chkbQuery.Checked && !chkbDel.Checked && !chkbEdit.Checked && !chkbPrint.Checked)
			{
				msg = ResourceManagement.GetLocationString("SelectOneOrMorePermit");
				return false;
			}
			if (base.PageFlag == PageEnum.Add)
			{
			}
			UserInfoManager userInfoManager = new UserInfoManager();
			string text = cmbGroupInfo.SelectedValue.ToString();
			UserInfo userInfo = new UserInfo();
			try
			{
				userInfo = userInfoManager.GetUserByID("Admin");
			}
			catch
			{
				msg = LocationControl.Ins.LocationItems[LacationEnum.OperateFailure];
				return false;
			}
			if (text.Equals(userInfo.GroupID))
			{
				msg = ResourceManagement.GetLocationString("CannotAddPermitOfAdmin");
				return false;
			}
			msg = "";
			return true;
		}

		private void BindComboBox()
		{
			GroupInfoBLL groupInfoBLL = new GroupInfoBLL();
			try
			{
				cmbGroupInfo.DataSource = (from @group in groupInfoBLL.GetAllData()
					where @group.Status.Equals(true)
					select @group).ToList();
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
			cmbGroupInfo.DisplayMember = "GroupName";
			cmbGroupInfo.ValueMember = "GroupID";
		}

		private void LoadFunctionItem()
		{
			try
			{
				treeView1.Nodes.Clear();
				treeView1.CheckBoxes = true;
				TreeNode treeNode = new TreeNode();
				SysFunction sysFunction = new SysFunction();
				sysFunction.FunctionKeyEnum = EnumFunctionName.SysFuncAllFunctions;
				treeNode.Text = sysFunction.LocationFunctionName;
				treeNode.Tag = sysFunction;
				AddNodes(sysFunction.FunctionKey, treeNode);
				treeView1.Nodes.Add(treeNode);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void AddNodes(string key, TreeNode node)
		{
			try
			{
				IList<SysFunction> functionItems = BaseData.Ins.FunctionItems;
				TreeNode treeNode = new TreeNode();
				SysFunction temp;
				foreach (SysFunction item in functionItems)
				{
					temp = item;
					treeNode = new TreeNode();
					treeNode.Tag = temp;
					treeNode.Text = temp.LocationFunctionName;
					if (permitGroups.Where((PermitGroup ins) => ins.FunctionID.Equals(temp.FunctionID.ToString())).Count() > 0)
					{
						treeNode.Checked = true;
					}
					node.Nodes.Add(treeNode);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		private void InitData()
		{
			switch (base.PageFlag)
			{
			case PageEnum.Show:
				Text = ResourceManagement.GetLocationString("FormPermitGroupManagement") + ResourceManagement.GetLocationString("tsbQuery");
				break;
			case PageEnum.Edit:
				Text = ResourceManagement.GetLocationString("FormPermitGroupManagement") + ResourceManagement.GetLocationString("tsbEdit");
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
		}

		private void SetStyleEdit()
		{
			pnlPermit.Visible = false;
			treeView1.CheckBoxes = false;
			foreach (TreeNode node in treeView1.Nodes)
			{
				Recursive(node);
			}
		}

		private List<PermitGroup> GetSelectedNodes(TreeView treeView)
		{
			List<PermitGroup> list = new List<PermitGroup>();
			TreeNodeCollection nodes = treeView.Nodes;
			foreach (TreeNode item in nodes)
			{
				Recursive(item, list);
			}
			return list;
		}

		private void Recursive(TreeNode treeNode, List<PermitGroup> outlst)
		{
			foreach (TreeNode node in treeNode.Nodes)
			{
				if (node.Checked)
				{
					PermitGroup permitGroup = new PermitGroup();
					permitGroup.GroupID = Convert.ToInt32(cmbGroupInfo.SelectedValue);
					SysFunction sysFunction = node.Tag as SysFunction;
					permitGroup.FunctionID = sysFunction.FunctionID.ToString();
					permitGroup.CanAdd = chkbAdd.Checked;
					permitGroup.CanDelete = chkbDel.Checked;
					permitGroup.CanEdit = chkbEdit.Checked;
					permitGroup.CanPrint = chkbPrint.Checked;
					permitGroup.CanQuery = chkbQuery.Checked;
					permitGroup.Creator = LoginUser.Ins.UserIns.UserID;
					permitGroup.CreateTime = LoginUser.Ins.GetNowTime();
					permitGroup.LastEditor = LoginUser.Ins.UserIns.UserID;
					permitGroup.LastEditTime = LoginUser.Ins.GetNowTime();
					if (sysFunction.FunctionKeyEnum == EnumFunctionName.SysFuncAllFunctions)
					{
						continue;
					}
					outlst.Add(permitGroup);
				}
				Recursive(node, outlst);
			}
		}

		private void Recursive(TreeNode treeNode)
		{
			foreach (TreeNode node in treeNode.Nodes)
			{
				if (node.Checked)
				{
					node.ForeColor = Color.Black;
				}
				else
				{
					node.ForeColor = Color.Gray;
				}
				Recursive(node);
			}
		}

		private void SetAllChildChecked(TreeNode treeNode, bool isChk)
		{
			foreach (TreeNode node in treeNode.Nodes)
			{
				node.Checked = isChk;
				SetAllChildChecked(node, isChk);
			}
		}

		private void bindByGroupID()
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

		private void cmbGroupInfo_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				GroupInfo groupInfo = new GroupInfo();
				groupInfo = cmbGroupInfo.SelectedItem as GroupInfo;
				permitGroups = permitBLL.GetDataByGroupID(groupInfo.GroupID);
				LoadFunctionItem();
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (e.Action != TreeViewAction.Unknown)
			{
				SetAllChildChecked(e.Node, e.Node.Checked);
			}
		}
	}
}
