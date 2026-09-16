using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class Short_Event_Descriptor_Demo : DescriptorDemoBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblLanguageCode;

		private ComboBox cmbLanguageCode;

		private Label lblEventName;

		private Label lblRemark;

		private TextBox txtRemark;

		private ValidatTextBox txtEventName;

		public Short_Event_Descriptor_Demo()
		{
			InitializeComponent();
		}

		public Short_Event_Descriptor_Demo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
			InitControl();
		}

		protected override void InitControl()
		{
			Tool.BindingComboBox(base.pageManger.GetLanguageCode(), cmbLanguageCode);
		}

		protected override void BindingData()
		{
			if (base.PageFlag != PageEnum.Add && base.PageFlag != PageEnum.Empty)
			{
				int num = 0;
				byte b = 0;
				byte[] bytes = Tool.ResolveByte(base.ET.DescriptionData, 0, 3);
				num += 3;
				cmbLanguageCode.SelectedValue = Encoding.ASCII.GetString(bytes);
				b = base.ET.DescriptionData[num];
				num++;
				bytes = Tool.ResolveByte(base.ET.DescriptionData, num, b);
				txtEventName.Text = CommTool.DescriptorBytesToString(bytes);
				num += b;
				b = base.ET.DescriptionData[num];
				num++;
				bytes = Tool.ResolveByte(base.ET.DescriptionData, num, b);
				txtRemark.Text = CommTool.DescriptorBytesToString(bytes);
			}
			if (base.PageFlag == PageEnum.Show)
			{
				cmbLanguageCode.Enabled = false;
				txtEventName.ReadOnly = true;
				txtRemark.ReadOnly = true;
			}
		}

		public override bool IsValidated(ErrorProvider error)
		{
			if (!ValidateChildren(ValidationConstraints.TabStop))
			{
				return false;
			}
			byte[] array = CommTool.DescriptorStringToBytes(txtEventName.Text);
			byte[] array2 = CommTool.DescriptorStringToBytes(txtRemark.Text);
			if (array.Length + array2.Length > 250)
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("DescriptorExceedsMaxLength"));
				return false;
			}
			base.ET.DataLength = array.Length + array2.Length + 5;
			base.ET.DescriptionData = new byte[base.ET.DataLength];
			int startIndex = 0;
			string value = Tool.GetValue(cmbLanguageCode);
			Tool.ToByteArray(Encoding.ASCII.GetBytes(value), base.ET.DescriptionData, ref startIndex);
			Tool.ToByteArray((byte)array.Length, base.ET.DescriptionData, ref startIndex);
			Tool.ToByteArray(CommTool.DescriptorStringToBytes(txtEventName.Text), base.ET.DescriptionData, ref startIndex);
			Tool.ToByteArray((byte)array2.Length, base.ET.DescriptionData, ref startIndex);
			Tool.ToByteArray(CommTool.DescriptorStringToBytes(txtRemark.Text), base.ET.DescriptionData, ref startIndex);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.Short_Event_Descriptor_Demo));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblLanguageCode = new System.Windows.Forms.Label();
			this.cmbLanguageCode = new System.Windows.Forms.ComboBox();
			this.lblEventName = new System.Windows.Forms.Label();
			this.lblRemark = new System.Windows.Forms.Label();
			this.txtRemark = new System.Windows.Forms.TextBox();
			this.txtEventName = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			base.grbContext.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbContext, "grbContext");
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblLanguageCode, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.cmbLanguageCode, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblEventName, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblRemark, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtRemark, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtEventName, 1, 1);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.lblLanguageCode, "lblLanguageCode");
			this.lblLanguageCode.Name = "lblLanguageCode";
			this.cmbLanguageCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLanguageCode.FormattingEnabled = true;
			resources.ApplyResources(this.cmbLanguageCode, "cmbLanguageCode");
			this.cmbLanguageCode.Name = "cmbLanguageCode";
			resources.ApplyResources(this.lblEventName, "lblEventName");
			this.lblEventName.Name = "lblEventName";
			resources.ApplyResources(this.lblRemark, "lblRemark");
			this.lblRemark.Name = "lblRemark";
			resources.ApplyResources(this.txtRemark, "txtRemark");
			this.txtRemark.Name = "txtRemark";
			this.txtEventName.IsValidated = true;
			resources.ApplyResources(this.txtEventName, "txtEventName");
			this.txtEventName.Name = "txtEventName";
			this.txtEventName.NotNull = true;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "Short_Event_Descriptor_Demo";
			base.grbContext.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
