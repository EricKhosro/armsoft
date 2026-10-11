using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class PB : Application
{
	public string PayerAcc { get; set; }

	public string PercentAcc { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public decimal Amount { get; set; }

	public string Currency { get; set; }

	public decimal FixedRate { get; set; }

	public string IssuanceNumber { get; set; }

	public int BondsCount { get; set; }

	public decimal UnitPrice { get; set; }

	public decimal NominalValue { get; set; }

	public StatementSendingType StatementSendingType { get; set; }

	public IssuedBond CurrentIssuedBond { get; set; }
}
