using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Com.Cn.Tool.UserControls
{
	public class DateTimeLong : UserControl
	{
		private IContainer components;

		private DateTimePicker dtpDate1;

		private DateTimePicker dtpDate2;

		private TableLayoutPanel tlpTable;

		private Label label1;

		private DateTime dt = DateTime.Now;

		[Description("只读属性")]
		[Category("外观")]
		public bool ReadOnly
		{
			get
			{
				return !base.Enabled;
			}
			set
			{
				base.Enabled = !value;
			}
		}

		[Description("所表示的时间")]
		[Category("外观")]
		public DateTime DT
		{
			get
			{
				dt = new DateTime(dtpDate1.Value.Year, dtpDate1.Value.Month, dtpDate1.Value.Day, dtpDate2.Value.Hour, dtpDate2.Value.Minute, dtpDate2.Value.Second);
				return dt;
			}
			set
			{
				dt = value;
				dtpDate1.Value = new DateTime(dt.Year, dt.Month, dt.Day);
				dtpDate2.Value = dt;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Com.Cn.Tool.UserControls.DateTimeLong));
			this.dtpDate1 = new System.Windows.Forms.DateTimePicker();
			this.dtpDate2 = new System.Windows.Forms.DateTimePicker();
			this.tlpTable = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.tlpTable.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(this.dtpDate1, "dtpDate1");
			this.dtpDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpDate1.Name = "dtpDate1";
			resources.ApplyResources(this.dtpDate2, "dtpDate2");
			this.dtpDate2.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.dtpDate2.Name = "dtpDate2";
			this.dtpDate2.ShowUpDown = true;
			resources.ApplyResources(this.tlpTable, "tlpTable");
			this.tlpTable.Controls.Add(this.dtpDate1, 0, 0);
			this.tlpTable.Controls.Add(this.dtpDate2, 1, 0);
			this.tlpTable.Controls.Add(this.label1, 2, 0);
			this.tlpTable.Name = "tlpTable";
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.AllowDrop = true;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.tlpTable);
			base.Name = "DateTimeLong";
			this.tlpTable.ResumeLayout(false);
			this.tlpTable.PerformLayout();
			base.ResumeLayout(false);
		}

		public DateTimeLong()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			dtpDate1.KeyPress += dtpDate1_KeyPress;
			dtpDate2.KeyPress += dtpDate2_KeyPress;
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
		}

		private void dtpDate2_KeyPress(object sender, KeyPressEventArgs e)
		{
			dtpDate2.Value = new DateTime(dtpDate1.Value.Year, dtpDate1.Value.Month, dtpDate1.Value.Day, dtpDate2.Value.Hour, dtpDate2.Value.Minute, dtpDate2.Value.Second);
		}

		private void dtpDate1_KeyPress(object sender, KeyPressEventArgs e)
		{
			dtpDate1.Value = new DateTime(dtpDate1.Value.Year, dtpDate1.Value.Month, dtpDate1.Value.Day, dtpDate2.Value.Hour, dtpDate2.Value.Minute, dtpDate2.Value.Second);
		}
	}
}
