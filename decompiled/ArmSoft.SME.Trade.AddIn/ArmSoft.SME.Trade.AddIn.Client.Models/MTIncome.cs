using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("B8222FB5-A2F5-4705-B795-703160C3BDA2")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTIncome : MTIncomeBase
{
	public MTIncomeSpecificationRow AddRow()
	{
		if (Specification == null)
		{
			Specification = new List<MTIncomeSpecificationRow>();
		}
		MTIncomeSpecificationRow mTIncomeSpecificationRow = new MTIncomeSpecificationRow();
		Specification.Add(mTIncomeSpecificationRow);
		return mTIncomeSpecificationRow;
	}
}
