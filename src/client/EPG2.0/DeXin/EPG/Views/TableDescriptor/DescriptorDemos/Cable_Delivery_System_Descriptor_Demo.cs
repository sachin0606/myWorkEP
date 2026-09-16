using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class Cable_Delivery_System_Descriptor_Demo : DescriptorDemoBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblSymbol_rate;

		private Label lblModulation;

		private Label lblFEC_outer;

		private Label lblReserved_future_use;

		private Label lblFrequency;

		private Label lblFEC_inner;

		private DescriptorTextBox txtFrequency;

		private DescriptorTextBox txtReserved_future_use;

		private DescriptorCombobox cmbModulation;

		private DescriptorCombobox cmbFEC_inner;

		private DescriptorCombobox cmbFEC_outer;

		private DescriptorTextBox txtSymoble_rate;

		public Cable_Delivery_System_Descriptor_Demo()
		{
			InitializeComponent();
		}

		public Cable_Delivery_System_Descriptor_Demo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
			InitControl();
		}

		protected override void InitControl()
		{
			Tool.BindingComboBox(BaseData.Ins.OuterFEC, cmbFEC_outer);
			Tool.BindingComboBox(BaseData.Ins.ModulationSchemeForCable, cmbModulation);
			Tool.BindingComboBox(BaseData.Ins.InnerFEC, cmbFEC_inner);
			CommTool.SetTextBoxInputNumberPoint(txtFrequency);
			CommTool.SetTextBoxInputNumberPoint(txtSymoble_rate);
			CommTool.SetTextBoxInputNumberPoint(txtSymoble_rate);
			DescriptorTextBox descriptorTextBox = txtReserved_future_use;
			bool visible = (lblReserved_future_use.Visible = false);
			descriptorTextBox.Visible = visible;
		}

		protected override void BindingData()
		{
			if (base.PageFlag != PageEnum.Add && base.PageFlag != PageEnum.Empty)
			{
				byte[] buffer = Tool.ResolveByte(base.ET.DescriptionData, 0, 4);
				byte b = 0;
				string text = Tool.BytesToString(buffer);
				txtFrequency.Text = text.Insert(4, ".");
				buffer = Tool.ResolveByte(base.ET.DescriptionData, 4, 1);
				txtReserved_future_use.Text = Tool.BytesToString(buffer);
				buffer = Tool.ResolveByte(base.ET.DescriptionData, 5, 1);
				b = buffer[0];
				text = Tool.GetInt32((b >> 4) & 0xF).ToString();
				txtReserved_future_use.Text += text;
				cmbFEC_outer.Value = (b & 0xF).ToString(FormatString.Ins.X2);
				buffer = Tool.ResolveByte(base.ET.DescriptionData, 6, 1);
				cmbModulation.Value = Tool.BytesToString(buffer);
				buffer = Tool.ResolveByte(base.ET.DescriptionData, 7, 3);
				txtSymoble_rate.Text = Tool.BytesToString(buffer).Insert(3, ".");
				buffer = Tool.ResolveByte(base.ET.DescriptionData, 10, 1);
				b = buffer[0];
				text = Tool.GetInt32((b >> 4) & 0xF).ToString();
				txtSymoble_rate.Text += text;
				cmbFEC_inner.Value = (b & 0xF).ToString(FormatString.Ins.X2);
			}
			if (base.PageFlag == PageEnum.Show)
			{
				cmbFEC_inner.Enabled = false;
				cmbModulation.Enabled = false;
				cmbFEC_outer.Enabled = false;
				txtFrequency.ReadOnly = true;
				txtReserved_future_use.ReadOnly = true;
				txtSymoble_rate.ReadOnly = true;
			}
		}

		public override bool IsValidated(ErrorProvider error)
		{
			if (!txtFrequency.IsValitdated(error) || !cmbModulation.IsValitdated(error) || !txtSymoble_rate.IsValitdated(error) || !cmbFEC_outer.IsValitdated(error) || !cmbFEC_inner.IsValitdated(error))
			{
				return false;
			}
			int startIndex = 0;
			base.ET.DataLength = 11;
			base.ET.DescriptionData = new byte[11];
			Int_String[] items = new Int_String[6]
			{
				new Int_String(8, CommTool.GetTextBoxString(4, 4, txtFrequency)),
				new Int_String(3, CommTool.GetTextBoxString(3, 0, txtReserved_future_use)),
				new Int_String(1, cmbFEC_outer.Value),
				new Int_String(2, cmbModulation.Value),
				new Int_String(7, CommTool.GetTextBoxString(3, 4, txtSymoble_rate)),
				new Int_String(1, cmbFEC_inner.Value)
			};
			Tool.ToByteArray(CommTool.NumberToByteString(items), base.ET.DescriptionData, ref startIndex);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.Cable_Delivery_System_Descriptor_Demo));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblSymbol_rate = new System.Windows.Forms.Label();
			this.lblModulation = new System.Windows.Forms.Label();
			this.lblFEC_outer = new System.Windows.Forms.Label();
			this.lblReserved_future_use = new System.Windows.Forms.Label();
			this.lblFrequency = new System.Windows.Forms.Label();
			this.lblFEC_inner = new System.Windows.Forms.Label();
			this.txtFrequency = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.txtReserved_future_use = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.cmbModulation = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.cmbFEC_inner = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.cmbFEC_outer = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.txtSymoble_rate = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			base.grbContext.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbContext, "grbContext");
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblSymbol_rate, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblModulation, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblFEC_outer, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblReserved_future_use, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblFrequency, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblFEC_inner, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.txtFrequency, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtReserved_future_use, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.cmbModulation, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.cmbFEC_inner, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.cmbFEC_outer, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtSymoble_rate, 1, 4);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.lblSymbol_rate, "lblSymbol_rate");
			this.lblSymbol_rate.Name = "lblSymbol_rate";
			resources.ApplyResources(this.lblModulation, "lblModulation");
			this.lblModulation.Name = "lblModulation";
			resources.ApplyResources(this.lblFEC_outer, "lblFEC_outer");
			this.lblFEC_outer.Name = "lblFEC_outer";
			resources.ApplyResources(this.lblReserved_future_use, "lblReserved_future_use");
			this.lblReserved_future_use.Name = "lblReserved_future_use";
			resources.ApplyResources(this.lblFrequency, "lblFrequency");
			this.lblFrequency.Name = "lblFrequency";
			resources.ApplyResources(this.lblFEC_inner, "lblFEC_inner");
			this.lblFEC_inner.Name = "lblFEC_inner";
			resources.ApplyResources(this.txtFrequency, "txtFrequency");
			this.txtFrequency.MaxValue = 0L;
			this.txtFrequency.MinValue = 0L;
			this.txtFrequency.Name = "txtFrequency";
			this.txtFrequency.Value = new byte[0];
			resources.ApplyResources(this.txtReserved_future_use, "txtReserved_future_use");
			this.txtReserved_future_use.MaxValue = 0L;
			this.txtReserved_future_use.MinValue = 0L;
			this.txtReserved_future_use.Name = "txtReserved_future_use";
			this.txtReserved_future_use.Value = new byte[0];
			this.cmbModulation.FormattingEnabled = true;
			resources.ApplyResources(this.cmbModulation, "cmbModulation");
			this.cmbModulation.MaxValue = 31L;
			this.cmbModulation.MinValue = 4L;
			this.cmbModulation.Name = "cmbModulation";
			this.cmbModulation.Value = "";
			this.cmbFEC_inner.FormattingEnabled = true;
			resources.ApplyResources(this.cmbFEC_inner, "cmbFEC_inner");
			this.cmbFEC_inner.MaxValue = 15L;
			this.cmbFEC_inner.MinValue = 7L;
			this.cmbFEC_inner.Name = "cmbFEC_inner";
			this.cmbFEC_inner.Value = "";
			this.cmbFEC_outer.FormattingEnabled = true;
			resources.ApplyResources(this.cmbFEC_outer, "cmbFEC_outer");
			this.cmbFEC_outer.MaxValue = 15L;
			this.cmbFEC_outer.MinValue = 3L;
			this.cmbFEC_outer.Name = "cmbFEC_outer";
			this.cmbFEC_outer.Value = "";
			resources.ApplyResources(this.txtSymoble_rate, "txtSymoble_rate");
			this.txtSymoble_rate.MaxValue = 0L;
			this.txtSymoble_rate.MinValue = 0L;
			this.txtSymoble_rate.Name = "txtSymoble_rate";
			this.txtSymoble_rate.Value = new byte[0];
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "Cable_Delivery_System_Descriptor_Demo";
			base.grbContext.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
