using DeXinEPGSoftDog;

namespace DeXin.EPG.BLL
{
	public class HTBDogBll
	{
		public static bool CheckHTBDog(int reTry = 3)
		{
			if (SoftDog.CheckHTBDog())
			{
				return true;
			}
			if (reTry < 0)
			{
				return false;
			}
			return CheckHTBDog(reTry - 1);
		}

		public static bool CheckHTBDogDesc(int reTry = 3)
		{
			if (SoftDogDes.CheckDog() == 0)
			{
				return true;
			}
			if (reTry < 0)
			{
				return false;
			}
			SoftDogDes.ReleaseDog();
			int num = 0;
			int num2 = 0;
			bool flag = true;
			num = 5;
			num2 = 6;
			if (SoftDogDes.IniDog(num) == num2)
			{
				return CheckHTBDogDesc(reTry - 1);
			}
			SoftDogDes.ReleaseDog();
			return false;
		}
	}
}
