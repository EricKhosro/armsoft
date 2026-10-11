using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Core.UI.InternetBank;

public class IBWGPayment
{
	public DateTime Date { get; set; }

	public decimal Amount { get; set; }

	public string Currency { get; set; }

	public string Details { get; set; }

	public Guid DocId { get; set; }

	public byte DocState { get; set; }

	public short DocType { get; set; }

	public string DocNum { get; set; }

	public string PayerAcc { get; set; }

	public string TaxCode { get; set; }

	public string SocialCard { get; set; }

	public bool IsManuallyEntered { get; set; }

	public List<IBWGPaymentRow> Rows { get; set; } = new List<IBWGPaymentRow>();
}
