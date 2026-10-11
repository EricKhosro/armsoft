using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class PaymentState
{
	public DocumentState State { get; set; }

	public string Comments { get; set; }
}
