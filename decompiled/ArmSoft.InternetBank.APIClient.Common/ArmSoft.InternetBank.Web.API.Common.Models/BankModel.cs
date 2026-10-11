using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class BankModel
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string EName { get; set; }

	public BankAccountFormat Format { get; set; }

	public bool AllowTransfers { get; set; }
}
