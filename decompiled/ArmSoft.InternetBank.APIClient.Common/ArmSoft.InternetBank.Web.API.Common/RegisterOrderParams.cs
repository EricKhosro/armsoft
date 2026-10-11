namespace ArmSoft.InternetBank.Web.API.Common;

public class RegisterOrderParams
{
	public decimal Amount { get; set; }

	public string Currency { get; set; }

	public int AttachedCardId { get; set; }
}
