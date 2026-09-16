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
	public class FormMultiIPOutputDetail : FormDetailBase
	{
		private MultiIPOutputBLL multiIPOutputBll = new MultiIPOutputBLL();

		private LogBLL logBLL = new LogBLL();

		private MultipleipSettings multipleIP = new MultipleipSettings();

		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblTSTableID;

		private ValidatTextBox txtIPAddress;

		private ValidatTextBox txtPort;

		private Label lblPort;

		private Label lblIP;

		private ComboBox cmbTS;

		private IList<TsInfo> UsefullTS { get; set; }

		public FormMultiIPOutputDetail(PageEnum pageFlag, long id, IList<TsInfo> usefullTS)
		{
			InitializeComponent();
			base.PageFlag = pageFlag;
			UsefullTS = usefullTS;
			try
			{
				multipleIP = multiIPOutputBll.GetDataByID(id).FirstOrDefault();
				if (multipleIP == null)
				{
					multipleIP = new MultipleipSettings();
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		protected override void FormLoad()
		{
			_FunctionName = EnumFunctionName.SysFuncMultipleIPOutput;
			InitData();
		}

		protected override bool FunSubmit()
		{
			try
			{
				if (!ValidateInput(tableLayoutPanel1))
				{
					return false;
				}
				IPAddress address = null;
				if (!IPAddress.TryParse(txtIPAddress.Text.Trim(), out address))
				{
					txtIPAddress.IsValidated = false;
					Tool.ShowErrorProviderMessage(txtIPAddress, CommTool.ShowInputFormatError(lblIP));
					return false;
				}
				ushort result = 0;
				if (!ushort.TryParse(txtPort.Text.Trim(), out result))
				{
					txtPort.IsValidated = false;
					Tool.ShowErrorProviderMessage(txtPort, CommTool.ShowInputFormatError(lblIP));
					return false;
				}
				BindControlToObject();
				switch (base.PageFlag)
				{
				case PageEnum.Add:
					multiIPOutputBll.Add(multipleIP);
					logBLL.AddLog(EnumFunctionName.SysFuncMultipleIPOutput, EnumOSName.Add, "id:" + multipleIP.Id + "[" + multipleIP.Ip.ToString() + ":" + multipleIP.Port + "]");
					break;
				case PageEnum.Edit:
					multiIPOutputBll.Update(multipleIP);
					logBLL.AddLog(EnumFunctionName.SysFuncMultipleIPOutput, EnumOSName.Edit, "id:" + multipleIP.Id + "[" + multipleIP.Ip.ToString() + ":" + multipleIP.Port + "]");
					break;
				}
				Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.OperateSucceed]);
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
			base.DialogResult = DialogResult.OK;
			return true;
		}

		private void BindControlToObject()
		{
			multipleIP.TsTableId = Tool.GetInt64(cmbTS.SelectedValue);
			multipleIP.Ip = IPAddress.Parse(txtIPAddress.Text.Trim()).ToString();
			multipleIP.Port = Tool.GetUInt16(txtPort.Text.Trim());
		}

		private void BindObjectToControl()
		{
			cmbTS.SelectedValue = Tool.GetString(multipleIP.TsTableId);
			txtIPAddress.Text = multipleIP.Ip;
			txtPort.Text = multipleIP.Port.ToString();
		}

		private void BindComboTS()
		{
			IList<BaseKey> lst = ((IEnumerable<BaseKey>)UsefullTS).ToList();
			Tool.BindingComboBox(lst, cmbTS);
		}

		private void InitData()
		{
			BindComboTS();
			switch (base.PageFlag)
			{
			case PageEnum.Add:
				SetReadOnlyOnAdd();
				break;
			case PageEnum.Edit:
				SetReadOnlyOnEdit();
				BindObjectToControl();
				break;
			case PageEnum.Show:
				SetReadOnlyOnShow();
				BindObjectToControl();
				break;
			}
		}

		private void SetReadOnlyOnAdd()
		{
			txtIPAddress.ReadOnly = false;
			txtPort.ReadOnly = false;
		}

		private void SetReadOnlyOnEdit()
		{
			txtIPAddress.ReadOnly = false;
			txtPort.ReadOnly = false;
		}

		private void SetReadOnlyOnShow()
		{
			ValidatTextBox validatTextBox = txtIPAddress;
			bool readOnly = (txtPort.ReadOnly = true);
			validatTextBox.ReadOnly = readOnly;
			cmbTS.Enabled = false;
		}

		private void btnSelectTS_Click(object sender, EventArgs e)
		{
		}

		private void cmbNetwork_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindComboTS();
		}

		private void cmbTS_SelectedIndexChanged(object sender, EventArgs e)
		{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.SendControlManagement.FormMultiIPOutputDetail));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.txtPort = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.lblPort = new System.Windows.Forms.Label();
			this.lblIP = new System.Windows.Forms.Label();
			this.txtIPAddress = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.cmbTS = new System.Windows.Forms.ComboBox();
			this.lblTSTableID = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.txtPort, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblPort, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblIP, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtIPAddress, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.cmbTS, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblTSTableID, 0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.txtPort, "txtPort");
			this.txtPort.IsValidated = true;
			this.txtPort.Name = "txtPort";
			this.txtPort.NotNull = true;
			resources.ApplyResources(this.lblPort, "lblPort");
			this.lblPort.Name = "lblPort";
			resources.ApplyResources(this.lblIP, "lblIP");
			this.lblIP.Name = "lblIP";
			resources.ApplyResources(this.txtIPAddress, "txtIPAddress");
			this.txtIPAddress.IsValidated = true;
			this.txtIPAddress.Name = "txtIPAddress";
			this.txtIPAddress.NotNull = true;
			resources.ApplyResources(this.cmbTS, "cmbTS");
			this.cmbTS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTS.FormattingEnabled = true;
			this.cmbTS.Name = "cmbTS";
			this.cmbTS.SelectedIndexChanged += new System.EventHandler(cmbTS_SelectedIndexChanged);
			resources.ApplyResources(this.lblTSTableID, "lblTSTableID");
			this.lblTSTableID.Name = "lblTSTableID";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormMultiIPOutputDetail";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
