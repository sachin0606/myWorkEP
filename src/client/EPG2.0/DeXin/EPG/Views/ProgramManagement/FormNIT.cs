using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.ProgramManagement
{
	public class FormNIT : FormDetailBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblNetWorkID;

		private Label lblNetWorkName;

		private Label lblVersion;

		private NumTextBox txtNetWorkID;

		private NumTextBox txtVersion;

		private ValidatTextBox txtNetWorkName;

		private Label label1;

		private ComboBox cmbLanguageCode;

		private CheckBox ckAutoVer;

		private NitInfo NITCurrent;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.ProgramManagement.FormNIT));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblNetWorkID = new System.Windows.Forms.Label();
			this.lblNetWorkName = new System.Windows.Forms.Label();
			this.lblVersion = new System.Windows.Forms.Label();
			this.txtNetWorkID = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.txtVersion = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.txtNetWorkName = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbLanguageCode = new System.Windows.Forms.ComboBox();
			this.ckAutoVer = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblNetWorkID, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblNetWorkName, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblVersion, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtNetWorkID, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtVersion, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtNetWorkName, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.cmbLanguageCode, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.ckAutoVer, 2, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.lblNetWorkID, "lblNetWorkID");
			this.lblNetWorkID.Name = "lblNetWorkID";
			resources.ApplyResources(this.lblNetWorkName, "lblNetWorkName");
			this.lblNetWorkName.Name = "lblNetWorkName";
			resources.ApplyResources(this.lblVersion, "lblVersion");
			this.lblVersion.Name = "lblVersion";
			this.txtNetWorkID.ForeColor = System.Drawing.Color.Black;
			this.txtNetWorkID.IsValidated = true;
			resources.ApplyResources(this.txtNetWorkID, "txtNetWorkID");
			this.txtNetWorkID.MaxValue = 65535;
			this.txtNetWorkID.Name = "txtNetWorkID";
			this.txtNetWorkID.NotNull = true;
			this.txtNetWorkID.SetMaxLength = 2;
			this.txtNetWorkID.Value = new byte[0];
			resources.ApplyResources(this.txtVersion, "txtVersion");
			this.txtVersion.ForeColor = System.Drawing.Color.Red;
			this.txtVersion.IsValidated = true;
			this.txtVersion.MaxValue = 31;
			this.txtVersion.Name = "txtVersion";
			this.txtVersion.NotNull = true;
			this.txtVersion.SetMaxLength = 2;
			this.txtVersion.Value = new byte[0];
			this.txtNetWorkName.IsValidated = true;
			resources.ApplyResources(this.txtNetWorkName, "txtNetWorkName");
			this.txtNetWorkName.Name = "txtNetWorkName";
			this.txtNetWorkName.NotNull = true;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.cmbLanguageCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLanguageCode.FormattingEnabled = true;
			resources.ApplyResources(this.cmbLanguageCode, "cmbLanguageCode");
			this.cmbLanguageCode.Name = "cmbLanguageCode";
			resources.ApplyResources(this.ckAutoVer, "ckAutoVer");
			this.ckAutoVer.Checked = true;
			this.ckAutoVer.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ckAutoVer.Name = "ckAutoVer";
			this.ckAutoVer.UseVisualStyleBackColor = true;
			this.ckAutoVer.CheckedChanged += new System.EventHandler(ckAutoVer_CheckedChanged);
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.Name = "FormNIT";
			base.Load += new System.EventHandler(FormNIT_Load);
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormNIT()
		{
			InitializeComponent();
			NITCurrent = new NitInfo();
			Tool.BindingComboBox(new DescriptorManagement().GetLanguageCode(), cmbLanguageCode);
			base.PageFlag = PageEnum.Add;
		}

		public FormNIT(NitInfo nit)
		{
			InitializeComponent();
			Tool.BindingComboBox(new DescriptorManagement().GetLanguageCode(), cmbLanguageCode);
			txtNetWorkID.Text = nit.NetworkId.ToString("X4");
			txtNetWorkName.Text = nit.NetworkName;
			if (nit.VersionNumber < 0)
			{
				txtVersion.Text = "00";
				ckAutoVer.Checked = true;
			}
			else
			{
				txtVersion.Text = nit.VersionNumber.ToString("X2");
				ckAutoVer.Checked = false;
			}
			cmbLanguageCode.SelectedValue = nit.LanguageCode;
			NITCurrent = nit;
			base.PageFlag = PageEnum.Edit;
		}

		private void FormNIT_Load(object sender, EventArgs e)
		{
			_FunctionName = EnumFunctionName.SysFuncNETWORK;
		}

		protected override bool FunSubmit()
		{
			if (!ValidateInput(tableLayoutPanel1))
			{
				return false;
			}
			NITInfoManager nITInfoManager = new NITInfoManager();
			NITCurrent.NetworkId = txtNetWorkID.ValueOfInt32;
			NITCurrent.NetworkName = txtNetWorkName.Text;
			if (ckAutoVer.Checked)
			{
				NITCurrent.VersionNumber = -1;
			}
			else
			{
				NITCurrent.VersionNumber = txtVersion.ValueOfInt32;
			}
			NITCurrent.LanguageCode = Tool.GetValue(cmbLanguageCode);
			try
			{
				if (nITInfoManager.SaveNITInfo(NITCurrent) == 1)
				{
					Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.SaveSucceed]);
					base.DialogResult = DialogResult.OK;
					return true;
				}
				Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.SaveFailure]);
				return false;
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex.Message);
				txtNetWorkID.Focus();
				txtNetWorkID.SelectAll();
				return false;
			}
		}

		protected override void FunCancel()
		{
			base.DialogResult = DialogResult.Cancel;
		}

		private void ckAutoVer_CheckedChanged(object sender, EventArgs e)
		{
			txtVersion.Enabled = !ckAutoVer.Checked;
			if (ckAutoVer.Checked)
			{
				txtVersion.Text = "00";
			}
			else if (NITCurrent != null && NITCurrent.VersionNumber > 0)
			{
				txtVersion.Text = NITCurrent.VersionNumber.ToString("X2");
			}
		}
	}
}
