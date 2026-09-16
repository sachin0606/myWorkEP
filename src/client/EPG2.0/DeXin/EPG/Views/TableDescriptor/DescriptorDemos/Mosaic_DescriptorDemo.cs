using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;
using DeXin.EPG.Views.TableDescriptor.DescriptorDemos.ExtendForms;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class Mosaic_DescriptorDemo : DescriptorDemoBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableContent;

		private Label lblhorizontal;

		private DescriptorCombobox cmbhorizontal;

		private DescriptorCombobox cmbvertical;

		private Label lblvertical;

		private Label lblmep;

		private Button btnAddcell;

		private DataGridView dgv;

		private DataGridViewTextBoxColumn colllogical_cell_id;

		private DataGridViewTextBoxColumn collogical_cell_presentation_info;

		private DataGridViewLinkColumn colData;

		private NumTextBox txtmosaic_entry_point;

		public Mosaic_DescriptorDemo()
		{
			InitializeComponent();
		}

		public Mosaic_DescriptorDemo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
			InitControl();
		}

		protected override void InitControl()
		{
			Tool.BindingComboBox(BaseData.Ins.NumberOfElementaryEClls, cmbhorizontal);
			Tool.BindingComboBox(BaseData.Ins.NumberOfElementaryEClls, cmbvertical);
		}

		protected override void BindingData()
		{
			if (base.PageFlag != PageEnum.Add && base.PageFlag != PageEnum.Empty)
			{
				int start = 0;
				byte[] array = Tool.ResolveByte(base.ET.DescriptionData, 1, ref start);
				txtmosaic_entry_point.Text = (array[0] >> 7).ToString(FormatString.Ins.X);
				cmbhorizontal.Value = ((array[0] >> 4) & 7).ToString(FormatString.Ins.X2);
				cmbvertical.Value = (array[0] & 7).ToString(FormatString.Ins.X2);
				while (start < base.ET.DataLength)
				{
					BindDataToRow(dgv.Rows.Add(new DataGridViewRow()), AnalyticalData(base.ET.DescriptionData, ref start));
				}
			}
		}

		private byte[] AnalyticalData(byte[] resource, ref int ref_index)
		{
			int start = ref_index;
			byte[] array = new byte[255];
			int num = 0;
			byte[] array2 = Tool.ResolveByte(resource, 2, ref start);
			num += 2;
			array2 = Tool.ResolveByte(resource, 1, ref start);
			int num2 = array2[0];
			num++;
			array2 = Tool.ResolveByte(resource, num2, ref start);
			num += num2;
			byte b = resource[start++];
			num++;
			if (b == 1)
			{
				num += 2;
			}
			else if (b == 2 || b == 3)
			{
				num += 6;
			}
			else if (b == 4)
			{
				num += 8;
			}
			return Tool.ResolveByte(resource, num, ref ref_index);
		}

		public override bool IsValidated(ErrorProvider error)
		{
			if (!txtmosaic_entry_point.IsValidated)
			{
				return false;
			}
			int startIndex = 0;
			byte[] array = new byte[255];
			byte b = 0;
			b = (byte)((txtmosaic_entry_point.Value[0] & 1) << 7);
			b |= (byte)((Tool.StringToBytes(cmbhorizontal.Value)[0] & 7) << 4);
			b |= 8;
			b |= (byte)(Tool.StringToBytes(cmbvertical.Value)[0] & 7);
			Tool.ToByteArray(b, array, ref startIndex);
			foreach (DataGridViewRow item in (IEnumerable)dgv.Rows)
			{
				byte[] resource = (byte[])item.Tag;
				Tool.ToByteArray(resource, array, ref startIndex);
			}
			base.ET.DataLength = startIndex;
			base.ET.DescriptionData = new byte[startIndex];
			startIndex = 0;
			for (int i = 0; i < base.ET.DataLength; i++)
			{
				Tool.ToByteArray(array[i], base.ET.DescriptionData, ref startIndex);
			}
			return true;
		}

		private void btnAddcell_Click(object sender, EventArgs e)
		{
			int horizontal_length = Tool.ToNumber32(cmbhorizontal.Value);
			int vertical_length = Tool.ToNumber32(cmbvertical.Value);
			Logical_cell_info logical_cell_info = new Logical_cell_info(horizontal_length, vertical_length, new byte[4] { 3, 248, 0, 0 });
			if (logical_cell_info.ShowDialog() == DialogResult.OK)
			{
				int index = dgv.Rows.Add(new DataGridViewRow());
				byte[] temp = logical_cell_info.ToBytes();
				BindDataToRow(index, temp);
			}
		}

		private void BindDataToRow(int index, byte[] temp)
		{
			dgv.Rows[index].Tag = temp;
			dgv.Rows[index].Cells[colllogical_cell_id.Name].Value = temp[0] >> 2;
			dgv.Rows[index].Cells[collogical_cell_presentation_info.Name].Value = temp[1] & 7;
			dgv.Rows[index].Cells[colData.Name].Value = "DATA";
		}

		private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == 2)
			{
				DataGridViewRow dataGridViewRow = dgv.Rows[e.RowIndex];
				int horizontal_length = Tool.ToNumber32(cmbhorizontal.Value);
				int vertical_length = Tool.ToNumber32(cmbvertical.Value);
				Logical_cell_info logical_cell_info = new Logical_cell_info(horizontal_length, vertical_length, (byte[])dataGridViewRow.Tag, PageEnum.Edit);
				if (logical_cell_info.ShowDialog() == DialogResult.OK)
				{
					byte[] temp = logical_cell_info.ToBytes();
					BindDataToRow(e.RowIndex, temp);
				}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.Mosaic_DescriptorDemo));
			this.tableContent = new System.Windows.Forms.TableLayoutPanel();
			this.lblmep = new System.Windows.Forms.Label();
			this.lblhorizontal = new System.Windows.Forms.Label();
			this.cmbhorizontal = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.lblvertical = new System.Windows.Forms.Label();
			this.cmbvertical = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.btnAddcell = new System.Windows.Forms.Button();
			this.dgv = new System.Windows.Forms.DataGridView();
			this.colllogical_cell_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.collogical_cell_presentation_info = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colData = new System.Windows.Forms.DataGridViewLinkColumn();
			this.txtmosaic_entry_point = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			base.grbContext.SuspendLayout();
			this.tableContent.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgv).BeginInit();
			base.SuspendLayout();
			resources.ApplyResources(base.grbContext, "grbContext");
			base.grbContext.Controls.Add(this.tableContent);
			resources.ApplyResources(this.tableContent, "tableContent");
			this.tableContent.Controls.Add(this.lblmep, 0, 0);
			this.tableContent.Controls.Add(this.lblhorizontal, 0, 1);
			this.tableContent.Controls.Add(this.cmbhorizontal, 1, 1);
			this.tableContent.Controls.Add(this.lblvertical, 0, 2);
			this.tableContent.Controls.Add(this.cmbvertical, 1, 2);
			this.tableContent.Controls.Add(this.btnAddcell, 0, 4);
			this.tableContent.Controls.Add(this.dgv, 0, 5);
			this.tableContent.Controls.Add(this.txtmosaic_entry_point, 1, 0);
			this.tableContent.Name = "tableContent";
			resources.ApplyResources(this.lblmep, "lblmep");
			this.lblmep.Name = "lblmep";
			resources.ApplyResources(this.lblhorizontal, "lblhorizontal");
			this.lblhorizontal.Name = "lblhorizontal";
			resources.ApplyResources(this.cmbhorizontal, "cmbhorizontal");
			this.cmbhorizontal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbhorizontal.FormattingEnabled = true;
			this.cmbhorizontal.MaxValue = 0L;
			this.cmbhorizontal.MinValue = 0L;
			this.cmbhorizontal.Name = "cmbhorizontal";
			this.cmbhorizontal.Value = "";
			resources.ApplyResources(this.lblvertical, "lblvertical");
			this.lblvertical.Name = "lblvertical";
			resources.ApplyResources(this.cmbvertical, "cmbvertical");
			this.cmbvertical.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbvertical.FormattingEnabled = true;
			this.cmbvertical.MaxValue = 0L;
			this.cmbvertical.MinValue = 0L;
			this.cmbvertical.Name = "cmbvertical";
			this.cmbvertical.Value = "";
			resources.ApplyResources(this.btnAddcell, "btnAddcell");
			this.btnAddcell.Name = "btnAddcell";
			this.btnAddcell.UseVisualStyleBackColor = true;
			this.btnAddcell.Click += new System.EventHandler(btnAddcell_Click);
			resources.ApplyResources(this.dgv, "dgv");
			this.dgv.AllowUserToAddRows = false;
			this.dgv.AllowUserToDeleteRows = false;
			this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv.Columns.AddRange(this.colllogical_cell_id, this.collogical_cell_presentation_info, this.colData);
			this.tableContent.SetColumnSpan(this.dgv, 3);
			this.dgv.Name = "dgv";
			this.dgv.ReadOnly = true;
			this.dgv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgv_CellContentClick);
			resources.ApplyResources(this.colllogical_cell_id, "colllogical_cell_id");
			this.colllogical_cell_id.Name = "colllogical_cell_id";
			this.colllogical_cell_id.ReadOnly = true;
			resources.ApplyResources(this.collogical_cell_presentation_info, "collogical_cell_presentation_info");
			this.collogical_cell_presentation_info.Name = "collogical_cell_presentation_info";
			this.collogical_cell_presentation_info.ReadOnly = true;
			resources.ApplyResources(this.colData, "colData");
			this.colData.Name = "colData";
			this.colData.ReadOnly = true;
			this.colData.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colData.Text = "Data";
			resources.ApplyResources(this.txtmosaic_entry_point, "txtmosaic_entry_point");
			this.txtmosaic_entry_point.ForeColor = System.Drawing.Color.Black;
			this.txtmosaic_entry_point.IsValidated = true;
			this.txtmosaic_entry_point.MaxValue = 1;
			this.txtmosaic_entry_point.Name = "txtmosaic_entry_point";
			this.txtmosaic_entry_point.NotNull = true;
			this.txtmosaic_entry_point.SetMaxLength = 1;
			DeXin.EPG.Tools.EPGUserControl.NumTextBox numTextBox = this.txtmosaic_entry_point;
			byte[] value = new byte[1];
			numTextBox.Value = value;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "Mosaic_DescriptorDemo";
			base.grbContext.ResumeLayout(false);
			this.tableContent.ResumeLayout(false);
			this.tableContent.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.dgv).EndInit();
			base.ResumeLayout(false);
		}
	}
}
