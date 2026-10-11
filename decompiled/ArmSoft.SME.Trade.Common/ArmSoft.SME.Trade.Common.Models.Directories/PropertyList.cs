namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class PropertyList : TradeDirectory
{
	public class PropertyListRowKey
	{
		public string DestCode { get; set; }

		public string Code { get; set; }
	}

	public string DestCode { get; set; }
}
