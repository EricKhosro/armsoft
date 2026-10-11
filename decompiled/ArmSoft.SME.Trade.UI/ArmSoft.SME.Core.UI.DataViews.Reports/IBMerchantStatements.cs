using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViewsData.Reports;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.DropDown;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

[DataView(10125)]
public class IBMerchantStatements : DataView<IBMerchantStatementsDataProvider.DataRow, IBMerchantStatementsDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly IBMerchantStatements _dataView;

		private readonly DateEditControl _startDate;

		private readonly DateEditControl _endDate;

		private readonly DropDownControl _account;

		public DataViewDialog(IBMerchantStatements dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = ArmSoft.SME.Trade.Common.Resources.MerchantStatements;
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
			_account = AddDropDownControl("Account", ArmSoft.SME.Common.Resources.Account, new AccountInBanks(isKeySettlAccount: true, BankingModesSE.InternetBank.Value), isRequired: true, allowAdditionButtons: false, !useParamValues);
			_account.Code = (useParamValues ? dataView.Account : "");
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.StartDate = _startDate.EditValue.Value;
			_dataView.EndDate = _endDate.EditValue.Value;
			_dataView.Account = _account.Code;
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
				if (string.IsNullOrWhiteSpace(Settings.InternetBankService.EnsureLoggedIn(_account.Code)))
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

	public override string Caption => $"{ArmSoft.SME.Trade.Common.Resources.MerchantStatements} - {StartDate.FormatDDMMYY()} - {EndDate.FormatDDMMYY()}";

	public override string HelpId => "MerchantStatement";

	public override string Name => "IBMerchantStatements";

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

	public IBMerchantStatements()
	{
		DataProvider = new IBMerchantStatementsDataProvider();
		FillColumnsFromDataProvider();
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("Account", ArmSoft.SME.Common.Resources.Account, "");
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("TransactionDate");
		}
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

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		IBMerchantStatementsDataProvider.DataRow dataRow = (IBMerchantStatementsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null && dataRow.CreatedDocISN.HasValue)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "OpenCorrespondentDoc", ArmSoft.SME.Common.Resources.OpenCorrespondentDoc, OpenCorrespondentDoc);
		}
	}

	public override string GetDefaultFunction()
	{
		return "OpenCorrespondentDoc";
	}

	private void OpenCorrespondentDoc()
	{
		IBMerchantStatementsDataProvider.DataRow dataRow = (IBMerchantStatementsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		Settings.DocumentService.ShowWindow(dataRow.CreatedDocISN.Value, WindowViewMode.Edit, Settings.HostingEnvironment.CurrentViewPanel.Id);
	}
}
