using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.InternetBank;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10042)]
public class PayOrdJournal : DocumentDataView<PayOrdJournalDataProvider.DataRow, PayOrdJournalDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly PayOrdJournal _dataView;

		private DateEditControl _dateBegin;

		private DateEditControl _dateEnd;

		private DropDownControl _partner;

		public DataViewDialog(PayOrdJournal dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = ArmSoft.SME.Common.Resources.PaymentOrders;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_dateBegin = AddDateControl(container, "DateBegin", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_dateBegin.EditValue = (useParamValues ? dataView.DateBegin : Settings.HostingEnvironment.DateBegin);
			_dateEnd = AddDateControl(container, "DateEnd", "", useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_dateEnd.EditValue = (useParamValues ? dataView.DateEnd : Settings.HostingEnvironment.DateEnd);
			AddSpecialDateButton(container, _dateBegin, _dateEnd);
			_partner = AddDropDownControl("PartnerCode", ArmSoft.SME.Common.Resources.Partner, new PartnersFilter(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_partner.Code = (useParamValues ? dataView.PartnerCode : string.Empty);
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.DateBegin = _dateBegin.EditValue.Value;
			_dataView.DateEnd = _dateEnd.EditValue.Value;
			_dataView.PartnerCode = _partner.Code;
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				ValidationService.ValidateWrongPeriod(_dateBegin.EditValue, _dateEnd.EditValue);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}
	}

	public override string HelpId => "CashBank_Journals_PaymentOrders";

	public override bool AllowEdit => AllowOperation;

	public override bool AllowView => true;

	public override bool AllowDelete => AllowOperation;

	public override bool AllowPreview => true;

	public override bool AllowDocumentHistory => true;

	public override bool DialogSupported => true;

	public override bool AllowExport => AllowOperation;

	public override string Caption => Resources.PaymentOrders + " " + DateBegin.FormatDDMMYY() + " -  " + DateEnd.FormatDDMMYY();

	public override LoadIndication LoadingIndication => LoadIndication.Always;

	public DateOnly DateBegin
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

	public DateOnly DateEnd
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

	public PayOrdJournal()
	{
		DataProvider = new PayOrdJournalDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("StartDate", Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("EndDate", Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("PartnerCode", Resources.Partner, "");
		FillColumnsFromDataProvider();
	}

	public override void InitContextFunctions()
	{
		if (AllowOperation && Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null && (Settings.LoginService.IsAdmin || Settings.UserAccessService.IsSubsystemAccessible(4)))
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ExportToBankInternetBank", Resources.SendToBank, () =>
			{
				ExportToBank(ExportType.InternetBank);
			});
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ExportToBankInternetBankPayOrderXML", Resources.ExportXMLToIBankFormat, () =>
			{
				ExportToBank(ExportType.InternetBankPayOrderXML);
			});
		}
	}

	private void ExportToBank(ExportType exportType)
	{
		List<IBPayment> list = new List<IBPayment>();
		List<PayOrdJournalDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<PayOrdJournalDataProvider.DataRow>();
		if (exportType == ExportType.InternetBank)
		{
			SettlementAccountHelper.CheckBelongToSameBank(selectedOrFocusedRows.Select((PayOrdJournalDataProvider.DataRow p) => p.PayerAcc).ToList());
		}
		if (selectedOrFocusedRows.Exists((PayOrdJournalDataProvider.DataRow p) => p.ExportedToBank) && ArmSoft.SME.Core.UI.MessageBox.Show(Resources.DocumentAlreadySentToBank, Resources.SendToBank, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes)
		{
			return;
		}
		Mouse.OverrideCursor = Cursors.Wait;
		foreach (PayOrdJournalDataProvider.DataRow item in selectedOrFocusedRows)
		{
			PayOrder payOrderDoc = Settings.DocumentService.Load<PayOrder>(item.ISN, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd.Value);
			list.Add(ConvertToIBPayment(payOrderDoc));
		}
		EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(Resources.CBExportErrorsCaption, "ExportToBankErrors");
		List<Guid> list2 = null;
		switch (exportType)
		{
		case ExportType.InternetBank:
			if ((TradeSettings.TradeCacheService.AccountInBank.GetAccountInBankBySettlementAccount(list[0].PayerAcc)?.BankingModeCode ?? "") == BankingModesSE.InternetBank.Value)
			{
				if (string.IsNullOrWhiteSpace(Settings.InternetBankService.EnsureLoggedIn(list[0].PayerAcc)))
				{
					return;
				}
				list2 = InternetBankHelper.ExportPaymentsToInternetBank(list, enumerateErrorsReportViewer, IBOperation.PaymentOrder);
				break;
			}
			throw new ASException(string.Format(Resources.NoBankingMode, list[0].PayerAcc));
		case ExportType.InternetBankPayOrderXML:
			list2 = InternetBankHelper.ExportToXMLForIBank(list, enumerateErrorsReportViewer, exportType);
			break;
		}
		string comment = "";
		if (exportType == ExportType.InternetBankPayOrderXML)
		{
			comment = Resources.ExportedXMLToIBankFormat;
		}
		foreach (PayOrdJournalDataProvider.DataRow item2 in selectedOrFocusedRows)
		{
			if (list2 == null || list2.Contains(item2.ISN))
			{
				Settings.DocumentService.WriteLog(item2.ISN, item2.DocState, DocumentLogOpSE.ExportedToBank, comment);
			}
		}
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		List<object> list3 = new List<object>();
		foreach (Guid item3 in selectedOrFocusedRows.Select((PayOrdJournalDataProvider.DataRow x) => x.ISN))
		{
			list3.Add(item3);
		}
		currentViewPanel.Update(list3);
		Mouse.OverrideCursor = null;
		if (enumerateErrorsReportViewer.HasErrors)
		{
			enumerateErrorsReportViewer.Show();
		}
	}

	private IBPayment ConvertToIBPayment(PayOrder payOrderDoc)
	{
		IBPayment iBPayment = new IBPayment();
		iBPayment.Amount = payOrderDoc.Summ.Value;
		iBPayment.AmountAMD = payOrderDoc.SummNCC.Value;
		iBPayment.Currency = payOrderDoc.CurrCode;
		iBPayment.Details = payOrderDoc.PaymentAim;
		iBPayment.Date = payOrderDoc.DocDate.ToDateTime();
		iBPayment.DocId = payOrderDoc.ISN;
		iBPayment.DocState = payOrderDoc.State;
		iBPayment.DocType = payOrderDoc.POType;
		iBPayment.DocNum = payOrderDoc.DocNum;
		iBPayment.PayerAcc = payOrderDoc.PayerSettlAcc.Replace("-", "");
		iBPayment.PayerName = payOrderDoc.PayerName;
		iBPayment.PayerTaxCode = payOrderDoc.PayerTaxCode.Replace("-", "");
		iBPayment.ReceiverAcc = payOrderDoc.ReceiverSettlAcc.Replace("-", "");
		iBPayment.ReceiverLegalStatus = InternetBankHelper.ConvertReceiverLegalStatus(payOrderDoc.ReceiverLegalStatus);
		if (string.IsNullOrWhiteSpace(payOrderDoc.PartnerContractGroup.PartnerCode))
		{
			iBPayment.ReceiverName = payOrderDoc.ReceiverName;
		}
		else
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(payOrderDoc.PartnerContractGroup.PartnerCode, throwExceptionNotExits: true);
			iBPayment.ReceiverName = (string.IsNullOrWhiteSpace(item.FullCaption) ? item.Caption : item.FullCaption);
		}
		return iBPayment;
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("ISN", value: false);
		dvp.Set_ColumnVisible("DocState", value: false);
		dvp.Set_ColumnVisible("PartTaxCode", Settings.ParametersService.ShowPartnerTaxCode());
		dvp.Set_ColumnSortOrder("DocDate");
		dvp.Set_ColumnSortOrder("DocNum");
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = DateBegin;
		ParametersProvider.EndDate = DateEnd;
		ParametersProvider.PartnerCode = PartnerCode;
	}

	public override void Add()
	{
		try
		{
			if (Settings.HostingEnvironment.CurrentViewPanel != null)
			{
				_ = Settings.HostingEnvironment.CurrentViewPanel.Id;
			}
			AddDocument(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd.Value);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
