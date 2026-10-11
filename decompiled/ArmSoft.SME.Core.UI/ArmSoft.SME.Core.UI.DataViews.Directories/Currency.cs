using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Documents;

namespace ArmSoft.SME.Core.UI.DataViews.Directories;

[DataView(2)]
public class Currency : DirectoryDataView<CurrencyDataProvider.DataRow, CurrencyDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.Currency.Value;

	public override string HelpId
	{
		get
		{
			if (Settings.SystemType != SystemTypes.Trade)
			{
				return "Accounting_Lists_Currencies";
			}
			return "Purchases_Lists_OtherLists_Currencies";
		}
	}

	public Currency()
	{
		DataProvider = new CurrencyDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Curr");
		}
	}
}
