using System.Text.Json;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directory;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10032)]
public class MTComplectsParts : TradeDirectoryDataView<MTComplectsPartsDataProvider.DataRow, MTComplectsPartsDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.MTComplectsPart.Value;

	public override string HelpId => "Purchases_Lists_Productsandservicessublist_ComplectationList";

	public string ComplectCode { get; set; }

	public override string Caption
	{
		get
		{
			if (string.IsNullOrWhiteSpace(ComplectCode))
			{
				return base.Caption;
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(ComplectCode);
			return base.Caption + ", " + item.MTCode + " - " + item.Caption;
		}
	}

	public MTComplectsParts()
	{
		DataProvider = new MTComplectsPartsDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
		AddParameter("ComplectCode", Resources.Complect, string.Empty);
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("MTDestID", value: false);
		dvp.Set_ColumnVisible("MTDestCode", string.IsNullOrWhiteSpace(ComplectCode));
		dvp.Set_ColumnVisible("MTDestCaption", string.IsNullOrWhiteSpace(ComplectCode));
		dvp.Set_ColumnVisible("ItemType", value: false);
		dvp.Set_ColumnVisible("MTSourceID", value: false);
		dvp.Set_ColumnVisible("QntUnit", value: false);
		dvp.Set_ColumnVisible("VAT", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("MTDestCode");
			dvp.Set_ColumnSortOrder("MTSourceCode");
		}
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.ComplectCode = ComplectCode;
	}

	public override void Add()
	{
		MTComplectsPartsDataProvider.DataRow dataRow = (MTComplectsPartsDataProvider.DataRow)ParentViewPanel.FocusedRow();
		MTComplectsDialogWindow mTComplectsDialogWindow = new MTComplectsDialogWindow(DialogWindowMode.NewModeWithNext);
		mTComplectsDialogWindow.DVPId = ParentViewPanel.Id;
		mTComplectsDialogWindow.IsFilteredComplect = !string.IsNullOrEmpty(ComplectCode);
		mTComplectsDialogWindow.Directory.Code = ((!string.IsNullOrEmpty(ComplectCode)) ? ComplectCode : (dataRow?.MTDestCode ?? ""));
		mTComplectsDialogWindow.ShowDialog();
	}

	public override void CreateCopy()
	{
		object key = GetKey(ParentViewPanel.FocusedRow());
		MTComplectsDialogWindow mTComplectsDialogWindow = new MTComplectsDialogWindow(Settings.DirectoryService.CreateCopy<MTComplectsPart>(DirectoryType, JsonSerializer.Serialize(key)), DialogWindowMode.CreateCopyModeWithNext);
		mTComplectsDialogWindow.DVPId = ParentViewPanel.Id;
		mTComplectsDialogWindow.IsFilteredComplect = !string.IsNullOrEmpty(ComplectCode);
		mTComplectsDialogWindow.ShowDialog();
	}
}
