using DeXin.EPG.DAL;
using DeXin.EPG.IDAL;

namespace DeXin.EPG.DALFactory
{
	public class SQLServerFactory : AbstractFactory
	{
		public override IPermitGroupManagementDAL CreatePermitGroupManagementDAL()
		{
			return new PermitGroupManagementDAL();
		}

		public override IUserInfo CreateUserInfoService()
		{
			return new UserInfoService();
		}

		public override INITInfoDAL CreateNITInfoDAL()
		{
			return new NITInfoDAL();
		}

		public override ITSInfoDAL CreateTSInofDAL()
		{
			return new TSInfoDAL();
		}

		public override IProgramInfoDAL CreateProgramInfoDAL()
		{
			return new ProgramInfoDAL();
		}

		public override IDescriptorType CreateDescriptorTypeService()
		{
			return new DescriptorTypeService();
		}

		public override IDescriptor CreateDescriptorService()
		{
			return new DescriptorService();
		}

		public override IGroupInfoDAL CreateGroupInfoDAL()
		{
			return new GroupInfoDAL();
		}

		public override ILogDAL CreateLogDAL()
		{
			return new LogDAL();
		}

		public override ISendSpeedInfoDAL CreateSendSpeedInfoDAL()
		{
			return new SendSpeedInfoDAL();
		}

		public override IDBBackupRestoreDAL CreateDBBackupRestoreDAL()
		{
			return new DBBackupRestoreDAL();
		}

		public override ISingleIPOutputDAL CreateSingleIPOutputDAL()
		{
			return new SingleIPOutputDAL();
		}

		public override IMultiIPOutputDAL CreateMultiIPOutputDAL()
		{
			return new MultiIPOutputDAL();
		}

		public override IPMTPIDReMappingDAL CreatePMTPIDReMappingDAL()
		{
			return new PMTPIDReMappingDAL();
		}

		public override IBATInfoDAL CreateBATInfoDAL()
		{
			return new BATInfoDAL();
		}

		public override IBATToProgramDAL CreateBATToProgramDAL()
		{
			return new BATToProgramDAL();
		}

		public override IEITInfoDAL CreateEITInfoDAL()
		{
			return new EITInfoDAL();
		}

		public override ISendOrNotInfoDAL CreateSendOrNotInfoDAL()
		{
			return new SendOrNotInfoDAL();
		}

		public override IReMappingPIDDAL CreateReMappingPIDDAL()
		{
			return new ReMappingPIDDAL();
		}

		public override IMultimediaDAL CreateMultimediaDAL()
		{
			return new MultimediaDAL();
		}

		public override IStreamTypeInfoDAL CreateStreamTypeInfoDAL()
		{
			return new StreamTypeInfoDAL();
		}

		public override IPATDAL CreatePATDAL()
		{
			return new PATDAL();
		}

		public override ISendNoticesDAL CreateSendNoticesDAL()
		{
			return new SendNoticesDAL();
		}

		public override ISystemSetting CreateSystemSettingDAL()
		{
			return new SystemSettingDAL();
		}
	}
}
