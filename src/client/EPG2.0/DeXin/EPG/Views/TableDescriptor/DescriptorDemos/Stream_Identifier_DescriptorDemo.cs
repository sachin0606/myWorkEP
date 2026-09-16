using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class Stream_Identifier_DescriptorDemo : DescriptorDemoBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblcomponent_tag;

		private DescriptorTextBox txtcomponent_tag;

		public Stream_Identifier_DescriptorDemo()
		{
			InitializeComponent();
		}

		public Stream_Identifier_DescriptorDemo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
			InitControl();
		}

		protected override void InitControl()
		{
			CommTool.SetTextBoxInputNumber16(txtcomponent_tag);
		}

		protected override void BindingData()
		{
			if (base.PageFlag != PageEnum.Add && base.PageFlag != PageEnum.Empty)
			{
				byte[] buffer = Tool.ResolveByte(base.ET.DescriptionData, 0, 1);
				byte b = 0;
				string text = "";
				txtcomponent_tag.Text = Tool.BytesToString(buffer);
			}
			if (base.PageFlag == PageEnum.Show)
			{
				txtcomponent_tag.ReadOnly = true;
			}
		}

		public override bool IsValidated(ErrorProvider error)
		{
			if (!txtcomponent_tag.IsValitdated(error))
			{
				return false;
			}
			int startIndex = 0;
			base.ET.DataLength = 1;
			base.ET.DescriptionData = new byte[1];
			Tool.ToByteArray(Tool.ToNumberByte(txtcomponent_tag.Text, false), base.ET.DescriptionData, ref startIndex);
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
			this.lblcomponent_tag = new System.Windows.Forms.Label();
			this.txtcomponent_tag = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			base.grbContext.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.tableLayoutPanel1);
			base.grbContext.Size = new System.Drawing.Size(586, 291);
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.lblcomponent_tag, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtcomponent_tag, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.13235f));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.86765f));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(580, 272);
			this.tableLayoutPanel1.TabIndex = 0;
			this.lblcomponent_tag.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblcomponent_tag.AutoSize = true;
			this.lblcomponent_tag.Location = new System.Drawing.Point(3, 9);
			this.lblcomponent_tag.Name = "lblcomponent_tag";
			this.lblcomponent_tag.Size = new System.Drawing.Size(81, 13);
			this.lblcomponent_tag.TabIndex = 0;
			this.lblcomponent_tag.Text = "component_tag";
			this.txtcomponent_tag.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.txtcomponent_tag.Location = new System.Drawing.Point(90, 6);
			this.txtcomponent_tag.MaxValue = 255L;
			this.txtcomponent_tag.MinValue = 0L;
			this.txtcomponent_tag.Name = "txtcomponent_tag";
			this.txtcomponent_tag.Size = new System.Drawing.Size(100, 20);
			this.txtcomponent_tag.TabIndex = 1;
			this.txtcomponent_tag.Value = new byte[0];
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "Stream_Identifier_DescriptorDeom";
			base.Size = new System.Drawing.Size(586, 291);
			base.grbContext.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
