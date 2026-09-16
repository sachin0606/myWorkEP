using System;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;

namespace DeXin.EPG.Tools
{
	public class EPGConfigurationManger : ConfigManager
	{
		private static EPGConfigurationManger _Instance;

		private string _Language = "";

		private string _LanguageUI = "";

		private int _PageSize = 25;

		public static EPGConfigurationManger Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new EPGConfigurationManger();
				}
				return _Instance;
			}
		}

		public string Language
		{
			get
			{
				return _Language;
			}
			set
			{
				_Language = value;
				Write(AppConfigKey.Language, _Language);
			}
		}

		public string LanguageUI
		{
			get
			{
				return _LanguageUI;
			}
			set
			{
				_LanguageUI = value;
				Write(AppConfigKey.LanguageUI, _LanguageUI);
			}
		}

		public int PageSize
		{
			get
			{
				return _PageSize;
			}
		}

		protected EPGConfigurationManger()
		{
			try
			{
				InitRead();
			}
			catch (Exception ex)
			{
				Tool.DoNoneException(ex);
			}
		}

		public void InitRead()
		{
			base.DefaultMessage = LocationControl.Ins.LocationItems[LacationEnum.OperateFailure];
			_Language = Read(AppConfigKey.Language);
			_LanguageUI = Read(AppConfigKey.LanguageUI);
			_PageSize = Tool.GetInt32(Read("PageSize"));
		}

		public override void Save()
		{
			appManger.Settings["Language"].Value = Language;
			appManger.Settings["LanguageUI"].Value = LanguageUI;
			base.Save();
		}
	}
}
