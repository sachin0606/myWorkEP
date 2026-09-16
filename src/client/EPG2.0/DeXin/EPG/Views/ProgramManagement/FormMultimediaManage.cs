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
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.ProgramManagement
{
	public class FormMultimediaManage : FormToolBase
	{
		private LogBLL loger = new LogBLL();

		private IContainer components = null;

		private ComboBox cmbNIT;

		private ComboBox cmbTS;

		private ListBox lbxPrg;

		private DGV dgvMedia;

		private SplitContainer splitContainer1;

		private GroupBox groupBox1;

		private TableLayoutPanel tableLayoutPanel2;

		private Label label3;

		private GroupBox groupBox2;

		private TableLayoutPanel tableLayoutPanel3;

		private Label label4;

		private GroupBox groupBox3;

		private DataGridViewCheckBoxColumn dcSelectCK;

		private DataGridViewTextBoxColumn dcPesPid;

		private DataGridViewTextBoxColumn dcPESType;

		private DataGridViewTextBoxColumn dcKey;

		private ContextMenuStrip cmsSelect;

		private ToolStripMenuItem tsmSelectAll;

		private ToolStripMenuItem tsmDeselectAll;

		public FormMultimediaManage()
		{
			InitializeComponent();
			IList<NitInfo> allNITInfos = new NITInfoManager().GetAllNITInfos();
			IList<BaseKey> lst = ((IEnumerable<BaseKey>)allNITInfos).ToList();
			Tool.BindingComboBox(lst, cmbNIT);
			SetMenuVisible(false, true, true, true, false, false, false, true, false);
			_FunctionName = EnumFunctionName.SysFuncMultimediaManage;
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
				dgvMedia.DataSource = null;
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
				dgvMedia.DataSource = null;
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
			IList<Multimedia> byProgramIdentity = new MultimediaManager().GetByProgramIdentity(@int);
			dgvMedia.DataSource = byProgramIdentity;
		}

		private void dgvMedia_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (dgvMedia.Rows.Count >= 1 && e.RowIndex >= 0)
			{
				DataGridViewRow currentRow = dgvMedia.CurrentRow;
				long key = Tool.GetInt32(currentRow.Cells[dcKey.Name].Value);
				Multimedia media = ((List<Multimedia>)dgvMedia.DataSource).Where((Multimedia b) => b.Id == key).First();
				new FormMultimediaDetail(media, PageEnum.Show).ShowDialog();
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
			if (new FormMultimediaDetail(program).ShowDialog() == DialogResult.OK)
			{
				lbxPrg_SelectedIndexChanged(null, null);
				loger.AddLog(_FunctionName, EnumOSName.Add, "");
			}
		}

		protected override void FuncEdit()
		{
			if (dgvMedia.Rows.Count >= 1)
			{
				DataGridViewRow currentRow = dgvMedia.CurrentRow;
				long batKey = Tool.GetInt32(currentRow.Cells[dcKey.Name].Value);
				Multimedia multimedia = ((List<Multimedia>)dgvMedia.DataSource).Where((Multimedia b) => b.Id == batKey).First();
				if (new FormMultimediaDetail(multimedia, PageEnum.Edit).ShowDialog() == DialogResult.OK)
				{
					lbxPrg_SelectedIndexChanged(null, null);
					loger.AddLog(_FunctionName, EnumOSName.Edit, "key:" + multimedia.Id);
				}
			}
		}

		protected override bool FuncDelete()
		{
			List<long> list = new List<long>();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DataGridViewRow item in (IEnumerable)dgvMedia.Rows)
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
				new MultimediaManager().Delete(list);
				lbxPrg_SelectedIndexChanged(null, null);
				loger.AddLog(_FunctionName, EnumOSName.Delete, stringBuilder.ToString());
				return true;
			}
			return false;
		}

		private void dgvMedia_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == dgvMedia.Columns[dcSelectCK.Name].Index && e.RowIndex >= 0 && dgvMedia.Rows.Count != 0)
			{
				DataGridViewRow dataGridViewRow = dgvMedia.Rows[e.RowIndex];
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
			foreach (DataGridViewRow item in (IEnumerable)dgvMedia.Rows)
			{
				item.Cells[dcSelectCK.Name].Value = true;
			}
		}

		private void tsmDeselectAll_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgvMedia.Rows)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.ProgramManagement.FormMultimediaManage));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.cmbNIT = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.lbxPrg = new System.Windows.Forms.ListBox();
			this.cmbTS = new System.Windows.Forms.ComboBox();
			this.dgvMedia = new Com.Cn.Tool.UserControls.DGV();
			this.dcSelectCK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.dcPesPid = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcPESType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dcKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cmsSelect = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
			base.pnlBase.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvMedia).BeginInit();
			this.cmsSelect.SuspendLayout();
			base.SuspendLayout();
			base.pnlBase.Controls.Add(this.splitContainer1);
			resources.ApplyResources(base.pnlBase, "pnlBase");
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
			this.splitContainer1.Panel2.Controls.Add(this.dgvMedia);
			this.groupBox1.Controls.Add(this.tableLayoutPanel2);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.cmbNIT, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.groupBox2, 0, 1);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			resources.ApplyResources(this.cmbNIT, "cmbNIT");
			this.cmbNIT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbNIT.FormattingEnabled = true;
			this.cmbNIT.Name = "cmbNIT";
			this.cmbNIT.SelectedIndexChanged += new System.EventHandler(cmbNIT_SelectedIndexChanged);
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.tableLayoutPanel2.SetColumnSpan(this.groupBox2, 2);
			this.groupBox2.Controls.Add(this.tableLayoutPanel3);
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
			this.tableLayoutPanel3.Controls.Add(this.label4, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.groupBox3, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.cmbTS, 1, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			this.tableLayoutPanel3.SetColumnSpan(this.groupBox3, 2);
			this.groupBox3.Controls.Add(this.lbxPrg);
			resources.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			resources.ApplyResources(this.lbxPrg, "lbxPrg");
			this.lbxPrg.FormattingEnabled = true;
			this.lbxPrg.Name = "lbxPrg";
			this.lbxPrg.SelectedIndexChanged += new System.EventHandler(lbxPrg_SelectedIndexChanged);
			resources.ApplyResources(this.cmbTS, "cmbTS");
			this.cmbTS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTS.FormattingEnabled = true;
			this.cmbTS.Name = "cmbTS";
			this.cmbTS.SelectedIndexChanged += new System.EventHandler(cmbTS_SelectedIndexChanged);
			this.dgvMedia.AllowUserToAddRows = false;
			this.dgvMedia.AllowUserToDeleteRows = false;
			this.dgvMedia.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvMedia.Columns.AddRange(this.dcSelectCK, this.dcPesPid, this.dcPESType, this.dcKey);
			this.dgvMedia.ContextMenuStrip = this.cmsSelect;
			resources.ApplyResources(this.dgvMedia, "dgvMedia");
			this.dgvMedia.Name = "dgvMedia";
			this.dgvMedia.ReadOnly = true;
			this.dgvMedia.RowTemplate.Height = 23;
			this.dgvMedia.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvMedia.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMedia_CellClick);
			this.dgvMedia.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMedia_CellDoubleClick);
			resources.ApplyResources(this.dcSelectCK, "dcSelectCK");
			this.dcSelectCK.Name = "dcSelectCK";
			this.dcSelectCK.ReadOnly = true;
			this.dcSelectCK.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dcSelectCK.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dcPesPid.DataPropertyName = "PesPid";
			dataGridViewCellStyle.Format = "X";
			dataGridViewCellStyle.NullValue = null;
			this.dcPesPid.DefaultCellStyle = dataGridViewCellStyle;
			resources.ApplyResources(this.dcPesPid, "dcPesPid");
			this.dcPesPid.Name = "dcPesPid";
			this.dcPesPid.ReadOnly = true;
			this.dcPESType.DataPropertyName = "PESType_StreamName";
			resources.ApplyResources(this.dcPESType, "dcPESType");
			this.dcPESType.Name = "dcPESType";
			this.dcPESType.ReadOnly = true;
			this.dcKey.DataPropertyName = "Id";
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
			base.Name = "FormMultimediaManage";
			base.Controls.SetChildIndex(base.pnlBase, 0);
			base.pnlBase.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dgvMedia).EndInit();
			this.cmsSelect.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
