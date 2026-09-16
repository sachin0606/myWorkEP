using System.Runtime.InteropServices;

namespace DeXinEPGSoftDog
{
	public class SoftDogDes
	{
		[DllImport("HTBDog\\DogCheck.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int CheckDog();

		[DllImport("HTBDog\\DogCheck.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int IniDog(int iSoftType);

		[DllImport("HTBDog\\DogCheck.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ReleaseDog();

		[DllImport("HTBDog\\DogCheck.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern string GetCurrentNo();

		[DllImport("HTBDog\\DogCheck.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int UpdateExtendDataADV1();

		[DllImport("HTBDog\\DogCheck.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int GetExtendDataADV1();

		[DllImport("HTBDog\\DogCheck.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int UpdateDateTimeADV1();

		[DllImport("HTBDog\\DogCheck.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int GetDateTimeADV1();
	}
}
