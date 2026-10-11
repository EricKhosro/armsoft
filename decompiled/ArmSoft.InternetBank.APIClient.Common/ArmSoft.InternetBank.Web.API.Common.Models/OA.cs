namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class OA : Application
{
	public string Currency { get; set; }

	public int? AccountTypeId { get; set; }

	public string DisplayAccountTypeName { get; set; }

	public string Goal { get; set; }

	public decimal? Amount { get; set; }

	public bool IsGoalAccount { get; set; }

	public string BranchCode { get; set; }
}
