namespace ArmSoft.Cloud.Client.AS7X;

public class CloudPublicApiKeyCreationRequest
{
	public int DbId { get; set; }

	public int UserID { get; set; }

	public string Description { get; set; }

	public bool IsEnabled { get; set; }

	public ApiType ApiType { get; set; }
}
