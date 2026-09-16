using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using Com.Cn.Tool.UserControls;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.TableDescriptor
{
	public class FormDescriptorType : FormToolBase
	{
		private DescriptorTypeManager pageManager = new DescriptorTypeManager();

		private LogBLL logManager = new LogBLL();

		private IContainer components = null;

		private GroupBox grbTableType;

		private DGV DGV;

		private TableLayoutPanel tableLayoutPanel1;

		private ComboBox cmbTableType;

		private Label lblTableType;

		private DataGridViewCheckBoxColumn CK;

		private DataGridViewTextBoxColumn colID;

		private DataGridViewTextBoxColumn colDesName;

		private DataGridViewTextBoxColumn colDesTag;

		private DataGridViewTextBoxColumn colDesLocation;

		private DataGridViewTextBoxColumn colDesType;

		private DataGridViewCheckBoxColumn colVisable;

		private EnumPossibleLoaction EnumLocation
		{
			get
			{
				EnumPossibleLoaction result = EnumPossibleLoaction.NIT;
				try
				{
					string value = Tool.GetValue(cmbTableType);
					if (value.Length == 0 || value.Contains('.'))
					{
						return result;
					}
					result = TypeManagent<EnumPossibleLoaction>.GetEnum(value);
				}
				catch (Exception ex)
				{
					result = EnumPossibleLoaction.NIT;
					Tool.DoNoneException(ex);
				}
				return result;
			}
		}

		public FormDescriptorType()
		{
			InitializeComponent();
			_FunctionName = EnumFunctionName.SysFuncDescriptorType;
			InitControl();
		}

		protected override void FormLoad()
		{
			DGV.InitDGV((string[])null);
			SetMenuVisible(true, true, true, true, false, false, true, true, false);
		}

		protected override void FuncQuery()
		{
			BindData();
		}

		protected override void FuncAdd()
		{
			ShowDetail(0L, PageEnum.Add);
		}

		protected override void FuncEdit()
		{
			DataGridViewRow currentRow = DGV.CurrentRow;
			if (currentRow != null)
			{
				ShowDetail(Tool.GetInt64(currentRow.Cells[colID.Name].Value), PageEnum.Edit);
			}
		}

		protected override bool FuncDelete()
		{
			bool flag = true;
			Dictionary<long, DescriptorType> dictionary = new Dictionary<long, DescriptorType>();
			foreach (DataGridViewRow item in (IEnumerable)DGV.Rows)
			{
				if (Tool.GetBoolean(item.Cells[CK.Name].Value))
				{
					DescriptorType descriptorType = item.Tag as DescriptorType;
					if (descriptorType != null)
					{
						dictionary.Add(descriptorType.Id, descriptorType);
					}
				}
			}
			if (dictionary.Count == 0)
			{
				msg = ResourceManagement.GetLocationString("NoAvailableContentDelete");
				flag = false;
			}
			else
			{
				if (!Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.ConfirmDelete], MessageBoxButtons.OKCancel))
				{
					return false;
				}
				flag = pageManager.DeleteDescriptorType(dictionary.Keys.ToList(), out msg);
				if (flag)
				{
					StringBuilder stringBuilder = new StringBuilder(EnumLocation.ToString() + ":");
					foreach (long key in dictionary.Keys)
					{
						stringBuilder.Append("Tag:[" + dictionary[key].DesTag + "]TagName:[" + dictionary[key].DesName + "]-");
					}
					logManager.AddLog(base.FunctionName, EnumOSName.Delete, stringBuilder.ToString());
					BindData();
				}
			}
			Tool.ShowMessage(msg);
			return flag;
		}

		private void cmbTableType_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindData();
		}

		private void DGV_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
				{
					DataGridViewRow dataGridViewRow = DGV.Rows[e.RowIndex];
					if (dataGridViewRow != null)
					{
						ShowDetail(Tool.GetInt64(dataGridViewRow.Cells[colID.Name].Value), PageEnum.Show);
					}
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void DGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			DGV.EndEdit();
			try
			{
				if (e.RowIndex >= 0 && e.ColumnIndex == 0)
				{
					DataGridViewRow dataGridViewRow = DGV.Rows[e.RowIndex];
					DescriptorType descriptorType = dataGridViewRow.Tag as DescriptorType;
					if (descriptorType != null && descriptorType.DesType == 0)
					{
						msg = ResourceManagement.GetLocationString("CannotDeleteSystemDefinedDescriptor");
						Tool.ShowMessage(msg);
					}
					else
					{
						dataGridViewRow.Cells[CK.Name].Value = !Tool.GetBoolean(dataGridViewRow.Cells[CK.Name].Value);
					}
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		protected override void FormShown()
		{
			base.FormShown();
		}

		private void BindData()
		{
			DGV.Rows.Clear();
			string value = Tool.GetValue(cmbTableType);
			if (value.Length == 0 || value.Contains('.'))
			{
				return;
			}
			try
			{
				IList<DescriptorType> descriptorTypesByLoaction = pageManager.GetDescriptorTypesByLoaction(EnumLocation);
				foreach (DescriptorType item in descriptorTypesByLoaction)
				{
					BindDataToRow(item, DGV.Rows.Add(new DataGridViewRow()));
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void BindDataToRow(DescriptorType item, int rowIndex)
		{
			DataGridViewRow dataGridViewRow = DGV.Rows[rowIndex];
			dataGridViewRow.Tag = item;
			dataGridViewRow.Cells[colID.Name].Value = Tool.GetString(item.Id);
			dataGridViewRow.Cells[colDesName.Name].Value = Tool.GetString(item.DesName);
			dataGridViewRow.Cells[colDesTag.Name].Value = Tool.FormatNumber(item.DesTag, FormatString.Ins.X2);
			dataGridViewRow.Cells[colDesLocation.Name].Value = Tool.GetString(item.DesLocation);
			dataGridViewRow.Cells[colDesType.Name].Value = item.DesTypeString;
			dataGridViewRow.Cells[colVisable.Name].Value = item.BKStatus;
		}

		private void ShowDetail(long id, PageEnum pageFlag)
		{
			try
			{
				FormDescriptorTypeDetail formDescriptorTypeDetail = new FormDescriptorTypeDetail(EnumLocation, id, pageFlag);
				if (formDescriptorTypeDetail.ShowDialog() == DialogResult.OK)
				{
					BindData();
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void InitControl()
		{
			Tool.BindingComboBox(BaseData.Ins.GetPosibleLocations(), cmbTableType);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.FormDescriptorType));
			this.grbTableType = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.cmbTableType = new System.Windows.Forms.ComboBox();
			this.lblTableType = new System.Windows.Forms.Label();
			this.DGV = new Com.Cn.Tool.UserControls.DGV();
			this.CK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDesName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDesTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDesLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDesType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colVisable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			base.pnlBase.SuspendLayout();
			this.grbTableType.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.DGV).BeginInit();
			base.SuspendLayout();
			resources.ApplyResources(base.pnlBase, "pnlBase");
			base.pnlBase.Controls.Add(this.DGV);
			base.pnlBase.Controls.Add(this.grbTableType);
			resources.ApplyResources(this.grbTableType, "grbTableType");
			this.grbTableType.Controls.Add(this.tableLayoutPanel1);
			this.grbTableType.Name = "grbTableType";
			this.grbTableType.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.cmbTableType, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblTableType, 0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.cmbTableType, "cmbTableType");
			this.cmbTableType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTableType.FormattingEnabled = true;
			this.cmbTableType.Name = "cmbTableType";
			this.cmbTableType.SelectedIndexChanged += new System.EventHandler(cmbTableType_SelectedIndexChanged);
			resources.ApplyResources(this.lblTableType, "lblTableType");
			this.lblTableType.Name = "lblTableType";
			resources.ApplyResources(this.DGV, "DGV");
			this.DGV.AllowUserToAddRows = false;
			this.DGV.AllowUserToDeleteRows = false;
			this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DGV.Columns.AddRange(this.CK, this.colID, this.colDesName, this.colDesTag, this.colDesLocation, this.colDesType, this.colVisable);
			this.DGV.Name = "DGV";
			this.DGV.RowTemplate.Height = 23;
			this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.DGV.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(DGV_CellMouseClick);
			this.DGV.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(DGV_CellMouseDoubleClick);
			resources.ApplyResources(this.CK, "CK");
			this.CK.Name = "CK";
			this.CK.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.CK.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			resources.ApplyResources(this.colID, "colID");
			this.colID.Name = "colID";
			resources.ApplyResources(this.colDesName, "colDesName");
			this.colDesName.Name = "colDesName";
			resources.ApplyResources(this.colDesTag, "colDesTag");
			this.colDesTag.Name = "colDesTag";
			resources.ApplyResources(this.colDesLocation, "colDesLocation");
			this.colDesLocation.Name = "colDesLocation";
			resources.ApplyResources(this.colDesType, "colDesType");
			this.colDesType.Name = "colDesType";
			resources.ApplyResources(this.colVisable, "colVisable");
			this.colVisable.Name = "colVisable";
			this.colVisable.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colVisable.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormDescriptorType";
			base.pnlBase.ResumeLayout(false);
			this.grbTableType.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.DGV).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
