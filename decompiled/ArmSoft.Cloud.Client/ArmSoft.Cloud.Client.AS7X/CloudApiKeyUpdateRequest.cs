namespace ArmSoft.Cloud.Client.AS7X;

public class CloudApiKeyUpdateRequest
{
	public int Id { get; set; }

	public int UserID { get; set; }

	public string Description { get; set; }

	public bool IsEnabled { get; set; }
}
