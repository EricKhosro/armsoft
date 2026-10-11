using System;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DataViewDataProvider;

namespace ArmSoft.SME.Trade.UI.DataView.Directories;

[DataView(10120)]
public class BonusCardsDataView : TradeDataView<BonusCardsDataProvider.DataRow, BonusCardsDataProvider.Param>
{
	public class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly BonusCardsDataView _dataView;

		private TextEditExt _cardNumber;

		public DataViewDialog(BonusCardsDataView dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			Name = _dataView.Name;
			HelpId = dataView.HelpId;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_cardNumber = AddStringControl(container, "CardNumber", ArmSoft.SME.Trade.Common.Resources.DiscountCardCode, 255, isRequired: false, storeValue: true);
			_cardNumber.Text = (useParamValues ? _dataView.CardNumber : string.Empty);
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.CardNumber = _cardNumber.Text;
		}
	}

	public override string HelpId => "b2b_armsoft_am_DiscountServer_BonusCards";

	public override bool DialogSupported => true;

	public string CardNumber
	{
		get
		{
			return (string)Parameters["CardNumber"].Value;
		}
		set
		{
			Parameters["CardNumber"].Value = value;
		}
	}

	public BonusCardsDataView()
	{
		DataProvider = new BonusCardsDataProvider();
		AddParameter("CardNumber", Resources.PartnerCard, string.Empty);
		FillColumnsFromDataProvider();
	}

	public override void InitContextFunctions()
	{
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		if (currentViewPanel != null && new Func<object>(currentViewPanel.FocusedRow) != null && Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.DiscountServerBonusTransactions).AllowView)
		{
			currentViewPanel.AddContextFunction("ContextFunctions", "ShowTransactionsJournal", Resources.OperationsJournal, ShowTransactionsJournal, "cloud_16.svg");
		}
	}

	private void ShowTransactionsJournal()
	{
		BonusTransactionsDataView bonusTransactionsDataView = new BonusTransactionsDataView();
		bonusTransactionsDataView.CardNumber = ((BonusCardsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow()).CardNumber;
		bonusTransactionsDataView.Show(useParamValues: true, openAsChild: false);
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.CardNumber = CardNumber;
	}
}
