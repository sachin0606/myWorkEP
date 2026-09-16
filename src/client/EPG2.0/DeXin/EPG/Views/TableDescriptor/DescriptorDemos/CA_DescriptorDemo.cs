using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class CA_DescriptorDemo : DescriptorDemoBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableCA_Des;

		private Label lblCA_system_ID;

		private Label lblReserved;

		private Label lblCA_PID;

		private Label lblData;

		private DescriptorTextBox txtCA_system_ID;

		private DescriptorTextBox txtReserved;

		private DescriptorTextBox txtCA_PID;

		private NumTextBox txtData;

		public CA_DescriptorDemo()
		{
			InitializeComponent();
		}

		public CA_DescriptorDemo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
			InitControl();
		}

		protected override void InitControl()
		{
			CommTool.SetTextBoxInputNumber16(txtCA_PID);
			CommTool.SetTextBoxInputNumber16(txtCA_system_ID);
			CommTool.SetTextBoxInputNumber16(txtReserved);
		}

		protected override void BindingData()
		{
			if (base.PageFlag == PageEnum.Add || base.PageFlag == PageEnum.Empty)
			{
				txtReserved.Text = "1";
			}
			else
			{
				byte[] buffer = Tool.ResolveByte(base.ET.DescriptionData, 0, 2);
				byte b = 0;
				string text = "";
				txtCA_system_ID.Text = Tool.BytesToString(buffer);
				b = Tool.ResolveByte(base.ET.DescriptionData, 2, 1)[0];
				text = ((byte)((b >> 5) & 7)).ToString(FormatString.Ins.X);
				txtReserved.Text = text;
				short num = 0;
				num |= (short)((b & 0x1F) << 8);
				b = Tool.ResolveByte(base.ET.DescriptionData, 3, 1)[0];
				text = ((short)(num | (short)(b & 0xFF))).ToString(FormatString.Ins.X4);
				txtCA_PID.Text += text;
				txtData.Value = Tool.ResolveByte(base.ET.DescriptionData, 4, base.ET.DataLength - 4);
			}
			if (base.PageFlag == PageEnum.Show)
			{
				txtData.ReadOnly = true;
				txtReserved.ReadOnly = true;
				txtCA_PID.ReadOnly = true;
				txtCA_system_ID.ReadOnly = true;
			}
		}

		public override bool IsValidated(ErrorProvider error)
		{
			if (!txtCA_system_ID.IsValitdated(error) || !txtReserved.IsValitdated(error) || !txtCA_PID.IsValitdated(error) || !txtData.IsValidated)
			{
				return false;
			}
			int startIndex = 0;
			base.ET.DataLength = 11;
			byte[] array = new byte[255];
			Tool.ToByteArray(Tool.ToNumber16(txtCA_system_ID.Text, false), array, ref startIndex);
			byte b = 0;
			byte b2 = Tool.ToNumberByte(txtReserved.Text, false);
			b |= (byte)((b2 & 7) << 5);
			short number = Tool.ToNumber16(txtCA_PID.Text, false);
			byte[] array2 = new byte[2];
			int startIndex2 = 0;
			Tool.ToByteArray(number, array2, ref startIndex2);
			b |= (byte)(array2[0] & 0x1F);
			Tool.ToByteArray(b, array, ref startIndex);
			Tool.ToByteArray(array2[1], array, ref startIndex);
			Tool.ToByteArray(txtData.Value, array, ref startIndex);
			base.ET.DataLength = startIndex;
			base.ET.DescriptionData = new byte[startIndex];
			startIndex = 0;
			for (int i = 0; i < base.ET.DataLength; i++)
			{
				Tool.ToByteArray(array[i], base.ET.DescriptionData, ref startIndex);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.CA_DescriptorDemo));
			this.tableCA_Des = new System.Windows.Forms.TableLayoutPanel();
			this.lblCA_system_ID = new System.Windows.Forms.Label();
			this.lblReserved = new System.Windows.Forms.Label();
			this.lblCA_PID = new System.Windows.Forms.Label();
			this.lblData = new System.Windows.Forms.Label();
			this.txtCA_system_ID = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.txtReserved = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.txtCA_PID = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.txtData = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			base.grbContext.SuspendLayout();
			this.tableCA_Des.SuspendLayout();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.tableCA_Des);
			resources.ApplyResources(base.grbContext, "grbContext");
			resources.ApplyResources(this.tableCA_Des, "tableCA_Des");
			this.tableCA_Des.Controls.Add(this.lblCA_system_ID, 0, 0);
			this.tableCA_Des.Controls.Add(this.lblReserved, 0, 1);
			this.tableCA_Des.Controls.Add(this.lblCA_PID, 0, 2);
			this.tableCA_Des.Controls.Add(this.lblData, 0, 3);
			this.tableCA_Des.Controls.Add(this.txtCA_system_ID, 1, 0);
			this.tableCA_Des.Controls.Add(this.txtReserved, 1, 1);
			this.tableCA_Des.Controls.Add(this.txtCA_PID, 1, 2);
			this.tableCA_Des.Controls.Add(this.txtData, 1, 3);
			this.tableCA_Des.Name = "tableCA_Des";
			resources.ApplyResources(this.lblCA_system_ID, "lblCA_system_ID");
			this.lblCA_system_ID.Name = "lblCA_system_ID";
			resources.ApplyResources(this.lblReserved, "lblReserved");
			this.lblReserved.Name = "lblReserved";
			resources.ApplyResources(this.lblCA_PID, "lblCA_PID");
			this.lblCA_PID.Name = "lblCA_PID";
			resources.ApplyResources(this.lblData, "lblData");
			this.lblData.Name = "lblData";
			resources.ApplyResources(this.txtCA_system_ID, "txtCA_system_ID");
			this.txtCA_system_ID.MaxValue = 65535L;
			this.txtCA_system_ID.MinValue = 0L;
			this.txtCA_system_ID.Name = "txtCA_system_ID";
			this.txtCA_system_ID.Value = new byte[0];
			resources.ApplyResources(this.txtReserved, "txtReserved");
			this.txtReserved.MaxValue = 7L;
			this.txtReserved.MinValue = 0L;
			this.txtReserved.Name = "txtReserved";
			this.txtReserved.Value = new byte[0];
			resources.ApplyResources(this.txtCA_PID, "txtCA_PID");
			this.txtCA_PID.MaxValue = 8191L;
			this.txtCA_PID.MinValue = 0L;
			this.txtCA_PID.Name = "txtCA_PID";
			this.txtCA_PID.Value = new byte[0];
			this.txtData.ForeColor = System.Drawing.Color.Black;
			this.txtData.IsValidated = true;
			resources.ApplyResources(this.txtData, "txtData");
			this.txtData.Name = "txtData";
			this.txtData.NotNull = true;
			this.txtData.SetMaxLength = 251;
			this.txtData.Value = new byte[0];
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "CA_DescriptorDemo";
			base.grbContext.ResumeLayout(false);
			this.tableCA_Des.ResumeLayout(false);
			this.tableCA_Des.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
