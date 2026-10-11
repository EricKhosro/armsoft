using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10021)]
public class Properties : TradeDirectoryDataView<PropertiesDataProvider.DataRow, PropertiesDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.Property.Value;

	public override string HelpId => "Purchases_Lists_Otherlists_PropertiesTypes";

	public Properties()
	{
		DataProvider = new PropertiesDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("NextProperty", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Code");
		}
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.HostingEnvironment.CurrentViewPanel != null && Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null && Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.PropertyList.Value).AllowView)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowPropertyList", Resources.MTPropValues, ShowPropertyList);
		}
	}

	private void ShowPropertyList()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			PropertiesDataProvider.DataRow dataRow = (PropertiesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				new PropertyList(dataRow.Code).Show();
			}
		}
	}
}
