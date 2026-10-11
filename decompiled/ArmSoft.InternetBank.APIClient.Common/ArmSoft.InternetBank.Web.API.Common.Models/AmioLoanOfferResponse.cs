using System;
using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AmioLoanOfferResponse
{
	public string OfferId { get; set; }

	public string SecuredOfferKey { get; set; }

	public int MinTerm { get; set; }

	public int MaxTerm { get; set; }

	public decimal MinAmount { get; set; }

	public decimal MaxAmount { get; set; }

	public string Currency { get; set; }

	public string ProductTitle { get; set; }

	public string ProductLoanEngine { get; set; }

	public decimal InterestRateExpression { get; set; }

	public string ProcuringType { get; set; }

	public List<AmioLoanAccountInformation> Accounts { get; set; }

	public DateTime DefaultFirstPaymentDate { get; set; }

	public DateTime OfferValidDate { get; set; }
}
