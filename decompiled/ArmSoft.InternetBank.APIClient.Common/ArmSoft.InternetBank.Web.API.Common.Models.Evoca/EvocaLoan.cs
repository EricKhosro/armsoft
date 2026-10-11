using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models.Evoca;

public class EvocaLoan
{
	public int Id { get; set; }

	public string Text { get; set; }

	public bool IsOverdraft { get; set; }

	public string TermsURL { get; set; }

	public string AgreementsURL { get; set; }

	public List<EvocaLoanData> Types { get; set; }
}
