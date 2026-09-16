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
	public class FormSendSpeed : FormToolBase
	{
		private SendSpeedInfoManager controlManager = new SendSpeedInfoManager();

		private IContainer components = null;

		private DGV dgvSendSpeeds;

		private TableLayoutPanel tableLayoutPanel1;

		private ComboBox cmbNIT;

		private Label lblTableType;

		private ContextMenuStrip cmsSelect;

		private ToolStripMenuItem tsmSelectAll;

		private ToolStripMenuItem tsmDeselectAll;

		private DataGridViewCheckBoxColumn dcSelectCK;

		private DataGridViewTextBoxColumn dcTSId;

		private DataGridViewTextBoxColumn dcTSName;

		private DataGridViewTextBoxColumn dcPAT;

		private DataGridViewTextBoxColumn dcCAT;

		private DataGridViewTextBoxColumn dcPMT;

		private DataGridViewTextBoxColumn dcBAT;

		private DataGridViewTextBoxColumn dcSDT_ACT;

		private DataGridViewTextBoxColumn dcSDT_OTH;

		private DataGridViewTextBoxColumn dcNIT_ACT;

		private DataGridViewTextBoxColumn dcNIT_OTH;

		private DataGridViewTextBoxColumn dcTDT;

		private DataGridViewTextBoxColumn dcEIT_ACT_PF;

		private DataGridViewTextBoxColumn dcEIT_ACT_SC;

		private DataGridViewTextBoxColumn dcEIT_OTH_PF;

		private DataGridViewTextBoxColumn dcEIT_OTH_SC;

		private DataGridViewTextBoxColumn dcTOT;

		private DataGridViewTextBoxColumn dcId;

		public FormSendSpeed()
		{
			InitializeComponent();
			_FunctionName = EnumFunctionName.SysFuncSendSpeedManagement;
			IList<NitInfo> allNITInfos = new NITInfoManager().GetAllNITInfos();
			IList<BaseKey> lst = ((IEnumerable<BaseKey>)allNITInfos).ToList();
			Tool.BindingComboBox(lst, cmbNIT);
			SetMenuVisible(false, true, true, true, false, false, false, true, false);
		}

		private void cmbNIT_SelectedIndexChanged(object sender, EventArgs e)
		{
			string value = Tool.GetValue(cmbNIT);
			IList<SendSpeedInfo> byNITIdentity = controlManager.GetByNITIdentity(Tool.GetInt64(value));
			dgvSendSpeeds.DataSource = byNITIdentity;
		}

		private void dgvSendSpeeds_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (dgvSendSpeeds.Rows.Count >= 1 && e.RowIndex >= 0)
			{
				DataGridViewRow dataGridViewRow = dgvSendSpeeds.Rows[e.RowIndex];
				long controlIdentity = Tool.GetInt32(dataGridViewRow.Cells[dcId.Name].Value);
				if (new FormSendSpeedDetail(controlIdentity, PageEnum.Show).ShowDialog() == DialogResult.OK)
				{
					cmbNIT_SelectedIndexChanged(sender, e);
				}
			}
		}

		protected override void FuncAdd()
		{
			base.FuncAdd();
			IList<TsInfo> tSInfoByNetWorkIdentity = new TSInfoManager().GetTSInfoByNetWorkIdentity(Tool.GetInt64(Tool.GetValue(cmbNIT)));
			IList<SendSpeedInfo> inner = dgvSendSpeeds.DataSource as IList<SendSpeedInfo>;
			IEnumerable<TsInfo> second = from b in tSInfoByNetWorkIdentity
				join a in inner on b.Identity equals a.TsTableId
				select b;
			IList<TsInfo> list = tSInfoByNetWorkIdentity.Except(second).ToList();
			if (list == null || list.Count == 0)
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("CannotAddAnyMore"));
			}
			else if (new FormSendSpeedDetail(list).ShowDialog() == DialogResult.OK)
			{
				cmbNIT_SelectedIndexChanged(null, null);
			}
		}

		protected override void FuncEdit()
		{
			base.FuncEdit();
			if (dgvSendSpeeds.Rows.Count >= 1)
			{
				DataGridViewRow currentRow = dgvSendSpeeds.CurrentRow;
				long controlIdentity = Tool.GetInt32(currentRow.Cells[dcId.Name].Value);
				if (new FormSendSpeedDetail(controlIdentity, PageEnum.Edit).ShowDialog() == DialogResult.OK)
				{
					cmbNIT_SelectedIndexChanged(null, null);
				}
			}
		}

		protected override bool FuncDelete()
		{
			List<long> list = new List<long>();
			foreach (DataGridViewRow item in (IEnumerable)dgvSendSpeeds.Rows)
			{
				if (Tool.GetBoolean(item.Cells[dcSelectCK.Name].Value))
				{
					list.Add(Tool.GetInt64(item.Cells[dcId.Name].Value));
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
				if (controlManager.Delete(list) == list.Count)
				{
					cmbNIT_SelectedIndexChanged(null, null);
					return true;
				}
				return false;
			}
			return false;
		}

		private void dgvSendSpeeds_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == dgvSendSpeeds.Columns[dcSelectCK.Name].Index && e.RowIndex >= 0 && dgvSendSpeeds.Rows.Count != 0)
			{
				DataGridViewRow dataGridViewRow = dgvSendSpeeds.Rows[e.RowIndex];
				DataGridViewCell dataGridViewCell = dataGridViewRow.Cells[dcSelectCK.Name];
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

		private void tsmSelectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvSendSpeeds.Rows)
			{
				item.Cells[dcSelectCK.Name].Value = true;
			}
		}

		private void tsmDeselectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvSendSpeeds.Rows)
			{
				item.Cells[dcSelectCK.Name].Value = false;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.SendControlManagement.FormSendSpeed));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.cmbNIT = new System.Windows.Forms.ComboBox();
			this.lblTableType = new System.Windows.Forms.Label();
			this.dgvSendSpeeds = new Com.Cn.Tool.UserControls.DGV();
			this.cmsSelect = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.dcSelectCK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.dcTSId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcTSName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcPAT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcCAT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcPMT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcBAT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcSDT_ACT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcSDT_OTH = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcNIT_ACT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcNIT_OTH = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcTDT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcEIT_ACT_PF = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcEIT_ACT_SC = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcEIT_OTH_PF = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcEIT_OTH_SC = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcTOT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			base.pnlBase.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvSendSpeeds).BeginInit();
			this.cmsSelect.SuspendLayout();
			base.SuspendLayout();
			base.pnlBase.Controls.Add(this.dgvSendSpeeds);
			base.pnlBase.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.pnlBase, "pnlBase");
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.cmbNIT, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblTableType, 1, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.cmbNIT, "cmbNIT");
			this.cmbNIT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbNIT.FormattingEnabled = true;
			this.cmbNIT.Name = "cmbNIT";
			this.cmbNIT.SelectedIndexChanged += new System.EventHandler(cmbNIT_SelectedIndexChanged);
			resources.ApplyResources(this.lblTableType, "lblTableType");
			this.lblTableType.Name = "lblTableType";
			this.dgvSendSpeeds.AllowUserToAddRows = false;
			this.dgvSendSpeeds.AllowUserToDeleteRows = false;
			this.dgvSendSpeeds.AllowUserToOrderColumns = true;
			this.dgvSendSpeeds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSendSpeeds.Columns.AddRange(this.dcSelectCK, this.dcTSId, this.dcTSName, this.dcPAT, this.dcCAT, this.dcPMT, this.dcBAT, this.dcSDT_ACT, this.dcSDT_OTH, this.dcNIT_ACT, this.dcNIT_OTH, this.dcTDT, this.dcEIT_ACT_PF, this.dcEIT_ACT_SC, this.dcEIT_OTH_PF, this.dcEIT_OTH_SC, this.dcTOT, this.dcId);
			this.dgvSendSpeeds.ContextMenuStrip = this.cmsSelect;
			resources.ApplyResources(this.dgvSendSpeeds, "dgvSendSpeeds");
			this.dgvSendSpeeds.Name = "dgvSendSpeeds";
			this.dgvSendSpeeds.ReadOnly = true;
			this.dgvSendSpeeds.RowTemplate.Height = 23;
			this.dgvSendSpeeds.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvSendSpeeds.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvSendSpeeds_CellClick);
			this.dgvSendSpeeds.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvSendSpeeds_CellMouseDoubleClick);
			this.cmsSelect.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmSelectAll, this.tsmDeselectAll });
			this.cmsSelect.Name = "cmsSelect";
			resources.ApplyResources(this.cmsSelect, "cmsSelect");
			this.tsmSelectAll.Name = "tsmSelectAll";
			resources.ApplyResources(this.tsmSelectAll, "tsmSelectAll");
			this.tsmSelectAll.Click += new System.EventHandler(tsmSelectAll_Click);
			this.tsmDeselectAll.Name = "tsmDeselectAll";
			resources.ApplyResources(this.tsmDeselectAll, "tsmDeselectAll");
			this.tsmDeselectAll.Click += new System.EventHandler(tsmDeselectAll_Click);
			resources.ApplyResources(this.dcSelectCK, "dcSelectCK");
			this.dcSelectCK.Name = "dcSelectCK";
			this.dcSelectCK.ReadOnly = true;
			this.dcSelectCK.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dcSelectCK.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dcTSId.DataPropertyName = "TsTableId";
			resources.ApplyResources(this.dcTSId, "dcTSId");
			this.dcTSId.Name = "dcTSId";
			this.dcTSId.ReadOnly = true;
			this.dcTSName.DataPropertyName = "TS_TsName";
			resources.ApplyResources(this.dcTSName, "dcTSName");
			this.dcTSName.Name = "dcTSName";
			this.dcTSName.ReadOnly = true;
			this.dcPAT.DataPropertyName = "Pat";
			resources.ApplyResources(this.dcPAT, "dcPAT");
			this.dcPAT.Name = "dcPAT";
			this.dcPAT.ReadOnly = true;
			this.dcCAT.DataPropertyName = "Cat";
			resources.ApplyResources(this.dcCAT, "dcCAT");
			this.dcCAT.Name = "dcCAT";
			this.dcCAT.ReadOnly = true;
			this.dcPMT.DataPropertyName = "Pmt";
			resources.ApplyResources(this.dcPMT, "dcPMT");
			this.dcPMT.Name = "dcPMT";
			this.dcPMT.ReadOnly = true;
			this.dcBAT.DataPropertyName = "Bat";
			resources.ApplyResources(this.dcBAT, "dcBAT");
			this.dcBAT.Name = "dcBAT";
			this.dcBAT.ReadOnly = true;
			this.dcSDT_ACT.DataPropertyName = "SdtAct";
			resources.ApplyResources(this.dcSDT_ACT, "dcSDT_ACT");
			this.dcSDT_ACT.Name = "dcSDT_ACT";
			this.dcSDT_ACT.ReadOnly = true;
			this.dcSDT_OTH.DataPropertyName = "SdtOth";
			resources.ApplyResources(this.dcSDT_OTH, "dcSDT_OTH");
			this.dcSDT_OTH.Name = "dcSDT_OTH";
			this.dcSDT_OTH.ReadOnly = true;
			this.dcNIT_ACT.DataPropertyName = "NitAct";
			resources.ApplyResources(this.dcNIT_ACT, "dcNIT_ACT");
			this.dcNIT_ACT.Name = "dcNIT_ACT";
			this.dcNIT_ACT.ReadOnly = true;
			this.dcNIT_OTH.DataPropertyName = "NitOth";
			resources.ApplyResources(this.dcNIT_OTH, "dcNIT_OTH");
			this.dcNIT_OTH.Name = "dcNIT_OTH";
			this.dcNIT_OTH.ReadOnly = true;
			this.dcTDT.DataPropertyName = "Tdt";
			resources.ApplyResources(this.dcTDT, "dcTDT");
			this.dcTDT.Name = "dcTDT";
			this.dcTDT.ReadOnly = true;
			this.dcEIT_ACT_PF.DataPropertyName = "EitActPf";
			resources.ApplyResources(this.dcEIT_ACT_PF, "dcEIT_ACT_PF");
			this.dcEIT_ACT_PF.Name = "dcEIT_ACT_PF";
			this.dcEIT_ACT_PF.ReadOnly = true;
			this.dcEIT_ACT_SC.DataPropertyName = "EitActSc";
			resources.ApplyResources(this.dcEIT_ACT_SC, "dcEIT_ACT_SC");
			this.dcEIT_ACT_SC.Name = "dcEIT_ACT_SC";
			this.dcEIT_ACT_SC.ReadOnly = true;
			this.dcEIT_OTH_PF.DataPropertyName = "EitOthPf";
			resources.ApplyResources(this.dcEIT_OTH_PF, "dcEIT_OTH_PF");
			this.dcEIT_OTH_PF.Name = "dcEIT_OTH_PF";
			this.dcEIT_OTH_PF.ReadOnly = true;
			this.dcEIT_OTH_SC.DataPropertyName = "EitOthSc";
			resources.ApplyResources(this.dcEIT_OTH_SC, "dcEIT_OTH_SC");
			this.dcEIT_OTH_SC.Name = "dcEIT_OTH_SC";
			this.dcEIT_OTH_SC.ReadOnly = true;
			this.dcTOT.DataPropertyName = "Tot";
			resources.ApplyResources(this.dcTOT, "dcTOT");
			this.dcTOT.Name = "dcTOT";
			this.dcTOT.ReadOnly = true;
			this.dcId.DataPropertyName = "Id";
			resources.ApplyResources(this.dcId, "dcId");
			this.dcId.Name = "dcId";
			this.dcId.ReadOnly = true;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormSendSpeed";
			base.Controls.SetChildIndex(base.pnlBase, 0);
			base.pnlBase.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvSendSpeeds).EndInit();
			this.cmsSelect.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
