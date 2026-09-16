using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.TableDescriptor.DescriptorDemos.ExtendForms;
using Util.Common.ExtensionHelper;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class Logical_Channel_Descriptor_Demo : DescriptorDemoBase
	{
		private IList<ProgramInfo> programs = new List<ProgramInfo>();

		private IList<LCNEntity> lcns = new List<LCNEntity>();

		private IContainer components = null;

		private DataGridView DGV;

		private DataGridViewTextBoxColumn colServiceID;

		private DataGridViewTextBoxColumn colName;

		private DataGridViewCheckBoxColumn colVisible;

		private DataGridViewTextBoxColumn colLcnNumber;

		private DataGridViewLinkColumn colEdit;

		private GroupBox grbLcnLength;

		private RadioButton rab14;

		private RadioButton rab10;

		private Panel panel1;

		private LcnType _LcnType
		{
			get
			{
				return (!rab10.Checked) ? LcnType.BIT_14 : LcnType.BIT_10;
			}
		}

		public Logical_Channel_Descriptor_Demo()
		{
			InitializeComponent();
		}

		public Logical_Channel_Descriptor_Demo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
			InitControl();
		}

		protected override void InitControl()
		{
			programs = new ProgramInfoManager().GetByTSIdentity(base.ET.TableID);
			DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			if (BaseData.Ins.lcntype == LcnType.BIT_14)
			{
				rab14.Checked = true;
			}
		}

		protected override void BindingData()
		{
			if (base.PageFlag == PageEnum.Add)
			{
				BindDGV();
			}
			else
			{
				if (base.ET == null || base.ET.DesTypeTag != 131)
				{
					return;
				}
				lcns.Clear();
				short serID = 0;
				bool flag = false;
				short num = 0;
				ProgramInfo programInfo = null;
				byte b = 0;
				byte b2 = 0;
				LCNEntity lCNEntity = null;
				int num2 = 0;
				while (num2 < base.ET.DataLength)
				{
					serID = (short)base.ET.DescriptionData.GetUInt16(num2);
					b = Tool.ResolveByte(base.ET.DescriptionData, num2 + 2, 1)[0];
					b2 = Tool.ResolveByte(base.ET.DescriptionData, num2 + 3, 1)[0];
					flag = ((b >> 7) & 1) == 1;
					num = ((_LcnType != LcnType.BIT_10) ? ((short)(((b & 0x3F) << 8) + b2)) : ((short)(((b & 3) << 8) + b2)));
					programInfo = programs.FirstOrDefault((ProgramInfo p) => p.ServiceId == (ushort)serID);
					num2 += 4;
					if (programInfo != null)
					{
						lCNEntity = new LCNEntity(serID, flag, num, programInfo.ProgramName);
						lcns.Add(lCNEntity);
					}
				}
				BindDGV();
			}
		}

		public override bool IsValidated(ErrorProvider error)
		{
			msg = "";
			if (base.PageFlag == PageEnum.Add || base.PageFlag == PageEnum.Edit)
			{
				int index = 0;
				base.ET.DataLength = DGV.Rows.Count * 4;
				base.ET.DescriptionData = new byte[base.ET.DataLength];
				foreach (DataGridViewRow item in (IEnumerable)DGV.Rows)
				{
					ushort resource = item.Cells[colServiceID.Name].Value.ToUInt16();
					short num = Tool.ToNumber16(Tool.GetString(item.Cells[colLcnNumber.Name].Value));
					base.ET.DescriptionData.Copy(resource, ref index);
					byte resource2 = 0;
					byte b = (byte)(Tool.GetBoolean(item.Cells[colVisible.Name].Value) ? 1 : 0);
					if (_LcnType == LcnType.BIT_10)
					{
						resource2 = (byte)(b << 7);
						resource2 |= 0x7C;
						resource2 |= (byte)((num >> 8) & 3);
					}
					else if (_LcnType == LcnType.BIT_14)
					{
						resource2 = (byte)((b << 7) | 0x40 | ((byte)(num >> 8) & 0x3F));
					}
					base.ET.DescriptionData.Copy(resource2, ref index);
					base.ET.DescriptionData.Copy((byte)(num & 0xFF), ref index);
				}
				return true;
			}
			return true;
		}

		private void BindDGV()
		{
			DGV.Rows.Clear();
			int num = 0;
			LCNEntity lCNEntity = null;
			ProgramInfo item;
			foreach (ProgramInfo program in programs)
			{
				item = program;
				num = DGV.Rows.Add(new DataGridViewRow());
				DGV.Rows[num].Tag = item;
				lCNEntity = lcns.FirstOrDefault((LCNEntity p) => (ushort)p.ServiceID == item.ServiceId);
				DGV.Rows[num].Cells[colServiceID.Name].Value = item.ServiceId;
				DGV.Rows[num].Cells[colName.Name].Value = item.ProgramName;
				if (lCNEntity != null)
				{
					DGV.Rows[num].Cells[colVisible.Name].Value = lCNEntity.Visible_service_flag;
					DGV.Rows[num].Cells[colLcnNumber.Name].Value = lCNEntity.LCNumber.ToString("X4");
				}
				if (base.PageFlag == PageEnum.Add || base.PageFlag == PageEnum.Edit)
				{
					DGV.Rows[num].Cells[colEdit.Name].Value = "EDIT LCN";
				}
			}
		}

		private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (base.PageFlag == PageEnum.Empty || base.PageFlag == PageEnum.Show || e.RowIndex < 0)
			{
				return;
			}
			DataGridViewColumn dataGridViewColumn = DGV.Columns[e.ColumnIndex];
			DataGridViewRow dataGridViewRow = DGV.Rows[e.RowIndex];
			if (dataGridViewColumn.Name == colEdit.Name)
			{
				GetLCNValue getLCNValue = new GetLCNValue(Tool.ToNumber16(Tool.GetString(dataGridViewRow.Cells[colLcnNumber.Name].Value)));
				if (getLCNValue.ShowDialog() == DialogResult.OK)
				{
					DGV.Rows[e.RowIndex].Cells[colLcnNumber.Name].Value = getLCNValue.ReturnLCN.ToString("X4");
				}
			}
			else
			{
				if (!(dataGridViewColumn.Name == colVisible.Name))
				{
					return;
				}
				if (Tool.GetBoolean(DGV.Rows[e.RowIndex].Cells[colVisible.Name].Value))
				{
					DGV.Rows[e.RowIndex].Cells[colVisible.Name].Value = false;
				}
				else if (Tool.ToNumber16(Tool.GetString(DGV.Rows[e.RowIndex].Cells[colLcnNumber.Name].Value)) == 0)
				{
					GetLCNValue getLCNValue = new GetLCNValue(Tool.ToNumber16(Tool.GetString(dataGridViewRow.Cells[colLcnNumber.Name].Value)));
					if (getLCNValue.ShowDialog() == DialogResult.OK)
					{
						DGV.Rows[e.RowIndex].Cells[colLcnNumber.Name].Value = getLCNValue.ReturnLCN.ToString("X4");
						DGV.Rows[e.RowIndex].Cells[colVisible.Name].Value = true;
					}
				}
				else
				{
					DGV.Rows[e.RowIndex].Cells[colVisible.Name].Value = true;
				}
			}
		}

		private void rabBit_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton radioButton = sender as RadioButton;
			if (radioButton != null && radioButton.Checked)
			{
				BindingData();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.Logical_Channel_Descriptor_Demo));
			this.DGV = new System.Windows.Forms.DataGridView();
			this.colServiceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colLcnNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colEdit = new System.Windows.Forms.DataGridViewLinkColumn();
			this.grbLcnLength = new System.Windows.Forms.GroupBox();
			this.rab14 = new System.Windows.Forms.RadioButton();
			this.rab10 = new System.Windows.Forms.RadioButton();
			this.panel1 = new System.Windows.Forms.Panel();
			base.grbContext.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.DGV).BeginInit();
			this.grbLcnLength.SuspendLayout();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(base.grbContext, "grbContext");
			base.grbContext.Controls.Add(this.panel1);
			base.grbContext.Controls.Add(this.grbLcnLength);
			resources.ApplyResources(this.DGV, "DGV");
			this.DGV.AllowUserToAddRows = false;
			this.DGV.AllowUserToDeleteRows = false;
			this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DGV.Columns.AddRange(this.colServiceID, this.colName, this.colVisible, this.colLcnNumber, this.colEdit);
			this.DGV.Name = "DGV";
			this.DGV.ReadOnly = true;
			this.DGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(DGV_CellContentClick);
			resources.ApplyResources(this.colServiceID, "colServiceID");
			this.colServiceID.Name = "colServiceID";
			this.colServiceID.ReadOnly = true;
			resources.ApplyResources(this.colName, "colName");
			this.colName.Name = "colName";
			this.colName.ReadOnly = true;
			resources.ApplyResources(this.colVisible, "colVisible");
			this.colVisible.Name = "colVisible";
			this.colVisible.ReadOnly = true;
			this.colVisible.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colVisible.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			resources.ApplyResources(this.colLcnNumber, "colLcnNumber");
			this.colLcnNumber.Name = "colLcnNumber";
			this.colLcnNumber.ReadOnly = true;
			resources.ApplyResources(this.colEdit, "colEdit");
			this.colEdit.Name = "colEdit";
			this.colEdit.ReadOnly = true;
			this.colEdit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colEdit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			resources.ApplyResources(this.grbLcnLength, "grbLcnLength");
			this.grbLcnLength.Controls.Add(this.rab14);
			this.grbLcnLength.Controls.Add(this.rab10);
			this.grbLcnLength.Name = "grbLcnLength";
			this.grbLcnLength.TabStop = false;
			resources.ApplyResources(this.rab14, "rab14");
			this.rab14.Name = "rab14";
			this.rab14.UseVisualStyleBackColor = true;
			this.rab14.CheckedChanged += new System.EventHandler(rabBit_CheckedChanged);
			resources.ApplyResources(this.rab10, "rab10");
			this.rab10.Checked = true;
			this.rab10.Name = "rab10";
			this.rab10.TabStop = true;
			this.rab10.UseVisualStyleBackColor = true;
			this.rab10.CheckedChanged += new System.EventHandler(rabBit_CheckedChanged);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Controls.Add(this.DGV);
			this.panel1.Name = "panel1";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "Logical_Channel_Descriptor_Demo";
			base.grbContext.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.DGV).EndInit();
			this.grbLcnLength.ResumeLayout(false);
			this.grbLcnLength.PerformLayout();
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
