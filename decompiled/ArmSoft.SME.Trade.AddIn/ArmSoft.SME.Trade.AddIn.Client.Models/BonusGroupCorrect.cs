using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("1F833A26-74AC-45C9-AA4F-23606E7897A7")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class BonusGroupCorrect : ExchangeBase
{
	public DateTime DocumentDate { get; set; }

	public string DocumentNumber { get; set; }

	public string Comment { get; set; } = string.Empty;

	public string CashDesk { get; set; } = string.Empty;

	public string CashierShiftNumber { get; set; } = string.Empty;

	public List<BonusSpecificationRow> Specification { get; set; }

	public BonusSpecificationRow AddRow()
	{
		if (Specification == null)
		{
			Specification = new List<BonusSpecificationRow>();
		}
		BonusSpecificationRow bonusSpecificationRow = new BonusSpecificationRow();
		Specification.Add(bonusSpecificationRow);
		return bonusSpecificationRow;
	}
}
