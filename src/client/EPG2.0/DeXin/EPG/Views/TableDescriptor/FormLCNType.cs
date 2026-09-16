using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.TableDescriptor
{
	public class FormLCNType : FormDetailBase
	{
		private IContainer components = null;

		private GroupBox grbLcnLength;

		private RadioButton rab14;

		private RadioButton rab10;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.FormLCNType));
			this.grbLcnLength = new System.Windows.Forms.GroupBox();
			this.rab14 = new System.Windows.Forms.RadioButton();
			this.rab10 = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.grbLcnLength.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.grbLcnLength);
			resources.ApplyResources(base.grbText, "grbText");
			this.grbLcnLength.Controls.Add(this.rab14);
			this.grbLcnLength.Controls.Add(this.rab10);
			resources.ApplyResources(this.grbLcnLength, "grbLcnLength");
			this.grbLcnLength.Name = "grbLcnLength";
			this.grbLcnLength.TabStop = false;
			resources.ApplyResources(this.rab14, "rab14");
			this.rab14.Name = "rab14";
			this.rab14.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.rab10, "rab10");
			this.rab10.Checked = true;
			this.rab10.Name = "rab10";
			this.rab10.TabStop = true;
			this.rab10.UseVisualStyleBackColor = true;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormLCNType";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.grbLcnLength.ResumeLayout(false);
			this.grbLcnLength.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormLCNType()
		{
			InitializeComponent();
			base.PageFlag = PageEnum.Edit;
		}

		protected override void FormShown()
		{
			if (BaseData.Ins.lcntype == LcnType.BIT_14)
			{
				rab14.Checked = true;
			}
			SysFunction sysFunction = new SysFunction(base.FunctionName);
			base.ContentText = sysFunction.LocationFunctionName;
		}

		protected override bool FunSubmit()
		{
			SystemSetting systemSetting = new SystemSetting(SystemSettingCode.LCNLENGTH);
			systemSetting.Value = (rab10.Checked ? LcnType.BIT_10.ToString() : LcnType.BIT_14.ToString());
			systemSetting.Comment = "LCNLENGTH";
			new SystemSettingBLL().Save(systemSetting);
			Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.SaveSucceed]);
			return true;
		}
	}
}
