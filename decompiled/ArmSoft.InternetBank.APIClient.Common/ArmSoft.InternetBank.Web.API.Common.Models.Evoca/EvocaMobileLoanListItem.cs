using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Evoca;

namespace ArmSoft.InternetBank.Web.API.Common.Models.Evoca;

public class EvocaMobileLoanListItem
{
	public int LoanTypeId { get; set; }

	public string Name { get; set; }

	public string Icon { get; set; }

	public string TermURL { get; set; }

	public string AgreementURL { get; set; }

	public EvocaMobileLoanType LoanType { get; set; }

	public IEnumerable<EvocaMobileLoanSettingsInformation> Settings { get; set; }
}
