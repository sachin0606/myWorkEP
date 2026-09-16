using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Tools;
using EPG.Views.TableDescriptor.DescriptorDemos;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class DescriptorDemoBase : UCDescriptorBase
	{
		private IContainer components = null;

		private DescriptorManagement _pageManger;

		private DescriptorSigleBase _ET;

		protected DescriptorManagement pageManger
		{
			get
			{
				if (_pageManger == null)
				{
					_pageManger = new DescriptorManagement();
				}
				return _pageManger;
			}
		}

		public DescriptorSigleBase ET
		{
			get
			{
				return _ET;
			}
			set
			{
				_ET = value;
			}
		}

		public override DescriptorBase Descriotor
		{
			get
			{
				return ET;
			}
			set
			{
				ET = (DescriptorSigleBase)value;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.DescriptorDemoBase));
			base.SuspendLayout();
			resources.ApplyResources(base.grbContext, "grbContext");
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "DescriptorDemoBase";
			base.ResumeLayout(false);
		}

		public DescriptorDemoBase()
		{
			InitializeComponent();
		}

		public DescriptorDemoBase(DescriptorBase descriptor, PageEnum p_PageFlag)
			: base(p_PageFlag)
		{
			InitializeComponent();
			Descriotor = descriptor;
		}
	}
}
