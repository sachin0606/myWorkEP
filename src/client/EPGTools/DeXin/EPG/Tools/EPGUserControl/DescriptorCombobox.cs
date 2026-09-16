using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DeXin.EPG.Resource;

namespace DeXin.EPG.Tools.EPGUserControl
{
	public class DescriptorCombobox : ComboBox
	{
		private long _MinValue = 0L;

		private long _MaxValue = 0L;

		[Category("Custom")]
		[Description("get:当DataSourece不存在值时,获取Text值;set 当DataSourece不存在值时Text显示  value")]
		public string Value
		{
			get
			{
				string text = ((base.SelectedValue == null) ? Text : base.SelectedValue.ToString());
				return (text.Length % 2 == 0) ? text : ("0" + text);
			}
			set
			{
				if (value.Length == 0)
				{
					base.Text = value;
					return;
				}
				base.SelectedValue = value;
				if (base.SelectedValue == null)
				{
					base.Text = value;
				}
			}
		}

		[Category("Custom")]
		[Description("最小值")]
		public long MinValue
		{
			get
			{
				return _MinValue;
			}
			set
			{
				_MinValue = ((value <= 0) ? 0 : value);
			}
		}

		[Description("最大值")]
		[Category("Custom")]
		public long MaxValue
		{
			get
			{
				return _MaxValue;
			}
			set
			{
				_MaxValue = ((value <= 0) ? 0 : value);
			}
		}

		public DescriptorCombobox()
		{
			base.TextChanged += DescriptorCombobox_TextChanged;
			base.KeyPress += DescriptorCombobox_KeyPress;
		}

		private void DescriptorCombobox_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			switch (keyChar)
			{
			default:
				if ((keyChar < 'A' || keyChar > 'F') && (keyChar < 'a' || keyChar > 'f'))
				{
					break;
				}
				goto case '\b';
			case '\b':
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				e.Handled = false;
				return;
			}
			e.Handled = true;
		}

		private void DescriptorCombobox_TextChanged(object sender, EventArgs e)
		{
			if (Value.Length <= 0)
			{
				return;
			}
			char[] array = Value.ToArray();
			char[] array2 = array;
			foreach (char c in array2)
			{
				switch (c)
				{
				case '\b':
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					continue;
				}
				if ((c < 'A' || c > 'F') && (c < 'a' || c > 'f'))
				{
					Value = "";
				}
			}
		}

		public bool IsValitdated(ErrorProvider error)
		{
			error.SetError(this, "");
			if (Value.Length == 0)
			{
				error.SetError(this, ResourceManagement.GetLocationString("ValueCannotBeNull"));
				return false;
			}
			base.SelectedValue = Value;
			if (base.SelectedValue != null)
			{
				return true;
			}
			if (MinValue > 0 && Convert.ToInt64(Value, 16) < MinValue)
			{
				error.SetError(this, ResourceManagement.GetLocationString("CannotBeLessThanMin") + "[0x" + MinValue.ToString("x2") + "]");
				return false;
			}
			if (MaxValue > 0 && Convert.ToInt64(Value, 16) > MaxValue)
			{
				error.SetError(this, ResourceManagement.GetLocationString("CannotBeGreaterThanMax") + "[0x" + MaxValue.ToString("x2") + "]");
				return false;
			}
			return true;
		}
	}
}
