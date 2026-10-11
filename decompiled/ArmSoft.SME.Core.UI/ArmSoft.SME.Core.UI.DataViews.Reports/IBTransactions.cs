using System;
using System.Collections.Generic;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViewsData.Reports;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

public class IBTransactions : DataView<IBTransactionsDataProvider.DataRow, IBTransactionsDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly IBTransactions _dataView;

		private readonly DateEditControl _startDate;

		private readonly DateEditControl _endDate;

		private readonly MultiSelectDropDownControl _account;

		public DataViewDialog(IBTransactions dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = dataView.Caption;
			MinWidth = 600.0;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_startDate = AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_startDate.EditValue = (useParamValues ? dataView.StartDate : Settings.HostingEnvironment.DateBegin);
			_endDate = AddDateControl(container, "EndDate", "", useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_endDate.EditValue = (useParamValues ? dataView.EndDate : Settings.HostingEnvironment.DateEnd);
			AddSpecialDateButton(container, _startDate, _endDate);
			IDropDownService requiredService = Settings.ServiceProvider.GetRequiredService<IDropDownService>();
			_account = AddMultiSelectDropDownControl("Account", ArmSoft.SME.Common.Resources.Account, requiredService.CreateDropDown(1, BankingModesSE.InternetBank.Value), isRequired: true, storeValue: true);
			_account.SelectedString = (useParamValues ? dataView.Account : "");
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.StartDate = _startDate.EditValue.Value;
			_dataView.EndDate = _endDate.EditValue.Value;
			_dataView.Account = _account.SelectedString;
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
				SettlementAccountHelper.CheckBelongToSameBank(_account.SelectedList());
				if (!InternetBankHelper.EnsureLoggedIn(new HashSet<string> { SettlementAccountHelper.GetBankMainCode(_account.SelectedList()[0]) }))
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

	public override string HelpId => "Bank_Journals_ImportedEntries";

	public override string Name => "InternetBank_Transactions";

	public override bool DialogSupported => true;

	public DateOnly StartDate
	{
		get
		{
			return (DateOnly)Parameters["StartDate"].Value;
		}
		set
		{
			Parameters["StartDate"].Value = value;
		}
	}

	public DateOnly EndDate
	{
		get
		{
			return (DateOnly)Parameters["EndDate"].Value;
		}
		set
		{
			Parameters["EndDate"].Value = value;
		}
	}

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

	public IBTransactions()
	{
		DataProvider = new IBTransactionsDataProvider();
		FillColumnsFromDataProvider();
		AddParameter("StartDate", Resources.DatePeriodBegin, Settings.HostingEnvironment.CurrentDate, "dd/MM/yy");
		AddParameter("EndDate", Resources.DatePeriodEnd, Settings.HostingEnvironment.CurrentDate, "dd/MM/yy");
		AddParameter("Account", Resources.Account, "");
		Columns.AddSorting("Date");
		Columns.AddSorting("Account");
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.Account = Account;
	}
}
