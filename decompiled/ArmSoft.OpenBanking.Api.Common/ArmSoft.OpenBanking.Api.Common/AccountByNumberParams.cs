using System.ComponentModel;

namespace ArmSoft.OpenBanking.Api.Common;

public class AccountByNumberParams
{
	[DefaultValue("")]
	public required string Number { get; set; }
}
