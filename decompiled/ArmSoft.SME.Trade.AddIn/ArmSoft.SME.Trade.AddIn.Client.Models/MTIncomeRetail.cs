using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("D2BF95AC-EDEF-47B6-B078-5B0FDCCC5E8E")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTIncomeRetail : MTIncomeBase
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
