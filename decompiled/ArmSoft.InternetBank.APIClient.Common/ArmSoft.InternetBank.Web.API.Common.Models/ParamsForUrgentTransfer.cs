namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class ParamsForUrgentTransfer
{
	public bool AllowUrgentTransfers { get; set; }

	public bool AllowUrgentBudgetaryTransfers { get; set; }

	public string UrgentTransferStartTime { get; set; }

	public string UrgentTransferEndTime { get; set; }

	public bool IsUrgentTransferAllowedNow { get; set; }

	public string UrgentTransferDescription { get; set; }
}
