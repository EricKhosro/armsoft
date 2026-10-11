using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Documents.Definition;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.PersistableObject;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.Documents;

public abstract class BankInOutWindow<T> : TradeDocumentWindow<T> where T : BankInOutBase, new()
{
	private DropDownControl _operType;

	private DropDownControl _secondSettlAcc;

	private DropDownControl _accountCode;

	private DropDownCommentControl _baseDocumentControl;

	private DropDownControl _currCode;

	private ExchangeRateControl _currExChngRate;

	private PopupCalcEditExt _summa;

	private PopupCalcEditExt _summaNCC;

	private PartnerContractGroupControls _partnerContractContainer;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	private Guid _baseISN = Guid.Empty;

	private TradeDocument _baseDocument;

	private bool _isAskQuestionConfirmSummChange = true;

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonPartnerOperationJournal;

	private ReportBarButtonItem _barButtonBankOperationJournal;

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private TradeDocument BaseDocument
	{
		get
		{
			return _baseDocument;
		}
		set
		{
			_baseDocument = value;
			if (_baseDocument == null)
			{
				_baseDocumentControl.Text = "";
			}
			else
			{
				_baseDocumentControl.Text = DocumentHelpers.GetDocumentDescription(_baseDocument, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum);
			}
		}
	}

	private Guid BaseISN
	{
		get
		{
			return _baseISN;
		}
		set
		{
			if (((IComparable)value).CompareTo((object?)_baseISN) == 0)
			{
				return;
			}
			_baseISN = value;
			if (_baseISN == Guid.Empty)
			{
				BaseDocument = null;
				return;
			}
			BaseDocument = (TradeDocument)Settings.DocumentService.Load(_baseISN, withoutAccessCheck: true);
			if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.FillUsingBaseQ, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
			{
				CreateBasedOnDoc();
			}
		}
	}

	public BankInOutWindow()
	{
	}

