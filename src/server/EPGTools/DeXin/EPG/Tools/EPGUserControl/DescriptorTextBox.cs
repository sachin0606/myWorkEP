using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using DeXin.EPG.Resource;

namespace DeXin.EPG.Tools.EPGUserControl
{
	public class DescriptorTextBox : TextBox
	{
		private long _MinValue = 0L;

		private long _MaxValue = 0L;

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

		[Category("Custom")]
		[Description("最大值")]
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

		[DefaultValue(new byte[] { })]
		public byte[] Value
		{
			get
			{
				return Tool.StringToBytes(Text);
			}
			set
			{
				Text = Tool.BytesToString(value);
			}
		}

		private void DescriptorTextBox_TextChanged(object sender, EventArgs e)
		{
			char[] array = Text.ToArray();
			char[] array2 = array;
			foreach (char c in array2)
			{
				if (c != '.' && c != '\b' && (c < '.' || c > '9'))
				{
					Text = "";
				}
			}
		}

		private void DescriptorTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if (keyChar == '.' && Text.Contains('.'))
			{
				e.Handled = true;
			}
			else if (keyChar == '.' || keyChar == '\b' || (keyChar >= '0' && keyChar <= '9'))
			{
				e.Handled = false;
			}
			else
			{
				e.Handled = true;
			}
		}

		public bool IsValitdated(ErrorProvider error)
		{
			error.SetError(this, "");
			if (Text.Length == 0)
			{
				error.SetError(this, ResourceManagement.GetLocationString("ValueCannotBeNull"));
				return false;
			}
			if (MinValue > 0 && Convert.ToInt64(Text, 16) < MinValue)
			{
				error.SetError(this, ResourceManagement.GetLocationString("CannotBeLessThanMin") + "[0x" + MinValue.ToString("x2") + "]");
				return false;
			}
			if (MaxValue > 0 && Convert.ToInt64(Text, 16) > MaxValue)
			{
				error.SetError(this, ResourceManagement.GetLocationString("CannotBeGreaterThanMax") + "[0x" + MaxValue.ToString("x2") + "]");
				return false;
			}
			return true;
		}
	}
}
