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
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.HelpClasses;
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

[DocumentWindow(1)]
public class PayOrderWindow : TradeDocumentWindow<PayOrder>
{
	private DropDownCommentControl _baseDocumentControl;

	private DropDownControl _docOwner;

	private TextEditExt _payerName;

	private TextEditExt _payerTaxCode;

	private TextEditExt _payerBankCaption;

	private DropDownControl _payerSettlAcc;

	private DropDownControl _currCode;

	private ExchangeRateControl _currExChngRate;

	private PopupCalcEditExt _summa;

	private PopupCalcEditExt _summaNCC;

	private InfoMessageControl _summaValueSpellInfo;

	private InfoMessageControl _summaNCCValueSpellInfo;

	private PartnerContractGroupControls _partnerContractContainer;

	private TextEditExt _receiverName;

	private TextEditExt _receiverTaxCode;

	private DropDownControl _receiverLegalStatus;

	private TextEditExt _receiverBankCaption;

	private DropDownControl _receiverSettlAcc;

	private ComboBoxEditExt _paymentAim;

	private PartnerAccountInBank _partnerAccountInBank;

	private string _currCodeUnitCaption = " ";

	private string _currCodeCentCaption = " ";

	private string _docAccountInBankCode = "";

	private Guid _baseISN = Guid.Empty;

	private TradeDocument _baseDocument;

	private bool _isAskQuestionConfirmSummChange = true;

	private ReportBarButtonItem _barButtonJumpTo;

	protected override double WidthValue => 790.0;

	protected override double HeightValue => 630.0;

	protected override double MinWidthValue => 590.0;

	protected override double MinHeightValue => 280.0;

	public TradeDocument BaseDocument
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

	public List<object> ImpTransIDUpdateList { get; set; }

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

	public PayOrderWindow()
	{
	}

