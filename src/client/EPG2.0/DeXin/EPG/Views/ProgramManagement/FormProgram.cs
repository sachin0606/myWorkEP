using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.ProgramManagement
{
	public class FormProgram : FormDetailBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label label2;

		private Label label1;

		private TextBox txtProviderName;

		private Label label3;

		private Label label4;

		private TextBox txtNIT;

		private Label label6;

		private TextBox txtTS;

		private NumTextBox txtPrgId;

		private GroupBox gbxAdvance;

		private TableLayoutPanel tableLayoutPanel2;

		private Label label5;

		private Label label8;

		private Label label9;

		private Label label7;

		private Label label10;

		private Label label11;

		private ValidatTextBox txtPrgName;

		private Button btAdvance;

		private FlowLayoutPanel flowLayoutPanel2;

		private RadioButton rbtEitPfYes;

		private RadioButton rbtEitPfNo;

		private NumTextBox txtPMTId;

		private NumTextBox txtPMTVersion;

		private NumTextBox txtPCRId;

		private Label label12;

		private FlowLayoutPanel flowLayoutPanel1;

		private RadioButton rbtEitScYes;

		private RadioButton rbtEitScNo;

		private FlowLayoutPanel flowLayoutPanel3;

		private RadioButton rbtFreeCAYes;

		private RadioButton rbtFreeCANo;

		private NumTextBox txtSdtRunState;

		private Label label13;

		private Label label14;

		private TextBox txtEITFileName;

		private FlowLayoutPanel flowLayoutPanel4;

		private CheckBox ckVerPMT;

		private DescriptorCombobox cmbServiceType;

		private TsInfo ts;

		private ProgramInfo prg;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.ProgramManagement.FormProgram));
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtProviderName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtNIT = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtTS = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.gbxAdvance = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.label14 = new System.Windows.Forms.Label();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.rbtEitPfYes = new System.Windows.Forms.RadioButton();
			this.rbtEitPfNo = new System.Windows.Forms.RadioButton();
			this.label10 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.txtPMTId = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.txtPCRId = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.rbtEitScYes = new System.Windows.Forms.RadioButton();
			this.rbtEitScNo = new System.Windows.Forms.RadioButton();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.rbtFreeCAYes = new System.Windows.Forms.RadioButton();
			this.rbtFreeCANo = new System.Windows.Forms.RadioButton();
			this.txtSdtRunState = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.txtEITFileName = new System.Windows.Forms.TextBox();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
			this.txtPMTVersion = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.ckVerPMT = new System.Windows.Forms.CheckBox();
			this.txtPrgId = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.txtPrgName = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.btAdvance = new System.Windows.Forms.Button();
			this.cmbServiceType = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.gbxAdvance.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			this.flowLayoutPanel4.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			resources.ApplyResources(this.txtProviderName, "txtProviderName");
			this.txtProviderName.Name = "txtProviderName";
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			resources.ApplyResources(this.txtNIT, "txtNIT");
			this.txtNIT.Name = "txtNIT";
			this.txtNIT.ReadOnly = true;
			this.txtNIT.TabStop = false;
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			resources.ApplyResources(this.txtTS, "txtTS");
			this.txtTS.Name = "txtTS";
			this.txtTS.ReadOnly = true;
			this.txtTS.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.gbxAdvance, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtProviderName, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtNIT, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.label6, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtTS, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtPrgId, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtPrgName, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.btAdvance, 0, 5);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.SetColumnSpan(this.gbxAdvance, 2);
			this.gbxAdvance.Controls.Add(this.tableLayoutPanel2);
			resources.ApplyResources(this.gbxAdvance, "gbxAdvance");
			this.gbxAdvance.Name = "gbxAdvance";
			this.gbxAdvance.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.label14, 0, 8);
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel2, 1, 5);
			this.tableLayoutPanel2.Controls.Add(this.label10, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.label5, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.label8, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.label13, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.txtPMTId, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.label9, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.label7, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.txtPCRId, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.label11, 0, 6);
			this.tableLayoutPanel2.Controls.Add(this.label12, 0, 7);
			this.tableLayoutPanel2.Controls.Add(this.cmbServiceType, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 1, 4);
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel3, 1, 7);
			this.tableLayoutPanel2.Controls.Add(this.txtSdtRunState, 1, 6);
			this.tableLayoutPanel2.Controls.Add(this.txtEITFileName, 1, 8);
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel4, 1, 2);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			resources.ApplyResources(this.label14, "label14");
			this.label14.Name = "label14";
			this.flowLayoutPanel2.Controls.Add(this.rbtEitPfYes);
			this.flowLayoutPanel2.Controls.Add(this.rbtEitPfNo);
			resources.ApplyResources(this.flowLayoutPanel2, "flowLayoutPanel2");
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			resources.ApplyResources(this.rbtEitPfYes, "rbtEitPfYes");
			this.rbtEitPfYes.Checked = true;
			this.rbtEitPfYes.Name = "rbtEitPfYes";
			this.rbtEitPfYes.TabStop = true;
			this.rbtEitPfYes.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.rbtEitPfNo, "rbtEitPfNo");
			this.rbtEitPfNo.Name = "rbtEitPfNo";
			this.rbtEitPfNo.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.label10, "label10");
			this.label10.Name = "label10";
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			resources.ApplyResources(this.label13, "label13");
			this.label13.Name = "label13";
			resources.ApplyResources(this.txtPMTId, "txtPMTId");
			this.txtPMTId.ForeColor = System.Drawing.Color.Black;
			this.txtPMTId.IsValidated = true;
			this.txtPMTId.MaxValue = 8191;
			this.txtPMTId.Name = "txtPMTId";
			this.txtPMTId.NotNull = true;
			this.txtPMTId.SetMaxLength = 2;
			this.txtPMTId.Value = new byte[2] { 31, 255 };
			resources.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			resources.ApplyResources(this.txtPCRId, "txtPCRId");
			this.txtPCRId.ForeColor = System.Drawing.Color.Black;
			this.txtPCRId.IsValidated = true;
			this.txtPCRId.MaxValue = 8191;
			this.txtPCRId.Name = "txtPCRId";
			this.txtPCRId.NotNull = true;
			this.txtPCRId.SetMaxLength = 2;
			this.txtPCRId.Value = new byte[2] { 31, 255 };
			resources.ApplyResources(this.label11, "label11");
			this.label11.Name = "label11";
			resources.ApplyResources(this.label12, "label12");
			this.label12.Name = "label12";
			resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			this.flowLayoutPanel1.Controls.Add(this.rbtEitScYes);
			this.flowLayoutPanel1.Controls.Add(this.rbtEitScNo);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			resources.ApplyResources(this.rbtEitScYes, "rbtEitScYes");
			this.rbtEitScYes.Checked = true;
			this.rbtEitScYes.Name = "rbtEitScYes";
			this.rbtEitScYes.TabStop = true;
			this.rbtEitScYes.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.rbtEitScNo, "rbtEitScNo");
			this.rbtEitScNo.Name = "rbtEitScNo";
			this.rbtEitScNo.UseVisualStyleBackColor = true;
			this.flowLayoutPanel3.Controls.Add(this.rbtFreeCAYes);
			this.flowLayoutPanel3.Controls.Add(this.rbtFreeCANo);
			resources.ApplyResources(this.flowLayoutPanel3, "flowLayoutPanel3");
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			resources.ApplyResources(this.rbtFreeCAYes, "rbtFreeCAYes");
			this.rbtFreeCAYes.Checked = true;
			this.rbtFreeCAYes.Name = "rbtFreeCAYes";
			this.rbtFreeCAYes.TabStop = true;
			this.rbtFreeCAYes.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.rbtFreeCANo, "rbtFreeCANo");
			this.rbtFreeCANo.Name = "rbtFreeCANo";
			this.rbtFreeCANo.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.txtSdtRunState, "txtSdtRunState");
			this.txtSdtRunState.ForeColor = System.Drawing.Color.Black;
			this.txtSdtRunState.IsValidated = true;
			this.txtSdtRunState.MaxValue = 7;
			this.txtSdtRunState.Name = "txtSdtRunState";
			this.txtSdtRunState.NotNull = true;
			this.txtSdtRunState.SetMaxLength = 2;
			this.txtSdtRunState.Value = new byte[1] { 4 };
			resources.ApplyResources(this.txtEITFileName, "txtEITFileName");
			this.txtEITFileName.Name = "txtEITFileName";
			resources.ApplyResources(this.flowLayoutPanel4, "flowLayoutPanel4");
			this.flowLayoutPanel4.Controls.Add(this.txtPMTVersion);
			this.flowLayoutPanel4.Controls.Add(this.ckVerPMT);
			this.flowLayoutPanel4.Name = "flowLayoutPanel4";
			resources.ApplyResources(this.txtPMTVersion, "txtPMTVersion");
			this.txtPMTVersion.ForeColor = System.Drawing.Color.Black;
			this.txtPMTVersion.IsValidated = true;
			this.txtPMTVersion.MaxValue = 31;
			this.txtPMTVersion.Name = "txtPMTVersion";
			this.txtPMTVersion.NotNull = true;
			this.txtPMTVersion.SetMaxLength = 1;
			DeXin.EPG.Tools.EPGUserControl.NumTextBox numTextBox = this.txtPMTVersion;
			byte[] value = new byte[1];
			numTextBox.Value = value;
			resources.ApplyResources(this.ckVerPMT, "ckVerPMT");
			this.ckVerPMT.Checked = true;
			this.ckVerPMT.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ckVerPMT.Name = "ckVerPMT";
			this.ckVerPMT.UseVisualStyleBackColor = true;
			this.ckVerPMT.CheckedChanged += new System.EventHandler(ckVerPMT_CheckedChanged);
			resources.ApplyResources(this.txtPrgId, "txtPrgId");
			this.txtPrgId.ForeColor = System.Drawing.Color.Black;
			this.txtPrgId.IsValidated = true;
			this.txtPrgId.MaxValue = 65535;
			this.txtPrgId.Name = "txtPrgId";
			this.txtPrgId.NotNull = true;
			this.txtPrgId.SetMaxLength = 2;
			this.txtPrgId.Value = new byte[0];
			resources.ApplyResources(this.txtPrgName, "txtPrgName");
			this.txtPrgName.IsValidated = true;
			this.txtPrgName.Name = "txtPrgName";
			this.txtPrgName.NotNull = true;
			resources.ApplyResources(this.btAdvance, "btAdvance");
			this.btAdvance.Name = "btAdvance";
			this.btAdvance.UseVisualStyleBackColor = true;
			this.btAdvance.Click += new System.EventHandler(btAdvance_Click);
			this.cmbServiceType.FormattingEnabled = true;
			resources.ApplyResources(this.cmbServiceType, "cmbServiceType");
			this.cmbServiceType.MaxValue = 0L;
			this.cmbServiceType.MinValue = 0L;
			this.cmbServiceType.Name = "cmbServiceType";
			this.cmbServiceType.Value = "";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.Name = "FormProgram";
			base.Load += new System.EventHandler(FormProgram_Load);
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			base.grbText.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.gbxAdvance.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.flowLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel4.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormProgram(TsInfo TS)
		{
			InitializeComponent();
			ts = TS;
			base.PageFlag = PageEnum.Add;
		}

		public FormProgram(ProgramInfo PRG)
		{
			InitializeComponent();
			prg = PRG;
			base.PageFlag = PageEnum.Edit;
		}

		private void FormProgram_Load(object sender, EventArgs e)
		{
			_FunctionName = EnumFunctionName.SysFuncProgramInfo;
			Tool.BindingComboBox(BaseData.Ins.ServiceTypes, cmbServiceType);
			cmbServiceType.SelectedIndex = -1;
			if (ts != null)
			{
				prg = new ProgramInfo();
				prg.TsTableId = ts.Identity;
				txtNIT.Text = ts.NIT.ToString();
				txtTS.Text = ts.ToString();
				cmbServiceType.SelectedIndex = 0;
			}
			else if (prg != null)
			{
				txtNIT.Text = prg.TS.NIT.ToString();
				txtTS.Text = prg.TS.ToString();
				txtPCRId.Text = prg.PCRPID.ToString("X4");
				txtPMTId.Text = prg.PmtId.ToString("X4");
				if (prg.PmtVersionNumber < 0)
				{
					txtPMTVersion.Text = "00";
					ckVerPMT.Checked = true;
				}
				else
				{
					txtPMTVersion.Text = prg.PmtVersionNumber.ToString("X2");
					ckVerPMT.Checked = false;
				}
				txtPrgId.Text = prg.ServiceId.ToString("X4");
				txtPrgName.Text = prg.ProgramName;
				txtProviderName.Text = prg.ProviderName;
				cmbServiceType.SelectedValue = prg.ServiceType.ToString("X2");
				if (cmbServiceType.SelectedIndex == -1)
				{
					cmbServiceType.Text = prg.ServiceType.ToString("X2");
				}
				txtSdtRunState.Text = prg.PrgSdtRunningstate.ToString("X4");
				rbtEitPfYes.Checked = prg.PrgSdtEitpfflag == 1;
				rbtEitPfNo.Checked = !rbtEitPfYes.Checked;
				rbtEitScYes.Checked = prg.PrgSdtEitscflag == 1;
				rbtEitScNo.Checked = !rbtEitScYes.Checked;
				rbtFreeCAYes.Checked = prg.PrgSdtFreecamod == 1;
				rbtFreeCANo.Checked = !rbtFreeCAYes.Checked;
				txtEITFileName.Text = prg.EITFileName;
			}
			btAdvance_Click(sender, e);
		}

		protected override bool FunSubmit()
		{
			if (!ValidateInput())
			{
				return false;
			}
			prg.PCRPID = txtPCRId.ValueOfInt32;
			prg.PmtId = txtPMTId.ValueOfInt32;
			if (ckVerPMT.Checked)
			{
				prg.PmtVersionNumber = -1;
			}
			else
			{
				prg.PmtVersionNumber = txtPMTVersion.ValueOfInt32;
			}
			prg.ServiceId = txtPrgId.ValueOfInt32;
			prg.ProgramName = txtPrgName.Text;
			prg.ServiceType = Convert.ToInt32(cmbServiceType.Value, 16);
			prg.ProviderName = txtProviderName.Text;
			prg.PrgSdtRunningstate = txtSdtRunState.ValueOfInt32;
			prg.PrgManualeventid = 0;
			prg.PrgSdtEitpfflag = (rbtEitPfYes.Checked ? 1 : 0);
			prg.PrgSdtEitscflag = (rbtEitScYes.Checked ? 1 : 0);
			prg.PrgSdtFreecamod = (rbtFreeCAYes.Checked ? 1 : 0);
			prg.EITFileName = txtEITFileName.Text.Trim();
			ProgramInfoManager programInfoManager = new ProgramInfoManager();
			try
			{
				if (programInfoManager.Save(prg) == 1)
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
				txtPrgId.Focus();
				txtPrgId.SelectAll();
				return false;
			}
		}

		protected override void FunCancel()
		{
			base.DialogResult = DialogResult.Cancel;
		}

		private void btAdvance_Click(object sender, EventArgs e)
		{
			if (gbxAdvance.Visible)
			{
				gbxAdvance.Hide();
				base.Height -= gbxAdvance.Height;
				btAdvance.Text = ResourceManagement.GetLocationString("ShowAdancedProperty");
			}
			else
			{
				base.Height += gbxAdvance.Height;
				gbxAdvance.Show();
				btAdvance.Text = ResourceManagement.GetLocationString("HideAdancedProperty");
			}
		}

		private void ckVerPMT_CheckedChanged(object sender, EventArgs e)
		{
			txtPMTVersion.Enabled = !ckVerPMT.Checked;
			if (ckVerPMT.Checked)
			{
				txtPMTVersion.Text = "00";
			}
			else if (prg != null && prg.PmtVersionNumber > 0)
			{
				txtPMTVersion.Text = prg.PmtVersionNumber.ToString("X2");
			}
		}
	}
}
