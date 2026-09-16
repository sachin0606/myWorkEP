using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Com.Cn.Tool.UserControls
{
	public class CHKListBox : CheckedListBox
	{
		private ContextMenuStrip cms;

		private IContainer components;

		private ToolStripMenuItem cmsSelectAll;

		private ToolStripMenuItem cmsDisSelectAll;

		public CHKListBox()
		{
			InitializeComponent();
			base.CheckOnClick = true;
		}

		public void SetUseSelectCMS()
		{
			ContextMenuStrip = cms;
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Com.Cn.Tool.UserControls.CHKListBox));
			this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmsSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsDisSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.cms.SuspendLayout();
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
			resources.ApplyResources(this, "$this");
			this.cms.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void cmsDisSelectaAll_Click(object sender, EventArgs e)
		{
			Tool.SetCheckedListBoxAllChecked(this, false);
		}

		private void cmsSelectAll_Click(object sender, EventArgs e)
		{
			Tool.SetCheckedListBoxAllChecked(this, true);
		}

		private void cmsAntiSelect_Click(object sender, EventArgs e)
		{
			Tool.SetCheckedListBoxAntiChecked(this);
		}
	}
}
