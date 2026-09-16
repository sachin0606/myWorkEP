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
	public class BATInfoManager
	{
		private AbstractFactory db = AbstractFactory.CreateFactory();

		public IList<BATInfo> GetByNetworkIdentity(long NetworkIdentity)
		{
			return db.CreateBATInfoDAL().GetByNetworkIdentity(NetworkIdentity);
		}

		public BATInfo GetByIdentity(long Identity)
		{
			return db.CreateBATInfoDAL().GetByIdentity(Identity);
		}

		public void Save(BATToProgram BATToPrgList, bool updateServiceList = true)
		{
			db.CreateBATInfoDAL().Save(BATToPrgList, updateServiceList);
		}

		public void Delete(IList<long> ListIdentity)
		{
			db.CreateBATInfoDAL().Delete(ListIdentity);
		}

		public BATInfo GetByBATId(long NetworkIdentity, int BATId)
		{
			return db.CreateBATInfoDAL().GetByBATId(NetworkIdentity, BATId);
		}

		public IList<BATInfo> GetOutputBAT()
		{
			return db.CreateBATInfoDAL().GetOutputBAT();
		}

		public List<StringBuilder> GetBouquetConfiguration()
		{
			NITInfoManager nITInfoManager = new NITInfoManager();
			BATInfoManager bATInfoManager = new BATInfoManager();
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
				stringBuilder.Append("Bouquet ID");
				stringBuilder.Append(",");
				stringBuilder.Append("Bouquet Name");
				stringBuilder.Append(",");
				stringBuilder.Append("TS ID");
				stringBuilder.Append(",");
				stringBuilder.Append("TS Name");
				stringBuilder.Append(",");
				stringBuilder.Append("Service ID");
				stringBuilder.Append(",");
				stringBuilder.Append("Channel Name");
				list.Add(stringBuilder);
				IList<TsInfo> tSInfoByNetWorkIdentity = tSInfoManager.GetTSInfoByNetWorkIdentity(item.Identity);
				IList<BATInfo> byNetworkIdentity = bATInfoManager.GetByNetworkIdentity(item.Identity);
				foreach (BATInfo item2 in byNetworkIdentity)
				{
					IList<DescriptorSigleBase> bAT_TS_DescriptorsByBATIDEntity = descriptorManagement.GetBAT_TS_DescriptorsByBATIDEntity(item2.Identity);
					TsInfo ts;
					foreach (TsInfo item3 in tSInfoByNetWorkIdentity)
					{
						ts = item3;
						DescriptorSigleBase servicelistdescriptor = bAT_TS_DescriptorsByBATIDEntity.FirstOrDefault((DescriptorSigleBase p) => p.TSTableID == ts.Identity && p.DesTypeTag == 65);
						List<Servicve_ID_Type> list2 = ToServicve_ID_Type(servicelistdescriptor);
						foreach (Servicve_ID_Type item4 in list2)
						{
							stringBuilder = new StringBuilder();
							stringBuilder.Append(item2.BATId);
							stringBuilder.Append(",");
							stringBuilder.Append(item2.BATName);
							stringBuilder.Append(",");
							stringBuilder.Append(ts.TsId);
							stringBuilder.Append(",");
							stringBuilder.Append(ts.TsName);
							stringBuilder.Append(",");
							stringBuilder.Append(item4.ProgramID);
							stringBuilder.Append(",");
							stringBuilder.Append(item4.ProgramName);
							list.Add(stringBuilder);
						}
					}
				}
			}
			return list;
		}

		private List<Servicve_ID_Type> ToServicve_ID_Type(DescriptorSigleBase servicelistdescriptor)
		{
			List<Servicve_ID_Type> list = new List<Servicve_ID_Type>();
			if (servicelistdescriptor == null)
			{
				return list;
			}
			ushort num = 0;
			byte b = 0;
			Servicve_ID_Type servicve_ID_Type = null;
			for (int i = 0; i < servicelistdescriptor.DataLength; i += 3)
			{
				num = (ushort)Tool.ResolveInt32(Tool.ResolveByte(servicelistdescriptor.DescriptionData, i, 2), NetWorkEnum.NetWorkToHost);
				b = Tool.ResolveByte(servicelistdescriptor.DescriptionData, i + 2, 1)[0];
				servicve_ID_Type = new Servicve_ID_Type(0L, num, "", b, servicelistdescriptor.TableID);
				list.Add(servicve_ID_Type);
			}
			return list;
		}
	}
}
