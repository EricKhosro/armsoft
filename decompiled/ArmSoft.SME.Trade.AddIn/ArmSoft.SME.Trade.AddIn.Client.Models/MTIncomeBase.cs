using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
public class MTIncomeBase : ExchangeBase
{
	public DateTime DocumentDate { get; set; }

	public string DocumentNumber { get; set; } = string.Empty;

	public string Storage { get; set; } = string.Empty;

	public string CurrencyCode { get; set; } = string.Empty;

	public decimal CurrencyExchangeRate { get; set; }

	public decimal CurrencyExchangeBase { get; set; }

	public string VATType { get; set; } = string.Empty;

	public bool IncludeVATInCost { get; set; }

	public string PriceType { get; set; } = string.Empty;

	public string PartnerCode { get; set; } = string.Empty;

	public string Contract { get; set; } = string.Empty;

	public decimal PartnerAmount { get; set; }

	public string InvoiceNumber { get; set; } = string.Empty;

	public DateTime? InvoiceDate { get; set; }

	public string Comment { get; set; } = string.Empty;

	public string BaseISN { get; set; } = string.Empty;

	public string Trust { get; set; } = string.Empty;

	public string Mediator { get; set; } = string.Empty;

	public string MediatorCaption { get; set; } = string.Empty;

	public string Machine { get; set; } = string.Empty;

	public string ChiefAccountant { get; set; } = string.Empty;

	public string Allower { get; set; } = string.Empty;

	public string Response { get; set; } = string.Empty;

	public decimal DiscountPercent { get; set; }

	public string ISN { get; set; } = string.Empty;

	public List<MTIncomeSpecificationRow> Specification { get; set; }
}
