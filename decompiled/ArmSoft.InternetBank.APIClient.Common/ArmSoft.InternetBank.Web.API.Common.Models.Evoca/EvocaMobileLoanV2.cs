using System;
using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models.Evoca;

public class EvocaMobileLoanV2
{
	public IEnumerable<EvocaMobileLoanSettingsV2> Settings { get; set; }

	public IEnumerable<EvocaMobileLoanCurrency> Currencies { get; set; }

	public bool HasApplication { get; set; }

	public Guid ApplicationId { get; set; }

	public EvocaMobileLoanV2()
	{
		HasApplication = false;
	}
}
