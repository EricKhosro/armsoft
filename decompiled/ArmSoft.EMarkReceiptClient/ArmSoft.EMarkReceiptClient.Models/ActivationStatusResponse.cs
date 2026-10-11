namespace ArmSoft.EMarkReceiptClient.Models;

public class ActivationStatusResponse
{
	public bool activated { get; set; }

	public string token { get; set; }

	public string crn { get; set; } = string.Empty;

	public string refresh_token { get; set; }
}
