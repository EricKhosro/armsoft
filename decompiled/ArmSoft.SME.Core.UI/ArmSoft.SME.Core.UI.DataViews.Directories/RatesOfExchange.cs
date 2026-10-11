using System;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;

namespace ArmSoft.SME.Core.UI.DataViews.Directories;

[DataView(3)]
public class RatesOfExchange : DirectoryDataView<RatesOfExchangeDataProvider.DataRow, RatesOfExchangeDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly RatesOfExchange _dataView;

		private readonly DateEditControl _startDate;

		private readonly DateEditControl _endDate;

		private readonly MultiSelectDropDownControl _curr;

		public DataViewDialog(RatesOfExchange dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = dataView.DialogCaption;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_startDate = AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: false, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_startDate.EditValue = (useParamValues ? dataView.StartDate : new DateOnly?(Settings.HostingEnvironment.DateBegin));
			_endDate = AddDateControl(container, "EndDate", "", useLongDate: false, isRequired: false, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_endDate.EditValue = (useParamValues ? dataView.EndDate : new DateOnly?(Settings.HostingEnvironment.DateEnd));
			AddSpecialDateButton(container, _startDate, _endDate);
			container = AddHorizontalLayoutGroup();
			_curr = AddMultiSelectDropDownControl(container, "Currencies", ArmSoft.SME.Common.Resources.Currencies, new ArmSoft.SME.Core.UI.DropDowns.Currency(shortMode: true, withOutNCC: true), isRequired: false, !useParamValues);
			_curr.SelectedString = (useParamValues ? dataView.Curr : string.Empty);
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.StartDate = _startDate.EditValue;
			_dataView.EndDate = _endDate.EditValue;
			_dataView.Curr = _curr.SelectedString;
		}
	}

	public override short DirectoryType => DirectoryTypeSE.RatesOfExchange.Value;

	public override string HelpId
	{
		get
		{
			if (Settings.SystemType != SystemTypes.Trade)
			{
				return "Accounting_Lists_ExchangeRates";
			}
			return "Purchases_Lists_OtherLists_ExchangeRates";
		}
	}

	public override string Caption => $"{base.Caption} - {StartDate.FormatDDMMYY()} - {EndDate.FormatDDMMYY()}";

	private string DialogCaption => base.Caption + " ";

	public override LoadIndication LoadingIndication => LoadIndication.AlwaysInside;

	public override bool DialogSupported => true;

	public DateOnly? StartDate
	{
		get
		{
			return (DateOnly?)Parameters["StartDate"].Value;
		}
		set
		{
			Parameters["StartDate"].Value = value;
		}
	}

	public DateOnly? EndDate
	{
		get
		{
			return (DateOnly?)Parameters["EndDate"].Value;
		}
		set
		{
			Parameters["EndDate"].Value = value;
		}
	}

	public string Curr
	{
		get
		{
			return (string)Parameters["Currencies"].Value;
		}
		set
		{
			Parameters["Currencies"].Value = value;
		}
	}

	public RatesOfExchange()
	{
		DataProvider = new RatesOfExchangeDataProvider(Settings.CreateApiClient());
		AddParameter("StartDate", Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("EndDate", Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("Currencies", Resources.Currencies, true);
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Date");
			dvp.Set_ColumnSortOrder("Curr");
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
		ParametersProvider.Currencies = Curr;
	}
}
