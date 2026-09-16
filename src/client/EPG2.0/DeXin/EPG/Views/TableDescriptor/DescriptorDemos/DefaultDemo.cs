using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class DefaultDemo : DescriptorDemoBase
	{
		private IContainer components = null;

		private NumTextBox txtData;

		private TableLayoutPanel tableLayoutPanel1;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.DefaultDemo));
			this.txtData = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			base.grbContext.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbContext, "grbContext");
			resources.ApplyResources(this.txtData, "txtData");
			this.txtData.ForeColor = System.Drawing.Color.Black;
			this.txtData.IsValidated = true;
			this.txtData.Name = "txtData";
			this.txtData.NotNull = true;
			this.txtData.Value = new byte[0];
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.txtData, 0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "DefaultDemo";
			base.grbContext.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
		}

		public DefaultDemo()
		{
			InitializeComponent();
		}

		public DefaultDemo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
		}

		protected override void BindingData()
		{
			if (base.PageFlag == PageEnum.Edit)
			{
				txtData.Value = base.ET.DescriptionData;
			}
			else if (base.PageFlag == PageEnum.Show)
			{
				txtData.Value = base.ET.DescriptionData;
				txtData.ReadOnly = true;
			}
		}

		public override bool IsValidated(ErrorProvider error)
		{
			if (base.PageFlag == PageEnum.Add || base.PageFlag == PageEnum.Edit)
			{
				if (Tool.GetLength(txtData.Text) % 2 > 0)
				{
					error.SetError(this, LocationControl.Ins.LocationItems[LacationEnum.InputFormatError]);
					txtData.IsValidated = false;
				}
				else
				{
					txtData.IsValidated = true;
				}
				base.ET.DescriptionData = txtData.Value;
				base.ET.DataLength = base.ET.DescriptionData.Length;
				return txtData.IsValidated;
			}
			return true;
		}
	}
}
