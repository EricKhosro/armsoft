namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AccountInfo
{
	public string Bank { get; set; }

	public string OwnerName { get; set; }

	public bool IsReceiverLegalStatusRequired { get; set; }
}
