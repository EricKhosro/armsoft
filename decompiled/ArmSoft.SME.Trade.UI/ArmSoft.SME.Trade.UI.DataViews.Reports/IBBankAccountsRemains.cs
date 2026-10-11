using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Trade.UI.DataViewDataProvider.Reports;
using ArmSoft.SME.Trade.UI.DropDown;

namespace ArmSoft.SME.Trade.UI.DataViews.Reports;

[DataView(22)]
public class IBBankAccountsRemains : DataView<IBBankAccountsRemainsDataProvider.DataRow, IBBankAccountsRemainsDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly IBBankAccountsRemains _dataView;

		private readonly MultiSelectDropDownControl _account;

		public DataViewDialog(IBBankAccountsRemains dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_account = AddMultiSelectDropDownControl(container, "Account", ArmSoft.SME.Common.Resources.Account, new AccountInBanks(isKeySettlAccount: true, BankingModesSE.InternetBank.Value), isRequired: true, storeValue: true);
			_account.SelectedString = (useParamValues ? _dataView.Account : "");
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.Account = _account.SelectedString;
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				SettlementAccountHelper.CheckBelongToSameBank(_account.SelectedList());
				if (string.IsNullOrWhiteSpace(Settings.InternetBankService.EnsureLoggedIn(_account.SelectedList()[0])))
				{
					args.isValid = false;
				}
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}
	}

	public override string HelpId => "CashBank_OnlineQueries_AccountsRemains";

	public override bool DialogSupported => true;

	public string Account
	{
		get
		{
			return (string)Parameters["Account"].Value;
		}
		set
		{
			Parameters["Account"].Value = value;
		}
	}

	public IBBankAccountsRemains()
	{
		DataProvider = new IBBankAccountsRemainsDataProvider();
		AddParameter("Account", Resources.Account, "");
		FillColumnsFromDataProvider();
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.Account = Account.SplitSelected();
	}
}
