using System;
using System.Globalization;
using System.Threading;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Views.BaseForm
{
	[Serializable]
	public class ThreadInfo
	{
		private static ThreadInfo _Ins = null;

		public string language = string.Empty;

		public string languageUI = string.Empty;

		public string skinPath = string.Empty;

		public CultureInfo cullureInfo = null;

		public CultureInfo UIcullureInfo = null;

		public static ThreadInfo Ins
		{
			get
			{
				if (_Ins == null)
				{
					_Ins = new ThreadInfo();
				}
				return _Ins;
			}
		}

		private ThreadInfo()
		{
			SetInfo();
		}

		public void SetInfo()
		{
			language = EPGConfigurationManger.Instance.Language;
			languageUI = EPGConfigurationManger.Instance.LanguageUI;
			if (string.IsNullOrEmpty(language))
			{
				cullureInfo = Thread.CurrentThread.CurrentCulture;
			}
			else
			{
				cullureInfo = new CultureInfo(language);
			}
			if (string.IsNullOrEmpty(languageUI))
			{
				UIcullureInfo = Thread.CurrentThread.CurrentUICulture;
			}
			else
			{
				UIcullureInfo = new CultureInfo(languageUI);
			}
			if (!UIcullureInfo.Name.ToLower().Equals("en") && !UIcullureInfo.Name.ToLower().Equals("zh-cn") && !UIcullureInfo.Name.ToLower().Equals("zh-tw"))
			{
				UIcullureInfo = new CultureInfo("en-US");
				cullureInfo = new CultureInfo("en-US");
			}
			Thread.CurrentThread.CurrentUICulture = UIcullureInfo;
			Thread.CurrentThread.CurrentCulture = cullureInfo;
			LocationControl.Ins.Init();
		}
	}
}
