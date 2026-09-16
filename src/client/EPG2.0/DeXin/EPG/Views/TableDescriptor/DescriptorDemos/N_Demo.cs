using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool.Model;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class N_Demo : DescriptorDemoBase
	{
		private IContainer components = null;

		private TextBox textBox1;

		private Button button2;

		private Button button1;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.N_Demo));
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			base.grbContext.SuspendLayout();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.textBox1);
			base.grbContext.Controls.Add(this.button2);
			base.grbContext.Controls.Add(this.button1);
			resources.ApplyResources(base.grbContext, "grbContext");
			resources.ApplyResources(this.button1, "button1");
			this.button1.Name = "button1";
			this.button1.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.button2, "button2");
			this.button2.Name = "button2";
			this.button2.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.textBox1, "textBox1");
			this.textBox1.Name = "textBox1";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "N_Demo";
			base.grbContext.ResumeLayout(false);
			base.grbContext.PerformLayout();
			base.ResumeLayout(false);
		}

		public N_Demo()
		{
			InitializeComponent();
		}

		public N_Demo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
		}
	}
}
