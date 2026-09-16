using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class Terrestrial_delivery_system_descriptorDemo : DescriptorDemoBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableData;

		private Label lblCentre_frequency;

		private Label lblBandwidth;

		private Label lblPriority;

		private Label lblTime_Slicing_indicator;

		private Label lblMPE_FEC_indicator;

		private Label lblReserved_future_use;

		private Label lblConstellation;

		private Label lblHierarchy_information;

		private Label lblCode_rate_HP_stream;

		private Label lblCode_rate_LP;

		private Label lblGuard_interval;

		private DescriptorCombobox cmbBandwidth;

		private CheckBox cboTime_Slicing_indicator;

		private CheckBox cboPrioryty;

		private CheckBox cboMPE_FEC_indicator;

		private DescriptorTextBox txtReserved_future_use;

		private DescriptorCombobox cmbConstellation;

		private DescriptorCombobox cmbHierarchy_information;

		private DescriptorCombobox cmbCode_rate_HP;

		private DescriptorCombobox cmbCode_rate_LP;

		private DescriptorCombobox cmbGuard_interval;

		private Label lblTransmission_mode;

		private DescriptorCombobox cmbTransmission_mode;

		private Label lblOther_frequency_flag;

		private CheckBox cboOther_frequency_flag;

		private NumTextBox txtCentre_frequency;

		public Terrestrial_delivery_system_descriptorDemo()
		{
			InitializeComponent();
		}

		public Terrestrial_delivery_system_descriptorDemo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
			InitControl();
		}

		protected override void InitControl()
		{
			Tool.BindingComboBox(BaseData.Ins.Bandwidth, cmbBandwidth);
			Tool.BindingComboBox(BaseData.Ins.Constellation, cmbConstellation);
			Tool.BindingComboBox(BaseData.Ins.Hierarchy_information, cmbHierarchy_information);
			Tool.BindingComboBox(BaseData.Ins.CodeRate, cmbCode_rate_HP);
			Tool.BindingComboBox(BaseData.Ins.CodeRate, cmbCode_rate_LP);
			Tool.BindingComboBox(BaseData.Ins.TransmissionMode, cmbTransmission_mode);
			Tool.BindingComboBox(BaseData.Ins.GuardInterval, cmbGuard_interval);
			txtCentre_frequency.SetMaxLength = 4;
			DescriptorTextBox descriptorTextBox = txtReserved_future_use;
			bool visible = (lblReserved_future_use.Visible = false);
			descriptorTextBox.Visible = visible;
		}

		protected override void BindingData()
		{
			if (base.PageFlag != PageEnum.Add && base.PageFlag != PageEnum.Empty)
			{
				byte[] buffer = Tool.ResolveByte(base.ET.DescriptionData, 0, 4);
				txtCentre_frequency.Text = Tool.BytesToString(buffer);
				byte b = Tool.ResolveByte(base.ET.DescriptionData, 4, 1)[0];
				cmbBandwidth.Value = ((byte)((b >> 5) & 7)).ToString(FormatString.Ins.X2);
				int num = (b >> 4) & 1;
				cboPrioryty.Checked = num == 1;
				num = (b >> 3) & 1;
				cboMPE_FEC_indicator.Checked = num == 0;
				num = (b >> 2) & 1;
				cboTime_Slicing_indicator.Checked = num == 0;
				b = Tool.ResolveByte(base.ET.DescriptionData, 5, 1)[0];
				cmbConstellation.Value = ((byte)((b >> 6) & 3)).ToString(FormatString.Ins.X2);
				cmbHierarchy_information.Value = ((byte)((b >> 3) & 7)).ToString(FormatString.Ins.X2);
				cmbCode_rate_HP.Value = ((byte)(b & 7)).ToString(FormatString.Ins.X2);
				b = Tool.ResolveByte(base.ET.DescriptionData, 6, 1)[0];
				cmbCode_rate_LP.Value = ((byte)((b >> 5) & 7)).ToString(FormatString.Ins.X2);
				cmbGuard_interval.Value = ((byte)((b >> 3) & 3)).ToString(FormatString.Ins.X2);
				cmbTransmission_mode.Value = ((byte)((b >> 1) & 3)).ToString(FormatString.Ins.X2);
				byte b2 = (byte)(b & 1);
				cboOther_frequency_flag.Checked = num == 0;
			}
			if (base.PageFlag == PageEnum.Show)
			{
				txtCentre_frequency.ReadOnly = true;
				DescriptorCombobox descriptorCombobox = cmbBandwidth;
				DescriptorCombobox descriptorCombobox2 = cmbCode_rate_HP;
				DescriptorCombobox descriptorCombobox3 = cmbCode_rate_LP;
				DescriptorCombobox descriptorCombobox4 = cmbConstellation;
				DescriptorCombobox descriptorCombobox5 = cmbGuard_interval;
				DescriptorCombobox descriptorCombobox6 = cmbHierarchy_information;
				DescriptorCombobox descriptorCombobox7 = cmbTransmission_mode;
				CheckBox checkBox = cboMPE_FEC_indicator;
				CheckBox checkBox2 = cboOther_frequency_flag;
				CheckBox checkBox3 = cboPrioryty;
				bool flag = (cboTime_Slicing_indicator.Enabled = false);
				flag = (checkBox3.Enabled = flag);
				flag = (checkBox2.Enabled = flag);
				flag = (checkBox.Enabled = flag);
				flag = (descriptorCombobox7.Enabled = flag);
				flag = (descriptorCombobox6.Enabled = flag);
				flag = (descriptorCombobox5.Enabled = flag);
				flag = (descriptorCombobox4.Enabled = flag);
				flag = (descriptorCombobox3.Enabled = flag);
				flag = (descriptorCombobox2.Enabled = flag);
				descriptorCombobox.Enabled = flag;
			}
		}

		public override bool IsValidated(ErrorProvider error)
		{
			if (!txtCentre_frequency.IsValidated || !cmbBandwidth.IsValitdated(error) || !cmbCode_rate_HP.IsValitdated(error) || !cmbCode_rate_LP.IsValitdated(error))
			{
				return false;
			}
			int startIndex = 0;
			base.ET.DataLength = 11;
			base.ET.DescriptionData = new byte[11];
			byte[] resource = Tool.StringToBytes(CommTool.GetTextBoxString(8, 0, txtCentre_frequency));
			Tool.ToByteArray(resource, base.ET.DescriptionData, ref startIndex);
			byte b = 0;
			byte b2 = Tool.StringToBytes(cmbBandwidth.Value)[0];
			b |= (byte)((b2 & 7) << 5);
			b2 = (byte)(cboPrioryty.Checked ? 1 : 0);
			b |= (byte)((b2 & 1) << 4);
			b2 = ((!cboMPE_FEC_indicator.Checked) ? ((byte)1) : ((byte)0));
			b |= (byte)((b2 & 1) << 3);
			b2 = ((!cboTime_Slicing_indicator.Checked) ? ((byte)1) : ((byte)0));
			b |= (byte)((b2 & 1) << 2);
			b2 = 0;
			b |= (byte)(b2 & 3);
			Tool.ToByteArray(b, base.ET.DescriptionData, ref startIndex);
			b = 0;
			b2 = Tool.StringToBytes(cmbConstellation.Value)[0];
			b = (byte)((b2 & 3) << 6);
			b2 = Tool.StringToBytes(cmbHierarchy_information.Value)[0];
			b |= (byte)((b2 & 7) << 3);
			b2 = Tool.StringToBytes(cmbCode_rate_HP.Value)[0];
			b |= (byte)(b2 & 7);
			Tool.ToByteArray(b, base.ET.DescriptionData, ref startIndex);
			b = 0;
			b2 = Tool.StringToBytes(cmbCode_rate_LP.Value)[0];
			b |= (byte)((b2 & 7) << 5);
			b2 = Tool.StringToBytes(cmbGuard_interval.Value)[0];
			b |= (byte)((b2 & 3) << 3);
			b2 = Tool.StringToBytes(cmbTransmission_mode.Value)[0];
			b |= (byte)((b2 & 3) << 1);
			b2 = (byte)(cboOther_frequency_flag.Checked ? 1 : 0);
			b |= (byte)(b2 & 1);
			Tool.ToByteArray(b, base.ET.DescriptionData, ref startIndex);
			Tool.ToByteArray(0, base.ET.DescriptionData, ref startIndex);
			return true;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.Terrestrial_delivery_system_descriptorDemo));
			this.tableData = new System.Windows.Forms.TableLayoutPanel();
			this.lblCentre_frequency = new System.Windows.Forms.Label();
			this.lblBandwidth = new System.Windows.Forms.Label();
			this.lblPriority = new System.Windows.Forms.Label();
			this.lblTime_Slicing_indicator = new System.Windows.Forms.Label();
			this.lblMPE_FEC_indicator = new System.Windows.Forms.Label();
			this.lblReserved_future_use = new System.Windows.Forms.Label();
			this.lblConstellation = new System.Windows.Forms.Label();
			this.lblHierarchy_information = new System.Windows.Forms.Label();
			this.lblCode_rate_HP_stream = new System.Windows.Forms.Label();
			this.lblCode_rate_LP = new System.Windows.Forms.Label();
			this.lblGuard_interval = new System.Windows.Forms.Label();
			this.cmbBandwidth = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.cboTime_Slicing_indicator = new System.Windows.Forms.CheckBox();
			this.cboPrioryty = new System.Windows.Forms.CheckBox();
			this.cboMPE_FEC_indicator = new System.Windows.Forms.CheckBox();
			this.txtReserved_future_use = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.cmbConstellation = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.cmbHierarchy_information = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.cmbCode_rate_HP = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.cmbCode_rate_LP = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.cmbGuard_interval = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.lblTransmission_mode = new System.Windows.Forms.Label();
			this.cmbTransmission_mode = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.lblOther_frequency_flag = new System.Windows.Forms.Label();
			this.cboOther_frequency_flag = new System.Windows.Forms.CheckBox();
			this.txtCentre_frequency = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			base.grbContext.SuspendLayout();
			this.tableData.SuspendLayout();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.tableData);
			resources.ApplyResources(base.grbContext, "grbContext");
			resources.ApplyResources(this.tableData, "tableData");
			this.tableData.Controls.Add(this.lblCentre_frequency, 0, 0);
			this.tableData.Controls.Add(this.lblBandwidth, 0, 1);
			this.tableData.Controls.Add(this.lblPriority, 0, 2);
			this.tableData.Controls.Add(this.lblTime_Slicing_indicator, 0, 3);
			this.tableData.Controls.Add(this.lblMPE_FEC_indicator, 0, 4);
			this.tableData.Controls.Add(this.lblReserved_future_use, 0, 5);
			this.tableData.Controls.Add(this.lblConstellation, 0, 6);
			this.tableData.Controls.Add(this.lblHierarchy_information, 0, 7);
			this.tableData.Controls.Add(this.lblCode_rate_HP_stream, 0, 8);
			this.tableData.Controls.Add(this.lblCode_rate_LP, 0, 9);
			this.tableData.Controls.Add(this.lblGuard_interval, 0, 10);
			this.tableData.Controls.Add(this.cmbBandwidth, 1, 1);
			this.tableData.Controls.Add(this.cboTime_Slicing_indicator, 1, 3);
			this.tableData.Controls.Add(this.cboPrioryty, 1, 2);
			this.tableData.Controls.Add(this.cboMPE_FEC_indicator, 1, 4);
			this.tableData.Controls.Add(this.txtReserved_future_use, 1, 5);
			this.tableData.Controls.Add(this.cmbConstellation, 1, 6);
			this.tableData.Controls.Add(this.cmbHierarchy_information, 1, 7);
			this.tableData.Controls.Add(this.cmbCode_rate_HP, 1, 8);
			this.tableData.Controls.Add(this.cmbCode_rate_LP, 1, 9);
			this.tableData.Controls.Add(this.cmbGuard_interval, 1, 10);
			this.tableData.Controls.Add(this.lblTransmission_mode, 0, 11);
			this.tableData.Controls.Add(this.cmbTransmission_mode, 1, 11);
			this.tableData.Controls.Add(this.lblOther_frequency_flag, 0, 12);
			this.tableData.Controls.Add(this.cboOther_frequency_flag, 1, 12);
			this.tableData.Controls.Add(this.txtCentre_frequency, 1, 0);
			this.tableData.Name = "tableData";
			resources.ApplyResources(this.lblCentre_frequency, "lblCentre_frequency");
			this.lblCentre_frequency.Name = "lblCentre_frequency";
			resources.ApplyResources(this.lblBandwidth, "lblBandwidth");
			this.lblBandwidth.Name = "lblBandwidth";
			resources.ApplyResources(this.lblPriority, "lblPriority");
			this.lblPriority.Name = "lblPriority";
			resources.ApplyResources(this.lblTime_Slicing_indicator, "lblTime_Slicing_indicator");
			this.lblTime_Slicing_indicator.Name = "lblTime_Slicing_indicator";
			resources.ApplyResources(this.lblMPE_FEC_indicator, "lblMPE_FEC_indicator");
			this.lblMPE_FEC_indicator.Name = "lblMPE_FEC_indicator";
			resources.ApplyResources(this.lblReserved_future_use, "lblReserved_future_use");
			this.lblReserved_future_use.Name = "lblReserved_future_use";
			resources.ApplyResources(this.lblConstellation, "lblConstellation");
			this.lblConstellation.Name = "lblConstellation";
			resources.ApplyResources(this.lblHierarchy_information, "lblHierarchy_information");
			this.lblHierarchy_information.Name = "lblHierarchy_information";
			resources.ApplyResources(this.lblCode_rate_HP_stream, "lblCode_rate_HP_stream");
			this.lblCode_rate_HP_stream.Name = "lblCode_rate_HP_stream";
			resources.ApplyResources(this.lblCode_rate_LP, "lblCode_rate_LP");
			this.lblCode_rate_LP.Name = "lblCode_rate_LP";
			resources.ApplyResources(this.lblGuard_interval, "lblGuard_interval");
			this.lblGuard_interval.Name = "lblGuard_interval";
			resources.ApplyResources(this.cmbBandwidth, "cmbBandwidth");
			this.cmbBandwidth.FormattingEnabled = true;
			this.cmbBandwidth.MaxValue = 7L;
			this.cmbBandwidth.MinValue = 4L;
			this.cmbBandwidth.Name = "cmbBandwidth";
			this.cmbBandwidth.Value = "";
			resources.ApplyResources(this.cboTime_Slicing_indicator, "cboTime_Slicing_indicator");
			this.cboTime_Slicing_indicator.Name = "cboTime_Slicing_indicator";
			this.cboTime_Slicing_indicator.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.cboPrioryty, "cboPrioryty");
			this.cboPrioryty.Name = "cboPrioryty";
			this.cboPrioryty.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.cboMPE_FEC_indicator, "cboMPE_FEC_indicator");
			this.cboMPE_FEC_indicator.Name = "cboMPE_FEC_indicator";
			this.cboMPE_FEC_indicator.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.txtReserved_future_use, "txtReserved_future_use");
			this.txtReserved_future_use.MaxValue = 3L;
			this.txtReserved_future_use.MinValue = 0L;
			this.txtReserved_future_use.Name = "txtReserved_future_use";
			this.txtReserved_future_use.Value = new byte[0];
			resources.ApplyResources(this.cmbConstellation, "cmbConstellation");
			this.cmbConstellation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbConstellation.FormattingEnabled = true;
			this.cmbConstellation.MaxValue = 0L;
			this.cmbConstellation.MinValue = 0L;
			this.cmbConstellation.Name = "cmbConstellation";
			this.cmbConstellation.Value = "";
			resources.ApplyResources(this.cmbHierarchy_information, "cmbHierarchy_information");
			this.cmbHierarchy_information.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbHierarchy_information.FormattingEnabled = true;
			this.cmbHierarchy_information.MaxValue = 0L;
			this.cmbHierarchy_information.MinValue = 0L;
			this.cmbHierarchy_information.Name = "cmbHierarchy_information";
			this.cmbHierarchy_information.Value = "";
			resources.ApplyResources(this.cmbCode_rate_HP, "cmbCode_rate_HP");
			this.cmbCode_rate_HP.FormattingEnabled = true;
			this.cmbCode_rate_HP.MaxValue = 7L;
			this.cmbCode_rate_HP.MinValue = 5L;
			this.cmbCode_rate_HP.Name = "cmbCode_rate_HP";
			this.cmbCode_rate_HP.Value = "";
			resources.ApplyResources(this.cmbCode_rate_LP, "cmbCode_rate_LP");
			this.cmbCode_rate_LP.FormattingEnabled = true;
			this.cmbCode_rate_LP.MaxValue = 7L;
			this.cmbCode_rate_LP.MinValue = 5L;
			this.cmbCode_rate_LP.Name = "cmbCode_rate_LP";
			this.cmbCode_rate_LP.Value = "";
			resources.ApplyResources(this.cmbGuard_interval, "cmbGuard_interval");
			this.cmbGuard_interval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbGuard_interval.FormattingEnabled = true;
			this.cmbGuard_interval.MaxValue = 0L;
			this.cmbGuard_interval.MinValue = 0L;
			this.cmbGuard_interval.Name = "cmbGuard_interval";
			this.cmbGuard_interval.Value = "";
			resources.ApplyResources(this.lblTransmission_mode, "lblTransmission_mode");
			this.lblTransmission_mode.Name = "lblTransmission_mode";
			resources.ApplyResources(this.cmbTransmission_mode, "cmbTransmission_mode");
			this.cmbTransmission_mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTransmission_mode.FormattingEnabled = true;
			this.cmbTransmission_mode.MaxValue = 0L;
			this.cmbTransmission_mode.MinValue = 0L;
			this.cmbTransmission_mode.Name = "cmbTransmission_mode";
			this.cmbTransmission_mode.Value = "";
			resources.ApplyResources(this.lblOther_frequency_flag, "lblOther_frequency_flag");
			this.lblOther_frequency_flag.Name = "lblOther_frequency_flag";
			resources.ApplyResources(this.cboOther_frequency_flag, "cboOther_frequency_flag");
			this.cboOther_frequency_flag.Name = "cboOther_frequency_flag";
			this.cboOther_frequency_flag.UseVisualStyleBackColor = true;
			this.txtCentre_frequency.ForeColor = System.Drawing.Color.Black;
			this.txtCentre_frequency.IsValidated = true;
			resources.ApplyResources(this.txtCentre_frequency, "txtCentre_frequency");
			this.txtCentre_frequency.MaxValue = -1;
			this.txtCentre_frequency.Name = "txtCentre_frequency";
			this.txtCentre_frequency.NotNull = true;
			this.txtCentre_frequency.Value = new byte[0];
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "Terrestrial_delivery_system_descriptorDemo";
			base.grbContext.ResumeLayout(false);
			this.tableData.ResumeLayout(false);
			this.tableData.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
