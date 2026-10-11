namespace ArmSoft.SME.Common.Models.Cloud;

public class CloudDBUserLogCreationRequest
{
	public string AdToken { get; set; }

	public int? DatabaseId { get; set; }

	public string Key { get; set; }

	public int? UserId { get; set; }

	public string ASUser { get; set; }

	public CloudDBUserLogAction Action { get; set; }

	public int ProcessId { get; set; }

	public string Station { get; set; }
}
