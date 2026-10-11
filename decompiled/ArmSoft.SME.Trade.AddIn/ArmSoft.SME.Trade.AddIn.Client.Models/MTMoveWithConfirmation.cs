using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("CB7CCA85-6989-4308-A0E2-D6FA831A1651")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTMoveWithConfirmation : ExchangeBase
{
	[ComVisible(true)]
	[Guid("5CCF3BF0-1721-4BF9-8F9C-FDE8DEDCCA06")]
	[ClassInterface(ClassInterfaceType.AutoDual)]
	public class MTMoveSpecificationRow
	{
		public string MTCode { get; set; } = "";

		public string Unit { get; set; } = "";

		public decimal Quantity { get; set; }

		public decimal ActualQuantity { get; set; }

		public decimal SalePrice { get; set; }

		public decimal SaleAmount { get; set; }

		public string PartyMethod { get; set; } = "";

		public string RowDescription { get; set; } = "";

		public List<QuantityUnitPart> UnitList { get; set; }
	}

	public string DocumentNumber { get; set; } = "";

	public DateTime DocumentDate { get; set; }

	public string Comment { get; set; } = "";

	public string StorageExpense { get; set; } = "";

	public string StorageIncome { get; set; } = "";

	public string Chief { get; set; } = "";

	public string Mediator { get; set; } = "";

	public string Allower { get; set; } = "";

	public string BookNumber { get; set; } = "";

	public int BookPage { get; set; }

	public int BookRow { get; set; }

	public string TaxExportType { get; set; } = "";

	public string TaxInvoiceSeries { get; set; } = "";

	public string TaxInvoiceNumber { get; set; } = "";

	public DateTime? TaxInvoiceDate { get; set; }

	public string TransportationMethod { get; set; } = "";

	public string PriceType { get; set; } = "";

	public string BaseISN { get; set; } = "";

	public bool Confirmed { get; set; }

	public List<MTMoveSpecificationRow> Specification { get; set; }

	public string ISN { get; set; } = "";

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
