using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10016)]
public class PriceTypes : TradeDirectoryDataView<PriceTypesDataProvider.DataRow, PriceTypesDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.PriceType.Value;

	public override string HelpId => "Purchases_Lists_Productsandservicessublist_PricelistTypes";

	public PriceTypes()
	{
		DataProvider = new PriceTypesDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Code");
		}
	}
}
