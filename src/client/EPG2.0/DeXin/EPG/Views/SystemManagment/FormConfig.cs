using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Net;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.SystemManagment
{
	public class FormConfig : FormDetailBase
	{
		private IContainer components = null;

		private GroupBox groupBox1;

		private TableLayoutPanel tableLayoutPanel2;

		private Label lblIPSMS;

		private TextBox txtIPSMS;

		private TextBox txtpwd;

		private Label lblDatabaseName;

		private Label lblPWD;

		private TextBox txtDataBase;

		private TextBox txtUid;

		private Label lblUserID;

		public bool EditedFlag = false;

		private SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.SystemManagment.FormConfig));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.lblIPSMS = new System.Windows.Forms.Label();
			this.txtIPSMS = new System.Windows.Forms.TextBox();
			this.txtpwd = new System.Windows.Forms.TextBox();
			this.lblDatabaseName = new System.Windows.Forms.Label();
			this.lblPWD = new System.Windows.Forms.Label();
			this.txtDataBase = new System.Windows.Forms.TextBox();
			this.txtUid = new System.Windows.Forms.TextBox();
			this.lblUserID = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.groupBox1);
			resources.ApplyResources(base.grbText, "grbText");
			this.groupBox1.Controls.Add(this.tableLayoutPanel2);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.lblIPSMS, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.txtIPSMS, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.txtpwd, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.lblDatabaseName, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.lblPWD, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.txtDataBase, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.txtUid, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.lblUserID, 0, 2);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			resources.ApplyResources(this.lblIPSMS, "lblIPSMS");
			this.lblIPSMS.Name = "lblIPSMS";
			resources.ApplyResources(this.txtIPSMS, "txtIPSMS");
			this.txtIPSMS.Name = "txtIPSMS";
			resources.ApplyResources(this.txtpwd, "txtpwd");
			this.txtpwd.Name = "txtpwd";
			resources.ApplyResources(this.lblDatabaseName, "lblDatabaseName");
			this.lblDatabaseName.Name = "lblDatabaseName";
			this.lblDatabaseName.Tag = "";
			resources.ApplyResources(this.lblPWD, "lblPWD");
			this.lblPWD.Name = "lblPWD";
			resources.ApplyResources(this.txtDataBase, "txtDataBase");
			this.txtDataBase.Name = "txtDataBase";
			resources.ApplyResources(this.txtUid, "txtUid");
			this.txtUid.Name = "txtUid";
			resources.ApplyResources(this.lblUserID, "lblUserID");
			this.lblUserID.Name = "lblUserID";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormConfig";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormConfig()
		{
			InitializeComponent();
			base.PageFlag = PageEnum.Add;
			_FunctionName = EnumFunctionName.SysFucnSystemSetting;
		}

		protected override void FormLoad()
		{
			base.MaximizeBox = false;
			base.FormBorderStyle = FormBorderStyle.Fixed3D;
			BindSMSConnectionstringToControl();
		}

		protected override void FunCancel()
		{
			base.FunCancel();
			Close();
		}

		private bool SaveSMSConfig(out string msg)
		{
			msg = ResourceManagement.GetLocationString("CAConfigSucceed");
			try
			{
				string value = Tool.GetValue(txtIPSMS);
				string value2 = Tool.GetValue(txtDataBase);
				string value3 = Tool.GetValue(txtUid);
				string value4 = Tool.GetValue(txtpwd);
				try
				{
					IPAddress.Parse(value);
				}
				catch
				{
					msg = lblIPSMS.Text + LocationControl.Ins.LocationItems[LacationEnum.InputFormatError];
					return false;
				}
				if (string.IsNullOrEmpty(value2))
				{
					msg = lblDatabaseName.Text + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
					return false;
				}
				if (string.IsNullOrEmpty(value3))
				{
					msg = lblUserID.Text + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
					return false;
				}
				if (string.IsNullOrEmpty(value4))
				{
					msg = lblPWD.Text + LocationControl.Ins.LocationItems[LacationEnum.IsNotNull];
					return false;
				}
				builder.InitialCatalog = value2;
				builder.DataSource = value;
				builder.UserID = value3;
				builder.Password = value4;
				try
				{
					EPGSettings.Instatce.DBConnectionString = builder.ConnectionString;
					msg = ResourceManagement.GetLocationString("DBConfigSucceeded");
					string text = ResourceManagement.GetLocationString("ModifyDBConfig") + " " + ResourceManagement.GetLocationString("DBConfigSucceeded") + "#" + value + "," + value2 + "," + value3;
					Tool.WriteTXTLog(text);
					EditedFlag = true;
				}
				catch
				{
					msg = ResourceManagement.GetLocationString("DBConfigFailed");
					return false;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return true;
		}

		private void BindSMSConnectionstringToControl()
		{
			SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(EPGSettings.Instatce.DBConnectionString);
			txtIPSMS.Text = sqlConnectionStringBuilder.DataSource.ToString();
			txtDataBase.Text = sqlConnectionStringBuilder.InitialCatalog;
			txtUid.Text = sqlConnectionStringBuilder.UserID;
			txtpwd.Text = sqlConnectionStringBuilder.Password;
		}

		protected override bool FunSubmit()
		{
			bool flag = false;
			string message = "";
			try
			{
				flag = SaveSMSConfig(out message);
				if (flag)
				{
					builder = new SqlConnectionStringBuilder(EPGSettings.Instatce.DBConnectionString);
				}
				Tool.ShowMessage(message);
			}
			catch (CustomException ex)
			{
				Tool.ShowMessage(ex.Message);
			}
			catch (Exception ex2)
			{
				Tool.ShowMessage(ex2);
			}
			return flag;
		}
	}
}
