using System;
using System.Runtime.InteropServices;

namespace DeXinEPGSoftDog
{
	public class SoftDog
	{
		[StructLayout(LayoutKind.Sequential)]
		public class UmhDog
		{
			public ushort DogBytes;

			public ushort DogAddr;

			public uint DogPassword;

			public uint NewPassword;

			public uint DogResult;

			public uint CurrentNu;

			public ushort DogCascade;

			public ushort NewCascade;

			public byte[] DogData;

			public uint Retocde;

			[DllImport("HTBDog\\htbdog.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public unsafe static extern void Check_Dog(ushort Cascade, uint Password, uint* lpRet);

			[DllImport("HTBDog\\htbdog.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public unsafe static extern void Read_Dog(ushort Cascade, uint Password, ushort addr, ushort bytes, void* pdata, uint* lpRet);

			[DllImport("HTBDog\\htbdog.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public unsafe static extern void Write_Dog(ushort Cascade, uint Password, ushort addr, ushort bytes, void* pdata, uint* lpRet);

			[DllImport("HTBDog\\htbdog.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public unsafe static extern void Convert_Dog(ushort Cascade, uint Password, ushort bytes, void* pdata, void* pResult, uint* lpRet);

			[DllImport("HTBDog\\htbdog.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public unsafe static extern void DisableShare_Dog(ushort Cascade, uint Password, uint* lpRet);

			[DllImport("HTBDog\\htbdog.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public unsafe static extern void SetPassword_Dog(ushort Cascade, uint Password, uint InNewPassword, uint* lpRet);

			[DllImport("HTBDog\\htbdog.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public unsafe static extern void SetCascade_Dog(ushort Cascade, uint Password, ushort NewCascade, uint* lpRet);

			[DllImport("HTBDog\\htbdog.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public unsafe static extern void GetCurrentNo_Dog(ushort Cascade, uint* lpCurrentNo, uint* lpRet);

			public UmhDog(ushort num)
			{
				DogBytes = num;
				DogData = new byte[DogBytes];
			}

			public unsafe void CheckDog()
			{
				fixed (uint* retocde = &Retocde)
				{
					Check_Dog(DogCascade, DogPassword, retocde);
				}
			}

			public unsafe void ReadDog()
			{
				fixed (byte* pdata = &DogData[0])
				{
					fixed (uint* retocde = &Retocde)
					{
						Read_Dog(DogCascade, DogPassword, DogAddr, DogBytes, pdata, retocde);
					}
				}
			}

			public unsafe void WriteDog()
			{
				fixed (byte* pdata = &DogData[0])
				{
					fixed (uint* retocde = &Retocde)
					{
						Write_Dog(DogCascade, DogPassword, DogAddr, DogBytes, pdata, retocde);
					}
				}
			}

			public unsafe void ConvertDog()
			{
				fixed (uint* retocde = &Retocde)
				{
					fixed (uint* dogResult = &DogResult)
					{
						fixed (byte* pdata = &DogData[0])
						{
							Convert_Dog(DogCascade, DogPassword, DogBytes, pdata, dogResult, retocde);
						}
					}
				}
			}

			public unsafe void DisableShareDog()
			{
				fixed (uint* retocde = &Retocde)
				{
					DisableShare_Dog(DogCascade, DogPassword, retocde);
				}
			}

			public unsafe void SetPasswordDog()
			{
				fixed (uint* retocde = &Retocde)
				{
					SetPassword_Dog(DogCascade, DogPassword, NewPassword, retocde);
				}
			}

			public unsafe void SetCascadeDog()
			{
				fixed (uint* retocde = &Retocde)
				{
					SetCascade_Dog(DogCascade, DogPassword, NewCascade, retocde);
				}
			}

			public unsafe void GetCurrentNoDog()
			{
				fixed (uint* retocde = &Retocde)
				{
					fixed (uint* currentNu = &CurrentNu)
					{
						GetCurrentNo_Dog(DogCascade, currentNu, retocde);
					}
				}
			}

			public unsafe void ReadData(ushort addressIndex, ushort len)
			{
				fixed (byte* pdata = &DogData[0])
				{
					fixed (uint* retocde = &Retocde)
					{
						Read_Dog(DogCascade, DogPassword, addressIndex, len, pdata, retocde);
					}
				}
			}

			public bool Passed(ushort addressIndex, ushort len, byte[] given)
			{
				ReadData(addressIndex, len);
				bool result = true;
				for (int i = 0; i < len; i++)
				{
					if (given[i] != DogData[i])
					{
						result = false;
					}
				}
				return result;
			}
		}

		public static bool CheckHTBDog()
		{
			UmhDog umhDog = new UmhDog(200);
			umhDog.DogCascade = (ushort)Convert.ToInt32("0");
			umhDog.DogPassword = (uint)Convert.ToInt32("0");
			byte[] given = new byte[10] { 66, 105, 103, 66, 97, 111, 57, 52, 57, 52 };
			return umhDog.Passed(0, 10, given);
		}
	}
}
