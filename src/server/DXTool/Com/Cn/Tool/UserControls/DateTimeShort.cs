using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool.Model;

namespace Com.Cn.Tool.UserControls
{
	public class DateTimeShort : DateTimePicker
	{
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

		public override string Text
		{
			get
			{
				return Tool.FormatDateTime(base.Value, FormatString.Ins.DataShort);
			}
		}

		public DateTimeShort()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Com.Cn.Tool.UserControls.DateTimeShort));
			base.SuspendLayout();
			resources.ApplyResources(this, "$this");
			base.AccessibleName = null;
			this.BackgroundImage = null;
			base.CalendarFont = null;
			base.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			base.CustomFormat = null;
			this.Font = null;
			base.Tag = "";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
