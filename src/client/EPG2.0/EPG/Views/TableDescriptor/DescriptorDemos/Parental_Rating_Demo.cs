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
using DeXin.EPG.Views.TableDescriptor.DescriptorDemos;

namespace EPG.Views.TableDescriptor.DescriptorDemos
{
	public class Parental_Rating_Demo : DescriptorDemoBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label label1;

		private ComboBox cmbcountry_code;

		private Label label2;

		private ComboBox cmbRating;

		private Button btnAdd;

		private DataGridView DGV;

		private Button btnRemove;

		private CheckBox cboDefine;

		private NumericUpDown numRateValue;

		private DataGridViewTextBoxColumn colcountroy_code;

		private DataGridViewTextBoxColumn colcmbrating;

		private DataGridViewTextBoxColumn colRating;

		private List<ParentalRating> _Items = new List<ParentalRating>();

		private List<EitRateSetting> rateings = new List<EitRateSetting>();

		private List<ParentalRating> Items
		{
			get
			{
				return _Items;
			}
			set
			{
				_Items = value;
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
			this.cmbcountry_code = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbRating = new System.Windows.Forms.ComboBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.cboDefine = new System.Windows.Forms.CheckBox();
			this.numRateValue = new System.Windows.Forms.NumericUpDown();
			this.DGV = new System.Windows.Forms.DataGridView();
			this.colcountroy_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colcmbrating = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colRating = new System.Windows.Forms.DataGridViewTextBoxColumn();
			base.grbContext.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.numRateValue).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.DGV).BeginInit();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.DGV);
			base.grbContext.Controls.Add(this.tableLayoutPanel1);
			base.grbContext.Size = new System.Drawing.Size(724, 393);
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel1.Controls.Add(this.cmbcountry_code, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.cmbRating, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.btnAdd, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnRemove, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.cboDefine, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.numRateValue, 3, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(718, 94);
			this.tableLayoutPanel1.TabIndex = 0;
			this.cmbcountry_code.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.cmbcountry_code.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbcountry_code.FormattingEnabled = true;
			this.cmbcountry_code.Location = new System.Drawing.Point(81, 3);
			this.cmbcountry_code.Name = "cmbcountry_code";
			this.cmbcountry_code.Size = new System.Drawing.Size(169, 21);
			this.cmbcountry_code.TabIndex = 1;
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "controy_code";
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(42, 34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(33, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "rating";
			this.cmbRating.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.cmbRating.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbRating.FormattingEnabled = true;
			this.cmbRating.Location = new System.Drawing.Point(81, 30);
			this.cmbRating.Name = "cmbRating";
			this.cmbRating.Size = new System.Drawing.Size(169, 21);
			this.cmbRating.TabIndex = 3;
			this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnAdd.Location = new System.Drawing.Point(81, 57);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 6;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
			this.btnRemove.Location = new System.Drawing.Point(256, 57);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(75, 23);
			this.btnRemove.TabIndex = 7;
			this.btnRemove.Text = "Remove";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(btnRemove_Click);
			this.cboDefine.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.cboDefine.AutoSize = true;
			this.cboDefine.Location = new System.Drawing.Point(256, 32);
			this.cboDefine.Name = "cboDefine";
			this.cboDefine.Size = new System.Drawing.Size(76, 17);
			this.cboDefine.TabIndex = 8;
			this.cboDefine.Text = "self-Define";
			this.cboDefine.UseVisualStyleBackColor = true;
			this.cboDefine.CheckedChanged += new System.EventHandler(cboDefine_CheckedChanged);
			this.numRateValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.numRateValue.Location = new System.Drawing.Point(338, 30);
			this.numRateValue.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
			this.numRateValue.Name = "numRateValue";
			this.numRateValue.ReadOnly = true;
			this.numRateValue.Size = new System.Drawing.Size(77, 20);
			this.numRateValue.TabIndex = 9;
			this.DGV.AllowUserToAddRows = false;
			this.DGV.AllowUserToDeleteRows = false;
			this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DGV.Columns.AddRange(this.colcountroy_code, this.colcmbrating, this.colRating);
			this.DGV.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DGV.Location = new System.Drawing.Point(3, 111);
			this.DGV.Name = "DGV";
			this.DGV.ReadOnly = true;
			this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.DGV.Size = new System.Drawing.Size(718, 280);
			this.DGV.TabIndex = 1;
			this.colcountroy_code.HeaderText = "countroy_code";
			this.colcountroy_code.Name = "colcountroy_code";
			this.colcountroy_code.ReadOnly = true;
			this.colcmbrating.HeaderText = "rating";
			this.colcmbrating.Name = "colcmbrating";
			this.colcmbrating.ReadOnly = true;
			this.colcmbrating.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colRating.HeaderText = "rating value";
			this.colRating.Name = "colRating";
			this.colRating.ReadOnly = true;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "Parental_Rating_Demo";
			base.Size = new System.Drawing.Size(724, 393);
			base.grbContext.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.numRateValue).EndInit();
			((System.ComponentModel.ISupportInitialize)this.DGV).EndInit();
			base.ResumeLayout(false);
		}

		public Parental_Rating_Demo()
		{
			InitializeComponent();
		}

		public Parental_Rating_Demo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
			InitControl();
		}

		protected override void InitControl()
		{
			IList<BaseKey> countryCode = BaseData.Ins.GetCountryCode();
			Tool.BindingComboBox(countryCode, cmbcountry_code);
			rateings = BaseData.Ins.GetEitRateSetting();
			cmbRating.DataSource = rateings.ToList();
			cmbRating.ValueMember = "RateValue";
			cmbRating.DisplayMember = "RateName";
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			object obj;
			if (!cboDefine.Checked)
			{
				obj = cmbRating.SelectedItem as EitRateSetting;
			}
			else
			{
				EitRateSetting eitRateSetting = new EitRateSetting();
				eitRateSetting.RateCode = "";
				eitRateSetting.RateValue = (byte)numRateValue.Value;
				eitRateSetting.RateName = numRateValue.Value.ToString();
				obj = eitRateSetting;
			}
			EitRateSetting eitRateSetting2 = (EitRateSetting)obj;
			if (eitRateSetting2 != null)
			{
				ParentalRating item = new ParentalRating(Tool.GetValue(cmbcountry_code), eitRateSetting2.RateCode, (byte)eitRateSetting2.RateValue);
				Items.Add(item);
				BindDGV();
			}
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			if (DGV.CurrentRow != null)
			{
				ParentalRating parentalRating = DGV.CurrentRow.Tag as ParentalRating;
				if (parentalRating != null)
				{
					Items.Remove(parentalRating);
					BindDGV();
				}
			}
		}

		protected override void BindingData()
		{
			base.PageFlag = PageEnum.Add;
			if (base.ET.DataLength > 0)
			{
				Items = ParentalRatingBase.DataConvert(base.ET.DescriptionData);
			}
			cmbcountry_code.SelectedValue = BaseData.Ins.GetDefaultConuntryCode();
			BindDGV();
		}

		private void BindDGV()
		{
			DGV.Rows.Clear();
			int num = 0;
			ParentalRating item;
			foreach (ParentalRating item2 in Items)
			{
				item = item2;
				num = DGV.Rows.Add(new DataGridViewRow());
				DGV.Rows[num].Tag = item;
				DGV.Rows[num].Cells[colcountroy_code.Name].Value = item.countroy_code;
				EitRateSetting eitRateSetting = rateings.FirstOrDefault((EitRateSetting p) => p.RateValue == item.Rating);
				DGV.Rows[num].Cells[colcmbrating.Name].Value = ((eitRateSetting == null) ? item.Rating.ToString() : eitRateSetting.RateName);
				DGV.Rows[num].Cells[colRating.Name].Value = item.Rating;
			}
		}

		public override bool IsValidated(ErrorProvider error)
		{
			ValidateChildren(ValidationConstraints.TabStop);
			int startIndex = 0;
			ParentalRatingBase parentalRatingBase = new ParentalRatingBase(base.ET.TableID);
			parentalRatingBase.Items.AddRange(Items);
			parentalRatingBase.DataLength = Items.Count * 4;
			parentalRatingBase.DescriptionData = new byte[parentalRatingBase.DataLength];
			foreach (ParentalRating item in Items)
			{
				Tool.ToByteArray(item.ToByte(), parentalRatingBase.DescriptionData, ref startIndex);
			}
			base.ET = parentalRatingBase;
			return true;
		}

		private void cboDefine_CheckedChanged(object sender, EventArgs e)
		{
			numRateValue.ReadOnly = !cboDefine.Checked;
			cmbRating.Enabled = !cboDefine.Checked;
		}
	}
}
