using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10008)]
public class Storages : TradeDirectoryDataView<StoragesDataProvider.DataRow, StoragesDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.Storage.Value;

	public override string HelpId => "Purchases_Lists_Storages";

	public override bool DialogSupported => false;

	public Storages()
	{
		DataProvider = new StoragesDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("IsDefault", value: false);
		dvp.Set_ColumnVisible("Retail", Settings.ParametersService.RetailAccounting());
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Code");
		}
	}
}
