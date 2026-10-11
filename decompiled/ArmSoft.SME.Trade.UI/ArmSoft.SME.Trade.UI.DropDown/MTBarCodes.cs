using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class MTBarCodes : DropDownView<MTBarCodeDataProvider.DataRow, MTBarCodeDataProvider.Param>
{
	public override string Caption => Resources.BarCodes;

	public override string HelpId => "Purchases_Lists_Productsandservicessublist_ProductsandservicesBarCodes";

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public override bool KeyMemberSupported => true;

	public bool ShowBarCodeFirst { get; set; }

	public bool IsShowQntUnitCaption { get; set; }

	public bool IsShowOnlyInclInPriceList { get; set; }

	public bool ShowAlsoMTQntUnitsEmptyBarCodes { get; set; }

	public MTBarCodes(bool isShowQntUnitCaption, bool showBarCodeFirst, bool isShowOnlyInclInPriceList = false, bool showAlsoMTQntUnitsEmptyBarCodes = false)
	{
		IsShowQntUnitCaption = isShowQntUnitCaption;
		ShowBarCodeFirst = showBarCodeFirst;
		IsShowOnlyInclInPriceList = isShowOnlyInclInPriceList;
		ShowAlsoMTQntUnitsEmptyBarCodes = showAlsoMTQntUnitsEmptyBarCodes;
		DataProvider = new MTBarCodeDataProvider();
		FillColumnsFromDataProvider();
		if (showBarCodeFirst)
		{
			Columns.Move("BarCode", 0);
		}
		if (!isShowOnlyInclInPriceList)
		{
			Columns["MTShowInPriceList"].Visible = false;
		}
		if (!showBarCodeFirst)
		{
			Columns.AddSorting("MTCode");
		}
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.IsShowOnlyInclInPriceList = IsShowOnlyInclInPriceList;
		ParametersProvider.ShowAlsoMTQntUnitsEmptyBarCodes = ShowAlsoMTQntUnitsEmptyBarCodes;
	}

	public override short ValueMemberLength()
	{
		if (!ShowBarCodeFirst)
		{
			return 3;
		}
		return 20;
	}

	public override string ValueMember(int index)
	{
		if (!ShowBarCodeFirst)
		{
			return Data[index].QntUnit;
		}
		return Data[index].BarCode;
	}

	public override string CommentMember(int index)
	{
		if (ShowBarCodeFirst)
		{
			return Data[index].MTCode;
		}
		if (!IsShowQntUnitCaption)
		{
			return Data[index].QntUnitBrief;
		}
		return Data[index].QntUnitCaption;
	}

	public override object KeyMember(int index)
	{
		return Data[index].BarCode;
	}
}
