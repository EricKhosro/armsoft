using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ardalis.SmartEnum;
using ArmSoft.ECRConnector;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
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
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.Models;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.Documents;

[DocumentWindow(2)]
public class CashInWindow : TradeDocumentWindow<CashIn>
{
	private DropDownControl _opType;

	private DropDownControl _cash;

	private TextEditExt _cashierShift;

	private TextEditExt _ecrCheckNum;

	private Button _printECRCheck;

	private LayoutGroup _paymentSystemLayout;

	private LayoutGroup _plasticCardLayout;

	private LayoutGroup _posLayout;

	private TextEditExt _nonCashPaymentId;

	private ImageEdit _paymentStatusImage;

	private bool _paymentStatus;

	private IEnumerable<POSTerminalHelpers.POSAcquiringInfoAsListItem> _posTerminals;

	private PopupMenu _posTerminalsList;

	private DropDownCommentControl _baseDocumentControl;

	private DropDownControl _docOwner;

	private DropDownControl _currCode;

	private ExchangeRateControl _currExChngRate;

	private PopupCalcEditExt _summa;

	private PopupCalcEditExt _summaNCC;

	private InfoMessageControl _summaValueSpellInfo;

	private InfoMessageControl _summaNCCValueSpellInfo;

	private PartnerContractGroupControls _partnerContractContainer;

	private DropDownControl _cashOutDesk;

	private DropDownControl _employee;

	private DropDownControl _accountInBank;

	private TextEditExt _accountInBankCurr;

	private ECR _ecr;

	private PartnerContractGroupControls _bankAcquiringContainer;

	private ComboBoxEdit _nonCashPaymentMethods;

	private ComboBoxEdit _posTerminal;

	private DropDownButton _cashDeskPOSTerminals;

	private CheckEditExt _printPartnerTinOnCheck;

	private TextEditExt _payerName;

	private ComboBoxEditExt _payBase;

	private ComboBoxEditExt _payAppendix;

	private ComboBoxEditExt _comment;

	private TextEditExt _chiefaccountant;

	private TextEditExt _cashier;

	private string _currCodeUnitCaption = " ";

	private string _currCodeCentCaption = " ";

	private Guid _baseISN = Guid.Empty;

	private TradeDocument _baseDocument;

	private bool _isAskQuestionConfirmSummChange = true;

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonPartnerOperationJournal;

	private ReportBarButtonItem _barButtonCashOperationJournal;

	private static readonly string[] _printableOperations = new string[3]
	{
		CashInOpTypesSE.FromPartner.Value,
		CashInOpTypesSE.ByPlCard.Value,
		CashInOpTypesSE.RevenueInput.Value
	};

	protected override double WidthValue => 790.0;

	protected override double HeightValue => 630.0;

	protected override double MinWidthValue => 590.0;

	protected override double MinHeightValue => 280.0;

