using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace EPG.Views
{
	public class FormSystemSetting : FormDetailBase
	{
		private SystemSettingBLL sysbll = new SystemSettingBLL();

		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel2;

		private Label label1;

		private ComboBox cmbEPGEncoder;

		public FormSystemSetting()
		{
			InitializeComponent();
			base.PageFlag = PageEnum.Edit;
		}

		protected override void FormShown()
		{
			Init();
			SysFunction sysFunction = new SysFunction(base.FunctionName);
			base.ContentText = sysFunction.LocationFunctionName;
		}

		private void Init()
		{
			SystemSetting temp = sysbll.GetSystemSetting(SystemSettingCode.EPGEncoding);
			IList<BaseKey> encoders = BaseData.Ins.GetEncoders();
			string selectedValue = "";
			if (temp != null)
			{
				selectedValue = temp.Value;
				if (encoders.Where((BaseKey p) => p.BKKey == temp.Value).Count() == 0)
				{
					encoders.Insert(0, new BaseKey(temp.Value, temp.Value, true));
				}
			}
			Tool.BindingComboBox(encoders, cmbEPGEncoder);
			cmbEPGEncoder.SelectedValue = selectedValue;
		}

		protected override bool FunSubmit()
		{
			try
			{
				SystemSetting systemSetting = new SystemSetting(SystemSettingCode.EPGEncoding);
				string text = ((cmbEPGEncoder.SelectedItem == null) ? cmbEPGEncoder.Text.ToUpper().Trim() : cmbEPGEncoder.SelectedValue.ToString());
				Encoding encoding = CommTool.ConvertToEncoder(text);
				systemSetting.Value = text;
				systemSetting.Comment = SystemSettingCode.EPGEncoding.ToString();
				sysbll.Save(systemSetting);
				CommTool.SetEncoder(systemSetting.Value);
				Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.SaveSucceed]);
			}
			catch (Exception)
			{
				Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.SaveFailure]);
			}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EPG.Views.FormSystemSetting));
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbEPGEncoder = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel2);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.cmbEPGEncoder, 1, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			resources.ApplyResources(this.cmbEPGEncoder, "cmbEPGEncoder");
			this.cmbEPGEncoder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbEPGEncoder.FormattingEnabled = true;
			this.cmbEPGEncoder.Name = "cmbEPGEncoder";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormSystemSetting";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
