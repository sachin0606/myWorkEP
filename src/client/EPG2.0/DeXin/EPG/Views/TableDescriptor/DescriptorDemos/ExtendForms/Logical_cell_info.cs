using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Tools.EPGUserControl;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos.ExtendForms
{
	public class Logical_cell_info : FormDetailBase
	{
		private int horizontal_length = 0;

		private int vertical_length = 0;

		private byte[] currentData = new byte[1];

		private IContainer components = null;

		private TableLayoutPanel tableContent;

		private Label lblcell_linkage_infoid;

		private Label lblbouquet_id;

		private NumTextBox txtbouquet_id;

		private Label lbloriginal_network_id;

		private Label lbltransport_stream_id;

		private Label lblservice_id;

		private Label lblevent_id;

		private NumTextBox txtoriginal_network_id;

		private NumTextBox txttransport_stream_id;

		private NumTextBox txtservice_id;

		private NumTextBox txtevent_id;

		private GroupBox grbLogicalCellInfo;

		private GroupBox grbcell_linkage_info;

		private TableLayoutPanel tableLayoutPanel2;

		private Label lbllogical_cell_id;

		private Label lbllogical_cell_presentation_info;

		private Label lblelementary_cell_field;

		private NumTextBox txtlogical_cell_id;

		private Panel pnlElements;

		private DescriptorCombobox cmbcell_linkage_infoid;

		private DescriptorCombobox cmblogical_cell_presentation_info;

		public Logical_cell_info(int _horizontal_length, int _vertical_length, byte[] _data, PageEnum _pageFlag = PageEnum.Add)
		{
			InitializeComponent();
			base.PageFlag = _pageFlag;
			horizontal_length = _horizontal_length;
			vertical_length = _vertical_length;
			currentData = _data;
			InitControls();
		}

		protected override bool FunSubmit()
		{
			if (!txtlogical_cell_id.IsValidated)
			{
				return false;
			}
			base.DialogResult = DialogResult.OK;
			return true;
		}

		private void InitControls()
		{
			Tool.BindingComboBox(BaseData.Ins.LogicalCellPresentationInfo, cmblogical_cell_presentation_info);
			Tool.BindingComboBox(BaseData.Ins.CellLinkageInfo, cmbcell_linkage_infoid);
			int num = 30;
			int num2 = 40;
			int num3 = 0;
			for (int i = 0; i <= vertical_length; i++)
			{
				for (int j = 0; j <= horizontal_length; j++)
				{
					CheckBox checkBox = new CheckBox();
					checkBox.Size = new Size(num2, num);
					checkBox.Location = new Point(j * num2, i * num);
					checkBox.Name = num3.ToString();
					checkBox.Text = num3.ToString();
					num3++;
					pnlElements.Controls.Add(checkBox);
				}
			}
		}

		public byte[] ToBytes()
		{
			byte[] array = new byte[255];
			int startIndex = 0;
			byte b = 0;
			b |= (byte)((txtlogical_cell_id.Value[0] & 0x3F) << 2);
			b |= 3;
			Tool.ToByteArray(b, array, ref startIndex);
			byte b2 = Tool.StringToBytes(cmblogical_cell_presentation_info.Value)[0];
			b = 248;
			b |= (byte)(b2 & 7);
			Tool.ToByteArray(b, array, ref startIndex);
			int num = 0;
			IList<byte> list = new List<byte>();
			foreach (CheckBox control in pnlElements.Controls)
			{
				if (control.Checked)
				{
					num++;
					b = 192;
					b |= (byte)(Tool.GetByte(control.Name) & 0x3F);
					list.Add(b);
				}
			}
			Tool.ToByteArray(Tool.GetByte(num), array, ref startIndex);
			Tool.ToByteArray(list.ToArray(), array, ref startIndex);
			byte b3 = Tool.GetByte(cmbcell_linkage_infoid.SelectedValue);
			Tool.ToByteArray(b3, array, ref startIndex);
			short number = Tool.ToNumber16(Tool.GetString(txtbouquet_id.Text));
			if (b3 == 1)
			{
				Tool.ToByteArray(number, array, ref startIndex);
			}
			else if (b3 == 2 || b3 == 3)
			{
				number = Tool.ToNumber16(Tool.GetString(txtoriginal_network_id.Text));
				Tool.ToByteArray(number, array, ref startIndex);
				number = Tool.ToNumber16(Tool.GetString(txttransport_stream_id.Text));
				Tool.ToByteArray(number, array, ref startIndex);
				number = Tool.ToNumber16(Tool.GetString(txtservice_id.Text));
				Tool.ToByteArray(number, array, ref startIndex);
			}
			else if (b3 == 4)
			{
				number = Tool.ToNumber16(Tool.GetString(txtoriginal_network_id.Text));
				Tool.ToByteArray(number, array, ref startIndex);
				number = Tool.ToNumber16(Tool.GetString(txttransport_stream_id.Text));
				Tool.ToByteArray(number, array, ref startIndex);
				number = Tool.ToNumber16(Tool.GetString(txtservice_id.Text));
				Tool.ToByteArray(number, array, ref startIndex);
				number = Tool.ToNumber16(Tool.GetString(txtevent_id.Text));
				Tool.ToByteArray(number, array, ref startIndex);
			}
			byte[] array2 = new byte[startIndex];
			Array.Copy(array, array2, startIndex);
			return array2;
		}

		private void Get_cell_linkage_info_Load(object sender, EventArgs e)
		{
			int start = 0;
			byte[] array = Tool.ResolveByte(currentData, 2, ref start);
			txtlogical_cell_id.Text = ((byte)(array[0] >> 2)).ToString(FormatString.Ins.X2);
			string selectedValue = ((byte)(array[1] & 7)).ToString(FormatString.Ins.X2);
			cmblogical_cell_presentation_info.SelectedValue = selectedValue;
			array = Tool.ResolveByte(currentData, 1, ref start);
			int length = array[0];
			byte[] array2 = Tool.ResolveByte(currentData, length, ref start);
			List<byte> list = new List<byte>();
			byte[] array3 = array2;
			foreach (byte b in array3)
			{
				list.Add((byte)(b & 0x3F));
			}
			foreach (byte item in list)
			{
				foreach (CheckBox control in pnlElements.Controls)
				{
					if (Tool.GetByte(control.Name) == item)
					{
						control.Checked = true;
						break;
					}
				}
			}
			byte b2 = currentData[start++];
			selectedValue = b2.ToString(FormatString.Ins.X2);
			cmbcell_linkage_infoid.SelectedValue = selectedValue;
			switch (b2)
			{
			case 1:
				txtbouquet_id.Value = Tool.ResolveByte(currentData, 2, ref start);
				break;
			default:
				if (b2 != 3)
				{
					if (b2 == 4)
					{
						txtoriginal_network_id.Value = Tool.ResolveByte(currentData, 2, ref start);
						txttransport_stream_id.Value = Tool.ResolveByte(currentData, 2, ref start);
						txtservice_id.Value = Tool.ResolveByte(currentData, 2, ref start);
						txtevent_id.Value = Tool.ResolveByte(currentData, 2, ref start);
					}
					break;
				}
				goto case 2;
			case 2:
				txtoriginal_network_id.Value = Tool.ResolveByte(currentData, 2, ref start);
				txttransport_stream_id.Value = Tool.ResolveByte(currentData, 2, ref start);
				txtservice_id.Value = Tool.ResolveByte(currentData, 2, ref start);
				break;
			}
		}

		private void cmbcell_linkage_infoid_SelectedIndexChanged(object sender, EventArgs e)
		{
			byte b = Tool.GetByte(cmbcell_linkage_infoid.Value);
			switch (b)
			{
			case 0:
				txtbouquet_id.ReadOnly = true;
				txtoriginal_network_id.ReadOnly = true;
				txttransport_stream_id.ReadOnly = true;
				txtservice_id.ReadOnly = true;
				txtevent_id.ReadOnly = true;
				break;
			case 1:
				txtbouquet_id.ReadOnly = false;
				txtoriginal_network_id.ReadOnly = true;
				txttransport_stream_id.ReadOnly = true;
				txtservice_id.ReadOnly = true;
				txtevent_id.ReadOnly = true;
				break;
			default:
				if (b != 3)
				{
					if (b == 4)
					{
						txtbouquet_id.ReadOnly = true;
						txtoriginal_network_id.ReadOnly = false;
						txttransport_stream_id.ReadOnly = false;
						txtservice_id.ReadOnly = false;
						txtevent_id.ReadOnly = false;
					}
					break;
				}
				goto case 2;
			case 2:
				txtbouquet_id.ReadOnly = true;
				txtoriginal_network_id.ReadOnly = false;
				txttransport_stream_id.ReadOnly = false;
				txtservice_id.ReadOnly = false;
				txtevent_id.ReadOnly = true;
				break;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.ExtendForms.Logical_cell_info));
			this.tableContent = new System.Windows.Forms.TableLayoutPanel();
			this.lblcell_linkage_infoid = new System.Windows.Forms.Label();
			this.lblbouquet_id = new System.Windows.Forms.Label();
			this.txtbouquet_id = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.lbloriginal_network_id = new System.Windows.Forms.Label();
			this.lbltransport_stream_id = new System.Windows.Forms.Label();
			this.lblservice_id = new System.Windows.Forms.Label();
			this.lblevent_id = new System.Windows.Forms.Label();
			this.txtoriginal_network_id = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.txttransport_stream_id = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.txtservice_id = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.txtevent_id = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.cmbcell_linkage_infoid = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.grbLogicalCellInfo = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.cmblogical_cell_presentation_info = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.lbllogical_cell_id = new System.Windows.Forms.Label();
			this.txtlogical_cell_id = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.lbllogical_cell_presentation_info = new System.Windows.Forms.Label();
			this.lblelementary_cell_field = new System.Windows.Forms.Label();
			this.pnlElements = new System.Windows.Forms.Panel();
			this.grbcell_linkage_info = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableContent.SuspendLayout();
			this.grbLogicalCellInfo.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.grbcell_linkage_info.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(base.error, "error");
			resources.ApplyResources(base.grbText, "grbText");
			base.grbText.Controls.Add(this.grbcell_linkage_info);
			base.grbText.Controls.Add(this.grbLogicalCellInfo);
			base.error.SetError(base.grbText, resources.GetString("grbText.Error"));
			base.error.SetIconAlignment(base.grbText, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("grbText.IconAlignment"));
			base.error.SetIconPadding(base.grbText, (int)resources.GetObject("grbText.IconPadding"));
			resources.ApplyResources(this.tableContent, "tableContent");
			this.tableContent.Controls.Add(this.lblcell_linkage_infoid, 0, 0);
			this.tableContent.Controls.Add(this.lblbouquet_id, 0, 1);
			this.tableContent.Controls.Add(this.txtbouquet_id, 1, 1);
			this.tableContent.Controls.Add(this.lbloriginal_network_id, 0, 2);
			this.tableContent.Controls.Add(this.lbltransport_stream_id, 0, 3);
			this.tableContent.Controls.Add(this.lblservice_id, 0, 4);
			this.tableContent.Controls.Add(this.lblevent_id, 0, 5);
			this.tableContent.Controls.Add(this.txtoriginal_network_id, 1, 2);
			this.tableContent.Controls.Add(this.txttransport_stream_id, 1, 3);
			this.tableContent.Controls.Add(this.txtservice_id, 1, 4);
			this.tableContent.Controls.Add(this.txtevent_id, 1, 5);
			this.tableContent.Controls.Add(this.cmbcell_linkage_infoid, 1, 0);
			base.error.SetError(this.tableContent, resources.GetString("tableContent.Error"));
			base.error.SetIconAlignment(this.tableContent, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("tableContent.IconAlignment"));
			base.error.SetIconPadding(this.tableContent, (int)resources.GetObject("tableContent.IconPadding"));
			this.tableContent.Name = "tableContent";
			resources.ApplyResources(this.lblcell_linkage_infoid, "lblcell_linkage_infoid");
			base.error.SetError(this.lblcell_linkage_infoid, resources.GetString("lblcell_linkage_infoid.Error"));
			base.error.SetIconAlignment(this.lblcell_linkage_infoid, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lblcell_linkage_infoid.IconAlignment"));
			base.error.SetIconPadding(this.lblcell_linkage_infoid, (int)resources.GetObject("lblcell_linkage_infoid.IconPadding"));
			this.lblcell_linkage_infoid.Name = "lblcell_linkage_infoid";
			resources.ApplyResources(this.lblbouquet_id, "lblbouquet_id");
			base.error.SetError(this.lblbouquet_id, resources.GetString("lblbouquet_id.Error"));
			base.error.SetIconAlignment(this.lblbouquet_id, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lblbouquet_id.IconAlignment"));
			base.error.SetIconPadding(this.lblbouquet_id, (int)resources.GetObject("lblbouquet_id.IconPadding"));
			this.lblbouquet_id.Name = "lblbouquet_id";
			resources.ApplyResources(this.txtbouquet_id, "txtbouquet_id");
			base.error.SetError(this.txtbouquet_id, resources.GetString("txtbouquet_id.Error"));
			this.txtbouquet_id.ForeColor = System.Drawing.Color.Black;
			base.error.SetIconAlignment(this.txtbouquet_id, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("txtbouquet_id.IconAlignment"));
			base.error.SetIconPadding(this.txtbouquet_id, (int)resources.GetObject("txtbouquet_id.IconPadding"));
			this.txtbouquet_id.IsValidated = true;
			this.txtbouquet_id.MaxValue = 65535;
			this.txtbouquet_id.Name = "txtbouquet_id";
			this.txtbouquet_id.NotNull = true;
			this.txtbouquet_id.ReadOnly = true;
			this.txtbouquet_id.SetMaxLength = 5;
			DeXin.EPG.Tools.EPGUserControl.NumTextBox numTextBox = this.txtbouquet_id;
			byte[] value = new byte[1];
			numTextBox.Value = value;
			resources.ApplyResources(this.lbloriginal_network_id, "lbloriginal_network_id");
			base.error.SetError(this.lbloriginal_network_id, resources.GetString("lbloriginal_network_id.Error"));
			base.error.SetIconAlignment(this.lbloriginal_network_id, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lbloriginal_network_id.IconAlignment"));
			base.error.SetIconPadding(this.lbloriginal_network_id, (int)resources.GetObject("lbloriginal_network_id.IconPadding"));
			this.lbloriginal_network_id.Name = "lbloriginal_network_id";
			resources.ApplyResources(this.lbltransport_stream_id, "lbltransport_stream_id");
			base.error.SetError(this.lbltransport_stream_id, resources.GetString("lbltransport_stream_id.Error"));
			base.error.SetIconAlignment(this.lbltransport_stream_id, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lbltransport_stream_id.IconAlignment"));
			base.error.SetIconPadding(this.lbltransport_stream_id, (int)resources.GetObject("lbltransport_stream_id.IconPadding"));
			this.lbltransport_stream_id.Name = "lbltransport_stream_id";
			resources.ApplyResources(this.lblservice_id, "lblservice_id");
			base.error.SetError(this.lblservice_id, resources.GetString("lblservice_id.Error"));
			base.error.SetIconAlignment(this.lblservice_id, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lblservice_id.IconAlignment"));
			base.error.SetIconPadding(this.lblservice_id, (int)resources.GetObject("lblservice_id.IconPadding"));
			this.lblservice_id.Name = "lblservice_id";
			resources.ApplyResources(this.lblevent_id, "lblevent_id");
			base.error.SetError(this.lblevent_id, resources.GetString("lblevent_id.Error"));
			base.error.SetIconAlignment(this.lblevent_id, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lblevent_id.IconAlignment"));
			base.error.SetIconPadding(this.lblevent_id, (int)resources.GetObject("lblevent_id.IconPadding"));
			this.lblevent_id.Name = "lblevent_id";
			resources.ApplyResources(this.txtoriginal_network_id, "txtoriginal_network_id");
			base.error.SetError(this.txtoriginal_network_id, resources.GetString("txtoriginal_network_id.Error"));
			this.txtoriginal_network_id.ForeColor = System.Drawing.Color.Black;
			base.error.SetIconAlignment(this.txtoriginal_network_id, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("txtoriginal_network_id.IconAlignment"));
			base.error.SetIconPadding(this.txtoriginal_network_id, (int)resources.GetObject("txtoriginal_network_id.IconPadding"));
			this.txtoriginal_network_id.IsValidated = true;
			this.txtoriginal_network_id.MaxValue = 65535;
			this.txtoriginal_network_id.Name = "txtoriginal_network_id";
			this.txtoriginal_network_id.NotNull = true;
			this.txtoriginal_network_id.ReadOnly = true;
			this.txtoriginal_network_id.SetMaxLength = 5;
			DeXin.EPG.Tools.EPGUserControl.NumTextBox numTextBox2 = this.txtoriginal_network_id;
			value = new byte[1];
			numTextBox2.Value = value;
			resources.ApplyResources(this.txttransport_stream_id, "txttransport_stream_id");
			base.error.SetError(this.txttransport_stream_id, resources.GetString("txttransport_stream_id.Error"));
			this.txttransport_stream_id.ForeColor = System.Drawing.Color.Black;
			base.error.SetIconAlignment(this.txttransport_stream_id, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("txttransport_stream_id.IconAlignment"));
			base.error.SetIconPadding(this.txttransport_stream_id, (int)resources.GetObject("txttransport_stream_id.IconPadding"));
			this.txttransport_stream_id.IsValidated = true;
			this.txttransport_stream_id.MaxValue = 65535;
			this.txttransport_stream_id.Name = "txttransport_stream_id";
			this.txttransport_stream_id.NotNull = true;
			this.txttransport_stream_id.ReadOnly = true;
			this.txttransport_stream_id.SetMaxLength = 5;
			DeXin.EPG.Tools.EPGUserControl.NumTextBox numTextBox3 = this.txttransport_stream_id;
			value = new byte[1];
			numTextBox3.Value = value;
			resources.ApplyResources(this.txtservice_id, "txtservice_id");
			base.error.SetError(this.txtservice_id, resources.GetString("txtservice_id.Error"));
			this.txtservice_id.ForeColor = System.Drawing.Color.Black;
			base.error.SetIconAlignment(this.txtservice_id, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("txtservice_id.IconAlignment"));
			base.error.SetIconPadding(this.txtservice_id, (int)resources.GetObject("txtservice_id.IconPadding"));
			this.txtservice_id.IsValidated = true;
			this.txtservice_id.MaxValue = 65535;
			this.txtservice_id.Name = "txtservice_id";
			this.txtservice_id.NotNull = true;
			this.txtservice_id.ReadOnly = true;
			this.txtservice_id.SetMaxLength = 5;
			DeXin.EPG.Tools.EPGUserControl.NumTextBox numTextBox4 = this.txtservice_id;
			value = new byte[1];
			numTextBox4.Value = value;
			resources.ApplyResources(this.txtevent_id, "txtevent_id");
			base.error.SetError(this.txtevent_id, resources.GetString("txtevent_id.Error"));
			this.txtevent_id.ForeColor = System.Drawing.Color.Black;
			base.error.SetIconAlignment(this.txtevent_id, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("txtevent_id.IconAlignment"));
			base.error.SetIconPadding(this.txtevent_id, (int)resources.GetObject("txtevent_id.IconPadding"));
			this.txtevent_id.IsValidated = true;
			this.txtevent_id.MaxValue = 65535;
			this.txtevent_id.Name = "txtevent_id";
			this.txtevent_id.NotNull = true;
			this.txtevent_id.ReadOnly = true;
			this.txtevent_id.SetMaxLength = 5;
			DeXin.EPG.Tools.EPGUserControl.NumTextBox numTextBox5 = this.txtevent_id;
			value = new byte[1];
			numTextBox5.Value = value;
			resources.ApplyResources(this.cmbcell_linkage_infoid, "cmbcell_linkage_infoid");
			this.cmbcell_linkage_infoid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			base.error.SetError(this.cmbcell_linkage_infoid, resources.GetString("cmbcell_linkage_infoid.Error"));
			this.cmbcell_linkage_infoid.FormattingEnabled = true;
			base.error.SetIconAlignment(this.cmbcell_linkage_infoid, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("cmbcell_linkage_infoid.IconAlignment"));
			base.error.SetIconPadding(this.cmbcell_linkage_infoid, (int)resources.GetObject("cmbcell_linkage_infoid.IconPadding"));
			this.cmbcell_linkage_infoid.MaxValue = 0L;
			this.cmbcell_linkage_infoid.MinValue = 0L;
			this.cmbcell_linkage_infoid.Name = "cmbcell_linkage_infoid";
			this.cmbcell_linkage_infoid.Value = "";
			this.cmbcell_linkage_infoid.SelectedIndexChanged += new System.EventHandler(cmbcell_linkage_infoid_SelectedIndexChanged);
			resources.ApplyResources(this.grbLogicalCellInfo, "grbLogicalCellInfo");
			this.grbLogicalCellInfo.Controls.Add(this.tableLayoutPanel2);
			base.error.SetError(this.grbLogicalCellInfo, resources.GetString("grbLogicalCellInfo.Error"));
			base.error.SetIconAlignment(this.grbLogicalCellInfo, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("grbLogicalCellInfo.IconAlignment"));
			base.error.SetIconPadding(this.grbLogicalCellInfo, (int)resources.GetObject("grbLogicalCellInfo.IconPadding"));
			this.grbLogicalCellInfo.Name = "grbLogicalCellInfo";
			this.grbLogicalCellInfo.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.cmblogical_cell_presentation_info, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.lbllogical_cell_id, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.txtlogical_cell_id, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.lbllogical_cell_presentation_info, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.lblelementary_cell_field, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.pnlElements, 1, 2);
			base.error.SetError(this.tableLayoutPanel2, resources.GetString("tableLayoutPanel2.Error"));
			base.error.SetIconAlignment(this.tableLayoutPanel2, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("tableLayoutPanel2.IconAlignment"));
			base.error.SetIconPadding(this.tableLayoutPanel2, (int)resources.GetObject("tableLayoutPanel2.IconPadding"));
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			resources.ApplyResources(this.cmblogical_cell_presentation_info, "cmblogical_cell_presentation_info");
			this.cmblogical_cell_presentation_info.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			base.error.SetError(this.cmblogical_cell_presentation_info, resources.GetString("cmblogical_cell_presentation_info.Error"));
			this.cmblogical_cell_presentation_info.FormattingEnabled = true;
			base.error.SetIconAlignment(this.cmblogical_cell_presentation_info, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("cmblogical_cell_presentation_info.IconAlignment"));
			base.error.SetIconPadding(this.cmblogical_cell_presentation_info, (int)resources.GetObject("cmblogical_cell_presentation_info.IconPadding"));
			this.cmblogical_cell_presentation_info.MaxValue = 0L;
			this.cmblogical_cell_presentation_info.MinValue = 0L;
			this.cmblogical_cell_presentation_info.Name = "cmblogical_cell_presentation_info";
			this.cmblogical_cell_presentation_info.Value = "";
			resources.ApplyResources(this.lbllogical_cell_id, "lbllogical_cell_id");
			base.error.SetError(this.lbllogical_cell_id, resources.GetString("lbllogical_cell_id.Error"));
			base.error.SetIconAlignment(this.lbllogical_cell_id, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lbllogical_cell_id.IconAlignment"));
			base.error.SetIconPadding(this.lbllogical_cell_id, (int)resources.GetObject("lbllogical_cell_id.IconPadding"));
			this.lbllogical_cell_id.Name = "lbllogical_cell_id";
			resources.ApplyResources(this.txtlogical_cell_id, "txtlogical_cell_id");
			base.error.SetError(this.txtlogical_cell_id, resources.GetString("txtlogical_cell_id.Error"));
			this.txtlogical_cell_id.ForeColor = System.Drawing.Color.Black;
			base.error.SetIconAlignment(this.txtlogical_cell_id, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("txtlogical_cell_id.IconAlignment"));
			base.error.SetIconPadding(this.txtlogical_cell_id, (int)resources.GetObject("txtlogical_cell_id.IconPadding"));
			this.txtlogical_cell_id.IsValidated = true;
			this.txtlogical_cell_id.MaxValue = 64;
			this.txtlogical_cell_id.Name = "txtlogical_cell_id";
			this.txtlogical_cell_id.NotNull = true;
			this.txtlogical_cell_id.SetMaxLength = 5;
			DeXin.EPG.Tools.EPGUserControl.NumTextBox numTextBox6 = this.txtlogical_cell_id;
			value = new byte[1];
			numTextBox6.Value = value;
			resources.ApplyResources(this.lbllogical_cell_presentation_info, "lbllogical_cell_presentation_info");
			base.error.SetError(this.lbllogical_cell_presentation_info, resources.GetString("lbllogical_cell_presentation_info.Error"));
			base.error.SetIconAlignment(this.lbllogical_cell_presentation_info, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lbllogical_cell_presentation_info.IconAlignment"));
			base.error.SetIconPadding(this.lbllogical_cell_presentation_info, (int)resources.GetObject("lbllogical_cell_presentation_info.IconPadding"));
			this.lbllogical_cell_presentation_info.Name = "lbllogical_cell_presentation_info";
			resources.ApplyResources(this.lblelementary_cell_field, "lblelementary_cell_field");
			base.error.SetError(this.lblelementary_cell_field, resources.GetString("lblelementary_cell_field.Error"));
			base.error.SetIconAlignment(this.lblelementary_cell_field, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lblelementary_cell_field.IconAlignment"));
			base.error.SetIconPadding(this.lblelementary_cell_field, (int)resources.GetObject("lblelementary_cell_field.IconPadding"));
			this.lblelementary_cell_field.Name = "lblelementary_cell_field";
			resources.ApplyResources(this.pnlElements, "pnlElements");
			this.pnlElements.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			base.error.SetError(this.pnlElements, resources.GetString("pnlElements.Error"));
			base.error.SetIconAlignment(this.pnlElements, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("pnlElements.IconAlignment"));
			base.error.SetIconPadding(this.pnlElements, (int)resources.GetObject("pnlElements.IconPadding"));
			this.pnlElements.Name = "pnlElements";
			resources.ApplyResources(this.grbcell_linkage_info, "grbcell_linkage_info");
			this.grbcell_linkage_info.Controls.Add(this.tableContent);
			base.error.SetError(this.grbcell_linkage_info, resources.GetString("grbcell_linkage_info.Error"));
			base.error.SetIconAlignment(this.grbcell_linkage_info, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("grbcell_linkage_info.IconAlignment"));
			base.error.SetIconPadding(this.grbcell_linkage_info, (int)resources.GetObject("grbcell_linkage_info.IconPadding"));
			this.grbcell_linkage_info.Name = "grbcell_linkage_info";
			this.grbcell_linkage_info.TabStop = false;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "Logical_cell_info";
			base.Load += new System.EventHandler(Get_cell_linkage_info_Load);
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableContent.ResumeLayout(false);
			this.tableContent.PerformLayout();
			this.grbLogicalCellInfo.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.grbcell_linkage_info.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
