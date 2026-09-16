using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.SendControlManagement
{
	public class FormSingleIPOutput : FormDetailBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblIPAddress;

		private Label lblPort;

		private ValidatTextBox txtIPAddress;

		private ValidatTextBox txtPort;

		private SingleIPOutputBLL singleIPBLL = new SingleIPOutputBLL();

		private SimpleipSettings singleIP = new SimpleipSettings();

		private IList<SimpleipSettings> ipsets = new List<SimpleipSettings>();

		private LogBLL logBll = new LogBLL();

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.SendControlManagement.FormSingleIPOutput));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.txtIPAddress = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.lblIPAddress = new System.Windows.Forms.Label();
			this.lblPort = new System.Windows.Forms.Label();
			this.txtPort = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.txtIPAddress, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblIPAddress, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblPort, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtPort, 1, 2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.txtIPAddress, "txtIPAddress");
			this.txtIPAddress.IsValidated = true;
			this.txtIPAddress.Name = "txtIPAddress";
			this.txtIPAddress.NotNull = true;
			resources.ApplyResources(this.lblIPAddress, "lblIPAddress");
			this.lblIPAddress.Name = "lblIPAddress";
			resources.ApplyResources(this.lblPort, "lblPort");
			this.lblPort.Name = "lblPort";
			resources.ApplyResources(this.txtPort, "txtPort");
			this.txtPort.IsValidated = true;
			this.txtPort.Name = "txtPort";
			this.txtPort.NotNull = true;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormSingleIPOutput";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormSingleIPOutput()
		{
			InitializeComponent();
			base.PageFlag = PageEnum.Add;
			_FunctionName = EnumFunctionName.SysFuncSingleIPOutput;
		}

		protected override void FormLoad()
		{
			try
			{
				ipsets = singleIPBLL.GetData();
				singleIP = ipsets.FirstOrDefault();
				BindToControl();
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		protected override bool FunSubmit()
		{
			try
			{
				if (!Check())
				{
					return false;
				}
				if (singleIP == null)
				{
					singleIP = new SimpleipSettings();
				}
				BindToObject(singleIP);
				singleIPBLL.Add(singleIP);
				logBll.AddLog(EnumFunctionName.SysFuncSingleIPOutput, EnumOSName.Add, "ip:" + singleIP.Ip.ToString() + ":" + singleIP.Port);
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
				return false;
			}
			Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.OperateSucceed]);
			return true;
		}

		protected override void FunCancel()
		{
			base.FunCancel();
			Close();
		}

		private bool Check()
		{
			bool result = true;
			if (!ValidateInput(tableLayoutPanel1))
			{
				result = false;
			}
			IPAddress address = null;
			if (!IPAddress.TryParse(txtIPAddress.Text.Trim(), out address))
			{
				txtIPAddress.IsValidated = false;
				Tool.ShowErrorProviderMessage(txtIPAddress, lblIPAddress.Text + LocationControl.Ins.LocationItems[LacationEnum.InputFormatError]);
				result = false;
			}
			ushort result2 = 0;
			if (!ushort.TryParse(txtPort.Text.Trim(), out result2))
			{
				txtPort.IsValidated = false;
				Tool.ShowErrorProviderMessage(txtPort, CommTool.ShowInputFormatError(lblPort));
				result = false;
			}
			return result;
		}

		private void BindToObject(SimpleipSettings singleIP)
		{
			singleIP.Ip = IPAddress.Parse(txtIPAddress.Text.Trim()).ToString();
			singleIP.Port = ushort.Parse(txtPort.Text.Trim());
		}

		private void BindToControl()
		{
			if (singleIP != null)
			{
				txtIPAddress.Text = singleIP.Ip;
				txtPort.Text = singleIP.Port.ToString();
			}
		}
	}
}
