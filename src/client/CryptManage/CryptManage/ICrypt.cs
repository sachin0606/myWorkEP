namespace CryptManage
{
	public interface ICrypt
	{
		string EnCrpty(string UnCopytedString);

		string DeCrypt(string CoyptedString);
	}
}
