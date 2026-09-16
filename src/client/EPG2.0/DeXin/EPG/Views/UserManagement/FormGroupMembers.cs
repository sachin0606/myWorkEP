using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool.UserControls;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.UserManagement
{
	public class FormGroupMembers : FormBase
	{
		private UserInfoManager userBLL = new UserInfoManager();

		private GroupInfo group = null;

		private IContainer components = null;

		private DataGridViewTextBoxColumn colUserID;

		private DataGridViewTextBoxColumn colUserName;

		private DGV dgv1;

		private GroupBox groupBox1;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		public FormGroupMembers()
		{
			InitializeComponent();
		}

		public FormGroupMembers(GroupInfo group)
		{
			InitializeComponent();
			this.group = group;
		}

		protected override void FormLoad()
		{
			IList<UserInfo> userByGroupID = userBLL.GetUserByGroupID(group.GroupID.ToString());
			dgv1.DataSource = userByGroupID;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.UserManagement.FormGroupMembers));
			this.colUserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgv1 = new Com.Cn.Tool.UserControls.DGV();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)this.dgv1).BeginInit();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.colUserID.DataPropertyName = "UserID";
			resources.ApplyResources(this.colUserID, "colUserID");
			this.colUserID.Name = "colUserID";
			this.colUserID.ReadOnly = true;
			this.colUserName.DataPropertyName = "UserName";
			resources.ApplyResources(this.colUserName, "colUserName");
			this.colUserName.Name = "colUserName";
			this.colUserName.ReadOnly = true;
			this.dgv1.AllowUserToAddRows = false;
			this.dgv1.AllowUserToDeleteRows = false;
			this.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv1.Columns.AddRange(this.dataGridViewTextBoxColumn1, this.dataGridViewTextBoxColumn2);
			resources.ApplyResources(this.dgv1, "dgv1");
			this.dgv1.Name = "dgv1";
			this.dgv1.ReadOnly = true;
			this.dgv1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridViewTextBoxColumn1.DataPropertyName = "UserID";
			resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.DataPropertyName = "UserName";
			resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.groupBox1.Controls.Add(this.dgv1);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.groupBox1);
			base.Name = "FormGroupMembers";
			((System.ComponentModel.ISupportInitialize)this.dgv1).EndInit();
			this.groupBox1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
