using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using DevExpress.Data.Filtering;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10005)]
public class SubstituteItemsGroups : TradeDirectoryDataView<CodeCaptionPairsDataProvider.DataRow, CodeCaptionPairsDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.SubstituteItemsGroups.Value;

	public override string HelpId => "Purchases_Lists_Otherlists_SubstituteItemGroups";

	public override string Caption => Resources.SubstituteItemsGroups;

	public SubstituteItemsGroups()
	{
		DataProvider = new SubstituteItemsGroupsDataProvider(TradeSettings.CreateTradeClient());
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
		if (Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MTComplectsParts).AllowView)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowCorrespondingGoodsAndServices", Resources.MaterialsAndServices, ShowCorrespondingGoodsAndServices);
		}
	}

	private void ShowCorrespondingGoodsAndServices()
	{
		CodeCaptionPairsDataProvider.DataRow dataRow = (CodeCaptionPairsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		Goods goods = new Goods();
		goods.FilterString = CriteriaOperator.Parse("[SubstituteItemsGroupCode] = ?", dataRow.Code).LegacyToString();
		goods.Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
	}
}
