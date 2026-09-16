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
	public class FormSendOrNot : FormToolBase
	{
		private IContainer components = null;

		private DGV dgvSendOrNot;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblNetwork;

		private ComboBox cmbNetwork;

		private ContextMenuStrip cmsSelect;

		private ToolStripMenuItem tsmSelectAll;

		private ToolStripMenuItem tsmDeselectAll;

		private DataGridViewCheckBoxColumn colSelect;

		private DataGridViewTextBoxColumn colID;

		private DataGridViewTextBoxColumn colTsTableId;

		private DataGridViewComboBoxColumn colTSName;

		private DataGridViewComboBoxColumn colPAT;

		private DataGridViewComboBoxColumn colCAT;

		private DataGridViewComboBoxColumn colPMT;

		private DataGridViewComboBoxColumn colBAT;

		private DataGridViewComboBoxColumn colSDTActual;

		private DataGridViewComboBoxColumn colSDTOther;

		private DataGridViewComboBoxColumn colNITActual;

		private DataGridViewComboBoxColumn colNITOther;

		private DataGridViewComboBoxColumn colTDT;

		private DataGridViewComboBoxColumn colTOT;

		private DataGridViewComboBoxColumn colEITActualPresentFollowing;

		private DataGridViewComboBoxColumn colEITOtherPresentFollowing;

		private DataGridViewComboBoxColumn colEITActualSchedule;

		private DataGridViewComboBoxColumn colEITOtherSchedule;

		private TSInfoManager tsInfoBLL = new TSInfoManager();

		private NITInfoManager nitBLL = new NITInfoManager();

		private SendOrNotInfoBLL sendOrNotBLL = new SendOrNotInfoBLL();

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.SendControlManagement.FormSendOrNot));
			this.dgvSendOrNot = new Com.Cn.Tool.UserControls.DGV();
			this.cmsSelect = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.lblNetwork = new System.Windows.Forms.Label();
			this.cmbNetwork = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colTsTableId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colTSName = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colPAT = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colCAT = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colPMT = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colBAT = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colSDTActual = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colSDTOther = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colNITActual = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colNITOther = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colTDT = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colTOT = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colEITActualPresentFollowing = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colEITOtherPresentFollowing = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colEITActualSchedule = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colEITOtherSchedule = new System.Windows.Forms.DataGridViewComboBoxColumn();
			base.pnlBase.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvSendOrNot).BeginInit();
			this.cmsSelect.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.pnlBase.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.pnlBase, "pnlBase");
			this.dgvSendOrNot.AllowUserToAddRows = false;
			this.dgvSendOrNot.AllowUserToDeleteRows = false;
			this.dgvSendOrNot.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSendOrNot.Columns.AddRange(this.colSelect, this.colID, this.colTsTableId, this.colTSName, this.colPAT, this.colCAT, this.colPMT, this.colBAT, this.colSDTActual, this.colSDTOther, this.colNITActual, this.colNITOther, this.colTDT, this.colTOT, this.colEITActualPresentFollowing, this.colEITOtherPresentFollowing, this.colEITActualSchedule, this.colEITOtherSchedule);
			this.tableLayoutPanel1.SetColumnSpan(this.dgvSendOrNot, 3);
			this.dgvSendOrNot.ContextMenuStrip = this.cmsSelect;
			resources.ApplyResources(this.dgvSendOrNot, "dgvSendOrNot");
			this.dgvSendOrNot.Name = "dgvSendOrNot";
			this.dgvSendOrNot.ReadOnly = true;
			this.dgvSendOrNot.RowTemplate.Height = 23;
			this.dgvSendOrNot.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvSendOrNot.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvMultiIPOutPut_CellMouseClick);
			this.dgvSendOrNot.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvMultiIPOutPut_CellMouseDoubleClick);
			this.cmsSelect.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmSelectAll, this.tsmDeselectAll });
			this.cmsSelect.Name = "cmsSelect";
			resources.ApplyResources(this.cmsSelect, "cmsSelect");
			this.tsmSelectAll.Name = "tsmSelectAll";
			resources.ApplyResources(this.tsmSelectAll, "tsmSelectAll");
			this.tsmSelectAll.Click += new System.EventHandler(tsmSelectAll_Click);
			this.tsmDeselectAll.Name = "tsmDeselectAll";
			resources.ApplyResources(this.tsmDeselectAll, "tsmDeselectAll");
			this.tsmDeselectAll.Click += new System.EventHandler(tsmDeselectAll_Click);
			resources.ApplyResources(this.lblNetwork, "lblNetwork");
			this.lblNetwork.Name = "lblNetwork";
			resources.ApplyResources(this.cmbNetwork, "cmbNetwork");
			this.cmbNetwork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbNetwork.FormattingEnabled = true;
			this.cmbNetwork.Name = "cmbNetwork";
			this.cmbNetwork.SelectedIndexChanged += new System.EventHandler(cmbNetwork_SelectedIndexChanged);
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblNetwork, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.dgvSendOrNot, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.cmbNetwork, 1, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.colSelect, "colSelect");
			this.colSelect.Name = "colSelect";
			this.colSelect.ReadOnly = true;
			this.colID.DataPropertyName = "id";
			resources.ApplyResources(this.colID, "colID");
			this.colID.Name = "colID";
			this.colID.ReadOnly = true;
			this.colTsTableId.DataPropertyName = "TSTableID";
			resources.ApplyResources(this.colTsTableId, "colTsTableId");
			this.colTsTableId.Name = "colTsTableId";
			this.colTsTableId.ReadOnly = true;
			this.colTSName.DataPropertyName = "TSTableID";
			this.colTSName.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			resources.ApplyResources(this.colTSName, "colTSName");
			this.colTSName.Name = "colTSName";
			this.colTSName.ReadOnly = true;
			this.colTSName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colTSName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colPAT.DataPropertyName = "PAT";
			this.colPAT.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			this.colPAT.DisplayStyleForCurrentCellOnly = true;
			resources.ApplyResources(this.colPAT, "colPAT");
			this.colPAT.Name = "colPAT";
			this.colPAT.ReadOnly = true;
			this.colPAT.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colPAT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colCAT.DataPropertyName = "CAT";
			this.colCAT.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			resources.ApplyResources(this.colCAT, "colCAT");
			this.colCAT.Name = "colCAT";
			this.colCAT.ReadOnly = true;
			this.colCAT.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colCAT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colPMT.DataPropertyName = "PMT";
			this.colPMT.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			resources.ApplyResources(this.colPMT, "colPMT");
			this.colPMT.Name = "colPMT";
			this.colPMT.ReadOnly = true;
			this.colPMT.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colPMT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colBAT.DataPropertyName = "BAT";
			this.colBAT.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			resources.ApplyResources(this.colBAT, "colBAT");
			this.colBAT.Name = "colBAT";
			this.colBAT.ReadOnly = true;
			this.colSDTActual.DataPropertyName = "SDTACT";
			this.colSDTActual.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			resources.ApplyResources(this.colSDTActual, "colSDTActual");
			this.colSDTActual.Name = "colSDTActual";
			this.colSDTActual.ReadOnly = true;
			this.colSDTActual.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colSDTActual.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colSDTOther.DataPropertyName = "SDTOth";
			this.colSDTOther.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			resources.ApplyResources(this.colSDTOther, "colSDTOther");
			this.colSDTOther.Name = "colSDTOther";
			this.colSDTOther.ReadOnly = true;
			this.colSDTOther.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colSDTOther.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colNITActual.DataPropertyName = "NITAct";
			this.colNITActual.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			resources.ApplyResources(this.colNITActual, "colNITActual");
			this.colNITActual.Name = "colNITActual";
			this.colNITActual.ReadOnly = true;
			this.colNITActual.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colNITActual.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colNITOther.DataPropertyName = "NITOth";
			this.colNITOther.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			resources.ApplyResources(this.colNITOther, "colNITOther");
			this.colNITOther.Name = "colNITOther";
			this.colNITOther.ReadOnly = true;
			this.colNITOther.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colNITOther.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colTDT.DataPropertyName = "TDT";
			this.colTDT.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			resources.ApplyResources(this.colTDT, "colTDT");
			this.colTDT.Name = "colTDT";
			this.colTDT.ReadOnly = true;
			this.colTDT.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colTDT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colTOT.DataPropertyName = "TOT";
			this.colTOT.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			resources.ApplyResources(this.colTOT, "colTOT");
			this.colTOT.Name = "colTOT";
			this.colTOT.ReadOnly = true;
			this.colTOT.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colTOT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colEITActualPresentFollowing.DataPropertyName = "EitActPF";
			this.colEITActualPresentFollowing.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			resources.ApplyResources(this.colEITActualPresentFollowing, "colEITActualPresentFollowing");
			this.colEITActualPresentFollowing.Name = "colEITActualPresentFollowing";
			this.colEITActualPresentFollowing.ReadOnly = true;
			this.colEITActualPresentFollowing.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colEITActualPresentFollowing.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colEITOtherPresentFollowing.DataPropertyName = "eitOthpf";
			this.colEITOtherPresentFollowing.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			resources.ApplyResources(this.colEITOtherPresentFollowing, "colEITOtherPresentFollowing");
			this.colEITOtherPresentFollowing.Name = "colEITOtherPresentFollowing";
			this.colEITOtherPresentFollowing.ReadOnly = true;
			this.colEITOtherPresentFollowing.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colEITOtherPresentFollowing.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colEITActualSchedule.DataPropertyName = "eitActsc";
			this.colEITActualSchedule.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			resources.ApplyResources(this.colEITActualSchedule, "colEITActualSchedule");
			this.colEITActualSchedule.Name = "colEITActualSchedule";
			this.colEITActualSchedule.ReadOnly = true;
			this.colEITActualSchedule.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colEITActualSchedule.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colEITOtherSchedule.DataPropertyName = "EitOTHSC";
			this.colEITOtherSchedule.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			resources.ApplyResources(this.colEITOtherSchedule, "colEITOtherSchedule");
			this.colEITOtherSchedule.Name = "colEITOtherSchedule";
			this.colEITOtherSchedule.ReadOnly = true;
			this.colEITOtherSchedule.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colEITOtherSchedule.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormSendOrNot";
			base.Controls.SetChildIndex(base.pnlBase, 0);
			base.pnlBase.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dgvSendOrNot).EndInit();
			this.cmsSelect.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormSendOrNot()
		{
			InitializeComponent();
			_FunctionName = EnumFunctionName.SysFuncSendOrNot;
		}

		protected override void FormLoad()
		{
			SetMenuVisible(false, true, true, true, false, false, false, true, false);
			BindComboNetwork();
			BindAllCombo();
			BindGrid(Tool.GetInt64(cmbNetwork.SelectedValue));
		}

		protected override void FuncAdd()
		{
			if (cmbNetwork.SelectedValue == null)
			{
				Tool.ShowMessage(CommTool.ShowInputFormatError(lblNetwork));
				return;
			}
			IList<SendOrNotInfo> controlled = dgvSendOrNot.DataSource as IList<SendOrNotInfo>;
			IList<TsInfo> tSInfoByNetWorkIdentity = tsInfoBLL.GetTSInfoByNetWorkIdentity(Tool.GetInt64(cmbNetwork.SelectedValue));
			if (controlled.Count == tSInfoByNetWorkIdentity.Count)
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("CannotAddAnyMore"));
				return;
			}
			IEnumerable<TsInfo> source = tSInfoByNetWorkIdentity.Where((TsInfo x) => !controlled.Select((SendOrNotInfo m) => m.TsTableId).Contains(x.Identity));
			FormSendOrNotDetail formSendOrNotDetail = new FormSendOrNotDetail(PageEnum.Add, source.ToList());
			formSendOrNotDetail.Owner = this;
			if (formSendOrNotDetail.ShowDialog() == DialogResult.OK)
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
			DataGridViewRow currentRow = dgvSendOrNot.CurrentRow;
			if (currentRow != null)
			{
				long @int = Tool.GetInt64(currentRow.Cells[colID.Name].Value);
				FormSendOrNotDetail formSendOrNotDetail = new FormSendOrNotDetail(PageEnum.Edit, @int, Tool.GetInt64(cmbNetwork.SelectedValue));
				if (formSendOrNotDetail.ShowDialog() == DialogResult.OK)
				{
					BindGrid(Tool.GetInt64(cmbNetwork.SelectedValue));
				}
			}
		}

		protected override bool FuncDelete()
		{
			string text = "";
			bool result = false;
			List<SendOrNotInfo> list = new List<SendOrNotInfo>();
			foreach (DataGridViewRow item in (IEnumerable)dgvSendOrNot.Rows)
			{
				if (Tool.GetBoolean(item.Cells[colSelect.Name].Value))
				{
					SendOrNotInfo sendOrNotInfo = new SendOrNotInfo();
					sendOrNotInfo.Id = Tool.GetInt32(item.Cells[colID.Name].Value);
					list.Add(sendOrNotInfo);
					text = text + " id:" + sendOrNotInfo.Id;
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
				sendOrNotBLL.Delete(list);
				BindGrid(Tool.GetInt64(cmbNetwork.SelectedValue));
				result = true;
				logBLL.AddLog(EnumFunctionName.SysFuncSendOrNot, EnumOSName.Delete, text);
			}
			return result;
		}

		private void dgvMultiIPOutPut_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex <= 0 && e.RowIndex >= 0 && dgvSendOrNot.RowCount != 0)
			{
				DataGridViewRow dataGridViewRow = dgvSendOrNot.Rows[e.RowIndex];
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
				DataGridViewRow dataGridViewRow = dgvSendOrNot.Rows[e.RowIndex];
				long @int = Tool.GetInt64(dataGridViewRow.Cells[colID.Name].Value);
				FormSendOrNotDetail formSendOrNotDetail = new FormSendOrNotDetail(PageEnum.Show, @int, Tool.GetInt64(cmbNetwork.SelectedValue));
				formSendOrNotDetail.ShowDialog();
			}
		}

		private void BindAllCombo()
		{
			IList<TBaseKey<int, string>> list = new List<TBaseKey<int, string>>();
			list.Add(new TBaseKey<int, string>(0, ResourceManagement.GetLocationString(EnumSendOrNot.NotSend.ToString())));
			list.Add(new TBaseKey<int, string>(1, ResourceManagement.GetLocationString(EnumSendOrNot.Send.ToString())));
			CommTool.BindComboBoxUsingTBaseKey(list, colPAT);
			CommTool.BindComboBoxUsingTBaseKey(list.ToList(), colCAT);
			CommTool.BindComboBoxUsingTBaseKey(list.ToList(), colPMT);
			CommTool.BindComboBoxUsingTBaseKey(list.ToList(), colBAT);
			CommTool.BindComboBoxUsingTBaseKey(list.ToList(), colSDTActual);
			CommTool.BindComboBoxUsingTBaseKey(list.ToList(), colSDTOther);
			CommTool.BindComboBoxUsingTBaseKey(list.ToList(), colNITActual);
			CommTool.BindComboBoxUsingTBaseKey(list.ToList(), colNITOther);
			CommTool.BindComboBoxUsingTBaseKey(list.ToList(), colTDT);
			CommTool.BindComboBoxUsingTBaseKey(list.ToList(), colTOT);
			CommTool.BindComboBoxUsingTBaseKey(list.ToList(), colEITActualPresentFollowing);
			CommTool.BindComboBoxUsingTBaseKey(list.ToList(), colEITActualSchedule);
			CommTool.BindComboBoxUsingTBaseKey(list.ToList(), colEITOtherPresentFollowing);
			CommTool.BindComboBoxUsingTBaseKey(list.ToList(), colEITOtherSchedule);
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
			dgvSendOrNot.DataSource = sendOrNotBLL.GetDataByNetworkTableID(networkid);
		}

		private void cmbNetwork_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindGrid(Tool.GetInt64(cmbNetwork.SelectedValue));
		}

		private void tsmSelectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvSendOrNot.Rows)
			{
				item.Cells[colSelect.Name].Value = true;
			}
		}

		private void tsmDeselectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvSendOrNot.Rows)
			{
				item.Cells[colSelect.Name].Value = false;
			}
		}
	}
}
