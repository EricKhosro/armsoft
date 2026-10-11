using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("A1009786-0EAE-4D9E-81D8-884D69F96C12")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTDecomplectation : ExchangeBase
{
	public string DocumentNumber { get; set; } = string.Empty;

	public DateTime DocumentDate { get; set; }

	public string StorageExpense { get; set; } = string.Empty;

	public string StorageIncome { get; set; } = string.Empty;

	public string MTComplectCode { get; set; } = string.Empty;

	public string QuantityUnit { get; set; } = string.Empty;

	public decimal Quantity { get; set; }

	public string Comment { get; set; } = string.Empty;

	public bool PartyMethod { get; set; }

	public List<MTDecomplectationSpecificationRow> Specification { get; set; }

	public MTDecomplectationSpecificationRow AddRow()
	{
		if (Specification == null)
		{
			Specification = new List<MTDecomplectationSpecificationRow>();
		}
		MTDecomplectationSpecificationRow mTDecomplectationSpecificationRow = new MTDecomplectationSpecificationRow();
		Specification.Add(mTDecomplectationSpecificationRow);
		return mTDecomplectationSpecificationRow;
	}
}
