using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("5BC6FAE4-D9CF-4C28-B114-492B660C1914")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class ReceivedService : ExchangeBase
{
	public DateTime DocumentDate { get; set; }

	public string DocumentNumber { get; set; } = string.Empty;

	public string CurrencyCode { get; set; } = string.Empty;

	public decimal CurrencyExchangeRate { get; set; }

	public decimal CurrencyExchangeBase { get; set; }

	public string PartnerCode { get; set; } = string.Empty;

	public string Contract { get; set; } = string.Empty;

	public decimal PartnerAmount { get; set; }

	public string PriceType { get; set; } = string.Empty;

	public string InvoiceNumber { get; set; } = string.Empty;

	public string Comment { get; set; } = string.Empty;

	public string VATType { get; set; } = string.Empty;

	public List<ReceivedServiceSpecificationRow> Specification { get; set; }

	public ReceivedServiceSpecificationRow AddRow()
	{
		if (Specification == null)
		{
			Specification = new List<ReceivedServiceSpecificationRow>();
		}
		ReceivedServiceSpecificationRow receivedServiceSpecificationRow = new ReceivedServiceSpecificationRow();
		Specification.Add(receivedServiceSpecificationRow);
		return receivedServiceSpecificationRow;
	}
}
