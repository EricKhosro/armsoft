namespace ArmSoft.InternetBank.Web.API.Common;

public class BondScheduleParams
{
	public string ContractNumber { get; set; }

	public int? Count { get; set; }

	public BondScheduleParams()
	{
		Count = null;
	}
}
