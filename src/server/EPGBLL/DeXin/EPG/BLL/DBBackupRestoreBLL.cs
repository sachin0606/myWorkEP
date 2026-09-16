using System;
using DeXin.EPG.DALFactory;

namespace DeXin.EPG.BLL
{
	public class DBBackupRestoreBLL
	{
		private AbstractFactory factory = AbstractFactory.CreateFactory();

		public bool BackupDB(string path)
		{
			bool flag = false;
			try
			{
				return factory.CreateDBBackupRestoreDAL().BackupDB(path);
			}
			catch (Exception ex)
			{
				flag = false;
				throw ex;
			}
		}

		public bool RestoreDB(string path)
		{
			bool flag = false;
			try
			{
				return factory.CreateDBBackupRestoreDAL().RestoreDB(path);
			}
			catch (Exception ex)
			{
				flag = false;
				throw ex;
			}
		}
	}
}
