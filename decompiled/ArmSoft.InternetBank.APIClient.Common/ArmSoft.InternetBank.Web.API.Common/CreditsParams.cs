using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class CreditsParams
{
	public CreditAccessLevels DesiredAccess { get; set; }

	public List<CreditType> CreditTypes { get; set; }

	public CreditsParams()
	{
		DesiredAccess = CreditAccessLevels.View;
		CreditTypes = null;
	}
}
