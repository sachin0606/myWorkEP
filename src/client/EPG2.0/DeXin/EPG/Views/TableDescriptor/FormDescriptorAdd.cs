using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using DeXin.EPG.BLL;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.TableDescriptor
{
	public class FormDescriptorAdd : FormDetailBase
	{
		private IContainer components = null;

		private Label lblLocation;

		private ComboBox cmbLocation;

		private Label lblDescriptorType;

		private ComboBox cmbDescriptorType;

		private TableLayoutPanel tableLayoutPanel1;

		public FormDescriptorAdd()
		{
			InitializeComponent();
			InitControl();
		}

		private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				string value = Tool.GetValue(cmbLocation);
				if (!value.Contains('.'))
				{
					EnumPossibleLoaction location = (EnumPossibleLoaction)(object)Tool.GetEnum(typeof(EnumPossibleLoaction), value);
					Tool.BindingComboBox(BaseData.Ins.GetDescriptorTypesByLocation(location, true), cmbDescriptorType);
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void InitControl()
		{
			Tool.BindingComboBox(BaseData.Ins.GetPosibleLocations(), cmbLocation);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.FormDescriptorAdd));
			this.lblLocation = new System.Windows.Forms.Label();
			this.cmbLocation = new System.Windows.Forms.ComboBox();
			this.lblDescriptorType = new System.Windows.Forms.Label();
			this.cmbDescriptorType = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.lblLocation, "lblLocation");
			this.lblLocation.Name = "lblLocation";
			resources.ApplyResources(this.cmbLocation, "cmbLocation");
			this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLocation.FormattingEnabled = true;
			this.cmbLocation.Name = "cmbLocation";
			this.cmbLocation.SelectedIndexChanged += new System.EventHandler(cmbLocation_SelectedIndexChanged);
			resources.ApplyResources(this.lblDescriptorType, "lblDescriptorType");
			this.lblDescriptorType.Name = "lblDescriptorType";
			resources.ApplyResources(this.cmbDescriptorType, "cmbDescriptorType");
			this.cmbDescriptorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDescriptorType.FormattingEnabled = true;
			this.cmbDescriptorType.Name = "cmbDescriptorType";
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblLocation, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.cmbDescriptorType, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.cmbLocation, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblDescriptorType, 0, 1);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.Name = "FormDescriptorAdd";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
