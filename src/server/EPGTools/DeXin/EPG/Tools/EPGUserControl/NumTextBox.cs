using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Com.Cn.Tool;
using DeXin.EPG.Resource;

namespace DeXin.EPG.Tools.EPGUserControl
{
	public class NumTextBox : ValidatTextBox
	{
		private string _SplitChar = "-";

		private string _text = string.Empty;

		private int _MaxValue = 0;

		private int _SetMaxLength = 255;

		[DefaultValue("-")]
		[Category("Custom")]
		[Description("自定义分隔符号  不能为 0-9 , a-f , A-F")]
		public string SplitChar
		{
			get
			{
				if (_SplitChar.Length == 0)
				{
					_SplitChar = " ";
				}
				return _SplitChar;
			}
			set
			{
				_SplitChar = value;
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

		public int ValueOfInt32
		{
			get
			{
				try
				{
					return Convert.ToInt32(Text, 16);
				}
				catch
				{
					return -1;
				}
			}
		}

		private string text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = ((value == null) ? "" : value.Replace(SplitChar, ""));
			}
		}

		[DefaultValue(0)]
		[Category("Custom")]
		[Description("最大输入值  0为不控制")]
		public int MaxValue
		{
			get
			{
				return _MaxValue;
			}
			set
			{
				_MaxValue = value;
			}
		}

		[Category("Custom")]
		[Description("设置MaxLength属性")]
		[DefaultValue(255)]
		public int SetMaxLength
		{
			get
			{
				return _SetMaxLength;
			}
			set
			{
				_SetMaxLength = value;
				base.MaxLength = 3 * value - 1;
			}
		}

		public override int MaxLength
		{
			get
			{
				return base.MaxLength;
			}
		}

		[DefaultValue("")]
		public override string Text
		{
			get
			{
				return text;
			}
			set
			{
				base.Text = value;
			}
		}

		public NumTextBox()
		{
			base.KeyPress += NumTextBox_KeyPress;
			base.TextChanged += NumTextBox_TextChanged;
			base.Validating += NumTextBox_Validating;
			MaxLength = 764;
		}

		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.ResumeLayout(false);
		}

		private void NumTextBox_Validating(object sender, CancelEventArgs e)
		{
			if (!base.IsValidated)
			{
				return;
			}
			if (ForeColor == Color.Red)
			{
				Text = "0" + Text;
			}
			else if (_MaxValue > 0)
			{
				int num = _MaxValue + 1;
				try
				{
					num = Convert.ToInt32(Text, 16);
				}
				catch
				{
				}
				if (num > _MaxValue)
				{
					base.IsValidated = false;
					Tool.ShowErrorProviderMessage(this, ResourceManagement.GetLocationString("ValueInputExceedLimited") + _MaxValue.ToString("X2"));
				}
				else
				{
					base.IsValidated = true;
					Tool.ShowErrorProviderMessage(this, "");
				}
			}
		}

		private void NumTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			switch (keyChar)
			{
			default:
				if ((keyChar < 'A' || keyChar > 'F') && (keyChar < 'a' || keyChar > 'f'))
				{
					break;
				}
				goto case '\u0003';
			case '\u0003':
			case '\b':
			case '\u0016':
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

		private void NumTextBox_TextChanged(object sender, EventArgs e)
		{
			char[] array = base.Text.Replace(SplitChar, "").ToCharArray();
			char[] array2 = array;
			foreach (char c in array2)
			{
				if ((c < '0' || c > '9') && (c < 'A' || c > 'F') && (c < 'a' || c > 'f'))
				{
					base.Text = "";
					break;
				}
			}
			FormatText();
		}

		private void FormatText()
		{
			string text = base.Text.Replace(SplitChar, "").Replace("\n", "").Replace("\r", "")
				.Replace(" ", "");
			ForeColor = ((text.Length % 2 > 0) ? Color.Red : Color.Black);
			char[] array = text.ToCharArray();
			string text2 = "";
			for (int i = 0; i < array.Length; i++)
			{
				text2 = ((i <= 0 || i % 2 != 0) ? (text2 + array[i]) : (text2 + SplitChar + array[i]));
			}
			string text3 = (Text = text2);
			this.text = text3;
			Select(text2.Length, 0);
		}
	}
}
