using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10023)]
public class DiscountGroups : TradeDirectoryDataView<DiscountGroupsDataProvider.DataRow, DiscountGroupsDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.DiscountGroup;

	public override string HelpId => "Sales_Lists_DiscountsGroups";

	public override bool DialogSupported => false;

	public DiscountGroups()
	{
		DataProvider = new DiscountGroupsDataProvider(TradeSettings.CreateTradeClient());
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
		Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "DiscountsMergeMethod", Resources.DiscountsMergeMethod, DiscountsMergeMethod);
	}

	public static void DiscountsMergeMethod()
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Name = "DiscountsMergeMethod",
			Caption = Resources.Discounts
		};
		DropDownControl dropDownControl = dialogWindow.AddDropDownControl("DiscountsMergeMethod", Resources.DiscountsMergeMethod, new SmartEnum(typeof(DiscountsMergeMethodsSE)), isRequired: true);
		dropDownControl.Code = Settings.ParametersService.DiscountsMergeMethod();
		if (dialogWindow.ShowDialog().Value)
		{
			Settings.ParametersService.DiscountsMergeMethod_set(dropDownControl.Code);
		}
	}
}
