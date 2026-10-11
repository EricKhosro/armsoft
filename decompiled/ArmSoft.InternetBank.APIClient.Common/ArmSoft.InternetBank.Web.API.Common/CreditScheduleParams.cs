namespace ArmSoft.InternetBank.Web.API.Common;

public class CreditScheduleParams
{
	public string ContractNumber { get; set; }

	public int? Count { get; set; }

	public CreditScheduleParams()
	{
		Count = null;
	}
}
