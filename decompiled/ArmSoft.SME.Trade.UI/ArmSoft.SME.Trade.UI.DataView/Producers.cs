using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10039)]
public class Producers : TradeDirectoryDataView<CodeCaptionPairsDataProvider.DataRow, CodeCaptionPairsDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.Producers.Value;

	public override string HelpId => "Purchases_Lists_Otherlists_Manufacturers";

	public override string Caption => Resources.Producers;

	public Producers()
	{
		DataProvider = new ProducersDataProvider(TradeSettings.CreateTradeClient());
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
