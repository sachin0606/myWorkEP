using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool;
using DeXin.EPG.Tools.EPGUserControl;

namespace DeXin.EPG.Views.BaseForm
{
	public class QueryBaseFormDynamicConditionNew : FormBase
	{
		private IContainer components = null;

		protected GroupBox gbContext;

		protected Button btnCancel;

		protected Button btnQuery;

		protected DynamicConditionSearchNew smsDynamicConditionSearchNew;

		public string conditions = "";

		protected string QueryText
		{
			get
			{
				return Text;
			}
			set
			{
				Text = value;
			}
		}

		protected string GroupBoxText
		{
			get
			{
				return gbContext.Text;
			}
			set
			{
				gbContext.Text = value;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.BaseForm.QueryBaseFormDynamicConditionNew));
			this.gbContext = new System.Windows.Forms.GroupBox();
			this.smsDynamicConditionSearchNew = new DeXin.EPG.Tools.EPGUserControl.DynamicConditionSearchNew();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnQuery = new System.Windows.Forms.Button();
			this.gbContext.SuspendLayout();
			base.SuspendLayout();
			this.gbContext.Controls.Add(this.smsDynamicConditionSearchNew);
			resources.ApplyResources(this.gbContext, "gbContext");
			this.gbContext.Name = "gbContext";
			this.gbContext.TabStop = false;
			resources.ApplyResources(this.smsDynamicConditionSearchNew, "smsDynamicConditionSearchNew");
			this.smsDynamicConditionSearchNew.basekeys = (System.Collections.Generic.Dictionary<string, string>)resources.GetObject("smsDynamicConditionSearchNew.basekeys");
			this.smsDynamicConditionSearchNew.Name = "smsDynamicConditionSearchNew";
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
			resources.ApplyResources(this.btnQuery, "btnQuery");
			this.btnQuery.Name = "btnQuery";
			this.btnQuery.UseVisualStyleBackColor = true;
			this.btnQuery.Click += new System.EventHandler(btnQuery_Click);
			base.AcceptButton = this.btnQuery;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnQuery);
			base.Controls.Add(this.gbContext);
			base.Name = "QueryBaseFormDynamicConditionNew";
			this.gbContext.ResumeLayout(false);
			this.gbContext.PerformLayout();
			base.ResumeLayout(false);
		}

		public QueryBaseFormDynamicConditionNew()
		{
			InitializeComponent();
		}

		private void btnQuery_Click(object sender, EventArgs e)
		{
			try
			{
				smsDynamicConditionSearchNew.PrepareCondition();
				conditions = smsDynamicConditionSearchNew.Conditions;
				FunQuery();
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			try
			{
				FunCancel();
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		protected virtual void FunQuery()
		{
		}

		protected virtual void FunCancel()
		{
			Close();
		}

		protected override void FormLoad()
		{
			base.FormBorderStyle = FormBorderStyle.Fixed3D;
			base.WindowState = FormWindowState.Normal;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.StartPosition = FormStartPosition.CenterParent;
		}
	}
}
