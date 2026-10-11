using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.Parameters;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class PriceTypesExtended : DropDownView<PriceTypesDataProvider.DataRow, PriceTypesDataProvider.Param>
{
	private bool _showPrimeCostRows;

	private bool _showPurchasePriceRow;

	private bool _showSalePriceRow;

	private bool _showRetailAvailabilityPriceRow;

	private bool _showDiscountPriceRow;

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public PriceTypesExtended(bool showPrimeCostRows = false, bool showPurchasePriceRow = false, bool showSalePriceRow = false, bool showRetailAvailabilityPriceRow = false, bool showDiscountPriceRow = false)
	{
		DataProvider = new PriceTypesDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
		_showDiscountPriceRow = showDiscountPriceRow;
		_showPrimeCostRows = showPrimeCostRows && Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
		_showPurchasePriceRow = showPurchasePriceRow;
		_showRetailAvailabilityPriceRow = showRetailAvailabilityPriceRow;
		_showSalePriceRow = showSalePriceRow;
	}

	public override short ValueMemberLength()
	{
		return 2;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Code;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.ShowPurchasePriceRow = _showPurchasePriceRow;
		ParametersProvider.ShowSalePriceRow = _showSalePriceRow;
		ParametersProvider.ShowRetailAvailabilityPriceRow = _showRetailAvailabilityPriceRow;
		ParametersProvider.ShowPrimeCostRows = _showPrimeCostRows;
		ParametersProvider.ShowDiscountPriceRow = _showDiscountPriceRow;
	}
}
