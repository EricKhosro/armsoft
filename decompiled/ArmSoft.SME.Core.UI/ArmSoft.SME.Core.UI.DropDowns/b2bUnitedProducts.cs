using System.Collections.Generic;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Client.Models.b2b;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViews;

namespace ArmSoft.SME.Core.UI.DropDowns;

[DropDown(7)]
public class b2bUnitedProducts : DropDownView<b2bUnitedProductRowBase, b2bUnitedProductsDataProvider.Param>
{
	public override string Caption => Resources.BarCodes;

	public b2bUnitedProducts()
	{
	}

	public b2bUnitedProducts(List<b2bUnitedProductRowBase> rows)
	{
		DataProvider = new b2bUnitedProductsDataProvider(rows);
		FillColumnsFromDataProvider();
	}
}
