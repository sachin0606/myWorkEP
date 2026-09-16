using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.SendControlManagement
{
	public class FormPMTPIDReMappingDetail : FormDetailBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblProgramTableID;

		private Label lblPID;

		private ComboBox cmbProgram;

		private NumTextBox txtPID1;

		private FlowLayoutPanel flowLayoutPanel1;

		private Label label1;

		private PMTPIDReMappingBLL pmtpidBll = new PMTPIDReMappingBLL();

		private ProgramInfoManager prgBLL = new ProgramInfoManager();

		private AllReMappingPIDBLLHelper allRemppingBLLHelper = new AllReMappingPIDBLLHelper();

		private LogBLL logBll = new LogBLL();

		private int initalPid = 0;

		private long tsTableID = 0L;

		private PmtPidOutputInfo pmtPIDoutput = new PmtPidOutputInfo();

		private IList<ProgramInfo> UsefullPrgs { get; set; }

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.SendControlManagement.FormPMTPIDReMappingDetail));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblPID = new System.Windows.Forms.Label();
			this.cmbProgram = new System.Windows.Forms.ComboBox();
			this.lblProgramTableID = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.txtPID1 = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblPID, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.cmbProgram, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblProgramTableID, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 1);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.lblPID, "lblPID");
			this.lblPID.Name = "lblPID";
			resources.ApplyResources(this.cmbProgram, "cmbProgram");
			this.cmbProgram.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbProgram.FormattingEnabled = true;
			this.cmbProgram.Name = "cmbProgram";
			resources.ApplyResources(this.lblProgramTableID, "lblProgramTableID");
			this.lblProgramTableID.Name = "lblProgramTableID";
			this.flowLayoutPanel1.Controls.Add(this.txtPID1);
			this.flowLayoutPanel1.Controls.Add(this.label1);
			resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			resources.ApplyResources(this.txtPID1, "txtPID1");
			this.txtPID1.ForeColor = System.Drawing.Color.Black;
			this.txtPID1.IsValidated = true;
			this.txtPID1.MaxValue = 8191;
			this.txtPID1.Name = "txtPID1";
			this.txtPID1.NotNull = true;
			this.txtPID1.SetMaxLength = 2;
			this.txtPID1.Value = new byte[0];
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormPMTPIDReMappingDetail";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormPMTPIDReMappingDetail(PageEnum pageFlag, long tsTableID, IList<ProgramInfo> usefullPrg)
		{
			InitializeComponent();
			base.PageFlag = pageFlag;
			this.tsTableID = tsTableID;
			UsefullPrgs = usefullPrg;
			_FunctionName = EnumFunctionName.SysFuncPMTPIDReMapping;
		}

		public FormPMTPIDReMappingDetail(PageEnum pageFlag, long id, long tsTableID)
		{
			InitializeComponent();
			base.PageFlag = pageFlag;
			this.tsTableID = tsTableID;
			try
			{
				UsefullPrgs = prgBLL.GetByTSIdentity(tsTableID);
				pmtPIDoutput = pmtpidBll.GetDataByID(id).FirstOrDefault();
				initalPid = pmtPIDoutput.OutputPid;
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
			_FunctionName = EnumFunctionName.SysFuncPMTPIDReMapping;
		}

		protected override void FormLoad()
		{
			InitData();
		}

		protected override bool FunSubmit()
		{
			try
			{
				if (!ValidateInput(tableLayoutPanel1))
				{
					return false;
				}
				if (cmbProgram.SelectedValue == null)
				{
					CommTool.ShowInputFormatError(lblProgramTableID);
					return false;
				}
				if (base.PageFlag == PageEnum.Add && pmtpidBll.GetDataByProgramTableID(Tool.GetInt64(cmbProgram.SelectedValue)).Count > 0)
				{
					Tool.ShowMessage(ResourceManagement.GetLocationString("CannotAddKeyRepeatly"));
					return false;
				}
				BindControlToObject();
				try
				{
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary = allRemppingBLLHelper.GetAllPid();
					if (base.PageFlag == PageEnum.Edit)
					{
						dictionary.Remove(initalPid.ToString());
					}
					dictionary.Add(pmtPIDoutput.OutputPid.ToString(), pmtPIDoutput.OutputPid.ToString());
				}
				catch (ArgumentException)
				{
					Tool.ShowMessage(ResourceManagement.GetLocationString("PIDCannotRepeat"));
					return false;
				}
				switch (base.PageFlag)
				{
				case PageEnum.Add:
					pmtpidBll.Add(pmtPIDoutput);
					logBll.AddLog(EnumFunctionName.SysFuncPMTPIDReMapping, EnumOSName.Add, "id:" + pmtPIDoutput.Id);
					break;
				case PageEnum.Edit:
					pmtpidBll.Update(pmtPIDoutput);
					logBll.AddLog(EnumFunctionName.SysFuncPMTPIDReMapping, EnumOSName.Edit, "id:" + pmtPIDoutput.Id);
					break;
				}
				Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.OperateSucceed]);
			}
			catch (Exception ex2)
			{
				Tool.ShowMessage(ex2);
			}
			base.DialogResult = DialogResult.OK;
			return true;
		}

		private void BindComboProgram()
		{
			Tool.BindingComboBox(((IEnumerable<BaseKey>)UsefullPrgs).ToList(), cmbProgram);
		}

		private void BindControlToObject()
		{
			pmtPIDoutput.ProgramTableId = Tool.GetInt64(cmbProgram.SelectedValue);
			pmtPIDoutput.OutputPid = txtPID1.ValueOfInt32;
		}

		private void BindObjectToControl()
		{
			cmbProgram.SelectedValue = Tool.GetInt64(pmtPIDoutput.ProgramTableId);
			txtPID1.Text = pmtPIDoutput.OutputPid.ToString("X4");
		}

		private void InitData()
		{
			switch (base.PageFlag)
			{
			case PageEnum.Add:
				SetReadOnlyOnAdd();
				BindComboProgram();
				SetDefaultPID();
				break;
			case PageEnum.Edit:
				SetReadOnlyOnEdit();
				BindObjectToControl();
				BindComboProgram();
				break;
			case PageEnum.Show:
				SetReadOnlyOnShow();
				BindObjectToControl();
				BindComboProgram();
				break;
			}
		}

		private void SetReadOnlyOnAdd()
		{
			txtPID1.ReadOnly = false;
			cmbProgram.Enabled = true;
		}

		private void SetReadOnlyOnEdit()
		{
			txtPID1.ReadOnly = false;
			cmbProgram.Enabled = false;
		}

		private void SetReadOnlyOnShow()
		{
			txtPID1.ReadOnly = true;
			cmbProgram.Enabled = false;
		}

		private void SetDefaultPID()
		{
			int num = 0;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary = allRemppingBLLHelper.GetAllPid();
			num = ((dictionary.Count() <= 0) ? 31 : dictionary.Max((KeyValuePair<string, string> p) => int.Parse(p.Key)));
			txtPID1.Text = (num + 1).ToString("X4");
		}
	}
}
