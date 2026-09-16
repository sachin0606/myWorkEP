namespace DeXin.EPG.IDAL
{
	public interface IDBBackupRestoreDAL
	{
		bool BackupDB(string path);

		bool RestoreDB(string path);
	}
}
