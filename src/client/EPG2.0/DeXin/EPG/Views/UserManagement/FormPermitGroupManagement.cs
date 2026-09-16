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
	public class FormPermitGroupManagement : FormToolBase
	{
		private IContainer components = null;

		private DGV dgvPermitGroup;

		private DataGridViewCheckBoxColumn SelectCK;

		private DataGridViewTextBoxColumn groupIDDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn GroupNameDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn functionIDDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn colFunctionName;

		private DataGridViewCheckBoxColumn canQueryDataGridViewCheckBoxColumn;

		private DataGridViewCheckBoxColumn canAddDataGridViewCheckBoxColumn;

		private DataGridViewCheckBoxColumn canDeleteDataGridViewCheckBoxColumn;

		private DataGridViewCheckBoxColumn canEditDataGridViewCheckBoxColumn;

		private DataGridViewCheckBoxColumn canPrintDataGridViewCheckBoxColumn;

		private DataGridViewTextBoxColumn creatorDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn createTimeDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn lastEditorDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn lastEditTimeDataGridViewTextBoxColumn;

		private ContextMenuStrip cmsSelect;

		private ToolStripMenuItem tsmSelectAll;

		private ToolStripMenuItem tsmDeselectAll;

		public IList<PermitGroup> permits = new List<PermitGroup>();

		public PagingSearch pagingSearch = new PagingSearch();

		private PermitGroupManagementBLL PermitBLL = new PermitGroupManagementBLL();

		private UserInfoManager userBLL = new UserInfoManager();

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.UserManagement.FormPermitGroupManagement));
			this.dgvPermitGroup = new Com.Cn.Tool.UserControls.DGV();
			this.SelectCK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.groupIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.GroupNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.functionIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colFunctionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.canQueryDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.canAddDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.canDeleteDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.canEditDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.canPrintDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.creatorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.createTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.lastEditorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.lastEditTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cmsSelect = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
			base.pnlBase.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvPermitGroup).BeginInit();
			this.cmsSelect.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(base.pnlBase, "pnlBase");
			base.pnlBase.Controls.Add(this.dgvPermitGroup);
			resources.ApplyResources(this.dgvPermitGroup, "dgvPermitGroup");
			this.dgvPermitGroup.AllowUserToAddRows = false;
			this.dgvPermitGroup.AllowUserToDeleteRows = false;
			this.dgvPermitGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvPermitGroup.Columns.AddRange(this.SelectCK, this.groupIDDataGridViewTextBoxColumn, this.GroupNameDataGridViewTextBoxColumn, this.functionIDDataGridViewTextBoxColumn, this.colFunctionName, this.canQueryDataGridViewCheckBoxColumn, this.canAddDataGridViewCheckBoxColumn, this.canDeleteDataGridViewCheckBoxColumn, this.canEditDataGridViewCheckBoxColumn, this.canPrintDataGridViewCheckBoxColumn, this.creatorDataGridViewTextBoxColumn, this.createTimeDataGridViewTextBoxColumn, this.lastEditorDataGridViewTextBoxColumn, this.lastEditTimeDataGridViewTextBoxColumn);
			this.dgvPermitGroup.ContextMenuStrip = this.cmsSelect;
			this.dgvPermitGroup.Name = "dgvPermitGroup";
			this.dgvPermitGroup.ReadOnly = true;
			this.dgvPermitGroup.RowTemplate.Height = 23;
			this.dgvPermitGroup.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvPermitGroup.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvPermitGroup_CellMouseClick);
			this.dgvPermitGroup.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvPermitGroup_CellMouseDoubleClick);
			this.dgvPermitGroup.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(dgvPermitGroup_DataError);
			resources.ApplyResources(this.SelectCK, "SelectCK");
			this.SelectCK.Name = "SelectCK";
			this.SelectCK.ReadOnly = true;
			this.SelectCK.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.SelectCK.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.groupIDDataGridViewTextBoxColumn.DataPropertyName = "GroupID";
			resources.ApplyResources(this.groupIDDataGridViewTextBoxColumn, "groupIDDataGridViewTextBoxColumn");
			this.groupIDDataGridViewTextBoxColumn.Name = "groupIDDataGridViewTextBoxColumn";
			this.groupIDDataGridViewTextBoxColumn.ReadOnly = true;
			this.GroupNameDataGridViewTextBoxColumn.DataPropertyName = "GroupName";
			resources.ApplyResources(this.GroupNameDataGridViewTextBoxColumn, "GroupNameDataGridViewTextBoxColumn");
			this.GroupNameDataGridViewTextBoxColumn.Name = "GroupNameDataGridViewTextBoxColumn";
			this.GroupNameDataGridViewTextBoxColumn.ReadOnly = true;
			this.functionIDDataGridViewTextBoxColumn.DataPropertyName = "FunctionID";
			resources.ApplyResources(this.functionIDDataGridViewTextBoxColumn, "functionIDDataGridViewTextBoxColumn");
			this.functionIDDataGridViewTextBoxColumn.Name = "functionIDDataGridViewTextBoxColumn";
			this.functionIDDataGridViewTextBoxColumn.ReadOnly = true;
			this.colFunctionName.DataPropertyName = "LocationFunctionName";
			resources.ApplyResources(this.colFunctionName, "colFunctionName");
			this.colFunctionName.Name = "colFunctionName";
			this.colFunctionName.ReadOnly = true;
			this.canQueryDataGridViewCheckBoxColumn.DataPropertyName = "CanQuery";
			resources.ApplyResources(this.canQueryDataGridViewCheckBoxColumn, "canQueryDataGridViewCheckBoxColumn");
			this.canQueryDataGridViewCheckBoxColumn.Name = "canQueryDataGridViewCheckBoxColumn";
			this.canQueryDataGridViewCheckBoxColumn.ReadOnly = true;
			this.canAddDataGridViewCheckBoxColumn.DataPropertyName = "CanAdd";
			resources.ApplyResources(this.canAddDataGridViewCheckBoxColumn, "canAddDataGridViewCheckBoxColumn");
			this.canAddDataGridViewCheckBoxColumn.Name = "canAddDataGridViewCheckBoxColumn";
			this.canAddDataGridViewCheckBoxColumn.ReadOnly = true;
			this.canDeleteDataGridViewCheckBoxColumn.DataPropertyName = "CanDelete";
			resources.ApplyResources(this.canDeleteDataGridViewCheckBoxColumn, "canDeleteDataGridViewCheckBoxColumn");
			this.canDeleteDataGridViewCheckBoxColumn.Name = "canDeleteDataGridViewCheckBoxColumn";
			this.canDeleteDataGridViewCheckBoxColumn.ReadOnly = true;
			this.canEditDataGridViewCheckBoxColumn.DataPropertyName = "CanEdit";
			resources.ApplyResources(this.canEditDataGridViewCheckBoxColumn, "canEditDataGridViewCheckBoxColumn");
			this.canEditDataGridViewCheckBoxColumn.Name = "canEditDataGridViewCheckBoxColumn";
			this.canEditDataGridViewCheckBoxColumn.ReadOnly = true;
			this.canPrintDataGridViewCheckBoxColumn.DataPropertyName = "CanPrint";
			resources.ApplyResources(this.canPrintDataGridViewCheckBoxColumn, "canPrintDataGridViewCheckBoxColumn");
			this.canPrintDataGridViewCheckBoxColumn.Name = "canPrintDataGridViewCheckBoxColumn";
			this.canPrintDataGridViewCheckBoxColumn.ReadOnly = true;
			this.creatorDataGridViewTextBoxColumn.DataPropertyName = "Creator";
			resources.ApplyResources(this.creatorDataGridViewTextBoxColumn, "creatorDataGridViewTextBoxColumn");
			this.creatorDataGridViewTextBoxColumn.Name = "creatorDataGridViewTextBoxColumn";
			this.creatorDataGridViewTextBoxColumn.ReadOnly = true;
			this.createTimeDataGridViewTextBoxColumn.DataPropertyName = "CreateTime";
			resources.ApplyResources(this.createTimeDataGridViewTextBoxColumn, "createTimeDataGridViewTextBoxColumn");
			this.createTimeDataGridViewTextBoxColumn.Name = "createTimeDataGridViewTextBoxColumn";
			this.createTimeDataGridViewTextBoxColumn.ReadOnly = true;
			this.lastEditorDataGridViewTextBoxColumn.DataPropertyName = "LastEditor";
			resources.ApplyResources(this.lastEditorDataGridViewTextBoxColumn, "lastEditorDataGridViewTextBoxColumn");
			this.lastEditorDataGridViewTextBoxColumn.Name = "lastEditorDataGridViewTextBoxColumn";
			this.lastEditorDataGridViewTextBoxColumn.ReadOnly = true;
			this.lastEditTimeDataGridViewTextBoxColumn.DataPropertyName = "LastEditTime";
			resources.ApplyResources(this.lastEditTimeDataGridViewTextBoxColumn, "lastEditTimeDataGridViewTextBoxColumn");
			this.lastEditTimeDataGridViewTextBoxColumn.Name = "lastEditTimeDataGridViewTextBoxColumn";
			this.lastEditTimeDataGridViewTextBoxColumn.ReadOnly = true;
			resources.ApplyResources(this.cmsSelect, "cmsSelect");
			this.cmsSelect.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmSelectAll, this.tsmDeselectAll });
			this.cmsSelect.Name = "cmsSelect";
			resources.ApplyResources(this.tsmSelectAll, "tsmSelectAll");
			this.tsmSelectAll.Name = "tsmSelectAll";
			this.tsmSelectAll.Click += new System.EventHandler(tsmSelectAll_Click);
			resources.ApplyResources(this.tsmDeselectAll, "tsmDeselectAll");
			this.tsmDeselectAll.Name = "tsmDeselectAll";
			this.tsmDeselectAll.Click += new System.EventHandler(tsmDeselectAll_Click);
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormPermitGroupManagement";
			base.Controls.SetChildIndex(base.pnlBase, 0);
			base.pnlBase.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dgvPermitGroup).EndInit();
			this.cmsSelect.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormPermitGroupManagement()
		{
			InitializeComponent();
			_FunctionName = EnumFunctionName.SysFuncPermitGroupManagemnent;
		}

		protected override void FormLoad()
		{
			SetMenuVisible(false, true, true, true, false, false, false, true, false);
			Bind();
		}

		private void Bind()
		{
			PrepareQueryByPaging();
			BindDataGridView(permits);
		}

		private void BindDataGridView(IList<PermitGroup> permits)
		{
			dgvPermitGroup.DataSource = permits;
		}

		protected override void FuncAdd()
		{
			FormPermitGroupManagementAdd formPermitGroupManagementAdd = new FormPermitGroupManagementAdd();
			formPermitGroupManagementAdd.Owner = this;
			formPermitGroupManagementAdd.StartPosition = FormStartPosition.CenterParent;
			if (formPermitGroupManagementAdd.ShowDialog() == DialogResult.OK)
			{
				Bind();
			}
		}

		protected override void FuncEdit()
		{
			DataGridViewRow currentRow = dgvPermitGroup.CurrentRow;
			if (currentRow == null)
			{
				return;
			}
			string text = currentRow.Cells[groupIDDataGridViewTextBoxColumn.Name].Value.ToString();
			UserInfo userInfo = new UserInfo();
			try
			{
				userInfo = userBLL.GetUserByID("Admin");
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
				return;
			}
			if (text.Equals(userInfo.GroupID))
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("CannotEditPermitOfAdmin"));
				return;
			}
			FormPermitGroupManagementDetail formPermitGroupManagementDetail = new FormPermitGroupManagementDetail(Convert.ToInt16(currentRow.Cells[groupIDDataGridViewTextBoxColumn.Name].Value), currentRow.Cells[functionIDDataGridViewTextBoxColumn.Name].Value.ToString(), PageEnum.Edit);
			formPermitGroupManagementDetail.Owner = this;
			formPermitGroupManagementDetail.StartPosition = FormStartPosition.CenterParent;
			if (formPermitGroupManagementDetail.ShowDialog() == DialogResult.OK)
			{
				Bind();
			}
		}

		protected override bool FuncDelete()
		{
			string text = "";
			List<PermitGroup> list = new List<PermitGroup>();
			foreach (DataGridViewRow item in (IEnumerable)dgvPermitGroup.Rows)
			{
				if (Tool.GetBoolean(item.Cells[SelectCK.Name].Value))
				{
					PermitGroup permitGroup = new PermitGroup();
					permitGroup.GroupID = Tool.GetInt32(item.Cells[groupIDDataGridViewTextBoxColumn.Name].Value);
					permitGroup.FunctionID = item.Cells[functionIDDataGridViewTextBoxColumn.Name].Value.ToString();
					list.Add(permitGroup);
					text = text + permitGroup.GroupID + ",";
				}
			}
			if (list.Count == 0)
			{
				Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.SelectOneOrMoreRows]);
				return false;
			}
			foreach (PermitGroup item2 in list)
			{
				string text2 = item2.GroupID.ToString();
				UserInfo userInfo = new UserInfo();
				try
				{
					userInfo = userBLL.GetUserByID("Admin");
				}
				catch (Exception ex)
				{
					Tool.ShowMessage(ex);
					return false;
				}
				if (text2.Equals(userInfo.GroupID))
				{
					Tool.ShowMessage(ResourceManagement.GetLocationString("CannotDeletePermitOfAdmin"));
					return false;
				}
			}
			DialogResult dialogResult = Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.ConfirmDelete], MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (dialogResult == DialogResult.Yes)
			{
				try
				{
					PermitBLL.DeleteWithSqlTrans(list);
					Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.DeleteSucceed]);
					LogBLL logBLL = new LogBLL();
					logBLL.AddLog(EnumFunctionName.SysFuncPermitGroupManagemnent, EnumOSName.Delete, text);
				}
				catch (Exception ex)
				{
					Tool.ShowMessage(ex);
				}
				Bind();
			}
			return true;
		}

		protected override void FuncQuery()
		{
		}

		private void PrepareQueryByPaging()
		{
			try
			{
				pagingSearch.PKName = "GroupID,FunctionID";
				pagingSearch.TableName = "V_PermitGroupManagement ";
				pagingSearch.Condition = PrepareCondition();
				pagingSearch.OrderBy = "GroupID,FunctionID";
				pagingSearch.ColumsNames = " * ";
				pagingSearch.PageIndex = 0;
				pagingSearch.PageSize = 0;
				pagingSearch.TotalCount = 0;
				permits = PermitBLL.GetPermitGroupDataByPaging(pagingSearch);
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private string PrepareCondition()
		{
			return "";
		}

		private void dgvPermitGroup_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
			{
				DataGridViewRow dataGridViewRow = dgvPermitGroup.Rows[e.RowIndex];
				int @int = Tool.GetInt32(dataGridViewRow.Cells[groupIDDataGridViewTextBoxColumn.Name].Value);
				string functionID = dataGridViewRow.Cells[functionIDDataGridViewTextBoxColumn.Name].Value.ToString();
				FormPermitGroupManagementDetail formPermitGroupManagementDetail = new FormPermitGroupManagementDetail(@int, functionID, PageEnum.Show);
				formPermitGroupManagementDetail.ShowDialog();
			}
		}

		private void dgvPermitGroup_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex == 0 && e.RowIndex >= 0 && dgvPermitGroup.Rows.Count != 0)
			{
				DataGridViewRow dataGridViewRow = dgvPermitGroup.Rows[e.RowIndex];
				DataGridViewCell dataGridViewCell = dataGridViewRow.Cells[SelectCK.Name];
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

		private void EPGPagination1_PageIndexChanged(object sender, EventArgs e)
		{
			Bind();
		}

		private void dgvPermitGroup_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
		}

		private void tsmSelectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvPermitGroup.Rows)
			{
				item.Cells[SelectCK.Name].Value = true;
			}
		}

		private void tsmDeselectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvPermitGroup.Rows)
			{
				item.Cells[SelectCK.Name].Value = false;
			}
		}
	}
}
