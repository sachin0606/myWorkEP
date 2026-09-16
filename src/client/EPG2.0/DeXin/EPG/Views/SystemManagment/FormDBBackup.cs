using System;
using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.BLL.ApplicationManagement;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.SystemManagment
{
	public class FormDBBackup : FormDetailBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel4;

		private Label lblBackupDBName;

		private Label lblBackupPath;

		private TextBox txtBackupPath;

		private TextBox txtBackup;

		public bool EditedFlag = false;

		private DBBackupRestoreBLL pageManger = new DBBackupRestoreBLL();

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.SystemManagment.FormDBBackup));
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.lblBackupDBName = new System.Windows.Forms.Label();
			this.lblBackupPath = new System.Windows.Forms.Label();
			this.txtBackupPath = new System.Windows.Forms.TextBox();
			this.txtBackup = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel4);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
			this.tableLayoutPanel4.Controls.Add(this.lblBackupDBName, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.lblBackupPath, 0, 3);
			this.tableLayoutPanel4.Controls.Add(this.txtBackupPath, 1, 3);
			this.tableLayoutPanel4.Controls.Add(this.txtBackup, 1, 1);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			resources.ApplyResources(this.lblBackupDBName, "lblBackupDBName");
			this.lblBackupDBName.Name = "lblBackupDBName";
			resources.ApplyResources(this.lblBackupPath, "lblBackupPath");
			this.lblBackupPath.Name = "lblBackupPath";
			resources.ApplyResources(this.txtBackupPath, "txtBackupPath");
			this.txtBackupPath.Name = "txtBackupPath";
			resources.ApplyResources(this.txtBackup, "txtBackup");
			this.txtBackup.Name = "txtBackup";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormDBBackup";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormDBBackup()
		{
			InitializeComponent();
			base.PageFlag = PageEnum.Add;
			_FunctionName = EnumFunctionName.SysFuncDBBackup;
		}

		protected override void FormLoad()
		{
			topTSMenu.Visible = false;
			base.MaximizeBox = false;
			base.FormBorderStyle = FormBorderStyle.Fixed3D;
			if (LoginUser.Ins.UserIns != null)
			{
			}
		}

		protected override bool FunSubmit()
		{
			bool flag = false;
			try
			{
				string message = ResourceManagement.GetLocationString("BackupFailure");
				string text = txtBackup.Text;
				if (text.Length == 0)
				{
					message = lblBackupDBName.Text + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
				}
				else
				{
					string path = txtBackupPath.Text.Trim() + "\\" + text;
					flag = pageManger.BackupDB(path);
					if (flag)
					{
						message = ResourceManagement.GetLocationString("BackupSucceed");
					}
				}
				Tool.ShowMessage(message);
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
			return flag;
		}

		protected override void FunCancel()
		{
			base.FunCancel();
			Close();
		}
	}
}
