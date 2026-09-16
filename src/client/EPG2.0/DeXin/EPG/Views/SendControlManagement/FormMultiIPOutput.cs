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
	public class FormMultiIPOutput : FormToolBase
	{
		private IContainer components = null;

		private DGV dgvMultiIPOutPut;

		private TableLayoutPanel tableLayoutPanel1;

		private ComboBox cmbNetwork;

		private Label lblNetwork;

		private ContextMenuStrip cmsSelect;

		private ToolStripMenuItem tsmSelectAll;

		private ToolStripMenuItem tsmDeselectAll;

		private DataGridViewCheckBoxColumn colSelect;

		private DataGridViewTextBoxColumn colID;

		private DataGridViewTextBoxColumn colTSTableID;

		private DataGridViewComboBoxColumn colTSName;

		private DataGridViewTextBoxColumn colIP;

		private DataGridViewTextBoxColumn colPort;

		private NITInfoManager nitBLL = new NITInfoManager();

		private IList<MultipleipSettings> multiIPs = new List<MultipleipSettings>();

		private MultiIPOutputBLL multiIPoutputBLL = new MultiIPOutputBLL();

		private TSInfoManager tsInfoBLL = new TSInfoManager();

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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.SendControlManagement.FormMultiIPOutput));
			this.dgvMultiIPOutPut = new Com.Cn.Tool.UserControls.DGV();
			this.cmsSelect = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.cmbNetwork = new System.Windows.Forms.ComboBox();
			this.lblNetwork = new System.Windows.Forms.Label();
			this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colTSTableID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colTSName = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
			base.pnlBase.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvMultiIPOutPut).BeginInit();
			this.cmsSelect.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.pnlBase.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.pnlBase, "pnlBase");
			this.dgvMultiIPOutPut.AllowUserToAddRows = false;
			this.dgvMultiIPOutPut.AllowUserToDeleteRows = false;
			this.dgvMultiIPOutPut.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvMultiIPOutPut.Columns.AddRange(this.colSelect, this.colID, this.colTSTableID, this.colTSName, this.colIP, this.colPort);
			this.tableLayoutPanel1.SetColumnSpan(this.dgvMultiIPOutPut, 3);
			this.dgvMultiIPOutPut.ContextMenuStrip = this.cmsSelect;
			resources.ApplyResources(this.dgvMultiIPOutPut, "dgvMultiIPOutPut");
			this.dgvMultiIPOutPut.Name = "dgvMultiIPOutPut";
			this.dgvMultiIPOutPut.ReadOnly = true;
			this.dgvMultiIPOutPut.RowTemplate.Height = 23;
			this.dgvMultiIPOutPut.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvMultiIPOutPut.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvMultiIPOutPut_CellMouseClick);
			this.dgvMultiIPOutPut.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvMultiIPOutPut_CellMouseDoubleClick);
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
			this.tableLayoutPanel1.Controls.Add(this.cmbNetwork, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.dgvMultiIPOutPut, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblNetwork, 0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.cmbNetwork, "cmbNetwork");
			this.cmbNetwork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbNetwork.FormattingEnabled = true;
			this.cmbNetwork.Name = "cmbNetwork";
			this.cmbNetwork.SelectedIndexChanged += new System.EventHandler(cmbNetwork_SelectedIndexChanged);
			resources.ApplyResources(this.lblNetwork, "lblNetwork");
			this.lblNetwork.Name = "lblNetwork";
			this.colSelect.Frozen = true;
			resources.ApplyResources(this.colSelect, "colSelect");
			this.colSelect.Name = "colSelect";
			this.colSelect.ReadOnly = true;
			this.colID.DataPropertyName = "ID";
			this.colID.Frozen = true;
			resources.ApplyResources(this.colID, "colID");
			this.colID.Name = "colID";
			this.colID.ReadOnly = true;
			this.colTSTableID.DataPropertyName = "TsTableID";
			this.colTSTableID.Frozen = true;
			resources.ApplyResources(this.colTSTableID, "colTSTableID");
			this.colTSTableID.Name = "colTSTableID";
			this.colTSTableID.ReadOnly = true;
			this.colTSName.DataPropertyName = "TsTableID";
			this.colTSName.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			resources.ApplyResources(this.colTSName, "colTSName");
			this.colTSName.Name = "colTSName";
			this.colTSName.ReadOnly = true;
			this.colTSName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colTSName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colIP.DataPropertyName = "ip";
			resources.ApplyResources(this.colIP, "colIP");
			this.colIP.Name = "colIP";
			this.colIP.ReadOnly = true;
			this.colPort.DataPropertyName = "port";
			resources.ApplyResources(this.colPort, "colPort");
			this.colPort.Name = "colPort";
			this.colPort.ReadOnly = true;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormMultiIPOutput";
			base.Controls.SetChildIndex(base.pnlBase, 0);
			base.pnlBase.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dgvMultiIPOutPut).EndInit();
			this.cmsSelect.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormMultiIPOutput()
		{
			InitializeComponent();
			_FunctionName = EnumFunctionName.SysFuncMultipleIPOutput;
		}

		protected override void FormLoad()
		{
			SetMenuVisible(false, true, true, true, false, false, false, true, false);
			BindComboNetwork();
			BindDataGrid(Tool.GetInt64(cmbNetwork.SelectedValue));
		}

		private void BindDataGrid(long networkTableid)
		{
			IList<TsInfo> tSInfoByNetWorkIdentity = tsInfoBLL.GetTSInfoByNetWorkIdentity(Tool.GetInt64(cmbNetwork.SelectedValue));
			IList<TBaseKey<long, string>> list = new List<TBaseKey<long, string>>();
			foreach (TsInfo item2 in tSInfoByNetWorkIdentity)
			{
				TBaseKey<long, string> item = new TBaseKey<long, string>(long.Parse(item2.BKKey), item2.BKValue);
				list.Add(item);
			}
			CommTool.BindComboBoxUsingTBaseKey(list, colTSName);
			MultiIPOutputBLL multiIPOutputBLL = new MultiIPOutputBLL();
			dgvMultiIPOutPut.DataSource = multiIPOutputBLL.GetDataByNetworkTableID(networkTableid);
		}

		protected override void FuncAdd()
		{
			IList<MultipleipSettings> controlled = dgvMultiIPOutPut.DataSource as IList<MultipleipSettings>;
			IList<TsInfo> tSInfoByNetWorkIdentity = tsInfoBLL.GetTSInfoByNetWorkIdentity(Tool.GetInt64(cmbNetwork.SelectedValue));
			if (controlled.Count == tSInfoByNetWorkIdentity.Count)
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("CannotAddAnyMore"));
				return;
			}
			IEnumerable<TsInfo> source = tSInfoByNetWorkIdentity.Where((TsInfo x) => !controlled.Select((MultipleipSettings m) => m.TsTableId).Contains(x.Identity));
			FormMultiIPOutputDetail formMultiIPOutputDetail = new FormMultiIPOutputDetail(PageEnum.Add, -1L, source.ToList());
			if (formMultiIPOutputDetail.ShowDialog() == DialogResult.OK)
			{
				BindDataGrid(Tool.GetInt64(cmbNetwork.SelectedValue));
			}
		}

		protected override void FuncEdit()
		{
			DataGridViewRow currentRow = dgvMultiIPOutPut.CurrentRow;
			if (currentRow != null)
			{
				long @int = Tool.GetInt64(currentRow.Cells[colID.Name].Value);
				FormMultiIPOutputDetail formMultiIPOutputDetail = new FormMultiIPOutputDetail(PageEnum.Edit, @int, tsInfoBLL.GetTSInfoByNetWorkIdentity(Tool.GetInt64(cmbNetwork.SelectedValue)));
				if (formMultiIPOutputDetail.ShowDialog() == DialogResult.OK)
				{
					BindDataGrid(Tool.GetInt64(cmbNetwork.SelectedValue));
				}
			}
		}

		protected override bool FuncDelete()
		{
			string remark = "";
			bool result = false;
			List<MultipleipSettings> list = new List<MultipleipSettings>();
			foreach (DataGridViewRow item in (IEnumerable)dgvMultiIPOutPut.Rows)
			{
				if (Tool.GetBoolean(item.Cells[colSelect.Name].Value))
				{
					MultipleipSettings multipleipSettings = new MultipleipSettings();
					multipleipSettings.Id = Tool.GetInt32(item.Cells[colID.Name].Value);
					list.Add(multipleipSettings);
					remark = "id:" + multipleipSettings.Id + "[" + multipleipSettings.Ip.ToString() + ":" + multipleipSettings.Port + "]";
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
				multiIPoutputBLL.Delete(list);
				logBLL.AddLog(EnumFunctionName.SysFuncMultipleIPOutput, EnumOSName.Delete, remark);
				BindDataGrid(Tool.GetInt64(cmbNetwork.SelectedValue));
				result = true;
			}
			return result;
		}

		private void dgvMultiIPOutPut_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex <= 0 && e.RowIndex >= 0 && dgvMultiIPOutPut.RowCount != 0)
			{
				DataGridViewRow dataGridViewRow = dgvMultiIPOutPut.Rows[e.RowIndex];
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
				DataGridViewRow dataGridViewRow = dgvMultiIPOutPut.Rows[e.RowIndex];
				long @int = Tool.GetInt64(dataGridViewRow.Cells[colID.Name].Value);
				FormMultiIPOutputDetail formMultiIPOutputDetail = new FormMultiIPOutputDetail(PageEnum.Show, @int, tsInfoBLL.GetTSInfoByNetWorkIdentity(Tool.GetInt64(cmbNetwork.SelectedValue)));
				formMultiIPOutputDetail.ShowDialog();
			}
		}

		private void BindComboNetwork()
		{
			IList<BaseKey> lst = ((IEnumerable<BaseKey>)nitBLL.GetAllNITInfos()).ToList();
			Tool.BindingComboBox(lst, cmbNetwork);
		}

		private void cmbNetwork_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindDataGrid(Tool.GetInt64(cmbNetwork.SelectedValue));
		}

		private void tsmSelectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvMultiIPOutPut.Rows)
			{
				item.Cells[colSelect.Name].Value = true;
			}
		}

		private void tsmDeselectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvMultiIPOutPut.Rows)
			{
				item.Cells[colSelect.Name].Value = false;
			}
		}
	}
}
