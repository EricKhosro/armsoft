using System.Collections.Generic;

namespace ArmSoft.OpenBanking.Api.Common;

public class ContractOnlineDataListResult<TOnlineData> where TOnlineData : BaseContractOnlineData
{
	public required IEnumerable<TOnlineData> Contracts { get; set; }

	public List<CurrencyAmountSummary>? Summary { get; set; }
}
