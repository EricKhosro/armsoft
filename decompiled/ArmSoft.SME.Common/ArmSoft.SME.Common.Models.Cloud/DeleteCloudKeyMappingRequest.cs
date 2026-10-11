namespace ArmSoft.SME.Common.Models.Cloud;

public class DeleteCloudKeyMappingRequest
{
	public string AdToken { get; set; }

	public string Key { get; set; }

	public int UserId { get; set; }
}
