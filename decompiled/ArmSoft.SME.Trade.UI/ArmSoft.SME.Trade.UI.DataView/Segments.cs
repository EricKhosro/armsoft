using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10011)]
public class Segments : TradeDirectoryDataView<CodeCaptionPairsDataProvider.DataRow, CodeCaptionPairsDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.Segments.Value;

	public override string HelpId => "Purchases_Lists_ProductsAndServicesSublist_ProductsSegments";

	public Segments()
	{
		DataProvider = new SegmentsDataProvider(TradeSettings.CreateTradeClient());
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

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		Settings.HostingEnvironment.CurrentViewPanel?.AddContextFunction("ContextFunctions", "ShowGoodsBySegment", Resources.GoodsBySegment, ShowGoodsBySegment);
	}

	private void ShowGoodsBySegment()
	{
		CodeCaptionPairsDataProvider.DataRow dataRow = (CodeCaptionPairsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			GoodsBySegments goodsBySegments = new GoodsBySegments();
			goodsBySegments.SegmentCode = dataRow.Code;
			goodsBySegments.Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
		}
	}
}
