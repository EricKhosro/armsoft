using System.ComponentModel;

namespace ArmSoft.OpenBanking.Api.Common;

public class GetCardParams
{
	[DefaultValue(true)]
	public bool IncludeAccountBalances { get; set; }
}
