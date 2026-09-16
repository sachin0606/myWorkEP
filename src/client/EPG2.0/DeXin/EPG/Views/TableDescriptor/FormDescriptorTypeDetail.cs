using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.TableDescriptor
{
	public class FormDescriptorTypeDetail : FormDetailBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblID;

		private Label lblTag;

		private Label lblName;

		private Label lblLocation;

		private TextBox txtID;

		private ValidatTextBox txtName;

		private CheckBox cboType;

		private ComboBox cmbLocation;

		private NumTextBox txtTag;

		private Label label1;

		private DescriptorTypeManager pageManager = new DescriptorTypeManager();

		private EnumPossibleLoaction PageLocation = EnumPossibleLoaction.NIT;

		private LogBLL logManager = new LogBLL();

		private DescriptorType ET = null;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.FormDescriptorTypeDetail));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblID = new System.Windows.Forms.Label();
			this.lblTag = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.lblLocation = new System.Windows.Forms.Label();
			this.txtID = new System.Windows.Forms.TextBox();
			this.txtName = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.cboType = new System.Windows.Forms.CheckBox();
			this.cmbLocation = new System.Windows.Forms.ComboBox();
			this.txtTag = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblID, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblTag, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblName, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblLocation, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtID, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtName, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.cboType, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.cmbLocation, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtTag, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 4);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.lblID, "lblID");
			this.lblID.Name = "lblID";
			resources.ApplyResources(this.lblTag, "lblTag");
			this.lblTag.Name = "lblTag";
			resources.ApplyResources(this.lblName, "lblName");
			this.lblName.Name = "lblName";
			resources.ApplyResources(this.lblLocation, "lblLocation");
			this.lblLocation.Name = "lblLocation";
			resources.ApplyResources(this.txtID, "txtID");
			this.txtID.Name = "txtID";
			resources.ApplyResources(this.txtName, "txtName");
			this.txtName.IsValidated = true;
			this.txtName.Name = "txtName";
			this.txtName.NotNull = true;
			resources.ApplyResources(this.cboType, "cboType");
			this.cboType.Name = "cboType";
			this.cboType.UseVisualStyleBackColor = true;
			this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLocation.FormattingEnabled = true;
			resources.ApplyResources(this.cmbLocation, "cmbLocation");
			this.cmbLocation.Name = "cmbLocation";
			this.txtTag.ForeColor = System.Drawing.Color.Black;
			this.txtTag.IsValidated = true;
			resources.ApplyResources(this.txtTag, "txtTag");
			this.txtTag.Name = "txtTag";
			this.txtTag.NotNull = true;
			this.txtTag.SetMaxLength = 1;
			this.txtTag.Value = new byte[0];
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.Name = "FormDescriptorTypeDetail";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormDescriptorTypeDetail(EnumPossibleLoaction location, long id, PageEnum _pageFlag)
		{
			InitializeComponent();
			_FunctionName = EnumFunctionName.SysFuncDescriptorType;
			InitControl();
			PageLocation = location;
			ET = new DescriptorType();
			ET.Id = id;
			base.PageFlag = _pageFlag;
		}

		protected override void FormLoad()
		{
			txtID.ReadOnly = true;
			cmbLocation.Enabled = false;
			cmbLocation.SelectedValue = PageLocation.ToString();
			InitForm();
		}

		protected override bool FunSubmit()
		{
			bool flag = false;
			switch (base.PageFlag)
			{
			case PageEnum.Add:
				if (!CheckSubmit())
				{
					return false;
				}
				ET = new DescriptorType();
				ET.DesTag = Tool.ToNumber32(txtTag.Text);
				ET.DesName = Tool.GetValue(txtName);
				ET.DesLocation = Tool.GetValue(cmbLocation);
				ET.DesType = ((!cboType.Checked) ? 1 : 0);
				flag = pageManager.AddDescriptorType(ET, out msg);
				if (flag)
				{
					logManager.AddLog(base.FunctionName, EnumOSName.Add, "Tag:[" + ET.DesTag + "]TagName:[" + ET.DesName + "]");
				}
				break;
			case PageEnum.Edit:
				if (!CheckSubmit())
				{
					return false;
				}
				ET.DesName = Tool.GetValue(txtName);
				ET.DesType = ((!cboType.Checked) ? 1 : 0);
				flag = pageManager.ModifyDescriptorType(ET, out msg);
				if (flag)
				{
					logManager.AddLog(base.FunctionName, EnumOSName.Edit, "Tag:[" + ET.DesTag + "]TagName:[" + ET.DesName + "]");
				}
				break;
			}
			Tool.ShowMessage(msg);
			if (flag)
			{
				base.DialogResult = DialogResult.OK;
			}
			return flag;
		}

		protected override void FunCancel()
		{
			Close();
		}

		private void InitForm()
		{
			if (ET.Id > 0)
			{
				ET = pageManager.GetDescriptorTypeByID(ET.Id);
				if (ET == null)
				{
					base.PageFlag = PageEnum.Add;
				}
			}
			switch (base.PageFlag)
			{
			case PageEnum.Edit:
				txtID.Text = Tool.GetString(ET.Id);
				txtTag.Text = Tool.FormatNumber(ET.DesTag, FormatString.Ins.X2);
				txtName.Text = Tool.GetString(ET.DesName);
				cmbLocation.SelectedValue = ET.DesLocation;
				cboType.Checked = ET.DesType == 0;
				txtTag.ReadOnly = true;
				break;
			case PageEnum.Show:
				txtID.Text = Tool.GetString(ET.Id);
				txtTag.Text = Tool.FormatNumber(ET.DesTag, FormatString.Ins.X2);
				txtName.Text = Tool.GetString(ET.DesName);
				cmbLocation.SelectedValue = ET.DesLocation;
				cboType.Checked = ET.DesType == 0;
				txtName.ReadOnly = true;
				txtTag.ReadOnly = true;
				cmbLocation.Enabled = false;
				cboType.Enabled = false;
				break;
			default:
			{
				TextBox textBox = txtID;
				ValidatTextBox validatTextBox = txtName;
				string text = (txtTag.Text = "");
				text = (validatTextBox.Text = text);
				textBox.Text = text;
				break;
			}
			}
		}

		private void InitControl()
		{
			Tool.BindingComboBox(BaseData.Ins.GetPosibleLocations(), cmbLocation);
		}

		private bool CheckSubmit()
		{
			if (!ValidateInput(grbText))
			{
				return false;
			}
			return true;
		}
	}
}
