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
using DeXin.EPG.Resource.ResourcesSpecialFiles;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;
using EPG.Tools;

namespace DeXin.EPG.Views.SendControlManagement
{
	public class FormReMappingPID : FormToolBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblNetwork;

		private DGV dgvRemappingPID;

		private ComboBox cmbNetwork;

		private ContextMenuStrip cmsSelect;

		private ToolStripMenuItem tsmSelectAll;

		private ToolStripMenuItem tsmDeselectAll;

		private DataGridViewCheckBoxColumn colSelect;

		private DataGridViewTextBoxColumn colID;

		private DataGridViewTextBoxColumn colTSTableID;

		private DataGridViewComboBoxColumn colTSName;

		private DataGridViewTextBoxColumn colPAT;

		private DataGridViewTextBoxColumn colCAT;

		private DataGridViewTextBoxColumn colsdtorbat;

		private DataGridViewTextBoxColumn colNIT;

		private DataGridViewTextBoxColumn coltot_or_tdt;

		private DataGridViewTextBoxColumn colEIT;

		private TSInfoManager tsInfoBLL = new TSInfoManager();

		private NITInfoManager nitBLL = new NITInfoManager();

		private ReMappingPIDBLL reMappingBLL = new ReMappingPIDBLL();

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.SendControlManagement.FormReMappingPID));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblNetwork = new System.Windows.Forms.Label();
			this.dgvRemappingPID = new Com.Cn.Tool.UserControls.DGV();
			this.cmsSelect = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.cmbNetwork = new System.Windows.Forms.ComboBox();
			this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colTSTableID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colTSName = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colPAT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colCAT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colsdtorbat = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colNIT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.coltot_or_tdt = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colEIT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			base.pnlBase.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvRemappingPID).BeginInit();
			this.cmsSelect.SuspendLayout();
			base.SuspendLayout();
			base.pnlBase.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.pnlBase, "pnlBase");
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblNetwork, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.dgvRemappingPID, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.cmbNetwork, 1, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.lblNetwork, "lblNetwork");
			this.lblNetwork.Name = "lblNetwork";
			this.dgvRemappingPID.AllowUserToAddRows = false;
			this.dgvRemappingPID.AllowUserToDeleteRows = false;
			this.dgvRemappingPID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvRemappingPID.Columns.AddRange(this.colSelect, this.colID, this.colTSTableID, this.colTSName, this.colPAT, this.colCAT, this.colsdtorbat, this.colNIT, this.coltot_or_tdt, this.colEIT);
			this.tableLayoutPanel1.SetColumnSpan(this.dgvRemappingPID, 3);
			this.dgvRemappingPID.ContextMenuStrip = this.cmsSelect;
			resources.ApplyResources(this.dgvRemappingPID, "dgvRemappingPID");
			this.dgvRemappingPID.Name = "dgvRemappingPID";
			this.dgvRemappingPID.ReadOnly = true;
			this.dgvRemappingPID.RowTemplate.Height = 23;
			this.dgvRemappingPID.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvRemappingPID.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvMultiIPOutPut_CellMouseClick);
			this.dgvRemappingPID.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvMultiIPOutPut_CellMouseDoubleClick);
			this.cmsSelect.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmSelectAll, this.tsmDeselectAll });
			this.cmsSelect.Name = "cmsSelect";
			resources.ApplyResources(this.cmsSelect, "cmsSelect");
			this.tsmSelectAll.Name = "tsmSelectAll";
			resources.ApplyResources(this.tsmSelectAll, "tsmSelectAll");
			this.tsmSelectAll.Click += new System.EventHandler(tsmSelectAll_Click);
			this.tsmDeselectAll.Name = "tsmDeselectAll";
			resources.ApplyResources(this.tsmDeselectAll, "tsmDeselectAll");
			this.tsmDeselectAll.Click += new System.EventHandler(tsmDeselectAll_Click);
			resources.ApplyResources(this.cmbNetwork, "cmbNetwork");
			this.cmbNetwork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbNetwork.FormattingEnabled = true;
			this.cmbNetwork.Name = "cmbNetwork";
			this.cmbNetwork.SelectedIndexChanged += new System.EventHandler(cmbNetwork_SelectedIndexChanged);
			resources.ApplyResources(this.colSelect, "colSelect");
			this.colSelect.Name = "colSelect";
			this.colSelect.ReadOnly = true;
			this.colSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colID.DataPropertyName = "ID";
			resources.ApplyResources(this.colID, "colID");
			this.colID.Name = "colID";
			this.colID.ReadOnly = true;
			this.colTSTableID.DataPropertyName = "tstableid";
			resources.ApplyResources(this.colTSTableID, "colTSTableID");
			this.colTSTableID.Name = "colTSTableID";
			this.colTSTableID.ReadOnly = true;
			this.colTSName.DataPropertyName = "tstableid";
			this.colTSName.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			resources.ApplyResources(this.colTSName, "colTSName");
			this.colTSName.Name = "colTSName";
			this.colTSName.ReadOnly = true;
			this.colPAT.DataPropertyName = "pat";
			dataGridViewCellStyle.Format = "X4";
			dataGridViewCellStyle.NullValue = null;
			this.colPAT.DefaultCellStyle = dataGridViewCellStyle;
			resources.ApplyResources(this.colPAT, "colPAT");
			this.colPAT.Name = "colPAT";
			this.colPAT.ReadOnly = true;
			this.colCAT.DataPropertyName = "cat";
			dataGridViewCellStyle2.Format = "X4";
			this.colCAT.DefaultCellStyle = dataGridViewCellStyle2;
			resources.ApplyResources(this.colCAT, "colCAT");
			this.colCAT.Name = "colCAT";
			this.colCAT.ReadOnly = true;
			this.colsdtorbat.DataPropertyName = "sdtorbat";
			dataGridViewCellStyle3.Format = "X4";
			this.colsdtorbat.DefaultCellStyle = dataGridViewCellStyle3;
			resources.ApplyResources(this.colsdtorbat, "colsdtorbat");
			this.colsdtorbat.Name = "colsdtorbat";
			this.colsdtorbat.ReadOnly = true;
			this.colNIT.DataPropertyName = "nit";
			dataGridViewCellStyle4.Format = "X4";
			this.colNIT.DefaultCellStyle = dataGridViewCellStyle4;
			resources.ApplyResources(this.colNIT, "colNIT");
			this.colNIT.Name = "colNIT";
			this.colNIT.ReadOnly = true;
			this.coltot_or_tdt.DataPropertyName = "TOTORTDT";
			dataGridViewCellStyle5.Format = "X4";
			this.coltot_or_tdt.DefaultCellStyle = dataGridViewCellStyle5;
			resources.ApplyResources(this.coltot_or_tdt, "coltot_or_tdt");
			this.coltot_or_tdt.Name = "coltot_or_tdt";
			this.coltot_or_tdt.ReadOnly = true;
			this.colEIT.DataPropertyName = "EIT";
			dataGridViewCellStyle6.Format = "X4";
			this.colEIT.DefaultCellStyle = dataGridViewCellStyle6;
			resources.ApplyResources(this.colEIT, "colEIT");
			this.colEIT.Name = "colEIT";
			this.colEIT.ReadOnly = true;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormReMappingPID";
			base.Controls.SetChildIndex(base.pnlBase, 0);
			base.pnlBase.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvRemappingPID).EndInit();
			this.cmsSelect.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormReMappingPID()
		{
			InitializeComponent();
			_FunctionName = EnumFunctionName.SysFuncRedefinePID;
		}

		protected override void FormLoad()
		{
			SetMenuVisible(false, true, true, true, true, false, false, true, false);
			tsbSave.Text = ResourceSpecial.Export;
			BindComboNetwork();
			BindGrid(Tool.GetInt64(cmbNetwork.SelectedValue));
		}

		protected override void FuncAdd()
		{
			if (cmbNetwork.SelectedValue == null)
			{
				Tool.ShowMessage(CommTool.ShowInputFormatError(lblNetwork));
				return;
			}
			IList<PidOutputInfo> controlled = dgvRemappingPID.DataSource as IList<PidOutputInfo>;
			IList<TsInfo> tSInfoByNetWorkIdentity = tsInfoBLL.GetTSInfoByNetWorkIdentity(Tool.GetInt64(cmbNetwork.SelectedValue));
			if (controlled.Count == tSInfoByNetWorkIdentity.Count)
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("CannotAddAnyMore"));
				return;
			}
			IEnumerable<TsInfo> source = tSInfoByNetWorkIdentity.Where((TsInfo x) => !controlled.Select((PidOutputInfo m) => m.TsTableId).Contains(x.Identity));
			FormReMappingPIDDetail formReMappingPIDDetail = new FormReMappingPIDDetail(PageEnum.Add, Tool.GetInt64(cmbNetwork.SelectedValue), source.ToList());
			if (formReMappingPIDDetail.ShowDialog() == DialogResult.OK)
			{
				BindGrid(Tool.GetInt64(cmbNetwork.SelectedValue));
			}
		}

		protected override void FuncEdit()
		{
			if (cmbNetwork.SelectedValue == null)
			{
				Tool.ShowMessage(CommTool.ShowInputFormatError(lblNetwork));
				return;
			}
			DataGridViewRow currentRow = dgvRemappingPID.CurrentRow;
			if (currentRow != null)
			{
				long @int = Tool.GetInt64(currentRow.Cells[colID.Name].Value);
				FormReMappingPIDDetail formReMappingPIDDetail = new FormReMappingPIDDetail(PageEnum.Edit, @int, Tool.GetInt64(cmbNetwork.SelectedValue));
				if (formReMappingPIDDetail.ShowDialog() == DialogResult.OK)
				{
					BindGrid(Tool.GetInt64(cmbNetwork.SelectedValue));
				}
			}
		}

		protected override bool FuncDelete()
		{
			bool result = false;
			List<PidOutputInfo> list = new List<PidOutputInfo>();
			foreach (DataGridViewRow item in (IEnumerable)dgvRemappingPID.Rows)
			{
				if (Tool.GetBoolean(item.Cells[colSelect.Name].Value))
				{
					PidOutputInfo pidOutputInfo = new PidOutputInfo();
					pidOutputInfo.Id = Tool.GetInt32(item.Cells[colID.Name].Value);
					list.Add(pidOutputInfo);
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
				reMappingBLL.Delete(list);
				BindGrid(Tool.GetInt64(cmbNetwork.SelectedValue));
				result = true;
			}
			return result;
		}

		protected override bool FuncSave()
		{
			CSVExporter.Export("PidOutputInfo", reMappingBLL.ExportToCSVData());
			return true;
		}

		private void dgvMultiIPOutPut_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex <= 0 && e.RowIndex >= 0 && dgvRemappingPID.RowCount != 0)
			{
				DataGridViewRow dataGridViewRow = dgvRemappingPID.Rows[e.RowIndex];
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
				DataGridViewRow dataGridViewRow = dgvRemappingPID.Rows[e.RowIndex];
				long @int = Tool.GetInt64(dataGridViewRow.Cells[colID.Name].Value);
				FormReMappingPIDDetail formReMappingPIDDetail = new FormReMappingPIDDetail(PageEnum.Show, @int, Tool.GetInt64(cmbNetwork.SelectedValue));
				formReMappingPIDDetail.ShowDialog();
			}
		}

		private void BindComboNetwork()
		{
			IList<BaseKey> lst = ((IEnumerable<BaseKey>)nitBLL.GetAllNITInfos()).ToList();
			Tool.BindingComboBox(lst, cmbNetwork);
		}

		private void BindGrid(long networkid)
		{
			IList<TsInfo> tSInfoByNetWorkIdentity = tsInfoBLL.GetTSInfoByNetWorkIdentity(Tool.GetInt64(cmbNetwork.SelectedValue));
			IList<TBaseKey<long, string>> list = new List<TBaseKey<long, string>>();
			foreach (TsInfo item2 in tSInfoByNetWorkIdentity)
			{
				TBaseKey<long, string> item = new TBaseKey<long, string>(long.Parse(item2.BKKey), item2.BKValue);
				list.Add(item);
			}
			CommTool.BindComboBoxUsingTBaseKey(list, colTSName);
			dgvRemappingPID.DataSource = reMappingBLL.GetDataByNetworkTableID(networkid);
		}

		private void cmbNetwork_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindGrid(Tool.GetInt64(cmbNetwork.SelectedValue));
		}

		private void tsmSelectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvRemappingPID.Rows)
			{
				item.Cells[colSelect.Name].Value = true;
			}
		}

		private void tsmDeselectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvRemappingPID.Rows)
			{
				item.Cells[colSelect.Name].Value = false;
			}
		}
	}
}
