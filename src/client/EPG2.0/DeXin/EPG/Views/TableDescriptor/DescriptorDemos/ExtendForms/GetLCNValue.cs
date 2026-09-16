using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Com.Cn.Tool.Model;
using DeXin.EPG.Tools.EPGUserControl;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos.ExtendForms
{
	public class GetLCNValue : FormDetailBase
	{
		private IContainer components = null;

		private TableLayoutPanel table;

		private Label lblLCN;

		private NumTextBox numTextBox1;

		public int ReturnLCN
		{
			get
			{
				return numTextBox1.ValueOfInt32;
			}
			private set
			{
				numTextBox1.Text = value.ToString("X4");
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.ExtendForms.GetLCNValue));
			this.table = new System.Windows.Forms.TableLayoutPanel();
			this.lblLCN = new System.Windows.Forms.Label();
			this.numTextBox1 = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.table.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(base.error, "error");
			resources.ApplyResources(base.grbText, "grbText");
			base.grbText.Controls.Add(this.table);
			base.error.SetError(base.grbText, resources.GetString("grbText.Error"));
			base.error.SetIconAlignment(base.grbText, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("grbText.IconAlignment"));
			base.error.SetIconPadding(base.grbText, (int)resources.GetObject("grbText.IconPadding"));
			resources.ApplyResources(this.table, "table");
			this.table.Controls.Add(this.lblLCN, 0, 0);
			this.table.Controls.Add(this.numTextBox1, 1, 0);
			base.error.SetError(this.table, resources.GetString("table.Error"));
			base.error.SetIconAlignment(this.table, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("table.IconAlignment"));
			base.error.SetIconPadding(this.table, (int)resources.GetObject("table.IconPadding"));
			this.table.Name = "table";
			resources.ApplyResources(this.lblLCN, "lblLCN");
			base.error.SetError(this.lblLCN, resources.GetString("lblLCN.Error"));
			base.error.SetIconAlignment(this.lblLCN, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lblLCN.IconAlignment"));
			base.error.SetIconPadding(this.lblLCN, (int)resources.GetObject("lblLCN.IconPadding"));
			this.lblLCN.Name = "lblLCN";
			resources.ApplyResources(this.numTextBox1, "numTextBox1");
			base.error.SetError(this.numTextBox1, resources.GetString("numTextBox1.Error"));
			this.numTextBox1.ForeColor = System.Drawing.Color.Black;
			base.error.SetIconAlignment(this.numTextBox1, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("numTextBox1.IconAlignment"));
			base.error.SetIconPadding(this.numTextBox1, (int)resources.GetObject("numTextBox1.IconPadding"));
			this.numTextBox1.IsValidated = true;
			this.numTextBox1.MaxValue = 1023;
			this.numTextBox1.Name = "numTextBox1";
			this.numTextBox1.NotNull = true;
			this.numTextBox1.Value = new byte[0];
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.Name = "GetLCNValue";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.table.ResumeLayout(false);
			this.table.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public GetLCNValue(int number)
		{
			InitializeComponent();
			numTextBox1.MaxValue = 16383;
			base.PageFlag = PageEnum.Add;
			numTextBox1.SetMaxLength = 2;
			ReturnLCN = number;
		}

		protected override bool FunSubmit()
		{
			if (numTextBox1.IsValidated)
			{
				base.DialogResult = DialogResult.OK;
				return true;
			}
			return false;
		}

		protected override void FunCancel()
		{
			base.DialogResult = DialogResult.Cancel;
		}
	}
}
