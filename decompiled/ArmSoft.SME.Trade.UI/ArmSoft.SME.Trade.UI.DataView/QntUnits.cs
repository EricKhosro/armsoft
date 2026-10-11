using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10015)]
public class QntUnits : TradeDirectoryDataView<QntUnitsDataProvider.DataRow, QntUnitsDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.QntUnit.Value;

	public override string HelpId => "Purchases_Lists_Productsandservicessublist_Units";

	public QntUnits()
	{
		DataProvider = new QntUnitsDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("UnitID", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Code");
		}
	}
}
