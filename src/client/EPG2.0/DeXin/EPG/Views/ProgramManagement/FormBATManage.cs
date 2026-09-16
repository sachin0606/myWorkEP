using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using Com.Cn.Tool.UserControls;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource.ResourcesSpecialFiles;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;
using EPG.Tools;

namespace DeXin.EPG.Views.ProgramManagement
{
	public class FormBATManage : FormToolBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private ComboBox cmbNIT;

		private Label lblTableType;

		private DGV dgvBAT;

		private DataGridViewCheckBoxColumn dcSelectCK;

		private DataGridViewTextBoxColumn dcBATId;

		private DataGridViewTextBoxColumn dcBATName;

		private DataGridViewTextBoxColumn dcVersion;

		private DataGridViewTextBoxColumn dcKey;

		private ContextMenuStrip cmsSelect;

		private ToolStripMenuItem tsmDeselectAll;

		private ToolStripMenuItem tsmSelectAll;

		private BATInfoManager batManager = new BATInfoManager();

		private LogBLL loger = new LogBLL();

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.ProgramManagement.FormBATManage));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.cmbNIT = new System.Windows.Forms.ComboBox();
			this.lblTableType = new System.Windows.Forms.Label();
			this.dgvBAT = new Com.Cn.Tool.UserControls.DGV();
			this.dcSelectCK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.dcBATId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcBATName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cmsSelect = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
			base.pnlBase.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvBAT).BeginInit();
			this.cmsSelect.SuspendLayout();
			base.SuspendLayout();
			base.pnlBase.Controls.Add(this.dgvBAT);
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
			this.dgvBAT.AllowUserToAddRows = false;
			this.dgvBAT.AllowUserToDeleteRows = false;
			this.dgvBAT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvBAT.Columns.AddRange(this.dcSelectCK, this.dcBATId, this.dcBATName, this.dcVersion, this.dcKey);
			this.dgvBAT.ContextMenuStrip = this.cmsSelect;
			resources.ApplyResources(this.dgvBAT, "dgvBAT");
			this.dgvBAT.Name = "dgvBAT";
			this.dgvBAT.ReadOnly = true;
			this.dgvBAT.RowTemplate.Height = 23;
			this.dgvBAT.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvBAT.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvBAT_CellClick);
			this.dgvBAT.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvBAT_CellDoubleClick);
			resources.ApplyResources(this.dcSelectCK, "dcSelectCK");
			this.dcSelectCK.Name = "dcSelectCK";
			this.dcSelectCK.ReadOnly = true;
			this.dcSelectCK.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dcSelectCK.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dcBATId.DataPropertyName = "BATId";
			dataGridViewCellStyle.Format = "X4";
			dataGridViewCellStyle.NullValue = null;
			this.dcBATId.DefaultCellStyle = dataGridViewCellStyle;
			resources.ApplyResources(this.dcBATId, "dcBATId");
			this.dcBATId.Name = "dcBATId";
			this.dcBATId.ReadOnly = true;
			this.dcBATName.DataPropertyName = "TableName";
			resources.ApplyResources(this.dcBATName, "dcBATName");
			this.dcBATName.Name = "dcBATName";
			this.dcBATName.ReadOnly = true;
			this.dcVersion.DataPropertyName = "BATVersionNumber";
			dataGridViewCellStyle2.Format = "X2";
			dataGridViewCellStyle2.NullValue = null;
			this.dcVersion.DefaultCellStyle = dataGridViewCellStyle2;
			resources.ApplyResources(this.dcVersion, "dcVersion");
			this.dcVersion.Name = "dcVersion";
			this.dcVersion.ReadOnly = true;
			this.dcKey.DataPropertyName = "Identity";
			resources.ApplyResources(this.dcKey, "dcKey");
			this.dcKey.Name = "dcKey";
			this.dcKey.ReadOnly = true;
			this.cmsSelect.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmSelectAll, this.tsmDeselectAll });
			this.cmsSelect.Name = "cmsSelect";
			resources.ApplyResources(this.cmsSelect, "cmsSelect");
			this.tsmSelectAll.Name = "tsmSelectAll";
			resources.ApplyResources(this.tsmSelectAll, "tsmSelectAll");
			this.tsmSelectAll.Click += new System.EventHandler(tsmSelectAll_Click);
			this.tsmDeselectAll.Name = "tsmDeselectAll";
			resources.ApplyResources(this.tsmDeselectAll, "tsmDeselectAll");
			this.tsmDeselectAll.Click += new System.EventHandler(tsmDeselectAll_Click);
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormBATManage";
			base.Controls.SetChildIndex(base.pnlBase, 0);
			base.pnlBase.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvBAT).EndInit();
			this.cmsSelect.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormBATManage()
		{
			InitializeComponent();
			IList<NitInfo> allNITInfos = new NITInfoManager().GetAllNITInfos();
			IList<BaseKey> lst = ((IEnumerable<BaseKey>)allNITInfos).ToList();
			Tool.BindingComboBox(lst, cmbNIT);
			SetMenuVisible(false, true, true, true, true, false, false, true, false);
			tsbSave.Text = ResourceSpecial.Export;
			_FunctionName = EnumFunctionName.SysFuncBATManage;
		}

		private void cmbNIT_SelectedIndexChanged(object sender, EventArgs e)
		{
			string value = Tool.GetValue(cmbNIT);
			IList<BATInfo> byNetworkIdentity = batManager.GetByNetworkIdentity(Tool.GetInt64(value));
			dgvBAT.DataSource = byNetworkIdentity;
		}

		private void dgvBAT_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (dgvBAT.Rows.Count >= 1 && e.RowIndex >= 0)
			{
				DataGridViewRow currentRow = dgvBAT.CurrentRow;
				long batKey = Tool.GetInt32(currentRow.Cells[dcKey.Name].Value);
				BATInfo bAT = ((List<BATInfo>)dgvBAT.DataSource).Where((BATInfo b) => b.Identity == batKey).First();
				new FormBATManageDetail(bAT, PageEnum.Show).ShowDialog();
			}
		}

		protected override void FuncAdd()
		{
			base.FuncAdd();
			NitInfo nITInfoByNetworkIdentity = new NITInfoManager().GetNITInfoByNetworkIdentity(Tool.GetInt64(Tool.GetValue(cmbNIT)));
			if (new FormBATManageDetail(nITInfoByNetworkIdentity).ShowDialog() == DialogResult.OK)
			{
				cmbNIT_SelectedIndexChanged(null, null);
				loger.AddLog(_FunctionName, EnumOSName.Add, "");
			}
		}

		protected override void FuncEdit()
		{
			base.FuncEdit();
			if (dgvBAT.Rows.Count >= 1)
			{
				DataGridViewRow currentRow = dgvBAT.CurrentRow;
				long batKey = Tool.GetInt32(currentRow.Cells[dcKey.Name].Value);
				BATInfo bATInfo = ((List<BATInfo>)dgvBAT.DataSource).Where((BATInfo b) => b.Identity == batKey).First();
				if (new FormBATManageDetail(bATInfo, PageEnum.Edit).ShowDialog() == DialogResult.OK)
				{
					cmbNIT_SelectedIndexChanged(null, null);
					loger.AddLog(_FunctionName, EnumOSName.Edit, "key:" + bATInfo.Identity);
				}
			}
		}

		protected override bool FuncDelete()
		{
			List<long> list = new List<long>();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DataGridViewRow item in (IEnumerable)dgvBAT.Rows)
			{
				if (Tool.GetBoolean(item.Cells[dcSelectCK.Name].Value))
				{
					long @int = Tool.GetInt64(item.Cells[dcKey.Name].Value);
					list.Add(@int);
					stringBuilder.Append(@int + "|");
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
				batManager.Delete(list);
				cmbNIT_SelectedIndexChanged(null, null);
				loger.AddLog(_FunctionName, EnumOSName.Delete, stringBuilder.ToString());
				return true;
			}
			return false;
		}

		protected override bool FuncSave()
		{
			List<string> fileLines = (from x in new BATInfoManager().GetBouquetConfiguration()
				select x.ToString()).ToList();
			CSVExporter.Export("BATInfo", fileLines);
			return true;
		}

		private void dgvBAT_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == dgvBAT.Columns[dcSelectCK.Name].Index && e.RowIndex >= 0 && dgvBAT.Rows.Count != 0)
			{
				DataGridViewRow dataGridViewRow = dgvBAT.Rows[e.RowIndex];
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
			foreach (DataGridViewRow item in (IEnumerable)dgvBAT.Rows)
			{
				item.Cells[dcSelectCK.Name].Value = true;
			}
		}

		private void tsmDeselectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvBAT.Rows)
			{
				item.Cells[dcSelectCK.Name].Value = false;
			}
		}
	}
}
