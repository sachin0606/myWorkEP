using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Com.Cn.Tool.UserControls
{
	public class DGV : DataGridView
	{
		private DataGridViewColumn CK;

		private ContextMenuStrip cms;

		private ToolStripMenuItem cmsSelectAll;

		private ToolStripMenuItem cmsDisSelectAll;

		private IContainer components;

		public DGV()
		{
			InitializeComponent();
			base.AllowUserToAddRows = false;
			base.AllowUserToDeleteRows = false;
			base.AutoGenerateColumns = false;
			base.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
		}

		public void InitDGV(params string[] noReadOnly)
		{
			Tool.SetDGVReadOnly(this, true, noReadOnly);
		}

		public void SetSelectedColumn(DataGridViewColumn column)
		{
			if (column != null)
			{
				ContextMenuStrip = cms;
				CK = column;
			}
		}

		private void cmsDisSelectaAll_Click(object sender, EventArgs e)
		{
			Tool.DGVSelect(this, CK, false);
		}

		private void cmsSelectAll_Click(object sender, EventArgs e)
		{
			Tool.DGVSelect(this, CK, true);
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Com.Cn.Tool.UserControls.DGV));
			this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmsSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsDisSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.cms.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this).BeginInit();
			base.SuspendLayout();
			this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.cmsSelectAll, this.cmsDisSelectAll });
			this.cms.Name = "cms";
			resources.ApplyResources(this.cms, "cms");
			this.cmsSelectAll.Name = "cmsSelectAll";
			resources.ApplyResources(this.cmsSelectAll, "cmsSelectAll");
			this.cmsSelectAll.Click += new System.EventHandler(cmsSelectAll_Click);
			this.cmsDisSelectAll.Name = "cmsDisSelectAll";
			resources.ApplyResources(this.cmsDisSelectAll, "cmsDisSelectAll");
			this.cmsDisSelectAll.Click += new System.EventHandler(cmsDisSelectaAll_Click);
			base.RowTemplate.Height = 23;
			base.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(DGV_CellContentClick);
			this.cms.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this).EndInit();
			base.ResumeLayout(false);
		}

		private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0 && CK != null && !base.Rows[e.RowIndex].ReadOnly && base.Columns[e.ColumnIndex] == CK)
			{
				base.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !Tool.GetBoolean(base.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
			}
			EndEdit();
		}
	}
}
