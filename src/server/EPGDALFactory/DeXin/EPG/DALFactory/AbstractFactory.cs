using System.Configuration;
using System.Reflection;
using DeXin.EPG.IDAL;

namespace DeXin.EPG.DALFactory
{
	public abstract class AbstractFactory
	{
		public static AbstractFactory CreateFactory()
		{
			AbstractFactory abstractFactory = null;
			try
			{
				string typeName = ConfigurationManager.AppSettings["FactoryName"].ToString();
				return (AbstractFactory)Assembly.Load("EPGDALFactory").CreateInstance(typeName, true);
			}
			catch
			{
				return new SQLServerFactory();
			}
		}

		public abstract IPermitGroupManagementDAL CreatePermitGroupManagementDAL();

		public abstract IGroupInfoDAL CreateGroupInfoDAL();

		public abstract ILogDAL CreateLogDAL();

		public abstract IUserInfo CreateUserInfoService();

		public abstract INITInfoDAL CreateNITInfoDAL();

		public abstract ITSInfoDAL CreateTSInofDAL();

		public abstract IProgramInfoDAL CreateProgramInfoDAL();

		public abstract IDescriptorType CreateDescriptorTypeService();

		public abstract IDescriptor CreateDescriptorService();

		public abstract ISendSpeedInfoDAL CreateSendSpeedInfoDAL();

		public abstract IDBBackupRestoreDAL CreateDBBackupRestoreDAL();

		public abstract ISingleIPOutputDAL CreateSingleIPOutputDAL();

		public abstract IMultiIPOutputDAL CreateMultiIPOutputDAL();

		public abstract IPMTPIDReMappingDAL CreatePMTPIDReMappingDAL();

		public abstract IBATInfoDAL CreateBATInfoDAL();

		public abstract IBATToProgramDAL CreateBATToProgramDAL();

		public abstract IEITInfoDAL CreateEITInfoDAL();

		public abstract ISendOrNotInfoDAL CreateSendOrNotInfoDAL();

		public abstract IReMappingPIDDAL CreateReMappingPIDDAL();

		public abstract IMultimediaDAL CreateMultimediaDAL();

		public abstract IStreamTypeInfoDAL CreateStreamTypeInfoDAL();

		public abstract IPATDAL CreatePATDAL();

		public abstract ISendNoticesDAL CreateSendNoticesDAL();

		public abstract ISystemSetting CreateSystemSettingDAL();
	}
}
