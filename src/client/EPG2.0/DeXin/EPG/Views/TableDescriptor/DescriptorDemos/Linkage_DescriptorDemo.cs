using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class Linkage_DescriptorDemo : DescriptorDemoBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label label1;

		private Label label2;

		private Label label3;

		private Label label4;

		private DescriptorCombobox dCbxLinkageType;

		private Label label5;

		private DescriptorTextBox txtOrgNID;

		private DescriptorTextBox txtServiceID;

		private NumTextBox txtPrivateData;

		private DescriptorTextBox txtTSID;

		public IList<KeyValuePair<string, string>> LinkageTypes
		{
			get
			{
				List<KeyValuePair<string, string>> types = new List<KeyValuePair<string, string>>();
				Enum.GetValues(typeof(LinkageType)).Cast<LinkageType>().ToList()
					.ForEach(delegate(LinkageType x)
					{
						List<KeyValuePair<string, string>> list = types;
						int num = (int)x;
						list.Add(new KeyValuePair<string, string>(num.ToString("X2"), x.ToString().Replace("_", " ")));
					});
				return types;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.Linkage_DescriptorDemo));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.dCbxLinkageType = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtOrgNID = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.txtServiceID = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.txtPrivateData = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.txtTSID = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			base.grbContext.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(base.grbContext, "grbContext");
			base.grbContext.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.dCbxLinkageType, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.txtOrgNID, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtServiceID, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtPrivateData, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.txtTSID, 1, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			resources.ApplyResources(this.dCbxLinkageType, "dCbxLinkageType");
			this.dCbxLinkageType.FormattingEnabled = true;
			this.dCbxLinkageType.MaxValue = 0L;
			this.dCbxLinkageType.MinValue = 0L;
			this.dCbxLinkageType.Name = "dCbxLinkageType";
			this.dCbxLinkageType.Value = "";
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			resources.ApplyResources(this.txtOrgNID, "txtOrgNID");
			this.txtOrgNID.MaxValue = 0L;
			this.txtOrgNID.MinValue = 0L;
			this.txtOrgNID.Name = "txtOrgNID";
			this.txtOrgNID.Value = new byte[0];
			resources.ApplyResources(this.txtServiceID, "txtServiceID");
			this.txtServiceID.MaxValue = 0L;
			this.txtServiceID.MinValue = 0L;
			this.txtServiceID.Name = "txtServiceID";
			this.txtServiceID.Value = new byte[0];
			resources.ApplyResources(this.txtPrivateData, "txtPrivateData");
			this.txtPrivateData.ForeColor = System.Drawing.Color.Black;
			this.txtPrivateData.IsValidated = true;
			this.txtPrivateData.Name = "txtPrivateData";
			this.txtPrivateData.NotNull = false;
			this.txtPrivateData.Value = new byte[0];
			resources.ApplyResources(this.txtTSID, "txtTSID");
			this.txtTSID.MaxValue = 0L;
			this.txtTSID.MinValue = 0L;
			this.txtTSID.Name = "txtTSID";
			this.txtTSID.Value = new byte[0];
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "Linkage_DescriptorDemo";
			base.grbContext.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
		}

		public Linkage_DescriptorDemo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
			dCbxLinkageType.DataSource = LinkageTypes;
			dCbxLinkageType.ValueMember = "key";
			dCbxLinkageType.DisplayMember = "value";
			CommTool.SetTextBoxInputNumber16(txtOrgNID);
			CommTool.SetTextBoxInputNumber16(txtPrivateData);
			CommTool.SetTextBoxInputNumber16(txtServiceID);
			CommTool.SetTextBoxInputNumber16(txtTSID);
		}

		protected override void BindingData()
		{
			if (base.PageFlag != PageEnum.Add && base.PageFlag != PageEnum.Empty)
			{
				byte[] array = Tool.ResolveByte(base.ET.DescriptionData, 0, 2);
				txtTSID.Value = Tool.ResolveByte(base.ET.DescriptionData, 0, 2);
				txtOrgNID.Text = Tool.BytesToString(Tool.ResolveByte(base.ET.DescriptionData, 2, 2));
				txtServiceID.Text = Tool.BytesToString(Tool.ResolveByte(base.ET.DescriptionData, 4, 2));
				dCbxLinkageType.Value = Tool.BytesToString(Tool.ResolveByte(base.ET.DescriptionData, 6, 1));
				txtPrivateData.Text = Tool.BytesToString(Tool.ResolveByte(base.ET.DescriptionData, 7, base.ET.DataLength - 7));
			}
			if (base.PageFlag == PageEnum.Show)
			{
				DescriptorTextBox descriptorTextBox = txtOrgNID;
				DescriptorTextBox descriptorTextBox2 = txtServiceID;
				NumTextBox numTextBox = txtPrivateData;
				bool flag = (txtTSID.ReadOnly = true);
				flag = (numTextBox.ReadOnly = flag);
				flag = (descriptorTextBox2.ReadOnly = flag);
				descriptorTextBox.ReadOnly = flag;
				dCbxLinkageType.Enabled = false;
			}
		}

		public override bool IsValidated(ErrorProvider error)
		{
			if (!txtOrgNID.IsValitdated(error) || !txtServiceID.IsValitdated(error) || !txtTSID.IsValitdated(error) || !txtPrivateData.IsValidated)
			{
				return false;
			}
			base.ET.DataLength = 7 + txtPrivateData.Value.Length;
			base.ET.DescriptionData = new byte[base.ET.DataLength];
			int startIndex = 0;
			Tool.ToByteArray(Tool.ToNumber16(txtTSID.Text), base.ET.DescriptionData, ref startIndex);
			Tool.ToByteArray(Tool.ToNumber16(txtOrgNID.Text), base.ET.DescriptionData, ref startIndex);
			Tool.ToByteArray(Tool.ToNumber16(txtServiceID.Text), base.ET.DescriptionData, ref startIndex);
			byte[] resource = Tool.StringToBytes(dCbxLinkageType.Value);
			Tool.ToByteArray(resource, base.ET.DescriptionData, ref startIndex);
			Tool.ToByteArray(txtPrivateData.Value, base.ET.DescriptionData, ref startIndex);
			return true;
		}
	}
}
