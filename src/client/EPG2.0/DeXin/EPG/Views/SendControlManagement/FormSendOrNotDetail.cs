using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.SendControlManagement
{
	public class FormSendOrNotDetail : FormDetailBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private CheckBox ckPAT;

		private CheckBox ckCAT;

		private CheckBox ckPMT;

		private CheckBox ckBAT;

		private CheckBox ckSDTActual;

		private CheckBox ckSDTOther;

		private CheckBox ckNITActual;

		private CheckBox ckNITOther;

		private CheckBox ckTDT;

		private CheckBox ckTOT;

		private CheckBox ckEITActualPF;

		private CheckBox ckEITOtherPF;

		private CheckBox ckEITAcutalSchedue;

		private CheckBox ckEITOtherSchedue;

		private GroupBox groupBox1;

		private FlowLayoutPanel flowLayoutPanel1;

		private Label lblTableType;

		private ComboBox cmbTS;

		private TSInfoManager tsInfoBLL = new TSInfoManager();

		private SendOrNotInfoBLL sendBLL = new SendOrNotInfoBLL();

		private LogBLL logBll = new LogBLL();

		private SendOrNotInfo sendInfo = new SendOrNotInfo();

		private IList<TsInfo> UsefullTS { get; set; }

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.SendControlManagement.FormSendOrNotDetail));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.ckPAT = new System.Windows.Forms.CheckBox();
			this.ckCAT = new System.Windows.Forms.CheckBox();
			this.ckPMT = new System.Windows.Forms.CheckBox();
			this.ckBAT = new System.Windows.Forms.CheckBox();
			this.ckSDTActual = new System.Windows.Forms.CheckBox();
			this.ckSDTOther = new System.Windows.Forms.CheckBox();
			this.ckNITActual = new System.Windows.Forms.CheckBox();
			this.ckNITOther = new System.Windows.Forms.CheckBox();
			this.ckTDT = new System.Windows.Forms.CheckBox();
			this.ckTOT = new System.Windows.Forms.CheckBox();
			this.ckEITActualPF = new System.Windows.Forms.CheckBox();
			this.ckEITOtherPF = new System.Windows.Forms.CheckBox();
			this.ckEITAcutalSchedue = new System.Windows.Forms.CheckBox();
			this.ckEITOtherSchedue = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblTableType = new System.Windows.Forms.Label();
			this.cmbTS = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(base.error, "error");
			resources.ApplyResources(base.grbText, "grbText");
			base.grbText.Controls.Add(this.groupBox1);
			base.grbText.Controls.Add(this.flowLayoutPanel1);
			base.error.SetError(base.grbText, resources.GetString("grbText.Error"));
			base.error.SetIconAlignment(base.grbText, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("grbText.IconAlignment"));
			base.error.SetIconPadding(base.grbText, (int)resources.GetObject("grbText.IconPadding"));
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.ckPAT, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.ckCAT, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.ckPMT, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.ckBAT, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.ckSDTActual, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.ckSDTOther, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.ckNITActual, 1, 7);
			this.tableLayoutPanel1.Controls.Add(this.ckNITOther, 1, 8);
			this.tableLayoutPanel1.Controls.Add(this.ckTDT, 1, 9);
			this.tableLayoutPanel1.Controls.Add(this.ckTOT, 1, 10);
			this.tableLayoutPanel1.Controls.Add(this.ckEITActualPF, 1, 11);
			this.tableLayoutPanel1.Controls.Add(this.ckEITOtherPF, 1, 12);
			this.tableLayoutPanel1.Controls.Add(this.ckEITAcutalSchedue, 1, 13);
			this.tableLayoutPanel1.Controls.Add(this.ckEITOtherSchedue, 1, 14);
			base.error.SetError(this.tableLayoutPanel1, resources.GetString("tableLayoutPanel1.Error"));
			base.error.SetIconAlignment(this.tableLayoutPanel1, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("tableLayoutPanel1.IconAlignment"));
			base.error.SetIconPadding(this.tableLayoutPanel1, (int)resources.GetObject("tableLayoutPanel1.IconPadding"));
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.ckPAT, "ckPAT");
			this.ckPAT.Checked = true;
			this.ckPAT.CheckState = System.Windows.Forms.CheckState.Checked;
			base.error.SetError(this.ckPAT, resources.GetString("ckPAT.Error"));
			base.error.SetIconAlignment(this.ckPAT, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("ckPAT.IconAlignment"));
			base.error.SetIconPadding(this.ckPAT, (int)resources.GetObject("ckPAT.IconPadding"));
			this.ckPAT.Name = "ckPAT";
			this.ckPAT.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.ckCAT, "ckCAT");
			this.ckCAT.Checked = true;
			this.ckCAT.CheckState = System.Windows.Forms.CheckState.Checked;
			base.error.SetError(this.ckCAT, resources.GetString("ckCAT.Error"));
			base.error.SetIconAlignment(this.ckCAT, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("ckCAT.IconAlignment"));
			base.error.SetIconPadding(this.ckCAT, (int)resources.GetObject("ckCAT.IconPadding"));
			this.ckCAT.Name = "ckCAT";
			this.ckCAT.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.ckPMT, "ckPMT");
			this.ckPMT.Checked = true;
			this.ckPMT.CheckState = System.Windows.Forms.CheckState.Checked;
			base.error.SetError(this.ckPMT, resources.GetString("ckPMT.Error"));
			base.error.SetIconAlignment(this.ckPMT, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("ckPMT.IconAlignment"));
			base.error.SetIconPadding(this.ckPMT, (int)resources.GetObject("ckPMT.IconPadding"));
			this.ckPMT.Name = "ckPMT";
			this.ckPMT.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.ckBAT, "ckBAT");
			this.ckBAT.Checked = true;
			this.ckBAT.CheckState = System.Windows.Forms.CheckState.Checked;
			base.error.SetError(this.ckBAT, resources.GetString("ckBAT.Error"));
			base.error.SetIconAlignment(this.ckBAT, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("ckBAT.IconAlignment"));
			base.error.SetIconPadding(this.ckBAT, (int)resources.GetObject("ckBAT.IconPadding"));
			this.ckBAT.Name = "ckBAT";
			this.ckBAT.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.ckSDTActual, "ckSDTActual");
			this.ckSDTActual.Checked = true;
			this.ckSDTActual.CheckState = System.Windows.Forms.CheckState.Checked;
			base.error.SetError(this.ckSDTActual, resources.GetString("ckSDTActual.Error"));
			base.error.SetIconAlignment(this.ckSDTActual, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("ckSDTActual.IconAlignment"));
			base.error.SetIconPadding(this.ckSDTActual, (int)resources.GetObject("ckSDTActual.IconPadding"));
			this.ckSDTActual.Name = "ckSDTActual";
			this.ckSDTActual.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.ckSDTOther, "ckSDTOther");
			this.ckSDTOther.Checked = true;
			this.ckSDTOther.CheckState = System.Windows.Forms.CheckState.Checked;
			base.error.SetError(this.ckSDTOther, resources.GetString("ckSDTOther.Error"));
			base.error.SetIconAlignment(this.ckSDTOther, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("ckSDTOther.IconAlignment"));
			base.error.SetIconPadding(this.ckSDTOther, (int)resources.GetObject("ckSDTOther.IconPadding"));
			this.ckSDTOther.Name = "ckSDTOther";
			this.ckSDTOther.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.ckNITActual, "ckNITActual");
			this.ckNITActual.Checked = true;
			this.ckNITActual.CheckState = System.Windows.Forms.CheckState.Checked;
			base.error.SetError(this.ckNITActual, resources.GetString("ckNITActual.Error"));
			base.error.SetIconAlignment(this.ckNITActual, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("ckNITActual.IconAlignment"));
			base.error.SetIconPadding(this.ckNITActual, (int)resources.GetObject("ckNITActual.IconPadding"));
			this.ckNITActual.Name = "ckNITActual";
			this.ckNITActual.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.ckNITOther, "ckNITOther");
			this.ckNITOther.Checked = true;
			this.ckNITOther.CheckState = System.Windows.Forms.CheckState.Checked;
			base.error.SetError(this.ckNITOther, resources.GetString("ckNITOther.Error"));
			base.error.SetIconAlignment(this.ckNITOther, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("ckNITOther.IconAlignment"));
			base.error.SetIconPadding(this.ckNITOther, (int)resources.GetObject("ckNITOther.IconPadding"));
			this.ckNITOther.Name = "ckNITOther";
			this.ckNITOther.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.ckTDT, "ckTDT");
			this.ckTDT.Checked = true;
			this.ckTDT.CheckState = System.Windows.Forms.CheckState.Checked;
			base.error.SetError(this.ckTDT, resources.GetString("ckTDT.Error"));
			base.error.SetIconAlignment(this.ckTDT, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("ckTDT.IconAlignment"));
			base.error.SetIconPadding(this.ckTDT, (int)resources.GetObject("ckTDT.IconPadding"));
			this.ckTDT.Name = "ckTDT";
			this.ckTDT.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.ckTOT, "ckTOT");
			this.ckTOT.Checked = true;
			this.ckTOT.CheckState = System.Windows.Forms.CheckState.Checked;
			base.error.SetError(this.ckTOT, resources.GetString("ckTOT.Error"));
			base.error.SetIconAlignment(this.ckTOT, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("ckTOT.IconAlignment"));
			base.error.SetIconPadding(this.ckTOT, (int)resources.GetObject("ckTOT.IconPadding"));
			this.ckTOT.Name = "ckTOT";
			this.ckTOT.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.ckEITActualPF, "ckEITActualPF");
			this.ckEITActualPF.Checked = true;
			this.ckEITActualPF.CheckState = System.Windows.Forms.CheckState.Checked;
			base.error.SetError(this.ckEITActualPF, resources.GetString("ckEITActualPF.Error"));
			base.error.SetIconAlignment(this.ckEITActualPF, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("ckEITActualPF.IconAlignment"));
			base.error.SetIconPadding(this.ckEITActualPF, (int)resources.GetObject("ckEITActualPF.IconPadding"));
			this.ckEITActualPF.Name = "ckEITActualPF";
			this.ckEITActualPF.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.ckEITOtherPF, "ckEITOtherPF");
			this.ckEITOtherPF.Checked = true;
			this.ckEITOtherPF.CheckState = System.Windows.Forms.CheckState.Checked;
			base.error.SetError(this.ckEITOtherPF, resources.GetString("ckEITOtherPF.Error"));
			base.error.SetIconAlignment(this.ckEITOtherPF, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("ckEITOtherPF.IconAlignment"));
			base.error.SetIconPadding(this.ckEITOtherPF, (int)resources.GetObject("ckEITOtherPF.IconPadding"));
			this.ckEITOtherPF.Name = "ckEITOtherPF";
			this.ckEITOtherPF.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.ckEITAcutalSchedue, "ckEITAcutalSchedue");
			this.ckEITAcutalSchedue.Checked = true;
			this.ckEITAcutalSchedue.CheckState = System.Windows.Forms.CheckState.Checked;
			base.error.SetError(this.ckEITAcutalSchedue, resources.GetString("ckEITAcutalSchedue.Error"));
			base.error.SetIconAlignment(this.ckEITAcutalSchedue, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("ckEITAcutalSchedue.IconAlignment"));
			base.error.SetIconPadding(this.ckEITAcutalSchedue, (int)resources.GetObject("ckEITAcutalSchedue.IconPadding"));
			this.ckEITAcutalSchedue.Name = "ckEITAcutalSchedue";
			this.ckEITAcutalSchedue.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.ckEITOtherSchedue, "ckEITOtherSchedue");
			this.ckEITOtherSchedue.Checked = true;
			this.ckEITOtherSchedue.CheckState = System.Windows.Forms.CheckState.Checked;
			base.error.SetError(this.ckEITOtherSchedue, resources.GetString("ckEITOtherSchedue.Error"));
			base.error.SetIconAlignment(this.ckEITOtherSchedue, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("ckEITOtherSchedue.IconAlignment"));
			base.error.SetIconPadding(this.ckEITOtherSchedue, (int)resources.GetObject("ckEITOtherSchedue.IconPadding"));
			this.ckEITOtherSchedue.Name = "ckEITOtherSchedue";
			this.ckEITOtherSchedue.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			this.flowLayoutPanel1.Controls.Add(this.lblTableType);
			this.flowLayoutPanel1.Controls.Add(this.cmbTS);
			base.error.SetError(this.flowLayoutPanel1, resources.GetString("flowLayoutPanel1.Error"));
			base.error.SetIconAlignment(this.flowLayoutPanel1, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("flowLayoutPanel1.IconAlignment"));
			base.error.SetIconPadding(this.flowLayoutPanel1, (int)resources.GetObject("flowLayoutPanel1.IconPadding"));
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			resources.ApplyResources(this.lblTableType, "lblTableType");
			base.error.SetError(this.lblTableType, resources.GetString("lblTableType.Error"));
			base.error.SetIconAlignment(this.lblTableType, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lblTableType.IconAlignment"));
			base.error.SetIconPadding(this.lblTableType, (int)resources.GetObject("lblTableType.IconPadding"));
			this.lblTableType.Name = "lblTableType";
			resources.ApplyResources(this.cmbTS, "cmbTS");
			this.cmbTS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			base.error.SetError(this.cmbTS, resources.GetString("cmbTS.Error"));
			this.cmbTS.FormattingEnabled = true;
			base.error.SetIconAlignment(this.cmbTS, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("cmbTS.IconAlignment"));
			base.error.SetIconPadding(this.cmbTS, (int)resources.GetObject("cmbTS.IconPadding"));
			this.cmbTS.Name = "cmbTS";
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.tableLayoutPanel1);
			base.error.SetError(this.groupBox1, resources.GetString("groupBox1.Error"));
			base.error.SetIconAlignment(this.groupBox1, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("groupBox1.IconAlignment"));
			base.error.SetIconPadding(this.groupBox1, (int)resources.GetObject("groupBox1.IconPadding"));
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormSendOrNotDetail";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormSendOrNotDetail()
		{
			InitializeComponent();
		}

		public FormSendOrNotDetail(PageEnum pageFlag, IList<TsInfo> usefullTS)
		{
			InitializeComponent();
			base.PageFlag = pageFlag;
			UsefullTS = usefullTS;
		}

		public FormSendOrNotDetail(PageEnum pageFlag, long id, long networkTableID)
		{
			InitializeComponent();
			base.PageFlag = pageFlag;
			UsefullTS = tsInfoBLL.GetTSInfoByNetWorkIdentity(networkTableID);
			try
			{
				sendInfo = sendBLL.GetDataByID(id).FirstOrDefault();
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		protected override void FormLoad()
		{
			InitData();
			_FunctionName = EnumFunctionName.SysFuncSendOrNot;
			bool flag = true;
		}

		private void BindComboTS()
		{
			IList<BaseKey> lst = ((IEnumerable<BaseKey>)UsefullTS).ToList();
			Tool.BindingComboBox(lst, cmbTS);
		}

		private void cmbNetwork_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		protected override bool FunSubmit()
		{
			try
			{
				if (!ValidateInput(tableLayoutPanel1))
				{
					return false;
				}
				if (cmbTS.SelectedValue == null)
				{
					CommTool.ShowInputFormatError(lblTableType);
					return false;
				}
				if (base.PageFlag == PageEnum.Add && sendBLL.GetDataByTSTableID(Tool.GetInt64(cmbTS.SelectedValue)).Count > 0)
				{
					Tool.ShowMessage(ResourceManagement.GetLocationString("CannotAddKeyRepeatly"));
					return false;
				}
				BindControlToObject();
				switch (base.PageFlag)
				{
				case PageEnum.Add:
					sendBLL.Add(sendInfo);
					logBll.AddLog(EnumFunctionName.SysFuncSendOrNot, EnumOSName.Add, " id:" + sendInfo.Id);
					break;
				case PageEnum.Edit:
					sendBLL.Update(sendInfo);
					logBll.AddLog(EnumFunctionName.SysFuncSendOrNot, EnumOSName.Edit, " id:" + sendInfo.Id);
					break;
				}
				Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.OperateSucceed]);
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
			base.DialogResult = DialogResult.OK;
			return true;
		}

		private void InitData()
		{
			BindComboTS();
			switch (base.PageFlag)
			{
			case PageEnum.Add:
				break;
			case PageEnum.Edit:
				cmbTS.Enabled = false;
				BindObjectToControl();
				break;
			case PageEnum.Show:
				SetReadOnlyOnShow();
				BindObjectToControl();
				break;
			}
		}

		private void BindObjectToControl()
		{
			cmbTS.SelectedValue = sendInfo.TsTableId.ToString();
			ckBAT.Checked = sendInfo.Bat == 1;
			ckCAT.Checked = sendInfo.Cat == 1;
			ckEITActualPF.Checked = sendInfo.EitActPf == 1;
			ckEITAcutalSchedue.Checked = sendInfo.EitActSc == 1;
			ckEITOtherPF.Checked = sendInfo.EitOthPf == 1;
			ckEITOtherSchedue.Checked = sendInfo.EitOthSc == 1;
			ckNITActual.Checked = sendInfo.NitAct == 1;
			ckNITOther.Checked = sendInfo.NitOth == 1;
			ckPAT.Checked = sendInfo.Pat == 1;
			ckPMT.Checked = sendInfo.Pmt == 1;
			ckSDTActual.Checked = sendInfo.SdtAct == 1;
			ckSDTOther.Checked = sendInfo.SdtOth == 1;
			ckTDT.Checked = sendInfo.Tdt == 1;
			ckTOT.Checked = sendInfo.Tot == 1;
		}

		private void BindControlToObject()
		{
			sendInfo.TsTableId = Tool.GetInt64(cmbTS.SelectedValue);
			sendInfo.Bat = (ckBAT.Checked ? 1 : 0);
			sendInfo.Pat = (ckPAT.Checked ? 1 : 0);
			sendInfo.Cat = (ckCAT.Checked ? 1 : 0);
			sendInfo.Pmt = (ckPMT.Checked ? 1 : 0);
			sendInfo.SdtAct = (ckSDTActual.Checked ? 1 : 0);
			sendInfo.SdtOth = (ckSDTOther.Checked ? 1 : 0);
			sendInfo.NitAct = (ckNITActual.Checked ? 1 : 0);
			sendInfo.NitOth = (ckNITOther.Checked ? 1 : 0);
			sendInfo.Tot = (ckTOT.Checked ? 1 : 0);
			sendInfo.Tdt = (ckTDT.Checked ? 1 : 0);
			sendInfo.EitActPf = (ckEITActualPF.Checked ? 1 : 0);
			sendInfo.EitActSc = (ckEITAcutalSchedue.Checked ? 1 : 0);
			sendInfo.EitOthPf = (ckEITOtherPF.Checked ? 1 : 0);
			sendInfo.EitOthSc = (ckEITOtherSchedue.Checked ? 1 : 0);
		}

		private void SetReadOnlyOnShow()
		{
			cmbTS.Enabled = false;
			ckBAT.Enabled = false;
			ckPAT.Enabled = false;
			ckCAT.Enabled = false;
			ckPMT.Enabled = false;
			ckSDTActual.Enabled = false;
			ckSDTOther.Enabled = false;
			ckNITActual.Enabled = false;
			ckNITOther.Enabled = false;
			ckTOT.Enabled = false;
			ckTDT.Enabled = false;
			ckEITActualPF.Enabled = false;
			ckEITAcutalSchedue.Enabled = false;
			ckEITOtherPF.Enabled = false;
			ckEITOtherSchedue.Enabled = false;
		}
	}
}
