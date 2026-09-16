using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using EPG.Views.TableDescriptor.DescriptorDemos.ExtendForms;

namespace EPG.Views.TableDescriptor.DescriptorDemos
{
	public class ExtendedEventDescriptorDemo : UCDescriptorBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblRemark;

		private TextBox txtRemark;

		private DataGridView DGVItems;

		private Label label1;

		private DataGridViewTextBoxColumn colKey;

		private DataGridViewTextBoxColumn colText;

		private DataGridViewLinkColumn colDelet;

		private Button btnAddItem;

		private Label lblLanguageCode;

		private ComboBox cmbLanguageCode;

		private DescriptorManagement descriptorbll = new DescriptorManagement();

		private EITInfoManager eitbll = new EITInfoManager();

		private ExtendedEventDescriptorBase _ET;

		private EitInfo EiT { get; set; }

		public ExtendedEventDescriptorBase ET
		{
			get
			{
				return _ET;
			}
			set
			{
				_ET = value;
			}
		}

		public override DescriptorBase Descriotor
		{
			get
			{
				return ET;
			}
			set
			{
				ET = (ExtendedEventDescriptorBase)value;
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.txtRemark = new System.Windows.Forms.TextBox();
			this.DGVItems = new System.Windows.Forms.DataGridView();
			this.colKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colText = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDelet = new System.Windows.Forms.DataGridViewLinkColumn();
			this.lblRemark = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnAddItem = new System.Windows.Forms.Button();
			this.cmbLanguageCode = new System.Windows.Forms.ComboBox();
			this.lblLanguageCode = new System.Windows.Forms.Label();
			base.grbContext.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.DGVItems).BeginInit();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.tableLayoutPanel1);
			base.grbContext.Size = new System.Drawing.Size(675, 463);
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
			this.tableLayoutPanel1.Controls.Add(this.lblLanguageCode, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.cmbLanguageCode, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtRemark, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.DGVItems, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblRemark, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.btnAddItem, 2, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(669, 444);
			this.tableLayoutPanel1.TabIndex = 1;
			this.txtRemark.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtRemark.Location = new System.Drawing.Point(133, 159);
			this.txtRemark.Multiline = true;
			this.txtRemark.Name = "txtRemark";
			this.txtRemark.Size = new System.Drawing.Size(345, 282);
			this.txtRemark.TabIndex = 3;
			this.DGVItems.AllowUserToAddRows = false;
			this.DGVItems.AllowUserToDeleteRows = false;
			this.DGVItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DGVItems.Columns.AddRange(this.colKey, this.colText, this.colDelet);
			this.DGVItems.Location = new System.Drawing.Point(133, 30);
			this.DGVItems.Name = "DGVItems";
			this.DGVItems.ReadOnly = true;
			this.DGVItems.Size = new System.Drawing.Size(345, 123);
			this.DGVItems.TabIndex = 5;
			this.DGVItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(DGVItems_CellContentClick);
			this.colKey.HeaderText = "Key";
			this.colKey.Name = "colKey";
			this.colKey.ReadOnly = true;
			this.colText.HeaderText = "Text";
			this.colText.Name = "colText";
			this.colText.ReadOnly = true;
			this.colDelet.HeaderText = "Delete";
			this.colDelet.Name = "colDelet";
			this.colDelet.ReadOnly = true;
			this.lblRemark.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.lblRemark.AutoSize = true;
			this.lblRemark.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblRemark.Location = new System.Drawing.Point(83, 156);
			this.lblRemark.Name = "lblRemark";
			this.lblRemark.Size = new System.Drawing.Size(44, 13);
			this.lblRemark.TabIndex = 4;
			this.lblRemark.Text = "Remark";
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.label1.AutoSize = true;
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(95, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Items";
			this.btnAddItem.Location = new System.Drawing.Point(484, 30);
			this.btnAddItem.Name = "btnAddItem";
			this.btnAddItem.Size = new System.Drawing.Size(75, 23);
			this.btnAddItem.TabIndex = 6;
			this.btnAddItem.Text = "Add Item";
			this.btnAddItem.UseVisualStyleBackColor = true;
			this.btnAddItem.Click += new System.EventHandler(btnAddItem_Click);
			this.cmbLanguageCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLanguageCode.Enabled = false;
			this.cmbLanguageCode.FormattingEnabled = true;
			this.cmbLanguageCode.Location = new System.Drawing.Point(133, 3);
			this.cmbLanguageCode.Name = "cmbLanguageCode";
			this.cmbLanguageCode.Size = new System.Drawing.Size(200, 21);
			this.cmbLanguageCode.TabIndex = 1;
			this.lblLanguageCode.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblLanguageCode.AutoSize = true;
			this.lblLanguageCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblLanguageCode.Location = new System.Drawing.Point(3, 7);
			this.lblLanguageCode.Name = "lblLanguageCode";
			this.lblLanguageCode.Size = new System.Drawing.Size(124, 13);
			this.lblLanguageCode.TabIndex = 0;
			this.lblLanguageCode.Text = "iso_639_language_code";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "ExtendedEventDescriptorDemo";
			base.Size = new System.Drawing.Size(675, 463);
			base.grbContext.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.DGVItems).EndInit();
			base.ResumeLayout(false);
		}

