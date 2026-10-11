using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("451AB1E6-F016-4118-9737-A26507ECA7D6")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTMoveOrder : ExchangeBase
{
	public string DocumentNumber { get; set; } = string.Empty;

	public DateTime DocumentDate { get; set; }

	public DateTime SupplyDate { get; set; }

	public string Comment { get; set; } = string.Empty;

	public bool IsMade { get; set; }

	public string StorageExpense { get; set; } = string.Empty;

	public string StorageIncome { get; set; } = string.Empty;

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
