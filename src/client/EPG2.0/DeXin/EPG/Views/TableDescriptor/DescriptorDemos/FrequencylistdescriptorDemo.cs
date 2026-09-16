using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class FrequencylistdescriptorDemo : DescriptorDemoBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label label1;

		private DescriptorCombobox cmbcoding_type;

		private Label label2;

		private Button btnAdd;

		private DataGridView dgv;

		private DataGridViewTextBoxColumn colcentre_frequency;

		private DataGridViewLinkColumn colRomove;

		private TextBox txtcentre_frequency;

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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnAdd = new System.Windows.Forms.Button();
			this.dgv = new System.Windows.Forms.DataGridView();
			this.colcentre_frequency = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colRomove = new System.Windows.Forms.DataGridViewLinkColumn();
			this.txtcentre_frequency = new System.Windows.Forms.TextBox();
			this.cmbcoding_type = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			base.grbContext.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgv).BeginInit();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.tableLayoutPanel1);
			base.grbContext.Size = new System.Drawing.Size(789, 549);
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.cmbcoding_type, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.btnAdd, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.dgv, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtcentre_frequency, 1, 1);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(783, 530);
			this.tableLayoutPanel1.TabIndex = 0;
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(28, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "coding_type";
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(90, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "centre_frequency";
			this.btnAdd.Location = new System.Drawing.Point(271, 30);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 3;
			this.btnAdd.Text = "Add To List";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
			this.dgv.AllowUserToAddRows = false;
			this.dgv.AllowUserToDeleteRows = false;
			this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv.Columns.AddRange(this.colcentre_frequency, this.colRomove);
			this.tableLayoutPanel1.SetColumnSpan(this.dgv, 3);
			this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgv.Location = new System.Drawing.Point(3, 59);
			this.dgv.Name = "dgv";
			this.dgv.ReadOnly = true;
			this.dgv.Size = new System.Drawing.Size(777, 468);
			this.dgv.TabIndex = 4;
			this.dgv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgv_CellContentClick);
			this.colcentre_frequency.HeaderText = "centre_frequency";
			this.colcentre_frequency.Name = "colcentre_frequency";
			this.colcentre_frequency.ReadOnly = true;
			this.colRomove.HeaderText = "Remove";
			this.colRomove.Name = "colRomove";
			this.colRomove.ReadOnly = true;
			this.colRomove.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colRomove.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.txtcentre_frequency.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtcentre_frequency.Location = new System.Drawing.Point(99, 30);
			this.txtcentre_frequency.MaxLength = 9;
			this.txtcentre_frequency.Name = "txtcentre_frequency";
			this.txtcentre_frequency.Size = new System.Drawing.Size(166, 20);
			this.txtcentre_frequency.TabIndex = 5;
			this.cmbcoding_type.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.cmbcoding_type.FormattingEnabled = true;
			this.cmbcoding_type.Location = new System.Drawing.Point(99, 3);
			this.cmbcoding_type.MaxValue = 0L;
			this.cmbcoding_type.MinValue = 0L;
			this.cmbcoding_type.Name = "cmbcoding_type";
			this.cmbcoding_type.Size = new System.Drawing.Size(166, 21);
			this.cmbcoding_type.TabIndex = 1;
			this.cmbcoding_type.Value = "";
			this.cmbcoding_type.SelectedIndexChanged += new System.EventHandler(cmbcoding_type_SelectedIndexChanged);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FrequencylistdescriptorDemo";
			base.Size = new System.Drawing.Size(789, 549);
			base.grbContext.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.dgv).EndInit();
			base.ResumeLayout(false);
		}

		public FrequencylistdescriptorDemo()
		{
			InitializeComponent();
		}

		public FrequencylistdescriptorDemo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
			InitControl();
		}

		protected override void InitControl()
		{
			Tool.BindingComboBox(BaseData.Ins.CodingTypeValues, cmbcoding_type);
			txtcentre_frequency.KeyPress += txtcentre_frequency_KeyPress;
		}

		private void txtcentre_frequency_KeyPress(object sender, KeyPressEventArgs e)
		{
			TextBox textBox = sender as TextBox;
			char keyChar = e.KeyChar;
			if (keyChar == '\b' || (keyChar == '.' && !textBox.Text.Contains('.')) || (keyChar >= '0' && keyChar <= '9') || (keyChar >= 'A' && keyChar <= 'F') || (keyChar >= 'a' && keyChar <= 'f'))
			{
				e.Handled = false;
			}
			else
			{
				e.Handled = true;
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			string text = txtcentre_frequency.Text;
			if (cmbcoding_type.Value == "01")
			{
				text = CommTool.GetTextBoxString(3, 5, txtcentre_frequency);
			}
			else if (cmbcoding_type.Value == "02")
			{
				text = CommTool.GetTextBoxString(4, 4, txtcentre_frequency);
			}
			if (text.Length > 8)
			{
				text = text.Substring(0, 8);
			}
			text = text.Replace(".", "");
			BindDataToRow(text.PadLeft(8, '0'), dgv.Rows.Add(new DataGridViewRow()));
		}

		private void BindDataToRow(string item, int index)
		{
			dgv.Rows[index].Tag = item;
			if (cmbcoding_type.Value == "01")
			{
				dgv.Rows[index].Cells[colcentre_frequency.Name].Value = item.Insert(3, ".");
			}
			else if (cmbcoding_type.Value == "02")
			{
				dgv.Rows[index].Cells[colcentre_frequency.Name].Value = item.Insert(4, ".");
			}
			else
			{
				dgv.Rows[index].Cells[colcentre_frequency.Name].Value = item;
			}
			dgv.Rows[index].Cells[colRomove.Name].Value = "Remove";
		}

		private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == 1 && (base.PageFlag == PageEnum.Add || base.PageFlag == PageEnum.Edit))
			{
				dgv.Rows.RemoveAt(e.RowIndex);
			}
		}

		protected override void BindingData()
		{
			if (base.PageFlag != PageEnum.Add && base.PageFlag != PageEnum.Empty)
			{
				int dataLength = base.ET.DataLength;
				int start = 0;
				byte b = Tool.ResolveByte(base.ET.DescriptionData, 1, ref start)[0];
				cmbcoding_type.Value = (b & 3).ToString("X2");
				while (start < base.ET.DataLength)
				{
					byte[] buffer = Tool.ResolveByte(base.ET.DescriptionData, 4, ref start);
					BindDataToRow(Tool.BytesToString(buffer), dgv.Rows.Add(new DataGridViewRow()));
				}
			}
			if (base.PageFlag == PageEnum.Show)
			{
				btnAdd.Enabled = false;
			}
		}

		public override bool IsValidated(ErrorProvider error)
		{
			if (!cmbcoding_type.IsValitdated(error))
			{
				return false;
			}
			int startIndex = 0;
			base.ET.DataLength = dgv.Rows.Count * 4 + 1;
			base.ET.DescriptionData = new byte[base.ET.DataLength];
			base.ET.DescriptionData[startIndex++] = (byte)(0x3FC0 | (Tool.StringToBytes(cmbcoding_type.Value)[0] & 3));
			foreach (DataGridViewRow item in (IEnumerable)dgv.Rows)
			{
				string args = item.Tag as string;
				Tool.ToByteArray(Tool.StringToBytes(args), base.ET.DescriptionData, ref startIndex);
			}
			return true;
		}

		private void cmbcoding_type_SelectedIndexChanged(object sender, EventArgs e)
		{
			foreach (DataGridViewRow item in (IEnumerable)dgv.Rows)
			{
				string text = item.Tag as string;
				if (cmbcoding_type.Value == "01")
				{
					item.Cells[colcentre_frequency.Name].Value = text.Insert(3, ".");
				}
				else if (cmbcoding_type.Value == "02")
				{
					item.Cells[colcentre_frequency.Name].Value = text.Insert(4, ".");
				}
				else
				{
					item.Cells[colcentre_frequency.Name].Value = text;
				}
			}
		}
	}
}
