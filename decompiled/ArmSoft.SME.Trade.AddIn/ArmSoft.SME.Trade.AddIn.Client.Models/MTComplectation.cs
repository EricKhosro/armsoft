using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("286DE915-7321-4879-ACC1-726F54BD7A94")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTComplectation : ExchangeBase
{
	public string DocumentNumber { get; set; } = string.Empty;

	public DateTime DocumentDate { get; set; }

	public string MTComplectCode { get; set; } = string.Empty;

	public string QuantityUnit { get; set; } = string.Empty;

	public string StorageExpense { get; set; } = string.Empty;

	public string StorageIncome { get; set; } = string.Empty;

	public string Comment { get; set; } = string.Empty;

	public decimal Quantity { get; set; }

	public List<QuantityUnitPart> QuantityUnitList { get; set; }

	public DateTime? UseFulLife { get; set; }

	public string Country { get; set; } = string.Empty;

	public string Producer { get; set; } = string.Empty;

	public string Property1 { get; set; } = string.Empty;

	public string Property2 { get; set; } = string.Empty;

	public List<MTComplectationSpecificationRow> Specification { get; set; }

	public string ISN { get; set; } = string.Empty;

	public MTComplectationSpecificationRow AddRow()
	{
		if (Specification == null)
		{
			Specification = new List<MTComplectationSpecificationRow>();
		}
		MTComplectationSpecificationRow mTComplectationSpecificationRow = new MTComplectationSpecificationRow();
		Specification.Add(mTComplectationSpecificationRow);
		return mTComplectationSpecificationRow;
	}
}
