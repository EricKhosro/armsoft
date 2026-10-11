using System.Collections.Generic;

namespace ArmSoft.Cloud.Client.Discount;

public class CardTransactionRequest
{
	public string CardNumber { get; set; }

	public CardTransactionRequestDocInfo DocInfo { get; set; }

	public List<CardTransactionRow> Rows { get; set; }
}
