using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("3C90E925-251C-4F59-8510-320E1E7A11C6")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTMove : ExchangeBase
{
	public string DocumentNumber { get; set; } = string.Empty;

	public DateTime DocumentDate { get; set; }

	public string Comment { get; set; } = string.Empty;

	public string StorageExpense { get; set; } = string.Empty;

	public string StorageIncome { get; set; } = string.Empty;

	public string Chief { get; set; } = string.Empty;

	public string Mediator { get; set; } = string.Empty;

	public string Allower { get; set; } = string.Empty;

	public string BookNumber { get; set; } = string.Empty;

	public int BookPage { get; set; }

	public int BookRow { get; set; }

	public string TaxExportType { get; set; } = string.Empty;

	public string TaxInvoiceSeries { get; set; } = string.Empty;

	public string TaxInvoiceNumber { get; set; } = string.Empty;

	public DateTime? TaxInvoiceDate { get; set; }

	public string TransportationMethod { get; set; } = string.Empty;

	public string PriceType { get; set; } = string.Empty;

	public string BaseISN { get; set; } = string.Empty;

	public List<MTMoveSpecificationRow> Specification { get; set; }

	public string ISN { get; set; } = string.Empty;

	public MTMoveSpecificationRow AddRow()
	{
		if (Specification == null)
		{
			Specification = new List<MTMoveSpecificationRow>();
		}
		MTMoveSpecificationRow mTMoveSpecificationRow = new MTMoveSpecificationRow();
		Specification.Add(mTMoveSpecificationRow);
		return mTMoveSpecificationRow;
	}
}
