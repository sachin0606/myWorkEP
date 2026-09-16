using System;
using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.SystemManagment
{
	public class FormDBRestore : FormDetailBase
	{
		public bool EditedFlag = false;

		private DBBackupRestoreBLL pageManger = new DBBackupRestoreBLL();

		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel3;

		private Label lblRestorePath;

		private TextBox txtRestore;

		private Label lblRestoreDB;

		private TextBox txtRestorePath;

		public FormDBRestore()
		{
			InitializeComponent();
			base.PageFlag = PageEnum.Add;
			_FunctionName = EnumFunctionName.SysFuncDBRestore;
		}

		protected override void FormLoad()
		{
		}

		protected override bool FunSubmit()
		{
			if (!ValidateInput())
			{
				return false;
			}
			try
			{
				string message = ResourceManagement.GetLocationString("RestoreFailure");
				string text = txtRestore.Text;
				bool flag = false;
				if (text.Length == 0)
				{
					message = lblRestoreDB.Text + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
				}
				else
				{
					string path = txtRestorePath.Text.Trim() + "\\" + text;
					if (pageManger.RestoreDB(path))
					{
						message = ResourceManagement.GetLocationString("RestoreSucceed");
					}
				}
				Tool.ShowMessage(message);
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
				return false;
			}
			return true;
		}

		protected override void FunCancel()
		{
			base.FunCancel();
			Close();
		}

		private void txtRestore_Validating(object sender, CancelEventArgs e)
		{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.SystemManagment.FormDBRestore));
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.txtRestore = new System.Windows.Forms.TextBox();
			this.txtRestorePath = new System.Windows.Forms.TextBox();
			this.lblRestorePath = new System.Windows.Forms.Label();
			this.lblRestoreDB = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel3);
			resources.ApplyResources(base.grbText, "grbText");
			this.tableLayoutPanel3.Controls.Add(this.txtRestore, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.txtRestorePath, 1, 3);
			this.tableLayoutPanel3.Controls.Add(this.lblRestorePath, 0, 3);
			this.tableLayoutPanel3.Controls.Add(this.lblRestoreDB, 0, 1);
			resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			resources.ApplyResources(this.txtRestore, "txtRestore");
			this.txtRestore.Name = "txtRestore";
			resources.ApplyResources(this.txtRestorePath, "txtRestorePath");
			this.txtRestorePath.Name = "txtRestorePath";
			resources.ApplyResources(this.lblRestorePath, "lblRestorePath");
			this.lblRestorePath.Name = "lblRestorePath";
			resources.ApplyResources(this.lblRestoreDB, "lblRestoreDB");
			this.lblRestoreDB.Name = "lblRestoreDB";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormDBRestore";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
