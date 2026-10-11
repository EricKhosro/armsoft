namespace ArmSoft.SME.Common.Models.Cloud;

public class EditCloudKeyMappingRequest
{
	public string AdToken { get; set; }

	public string Key { get; set; }

	public int UserId { get; set; }

	public bool IsAdmin { get; set; }

	public bool LoginDisabled { get; set; }

	public bool IsDBAdmin { get; set; }
}
