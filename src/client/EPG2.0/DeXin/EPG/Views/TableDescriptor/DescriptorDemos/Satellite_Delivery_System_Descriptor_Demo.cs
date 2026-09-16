using System;
using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class Satellite_Delivery_System_Descriptor_Demo : DescriptorDemoBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblSymobl_rate;

		private Label lblModulation;

		private Label lblPolarization;

		private Label lblWest_east_flag;

		private Label lblOrbital_position;

		private Label lblFrequency;

		private Label lblFEC_inner;

		private Panel panelWestEast_Flag;

		private RadioButton radEast;

		private RadioButton rabWest;

		private ComboBox cmbPolarization;

		private DescriptorTextBox txtFrequency;

		private DescriptorTextBox txtOrbital_position;

		private DescriptorCombobox cmbModulation;

		private DescriptorTextBox txtSymoble_rate;

		private DescriptorCombobox cmbFEC_inner;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.Satellite_Delivery_System_Descriptor_Demo));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblSymobl_rate = new System.Windows.Forms.Label();
			this.lblModulation = new System.Windows.Forms.Label();
			this.lblPolarization = new System.Windows.Forms.Label();
			this.lblWest_east_flag = new System.Windows.Forms.Label();
			this.lblOrbital_position = new System.Windows.Forms.Label();
			this.lblFrequency = new System.Windows.Forms.Label();
			this.lblFEC_inner = new System.Windows.Forms.Label();
			this.panelWestEast_Flag = new System.Windows.Forms.Panel();
			this.radEast = new System.Windows.Forms.RadioButton();
			this.rabWest = new System.Windows.Forms.RadioButton();
			this.cmbPolarization = new System.Windows.Forms.ComboBox();
			this.txtFrequency = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.txtOrbital_position = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.cmbModulation = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.txtSymoble_rate = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.cmbFEC_inner = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			base.grbContext.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.panelWestEast_Flag.SuspendLayout();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbContext, "grbContext");
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblSymobl_rate, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.lblModulation, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblPolarization, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblWest_east_flag, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblOrbital_position, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblFrequency, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblFEC_inner, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.panelWestEast_Flag, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.cmbPolarization, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtFrequency, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtOrbital_position, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.cmbModulation, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.txtSymoble_rate, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.cmbFEC_inner, 1, 6);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.lblSymobl_rate, "lblSymobl_rate");
			this.lblSymobl_rate.Name = "lblSymobl_rate";
			resources.ApplyResources(this.lblModulation, "lblModulation");
			this.lblModulation.Name = "lblModulation";
			resources.ApplyResources(this.lblPolarization, "lblPolarization");
			this.lblPolarization.Name = "lblPolarization";
			resources.ApplyResources(this.lblWest_east_flag, "lblWest_east_flag");
			this.lblWest_east_flag.Name = "lblWest_east_flag";
			resources.ApplyResources(this.lblOrbital_position, "lblOrbital_position");
			this.lblOrbital_position.Name = "lblOrbital_position";
			resources.ApplyResources(this.lblFrequency, "lblFrequency");
			this.lblFrequency.Name = "lblFrequency";
			resources.ApplyResources(this.lblFEC_inner, "lblFEC_inner");
			this.lblFEC_inner.Name = "lblFEC_inner";
			this.panelWestEast_Flag.Controls.Add(this.radEast);
			this.panelWestEast_Flag.Controls.Add(this.rabWest);
			resources.ApplyResources(this.panelWestEast_Flag, "panelWestEast_Flag");
			this.panelWestEast_Flag.Name = "panelWestEast_Flag";
			resources.ApplyResources(this.radEast, "radEast");
			this.radEast.Name = "radEast";
			this.radEast.TabStop = true;
			this.radEast.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.rabWest, "rabWest");
			this.rabWest.Checked = true;
			this.rabWest.Name = "rabWest";
			this.rabWest.TabStop = true;
			this.rabWest.UseVisualStyleBackColor = true;
			this.cmbPolarization.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPolarization.FormattingEnabled = true;
			resources.ApplyResources(this.cmbPolarization, "cmbPolarization");
			this.cmbPolarization.Name = "cmbPolarization";
			resources.ApplyResources(this.txtFrequency, "txtFrequency");
			this.txtFrequency.MaxValue = 0L;
			this.txtFrequency.MinValue = 0L;
			this.txtFrequency.Name = "txtFrequency";
			this.txtFrequency.Value = new byte[0];
			resources.ApplyResources(this.txtOrbital_position, "txtOrbital_position");
			this.txtOrbital_position.MaxValue = 0L;
			this.txtOrbital_position.MinValue = 0L;
			this.txtOrbital_position.Name = "txtOrbital_position";
			this.txtOrbital_position.Value = new byte[0];
			this.cmbModulation.FormattingEnabled = true;
			resources.ApplyResources(this.cmbModulation, "cmbModulation");
			this.cmbModulation.MaxValue = 31L;
			this.cmbModulation.MinValue = 4L;
			this.cmbModulation.Name = "cmbModulation";
			this.cmbModulation.Value = "";
			resources.ApplyResources(this.txtSymoble_rate, "txtSymoble_rate");
			this.txtSymoble_rate.MaxValue = 0L;
			this.txtSymoble_rate.MinValue = 0L;
			this.txtSymoble_rate.Name = "txtSymoble_rate";
			this.txtSymoble_rate.Value = new byte[0];
			this.cmbFEC_inner.FormattingEnabled = true;
			resources.ApplyResources(this.cmbFEC_inner, "cmbFEC_inner");
			this.cmbFEC_inner.MaxValue = 14L;
			this.cmbFEC_inner.MinValue = 7L;
			this.cmbFEC_inner.Name = "cmbFEC_inner";
			this.cmbFEC_inner.Value = "";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "Satellite_Delivery_System_Descriptor_Demo";
			base.grbContext.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panelWestEast_Flag.ResumeLayout(false);
			this.panelWestEast_Flag.PerformLayout();
			base.ResumeLayout(false);
		}

		public Satellite_Delivery_System_Descriptor_Demo()
		{
			InitializeComponent();
		}

		public Satellite_Delivery_System_Descriptor_Demo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
			InitControl();
		}

		protected override void InitControl()
		{
			Tool.BindingComboBox(BaseData.Ins.Polarizations, cmbPolarization);
			Tool.BindingComboBox(BaseData.Ins.ModulationSchemeForSatellite, cmbModulation);
			Tool.BindingComboBox(BaseData.Ins.InnerFEC, cmbFEC_inner);
			txtSymoble_rate.MaxLength = 8;
			CommTool.SetTextBoxInputNumberPoint(txtFrequency);
			CommTool.SetTextBoxInputNumberPoint(txtOrbital_position);
			CommTool.SetTextBoxInputNumberPoint(txtSymoble_rate);
		}

		protected override void BindingData()
		{
			if (base.PageFlag != PageEnum.Add && base.PageFlag != PageEnum.Empty)
			{
				byte[] buffer = Tool.ResolveByte(base.ET.DescriptionData, 0, 4);
				byte b = 0;
				txtFrequency.Text = Tool.BytesToString(buffer).Insert(3, ".");
				buffer = Tool.ResolveByte(base.ET.DescriptionData, 4, 2);
				txtOrbital_position.Text = Tool.BytesToString(buffer).Insert(3, ".");
				buffer = Tool.ResolveByte(base.ET.DescriptionData, 6, 1);
				b = buffer[0];
				if (1 == ((b >> 7) & 1))
				{
					rabWest.Checked = true;
				}
				else
				{
					radEast.Checked = true;
				}
				string selectedValue = ((b >> 5) & 3).ToString(FormatString.Ins.X2);
				cmbPolarization.SelectedValue = selectedValue;
				cmbModulation.Value = (b & 0x1F).ToString(FormatString.Ins.X2);
				buffer = Tool.ResolveByte(base.ET.DescriptionData, 7, 3);
				txtSymoble_rate.Text = Tool.BytesToString(buffer).Insert(3, ".");
				buffer = Tool.ResolveByte(base.ET.DescriptionData, 10, 1);
				b = buffer[0];
				string text = Tool.GetInt32((b >> 4) & 0xF).ToString();
				txtSymoble_rate.Text += text;
				cmbFEC_inner.Value = (b & 0xF).ToString(FormatString.Ins.X2);
			}
			if (base.PageFlag == PageEnum.Show)
			{
				cmbFEC_inner.Enabled = false;
				cmbModulation.Enabled = false;
				cmbPolarization.Enabled = false;
				panelWestEast_Flag.Enabled = false;
				txtFrequency.ReadOnly = true;
				txtOrbital_position.ReadOnly = true;
				txtSymoble_rate.ReadOnly = true;
			}
		}

		public override bool IsValidated(ErrorProvider error)
		{
			if (!txtFrequency.IsValitdated(error) || !txtOrbital_position.IsValitdated(error) || !cmbModulation.IsValitdated(error) || !txtSymoble_rate.IsValitdated(error) || !cmbFEC_inner.IsValitdated(error))
			{
				return false;
			}
			int startIndex = 0;
			base.ET.DataLength = 11;
			base.ET.DescriptionData = new byte[11];
			string textBoxString = CommTool.GetTextBoxString(3, 5, txtFrequency);
			Tool.ToByteArray(Tool.StringToBytes(textBoxString), base.ET.DescriptionData, ref startIndex);
			textBoxString = CommTool.GetTextBoxString(3, 1, txtOrbital_position);
			Tool.ToByteArray(Tool.StringToBytes(textBoxString), base.ET.DescriptionData, ref startIndex);
			byte b = (byte)(rabWest.Checked ? 1 : 0);
			textBoxString = Tool.GetValue(cmbPolarization);
			byte b2 = Tool.StringToBytes(textBoxString)[0];
			textBoxString = cmbModulation.Value;
			byte b3 = Tool.StringToBytes(textBoxString)[0];
			byte b4 = 0;
			b4 |= (byte)((b & 1) << 7);
			b4 |= (byte)((b2 & 3) << 5);
			b4 |= (byte)(b3 & 0x1F);
			Tool.ToByteArray(b4, base.ET.DescriptionData, ref startIndex);
			textBoxString = CommTool.GetTextBoxString(3, 4, txtSymoble_rate);
			b4 = Tool.StringToBytes(cmbFEC_inner.Value)[0];
			int a = Convert.ToInt32(textBoxString.Substring(0, 1));
			int b5 = Convert.ToInt32(textBoxString.Substring(1, 1));
			b4 = CommTool.NumberToByte(a, b5);
			Tool.ToByteArray(b4, base.ET.DescriptionData, ref startIndex);
			a = Convert.ToInt32(textBoxString.Substring(2, 1));
			b5 = Convert.ToInt32(textBoxString.Substring(3, 1));
			b4 = CommTool.NumberToByte(a, b5);
			Tool.ToByteArray(b4, base.ET.DescriptionData, ref startIndex);
			a = Convert.ToInt32(textBoxString.Substring(4, 1));
			b5 = Convert.ToInt32(textBoxString.Substring(5, 1));
			b4 = CommTool.NumberToByte(a, b5);
			Tool.ToByteArray(b4, base.ET.DescriptionData, ref startIndex);
			a = Convert.ToInt32(textBoxString.Substring(6, 1));
			b5 = Tool.GetInt32(Tool.StringToBytes(cmbFEC_inner.Value)[0]);
			b4 = CommTool.NumberToByte(a, b5);
			Tool.ToByteArray(b4, base.ET.DescriptionData, ref startIndex);
			return true;
		}
	}
}
