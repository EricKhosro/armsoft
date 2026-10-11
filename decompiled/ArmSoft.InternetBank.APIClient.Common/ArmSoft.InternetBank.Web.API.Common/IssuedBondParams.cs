using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class IssuedBondParams
{
	public string Currency { get; set; }

	public BondIssuanceType IssuedBondType { get; set; }

	public IssuedBondParams()
	{
		Currency = string.Empty;
		IssuedBondType = BondIssuanceType.NotSet;
	}
}