	public PayOrderWindow(PayOrder doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
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
			DocType = ((TradeDocDefinition)Definition).BaseDocTypes.First().ToString(),
			DocState = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.DraftOrProcessed.ValueUI
		};
		_baseDocumentControl = AddDropDownCommentControl(layoutGroupExt, ArmSoft.SME.Common.Resources.BaseDoc, isRequired: false, idv, 50, HorizontalAlignment.Stretch, "BaseDocumentControl");
		_baseDocumentControl.ShowClearButton = true;
		_baseDocumentControl.ShowSearchButton = true;
		_baseDocumentControl.IsTextEditable = false;
		_baseDocumentControl.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(BaseDocumentControl_ClickDropDown, this).Handler;
		_baseDocumentControl.EditKeyValueChanged += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentControl_EditKeyValueChanged, this).Handler;
		_baseDocumentControl.BtnClearClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentDelete_Click, this).Handler;
		_baseDocumentControl.BtnSearchClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentSearch_Click, this).Handler;
		_payerName = AddStringControl(layoutGroupExt, Definition.Fields["PayerName"], HorizontalAlignment.Stretch);
		LayoutGroupExt layoutGroupExt2 = AddLayoutGroup(layoutGroupExt, LayoutGroupView.Group, Orientation.Horizontal);
		layoutGroupExt2.VerticalAlignment = VerticalAlignment.Top;
		layoutGroupExt2.HorizontalAlignment = HorizontalAlignment.Stretch;
		_payerSettlAcc = AddDropDownControl(layoutGroupExt2, Definition.Fields["PayerSettlAcc"], new AccountInBanks(isKeySettlAccount: true), allowAdditionButtons: true);
		_payerSettlAcc.ShowComment = false;
		_payerSettlAcc.CheckValue = false;
		_payerSettlAcc.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PayerSettlAcc_EditValueChanged, this).Handler;
		_payerBankCaption = AddStringControl(layoutGroupExt2, ArmSoft.SME.Common.Resources.Bank, isRequired: false, 255, HorizontalAlignment.Stretch, "PayerBankCaption");
		_payerBankCaption.IsTabStop = false;
		_payerTaxCode = AddStringControl(layoutGroupExt, Definition.Fields["PayerTaxCode"], HorizontalAlignment.Stretch);
		_currCode = AddDropDownControl(layoutGroupExt, Definition.Fields["CurrCode"], new Currency(shortMode: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_currCode.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CurrCode_EditValueChanged, this).Handler;
		_currExChngRate = AddExchangeRateControl(layoutGroupExt, Definition.Fields["ExchangeRate"]);
		_currExChngRate.EditValueChanged += new SafeDocumentEventHandler<RateEditValueChangedEventArgs>(CurrExChngRate_EditValueChanged, this).Handler;
		LayoutGroupExt layoutGroupExt3 = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Horizontal, ArmSoft.SME.Common.Resources.Summa);
		layoutGroupExt3.HorizontalAlignment = HorizontalAlignment.Stretch;
		_summa = AddNumericControl(layoutGroupExt3, Definition.Fields["Summ"]);
		_summa.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Summa_EditValueChanged, this).Handler;
		_summaValueSpellInfo = AddInfoMessageControl(layoutGroupExt3, "", "copy_16.svg", HorizontalAlignment.Left, VerticalAlignment.Center);
		_summaValueSpellInfo.MessageTitle = ArmSoft.SME.Common.Resources.SummaInWords;
		_summaNCC = AddNumericControl(layoutGroupExt3, Definition.Fields["SummNCC"]);
		_summaNCC.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(SummaNCC_EditValueChanged, this).Handler;
		_summaNCCValueSpellInfo = AddInfoMessageControl(layoutGroupExt3, "", "copy_16.svg", HorizontalAlignment.Left, VerticalAlignment.Center);
		_summaNCCValueSpellInfo.MessageTitle = ArmSoft.SME.Common.Resources.SummaInWords;
		LayoutGroupExt layoutGroupExt4 = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Vertical);
		layoutGroupExt4.HorizontalAlignment = HorizontalAlignment.Stretch;
		_partnerContractContainer = AddPartnerContractGroupControls(layoutGroupExt4, (PartnerContractGroupDefinition)Definition.Fields["PartnerContractGroup"], _date, _currCode, _summa, _summaNCC);
		_partnerContractContainer.PartnerCodeEditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PartnerCodeEditValueChanged, this).Handler;
		_partnerContractContainer.PartnerCodeEditAdditionalValueChanged += new SafeDocumentEventHandler<EventArgs>(PartnerCode_EditAdditionalValueChanged, this).Handler;
		_receiverName = AddStringControl(layoutGroupExt4, Definition.Fields["ReceiverName"], HorizontalAlignment.Stretch);
		LayoutGroupExt layoutGroupExt5 = AddLayoutGroup(layoutGroupExt4, LayoutGroupView.Group, Orientation.Horizontal);
		layoutGroupExt5.VerticalAlignment = VerticalAlignment.Top;
		layoutGroupExt5.HorizontalAlignment = HorizontalAlignment.Stretch;
		_partnerAccountInBank = new PartnerAccountInBank();
		_receiverSettlAcc = AddDropDownControl(layoutGroupExt5, Definition.Fields["ReceiverSettlAcc"], _partnerAccountInBank);
		_receiverSettlAcc.ShowComment = false;
		_receiverSettlAcc.CheckValue = false;
		_receiverSettlAcc.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(ReceiverSettlAcc_EditValueChanged, this)
		{
			HandleOnLoadingDocumentToControls = true
		}.Handler;
		_receiverBankCaption = AddStringControl(layoutGroupExt5, ArmSoft.SME.Common.Resources.Bank, isRequired: false, 255, HorizontalAlignment.Stretch, "ReceiverBankCaption");
		_receiverBankCaption.IsTabStop = false;
		_receiverTaxCode = AddStringControl(layoutGroupExt4, Definition.Fields["ReceiverTaxCode"], HorizontalAlignment.Stretch);
		_receiverTaxCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ReceiverTaxCode_EditValueChanged).Handler;
		_receiverLegalStatus = AddSmartEnumDropDownControl(layoutGroupExt4, Definition.Fields["ReceiverLegalStatus"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		LayoutGroupExt layoutGroupExt6 = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Vertical);
		layoutGroupExt6.HorizontalAlignment = HorizontalAlignment.Stretch;
		_paymentAim = AddComboBoxControl(layoutGroupExt6, Definition.Fields["PaymentAim"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_docOwner = AddDropDownControl(layoutGroupExt6, Definition.Fields["Owner"], new UsersFilter());
		_docOwner.CodeWidth = 12;
		_docOwner.ShowComment = false;
		_docOwner.InnerCodeEdit.Width = _docOwner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_docOwner.Parent).Visibility = Visibility.Collapsed;
		}
		_payerSettlAcc.InnerCodeEdit.MaskType = MaskType.RegEx;
		_payerSettlAcc.InnerCodeEdit.Mask = "\\d{1," + _payerSettlAcc.DataView.ValueMemberLength() + "}";
		_payerSettlAcc.InnerCodeEdit.MaskShowPlaceHolders = false;
		_receiverSettlAcc.InnerCodeEdit.MaskType = MaskType.RegEx;
		_receiverSettlAcc.InnerCodeEdit.Mask = "\\d{1," + _receiverSettlAcc.DataView.ValueMemberLength() + "}";
		_receiverSettlAcc.InnerCodeEdit.MaskShowPlaceHolders = false;
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_docOwner.Code = Settings.LoginService.UserName;
		if (_viewMode != WindowViewMode.Copy)
		{
			_payerName.EditValue = Settings.ParametersService.CompanyName();
			_payerTaxCode.EditValue = Settings.ParametersService.TaxCode();
		}
		if (string.IsNullOrWhiteSpace(_payerSettlAcc.Code))
		{
			_isAskQuestionConfirmSummChange = false;
			_payerSettlAcc.Code = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount(isSettlAccount: true);
			_isAskQuestionConfirmSummChange = true;
			if (BaseDocument != null && _currCode.Code != BaseDocument.CurrCode)
			{
				_payerSettlAcc.Code = string.Empty;
			}
		}
	}

	protected override bool AllowNewAfterSave()
	{
		return ImpTransIDUpdateList == null;
	}

	protected override void OnNewAfterSave()
	{
		PO = new PayOrder();
		base.OnNewAfterSave();
		_summa.EditValue = 0m;
		_summaNCC.EditValue = 0m;
		_docOwner.Code = Settings.LoginService.UserName;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
		_partnerContractContainer.CalculatePartDebtRem();
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_baseISN = PO.BaseISN;
		if (_baseISN == Guid.Empty)
		{
			BaseDocument = null;
		}
		else
		{
			BaseDocument = (TradeDocument)Settings.DocumentService.Load(_baseISN, withoutAccessCheck: true);
		}
		if (!(_baseISN == Guid.Empty) && !PO.StoredInDb && _viewMode != WindowViewMode.Copy)
		{
			CreateBasedOnDoc();
		}
		if (!string.IsNullOrWhiteSpace(_partnerContractContainer.PartnerCode.Code))
		{
			_partnerContractContainer.ValueChangedForLoadDoc();
		}
		else
		{
			_receiverSettlAcc.DataView = new PartnerAccountInBank();
		}
		ChangeBankCaption(_payerSettlAcc, _payerBankCaption);
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
		PO.AccountInBankCode = _docAccountInBankCode;
	}

	private void PayerSettlAcc_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		ChangeBankCaption(_payerSettlAcc, _payerBankCaption);
		string code = "";
		if (!string.IsNullOrWhiteSpace(_payerSettlAcc.Code))
		{
			AccountInBankCacheRow accountInBankBySettlementAccount = TradeSettings.TradeCacheService.AccountInBank.GetAccountInBankBySettlementAccount(_payerSettlAcc.Code);
			if (accountInBankBySettlementAccount != null)
			{
				_docAccountInBankCode = accountInBankBySettlementAccount.Code;
				code = accountInBankBySettlementAccount.Curr;
				_payerBankCaption.Text = Settings.CoreCacheService.Bank.GetBankCaptionBySettlementAccount(_payerSettlAcc.Code, out var _);
			}
			else
			{
				_docAccountInBankCode = "";
			}
		}
		_currCode.Code = code;
		_currCode.DoValidate();
		SetNextDocumentNumber();
	}

	protected override string NextDocNumberRelatedInfo()
	{
		return _payerSettlAcc.Code;
	}

	private void ReceiverSettlAcc_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		ChangeBankCaption(_receiverSettlAcc, _receiverBankCaption);
	}

	private static void ChangeBankCaption(DropDownControl settlAccControl, TextEditExt bankControl)
	{
		bankControl.EditValue = "";
		if (!string.IsNullOrWhiteSpace(settlAccControl.Code))
		{
			string bankCaptionBySettlementAccount = Settings.CoreCacheService.Bank.GetBankCaptionBySettlementAccount(settlAccControl.Code, out var _);
			if (!string.IsNullOrWhiteSpace(bankCaptionBySettlementAccount))
			{
				bankControl.EditValue = bankCaptionBySettlementAccount;
			}
		}
		bankControl.DoValidate();
	}

	private void CurrCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		decimal value = _summaNCC.Value;
		bool flag = false;
		if (_isAskQuestionConfirmSummChange && _summa.Value != 0m && !string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.DoValidate() && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ConfirmSummChange, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
		{
			flag = true;
		}
		ChangedCurrCodeOrDate();
		if (flag && _currExChngRate.Rate != 0m)
		{
			_summa.EditValue = (value * _currExChngRate.Base / _currExChngRate.Rate).RoundAs(2);
		}
	}

	private void ChangedCurrCodeOrDate()
	{
		_currCodeUnitCaption = " ";
		_currCodeCentCaption = " ";
		if (string.IsNullOrWhiteSpace(_currCode.Code) || _currCode.Code == "AMD" || !_currCode.DoValidate())
		{
			_currExChngRate.IsReadOnly = true;
			_currExChngRate.Rate = 1m;
			_currExChngRate.Base = 1m;
			_currCodeUnitCaption = ArmSoft.SME.Trade.Common.Resources.Dram;
			_currCodeCentCaption = ArmSoft.SME.Trade.Common.Resources.Luma;
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
			CurrenciesCacheRow item = Settings.CoreCacheService.Currency.GetItem(_currCode.Code);
			if (item != null)
			{
				_currCodeUnitCaption = item.UnitName;
				_currCodeCentCaption = item.CentName;
			}
		}
		_currExChngRate.DoValidate();
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			_payerBankCaption.IsReadOnly = true;
			_currCode.IsReadOnly = true;
			_receiverBankCaption.IsReadOnly = true;
			_currExChngRate.IsReadOnly = _currCode.Code == "AMD";
			_summaNCC.IsReadOnly = !string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.Code == "AMD";
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

	private void Summa_EditValueChanged(object sender, RoutedEventArgs e)
	{
		if (_currExChngRate.DoValidate())
		{
			if (string.IsNullOrWhiteSpace(_currCode.Code) || _currCode.Code == "AMD" || !_currCode.DoValidate())
			{
				_currExChngRate.Rate = 1m;
				_currExChngRate.Base = 1m;
				_currCodeUnitCaption = ArmSoft.SME.Trade.Common.Resources.Dram;
				_currCodeCentCaption = ArmSoft.SME.Trade.Common.Resources.Luma;
				_summaNCC.EditValue = _summa.EditValue;
			}
			else if (_currExChngRate.DoValidate() && _currExChngRate.Base != 0m)
			{
				_summaNCC.EditValue = (_summa.Value * _currExChngRate.Rate / _currExChngRate.Base).RoundAs(2);
			}
		}
		ChangeSumma();
	}

	private void SummaNCC_EditValueChanged(object sender, RoutedEventArgs e)
	{
		if (_summa.Value != 0m && _currExChngRate.Base != 0m)
		{
			_currExChngRate.Rate = ((decimal)_summaNCC.EditValue / _summa.Value * _currExChngRate.Base).RoundAs(4);
			_currExChngRate.DoValidate();
		}
		ChangeSummaNCC();
	}

	private void CurrExChngRate_EditValueChanged(object sender, RateEditValueChangedEventArgs e)
	{
		if (_currExChngRate.DoValidate() && _summa.Value != 0m && _currExChngRate.Base != 0m)
		{
			_summaNCC.EditValue = (_summa.Value * _currExChngRate.Rate / _currExChngRate.Base).RoundAs(2);
		}
	}

	private void ChangeSumma()
	{
		_summaValueSpellInfo.Message = (_summa.DoValidate() ? ((decimal)_summa.EditValue).STOC(_currCodeUnitCaption, _currCodeCentCaption, toUpperFirstChar: false, isPrintMode: false) : "");
		_summa.DoValidate();
		_partnerContractContainer.CalculatePartSumma();
	}

	private void ChangeSummaNCC()
	{
		_summaNCCValueSpellInfo.Message = (_summaNCC.DoValidate() ? ((decimal)_summaNCC.EditValue).STOC(ArmSoft.SME.Trade.Common.Resources.Dram, ArmSoft.SME.Trade.Common.Resources.Luma, toUpperFirstChar: false, isPrintMode: false) : "");
		_summaNCC.DoValidate();
		_partnerContractContainer.CalculatePartSumma();
	}

	protected override void CreateChildDocument(short childDocType, PayOrder baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		if (childDocType == 2 || (uint)(childDocType - 4) <= 1u)
		{
			CreateBasedOnDocHelper.CreateDocument((ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE)(SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
		}
	}

	private void PartnerCodeEditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		PartnerCodeEditValueChanged();
		ReceiverTaxCodeEditValueChanged();
	}

	private void PartnerCode_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		PartnerCodeEditValueChanged();
	}

	private void ReceiverTaxCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		ReceiverTaxCodeEditValueChanged();
	}

	private void PartnerCodeEditValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(_partnerContractContainer.PartnerCode.Code) && _partnerContractContainer.PartnerCode.DoValidate())
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partnerContractContainer.PartnerCode.Code);
			_receiverName.EditValue = item.Caption;
			_receiverName.DoValidate();
			_receiverTaxCode.EditValue = item.TaxCode;
			_receiverTaxCode.DoValidate();
			_paymentAim.EditValue = item.MainAIM;
			_paymentAim.DoValidate();
			_receiverSettlAcc.DataView = new PartnerAccountInBank(item.PartCode);
			_receiverSettlAcc.Code = item.SettlAcc;
		}
		else
		{
			_receiverSettlAcc.DataView = _partnerAccountInBank;
			_receiverName.EditValue = string.Empty;
			_receiverTaxCode.EditValue = string.Empty;
		}
	}

	private void ReceiverTaxCodeEditValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(_receiverTaxCode.Text))
		{
			_receiverLegalStatus.Code = ReceiverLegalStatusTypeSE.LegalEntity.Value;
		}
		else
		{
			_receiverLegalStatus.Code = ReceiverLegalStatusTypeSE.FizEntity.Value;
		}
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
		_summa.EditValue = BaseDocument.Summ;
		_summaNCC.EditValue = (_summa.Value * _currExChngRate.Rate / _currExChngRate.Base).RoundAs(2);
		_partnerContractContainer.PartnerCode.Code = BaseDocument.PartnerContractGroup.PartnerCode;
		_partnerContractContainer.Contract.Code = BaseDocument.PartnerContractGroup.ContractCode;
		PartnerCodeEditValueChanged();
		_paymentAim.EditValue = BaseDocument.Comment;
	}

	protected override bool AllowPrint()
	{
		return true;
	}

	protected override bool AllowPrintOnNew()
	{
		return true;
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		BarSubItem barSubItem = new BarSubItem();
		_barButtonJumpTo = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref barSubItem);
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
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
	}

	protected override bool BeforeStore(StoreRequest sr)
	{
		if (sr.State != (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft)
		{
			string text = _receiverSettlAcc.Code.Replace("-", "");
			string settlementAccountKey = SettlementAccountHelper.GetSettlementAccountKey(text);
			bool flag = text.TrimEnd() != settlementAccountKey.TrimEnd();
			if (flag)
			{
				if (SettlementAccountHelper.Is_HSBC_Account(text))
				{
					string eating6And7SSymbolsHSBCAccount = SettlementAccountHelper.GetEating6And7SSymbolsHSBCAccount(text);
					settlementAccountKey = SettlementAccountHelper.GetSettlementAccountKey(eating6And7SSymbolsHSBCAccount);
					flag = eating6And7SSymbolsHSBCAccount.TrimEnd() != settlementAccountKey.TrimEnd();
				}
				if (flag && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateInvalidRecAccKey, Caption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
				{
					ControlsHelper.SetFocus(_receiverSettlAcc);
					return false;
				}
			}
		}
		return true;
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		if (ImpTransIDUpdateList != null)
		{
			foreach (object impTransIDUpdate in ImpTransIDUpdateList)
			{
				dvp.Update(impTransIDUpdate);
			}
			return;
		}
		base.UpdateDataViewPanel(dvp);
	}
}
