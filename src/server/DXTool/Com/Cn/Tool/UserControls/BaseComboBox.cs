using System.ComponentModel;
using System.Windows.Forms;

namespace Com.Cn.Tool.UserControls
{
	public class BaseComboBox : ComboBox
	{
		private ComboBoxType _comboBoxType;

		[Category("Customer")]
		[Description("下拉列表类型 Com.Cn.Tool.UserControls.TextBoxType")]
		public ComboBoxType comboBoxType
		{
			get
			{
				return _comboBoxType;
			}
			set
			{
				_comboBoxType = value;
			}
		}
	}
}
