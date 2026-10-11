using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class PaymentGroupItem
{
	public string Code { get; set; }

	public LocalizableString Title { get; set; }

	public string IconUrl { get; set; }

	public string Url { get; set; }

	public LocalizableString SearchFieldName { get; set; }

	public PaymentGroupRowType Type { get; set; }

	public UtilityType UtilityType { get; set; }

	public bool IsNew { get; set; }

	public bool IsContainNewItem { get; set; }

	public PaymentGroupItem()
	{
		UtilityType = UtilityType.Unknown;
	}
}
