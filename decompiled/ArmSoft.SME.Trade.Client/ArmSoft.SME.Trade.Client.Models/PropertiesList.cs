namespace ArmSoft.SME.Trade.Client.Models;

public class PropertiesList
{
	public class PropertyListRowKey
	{
		public string DestCode { get; set; }

		public string Code { get; set; }
	}

	public string DestCode { get; set; }

	public string Code { get; set; }

	public string Caption { get; set; }

	public byte[] TS { get; set; }
}