	private bool PaymentAccepted
	{
		get
		{
			return _paymentStatus;
		}
		set
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			_paymentStatus = value;
			if (value)
			{
				_paymentStatusImage.Source = ImageService.GetImage("check_20.svg");
			}
			else
			{
				_paymentStatusImage.Source = ImageService.GetImage("check_disable_20.svg");
			}
		}
	}

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

	public CashInWindow()
	{
	}

	public CashInWindow(CashIn doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		_opType = AddSmartEnumDropDownControl(null, Definition.Fields["OpType"]);
		_opType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(OpType_EditValueChanged, this).Handler;
		_cash = AddDropDownControl(null, Definition.Fields["CashDesk"], new Cashes(withoutCheckAccess: true));
		_cash.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CashDesk_EditValueChanged, this).Handler;
		_cashierShift = AddStringControl(null, Definition.Fields["CashierShiftNumber"]);
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_date.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Date_EditValueChanged, this).Handler;
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_ecrCheckNum = AddStringControl(container, Definition.Fields["ECRCheckNum"]);
		_printECRCheck = AddButton(container, "", $"{ArmSoft.SME.Trade.Common.Resources.PrintECRPrepayment} (F3)", "", $"{ArmSoft.SME.Common.Resources.ECRCheck} (F3)");
		_printECRCheck.Click += new SafeDocumentEventHandler<RoutedEventArgs>(PrintECRCheck_Click, this).Handler;
		LayoutGroupExt layoutGroupExt = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Vertical);
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Stretch;
		DocumentsJournal idv = new DocumentsJournal(this, ReportTypesSE.AllDocument, ((TradeDocDefinition)Definition).BaseDocTypes)
		{
			DocType = ((TradeDocDefinition)Definition).BaseDocTypes.First().ToString().PadLeft(2, '0'),
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
		_currCode = AddDropDownControl(layoutGroupExt, Definition.Fields["CurrCode"], new Currency(shortMode: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_currCode.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CurrCode_EditValueChanged, this).Handler;
		_currExChngRate = AddExchangeRateControl(layoutGroupExt, Definition.Fields["ExchangeRate"]);
		_currExChngRate.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CurrExChngRate_EditValueChanged, this).Handler;
		LayoutGroupExt layoutGroupExt2 = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Horizontal, ArmSoft.SME.Common.Resources.Summa);
		layoutGroupExt2.HorizontalAlignment = HorizontalAlignment.Stretch;
		_summa = AddNumericControl(layoutGroupExt2, Definition.Fields["Summ"]);
		_summa.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Summa_EditValueChanged, this).Handler;
		_summaValueSpellInfo = AddInfoMessageControl(layoutGroupExt2, "", "copy_16.svg", HorizontalAlignment.Left, VerticalAlignment.Center);
		_summaValueSpellInfo.MessageTitle = ArmSoft.SME.Common.Resources.SummaInWords;
		_summaNCC = AddNumericControl(layoutGroupExt2, Definition.Fields["SummNCC"]);
		_summaNCC.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(SummaNCC_EditValueChanged, this).Handler;
		_summaNCCValueSpellInfo = AddInfoMessageControl(layoutGroupExt2, "", "copy_16.svg", HorizontalAlignment.Left, VerticalAlignment.Center);
		_summaNCCValueSpellInfo.MessageTitle = ArmSoft.SME.Common.Resources.SummaInWords;
		LayoutGroupExt layoutGroupExt3 = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Vertical);
		layoutGroupExt3.HorizontalAlignment = HorizontalAlignment.Stretch;
		_partnerContractContainer = AddPartnerContractGroupControls(layoutGroupExt3, (PartnerContractGroupDefinition)Definition.Fields["PartnerContractGroup"], _date, _currCode, _summa, _summaNCC);
		_partnerContractContainer.PartnerCodeEditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PartnerCodeEditValueChanged, this).Handler;
		_partnerContractContainer.PartnerCodeEditAdditionalValueChanged += new SafeDocumentEventHandler<EventArgs>(PartnerCode_EditAdditionalValueChanged, this).Handler;
		_printPartnerTinOnCheck = AddBooleanControl(layoutGroupExt3, Definition.Fields["PrintPartnerTinOnCheck"]);
		_cashOutDesk = AddDropDownControl(layoutGroupExt3, Definition.Fields["CashOutDesk"], new Cashes(withoutCheckAccess: true));
		_cashOutDesk.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CashOutDesk_EditValueChanged, this).Handler;
		_employee = AddDropDownControl(layoutGroupExt3, Definition.Fields["Employee"], new Employees());
		_employee.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Employee_EditValueChanged, this).Handler;
		_accountInBank = AddDropDownControl(layoutGroupExt3, Definition.Fields["AccountInBankCode"], new AccountInBanks(isKeySettlAccount: false));
		_accountInBank.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(AccountInBank_EditValueChanged, this).Handler;
		_accountInBankCurr = AddStringControl(layoutGroupExt3, ArmSoft.SME.Common.Resources.Curr, isRequired: false, 3, HorizontalAlignment.Left, "AccountInBankCurr");
		_plasticCardLayout = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.PlasticCartPaymentDialog);
		_plasticCardLayout.HorizontalAlignment = HorizontalAlignment.Stretch;
		_plasticCardLayout.IsCollapsible = true;
		_bankAcquiringContainer = AddPartnerContractGroupControls(_plasticCardLayout, (PartnerContractGroupDefinition)Definition.Fields["BankAcquiringPartnerGroup"], _date, _currCode, _summa, _summaNCC, isShowContractCurrCode: false, isShowSummaDebt: false);
		((LayoutItemWithRequired)(object)_bankAcquiringContainer.PartnerGroup.Parent).Visibility = Visibility.Collapsed;
		_nonCashPaymentMethods = AddComboBoxControl(_plasticCardLayout, ArmSoft.SME.Trade.Common.Resources.PlasticCart, 6, isRequired: false, null, HorizontalAlignment.Left, "NonCashPaymentMethods");
		_nonCashPaymentMethods.ItemTemplate = (DataTemplate)Application.Current.Resources["PlasticCardTemplate"];
		_nonCashPaymentMethods.ItemsSource = NonCashPaymentHelpers.GetNonCashPayments();
		_nonCashPaymentMethods.IsTextEditable = false;
		_nonCashPaymentMethods.ApplyItemTemplateToSelectedItem = true;
		_nonCashPaymentMethods.Width = 197.0;
		_nonCashPaymentMethods.Height = 20.0;
		_posLayout = AddLayoutGroup(_plasticCardLayout, LayoutGroupView.Group, Orientation.Horizontal);
		_posLayout.HorizontalAlignment = HorizontalAlignment.Stretch;
		_posTerminal = AddComboBoxControl(_posLayout, ArmSoft.SME.Trade.Common.Resources.POSTerminal, 15, isRequired: false, null, HorizontalAlignment.Left, "POSTerminal");
		_posTerminal.ItemsSource = (from x in SmartEnum<POSTerminalTypesSE, string>.List
			orderby x.Value
			select x.Caption).ToList();
		_cashDeskPOSTerminals = AddDropDownButton(_posLayout, "fill_left_16.svg", ArmSoft.SME.Common.Resources.ButtonLoad);
		_cashDeskPOSTerminals.Width = 100.0;
		_cashDeskPOSTerminals.Margin = new Thickness(18.0, 0.0, 0.0, 0.0);
		_paymentSystemLayout = AddLayoutGroup(_plasticCardLayout, LayoutGroupView.Group, Orientation.Horizontal);
		_nonCashPaymentId = AddStringControl(_paymentSystemLayout, Definition.Fields["PaymentId"]);
		_nonCashPaymentId.Width = 223.0;
		_paymentStatusImage = AddImageControl(_paymentSystemLayout, 20.0, 20.0, VerticalAlignment.Center, HorizontalAlignment.Left, showBorder: false);
		LayoutGroupExt layoutGroupExt4 = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Vertical);
		layoutGroupExt4.HorizontalAlignment = HorizontalAlignment.Stretch;
		_payerName = AddStringControl(layoutGroupExt4, Definition.Fields["PayerName"], HorizontalAlignment.Stretch);
		List<string> captionsList = TradeSettings.TradeCacheService.StandardComments.GetCaptionsList();
		_payBase = AddComboBoxControl(layoutGroupExt4, Definition.Fields["Base"], captionsList, HorizontalAlignment.Stretch);
		_payAppendix = AddComboBoxControl(layoutGroupExt4, Definition.Fields["Appendix"], captionsList, HorizontalAlignment.Stretch);
		_comment = AddComboBoxControl(layoutGroupExt4, Definition.Fields["Comment"], captionsList, HorizontalAlignment.Stretch);
		_docOwner = AddDropDownControl(layoutGroupExt4, Definition.Fields["Owner"], new UsersFilter());
		_docOwner.CodeWidth = 12;
		_docOwner.ShowComment = false;
		_docOwner.InnerCodeEdit.Width = _docOwner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_docOwner.Parent).Visibility = Visibility.Collapsed;
		}
		LayoutGroupExt layoutGroupExt5 = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Common.Resources.Additional);
		_chiefaccountant = AddStringControl(layoutGroupExt5, ArmSoft.SME.Common.Resources.ChAccPost1, isRequired: false, 100, HorizontalAlignment.Stretch, "ChiefAccountant");
		_cashier = AddStringControl(layoutGroupExt5, ArmSoft.SME.Common.Resources.Cashier, isRequired: false, 100, HorizontalAlignment.Stretch, "Cashier");
		layoutGroupExt5.IsCollapsible = true;
		layoutGroupExt5.IsCollapsed = true;
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
		if (string.IsNullOrWhiteSpace(_cash.Code))
		{
			_cash.Code = Settings.ParametersService.DefaultCash();
			if (string.IsNullOrWhiteSpace(_cash.Code))
			{
				CashCacheRow cashCacheRow = TradeSettings.TradeCacheService.Cash.GetList().FirstOrDefault((CashCacheRow x) => x.IsDefault);
				if (cashCacheRow != null)
				{
					_cash.Code = cashCacheRow.Code;
				}
			}
			if (!POSTerminalHelpers.IsActivePOSTerminal(_cash.Code) && string.IsNullOrWhiteSpace(_nonCashPaymentId.Text))
			{
				_paymentSystemLayout.Visibility = Visibility.Collapsed;
			}
			else
			{
				_paymentSystemLayout.Visibility = Visibility.Visible;
			}
		}
		if (TradeSettings.TradeCacheService.Cash.GetItem(_cash.Code) != null)
		{
			CashCacheRow.NonCashInfo nonCashInfo = TradeSettings.TradeCacheService.Cash.GetNonCashInfoListByCashCode(_cash.Code).FirstOrDefault((CashCacheRow.NonCashInfo x) => x.Type == 5);
			if (nonCashInfo != null)
			{
				PO.POSTerminalType = nonCashInfo.Device;
				PO.NonCashPaymentMethod = nonCashInfo.ECRPaymentMethod;
				PO.BankAcquiringPartnerGroup.PartnerCode = nonCashInfo.Partner;
				PO.BankAcquiringPartnerGroup.ContractCode = nonCashInfo.Contract;
			}
		}
		_setFocusOnActivation = _date;
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			_accountInBankCurr.IsReadOnly = true;
			_printPartnerTinOnCheck.IsReadOnly = !string.IsNullOrWhiteSpace(_ecrCheckNum.Text) || _opType.Code != "4";
			_partnerContractContainer.IsReadOnly = !string.IsNullOrWhiteSpace(_ecrCheckNum.Text) && _printPartnerTinOnCheck.IsChecked.Value;
			_cashierShift.IsReadOnly = true;
			_ecrCheckNum.IsReadOnly = true;
			_nonCashPaymentId.IsReadOnly = true;
			bool flag = _opType.Code != CashInOpTypesSE.FromSettlementAcc.Value && !_printECRCheck.IsEnabled;
			_opType.IsReadOnly = flag;
			_cash.IsReadOnly = flag;
			_docNum.IsReadOnly = flag;
			_date.IsReadOnly = flag;
			_currCode.IsReadOnly = flag;
			_currExChngRate.IsReadOnly = (_currCode.Code == "AMD") | flag;
			_baseDocumentControl.IsReadOnly = flag;
			_summa.IsReadOnly = flag;
			_summaNCC.IsReadOnly = (_currCode.Code == "AMD") | flag;
			_bankAcquiringContainer.IsReadOnly = flag;
			_nonCashPaymentMethods.IsReadOnly = flag;
			_cashDeskPOSTerminals.IsEnabled = flag;
			_posTerminal.IsReadOnly = flag;
			_cashOutDesk.IsReadOnly = flag;
			_accountInBank.IsReadOnly = flag;
			_employee.IsReadOnly = flag;
			_chiefaccountant.IsReadOnly = true;
			_cashier.IsReadOnly = true;
		}
	}

	protected override bool AllowNewAfterSave()
	{
		return PO.IBKey == null;
	}

	protected override void OnNewAfterSave()
	{
		_printECRCheck.IsEnabled = _opType.Code.InList(_printableOperations);
		base.OnNewAfterSave();
		_docOwner.Code = Settings.LoginService.UserName;
		if (TradeSettings.TradeCacheService.Cash.GetItem(_cash.Code) != null)
		{
			CashCacheRow.NonCashInfo nonCashInfo = TradeSettings.TradeCacheService.Cash.GetNonCashInfoListByCashCode(_cash.Code).FirstOrDefault((CashCacheRow.NonCashInfo x) => x.Type == 5);
			if (nonCashInfo != null)
			{
				PO.POSTerminalType = nonCashInfo.Device;
				PO.NonCashPaymentMethod = nonCashInfo.ECRPaymentMethod;
				PO.BankAcquiringPartnerGroup.PartnerCode = nonCashInfo.Partner;
				PO.BankAcquiringPartnerGroup.ContractCode = nonCashInfo.Contract;
				PO.POSTerminalID = nonCashInfo.POSTerminalID;
				PO.POSTerminalAddress = nonCashInfo.POSTerminalAddress;
			}
		}
		_nonCashPaymentId.Text = "";
		PaymentAccepted = false;
		_ecrCheckNum.Text = "";
		_printPartnerTinOnCheck.IsChecked = false;
		_baseDocumentControl.KeyValue = Guid.Empty;
		_summa.EditValue = 0m;
		_summaNCC.EditValue = 0m;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
		if (_opType.Code == CashInOpTypesSE.FromPartner || _opType.Code == CashInOpTypesSE.ByPlCard)
		{
			_partnerContractContainer.CalculatePartDebtRem();
		}
	}

	protected override void OnF3()
	{
		if (_printECRCheck.IsEnabled)
		{
			try
			{
				PrintCheck();
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
		}
	}

	private void PrintECRCheck_Click(object sender, RoutedEventArgs e)
	{
		OnF3();
	}

	public override void OnMagneticCardReader(string cardData)
	{
		if (string.IsNullOrWhiteSpace(cardData) || _viewMode == WindowViewMode.View)
		{
			return;
		}
		try
		{
			if (MCRInfo.GetInstance().IsValidCard(cardData))
			{
				string cardNumber = MCRInfo.GetInstance().GetCardNumber(cardData);
				if (TradeSettings.TradeCacheService.Partner.GetContractByDiscountCard(cardNumber, out var partnerCacheRow) != null)
				{
					_partnerContractContainer.PartnerCode.Code = partnerCacheRow.PartCode;
					PartnerCodeEditValueChanged();
					return;
				}
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidCardNotExist, cardData));
			}
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.InvalidCardCode);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public override void OnScanner(string scannerData)
	{
		if (string.IsNullOrWhiteSpace(scannerData) || _viewMode == WindowViewMode.View)
		{
			return;
		}
		try
		{
			if (Settings.ParametersService.UsePartnerEmployeeSearchNewMode())
			{
				if (DocumentHelpers.IsValidPartnerEmployeeCardCode(scannerData, Settings.ParametersService.PartnerEmployeeCardCodePrefixes(), Settings.ParametersService.UsePartnerCardLenght(), Settings.ParametersService.PartnerCardLenght()))
				{
					if (TradeSettings.TradeCacheService.Partner.GetContractByDiscountCard(scannerData, out var partnerCacheRow) != null)
					{
						_partnerContractContainer.PartnerCode.Code = partnerCacheRow.PartCode;
						PartnerCodeEditValueChanged();
						return;
					}
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidCardNotExist, scannerData));
				}
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.InvalidCardCode);
			}
			if (MCRInfo.GetInstance().IsValidCard(scannerData))
			{
				string cardNumber = MCRInfo.GetInstance().GetCardNumber(scannerData);
				if (TradeSettings.TradeCacheService.Partner.GetContractByDiscountCard(cardNumber, out var partnerCacheRow2) != null)
				{
					_partnerContractContainer.PartnerCode.Code = partnerCacheRow2.PartCode;
					PartnerCodeEditValueChanged();
					return;
				}
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidCardNotExist, cardNumber));
			}
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.InvalidCardCode);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
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
		_printECRCheck.IsEnabled = string.IsNullOrWhiteSpace(PO.ECRCheckNum) && _opType.Code.InList(_printableOperations);
		if (PO.NonCashPaymentMethod.HasValue)
		{
			_nonCashPaymentMethods.SelectedItem = NonCashPaymentHelpers.GetPaymentItem(PO.NonCashPaymentMethod.Value);
		}
		_posTerminal.SelectedItem = POSTerminalHelpers.GetPOSTerminalTypeCaptionByCode(PO.POSTerminalType);
		_partnerContractContainer.ValueChangedForLoadDoc();
		_bankAcquiringContainer.ValueChangedForLoadDoc();
		if (!(_baseISN == Guid.Empty) && !PO.StoredInDb && _viewMode != WindowViewMode.Copy)
		{
			CreateBasedOnDoc();
		}
		OpType_EditValueChanged();
		if (!string.IsNullOrWhiteSpace(_accountInBank.Code))
		{
			AccountInBankCacheRow item = TradeSettings.TradeCacheService.AccountInBank.GetItem(_accountInBank.Code);
			if (item != null)
			{
				_accountInBankCurr.EditValue = item.Curr;
			}
		}
		_chiefaccountant.Text = Settings.ParametersService.ChiefAccountantName();
		_cashier.Text = Settings.ParametersService.CashierName();
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
		PO.NonCashPaymentMethod = ((NonCashPaymentHelpers.NonCashPaymentItem)_nonCashPaymentMethods.SelectedItem).PaymentType;
		PO.POSTerminalType = POSTerminalHelpers.GetPOSTerminalTypeCodeByCaption((_posTerminal.SelectedItem != null) ? _posTerminal.SelectedItem.ToString() : "");
		PO.BaseISN = _baseISN;
	}

	protected override string NextDocNumberRelatedInfo()
	{
		return _cash.Code;
	}

	private void PartnerCodeEditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		PartnerCodeEditValueChanged();
	}

	private void PartnerCode_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		PartnerCodeEditValueChanged();
	}

	private void PartnerCodeEditValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(_partnerContractContainer.PartnerCode.Code) && _partnerContractContainer.PartnerCode.DoValidate())
		{
			_payerName.EditValue = _partnerContractContainer.PartnerCode.Comment.Trim();
			_payerName.DoValidate();
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
		_currExChngRate.Rate = BaseDocument.ExchangeRate.Rate;
		_currExChngRate.Base = BaseDocument.ExchangeRate.Base;
		if (BaseDocument.POType == 18)
		{
			MTBill mTBill = (MTBill)BaseDocument;
			_isAskQuestionConfirmSummChange = false;
			_currCode.Code = "AMD";
			_isAskQuestionConfirmSummChange = true;
			if (BaseDocument.ExchangeRate.Base != 0m)
			{
				_summa.EditValue = (mTBill.SummaCredit.Value * BaseDocument.ExchangeRate.Rate / BaseDocument.ExchangeRate.Base).RoundAs(2);
			}
			else
			{
				_summa.EditValue = mTBill.SummaCredit;
			}
			_summaNCC.EditValue = _summa.EditValue;
		}
		else
		{
			_isAskQuestionConfirmSummChange = false;
			_currCode.Code = BaseDocument.CurrCode;
			_isAskQuestionConfirmSummChange = true;
			_summa.EditValue = BaseDocument.Summ;
			_summaNCC.EditValue = BaseDocument.SummNCC;
		}
		switch (BaseDocument.POType)
		{
		case 18:
			_payBase.EditValue = ArmSoft.SME.Trade.Common.Resources.DocMTBill + " N " + BaseDocument.DocNum.Trim() + ", " + ArmSoft.SME.Common.Resources.DateShort + " " + BaseDocument.DocDate.FormatDDMMYY();
			break;
		case 8:
		{
			string text = (string.IsNullOrWhiteSpace(BaseDocument.InvoiceNumber) ? (" N " + BaseDocument.DocNum.Trim() + ", ") : (" " + BaseDocument.InvoiceNumber.Trim() + ", "));
			_payBase.EditValue = ArmSoft.SME.Common.Resources.DocInvoiceShort + text + ArmSoft.SME.Common.Resources.DateShort + " " + BaseDocument.DocDate.FormatDDMMYY();
			break;
		}
		case 21:
			_payBase.EditValue = ArmSoft.SME.Common.Resources.DocInvoiceRetailShort + " N " + BaseDocument.DocNum.Trim() + ", " + ArmSoft.SME.Common.Resources.DateShort + " " + BaseDocument.DocDate.FormatDDMMYY();
			break;
		case 33:
			_payBase.EditValue = ArmSoft.SME.Common.Resources.InvoiceForPay + " N " + BaseDocument.DocNum.Trim() + ", " + ArmSoft.SME.Common.Resources.DateShort + " " + BaseDocument.DocDate.FormatDDMMYY();
			break;
		}
		if (_opType.Code == "1" || _opType.Code == "4")
		{
			_partnerContractContainer.PartnerCode.Code = BaseDocument.PartnerContractGroup.PartnerCode;
			_partnerContractContainer.Contract.Code = BaseDocument.PartnerContractGroup.ContractCode;
			_partnerContractContainer.ValueChangedForLoadDoc();
			PartnerCodeEditValueChanged();
		}
		_comment.EditValue = BaseDocument.Comment;
		if (BaseDocument.POType == 1)
		{
			PayOrder payOrder = (PayOrder)BaseDocument;
			_payerName.EditValue = payOrder.ReceiverName;
			_comment.EditValue = payOrder.PaymentAim;
		}
		else if (BaseDocument.POType == 3)
		{
			CashOut cashOut = (CashOut)BaseDocument;
			_cash.Code = cashOut.CashDesk;
			_payerName.EditValue = cashOut.ReceiverName;
			_payAppendix.EditValue = cashOut.Appendix;
			_opType.Code = "1";
		}
		else if (BaseDocument.POType == 33)
		{
			InvoiceForPay invoiceForPay = (InvoiceForPay)BaseDocument;
			_partnerContractContainer.PartnerCode.Code = invoiceForPay.PartnerContractGroup.PartnerCode;
			_currExChngRate.Rate = BaseDocument.ExchangeRate.Rate;
			_currExChngRate.Base = BaseDocument.ExchangeRate.Base;
			_partnerContractContainer.ValueChangedForLoadDoc();
		}
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
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_report_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barButtonJumpTo = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barSubItemJournals.Content = "";
		_barSubItemJournals.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemJournals.LargeGlyph = ImageService.GetImage("report_32.svg");
		_barButtonPartnerOperationJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartOpJournal.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
		_barButtonPartnerOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonCashOperationJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.CashOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.KassaOp);
		_barButtonCashOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
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
		ReportBarButtonItem barButtonCashOperationJournal = _barButtonCashOperationJournal;
		num = 2;
		List<PropertyWithValue> list3 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<PropertyWithValue> span3 = CollectionsMarshal.AsSpan(list3);
		span3[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span3[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonCashOperationJournal.PropertyWithValues = list3;
	}

	protected override void CreateChildDocument(short childDocType, CashIn baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		CreateBasedOnDocHelper.CreateDocument((ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE)(SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
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

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonPartnerOperationJournal.IsEnabled = isEnabled;
		_barButtonCashOperationJournal.IsEnabled = isEnabled;
	}

	private void OpType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		OpType_EditValueChanged();
	}

	private void OpType_EditValueChanged()
	{
		if (string.IsNullOrWhiteSpace(_opType.Code))
		{
			return;
		}
		switch (_opType.Code)
		{
		case "1":
			((LayoutItemWithRequired)(object)_printPartnerTinOnCheck.Parent).Visibility = Visibility.Visible;
			_printPartnerTinOnCheck.IsChecked = false;
			_printPartnerTinOnCheck.IsReadOnly = true;
			_partnerContractContainer.SetVisibility(Visibility.Visible);
			((LayoutItemWithRequired)(object)_cashOutDesk.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_employee.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_accountInBank.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_accountInBankCurr.Parent).Visibility = Visibility.Collapsed;
			_plasticCardLayout.Visibility = Visibility.Collapsed;
			_posLayout.Visibility = Visibility.Collapsed;
			_accountInBankCurr.EditValue = "";
			_bankAcquiringContainer.PartnerCode.IsRequired = false;
			_bankAcquiringContainer.PartnerCode.Code = "";
			PO.POSTerminalID = "";
			PO.POSTerminalAddress = "";
			_posTerminal.SelectedIndex = 0;
			_nonCashPaymentMethods.SelectedIndex = 0;
			_printECRCheck.Content = $"{ArmSoft.SME.Trade.Common.Resources.PrintECRPrepayment} (F3)";
			_printECRCheck.IsEnabled = string.IsNullOrWhiteSpace(_ecrCheckNum.Text);
			_cashOutDesk.IsRequired = false;
			_cashOutDesk.Code = "";
			_employee.Code = "";
			_accountInBank.IsRequired = false;
			_accountInBank.Code = "";
			break;
		case "2":
			((LayoutItemWithRequired)(object)_printPartnerTinOnCheck.Parent).Visibility = Visibility.Collapsed;
			_printPartnerTinOnCheck.IsChecked = false;
			_printPartnerTinOnCheck.IsReadOnly = true;
			_partnerContractContainer.SetVisibility(Visibility.Collapsed);
			((LayoutItemWithRequired)(object)_cashOutDesk.Parent).Visibility = Visibility.Visible;
			((LayoutItemWithRequired)(object)_employee.Parent).Visibility = Visibility.Visible;
			((LayoutItemWithRequired)(object)_accountInBank.Parent).Visibility = Visibility.Collapsed;
			_plasticCardLayout.Visibility = Visibility.Collapsed;
			_posLayout.Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_accountInBank.Parent).Visibility = Visibility.Collapsed;
			_accountInBankCurr.EditValue = "";
			_bankAcquiringContainer.PartnerCode.IsRequired = false;
			_bankAcquiringContainer.PartnerCode.Code = "";
			PO.POSTerminalID = "";
			PO.POSTerminalAddress = "";
			_posTerminal.SelectedIndex = 0;
			_nonCashPaymentMethods.SelectedIndex = 0;
			_printECRCheck.Content = $"{ArmSoft.SME.Trade.Common.Resources.PrintECRCashIn} (F3)";
			_printECRCheck.IsEnabled = string.IsNullOrWhiteSpace(_ecrCheckNum.Text);
			_cashOutDesk.IsRequired = true;
			_partnerContractContainer.PartnerCode.Code = "";
			_accountInBank.IsRequired = false;
			_accountInBank.Code = "";
			break;
		case "3":
			((LayoutItemWithRequired)(object)_printPartnerTinOnCheck.Parent).Visibility = Visibility.Collapsed;
			_printPartnerTinOnCheck.IsChecked = false;
			_printPartnerTinOnCheck.IsReadOnly = true;
			_partnerContractContainer.SetVisibility(Visibility.Collapsed);
			((LayoutItemWithRequired)(object)_cashOutDesk.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_employee.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_accountInBank.Parent).Visibility = Visibility.Visible;
			_plasticCardLayout.Visibility = Visibility.Collapsed;
			_posLayout.Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_accountInBankCurr.Parent).Visibility = Visibility.Visible;
			_accountInBankCurr.EditValue = "";
			_bankAcquiringContainer.PartnerCode.IsRequired = false;
			_bankAcquiringContainer.PartnerCode.Code = "";
			PO.POSTerminalID = "";
			PO.POSTerminalAddress = "";
			_posTerminal.SelectedIndex = 0;
			_nonCashPaymentMethods.SelectedIndex = 0;
			_printECRCheck.IsEnabled = false;
			_cashOutDesk.IsRequired = false;
			_cashOutDesk.Code = "";
			_employee.Code = "";
			_partnerContractContainer.PartnerCode.Code = "";
			_accountInBank.IsRequired = true;
			break;
		case "4":
			((LayoutItemWithRequired)(object)_printPartnerTinOnCheck.Parent).Visibility = Visibility.Visible;
			_partnerContractContainer.SetVisibility(Visibility.Visible);
			((LayoutItemWithRequired)(object)_cashOutDesk.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_employee.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_accountInBank.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_accountInBankCurr.Parent).Visibility = Visibility.Collapsed;
			_plasticCardLayout.Visibility = Visibility.Visible;
			_plasticCardLayout.IsCollapsed = false;
			_posLayout.Visibility = Visibility.Visible;
			if (_cashDeskPOSTerminals.PopupContent == null)
			{
				_posTerminals = POSTerminalHelpers.GetPOSInfoByCashDesk(_cash.Code);
				if (_posTerminals != null && _posTerminals.Any())
				{
					_posTerminalsList = new PopupMenu();
					foreach (POSTerminalHelpers.POSAcquiringInfoAsListItem posTerminal in _posTerminals)
					{
						BarButtonItem barButtonItem = new BarButtonItem();
						barButtonItem.Content = posTerminal;
						barButtonItem.ItemClick += CashDeskPOSTerminals_SelectionChanged;
						_posTerminalsList.Items.Add(barButtonItem);
					}
					_cashDeskPOSTerminals.IsEnabled = true;
					_cashDeskPOSTerminals.PopupContent = _posTerminalsList;
				}
				else
				{
					_cashDeskPOSTerminals.IsEnabled = false;
					_cashDeskPOSTerminals.PopupContent = null;
				}
			}
			((LayoutItemWithRequired)(object)_accountInBank.Parent).Visibility = Visibility.Collapsed;
			_accountInBankCurr.EditValue = "";
			_cashOutDesk.IsRequired = false;
			_cashOutDesk.Code = "";
			_employee.Code = "";
			_accountInBank.IsRequired = false;
			_accountInBank.Code = "";
			_nonCashPaymentMethods.SelectedItem = NonCashPaymentHelpers.GetPaymentItem(PO.NonCashPaymentMethod.Value);
			_posTerminal.SelectedItem = POSTerminalHelpers.GetPOSTerminalTypeCaptionByCode(PO.POSTerminalType);
			_bankAcquiringContainer.PartnerCode.Code = PO.BankAcquiringPartnerGroup.PartnerCode;
			_bankAcquiringContainer.Contract.Code = PO.BankAcquiringPartnerGroup.ContractCode;
			_bankAcquiringContainer.PartnerCode.IsRequired = true;
			_printECRCheck.Content = $"{ArmSoft.SME.Trade.Common.Resources.PrintECRPrepayment} (F3)";
			_printECRCheck.IsEnabled = string.IsNullOrWhiteSpace(_ecrCheckNum.Text);
			_printPartnerTinOnCheck.IsReadOnly = !string.IsNullOrWhiteSpace(_ecrCheckNum.Text);
			break;
		}
	}

	private void CashDesk_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_cash.Code) && TradeSettings.TradeCacheService.Cash.GetItem(_cash.Code) != null)
		{
			if (!TradeSettings.CreateTradeClient().CommonServices.IsCashDeskAvailable(Convert.ToString(e.NewValue)))
			{
				e.Handled = true;
				_cash.Code = Convert.ToString(e.OldValue);
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.CashDeskNotAvailForUser, Settings.LoginService.UserName, e.NewValue.ToString()), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			if (_cashOutDesk.Code == e.NewValue.ToString())
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.CashInErrSameCashDesks, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				_cash.Code = "";
			}
		}
		if (Convert.ToString(e.NewValue) != Convert.ToString(e.OldValue) && _opType.Code == CashInOpTypesSE.ByPlCard)
		{
			SetPOSTerminalInfo();
		}
		SetCashierShiftNumber();
		SetNextDocumentNumber();
	}

	private void CashOutDesk_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(e.NewValue.ToString()) && TradeSettings.TradeCacheService.Cash.GetItem(_cashOutDesk.Code) != null)
		{
			if (!TradeSettings.CreateTradeClient().CommonServices.IsCashDeskAvailable(e.NewValue.ToString()))
			{
				e.Handled = true;
				_cashOutDesk.Code = e.OldValue.ToString();
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.CashDeskNotAvailForUser, Settings.LoginService.UserName, e.NewValue.ToString()), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			else if (_cash.Code == e.NewValue.ToString())
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.CashInErrSameCashDesks, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				_cashOutDesk.Code = "";
			}
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

	private void AccountInBank_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_accountInBank.Code))
		{
			AccountInBankCacheRow item = TradeSettings.TradeCacheService.AccountInBank.GetItem(_accountInBank.Code);
			if (item != null)
			{
				if (item.Curr != _currCode.Code)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.DocCurrDifferSettAccCurr, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
					_accountInBank.Code = "";
				}
				_accountInBankCurr.EditValue = item.Curr;
			}
		}
		else
		{
			_accountInBankCurr.EditValue = "";
		}
	}

	private void Employee_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_employee.Code))
		{
			EmployeeCacheRow item = TradeSettings.TradeCacheService.Employee.GetItem(_employee.Code);
			if (item != null)
			{
				_payerName.EditValue = item.Caption;
			}
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
		SummaNCC_EditValueChanged();
	}

	private void SummaNCC_EditValueChanged()
	{
		if (_summa.Value != 0m && _currExChngRate.Base != 0m)
		{
			_currExChngRate.Rate = ((decimal)_summaNCC.EditValue / _summa.Value * _currExChngRate.Base).RoundAs(4);
			_currExChngRate.DoValidate();
		}
		ChangeSummaNCC();
	}

	private void CurrExChngRate_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_currExChngRate.DoValidate() && _summa.Value != 0m && _currExChngRate.Base != 0m)
		{
			_summaNCC.EditValue = (_summa.Value * _currExChngRate.Rate / _currExChngRate.Base).RoundAs(2);
		}
	}

	private void CashDeskPOSTerminals_SelectionChanged(object sender, ItemClickEventArgs e)
	{
		POSTerminalHelpers.POSAcquiringInfoAsListItem pOSAcquiringInfoAsListItem = (POSTerminalHelpers.POSAcquiringInfoAsListItem)((BarButtonItem)sender).Content;
		if (pOSAcquiringInfoAsListItem != null)
		{
			if (!_posTerminal.IsEnabled)
			{
				_posTerminal.IsEnabled = true;
				_nonCashPaymentMethods.SelectedIndex = 0;
			}
			_posTerminal.SelectedItem = POSTerminalHelpers.GetPOSTerminalTypeCaptionByCode(pOSAcquiringInfoAsListItem.Device);
			_bankAcquiringContainer.PartnerCode.Code = pOSAcquiringInfoAsListItem.Partner;
			_bankAcquiringContainer.Contract.Code = pOSAcquiringInfoAsListItem.Contract;
			_nonCashPaymentMethods.SelectedItem = NonCashPaymentHelpers.GetPaymentItem(pOSAcquiringInfoAsListItem.NonCashPaymentMethod);
			PO.POSTerminalID = pOSAcquiringInfoAsListItem.POSTerminalID;
			PO.POSTerminalAddress = pOSAcquiringInfoAsListItem.POSTerminalAddress;
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

	private ImageEdit AddImageControl(LayoutGroup container, double height, double width, VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment, bool showBorder)
	{
		LayoutGroupExt container2 = AddLayoutGroup(container, LayoutGroupView.Group, Orientation.Horizontal);
		return AddImageControl(container2, height, width, isTabStop: false, showBorder, showMenu: false, ShowMenuMode.Hover, ShowLoadDialogOnClickMode.Never, Stretch.Uniform, verticalAlignment, horizontalAlignment, "Image");
	}

	private void PrintCheck()
	{
		CashCacheRow cashRow = TradeSettings.TradeCacheService.Cash.GetItem(_cash.Code);
		if (!ECRPreliminaryChecksAndInitalization(cashRow))
		{
			return;
		}
		long poscrn = 0L;
		PosTerminalResponse posTerminalResponse = PayWithPOSTerminalIfChosen(ref poscrn);
		ECRPrintCheckWindow eCRPrintCheckWindow;
		if (_opType.Code == "2")
		{
			eCRPrintCheckWindow = new ECRPrintCheckWindow(this, _ecr, cashRow.ECROperator, cashRow.ECROperatorPassword, cashRow.ShowECRTracking, CreateCashInRequest(cashRow.ECROperator), ECRPrintCheckWindow.ECRCheckType.CashInOut);
		}
		else
		{
			string templateCode = PrintTemplateHelper.CombinePrintTemplateName(2, 3);
			Utilities.PrintReceiptRequest request = CreatePrePaymentRequest(posTerminalResponse);
			eCRPrintCheckWindow = new ECRPrintCheckWindow(this, _ecr, cashRow.ECROperator, cashRow.ECROperatorPassword, cashRow.ShowECRTracking, request, ECRPrintCheckWindow.ECRCheckType.PrePayment, (ECRPrintCheckWindow window, Utilities.PrintReceiptResponse response) =>
			{
				ECRHelpers.PrintOnOtherPrinter(window, response, cashRow, PO, templateCode);
			}, (string checkNumber, string errorMessage) =>
			{
				int num = 1;
				List<PropertyWithValue> list = new List<PropertyWithValue>(num);
				CollectionsMarshal.SetCount(list, num);
				CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("ECRCheckNum", checkNumber);
				List<PropertyWithValue> editedProperties = list;
				PO.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, errorMessage, POISN(), editedProperties, PO.TS);
				_ecrCheckNum.Text = checkNumber;
				PO.ECRCheckNum = checkNumber;
			});
		}
		if (eCRPrintCheckWindow.Succeeded)
		{
			UpdateDocAndWindowAfterCheckPrint(eCRPrintCheckWindow.CheckNumber, eCRPrintCheckWindow.ECRCrn);
			ConfirmPOSTerminalPaymentIfDone(posTerminalResponse);
		}
		else
		{
			CancelPOSTerminalPaymentIfDone(posTerminalResponse, poscrn);
		}
	}

	private void UpdateDocAndWindowAfterCheckPrint(string checkNumber, string ecrCRN)
	{
		_ecrCheckNum.Text = checkNumber;
		PO.ECRCheckNum = checkNumber;
		PO.ECRCRN = ecrCRN;
		int num = 2;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("ECRCheckNum", PO.ECRCheckNum);
		span[1] = new PropertyWithValue("ECRCRN", PO.ECRCRN);
		List<PropertyWithValue> editedProperties = list;
		PO.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, "", POISN(), editedProperties, PO.TS);
		LoadDataToControls();
		_printPartnerTinOnCheck.IsReadOnly = !string.IsNullOrWhiteSpace(PO.ECRCheckNum);
		IsChanged = false;
		SetControlsReadonly();
		UpdateDataViewPanel(DVPID);
	}

	private PosTerminalResponse PayWithPOSTerminalIfChosen(ref long poscrn)
	{
		if (PO.POSTerminalType == "2")
		{
			return Arcus2Helpers.CheckAndPurchase(PO.POSTerminalID, _summaNCC.Value);
		}
		if (PO.POSTerminalType == "4")
		{
			return PossoftPayClientHelper.Purchase(out poscrn, PO.POSTerminalAddress, _summaNCC.Value);
		}
		return null;
	}

	private void ConfirmPOSTerminalPaymentIfDone(PosTerminalResponse posTerminalResponse)
	{
		if (posTerminalResponse != null && !string.IsNullOrWhiteSpace(posTerminalResponse.RRN) && (!(PO.POSTerminalType != "2") || !(PO.POSTerminalType != "4")))
		{
			UpdatePaymentId(posTerminalResponse.RRN);
		}
	}

	private void CancelPOSTerminalPaymentIfDone(PosTerminalResponse posTerminalResponse, long poscrn)
	{
		if (posTerminalResponse != null && !string.IsNullOrWhiteSpace(posTerminalResponse.RRN))
		{
			if (PO.POSTerminalType == "2")
			{
				Arcus2Helpers.Cancel(PO.POSTerminalID, _summaNCC.Value, posTerminalResponse.RRN);
			}
			else if (PO.POSTerminalType == "4")
			{
				PossoftPayClientHelper.Cancel(poscrn, PO.POSTerminalAddress);
			}
		}
	}

	private void UpdatePaymentId(string paymentId)
	{
		try
		{
			string text = (SmartEnum<POSTerminalTypesSE, string>.TryFromValue(PO.POSTerminalType, out var result) ? (result.CaptionShort + ": ") : "");
			_nonCashPaymentId.Text = text + paymentId;
			PaymentAccepted = true;
			PO.PaymentId = _nonCashPaymentId.Text;
			PO.PaymentAccepted = true;
			int num = 2;
			List<PropertyWithValue> list = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list, num);
			Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
			span[0] = new PropertyWithValue("PaymentId", PO.PaymentId);
			span[1] = new PropertyWithValue("PaymentAccepted", PO.PaymentAccepted.ToString());
			List<PropertyWithValue> properties = list;
			StoreRequest sr = new StoreRequest
			{
				IsDataChanged = true,
				LogComment = _nonCashPaymentId.Text,
				WithoutGrids = true,
				WithoutValidation = true,
				WithoutAccessCheck = true,
				RegenerateEntries = false
			};
			PO.TS = Settings.DocumentService.EditFields(PO.ISN, properties, sr);
			LoadDataToControls();
			IsChanged = false;
			UpdateDataViewPanel(DVPID);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private Utilities.PrintCashInOutRequest CreateCashInRequest(string ecrOperator)
	{
		return new Utilities.PrintCashInOutRequest
		{
			amount = (double)_summaNCC.Value,
			isCashIn = true,
			cashierId = int.Parse(ecrOperator),
			description = _comment.Text
		};
	}

	private Utilities.PrintReceiptRequest CreatePrePaymentRequest(PosTerminalResponse posTerminalResponse)
	{
		decimal d = 0m;
		decimal d2 = 0m;
		if (_opType.Code == "4")
		{
			d2 = _summaNCC.Value;
		}
		else
		{
			d = _summaNCC.Value;
		}
		int? paymentSystem = null;
		if (PO.POSTerminalType == POSTerminalTypesSE.ECRPOS.Value)
		{
			paymentSystem = NonCashPaymentHelpers.GetECRPaymentSystem((ArmSoft.SME.Common.Constants.Common.NonCashPaymentType)PO.NonCashPaymentMethod.Value);
		}
		Utilities.PrintReceiptRequest printReceiptRequest = new Utilities.PrintReceiptRequest
		{
			mode = 3,
			paidAmount = decimal.ToDouble(d),
			partialAmount = 0.0,
			prePaymentAmount = 0.0,
			paidAmountCard = decimal.ToDouble(d2),
			PaymentSystem = paymentSystem,
			useExtPOS = (PO.POSTerminalType != POSTerminalTypesSE.ECRPOS.Value),
			partnerTin = GetPartnerTin()
		};
		if (posTerminalResponse != null && !string.IsNullOrWhiteSpace(posTerminalResponse.RRN))
		{
			printReceiptRequest.rrn = posTerminalResponse.RRN;
			if (!string.IsNullOrWhiteSpace(posTerminalResponse.TerminalId))
			{
				printReceiptRequest.terminalId = posTerminalResponse.TerminalId;
			}
		}
		return printReceiptRequest;
	}

	private string GetPartnerTin()
	{
		if (!_printPartnerTinOnCheck.IsChecked.HasValue || !_printPartnerTinOnCheck.IsChecked.Value)
		{
			return null;
		}
		string result = "";
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partnerContractContainer.PartnerCode.Code);
		if (item != null)
		{
			result = item.TaxCode;
		}
		return result;
	}

	private bool ECRPreliminaryChecksAndInitalization(CashCacheRow cashRow)
	{
		if (_date.EditValue != TradeSettings.CreateTradeClient().CommonServices.GetServerDate().ToDateOnly())
		{
			throw new ASException(ArmSoft.SME.Common.Resources.DocDateNotCurrent);
		}
		if (cashRow == null)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.NotInKassa, PO.CashDesk.Trim()));
		}
		if (string.IsNullOrWhiteSpace(cashRow.ECROperator) || string.IsNullOrWhiteSpace(cashRow.ECROperatorPassword))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidateECROperatorOrECROperatorPin, _cash.Code), ArmSoft.SME.Common.Resources.PrintECRCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (!cashRow.ECROperator.IsNumeric())
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.PrintECRCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.MustBeSaveDocBeforePrintECRCheck, ArmSoft.SME.Common.Resources.PrintECRCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if ((!StoredInDb || IsChanged || State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value) && !SaveDocument())
		{
			return false;
		}
		if (_opType.Code == "1")
		{
			string cashSummLimitType = cashRow.CashSummLimitType;
			if (cashSummLimitType == CashLimitsSE.OnlyNonCash.Value)
			{
				if (_summaNCC.Value != 0m)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PrePayOnlyNonCash, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
					return false;
				}
			}
			else if (cashSummLimitType == CashLimitsSE.WithLimit.Value && _summa.Value > 300000m)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PrePayOnlyNonCash, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
				return false;
			}
		}
		ECRHelpers.InitECR(ref _ecr, _cash.Code);
		if (_ecr == null)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.PrintECRCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		return true;
	}

	private void SetPOSTerminalInfo()
	{
		List<CashCacheRow.NonCashInfo> nonCashInfoListByCashCode = TradeSettings.TradeCacheService.Cash.GetNonCashInfoListByCashCode(_cash.Code);
		if (nonCashInfoListByCashCode != null)
		{
			_posTerminals = POSTerminalHelpers.GetPOSInfoByCashDesk(_cash.Code);
			if (_posTerminals != null && _posTerminals.Any())
			{
				_cashDeskPOSTerminals.IsEnabled = true;
				_posTerminalsList = new PopupMenu();
				foreach (POSTerminalHelpers.POSAcquiringInfoAsListItem posTerminal in _posTerminals)
				{
					BarButtonItem barButtonItem = new BarButtonItem();
					barButtonItem.Content = posTerminal;
					barButtonItem.ItemClick += CashDeskPOSTerminals_SelectionChanged;
					_posTerminalsList.Items.Add(barButtonItem);
				}
				_cashDeskPOSTerminals.PopupContent = _posTerminalsList;
			}
			else
			{
				_cashDeskPOSTerminals.PopupContent = null;
				_cashDeskPOSTerminals.IsEnabled = false;
			}
			CashCacheRow.NonCashInfo nonCashInfo = nonCashInfoListByCashCode.FirstOrDefault((CashCacheRow.NonCashInfo x) => x.Type == 5);
			if (nonCashInfo != null)
			{
				_posTerminal.SelectedItem = POSTerminalHelpers.GetPOSTerminalTypeCaptionByCode(nonCashInfo.Device);
				_nonCashPaymentMethods.SelectedItem = NonCashPaymentHelpers.GetPaymentItem(nonCashInfo.ECRPaymentMethod);
				_bankAcquiringContainer.PartnerCode.Code = nonCashInfo.Partner;
				_bankAcquiringContainer.Contract.Code = nonCashInfo.Contract;
				PO.POSTerminalID = nonCashInfo.POSTerminalID;
				PO.POSTerminalAddress = nonCashInfo.POSTerminalAddress;
			}
			else
			{
				_posTerminal.SelectedIndex = Convert.ToInt32("0");
				_nonCashPaymentMethods.SelectedIndex = 0;
				_bankAcquiringContainer.PartnerCode.Code = "";
				_bankAcquiringContainer.Contract.Code = "";
				PO.POSTerminalID = "";
				PO.POSTerminalAddress = "";
			}
		}
		else
		{
			_cashDeskPOSTerminals.PopupContent = null;
			_cashDeskPOSTerminals.IsEnabled = false;
			_posTerminal.SelectedIndex = Convert.ToInt32("0");
			_bankAcquiringContainer.PartnerCode.Code = "";
			_bankAcquiringContainer.Contract.Code = "";
			_nonCashPaymentMethods.SelectedIndex = 0;
			PO.POSTerminalID = "";
			PO.POSTerminalAddress = "";
		}
	}

	private void SetCashierShiftNumber()
	{
		if (!string.IsNullOrWhiteSpace(_cash.Code))
		{
			_cashierShift.Text = TradeSettings.CreateTradeClient().CommonServices.GetOpenCashierShiftNumberByCashDesk(_cash.Code);
		}
		else
		{
			_cashierShift.Text = "";
		}
	}
}
