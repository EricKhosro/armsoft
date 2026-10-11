using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10025)]
public class BonusScale : TradeDirectoryDataView<BonusScaleDataProvider.DataRow, BonusScaleDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.BonusScale.Value;

	public override string HelpId => "Sales_Lists_BonusScale";

	public BonusScale()
	{
		DataProvider = new BonusScaleDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("ScaleRowID", value: false);
		dvp.Set_ColumnVisible("Point", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("BeginSum");
		}
	}
}
