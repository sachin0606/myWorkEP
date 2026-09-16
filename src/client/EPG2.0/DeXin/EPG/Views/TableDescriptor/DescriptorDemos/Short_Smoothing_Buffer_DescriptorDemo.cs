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
	public class Short_Smoothing_Buffer_DescriptorDemo : DescriptorDemoBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private DescriptorCombobox cmbsb_size;

		private DescriptorCombobox cmbsb_leak_rate;

		private Label label2;

		private Label label1;

		private Label label3;

		private NumTextBox txtData;

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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.cmbsb_size = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbsb_leak_rate = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtData = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			base.grbContext.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.tableLayoutPanel1);
			base.grbContext.Size = new System.Drawing.Size(730, 445);
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
			this.tableLayoutPanel1.Controls.Add(this.txtData, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.cmbsb_size, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.cmbsb_leak_rate, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(724, 426);
			this.tableLayoutPanel1.TabIndex = 0;
			this.cmbsb_size.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.cmbsb_size.FormattingEnabled = true;
			this.cmbsb_size.Location = new System.Drawing.Point(96, 3);
			this.cmbsb_size.MaxValue = 0L;
			this.cmbsb_size.MinValue = 0L;
			this.cmbsb_size.Name = "cmbsb_size";
			this.cmbsb_size.Size = new System.Drawing.Size(169, 21);
			this.cmbsb_size.TabIndex = 1;
			this.cmbsb_size.Value = "";
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label2.AutoSize = true;
			this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label2.Location = new System.Drawing.Point(22, 34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "sb_leak_rate";
			this.cmbsb_leak_rate.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.cmbsb_leak_rate.FormattingEnabled = true;
			this.cmbsb_leak_rate.Location = new System.Drawing.Point(96, 30);
			this.cmbsb_leak_rate.MaxValue = 0L;
			this.cmbsb_leak_rate.MinValue = 0L;
			this.cmbsb_leak_rate.Name = "cmbsb_leak_rate";
			this.cmbsb_leak_rate.Size = new System.Drawing.Size(169, 21);
			this.cmbsb_leak_rate.TabIndex = 1;
			this.cmbsb_leak_rate.Value = "";
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(48, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(42, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "sb_size";
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 54);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(87, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "DVB_reserved(s)";
			this.txtData.ForeColor = System.Drawing.Color.Black;
			this.txtData.IsValidated = true;
			this.txtData.Location = new System.Drawing.Point(96, 57);
			this.txtData.MaxLength = 764;
			this.txtData.Multiline = true;
			this.txtData.Name = "txtData";
			this.txtData.NotNull = true;
			this.txtData.Size = new System.Drawing.Size(352, 209);
			this.txtData.TabIndex = 4;
			this.txtData.Value = new byte[0];
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "Short_Smoothing_Buffer_DescriptorDemo";
			base.Size = new System.Drawing.Size(730, 445);
			base.grbContext.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
		}

		public Short_Smoothing_Buffer_DescriptorDemo()
		{
			InitializeComponent();
		}

		public Short_Smoothing_Buffer_DescriptorDemo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
			InitControl();
		}

		protected override void InitControl()
		{
			Tool.BindingComboBox(BaseData.Ins.SmoothingBufferSize, cmbsb_size);
			Tool.BindingComboBox(BaseData.Ins.SmoothingBufferLeakRate, cmbsb_leak_rate);
		}

		protected override void BindingData()
		{
			if (base.PageFlag != PageEnum.Add && base.PageFlag != PageEnum.Empty)
			{
				int start = 0;
				byte b = Tool.ResolveByte(base.ET.DescriptionData, 1, ref start)[0];
				cmbsb_size.Value = (b >> 6).ToString("X2");
				cmbsb_leak_rate.Value = (b & 0x3F).ToString("X2");
				txtData.Value = Tool.ResolveByte(base.ET.DescriptionData, base.ET.DataLength - 1, ref start);
			}
		}

		public override bool IsValidated(ErrorProvider error)
		{
			if (!cmbsb_size.IsValitdated(error) || !cmbsb_leak_rate.IsValitdated(error))
			{
				return false;
			}
			int startIndex = 1;
			base.ET.DataLength = txtData.Value.Length + 1;
			base.ET.DescriptionData = new byte[base.ET.DataLength];
			base.ET.DescriptionData[0] = (byte)((Tool.StringToBytes(cmbsb_size.Value)[0] & 3) << 6);
			base.ET.DescriptionData[0] |= (byte)(Tool.StringToBytes(cmbsb_leak_rate.Value)[0] & 0x3F);
			Tool.ToByteArray(txtData.Value, base.ET.DescriptionData, ref startIndex);
			return true;
		}
	}
}
