using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using Com.Cn.Tool.UserControls;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.SendControlManagement
{
	public class FormPMTPIDReMapping : FormToolBase
	{
		private IContainer components = null;

		private DGV dgvPMTPIDRemapping;

		private TableLayoutPanel tableLayoutPanel1;

		private ComboBox cmbTS;

		private Label lblNetwork;

		private ComboBox cmbNetwork;

		private Label lblTSTableID;

		private ContextMenuStrip cmsSelect;

		private ToolStripMenuItem tsmSelectAll;

		private ToolStripMenuItem tsmDeselectAll;

		private DataGridViewCheckBoxColumn colSelect;

		private DataGridViewTextBoxColumn colID;

		private DataGridViewTextBoxColumn colProgramTableID;

		private DataGridViewTextBoxColumn colOutputPID;

		private LogBLL logBLL = new LogBLL();

		private PMTPIDReMappingBLL pmtPIDReMappingBLL = new PMTPIDReMappingBLL();

		private TSInfoManager tsInfoBLL = new TSInfoManager();

		private NITInfoManager nitBLL = new NITInfoManager();

		private ProgramInfoManager prgBLL = new ProgramInfoManager();

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.SendControlManagement.FormPMTPIDReMapping));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
			this.dgvPMTPIDRemapping = new Com.Cn.Tool.UserControls.DGV();
			this.cmsSelect = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.cmbTS = new System.Windows.Forms.ComboBox();
			this.lblNetwork = new System.Windows.Forms.Label();
			this.cmbNetwork = new System.Windows.Forms.ComboBox();
			this.lblTSTableID = new System.Windows.Forms.Label();
			this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colProgramTableID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colOutputPID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			base.pnlBase.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvPMTPIDRemapping).BeginInit();
			this.cmsSelect.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.pnlBase.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.pnlBase, "pnlBase");
			this.dgvPMTPIDRemapping.AllowUserToAddRows = false;
			this.dgvPMTPIDRemapping.AllowUserToDeleteRows = false;
			this.dgvPMTPIDRemapping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvPMTPIDRemapping.Columns.AddRange(this.colSelect, this.colID, this.colProgramTableID, this.colOutputPID);
			this.tableLayoutPanel1.SetColumnSpan(this.dgvPMTPIDRemapping, 4);
			this.dgvPMTPIDRemapping.ContextMenuStrip = this.cmsSelect;
			resources.ApplyResources(this.dgvPMTPIDRemapping, "dgvPMTPIDRemapping");
			this.dgvPMTPIDRemapping.Name = "dgvPMTPIDRemapping";
			this.dgvPMTPIDRemapping.ReadOnly = true;
			this.dgvPMTPIDRemapping.RowTemplate.Height = 23;
			this.dgvPMTPIDRemapping.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvPMTPIDRemapping.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvMultiIPOutPut_CellMouseClick);
			this.dgvPMTPIDRemapping.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvMultiIPOutPut_CellMouseDoubleClick);
			this.cmsSelect.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmSelectAll, this.tsmDeselectAll });
			this.cmsSelect.Name = "cmsSelect";
			resources.ApplyResources(this.cmsSelect, "cmsSelect");
			this.tsmSelectAll.Name = "tsmSelectAll";
			resources.ApplyResources(this.tsmSelectAll, "tsmSelectAll");
			this.tsmSelectAll.Click += new System.EventHandler(tsmSelectAll_Click);
			this.tsmDeselectAll.Name = "tsmDeselectAll";
			resources.ApplyResources(this.tsmDeselectAll, "tsmDeselectAll");
			this.tsmDeselectAll.Click += new System.EventHandler(tsmDeselectAll_Click);
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.cmbTS, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.dgvPMTPIDRemapping, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblNetwork, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.cmbNetwork, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblTSTableID, 2, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.cmbTS, "cmbTS");
			this.cmbTS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTS.FormattingEnabled = true;
			this.cmbTS.Name = "cmbTS";
			this.cmbTS.SelectedIndexChanged += new System.EventHandler(cmbTS_SelectedIndexChanged);
			resources.ApplyResources(this.lblNetwork, "lblNetwork");
			this.lblNetwork.Name = "lblNetwork";
			resources.ApplyResources(this.cmbNetwork, "cmbNetwork");
			this.cmbNetwork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbNetwork.FormattingEnabled = true;
			this.cmbNetwork.Name = "cmbNetwork";
			this.cmbNetwork.SelectedIndexChanged += new System.EventHandler(cmbNetwork_SelectedIndexChanged);
			resources.ApplyResources(this.lblTSTableID, "lblTSTableID");
			this.lblTSTableID.Name = "lblTSTableID";
			resources.ApplyResources(this.colSelect, "colSelect");
			this.colSelect.Name = "colSelect";
			this.colSelect.ReadOnly = true;
			this.colID.DataPropertyName = "ID";
			resources.ApplyResources(this.colID, "colID");
			this.colID.Name = "colID";
			this.colID.ReadOnly = true;
			this.colProgramTableID.DataPropertyName = "Program_ProgramName";
			resources.ApplyResources(this.colProgramTableID, "colProgramTableID");
			this.colProgramTableID.Name = "colProgramTableID";
			this.colProgramTableID.ReadOnly = true;
			this.colProgramTableID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colProgramTableID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colOutputPID.DataPropertyName = "OutputPid";
			dataGridViewCellStyle.Format = "X4";
			this.colOutputPID.DefaultCellStyle = dataGridViewCellStyle;
			resources.ApplyResources(this.colOutputPID, "colOutputPID");
			this.colOutputPID.Name = "colOutputPID";
			this.colOutputPID.ReadOnly = true;
			this.colOutputPID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colOutputPID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormPMTPIDReMapping";
			base.Controls.SetChildIndex(base.pnlBase, 0);
			base.pnlBase.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dgvPMTPIDRemapping).EndInit();
			this.cmsSelect.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormPMTPIDReMapping()
		{
			InitializeComponent();
			_FunctionName = EnumFunctionName.SysFuncPMTPIDReMapping;
		}

		protected override void FormLoad()
		{
			SetMenuVisible(false, true, true, true, false, false, false, true, false);
			BindComboNetwork();
			BindComboTS(Tool.GetInt64(cmbNetwork.SelectedValue));
			BindComboProgram(Tool.GetInt64(cmbTS.SelectedValue));
			BindDataGrid(Tool.GetInt64(cmbTS.SelectedValue));
		}

		protected override void FuncAdd()
		{
			if (cmbTS.SelectedValue == null)
			{
				Tool.ShowMessage(CommTool.ShowInputFormatError(lblTSTableID));
				return;
			}
			IList<PmtPidOutputInfo> controlled = dgvPMTPIDRemapping.DataSource as IList<PmtPidOutputInfo>;
			IList<ProgramInfo> byTSIdentity = prgBLL.GetByTSIdentity(Tool.GetInt64(cmbTS.SelectedValue));
			if (controlled.Count == byTSIdentity.Count)
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("CannotAddAnyMore"));
				return;
			}
			IEnumerable<ProgramInfo> source = byTSIdentity.Where((ProgramInfo x) => !controlled.Select((PmtPidOutputInfo m) => m.ProgramTableId).Contains(x.Identity));
			FormPMTPIDReMappingDetail formPMTPIDReMappingDetail = new FormPMTPIDReMappingDetail(PageEnum.Add, Tool.GetInt64(cmbTS.SelectedValue), source.ToList());
			if (formPMTPIDReMappingDetail.ShowDialog() == DialogResult.OK)
			{
				BindDataGrid(Tool.GetInt64(cmbTS.SelectedValue));
			}
		}

		protected override void FuncEdit()
		{
			if (cmbTS.SelectedValue == null)
			{
				Tool.ShowMessage(CommTool.ShowInputFormatError(lblTSTableID));
				return;
			}
			DataGridViewRow currentRow = dgvPMTPIDRemapping.CurrentRow;
			if (currentRow != null)
			{
				long @int = Tool.GetInt64(currentRow.Cells[colID.Name].Value);
				FormPMTPIDReMappingDetail formPMTPIDReMappingDetail = new FormPMTPIDReMappingDetail(PageEnum.Edit, @int, Tool.GetInt64(cmbTS.SelectedValue));
				if (formPMTPIDReMappingDetail.ShowDialog() == DialogResult.OK)
				{
					BindDataGrid(Tool.GetInt64(cmbTS.SelectedValue));
				}
			}
		}

		protected override bool FuncDelete()
		{
			string text = "";
			bool result = false;
			List<PmtPidOutputInfo> list = new List<PmtPidOutputInfo>();
			foreach (DataGridViewRow item in (IEnumerable)dgvPMTPIDRemapping.Rows)
			{
				if (Tool.GetBoolean(item.Cells[colSelect.Name].Value))
				{
					PmtPidOutputInfo pmtPidOutputInfo = new PmtPidOutputInfo();
					pmtPidOutputInfo.Id = Tool.GetInt32(item.Cells[colID.Name].Value);
					list.Add(pmtPidOutputInfo);
					text = text + "id:" + pmtPidOutputInfo.Id;
				}
			}
			if (list.Count == 0)
			{
				Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.SelectOneOrMoreRows]);
				return false;
			}
			DialogResult dialogResult = Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.ConfirmDelete], MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (dialogResult == DialogResult.Yes)
			{
				pmtPIDReMappingBLL.Delete(list);
				logBLL.AddLog(EnumFunctionName.SysFuncPMTPIDReMapping, EnumOSName.Delete, text);
				BindDataGrid(Tool.GetInt64(cmbTS.SelectedValue));
				result = true;
			}
			return result;
		}

		private void dgvMultiIPOutPut_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex <= 0 && e.RowIndex >= 0 && dgvPMTPIDRemapping.RowCount != 0)
			{
				DataGridViewRow dataGridViewRow = dgvPMTPIDRemapping.Rows[e.RowIndex];
				DataGridViewCell dataGridViewCell = dataGridViewRow.Cells[0];
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

		private void dgvMultiIPOutPut_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
			{
				DataGridViewRow dataGridViewRow = dgvPMTPIDRemapping.Rows[e.RowIndex];
				long @int = Tool.GetInt64(dataGridViewRow.Cells[colID.Name].Value);
				FormPMTPIDReMappingDetail formPMTPIDReMappingDetail = new FormPMTPIDReMappingDetail(PageEnum.Show, @int, Tool.GetInt64(cmbTS.SelectedValue));
				formPMTPIDReMappingDetail.ShowDialog();
			}
		}

		private void BindComboNetwork()
		{
			IList<BaseKey> lst = ((IEnumerable<BaseKey>)nitBLL.GetAllNITInfos()).ToList();
			Tool.BindingComboBox(lst, cmbNetwork);
		}

		private void BindComboTS(long networkid)
		{
			IList<TsInfo> tSInfoByNetWorkIdentity = tsInfoBLL.GetTSInfoByNetWorkIdentity(networkid);
			IList<BaseKey> lst = ((IEnumerable<BaseKey>)tSInfoByNetWorkIdentity).ToList();
			Tool.BindingComboBox(lst, cmbTS);
		}

		private void BindComboProgram(long tsTableid)
		{
		}

		private void BindDataGrid(long TSTableID)
		{
			dgvPMTPIDRemapping.DataSource = pmtPIDReMappingBLL.GetDataByTSTableID(TSTableID);
		}

		private void cmbTS_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindDataGrid(Tool.GetInt64(cmbTS.SelectedValue));
		}

		private void cmbNetwork_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindComboTS(Tool.GetInt64(cmbNetwork.SelectedValue));
			if (cmbTS.SelectedValue == null)
			{
				BindDataGrid(Tool.GetInt64(cmbTS.SelectedValue));
			}
		}

		private void tsmSelectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvPMTPIDRemapping.Rows)
			{
				item.Cells[colSelect.Name].Value = true;
			}
		}

		private void tsmDeselectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvPMTPIDRemapping.Rows)
			{
				item.Cells[colSelect.Name].Value = false;
			}
		}
	}
}
