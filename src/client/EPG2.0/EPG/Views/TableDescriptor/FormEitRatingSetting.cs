using System;
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
using DeXin.EPG.Views.BaseForm;

namespace EPG.Views.TableDescriptor
{
	public class FormEitRatingSetting : FormDetailBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel2;

		private Label label1;

		private ComboBox cmbcountry_code;

		private GroupBox groupBox1;

		private GroupBox groupBox2;

		private TableLayoutPanel tableLayoutPanel3;

		private Label label2;

		private TextBox txtratecode;

		private Label label3;

		private Label label4;

		private TextBox txtratename;

		private Button btnAdd;

		private DataGridView DGV;

		private NumericUpDown numratevalue;

		private DataGridViewTextBoxColumn colratecode;

		private DataGridViewTextBoxColumn colratevalue;

		private DataGridViewTextBoxColumn colratename;

		private DataGridViewLinkColumn colDelete;

		private SystemSettingBLL sysbll = new SystemSettingBLL();

		private DescriptorManagement desbll = new DescriptorManagement();

		private List<EitRateSetting> eitrates = new List<EitRateSetting>();

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
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbcountry_code = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.txtratecode = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtratename = new System.Windows.Forms.TextBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.DGV = new System.Windows.Forms.DataGridView();
			this.colratecode = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colratevalue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colratename = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDelete = new System.Windows.Forms.DataGridViewLinkColumn();
			this.numratevalue = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.DGV).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.numratevalue).BeginInit();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.groupBox2);
			base.grbText.Controls.Add(this.groupBox1);
			base.grbText.Text = "所有功能";
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 396f));
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.cmbcountry_code, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(601, 29);
			this.tableLayoutPanel2.TabIndex = 0;
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "country_code";
			this.cmbcountry_code.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.cmbcountry_code.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbcountry_code.FormattingEnabled = true;
			this.cmbcountry_code.Location = new System.Drawing.Point(81, 4);
			this.cmbcountry_code.Name = "cmbcountry_code";
			this.cmbcountry_code.Size = new System.Drawing.Size(121, 21);
			this.cmbcountry_code.TabIndex = 1;
			this.groupBox1.Controls.Add(this.tableLayoutPanel2);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(3, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(607, 48);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Country_Code";
			this.groupBox2.Controls.Add(this.tableLayoutPanel3);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(3, 64);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(607, 441);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Rating Setting";
			this.tableLayoutPanel3.ColumnCount = 7;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.txtratecode, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.label3, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.label4, 4, 0);
			this.tableLayoutPanel3.Controls.Add(this.txtratename, 5, 0);
			this.tableLayoutPanel3.Controls.Add(this.btnAdd, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.DGV, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.numratevalue, 3, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 3;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(601, 422);
			this.tableLayoutPanel3.TabIndex = 0;
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 6);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "rate_code";
			this.txtratecode.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.txtratecode.Location = new System.Drawing.Point(64, 3);
			this.txtratecode.Name = "txtratecode";
			this.txtratecode.Size = new System.Drawing.Size(100, 20);
			this.txtratecode.TabIndex = 1;
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(170, 6);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(57, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "rate_value";
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(359, 6);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(57, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "rate_name";
			this.txtratename.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.txtratename.Location = new System.Drawing.Point(422, 3);
			this.txtratename.Name = "txtratename";
			this.txtratename.Size = new System.Drawing.Size(100, 20);
			this.txtratename.TabIndex = 3;
			this.btnAdd.Location = new System.Drawing.Point(64, 29);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 4;
			this.btnAdd.Text = "add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
			this.DGV.AllowUserToAddRows = false;
			this.DGV.AllowUserToDeleteRows = false;
			this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DGV.Columns.AddRange(this.colratecode, this.colratevalue, this.colratename, this.colDelete);
			this.tableLayoutPanel3.SetColumnSpan(this.DGV, 7);
			this.DGV.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DGV.Location = new System.Drawing.Point(3, 58);
			this.DGV.Name = "DGV";
			this.DGV.ReadOnly = true;
			this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.DGV.Size = new System.Drawing.Size(595, 361);
			this.DGV.TabIndex = 5;
			this.DGV.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(DGV_CellMouseClick);
			this.colratecode.HeaderText = "rate_code";
			this.colratecode.Name = "colratecode";
			this.colratecode.ReadOnly = true;
			this.colratevalue.HeaderText = "rate_value";
			this.colratevalue.Name = "colratevalue";
			this.colratevalue.ReadOnly = true;
			this.colratename.HeaderText = "rate_name";
			this.colratename.Name = "colratename";
			this.colratename.ReadOnly = true;
			this.colDelete.HeaderText = "Delete";
			this.colDelete.Name = "colDelete";
			this.colDelete.ReadOnly = true;
			this.numratevalue.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.numratevalue.Location = new System.Drawing.Point(233, 3);
			this.numratevalue.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
			this.numratevalue.Name = "numratevalue";
			this.numratevalue.Size = new System.Drawing.Size(120, 20);
			this.numratevalue.TabIndex = 6;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(613, 583);
			base.Name = "FormEitRatingSetting";
			this.Text = "FormEitRatingSetting";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.DGV).EndInit();
			((System.ComponentModel.ISupportInitialize)this.numratevalue).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormEitRatingSetting()
		{
			InitializeComponent();
			base.PageFlag = PageEnum.Edit;
		}

		protected override void FormShown()
		{
			Init();
			SysFunction sysFunction = new SysFunction(base.FunctionName);
			base.ContentText = sysFunction.LocationFunctionName;
			SystemSetting systemSetting = sysbll.GetSystemSetting(SystemSettingCode.CountryCode);
			if (systemSetting != null)
			{
				cmbcountry_code.SelectedValue = systemSetting.Value;
			}
			eitrates = sysbll.GetEitRateSetting();
			BindDGV();
		}

		private void BindDGV()
		{
			DGV.Rows.Clear();
			eitrates.ForEach(delegate(EitRateSetting item)
			{
				int index = DGV.Rows.Add(new DataGridViewRow());
				DGV.Rows[index].Tag = item;
				DGV.Rows[index].Cells[colDelete.Name].Value = "delete";
				DGV.Rows[index].Cells[colratecode.Name].Value = item.RateCode;
				DGV.Rows[index].Cells[colratevalue.Name].Value = item.RateValue;
				DGV.Rows[index].Cells[colratename.Name].Value = item.RateName;
			});
		}

		private void Init()
		{
			IList<BaseKey> countryCode = BaseData.Ins.GetCountryCode();
			Tool.BindingComboBox(countryCode, cmbcountry_code);
		}

		protected override bool FunSubmit()
		{
			SystemSetting systemSetting = new SystemSetting(SystemSettingCode.CountryCode);
			systemSetting.Value = cmbcountry_code.SelectedValue.ToString();
			systemSetting.Comment = "CountryCode";
			sysbll.Save(systemSetting);
			sysbll.SaveOrUpdate(eitrates);
			Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.SaveSucceed]);
			return true;
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			string code = Tool.GetValue(txtratecode);
			string value = Tool.GetValue(txtratename);
			int @int = Tool.GetInt32(numratevalue.Value);
			if (string.IsNullOrEmpty(code))
			{
				txtratecode.Focus();
				return;
			}
			if (string.IsNullOrEmpty(value))
			{
				txtratename.Focus();
				return;
			}
			EitRateSetting eitRateSetting = eitrates.FirstOrDefault((EitRateSetting p) => p.RateCode == code);
			if (eitRateSetting == null)
			{
				eitRateSetting = new EitRateSetting();
				eitrates.Add(eitRateSetting);
			}
			eitRateSetting.RateCode = code;
			eitRateSetting.RateValue = @int;
			eitRateSetting.RateName = value;
			BindDGV();
		}

		private void DGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				DataGridViewColumn dataGridViewColumn = DGV.Columns[e.ColumnIndex];
				DataGridViewRow dataGridViewRow = DGV.Rows[e.RowIndex];
				EitRateSetting eitRateSetting = dataGridViewRow.Tag as EitRateSetting;
				if (eitRateSetting != null)
				{
					txtratecode.Text = eitRateSetting.RateCode;
					txtratename.Text = eitRateSetting.RateName;
					numratevalue.Value = eitRateSetting.RateValue;
				}
				if (dataGridViewColumn.Name == colDelete.Name && eitRateSetting != null)
				{
					eitrates.Remove(eitRateSetting);
					BindDGV();
				}
			}
		}
	}
}
