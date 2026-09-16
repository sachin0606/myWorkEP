using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using Com.Cn.Tool.UserControls;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class Service_List_DescriptorDemo : DescriptorDemoBase
	{
		private ProgramInfoManager programManager = new ProgramInfoManager();

		private IList<Servicve_ID_Type> LST = new List<Servicve_ID_Type>();

		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblServiceId;

		private Label lblServiceType;

		private DescriptorCombobox cmbServiceType;

		private DescriptorCombobox cmbServiceID;

		private DGV DGV;

		private Button btnConfig;

		private ContextMenuStrip CMS;

		private ToolStripMenuItem cmsClear;

		private DataGridViewTextBoxColumn colProgramID;

		private DataGridViewTextBoxColumn colProgramName;

		private DataGridViewTextBoxColumn colServiceType;

		private DataGridViewTextBoxColumn colServiceTypeName;

		public Service_List_DescriptorDemo()
		{
			InitializeComponent();
		}

		public Service_List_DescriptorDemo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
			InitControl();
		}

		protected override void InitControl()
		{
			Tool.BindingComboBox(((IEnumerable<BaseKey>)programManager.GetByTSIdentity(base.ET.TableID)).ToList(), cmbServiceID);
			Tool.BindingComboBox(BaseData.Ins.ServiceTypes, cmbServiceType);
			Tool.SetDGVReadOnly((DataGridView)DGV, true, (string[])null);
		}

		protected override void BindingData()
		{
			if (base.PageFlag != PageEnum.Add && base.PageFlag != PageEnum.Empty && base.ET != null && base.ET.DesTypeTag == 65)
			{
				LST.Clear();
				ushort num = 0;
				byte b = 0;
				Servicve_ID_Type servicve_ID_Type = null;
				for (int i = 0; i < base.ET.DataLength; i += 3)
				{
					num = (ushort)Tool.ResolveInt32(Tool.ResolveByte(base.ET.DescriptionData, i, 2), NetWorkEnum.NetWorkToHost);
					b = Tool.ResolveByte(base.ET.DescriptionData, i + 2, 1)[0];
					servicve_ID_Type = new Servicve_ID_Type(0L, num, "", b, base.ET.TableID);
					LST.Add(servicve_ID_Type);
				}
				BindDGV();
			}
			if (base.PageFlag == PageEnum.Show)
			{
				cmbServiceID.Enabled = false;
				cmbServiceType.Enabled = false;
				btnConfig.Enabled = false;
				cmsClear.Enabled = false;
			}
		}

		public override bool IsValidated(ErrorProvider error)
		{
			msg = "";
			if (base.PageFlag == PageEnum.Add || base.PageFlag == PageEnum.Edit)
			{
				int startIndex = 0;
				base.ET.DataLength = LST.Count * 3;
				base.ET.DescriptionData = new byte[base.ET.DataLength];
				foreach (Servicve_ID_Type item in LST)
				{
					Tool.ToByteArray(item.ProgramID, base.ET.DescriptionData, ref startIndex);
					Tool.ToByteArray(item.ServiceTypeID, base.ET.DescriptionData, ref startIndex);
				}
				return true;
			}
			return true;
		}

		private void BindDGV()
		{
			DGV.Rows.Clear();
			int num = 0;
			Servicve_ID_Type item;
			foreach (Servicve_ID_Type item2 in LST)
			{
				item = item2;
				num = DGV.Rows.Add(new DataGridViewRow());
				DGV.Rows[num].Tag = item;
				DGV.Rows[num].Cells[colProgramID.Name].Value = item.ProgramID;
				DGV.Rows[num].Cells[colProgramName.Name].Value = item.ProgramName;
				BaseKey baseKey = BaseData.Ins.ServiceTypes.FirstOrDefault((BaseKey p) => p.BKKey == item.ServiceTypeID.ToString(FormatString.Ins.X2));
				if (baseKey != null)
				{
					DGV.Rows[num].Cells[colServiceType.Name].Value = baseKey.BKKey;
					DGV.Rows[num].Cells[colServiceTypeName.Name].Value = baseKey.BKValue;
				}
				else
				{
					DGV.Rows[num].Cells[colServiceType.Name].Value = item.ServiceTypeID.ToString(FormatString.Ins.X2);
					DGV.Rows[num].Cells[colServiceTypeName.Name].Value = item.ServiceTypeID.ToString(FormatString.Ins.X2);
				}
			}
		}

		private void btnConfig_Click(object sender, EventArgs e)
		{
			ProgramInfo program = cmbServiceID.SelectedItem as ProgramInfo;
			if (program != null)
			{
				byte b = Tool.StringToBytes(cmbServiceType.Value)[0];
				Servicve_ID_Type servicve_ID_Type = LST.FirstOrDefault((Servicve_ID_Type p) => p.ProgramID == (short)program.ServiceId);
				if (servicve_ID_Type == null)
				{
					servicve_ID_Type = new Servicve_ID_Type(program.Identity, (ushort)program.ServiceId, program.ProgramName, b, base.ET.TableID);
					LST.Add(servicve_ID_Type);
				}
				else
				{
					servicve_ID_Type.ProgramName = program.ProgramName;
					servicve_ID_Type.ServiceTypeID = b;
				}
				BindDGV();
			}
		}

		private void DGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				DataGridViewRow dataGridViewRow = DGV.Rows[e.RowIndex];
				Servicve_ID_Type servicve_ID_Type = dataGridViewRow.Tag as Servicve_ID_Type;
				if (servicve_ID_Type != null)
				{
					cmbServiceID.SelectedValue = servicve_ID_Type.IDendity.ToString();
					cmbServiceType.Value = servicve_ID_Type.ServiceTypeID.ToString(FormatString.Ins.X2);
				}
			}
		}

		private void cmsClear_Click(object sender, EventArgs e)
		{
			DataGridViewRow currentRow = DGV.CurrentRow;
			if (currentRow == null)
			{
				return;
			}
			Servicve_ID_Type item = currentRow.Tag as Servicve_ID_Type;
			if (item != null)
			{
				item = LST.FirstOrDefault((Servicve_ID_Type p) => p.ProgramID == item.ProgramID);
				if (item != null)
				{
					LST.Remove(item);
				}
				BindDGV();
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.Service_List_DescriptorDemo));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblServiceId = new System.Windows.Forms.Label();
			this.lblServiceType = new System.Windows.Forms.Label();
			this.cmbServiceType = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.cmbServiceID = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.btnConfig = new System.Windows.Forms.Button();
			this.DGV = new Com.Cn.Tool.UserControls.DGV();
			this.CMS = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmsClear = new System.Windows.Forms.ToolStripMenuItem();
			this.colProgramID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colProgramName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colServiceType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colServiceTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			base.grbContext.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.DGV).BeginInit();
			this.CMS.SuspendLayout();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.DGV);
			base.grbContext.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbContext, "grbContext");
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblServiceId, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblServiceType, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.cmbServiceType, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.cmbServiceID, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnConfig, 2, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.lblServiceId, "lblServiceId");
			this.lblServiceId.Name = "lblServiceId";
			resources.ApplyResources(this.lblServiceType, "lblServiceType");
			this.lblServiceType.Name = "lblServiceType";
			resources.ApplyResources(this.cmbServiceType, "cmbServiceType");
			this.cmbServiceType.FormattingEnabled = true;
			this.cmbServiceType.MaxValue = 254L;
			this.cmbServiceType.MinValue = 128L;
			this.cmbServiceType.Name = "cmbServiceType";
			this.cmbServiceType.Value = "";
			resources.ApplyResources(this.cmbServiceID, "cmbServiceID");
			this.cmbServiceID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbServiceID.FormattingEnabled = true;
			this.cmbServiceID.MaxValue = 0L;
			this.cmbServiceID.MinValue = 0L;
			this.cmbServiceID.Name = "cmbServiceID";
			this.cmbServiceID.Value = "";
			resources.ApplyResources(this.btnConfig, "btnConfig");
			this.btnConfig.Name = "btnConfig";
			this.btnConfig.UseVisualStyleBackColor = true;
			this.btnConfig.Click += new System.EventHandler(btnConfig_Click);
			this.DGV.AllowUserToAddRows = false;
			this.DGV.AllowUserToDeleteRows = false;
			this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DGV.Columns.AddRange(this.colProgramID, this.colProgramName, this.colServiceType, this.colServiceTypeName);
			this.DGV.ContextMenuStrip = this.CMS;
			resources.ApplyResources(this.DGV, "DGV");
			this.DGV.Name = "DGV";
			this.DGV.ReadOnly = true;
			this.DGV.RowTemplate.Height = 23;
			this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.DGV.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(DGV_CellMouseClick);
			this.CMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.cmsClear });
			this.CMS.Name = "CMS";
			resources.ApplyResources(this.CMS, "CMS");
			this.cmsClear.Name = "cmsClear";
			resources.ApplyResources(this.cmsClear, "cmsClear");
			this.cmsClear.Click += new System.EventHandler(cmsClear_Click);
			resources.ApplyResources(this.colProgramID, "colProgramID");
			this.colProgramID.Name = "colProgramID";
			this.colProgramID.ReadOnly = true;
			resources.ApplyResources(this.colProgramName, "colProgramName");
			this.colProgramName.Name = "colProgramName";
			this.colProgramName.ReadOnly = true;
			resources.ApplyResources(this.colServiceType, "colServiceType");
			this.colServiceType.Name = "colServiceType";
			this.colServiceType.ReadOnly = true;
			this.colServiceType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			resources.ApplyResources(this.colServiceTypeName, "colServiceTypeName");
			this.colServiceTypeName.Name = "colServiceTypeName";
			this.colServiceTypeName.ReadOnly = true;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "Service_List_DescriptorDemo";
			base.Controls.SetChildIndex(base.grbContext, 0);
			base.grbContext.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.DGV).EndInit();
			this.CMS.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
