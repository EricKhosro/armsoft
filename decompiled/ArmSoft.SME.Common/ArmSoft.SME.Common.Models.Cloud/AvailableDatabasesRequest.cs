namespace ArmSoft.SME.Common.Models.Cloud;

public class AvailableDatabasesRequest
{
	public string AdToken { get; set; }

	public short? ContractType { get; set; }

	public int? ServerId { get; set; }
}
