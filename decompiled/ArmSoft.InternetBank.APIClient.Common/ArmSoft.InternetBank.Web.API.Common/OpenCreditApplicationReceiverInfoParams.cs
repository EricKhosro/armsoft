using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class OpenCreditApplicationReceiverInfoParams
{
	public ClientStatus ReceiverStatus { get; set; }

	public string ReceiverInfo { get; set; }
}
