using System.ComponentModel;

namespace ArmSoft.OpenBanking.Api.Common;

public class GetBankParams
{
	[DefaultValue("")]
	public required string Code { get; set; }
}
