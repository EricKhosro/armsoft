using System;
using System.Collections.Generic;
using System.Windows;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.DataViews.Directories;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Report;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

public class ImportExchangeRates : DialogWindow
{
	protected MultiSelectDropDownControl _currencies;

	protected DateEditControl _dateBegin;

	protected DateEditControl _dateEnd;

	public ImportExchangeRates()
	{
		Caption = ArmSoft.SME.Common.Resources.CBAImportExchangeRates;
		Name = "ImportTransactions";
		HelpId = "CashBank_Operations_ImportExchangeRates";
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		AddStoreValuesButton(container);
		_dateBegin = AddDateControl(container, "DateBegin", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, storeValue: true);
		_dateBegin.EditValue = Settings.HostingEnvironment.DateBegin;
		_dateEnd = AddDateControl(container, "DateEnd", string.Empty, useLongDate: false, isRequired: true, storeValue: true);
		_dateEnd.EditValue = Settings.HostingEnvironment.DateEnd;
		AddSpecialDateButton(container, _dateBegin, _dateEnd);
		_currencies = AddMultiSelectDropDownControl("Currencies", ArmSoft.SME.Common.Resources.Currencies, new OurCurrenciesDataView(), isRequired: true, storeValue: true);
		ShowDialog();
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		try
		{
			base.DialogValidation(args);
			ValidationService.ValidateWrongPeriod(_dateBegin.EditValue, _dateEnd.EditValue);
			if (new RatesOfExchangeDataProvider(Settings.CreateApiClient()).GetRows(new RatesOfExchangeDataProvider.Param
			{
				Currencies = _currencies.SelectedString,
				StartDate = _dateBegin.EditValue,
				EndDate = _dateEnd.EditValue
			}, withoutAccessCheck: true).Count > 0 && MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.ExistsExchangeRates, _dateBegin.EditValue.FormatDDMMYY(), _dateEnd.EditValue.FormatDDMMYY()), Caption, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
			{
				args.isValid = false;
				return;
			}
			List<RatesOfExchangeDataProvider.DataRow> exchangeRates = ExchangeRatesClient.GetExchangeRates(_dateBegin.EditValue.Value, _dateEnd.EditValue.Value, _currencies.SelectedList());
			if (exchangeRates.Count > 0)
			{
				EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(ArmSoft.SME.Common.Resources.EditErrors, "EditSelectedErrors");
				ImportExchangeRatesRequestModel importExchangeRatesRequestModel = new ImportExchangeRatesRequestModel
				{
					RatesOfExchange = new List<RatesOfExchangeDataRow>()
				};
				foreach (RatesOfExchangeDataProvider.DataRow item in exchangeRates)
				{
					importExchangeRatesRequestModel.RatesOfExchange.Add(new RatesOfExchangeDataRow
					{
						Base = item.Base,
						Curr = item.Curr,
						Rate = item.Rate,
						Date = item.Date
					});
				}
				importExchangeRatesRequestModel.StartDate = _dateBegin.EditValue.Value;
				importExchangeRatesRequestModel.EndDate = _dateEnd.EditValue.Value;
				ImportExchnageRatesResponsModel importExchnageRatesResponsModel = DataProcessingRequestHelper.Execute<ImportExchangeRatesRequestModel, ImportExchnageRatesResponsModel>(DPRType.Other, "ImportExchangeRates", importExchangeRatesRequestModel);
				if (importExchnageRatesResponsModel.Exceptions != null && importExchnageRatesResponsModel.Exceptions.Count != 0)
				{
					foreach (ImportExchnageRateResponsModel exception in importExchnageRatesResponsModel.Exceptions)
					{
						enumerateErrorsReportViewer.AddError(exception.Exception.Message);
					}
				}
				if (enumerateErrorsReportViewer.HasErrors)
				{
					enumerateErrorsReportViewer.Show();
				}
				ArmSoft.SME.Core.UI.DataViews.Directories.RatesOfExchange ratesOfExchange = new ArmSoft.SME.Core.UI.DataViews.Directories.RatesOfExchange();
				ratesOfExchange.StartDate = _dateBegin.EditValue;
				ratesOfExchange.EndDate = _dateEnd.EditValue;
				ratesOfExchange.Curr = _currencies.SelectedString;
				ratesOfExchange.Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
				if (Settings.CoreCacheService.CurrentExchangeRates.StoreInLocalStorage)
				{
					Settings.CoreCacheService.CurrentExchangeRates.Refresh();
				}
				return;
			}
			throw new ASException(ArmSoft.SME.Common.Resources.ExchangeRatesToImportNotFound);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
