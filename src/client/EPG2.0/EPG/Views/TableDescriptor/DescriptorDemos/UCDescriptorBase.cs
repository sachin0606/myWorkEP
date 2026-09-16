using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Com.Cn.Tool.Model;
using DeXin.EPG.Tools;

namespace EPG.Views.TableDescriptor.DescriptorDemos
{
	public class UCDescriptorBase : UserControl
	{
		private PageEnum _PageFlag = PageEnum.Empty;

		protected string msg = "";

		private IContainer components = null;

		protected GroupBox grbContext;

		public virtual DescriptorBase Descriotor { get; set; }

		public PageEnum PageFlag
		{
			get
			{
				return _PageFlag;
			}
			set
			{
				_PageFlag = value;
			}
		}

		public UCDescriptorBase()
		{
			InitializeComponent();
		}

		public UCDescriptorBase(PageEnum pageFlag)
		{
			InitializeComponent();
			PageFlag = pageFlag;
		}

		public virtual bool IsValidated(ErrorProvider error)
		{
			msg = "";
			return true;
		}

		protected virtual void BindingData()
		{
		}

		protected virtual void SetStyle()
		{
		}

		protected virtual void InitControl()
		{
		}

		private void UCDescriptorBase_Load(object sender, EventArgs e)
		{
			BindingData();
			SetStyle();
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
			this.grbContext = new System.Windows.Forms.GroupBox();
			base.SuspendLayout();
			this.grbContext.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grbContext.Location = new System.Drawing.Point(0, 0);
			this.grbContext.Name = "grbContext";
			this.grbContext.Size = new System.Drawing.Size(150, 150);
			this.grbContext.TabIndex = 2;
			this.grbContext.TabStop = false;
			this.grbContext.Text = "Data";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.grbContext);
			base.Name = "UCDescriptorBase";
			base.Load += new System.EventHandler(UCDescriptorBase_Load);
			base.ResumeLayout(false);
		}
	}
}