		public ExtendedEventDescriptorDemo(DescriptorBase item, PageEnum PageFlag)
			: base(PageFlag)
		{
			InitializeComponent();
			EiT = eitbll.GetbyID(item.TableID);
			List<ExtendedEventDescriptor> list = new List<ExtendedEventDescriptor>();
			ET = new ExtendedEventDescriptorBase(item.TableID, EiT.Program.TS.NIT.LanguageCode)
			{
				TableID = item.TableID,
				Location = EnumPossibleLoaction.EIT,
				DesTypeID = item.DesTypeID
			};
			ET.Setkvs(new List<KeyValuePair<string, string>>());
			ET.SetRemark(EiT.EventLongRemark);
			InitControl();
		}

		protected override void BindingData()
		{
			cmbLanguageCode.SelectedValue = ET.LanguageCode;
			List<DescriptorSigleBase> descriptors = (from p in descriptorbll.GetDescriptors(EnumPossibleLoaction.EIT, EiT.Identity)
				where p.DesTypeTag == 78
				select p).ToList();
			ExtendedEventDescriptorBase.FormDB(descriptors).ForEach(delegate(KeyValuePair<string, string> kv)
			{
				int index = DGVItems.Rows.Add(new DataGridViewRow());
				BindToRow(index, kv);
			});
			txtRemark.Text = EiT.EventLongRemark;
		}

		protected override void InitControl()
		{
			Tool.BindingComboBox(descriptorbll.GetLanguageCode(), cmbLanguageCode);
		}

		public override bool IsValidated(ErrorProvider error)
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			foreach (DataGridViewRow item in (IEnumerable)DGVItems.Rows)
			{
				list.Add((KeyValuePair<string, string>)item.Tag);
			}
			ET = new ExtendedEventDescriptorBase(EiT.Identity, cmbLanguageCode.SelectedValue.ToString());
			ET.Setkvs(list);
			ET.SetRemark(txtRemark.Text);
			return true;
		}

		private void btnAddItem_Click(object sender, EventArgs e)
		{
			ExtendedEventItem extendedEventItem = new ExtendedEventItem(base.PageFlag);
			if (extendedEventItem.ShowDialog() == DialogResult.OK)
			{
				KeyValuePair<string, string> kv = new KeyValuePair<string, string>(extendedEventItem.Key, extendedEventItem.Value);
				int index = DGVItems.Rows.Add(new DataGridViewRow());
				BindToRow(index, kv);
			}
		}

		private void DGVItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				DataGridViewColumn dataGridViewColumn = DGVItems.Columns[e.ColumnIndex];
				if (dataGridViewColumn.Name == colDelet.Name)
				{
					DGVItems.Rows.RemoveAt(e.RowIndex);
				}
			}
		}

		private void BindToRow(int index, KeyValuePair<string, string> kv)
		{
			DGVItems.Rows[index].Tag = kv;
			DGVItems.Rows[index].Cells[colKey.Name].Value = kv.Key;
			DGVItems.Rows[index].Cells[colText.Name].Value = kv.Value;
			DGVItems.Rows[index].Cells[colDelet.Name].Value = "Delete";
		}
	}
}
