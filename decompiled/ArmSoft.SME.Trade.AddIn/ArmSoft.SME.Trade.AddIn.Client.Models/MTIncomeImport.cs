using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("2F3DAA2C-7EEE-4FEB-ABC5-26F2C9FCD504")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTIncomeImport : ExchangeBase
{
	public DateTime DocumentDate { get; set; }

	public string DocumentNumber { get; set; } = string.Empty;

	public string Storage { get; set; } = string.Empty;

	public string CurrencyCode { get; set; } = string.Empty;

	public decimal CurrencyExchangeRate { get; set; }

	public decimal CurrencyExchangeBase { get; set; }

	public string PriceType { get; set; } = string.Empty;

	public string PartnerCode { get; set; } = string.Empty;

	public string Contract { get; set; } = string.Empty;

	public decimal PartnerAmount { get; set; }

	public string CustomPartnerCode { get; set; } = string.Empty;

	public string CustomContract { get; set; } = string.Empty;

	public decimal CustomPartnerAmount { get; set; }

	public decimal CustomFeeAmount { get; set; }

	public string InvoiceNumber { get; set; } = string.Empty;

	public string Comment { get; set; } = string.Empty;

	public string Trust { get; set; } = string.Empty;

	public string Mediator { get; set; } = string.Empty;

	public string Machine { get; set; } = string.Empty;

	public string ChiefAccountant { get; set; } = string.Empty;

	public string Allower { get; set; } = string.Empty;

	public string Response { get; set; } = string.Empty;

	public string ISN { get; set; } = string.Empty;

	public List<MTIncomeImportSpecificationRow> Specification { get; set; }

	public MTIncomeImportSpecificationRow AddRow()
	{
		if (Specification == null)
		{
			Specification = new List<MTIncomeImportSpecificationRow>();
		}
		MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow = new MTIncomeImportSpecificationRow();
		Specification.Add(mTIncomeImportSpecificationRow);
		return mTIncomeImportSpecificationRow;
	}
}
