using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using Com.Cn.Tool.UserControls;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Resource.ResourcesSpecialFiles;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.ProgramManagement
{
	public class FormEITManageDetail : FormDetailBase
	{
		private EitInfo CurrEIT;

		private ProgramInfo CurrProgram;

		private EITInfoManager eitManager = new EITInfoManager();

		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblTSName;

		private Label lblTSId;

		private Label lblVersion;

		private Label lblNIT;

		private Label lblOrgNIT;

		private NumTextBox txtEITId;

		private CheckBox cbxAutoEITId;

		private DateTimeLong dtStart;

		private DateTimeLong dtEnd;

		private ValidatTextBox txtEITRemark;

		private ValidatTextBox txtEITName;

		private Label label1;

		private ValidatTextBox txtEITLRemark;

		private Label label2;

		private ComboBox cmbRatings;

		private CheckBox cbxEnableRate;

		public FormEITManageDetail(ProgramInfo Program)
		{
			InitializeComponent();
			initRatingsCmb();
			CurrProgram = Program;
			CurrEIT = new EitInfo
			{
				ProgramTableId = Program.Identity,
				Program = Program
			};
			base.PageFlag = PageEnum.Add;
			dtStart.DT = (dtEnd.DT = DateTime.Now);
		}

		public FormEITManageDetail(EitInfo EIT, PageEnum DoModel)
		{
			InitializeComponent();
			initRatingsCmb();
			CurrProgram = EIT.Program;
			CurrEIT = EIT;
			txtEITId.Text = EIT.EventId.ToString("X4");
			txtEITName.Text = EIT.EventName;
			txtEITRemark.Text = EIT.EventRemark;
			txtEITLRemark.Text = EIT.EventLongRemark;
			dtStart.DT = EIT.StartTime;
			dtEnd.DT = EIT.EndTime;
			if (EIT.EitRateValue.HasValue)
			{
				cbxEnableRate.Checked = true;
				List<EitRateSetting> eitRateSetting = new SystemSettingBLL().GetEitRateSetting();
				if (eitRateSetting.Count((EitRateSetting x) => x.RateValue == EIT.EitRateValue) > 0)
				{
					cmbRatings.SelectedValue = EIT.EitRateValue;
				}
				else
				{
					cmbRatings.SelectedItem = null;
					cmbRatings.Text = EIT.EitRateValue.ToString();
				}
			}
			base.PageFlag = DoModel;
		}

		private void FormEITManageDetail_Load(object sender, EventArgs e)
		{
			_FunctionName = EnumFunctionName.SysFuncEITManage;
			cbxAutoEITId_CheckedChanged(sender, e);
			if (base.PageFlag == PageEnum.Show)
			{
				foreach (Control control in tableLayoutPanel1.Controls)
				{
					if (control is TextBoxBase)
					{
						((TextBoxBase)control).ReadOnly = true;
					}
					else
					{
						control.Enabled = false;
					}
				}
				return;
			}
			if (base.PageFlag == PageEnum.Edit)
			{
				txtEITId.ReadOnly = true;
				cbxAutoEITId.Enabled = false;
			}
		}

		private void initRatingsCmb()
		{
			cmbRatings.DataSource = BaseData.Ins.GetEitRateSetting();
			cmbRatings.ValueMember = "RateValue";
			cmbRatings.DisplayMember = "RateName";
		}

		private void cbxAutoEITId_CheckedChanged(object sender, EventArgs e)
		{
			if (base.PageFlag == PageEnum.Add)
			{
				if (cbxAutoEITId.Checked)
				{
					txtEITId.ReadOnly = true;
					txtEITId.Text = eitManager.GetEITIdFromEITInfo(CurrProgram).ToString("X4");
				}
				else
				{
					txtEITId.ReadOnly = false;
				}
			}
		}

		protected override bool FunSubmit()
		{
			if (!ValidateInput())
			{
				return false;
			}
			if (dtStart.DT >= dtEnd.DT)
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("StartTimeSmallerThanEnd"));
				return false;
			}
			CurrEIT.EventId = txtEITId.ValueOfInt32;
			CurrEIT.EventName = txtEITName.Text;
			CurrEIT.EventRemark = txtEITRemark.Text;
			CurrEIT.EventLongRemark = txtEITLRemark.Text;
			CurrEIT.StartTime = dtStart.DT;
			CurrEIT.EndTime = dtEnd.DT;
			int result = 255;
			int.TryParse(ConfigurationManager.AppSettings["EITShortRemarkValueInputExceedLimited"], out result);
			if (CurrEIT.EventRemark.Length > result)
			{
				Tool.ShowMessage(lblTSId.Text + ResourceManagement.GetLocationString("ValueInputExceedLimited"));
				return false;
			}
			if (cbxEnableRate.Checked)
			{
				if (cmbRatings.SelectedValue != null)
				{
					CurrEIT.EitRateValue = Tool.GetInt32(cmbRatings.SelectedValue);
				}
				else
				{
					CurrEIT.EitRateValue = Tool.GetInt32(cmbRatings.Text);
				}
			}
			else
			{
				CurrEIT.EitRateValue = null;
				CurrEIT.EITRateSetting = null;
			}
			if (eitManager.Save(CurrEIT) == 1)
			{
				base.DialogResult = DialogResult.OK;
				return true;
			}
			return false;
		}

		protected override void FunCancel()
		{
			base.FunCancel();
			base.DialogResult = DialogResult.Cancel;
		}

		private void cbxEnableRate_CheckedChanged(object sender, EventArgs e)
		{
			if (cbxEnableRate.Checked)
			{
				SystemSetting systemSetting = new SystemSettingBLL().GetSystemSetting(SystemSettingCode.CountryCode);
				if (systemSetting == null)
				{
					cbxEnableRate.Checked = false;
					throw new CustomException(ResourceSpecial.PlsCfgCountryCodeFirst, MessageType.SHOW);
				}
			}
			cmbRatings.Enabled = cbxEnableRate.Checked;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.ProgramManagement.FormEITManageDetail));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblTSName = new System.Windows.Forms.Label();
			this.lblTSId = new System.Windows.Forms.Label();
			this.lblVersion = new System.Windows.Forms.Label();
			this.lblNIT = new System.Windows.Forms.Label();
			this.lblOrgNIT = new System.Windows.Forms.Label();
			this.txtEITId = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.cbxAutoEITId = new System.Windows.Forms.CheckBox();
			this.dtStart = new Com.Cn.Tool.UserControls.DateTimeLong();
			this.dtEnd = new Com.Cn.Tool.UserControls.DateTimeLong();
			this.txtEITRemark = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.txtEITName = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtEITLRemark = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbRatings = new System.Windows.Forms.ComboBox();
			this.cbxEnableRate = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblTSName, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblTSId, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblVersion, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.lblNIT, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblOrgNIT, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtEITId, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.cbxAutoEITId, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.dtStart, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.dtEnd, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.txtEITRemark, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtEITName, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtEITLRemark, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this.cmbRatings, 1, 7);
			this.tableLayoutPanel1.Controls.Add(this.cbxEnableRate, 2, 7);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.lblTSName, "lblTSName");
			this.lblTSName.Name = "lblTSName";
			resources.ApplyResources(this.lblTSId, "lblTSId");
			this.lblTSId.Name = "lblTSId";
			resources.ApplyResources(this.lblVersion, "lblVersion");
			this.lblVersion.Name = "lblVersion";
			resources.ApplyResources(this.lblNIT, "lblNIT");
			this.lblNIT.Name = "lblNIT";
			resources.ApplyResources(this.lblOrgNIT, "lblOrgNIT");
			this.lblOrgNIT.Name = "lblOrgNIT";
			resources.ApplyResources(this.txtEITId, "txtEITId");
			this.txtEITId.ForeColor = System.Drawing.Color.Black;
			this.txtEITId.IsValidated = true;
			this.txtEITId.MaxValue = 65535;
			this.txtEITId.Name = "txtEITId";
			this.txtEITId.NotNull = true;
			this.txtEITId.SetMaxLength = 2;
			this.txtEITId.Value = new byte[0];
			resources.ApplyResources(this.cbxAutoEITId, "cbxAutoEITId");
			this.cbxAutoEITId.Checked = true;
			this.cbxAutoEITId.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbxAutoEITId.Name = "cbxAutoEITId";
			this.cbxAutoEITId.UseVisualStyleBackColor = true;
			this.cbxAutoEITId.CheckedChanged += new System.EventHandler(cbxAutoEITId_CheckedChanged);
			this.dtStart.AllowDrop = true;
			this.dtStart.DT = new System.DateTime(2012, 5, 15, 10, 25, 17, 0);
			resources.ApplyResources(this.dtStart, "dtStart");
			this.dtStart.Name = "dtStart";
			this.dtStart.ReadOnly = false;
			this.dtEnd.AllowDrop = true;
			this.dtEnd.DT = new System.DateTime(2012, 5, 15, 10, 25, 17, 0);
			resources.ApplyResources(this.dtEnd, "dtEnd");
			this.dtEnd.Name = "dtEnd";
			this.dtEnd.ReadOnly = false;
			resources.ApplyResources(this.txtEITRemark, "txtEITRemark");
			this.txtEITRemark.IsValidated = true;
			this.txtEITRemark.Name = "txtEITRemark";
			this.txtEITRemark.NotNull = false;
			resources.ApplyResources(this.txtEITName, "txtEITName");
			this.txtEITName.IsValidated = true;
			this.txtEITName.Name = "txtEITName";
			this.txtEITName.NotNull = true;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			resources.ApplyResources(this.txtEITLRemark, "txtEITLRemark");
			this.txtEITLRemark.IsValidated = true;
			this.txtEITLRemark.Name = "txtEITLRemark";
			this.txtEITLRemark.NotNull = false;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			resources.ApplyResources(this.cmbRatings, "cmbRatings");
			this.cmbRatings.FormattingEnabled = true;
			this.cmbRatings.Name = "cmbRatings";
			resources.ApplyResources(this.cbxEnableRate, "cbxEnableRate");
			this.cbxEnableRate.Name = "cbxEnableRate";
			this.cbxEnableRate.UseVisualStyleBackColor = true;
			this.cbxEnableRate.CheckedChanged += new System.EventHandler(cbxEnableRate_CheckedChanged);
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.Name = "FormEITManageDetail";
			base.Load += new System.EventHandler(FormEITManageDetail_Load);
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			base.grbText.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
