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
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.ProgramManagement
{
	public class FormEITManage : FormToolBase
	{
		private LogBLL loger = new LogBLL();

		private IContainer components = null;

		private ComboBox cmbNIT;

		private Label lblTableType;

		private Label label1;

		private ComboBox cmbTS;

		private ListBox lbxPrg;

		private SplitContainer splitContainer1;

		private GroupBox groupBox1;

		private TableLayoutPanel tableLayoutPanel1;

		private GroupBox groupBox2;

		private TableLayoutPanel tableLayoutPanel2;

		private GroupBox groupBox3;

		private DGV dgvEIT;

		private EPGPagination eitPager;

		private ContextMenuStrip cmsSelectEITFile;

		private ToolStripMenuItem tsmSelectAll;

		private ToolStripMenuItem tsmDeselectAll;

		private DataGridViewCheckBoxColumn dcSelectCK;

		private DataGridViewTextBoxColumn dcEventId;

		private DataGridViewTextBoxColumn dcEventName;

		private DataGridViewTextBoxColumn dcEventRemark;

		private DataGridViewTextBoxColumn dcStartTime;

		private DataGridViewTextBoxColumn dcEndTime;

		private DataGridViewTextBoxColumn dcEventRate;

		private DataGridViewTextBoxColumn dcKey;

		public FormEITManage()
		{
			InitializeComponent();
			IList<NitInfo> allNITInfos = new NITInfoManager().GetAllNITInfos();
			IList<BaseKey> lst = ((IEnumerable<BaseKey>)allNITInfos).ToList();
			Tool.BindingComboBox(lst, cmbNIT);
			SetMenuVisible(true, true, true, true, false, false, false, true, false);
			_FunctionName = EnumFunctionName.SysFuncEITManage;
			tsbQuery.Text = ResourceManagement.GetLocationString("ImportEvent");
		}

		private void cmbNIT_SelectedIndexChanged(object sender, EventArgs e)
		{
			long @int = Tool.GetInt64(Tool.GetValue(cmbNIT));
			IList<TsInfo> tSInfoByNetWorkIdentity = new TSInfoManager().GetTSInfoByNetWorkIdentity(@int);
			IList<BaseKey> list = ((IEnumerable<BaseKey>)tSInfoByNetWorkIdentity).ToList();
			if (list.Count == 0)
			{
				cmbTS.DataSource = null;
				lbxPrg.DataSource = null;
				dgvEIT.DataSource = null;
			}
			else
			{
				Tool.BindingComboBox(list, cmbTS);
			}
		}

		private void cmbTS_SelectedIndexChanged(object sender, EventArgs e)
		{
			long @int = Tool.GetInt64(Tool.GetValue(cmbTS));
			IList<BaseKey> list = ((IEnumerable<BaseKey>)new ProgramInfoManager().GetByTSIdentity(@int)).ToList();
			if (list.Count == 0)
			{
				lbxPrg.DataSource = null;
				dgvEIT.DataSource = null;
			}
			else
			{
				lbxPrg.DataSource = list;
				lbxPrg.ValueMember = "BKKey";
				lbxPrg.DisplayMember = "BKValue";
			}
		}

		private void lbxPrg_SelectedIndexChanged(object sender, EventArgs e)
		{
			long @int = Tool.GetInt64(lbxPrg.SelectedValue);
			if (((lbxPrg.Tag == null) ? (-1) : Convert.ToInt64(lbxPrg.Tag)) != @int)
			{
				lbxPrg.Tag = @int;
				eitPager.PageIndex = 0;
				int totalSize = 0;
				IList<EitInfo> byProgramIdentity = new EITInfoManager().GetByProgramIdentity(@int, eitPager.PageIndex, eitPager.PageSize, out totalSize);
				eitPager.TotleCount = totalSize;
				dgvEIT.DataSource = byProgramIdentity;
			}
		}

		private void dgvEIT_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (dgvEIT.Rows.Count >= 1 && e.RowIndex >= 0)
			{
				DataGridViewRow currentRow = dgvEIT.CurrentRow;
				long batKey = Tool.GetInt32(currentRow.Cells[dcKey.Name].Value);
				EitInfo eIT = ((List<EitInfo>)dgvEIT.DataSource).Where((EitInfo b) => b.Identity == batKey).First();
				new FormEITManageDetail(eIT, PageEnum.Show).ShowDialog();
			}
		}

		private void eitPager_PageIndexChanged(object sender, EventArgs e)
		{
			long @int = Tool.GetInt64(lbxPrg.SelectedValue);
			int totalSize = 0;
			IList<EitInfo> byProgramIdentity = new EITInfoManager().GetByProgramIdentity(@int, eitPager.PageIndex, eitPager.PageSize, out totalSize);
			eitPager.TotleCount = totalSize;
			dgvEIT.DataSource = byProgramIdentity;
		}

		protected override void FuncQuery()
		{
			if (new FormImportEIT().ShowDialog() == DialogResult.OK)
			{
				lbxPrg.Tag = -1;
				lbxPrg_SelectedIndexChanged(null, null);
				loger.AddLog(_FunctionName, EnumOSName.Add, "import eit form files");
			}
		}

		protected override void FuncAdd()
		{
			if (lbxPrg.Items.Count == 0)
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("NoAvailableProgram"));
				return;
			}
			ProgramInfo program = lbxPrg.SelectedItem as ProgramInfo;
			if (new FormEITManageDetail(program).ShowDialog() == DialogResult.OK)
			{
				lbxPrg.Tag = -1;
				lbxPrg_SelectedIndexChanged(null, null);
				loger.AddLog(_FunctionName, EnumOSName.Add, "");
			}
		}

		protected override void FuncEdit()
		{
			if (dgvEIT.Rows.Count >= 1)
			{
				DataGridViewRow currentRow = dgvEIT.CurrentRow;
				long batKey = Tool.GetInt32(currentRow.Cells[dcKey.Name].Value);
				EitInfo eitInfo = ((List<EitInfo>)dgvEIT.DataSource).Where((EitInfo b) => b.Identity == batKey).First();
				if (new FormEITManageDetail(eitInfo, PageEnum.Edit).ShowDialog() == DialogResult.OK)
				{
					lbxPrg.Tag = -1;
					lbxPrg_SelectedIndexChanged(null, null);
					loger.AddLog(_FunctionName, EnumOSName.Edit, "key:" + eitInfo.Identity);
				}
			}
		}

		protected override bool FuncDelete()
		{
			List<long> list = new List<long>();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DataGridViewRow item in (IEnumerable)dgvEIT.Rows)
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
				new EITInfoManager().Delete(list);
				lbxPrg.Tag = -1;
				lbxPrg_SelectedIndexChanged(null, null);
				loger.AddLog(_FunctionName, EnumOSName.Delete, stringBuilder.ToString());
				return true;
			}
			return false;
		}

		private void dgvEIT_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == dgvEIT.Columns[dcSelectCK.Name].Index && e.RowIndex >= 0 && dgvEIT.Rows.Count != 0)
			{
				DataGridViewRow dataGridViewRow = dgvEIT.Rows[e.RowIndex];
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
			foreach (DataGridViewRow item in (IEnumerable)dgvEIT.Rows)
			{
				item.Cells[dcSelectCK.Name].Value = true;
			}
		}

		private void tsmDeselectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvEIT.Rows)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.ProgramManagement.FormEITManage));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblTableType = new System.Windows.Forms.Label();
			this.cmbNIT = new System.Windows.Forms.ComboBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.cmbTS = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.lbxPrg = new System.Windows.Forms.ListBox();
			this.dgvEIT = new Com.Cn.Tool.UserControls.DGV();
			this.dcSelectCK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.dcEventId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcEventName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcEventRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcEndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcEventRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cmsSelectEITFile = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.eitPager = new DeXin.EPG.Tools.EPGUserControl.EPGPagination();
			base.pnlBase.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvEIT).BeginInit();
			this.cmsSelectEITFile.SuspendLayout();
			base.SuspendLayout();
			base.pnlBase.Controls.Add(this.splitContainer1);
			resources.ApplyResources(base.pnlBase, "pnlBase");
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
			this.splitContainer1.Panel2.Controls.Add(this.dgvEIT);
			this.splitContainer1.Panel2.Controls.Add(this.eitPager);
			this.groupBox1.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblTableType, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.cmbNIT, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.lblTableType, "lblTableType");
			this.lblTableType.Name = "lblTableType";
			resources.ApplyResources(this.cmbNIT, "cmbNIT");
			this.cmbNIT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbNIT.FormattingEnabled = true;
			this.cmbNIT.Name = "cmbNIT";
			this.cmbNIT.SelectedIndexChanged += new System.EventHandler(cmbNIT_SelectedIndexChanged);
			this.tableLayoutPanel1.SetColumnSpan(this.groupBox2, 2);
			this.groupBox2.Controls.Add(this.tableLayoutPanel2);
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.cmbTS, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.groupBox3, 0, 1);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			resources.ApplyResources(this.cmbTS, "cmbTS");
			this.cmbTS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTS.FormattingEnabled = true;
			this.cmbTS.Name = "cmbTS";
			this.cmbTS.SelectedIndexChanged += new System.EventHandler(cmbTS_SelectedIndexChanged);
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.tableLayoutPanel2.SetColumnSpan(this.groupBox3, 2);
			this.groupBox3.Controls.Add(this.lbxPrg);
			resources.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			resources.ApplyResources(this.lbxPrg, "lbxPrg");
			this.lbxPrg.FormattingEnabled = true;
			this.lbxPrg.Name = "lbxPrg";
			this.lbxPrg.SelectedIndexChanged += new System.EventHandler(lbxPrg_SelectedIndexChanged);
			this.dgvEIT.AllowUserToAddRows = false;
			this.dgvEIT.AllowUserToDeleteRows = false;
			this.dgvEIT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvEIT.Columns.AddRange(this.dcSelectCK, this.dcEventId, this.dcEventName, this.dcEventRemark, this.dcStartTime, this.dcEndTime, this.dcEventRate, this.dcKey);
			this.dgvEIT.ContextMenuStrip = this.cmsSelectEITFile;
			resources.ApplyResources(this.dgvEIT, "dgvEIT");
			this.dgvEIT.Name = "dgvEIT";
			this.dgvEIT.ReadOnly = true;
			this.dgvEIT.RowTemplate.Height = 23;
			this.dgvEIT.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvEIT.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvEIT_CellClick);
			this.dgvEIT.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvEIT_CellDoubleClick);
			resources.ApplyResources(this.dcSelectCK, "dcSelectCK");
			this.dcSelectCK.Name = "dcSelectCK";
			this.dcSelectCK.ReadOnly = true;
			this.dcSelectCK.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dcSelectCK.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dcEventId.DataPropertyName = "EventId";
			dataGridViewCellStyle.Format = "X4";
			dataGridViewCellStyle.NullValue = null;
			this.dcEventId.DefaultCellStyle = dataGridViewCellStyle;
			resources.ApplyResources(this.dcEventId, "dcEventId");
			this.dcEventId.Name = "dcEventId";
			this.dcEventId.ReadOnly = true;
			this.dcEventName.DataPropertyName = "EventName";
			resources.ApplyResources(this.dcEventName, "dcEventName");
			this.dcEventName.Name = "dcEventName";
			this.dcEventName.ReadOnly = true;
			this.dcEventRemark.DataPropertyName = "EventRemark";
			dataGridViewCellStyle2.Format = "X2";
			dataGridViewCellStyle2.NullValue = null;
			this.dcEventRemark.DefaultCellStyle = dataGridViewCellStyle2;
			resources.ApplyResources(this.dcEventRemark, "dcEventRemark");
			this.dcEventRemark.Name = "dcEventRemark";
			this.dcEventRemark.ReadOnly = true;
			this.dcStartTime.DataPropertyName = "StartTime";
			resources.ApplyResources(this.dcStartTime, "dcStartTime");
			this.dcStartTime.Name = "dcStartTime";
			this.dcStartTime.ReadOnly = true;
			this.dcEndTime.DataPropertyName = "EndTime";
			resources.ApplyResources(this.dcEndTime, "dcEndTime");
			this.dcEndTime.Name = "dcEndTime";
			this.dcEndTime.ReadOnly = true;
			this.dcEventRate.DataPropertyName = "EITRateSetting_RateName";
			resources.ApplyResources(this.dcEventRate, "dcEventRate");
			this.dcEventRate.Name = "dcEventRate";
			this.dcEventRate.ReadOnly = true;
			this.dcKey.DataPropertyName = "Identity";
			resources.ApplyResources(this.dcKey, "dcKey");
			this.dcKey.Name = "dcKey";
			this.dcKey.ReadOnly = true;
			this.cmsSelectEITFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmSelectAll, this.tsmDeselectAll });
			this.cmsSelectEITFile.Name = "cmsSelectEITFile";
			resources.ApplyResources(this.cmsSelectEITFile, "cmsSelectEITFile");
			this.tsmSelectAll.Name = "tsmSelectAll";
			resources.ApplyResources(this.tsmSelectAll, "tsmSelectAll");
			this.tsmSelectAll.Click += new System.EventHandler(tsmSelectAll_Click);
			this.tsmDeselectAll.Name = "tsmDeselectAll";
			resources.ApplyResources(this.tsmDeselectAll, "tsmDeselectAll");
			this.tsmDeselectAll.Click += new System.EventHandler(tsmDeselectAll_Click);
			resources.ApplyResources(this.eitPager, "eitPager");
			this.eitPager.Name = "eitPager";
			this.eitPager.PageIndex = 0;
			this.eitPager.PageSize = 50;
			this.eitPager.TotleCount = 0;
			this.eitPager.PageIndexChanged += new System.EventHandler(eitPager_PageIndexChanged);
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormEITManage";
			base.Controls.SetChildIndex(base.pnlBase, 0);
			base.pnlBase.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dgvEIT).EndInit();
			this.cmsSelectEITFile.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
