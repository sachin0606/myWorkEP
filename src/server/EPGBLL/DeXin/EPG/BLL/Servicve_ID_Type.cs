using System;
using DeXin.EPG.Model;

namespace DeXin.EPG.BLL
{
	public class Servicve_ID_Type
	{
		private ProgramInfoManager programManager = new ProgramInfoManager();

		private long _TsID = 0L;

		private long _IDendity = 0L;

		private ushort _ProgramID = 0;

		private string _ProgramName = string.Empty;

		private byte _ServiceTypeID = 0;

		public long TsID
		{
			get
			{
				return _TsID;
			}
			set
			{
				_TsID = value;
			}
		}

		public long IDendity
		{
			get
			{
				return _IDendity;
			}
			set
			{
				_IDendity = value;
			}
		}

		public ushort ProgramID
		{
			get
			{
				return _ProgramID;
			}
			set
			{
				_ProgramID = value;
			}
		}

		public string ProgramName
		{
			get
			{
				if (_ProgramName.Length == 0 && TsID > 0)
				{
					ProgramInfo byTSIdentityAndServiceId = programManager.GetByTSIdentityAndServiceId(TsID, Convert.ToInt32(ProgramID));
					if (byTSIdentityAndServiceId != null)
					{
						_ProgramName = byTSIdentityAndServiceId.ProgramName;
						IDendity = byTSIdentityAndServiceId.Identity;
					}
				}
				return _ProgramName;
			}
			set
			{
				_ProgramName = value;
			}
		}

		public byte ServiceTypeID
		{
			get
			{
				return _ServiceTypeID;
			}
			set
			{
				_ServiceTypeID = value;
			}
		}

		public Servicve_ID_Type(long identity, ushort proID, string proName, byte serviceType, long tsID)
		{
			ProgramID = proID;
			ProgramName = proName;
			ServiceTypeID = serviceType;
			TsID = tsID;
			IDendity = identity;
		}
	}
}
