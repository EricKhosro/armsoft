using System.Collections.Generic;

namespace ArmSoft.OpenBanking.Api.Common;

public class MultiplePayment : AccountPayment
{
	public List<MultiplePaymentRow> Rows { get; set; } = new List<MultiplePaymentRow>();

	public string TaxCode { get; set; } = string.Empty;

	public string SocialCard { get; set; } = string.Empty;
}
