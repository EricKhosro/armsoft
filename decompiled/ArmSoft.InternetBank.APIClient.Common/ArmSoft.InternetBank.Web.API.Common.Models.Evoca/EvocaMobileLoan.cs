using System;
using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models.Evoca;

public class EvocaMobileLoan
{
	public IEnumerable<EvocaMobileLoanSettings> Settings { get; set; }

	public IEnumerable<EvocaMobileLoanCurrency> Currencies { get; set; }

	public bool HasApplication { get; set; }

	public Guid ApplicationId { get; set; }

	public EvocaMobileLoan()
	{
		HasApplication = false;
	}
}
