using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using Com.Cn.Tool.UserControls;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.UserManagement
{
	public class FormUserGroup : FormToolBase
	{
		private IContainer components = null;

		private DGV dgvGroupInfo;

		private DataGridViewCheckBoxColumn SelectDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn groupIDDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn groupNameDataGridViewTextBoxColumn;

		private DataGridViewCheckBoxColumn statusDataGridViewCheckBoxColumn;

		private DataGridViewTextBoxColumn creatorDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn createTimeDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn lastEditorDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn lastEditTimeDataGridViewTextBoxColumn;

		private DataGridViewButtonColumn colGroupDetail;

		private ContextMenuStrip cmsSelect;

		private ToolStripMenuItem tsmSelectAll;

		private ToolStripMenuItem tsmDeselectAll;

		public FormUserGroup()
		{
			InitializeComponent();
			_FunctionName = EnumFunctionName.SysFuncUSERGROUP;
		}

		public FormUserGroup(EnumFunctionName fucntionName)
		{
			InitializeComponent();
			_FunctionName = fucntionName;
		}

		protected override void FormLoad()
		{
			colGroupDetail.Text = ResourceManagement.GetLocationString("LookOver");
			SetMenuVisible(false, true, true, true, false, false, false, true, false);
			BindDataGridView();
		}

		private void BindDataGridView()
		{
			GroupInfoBLL groupInfoBLL = new GroupInfoBLL();
			try
			{
				dgvGroupInfo.DataSource = groupInfoBLL.GetAllData();
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		protected override void FuncAdd()
		{
			FormGroupInfoDetail formGroupInfoDetail = new FormGroupInfoDetail(PageEnum.Add);
			formGroupInfoDetail.Owner = this;
			formGroupInfoDetail.StartPosition = FormStartPosition.CenterParent;
			if (formGroupInfoDetail.ShowDialog() == DialogResult.OK)
			{
				BindDataGridView();
			}
		}

		protected override bool FuncDelete()
		{
			string text = "";
			LogBLL logBLL = new LogBLL();
			UserInfoManager userInfoManager = new UserInfoManager();
			List<GroupInfo> list = new List<GroupInfo>();
			PermitGroupManagementBLL permitGroupManagementBLL = new PermitGroupManagementBLL();
			foreach (DataGridViewRow item in (IEnumerable)dgvGroupInfo.Rows)
			{
				if (Tool.GetBoolean(item.Cells[SelectDataGridViewTextBoxColumn.Name].Value))
				{
					GroupInfo groupInfo = new GroupInfo();
					groupInfo.GroupID = Tool.GetInt32(item.Cells[groupIDDataGridViewTextBoxColumn.Name].Value);
					list.Add(groupInfo);
				}
			}
			if (list.Count == 0)
			{
				Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.SelectOneOrMoreRows]);
				return false;
			}
			try
			{
				foreach (GroupInfo item2 in list)
				{
					if (userInfoManager.GetUserByGroupID(item2.GroupID.ToString()).Count > 0)
					{
						Tool.ShowMessage(ResourceManagement.GetLocationString("GroupContainUser"));
						return false;
					}
					if (permitGroupManagementBLL.GetDataByGroupID(item2.GroupID).Count > 0)
					{
						Tool.ShowMessage(ResourceManagement.GetLocationString("GroupHasSetPermit"));
						return false;
					}
					string text2 = text;
					text = text2 + "[" + item2.GroupID + "][" + item2.GroupName + "]";
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
				return false;
			}
			DialogResult dialogResult = Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.ConfirmDelete], MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (dialogResult == DialogResult.Yes)
			{
				GroupInfoBLL groupInfoBLL = new GroupInfoBLL();
				try
				{
					groupInfoBLL.DeleteWithTrans(list);
					Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.DeleteSucceed]);
					logBLL.AddLog(EnumFunctionName.SysFuncUSERGROUP, EnumOSName.Delete, text);
				}
				catch (Exception ex)
				{
					Tool.ShowMessage(ex);
				}
				BindDataGridView();
			}
			return true;
		}

		protected override void FuncEdit()
		{
			DataGridViewRow currentRow = dgvGroupInfo.CurrentRow;
			if (currentRow == null)
			{
				return;
			}
			UserInfoManager userInfoManager = new UserInfoManager();
			string text = currentRow.Cells[groupIDDataGridViewTextBoxColumn.Name].Value.ToString();
			UserInfo userInfo = new UserInfo();
			try
			{
				userInfo = userInfoManager.GetUserByID("Admin");
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
				return;
			}
			if (text.Equals(userInfo.GroupID))
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("CannotEditGroupOfAdmin"));
				return;
			}
			FormGroupInfoDetail formGroupInfoDetail = new FormGroupInfoDetail(Convert.ToInt16(currentRow.Cells[groupIDDataGridViewTextBoxColumn.Name].Value), PageEnum.Edit);
			formGroupInfoDetail.Owner = this;
			formGroupInfoDetail.StartPosition = FormStartPosition.CenterParent;
			if (formGroupInfoDetail.ShowDialog() == DialogResult.OK)
			{
				BindDataGridView();
			}
		}

		private void dgvGroupInfo_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
			{
				DataGridViewRow dataGridViewRow = dgvGroupInfo.Rows[e.RowIndex];
				int @int = Tool.GetInt32(dataGridViewRow.Cells[groupIDDataGridViewTextBoxColumn.Name].Value);
				FormGroupInfoDetail formGroupInfoDetail = new FormGroupInfoDetail(@int, PageEnum.Show);
				formGroupInfoDetail.Owner = this;
				formGroupInfoDetail.ShowDialog();
			}
		}

		private void dgvGroupInfo_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex == 0 && e.RowIndex >= 0 && dgvGroupInfo.Rows.Count != 0)
			{
				DataGridViewRow dataGridViewRow = dgvGroupInfo.Rows[e.RowIndex];
				DataGridViewCell dataGridViewCell = dataGridViewRow.Cells[SelectDataGridViewTextBoxColumn.Name];
				if (Tool.GetBoolean(dataGridViewCell.Value))
				{
					dataGridViewCell.Value = false;
				}
				else
				{
					dataGridViewCell.Value = true;
				}
			}
		}

		private void dgvGroupInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
		}

		private void dgvGroupInfo_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == dgvGroupInfo.Columns[colGroupDetail.Name].Index)
			{
				DataGridViewRow dataGridViewRow = dgvGroupInfo.Rows[e.RowIndex];
				int @int = Tool.GetInt32(dataGridViewRow.Cells[groupIDDataGridViewTextBoxColumn.Name].Value);
				string groupName = dataGridViewRow.Cells[groupNameDataGridViewTextBoxColumn.Name].Value.ToString();
				GroupInfo groupInfo = new GroupInfo();
				groupInfo.GroupID = @int;
				groupInfo.GroupName = groupName;
				FormGroupMembers formGroupMembers = new FormGroupMembers(groupInfo);
				formGroupMembers.ShowDialog();
			}
		}

		private void tsmSelectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvGroupInfo.Rows)
			{
				item.Cells[SelectDataGridViewTextBoxColumn.Name].Value = true;
			}
		}

		private void tsmDeselectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvGroupInfo.Rows)
			{
				item.Cells[SelectDataGridViewTextBoxColumn.Name].Value = false;
			}
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.UserManagement.FormUserGroup));
			this.dgvGroupInfo = new Com.Cn.Tool.UserControls.DGV();
			this.SelectDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.groupIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.groupNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.statusDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.creatorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.createTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.lastEditorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.lastEditTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colGroupDetail = new System.Windows.Forms.DataGridViewButtonColumn();
			this.cmsSelect = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
			base.pnlBase.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvGroupInfo).BeginInit();
			this.cmsSelect.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(base.pnlBase, "pnlBase");
			base.pnlBase.Controls.Add(this.dgvGroupInfo);
			resources.ApplyResources(this.dgvGroupInfo, "dgvGroupInfo");
			this.dgvGroupInfo.AllowUserToAddRows = false;
			this.dgvGroupInfo.AllowUserToDeleteRows = false;
			this.dgvGroupInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvGroupInfo.Columns.AddRange(this.SelectDataGridViewTextBoxColumn, this.groupIDDataGridViewTextBoxColumn, this.groupNameDataGridViewTextBoxColumn, this.statusDataGridViewCheckBoxColumn, this.creatorDataGridViewTextBoxColumn, this.createTimeDataGridViewTextBoxColumn, this.lastEditorDataGridViewTextBoxColumn, this.lastEditTimeDataGridViewTextBoxColumn, this.colGroupDetail);
			this.dgvGroupInfo.ContextMenuStrip = this.cmsSelect;
			this.dgvGroupInfo.Name = "dgvGroupInfo";
			this.dgvGroupInfo.ReadOnly = true;
			this.dgvGroupInfo.RowTemplate.Height = 23;
			this.dgvGroupInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvGroupInfo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvGroupInfo_CellClick);
			this.dgvGroupInfo.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvGroupInfo_CellMouseClick);
			this.dgvGroupInfo.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvGroupInfo_CellMouseDoubleClick);
			resources.ApplyResources(this.SelectDataGridViewTextBoxColumn, "SelectDataGridViewTextBoxColumn");
			this.SelectDataGridViewTextBoxColumn.Name = "SelectDataGridViewTextBoxColumn";
			this.SelectDataGridViewTextBoxColumn.ReadOnly = true;
			this.groupIDDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.groupIDDataGridViewTextBoxColumn.DataPropertyName = "groupID";
			resources.ApplyResources(this.groupIDDataGridViewTextBoxColumn, "groupIDDataGridViewTextBoxColumn");
			this.groupIDDataGridViewTextBoxColumn.Name = "groupIDDataGridViewTextBoxColumn";
			this.groupIDDataGridViewTextBoxColumn.ReadOnly = true;
			this.groupNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.groupNameDataGridViewTextBoxColumn.DataPropertyName = "groupName";
			resources.ApplyResources(this.groupNameDataGridViewTextBoxColumn, "groupNameDataGridViewTextBoxColumn");
			this.groupNameDataGridViewTextBoxColumn.Name = "groupNameDataGridViewTextBoxColumn";
			this.groupNameDataGridViewTextBoxColumn.ReadOnly = true;
			this.statusDataGridViewCheckBoxColumn.DataPropertyName = "status";
			resources.ApplyResources(this.statusDataGridViewCheckBoxColumn, "statusDataGridViewCheckBoxColumn");
			this.statusDataGridViewCheckBoxColumn.Name = "statusDataGridViewCheckBoxColumn";
			this.statusDataGridViewCheckBoxColumn.ReadOnly = true;
			this.creatorDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.creatorDataGridViewTextBoxColumn.DataPropertyName = "creator";
			resources.ApplyResources(this.creatorDataGridViewTextBoxColumn, "creatorDataGridViewTextBoxColumn");
			this.creatorDataGridViewTextBoxColumn.Name = "creatorDataGridViewTextBoxColumn";
			this.creatorDataGridViewTextBoxColumn.ReadOnly = true;
			this.createTimeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.createTimeDataGridViewTextBoxColumn.DataPropertyName = "createTime";
			resources.ApplyResources(this.createTimeDataGridViewTextBoxColumn, "createTimeDataGridViewTextBoxColumn");
			this.createTimeDataGridViewTextBoxColumn.Name = "createTimeDataGridViewTextBoxColumn";
			this.createTimeDataGridViewTextBoxColumn.ReadOnly = true;
			this.lastEditorDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.lastEditorDataGridViewTextBoxColumn.DataPropertyName = "lastEditor";
			resources.ApplyResources(this.lastEditorDataGridViewTextBoxColumn, "lastEditorDataGridViewTextBoxColumn");
			this.lastEditorDataGridViewTextBoxColumn.Name = "lastEditorDataGridViewTextBoxColumn";
			this.lastEditorDataGridViewTextBoxColumn.ReadOnly = true;
			this.lastEditTimeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.lastEditTimeDataGridViewTextBoxColumn.DataPropertyName = "lastEditTime";
			resources.ApplyResources(this.lastEditTimeDataGridViewTextBoxColumn, "lastEditTimeDataGridViewTextBoxColumn");
			this.lastEditTimeDataGridViewTextBoxColumn.Name = "lastEditTimeDataGridViewTextBoxColumn";
			this.lastEditTimeDataGridViewTextBoxColumn.ReadOnly = true;
			this.colGroupDetail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			resources.ApplyResources(this.colGroupDetail, "colGroupDetail");
			this.colGroupDetail.Name = "colGroupDetail";
			this.colGroupDetail.ReadOnly = true;
			this.colGroupDetail.Text = "Look Over";
			this.colGroupDetail.UseColumnTextForButtonValue = true;
			resources.ApplyResources(this.cmsSelect, "cmsSelect");
			this.cmsSelect.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmSelectAll, this.tsmDeselectAll });
			this.cmsSelect.Name = "cmsSelectEITFile";
			resources.ApplyResources(this.tsmSelectAll, "tsmSelectAll");
			this.tsmSelectAll.Name = "tsmSelectAll";
			this.tsmSelectAll.Click += new System.EventHandler(tsmSelectAll_Click);
			resources.ApplyResources(this.tsmDeselectAll, "tsmDeselectAll");
			this.tsmDeselectAll.Name = "tsmDeselectAll";
			this.tsmDeselectAll.Click += new System.EventHandler(tsmDeselectAll_Click);
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormUserGroup";
			base.Controls.SetChildIndex(base.pnlBase, 0);
			base.pnlBase.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dgvGroupInfo).EndInit();
			this.cmsSelect.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
