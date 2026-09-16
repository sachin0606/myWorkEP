using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;
using DeXin.EPG.Views.TableDescriptor.DescriptorDemos;

namespace EPG.Views.TableDescriptor.DescriptorDemos
{
	public class Data_Broadcast_Id_DescriptorDemo : DescriptorDemoBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lbltxtdata_broadcast_id;

		private DescriptorTextBox txtdata_broadcast_id;

		private Label lblid_selector_bytes;

		private NumTextBox txtid_selector_bytes;

		public Data_Broadcast_Id_DescriptorDemo()
		{
			InitializeComponent();
		}

		public Data_Broadcast_Id_DescriptorDemo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
			InitControl();
		}

		protected override void InitControl()
		{
			CommTool.SetTextBoxInputNumber16(txtdata_broadcast_id);
		}

		protected override void BindingData()
		{
			if (base.PageFlag != PageEnum.Add && base.PageFlag != PageEnum.Empty)
			{
				byte[] buffer = Tool.ResolveByte(base.ET.DescriptionData, 0, 2);
				txtdata_broadcast_id.Text = Tool.BytesToString(buffer);
				txtid_selector_bytes.Value = Tool.ResolveByte(base.ET.DescriptionData, 2, base.ET.DataLength - 2);
			}
			if (base.PageFlag == PageEnum.Show)
			{
				txtdata_broadcast_id.ReadOnly = true;
				txtid_selector_bytes.ReadOnly = true;
			}
		}

		public override bool IsValidated(ErrorProvider error)
		{
			if (!txtdata_broadcast_id.IsValitdated(error) || !txtid_selector_bytes.IsValidated)
			{
				return false;
			}
			int startIndex = 0;
			base.ET.DataLength = 2 + txtid_selector_bytes.Value.Length;
			base.ET.DescriptionData = new byte[base.ET.DataLength];
			Tool.ToByteArray(Tool.ToNumber16(txtdata_broadcast_id.Text, false), base.ET.DescriptionData, ref startIndex);
			Tool.ToByteArray(txtid_selector_bytes.Value, base.ET.DescriptionData, ref startIndex);
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lbltxtdata_broadcast_id = new System.Windows.Forms.Label();
			this.txtdata_broadcast_id = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.lblid_selector_bytes = new System.Windows.Forms.Label();
			this.txtid_selector_bytes = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			base.grbContext.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.tableLayoutPanel1);
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.lbltxtdata_broadcast_id, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtdata_broadcast_id, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblid_selector_bytes, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtid_selector_bytes, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.13235f));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.86765f));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(286, 171);
			this.tableLayoutPanel1.TabIndex = 1;
			this.lbltxtdata_broadcast_id.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lbltxtdata_broadcast_id.AutoSize = true;
			this.lbltxtdata_broadcast_id.Location = new System.Drawing.Point(3, 3);
			this.lbltxtdata_broadcast_id.Name = "lbltxtdata_broadcast_id";
			this.lbltxtdata_broadcast_id.Size = new System.Drawing.Size(95, 13);
			this.lbltxtdata_broadcast_id.TabIndex = 0;
			this.lbltxtdata_broadcast_id.Text = "data_broadcast_id";
			this.txtdata_broadcast_id.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.txtdata_broadcast_id.Location = new System.Drawing.Point(104, 3);
			this.txtdata_broadcast_id.MaxValue = 65535L;
			this.txtdata_broadcast_id.MinValue = 0L;
			this.txtdata_broadcast_id.Name = "txtdata_broadcast_id";
			this.txtdata_broadcast_id.Size = new System.Drawing.Size(100, 20);
			this.txtdata_broadcast_id.TabIndex = 1;
			this.txtdata_broadcast_id.Value = new byte[0];
			this.lblid_selector_bytes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.lblid_selector_bytes.AutoSize = true;
			this.lblid_selector_bytes.Location = new System.Drawing.Point(3, 20);
			this.lblid_selector_bytes.Name = "lblid_selector_bytes";
			this.lblid_selector_bytes.Size = new System.Drawing.Size(95, 13);
			this.lblid_selector_bytes.TabIndex = 0;
			this.lblid_selector_bytes.Text = "id_selector_byte(s)";
			this.txtid_selector_bytes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtid_selector_bytes.ForeColor = System.Drawing.Color.Black;
			this.txtid_selector_bytes.IsValidated = true;
			this.txtid_selector_bytes.Location = new System.Drawing.Point(104, 23);
			this.txtid_selector_bytes.MaxLength = 758;
			this.txtid_selector_bytes.Multiline = true;
			this.txtid_selector_bytes.Name = "txtid_selector_bytes";
			this.txtid_selector_bytes.NotNull = true;
			this.txtid_selector_bytes.SetMaxLength = 253;
			this.txtid_selector_bytes.Size = new System.Drawing.Size(179, 145);
			this.txtid_selector_bytes.TabIndex = 2;
			this.txtid_selector_bytes.Value = new byte[0];
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "Data_Broadcast_Id_DescriptorDemo";
			base.grbContext.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
