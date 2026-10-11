namespace ArmSoft.Cloud.Client.AS7X;

public class CloudPublicApiKeyInfo
{
	public int Id { get; set; }

	public string Description { get; set; }

	public string SecretHint { get; set; }

	public bool IsEnabled { get; set; }

	public int UserID { get; set; }

	public string UserName { get; set; }

	public ApiType ApiType { get; set; }
}