	public BankInOutWindow(T doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		_operType = AddSmartEnumDropDownControl(null, Definition.Fields["OperType"]);
		_operType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(OperType_EditValueChanged, this).Handler;
		_accountCode = AddDropDownControl(null, Definition.Fields["AccountInBankCode"], new AccountInBanks(isKeySettlAccount: false), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_accountCode.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(AccountCode_EditValueChanged, this).Handler;
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_date.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Date_EditValueChanged, this).Handler;
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		LayoutGroupExt layoutGroupExt = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Vertical);
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Stretch;
		DocumentsJournal idv = new DocumentsJournal(this, ReportTypesSE.AllDocument, ((TradeDocDefinition)Definition).BaseDocTypes)
		{
			DocType = ((TradeDocDefinition)Definition).BaseDocTypes.First().ToString().PadLeft(2, '0'),
			DocState = DocumentStateSE.DraftOrProcessed.ValueUI
		};
		_baseDocumentControl = AddDropDownCommentControl(layoutGroupExt, ArmSoft.SME.Common.Resources.BaseDoc, isRequired: false, idv, 50, HorizontalAlignment.Stretch, "BaseDocumentControl");
		_baseDocumentControl.ShowClearButton = true;
		_baseDocumentControl.ShowSearchButton = true;
		_baseDocumentControl.IsTextEditable = false;
		_baseDocumentControl.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(BaseDocumentControl_ClickDropDown, this).Handler;
		_baseDocumentControl.EditKeyValueChanged += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentControl_EditKeyValueChanged, this).Handler;
		_baseDocumentControl.BtnClearClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentDelete_Click, this).Handler;
		_baseDocumentControl.BtnSearchClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentSearch_Click, this).Handler;
		_currCode = AddDropDownControl(layoutGroupExt, Definition.Fields["CurrCode"], new Currency(shortMode: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_currCode.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CurrCode_EditValueChanged, this).Handler;
		_currCode.IsTabStop = false;
		_currCode.IsEnabled = false;
		_summa = AddNumericControl(layoutGroupExt, Definition.Fields["Summ"]);
		_summa.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Summa_EditValueChanged, this).Handler;
		_currExChngRate = AddExchangeRateControl(layoutGroupExt, Definition.Fields["ExchangeRate"]);
		_currExChngRate.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CurrExChngRate_EditValueChanged, this).Handler;
		_summaNCC = AddNumericControl(layoutGroupExt, Definition.Fields["SummNCC"]);
		_summaNCC.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(SummaNCC_EditValueChanged, this).Handler;
		LayoutGroupExt layoutGroupExt2 = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Vertical);
		layoutGroupExt2.HorizontalAlignment = HorizontalAlignment.Stretch;
		_partnerContractContainer = AddPartnerContractGroupControls(layoutGroupExt2, (PartnerContractGroupDefinition)Definition.Fields["PartnerContractGroup"], _date, _currCode, _summa, _summaNCC);
		_secondSettlAcc = AddDropDownControl(layoutGroupExt2, Definition.Fields["SecondAcc"], new AccountInBanks(isKeySettlAccount: false), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_secondSettlAcc.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(SecondSettlAcc_EditValueChanged, this).Handler;
		LayoutGroupExt layoutGroupExt3 = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Vertical);
		layoutGroupExt3.HorizontalAlignment = HorizontalAlignment.Stretch;
		List<string> captionsList = TradeSettings.TradeCacheService.StandardComments.GetCaptionsList();
		_comment = AddComboBoxControl(layoutGroupExt3, Definition.Fields["Comment"], captionsList, HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(layoutGroupExt3, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
	}

	private void OperType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		OperType_EditValueChanged();
	}

	private void OperType_EditValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(_operType.Code))
		{
			if (_operType.Code == "1")
			{
				_partnerContractContainer.SetVisibility(Visibility.Visible);
				((LayoutItemWithRequired)(object)_secondSettlAcc.Parent).Visibility = Visibility.Collapsed;
				_secondSettlAcc.IsRequired = false;
				_secondSettlAcc.Code = "";
			}
			else if (_operType.Code == "2")
			{
				_partnerContractContainer.Visibility = Visibility.Collapsed;
				_partnerContractContainer.SetVisibility(Visibility.Collapsed);
				_partnerContractContainer.PartnerCode.Code = "";
				((LayoutItemWithRequired)(object)_secondSettlAcc.Parent).Visibility = Visibility.Visible;
				_secondSettlAcc.IsRequired = true;
			}
		}
	}

	private void SecondSettlAcc_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (string.IsNullOrWhiteSpace(_secondSettlAcc.Code))
		{
			return;
		}
		AccountInBankCacheRow accountInBankByCode = TradeSettings.TradeCacheService.AccountInBank.GetAccountInBankByCode(_secondSettlAcc.Code);
		if (accountInBankByCode != null)
		{
			if (_accountCode.Code == e.NewValue.ToString())
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.SameAccounts, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				_secondSettlAcc.Code = "";
			}
			AccountInBankCacheRow accountInBankCacheRow = null;
			if (!string.IsNullOrWhiteSpace(_accountCode.Code))
			{
				accountInBankCacheRow = TradeSettings.TradeCacheService.AccountInBank.GetAccountInBankByCode(_accountCode.Code);
			}
			if (accountInBankCacheRow != null && accountInBankCacheRow.Curr != accountInBankByCode.Curr)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.DifferentCurrAccounts, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				_secondSettlAcc.Code = "";
			}
		}
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			_currCode.IsReadOnly = true;
			_currExChngRate.IsReadOnly = _currCode.Code == "AMD";
			_summaNCC.IsReadOnly = !string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.Code == "AMD";
		}
	}

	private void SummaNCC_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		SummaNCC_EditValueChanged();
	}

	private void SummaNCC_EditValueChanged()
	{
		if (_summa.Value != 0m && !(_currExChngRate.Base == 0m))
		{
			_currExChngRate.Rate = (_summaNCC.Value / _summa.Value * _currExChngRate.Base).RoundAs(4);
			_currExChngRate.DoValidate();
		}
		ChangeSummaNCC();
	}

	private void Summa_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_currExChngRate.DoValidate())
		{
			if (string.IsNullOrWhiteSpace(_currCode.Code) || _currCode.Code == "AMD" || !_currCode.DoValidate())
			{
				_currExChngRate.Rate = 1m;
				_currExChngRate.Base = 1m;
				_summaNCC.EditValue = _summa.EditValue;
			}
			else if (_currExChngRate.DoValidate() && !(_currExChngRate.Base == 0m))
			{
				_summaNCC.EditValue = (_summa.Value * _currExChngRate.Rate / _currExChngRate.Base).RoundAs(2);
			}
		}
		ChangeSumma();
	}

	private void CurrExChngRate_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_currExChngRate.DoValidate() && _summa.Value != 0m && !(_currExChngRate.Base == 0m))
		{
			_summaNCC.EditValue = (_summa.Value * _currExChngRate.Rate / _currExChngRate.Base).RoundAs(2);
		}
	}

	private void CurrCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		decimal value = _summaNCC.Value;
		bool flag = false;
		if (_isAskQuestionConfirmSummChange && !(_summa.Value == 0m) && !string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.DoValidate() && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ConfirmSummChange, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
		{
			flag = true;
		}
		ChangedCurrCodeOrDate();
		if (flag && !(_currExChngRate.Rate == 0m))
		{
			_summa.EditValue = (value * _currExChngRate.Base / _currExChngRate.Rate).RoundAs(2);
		}
	}

	private void Date_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	protected override void OnNew()
	{
		base.OnNew();
		if (_date.EditValue == ArmSoft.SME.Common.Constants.Common.DefaultDate || _viewMode == WindowViewMode.Copy)
		{
			_date.EditValue = Settings.HostingEnvironment.CurrentDate;
			ChangedCurrCodeOrDate();
		}
		else
		{
			SummaNCC_EditValueChanged();
		}
		if (_viewMode == WindowViewMode.NewCreate)
		{
			_operType.Code = "1";
			if (string.IsNullOrWhiteSpace(_accountCode.Code))
			{
				_isAskQuestionConfirmSummChange = false;
				_accountCode.Code = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount(isSettlAccount: false);
				_isAskQuestionConfirmSummChange = true;
				if (BaseDocument != null && _currCode.Code != BaseDocument.CurrCode)
				{
					_accountCode.Code = string.Empty;
				}
			}
		}
		_owner.Code = Settings.LoginService.UserName;
		_setFocusOnActivation = _date;
	}

	protected override bool AllowNewAfterSave()
	{
		return PO.IBKey == null;
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_baseDocumentControl.KeyValue = Guid.Empty;
		_summa.EditValue = 0m;
		_summaNCC.EditValue = 0m;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
		if (_operType.Code == "1")
		{
			_partnerContractContainer.CalculatePartDebtRem();
		}
		_partnerContractContainer.CalculatePartDebtRem();
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		OperType_EditValueChanged();
		_baseISN = PO.BaseISN;
		if (_baseISN == Guid.Empty)
		{
			BaseDocument = null;
		}
		else
		{
			BaseDocument = (TradeDocument)Settings.DocumentService.Load(_baseISN, withoutAccessCheck: true);
		}
		_owner.Code = PO.Owner;
		if (!(_baseISN == Guid.Empty) && !PO.StoredInDb && _viewMode != WindowViewMode.Copy)
		{
			CreateBasedOnDoc();
		}
		_partnerContractContainer.ValueChangedForLoadDoc();
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString().PadLeft(2, '0'));
		span[1] = new PropertyWithValue("StartDate", PO.DocDate.ToString());
		span[2] = new PropertyWithValue("EndDate", PO.DocDate.ToString());
		span[3] = new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString());
		barButtonJumpTo.PropertyWithValues = list;
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.BaseISN = BaseISN;
	}

	private void AccountCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		string code = string.Empty;
		if (!string.IsNullOrWhiteSpace(_accountCode.Code))
		{
			AccountInBankCacheRow accountInBankByCode = TradeSettings.TradeCacheService.AccountInBank.GetAccountInBankByCode(_accountCode.Code);
			if (accountInBankByCode != null)
			{
				code = accountInBankByCode.Curr;
				if (PO.POType == 4 && _secondSettlAcc != null && _secondSettlAcc.Code == e.NewValue.ToString())
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.SameAccounts, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
					_accountCode.Code = "";
				}
				AccountInBankCacheRow accountInBankCacheRow = null;
				if (_secondSettlAcc != null && !string.IsNullOrWhiteSpace(_secondSettlAcc.Code))
				{
					accountInBankCacheRow = TradeSettings.TradeCacheService.AccountInBank.GetAccountInBankByCode(_secondSettlAcc.Code);
				}
				if (accountInBankCacheRow != null && accountInBankCacheRow.Curr != accountInBankByCode.Curr)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.DifferentCurrAccounts, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
					_accountCode.Code = "";
				}
			}
		}
		if (Settings.Localization == Localizations.Armenia)
		{
			_currCode.Code = code;
		}
		_currCode.DoValidate();
	}

	private void ChangedCurrCodeOrDate()
	{
		if (string.IsNullOrWhiteSpace(_currCode.Code) || _currCode.Code == "AMD" || !_currCode.DoValidate())
		{
			_currExChngRate.IsReadOnly = true;
			_currExChngRate.Rate = 1m;
			_currExChngRate.Base = 1m;
			_summaNCC.EditValue = _summa.EditValue;
			_summaNCC.IsReadOnly = true;
		}
		else
		{
			try
			{
				if (!_date.DoValidate())
				{
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
				}
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				_currExChngRate.Rate = exchangeRates.Rate;
				_currExChngRate.Base = exchangeRates.Base;
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
			_summaNCC.IsReadOnly = false;
			_currExChngRate.IsReadOnly = false;
		}
		_currExChngRate.DoValidate();
	}

	private void ChangeSumma()
	{
		_summa.DoValidate();
		if (_operType.Code == "1")
		{
			_partnerContractContainer.CalculatePartSumma();
		}
	}

	private void ChangeSummaNCC()
	{
		_summaNCC.DoValidate();
		if (_operType.Code == "1")
		{
			_partnerContractContainer.CalculatePartSumma();
		}
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonPartnerOperationJournal.IsEnabled = isEnabled;
		_barButtonBankOperationJournal.IsEnabled = isEnabled;
	}

	protected override void CreateChildDocument(short childDocType, T baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		CreateBasedOnDocHelper.CreateDocument((DocumentTypeSE)(SmartEnum<DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
	}

	private void BaseDocumentControl_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		DocumentsJournal documentsJournal = (DocumentsJournal)e.DataView;
		documentsJournal.DialogTop = e.Top;
		documentsJournal.DialogLeft = e.Left;
		if (!e.DataView.ShowDialog())
		{
			e.Cancel = true;
		}
	}

	private void BaseDocumentControl_EditKeyValueChanged(object sender, EditKeyValueChangedEventArgs e)
	{
		BaseISN = ((e.KeyValue != null) ? ((Guid)e.KeyValue) : Guid.Empty);
	}

	private void BaseDocumentDelete_Click(object sender, EditKeyValueChangedEventArgs e)
	{
		_baseDocumentControl.KeyValue = Guid.Empty;
		_baseDocumentControl.DoValidate();
	}

	private void BaseDocumentSearch_Click(object sender, EditKeyValueChangedEventArgs e)
	{
		if (BaseISN != Guid.Empty)
		{
			Settings.DocumentService.ShowWindow(BaseISN, WindowViewMode.Edit);
		}
	}

	private void CreateBasedOnDoc()
	{
		_comment.EditValue = BaseDocument.Comment;
		_isAskQuestionConfirmSummChange = false;
		if (BaseDocument.POType != (short)DocumentTypeSE.MTBill)
		{
			if (BaseDocument.POType == 1)
			{
				PayOrder payOrder = (PayOrder)BaseDocument;
				_accountCode.Code = payOrder.AccountInBankCode;
				_comment.EditValue = payOrder.PaymentAim;
			}
			else if (BaseDocument.POType == 4 || BaseDocument.POType == 5)
			{
				if (PO.POType == 4)
				{
					if (BaseDocument.POType == 5)
					{
						BankOut bankOut = (BankOut)BaseDocument;
						_accountCode.Code = bankOut.AccountInBankCode;
					}
				}
				else if (BaseDocument.POType == 4)
				{
					BankIn bankIn = (BankIn)BaseDocument;
					_accountCode.Code = bankIn.AccountInBankCode;
				}
			}
			else if (BaseDocument.POType == 8)
			{
				MTInvoice mTInvoice = (MTInvoice)BaseDocument;
				if (!string.IsNullOrWhiteSpace(mTInvoice.SupplierSettlAcc))
				{
					AccountInBankCacheRow accountInBankBySettlementAccount = TradeSettings.TradeCacheService.AccountInBank.GetAccountInBankBySettlementAccount(mTInvoice.SupplierSettlAcc);
					if (accountInBankBySettlementAccount != null)
					{
						_accountCode.Code = accountInBankBySettlementAccount.Code;
					}
				}
			}
			_isAskQuestionConfirmSummChange = true;
			if (BaseDocument.POType != 29)
			{
				_currCode.Code = BaseDocument.CurrCode;
				_currExChngRate.Rate = BaseDocument.ExchangeRate.Rate;
				_currExChngRate.Base = BaseDocument.ExchangeRate.Base;
			}
			_summaNCC.EditValue = BaseDocument.SummNCC;
			_summa.EditValue = BaseDocument.Summ;
			if (_operType.Code == "1")
			{
				_partnerContractContainer.PartnerCode.Code = BaseDocument.PartnerContractGroup.PartnerCode;
				_partnerContractContainer.Contract.Code = BaseDocument.PartnerContractGroup.ContractCode;
			}
		}
		else
		{
			MTBill mTBill = (MTBill)BaseDocument;
			_isAskQuestionConfirmSummChange = true;
			_currCode.Code = "AMD";
			_currExChngRate.Rate = BaseDocument.ExchangeRate.Rate;
			_currExChngRate.Base = BaseDocument.ExchangeRate.Base;
			if (!(BaseDocument.ExchangeRate.Base == 0m))
			{
				_summa.EditValue = (mTBill.SummaCredit * (decimal?)BaseDocument.ExchangeRate.Rate / (decimal?)BaseDocument.ExchangeRate.Base).Value.RoundAs(2);
			}
			else
			{
				_summa.EditValue = mTBill.SummaCredit;
			}
			_summaNCC.EditValue = _summa.EditValue;
			_partnerContractContainer.PartnerCode.Code = mTBill.PartnerContractGroup.PartnerCode;
			_partnerContractContainer.Contract.Code = mTBill.PartnerContractGroup.ContractCode;
		}
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_report_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barButtonPartnerOperationJournal = new ReportBarButtonItem(DataProviderTypeSE.PartOpJournal.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
		_barButtonPartnerOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonBankOperationJournal = new ReportBarButtonItem(DataProviderTypeSE.BankOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.BankOp);
		_barButtonBankOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
	}

	protected override void SetReportBarButtonsProperties()
	{
		base.SetReportBarButtonsProperties();
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString().PadLeft(2, '0'));
		span[1] = new PropertyWithValue("StartDate", PO.DocDate.ToString());
		span[2] = new PropertyWithValue("EndDate", PO.DocDate.ToString());
		span[3] = new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString());
		barButtonJumpTo.PropertyWithValues = list;
		ReportBarButtonItem barButtonPartnerOperationJournal = _barButtonPartnerOperationJournal;
		num = 3;
		List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<PropertyWithValue> span2 = CollectionsMarshal.AsSpan(list2);
		span2[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span2[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		span2[2] = new PropertyWithValue("ShowLinks", true.ToString());
		barButtonPartnerOperationJournal.PropertyWithValues = list2;
		ReportBarButtonItem barButtonBankOperationJournal = _barButtonBankOperationJournal;
		num = 2;
		List<PropertyWithValue> list3 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<PropertyWithValue> span3 = CollectionsMarshal.AsSpan(list3);
		span3[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span3[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonBankOperationJournal.PropertyWithValues = list3;
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		if (dvp.IdvName() == "InternetBank_Transactions")
		{
			if (PO.IBKey == null)
			{
				PO.IBKey = Settings.CreateApiClient().InternetBank.GetIBTransactionKey(PO.ISN);
			}
			dvp.Update(PO.IBKey);
		}
		else
		{
			base.UpdateDataViewPanel(dvp);
		}
	}

	protected override Control ControlByName(string name)
	{
		return name switch
		{
			"Date" => (Control)_date, 
			"DocNum" => _docNum, 
			"AccountCode" => _accountCode, 
			"CurrCode" => _currCode, 
			"Summ" => _summa, 
			"SummNCC" => _summaNCC, 
			"PartnerCode" => _partnerContractContainer.PartnerCode, 
			"Contract" => _partnerContractContainer.Contract, 
			"SummaPart" => _partnerContractContainer.SummaPart, 
			"Comment" => _comment, 
			_ => null, 
		};
	}
}
