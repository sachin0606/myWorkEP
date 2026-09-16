using System.Windows.Forms;

namespace EPGServiceNotify
{
	public class BalloonInfo
	{
		private string _Title;

		private string _Content;

		private int _Timeout;

		private ToolTipIcon _BalloonType;

		public string Title
		{
			get
			{
				return _Title;
			}
			set
			{
				_Title = value;
			}
		}

		public string Content
		{
			get
			{
				return _Content;
			}
			set
			{
				_Content = value;
			}
		}

		public int Timeout
		{
			get
			{
				return _Timeout;
			}
			set
			{
				_Timeout = value;
			}
		}

		public ToolTipIcon BalloonType
		{
			get
			{
				return _BalloonType;
			}
			set
			{
				_BalloonType = value;
			}
		}
	}
}
