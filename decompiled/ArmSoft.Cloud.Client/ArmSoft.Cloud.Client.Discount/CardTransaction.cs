using System;
using System.Collections.Generic;

namespace ArmSoft.Cloud.Client.Discount;

public class CardTransaction
{
	public Guid ISN { get; set; }

	public string CardNumber { get; set; }

	public DateTime Date { get; set; }

	public bool Deleted { get; set; }

	public CardTransactionDocInfo DocInfo { get; set; }

	public List<CardTransactionRow> Rows { get; set; }
}
