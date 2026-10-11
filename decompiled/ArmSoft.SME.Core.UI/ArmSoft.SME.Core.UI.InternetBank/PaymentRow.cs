using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Core.UI.InternetBank;

public class PaymentRow
{
	public decimal Amount { get; set; }

	public bool CanBeSigned { get; set; }

	public string Comments { get; set; }

	public bool CreatedByAdmin { get; set; }

	public string Currency { get; set; }

	public DateOnly Date { get; set; }

	public string Details { get; set; }

	public string DisplayCancelRequestState { get; set; }

	public string StateWithText { get; set; }

	public string DisplayState { get; set; }

	public string DisplaySubState { get; set; }

	public string DisplayType { get; set; }

	public Guid DocId { get; set; }

	public string DocIdString { get; set; }

	public string DocNum { get; set; }

	public string PayerAccOrCard { get; set; }

	public string Receiver { get; set; }

	public string ReceiverAccOrCard { get; set; }

	public byte State { get; set; }

	public string Template { get; set; }

	public byte[] TimeStamp { get; set; }

	public string Type { get; set; }

	public string UtilityCode { get; set; }

	public List<Link> Links { get; set; }
}
