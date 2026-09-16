namespace DeXin.EPG.Model
{
	public class LCNEntity
	{
		public bool Visible_service_flag { get; set; }

		public short ServiceID { get; set; }

		public string ServiceName { get; set; }

		public short LCNumber { get; set; }

		public LCNEntity(short serviceID, bool visible_service_flag, short lcn, string serviceName)
		{
			ServiceID = serviceID;
			LCNumber = lcn;
			Visible_service_flag = visible_service_flag;
			ServiceName = serviceName;
		}
	}
}
