using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.DALFactory;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;

namespace DeXin.EPG.BLL
{
	public class ProgramInfoManager
	{
		private AbstractFactory db = AbstractFactory.CreateFactory();

		public IList<ProgramInfo> GetByTSIdentity(long TSIdentity)
		{
			return db.CreateProgramInfoDAL().GetProgramInfosByTSIdentity(TSIdentity);
		}

		public ProgramInfo GetByTSIdentityAndServiceId(long TSIdentity, int ServiceId)
		{
			return db.CreateProgramInfoDAL().GetByTSIdentityAndServiceId(TSIdentity, ServiceId);
		}

		public ProgramInfo Get(int NetworkID, int TSID, int ServiceID)
		{
			return db.CreateProgramInfoDAL().Get(NetworkID, TSID, ServiceID);
		}

		public int DeleteProgram(long ProgramIdentity)
		{
			return db.CreateProgramInfoDAL().DeleteProgram(ProgramIdentity);
		}

		public bool CanSaveProgramInfo(ProgramInfo Program)
		{
			return db.CreateProgramInfoDAL().CanSaveProgramInfo(Program);
		}

		public int Save(ProgramInfo Program)
		{
			return db.CreateProgramInfoDAL().Save(Program);
		}

		public IList<ProgramInfo> GetOutputProgram()
		{
			return db.CreateProgramInfoDAL().GetOutputProgram();
		}

		public IList<ProgramInfo> GetHasEITFileProgram()
		{
			return db.CreateProgramInfoDAL().GetHasEITFileProgram();
		}

		public ProgramInfo GetByIdentity(long Identity)
		{
			return db.CreateProgramInfoDAL().GetByIdentity(Identity);
		}

		public IList<ProgramInfo> GetByEitFile(string fileName)
		{
			return db.CreateProgramInfoDAL().GetByEitFile(fileName);
		}

		public List<StringBuilder> GetChannelConfiguration()
		{
			NITInfoManager nITInfoManager = new NITInfoManager();
			TSInfoManager tSInfoManager = new TSInfoManager();
			ProgramInfoManager programInfoManager = new ProgramInfoManager();
			DescriptorManagement descriptorManagement = new DescriptorManagement();
			IList<NitInfo> allNITInfos = nITInfoManager.GetAllNITInfos();
			LcnType lcntype = BaseData.Ins.lcntype;
			List<StringBuilder> list = new List<StringBuilder>();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Channel Configuration");
			stringBuilder.Append(",");
			list.Add(stringBuilder);
			foreach (NitInfo item in allNITInfos)
			{
				stringBuilder = new StringBuilder();
				stringBuilder.Append("Network ID");
				stringBuilder.Append(",");
				stringBuilder.Append(item.NetworkId);
				stringBuilder.Append(",");
				list.Add(stringBuilder);
				stringBuilder = new StringBuilder();
				stringBuilder.Append("Network Name");
				stringBuilder.Append(",");
				stringBuilder.Append(item.NetworkName);
				stringBuilder.Append(",");
				list.Add(stringBuilder);
				stringBuilder = new StringBuilder();
				stringBuilder.Append("Original Network ID");
				stringBuilder.Append(",");
				stringBuilder.Append("TS ID");
				stringBuilder.Append(",");
				stringBuilder.Append("TS Name");
				stringBuilder.Append(",");
				stringBuilder.Append("Service ID");
				stringBuilder.Append(",");
				stringBuilder.Append("Channel Name");
				stringBuilder.Append(",");
				stringBuilder.Append("Provider Name");
				stringBuilder.Append(",");
				stringBuilder.Append("LCN");
				stringBuilder.Append(",");
				stringBuilder.Append("EIT File Name");
				stringBuilder.Append(",");
				list.Add(stringBuilder);
				IList<TsInfo> tSInfoByNetWorkIdentity = tSInfoManager.GetTSInfoByNetWorkIdentity(item.Identity);
				IList<DescriptorSigleBase> nIT_TS_DescriptorsByNetWorkIDEntity = descriptorManagement.GetNIT_TS_DescriptorsByNetWorkIDEntity(item.Identity);
				TsInfo ts;
				foreach (TsInfo item2 in tSInfoByNetWorkIdentity)
				{
					ts = item2;
					IList<ProgramInfo> byTSIdentity = programInfoManager.GetByTSIdentity(ts.Identity);
					DescriptorSigleBase lcndescriptor = nIT_TS_DescriptorsByNetWorkIDEntity.FirstOrDefault((DescriptorSigleBase p) => p.TSTableID == ts.Identity && p.DesTypeTag == 131);
					List<LCNEntity> source = ToLcnEntitys(lcndescriptor, lcntype);
					ProgramInfo program;
					foreach (ProgramInfo item3 in byTSIdentity)
					{
						program = item3;
						stringBuilder = new StringBuilder();
						stringBuilder.Append(ts.OriginalNetworkId);
						stringBuilder.Append(",");
						stringBuilder.Append(ts.TsId);
						stringBuilder.Append(",");
						stringBuilder.Append(ts.TsName);
						stringBuilder.Append(",");
						stringBuilder.Append(program.ServiceId);
						stringBuilder.Append(",");
						stringBuilder.Append(program.ProgramName);
						stringBuilder.Append(",");
						stringBuilder.Append(program.ProviderName);
						stringBuilder.Append(",");
						LCNEntity lCNEntity = source.FirstOrDefault((LCNEntity p) => p.ServiceID == program.ServiceId);
						if (lCNEntity == null)
						{
							stringBuilder.Append("");
							stringBuilder.Append(",");
						}
						else
						{
							stringBuilder.Append(lCNEntity.LCNumber);
							stringBuilder.Append(",");
						}
						stringBuilder.Append(program.EITFileName);
						stringBuilder.Append(",");
						list.Add(stringBuilder);
					}
				}
			}
			return list;
		}

		private List<LCNEntity> ToLcnEntitys(DescriptorSigleBase lcndescriptor, LcnType _LcnType)
		{
			List<LCNEntity> list = new List<LCNEntity>();
			if (lcndescriptor == null)
			{
				return list;
			}
			short num = 0;
			bool flag = false;
			short num2 = 0;
			byte b = 0;
			byte b2 = 0;
			LCNEntity lCNEntity = null;
			int num3 = 0;
			while (num3 < lcndescriptor.DataLength)
			{
				num = Tool.ResolveInt16(Tool.ResolveByte(lcndescriptor.DescriptionData, num3, 2), NetWorkEnum.NetWorkToHost);
				b = Tool.ResolveByte(lcndescriptor.DescriptionData, num3 + 2, 1)[0];
				b2 = Tool.ResolveByte(lcndescriptor.DescriptionData, num3 + 3, 1)[0];
				flag = ((b >> 7) & 1) == 1;
				num2 = ((_LcnType != LcnType.BIT_10) ? ((short)(((b & 0x3F) << 8) + b2)) : ((short)(((b & 3) << 8) + b2)));
				num3 += 4;
				lCNEntity = new LCNEntity(num, flag, num2, "");
				list.Add(lCNEntity);
			}
			return list;
		}
	}
}
