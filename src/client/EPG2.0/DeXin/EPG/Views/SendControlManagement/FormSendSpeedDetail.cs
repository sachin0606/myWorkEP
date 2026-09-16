using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.SendControlManagement
{
	public class FormSendSpeedDetail : FormDetailBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel2;

		private Label lblSpeed;

		private Label label1;

		private Label label2;

		private Label label3;

		private Label label4;

		private Label label5;

		private Label label6;

		private Label label7;

		private ValidatTextBox txtPAT;

		private ValidatTextBox txtCAT;

		private ValidatTextBox txtPMT;

		private ValidatTextBox txtNIT_ACT;

		private ValidatTextBox txtNIT_OTH;

		private ValidatTextBox txtBAT;

		private ValidatTextBox txtSDT_ACT;

		private ValidatTextBox txtSDT_OTH;

		private ValidatTextBox txtTDT;

		private Label label13;

		private Label label12;

		private ValidatTextBox txtTOT;

		private Label label11;

		private ValidatTextBox txtEIT_OTH_SC;

		private Label label10;

		private ValidatTextBox txtEIT_OTH_PF;

		private Label label9;

		private ValidatTextBox txtEIT_ACT_SC;

		private Label label8;

		private Label lblSDT_OTH;

		private ValidatTextBox txtEIT_ACT_PF;

		private Label lblTableType;

		private ComboBox cmbTS;

		private GroupBox grbLoaction;

		private FlowLayoutPanel flowLayoutPanel1;

		private Button btResetPAT;

		private Button btResetCAT;

		private Button btResetPMT;

		private Button btResetNIT_ACT;

		private Button btResetNIT_OTH;

		private Button btResetBAT;

		private Button btResetSDT_ACT;

		private Button btResetSDT_OTH;

		private Button btResetEIT_ACT_PF;

		private Button btResetEIT_ACT_SC;

		private Button btResetEIT_OTH_PF;

		private Button btResetEIT_OTH_SC;

		private Button btResetTOT;

		private Button btResetTDT;

		private SendSpeedInfo controller;

		private SendSpeedInfoManager controlManager = new SendSpeedInfoManager();

		private static readonly SendSpeedInfo DefaultSendSpeed = new SendSpeedInfo
		{
			Bat = 3000,
			Cat = 1000,
			EitActPf = 1000,
			EitActSc = 3000,
			EitOthPf = 2000,
			EitOthSc = 4000,
			NitAct = 1000,
			NitOth = 4000,
			Pat = 300,
			Pmt = 300,
			SdtAct = 1000,
			SdtOth = 4000,
			Tdt = 2000,
			Tot = 2000
		};

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.SendControlManagement.FormSendSpeedDetail));
			this.txtEIT_ACT_PF = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.lblSDT_OTH = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.txtEIT_ACT_SC = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.txtEIT_OTH_PF = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.txtEIT_OTH_SC = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.txtTOT = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.txtTDT = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.txtSDT_OTH = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.txtSDT_ACT = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.txtBAT = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.txtNIT_OTH = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.txtNIT_ACT = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.txtPMT = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.txtCAT = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.txtPAT = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblSpeed = new System.Windows.Forms.Label();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.btResetPAT = new System.Windows.Forms.Button();
			this.btResetCAT = new System.Windows.Forms.Button();
			this.btResetPMT = new System.Windows.Forms.Button();
			this.btResetNIT_ACT = new System.Windows.Forms.Button();
			this.btResetNIT_OTH = new System.Windows.Forms.Button();
			this.btResetBAT = new System.Windows.Forms.Button();
			this.btResetSDT_ACT = new System.Windows.Forms.Button();
			this.btResetSDT_OTH = new System.Windows.Forms.Button();
			this.btResetEIT_ACT_PF = new System.Windows.Forms.Button();
			this.btResetEIT_ACT_SC = new System.Windows.Forms.Button();
			this.btResetEIT_OTH_PF = new System.Windows.Forms.Button();
			this.btResetEIT_OTH_SC = new System.Windows.Forms.Button();
			this.btResetTOT = new System.Windows.Forms.Button();
			this.btResetTDT = new System.Windows.Forms.Button();
			this.cmbTS = new System.Windows.Forms.ComboBox();
			this.lblTableType = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.grbLoaction = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.grbLoaction.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.grbLoaction);
			base.grbText.Controls.Add(this.flowLayoutPanel1);
			resources.ApplyResources(base.grbText, "grbText");
			this.txtEIT_ACT_PF.IsValidated = true;
			resources.ApplyResources(this.txtEIT_ACT_PF, "txtEIT_ACT_PF");
			this.txtEIT_ACT_PF.Name = "txtEIT_ACT_PF";
			this.txtEIT_ACT_PF.NotNull = true;
			resources.ApplyResources(this.lblSDT_OTH, "lblSDT_OTH");
			this.lblSDT_OTH.Name = "lblSDT_OTH";
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			this.txtEIT_ACT_SC.IsValidated = true;
			resources.ApplyResources(this.txtEIT_ACT_SC, "txtEIT_ACT_SC");
			this.txtEIT_ACT_SC.Name = "txtEIT_ACT_SC";
			this.txtEIT_ACT_SC.NotNull = true;
			resources.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			this.txtEIT_OTH_PF.IsValidated = true;
			resources.ApplyResources(this.txtEIT_OTH_PF, "txtEIT_OTH_PF");
			this.txtEIT_OTH_PF.Name = "txtEIT_OTH_PF";
			this.txtEIT_OTH_PF.NotNull = true;
			resources.ApplyResources(this.label10, "label10");
			this.label10.Name = "label10";
			this.txtEIT_OTH_SC.IsValidated = true;
			resources.ApplyResources(this.txtEIT_OTH_SC, "txtEIT_OTH_SC");
			this.txtEIT_OTH_SC.Name = "txtEIT_OTH_SC";
			this.txtEIT_OTH_SC.NotNull = true;
			resources.ApplyResources(this.label11, "label11");
			this.label11.Name = "label11";
			this.txtTOT.IsValidated = true;
			resources.ApplyResources(this.txtTOT, "txtTOT");
			this.txtTOT.Name = "txtTOT";
			this.txtTOT.NotNull = true;
			resources.ApplyResources(this.label12, "label12");
			this.label12.Name = "label12";
			resources.ApplyResources(this.label13, "label13");
			this.label13.Name = "label13";
			this.txtTDT.IsValidated = true;
			resources.ApplyResources(this.txtTDT, "txtTDT");
			this.txtTDT.Name = "txtTDT";
			this.txtTDT.NotNull = true;
			this.txtSDT_OTH.IsValidated = true;
			resources.ApplyResources(this.txtSDT_OTH, "txtSDT_OTH");
			this.txtSDT_OTH.Name = "txtSDT_OTH";
			this.txtSDT_OTH.NotNull = true;
			this.txtSDT_ACT.IsValidated = true;
			resources.ApplyResources(this.txtSDT_ACT, "txtSDT_ACT");
			this.txtSDT_ACT.Name = "txtSDT_ACT";
			this.txtSDT_ACT.NotNull = true;
			this.txtBAT.IsValidated = true;
			resources.ApplyResources(this.txtBAT, "txtBAT");
			this.txtBAT.Name = "txtBAT";
			this.txtBAT.NotNull = true;
			this.txtNIT_OTH.IsValidated = true;
			resources.ApplyResources(this.txtNIT_OTH, "txtNIT_OTH");
			this.txtNIT_OTH.Name = "txtNIT_OTH";
			this.txtNIT_OTH.NotNull = true;
			this.txtNIT_ACT.IsValidated = true;
			resources.ApplyResources(this.txtNIT_ACT, "txtNIT_ACT");
			this.txtNIT_ACT.Name = "txtNIT_ACT";
			this.txtNIT_ACT.NotNull = true;
			this.txtPMT.IsValidated = true;
			resources.ApplyResources(this.txtPMT, "txtPMT");
			this.txtPMT.Name = "txtPMT";
			this.txtPMT.NotNull = true;
			this.txtCAT.IsValidated = true;
			resources.ApplyResources(this.txtCAT, "txtCAT");
			this.txtCAT.Name = "txtCAT";
			this.txtCAT.NotNull = true;
			this.txtPAT.IsValidated = true;
			resources.ApplyResources(this.txtPAT, "txtPAT");
			this.txtPAT.Name = "txtPAT";
			this.txtPAT.NotNull = true;
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			resources.ApplyResources(this.lblSpeed, "lblSpeed");
			this.tableLayoutPanel2.SetColumnSpan(this.lblSpeed, 2);
			this.lblSpeed.Name = "lblSpeed";
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.label2, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.label3, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.label4, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.label5, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.label6, 0, 6);
			this.tableLayoutPanel2.Controls.Add(this.label7, 0, 7);
			this.tableLayoutPanel2.Controls.Add(this.txtPAT, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.txtCAT, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.txtPMT, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.txtNIT_ACT, 1, 4);
			this.tableLayoutPanel2.Controls.Add(this.txtNIT_OTH, 1, 5);
			this.tableLayoutPanel2.Controls.Add(this.txtBAT, 1, 6);
			this.tableLayoutPanel2.Controls.Add(this.txtSDT_ACT, 1, 7);
			this.tableLayoutPanel2.Controls.Add(this.txtSDT_OTH, 1, 8);
			this.tableLayoutPanel2.Controls.Add(this.txtTDT, 1, 14);
			this.tableLayoutPanel2.Controls.Add(this.label13, 0, 14);
			this.tableLayoutPanel2.Controls.Add(this.label12, 0, 13);
			this.tableLayoutPanel2.Controls.Add(this.txtTOT, 1, 13);
			this.tableLayoutPanel2.Controls.Add(this.label11, 0, 12);
			this.tableLayoutPanel2.Controls.Add(this.txtEIT_OTH_SC, 1, 12);
			this.tableLayoutPanel2.Controls.Add(this.label10, 0, 11);
			this.tableLayoutPanel2.Controls.Add(this.txtEIT_OTH_PF, 1, 11);
			this.tableLayoutPanel2.Controls.Add(this.label9, 0, 10);
			this.tableLayoutPanel2.Controls.Add(this.txtEIT_ACT_SC, 1, 10);
			this.tableLayoutPanel2.Controls.Add(this.label8, 0, 9);
			this.tableLayoutPanel2.Controls.Add(this.lblSDT_OTH, 0, 8);
			this.tableLayoutPanel2.Controls.Add(this.txtEIT_ACT_PF, 1, 9);
			this.tableLayoutPanel2.Controls.Add(this.lblSpeed, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.btResetPAT, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.btResetCAT, 2, 2);
			this.tableLayoutPanel2.Controls.Add(this.btResetPMT, 2, 3);
			this.tableLayoutPanel2.Controls.Add(this.btResetNIT_ACT, 2, 4);
			this.tableLayoutPanel2.Controls.Add(this.btResetNIT_OTH, 2, 5);
			this.tableLayoutPanel2.Controls.Add(this.btResetBAT, 2, 6);
			this.tableLayoutPanel2.Controls.Add(this.btResetSDT_ACT, 2, 7);
			this.tableLayoutPanel2.Controls.Add(this.btResetSDT_OTH, 2, 8);
			this.tableLayoutPanel2.Controls.Add(this.btResetEIT_ACT_PF, 2, 9);
			this.tableLayoutPanel2.Controls.Add(this.btResetEIT_ACT_SC, 2, 10);
			this.tableLayoutPanel2.Controls.Add(this.btResetEIT_OTH_PF, 2, 11);
			this.tableLayoutPanel2.Controls.Add(this.btResetEIT_OTH_SC, 2, 12);
			this.tableLayoutPanel2.Controls.Add(this.btResetTOT, 2, 13);
			this.tableLayoutPanel2.Controls.Add(this.btResetTDT, 2, 14);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			resources.ApplyResources(this.btResetPAT, "btResetPAT");
			this.btResetPAT.Name = "btResetPAT";
			this.btResetPAT.UseVisualStyleBackColor = true;
			this.btResetPAT.Click += new System.EventHandler(btResetPAT_Click);
			resources.ApplyResources(this.btResetCAT, "btResetCAT");
			this.btResetCAT.Name = "btResetCAT";
			this.btResetCAT.UseVisualStyleBackColor = true;
			this.btResetCAT.Click += new System.EventHandler(btResetCAT_Click);
			resources.ApplyResources(this.btResetPMT, "btResetPMT");
			this.btResetPMT.Name = "btResetPMT";
			this.btResetPMT.UseVisualStyleBackColor = true;
			this.btResetPMT.Click += new System.EventHandler(btResetPMT_Click);
			resources.ApplyResources(this.btResetNIT_ACT, "btResetNIT_ACT");
			this.btResetNIT_ACT.Name = "btResetNIT_ACT";
			this.btResetNIT_ACT.UseVisualStyleBackColor = true;
			this.btResetNIT_ACT.Click += new System.EventHandler(btResetNIT_ACT_Click);
			resources.ApplyResources(this.btResetNIT_OTH, "btResetNIT_OTH");
			this.btResetNIT_OTH.Name = "btResetNIT_OTH";
			this.btResetNIT_OTH.UseVisualStyleBackColor = true;
			this.btResetNIT_OTH.Click += new System.EventHandler(btResetNIT_OTH_Click);
			resources.ApplyResources(this.btResetBAT, "btResetBAT");
			this.btResetBAT.Name = "btResetBAT";
			this.btResetBAT.UseVisualStyleBackColor = true;
			this.btResetBAT.Click += new System.EventHandler(btResetBAT_Click);
			resources.ApplyResources(this.btResetSDT_ACT, "btResetSDT_ACT");
			this.btResetSDT_ACT.Name = "btResetSDT_ACT";
			this.btResetSDT_ACT.UseVisualStyleBackColor = true;
			this.btResetSDT_ACT.Click += new System.EventHandler(btResetSDT_ACT_Click);
			resources.ApplyResources(this.btResetSDT_OTH, "btResetSDT_OTH");
			this.btResetSDT_OTH.Name = "btResetSDT_OTH";
			this.btResetSDT_OTH.UseVisualStyleBackColor = true;
			this.btResetSDT_OTH.Click += new System.EventHandler(btResetSDT_OTH_Click);
			resources.ApplyResources(this.btResetEIT_ACT_PF, "btResetEIT_ACT_PF");
			this.btResetEIT_ACT_PF.Name = "btResetEIT_ACT_PF";
			this.btResetEIT_ACT_PF.UseVisualStyleBackColor = true;
			this.btResetEIT_ACT_PF.Click += new System.EventHandler(btResetEIT_ACT_PF_Click);
			resources.ApplyResources(this.btResetEIT_ACT_SC, "btResetEIT_ACT_SC");
			this.btResetEIT_ACT_SC.Name = "btResetEIT_ACT_SC";
			this.btResetEIT_ACT_SC.UseVisualStyleBackColor = true;
			this.btResetEIT_ACT_SC.Click += new System.EventHandler(btResetEIT_ACT_SC_Click);
			resources.ApplyResources(this.btResetEIT_OTH_PF, "btResetEIT_OTH_PF");
			this.btResetEIT_OTH_PF.Name = "btResetEIT_OTH_PF";
			this.btResetEIT_OTH_PF.UseVisualStyleBackColor = true;
			this.btResetEIT_OTH_PF.Click += new System.EventHandler(btResetEIT_OTH_PF_Click);
			resources.ApplyResources(this.btResetEIT_OTH_SC, "btResetEIT_OTH_SC");
			this.btResetEIT_OTH_SC.Name = "btResetEIT_OTH_SC";
			this.btResetEIT_OTH_SC.UseVisualStyleBackColor = true;
			this.btResetEIT_OTH_SC.Click += new System.EventHandler(btResetEIT_OTH_SC_Click);
			resources.ApplyResources(this.btResetTOT, "btResetTOT");
			this.btResetTOT.Name = "btResetTOT";
			this.btResetTOT.UseVisualStyleBackColor = true;
			this.btResetTOT.Click += new System.EventHandler(btResetTOT_Click);
			resources.ApplyResources(this.btResetTDT, "btResetTDT");
			this.btResetTDT.Name = "btResetTDT";
			this.btResetTDT.UseVisualStyleBackColor = true;
			this.btResetTDT.Click += new System.EventHandler(btResetTDT_Click);
			resources.ApplyResources(this.cmbTS, "cmbTS");
			this.cmbTS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTS.FormattingEnabled = true;
			this.cmbTS.Name = "cmbTS";
			resources.ApplyResources(this.lblTableType, "lblTableType");
			this.lblTableType.Name = "lblTableType";
			this.flowLayoutPanel1.Controls.Add(this.lblTableType);
			this.flowLayoutPanel1.Controls.Add(this.cmbTS);
			resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.grbLoaction.Controls.Add(this.tableLayoutPanel2);
			resources.ApplyResources(this.grbLoaction, "grbLoaction");
			this.grbLoaction.Name = "grbLoaction";
			this.grbLoaction.TabStop = false;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormSendSpeedDetail";
			base.Load += new System.EventHandler(FormSendSpeedDetail_Load);
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.grbLoaction.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private FormSendSpeedDetail()
		{
			InitializeComponent();
		}

		public FormSendSpeedDetail(IList<TsInfo> TSList)
		{
			InitializeComponent();
			base.PageFlag = PageEnum.Add;
			Tool.BindingComboBox(((IEnumerable<BaseKey>)TSList).ToList(), cmbTS);
			controller = new SendSpeedInfo();
			ShowSpeedSettingToUI(DefaultSendSpeed);
		}

		public FormSendSpeedDetail(long ControlIdentity, PageEnum DoModel)
		{
			InitializeComponent();
			base.PageFlag = DoModel;
			controller = controlManager.GetByIdentity(ControlIdentity);
			ShowSpeedSettingToUI(controller);
			Tool.BindingComboBox(new List<BaseKey> { controller.TS }, cmbTS);
		}

		private void ShowSpeedSettingToUI(SendSpeedInfo speedSetting)
		{
			txtBAT.Text = speedSetting.Bat.ToString();
			txtCAT.Text = speedSetting.Cat.ToString();
			txtEIT_ACT_PF.Text = speedSetting.EitActPf.ToString();
			txtEIT_ACT_SC.Text = speedSetting.EitActSc.ToString();
			txtEIT_OTH_PF.Text = speedSetting.EitOthPf.ToString();
			txtEIT_OTH_SC.Text = speedSetting.EitOthSc.ToString();
			txtNIT_ACT.Text = speedSetting.NitAct.ToString();
			txtNIT_OTH.Text = speedSetting.NitOth.ToString();
			txtPAT.Text = speedSetting.Pat.ToString();
			txtPMT.Text = speedSetting.Pmt.ToString();
			txtSDT_ACT.Text = speedSetting.SdtAct.ToString();
			txtSDT_OTH.Text = speedSetting.SdtOth.ToString();
			txtTDT.Text = speedSetting.Tdt.ToString();
			txtTOT.Text = speedSetting.Tot.ToString();
		}

		private void FormSendSpeedDetail_Load(object sender, EventArgs e)
		{
			foreach (Control control2 in tableLayoutPanel2.Controls)
			{
				if (!(control2 is ValidatTextBox))
				{
					continue;
				}
				control2.Validating += delegate(object senderX, CancelEventArgs eX)
				{
					ValidatTextBox validatTextBox = senderX as ValidatTextBox;
					if (validatTextBox.Enabled && !validatTextBox.ReadOnly && validatTextBox.IsValidated)
					{
						int num = -1;
						try
						{
							num = Convert.ToInt32(validatTextBox.Text);
						}
						catch
						{
						}
						if (num < 0)
						{
							validatTextBox.IsValidated = false;
							Tool.ShowErrorProviderMessage(validatTextBox, LocationControl.Ins.LocationItems[LacationEnum.InputFormatError]);
						}
						else
						{
							validatTextBox.IsValidated = true;
							Tool.ShowErrorProviderMessage(validatTextBox, "");
						}
					}
				};
			}
			_FunctionName = EnumFunctionName.SysFuncSendSpeedManagement;
			switch (base.PageFlag)
			{
			case PageEnum.Show:
				cmbTS.Enabled = false;
				foreach (Control control3 in tableLayoutPanel2.Controls)
				{
					if (control3 is TextBoxBase)
					{
						((TextBoxBase)control3).ReadOnly = true;
					}
				}
				break;
			}
			bool flag = true;
		}

		protected override bool FunSubmit()
		{
			if (!ValidateInput(tableLayoutPanel2))
			{
				return false;
			}
			controller.Bat = Tool.GetInt32(txtBAT.Text);
			controller.Cat = Tool.GetInt32(txtCAT.Text);
			controller.EitActPf = Tool.GetInt32(txtEIT_ACT_PF.Text);
			controller.EitActSc = Tool.GetInt32(txtEIT_ACT_SC.Text);
			controller.EitOthPf = Tool.GetInt32(txtEIT_OTH_PF.Text);
			controller.EitOthSc = Tool.GetInt32(txtEIT_OTH_SC.Text);
			controller.NitAct = Tool.GetInt32(txtNIT_ACT.Text);
			controller.NitOth = Tool.GetInt32(txtNIT_OTH.Text);
			controller.Pat = Tool.GetInt32(txtPAT.Text);
			controller.Pmt = Tool.GetInt32(txtPMT.Text);
			controller.SdtAct = Tool.GetInt32(txtSDT_ACT.Text);
			controller.SdtOth = Tool.GetInt32(txtSDT_OTH.Text);
			controller.Tdt = Tool.GetInt32(txtTDT.Text);
			controller.Tot = Tool.GetInt32(txtTOT.Text);
			controller.TsTableId = Tool.GetInt64(Tool.GetValue(cmbTS));
			if (controlManager.Save(controller) == 1)
			{
				Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.SaveSucceed]);
				base.DialogResult = DialogResult.OK;
				return true;
			}
			Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.SaveFailure]);
			return false;
		}

		protected override void FunCancel()
		{
			base.FunCancel();
			base.DialogResult = DialogResult.Cancel;
		}

		private void btResetPAT_Click(object sender, EventArgs e)
		{
			txtPAT.Text = DefaultSendSpeed.Pat.ToString();
		}

		private void btResetCAT_Click(object sender, EventArgs e)
		{
			txtCAT.Text = DefaultSendSpeed.Cat.ToString();
		}

		private void btResetPMT_Click(object sender, EventArgs e)
		{
			txtPMT.Text = DefaultSendSpeed.Pmt.ToString();
		}

		private void btResetNIT_ACT_Click(object sender, EventArgs e)
		{
			txtNIT_ACT.Text = DefaultSendSpeed.NitAct.ToString();
		}

		private void btResetNIT_OTH_Click(object sender, EventArgs e)
		{
			txtNIT_OTH.Text = DefaultSendSpeed.NitOth.ToString();
		}

		private void btResetBAT_Click(object sender, EventArgs e)
		{
			txtBAT.Text = DefaultSendSpeed.Bat.ToString();
		}

		private void btResetSDT_ACT_Click(object sender, EventArgs e)
		{
			txtSDT_ACT.Text = DefaultSendSpeed.SdtAct.ToString();
		}

		private void btResetSDT_OTH_Click(object sender, EventArgs e)
		{
			txtSDT_OTH.Text = DefaultSendSpeed.SdtOth.ToString();
		}

		private void btResetEIT_ACT_PF_Click(object sender, EventArgs e)
		{
			txtEIT_ACT_PF.Text = DefaultSendSpeed.EitActPf.ToString();
		}

		private void btResetEIT_ACT_SC_Click(object sender, EventArgs e)
		{
			txtEIT_ACT_SC.Text = DefaultSendSpeed.EitActSc.ToString();
		}

		private void btResetEIT_OTH_PF_Click(object sender, EventArgs e)
		{
			txtEIT_OTH_PF.Text = DefaultSendSpeed.EitOthPf.ToString();
		}

		private void btResetEIT_OTH_SC_Click(object sender, EventArgs e)
		{
			txtEIT_OTH_SC.Text = DefaultSendSpeed.EitOthSc.ToString();
		}

		private void btResetTOT_Click(object sender, EventArgs e)
		{
			txtTOT.Text = DefaultSendSpeed.Tot.ToString();
		}

		private void btResetTDT_Click(object sender, EventArgs e)
		{
			txtTDT.Text = DefaultSendSpeed.Tdt.ToString();
		}
	}
}
