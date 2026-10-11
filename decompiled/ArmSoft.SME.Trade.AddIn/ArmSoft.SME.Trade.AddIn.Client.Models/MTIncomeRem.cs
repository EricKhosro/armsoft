using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("8909FC97-8537-4814-A230-73DA9B815000")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTIncomeRem : ExchangeBase
{
	public DateTime DocumentDate { get; set; }

	public string DocumentNumber { get; set; } = string.Empty;

	public string Storage { get; set; } = string.Empty;

	public string PartnerCode { get; set; } = string.Empty;

	public string Contract { get; set; } = string.Empty;

	public string Comment { get; set; } = string.Empty;

	public List<MTIncomeRemSpecificationRow> Specification { get; set; }

	public MTIncomeRemSpecificationRow AddRow()
	{
		if (Specification == null)
		{
			Specification = new List<MTIncomeRemSpecificationRow>();
		}
		MTIncomeRemSpecificationRow mTIncomeRemSpecificationRow = new MTIncomeRemSpecificationRow();
		Specification.Add(mTIncomeRemSpecificationRow);
		return mTIncomeRemSpecificationRow;
	}
}
