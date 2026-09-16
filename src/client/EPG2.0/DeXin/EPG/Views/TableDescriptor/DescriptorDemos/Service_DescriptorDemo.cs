using System;
using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class Service_DescriptorDemo : DescriptorDemoBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableService_Descriptor;

		private Label lblService_type;

		private DescriptorCombobox cmbService_type;

		private Label lblService_provider_name;

		private Label lblService_name;

		private DescriptorTextBox txtService_provider_name;

		private DescriptorTextBox txtService_name;

		public Service_DescriptorDemo()
		{
			InitializeComponent();
		}

		public Service_DescriptorDemo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
			InitControl();
		}

		protected override void InitControl()
		{
			Tool.BindingComboBox(BaseData.Ins.ServiceTypes, cmbService_type);
		}

		protected override void BindingData()
		{
			if (base.PageFlag != PageEnum.Add && base.PageFlag != PageEnum.Empty)
			{
				int num = 0;
				byte b = base.ET.DescriptionData[num];
				cmbService_type.Value = b.ToString(FormatString.Ins.X2);
				num++;
				b = base.ET.DescriptionData[num];
				int length = Convert.ToInt32(b);
				num++;
				txtService_provider_name.Text = CommTool.DescriptorBytesToString(Tool.ResolveByte(base.ET.DescriptionData, num, length));
				num += b;
				b = base.ET.DescriptionData[num];
				int length2 = Convert.ToInt32(b);
				num++;
				txtService_name.Text = CommTool.DescriptorBytesToString(Tool.ResolveByte(base.ET.DescriptionData, num, length2));
			}
			if (base.PageFlag == PageEnum.Show)
			{
				cmbService_type.Enabled = false;
				txtService_name.ReadOnly = true;
				txtService_provider_name.ReadOnly = true;
			}
		}

		public override bool IsValidated(ErrorProvider error)
		{
			if (!cmbService_type.IsValitdated(error) || !txtService_provider_name.IsValitdated(error) || !txtService_name.IsValitdated(error))
			{
				return false;
			}
			int startIndex = 0;
			byte[] array = CommTool.DescriptorStringToBytes(txtService_provider_name.Text);
			byte[] array2 = CommTool.DescriptorStringToBytes(txtService_name.Text);
			base.ET.DataLength = 3 + array2.Length + array.Length;
			if (base.ET.DataLength > 255)
			{
				error.SetError(txtService_name, ResourceManagement.GetLocationString("DescriptorExceedsMaxLength"));
				error.SetError(txtService_provider_name, ResourceManagement.GetLocationString("DescriptorExceedsMaxLength"));
				return false;
			}
			base.ET.DescriptionData = new byte[base.ET.DataLength];
			Int_String[] items = new Int_String[1]
			{
				new Int_String(2, cmbService_type.Value)
			};
			Tool.ToByteArray(CommTool.NumberToByteString(items), base.ET.DescriptionData, ref startIndex);
			Tool.ToByteArray((byte)array.Length, base.ET.DescriptionData, ref startIndex);
			Tool.ToByteArray(array, base.ET.DescriptionData, ref startIndex);
			Tool.ToByteArray((byte)array2.Length, base.ET.DescriptionData, ref startIndex);
			Tool.ToByteArray(array2, base.ET.DescriptionData, ref startIndex);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.Service_DescriptorDemo));
			this.tableService_Descriptor = new System.Windows.Forms.TableLayoutPanel();
			this.lblService_type = new System.Windows.Forms.Label();
			this.cmbService_type = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.lblService_provider_name = new System.Windows.Forms.Label();
			this.lblService_name = new System.Windows.Forms.Label();
			this.txtService_provider_name = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.txtService_name = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			base.grbContext.SuspendLayout();
			this.tableService_Descriptor.SuspendLayout();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.tableService_Descriptor);
			resources.ApplyResources(base.grbContext, "grbContext");
			resources.ApplyResources(this.tableService_Descriptor, "tableService_Descriptor");
			this.tableService_Descriptor.Controls.Add(this.lblService_type, 0, 0);
			this.tableService_Descriptor.Controls.Add(this.cmbService_type, 1, 0);
			this.tableService_Descriptor.Controls.Add(this.lblService_provider_name, 0, 1);
			this.tableService_Descriptor.Controls.Add(this.lblService_name, 0, 2);
			this.tableService_Descriptor.Controls.Add(this.txtService_provider_name, 1, 1);
			this.tableService_Descriptor.Controls.Add(this.txtService_name, 1, 2);
			this.tableService_Descriptor.Name = "tableService_Descriptor";
			resources.ApplyResources(this.lblService_type, "lblService_type");
			this.lblService_type.Name = "lblService_type";
			resources.ApplyResources(this.cmbService_type, "cmbService_type");
			this.cmbService_type.FormattingEnabled = true;
			this.cmbService_type.MaxValue = 254L;
			this.cmbService_type.MinValue = 128L;
			this.cmbService_type.Name = "cmbService_type";
			this.cmbService_type.Value = "";
			resources.ApplyResources(this.lblService_provider_name, "lblService_provider_name");
			this.lblService_provider_name.Name = "lblService_provider_name";
			resources.ApplyResources(this.lblService_name, "lblService_name");
			this.lblService_name.Name = "lblService_name";
			resources.ApplyResources(this.txtService_provider_name, "txtService_provider_name");
			this.txtService_provider_name.MaxValue = 0L;
			this.txtService_provider_name.MinValue = 0L;
			this.txtService_provider_name.Name = "txtService_provider_name";
			this.txtService_provider_name.Value = new byte[0];
			resources.ApplyResources(this.txtService_name, "txtService_name");
			this.txtService_name.MaxValue = 0L;
			this.txtService_name.MinValue = 0L;
			this.txtService_name.Name = "txtService_name";
			this.txtService_name.Value = new byte[0];
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "Service_DescriptorDemo";
			base.grbContext.ResumeLayout(false);
			this.tableService_Descriptor.ResumeLayout(false);
			this.tableService_Descriptor.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
