namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class Storage : TradeDirectory
{
	public string Response { get; set; }

	public string Address { get; set; }

	public bool IsRetail { get; set; }

	public bool IsDefault { get; set; }

	public string Body { get; set; }

	public bool StorageExistsInOtherDocs { get; set; }
}
