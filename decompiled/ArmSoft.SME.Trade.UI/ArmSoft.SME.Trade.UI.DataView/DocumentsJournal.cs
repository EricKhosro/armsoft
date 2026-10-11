using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Ardalis.SmartEnum;
using ArmSoft.Cloud.Client.Discount;
using ArmSoft.ECRConnector;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Models.Exchange;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.DPR;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Cache;
using ArmSoft.SME.Trade.UI.DataView.Reports;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.Services;
using ArmSoft.TaxService.Enums;
using ArmSoft.TaxService.Models;
using ArmSoft.TaxService.Models.AccDocs;
using ArmSoft.TaxService.Models.Invoices;
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Trade.UI.DataView;

public abstract class DocumentsJournal<R, P> : DocumentDataView<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly DocumentsJournal<R, P> _dataView;

		private bool _addSelectCellsTab;

		private DateEditControl _startDate;

		private DateEditControl _endDate;

		private DateEditControl _delDateBegin;

		private DateEditControl _delDateEnd;

		public DataViewDialog(DocumentsJournal<R, P> dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			_addSelectCellsTab = !dataView._isTransportationDocuments && !dataView._isDeferedDocuments && !dataView._isDeletedDocJournal && !dataView._isReceivedTaxInvoices;
			Caption = (dataView._isSelectedBaseDoc ? ArmSoft.SME.Common.Resources.Documents : dataView.InnerCaption);
			Name = dataView.Name;
			HelpId = dataView.HelpId;
			if (dataView._isSelectedBaseDoc)
			{
				Top = dataView.DialogTop;
				Left = dataView.DialogLeft;
				WindowStartupLocation = WindowStartupLocation.Manual;
			}
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_startDate = AddDateControl(container, "DateBegin", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, !dataView._isDeletedDocJournal, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_startDate.EditValue = (useParamValues ? dataView.StartDate : new DateOnly?(Settings.HostingEnvironment.DateBegin));
			_endDate = AddDateControl(container, "DateEnd", "", useLongDate: false, !dataView._isDeletedDocJournal, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_endDate.EditValue = (useParamValues ? dataView.EndDate : new DateOnly?(Settings.HostingEnvironment.DateEnd));
			AddSpecialDateButton(container, _startDate, _endDate);
			if (dataView._isDeletedDocJournal)
			{
				container = AddHorizontalLayoutGroup();
				_delDateBegin = AddDateControl(container, "DelStartDate", ArmSoft.SME.Common.Resources.RemovalDatePeriod, useLongDate: false, isRequired: false, !useParamValues);
				_delDateBegin.EditValue = (useParamValues ? dataView.DelStartDate : ((DateOnly?)null));
				_delDateEnd = AddDateControl(container, "DelEndDate", "", useLongDate: false, isRequired: false, !useParamValues);
				_delDateEnd.EditValue = (useParamValues ? dataView.DelEndDate : ((DateOnly?)null));
				AddSpecialDateButton(container, _delDateBegin, _delDateEnd);
			}
			DXTabControl tc = null;
			LayoutGroup layoutGroup = null;
			if (!isRefreshMode && _addSelectCellsTab)
			{
				tc = AddTabControl(null);
				layoutGroup = AddTabedGroup(tc, ArmSoft.SME.Common.Resources.General);
			}
			if (!dataView._isReceivedTaxInvoices)
			{
				if (!dataView._isPurchaseOrderDocs && !dataView._isSentInvoicesForPayment && !dataView._isReconciliationStatementDoc && !dataView._isBillDocuments)
				{
					AddMultiSelectDropDownControl(layoutGroup, "DocTypes", ArmSoft.SME.Common.Resources.TypeWord, new DocumentType(CommonHelpers.GetDocumentTypesByReportType(dataView._reportType, showReceivedTaxInvoice: true, Settings.ParametersService.UseMarking()), checkViewAccess: true, checkAddAccess: true), isRequired: false, !useParamValues).SelectedString = (useParamValues ? dataView.DocTypeParam : string.Empty);
				}
				if (!dataView._isTransportationDocuments)
				{
					if (!dataView._isECRCheckDocuments && !dataView._isPurchaseOrderDocs && !dataView._isReconciliationStatementDoc && !dataView._isOnlineDocsDataView)
					{
						AddDropDownControl(layoutGroup, "CurrCode", ArmSoft.SME.Common.Resources.Curr, new Currency(shortMode: true), isRequired: false, allowAdditionButtons: false, !useParamValues).Code = (useParamValues ? dataView.CurrCode : string.Empty);
					}
					if (dataView._isCashDocsDataView)
					{
						AddDropDownControl(layoutGroup, "CashCode", ArmSoft.SME.Common.Resources.Cash, new ArmSoft.SME.Trade.UI.DropDown.Cashes(), isRequired: false, allowAdditionButtons: false, !useParamValues).Code = (useParamValues ? dataView.CashCode : string.Empty);
					}
					AddDropDownControl(layoutGroup, "PartnerCode", ArmSoft.SME.Common.Resources.Partner, new PartnersFilter(), isRequired: false, allowAdditionButtons: false, !useParamValues).Code = (useParamValues ? dataView.PartnerCode : string.Empty);
					if (dataView._isWithGoodsFilter)
					{
						AddDropDownControl(layoutGroup, "GoodCode", ArmSoft.SME.Common.Resources.ProductsAndServices, new GoodsFilter(shortMode: false), isRequired: false, allowAdditionButtons: false, !useParamValues).Code = (useParamValues ? dataView.GoodCode : string.Empty);
					}
					_ = _dataView._isOnlineDocsDataView;
					if (dataView._isECRCheckDocuments || dataView._isOnlineDocsDataView)
					{
						LayoutGroup container2 = layoutGroup;
						string storage = ArmSoft.SME.Common.Resources.Storage;
						bool? checkViewOrOperationAccess = true;
						AddDropDownControl(container2, "StorageCode", storage, new ArmSoft.SME.Trade.UI.DropDown.Storages(null, checkViewOrOperationAccess), isRequired: false, allowAdditionButtons: false, !useParamValues).Code = (useParamValues ? dataView.StorageCode : string.Empty);
						AddDropDownControl(layoutGroup, "CashCode", ArmSoft.SME.Common.Resources.Cash + " (" + ArmSoft.SME.Common.Resources.ECR + ")", new ArmSoft.SME.Trade.UI.DropDown.Cashes(withoutCheckAccess: false, showOnlyECRCashes: true), isRequired: false, allowAdditionButtons: false, !useParamValues).Code = (useParamValues ? dataView.CashCode : string.Empty);
					}
					if (!dataView._isPurchaseOrderDocs && !dataView._isSentInvoicesForPayment && !dataView._isReconciliationStatementDoc)
					{
						AddDropDownControl(layoutGroup, "EmplCode", ArmSoft.SME.Common.Resources.Employee, new ArmSoft.SME.Trade.UI.DropDown.Employees(), isRequired: false, allowAdditionButtons: false, !useParamValues).Code = (useParamValues ? dataView.EmplCode : string.Empty);
					}
					if (dataView._isECRCheckDocuments || dataView._isOnlineDocsDataView)
					{
						AddBooleanControl(layoutGroup, "ShowPayments", ArmSoft.SME.Common.Resources.ShowPayments, storeValue: true).IsChecked = useParamValues && dataView.ShowPayments;
						AddBooleanControl(layoutGroup, "ShowOutSummsAsNegatives", ArmSoft.SME.Common.Resources.ShowOutSumNegative, !useParamValues).IsChecked = useParamValues && dataView.ShowOutSummsAsNegatives;
					}
					if (dataView._isTaxInvoicesDocument)
					{
						AddBooleanControl(layoutGroup, "ShowOutSummsAsNegatives", ArmSoft.SME.Common.Resources.ShowOutSumNegative, !useParamValues).IsChecked = useParamValues && dataView.ShowOutSummsAsNegatives;
					}
				}
				if (!dataView._isDeletedDocJournal && !dataView._isSentInvoicesForPayment && !dataView._isReconciliationStatementDoc)
				{
					AddDropDownControl(layoutGroup, "DocStates", ArmSoft.SME.Common.Resources.State, new DocumentState(), isRequired: false, allowAdditionButtons: false, !useParamValues).Code = ((useParamValues && dataView.DocState != null) ? Convert.ToString(dataView.DocState) : Convert.ToString("0"));
				}
			}
			else
			{
				AddBooleanControl(layoutGroup, "ShowOutSummsAsNegatives", ArmSoft.SME.Common.Resources.ShowOutSumNegative, !useParamValues).IsChecked = useParamValues && dataView.ShowOutSummsAsNegatives;
			}
			if (!dataView._isReceivedTaxInvoices)
			{
				DropDownControl dropDownControl = AddDropDownControl(layoutGroup, "DocOwner", ArmSoft.SME.Common.Resources.DocOwner, new UsersFilter(), isRequired: false, allowAdditionButtons: false, storeValue: true, isShowComment: false, 50);
				if (Settings.LoginService.IsAdmin || !Settings.ParametersService.DenyAccessToOtherOwnerDocs())
				{
					dropDownControl.Code = (useParamValues ? _dataView.DocOwner : string.Empty);
				}
				else
				{
					dropDownControl.Code = Settings.LoginService.UserName;
					if (!_dataView._isTransportationDocuments)
					{
						dropDownControl.IsReadOnly = true;
					}
				}
			}
			if (!isRefreshMode && _addSelectCellsTab)
			{
				AddColumnsSelectionTab(tc, layoutGroup, null);
			}
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.StartDate = (DateOnly?)base["DateBegin"];
			_dataView.EndDate = (DateOnly?)base["DateEnd"];
			if (_dataView._isDeletedDocJournal)
			{
				_dataView.DelStartDate = (DateOnly?)base["DelStartDate"];
				_dataView.DelEndDate = (DateOnly?)base["DelEndDate"];
			}
			if (!_dataView._isReceivedTaxInvoices)
			{
				if (!_dataView._isPurchaseOrderDocs && !_dataView._isSentInvoicesForPayment && !_dataView._isReconciliationStatementDoc && !_dataView._isBillDocuments)
				{
					_dataView.DocTypeParam = (string)base["DocTypes"];
				}
				if (!_dataView._isTransportationDocuments)
				{
					if (_dataView._isECRCheckDocuments || _dataView._isOnlineDocsDataView)
					{
						_dataView.ShowPayments = (bool)base["ShowPayments"];
						_dataView.StorageCode = (string)base["StorageCode"];
						_dataView.ShowOutSummsAsNegatives = (bool)base["ShowOutSummsAsNegatives"];
					}
					else if (!_dataView._isReconciliationStatementDoc)
					{
						_dataView.CurrCode = (string)base["CurrCode"];
					}
					if (_dataView._isTaxInvoicesDocument)
					{
						_dataView.ShowOutSummsAsNegatives = (bool)base["ShowOutSummsAsNegatives"];
					}
					if (_dataView._isECRCheckDocuments || _dataView._isCashDocsDataView || _dataView._isOnlineDocsDataView)
					{
						_dataView.CashCode = (string)base["CashCode"];
					}
					_dataView.PartnerCode = (string)base["PartnerCode"];
					if (_dataView._isWithGoodsFilter)
					{
						_dataView.GoodCode = (string)base["GoodCode"];
					}
					_ = _dataView._isOnlineDocsDataView;
					if (!_dataView._isSentInvoicesForPayment && !_dataView._isReconciliationStatementDoc)
					{
						_dataView.EmplCode = (string)base["EmplCode"];
					}
				}
				if (!_dataView._isDeletedDocJournal && !_dataView._isSentInvoicesForPayment && !_dataView._isReconciliationStatementDoc)
				{
					_dataView.DocState = (string)base["DocStates"];
				}
			}
			else
			{
				_dataView.ShowOutSummsAsNegatives = (bool)base["ShowOutSummsAsNegatives"];
			}
			if (!_dataView._isReceivedTaxInvoices)
			{
				_dataView.DocOwner = (string)base["DocOwner"];
			}
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
				ValidationService.ValidateWrongPeriod(_delDateBegin?.EditValue, _delDateEnd?.EditValue);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}
	}

	private ReportTypesSE _reportType;

	private bool _isVisibleOrderNumColumn;

	private bool _isWarehouseDocuments;

	private bool _isPurchaseDocuments;

	private bool _isTaxInvoicesDocument;

	private bool _isReceivedTaxInvoices;

	private bool _isPurchaseOrderDocs;

	private bool _isSentInvoicesForPayment;

	private bool _isReconciliationStatementDoc;

	private bool _isTransportationDocuments;

	private bool _isECRCheckDocuments;

	private bool _isBillDocuments;

	private bool _isSelectedBaseDoc;

	private bool _isDeletedDocJournal;

	private bool _isDSConnectedDocs;

	private bool _isCashDocsDataView;

	private bool _isSaleDocsDataView;

	private bool _isOnlineDocsDataView;

	private bool _isWithGoodsFilter;

	private bool _showCashDeskAndShiftNumber;

	private bool _isDeferedDocuments;

	private readonly bool _useDiscountServer;

	private bool _firstTimeLoad;

	private ECR _ecr;

	private List<ViewColumn> potentiallyColumns;

	private string _invExpType;

	private bool _exportDiscount;

	private bool _exportCaption;

	private bool _isNotRequiredTaxCode;

	private bool _exportEmarkCodes;

	private short _eInvExportType;

	private string _priceType = string.Empty;

	public Guid SelectedRowDocISN { get; set; }

	public double DialogTop { get; set; }

	public double DialogLeft { get; set; }

	public override List<ViewColumn> PotentiallyColumns => potentiallyColumns;

	public override string HelpId
	{
		get
		{
			if (_isTaxInvoicesDocument)
			{
				return "Sales_Journals_TaxInvoicesJournal";
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
				return "Purchases_Journals_ReconcillattionStatementsJournal";
			}
			if (_isReceivedTaxInvoices)
			{
				return "Accounting_Journals_ReceivedTaxInvoices";
			}
			return "Purchases_Journals_Documents";
		}
	}

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

	public DateOnly? DelStartDate
	{
		get
		{
			return (DateOnly?)Parameters["DelStartDate"].Value;
		}
		set
		{
			Parameters["DelStartDate"].Value = value;
		}
	}

	public DateOnly? DelEndDate
	{
		get
		{
			return (DateOnly?)Parameters["DelEndDate"].Value;
		}
		set
		{
			Parameters["DelEndDate"].Value = value;
		}
	}

	public string DocTypeParam
	{
		get
		{
			return Convert.ToString(Parameters["DocType"].Value);
		}
		set
		{
			Parameters["DocType"].Value = value;
		}
	}

	public string CurrCode
	{
		get
		{
			return Convert.ToString(Parameters["CurrCode"].Value);
		}
		set
		{
			Parameters["CurrCode"].Value = value;
		}
	}

	public string PartnerCode
	{
		get
		{
			return Convert.ToString(Parameters["PartnerCode"].Value);
		}
		set
		{
			Parameters["PartnerCode"].Value = value;
		}
	}

	public string GoodCode
	{
		get
		{
			return Convert.ToString(Parameters["GoodCode"].Value);
		}
		set
		{
			Parameters["GoodCode"].Value = value;
		}
	}

	public string StorageCode
	{
		get
		{
			return Convert.ToString(Parameters["StorageCode"].Value);
		}
		set
		{
			Parameters["StorageCode"].Value = value;
		}
	}

	public string CashCode
	{
		get
		{
			return Convert.ToString(Parameters["CashCode"].Value);
		}
		set
		{
			Parameters["CashCode"].Value = value;
		}
	}

	public string EmplCode
	{
		get
		{
			return Convert.ToString(Parameters["EmplCode"].Value);
		}
		set
		{
			Parameters["EmplCode"].Value = value;
		}
	}

	public string DocState
	{
		get
		{
			return Convert.ToString(Parameters["DocState"].Value);
		}
		set
		{
			Parameters["DocState"].Value = value;
		}
	}

	public bool ShowPayments
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowPayments"].Value);
		}
		set
		{
			Parameters["ShowPayments"].Value = value;
		}
	}

	public bool ShowOutSummsAsNegatives
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowOutSummsAsNegatives"].Value);
		}
		set
		{
			Parameters["ShowOutSummsAsNegatives"].Value = value;
		}
	}

	public string DocOwner
	{
		get
		{
			return Convert.ToString(Parameters["DocOwner"].Value);
		}
		set
		{
			Parameters["DocOwner"].Value = value;
		}
	}

	public Guid DocISN
	{
		get
		{
			return (Guid)Parameters["DocISN"].Value;
		}
		set
		{
			Parameters["DocISN"].Value = value;
		}
	}

	public override string Name => _reportType.Name;

	public override string Caption => InnerCaption + " " + StartDate.FormatDDMMYY() + " - " + EndDate.FormatDDMMYY();

	private string InnerCaption => _reportType.Caption;

	public override LoadIndication LoadingIndication => LoadIndication.Always;

	public override bool DialogSupported => true;

	public override bool AllowExport => AllowOperation;

	public override bool AllowDeleteSelected
	{
		get
		{
			if (!AllowOperation || !Settings.ParametersService.AllowGroupDelete())
			{
				return ConfigHelper.ASAdminLogin;
			}
			return true;
		}
	}

	public override bool AllowEditSelected
	{
		get
		{
			if (AllowOperation && !_isReceivedTaxInvoices)
			{
				if (!Settings.LoginService.IsAdmin)
				{
					return !Settings.ParametersService.DenyAccessToOtherOwnerDocs();
				}
				return true;
			}
			return false;
		}
	}

	public override bool AllowAdd
	{
		get
		{
			if (!_isDeletedDocJournal)
			{
				return !_isOnlineDocsDataView;
			}
			return false;
		}
	}

	public override bool AllowCreateCopy
	{
		get
		{
			if (!_isDeletedDocJournal && !_isReceivedTaxInvoices)
			{
				return AllowOperation;
			}
			return false;
		}
	}

	public DocumentsJournal(ReportTypesSE reportType, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE docType = null)
	{
		_firstTimeLoad = true;
		_useDiscountServer = Settings.ParametersService.UseBonus() && Settings.ParametersService.ConnectTob2barmsoftDiscountServer();
		_reportType = reportType;
		_isSelectedBaseDoc = !(docType == null);
		_isWarehouseDocuments = reportType == ReportTypesSE.WarehouseDocument;
		_isPurchaseDocuments = reportType == ReportTypesSE.PurchaseDocument;
		_isVisibleOrderNumColumn = ArmSoft.SME.Common.Extensions.StringExtensions.InList(reportType, ReportTypesSE.AllDocument, ReportTypesSE.DeletedDocument, ReportTypesSE.CashDocument, ReportTypesSE.BankDocument, ReportTypesSE.PurchaseDocument, ReportTypesSE.SaleDocument, ReportTypesSE.WarehouseDocument);
		_isWithGoodsFilter = !ArmSoft.SME.Common.Extensions.StringExtensions.InList(reportType, ReportTypesSE.ReconciliationStatements, ReportTypesSE.CashDocument, ReportTypesSE.BankDocument);
		_isCashDocsDataView = reportType == ReportTypesSE.CashDocument;
		_isSaleDocsDataView = reportType == ReportTypesSE.SaleDocument;
		_isTaxInvoicesDocument = reportType == ReportTypesSE.TaxInvoices;
		_isReceivedTaxInvoices = reportType == ReportTypesSE.ReceivedTaxInvoices;
		_isTransportationDocuments = reportType == ReportTypesSE.TransportationDocument;
		_isReconciliationStatementDoc = reportType == ReportTypesSE.ReconciliationStatements;
		_isECRCheckDocuments = reportType == ReportTypesSE.ECRCheckDocument;
		_isBillDocuments = reportType == ReportTypesSE.BillDocument;
		_isPurchaseOrderDocs = reportType == ReportTypesSE.PurchaseOrderDocument;
		_isSentInvoicesForPayment = reportType == ReportTypesSE.SentInvoicesForPayment;
		_isDeletedDocJournal = reportType == ReportTypesSE.DeletedDocument;
		_isDSConnectedDocs = reportType == ReportTypesSE.AllDocument || reportType == ReportTypesSE.SaleDocument || _isECRCheckDocuments || _isCashDocsDataView;
		_isDeferedDocuments = reportType == ReportTypesSE.DeferredDocument;
		_showCashDeskAndShiftNumber = _isCashDocsDataView || _isDeferedDocuments || _isECRCheckDocuments;
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, null, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, null, "dd/MM/yy");
		if (!_isReceivedTaxInvoices)
		{
			if (ArmSoft.SME.Common.Extensions.StringExtensions.InList(reportType, ReportTypesSE.BillDocument, ReportTypesSE.DeferredDocument) || Settings.LoginService.IsAdmin || !Settings.ParametersService.DenyAccessToOtherOwnerDocs())
			{
				AddParameter("DocOwner", ArmSoft.SME.Common.Resources.DocOwner, null);
			}
			else
			{
				AddParameter("DocOwner", ArmSoft.SME.Common.Resources.DocOwner, Settings.LoginService.UserName);
			}
		}
		if (_isDeletedDocJournal)
		{
			AddParameter("DelStartDate", ArmSoft.SME.Common.Resources.RemovalDatePeriodBegin, null, "dd/MM/yy");
			AddParameter("DelEndDate", ArmSoft.SME.Common.Resources.RemovalDatePeriodEnd, null, "dd/MM/yy");
		}
		if (!_isReceivedTaxInvoices && !_isPurchaseOrderDocs && !_isSentInvoicesForPayment && !_isReconciliationStatementDoc)
		{
			AddParameter("DocType", ArmSoft.SME.Common.Resources.TypeWord, "", "", includeCaptionToolTip: false, new DocumentType(CommonHelpers.GetDocumentTypesByReportType(reportType, showReceivedTaxInvoice: true, Settings.ParametersService.UseMarking())));
		}
		if (!_isTransportationDocuments && !_isReceivedTaxInvoices)
		{
			if (!_isECRCheckDocuments && !_isReconciliationStatementDoc)
			{
				AddParameter("CurrCode", ArmSoft.SME.Common.Resources.Curr, "");
			}
			AddParameter("PartnerCode", ArmSoft.SME.Common.Resources.Partner, "");
			if (_isWithGoodsFilter)
			{
				AddParameter("GoodCode", ArmSoft.SME.Trade.Common.Resources.MaterialOrService, "");
			}
			if (_isECRCheckDocuments)
			{
				AddParameter("StorageCode", ArmSoft.SME.Common.Resources.Storage, "");
			}
			if (_isECRCheckDocuments || _isCashDocsDataView)
			{
				AddParameter("CashCode", ArmSoft.SME.Common.Resources.Cash, "");
			}
			if (!_isSentInvoicesForPayment && !_isReconciliationStatementDoc)
			{
				AddParameter("EmplCode", ArmSoft.SME.Common.Resources.Employee, "");
			}
		}
		if (_isDeferedDocuments)
		{
			AddParameter("CashCode", ArmSoft.SME.Common.Resources.Cash, "");
			AddParameter("ShiftNumber", ArmSoft.SME.Trade.Common.Resources.CashierShiftNum, "");
		}
		if (!_isDeletedDocJournal && !_isReceivedTaxInvoices && !_isSentInvoicesForPayment && !_isReconciliationStatementDoc)
		{
			AddParameter("DocState", ArmSoft.SME.Common.Resources.State, "", "", includeCaptionToolTip: false, new DocumentState());
		}
		if ((_isECRCheckDocuments || _isDeferedDocuments || reportType == ReportTypesSE.SaleDocument || reportType == ReportTypesSE.CashDocument) && (_isECRCheckDocuments || _isOnlineDocsDataView))
		{
			AddParameter("ShowPayments", ArmSoft.SME.Trade.Common.Resources.ShowPayments, false);
			AddParameter("ShowOutSummsAsNegatives", ArmSoft.SME.Common.Resources.ShowOutSumNegative, false);
		}
		if (_isTaxInvoicesDocument || _isReceivedTaxInvoices)
		{
			AddParameter("ShowOutSummsAsNegatives", ArmSoft.SME.Common.Resources.ShowOutSumNegative, false);
		}
		_ = _isOnlineDocsDataView;
		AddParameter("DocISN", ArmSoft.SME.Common.Resources.DocumentISN, Guid.Empty);
		Columns.Add("DocDate", ArmSoft.SME.Common.Resources.DateWord);
		if (_isVisibleOrderNumColumn)
		{
			Columns.Add("OrderNum", ArmSoft.SME.Common.Resources.OrderNumber).Format = DecimalExtension.GetNumberFormat(7, 0);
		}
		if (_isDeferedDocuments)
		{
			Columns.Add("LastModifyDate", ArmSoft.SME.Common.Resources.Time).Format = "H:mm:ss";
		}
		Columns.Add("DocNum", ArmSoft.SME.Common.Resources.DocNumber);
		if (_isECRCheckDocuments || _isBillDocuments)
		{
			Columns.Add("ECRCheckNum", ArmSoft.SME.Common.Resources.ECRCheckNum);
		}
		if (_isTaxInvoicesDocument || _isTransportationDocuments || _isReceivedTaxInvoices)
		{
			if (_isReceivedTaxInvoices)
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
				if (_isTaxInvoicesDocument && Settings.ParametersService.UseMarking())
				{
					Columns.Add("MarkingsExported", ArmSoft.SME.Trade.Common.Resources.EMarkCodesExported);
				}
			}
		}
		else if (!_isPurchaseOrderDocs && !_isSentInvoicesForPayment && !_isReconciliationStatementDoc)
		{
			Columns.Add("DocTypeName", ArmSoft.SME.Common.Resources.TypeWord);
		}
		if (!_isTransportationDocuments && !_isPurchaseOrderDocs && !_isReconciliationStatementDoc && !_isDeferedDocuments)
		{
			Columns.Add("CurrCode", ArmSoft.SME.Common.Resources.Curr);
		}
		_ = _isOnlineDocsDataView;
		if (!_isPurchaseOrderDocs && !_isReconciliationStatementDoc)
		{
			Columns.Add("Summ", ArmSoft.SME.Common.Resources.Summa).FooterSum = true;
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
		if (_isECRCheckDocuments || _isOnlineDocsDataView)
		{
			Columns.Add("SummaCash", ArmSoft.SME.Trade.Common.Resources.CashPaymentForPrint).FooterSum = true;
			Columns.Add("SummaBank", ArmSoft.SME.Trade.Common.Resources.PlasticCartPaymentDialog).FooterSum = true;
			Columns.Add("SummaPrePay", ArmSoft.SME.Common.Resources.PaymentByPrePay).FooterSum = true;
			Columns.Add("SummaBonus", ArmSoft.SME.Trade.Common.Resources.PaymentByBonus).FooterSum = true;
			Columns.Add("SummaCredit", ArmSoft.SME.Trade.Common.Resources.CreditSale).FooterSum = true;
		}
		if (!_isReceivedTaxInvoices)
		{
			if (!_isDeletedDocJournal && !_isSentInvoicesForPayment && !_isReconciliationStatementDoc)
			{
				Columns.Add("DocStateName", ArmSoft.SME.Common.Resources.State);
			}
			if (!_isPurchaseOrderDocs && !_isDeferedDocuments)
			{
				Columns.Add("StorageCaption", ArmSoft.SME.Common.Resources.Storage);
			}
			if (_isTransportationDocuments && Settings.ParametersService.MTMoveWithConfirmation())
			{
				Columns.Add("MoveIsDone", ArmSoft.SME.Trade.Common.Resources.IsDone);
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
		if (!_isTransportationDocuments && !_isReceivedTaxInvoices && !_isPurchaseOrderDocs && !_isDeferedDocuments)
		{
			Columns.Add("PartCode", ArmSoft.SME.Common.Resources.Partner);
			Columns.Add("PartName", ArmSoft.SME.Common.Resources.PartnerName);
			if (flag)
			{
				Columns.Add("PartTaxCode", ArmSoft.SME.Common.Resources.OptionsTaxCode);
			}
			if (!_isSentInvoicesForPayment && !_isReconciliationStatementDoc && !_isDeferedDocuments)
			{
				Columns.Add("EmplCode", ArmSoft.SME.Common.Resources.Employee);
				Columns.Add("EmplName", ArmSoft.SME.Common.Resources.Name);
			}
		}
		if (_isDSConnectedDocs && _useDiscountServer)
		{
			Columns.Add("ExportedToDiscountServer", ArmSoft.SME.Trade.Common.Resources.ExportedToDiscountServer);
		}
		if (!_isDeferedDocuments)
		{
			Columns.Add("Creator", _isECRCheckDocuments ? ArmSoft.SME.Trade.Common.Resources.LastUser : ArmSoft.SME.Common.Resources.UserName);
			Columns.Add("LastModifyDate", ArmSoft.SME.Common.Resources.LastModifyDate);
		}
		if (!_isReceivedTaxInvoices)
		{
			Columns.Add("DocOwner", ArmSoft.SME.Common.Resources.DocOwner);
		}
		potentiallyColumns = new List<ViewColumn>();
		potentiallyColumns.AddRange(Columns.ViewList);
		potentiallyColumns.AddRange(PartnerCache.GetDataViewColumns(ArmSoft.SME.Common.Resources.Partner, "", "", includeCode: false, includeCaption: true, !flag));
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		if (_isECRCheckDocuments || _isOnlineDocsDataView || _isCashDocsDataView)
		{
			if (!_isCashDocsDataView && !TradeSettings.CreateTradeClient().CommonServices.ExistsAvailableStorage())
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.AccessToStoragesDenied, Settings.LoginService.UserName), 48);
			}
			if (!TradeSettings.CreateTradeClient().CommonServices.ExistsAvailableCashDesk())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.AccessToCashesDenied, 48);
			}
		}
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.ReportType = _reportType;
		ParametersProvider.DocISN = DocISN;
		if (_isDeletedDocJournal)
		{
			ParametersProvider.DelStartDate = DelStartDate;
			ParametersProvider.DelEndDate = DelEndDate;
		}
		if (!_isReceivedTaxInvoices)
		{
			if (!_isPurchaseOrderDocs && !_isSentInvoicesForPayment && !_isReconciliationStatementDoc)
			{
				ParametersProvider.DocType = DocTypeParam;
			}
			if (!_isTransportationDocuments)
			{
				if (!_isReconciliationStatementDoc)
				{
					if (_isECRCheckDocuments || _isOnlineDocsDataView)
					{
						ParametersProvider.ShowPayments = ShowPayments;
						ParametersProvider.StorageCode = StorageCode;
						ParametersProvider.ShowOutSummsAsNegatives = ShowOutSummsAsNegatives;
						if (!_isOnlineDocsDataView)
						{
						}
					}
					else if (_isTaxInvoicesDocument)
					{
						ParametersProvider.ShowOutSummsAsNegatives = ShowOutSummsAsNegatives;
					}
					else
					{
						ParametersProvider.CurrCode = CurrCode;
					}
					if (_isECRCheckDocuments || _isCashDocsDataView || _isOnlineDocsDataView)
					{
						ParametersProvider.CashCode = CashCode;
					}
					if (!_isSentInvoicesForPayment && !string.IsNullOrWhiteSpace(EmplCode))
					{
						EmployeeCacheRow item = TradeSettings.TradeCacheService.Employee.GetItem(EmplCode);
						if (item != null)
						{
							ParametersProvider.EmplCode = item.EmployeeCode;
						}
						else
						{
							ParametersProvider.EmplCode = string.Empty;
						}
					}
					else
					{
						ParametersProvider.EmplCode = string.Empty;
					}
				}
				if (!string.IsNullOrWhiteSpace(PartnerCode))
				{
					PartnerCacheRow item2 = TradeSettings.TradeCacheService.Partner.GetItem(PartnerCode);
					if (item2 != null)
					{
						ParametersProvider.PartnerCode = item2.PartCode;
					}
					else
					{
						ParametersProvider.PartnerCode = string.Empty;
					}
				}
				else
				{
					ParametersProvider.PartnerCode = string.Empty;
				}
				if (_isWithGoodsFilter && !string.IsNullOrWhiteSpace(GoodCode))
				{
					MaterialCacheRow item3 = TradeSettings.TradeCacheService.Material.GetItem(GoodCode);
					if (item3 != null)
					{
						ParametersProvider.GoodCode = item3.MTCode;
					}
					else
					{
						ParametersProvider.GoodCode = string.Empty;
					}
				}
				else
				{
					ParametersProvider.GoodCode = string.Empty;
				}
			}
			if (!_isDeletedDocJournal && !_isSentInvoicesForPayment && !_isReconciliationStatementDoc)
			{
				ParametersProvider.DocState = DocState;
			}
			else if (_isSentInvoicesForPayment || _isReconciliationStatementDoc)
			{
				ParametersProvider.DocState = Convert.ToString("0");
			}
		}
		else
		{
			ParametersProvider.DocState = Convert.ToString("0");
			ParametersProvider.ShowOutSummsAsNegatives = ShowOutSummsAsNegatives;
		}
		if (!_isReceivedTaxInvoices)
		{
			ParametersProvider.DocOwner = DocOwner;
		}
		if (_isDeferedDocuments)
		{
			ParametersProvider.DocState = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value.ToString();
			ParametersProvider.CashCode = Parameters["CashCode"].Value.ToString();
			ParametersProvider.ShiftNumber = Parameters["ShiftNumber"].Value.ToString();
		}
	}

	protected override void SelectByCondition(DataViewPanel dvp)
	{
		if (_firstTimeLoad && SelectedRowDocISN != Guid.Empty)
		{
			_firstTimeLoad = false;
			dvp.SelectByCondition(CriteriaOperator.Parse("[ISN] = ?", SelectedRowDocISN), preservePreviousSelection: false);
			dvp.SetFocus();
		}
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("OrderNum", _isVisibleOrderNumColumn);
		dvp.Set_ColumnVisible("InvoiceNumber", _isPurchaseDocuments);
		dvp.Set_ColumnVisible("SummaCash", (_isECRCheckDocuments || _isOnlineDocsDataView) && ShowPayments);
		dvp.Set_ColumnVisible("SummaBank", (_isECRCheckDocuments || _isOnlineDocsDataView) && ShowPayments);
		dvp.Set_ColumnVisible("SummaBonus", (_isECRCheckDocuments || _isOnlineDocsDataView) && ShowPayments);
		dvp.Set_ColumnVisible("SummaCredit", (_isECRCheckDocuments || _isOnlineDocsDataView) && ShowPayments);
		dvp.Set_ColumnVisible("QRPaymentID", (_isECRCheckDocuments || _isOnlineDocsDataView) && ShowPayments);
		dvp.Set_ColumnVisible("QRPaymentAccepted", (_isECRCheckDocuments || _isOnlineDocsDataView) && ShowPayments);
		dvp.Set_ColumnVisible("SummaPrePay", (_isECRCheckDocuments || _isOnlineDocsDataView) && ShowPayments);
		dvp.Set_ColumnVisible("SubmissionDate", _isReceivedTaxInvoices);
		dvp.Set_ColumnVisible("SupplyDate", _isReceivedTaxInvoices);
		dvp.Set_ColumnVisible("SerialAndNumber", _isReceivedTaxInvoices || _isTaxInvoicesDocument || _isTransportationDocuments);
		dvp.Set_ColumnVisible("SupplierTaxCode", _isReceivedTaxInvoices);
		dvp.Set_ColumnVisible("SupplierName", _isReceivedTaxInvoices);
		dvp.Set_ColumnVisible("TaxExportCaption", _isTaxInvoicesDocument || _isTransportationDocuments);
		dvp.Set_ColumnVisible("InvoiceDate", _isTaxInvoicesDocument || _isTransportationDocuments);
		dvp.Set_ColumnVisible("CreatedDocsCount", _isReceivedTaxInvoices);
		dvp.Set_ColumnVisible("SummaVAT", _isReceivedTaxInvoices);
		dvp.Set_ColumnVisible("SummaWithVAT", _isReceivedTaxInvoices);
		dvp.Set_ColumnVisible("TaxInvType", _isReceivedTaxInvoices);
		dvp.Set_ColumnVisible("BaseInvSeriaAndNumber", _isReceivedTaxInvoices);
		dvp.Set_ColumnVisible("BaseInvDate", _isReceivedTaxInvoices);
		dvp.Set_ColumnVisible("BaseInvDate", _isReceivedTaxInvoices);
		dvp.Set_ColumnVisible("DocDate", !_isReceivedTaxInvoices);
		dvp.Set_ColumnVisible("DocNum", !_isReceivedTaxInvoices);
		if (!_isReceivedTaxInvoices && !dvp.IsSorted && !_isDeferedDocuments)
		{
			dvp.Set_ColumnSortOrder("DocDate");
			if (_isVisibleOrderNumColumn)
			{
				dvp.Set_ColumnSortOrder("OrderNum");
			}
			if (!_isECRCheckDocuments && !_isBillDocuments && !_isTaxInvoicesDocument && !_isOnlineDocsDataView)
			{
				if (Columns.Contains("DocTypeName"))
				{
					dvp.Set_ColumnSortOrder("DocTypeName");
				}
				dvp.Set_ColumnSortOrder("DocNum");
			}
		}
		else if (_isDeferedDocuments)
		{
			dvp.Set_ColumnSortOrder("DocDate", ColumnSortOrder.Descending);
			dvp.Set_ColumnSortOrder("LastModifyDate", ColumnSortOrder.Descending);
		}
	}

	public override void EditSelected()
	{
		ValidateSelectedRowsForEditing();
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = ArmSoft.SME.Common.Resources.EditSelected,
			Name = "EditSelected"
		};
		dialogWindow.AddDropDownControl("Owner", ArmSoft.SME.Common.Resources.DocOwner, new UsersFilter(), isRequired: true, allowAdditionButtons: false, storeValue: false, isShowComment: false, 50);
		if (dialogWindow.ShowDialog().Value)
		{
			int num = 1;
			List<PropertyWithValue> list = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list, num);
			CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("Owner", Convert.ToString(dialogWindow["Owner"]));
			EditSelectedRows(list, doRefresh: false, withoutValidation: true);
		}
	}

	public override void Add()
	{
		try
		{
			if (Settings.HostingEnvironment.CurrentViewPanel != null)
			{
				_ = Settings.HostingEnvironment.CurrentViewPanel.Id;
			}
			if (_isTaxInvoicesDocument)
			{
				AddDocument(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value);
			}
			else if (_isPurchaseOrderDocs)
			{
				AddDocument(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder.Value);
			}
			else if (_isSentInvoicesForPayment)
			{
				AddDocument(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.InvoiceForPay.Value);
			}
			else if (_isReconciliationStatementDoc)
			{
				AddDocument(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.Reconciliation.Value);
			}
			else if (_isReceivedTaxInvoices)
			{
				if (TradeTaxServiceHelper.ImportReceivedTaxDocuments())
				{
					Settings.HostingEnvironment.CurrentViewPanel?.Refresh();
				}
			}
			else
			{
				InitContextFunctionsHelper.CreateNewDocFromMenu(CommonHelpers.GetDocumentTypesByReportType(_reportType, showReceivedTaxInvoice: false, Settings.ParametersService.UseMarking()));
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public override void OnScanner(string scannerData)
	{
		if (!_isECRCheckDocuments && !_isSaleDocsDataView)
		{
			return;
		}
		try
		{
			byte[] array = Convert.FromBase64String(scannerData);
			if (array.Length != 0)
			{
				InitContextFunctionsHelper.FindDocInDataViewByBarcode(Encoding.Unicode.GetString(array), "DocNum", 18);
			}
		}
		catch (Exception)
		{
		}
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() == null)
		{
			return;
		}
		DocumentsJournalDataProvider.DataRow dataRow = (DocumentsJournalDataProvider.DataRow)currentViewPanel.FocusedRow();
		if (dataRow != null && !_isReconciliationStatementDoc && AllowOperation)
		{
			DefinitionMappingsService.TryResolveDocumentDefinition(dataRow.DocType, out var definition);
			if ((definition.AllowStoreAsDraft && dataRow.DocState != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value) || (currentViewPanel.GetSelectedAny() && Settings.ParametersService.DraftSelected()))
			{
				currentViewPanel.AddContextFunction("ContextFunctions", "SetDocDraftState", ArmSoft.SME.Common.Resources.SetDocDraft, SetDocDraftState, "", "", storeOpContext: true);
			}
			if ((definition.AllowStoreAsDraft && dataRow.DocState != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Processed.Value) || (currentViewPanel.GetSelectedAny() && Settings.ParametersService.DraftSelected()))
			{
				currentViewPanel.AddContextFunction("ContextFunctions", "SetDocProcessedState", ArmSoft.SME.Common.Resources.SetDocProcess, SetDocProcessedState, "", "", storeOpContext: true);
			}
			if (dataRow.DocState == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && _isVisibleOrderNumColumn)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextSeparator("ContextFunctions");
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ChangeDocumentOrderNumber", ArmSoft.SME.Common.Resources.ChangeDocumentOrderNumber, ChangeDocumentOrderNumber);
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "BringToDayBegin", ArmSoft.SME.Common.Resources.BringToDayBegin, BringToDayBegin);
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "BringToDayEnd", ArmSoft.SME.Common.Resources.BringToDayEnd, BringToDayEnd);
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "DocumentOrderNumberUp", ArmSoft.SME.Common.Resources.Up, DocumentOrderNumberUp, "arrow_up_16.svg");
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "DocumentOrderNumberDown", ArmSoft.SME.Common.Resources.Down, DocumentOrderNumberDown, "arrow_down_16.svg");
			}
			Settings.HostingEnvironment.CurrentViewPanel.AddContextSeparator("ContextFunctions");
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowDocReports", ArmSoft.SME.Common.Resources.Reports, ShowDocReports, "report_16.svg");
		}
		if (AllowOperation)
		{
			if (_isTaxInvoicesDocument)
			{
				if (dataRow.DocType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.InvoiceForPay.Value)
				{
					Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ExportInvoiceForEInvoicing", ArmSoft.SME.Common.Resources.ExportToEInvTaxInv, ExportInvoiceForEInvoicing, "", "", storeOpContext: true);
					Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ExportAccDocsForEInvoicing", ArmSoft.SME.Common.Resources.ExportToEInvAccDocs, ExportAccDocsForEInvoicing, "", "", storeOpContext: true);
					Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "AssignTaxInvNumToDocNum", ArmSoft.SME.Common.Resources.AssignTaxInvNumToDocNum, AssignTaxInvNumToDocNum, "", "", storeOpContext: true);
					if (Settings.ParametersService.UseMarking())
					{
						Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ExportMarkings", ArmSoft.SME.Trade.Common.Resources.ExportEMarkCodes, ExportMarkings);
					}
					if (dataRow.DocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
					{
						Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ExportMTMovDocsForEInvoicing", ArmSoft.SME.Common.Resources.ExportMTMovesToEInv, ExportMTMovDocsForEInvoicing, "", "", storeOpContext: true);
					}
				}
			}
			else if (_isTransportationDocuments)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ExportMTMovDocsForEInvoicing", ArmSoft.SME.Common.Resources.ExportMTMovesToEInv, ExportMTMovDocsForEInvoicing, "", "", storeOpContext: true);
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "AssignTaxInvNumToDocNum", ArmSoft.SME.Common.Resources.AssignTaxInvNumToDocNum, AssignTaxInvNumToDocNum, "", "", storeOpContext: true);
			}
			else if (_isECRCheckDocuments && Settings.ParametersService.AllowChangeECRCheckNum() && (dataRow.ECRCheckNum == "XXXXXXXX" || dataRow.PrePaymentECRCheckNum == "XXXXXXXX"))
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ChangeECRCheckNum", ArmSoft.SME.Common.Resources.ChangeECRCheckNum, ChangeECRCheckNum, "", "", storeOpContext: true);
			}
		}
		if (_isECRCheckDocuments)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ECRPrintLastCheckDuplicate", ArmSoft.SME.Common.Resources.ECRPrintLastCheckDuplicate, ECRPrintLastCheckDuplicate, "", "", storeOpContext: true);
		}
		if (AllowOperation)
		{
			if (_isECRCheckDocuments)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ECRPrintCheckTemplate", ArmSoft.SME.Common.Resources.ECRPrintTemplate, ECRPrintCheckTemplate, "", "", storeOpContext: true);
			}
		}
		else if (_isTaxInvoicesDocument)
		{
			if (dataRow.DocType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.InvoiceForPay.Value)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ExportInvoiceForEInvoicing", ArmSoft.SME.Common.Resources.ExportToEInvTaxInv, ExportInvoiceForEInvoicing, "", "", storeOpContext: true);
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ExportAccDocsForEInvoicing", ArmSoft.SME.Common.Resources.ExportToEInvAccDocs, ExportAccDocsForEInvoicing, "", "", storeOpContext: true);
				if (dataRow.DocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
				{
					Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ExportMTMovDocsForEInvoicing", ArmSoft.SME.Common.Resources.ExportMTMovesToEInv, ExportMTMovDocsForEInvoicing, "", "", storeOpContext: true);
				}
			}
		}
		else if (_isTransportationDocuments)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ExportMTMovDocsForEInvoicing", ArmSoft.SME.Common.Resources.ExportMTMovesToEInv, ExportMTMovDocsForEInvoicing, "", "", storeOpContext: true);
		}
		if (_isDSConnectedDocs && _useDiscountServer && AllowOperation && dataRow.TransactionISN != Guid.Empty)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "DeleteDSTransactionForDoc", ArmSoft.SME.Trade.Common.Resources.RemoveDSTransactions, DeleteDSTransactionForDoc);
		}
	}

	public override void DeleteSelected()
	{
		foreach (DocumentsJournalDataProvider.DataRow selectedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<DocumentsJournalDataProvider.DataRow>())
		{
			if (_isReceivedTaxInvoices || !(Settings.DocumentService.Load(selectedRow.ISN) is TradeDocument tradeDocument) || !(tradeDocument.TransactionISN != Guid.Empty))
			{
				continue;
			}
			if (selectedRow.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill || selectedRow.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer || selectedRow.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn)
			{
				DiscountServerService.DeleteTransactionsByISN(selectedRow.TransactionISN);
				List<PropertyWithValue> properties = new List<PropertyWithValue>
				{
					new PropertyWithValue
					{
						Name = "TransactionISN",
						Value = Guid.Empty.ToString()
					}
				};
				StoreRequest sr = new StoreRequest
				{
					LogComment = ArmSoft.SME.Trade.Common.Resources.DeleteFromDiscountServer,
					WithoutValidation = true,
					WithoutGrids = true,
					WithoutAccessCheck = true
				};
				Settings.DocumentService.EditFields(selectedRow.ISN, properties, sr, selectedRow.TS);
			}
			if (selectedRow.DocType != (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BonusGroupCorrect)
			{
				continue;
			}
			BonusGroupCorrect bonusGroupCorrect = (BonusGroupCorrect)tradeDocument;
			DiscountServerService.DeleteTransactionsByDocISN(selectedRow.ISN);
			tradeDocument.TransactionISN = Guid.Empty;
			foreach (PartnerBonusRemGroupCorrectBase.PartnerBonusRemSpecificationRow partRem in bonusGroupCorrect.PartRemList)
			{
				partRem.TransactionISN = Guid.Empty;
			}
			Settings.DocumentService.Store(bonusGroupCorrect, new StoreRequest
			{
				LogComment = ArmSoft.SME.Trade.Common.Resources.DeleteFromDiscountServer
			});
		}
		DeleteDialog deleteDialog = new DeleteDialog();
		if (!Convert.ToBoolean(deleteDialog.ShowDialog()))
		{
			return;
		}
		List<Guid> list = new List<Guid>();
		foreach (DocumentsJournalDataProvider.DataRow item in from row in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<DocumentsJournalDataProvider.DataRow>()
			orderby row.DocDate descending, row.OrderNum descending
			select row)
		{
			list.Add(item.ISN);
		}
		DeleteSelected(list, deleteDialog.Comment.Text);
	}

	public override void Delete()
	{
		DocumentsJournalDataProvider.DataRow dataRow = (DocumentsJournalDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		DeleteDialog deleteDialog = new DeleteDialog();
		if (!Convert.ToBoolean(deleteDialog.ShowDialog()))
		{
			return;
		}
		if (!_isDeletedDocJournal && !_isReceivedTaxInvoices && Settings.DocumentService.Load(dataRow.ISN) is TradeDocument tradeDocument && tradeDocument.TransactionISN != Guid.Empty)
		{
			if (dataRow.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill || dataRow.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer || dataRow.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn)
			{
				DiscountServerService.DeleteTransactionsByISN(dataRow.TransactionISN);
				List<PropertyWithValue> properties = new List<PropertyWithValue>
				{
					new PropertyWithValue
					{
						Name = "TransactionISN",
						Value = Guid.Empty.ToString()
					}
				};
				StoreRequest sr = new StoreRequest
				{
					LogComment = ArmSoft.SME.Trade.Common.Resources.DeleteFromDiscountServer,
					WithoutValidation = true,
					WithoutGrids = true,
					WithoutAccessCheck = true
				};
				Settings.DocumentService.EditFields(dataRow.ISN, properties, sr, dataRow.TS);
			}
			if (dataRow.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BonusGroupCorrect)
			{
				BonusGroupCorrect bonusGroupCorrect = (BonusGroupCorrect)tradeDocument;
				DiscountServerService.DeleteTransactionsByDocISN(dataRow.ISN);
				tradeDocument.TransactionISN = Guid.Empty;
				foreach (PartnerBonusRemGroupCorrectBase.PartnerBonusRemSpecificationRow partRem in bonusGroupCorrect.PartRemList)
				{
					partRem.TransactionISN = Guid.Empty;
				}
				Settings.DocumentService.Store(bonusGroupCorrect, new StoreRequest
				{
					LogComment = ArmSoft.SME.Trade.Common.Resources.DeleteFromDiscountServer
				});
			}
		}
		Settings.DocumentService.Delete(dataRow.GetDocISN(), new PODeleteRequest
		{
			LogComment = deleteDialog.Comment.Text
		});
		Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.GetDocISN());
	}

	private void DeleteDSTransactionForDoc()
	{
		Mouse.OverrideCursor = Cursors.Wait;
		DocumentsJournalDataProvider.DataRow dataRow = (DocumentsJournalDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null && dataRow.DocState != (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
		{
			if (dataRow.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill || dataRow.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer || dataRow.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn)
			{
				DiscountServerService.DeleteTransactionsByISN(dataRow.TransactionISN);
				List<PropertyWithValue> properties = new List<PropertyWithValue>
				{
					new PropertyWithValue
					{
						Name = "TransactionISN",
						Value = Guid.Empty.ToString()
					}
				};
				StoreRequest sr = new StoreRequest
				{
					LogComment = ArmSoft.SME.Trade.Common.Resources.DeleteFromDiscountServer,
					WithoutValidation = true,
					WithoutGrids = true,
					WithoutAccessCheck = true
				};
				Settings.DocumentService.EditFields(dataRow.ISN, properties, sr, dataRow.TS);
			}
			else if (dataRow.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BonusGroupCorrect)
			{
				BonusGroupCorrect bonusGroupCorrect = (BonusGroupCorrect)Settings.DocumentService.Load(dataRow.ISN);
				DiscountServerService.DeleteTransactionsByDocISN(dataRow.ISN);
				bonusGroupCorrect.TransactionISN = Guid.Empty;
				foreach (PartnerBonusRemGroupCorrectBase.PartnerBonusRemSpecificationRow partRem in bonusGroupCorrect.PartRemList)
				{
					partRem.TransactionISN = Guid.Empty;
				}
				Settings.DocumentService.Store(bonusGroupCorrect, new StoreRequest
				{
					LogComment = ArmSoft.SME.Trade.Common.Resources.DeleteFromDiscountServer
				});
			}
		}
		Mouse.OverrideCursor = null;
	}

	private void SetDocDraftState()
	{
		List<DocumentsJournalDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<DocumentsJournalDataProvider.DataRow>();
		bool flag = InitContextFunctionsHelper.ChangeDocState(selectedOrFocusedRows.Select((DocumentsJournalDataProvider.DataRow p) => p.ISN).ToList(), ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft);
		selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<DocumentsJournalDataProvider.DataRow>();
		if (flag && _useDiscountServer)
		{
			foreach (DocumentsJournalDataProvider.DataRow item in selectedOrFocusedRows)
			{
				if ((item.DocType != (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill && item.DocType != (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer && item.DocType != (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn && item.DocType != (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BonusGroupCorrect) || item.DocState != (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft || !(item.TransactionISN != Guid.Empty))
				{
					continue;
				}
				if (item.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill || item.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer || item.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn)
				{
					DiscountServerService.DeleteTransactionsByISN(item.TransactionISN);
					List<PropertyWithValue> properties = new List<PropertyWithValue>
					{
						new PropertyWithValue
						{
							Name = "TransactionISN",
							Value = Guid.Empty.ToString()
						}
					};
					StoreRequest sr = new StoreRequest
					{
						LogComment = ArmSoft.SME.Trade.Common.Resources.DeleteFromDiscountServer,
						WithoutValidation = true,
						WithoutGrids = true,
						WithoutAccessCheck = true
					};
					Settings.DocumentService.EditFields(item.ISN, properties, sr, item.TS);
				}
				else
				{
					if (item.DocType != (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BonusGroupCorrect)
					{
						continue;
					}
					BonusGroupCorrect bonusGroupCorrect = (BonusGroupCorrect)Settings.DocumentService.Load(item.ISN, item.DocType);
					bonusGroupCorrect.TransactionISN = Guid.Empty;
					foreach (PartnerBonusRemGroupCorrectBase.PartnerBonusRemSpecificationRow partRem in bonusGroupCorrect.PartRemList)
					{
						partRem.TransactionISN = Guid.Empty;
					}
					Settings.DocumentService.Store(bonusGroupCorrect, new StoreRequest
					{
						LogComment = ArmSoft.SME.Trade.Common.Resources.DeleteFromDiscountServer
					});
				}
			}
		}
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		List<object> list = new List<object>();
		foreach (Guid item2 in selectedOrFocusedRows.Select((DocumentsJournalDataProvider.DataRow p) => p.ISN))
		{
			list.Add(item2);
		}
		currentViewPanel.Update(list);
	}

	private void RecordDiscountServerdata(TradeDocument doc)
	{
		List<CardTransactionRequest> list = new List<CardTransactionRequest>();
		BonusGroupCorrect bonusGroupCorrect = null;
		decimal num = 0m;
		decimal num2 = 0m;
		decimal num3 = 0m;
		switch (doc.POType)
		{
		case 18:
		{
			MTBill mTBill = (MTBill)doc;
			num = mTBill.SummaAccumulatedBonus.Value;
			num2 = mTBill.SumAddedBonus.Value;
			num3 = -mTBill.SummaPayByBonus.Value;
			break;
		}
		case 15:
		{
			MTReturnFromCustomer mTReturnFromCustomer = (MTReturnFromCustomer)doc;
			num = -mTReturnFromCustomer.MTList.Sum((MTReturnFromCustomerBase.MTRetFromCustSpecificationRow el) => el.AccumulatedBonus).Value;
			num2 = 0m;
			num3 = mTReturnFromCustomer.MTList.Sum((MTReturnFromCustomerBase.MTRetFromCustSpecificationRow el) => el.UsedBonus).Value;
			break;
		}
		case 38:
		{
			MTBillReturn mTBillReturn = (MTBillReturn)doc;
			num = -mTBillReturn.MTList.Sum((MTReturnFromCustomerBase.MTRetFromCustSpecificationRow el) => el.AccumulatedBonus).Value;
			num2 = 0m;
			num3 = mTBillReturn.MTList.Sum((MTReturnFromCustomerBase.MTRetFromCustSpecificationRow el) => el.UsedBonus).Value;
			break;
		}
		case 30:
			bonusGroupCorrect = (BonusGroupCorrect)doc;
			foreach (PartnerBonusRemGroupCorrectBase.PartnerBonusRemSpecificationRow partRem in bonusGroupCorrect.PartRemList)
			{
				PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(partRem.PartCode, partRem.Contract);
				if (contract != null && contract.IsBonusCard && !string.IsNullOrWhiteSpace(contract.DiscountCard))
				{
					CardTransactionRequest cardTransactionRequest = new CardTransactionRequest();
					cardTransactionRequest.CardNumber = contract.DiscountCard;
					cardTransactionRequest.DocInfo = new CardTransactionRequestDocInfo
					{
						ISN = bonusGroupCorrect.ISN,
						Type = ArmSoft.Cloud.Client.Discount.DocType.BonusCorrection,
						Date = bonusGroupCorrect.DocDate.ToDateTime(),
						Number = bonusGroupCorrect.DocNum,
						Amount = bonusGroupCorrect.Summ.Value,
						CashCode = bonusGroupCorrect.CashDesk,
						ShiftNumber = bonusGroupCorrect.CashierShiftNumber,
						User = Settings.LoginService.UserName
					};
					cardTransactionRequest.Rows = new List<CardTransactionRow>
					{
						new CardTransactionRow
						{
							Op = OpType.Correct,
							Amount = partRem.Summa.Value
						}
					};
					list.Add(cardTransactionRequest);
				}
			}
			break;
		}
		if (doc.POType != 30)
		{
			CardTransactionRequest cardTransactionRequest = new CardTransactionRequest();
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(doc.PartnerID, doc.PartnerContractGroup.ContractCode);
			if (contract != null && contract.IsBonusCard)
			{
				cardTransactionRequest.CardNumber = contract.DiscountCard;
				cardTransactionRequest.DocInfo = new CardTransactionRequestDocInfo
				{
					ISN = doc.ISN,
					Type = ((doc.POType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill) ? ArmSoft.Cloud.Client.Discount.DocType.Sale : ArmSoft.Cloud.Client.Discount.DocType.Return),
					Date = doc.DocDate.ToDateTime(),
					Number = doc.DocNum,
					Amount = doc.Summ.Value,
					CashCode = doc.CashDesk,
					ShiftNumber = doc.CashierShiftNumber,
					User = Settings.LoginService.UserName
				};
				cardTransactionRequest.Rows = new List<CardTransactionRow>();
				if (num != 0m)
				{
					cardTransactionRequest.Rows.Add(new CardTransactionRow
					{
						Op = OpType.Accumulate,
						Amount = num
					});
				}
				if (num2 != 0m)
				{
					cardTransactionRequest.Rows.Add(new CardTransactionRow
					{
						Op = OpType.Accumulate,
						Amount = num2
					});
				}
				if (num3 != 0m)
				{
					cardTransactionRequest.Rows.Add(new CardTransactionRow
					{
						Op = OpType.Use,
						Amount = num3
					});
				}
				CardTransaction cardTransaction = DiscountServerService.RecordDiscountServerTransaction(cardTransactionRequest);
				if (cardTransaction != null)
				{
					doc.TransactionISN = cardTransaction.ISN;
					InitContextFunctionsHelper.UpdateDocTransactionISN(doc, doc.TransactionISN);
				}
			}
			return;
		}
		List<CardTransaction> list2 = DiscountServerService.RecordDiscountServerTransactions(list);
		if (list2.Count > 0 && list2[0] != null)
		{
			bonusGroupCorrect.TransactionISN = list2[0].ISN;
			InitContextFunctionsHelper.UpdateDocTransactionISN(bonusGroupCorrect, list2[0].ISN);
		}
		foreach (CardTransaction item in list2)
		{
			if (item == null)
			{
				continue;
			}
			foreach (PartnerBonusRemGroupCorrectBase.PartnerBonusRemSpecificationRow partRem2 in bonusGroupCorrect.PartRemList)
			{
				if (partRem2.TransactionISN.HasValue && TradeSettings.TradeCacheService.Partner.GetContract(partRem2.PartID.Value, partRem2.Contract).DiscountCard == item.CardNumber)
				{
					partRem2.TransactionISN = item.ISN;
				}
			}
		}
		Dictionary<string, Guid> dictionary = new Dictionary<string, Guid>();
		foreach (CardTransaction item2 in list2)
		{
			dictionary.Add(item2.CardNumber, item2.ISN);
		}
		foreach (PartnerBonusRemGroupCorrectBase.PartnerBonusRemSpecificationRow partRem3 in bonusGroupCorrect.PartRemList)
		{
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(partRem3.PartID.Value, partRem3.Contract);
			if (contract != null && dictionary.ContainsKey(contract.DiscountCard))
			{
				partRem3.TransactionISN = dictionary[contract.DiscountCard];
			}
		}
		Settings.HostingEnvironment.CurrentViewPanel.Update(doc.ISN);
	}

	private void SetDocProcessedState()
	{
		List<DocumentsJournalDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<DocumentsJournalDataProvider.DataRow>();
		if (InitContextFunctionsHelper.ChangeDocState(selectedOrFocusedRows.Select((DocumentsJournalDataProvider.DataRow p) => p.ISN).ToList(), ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Processed) && _useDiscountServer)
		{
			foreach (DocumentsJournalDataProvider.DataRow item in selectedOrFocusedRows)
			{
				if (item.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill || item.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer || item.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn || item.DocType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BonusGroupCorrect)
				{
					TradeDocument tradeDocument = (TradeDocument)Settings.DocumentService.Load(item.ISN, item.DocType);
					if (tradeDocument.State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Processed && tradeDocument.TransactionISN == Guid.Empty)
					{
						RecordDiscountServerdata(tradeDocument);
					}
				}
			}
		}
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		List<object> list = new List<object>();
		foreach (Guid item2 in selectedOrFocusedRows.Select((DocumentsJournalDataProvider.DataRow p) => p.ISN))
		{
			list.Add(item2);
		}
		currentViewPanel.Update(list);
	}

	private void ShowDocReports()
	{
		DocumentsJournalDataProvider.DataRow dataRow = (DocumentsJournalDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow == null)
		{
			return;
		}
		Dictionary<ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE, string> dictionary = new Dictionary<ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE, string>();
		short docType = dataRow.DocType;
		if (docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value || docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value || docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer.Value)
		{
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepSalesAnalysis, ArmSoft.SME.Trade.Common.Resources.MTRepSalesAnalysis);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsRepOperations, ArmSoft.SME.Trade.Common.Resources.MTOperations);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepOperationsRetail, ArmSoft.SME.Trade.Common.Resources.MTOperationsRetail);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartOpJournal, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.BonusOpJournal, ArmSoft.SME.Trade.Common.Resources.BonusOp);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.CashOperations, ArmSoft.SME.Trade.Common.Resources.KassaOp);
		}
		else if (docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value)
		{
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepSalesAnalysis, ArmSoft.SME.Trade.Common.Resources.MTRepSalesAnalysis);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsRepOperations, ArmSoft.SME.Trade.Common.Resources.MTOperations);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartOpJournal, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
		}
		else if (docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value)
		{
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepSalesAnalysis, ArmSoft.SME.Trade.Common.Resources.MTRepSalesAnalysis);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsRepOperations, ArmSoft.SME.Trade.Common.Resources.MTOperations);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepOperationsRetail, ArmSoft.SME.Trade.Common.Resources.MTOperationsRetail);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartOpJournal, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
		}
		else if (docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value || docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeImport.Value || docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTOverheadExp.Value)
		{
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsRepOperations, ArmSoft.SME.Trade.Common.Resources.MTOperations);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartOpJournal, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
		}
		else if (docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value || docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value)
		{
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsRepOperations, ArmSoft.SME.Trade.Common.Resources.MTOperations);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepOperationsRetail, ArmSoft.SME.Trade.Common.Resources.MTOperationsRetail);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartOpJournal, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
		}
		else if (docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.ReceivedService.Value || docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PartnerRemCorrect.Value || docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PartnerRemGroupCorrect.Value || docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PartnerRemMove.Value || docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PartnersGroupRevaluation.Value)
		{
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartOpJournal, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
		}
		else if (docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BonusGroupCorrect.Value)
		{
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.BonusOpJournal, ArmSoft.SME.Trade.Common.Resources.BonusOp);
		}
		else if (docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value || docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value || docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTExpense.Value)
		{
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsRepOperations, ArmSoft.SME.Trade.Common.Resources.MTOperations);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepOperationsRetail, ArmSoft.SME.Trade.Common.Resources.MTOperationsRetail);
		}
		else if (docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTRevaluationRetail.Value)
		{
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepOperationsRetail, ArmSoft.SME.Trade.Common.Resources.MTOperationsRetail);
		}
		else if (docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTRevaluation.Value || docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value || docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTDeComplectation.Value)
		{
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsRepOperations, ArmSoft.SME.Trade.Common.Resources.MTOperations);
		}
		else if (docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut.Value || docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn.Value)
		{
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartOpJournal, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.CashOperations, ArmSoft.SME.Trade.Common.Resources.KassaOp);
		}
		else if (docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankIn.Value || docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankOut.Value)
		{
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartOpJournal, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.BankOperations, ArmSoft.SME.Trade.Common.Resources.BankOp);
		}
		else if (docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.Revaluation.Value)
		{
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartOpJournal, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.CashOperations, ArmSoft.SME.Trade.Common.Resources.KassaOp);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.BankOperations, ArmSoft.SME.Trade.Common.Resources.BankOp);
		}
		else if (docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRem.Value)
		{
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsRepOperations, ArmSoft.SME.Trade.Common.Resources.MTOperations);
			dictionary.Add(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepOperationsRetail, ArmSoft.SME.Trade.Common.Resources.MTOperationsRetail);
		}
		List<PropertyWithValue> parameters = new List<PropertyWithValue>
		{
			new PropertyWithValue("DocISN", dataRow.ISN.ToString()),
			new PropertyWithValue("AdditionalCaption", string.Format(ArmSoft.SME.Common.Resources.DocDescr, SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>.FromValue(dataRow.DocType).Caption, dataRow.DocNum))
		};
		if (dictionary.Count == 0)
		{
			string isNotExistReportForDoc = ArmSoft.SME.Trade.Common.Resources.IsNotExistReportForDoc;
			InlineArray4<object> buffer = default;
			buffer[0] = SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>.FromValue(dataRow.DocType).FullCaption;
			buffer[1] = ArmSoft.SME.Common.Resources.Reports;
			buffer[2] = MessageBoxButton.OK;
			buffer[3] = MessageBoxImage.Asterisk;
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(isNotExistReportForDoc, (ReadOnlySpan<object?>)buffer));
			return;
		}
		if (dictionary.Count == 1)
		{
			Settings.ServiceProvider.GetRequiredService<IDataViewService>().ShowDataView(dictionary.Keys.First().Value, parameters, showDialog: false);
			return;
		}
		ContextMenu contextMenu = new ContextMenu
		{
			FontFamily = Settings.FontFamily,
			FontSize = Settings.GetFontSize()
		};
		foreach (KeyValuePair<ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE, string> report in dictionary)
		{
			MenuItem menuItem = new MenuItem();
			menuItem.Header = report.Value;
			menuItem.Tag = report.Key;
			menuItem.Click += (object sender, RoutedEventArgs e) =>
			{
				if (report.Key == ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepSalesAnalysis)
				{
					parameters.Add(new PropertyWithValue("ShowNCCVATSumm", true.ToString()));
				}
				Settings.ServiceProvider.GetRequiredService<IDataViewService>().ShowDataView(report.Key, parameters, showDialog: false);
			};
			contextMenu.Items.Add(menuItem);
		}
		contextMenu.ToolTip = ArmSoft.SME.Common.Resources.Reports;
		contextMenu.PlacementTarget = Settings.HostingEnvironment.LayoutManager;
		contextMenu.Placement = PlacementMode.Center;
		contextMenu.VerticalOffset = 0.0;
		contextMenu.HorizontalOffset = 0.0;
		contextMenu.IsOpen = true;
	}

	protected override void DocumentLinkedList()
	{
		DocumentsJournalDataProvider.DataRow dataRow = (DocumentsJournalDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			Document document = Settings.DocumentService.Load(dataRow.ISN, dataRow.DocType);
			DocumentLinkedList documentLinkedList = new DocumentLinkedList();
			documentLinkedList.DocNum = dataRow.DocNum;
			documentLinkedList.DocOwner = (Settings.ParametersService.DenyAccessToOtherOwnerDocs() ? dataRow.DocOwner : null);
			documentLinkedList.DocDate = Convert.ToString(dataRow.DocDate);
			documentLinkedList.DocumentTypeName = dataRow.DocTypeName;
			documentLinkedList.DocISN = document.ISN;
			documentLinkedList.BaseDocISN = document.ParentDocs.FirstOrDefault().Key;
			documentLinkedList.AllowOperation = AllowOperation;
			documentLinkedList.Show();
		}
	}

	protected override void CreateChildDocument(short childDocType)
	{
		base.CreateChildDocument(childDocType);
		List<DocumentsJournalDataProvider.DataRow> list = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<DocumentsJournalDataProvider.DataRow>().ToList();
		DocumentsJournalDataProvider.DataRow dataRow = (DocumentsJournalDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		try
		{
			Document document = Settings.DocumentService.Load(dataRow.ISN, dataRow.DocType);
			if (dataRow.DocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.ReceivedTaxInvoice.Value)
			{
				if (dataRow.CreatedDocsCount != 0 && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.AlreadyDocBased, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
				{
					return;
				}
				ReceivedTaxInvoice receivedTaxInvoice = (ReceivedTaxInvoice)document;
				if (receivedTaxInvoice == null)
				{
					throw new ASException(ArmSoft.SME.Common.Resources.BaseDocNotExist);
				}
				foreach (TaxDocument.SpecificationRow item in receivedTaxInvoice.Items)
				{
					if (string.IsNullOrWhiteSpace(item.ItemCode))
					{
						if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ExistsRowsWithoutCodeMapping, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
						{
							return;
						}
						break;
					}
				}
				foreach (DocumentsJournalDataProvider.DataRow item2 in list)
				{
					document = Settings.DocumentService.Load(item2.ISN, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.ReceivedTaxInvoice.Value);
					receivedTaxInvoice = (ReceivedTaxInvoice)document;
					if (receivedTaxInvoice == null)
					{
						continue;
					}
					if (receivedTaxInvoice.IsCorrector)
					{
						if (childDocType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value)
						{
							throw new ASException(ArmSoft.SME.Trade.Common.Resources.CreateRetToSuppForCorrInv);
						}
					}
					else if (childDocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value)
					{
						throw new ASException(ArmSoft.SME.Trade.Common.Resources.ReturnToSuppvalidate);
					}
				}
			}
			if (_isReceivedTaxInvoices && childDocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.ReceivedService.Value && list.Count > 1)
			{
				InitContextFunctionsHelper.GroupCreateNewReceivedServices();
			}
			else if (_isReceivedTaxInvoices && (childDocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value || childDocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value) && list.Count > 1)
			{
				InitContextFunctionsHelper.GroupCreateNewMTIncomes((ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE)(SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>)childDocType);
			}
			else if (childDocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut.Value && list.Count > 1)
			{
				InitContextFunctionsHelper.GroupCreateNewCashOutDoc();
			}
			else if (childDocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd.Value && list.Count > 1)
			{
				InitContextFunctionsHelper.GroupCreateNewPayOrderDoc();
			}
			else
			{
				CreateBasedOnDocHelper.CreateDocument((ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE)(SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>)childDocType, dataRow.ISN, Settings.HostingEnvironment.CurrentViewPanel.Id);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ChangeDocumentOrderNumber()
	{
		DocumentsJournalDataProvider.DataRow dataRow = (DocumentsJournalDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null && new ChangeDayOrderWindow(dataRow.ISN).Invoke().Value)
		{
			Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.ISN);
		}
	}

	private void BringToDayBegin()
	{
		try
		{
			InitContextFunctionsHelper.EditDocumentOrderNumbers(ChangeDocOrderNumTypesSE.BringToDayBegin);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void BringToDayEnd()
	{
		try
		{
			InitContextFunctionsHelper.EditDocumentOrderNumbers(ChangeDocOrderNumTypesSE.BringToDayEnd);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void DocumentOrderNumberUp()
	{
		try
		{
			DocumentsJournalDataProvider.DataRow dataRow = (DocumentsJournalDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				int previousFreeOrderNumber = TradeSettings.CreateTradeClient().CommonServices.GetPreviousFreeOrderNumber(dataRow.ISN, dataRow.OrderNum, dataRow.DocDate);
				if (previousFreeOrderNumber != dataRow.OrderNum)
				{
					TradeSettings.CreateTradeClient().CommonServices.ChangeDocumentOrderNumber(dataRow.ISN, previousFreeOrderNumber, updateMaxOrderNum: false);
					Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.ISN);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void DocumentOrderNumberDown()
	{
		try
		{
			DocumentsJournalDataProvider.DataRow dataRow = (DocumentsJournalDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				int nextFreeOrderNumber = TradeSettings.CreateTradeClient().CommonServices.GetNextFreeOrderNumber(dataRow.ISN, dataRow.OrderNum, dataRow.DocDate);
				if (nextFreeOrderNumber != dataRow.OrderNum)
				{
					TradeSettings.CreateTradeClient().CommonServices.ChangeDocumentOrderNumber(dataRow.ISN, nextFreeOrderNumber);
					Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.ISN);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ChangeECRCheckNum()
	{
		DocumentsJournalDataProvider.DataRow dataRow = (DocumentsJournalDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (!MemoryExtensions.Contains<short>((ReadOnlySpan<short>)new short[5]
		{
			ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value,
			ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value,
			ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn.Value,
			ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut.Value,
			ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer.Value
		}, dataRow.DocType))
		{
			return;
		}
		TradeDocument tradeDocument = (TradeDocument)Settings.DocumentService.Load(dataRow.ISN);
		if (tradeDocument == null)
		{
			return;
		}
		List<PropertyWithValue> list = new List<PropertyWithValue>();
		string text = "";
		string name = "";
		string text2;
		string name2;
		switch (dataRow.DocType)
		{
		default:
			return;
		case 18:
			text2 = ((MTBill)tradeDocument).ECRCheckNum;
			text = ((MTBill)tradeDocument).PrePaymentECRCheckNum;
			name2 = "ECRCheckNum";
			name = "PrePaymentECRCheckNum";
			break;
		case 38:
			text2 = ((MTBillReturn)tradeDocument).ECRCheckNum;
			text = ((MTBillReturn)tradeDocument).PrePaymentECRCheckNum;
			name2 = "ECRCheckNum";
			name = "PrePaymentECRCheckNum";
			break;
		case 2:
			text2 = ((CashIn)tradeDocument).ECRCheckNum;
			name2 = "ECRCheckNum";
			break;
		case 3:
			text2 = ((CashOut)tradeDocument).ECRReturnCheckNum;
			name2 = "ECRReturnCheckNum";
			break;
		case 15:
			text2 = ((MTReturnFromCustomer)tradeDocument).ECRReturnCheckNum;
			name2 = "ECRReturnCheckNum";
			break;
		}
		if (text2 != "XXXXXXXX" && text != "XXXXXXXX")
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.RowChangedWhenEdited);
			return;
		}
		DialogWindow dialogWindow = new DialogWindow();
		if (text2 == "XXXXXXXX" && text != "XXXXXXXX")
		{
			TextEditExt textEditExt = dialogWindow.AddStringControl("ECRCheckNum", ArmSoft.SME.Common.Resources.ECRCheckNum, 8);
			dialogWindow.Caption = ArmSoft.SME.Common.Resources.ChangeECRCheckNum;
			textEditExt.Text = text2;
			if (dialogWindow.ShowDialog().Value)
			{
				string text3 = Convert.ToString(dialogWindow["ECRCheckNum"]);
				list.Add(new PropertyWithValue(name2, text3));
				ECRHelpers.ChangeDocECRCheckInfo(text3, string.Empty, tradeDocument.ISN, list, tradeDocument.TS, edited: true);
				Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.ISN);
			}
		}
		else if (text == "XXXXXXXX" && text2 != "XXXXXXXX")
		{
			TextEditExt textEditExt2 = dialogWindow.AddStringControl("ECRCheckNum2", ArmSoft.SME.Trade.Common.Resources.PrePaymentECRCheckNum, 8);
			dialogWindow.Caption = ArmSoft.SME.Common.Resources.ChangeECRCheckNum;
			textEditExt2.Text = text;
			if (dialogWindow.ShowDialog().Value)
			{
				string text4 = Convert.ToString(dialogWindow["ECRCheckNum2"]);
				list.Add(new PropertyWithValue(name, text4));
				ECRHelpers.ChangeDocECRCheckInfo(text4, string.Empty, tradeDocument.ISN, list, tradeDocument.TS, edited: true);
				Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.ISN);
			}
		}
		else if (text2 == "XXXXXXXX" && text == "XXXXXXXX")
		{
			TextEditExt textEditExt3 = dialogWindow.AddStringControl("ECRCheckNum", ArmSoft.SME.Common.Resources.ECRCheckNum, 8);
			dialogWindow.Caption = ArmSoft.SME.Common.Resources.ChangeECRCheckNum;
			textEditExt3.IsEnabled = text2 == "XXXXXXXX";
			textEditExt3.Text = text2;
			TextEditExt textEditExt4 = dialogWindow.AddStringControl("ECRCheckNum2", ArmSoft.SME.Trade.Common.Resources.PrePaymentECRCheckNum, 8);
			textEditExt4.IsEnabled = text == "XXXXXXXX";
			textEditExt4.Text = text;
			if (dialogWindow.ShowDialog().Value)
			{
				string text5 = Convert.ToString(dialogWindow["ECRCheckNum"]);
				string text6 = Convert.ToString(dialogWindow["ECRCheckNum2"]);
				list.Add(new PropertyWithValue(name2, text5));
				tradeDocument.TS = ECRHelpers.ChangeDocECRCheckInfo(text5, string.Empty, tradeDocument.ISN, list, tradeDocument.TS, edited: true);
				list.Clear();
				list.Add(new PropertyWithValue(name, text6));
				ECRHelpers.ChangeDocECRCheckInfo(text6, string.Empty, tradeDocument.ISN, list, tradeDocument.TS, edited: true);
				Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.ISN);
			}
		}
	}

	private void ECRPrintLastCheckDuplicate()
	{
		ECRPrintLastCheckDuplicateOrTemplate(printTemplate: false);
	}

	private void ECRPrintCheckTemplate()
	{
		ECRPrintLastCheckDuplicateOrTemplate(printTemplate: true);
	}

	private void ECRPrintLastCheckDuplicateOrTemplate(bool printTemplate)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		try
		{
			DocumentsJournalDataProvider.DataRow dataRow = (DocumentsJournalDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow == null)
			{
				return;
			}
			string text = "";
			switch (dataRow.DocType)
			{
			case 18:
			{
				MTBill mTBill = (MTBill)Settings.DocumentService.Load(dataRow.ISN);
				if (mTBill != null)
				{
					text = mTBill.CashDesk;
				}
				break;
			}
			case 2:
			{
				CashIn cashIn = (CashIn)Settings.DocumentService.Load(dataRow.ISN);
				if (cashIn != null)
				{
					text = cashIn.CashDesk;
				}
				break;
			}
			case 3:
			{
				CashOut cashOut = (CashOut)Settings.DocumentService.Load(dataRow.ISN);
				if (cashOut != null)
				{
					text = cashOut.CashDesk;
				}
				break;
			}
			case 15:
			{
				MTReturnFromCustomer mTReturnFromCustomer = (MTReturnFromCustomer)Settings.DocumentService.Load(dataRow.ISN);
				if (mTReturnFromCustomer != null)
				{
					text = mTReturnFromCustomer.CashDesk;
				}
				break;
			}
			case 38:
			{
				MTBillReturn mTBillReturn = (MTBillReturn)Settings.DocumentService.Load(dataRow.ISN);
				if (mTBillReturn != null)
				{
					text = mTBillReturn.CashDesk;
				}
				break;
			}
			}
			CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(text);
			if (item == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.NotInKassa, text));
			}
			if (string.IsNullOrWhiteSpace(item.ECROperator) || string.IsNullOrWhiteSpace(item.ECROperatorPassword))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidateECROperatorOrECROperatorPin, text), ArmSoft.SME.Common.Resources.ECRPrintLastCheckDuplicate, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			if (!item.ECROperator.IsNumeric())
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.ECRPrintLastCheckDuplicate, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			ECRHelpers.InitECR(ref _ecr, text);
			if (_ecr == null)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.ECRPrintLastCheckDuplicate, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			_ecr.Connect();
			flag = true;
			_ecr.OperatorLogin(int.Parse(item.ECROperator), item.ECROperatorPassword);
			flag2 = true;
			if (printTemplate)
			{
				_ecr.PrintTemplate();
			}
			else
			{
				_ecr.PrintLastReceipt();
			}
		}
		catch (Utilities.ECRException ex)
		{
			Settings.ErrorHandler(new ASException(ex.Message));
		}
		catch (Exception ex2)
		{
			Settings.ErrorHandler(ex2);
		}
		finally
		{
			try
			{
				if (flag)
				{
					if (flag2)
					{
						_ecr.OperatorLogout();
					}
					_ecr.DisConnect();
				}
			}
			catch
			{
			}
		}
	}

	private void ExportInvoiceForEInvoicing()
	{
		ExportInvoicesForEInvoicing();
	}

	private void ExportAccDocsForEInvoicing()
	{
		ExportInvoicesForEInvoicing(isAccDoc: true);
	}

	public void ExportInvoicesForEInvoicing(bool isAccDoc = false)
	{
		List<MTInvoice> invoices = new List<MTInvoice>();
		List<MTInvoiceRetail> retailInvoices = new List<MTInvoiceRetail>();
		List<MTReturnFromCustomer> correctionInvoices = new List<MTReturnFromCustomer>();
		List<MTBill> bill = new List<MTBill>();
		string text = "";
		GroupDocsByTypeForExport(ref invoices, ref retailInvoices, ref correctionInvoices, ref bill);
		bool exportInNCC = AllowExportInNCCDialog(invoices);
		bool isReasonableUpdateDoc = false;
		if (((!isAccDoc || !CheckAccDocsForExportEInvoice(invoices, correctionInvoices, retailInvoices, bill, exportInNCC)) && (isAccDoc || !CheckInvoicesForExportEInvoice(invoices, correctionInvoices, retailInvoices, bill, exportInNCC))) || !GetInvExportType(isAccDoc))
		{
			return;
		}
		Mouse.OverrideCursor = Cursors.Wait;
		EnumerateErrorsReportViewer errorRepViewer = new EnumerateErrorsReportViewer(ArmSoft.SME.Common.Resources.ASerrWindow, "ExportInvoicesToTaxService");
		List<Service> serviceInvoices = new List<Service>();
		List<Good> goodInvoices = new List<Good>();
		List<AccDocServices> servAccDocs = new List<AccDocServices>();
		List<AccDocGoods> goodAccDocs = new List<AccDocGoods>();
		List<Guid> list = new List<Guid>();
		List<(TaxDocument, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE)> list2 = new List<(TaxDocument, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE)>();
		string logComment = "";
		if (_eInvExportType == TaxServiceSourceTypeSE.File.Value)
		{
			text = Settings.TaxServiceHelper.ShowSaveFileDialog(isAccDoc ? "AccDocExport" : "InvoiceExport");
			if (!string.IsNullOrWhiteSpace(text))
			{
				list2 = ExportInvoicesToTaxService(ref errorRepViewer, invoices, correctionInvoices, retailInvoices, bill, ref isReasonableUpdateDoc, isAccDoc, ref serviceInvoices, ref goodInvoices, ref servAccDocs, ref goodAccDocs);
				Settings.TaxService.Authenticate();
				if (isAccDoc)
				{
					Settings.TaxService.Client.Dispatcher.CreateAccDocsXMLFile(text, goodAccDocs, servAccDocs);
				}
				else
				{
					Settings.TaxService.Client.Dispatcher.CreateInvoicesXMLFile(text, goodInvoices, null, serviceInvoices);
				}
				List<Guid> list3 = new List<Guid>();
				list3.AddRange(goodAccDocs.Select((AccDocGoods p) => p.Id));
				list3.AddRange(servAccDocs.Select((AccDocServices p) => p.Id));
				list3.AddRange(goodInvoices.Select((Good p) => p.Id));
				list3.AddRange(serviceInvoices.Select((Service p) => p.Id));
				list = list3;
				logComment = (isAccDoc ? ArmSoft.SME.Common.Resources.ExportAccDocToXML_For_e_Invoicing : ArmSoft.SME.Common.Resources.ExportToXmlInvoices);
			}
		}
		else if (_eInvExportType == TaxServiceSourceTypeSE.EInvoicing.Value && Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
		{
			list2 = ExportInvoicesToTaxService(ref errorRepViewer, invoices, correctionInvoices, retailInvoices, bill, ref isReasonableUpdateDoc, isAccDoc, ref serviceInvoices, ref goodInvoices, ref servAccDocs, ref goodAccDocs);
			if (isAccDoc)
			{
				foreach (AccDocGoods accDocGood in goodAccDocs)
				{
					try
					{
						Settings.TaxService.Client.AccDocGoods.CreateDraft(accDocGood, rewrite: false, fillDataFromBase: true);
						list.Add(accDocGood.Id);
					}
					catch (Exception ex)
					{
						list2.RemoveAll(((TaxDocument taxDoc, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE docType) x) => x.taxDoc.ISN == accDocGood.Id);
						errorRepViewer.AddError(ex.Message, accDocGood.Id);
					}
				}
				foreach (AccDocServices accDocService in servAccDocs)
				{
					try
					{
						Settings.TaxService.Client.AccDocServices.CreateDraft(accDocService);
						list.Add(accDocService.Id);
					}
					catch (Exception ex2)
					{
						list2.RemoveAll(((TaxDocument taxDoc, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE docType) x) => x.taxDoc.ISN == accDocService.Id);
						errorRepViewer.AddError(ex2.Message, accDocService.Id);
					}
				}
			}
			else
			{
				foreach (Good goodInvoice in goodInvoices)
				{
					try
					{
						Settings.TaxService.Client.Goods.CreateDraft(goodInvoice, rewrite: false, fillDataFromBase: true);
						list.Add(goodInvoice.Id);
					}
					catch (Exception ex3)
					{
						list2.RemoveAll(((TaxDocument taxDoc, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE docType) x) => x.taxDoc.ISN == goodInvoice.Id);
						errorRepViewer.AddError(ex3.Message, goodInvoice.Id);
					}
				}
				foreach (Service serviceInvoice in serviceInvoices)
				{
					try
					{
						Settings.TaxService.Client.Services.CreateDraft(serviceInvoice);
						list.Add(serviceInvoice.Id);
					}
					catch (Exception ex4)
					{
						list2.RemoveAll(((TaxDocument taxDoc, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE docType) x) => x.taxDoc.ISN == serviceInvoice.Id);
						errorRepViewer.AddError(ex4.Message, serviceInvoice.Id);
					}
				}
			}
			logComment = ArmSoft.SME.Common.Resources.ExportToTaxProgram;
		}
		Mouse.OverrideCursor = null;
		ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(isAccDoc ? ArmSoft.SME.Trade.Common.Resources.AccDocExportedCount : ArmSoft.SME.Common.Resources.TaxInvExportedCount, list.Count), isAccDoc ? ArmSoft.SME.Common.Resources.ExportAccDocToEInv : ArmSoft.SME.Common.Resources.ExportInvoiceToEInv, MessageBoxButton.OK, MessageBoxImage.Asterisk);
		if (list.Count > 0)
		{
			List<string> exportedInvoicesWithRowISNDict = (from x in (from x in list.Select((Guid isn) =>
					{
						Dictionary<int, Guid> item = invoices.FirstOrDefault((MTInvoice i) => i.ISN == isn)?.ROWISNDict ?? retailInvoices.FirstOrDefault((MTInvoiceRetail r) => r.ISN == isn)?.ROWISNDict ?? bill.FirstOrDefault((MTBill b) => b.ISN == isn)?.ROWISNDict;
						return (ISN: isn, RowISNDict: item);
					})
					where x.RowISNDict != null
					select x).ToList()
				select string.Join(",", new string[1] { x.ISN.ToString() }.Concat(x.RowISNDict.SelectMany((KeyValuePair<int, Guid> kvp) => new string[2]
				{
					kvp.Key.ToString(),
					kvp.Value.ToString()
				})))).ToList();
			List<Guid> corrections = list.Where((Guid isn) => correctionInvoices.Any((MTReturnFromCustomer x) => x.ISN == isn)).ToList();
			ChangeTaxInvoiceFieldsRequestModel request = new ChangeTaxInvoiceFieldsRequestModel
			{
				ExportedInvoicesWithRowISNDict = exportedInvoicesWithRowISNDict,
				Corrections = corrections,
				ExportedType = TaxInvoicesWriteTypesSE.TaxInvExportFormElectronic.Value,
				LogComment = logComment
			};
			DocumentEditFieldsResponseModel documentEditFieldsResponseModel = DataProcessingRequestHelper.Execute<ChangeTaxInvoiceFieldsRequestModel, DocumentEditFieldsResponseModel>(DPRType.Other, "ChangeTaxInvoiceFields", request);
			if (documentEditFieldsResponseModel.Exceptions != null && documentEditFieldsResponseModel.Exceptions.Count != 0)
			{
				documentEditFieldsResponseModel.Exceptions.ForEach((DocumentEditResponseModel documentEditResponseModel) =>
				{
					errorRepViewer.AddError(documentEditResponseModel.Exception.Message, documentEditResponseModel.DocISN);
				});
			}
		}
		if (_exportEmarkCodes)
		{
			List<Guid> list4 = ExportMarkingsToTaxService(list2, _eInvExportType, text, errorRepViewer);
			if (list4 != null)
			{
				UpdateDocExporteMarkingsdState(errorRepViewer, list4, _eInvExportType);
			}
		}
		if (isReasonableUpdateDoc && Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			List<object> list5 = new List<object>();
			list5.AddRange(((IEnumerable<MTInvoice>)invoices).Select((Func<MTInvoice, object>)((MTInvoice x) => x.ISN)));
			list5.AddRange(((IEnumerable<MTInvoiceRetail>)retailInvoices).Select((Func<MTInvoiceRetail, object>)((MTInvoiceRetail x) => x.ISN)));
			list5.AddRange(((IEnumerable<MTReturnFromCustomer>)correctionInvoices).Select((Func<MTReturnFromCustomer, object>)((MTReturnFromCustomer x) => x.ISN)));
			list5.AddRange(((IEnumerable<MTBill>)bill).Select((Func<MTBill, object>)((MTBill x) => x.ISN)));
			Settings.HostingEnvironment.CurrentViewPanel.Update(list5);
		}
		if (errorRepViewer.HasErrors)
		{
			errorRepViewer.Show();
		}
	}

	public void GroupDocsByTypeForExport(ref List<MTInvoice> taxInvoices, ref List<MTInvoiceRetail> taxInvoicesRetail, ref List<MTReturnFromCustomer> taxCorrectionInvoices, ref List<MTBill> taxBill)
	{
		if (!Settings.HostingEnvironment.CurrentViewPanel.GetSelectedAny())
		{
			DocumentsJournalDataProvider.DataRow dataRow = (DocumentsJournalDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow == null)
			{
				return;
			}
			if (dataRow.DocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer.Value)
			{
				MTReturnFromCustomer mTReturnFromCustomer = Settings.DocumentService.Load<MTReturnFromCustomer>(dataRow.ISN, dataRow.DocType);
				if (mTReturnFromCustomer == null)
				{
					return;
				}
				if (mTReturnFromCustomer.State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value)
				{
					foreach (MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mT in mTReturnFromCustomer.MTList)
					{
						if (mT.Summa.Value == 0m)
						{
							throw new ASException(ArmSoft.SME.Trade.Common.Resources.ExistsRowWithZeroAmount);
						}
					}
				}
				if (string.IsNullOrWhiteSpace(mTReturnFromCustomer.BaseDocTaxInvoiceNumber) || string.IsNullOrWhiteSpace(mTReturnFromCustomer.BaseDocTaxInvoiceSeries) || !mTReturnFromCustomer.BaseDocTaxInvoiceDate.HasValue)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ExistsMTRetWithOutBaseInvNumber);
				}
				taxCorrectionInvoices.Add(mTReturnFromCustomer);
				return;
			}
			if (dataRow.DocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value)
			{
				MTInvoice mTInvoice = Settings.DocumentService.Load<MTInvoice>(dataRow.ISN, dataRow.DocType);
				if (mTInvoice == null)
				{
					return;
				}
				if (mTInvoice.State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value)
				{
					foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT2 in mTInvoice.MTList)
					{
						if (mT2.Summa == 0m)
						{
							throw new ASException(ArmSoft.SME.Trade.Common.Resources.ExistsRowWithZeroAmount);
						}
					}
				}
				taxInvoices.Add(mTInvoice);
				return;
			}
			if (dataRow.DocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
			{
				MTBill mTBill = Settings.DocumentService.Load<MTBill>(dataRow.ISN, dataRow.DocType);
				if (mTBill == null)
				{
					return;
				}
				if (mTBill.State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value)
				{
					foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT3 in mTBill.MTList)
					{
						if (mT3.Summa == 0m)
						{
							throw new ASException(ArmSoft.SME.Trade.Common.Resources.ExistsRowWithZeroAmount);
						}
					}
				}
				taxBill.Add(mTBill);
				return;
			}
			MTInvoiceRetail mTInvoiceRetail = Settings.DocumentService.Load<MTInvoiceRetail>(dataRow.ISN, dataRow.DocType);
			if (mTInvoiceRetail == null)
			{
				return;
			}
			if (mTInvoiceRetail.State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value)
			{
				foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT4 in mTInvoiceRetail.MTList)
				{
					if (mT4.Summa == 0m)
					{
						throw new ASException(ArmSoft.SME.Trade.Common.Resources.ExistsRowWithZeroAmount);
					}
				}
			}
			taxInvoicesRetail.Add(mTInvoiceRetail);
			return;
		}
		foreach (DocumentsJournalDataProvider.DataRow selectedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<DocumentsJournalDataProvider.DataRow>())
		{
			if (selectedRow.DocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value)
			{
				MTInvoice mTInvoice = Settings.DocumentService.Load<MTInvoice>(selectedRow.ISN, selectedRow.DocType);
				if (mTInvoice == null)
				{
					continue;
				}
				if (mTInvoice.State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value)
				{
					foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT5 in mTInvoice.MTList)
					{
						if (mT5.Summa == 0m)
						{
							throw new ASException(ArmSoft.SME.Common.Resources.ExistsRowWithZeroAmountInSelection);
						}
					}
				}
				taxInvoices.Add(mTInvoice);
				continue;
			}
			if (selectedRow.DocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer.Value)
			{
				MTReturnFromCustomer mTReturnFromCustomer = Settings.DocumentService.Load<MTReturnFromCustomer>(selectedRow.ISN, selectedRow.DocType);
				if (mTReturnFromCustomer == null)
				{
					continue;
				}
				if (mTReturnFromCustomer.State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value)
				{
					foreach (MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mT6 in mTReturnFromCustomer.MTList)
					{
						if (mT6.Summa.Value == 0m)
						{
							throw new ASException(ArmSoft.SME.Trade.Common.Resources.ExistsRowWithZeroAmount);
						}
					}
				}
				if (string.IsNullOrWhiteSpace(mTReturnFromCustomer.BaseDocTaxInvoiceNumber) || string.IsNullOrWhiteSpace(mTReturnFromCustomer.BaseDocTaxInvoiceSeries) || !mTReturnFromCustomer.BaseDocTaxInvoiceDate.HasValue)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ExistsMTRetWithOutBaseInvNumber);
				}
				taxCorrectionInvoices.Add(mTReturnFromCustomer);
				continue;
			}
			if (selectedRow.DocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
			{
				MTBill mTBill = Settings.DocumentService.Load<MTBill>(selectedRow.ISN, selectedRow.DocType);
				if (mTBill != null && mTBill.State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value)
				{
					foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT7 in mTBill.MTList)
					{
						if (mT7.Summa == 0m)
						{
							throw new ASException(ArmSoft.SME.Common.Resources.ExistsRowWithZeroAmountInSelection);
						}
					}
				}
				taxBill.Add(mTBill);
				continue;
			}
			MTInvoiceRetail mTInvoiceRetail = Settings.DocumentService.Load<MTInvoiceRetail>(selectedRow.ISN, selectedRow.DocType);
			if (mTInvoiceRetail == null)
			{
				continue;
			}
			if (mTInvoiceRetail.State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value)
			{
				foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT8 in mTInvoiceRetail.MTList)
				{
					if (mT8.Summa == 0m)
					{
						throw new ASException(ArmSoft.SME.Trade.Common.Resources.ExistsRowWithZeroAmount);
					}
				}
			}
			taxInvoicesRetail.Add(mTInvoiceRetail);
		}
	}

	private bool AllowExportInNCCDialog(List<MTInvoice> taxInvoices)
	{
		foreach (MTInvoice taxInvoice in taxInvoices)
		{
			if (taxInvoice.CurrCode != "AMD")
			{
				return ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ConfirmExportInNCC, ArmSoft.SME.Common.Resources.ExportDosToEInvoicing, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes;
			}
		}
		return false;
	}

	private bool CheckAccDocsForExportEInvoice(List<MTInvoice> taxInvoices, List<MTReturnFromCustomer> taxCorrectionInvoices, List<MTInvoiceRetail> taxInvoicesRetail, List<MTBill> taxBill, bool exportInNCC = false)
	{
		int num = taxInvoices.Count + taxCorrectionInvoices.Count;
		if (taxInvoices.Count == 1)
		{
			if (taxInvoices[0].VATType == "1" || taxInvoices[0].VATType == "2" || (taxInvoices[0].CurrCode != "AMD" && !exportInNCC))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NotAccDoc);
				return false;
			}
			if (!string.IsNullOrWhiteSpace(taxInvoices[0].TaxInvoiceSeries + taxInvoices[0].TaxInvoiceNumber) || taxInvoices[0].ExportedType.Trim() == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || taxInvoices[0].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.AccDocNotForExport);
				return false;
			}
		}
		else
		{
			for (int num2 = taxInvoices.Count - 1; num2 >= 0; num2--)
			{
				if (taxInvoices[num2].VATType == "1" || taxInvoices[num2].VATType == "2" || (taxInvoices[num2].CurrCode != "AMD" && !exportInNCC) || !string.IsNullOrWhiteSpace(taxInvoices[num2].TaxInvoiceSeries + taxInvoices[num2].TaxInvoiceNumber) || taxInvoices[num2].ExportedType.Trim() == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || taxInvoices[num2].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
				{
					taxInvoices.Remove(taxInvoices[num2]);
				}
			}
		}
		if (taxCorrectionInvoices.Count == 1)
		{
			if (taxCorrectionInvoices[0].VATType == "1" || taxCorrectionInvoices[0].VATType == "2" || ((taxCorrectionInvoices[0].CurrCode != "AMD") & !exportInNCC))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NotAccDoc);
				return false;
			}
			if (!string.IsNullOrWhiteSpace(taxCorrectionInvoices[0].TaxInvoiceSeries + taxCorrectionInvoices[0].TaxInvoiceNumber) || taxCorrectionInvoices[0].ExportedType.Trim() == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || taxCorrectionInvoices[0].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.AccDocNotForExport);
				return false;
			}
		}
		else
		{
			for (int num3 = taxCorrectionInvoices.Count - 1; num3 >= 0; num3--)
			{
				if (taxCorrectionInvoices[num3].VATType == "1" || taxCorrectionInvoices[num3].VATType == "2" || ((taxCorrectionInvoices[num3].CurrCode != "AMD") & !exportInNCC) || !string.IsNullOrWhiteSpace(taxCorrectionInvoices[num3].TaxInvoiceSeries + taxCorrectionInvoices[num3].TaxInvoiceNumber) || taxCorrectionInvoices[num3].ExportedType.Trim() == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || taxCorrectionInvoices[num3].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
				{
					taxCorrectionInvoices.Remove(taxCorrectionInvoices[num3]);
				}
			}
		}
		if (taxInvoicesRetail.Count == 1)
		{
			if (taxInvoicesRetail[0].VATType == "2" || taxInvoicesRetail[0].CurrCode != "AMD")
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NotAccDoc);
				return false;
			}
			if (!string.IsNullOrWhiteSpace(taxInvoicesRetail[0].TaxInvoiceSeries + taxInvoicesRetail[0].TaxInvoiceNumber) || taxInvoicesRetail[0].ExportedType.Trim() == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || taxInvoicesRetail[0].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.AccDocNotForExport);
				return false;
			}
		}
		else
		{
			for (int num4 = taxInvoicesRetail.Count - 1; num4 >= 0; num4--)
			{
				if (taxInvoicesRetail[num4].VATType == "2" || taxInvoicesRetail[num4].CurrCode != "AMD" || !string.IsNullOrWhiteSpace(taxInvoicesRetail[num4].TaxInvoiceSeries + taxInvoicesRetail[num4].TaxInvoiceNumber) || taxInvoicesRetail[num4].ExportedType.Trim() == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || taxInvoicesRetail[num4].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
				{
					taxInvoicesRetail.Remove(taxInvoicesRetail[num4]);
				}
			}
		}
		if (taxBill.Count == 1)
		{
			if (taxBill[0].VATType == "1" || taxBill[0].VATType == "2" || taxBill[0].CurrCode != "AMD")
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NotAccDoc);
				return false;
			}
			if (!string.IsNullOrWhiteSpace(taxBill[0].TaxInvoiceSeries + taxBill[0].TaxInvoiceNumber) || taxBill[0].ExportedType.Trim() == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || taxBill[0].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.AccDocNotForExport);
				return false;
			}
		}
		else
		{
			for (int num5 = taxBill.Count - 1; num5 >= 0; num5--)
			{
				if (taxBill[num5].VATType == "1" || taxBill[num5].VATType == "2" || taxBill[num5].CurrCode != "AMD" || !string.IsNullOrWhiteSpace(taxBill[num5].TaxInvoiceSeries + taxBill[num5].TaxInvoiceNumber) || taxBill[num5].ExportedType.Trim() == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || taxBill[num5].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
				{
					taxBill.Remove(taxBill[num5]);
				}
			}
		}
		if (taxInvoices.Count == 0 && taxCorrectionInvoices.Count == 0 && taxInvoicesRetail.Count == 0 && taxBill.Count == 0)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.SelectedAccDocsNotForExportToXML, num));
			return false;
		}
		return true;
	}

	private bool CheckInvoicesForExportEInvoice(List<MTInvoice> taxInvoices, List<MTReturnFromCustomer> taxCorrectionInvoices, List<MTInvoiceRetail> taxInvoicesRetail, List<MTBill> taxBill, bool exportInNCC = false)
	{
		int num = taxInvoices.Count + taxCorrectionInvoices.Count + taxInvoicesRetail.Count + taxBill.Count;
		if (taxInvoices.Count == 1)
		{
			if (taxInvoices[0].VATType.Trim() == "3" || taxInvoices[0].VATType.Trim() == "4" || (taxInvoices[0].CurrCode != "AMD" && !exportInNCC))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NotInvoiceDoc);
				return false;
			}
			if (!string.IsNullOrWhiteSpace(taxInvoices[0].TaxInvoiceSeries + taxInvoices[0].TaxInvoiceNumber) || taxInvoices[0].ExportedType.Trim() == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || taxInvoices[0].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.InvoiceNotForExport);
				return false;
			}
		}
		else
		{
			for (int num2 = taxInvoices.Count - 1; num2 >= 0; num2--)
			{
				if (taxInvoices[num2].VATType.Trim() == "3" || taxInvoices[num2].VATType.Trim() == "4" || (taxInvoices[num2].CurrCode != "AMD" && !exportInNCC) || !string.IsNullOrWhiteSpace(taxInvoices[num2].TaxInvoiceSeries + taxInvoices[num2].TaxInvoiceNumber) || taxInvoices[num2].ExportedType.Trim() == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || taxInvoices[num2].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
				{
					taxInvoices.Remove(taxInvoices[num2]);
				}
			}
		}
		if (taxCorrectionInvoices.Count == 1)
		{
			if (taxCorrectionInvoices[0].VATType.Trim() == "3" || taxCorrectionInvoices[0].VATType.Trim() == "4" || taxCorrectionInvoices[0].CurrCode != "AMD")
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NotInvoiceDoc);
				return false;
			}
			if (!string.IsNullOrWhiteSpace(taxCorrectionInvoices[0].TaxInvoiceSeries + taxCorrectionInvoices[0].TaxInvoiceNumber) || taxCorrectionInvoices[0].ExportedType.Trim() == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || taxCorrectionInvoices[0].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.InvoiceNotForExport);
				return false;
			}
		}
		else
		{
			for (int num3 = taxCorrectionInvoices.Count - 1; num3 >= 0; num3--)
			{
				if (taxCorrectionInvoices[num3].VATType.Trim() == "3" || taxCorrectionInvoices[num3].VATType.Trim() == "4" || taxCorrectionInvoices[num3].CurrCode != "AMD" || !string.IsNullOrWhiteSpace(taxCorrectionInvoices[num3].TaxInvoiceSeries + taxCorrectionInvoices[num3].TaxInvoiceNumber) || taxCorrectionInvoices[num3].ExportedType.Trim() == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || taxCorrectionInvoices[num3].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
				{
					taxCorrectionInvoices.Remove(taxCorrectionInvoices[num3]);
				}
			}
		}
		if (taxInvoicesRetail.Count == 1)
		{
			if (taxInvoicesRetail[0].VATType.Trim() == "3" || taxInvoicesRetail[0].VATType.Trim() == "4" || taxInvoicesRetail[0].CurrCode != "AMD")
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NotInvoiceDoc);
				return false;
			}
			if (!string.IsNullOrWhiteSpace(taxInvoicesRetail[0].TaxInvoiceSeries + taxInvoicesRetail[0].TaxInvoiceNumber) || taxInvoicesRetail[0].ExportedType.Trim() == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || taxInvoicesRetail[0].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.InvoiceNotForExport);
				return false;
			}
		}
		else
		{
			for (int num4 = taxInvoicesRetail.Count - 1; num4 >= 0; num4--)
			{
				if (taxInvoicesRetail[num4].VATType.Trim() == "3" || taxInvoicesRetail[num4].VATType.Trim() == "4" || taxInvoicesRetail[num4].CurrCode != "AMD" || !string.IsNullOrWhiteSpace(taxInvoicesRetail[num4].TaxInvoiceSeries + taxInvoicesRetail[num4].TaxInvoiceNumber) || taxInvoicesRetail[num4].ExportedType.Trim() == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || taxInvoicesRetail[num4].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
				{
					taxInvoicesRetail.Remove(taxInvoicesRetail[num4]);
				}
			}
		}
		if (taxBill.Count == 1)
		{
			if (taxBill[0].VATType == "3" || taxBill[0].VATType == "4" || taxBill[0].CurrCode != "AMD")
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NotInvoiceDoc);
				return false;
			}
			if (!string.IsNullOrWhiteSpace(taxBill[0].TaxInvoiceSeries + taxBill[0].TaxInvoiceNumber) || taxBill[0].ExportedType.Trim() == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || taxBill[0].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.InvoiceNotForExport);
				return false;
			}
		}
		else
		{
			for (int num5 = taxBill.Count - 1; num5 >= 0; num5--)
			{
				if (taxBill[num5].VATType == "3" || taxBill[num5].VATType == "4" || taxBill[num5].CurrCode != "AMD" || !string.IsNullOrWhiteSpace(taxBill[num5].TaxInvoiceSeries + taxBill[num5].TaxInvoiceNumber) || taxBill[num5].ExportedType.Trim() == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || taxBill[num5].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
				{
					taxBill.Remove(taxBill[num5]);
				}
			}
		}
		if (taxInvoices.Count == 0 && taxCorrectionInvoices.Count == 0 && taxInvoicesRetail.Count == 0 && taxBill.Count == 0)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.SelectedInvoicesNotForExportToXML, num));
			return false;
		}
		return true;
	}

	private bool GetInvExportType(bool isAccDoc)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = ArmSoft.SME.Common.Resources.ExportDosToEInvoicing,
			Name = "InvExportForEInv"
		};
		LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
		dialogWindow.AddStoreValuesButton(container);
		dialogWindow.AddDropDownControl(container, "ExportType", ArmSoft.SME.Common.Resources.Export, new SmartEnum(typeof(TaxServiceSourceTypeSE)), isRequired: true, allowAdditionButtons: false, storeValue: true);
		List<string> values = new List<string>
		{
			TaxAccAndInvoiceDocsTypeSE.Goods.Value,
			TaxAccAndInvoiceDocsTypeSE.Services.Value
		};
		dialogWindow.AddDropDownControl("InvType", isAccDoc ? ArmSoft.SME.Trade.Common.Resources.AccDocType : ArmSoft.SME.Common.Resources.TaxAccType, new SmartEnum(typeof(TaxAccAndInvoiceDocsTypeSE), values), isRequired: false, allowAdditionButtons: false, storeValue: true);
		dialogWindow.AddDropDownControl("Captions", ArmSoft.SME.Common.Resources.ExportCaptionInterpretation, new SmartEnum(typeof(CaptionSE)), isRequired: true, allowAdditionButtons: false, storeValue: true).Code = "2";
		dialogWindow.AddBooleanControl("ExportDiscount", ArmSoft.SME.Common.Resources.ExportDiscount, storeValue: true);
		dialogWindow.AddBooleanControl("IsNotRequiredTaxCode", ArmSoft.SME.Trade.Common.Resources.TaxCodeIsNotRequired, storeValue: true);
		if (Settings.ParametersService.UseMarking())
		{
			dialogWindow.AddBooleanControl("ExportEmarkCodes", ArmSoft.SME.Trade.Common.Resources.ExportEMarkCodes, storeValue: true);
		}
		if (Convert.ToBoolean(dialogWindow.ShowDialog()))
		{
			_invExpType = Convert.ToString(dialogWindow["InvType"]);
			_exportCaption = Convert.ToString(dialogWindow["Captions"]) == "1";
			_exportDiscount = Convert.ToBoolean(dialogWindow["ExportDiscount"]);
			_isNotRequiredTaxCode = Convert.ToBoolean(dialogWindow["IsNotRequiredTaxCode"]);
			_exportEmarkCodes = Settings.ParametersService.UseMarking() && Convert.ToBoolean(dialogWindow["ExportEmarkCodes"]);
			_eInvExportType = Convert.ToInt16(dialogWindow["ExportType"]);
			return true;
		}
		return false;
	}

	private List<(TaxDocument taxDoc, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE docType)> ExportInvoicesToTaxService(ref EnumerateErrorsReportViewer errorRepViewer, List<MTInvoice> invoices, List<MTReturnFromCustomer> correctionInvoices, List<MTInvoiceRetail> retailInvoices, List<MTBill> bill, ref bool isReasonableUpdateDoc, bool isAccDoc, ref List<Service> serviceInvoices, ref List<Good> goodInvoices, ref List<AccDocServices> servAccDocs, ref List<AccDocGoods> goodAccDocs)
	{
		List<(TaxDocument, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE)> list = new List<(TaxDocument, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE)>();
		bool flag = false;
		bool flag2 = false;
		bool isSelectedExportTypeGood = !string.IsNullOrWhiteSpace(_invExpType) && _invExpType == InvoiceRowTypeSE.Goods.Value;
		foreach (MTInvoice invoice in invoices)
		{
			try
			{
				if (Settings.ParametersService.CPACheckMode() != CheckModeSE.DontCheck && invoice.MTList.FirstOrDefault((MTInvoiceBase.MTInvoiceSpecificationRow p) => (isSelectedExportTypeGood || p.ItemType == InvoiceRowTypeSE.Goods.Value) && string.IsNullOrWhiteSpace(p.CPACode)) != null)
				{
					if (!(Settings.ParametersService.CPACheckMode() == CheckModeSE.Warning.Value))
					{
						throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(invoice, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
					}
					if (!flag)
					{
						if (ArmSoft.SME.Core.UI.MessageBox.Show($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}: {ArmSoft.SME.Common.Resources.AskContinue}", ArmSoft.SME.Common.Resources.ExportToTaxProgram, MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
						{
							flag = true;
							flag2 = false;
							throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(invoice, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
						}
						flag = true;
						flag2 = true;
					}
					else if (!flag2)
					{
						throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(invoice, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
					}
				}
				TaxDocument taxDocument = CreateTaxDocument(invoice, isSelectedExportTypeGood);
				list.Add((taxDocument, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice));
				if (isAccDoc)
				{
					if (string.IsNullOrWhiteSpace(_invExpType))
					{
						if (invoice.MTList.Any((MTInvoiceBase.MTInvoiceSpecificationRow r) => r.ItemType == InvoiceRowTypeSE.Goods.Value))
						{
							goodAccDocs.Add(TaxServiceHelper.CreateGoodAccDoc(taxDocument));
						}
						else
						{
							servAccDocs.Add(TaxServiceHelper.CreateServiceAccDoc(taxDocument));
						}
					}
					else if (_invExpType == TaxAccAndInvoiceDocsTypeSE.Goods.Value)
					{
						goodAccDocs.Add(TaxServiceHelper.CreateGoodAccDoc(taxDocument));
					}
					else
					{
						servAccDocs.Add(TaxServiceHelper.CreateServiceAccDoc(taxDocument));
					}
				}
				else if (string.IsNullOrWhiteSpace(_invExpType))
				{
					if (invoice.MTList.Any((MTInvoiceBase.MTInvoiceSpecificationRow r) => r.ItemType == InvoiceRowTypeSE.Goods.Value))
					{
						goodInvoices.Add(TaxServiceHelper.CreateGoodInvoiceDoc(taxDocument));
					}
					else
					{
						serviceInvoices.Add(TaxServiceHelper.CreateServiceInvoiceDoc(taxDocument));
					}
				}
				else if (_invExpType == TaxAccAndInvoiceDocsTypeSE.Goods.Value)
				{
					goodInvoices.Add(TaxServiceHelper.CreateGoodInvoiceDoc(taxDocument));
				}
				else
				{
					serviceInvoices.Add(TaxServiceHelper.CreateServiceInvoiceDoc(taxDocument));
				}
				invoice.ROWISNDict = taxDocument.ROWISNDict;
				isReasonableUpdateDoc = true;
			}
			catch (Exception ex)
			{
				errorRepViewer.AddError(ex, invoice.ISN);
			}
		}
		foreach (MTInvoiceRetail retailInvoice in retailInvoices)
		{
			try
			{
				if (Settings.ParametersService.CPACheckMode() != CheckModeSE.DontCheck && retailInvoice.MTList.FirstOrDefault((MTInvoiceBase.MTInvoiceSpecificationRow p) => (isSelectedExportTypeGood || p.ItemType == InvoiceRowTypeSE.Goods.Value) && string.IsNullOrWhiteSpace(p.CPACode)) != null)
				{
					if (!(Settings.ParametersService.CPACheckMode() == CheckModeSE.Warning.Value))
					{
						throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(retailInvoice, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
					}
					if (!flag)
					{
						if (ArmSoft.SME.Core.UI.MessageBox.Show($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}: {ArmSoft.SME.Common.Resources.AskContinue}", ArmSoft.SME.Common.Resources.ExportToTaxProgram, MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
						{
							flag = true;
							flag2 = false;
							throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(retailInvoice, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
						}
						flag = true;
						flag2 = true;
					}
					else if (!flag2)
					{
						throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(retailInvoice, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
					}
				}
				TaxDocument taxDocument2 = CreateTaxDocument(retailInvoice, isSelectedExportTypeGood);
				list.Add((taxDocument2, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail));
				if (isAccDoc)
				{
					if (string.IsNullOrWhiteSpace(_invExpType))
					{
						if (retailInvoice.MTList.Where((MTInvoiceBase.MTInvoiceSpecificationRow r) => r.ItemType == InvoiceRowTypeSE.Goods.Value).ToList().Count == 0)
						{
							servAccDocs.Add(TaxServiceHelper.CreateServiceAccDoc(taxDocument2));
						}
						else
						{
							goodAccDocs.Add(TaxServiceHelper.CreateGoodAccDoc(taxDocument2));
						}
					}
					else if (_invExpType == TaxAccAndInvoiceDocsTypeSE.Goods.Value)
					{
						goodAccDocs.Add(TaxServiceHelper.CreateGoodAccDoc(taxDocument2));
					}
					else
					{
						servAccDocs.Add(TaxServiceHelper.CreateServiceAccDoc(taxDocument2));
					}
				}
				else if (string.IsNullOrWhiteSpace(_invExpType))
				{
					if (retailInvoice.MTList.Where((MTInvoiceBase.MTInvoiceSpecificationRow r) => r.ItemType == InvoiceRowTypeSE.Goods.Value).ToList().Count == 0)
					{
						serviceInvoices.Add(TaxServiceHelper.CreateServiceInvoiceDoc(taxDocument2));
					}
					else
					{
						goodInvoices.Add(TaxServiceHelper.CreateGoodInvoiceDoc(taxDocument2));
					}
				}
				else if (_invExpType == TaxAccAndInvoiceDocsTypeSE.Goods.Value)
				{
					goodInvoices.Add(TaxServiceHelper.CreateGoodInvoiceDoc(taxDocument2));
				}
				else
				{
					serviceInvoices.Add(TaxServiceHelper.CreateServiceInvoiceDoc(taxDocument2));
				}
				retailInvoice.ROWISNDict = taxDocument2.ROWISNDict;
				isReasonableUpdateDoc = true;
			}
			catch (Exception ex2)
			{
				errorRepViewer.AddError(ex2, retailInvoice.ISN);
			}
		}
		foreach (MTBill item in bill)
		{
			try
			{
				if (Settings.ParametersService.CPACheckMode() != CheckModeSE.DontCheck && item.MTList.FirstOrDefault((MTInvoiceBase.MTInvoiceSpecificationRow p) => (isSelectedExportTypeGood || p.ItemType == InvoiceRowTypeSE.Goods.Value) && string.IsNullOrWhiteSpace(p.CPACode)) != null)
				{
					if (!(Settings.ParametersService.CPACheckMode() == CheckModeSE.Warning.Value))
					{
						throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(item, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
					}
					if (!flag)
					{
						if (ArmSoft.SME.Core.UI.MessageBox.Show($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}: {ArmSoft.SME.Common.Resources.AskContinue}", ArmSoft.SME.Common.Resources.ExportToTaxProgram, MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
						{
							flag = true;
							flag2 = false;
							throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(item, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
						}
						flag = true;
						flag2 = true;
					}
					else if (!flag2)
					{
						throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(item, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
					}
				}
				TaxDocument taxDocument3 = CreateTaxDocument(item, isSelectedExportTypeGood);
				list.Add((taxDocument3, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill));
				if (isAccDoc)
				{
					if (string.IsNullOrWhiteSpace(_invExpType))
					{
						if (item.MTList.Where((MTInvoiceBase.MTInvoiceSpecificationRow r) => r.ItemType == InvoiceRowTypeSE.Goods.Value).ToList().Count == 0)
						{
							servAccDocs.Add(TaxServiceHelper.CreateServiceAccDoc(taxDocument3));
						}
						else
						{
							goodAccDocs.Add(TaxServiceHelper.CreateGoodAccDoc(taxDocument3));
						}
					}
					else if (_invExpType == TaxAccAndInvoiceDocsTypeSE.Goods.Value)
					{
						goodAccDocs.Add(TaxServiceHelper.CreateGoodAccDoc(taxDocument3));
					}
					else
					{
						servAccDocs.Add(TaxServiceHelper.CreateServiceAccDoc(taxDocument3));
					}
				}
				else if (string.IsNullOrWhiteSpace(_invExpType))
				{
					if (item.MTList.Where((MTInvoiceBase.MTInvoiceSpecificationRow r) => r.ItemType == InvoiceRowTypeSE.Goods.Value).ToList().Count == 0)
					{
						serviceInvoices.Add(TaxServiceHelper.CreateServiceInvoiceDoc(taxDocument3));
					}
					else
					{
						goodInvoices.Add(TaxServiceHelper.CreateGoodInvoiceDoc(taxDocument3));
					}
				}
				else if (_invExpType == TaxAccAndInvoiceDocsTypeSE.Goods.Value)
				{
					goodInvoices.Add(TaxServiceHelper.CreateGoodInvoiceDoc(taxDocument3));
				}
				else
				{
					serviceInvoices.Add(TaxServiceHelper.CreateServiceInvoiceDoc(taxDocument3));
				}
				item.IsRegenerateTransactionsRequired = false;
				item.ROWISNDict = taxDocument3.ROWISNDict;
				isReasonableUpdateDoc = true;
			}
			catch (Exception ex3)
			{
				errorRepViewer.AddError(ex3, item.ISN);
			}
		}
		foreach (MTReturnFromCustomer correctionInvoice in correctionInvoices)
		{
			try
			{
				if (Settings.ParametersService.CPACheckMode() != CheckModeSE.DontCheck && correctionInvoice.MTList.FirstOrDefault((MTReturnFromCustomerBase.MTRetFromCustSpecificationRow p) => (isSelectedExportTypeGood || p.ItemType == InvoiceRowTypeSE.Goods.Value) && string.IsNullOrWhiteSpace(p.CPACode)) != null)
				{
					if (!(Settings.ParametersService.CPACheckMode() == CheckModeSE.Warning.Value))
					{
						throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(correctionInvoice, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
					}
					if (!flag)
					{
						if (ArmSoft.SME.Core.UI.MessageBox.Show($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}: {ArmSoft.SME.Common.Resources.AskContinue}", ArmSoft.SME.Common.Resources.ExportToTaxProgram, MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
						{
							flag = true;
							flag2 = false;
							throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(correctionInvoice, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
						}
						flag = true;
						flag2 = true;
					}
					else if (!flag2)
					{
						throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(correctionInvoice, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
					}
				}
				TaxDocument taxDocument4 = CreateTaxDocument(correctionInvoice, isSelectedExportTypeGood);
				list.Add((taxDocument4, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer));
				if (isAccDoc)
				{
					if (string.IsNullOrWhiteSpace(_invExpType))
					{
						if (correctionInvoice.MTList.Where((MTReturnFromCustomerBase.MTRetFromCustSpecificationRow r) => r.ItemType == InvoiceRowTypeSE.Goods.Value).ToList().Count == 0)
						{
							servAccDocs.Add(TaxServiceHelper.CreateServiceAccDoc(taxDocument4));
						}
						else
						{
							goodAccDocs.Add(TaxServiceHelper.CreateGoodAccDoc(taxDocument4));
						}
					}
					else if (_invExpType == TaxAccAndInvoiceDocsTypeSE.Goods.Value)
					{
						goodAccDocs.Add(TaxServiceHelper.CreateGoodAccDoc(taxDocument4));
					}
					else
					{
						servAccDocs.Add(TaxServiceHelper.CreateServiceAccDoc(taxDocument4));
					}
				}
				else if (string.IsNullOrWhiteSpace(_invExpType))
				{
					if (correctionInvoice.MTList.Where((MTReturnFromCustomerBase.MTRetFromCustSpecificationRow r) => r.ItemType == InvoiceRowTypeSE.Goods.Value).ToList().Count == 0)
					{
						serviceInvoices.Add(TaxServiceHelper.CreateServiceInvoiceDoc(taxDocument4));
					}
					else
					{
						goodInvoices.Add(TaxServiceHelper.CreateGoodInvoiceDoc(taxDocument4));
					}
				}
				else if (_invExpType == TaxAccAndInvoiceDocsTypeSE.Goods.Value)
				{
					goodInvoices.Add(TaxServiceHelper.CreateGoodInvoiceDoc(taxDocument4));
				}
				else
				{
					serviceInvoices.Add(TaxServiceHelper.CreateServiceInvoiceDoc(taxDocument4));
				}
				isReasonableUpdateDoc = true;
			}
			catch (Exception ex4)
			{
				errorRepViewer.AddError(ex4, correctionInvoice.ISN);
			}
		}
		return list;
	}

	private List<Guid> ExportMarkingsToTaxService(List<(TaxDocument taxDoc, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE docType)> docs, short exportType, string filePath, EnumerateErrorsReportViewer errorRepViewer)
	{
		List<Guid> list = new List<Guid>();
		Settings.TaxService.Authenticate();
		if (exportType == TaxServiceSourceTypeSE.File.Value)
		{
			foreach (var doc in docs)
			{
				try
				{
					if (doc.docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice)
					{
						if (doc.taxDoc.Markings != null && doc.taxDoc.Markings.Count != 0)
						{
							string fileFullName = Path.Combine(Settings.CommonCoreHelper.GetDirectorySafe(filePath), doc.taxDoc.ISN.ToString() + ".csv");
							Settings.TaxService.Client.Dispatcher.ExportMarkings(doc.taxDoc.Markings, fileFullName);
							list.Add(doc.taxDoc.ISN);
							continue;
						}
						errorRepViewer.AddError(ArmSoft.SME.Common.Resources.DocNumber + " " + doc.taxDoc.DocNum + ", " + ArmSoft.SME.Common.Resources.DateWord + " " + doc.taxDoc.DocDate.FormatDDMMYYYY() + Environment.NewLine + ArmSoft.SME.Common.Resources.DocumentDoesNotHaveEMarks, doc.taxDoc.ISN);
					}
					else
					{
						errorRepViewer.AddError(string.Format(ArmSoft.SME.Trade.Common.Resources.EMarkExportIsNotAvailableForThisTypeOfDoc, SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>.FromValue(doc.docType).Caption), doc.taxDoc.ISN);
					}
				}
				catch (Exception ex)
				{
					errorRepViewer.AddError(ex.Message, doc.taxDoc.ISN);
				}
			}
		}
		else
		{
			bool flag = false;
			MessageBoxResult messageBoxResult = MessageBoxResult.No;
			foreach (var doc2 in docs)
			{
				try
				{
					if (!Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
					{
						continue;
					}
					if (doc2.docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice)
					{
						if (doc2.taxDoc.Markings != null && doc2.taxDoc.Markings.Count != 0)
						{
							FailureResponse failureResponse = Settings.TaxService.Client.Dispatcher.ImportMarkings(doc2.taxDoc.ISN, doc2.taxDoc.Markings, rewrite: false);
							if (failureResponse != null)
							{
								foreach (FailedCodes code in failureResponse.Codes)
								{
									errorRepViewer.AddError(ArmSoft.SME.Common.Resources.DocNumber + " " + doc2.taxDoc.DocNum + ", " + ArmSoft.SME.Common.Resources.DateWord + " " + doc2.taxDoc.DocDate.FormatDDMMYYYY() + Environment.NewLine + code.Code + Environment.NewLine + code.Status, doc2.taxDoc.ISN);
								}
								if (!flag)
								{
									Mouse.OverrideCursor = null;
									messageBoxResult = ArmSoft.SME.Core.UI.MessageBox.Show(failureResponse.Message + Environment.NewLine + ArmSoft.SME.Common.Resources.AskContinue, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
									flag = true;
									Mouse.OverrideCursor = Cursors.Wait;
								}
								if (messageBoxResult == MessageBoxResult.Yes)
								{
									Settings.TaxService.Client.Dispatcher.ImportMarkings(doc2.taxDoc.ISN, doc2.taxDoc.Markings, rewrite: true, ignoreWarnings: true);
									list.Add(doc2.taxDoc.ISN);
								}
							}
							else
							{
								list.Add(doc2.taxDoc.ISN);
							}
						}
						else
						{
							errorRepViewer.AddError(ArmSoft.SME.Common.Resources.DocNumber + " " + doc2.taxDoc.DocNum + ", " + ArmSoft.SME.Common.Resources.DateWord + " " + doc2.taxDoc.DocDate.FormatDDMMYYYY() + Environment.NewLine + ArmSoft.SME.Common.Resources.DocumentDoesNotHaveEMarks, doc2.taxDoc.ISN);
						}
					}
					else
					{
						errorRepViewer.AddError(string.Format(ArmSoft.SME.Trade.Common.Resources.EMarkExportIsNotAvailableForThisTypeOfDoc, SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>.FromValue(doc2.docType).Caption), doc2.taxDoc.ISN);
					}
				}
				catch (Exception ex2)
				{
					errorRepViewer.AddError(ex2.Message, doc2.taxDoc.ISN);
				}
			}
			Mouse.OverrideCursor = null;
		}
		return list;
	}

	private static void UpdateDocExporteMarkingsdState(EnumerateErrorsReportViewer errorRepViewer, List<Guid> exportedDocuments, short eInvExportType)
	{
		DocumentEditFieldsWithSameValueRequestModel documentEditFieldsWithSameValueRequestModel = new DocumentEditFieldsWithSameValueRequestModel
		{
			DocISNs = exportedDocuments
		};
		int num = 1;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("MarkingsExported", "true");
		documentEditFieldsWithSameValueRequestModel.Properties = list;
		documentEditFieldsWithSameValueRequestModel.StoreRequest = new StoreRequest
		{
			LogComment = ((eInvExportType == TaxServiceSourceTypeSE.EInvoicing.Value) ? ArmSoft.SME.Common.Resources.EMarksExportToEInvoicing : ArmSoft.SME.Common.Resources.EMarksExportAsXML),
			WithoutValidation = true,
			WithoutAccessCheck = true,
			WithoutGrids = true
		};
		DocumentEditFieldsWithSameValueRequestModel request = documentEditFieldsWithSameValueRequestModel;
		DocumentEditFieldsResponseModel documentEditFieldsResponseModel = DataProcessingRequestHelper.Execute<DocumentEditFieldsWithSameValueRequestModel, DocumentEditFieldsResponseModel>(DPRType.Other, "EditDocumentsFieldsWithSameValue", request);
		if (documentEditFieldsResponseModel.Exceptions != null && documentEditFieldsResponseModel.Exceptions.Count != 0)
		{
			foreach (DocumentEditResponseModel exception in documentEditFieldsResponseModel.Exceptions)
			{
				errorRepViewer.AddError(exception.Exception.Message, exception.DocISN);
			}
		}
		Settings.HostingEnvironment.CurrentViewPanel.Update(exportedDocuments.Cast<object>().ToList());
	}

	private void ExportMTMovDocsForEInvoicing()
	{
		List<MTMoveWithConfirmationBase> list = new List<MTMoveWithConfirmationBase>();
		List<MTBill> list2 = new List<MTBill>();
		foreach (DocumentsJournalDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<DocumentsJournalDataProvider.DataRow>())
		{
			if (selectedOrFocusedRow.DocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value)
			{
				list.Add(Settings.DocumentService.Load<MTMove>(selectedOrFocusedRow.ISN));
			}
			else if (selectedOrFocusedRow.DocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value)
			{
				list.Add(Settings.DocumentService.Load<MTMoveWithConfirmation>(selectedOrFocusedRow.ISN));
			}
			else if (selectedOrFocusedRow.DocType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
			{
				list2.Add(Settings.DocumentService.Load<MTBill>(selectedOrFocusedRow.ISN));
			}
		}
		if (!CheckMTMoveDocsForExportEInvoice(list, list2) || !GetMTMoveExportParams())
		{
			return;
		}
		Mouse.OverrideCursor = Cursors.Wait;
		EnumerateErrorsReportViewer errorRepViewer = new EnumerateErrorsReportViewer(ArmSoft.SME.Common.Resources.ASerrWindow, "ExportInvoicesToTaxService");
		List<AccDocTransportation> accDocTransportations = new List<AccDocTransportation>();
		int exportedDocsCount = 0;
		List<Guid> list3 = new List<Guid>();
		string logComment = "";
		if (_eInvExportType == TaxServiceSourceTypeSE.File.Value)
		{
			string fileNameFromExportDialog = Settings.CommonCoreHelper.GetFileNameFromExportDialog(".xml", "XML Files (.xml)|*.xml", $"MTMoveExport {Settings.ParametersService.TaxCode()} {DateTime.Now:yyyyMMdd HHmmss}");
			if (!string.IsNullOrWhiteSpace(fileNameFromExportDialog))
			{
				ExportMTMoveDocsToTaxService(ref errorRepViewer, list, list2, ref accDocTransportations, ref exportedDocsCount);
				Settings.TaxService.Authenticate();
				Settings.TaxService.Client.AccDocTransportation.CreateXMLFile(accDocTransportations, fileNameFromExportDialog);
				list3 = accDocTransportations.Select((AccDocTransportation p) => p.Id).ToList();
				logComment = ArmSoft.SME.Common.Resources.ExportMTMovesToXml;
			}
		}
		else if (_eInvExportType == TaxServiceSourceTypeSE.EInvoicing.Value && Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
		{
			ExportMTMoveDocsToTaxService(ref errorRepViewer, list, list2, ref accDocTransportations, ref exportedDocsCount);
			foreach (AccDocTransportation item in accDocTransportations)
			{
				try
				{
					Settings.TaxService.Client.AccDocTransportation.CreateDraft(item);
					list3.Add(item.Id);
				}
				catch (Exception ex)
				{
					errorRepViewer.AddError(ex, item.Id);
				}
			}
			logComment = ArmSoft.SME.Common.Resources.ExportToTaxProgram;
		}
		Mouse.OverrideCursor = null;
		ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.MTMoveExportedCount, list3.Count), ArmSoft.SME.Common.Resources.ExportMTMovesToEInv, MessageBoxButton.OK, MessageBoxImage.Asterisk);
		if (list3.Count > 0)
		{
			DocumentEditFieldsWithSameValueRequestModel documentEditFieldsWithSameValueRequestModel = new DocumentEditFieldsWithSameValueRequestModel
			{
				DocISNs = list3
			};
			int num = 1;
			List<PropertyWithValue> list4 = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list4, num);
			CollectionsMarshal.AsSpan(list4)[0] = new PropertyWithValue("ExportedType", TaxInvoicesWriteTypesSE.TaxInvExportFormElectronic.Value);
			documentEditFieldsWithSameValueRequestModel.Properties = list4;
			documentEditFieldsWithSameValueRequestModel.StoreRequest = new StoreRequest
			{
				LogComment = logComment,
				WithoutValidation = true,
				WithoutAccessCheck = true,
				WithoutGrids = true
			};
			DocumentEditFieldsWithSameValueRequestModel request = documentEditFieldsWithSameValueRequestModel;
			DocumentEditFieldsResponseModel documentEditFieldsResponseModel = DataProcessingRequestHelper.Execute<DocumentEditFieldsWithSameValueRequestModel, DocumentEditFieldsResponseModel>(DPRType.Other, "EditDocumentsFieldsWithSameValue", request);
			if (documentEditFieldsResponseModel.Exceptions != null && documentEditFieldsResponseModel.Exceptions.Count != 0)
			{
				foreach (DocumentEditResponseModel exception in documentEditFieldsResponseModel.Exceptions)
				{
					errorRepViewer.AddError(exception.Exception.Message, exception.DocISN);
				}
			}
			Settings.HostingEnvironment.CurrentViewPanel.Update(list3.Cast<object>().ToList());
		}
		if (errorRepViewer.HasErrors)
		{
			errorRepViewer.Show();
		}
	}

	private bool CheckMTMoveDocsForExportEInvoice(List<MTMoveWithConfirmationBase> MTMoveDocs, List<MTBill> MTBillDocs)
	{
		int num = MTMoveDocs.Count + MTBillDocs.Count;
		if (MTMoveDocs.Count == 1)
		{
			if (!string.IsNullOrWhiteSpace(MTMoveDocs[0].TaxInvoiceSeries + MTMoveDocs[0].TaxInvoiceNumber) || MTMoveDocs[0].ExportedType == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || MTMoveDocs[0].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.MTMoveNotForExport);
				return false;
			}
		}
		else
		{
			for (int num2 = MTMoveDocs.Count - 1; num2 >= 0; num2--)
			{
				if (!string.IsNullOrWhiteSpace(MTMoveDocs[num2].TaxInvoiceSeries + MTMoveDocs[num2].TaxInvoiceNumber) || MTMoveDocs[num2].ExportedType == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || MTMoveDocs[num2].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
				{
					MTMoveDocs.Remove(MTMoveDocs[num2]);
				}
			}
		}
		if (MTBillDocs.Count == 1)
		{
			if (!string.IsNullOrWhiteSpace(MTBillDocs[0].TaxInvoiceSeries + MTBillDocs[0].TaxInvoiceNumber) || MTBillDocs[0].ExportedType == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || MTBillDocs[0].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.MTMoveNotForExport);
				return false;
			}
		}
		else
		{
			for (int num3 = MTBillDocs.Count - 1; num3 >= 0; num3--)
			{
				if (!string.IsNullOrWhiteSpace(MTBillDocs[num3].TaxInvoiceSeries + MTBillDocs[num3].TaxInvoiceNumber) || MTBillDocs[num3].ExportedType == TaxInvoicesWriteTypesSE.TaxInvExportFormPaper.Value || MTBillDocs[num3].State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
				{
					MTBillDocs.Remove(MTBillDocs[num3]);
				}
			}
		}
		if (MTMoveDocs.Count == 0 && MTBillDocs.Count == 0)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.SelectedMTMovesNotForExport, num));
			return false;
		}
		return true;
	}

	private bool GetMTMoveExportParams()
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = ArmSoft.SME.Common.Resources.ExportDosToEInvoicing,
			Name = "GetPriceTypeForXMLExport"
		};
		LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
		dialogWindow.AddStoreValuesButton(container);
		dialogWindow.AddDropDownControl(container, "ExportType", ArmSoft.SME.Common.Resources.Export, new SmartEnum(typeof(TaxServiceSourceTypeSE)), isRequired: true, allowAdditionButtons: false, storeValue: true);
		dialogWindow.AddDropDownControl("PriceType", ArmSoft.SME.Common.Resources.PriceListType, new PriceTypesExtended(showPrimeCostRows: true), isRequired: false, allowAdditionButtons: false, storeValue: true);
		dialogWindow.AddDropDownControl("Captions", ArmSoft.SME.Common.Resources.ExportCaptionInterpretation, new SmartEnum(typeof(CaptionSE)), isRequired: true, allowAdditionButtons: false, storeValue: true).Code = "2";
		if (Convert.ToBoolean(dialogWindow.ShowDialog()))
		{
			_priceType = Convert.ToString(dialogWindow["PriceType"]);
			_exportCaption = Convert.ToString(dialogWindow["Captions"]) == "1";
			_eInvExportType = Convert.ToInt16(dialogWindow["ExportType"]);
			return true;
		}
		return false;
	}

	private void AssignTaxInvNumToDocNum()
	{
		Mouse.OverrideCursor = Cursors.Wait;
		Dictionary<Guid, short> dictionary = new Dictionary<Guid, short>();
		foreach (DocumentsJournalDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<DocumentsJournalDataProvider.DataRow>())
		{
			if (!string.IsNullOrWhiteSpace(selectedOrFocusedRow.InvoiceNumber))
			{
				dictionary.Add(selectedOrFocusedRow.ISN, selectedOrFocusedRow.DocType);
			}
		}
		int num = 0;
		if (dictionary.Count > 0)
		{
			TradeDocument tradeDocument = null;
			foreach (KeyValuePair<Guid, short> item in dictionary)
			{
				tradeDocument = (TradeDocument)Settings.DocumentService.Load(item.Key, item.Value);
				if (tradeDocument != null)
				{
					tradeDocument.DocNum = tradeDocument.InvoiceNumber.Left(12);
					Settings.DocumentService.Store(tradeDocument, new StoreRequest
					{
						State = tradeDocument.State,
						LogComment = ArmSoft.SME.Common.Resources.AssignTaxInvNumToDocNum,
						WithoutValidation = true,
						WithoutAccessCheck = true,
						WithoutGrids = true
					});
					num++;
				}
			}
			DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
			List<object> list = new List<object>();
			foreach (Guid item2 in dictionary.Select((KeyValuePair<Guid, short> x) => x.Key))
			{
				list.Add(item2);
			}
			currentViewPanel.Update(list);
		}
		Mouse.OverrideCursor = null;
		ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.AssignedDocCount, num), ArmSoft.SME.Common.Resources.AssignTaxInvNumToDocNum, MessageBoxButton.OK, MessageBoxImage.Asterisk);
	}

	private void ExportMarkings()
	{
		List<MTInvoice> taxInvoices = new List<MTInvoice>();
		List<MTInvoiceRetail> taxInvoicesRetail = new List<MTInvoiceRetail>();
		List<MTReturnFromCustomer> taxCorrectionInvoices = new List<MTReturnFromCustomer>();
		List<MTBill> taxBill = new List<MTBill>();
		string text = "";
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = ArmSoft.SME.Common.Resources.ExportDosToEInvoicing,
			Name = "ExportType"
		};
		LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
		dialogWindow.AddStoreValuesButton(container);
		DropDownControl dropDownControl = dialogWindow.AddDropDownControl(container, "ExportType", ArmSoft.SME.Common.Resources.Export, new SmartEnum(typeof(TaxServiceSourceTypeSE)), isRequired: true, allowAdditionButtons: false, storeValue: true);
		if (!Convert.ToBoolean(dialogWindow.ShowDialog()))
		{
			return;
		}
		text = dropDownControl.Code;
		GroupDocsByTypeForExport(ref taxInvoices, ref taxInvoicesRetail, ref taxCorrectionInvoices, ref taxBill);
		AllowExportInNCCDialog(taxInvoices);
		List<Service> serviceInvoices = new List<Service>();
		List<Good> goodInvoices = new List<Good>();
		List<AccDocServices> servAccDocs = new List<AccDocServices>();
		List<AccDocGoods> goodAccDocs = new List<AccDocGoods>();
		List<Guid> exportedDocuments = new List<Guid>();
		bool isReasonableUpdateDoc = false;
		EnumerateErrorsReportViewer errorRepViewer = new EnumerateErrorsReportViewer(ArmSoft.SME.Common.Resources.ASerrWindow, "ExportInvoicesToTaxService");
		List<(TaxDocument, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE)> docs = ExportInvoicesToTaxService(ref errorRepViewer, taxInvoices, taxCorrectionInvoices, taxInvoicesRetail, taxBill, ref isReasonableUpdateDoc, isAccDoc: true, ref serviceInvoices, ref goodInvoices, ref servAccDocs, ref goodAccDocs);
		try
		{
			Mouse.OverrideCursor = Cursors.Wait;
			if (text == TaxServiceSourceTypeSE.File.Value.ToString())
			{
				string exportDirectory = Settings.CommonCoreHelper.GetExportDirectory();
				if (!string.IsNullOrWhiteSpace(exportDirectory))
				{
					exportedDocuments = ExportMarkingsToTaxService(docs, short.Parse(text), exportDirectory, errorRepViewer);
				}
			}
			else
			{
				exportedDocuments = ExportMarkingsToTaxService(docs, short.Parse(text), "", errorRepViewer);
			}
			UpdateDocExporteMarkingsdState(errorRepViewer, exportedDocuments, short.Parse(text));
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			Mouse.OverrideCursor = null;
		}
		if (errorRepViewer.HasErrors)
		{
			errorRepViewer.Show();
		}
	}

	private TaxDocument CreateTaxDocument(MTInvoice invoice, bool isSelectedExportTypeGood)
	{
		TINInfo tINInfo = null;
		TINInfo tINInfo2 = null;
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(invoice.PartnerContractGroup.PartnerCode);
		PartnerCacheRow.ContractRow contractRow = null;
		TaxDocument taxInvoice = (TaxDocument)Settings.DocumentService.Create(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.TaxDocument.Value);
		if (_eInvExportType == TaxServiceSourceTypeSE.EInvoicing.Value && Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
		{
			tINInfo = Settings.TaxService.Client.GetTinInfo(Settings.ParametersService.TaxCode());
			if (!string.IsNullOrWhiteSpace(invoice.BuyerTaxCode))
			{
				tINInfo2 = Settings.TaxService.Client.GetTinInfo(invoice.BuyerTaxCode, invoice.DocDate.ToDateTime());
			}
		}
		if (item != null)
		{
			contractRow = TradeSettings.TradeCacheService.Partner.GetContract(item, invoice.PartnerContractGroup.ContractCode);
		}
		taxInvoice.ISN = invoice.ISN;
		taxInvoice.DocNum = invoice.DocNum;
		taxInvoice.DocDate = invoice.DocDate;
		taxInvoice.BaseDocISN = invoice.BaseISN;
		taxInvoice.SupplyDate = invoice.DocDate;
		taxInvoice.DealNumber = contractRow?.Description;
		taxInvoice.DealDate = contractRow?.ContractDate;
		taxInvoice.AdditionalInfo = invoice.Comment;
		taxInvoice.SupplierTIN = Settings.ParametersService.TaxCode();
		taxInvoice.SupplierVATNumber = ((tINInfo != null) ? tINInfo.VatNumber : "");
		taxInvoice.SupplierName = ((tINInfo != null) ? (tINInfo.Name + " " + tINInfo.OrganizationalType) : Settings.ParametersService.CompanyName());
		taxInvoice.SupplierAddress = ((tINInfo != null) ? tINInfo.Address : Settings.ParametersService.CompanyAddress());
		taxInvoice.SupplierBankAccountNumber = invoice.SupplierSettlAcc.Trim();
		taxInvoice.SupplierBankName = Settings.CoreCacheService.Bank.GetBankCaptionBySettlementAccount(invoice.SupplierSettlAcc.Trim(), out var bankCode);
		taxInvoice.SupplierAdditionalData = invoice.SupplierAdditionalData.Trim();
		taxInvoice.BuyerHasNoTin = _isNotRequiredTaxCode;
		taxInvoice.SourceFullAddress = ((!string.IsNullOrWhiteSpace(invoice.SupplierBusinessAddress)) ? invoice.SupplierBusinessAddress.Trim() : Settings.ParametersService.CompanyBusinessAddress());
		taxInvoice.BuyerTIN = ((tINInfo2 != null) ? tINInfo2.TIN : invoice.BuyerTaxCode);
		taxInvoice.BuyerVATNumber = ((tINInfo2 != null) ? tINInfo2.VatNumberForDate : "");
		taxInvoice.BuyerIdentityType = (IdentityType)((!string.IsNullOrWhiteSpace(invoice.BuyerIDDocType)) ? Convert.ToInt16(invoice.BuyerIDDocType) : 0);
		taxInvoice.BuyerIdentityNo = invoice.BuyerPassport.Trim();
		taxInvoice.BuyerName = ((tINInfo2 != null && !string.IsNullOrWhiteSpace(tINInfo2.Name)) ? (tINInfo2.Name + " " + tINInfo2.OrganizationalType) : invoice.BuyerName);
		taxInvoice.BuyerAddress = ((tINInfo2 != null && !string.IsNullOrWhiteSpace(tINInfo2.Address)) ? tINInfo2.Address : invoice.BuyerLegalAddress);
		taxInvoice.BuyerBankName = Settings.CoreCacheService.Bank.GetBankCaptionBySettlementAccount(invoice.BuyerSettlAcc.Trim(), out bankCode);
		taxInvoice.BuyerBankAccountNumber = invoice.BuyerSettlAcc.Trim();
		taxInvoice.BuyerAdditionalData = invoice.BuyerAdditionalData;
		if (invoice.BuyerTaxCode.StartsWith("9"))
		{
			taxInvoice.BuyerTIN = invoice.BuyerTaxCode.Trim();
			taxInvoice.BuyerHasNoTin = true;
			taxInvoice.BuyerName = invoice.BuyerName;
			taxInvoice.BuyerAddress = invoice.BuyerLegalAddress;
		}
		taxInvoice.MediatorName = invoice.ByWhomFio;
		taxInvoice.MediatorLicenseDate = invoice.AuthorizationDate;
		taxInvoice.MediatorLicenseNumber = invoice.AuthorizationNumber;
		taxInvoice.DeliveryMethod = invoice.BuyerShippingMethod;
		taxInvoice.DestinationFullAddress = ((!string.IsNullOrWhiteSpace(invoice.BuyerBusinessAddress)) ? invoice.BuyerBusinessAddress.Trim() : invoice.BuyerLegalAddress.Trim());
		taxInvoice.Markings = invoice.MTList.SelectMany((MTInvoiceBase.MTInvoiceSpecificationRow x) => x.MarkingList.ToList()).ToList();
		FillProductsList(ref taxInvoice, invoice, _exportDiscount, _exportCaption, isSelectedExportTypeGood: false);
		return taxInvoice;
	}

	private TaxDocument CreateTaxDocument(MTBill invoice, bool isSelectedExportTypeGood)
	{
		TINInfo tINInfo = null;
		TINInfo tINInfo2 = null;
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(invoice.PartnerContractGroup.PartnerCode);
		PartnerCacheRow.ContractRow contractRow = null;
		TaxDocument taxInvoice = (TaxDocument)Settings.DocumentService.Create(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.TaxDocument.Value);
		if (_eInvExportType == TaxServiceSourceTypeSE.EInvoicing.Value && Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
		{
			tINInfo = Settings.TaxService.Client.GetTinInfo(Settings.ParametersService.TaxCode());
			if (!string.IsNullOrWhiteSpace(item?.TaxCode))
			{
				tINInfo2 = Settings.TaxService.Client.GetTinInfo(item.TaxCode, invoice.DocDate.ToDateTime());
			}
		}
		if (item != null)
		{
			contractRow = TradeSettings.TradeCacheService.Partner.GetContract(item, invoice.PartnerContractGroup.ContractCode);
		}
		taxInvoice.ISN = invoice.ISN;
		taxInvoice.DocNum = invoice.DocNum;
		taxInvoice.DocDate = invoice.DocDate;
		taxInvoice.BaseDocISN = invoice.BaseISN;
		taxInvoice.SupplyDate = invoice.DocDate;
		taxInvoice.DealNumber = contractRow?.Description;
		taxInvoice.DealDate = contractRow?.ContractDate;
		taxInvoice.AdditionalInfo = invoice.Comment;
		taxInvoice.SupplierTIN = Settings.ParametersService.TaxCode();
		taxInvoice.SupplierVATNumber = ((tINInfo != null) ? tINInfo.VatNumber : "");
		taxInvoice.SupplierName = ((tINInfo != null) ? (tINInfo.Name + " " + tINInfo.OrganizationalType) : Settings.ParametersService.CompanyName());
		taxInvoice.SupplierAddress = ((tINInfo != null) ? tINInfo.Address : Settings.ParametersService.CompanyAddress());
		taxInvoice.SupplierBankAccountNumber = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount()?.SettlAccount ?? "";
		taxInvoice.SupplierBankName = Settings.CoreCacheService.Bank.GetBankCaptionBySettlementAccount(taxInvoice.SupplierBankAccountNumber.Trim(), out var bankCode);
		string text = TradeSettings.TradeCacheService.Storage.GetItem(invoice.Storage)?.Address;
		taxInvoice.SourceFullAddress = ((!string.IsNullOrWhiteSpace(text)) ? text : Settings.ParametersService.CompanyBusinessAddress());
		taxInvoice.BuyerHasNoTin = _isNotRequiredTaxCode;
		taxInvoice.CrmNo = invoice.ECRCheckNum;
		if (tINInfo2 != null)
		{
			taxInvoice.BuyerTIN = tINInfo2.TIN;
			taxInvoice.BuyerVATNumber = tINInfo2.VatNumberForDate;
			taxInvoice.BuyerIdentityType = (IdentityType)((!string.IsNullOrWhiteSpace(item.IDDocType)) ? Convert.ToInt16(item.IDDocType) : 0);
			taxInvoice.BuyerIdentityNo = item.Passport.Trim();
			taxInvoice.BuyerName = ((!string.IsNullOrWhiteSpace(tINInfo2.Name)) ? (tINInfo2.Name + " " + tINInfo2.OrganizationalType) : invoice.BuyerName);
			taxInvoice.BuyerAddress = ((!string.IsNullOrWhiteSpace(tINInfo2.Address)) ? tINInfo2.Address : item.Address);
			taxInvoice.DestinationFullAddress = item.BusinessAddress;
		}
		else if (item != null)
		{
			taxInvoice.BuyerTIN = item.TaxCode;
			taxInvoice.BuyerVATNumber = string.Empty;
			taxInvoice.BuyerIdentityType = (IdentityType)((!string.IsNullOrWhiteSpace(item.IDDocType)) ? Convert.ToInt16(item.IDDocType) : 0);
			taxInvoice.BuyerIdentityNo = item.Passport;
			taxInvoice.BuyerName = item.Caption;
			taxInvoice.BuyerAddress = item.Address;
			taxInvoice.DestinationFullAddress = item.BusinessAddress;
		}
		taxInvoice.BuyerBankName = ((item != null) ? Settings.CoreCacheService.Bank.GetBankCaptionBySettlementAccount(item.SettlAcc.Trim(), out bankCode) : string.Empty);
		taxInvoice.BuyerBankAccountNumber = ((item != null) ? item.SettlAcc.Trim() : string.Empty);
		FillProductsList(ref taxInvoice, invoice, _exportDiscount, _exportCaption, isSelectedExportTypeGood);
		return taxInvoice;
	}

	private TaxDocument CreateTaxDocument(MTInvoiceRetail invoice, bool isSelectedExportTypeGood)
	{
		TINInfo tINInfo = null;
		TINInfo tINInfo2 = null;
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(invoice.PartnerContractGroup.PartnerCode);
		PartnerCacheRow.ContractRow contractRow = null;
		TaxDocument taxInvoice = (TaxDocument)Settings.DocumentService.Create(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.TaxDocument.Value);
		if (_eInvExportType == TaxServiceSourceTypeSE.EInvoicing.Value && Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
		{
			tINInfo = Settings.TaxService.Client.GetTinInfo(Settings.ParametersService.TaxCode());
			if (!string.IsNullOrWhiteSpace(invoice.BuyerTaxCode))
			{
				tINInfo2 = Settings.TaxService.Client.GetTinInfo(invoice.BuyerTaxCode, invoice.DocDate.ToDateTime());
			}
		}
		if (item != null)
		{
			contractRow = TradeSettings.TradeCacheService.Partner.GetContract(item, invoice.PartnerContractGroup.ContractCode);
		}
		taxInvoice.ISN = invoice.ISN;
		taxInvoice.DocNum = invoice.DocNum;
		taxInvoice.DocDate = invoice.DocDate;
		taxInvoice.BaseDocISN = invoice.BaseISN;
		taxInvoice.SupplyDate = invoice.DocDate;
		taxInvoice.DealNumber = contractRow?.Description;
		taxInvoice.DealDate = contractRow?.ContractDate;
		taxInvoice.AdditionalInfo = invoice.Comment;
		taxInvoice.SupplierTIN = Settings.ParametersService.TaxCode();
		taxInvoice.SupplierVATNumber = ((tINInfo != null) ? tINInfo.VatNumber : "");
		taxInvoice.SupplierName = ((tINInfo != null) ? (tINInfo.Name + " " + tINInfo.OrganizationalType) : Settings.ParametersService.CompanyName());
		taxInvoice.SupplierAddress = ((tINInfo != null) ? tINInfo.Address : Settings.ParametersService.CompanyAddress());
		taxInvoice.SupplierBankAccountNumber = invoice.SupplierSettlAcc.Trim();
		taxInvoice.SupplierBankName = Settings.CoreCacheService.Bank.GetBankCaptionBySettlementAccount(invoice.SupplierSettlAcc.Trim(), out var bankCode);
		taxInvoice.SupplierAdditionalData = invoice.SupplierAdditionalData.Trim();
		taxInvoice.SourceFullAddress = ((!string.IsNullOrWhiteSpace(invoice.SupplierBusinessAddress)) ? invoice.SupplierBusinessAddress.Trim() : Settings.ParametersService.CompanyBusinessAddress());
		taxInvoice.BuyerHasNoTin = _isNotRequiredTaxCode;
		taxInvoice.BuyerTIN = ((tINInfo2 != null) ? tINInfo2.TIN : invoice.BuyerTaxCode);
		taxInvoice.BuyerVATNumber = ((tINInfo2 != null) ? tINInfo2.VatNumberForDate : "");
		taxInvoice.BuyerIdentityType = (IdentityType)((!string.IsNullOrWhiteSpace(invoice.BuyerIDDocType)) ? Convert.ToInt16(invoice.BuyerIDDocType) : 0);
		taxInvoice.BuyerIdentityNo = invoice.BuyerPassport.Trim();
		taxInvoice.BuyerName = ((tINInfo2 != null && !string.IsNullOrWhiteSpace(tINInfo2.Name)) ? (tINInfo2.Name + " " + tINInfo2.OrganizationalType) : invoice.BuyerName);
		taxInvoice.BuyerAddress = ((tINInfo2 != null && !string.IsNullOrWhiteSpace(tINInfo2.Address)) ? tINInfo2.Address : invoice.BuyerLegalAddress);
		taxInvoice.BuyerBankName = Settings.CoreCacheService.Bank.GetBankCaptionBySettlementAccount(invoice.BuyerSettlAcc.Trim(), out bankCode);
		taxInvoice.BuyerBankAccountNumber = invoice.BuyerSettlAcc.Trim();
		taxInvoice.BuyerAdditionalData = invoice.BuyerAdditionalData;
		taxInvoice.BuyerBankName = Settings.CoreCacheService.Bank.GetBankCaptionBySettlementAccount(invoice.BuyerSettlAcc.Trim(), out bankCode);
		if (invoice.BuyerTaxCode.StartsWith("9"))
		{
			taxInvoice.BuyerTIN = invoice.BuyerTaxCode.Trim();
			taxInvoice.BuyerHasNoTin = true;
			taxInvoice.BuyerName = invoice.BuyerName;
			taxInvoice.BuyerAddress = invoice.BuyerLegalAddress;
		}
		taxInvoice.MediatorName = invoice.ByWhomFio;
		taxInvoice.MediatorLicenseDate = invoice.AuthorizationDate;
		taxInvoice.MediatorLicenseNumber = invoice.AuthorizationNumber;
		taxInvoice.DeliveryMethod = invoice.BuyerShippingMethod;
		taxInvoice.DestinationFullAddress = ((!string.IsNullOrWhiteSpace(invoice.BuyerBusinessAddress)) ? invoice.BuyerBusinessAddress.Trim() : invoice.BuyerLegalAddress.Trim());
		FillProductsList(ref taxInvoice, invoice, _exportDiscount, _exportCaption, isSelectedExportTypeGood);
		return taxInvoice;
	}

	private TaxDocument CreateTaxDocument(MTReturnFromCustomer invoice, bool isSelectedExportTypeGood)
	{
		TINInfo tINInfo = null;
		TINInfo tINInfo2 = null;
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(invoice.PartnerContractGroup.PartnerCode);
		PartnerCacheRow.ContractRow contractRow = null;
		TaxDocument taxInvoice = (TaxDocument)Settings.DocumentService.Create(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.TaxDocument.Value);
		if (_eInvExportType == TaxServiceSourceTypeSE.EInvoicing.Value && Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
		{
			tINInfo = Settings.TaxService.Client.GetTinInfo(Settings.ParametersService.TaxCode());
			if (!string.IsNullOrWhiteSpace(invoice.BuyerTaxCode))
			{
				tINInfo2 = Settings.TaxService.Client.GetTinInfo(invoice.BuyerTaxCode, invoice.DocDate.ToDateTime());
			}
		}
		if (item != null)
		{
			contractRow = TradeSettings.TradeCacheService.Partner.GetContract(item, invoice.PartnerContractGroup.ContractCode);
		}
		taxInvoice.ISN = invoice.ISN;
		taxInvoice.DocNum = invoice.DocNum;
		taxInvoice.DocDate = invoice.DocDate;
		taxInvoice.BaseDocISN = invoice.BaseISN;
		taxInvoice.BaseSerialNumber = invoice.BaseDocTaxInvoiceSeries.ToString() + invoice.BaseDocTaxInvoiceNumber.ToString();
		taxInvoice.BaseDate = invoice.BaseDocTaxInvoiceDate;
		taxInvoice.SupplyDate = invoice.DocDate;
		taxInvoice.DealNumber = contractRow?.Description;
		taxInvoice.DealDate = contractRow?.ContractDate;
		taxInvoice.AdditionalInfo = invoice.Comment;
		taxInvoice.SupplierTIN = Settings.ParametersService.TaxCode();
		taxInvoice.SupplierVATNumber = ((tINInfo != null) ? tINInfo.VatNumber : "");
		taxInvoice.SupplierName = ((tINInfo != null) ? (tINInfo.Name + " " + tINInfo.OrganizationalType) : Settings.ParametersService.CompanyName());
		taxInvoice.SupplierAddress = ((tINInfo != null) ? tINInfo.Address : Settings.ParametersService.CompanyAddress());
		taxInvoice.SupplierBankAccountNumber = invoice.SupplierSettlAcc.Trim();
		taxInvoice.SupplierBankName = Settings.CoreCacheService.Bank.GetBankCaptionBySettlementAccount(invoice.SupplierSettlAcc.Trim(), out var bankCode);
		taxInvoice.CrmNo = invoice.ECRCheckNum;
		taxInvoice.BuyerHasNoTin = _isNotRequiredTaxCode;
		taxInvoice.SourceFullAddress = invoice.BuyerBusinessAddress;
		taxInvoice.BuyerTIN = ((tINInfo2 != null) ? tINInfo2.TIN : invoice.BuyerTaxCode);
		taxInvoice.BuyerVATNumber = ((tINInfo2 != null) ? tINInfo2.VatNumberForDate : "");
		taxInvoice.BuyerIdentityType = (IdentityType)((!string.IsNullOrWhiteSpace(invoice.BuyerIDDocType)) ? Convert.ToInt16(invoice.BuyerIDDocType) : 0);
		taxInvoice.BuyerIdentityNo = invoice.BuyerPassport.Trim();
		taxInvoice.BuyerName = ((tINInfo2 != null && !string.IsNullOrWhiteSpace(tINInfo2.Name)) ? (tINInfo2.Name + " " + tINInfo2.OrganizationalType) : invoice.BuyerName);
		taxInvoice.BuyerAddress = ((tINInfo2 != null && !string.IsNullOrWhiteSpace(tINInfo2.Address)) ? tINInfo2.Address : invoice.BuyerLegalAddress);
		taxInvoice.BuyerBankName = Settings.CoreCacheService.Bank.GetBankCaptionBySettlementAccount(invoice.BuyerSettlAcc.Trim(), out bankCode);
		taxInvoice.BuyerBankAccountNumber = invoice.BuyerSettlAcc.Trim();
		taxInvoice.BuyerAdditionalData = invoice.BuyerAdditionalData;
		if (invoice.BuyerTaxCode.StartsWith("9"))
		{
			taxInvoice.BuyerTIN = invoice.BuyerTaxCode.Trim();
			taxInvoice.BuyerHasNoTin = true;
			taxInvoice.BuyerName = invoice.BuyerName;
			taxInvoice.BuyerAddress = invoice.BuyerLegalAddress;
		}
		taxInvoice.MediatorName = invoice.ByWhomFio;
		taxInvoice.MediatorLicenseDate = invoice.AuthorizationDate;
		taxInvoice.MediatorLicenseNumber = invoice.AuthorizationNumber;
		taxInvoice.DeliveryMethod = invoice.BuyerShippingMethod;
		taxInvoice.DestinationFullAddress = ((!string.IsNullOrWhiteSpace(invoice.SupplierBusinessAddress)) ? invoice.SupplierBusinessAddress.Trim() : Settings.ParametersService.CompanyBusinessAddress());
		FillProductsList(ref taxInvoice, invoice, _exportDiscount, _exportCaption, isSelectedExportTypeGood);
		return taxInvoice;
	}

	private TaxDocument CreateTaxDocument(MTMoveWithConfirmationBase mtMove)
	{
		TINInfo tINInfo = null;
		TaxDocument taxInvoice = (TaxDocument)Settings.DocumentService.Create(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.TaxDocument.Value);
		if (_eInvExportType == TaxServiceSourceTypeSE.EInvoicing.Value && Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
		{
			tINInfo = Settings.TaxService.Client.GetTinInfo(Settings.ParametersService.TaxCode());
		}
		taxInvoice.ISN = mtMove.ISN;
		taxInvoice.DocNum = mtMove.DocNum;
		taxInvoice.DocDate = mtMove.DocDate;
		taxInvoice.BaseDocISN = mtMove.BaseISN;
		taxInvoice.SupplyDate = mtMove.DocDate;
		taxInvoice.DealNumber = mtMove.PartnerContractGroup.ContractCode.Trim();
		taxInvoice.AdditionalInfo = mtMove.Comment;
		taxInvoice.SupplierTIN = Settings.ParametersService.TaxCode();
		taxInvoice.SupplierVATNumber = ((tINInfo != null) ? tINInfo.VatNumber : "");
		taxInvoice.SupplierName = ((tINInfo != null) ? (tINInfo.Name + " " + tINInfo.OrganizationalType) : Settings.ParametersService.CompanyName());
		taxInvoice.SupplierAddress = ((tINInfo != null) ? tINInfo.Address : Settings.ParametersService.CompanyAddress());
		string obj = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount()?.SettlAccount ?? "";
		string settlementAccount = (taxInvoice.SupplierBankAccountNumber = obj);
		taxInvoice.SupplierBankName = Settings.CoreCacheService.Bank.GetBankCaptionBySettlementAccount(settlementAccount, out var _);
		StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(mtMove.StorageExpense);
		taxInvoice.SourceFullAddress = ((item == null) ? "" : item.Address);
		bool storageExpIsRetail = item?.Retail ?? false;
		item = TradeSettings.TradeCacheService.Storage.GetItem(mtMove.StorageIncome);
		bool storageIncIsRetail = item?.Retail ?? false;
		taxInvoice.DestinationFullAddress = ((item == null) ? "" : item.Address);
		taxInvoice.DeliveryMethod = mtMove.TransportationMethod;
		taxInvoice.SourceIsCar = mtMove.MotorVehicle;
		taxInvoice.SourceCarMake = mtMove.Brand;
		taxInvoice.SourceCarPlateNumber = mtMove.LicensePlate;
		FillProductsList(ref taxInvoice, mtMove, _exportCaption, _priceType, storageExpIsRetail, storageIncIsRetail);
		return taxInvoice;
	}

	private void FillProductsList(ref TaxDocument taxInvoice, MTInvoice invoice, bool exportDiscount, bool exportCaption, bool isSelectedExportTypeGood)
	{
		decimal num = 0m;
		decimal number = 0m;
		decimal num2 = 0m;
		decimal number2 = 0m;
		string itemCode = string.Empty;
		string itemName = string.Empty;
		List<MTInvoiceBase.MTInvoiceSpecificationRow> mTList = invoice.MTList;
		string vATType = invoice.VATType;
		if (mTList.Count == 0)
		{
			return;
		}
		foreach (MTInvoiceBase.MTInvoiceSpecificationRow item in mTList)
		{
			MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(item.ItemID);
			if (itemById != null)
			{
				itemName = ((!exportCaption && !string.IsNullOrWhiteSpace(itemById.FullCaption)) ? itemById.FullCaption : itemById.Caption);
				itemCode = itemById.MTCode;
			}
			decimal num3 = (item.Summa * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base).RoundAs(2);
			if (item.VAT.HasValue && item.VAT.Value)
			{
				if (vATType.InList("1", "2", "5"))
				{
					num2 = CalcVATAmountByType(item.Summa * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base, vATType, 5);
					if (vATType != "1")
					{
						num3 -= CalcVATAmountByType(num3, vATType, 5);
					}
					number2 += num2;
				}
			}
			else
			{
				num2 = 0m;
			}
			decimal price;
			if (item.Discount == 0m || !exportDiscount)
			{
				price = (num3 / item.Quantity).RoundAs(5);
			}
			else
			{
				price = ((!(item.Discount == 100m)) ? (num3 / item.Quantity * 100m / (100m - item.Discount)).RoundAs(5) : 0m);
			}
			num3 = (item.Summa * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base).RoundAs(2);
			if (item.VAT.HasValue && item.VAT.Value && vATType == "2")
			{
				num3 -= num2;
			}
			decimal num4 = num3.RoundAs(2);
			decimal totalSum = ((!vATType.InList("1", "2", "5")) ? num3.RoundAs(2) : (num3 + num2).RoundAs(2));
			num += num4;
			decimal discount = ((!exportDiscount || !(item.Discount != 0m)) ? 0m : item.Discount.RoundAs(2));
			decimal? num5 = ((!vATType.InList("1", "2", "5") || !item.VAT.HasValue || !item.VAT.Value) ? new decimal?(0m) : new decimal?(Settings.ParametersService.VAT()));
			decimal? num6 = num2.RoundAs(2);
			decimal num7 = (item.EnvFeeSumma * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base).RoundAs(2);
			number += num7;
			string cPACode = "";
			if ((item.ItemType == InvoiceRowTypeSE.Goods.Value) | isSelectedExportTypeGood)
			{
				cPACode = ((item.CPACode.Length > 4) ? item.CPACode.Substring(0, 4) : item.CPACode);
			}
			else if (item.ItemType == InvoiceRowTypeSE.Services.Value)
			{
				cPACode = Settings.ParametersService.CPACodeForServices();
			}
			taxInvoice.Items.Add(new TaxDocument.SpecificationRow
			{
				ItemID = item.ItemID,
				ItemCode = itemCode,
				ItemName = itemName,
				CPACode = cPACode,
				UnitBrief = item.UnitBrief,
				Quantity = item.Quantity.RoundAs(3),
				Price = price,
				Sum = num4,
				Discount = discount,
				VAT = num5.Value,
				DealType = item.DealType,
				SumVAT = num6.Value,
				EnvironmentalFee = num7,
				TotalSum = totalSum,
				RowId = item.RowID,
				RowISN = Guid.NewGuid()
			});
		}
		decimal num8 = ((!(invoice.CurrCode == "AMD")) ? number2.RoundAs(2) : invoice.SummVAT.RoundAs(2));
		num = (invoice.Summ.Value * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base).RoundAs(2);
		taxInvoice.TotalVAT = num8;
		taxInvoice.TotalSumWithoutTaxes = num - num8;
		taxInvoice.TotalSum = num;
		taxInvoice.TotalEnvironmentalFee = number.RoundAs(2);
	}

	private void FillProductsList(ref TaxDocument taxInvoice, MTBill invoice, bool exportDiscount, bool exportCaption, bool isSelectedExportTypeGood)
	{
		decimal num = 0m;
		decimal number = 0m;
		decimal number2 = 0m;
		decimal num2 = 0m;
		string itemCode = string.Empty;
		string itemName = string.Empty;
		List<MTInvoiceBase.MTInvoiceSpecificationRow> mTList = invoice.MTList;
		string vATType = invoice.VATType;
		if (mTList.Count == 0)
		{
			return;
		}
		foreach (MTInvoiceBase.MTInvoiceSpecificationRow item in mTList)
		{
			MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(item.ItemID);
			if (itemById != null)
			{
				itemName = ((!exportCaption && !string.IsNullOrWhiteSpace(itemById.FullCaption)) ? itemById.FullCaption : itemById.Caption);
				itemCode = itemById.MTCode;
			}
			decimal num3 = (item.Summa * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base).RoundAs(2);
			if (item.VAT.HasValue && item.VAT.Value)
			{
				if (vATType == "5")
				{
					num2 = CalcVATAmountByType(num3, "2", 5);
					num3 -= num2;
					number += num2;
				}
			}
			else
			{
				num2 = 0m;
			}
			decimal price;
			if (item.Discount == 0m || !exportDiscount)
			{
				price = (num3 / item.Quantity).RoundAs(5);
			}
			else
			{
				price = ((!(item.Discount == 100m)) ? (num3 / item.Quantity * 100m / (100m - item.Discount)).RoundAs(5) : 0m);
			}
			num3 = (item.Summa * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base).RoundAs(2);
			if (item.VAT.HasValue && item.VAT.Value && vATType == "5")
			{
				num3 -= num2;
			}
			decimal num4 = num3.RoundAs(2);
			decimal totalSum = ((!(vATType == "5")) ? num3.RoundAs(2) : (num3 + num2).RoundAs(2));
			num += num4;
			decimal discount = ((!exportDiscount || !(item.Discount != 0m)) ? 0m : item.Discount.RoundAs(2));
			decimal? num5 = ((!(vATType == "5") || !item.VAT.HasValue || !item.VAT.Value) ? new decimal?(0m) : new decimal?(Settings.ParametersService.VAT()));
			decimal? num6 = num2.RoundAs(2);
			decimal environmentalFee = (item.EnvFeeSumma * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base).RoundAs(2);
			number2 += item.EnvFeeSumma;
			string cPACode = "";
			if ((item.ItemType == InvoiceRowTypeSE.Goods.Value) | isSelectedExportTypeGood)
			{
				cPACode = ((item.CPACode.Length > 4) ? item.CPACode.Substring(0, 4) : item.CPACode);
			}
			else if (item.ItemType == InvoiceRowTypeSE.Services.Value)
			{
				cPACode = Settings.ParametersService.CPACodeForServices();
			}
			taxInvoice.Items.Add(new TaxDocument.SpecificationRow
			{
				ItemID = item.ItemID,
				ItemCode = itemCode,
				ItemName = itemName,
				CPACode = cPACode,
				UnitBrief = item.UnitBrief,
				Quantity = item.Quantity.RoundAs(3),
				Price = price,
				Sum = num4,
				Discount = discount,
				VAT = num5.Value,
				DealType = item.DealType,
				SumVAT = num6.Value,
				EnvironmentalFee = environmentalFee,
				TotalSum = totalSum,
				RowId = item.RowID,
				RowISN = Guid.NewGuid()
			});
		}
		num = (invoice.Summ.Value * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base).RoundAs(2);
		number = number.RoundAs(2);
		taxInvoice.TotalVAT = number;
		taxInvoice.TotalSumWithoutTaxes = num - number;
		taxInvoice.TotalSum = num;
		taxInvoice.TotalEnvironmentalFee = number2.RoundAs(2);
	}

	private void FillProductsList(ref TaxDocument taxInvoice, MTInvoiceRetail invoice, bool exportDiscount, bool exportCaption, bool isSelectedExportTypeGood)
	{
		decimal num = 0m;
		decimal number = 0m;
		decimal num2 = 0m;
		decimal num3 = 0m;
		decimal number2 = 0m;
		string itemCode = string.Empty;
		string itemName = string.Empty;
		decimal? num4 = null;
		decimal? num5 = null;
		MaterialCacheRow materialCacheRow = null;
		List<MTInvoiceBase.MTInvoiceSpecificationRow> mTList = invoice.MTList;
		string vATType = invoice.VATType;
		if (mTList.Count == 0)
		{
			return;
		}
		foreach (MTInvoiceBase.MTInvoiceSpecificationRow item in mTList)
		{
			materialCacheRow = TradeSettings.TradeCacheService.Material.GetItemById(item.ItemID);
			if (materialCacheRow != null)
			{
				itemName = ((!exportCaption && !string.IsNullOrWhiteSpace(materialCacheRow.FullCaption)) ? materialCacheRow.FullCaption : materialCacheRow.Caption);
				itemCode = materialCacheRow.MTCode;
			}
			num3 = (item.Summa * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base).RoundAs(2);
			if (item.VAT.HasValue && item.VAT.Value)
			{
				if (vATType.InList("1", "2", "5"))
				{
					num2 = CalcVATAmountByType(item.Summa * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base, vATType, 5);
					num3 -= CalcVATAmountByType(num3, vATType, 5);
					number2 += num2;
				}
			}
			else
			{
				num2 = 0m;
			}
			decimal price;
			if (item.Discount == 0m || !exportDiscount)
			{
				price = (num3 / item.Quantity).RoundAs(5);
			}
			else
			{
				price = ((!(item.Discount == 100m)) ? (num3 / item.Quantity * 100m / (100m - item.Discount)).RoundAs(5) : 0m);
			}
			num3 = (item.Summa * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base).RoundAs(2);
			if (item.VAT.HasValue && item.VAT.Value && vATType.InList("1", "2", "5"))
			{
				num3 -= num2;
			}
			decimal num6 = num3.RoundAs(2);
			decimal totalSum = ((!vATType.InList("1", "2", "5")) ? num3.RoundAs(2) : (num3 + num2).RoundAs(2));
			num += num6;
			decimal discount = ((!exportDiscount || !(item.Discount != 0m)) ? 0m : item.Discount.RoundAs(2));
			num4 = ((!vATType.InList("1", "2", "5") || !item.VAT.HasValue || !item.VAT.Value) ? new decimal?(0m) : new decimal?(Settings.ParametersService.VAT()));
			num5 = num2.RoundAs(2);
			decimal environmentalFee = (item.EnvFeeSumma * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base).RoundAs(2);
			number += item.EnvFeeSumma;
			string cPACode = "";
			if ((item.ItemType == InvoiceRowTypeSE.Goods.Value) | isSelectedExportTypeGood)
			{
				cPACode = ((item.CPACode.Length > 4) ? item.CPACode.Substring(0, 4) : item.CPACode);
			}
			else if (item.ItemType == InvoiceRowTypeSE.Services.Value)
			{
				cPACode = Settings.ParametersService.CPACodeForServices();
			}
			taxInvoice.Items.Add(new TaxDocument.SpecificationRow
			{
				ItemID = item.ItemID,
				ItemCode = itemCode,
				ItemName = itemName,
				CPACode = cPACode,
				UnitBrief = item.UnitBrief,
				Quantity = item.Quantity.RoundAs(3),
				Price = price,
				Sum = num6,
				Discount = discount,
				VAT = num4.Value,
				DealType = item.DealType,
				SumVAT = num5.Value,
				EnvironmentalFee = environmentalFee,
				TotalSum = totalSum,
				RowId = item.RowID,
				RowISN = Guid.NewGuid()
			});
		}
		num = (invoice.Summ.Value * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base).RoundAs(2);
		if (invoice.CurrCode == "AMD")
		{
			taxInvoice.TotalVAT = invoice.SummVAT.RoundAs(2);
		}
		else
		{
			taxInvoice.TotalVAT = number2.RoundAs(2);
		}
		taxInvoice.TotalSumWithoutTaxes = num - invoice.SummVAT;
		taxInvoice.TotalSum = num;
		taxInvoice.TotalEnvironmentalFee = number.RoundAs(2);
	}

	private void FillProductsList(ref TaxDocument taxInvoice, MTReturnFromCustomer invoice, bool exportDiscount, bool exportCaption, bool isSelectedExportTypeGood)
	{
		decimal num = 0m;
		decimal num2 = 0m;
		decimal number = 0m;
		decimal num3 = 0m;
		decimal num4 = 0m;
		string itemCode = string.Empty;
		string itemName = string.Empty;
		decimal? num5 = null;
		decimal? num6 = null;
		MaterialCacheRow materialCacheRow = null;
		List<MTReturnFromCustomerBase.MTRetFromCustSpecificationRow> mTList = invoice.MTList;
		string vATType = invoice.VATType;
		if (mTList.Count == 0)
		{
			return;
		}
		foreach (MTReturnFromCustomerBase.MTRetFromCustSpecificationRow item in mTList)
		{
			materialCacheRow = TradeSettings.TradeCacheService.Material.GetItemById(item.ItemID.Value);
			if (materialCacheRow != null)
			{
				itemName = ((!exportCaption && !string.IsNullOrWhiteSpace(materialCacheRow.FullCaption)) ? materialCacheRow.FullCaption : materialCacheRow.Caption);
				itemCode = materialCacheRow.MTCode;
			}
			num4 = (item.Summa.Value * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base).RoundAs(2);
			if (item.VAT.Value)
			{
				if (vATType.InList("1", "2", "5"))
				{
					num3 = CalcVATAmountByType(item.Summa.Value * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base, vATType, 5);
					if (vATType != "1")
					{
						num4 -= CalcVATAmountByType(num4, vATType, 5);
					}
				}
			}
			else
			{
				num3 = 0m;
			}
			decimal? discount = item.Discount;
			decimal price;
			if (((discount.GetValueOrDefault() == 0m) & discount.HasValue) || !exportDiscount)
			{
				price = (num4 / item.Quantity.Value).RoundAs(5);
			}
			else
			{
				discount = item.Discount;
				decimal num7 = 100;
				price = ((!((discount.GetValueOrDefault() == num7) & discount.HasValue)) ? (num4 / item.Quantity.Value * 100m / (100m - item.Discount.Value)).RoundAs(5) : 0m);
			}
			num4 = (item.Summa.Value * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base).RoundAs(2);
			if (item.VAT.Value && vATType == "2")
			{
				num4 -= num3;
			}
			decimal num8 = num4.RoundAs(2);
			decimal totalSum = ((!vATType.InList("1", "2", "5")) ? num4.RoundAs(2) : (num4 + num3).RoundAs(2));
			num += num8;
			decimal discount2 = ((!exportDiscount || !(item.Discount.Value != 0m)) ? 0m : item.Discount.Value.RoundAs(2));
			num5 = ((!vATType.InList("1", "2", "5") || !item.VAT.Value) ? new decimal?(0m) : new decimal?(Settings.ParametersService.VAT()));
			num6 = num3.RoundAs(2);
			string cPACode = "";
			if ((item.ItemType == InvoiceRowTypeSE.Goods.Value) | isSelectedExportTypeGood)
			{
				cPACode = ((item.CPACode.Length > 4) ? item.CPACode.Substring(0, 4) : item.CPACode);
			}
			else if (item.ItemType == InvoiceRowTypeSE.Services.Value)
			{
				cPACode = Settings.ParametersService.CPACodeForServices();
			}
			taxInvoice.Items.Add(new TaxDocument.SpecificationRow
			{
				ItemID = item.ItemID.Value,
				ItemCode = itemCode,
				ItemName = itemName,
				CPACode = cPACode,
				UnitBrief = item.UnitBrief,
				Quantity = item.Quantity.Value.RoundAs(3),
				Price = price,
				Sum = num8,
				Discount = discount2,
				VAT = num5.Value,
				DealType = item.DealType,
				SumVAT = num6.Value,
				EnvironmentalFee = 0m,
				TotalSum = totalSum,
				RowId = item.RowID.Value,
				RowISN = Guid.NewGuid()
			});
		}
		num2 = (invoice.SummVAT * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base).RoundAs(2);
		num = (invoice.Summ.Value * invoice.ExchangeRate.Rate / invoice.ExchangeRate.Base).RoundAs(2);
		taxInvoice.TotalVAT = num2;
		taxInvoice.TotalSumWithoutTaxes = num - num2;
		taxInvoice.TotalSum = num;
		taxInvoice.TotalEnvironmentalFee = number.RoundAs(2);
	}

	private void FillProductsList(ref TaxDocument taxInvoice, MTMoveWithConfirmationBase invoice, bool exportCaption, string priceType, bool storageExpIsRetail, bool storageIncIsRetail)
	{
		string itemCode = string.Empty;
		string itemName = string.Empty;
		decimal number = 0m;
		MaterialCacheRow materialCacheRow = null;
		List<MTMoveBase.MTMoveSpecificationRow> mTList = invoice.MTList;
		if (mTList.Count == 0)
		{
			return;
		}
		using (List<MTMoveBase.MTMoveSpecificationRow>.Enumerator enumerator = mTList.GetEnumerator())
		{
			decimal price;
			decimal num3;
			MTMoveBase.MTMoveSpecificationRow current;
			decimal num;
			decimal num2;
			for (; enumerator.MoveNext(); price = num.RoundAs(2), num3 = num2.RoundAs(2), taxInvoice.Items.Add(new TaxDocument.SpecificationRow
			{
				ItemID = current.MTID,
				RowId = current.RowID,
				ItemCode = itemCode,
				ItemName = itemName,
				CPACode = ((current.CPACode.Length > 4) ? current.CPACode.Substring(0, 4) : current.CPACode),
				UnitBrief = current.UnitBrief,
				Quantity = current.Quantity.RoundAs(3),
				Price = price,
				Sum = num3,
				Discount = 0m,
				VAT = 0m,
				EnvironmentalFee = 0m,
				TotalSum = num3,
				RowISN = Guid.NewGuid()
			}), number += num2)
			{
				current = enumerator.Current;
				materialCacheRow = TradeSettings.TradeCacheService.Material.GetItemById(current.MTID);
				if (materialCacheRow != null)
				{
					itemName = ((!exportCaption && !string.IsNullOrWhiteSpace(materialCacheRow.FullCaption)) ? materialCacheRow.FullCaption : materialCacheRow.Caption);
					itemCode = materialCacheRow.MTCode;
				}
				if (string.IsNullOrWhiteSpace(priceType))
				{
					num = 0m;
					num2 = 0m;
				}
				else
				{
					num = TradeSettings.CreateTradeClient().CommonServices.GetPrice(priceType, current.MTID, current.UnitCode, invoice.DocDate).Price;
					num2 = current.Quantity.RoundAs(2) * num;
				}
				if (storageExpIsRetail | storageIncIsRetail)
				{
					num = current.Price.Value;
					num2 = current.Summa.Value;
					continue;
				}
				if (!string.IsNullOrWhiteSpace(priceType))
				{
					switch (priceType)
					{
					case "S2":
					case "S3":
						break;
					case "S1":
						goto IL_0162;
					default:
						goto IL_01c8;
					}
				}
				num = 0m;
				num2 = 0m;
				continue;
				IL_01c8:
				num = TradeSettings.CreateTradeClient().CommonServices.GetPrice(priceType, current.MTID, current.UnitCode, invoice.DocDate).Price;
				num2 = current.Quantity * num;
				continue;
				IL_0162:
				decimal selfCostsSummByDocMTAndRow = TradeSettings.CreateTradeClient().CommonServices.GetSelfCostsSummByDocMTAndRow(invoice.ISN, current.MTID, current.RowID, isMTMoveDoc: true);
				if (current.Quantity != 0m)
				{
					num = (selfCostsSummByDocMTAndRow / current.Quantity).RoundAs(2);
					num2 = selfCostsSummByDocMTAndRow;
				}
				else
				{
					num = 0m;
					num2 = 0m;
				}
			}
		}
		taxInvoice.TotalSumWithoutTaxes = number.RoundAs(2);
		taxInvoice.TotalSum = taxInvoice.TotalSumWithoutTaxes;
	}

	private void ExportMTMoveDocsToTaxService(ref EnumerateErrorsReportViewer errorRepViewer, List<MTMoveWithConfirmationBase> mtMoves, List<MTBill> mtBills, ref List<AccDocTransportation> accDocTransportations, ref int exportedDocsCount)
	{
		bool flag = false;
		bool flag2 = false;
		foreach (MTMoveWithConfirmationBase mtMove in mtMoves)
		{
			try
			{
				if (Settings.ParametersService.CPACheckMode() != CheckModeSE.DontCheck && mtMove.MTList.FirstOrDefault((MTMoveBase.MTMoveSpecificationRow p) => string.IsNullOrWhiteSpace(p.CPACode)) != null)
				{
					if (!(Settings.ParametersService.CPACheckMode() == CheckModeSE.Warning.Value))
					{
						throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(mtMove, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
					}
					if (!flag)
					{
						if (ArmSoft.SME.Core.UI.MessageBox.Show($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}: {ArmSoft.SME.Common.Resources.AskContinue}", ArmSoft.SME.Common.Resources.ExportToTaxProgram, MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
						{
							flag = true;
							flag2 = false;
							throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(mtMove, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
						}
						flag = true;
						flag2 = true;
					}
					else if (!flag2)
					{
						throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(mtMove, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
					}
				}
				TaxDocument doc = CreateTaxDocument(mtMove);
				accDocTransportations.Add(TaxServiceHelper.CreateTransportationAccDoc(doc));
				exportedDocsCount++;
			}
			catch (Exception ex)
			{
				errorRepViewer.AddError(ex, mtMove.ISN);
			}
		}
		foreach (MTBill mtBill in mtBills)
		{
			try
			{
				if (Settings.ParametersService.CPACheckMode() != CheckModeSE.DontCheck && mtBill.MTList.FirstOrDefault((MTInvoiceBase.MTInvoiceSpecificationRow p) => string.IsNullOrWhiteSpace(p.CPACode)) != null)
				{
					if (!(Settings.ParametersService.CPACheckMode() == CheckModeSE.Warning.Value))
					{
						throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(mtBill, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
					}
					if (!flag)
					{
						if (ArmSoft.SME.Core.UI.MessageBox.Show($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}: {ArmSoft.SME.Common.Resources.AskContinue}", ArmSoft.SME.Common.Resources.ExportToTaxProgram, MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
						{
							flag = true;
							flag2 = false;
							throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(mtBill, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
						}
						flag = true;
						flag2 = true;
					}
					else if (!flag2)
					{
						throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ClassifierCheck}{Environment.NewLine}{ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(mtBill, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum)}");
					}
				}
				TaxDocument doc2 = CreateTaxDocument(mtBill, isSelectedExportTypeGood: false);
				accDocTransportations.Add(TaxServiceHelper.CreateTransportationAccDoc(doc2));
				exportedDocsCount++;
			}
			catch (Exception ex2)
			{
				errorRepViewer.AddError(ex2, mtBill.ISN);
			}
		}
	}

	public decimal CalcVATAmountByType(decimal summa, string vatType, int precision = 2)
	{
		decimal num = Settings.ParametersService.VAT();
		decimal num2 = Settings.ParametersService.VATIncluded();
		switch (vatType)
		{
		case "1":
			return (summa * num / 100m).RoundAs(precision);
		case "2":
			return (summa * num / (100m + num)).RoundAs(precision);
		case "5":
			if (Settings.Localization == Localizations.Armenia)
			{
				return (summa * num2 / 100m).RoundAs(precision);
			}
			return (summa * num / (100m + num)).RoundAs(precision);
		default:
			return 0m;
		}
	}

	protected override void SetCacheRow(ExtendableRow row)
	{
		DocumentsJournalDataProvider.DataRow dataRow = row as DocumentsJournalDataProvider.DataRow;
		dataRow.PartnerCacheRow = TradeSettings.TradeCacheService.Partner.GetItemById(dataRow.PartID);
	}

	public override void ExportToXML(string fileName, DocumentsJournalDataProvider.DataRow row)
	{
		List<DocumentsJournalDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<DocumentsJournalDataProvider.DataRow>();
		List<DocumentExchangeRow> list = new List<DocumentExchangeRow>();
		foreach (DocumentsJournalDataProvider.DataRow item in selectedOrFocusedRows)
		{
			DefinitionMappingsService.TryResolveDocumentDefinition(item.DocType, out var definition);
			if (definition.IsExchangeable)
			{
				list.Add(new DocumentExchangeRow
				{
					DocType = item.DocType,
					ISN = item.ISN
				});
			}
			if (item.DocType != (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome && item.DocType != (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail && item.DocType != (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeImport)
			{
				continue;
			}
			foreach (Guid basedOvExpenseISN in TradeSettings.CreateTradeClient().CommonServices.GetBasedOvExpenseISNList(item.ISN))
			{
				list.Add(new DocumentExchangeRow
				{
					DocType = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTOverheadExp,
					ISN = basedOvExpenseISN
				});
			}
		}
		list = list.DistinctBy((DocumentExchangeRow x) => new { x.DocType, x.ISN }).ToList();
		byte[] array = Settings.CreateApiClient().Exchange.ExportDocument(list);
		using FileStream fileStream = new FileStream(fileName, FileMode.Create);
		fileStream.Write(array);
	}
}
