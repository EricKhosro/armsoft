namespace ArmSoft.SME.Core.AddIn.Client.Models;

public class CloudUserExistenceRequest
{
	public string AdToken { get; set; }

	public int UserId { get; set; }

	public string KeySerial { get; set; }
}
