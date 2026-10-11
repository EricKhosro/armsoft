namespace ArmSoft.InternetBank.Web.API.Common;

public class DepositScheduleParams
{
	public string ContractNumber { get; set; }

	public int? Count { get; set; }

	public DepositScheduleParams()
	{
		Count = null;
	}
}
