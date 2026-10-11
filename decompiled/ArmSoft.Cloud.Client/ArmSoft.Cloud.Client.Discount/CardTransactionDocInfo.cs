using System;
using Newtonsoft.Json;

namespace ArmSoft.Cloud.Client.Discount;

public class CardTransactionDocInfo
{
	public int DbId { get; set; }

	public string FingerPrint { get; set; }

	public string User { get; set; }

	public Guid ISN { get; set; }

	[JsonConverter(typeof(OnlyDateConverter))]
	public DateTime Date { get; set; }

	public DocType Type { get; set; }

	public string Number { get; set; }

	public decimal Amount { get; set; }

	public string CashCode { get; set; }

	public string ShiftNumber { get; set; }
}
