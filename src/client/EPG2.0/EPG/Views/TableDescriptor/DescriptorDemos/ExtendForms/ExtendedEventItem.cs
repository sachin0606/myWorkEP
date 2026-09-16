using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Com.Cn.Tool.Model;
using DeXin.EPG.Views.BaseForm;

namespace EPG.Views.TableDescriptor.DescriptorDemos.ExtendForms
{
	public class ExtendedEventItem : FormDetailBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel2;

		private Label label1;

		private Label label2;

		private TextBox txtKey;

		private TextBox txtText;

		public string Key
		{
			get
			{
				return txtKey.Text;
			}
		}

		public string Value
		{
			get
			{
				return txtText.Text;
			}
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
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtKey = new System.Windows.Forms.TextBox();
			this.txtText = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel2);
			base.grbText.Size = new System.Drawing.Size(363, 162);
			base.grbText.Text = "所有功能";
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.txtKey, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.txtText, 1, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(357, 143);
			this.tableLayoutPanel2.TabIndex = 0;
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Item Key";
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 66);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(51, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Item Text";
			this.txtKey.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.txtKey.Location = new System.Drawing.Point(60, 3);
			this.txtKey.Multiline = true;
			this.txtKey.Name = "txtKey";
			this.txtKey.Size = new System.Drawing.Size(260, 35);
			this.txtKey.TabIndex = 1;
			this.txtText.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.txtText.Location = new System.Drawing.Point(60, 44);
			this.txtText.Multiline = true;
			this.txtText.Name = "txtText";
			this.txtText.Size = new System.Drawing.Size(260, 57);
			this.txtText.TabIndex = 1;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(363, 237);
			base.Name = "ExtendedEventItem";
			this.Text = "Add Item";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public ExtendedEventItem(PageEnum pageflag)
		{
			InitializeComponent();
			base.PageFlag = pageflag;
		}

		protected override bool FunSubmit()
		{
			base.DialogResult = DialogResult.OK;
			return base.FunSubmit();
		}
	}
}
