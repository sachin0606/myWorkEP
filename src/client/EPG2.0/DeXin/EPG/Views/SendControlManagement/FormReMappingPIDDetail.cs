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
	public class FormReMappingPIDDetail : FormDetailBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private ComboBox cmbTS;

		private Label lblTSTableID;

		private Label lblPAT;

		private Label lblCAT;

		private NumTextBox txtPAT;

		private NumTextBox txtCAT;

		private Label lblSDTOrBAT;

		private Label lblNIT;

		private Label lblTOTOrTDT;

		private Label lblEIT;

		private NumTextBox txtSDTOrBAT;

		private NumTextBox txtNIT;

		private NumTextBox txtTOTorTDT;

		private NumTextBox txtEIT;

		private ValidatTextBox txtNetwork;

		private Label lblNetwork;

		private FlowLayoutPanel flowLayoutPanel2;

		private FlowLayoutPanel flowLayoutPanel1;

		private FlowLayoutPanel flowLayoutPanel3;

		private FlowLayoutPanel flowLayoutPanel4;

		private FlowLayoutPanel flowLayoutPanel5;

		private FlowLayoutPanel flowLayoutPanel6;

		private Label label1;

		private Label label2;

		private Label label3;

		private Label label4;

		private Label label5;

		private Label label6;

		private long networkTableID = 0L;

		private ReMappingPIDBLL remappingBLL = new ReMappingPIDBLL();

		private TSInfoManager tsInfoBLL = new TSInfoManager();

		private NITInfoManager nitBLL = new NITInfoManager();

		private AllReMappingPIDBLLHelper allRemppingBLLHelper = new AllReMappingPIDBLLHelper();

		private LogBLL logBLL = new LogBLL();

		private PidOutputInfo pidOutPutInfo = new PidOutputInfo();

		private Dictionary<string, string> allpid = new Dictionary<string, string>();

		private Dictionary<int, string> initialdics = null;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.SendControlManagement.FormReMappingPIDDetail));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.cmbTS = new System.Windows.Forms.ComboBox();
			this.lblTSTableID = new System.Windows.Forms.Label();
			this.lblPAT = new System.Windows.Forms.Label();
			this.lblCAT = new System.Windows.Forms.Label();
			this.lblSDTOrBAT = new System.Windows.Forms.Label();
			this.lblNIT = new System.Windows.Forms.Label();
			this.lblTOTOrTDT = new System.Windows.Forms.Label();
			this.lblEIT = new System.Windows.Forms.Label();
			this.txtNetwork = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.lblNetwork = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.txtPAT = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.txtCAT = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
			this.txtSDTOrBAT = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
			this.txtNIT = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
			this.txtTOTorTDT = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.txtEIT = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.label6 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			this.flowLayoutPanel4.SuspendLayout();
			this.flowLayoutPanel5.SuspendLayout();
			this.flowLayoutPanel6.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.cmbTS, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblTSTableID, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblPAT, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblCAT, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblSDTOrBAT, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblNIT, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.lblTOTOrTDT, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.lblEIT, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this.txtNetwork, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblNetwork, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel3, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel4, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel5, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel6, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 7);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.cmbTS, "cmbTS");
			this.cmbTS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTS.FormattingEnabled = true;
			this.cmbTS.Name = "cmbTS";
			resources.ApplyResources(this.lblTSTableID, "lblTSTableID");
			this.lblTSTableID.Name = "lblTSTableID";
			resources.ApplyResources(this.lblPAT, "lblPAT");
			this.lblPAT.Name = "lblPAT";
			resources.ApplyResources(this.lblCAT, "lblCAT");
			this.lblCAT.Name = "lblCAT";
			resources.ApplyResources(this.lblSDTOrBAT, "lblSDTOrBAT");
			this.lblSDTOrBAT.Name = "lblSDTOrBAT";
			resources.ApplyResources(this.lblNIT, "lblNIT");
			this.lblNIT.Name = "lblNIT";
			resources.ApplyResources(this.lblTOTOrTDT, "lblTOTOrTDT");
			this.lblTOTOrTDT.Name = "lblTOTOrTDT";
			resources.ApplyResources(this.lblEIT, "lblEIT");
			this.lblEIT.Name = "lblEIT";
			resources.ApplyResources(this.txtNetwork, "txtNetwork");
			this.txtNetwork.IsValidated = true;
			this.txtNetwork.Name = "txtNetwork";
			this.txtNetwork.NotNull = true;
			this.txtNetwork.ReadOnly = true;
			resources.ApplyResources(this.lblNetwork, "lblNetwork");
			this.lblNetwork.Name = "lblNetwork";
			this.flowLayoutPanel1.Controls.Add(this.txtPAT);
			this.flowLayoutPanel1.Controls.Add(this.label1);
			resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			resources.ApplyResources(this.txtPAT, "txtPAT");
			this.txtPAT.ForeColor = System.Drawing.Color.Black;
			this.txtPAT.IsValidated = true;
			this.txtPAT.MaxValue = 8191;
			this.txtPAT.Name = "txtPAT";
			this.txtPAT.NotNull = true;
			this.txtPAT.SetMaxLength = 2;
			this.txtPAT.Value = new byte[0];
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.flowLayoutPanel3.Controls.Add(this.txtCAT);
			this.flowLayoutPanel3.Controls.Add(this.label2);
			resources.ApplyResources(this.flowLayoutPanel3, "flowLayoutPanel3");
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			resources.ApplyResources(this.txtCAT, "txtCAT");
			this.txtCAT.ForeColor = System.Drawing.Color.Black;
			this.txtCAT.IsValidated = true;
			this.txtCAT.MaxValue = 8191;
			this.txtCAT.Name = "txtCAT";
			this.txtCAT.NotNull = true;
			this.txtCAT.SetMaxLength = 2;
			this.txtCAT.Value = new byte[0];
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			this.flowLayoutPanel4.Controls.Add(this.txtSDTOrBAT);
			this.flowLayoutPanel4.Controls.Add(this.label3);
			resources.ApplyResources(this.flowLayoutPanel4, "flowLayoutPanel4");
			this.flowLayoutPanel4.Name = "flowLayoutPanel4";
			resources.ApplyResources(this.txtSDTOrBAT, "txtSDTOrBAT");
			this.txtSDTOrBAT.ForeColor = System.Drawing.Color.Black;
			this.txtSDTOrBAT.IsValidated = true;
			this.txtSDTOrBAT.MaxValue = 8191;
			this.txtSDTOrBAT.Name = "txtSDTOrBAT";
			this.txtSDTOrBAT.NotNull = true;
			this.txtSDTOrBAT.SetMaxLength = 2;
			this.txtSDTOrBAT.Value = new byte[0];
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.flowLayoutPanel5.Controls.Add(this.txtNIT);
			this.flowLayoutPanel5.Controls.Add(this.label4);
			resources.ApplyResources(this.flowLayoutPanel5, "flowLayoutPanel5");
			this.flowLayoutPanel5.Name = "flowLayoutPanel5";
			resources.ApplyResources(this.txtNIT, "txtNIT");
			this.txtNIT.ForeColor = System.Drawing.Color.Black;
			this.txtNIT.IsValidated = true;
			this.txtNIT.MaxValue = 8191;
			this.txtNIT.Name = "txtNIT";
			this.txtNIT.NotNull = true;
			this.txtNIT.SetMaxLength = 2;
			this.txtNIT.Value = new byte[0];
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			this.flowLayoutPanel6.Controls.Add(this.txtTOTorTDT);
			this.flowLayoutPanel6.Controls.Add(this.label5);
			resources.ApplyResources(this.flowLayoutPanel6, "flowLayoutPanel6");
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			resources.ApplyResources(this.txtTOTorTDT, "txtTOTorTDT");
			this.txtTOTorTDT.ForeColor = System.Drawing.Color.Black;
			this.txtTOTorTDT.IsValidated = true;
			this.txtTOTorTDT.MaxValue = 8191;
			this.txtTOTorTDT.Name = "txtTOTorTDT";
			this.txtTOTorTDT.NotNull = true;
			this.txtTOTorTDT.SetMaxLength = 2;
			this.txtTOTorTDT.Value = new byte[0];
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			this.flowLayoutPanel2.Controls.Add(this.txtEIT);
			this.flowLayoutPanel2.Controls.Add(this.label6);
			resources.ApplyResources(this.flowLayoutPanel2, "flowLayoutPanel2");
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			resources.ApplyResources(this.txtEIT, "txtEIT");
			this.txtEIT.ForeColor = System.Drawing.Color.Black;
			this.txtEIT.IsValidated = true;
			this.txtEIT.MaxValue = 8191;
			this.txtEIT.Name = "txtEIT";
			this.txtEIT.NotNull = true;
			this.txtEIT.SetMaxLength = 2;
			this.txtEIT.Value = new byte[0];
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormReMappingPIDDetail";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.flowLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel4.PerformLayout();
			this.flowLayoutPanel5.ResumeLayout(false);
			this.flowLayoutPanel5.PerformLayout();
			this.flowLayoutPanel6.ResumeLayout(false);
			this.flowLayoutPanel6.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormReMappingPIDDetail(PageEnum pageFlag, long networkTableID, IList<TsInfo> usefullTS)
		{
			InitializeComponent();
			base.PageFlag = pageFlag;
			this.networkTableID = networkTableID;
			UsefullTS = usefullTS;
		}

		public FormReMappingPIDDetail(PageEnum pageFlag, long id, long networkTableID)
		{
			InitializeComponent();
			base.PageFlag = pageFlag;
			this.networkTableID = networkTableID;
			UsefullTS = tsInfoBLL.GetTSInfoByNetWorkIdentity(networkTableID);
			try
			{
				pidOutPutInfo = remappingBLL.GetDataByID(id).FirstOrDefault();
				initialdics = new Dictionary<int, string>();
				initialdics.Add(pidOutPutInfo.Pat, "");
				initialdics.Add(pidOutPutInfo.Cat, "");
				initialdics.Add(pidOutPutInfo.SdtOrBat, "");
				initialdics.Add(pidOutPutInfo.Nit, "");
				initialdics.Add(pidOutPutInfo.TotOrTdt, "");
				initialdics.Add(pidOutPutInfo.Eit, "");
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		protected override void FormLoad()
		{
			_FunctionName = EnumFunctionName.SysFuncRedefinePID;
			InitData();
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
					CommTool.ShowInputFormatError(lblTSTableID);
					return false;
				}
				if (base.PageFlag == PageEnum.Add && remappingBLL.GetDataByTSTableID(Tool.GetInt64(cmbTS.SelectedValue)).Count > 0)
				{
					Tool.ShowMessage(ResourceManagement.GetLocationString("CannotAddKeyRepeatly"));
					return false;
				}
				BindControlToObject();
				try
				{
					allpid.Clear();
					allRemppingBLLHelper = new AllReMappingPIDBLLHelper();
					allpid = allRemppingBLLHelper.GetAllPid();
					if (base.PageFlag == PageEnum.Edit)
					{
						foreach (KeyValuePair<int, string> initialdic in initialdics)
						{
							allpid.Remove(initialdic.Key.ToString());
						}
					}
					allpid.Add(pidOutPutInfo.Pat.ToString(), pidOutPutInfo.Pat.ToString());
					allpid.Add(pidOutPutInfo.Cat.ToString(), pidOutPutInfo.Cat.ToString());
					allpid.Add(pidOutPutInfo.SdtOrBat.ToString(), pidOutPutInfo.SdtOrBat.ToString());
					allpid.Add(pidOutPutInfo.Nit.ToString(), pidOutPutInfo.Nit.ToString());
					allpid.Add(pidOutPutInfo.TotOrTdt.ToString(), pidOutPutInfo.TotOrTdt.ToString());
					allpid.Add(pidOutPutInfo.Eit.ToString(), pidOutPutInfo.Eit.ToString());
				}
				catch (ArgumentException)
				{
					Tool.ShowMessage(ResourceManagement.GetLocationString("PIDCannotRepeat"));
					return false;
				}
				switch (base.PageFlag)
				{
				case PageEnum.Add:
					remappingBLL.Add(pidOutPutInfo);
					logBLL.AddLog(EnumFunctionName.SysFuncRedefinePID, EnumOSName.Add, "id:" + pidOutPutInfo.Id);
					break;
				case PageEnum.Edit:
					remappingBLL.Update(pidOutPutInfo);
					logBLL.AddLog(EnumFunctionName.SysFuncRedefinePID, EnumOSName.Edit, "id:" + pidOutPutInfo.Id);
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

		private void InitData()
		{
			txtNetwork.Text = nitBLL.GetNITInfoByNetworkIdentity(networkTableID).BKValue;
			BindComboTS();
			switch (base.PageFlag)
			{
			case PageEnum.Add:
				SetDefaultPID();
				break;
			case PageEnum.Edit:
				txtNetwork.ReadOnly = true;
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
			cmbTS.SelectedValue = pidOutPutInfo.TsTableId.ToString();
			txtCAT.Text = pidOutPutInfo.Cat.ToString("X4");
			txtEIT.Text = pidOutPutInfo.Eit.ToString("X4");
			txtNIT.Text = pidOutPutInfo.Nit.ToString("X4");
			txtPAT.Text = pidOutPutInfo.Pat.ToString("X4");
			txtSDTOrBAT.Text = pidOutPutInfo.SdtOrBat.ToString("X4");
			txtTOTorTDT.Text = pidOutPutInfo.TotOrTdt.ToString("X4");
		}

		private void BindControlToObject()
		{
			pidOutPutInfo.TsTableId = Tool.GetInt64(cmbTS.SelectedValue);
			pidOutPutInfo.Pat = txtPAT.ValueOfInt32;
			pidOutPutInfo.Cat = txtCAT.ValueOfInt32;
			pidOutPutInfo.Nit = txtNIT.ValueOfInt32;
			pidOutPutInfo.Eit = txtEIT.ValueOfInt32;
			pidOutPutInfo.SdtOrBat = txtSDTOrBAT.ValueOfInt32;
			pidOutPutInfo.TotOrTdt = txtTOTorTDT.ValueOfInt32;
		}

		private void SetReadOnlyOnShow()
		{
			txtCAT.ReadOnly = true;
			txtEIT.ReadOnly = true;
			txtNetwork.ReadOnly = true;
			txtNIT.ReadOnly = true;
			txtPAT.ReadOnly = true;
			txtSDTOrBAT.ReadOnly = true;
			txtTOTorTDT.ReadOnly = true;
			cmbTS.Enabled = false;
		}

		private void SetDefaultPID()
		{
			int num = 0;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary = allRemppingBLLHelper.GetAllPid();
			num = ((dictionary.Count() <= 0) ? 31 : dictionary.Max((KeyValuePair<string, string> p) => int.Parse(p.Key)));
			txtPAT.Text = (num + 1).ToString("X4");
			txtCAT.Text = (num + 2).ToString("X4");
			txtSDTOrBAT.Text = (num + 3).ToString("X4");
			txtNIT.Text = (num + 4).ToString("X4");
			txtTOTorTDT.Text = (num + 5).ToString("X4");
			txtEIT.Text = (num + 6).ToString("X4");
		}
	}
}
