using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class TextBoxDemo : DescriptorDemoBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private ValidatTextBox txtData;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.TextBoxDemo));
			this.txtData = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			base.grbContext.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbContext, "grbContext");
			resources.ApplyResources(this.txtData, "txtData");
			this.txtData.IsValidated = true;
			this.txtData.Name = "txtData";
			this.txtData.NotNull = true;
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.txtData, 0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this, "$this");
			base.Name = "TextBoxDemo";
			base.grbContext.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
		}

		public TextBoxDemo()
		{
			InitializeComponent();
		}

		public TextBoxDemo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
		}

		protected override void BindingData()
		{
			if (base.PageFlag == PageEnum.Edit)
			{
				txtData.Text = CommTool.DescriptorBytesToString(base.ET.DescriptionData);
			}
			else if (base.PageFlag == PageEnum.Show)
			{
				txtData.Text = CommTool.DescriptorBytesToString(base.ET.DescriptionData);
				txtData.ReadOnly = true;
			}
		}

		public override bool IsValidated(ErrorProvider error)
		{
			if (txtData.IsValidated)
			{
				base.ET.DescriptionData = CommTool.DescriptorStringToBytes(txtData.Text);
				base.ET.DataLength = base.ET.DescriptionData.Length;
				return txtData.IsValidated;
			}
			return false;
		}
	}
}
