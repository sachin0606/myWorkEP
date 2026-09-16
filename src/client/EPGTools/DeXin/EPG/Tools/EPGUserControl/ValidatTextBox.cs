using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool;

namespace DeXin.EPG.Tools.EPGUserControl
{
	public class ValidatTextBox : TextBox
	{
		private bool _NotNull = true;

		private bool _IsValidated = true;

		[Category("Custom")]
		[Description("是否为必填项")]
		public bool NotNull
		{
			get
			{
				return _NotNull;
			}
			set
			{
				_NotNull = value;
			}
		}

		[Description("是否通过验证")]
		[Category("Custom")]
		public bool IsValidated
		{
			get
			{
				return _IsValidated;
			}
			set
			{
				_IsValidated = value;
			}
		}

		public ValidatTextBox()
		{
			base.Validating += ValidatTextBox_Validating;
		}

		private void ValidatTextBox_Validating(object sender, CancelEventArgs e)
		{
			if (base.Enabled && !base.ReadOnly && NotNull)
			{
				if (Tool.GetLength(Text) == 0)
				{
					IsValidated = false;
					Tool.ShowErrorProviderMessage(this, LocationControl.Ins.LocationItems[LacationEnum.IsNotNull]);
				}
				else
				{
					IsValidated = true;
					Tool.ShowErrorProviderMessage(this, "");
				}
			}
		}
	}
}
