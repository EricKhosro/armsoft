using System.Collections.Generic;
using System.Windows.Input;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.UI.DataViewDataProvider;
using ArmSoft.SME.Trade.UI.Services;

namespace ArmSoft.SME.Trade.UI.DataView.Directories.DiscountServer;

public class BonusCardsComparisonDataView : TradeDataView<BonusCardsComparisonDataProvider.DataRow, BonusCardsComparisonDataProvider.Param>
{
	public override string HelpId => "DiscountServer_BonusCardsComparison";

	public string CardNumbers
	{
		get
		{
			return (string)Parameters["CardNumbers"].Value;
		}
		set
		{
			Parameters["CardNumbers"].Value = value;
		}
	}

	public BonusCardsComparisonDataView()
	{
		DataProvider = new BonusCardsComparisonDataProvider();
		AddParameter("CardNumbers", ArmSoft.SME.Trade.Common.Resources.PartnerCard, string.Empty);
		FillColumnsFromDataProvider();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.CardNumbers = CardNumbers;
	}

	public override void InitContextFunctions()
	{
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		if (currentViewPanel == null)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		if (currentViewPanel.GetSelectedAny())
		{
			flag = true;
			flag2 = true;
		}
		else
		{
			BonusCardsComparisonDataProvider.DataRow dataRow = (BonusCardsComparisonDataProvider.DataRow)currentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				switch (dataRow.ComparisonResult)
				{
				case CommonConst.b2bComparisonResult.NotFoundInB2B:
					flag = true;
					break;
				case CommonConst.b2bComparisonResult.NotEqual:
					flag = true;
					flag2 = true;
					break;
				}
			}
		}
		if (flag)
		{
			currentViewPanel.AddContextFunction("ContextFunctions", "ExportToDiscountServer", ArmSoft.SME.Trade.Common.Resources.ExportToDiscountServer, ExportToDiscountServer);
		}
		if (flag2)
		{
			currentViewPanel.AddContextFunction("ContextFunctions", "ImportFromDiscountServer", ArmSoft.SME.Trade.Common.Resources.ImportFromDiscountServer, ImportFromDiscountServer);
		}
	}

	private void ImportFromDiscountServer()
	{
		Mouse.OverrideCursor = Cursors.Wait;
		foreach (BonusCardsComparisonDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<BonusCardsComparisonDataProvider.DataRow>())
		{
			if (selectedOrFocusedRow.ComparisonResult == CommonConst.b2bComparisonResult.NotEqual)
			{
				List<string> updatedCards = null;
				DiscountServerService.ImportCardInfo(DiscountServerService.GetCardInfo(selectedOrFocusedRow.CardNumber), ref updatedCards);
			}
		}
		Settings.HostingEnvironment.CurrentViewPanel.Refresh();
		Mouse.OverrideCursor = null;
		MessageBox.Show(ArmSoft.SME.Common.Resources.ChangeRequestCompleted, ArmSoft.SME.Trade.Common.Resources.ExportToDiscountServer);
	}

	private void ExportToDiscountServer()
	{
		Mouse.OverrideCursor = Cursors.Wait;
		foreach (BonusCardsComparisonDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<BonusCardsComparisonDataProvider.DataRow>())
		{
			if (selectedOrFocusedRow.ComparisonResult == CommonConst.b2bComparisonResult.NotEqual || selectedOrFocusedRow.ComparisonResult == CommonConst.b2bComparisonResult.NotFoundInB2B)
			{
				PartnerCacheRow.ContractRow contractByDiscountCard = TradeSettings.TradeCacheService.Partner.GetContractByDiscountCard(selectedOrFocusedRow.CardNumber, out var partnerCacheRow);
				if (contractByDiscountCard != null)
				{
					DiscountServerService.CreateCardInfo(partnerCacheRow, contractByDiscountCard);
					TradeSettings.CreateTradeClient().CommonServices.EditDiscountServerData(partnerCacheRow.Id, contractByDiscountCard.Contract, exportedToDiscountServer: true);
				}
			}
		}
		Settings.HostingEnvironment.CurrentViewPanel.Refresh();
		Mouse.OverrideCursor = null;
	}
}
