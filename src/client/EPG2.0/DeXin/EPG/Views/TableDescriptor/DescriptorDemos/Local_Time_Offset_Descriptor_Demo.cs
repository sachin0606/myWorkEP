using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using Com.Cn.Tool.UserControls;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class Local_Time_Offset_Descriptor_Demo : DescriptorDemoBase
	{
		private IContainer components = null;

		private ContextMenuStrip cmsDGV;

		private ToolStripMenuItem cmsDelete;

		private DGV DGV;

		private GroupBox groupBox1;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblcontroy_code;

		private Label lbllocal_time_offset_polarity;

		private Label lblcountry_region_id;

		private Panel panel1;

		private RadioButton radWest;

		private RadioButton radEast;

		private Label lbllocal_time_offset;

		private Label lbltime_of_change;

		private Button btnAdd;

		private Label lblnext_time_offset;

		private Button btnRemove;

		private DescriptorTextBox txtcountry_region_id;

		private DescriptorTextBox txtlocal_time_offset;

		private DescriptorTextBox txtnext_time_offset;

		private DateTimeLong dttime_of_change;

		private DataGridViewTextBoxColumn colCountry_Code;

		private DataGridViewTextBoxColumn colCountry_Region_Id;

		private DataGridViewTextBoxColumn colLocal_Time_Offset_Polarity;

		private DataGridViewTextBoxColumn colLocal_Time_Offset;

		private DataGridViewTextBoxColumn colTime_Of_Change;

		private DataGridViewTextBoxColumn colNext_Time_Offset;

		private ComboBox cmbcountry_code;

		private IList<LocalTimeOffSetEntity> countries = new List<LocalTimeOffSetEntity>();

		private ErrorProvider error = new ErrorProvider();

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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.Local_Time_Offset_Descriptor_Demo));
			this.cmsDGV = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmsDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblcontroy_code = new System.Windows.Forms.Label();
			this.lbllocal_time_offset_polarity = new System.Windows.Forms.Label();
			this.lblcountry_region_id = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.radWest = new System.Windows.Forms.RadioButton();
			this.radEast = new System.Windows.Forms.RadioButton();
			this.lbllocal_time_offset = new System.Windows.Forms.Label();
			this.lbltime_of_change = new System.Windows.Forms.Label();
			this.lblnext_time_offset = new System.Windows.Forms.Label();
			this.txtcountry_region_id = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.txtlocal_time_offset = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.txtnext_time_offset = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.dttime_of_change = new Com.Cn.Tool.UserControls.DateTimeLong();
			this.cmbcountry_code = new System.Windows.Forms.ComboBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.DGV = new Com.Cn.Tool.UserControls.DGV();
			this.colCountry_Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colCountry_Region_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colLocal_Time_Offset_Polarity = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colLocal_Time_Offset = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colTime_Of_Change = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colNext_Time_Offset = new System.Windows.Forms.DataGridViewTextBoxColumn();
			base.grbContext.SuspendLayout();
			this.cmsDGV.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.DGV).BeginInit();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.DGV);
			base.grbContext.Controls.Add(this.groupBox1);
			resources.ApplyResources(base.grbContext, "grbContext");
			this.cmsDGV.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.cmsDelete });
			this.cmsDGV.Name = "cmsDGV";
			resources.ApplyResources(this.cmsDGV, "cmsDGV");
			this.cmsDelete.Name = "cmsDelete";
			resources.ApplyResources(this.cmsDelete, "cmsDelete");
			this.groupBox1.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblcontroy_code, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbllocal_time_offset_polarity, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblcountry_region_id, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.lbllocal_time_offset, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lbltime_of_change, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblnext_time_offset, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.txtcountry_region_id, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtlocal_time_offset, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtnext_time_offset, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.dttime_of_change, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.cmbcountry_code, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnAdd, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.btnRemove, 1, 6);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.lblcontroy_code, "lblcontroy_code");
			this.lblcontroy_code.Name = "lblcontroy_code";
			resources.ApplyResources(this.lbllocal_time_offset_polarity, "lbllocal_time_offset_polarity");
			this.lbllocal_time_offset_polarity.Name = "lbllocal_time_offset_polarity";
			resources.ApplyResources(this.lblcountry_region_id, "lblcountry_region_id");
			this.lblcountry_region_id.Name = "lblcountry_region_id";
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Controls.Add(this.radWest);
			this.panel1.Controls.Add(this.radEast);
			this.panel1.Name = "panel1";
			resources.ApplyResources(this.radWest, "radWest");
			this.radWest.Name = "radWest";
			this.radWest.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.radEast, "radEast");
			this.radEast.Checked = true;
			this.radEast.Name = "radEast";
			this.radEast.TabStop = true;
			this.radEast.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.lbllocal_time_offset, "lbllocal_time_offset");
			this.lbllocal_time_offset.Name = "lbllocal_time_offset";
			resources.ApplyResources(this.lbltime_of_change, "lbltime_of_change");
			this.lbltime_of_change.Name = "lbltime_of_change";
			resources.ApplyResources(this.lblnext_time_offset, "lblnext_time_offset");
			this.lblnext_time_offset.Name = "lblnext_time_offset";
			resources.ApplyResources(this.txtcountry_region_id, "txtcountry_region_id");
			this.txtcountry_region_id.MaxValue = 60L;
			this.txtcountry_region_id.MinValue = 0L;
			this.txtcountry_region_id.Name = "txtcountry_region_id";
			this.txtcountry_region_id.Value = new byte[0];
			resources.ApplyResources(this.txtlocal_time_offset, "txtlocal_time_offset");
			this.txtlocal_time_offset.MaxValue = 4608L;
			this.txtlocal_time_offset.MinValue = 0L;
			this.txtlocal_time_offset.Name = "txtlocal_time_offset";
			this.txtlocal_time_offset.Value = new byte[0];
			resources.ApplyResources(this.txtnext_time_offset, "txtnext_time_offset");
			this.txtnext_time_offset.MaxValue = 4608L;
			this.txtnext_time_offset.MinValue = 0L;
			this.txtnext_time_offset.Name = "txtnext_time_offset";
			this.txtnext_time_offset.Value = new byte[0];
			this.dttime_of_change.AllowDrop = true;
			resources.ApplyResources(this.dttime_of_change, "dttime_of_change");
			this.dttime_of_change.Name = "dttime_of_change";
			this.dttime_of_change.ReadOnly = false;
			this.cmbcountry_code.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbcountry_code.FormattingEnabled = true;
			resources.ApplyResources(this.cmbcountry_code, "cmbcountry_code");
			this.cmbcountry_code.Name = "cmbcountry_code";
			resources.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
			resources.ApplyResources(this.btnRemove, "btnRemove");
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(btnRemove_Click);
			this.DGV.AllowUserToAddRows = false;
			this.DGV.AllowUserToDeleteRows = false;
			this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DGV.Columns.AddRange(this.colCountry_Code, this.colCountry_Region_Id, this.colLocal_Time_Offset_Polarity, this.colLocal_Time_Offset, this.colTime_Of_Change, this.colNext_Time_Offset);
			resources.ApplyResources(this.DGV, "DGV");
			this.DGV.Name = "DGV";
			this.DGV.ReadOnly = true;
			this.DGV.RowTemplate.Height = 23;
			this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			resources.ApplyResources(this.colCountry_Code, "colCountry_Code");
			this.colCountry_Code.Name = "colCountry_Code";
			this.colCountry_Code.ReadOnly = true;
			resources.ApplyResources(this.colCountry_Region_Id, "colCountry_Region_Id");
			this.colCountry_Region_Id.Name = "colCountry_Region_Id";
			this.colCountry_Region_Id.ReadOnly = true;
			resources.ApplyResources(this.colLocal_Time_Offset_Polarity, "colLocal_Time_Offset_Polarity");
			this.colLocal_Time_Offset_Polarity.Name = "colLocal_Time_Offset_Polarity";
			this.colLocal_Time_Offset_Polarity.ReadOnly = true;
			resources.ApplyResources(this.colLocal_Time_Offset, "colLocal_Time_Offset");
			this.colLocal_Time_Offset.Name = "colLocal_Time_Offset";
			this.colLocal_Time_Offset.ReadOnly = true;
			resources.ApplyResources(this.colTime_Of_Change, "colTime_Of_Change");
			this.colTime_Of_Change.Name = "colTime_Of_Change";
			this.colTime_Of_Change.ReadOnly = true;
			resources.ApplyResources(this.colNext_Time_Offset, "colNext_Time_Offset");
			this.colNext_Time_Offset.Name = "colNext_Time_Offset";
			this.colNext_Time_Offset.ReadOnly = true;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "Local_Time_Offset_Descriptor_Demo";
			base.Controls.SetChildIndex(base.grbContext, 0);
			base.grbContext.ResumeLayout(false);
			this.cmsDGV.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.DGV).EndInit();
			base.ResumeLayout(false);
		}

		public Local_Time_Offset_Descriptor_Demo()
		{
			InitializeComponent();
		}

		public Local_Time_Offset_Descriptor_Demo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
			InitControl();
		}

		protected override void BindingData()
		{
			base.PageFlag = PageEnum.Add;
			if (base.ET.DataLength > 0)
			{
				DataConvert(base.ET.DescriptionData);
			}
			BindDGV();
		}

		public override bool IsValidated(ErrorProvider error)
		{
			ValidateChildren(ValidationConstraints.TabStop);
			int startIndex = 0;
			base.ET.DataLength = countries.Count * 13;
			base.ET.DescriptionData = new byte[base.ET.DataLength];
			foreach (LocalTimeOffSetEntity country in countries)
			{
				Tool.ToByteArray(country.ToByte(), base.ET.DescriptionData, ref startIndex);
			}
			return true;
		}

		private void DataConvert(byte[] data)
		{
			countries.Clear();
			for (int i = 0; i < data.Length; i += 13)
			{
				byte[] data2 = Tool.ResolveByte(data, i, 13);
				LocalTimeOffSetEntity item = LocalTimeOffSetEntity.InitData(data2);
				countries.Add(item);
			}
		}

		private void BindDGV()
		{
			DGV.Rows.Clear();
			int num = 0;
			foreach (LocalTimeOffSetEntity country in countries)
			{
				num = DGV.Rows.Add(new DataGridViewRow());
				DGV.Rows[num].Tag = country;
				DGV.Rows[num].Cells[colCountry_Code.Name].Value = country.CountryCode;
				DGV.Rows[num].Cells[colCountry_Region_Id.Name].Value = country.CountryRegionID;
				DGV.Rows[num].Cells[colLocal_Time_Offset_Polarity.Name].Value = country.LocalTimeOffsetPolarity;
				DGV.Rows[num].Cells[colLocal_Time_Offset.Name].Value = country.LocalTimeOffset;
				DGV.Rows[num].Cells[colTime_Of_Change.Name].Value = country.TimeOfChange.ToString();
				DGV.Rows[num].Cells[colNext_Time_Offset.Name].Value = country.NextTimeOffset;
			}
		}

		protected override void InitControl()
		{
			Tool.SetTextBoxInputNumber(txtcountry_region_id);
			Tool.SetTextBoxInputNumber(txtlocal_time_offset);
			Tool.SetTextBoxInputNumber(txtnext_time_offset);
			IList<BaseKey> countryCode = base.pageManger.GetCountryCode();
			Tool.BindingComboBox(countryCode, cmbcountry_code);
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			if (countries.Count >= 19)
			{
				MessageBox.Show(ResourceManagement.GetLocationString("DescriptorExceedsMaxLength"));
			}
			else if (txtcountry_region_id.IsValitdated(error) && txtlocal_time_offset.IsValitdated(error) && txtnext_time_offset.IsValitdated(error))
			{
				LocalTimeOffSetEntity localTimeOffSetEntity = new LocalTimeOffSetEntity();
				localTimeOffSetEntity.CountryCode = Tool.GetValue(cmbcountry_code);
				localTimeOffSetEntity.CountryRegionID = Tool.GetInt32(txtcountry_region_id.Text);
				localTimeOffSetEntity.LocalTimeOffsetPolarity = ((!radEast.Checked) ? 1 : 0);
				localTimeOffSetEntity.LocalTimeOffset = Tool.GetValue(txtlocal_time_offset);
				localTimeOffSetEntity.TimeOfChange = dttime_of_change.DT;
				localTimeOffSetEntity.NextTimeOffset = Tool.GetValue(txtnext_time_offset);
				countries.Add(localTimeOffSetEntity);
				BindDGV();
			}
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			if (DGV.CurrentRow != null)
			{
				LocalTimeOffSetEntity localTimeOffSetEntity = DGV.CurrentRow.Tag as LocalTimeOffSetEntity;
				if (localTimeOffSetEntity != null)
				{
					countries.Remove(localTimeOffSetEntity);
					BindDGV();
				}
			}
		}
	}
}
