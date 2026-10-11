using System.Text.Json;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directory;
using DevExpress.Data;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10036)]
public class PartnerAccountInBank : TradeDirectoryDataView<PartnerAccountsInBankDataProvider.DataRow, PartnerAccountsInBankDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.PartnerAccountsInBank.Value;

	public override string HelpId => "Purchases_Lists_Partnerssublists_PartnerSettlementAccounts";

	public override string Caption
	{
		get
		{
			if (string.IsNullOrWhiteSpace(PartnerCode))
			{
				return ArmSoft.SME.Trade.Common.Resources.PartnersAccountsInBank;
			}
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(PartnerCode);
			return ArmSoft.SME.Common.Resources.PartnerAccountsInBank + ", " + item.PartCode + " - " + item.Caption;
		}
	}

	public string PartnerCode
	{
		get
		{
			return (string)Parameters["PartnerCode"].Value;
		}
		set
		{
			Parameters["PartnerCode"].Value = value;
		}
	}

	public PartnerAccountInBank()
	{
		DataProvider = new PartnerAccountsInBankDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
		AddParameter("PartnerCode", ArmSoft.SME.Common.Resources.Partner, "");
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.PartnerCode = PartnerCode;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("PartID", value: false);
		dvp.Set_ColumnVisible("PartCode", string.IsNullOrWhiteSpace(PartnerCode));
		dvp.Set_ColumnVisible("PartCaption", string.IsNullOrWhiteSpace(PartnerCode));
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("PartCode");
			dvp.Set_ColumnSortOrder("IsDefault", ColumnSortOrder.Descending);
			dvp.Set_ColumnSortOrder("SettlAccount");
		}
	}

	public override void Add()
	{
		PartnerAccountsInBankDataProvider.DataRow dataRow = (PartnerAccountsInBankDataProvider.DataRow)ParentViewPanel.FocusedRow();
		PartnerAccountInBankDialogWindow partnerAccountInBankDialogWindow = new PartnerAccountInBankDialogWindow(DialogWindowMode.NewMode);
		partnerAccountInBankDialogWindow.DVPId = ParentViewPanel.Id;
		partnerAccountInBankDialogWindow.IsFilteredPartner = !string.IsNullOrWhiteSpace(PartnerCode);
		partnerAccountInBankDialogWindow.Directory.Code = ((!string.IsNullOrWhiteSpace(PartnerCode)) ? PartnerCode : (dataRow?.PartCode ?? ""));
		partnerAccountInBankDialogWindow.ShowDialog();
	}

	public override void CreateCopy()
	{
		object key = GetKey(ParentViewPanel.FocusedRow());
		PartnerAccountInBankDialogWindow partnerAccountInBankDialogWindow = new PartnerAccountInBankDialogWindow(Settings.DirectoryService.CreateCopy<ArmSoft.SME.Trade.Common.Models.Directories.PartnerAccountInBank>(DirectoryType, JsonSerializer.Serialize(key)), DialogWindowMode.CreateCopyMode);
		partnerAccountInBankDialogWindow.DVPId = ParentViewPanel.Id;
		partnerAccountInBankDialogWindow.IsFilteredPartner = !string.IsNullOrWhiteSpace(PartnerCode);
		partnerAccountInBankDialogWindow.ShowDialog();
	}
}
