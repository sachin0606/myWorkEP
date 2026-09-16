using System.ComponentModel;
using System.Windows.Forms;

namespace Com.Cn.Tool.UserControls
{
	public class BaseComboBox : ComboBox
	{
		private ComboBoxType _comboBoxType;

		[Description("下拉列表类型 Com.Cn.Tool.UserControls.TextBoxType")]
		[Category("Customer")]
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
