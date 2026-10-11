using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class DocumentsJournal : DropDownView<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	private class DropDownViewDialog : DropDownViewParametersDialogWindow
	{
		private readonly DocumentsJournal _dataView;

		private readonly DateEditControl _dateBegin;

		private readonly DateEditControl _dateEnd;

		private readonly DateEditControl _delDateBegin;

		private readonly DateEditControl _delDateEnd;

		private readonly DropDownControl _goodCode;

		private readonly DropDownControl _partnerCode;

		private readonly DropDownControl _storageCode;

		private readonly DropDownControl _emplCode;

		private readonly DropDownControl _docStates;

		private readonly DropDownControl _docOwner;

		private readonly MultiSelectDropDownControl _docTypes;

		private readonly DropDownControl _currCode;

		public DropDownViewDialog(DocumentsJournal dataView, Window owner)
			: base(dataView, owner)
		{
			_dataView = dataView;
			Top = dataView.DialogTop;
			Left = dataView.DialogLeft;
			Owner = owner;
			Caption = ArmSoft.SME.Common.Resources.Documents;
			Name = _dataView.Name;
			HelpId = _dataView.HelpId;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			_dateBegin = AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, storeValue: true, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_dateBegin.EditValue = _dataView.StartDate;
			_dateEnd = AddDateControl(container, "EndDate", "", useLongDate: false, isRequired: true, storeValue: true, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_dateEnd.EditValue = _dataView.EndDate;
			AddSpecialDateButton(container, _dateBegin, _dateEnd);
			if (_dataView._isDeletedDocJournal)
			{
				container = AddHorizontalLayoutGroup();
				_delDateBegin = AddDateControl(container, "DelStartDate", ArmSoft.SME.Common.Resources.RemovalDatePeriod, useLongDate: false, isRequired: false, storeValue: true);
				_delDateBegin.EditValue = _dataView.DelStartDate;
				_delDateEnd = AddDateControl(container, "DelEndDate", "", useLongDate: false, isRequired: false, storeValue: true);
				_delDateEnd.EditValue = _dataView.DelEndDate;
				AddSpecialDateButton(container, _delDateBegin, _delDateEnd);
			}
			if (!_dataView._isReceivedTaxInvoices)
			{
				if (!_dataView._isPurchaseOrderDocs && !_dataView._isSentInvoicesForPayment && !_dataView._isReconciliationStatementDoc && !_dataView._isBillDocuments)
				{
					_docTypes = AddMultiSelectDropDownControl("DocType", ArmSoft.SME.Common.Resources.TypeWord, new DocumentType(dataView._baseDocTypes, checkViewAccess: true, checkAddAccess: true), isRequired: false, storeValue: true);
					_docTypes.SelectedString = _dataView.DocType;
				}
				if (!_dataView._isTransportationDocuments)
				{
					if (!_dataView._isECRCheckDocuments && !_dataView._isPurchaseOrderDocs && !_dataView._isReconciliationStatementDoc)
					{
						_currCode = AddDropDownControl("CurrCode", ArmSoft.SME.Common.Resources.Curr, new Currency(shortMode: true), isRequired: false, allowAdditionButtons: false, storeValue: true);
						_currCode.Code = _dataView.CurrCode;
					}
					if (_dataView._isCashDocsDataView)
					{
						AddDropDownControl("CashCode", ArmSoft.SME.Common.Resources.Cash, new Cashes(), isRequired: false, allowAdditionButtons: false, storeValue: true).Code = _dataView.CashCode;
					}
					_partnerCode = AddDropDownControl("PartnerCode", ArmSoft.SME.Common.Resources.Partner, new PartnersFilter(), isRequired: false, allowAdditionButtons: false, storeValue: true);
					_partnerCode.Code = _dataView.PartnerCode;
					if (_dataView._isWithGoodsFilter)
					{
						_goodCode = AddDropDownControl("GoodCode", ArmSoft.SME.Common.Resources.ProductsAndServices, new GoodsFilter(shortMode: false), isRequired: false, allowAdditionButtons: false, storeValue: true);
						_goodCode.Code = _dataView.GoodCode;
					}
					if (_dataView._isECRCheckDocuments)
					{
						string storage = ArmSoft.SME.Common.Resources.Storage;
						bool? checkViewOrOperationAccess = true;
						_storageCode = AddDropDownControl("StorageCode", storage, new Storages(null, checkViewOrOperationAccess), isRequired: false, allowAdditionButtons: false, storeValue: true);
						_storageCode.Code = _dataView.StorageCode;
						AddDropDownControl("CashCode", ArmSoft.SME.Common.Resources.Cash + " (" + ArmSoft.SME.Common.Resources.ECR + ")", new Cashes(withoutCheckAccess: false, showOnlyECRCashes: true), isRequired: false, allowAdditionButtons: false, storeValue: true).Code = _dataView.CashCode;
					}
					if (!_dataView._isPurchaseOrderDocs && !_dataView._isSentInvoicesForPayment && !_dataView._isReconciliationStatementDoc && !(_dataView._reportType == ReportTypesSE.IBCorrespondent))
					{
						_emplCode = AddDropDownControl("EmplCode", ArmSoft.SME.Common.Resources.Employee, new Employees(), isRequired: false, allowAdditionButtons: false, storeValue: true);
						_emplCode.Code = _dataView.EmplCode;
					}
					if (_dataView._isECRCheckDocuments)
					{
						AddBooleanControl("ShowPayments", ArmSoft.SME.Common.Resources.ShowPayments, storeValue: true);
						AddBooleanControl("ShowOutSummsAsNegatives", ArmSoft.SME.Common.Resources.ShowOutSumNegative, storeValue: true);
					}
					if (_dataView._isTaxInvoicesDocument)
					{
						AddBooleanControl("ShowOutSummsAsNegatives", ArmSoft.SME.Common.Resources.ShowOutSumNegative, storeValue: true);
					}
				}
				if (!_dataView._isDeletedDocJournal && !_dataView._isSentInvoicesForPayment && !_dataView._isReconciliationStatementDoc && !(_dataView._reportType == ReportTypesSE.IBCorrespondent))
				{
					_docStates = AddDropDownControl("DocState", ArmSoft.SME.Common.Resources.State, new DocumentState(), isRequired: false, allowAdditionButtons: false, storeValue: true);
					_docStates.Code = _dataView.DocState ?? DocumentStateSE.DraftOrProcessed.ValueUI;
				}
			}
			if (_dataView._isReceivedTaxInvoices || !(_dataView._reportType != ReportTypesSE.IBCorrespondent))
			{
				return;
			}
			_docOwner = AddDropDownControl("DocOwner", ArmSoft.SME.Common.Resources.DocOwner, new UsersFilter(), isRequired: false, allowAdditionButtons: false, storeValue: true, isShowComment: false, 50);
			if (Settings.LoginService.IsAdmin || !Settings.ParametersService.DenyAccessToOtherOwnerDocs())
			{
				_docOwner.Code = ((!string.IsNullOrWhiteSpace(dataView.DocOwner)) ? _dataView.DocOwner : "");
				return;
			}
			_docOwner.Code = Settings.LoginService.UserName;
			if (!dataView._isTransportationDocuments)
			{
				_docOwner.IsReadOnly = true;
			}
		}

		public override void ApplyParams()
		{
			_dataView.StartDate = _dateBegin.EditValue;
			_dataView.EndDate = _dateEnd.EditValue;
			if (_dataView._isDeletedDocJournal)
			{
				_dataView.DelStartDate = _delDateBegin.EditValue;
				_dataView.DelEndDate = _delDateEnd.EditValue;
			}
			if (!_dataView._isReceivedTaxInvoices)
			{
				if (!_dataView._isPurchaseOrderDocs && !_dataView._isSentInvoicesForPayment && !_dataView._isReconciliationStatementDoc && !_dataView._isBillDocuments)
				{
					_dataView.DocType = _docTypes.SelectedString;
				}
				if (!_dataView._isTransportationDocuments)
				{
					if (_dataView._isECRCheckDocuments)
					{
						_dataView.StorageCode = _storageCode.Code;
					}
					else if (!_dataView._isReconciliationStatementDoc)
					{
						_dataView.CurrCode = _currCode.Code;
					}
					_ = _dataView._isTaxInvoicesDocument;
					if (!_dataView._isECRCheckDocuments)
					{
						_ = _dataView._isCashDocsDataView;
					}
					_dataView.PartnerCode = _partnerCode.Code;
					if (_dataView._isWithGoodsFilter)
					{
						_dataView.GoodCode = _goodCode.Code;
					}
					if (!_dataView._isSentInvoicesForPayment && !_dataView._isReconciliationStatementDoc && !(_dataView._reportType == ReportTypesSE.IBCorrespondent))
					{
						_dataView.EmplCode = _emplCode.Code;
					}
				}
				if (!_dataView._isDeletedDocJournal && !_dataView._isSentInvoicesForPayment && !_dataView._isReconciliationStatementDoc && !(_dataView._reportType == ReportTypesSE.IBCorrespondent))
				{
					_dataView.DocState = _docStates.Code;
				}
			}
			if (!_dataView._isReceivedTaxInvoices && _dataView._reportType != ReportTypesSE.IBCorrespondent)
			{
				_dataView.DocOwner = _docOwner.Code;
			}
		}

		protected override void DialogValidation(DialogWindow dialog, DialogWindowValidationArgs args)
		{
			try
			{
				ValidationService.ValidateWrongPeriod(_dateBegin.EditValue, _dateEnd.EditValue);
				ValidationService.ValidateWrongPeriod(_delDateBegin?.EditValue, _delDateEnd?.EditValue);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}
	}

	private readonly ReportTypesSE _reportType;

	private readonly bool _isVisibleOrderNumColumn;

	private readonly bool _isWarehouseDocuments;

	private readonly bool _isPurchaseDocuments;

	private readonly bool _isTaxInvoicesDocument;

	private readonly bool _isReceivedTaxInvoices;

	private readonly bool _isPurchaseOrderDocs;

	private readonly bool _isSentInvoicesForPayment;

	private readonly bool _isReconciliationStatementDoc;

	private readonly bool _isTransportationDocuments;

	private readonly bool _isECRCheckDocuments;

	private readonly bool _isBillDocuments;

	private readonly bool _isDeletedDocJournal;

	private readonly bool _isDSConnectedDocs;

	private readonly bool _isCashDocsDataView;

	private readonly bool _isSaleDocsDataView;

	private readonly bool _isWithGoodsFilter;

	private readonly bool _showCashDeskAndShiftNumber;

	private readonly Window _owner;

	private readonly bool _isDeferedDocuments;

	private readonly bool _useDiscountServer;

	private readonly List<short> _baseDocTypes;

	private readonly bool _forPosReturn;

	public override string HelpId
	{
		get
		{
			if (_isTaxInvoicesDocument)
			{
				return "Sales_Journals_SalesInvJournal";
			}
			if (_isWarehouseDocuments)
			{
				return "Warehouse_Journals_WarehouseDocuments";
			}
			if (_isPurchaseDocuments)
			{
				return "Purchases_Journals_PurchaseDocuments";
			}
			if (_isDeletedDocJournal)
			{
				return "Admintools_DeletedDocs";
			}
			if (_isTransportationDocuments)
			{
				return "Warehouse_Journals_WarehouseTransferDocuments";
			}
			if (_isECRCheckDocuments)
			{
				return "Sales_Journals_ECR";
			}
			if (_isCashDocsDataView)
			{
				return "Cashbank_Journals_CashDocuments";
			}
			if (_reportType == ReportTypesSE.BankDocument)
			{
				return "Cashbank_Journals_BankDocuments";
			}
			if (_isSaleDocsDataView)
			{
				return "Sales_Journals_SalesDocuments";
			}
			if (_isPurchaseOrderDocs)
			{
				return "Purchases_Journals_SentOrdersJournal";
			}
			if (_isSentInvoicesForPayment)
			{
				return "Sales_Journals_SubmittedTransferInvoices";
			}
			if (_isReconciliationStatementDoc)
			{
				return "Trade_Journals_ReconcillattionStatementsJournal";
			}
			return "Purchases_Journals_Documents";
		}
	}

	public override bool DialogSupported => true;

	public override string Caption => ArmSoft.SME.Common.Resources.DocumentsJournal + " " + StartDate.FormatDDMMYY() + " - " + EndDate.FormatDDMMYY();

	public override bool KeyMemberSupported => true;

	public DateOnly? StartDate { get; set; } = Settings.HostingEnvironment.DateBegin;

	public DateOnly? EndDate { get; set; } = Settings.HostingEnvironment.DateEnd;

	public DateOnly? DelStartDate { get; set; }

	public DateOnly? DelEndDate { get; set; }

	public string DocType { get; set; } = string.Empty;

	public string CurrCode { get; set; } = string.Empty;

	public string EmplCode { get; set; } = string.Empty;

	public string PartnerCode { get; set; } = string.Empty;

	public string GoodCode { get; set; } = string.Empty;

	public string StorageCode { get; set; } = string.Empty;

	public string CashCode { get; set; } = string.Empty;

	public string ShiftNumber { get; set; } = string.Empty;

	public string DocState { get; set; } = string.Empty;

	public string Source { get; set; } = string.Empty;

	public string DocOwner { get; set; } = string.Empty;

	public double DialogTop { get; set; }

	public double DialogLeft { get; set; }

	public DocumentsJournal(Window owner, ReportTypesSE reportType, HashSet<short> baseDocTypes, bool forPosReturn = false)
	{
		_useDiscountServer = Settings.ParametersService.UseBonus() && Settings.ParametersService.ConnectTob2barmsoftDiscountServer();
		_owner = owner;
		_reportType = reportType;
		_forPosReturn = forPosReturn;
		if (baseDocTypes != null)
		{
			_baseDocTypes = baseDocTypes.ToList();
		}
		_isWarehouseDocuments = _reportType == ReportTypesSE.WarehouseDocument;
		_isPurchaseDocuments = _reportType == ReportTypesSE.PurchaseDocument;
		_isVisibleOrderNumColumn = StringExtensions.InList(_reportType, ReportTypesSE.AllDocument, ReportTypesSE.DeletedDocument, ReportTypesSE.CashDocument, ReportTypesSE.BankDocument, ReportTypesSE.PurchaseDocument, ReportTypesSE.SaleDocument, ReportTypesSE.WarehouseDocument);
		_isWithGoodsFilter = !StringExtensions.InList(_reportType, ReportTypesSE.ReconciliationStatements, ReportTypesSE.CashDocument, ReportTypesSE.BankDocument, ReportTypesSE.IBCorrespondent);
		_isCashDocsDataView = _reportType == ReportTypesSE.CashDocument;
		_isSaleDocsDataView = _reportType == ReportTypesSE.SaleDocument;
		_isTaxInvoicesDocument = _reportType == ReportTypesSE.TaxInvoices;
		_isReceivedTaxInvoices = _reportType == ReportTypesSE.ReceivedTaxInvoices;
		_isTransportationDocuments = _reportType == ReportTypesSE.TransportationDocument;
		_isReconciliationStatementDoc = _reportType == ReportTypesSE.ReconciliationStatements;
		_isECRCheckDocuments = _reportType == ReportTypesSE.ECRCheckDocument;
		_isBillDocuments = _reportType == ReportTypesSE.BillDocument;
		_isPurchaseOrderDocs = _reportType == ReportTypesSE.PurchaseOrderDocument;
		_isSentInvoicesForPayment = _reportType == ReportTypesSE.SentInvoicesForPayment;
		_isDeletedDocJournal = _reportType == ReportTypesSE.DeletedDocument;
		_isDSConnectedDocs = _reportType == ReportTypesSE.AllDocument || _reportType == ReportTypesSE.SaleDocument || _isECRCheckDocuments || _isCashDocsDataView;
		_isDeferedDocuments = _reportType == ReportTypesSE.DeferredDocument;
		_showCashDeskAndShiftNumber = _isCashDocsDataView || _isDeferedDocuments || _isECRCheckDocuments;
		DataProvider = new AllDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
		if (!_isReceivedTaxInvoices)
		{
			if (StringExtensions.InList(_reportType, ReportTypesSE.BillDocument, ReportTypesSE.DeferredDocument) && (Settings.LoginService.IsAdmin || !Settings.ParametersService.DenyAccessToOtherOwnerDocs()))
			{
				DocOwner = "";
			}
			else
			{
				DocOwner = Settings.LoginService.UserName;
			}
		}
		ConfigureColumns();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.DelStartDate = DelStartDate;
		ParametersProvider.DelEndDate = DelEndDate;
		if (string.IsNullOrWhiteSpace(DocType) && _baseDocTypes != null && _baseDocTypes.Count != 0)
		{
			ParametersProvider.DocType = string.Join(",", _baseDocTypes);
		}
		else
		{
			ParametersProvider.DocType = DocType;
		}
		ParametersProvider.CurrCode = CurrCode;
		ParametersProvider.EmplCode = EmplCode;
		ParametersProvider.PartnerCode = PartnerCode;
		ParametersProvider.GoodCode = GoodCode;
		ParametersProvider.StorageCode = StorageCode;
		ParametersProvider.CashCode = CashCode;
		ParametersProvider.ShiftNumber = ShiftNumber;
		ParametersProvider.DocState = DocState;
		ParametersProvider.Source = Source;
		ParametersProvider.DocOwner = DocOwner;
		ParametersProvider.ReportType = _reportType;
	}

	private void ConfigureColumns()
	{
		string currencyFormat = DecimalExtension.GetCurrencyFormat(16, 2);
		Columns.Add("DocDate", ArmSoft.SME.Common.Resources.DateWord);
		if (_isDeferedDocuments || _forPosReturn)
		{
			Columns.Add("LastModifyDate", ArmSoft.SME.Common.Resources.LastChange).Format = "H:mm:ss";
		}
		if (_isVisibleOrderNumColumn)
		{
			Columns.Add("OrderNum", ArmSoft.SME.Common.Resources.OrderNumber);
		}
		Columns.Add("DocNum", ArmSoft.SME.Common.Resources.DocNumber);
		if (_isECRCheckDocuments || _isBillDocuments)
		{
			Columns.Add("ECRCheckNum", ArmSoft.SME.Common.Resources.ECRCheckNum);
		}
		if (_isTaxInvoicesDocument || _isTransportationDocuments || _isReceivedTaxInvoices)
		{
			if (_isTaxInvoicesDocument)
			{
				Columns.Add("SubmissionDate", ArmSoft.SME.Common.Resources.SubmissionDate);
				Columns.Add("SupplyDate", ArmSoft.SME.Common.Resources.DeliveryDate);
			}
			Columns.Add("SerialAndNumber", ArmSoft.SME.Common.Resources.TaxSerialNumber);
			if (_isReceivedTaxInvoices)
			{
				Columns.Add("SupplierTaxCode", ArmSoft.SME.Common.Resources.SupplierTaxCode);
				Columns.Add("SupplierName", ArmSoft.SME.Common.Resources.SupplierName);
				Columns.Add("SupplyAddress", ArmSoft.SME.Trade.Common.Resources.SupplyAddress);
			}
			if (_isTaxInvoicesDocument || _isTransportationDocuments)
			{
				Columns.Add("DocTypeName", ArmSoft.SME.Common.Resources.TypeWord);
			}
		}
		else if (!_isPurchaseOrderDocs && !_isSentInvoicesForPayment && !_isReconciliationStatementDoc)
		{
			Columns.Add("DocTypeName", ArmSoft.SME.Common.Resources.TypeWord);
		}
		if (!_isTransportationDocuments && !_isPurchaseOrderDocs && !_isReconciliationStatementDoc && !_isDeferedDocuments && !_forPosReturn)
		{
			Columns.Add("CurrCode", ArmSoft.SME.Common.Resources.Curr);
		}
		if (!_isPurchaseOrderDocs && !_isReconciliationStatementDoc)
		{
			Columns.Add("Summ", ArmSoft.SME.Common.Resources.Summa).Format = currencyFormat;
		}
		else if (_isPurchaseOrderDocs)
		{
			Columns.Add("PartnerEMail", ArmSoft.SME.Common.Resources.Email);
		}
		bool flag = Settings.ParametersService.ShowPartnerTaxCode();
		if (_isPurchaseOrderDocs)
		{
			Columns.Add("PartCode", ArmSoft.SME.Common.Resources.Supplier);
			Columns.Add("PartName", ArmSoft.SME.Common.Resources.Name);
			if (flag)
			{
				Columns.Add("PartTaxCode", ArmSoft.SME.Common.Resources.OptionsTaxCode);
			}
		}
		if (_isReceivedTaxInvoices)
		{
			Columns.Add("SummaVAT", ArmSoft.SME.Common.Resources.SummaVAT).FooterSum = true;
			Columns.Add("SummaWithVAT", ArmSoft.SME.Common.Resources.TotalSum).FooterSum = true;
			Columns.Add("TaxInvType", ArmSoft.SME.Common.Resources.TaxAccType);
			Columns.Add("BaseInvDate", ArmSoft.SME.Common.Resources.BaseSubmissionDate);
			Columns.Add("BaseInvSeriaAndNumber", ArmSoft.SME.Common.Resources.BaseInvoiceNumber);
			Columns.Add("CreatedDocsCount", ArmSoft.SME.Common.Resources.CreatedDocsCount);
		}
		if (_isECRCheckDocuments)
		{
			Columns.Add("SummaCash", ArmSoft.SME.Trade.Common.Resources.CashPaymentForPrint).FooterSum = true;
			Columns.Add("SummaBank", ArmSoft.SME.Trade.Common.Resources.PlasticCartPaymentDialog).FooterSum = true;
			Columns.Add("SummaPrePay", ArmSoft.SME.Trade.Common.Resources.PaymentByPrePay).FooterSum = true;
			Columns.Add("SummaBonus", ArmSoft.SME.Trade.Common.Resources.PaymentByBonus).FooterSum = true;
			Columns.Add("SummaCredit", ArmSoft.SME.Trade.Common.Resources.CreditSale).FooterSum = true;
		}
		if (!_isReceivedTaxInvoices)
		{
			if (!_isDeletedDocJournal && !_isSentInvoicesForPayment && !_isReconciliationStatementDoc && !_isDeferedDocuments && !_forPosReturn)
			{
				Columns.Add("DocStateName", ArmSoft.SME.Common.Resources.State);
			}
			if (!_isPurchaseOrderDocs && !_isDeferedDocuments)
			{
				Columns.Add("StorageCaption", ArmSoft.SME.Common.Resources.Storage);
			}
			if (_showCashDeskAndShiftNumber)
			{
				Columns.Add("Cashdesk", ArmSoft.SME.Common.Resources.Cash);
				Columns.Add("CashierShiftNumber", ArmSoft.SME.Trade.Common.Resources.CashierShift);
			}
			if (_isECRCheckDocuments)
			{
				Columns.Add("PaymentMethod", ArmSoft.SME.Common.Resources.PaymentSystem);
				Columns.Add("POSPartnerName", ArmSoft.SME.Trade.Common.Resources.POSTerminalPart);
				Columns.Add("POSPartContractName", ArmSoft.SME.Trade.Common.Resources.POSTerminalContract);
				Columns.Add("QRPaymentID", ArmSoft.SME.Common.Resources.QRPaymentID);
				Columns.Add("QRPaymentAccepted", ArmSoft.SME.Common.Resources.IsPaid);
			}
			if (!_isPurchaseOrderDocs && !_isSentInvoicesForPayment && !_isDeferedDocuments)
			{
				Columns.Add("Info", ArmSoft.SME.Trade.Common.Resources.DocInfo);
			}
			if (!_isDeferedDocuments)
			{
				Columns.Add("Comment", ArmSoft.SME.Common.Resources.Comment);
			}
		}
		if (_isTaxInvoicesDocument || _isPurchaseDocuments || _isTransportationDocuments)
		{
			if (_isPurchaseDocuments)
			{
				Columns.Add("InvoiceNumber", ArmSoft.SME.Common.Resources.InvoiceNumberShort);
			}
			else
			{
				Columns.Add("ISN", ArmSoft.SME.Common.Resources.DocumentISN);
				Columns.Add("InvoiceDate", _isTaxInvoicesDocument ? ArmSoft.SME.Common.Resources.TaxInvoiceDateShort : ArmSoft.SME.Common.Resources.InvoiceDateShort);
				Columns.Add("TaxExportCaption", ArmSoft.SME.Common.Resources.TaxInvoiceExportForm);
			}
		}
		else if (_isTransportationDocuments)
		{
			Columns.Add("InvoiceDate", ArmSoft.SME.Common.Resources.TaxInvoiceDateShort);
		}
		if (_isTaxInvoicesDocument)
		{
			Columns.Add("CreatedDocsCount", ArmSoft.SME.Common.Resources.CreatedDocsCount);
		}
		if (!_isTransportationDocuments && !_isReceivedTaxInvoices && !_isPurchaseOrderDocs)
		{
			Columns.Add("PartCode", ArmSoft.SME.Common.Resources.Partner);
			Columns.Add("PartName", ArmSoft.SME.Common.Resources.PartnerName);
			if (flag && !_isDeferedDocuments && !_forPosReturn)
			{
				Columns.Add("PartTaxCode", ArmSoft.SME.Common.Resources.OptionsTaxCode);
			}
			if (!_isSentInvoicesForPayment && !_isReconciliationStatementDoc && !_isDeferedDocuments && !_forPosReturn)
			{
				Columns.Add("EmplCode", ArmSoft.SME.Common.Resources.Employee);
				Columns.Add("EmplName", ArmSoft.SME.Common.Resources.Name);
			}
		}
		if (_isDSConnectedDocs && _useDiscountServer)
		{
			Columns.Add("ExportedToDiscountServer", ArmSoft.SME.Trade.Common.Resources.ExportedToDiscountServer);
		}
		if (!_isDeferedDocuments && !_forPosReturn)
		{
			Columns.Add("Creator", _isECRCheckDocuments ? ArmSoft.SME.Trade.Common.Resources.LastUser : ArmSoft.SME.Common.Resources.UserName);
		}
		if (!_isReceivedTaxInvoices && !_forPosReturn)
		{
			Columns.Add("DocOwner", ArmSoft.SME.Common.Resources.DocOwner);
		}
		if (!_isReceivedTaxInvoices && !_isDeferedDocuments)
		{
			Columns.AddSorting("DocDate", _isBillDocuments);
			if (_isVisibleOrderNumColumn)
			{
				Columns.AddSorting("OrderNum");
			}
			if (!_isECRCheckDocuments && !_isBillDocuments && !_isTaxInvoicesDocument)
			{
				if (Columns.Contains("DocTypeName"))
				{
					Columns.AddSorting("DocTypeName");
				}
				Columns.AddSorting("DocNum");
			}
		}
		else if (_isDeferedDocuments)
		{
			Columns.AddSorting("DocDate", descending: true);
			Columns.AddSorting("LastModifyDate", descending: true);
		}
	}

	public override DropDownViewParametersDialogWindow CreateDialog()
	{
		return new DropDownViewDialog(this, _owner);
	}

	public override short ValueMemberLength()
	{
		return 255;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Comment;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Comment;
	}

	public override object KeyMember(int index)
	{
		return Data[index].ISN;
	}
}
