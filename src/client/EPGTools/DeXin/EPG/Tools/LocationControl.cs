using System.Collections.Generic;
using DeXin.EPG.Resource;

namespace DeXin.EPG.Tools
{
	public class LocationControl
	{
		public Dictionary<LacationEnum, string> LocationItems = new Dictionary<LacationEnum, string>();

		private static LocationControl _ins;

		public static LocationControl Ins
		{
			get
			{
				if (_ins == null)
				{
					_ins = new LocationControl();
				}
				return _ins;
			}
		}

		private LocationControl()
		{
			Init();
		}

		public void Init()
		{
			LocationItems.Clear();
			AddItem(LacationEnum.SaveSucceed);
			AddItem(LacationEnum.SaveFailure);
			AddItem(LacationEnum.EditSucceed);
			AddItem(LacationEnum.EditFailure);
			AddItem(LacationEnum.DeleteSucceed);
			AddItem(LacationEnum.DeleteFailure);
			AddItem(LacationEnum.OperateSucceed);
			AddItem(LacationEnum.OperateFailure);
			AddItem(LacationEnum.IsNotNull);
			AddItem(LacationEnum.InputFormatError);
			AddItem(LacationEnum.ConfirmDelete);
			AddItem(LacationEnum.ConfirmSubmit);
			AddItem(LacationEnum.DatabaseConnectionException);
			AddItem(LacationEnum.SelectOneOrMoreRows);
			AddItem(LacationEnum.PutInAccount);
			AddItem(LacationEnum.PutInPassword);
			AddItem(LacationEnum.AccountIsNull);
			AddItem(LacationEnum.AccountIsNotExist);
			AddItem(LacationEnum.PasswordIswrong);
			AddItem(LacationEnum.LoinInSucceed);
			AddItem(LacationEnum.UserDisEnabled);
			AddItem(LacationEnum.ConfirmForbid);
			AddItem(LacationEnum.tsbAdd);
			AddItem(LacationEnum.tsbCancel);
			AddItem(LacationEnum.tsbClose);
			AddItem(LacationEnum.tsbDelete);
			AddItem(LacationEnum.tsbEdit);
			AddItem(LacationEnum.tsbHelp);
			AddItem(LacationEnum.tsbPrint);
			AddItem(LacationEnum.tsbSave);
			AddItem(LacationEnum.tsbQuer);
		}

		private void AddItem(LacationEnum lac)
		{
			LocationItems.Add(lac, ResourceManagement.GetLocationString(lac.ToString()));
		}
	}
}
