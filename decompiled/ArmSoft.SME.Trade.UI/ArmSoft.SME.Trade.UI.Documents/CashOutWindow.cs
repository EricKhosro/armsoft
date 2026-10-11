using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Ardalis.SmartEnum;
using ArmSoft.ECRConnector;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Documents;
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
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.Models;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.Documents;

[DocumentWindow(3)]
public class CashOutWindow : TradeDocumentWindow<CashOut>
{
	private DropDownControl _opType;

	private DropDownControl _cashDesk;

	private TextEditExt _cashierShift;

	private DropDownControl _currCode;

	private ExchangeRateControl _currExChngRate;

	private PopupCalcEditExt _summa;

	private PopupCalcEditExt _summaNCC;

	private InfoMessageControl _summaValueSpellInfo;

	private InfoMessageControl _summaNCCValueSpellInfo;

	private PartnerContractGroupControls _partnerContractContainer;

	private DropDownControl _cashInDesk;

	private DropDownControl _employee;

	private DropDownControl _settlAcc;

	private TextEditExt _settlAccCurrCode;

	private TextEditExt _ecrReturnCheckNum;

	private Button _printECRCheck;

	private ECR _ecr;

	private ComboBoxEditExt _posTerminalTypes;

	private DropDownButton _cashDeskPOSTerminals;

	private PartnerContractGroupControls _bankAcquiringContainer;

	private ComboBoxEditExt _nonCashPaymentMethods;

	private IEnumerable<POSTerminalHelpers.POSAcquiringInfoAsListItem> _posTerminals;

	private PopupMenu _listOfPOSTerminals;

	private LayoutGroup _lgPOS;

	private LayoutGroup _lgPlCard;

	private TextEditExt _receiverName;

	private TextEditExt _nPassport;

	private ComboBoxEditExt _base;

	private ComboBoxEditExt _appendix;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	private TextEditExt _chiefAccPost1;

	private TextEditExt _cashier;

	private string _currCodeUnitCaption = " ";

	private string _currCodeCentCaption = " ";

	private DropDownCommentControl _baseDocumentControl;

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
		CashOutOpTypesSE.ToPartner.Value,
		CashOutOpTypesSE.ByPlCard.Value,
		CashOutOpTypesSE.RevenueOutput.Value
	};

	protected override double WidthValue => 630.0;

	protected override double HeightValue => 650.0;

	protected override double MinWidthValue => 600.0;

	protected override double MinHeightValue => 250.0;

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

	public CashOutWindow()
	{
	}

	public CashOutWindow(CashOut doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		_opType = AddSmartEnumDropDownControl(null, Definition.Fields["OpType"]);
		_opType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(OpType_EditValueChanged, this).Handler;
		_cashDesk = AddDropDownControl(null, Definition.Fields["CashDesk"], new Cashes(withoutCheckAccess: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_cashDesk.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CashDesk_EditValueChanged, this).Handler;
		_cashierShift = AddStringControl(null, Definition.Fields["CashierShiftNumber"]);
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Date_EditValueChanged, this).Handler;
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_ecrReturnCheckNum = AddStringControl(container, Definition.Fields["ECRReturnCheckNum"]);
		_printECRCheck = AddButton(container, null, $"{ArmSoft.SME.Trade.Common.Resources.PrintECRPrepaymentReturn} (F3)", null, $"{ArmSoft.SME.Common.Resources.ECRCheck} (F3)", isTabStop: true);
		_printECRCheck.Click += new SafeDocumentEventHandler<RoutedEventArgs>(BE_PrintECRCheck_Click, this).Handler;
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
		_cashInDesk = AddDropDownControl(layoutGroupExt3, Definition.Fields["CashInDesk"], new Cashes(withoutCheckAccess: true));
		_cashInDesk.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CashInDesk_EditValueChanged, this).Handler;
		_employee = AddDropDownControl(layoutGroupExt3, Definition.Fields["Employee"], new Employees());
		_employee.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Employee_EditValueChanged, this).Handler;
		_settlAcc = AddDropDownControl(layoutGroupExt3, Definition.Fields["AccountInBankCode"], new AccountInBanks(isKeySettlAccount: false));
		_settlAcc.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(SettlAcc_EditValueChanged, this).Handler;
		_settlAccCurrCode = AddStringControl(layoutGroupExt3, ArmSoft.SME.Common.Resources.Curr, isRequired: false, 3, HorizontalAlignment.Left, "SettlAccCurrCode");
		_lgPlCard = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.PlasticCartPaymentDialog);
		_lgPlCard.HorizontalAlignment = HorizontalAlignment.Stretch;
		_lgPlCard.IsCollapsible = true;
		_bankAcquiringContainer = AddPartnerContractGroupControls(_lgPlCard, (PartnerContractGroupDefinition)Definition.Fields["BankAcquiringPartnerGroup"], _date, _currCode, _summa, _summaNCC, isShowContractCurrCode: false, isShowSummaDebt: false);
		((LayoutItemWithRequired)(object)_bankAcquiringContainer.PartnerGroup.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_bankAcquiringContainer.PartnerGroup.Parent).Visibility = Visibility.Collapsed;
		_nonCashPaymentMethods = AddComboBoxControl(_lgPlCard, ArmSoft.SME.Trade.Common.Resources.PlasticCart, 1, isRequired: false, null, HorizontalAlignment.Left, "NonCashPaymentMethods");
		_nonCashPaymentMethods.ItemTemplate = (DataTemplate)Application.Current.Resources["PlasticCardTemplate"];
		_nonCashPaymentMethods.ItemsSource = NonCashPaymentHelpers.GetNonCashPayments();
		_nonCashPaymentMethods.IsTextEditable = false;
		_nonCashPaymentMethods.ApplyItemTemplateToSelectedItem = true;
		_nonCashPaymentMethods.Width = 197.0;
		_nonCashPaymentMethods.Height = 20.0;
		_lgPOS = AddLayoutGroup(_lgPlCard, LayoutGroupView.Group, Orientation.Horizontal);
		_lgPOS.HorizontalAlignment = HorizontalAlignment.Stretch;
		_posTerminalTypes = AddComboBoxControl(_lgPOS, ArmSoft.SME.Trade.Common.Resources.POSTerminal, 15, isRequired: false, null, HorizontalAlignment.Left, "POSTerminal");
		_posTerminalTypes.ItemsSource = (from x in SmartEnum<POSTerminalTypesSE, string>.List
			orderby x.Value
			select x.Caption).ToList();
		_cashDeskPOSTerminals = AddDropDownButton(_lgPOS, "fill_left_16.svg", ArmSoft.SME.Common.Resources.ButtonLoad);
		_cashDeskPOSTerminals.Width = 100.0;
		_cashDeskPOSTerminals.Margin = new Thickness(18.0, 0.0, 0.0, 0.0);
		LayoutGroupExt layoutGroupExt4 = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Vertical);
		layoutGroupExt4.HorizontalAlignment = HorizontalAlignment.Stretch;
		_receiverName = AddStringControl(layoutGroupExt4, Definition.Fields["ReceiverName"], HorizontalAlignment.Stretch);
		_nPassport = AddStringControl(layoutGroupExt4, Definition.Fields["NPassport"], HorizontalAlignment.Stretch);
		List<string> captionsList = TradeSettings.TradeCacheService.StandardComments.GetCaptionsList();
		_base = AddComboBoxControl(layoutGroupExt4, Definition.Fields["Base"], captionsList, HorizontalAlignment.Stretch);
		_appendix = AddComboBoxControl(layoutGroupExt4, Definition.Fields["Appendix"], captionsList, HorizontalAlignment.Stretch);
		_comment = AddComboBoxControl(layoutGroupExt4, Definition.Fields["Comment"], captionsList, HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(layoutGroupExt4, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		LayoutGroupExt layoutGroupExt5 = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Common.Resources.Additional);
		layoutGroupExt5.IsCollapsible = true;
		layoutGroupExt5.IsCollapsed = true;
		_chiefAccPost1 = AddStringControl(layoutGroupExt5, ArmSoft.SME.Common.Resources.ChAccPost1, isRequired: false, 100, HorizontalAlignment.Stretch, "ChiefAccountant");
		_chiefAccPost1.IsTabStop = false;
		_chiefAccPost1.EditValue = Settings.ParametersService.ChiefAccountantName();
		_cashier = AddStringControl(layoutGroupExt5, ArmSoft.SME.Common.Resources.Cashier, isRequired: false, 50, HorizontalAlignment.Stretch, "Cashier");
		_cashier.IsTabStop = false;
		_cashier.EditValue = Settings.ParametersService.CashierName();
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			_ecrReturnCheckNum.IsReadOnly = true;
			_ecrReturnCheckNum.IsEnabled = false;
			_cashierShift.IsReadOnly = true;
			_settlAccCurrCode.IsReadOnly = true;
			_chiefAccPost1.IsReadOnly = true;
			_cashier.IsReadOnly = true;
			bool flag = _opType.Code != CashOutOpTypesSE.ToSettlementAcc.Value && !string.IsNullOrWhiteSpace(_ecrReturnCheckNum.Text);
			_opType.IsReadOnly = flag;
			_cashDesk.IsReadOnly = flag;
			_docNum.IsReadOnly = flag;
			_date.IsReadOnly = flag;
			_currCode.IsReadOnly = flag;
			_currExChngRate.IsReadOnly = (_currCode.Code == "AMD") | flag;
			_baseDocumentControl.IsReadOnly = flag;
			_summa.IsReadOnly = flag;
			_summaNCC.IsReadOnly = (_currCode.Code == "AMD") | flag;
			_partnerContractContainer.IsReadOnly = !string.IsNullOrWhiteSpace(_ecrReturnCheckNum.Text);
			_bankAcquiringContainer.IsReadOnly = flag;
			_nonCashPaymentMethods.IsReadOnly = flag;
			_cashDeskPOSTerminals.IsEnabled = flag;
			_posTerminalTypes.IsReadOnly = flag;
			_cashInDesk.IsReadOnly = flag;
			_settlAcc.IsReadOnly = flag;
			_employee.IsReadOnly = flag;
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
		if (string.IsNullOrWhiteSpace(_cashDesk.Code))
		{
			_cashDesk.Code = Settings.ParametersService.DefaultCash();
			if (string.IsNullOrWhiteSpace(_cashDesk.Code))
			{
				CashCacheRow cashCacheRow = TradeSettings.TradeCacheService.Cash.GetList().FirstOrDefault((CashCacheRow x) => x.IsDefault);
				if (cashCacheRow != null)
				{
					_cashDesk.Code = cashCacheRow.Code;
				}
			}
		}
		_owner.Code = Settings.LoginService.UserName;
		if (BaseDocument == null || BaseDocument.POType != DocumentTypeSE.CashIn.Value)
		{
			List<CashCacheRow.NonCashInfo> nonCashInfoListByCashCode = TradeSettings.TradeCacheService.Cash.GetNonCashInfoListByCashCode(_cashDesk.Code);
			if (nonCashInfoListByCashCode != null)
			{
				CashCacheRow.NonCashInfo nonCashInfo = nonCashInfoListByCashCode.FirstOrDefault((CashCacheRow.NonCashInfo x) => x.Type == 5);
				if (nonCashInfo != null)
				{
					PO.POSTerminalType = nonCashInfo.Device;
					PO.NonCashPaymentMethod = nonCashInfo.ECRPaymentMethod;
					PO.BankAcquiringPartnerGroup.PartnerCode = nonCashInfo.Partner;
					PO.BankAcquiringPartnerGroup.ContractCode = nonCashInfo.Contract;
					PO.POSTerminalAddress = nonCashInfo.POSTerminalAddress;
				}
			}
		}
		_printECRCheck.IsEnabled = BaseDocument != null && BaseDocument.POType == DocumentTypeSE.CashIn.Value;
		_setFocusOnActivation = _date;
	}

	protected override void OnNewAfterSave()
	{
		_printECRCheck.IsEnabled = _opType.Code.InList(_printableOperations);
		base.OnNewAfterSave();
		_ecrReturnCheckNum.Text = "";
		BaseDocument = null;
		_summa.EditValue = 0m;
		_summaNCC.EditValue = 0m;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		LoadDataToControls();
		_setFocusOnActivation = _docNum;
		if (_opType.Code == "1")
		{
			_partnerContractContainer.CalculatePartDebtRem();
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
		_printECRCheck.IsEnabled = string.IsNullOrWhiteSpace(PO.ECRReturnCheckNum) && _opType.Code.InList(_printableOperations);
		_posTerminalTypes.SelectedItem = POSTerminalHelpers.GetPOSTerminalTypeCaptionByCode(PO.POSTerminalType);
		if (PO.NonCashPaymentMethod.HasValue)
		{
			_nonCashPaymentMethods.SelectedItem = NonCashPaymentHelpers.GetPaymentItem(PO.NonCashPaymentMethod.Value);
		}
		if (!(_baseISN == Guid.Empty) && !PO.StoredInDb && _viewMode != WindowViewMode.Copy)
		{
			CreateBasedOnDoc();
			SetCashierShiftNumber();
		}
		OpType_EditValueChanged();
		if (!string.IsNullOrWhiteSpace(_settlAcc.Code))
		{
			AccountInBankCacheRow item = TradeSettings.TradeCacheService.AccountInBank.GetItem(_settlAcc.Code);
			if (item != null)
			{
				_settlAccCurrCode.EditValue = item.Curr;
			}
		}
		_partnerContractContainer.ValueChangedForLoadDoc();
		_bankAcquiringContainer.ValueChangedForLoadDoc();
		PartnerCodeEditValueChanged();
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
		PO.POSTerminalType = POSTerminalHelpers.GetPOSTerminalTypeCodeByCaption((_posTerminalTypes.SelectedItem != null) ? _posTerminalTypes.SelectedItem.ToString() : "");
		PO.BaseISN = _baseISN;
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
		if (_opType.Code == "1")
		{
			_partnerContractContainer.SetVisibility(Visibility.Visible);
			((LayoutItemWithRequired)(object)_cashInDesk.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_employee.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_settlAcc.Parent).Visibility = Visibility.Collapsed;
			_lgPlCard.Visibility = Visibility.Collapsed;
			_lgPOS.Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_settlAccCurrCode.Parent).Visibility = Visibility.Collapsed;
			_settlAccCurrCode.EditValue = "";
			_bankAcquiringContainer.PartnerCode.IsRequired = false;
			_bankAcquiringContainer.PartnerCode.Code = "";
			_posTerminalTypes.SelectedIndex = 0;
			_nonCashPaymentMethods.SelectedIndex = 0;
			_printECRCheck.Content = $"{ArmSoft.SME.Trade.Common.Resources.PrintECRPrepaymentReturn} (F3)";
			_printECRCheck.IsEnabled = string.IsNullOrWhiteSpace(_ecrReturnCheckNum.Text);
			_cashInDesk.IsRequired = false;
			_cashInDesk.Code = "";
			_employee.Code = "";
			_settlAcc.IsRequired = false;
			_settlAcc.Code = "";
			return;
		}
		if (_opType.Code == "2")
		{
			_partnerContractContainer.SetVisibility(Visibility.Collapsed);
			((LayoutItemWithRequired)(object)_cashInDesk.Parent).Visibility = Visibility.Visible;
			((LayoutItemWithRequired)(object)_employee.Parent).Visibility = Visibility.Visible;
			((LayoutItemWithRequired)(object)_settlAcc.Parent).Visibility = Visibility.Collapsed;
			_lgPlCard.Visibility = Visibility.Collapsed;
			_lgPOS.Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_settlAccCurrCode.Parent).Visibility = Visibility.Collapsed;
			_settlAccCurrCode.EditValue = "";
			_bankAcquiringContainer.PartnerCode.IsRequired = false;
			_bankAcquiringContainer.PartnerCode.Code = "";
			_posTerminalTypes.SelectedIndex = 0;
			_nonCashPaymentMethods.SelectedIndex = 0;
			_printECRCheck.Content = $"{ArmSoft.SME.Trade.Common.Resources.PrintECRCashOut} (F3)";
			_printECRCheck.IsEnabled = string.IsNullOrWhiteSpace(_ecrReturnCheckNum.Text);
			_cashInDesk.IsRequired = true;
			_partnerContractContainer.PartnerCode.Code = "";
			_settlAcc.IsRequired = false;
			_settlAcc.Code = "";
			return;
		}
		if (_opType.Code == "3")
		{
			_partnerContractContainer.SetVisibility(Visibility.Collapsed);
			((LayoutItemWithRequired)(object)_cashInDesk.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_employee.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_settlAcc.Parent).Visibility = Visibility.Visible;
			_lgPlCard.Visibility = Visibility.Collapsed;
			_lgPOS.Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_settlAccCurrCode.Parent).Visibility = Visibility.Visible;
			_settlAccCurrCode.EditValue = "";
			_bankAcquiringContainer.PartnerCode.IsRequired = false;
			_bankAcquiringContainer.PartnerCode.Code = "";
			_posTerminalTypes.SelectedIndex = 0;
			_nonCashPaymentMethods.SelectedIndex = 0;
			_printECRCheck.IsEnabled = false;
			_cashInDesk.IsRequired = false;
			_cashInDesk.Code = "";
			_employee.Code = "";
			_partnerContractContainer.PartnerCode.Code = "";
			_settlAcc.IsRequired = true;
			return;
		}
		_partnerContractContainer.SetVisibility(Visibility.Visible);
		((LayoutItemWithRequired)(object)_cashInDesk.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_employee.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_settlAcc.Parent).Visibility = Visibility.Collapsed;
		_lgPlCard.Visibility = Visibility.Visible;
		_lgPlCard.IsCollapsed = false;
		_lgPOS.Visibility = Visibility.Visible;
		if (_cashDeskPOSTerminals.PopupContent == null)
		{
			_posTerminals = POSTerminalHelpers.GetPOSInfoByCashDesk(_cashDesk.Code);
			if (_posTerminals != null && _posTerminals.Any())
			{
				_listOfPOSTerminals = new PopupMenu();
				foreach (POSTerminalHelpers.POSAcquiringInfoAsListItem posTerminal in _posTerminals)
				{
					BarButtonItem barButtonItem = new BarButtonItem();
					barButtonItem.Content = posTerminal;
					barButtonItem.ItemClick += CashDeskPOSTerminals_SelectionChanged;
					_listOfPOSTerminals.Items.Add(barButtonItem);
				}
				_cashDeskPOSTerminals.IsEnabled = true;
				_cashDeskPOSTerminals.PopupContent = _listOfPOSTerminals;
			}
			else
			{
				_cashDeskPOSTerminals.IsEnabled = false;
				_cashDeskPOSTerminals.PopupContent = null;
			}
		}
		((LayoutItemWithRequired)(object)_settlAccCurrCode.Parent).Visibility = Visibility.Collapsed;
		_settlAccCurrCode.EditValue = "";
		_cashInDesk.IsRequired = false;
		_cashInDesk.Code = "";
		_employee.Code = "";
		_settlAcc.IsRequired = false;
		_settlAcc.Code = "";
		_nonCashPaymentMethods.SelectedItem = NonCashPaymentHelpers.GetPaymentItem(PO.NonCashPaymentMethod.Value);
		_posTerminalTypes.SelectedItem = POSTerminalHelpers.GetPOSTerminalTypeCaptionByCode(PO.POSTerminalType);
		_bankAcquiringContainer.PartnerCode.Code = PO.BankAcquiringPartnerGroup.PartnerCode;
		_bankAcquiringContainer.Contract.Code = PO.BankAcquiringPartnerGroup.ContractCode;
		_bankAcquiringContainer.PartnerCode.IsRequired = true;
		_printECRCheck.Content = $"{ArmSoft.SME.Trade.Common.Resources.PrintECRPrepaymentReturn} (F3)";
		_printECRCheck.IsEnabled = string.IsNullOrWhiteSpace(_ecrReturnCheckNum.Text);
	}

	private void CashDeskPOSTerminals_SelectionChanged(object sender, ItemClickEventArgs e)
	{
		POSTerminalHelpers.POSAcquiringInfoAsListItem pOSAcquiringInfoAsListItem = (POSTerminalHelpers.POSAcquiringInfoAsListItem)((BarButtonItem)sender).Content;
		if (pOSAcquiringInfoAsListItem != null)
		{
			if (!_posTerminalTypes.IsEnabled)
			{
				_posTerminalTypes.IsEnabled = true;
				_nonCashPaymentMethods.SelectedIndex = 0;
			}
			_posTerminalTypes.SelectedIndex = Convert.ToInt32(pOSAcquiringInfoAsListItem.Device);
			_nonCashPaymentMethods.SelectedItem = NonCashPaymentHelpers.GetPaymentItem(pOSAcquiringInfoAsListItem.NonCashPaymentMethod);
			_bankAcquiringContainer.Contract.Code = pOSAcquiringInfoAsListItem.Contract;
			_bankAcquiringContainer.PartnerCode.Code = pOSAcquiringInfoAsListItem.Partner;
			PO.POSTerminalAddress = pOSAcquiringInfoAsListItem.POSTerminalAddress;
		}
	}

	private void Employee_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_employee.Code))
		{
			_receiverName.EditValue = _employee.Comment.Trim();
		}
	}

	private void SettlAcc_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_settlAcc.Code))
		{
			AccountInBankCacheRow item = TradeSettings.TradeCacheService.AccountInBank.GetItem(_settlAcc.Code);
			if (item != null)
			{
				if (item.Curr != _currCode.Code)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.DocCurrDifferSettAccCurr, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
					_settlAcc.Code = "";
				}
				_settlAccCurrCode.EditValue = item.Curr;
			}
		}
		else
		{
			_settlAccCurrCode.EditValue = string.Empty;
		}
	}

	private void CashInDesk_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(e.NewValue.ToString()) && TradeSettings.TradeCacheService.Cash.GetItem(_cashInDesk.Code) != null)
		{
			if (!TradeSettings.CreateTradeClient().CommonServices.IsCashDeskAvailable(Convert.ToString(e.NewValue)))
			{
				e.Handled = true;
				_cashInDesk.Code = Convert.ToString(e.OldValue);
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.CashDeskNotAvailForUser, Settings.LoginService.UserName, Convert.ToString(e.NewValue)), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			else if (_cashDesk.Code == e.NewValue.ToString())
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.CashInErrSameCashDesks, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				_cashInDesk.Code = "";
			}
		}
	}

	private void CashDesk_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_cashDesk.Code) && TradeSettings.TradeCacheService.Cash.GetItem(_cashDesk.Code) != null)
		{
			if (!TradeSettings.CreateTradeClient().CommonServices.IsCashDeskAvailable(Convert.ToString(e.NewValue)))
			{
				e.Handled = true;
				_cashDesk.Code = Convert.ToString(e.OldValue);
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.CashDeskNotAvailForUser, Settings.LoginService.UserName, e.NewValue.ToString()), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			if (_cashInDesk.Code == e.NewValue.ToString())
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.CashInErrSameCashDesks, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				_cashDesk.Code = "";
			}
		}
		if (Convert.ToString(e.NewValue) != Convert.ToString(e.OldValue) && _opType.Code == "4")
		{
			SetPOSTerminalInfo();
		}
		SetCashierShiftNumber();
		SetNextDocumentNumber();
	}

	protected override string NextDocNumberRelatedInfo()
	{
		return _cashDesk.Code;
	}

	private void SetPOSTerminalInfo()
	{
		List<CashCacheRow.NonCashInfo> nonCashInfoListByCashCode = TradeSettings.TradeCacheService.Cash.GetNonCashInfoListByCashCode(_cashDesk.Code);
		if (nonCashInfoListByCashCode != null)
		{
			_posTerminals = POSTerminalHelpers.GetPOSInfoByCashDesk(_cashDesk.Code);
			if (_posTerminals != null && _posTerminals.Any())
			{
				_cashDeskPOSTerminals.IsEnabled = true;
				_listOfPOSTerminals = new PopupMenu();
				foreach (POSTerminalHelpers.POSAcquiringInfoAsListItem posTerminal in _posTerminals)
				{
					BarButtonItem barButtonItem = new BarButtonItem
					{
						Content = posTerminal
					};
					barButtonItem.ItemClick += CashDeskPOSTerminals_SelectionChanged;
					_listOfPOSTerminals.Items.Add(barButtonItem);
				}
				_cashDeskPOSTerminals.PopupContent = _listOfPOSTerminals;
			}
			else
			{
				_cashDeskPOSTerminals.PopupContent = null;
				_cashDeskPOSTerminals.IsEnabled = false;
			}
			CashCacheRow.NonCashInfo nonCashInfo = nonCashInfoListByCashCode.FirstOrDefault((CashCacheRow.NonCashInfo x) => x.Type == 5);
			if (nonCashInfo != null)
			{
				_posTerminalTypes.SelectedIndex = Convert.ToInt32(nonCashInfo.Device);
				_nonCashPaymentMethods.SelectedItem = NonCashPaymentHelpers.GetPaymentItem(nonCashInfo.ECRPaymentMethod);
				_bankAcquiringContainer.PartnerCode.Code = nonCashInfo.Partner;
				_bankAcquiringContainer.Contract.Code = nonCashInfo.Contract;
				PO.POSTerminalAddress = nonCashInfo.POSTerminalAddress;
			}
			else
			{
				_posTerminalTypes.SelectedIndex = Convert.ToInt32("0");
				_nonCashPaymentMethods.SelectedIndex = 0;
				_bankAcquiringContainer.PartnerCode.Code = "";
				_bankAcquiringContainer.Contract.Code = "";
				PO.POSTerminalAddress = "";
			}
		}
		else
		{
			_cashDeskPOSTerminals.PopupContent = null;
			_cashDeskPOSTerminals.IsEnabled = false;
			_posTerminalTypes.SelectedIndex = Convert.ToInt32("0");
			_bankAcquiringContainer.PartnerCode.Code = "";
			_bankAcquiringContainer.Contract.Code = "";
			_nonCashPaymentMethods.SelectedIndex = 0;
			PO.POSTerminalAddress = "";
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
		SummaNCC_EditValueChanged();
	}

	private void SummaNCC_EditValueChanged()
	{
		if (_summa.Value != 0m && _currExChngRate.Base != 0m)
		{
			_currExChngRate.Rate = (Convert.ToDecimal(_summaNCC.EditValue) / _summa.Value * _currExChngRate.Base).RoundAs(4);
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

	private void ChangeSumma()
	{
		_summaValueSpellInfo.Message = (_summa.DoValidate() ? Convert.ToDecimal(_summa.EditValue).STOC(_currCodeUnitCaption, _currCodeCentCaption, toUpperFirstChar: true, isPrintMode: false) : "");
		_summa.DoValidate();
		_partnerContractContainer.CalculatePartSumma();
	}

	private void ChangeSummaNCC()
	{
		_summaNCCValueSpellInfo.Message = (_summaNCC.DoValidate() ? Convert.ToDecimal(_summaNCC.EditValue).STOC(ArmSoft.SME.Trade.Common.Resources.Dram, ArmSoft.SME.Trade.Common.Resources.Luma, toUpperFirstChar: true, isPrintMode: false) : "");
		_summaNCC.DoValidate();
		_partnerContractContainer.CalculatePartSumma();
	}

	private void SetCashierShiftNumber()
	{
		if (!string.IsNullOrWhiteSpace(_cashDesk.Code))
		{
			_cashierShift.Text = TradeSettings.CreateTradeClient().CommonServices.GetOpenCashierShiftNumberByCashDesk(_cashDesk.Code);
		}
		else
		{
			_cashierShift.Text = "";
		}
	}

	private void PartnerCodeEditValueChanged(object sender, RoutedEventArgs e)
	{
		PartnerCodeEditValueChanged();
	}

	private void PartnerCode_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		PartnerCodeEditValueChanged();
	}

	private void ContractCodeEditValueChanged_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		if (_partnerContractContainer.ContractCurrCode.Code == "")
		{
			_partnerContractContainer.SummaPart.Value = 0m;
			_partnerContractContainer.SummaPart.IsReadOnly = true;
		}
		else if (_partnerContractContainer.ContractCurrCode.Code != "AMD")
		{
			_partnerContractContainer.SummaPart.IsReadOnly = false;
		}
		else
		{
			_partnerContractContainer.SummaPart.Value = _summaNCC.Value;
			_partnerContractContainer.SummaPart.IsReadOnly = true;
		}
	}

	private void PartnerCodeEditValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(_partnerContractContainer.PartnerCode.Code) && _partnerContractContainer.PartnerCode.DoValidate())
		{
			_receiverName.EditValue = _partnerContractContainer.PartnerCode.Comment.Trim();
			_receiverName.DoValidate();
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
		_printECRCheck.IsEnabled = BaseDocument != null && BaseDocument.POType == 2;
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
		_isAskQuestionConfirmSummChange = false;
		_currCode.Code = BaseDocument.CurrCode;
		_isAskQuestionConfirmSummChange = true;
		_currExChngRate.Rate = BaseDocument.ExchangeRate.Rate;
		_currExChngRate.Base = BaseDocument.ExchangeRate.Base;
		_summa.EditValue = BaseDocument.Summ;
		_summaNCC.EditValue = BaseDocument.SummNCC;
		if (_opType.Code == "1" || _opType.Code == "4")
		{
			_partnerContractContainer.PartnerCode.Code = BaseDocument.PartnerContractGroup.PartnerCode;
			_partnerContractContainer.Contract.Code = BaseDocument.PartnerContractGroup.ContractCode;
			_partnerContractContainer.ValueChangedForLoadDoc();
			PartnerCodeEditValueChanged();
		}
		_comment.EditValue = BaseDocument.Comment;
		if (BaseDocument.POType == 2)
		{
			CashIn cashIn = (CashIn)BaseDocument;
			_cashDesk.Code = cashIn.CashDesk;
			_receiverName.EditValue = cashIn.PayerName;
			_base.EditValue = cashIn.Base;
			_appendix.EditValue = cashIn.Appendix;
			PO.BankAcquiringPartnerGroup.PartnerCode = cashIn.BankAcquiringPartnerGroup.PartnerCode;
			PO.BankAcquiringPartnerGroup.ContractCode = cashIn.BankAcquiringPartnerGroup.ContractCode;
			PO.NonCashPaymentMethod = cashIn.NonCashPaymentMethod.Value;
			PO.POSTerminalType = cashIn.POSTerminalType;
			_opType.Code = cashIn.OpType;
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

	private void BE_PrintECRCheck_Click(object sender, RoutedEventArgs e)
	{
		OnF3();
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

	private void PrintCheck()
	{
		CashCacheRow cashRow = TradeSettings.TradeCacheService.Cash.GetItem(_cashDesk.Code);
		ECRPrintCheckWindow eCRPrintCheckWindow;
		if (_opType.Code == "2")
		{
			if (!ECRPreliminaryChecksAndInitalization(cashRow))
			{
				return;
			}
			eCRPrintCheckWindow = new ECRPrintCheckWindow(this, _ecr, cashRow.ECROperator, cashRow.ECROperatorPassword, cashRow.ShowECRTracking, CreateCashOutRequest(cashRow.ECROperator), ECRPrintCheckWindow.ECRCheckType.CashInOut);
		}
		else
		{
			if (!ECRPreliminaryChecksAndInitalization(cashRow, prepaymentCheck: true, (BaseDocument != null) ? ((CashIn)BaseDocument).ECRCRN : ""))
			{
				return;
			}
			PosTerminalResponse posTerminalResponse = RefundPOSTerminalIfChosen();
			string templateCode = PrintTemplateHelper.CombinePrintTemplateName(3, 3);
			eCRPrintCheckWindow = new ECRPrintCheckWindow(this, _ecr, cashRow.ECROperator, cashRow.ECROperatorPassword, cashRow.ShowECRTracking, CreateReturnRequest(cashRow, posTerminalResponse), ECRPrintCheckWindow.ECRCheckType.Return, (ECRPrintCheckWindow window, Utilities.PrintReceiptResponse response) =>
			{
				ECRHelpers.PrintOnOtherPrinter(window, response, cashRow, PO, templateCode);
			}, (string checkNumber, string errorMessage) =>
			{
				int num = 1;
				List<PropertyWithValue> list = new List<PropertyWithValue>(num);
				CollectionsMarshal.SetCount(list, num);
				CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("ECRReturnCheckNum", checkNumber);
				List<PropertyWithValue> editedProperties = list;
				PO.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, errorMessage, POISN(), editedProperties, PO.TS);
				_ecrReturnCheckNum.Text = checkNumber;
				PO.ECRReturnCheckNum = checkNumber;
			});
		}
		if (eCRPrintCheckWindow.Succeeded)
		{
			UpdateDocAndWindowAfterCheckPrint(eCRPrintCheckWindow.CheckNumber);
		}
	}

	private PosTerminalResponse RefundPOSTerminalIfChosen()
	{
		PosTerminalResponse result = null;
		if (BaseDocument == null || BaseDocument.POType != (short)DocumentTypeSE.CashIn || PO.PaymentRefundAccepted)
		{
			return result;
		}
		CashIn cashIn = (CashIn)BaseDocument;
		if (!cashIn.POSTerminalType.InList("2", "4"))
		{
			return result;
		}
		string text;
		if (string.IsNullOrWhiteSpace(cashIn.PaymentId))
		{
			text = "";
		}
		else
		{
			text = (cashIn.PaymentId.Contains(':') ? cashIn.PaymentId.Split(':')[1].Trim() : cashIn.PaymentId.Trim());
		}
		if (cashIn.POSTerminalType == "2")
		{
			result = Arcus2Helpers.CheckAndRefund(cashIn.POSTerminalID, _summaNCC.Value, cashIn.Summ.Value, text);
		}
		if (cashIn.POSTerminalType == "4")
		{
			result = PossoftPayClientHelper.Refund(text, PO.POSTerminalAddress, _summaNCC.Value);
		}
		UpdatePaymentStatus();
		return result;
	}

	private void UpdatePaymentStatus()
	{
		try
		{
			PO.PaymentRefundAccepted = true;
			int num = 1;
			List<PropertyWithValue> list = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list, num);
			CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("PaymentRefundAccepted", PO.PaymentRefundAccepted.ToString());
			List<PropertyWithValue> properties = list;
			StoreRequest sr = new StoreRequest
			{
				IsDataChanged = true,
				LogComment = ArmSoft.SME.Common.Resources.IsRefunded,
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

	private bool ECRPreliminaryChecksAndInitalization(CashCacheRow cashRow, bool prepaymentCheck = false, string baseDocECRCRN = "")
	{
		if (((!(_opType.Code == "1") && !(_opType.Code == "4")) || BaseDocument == null || BaseDocument.POType != (short)DocumentTypeSE.CashIn) && !(_opType.Code == "2"))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.BaseDocNeedsForECRC, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (_date.EditValue != TradeSettings.CreateTradeClient().CommonServices.GetServerDate().ToDateOnly())
		{
			throw new ASException(ArmSoft.SME.Common.Resources.DocDateNotCurrent);
		}
		if (cashRow == null)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.NotInKassa, _cashDesk.Code.Trim()));
		}
		if (prepaymentCheck && string.IsNullOrWhiteSpace(baseDocECRCRN) && string.IsNullOrWhiteSpace(cashRow.ECRCrn))
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.ECRCrn));
		}
		if (string.IsNullOrWhiteSpace(cashRow.ECROperator) || string.IsNullOrWhiteSpace(cashRow.ECROperatorPassword))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidateECROperatorOrECROperatorPin, _cashDesk.Code), ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (!cashRow.ECROperator.IsNumeric())
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (State == DocumentStateSE.Deleted.Value)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.MustBeSaveDocBeforePrintECRCheck, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if ((!StoredInDb || IsChanged || State == DocumentStateSE.Draft.Value) && !SaveDocument())
		{
			return false;
		}
		if (_opType.Code == "1")
		{
			CashIn cashIn = (CashIn)BaseDocument;
			if (string.IsNullOrWhiteSpace(cashIn.ECRCheckNum) || cashIn.ECRCheckNum == "XXXXXXXX")
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyECRNumForBaseDoc, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return false;
			}
		}
		ECRHelpers.InitECR(ref _ecr, PO.CashDesk);
		if (_ecr == null)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		return true;
	}

	private Utilities.PrintReturnReceiptRequest CreateReturnRequest(CashCacheRow cashRow, PosTerminalResponse posTerminalResponse)
	{
		CashIn cashIn = (CashIn)BaseDocument;
		Utilities.PrintReturnReceiptRequest printReturnReceiptRequest = new Utilities.PrintReturnReceiptRequest();
		if (string.IsNullOrWhiteSpace(cashIn.ECRCRN))
		{
			if (!string.IsNullOrWhiteSpace(cashIn.CashDesk) && cashRow.Code.Trim() != cashIn.CashDesk.Trim())
			{
				CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(cashIn.CashDesk);
				printReturnReceiptRequest.crn = ((item == null) ? cashRow.ECRCrn : item.ECRCrn);
			}
			else
			{
				printReturnReceiptRequest.crn = cashRow.ECRCrn;
			}
		}
		else
		{
			printReturnReceiptRequest.crn = cashIn.ECRCRN;
		}
		int.TryParse(cashIn.ECRCheckNum, out var result);
		printReturnReceiptRequest.returnTicketId = result;
		printReturnReceiptRequest.cardAmountForReturn = ((cashIn.OpType == "4") ? _summaNCC.Value : 0m);
		printReturnReceiptRequest.cashAmountForReturn = _summaNCC.Value - printReturnReceiptRequest.cardAmountForReturn;
		if (posTerminalResponse != null && !string.IsNullOrWhiteSpace(posTerminalResponse.RRN))
		{
			printReturnReceiptRequest.rrn = posTerminalResponse.RRN;
			if (!string.IsNullOrWhiteSpace(posTerminalResponse.TerminalId))
			{
				printReturnReceiptRequest.terminalId = posTerminalResponse.TerminalId;
			}
		}
		return printReturnReceiptRequest;
	}

	private Utilities.PrintCashInOutRequest CreateCashOutRequest(string ecrOperator)
	{
		return new Utilities.PrintCashInOutRequest
		{
			amount = (double)_summaNCC.Value,
			isCashIn = false,
			cashierId = int.Parse(ecrOperator),
			description = _comment.Text
		};
	}

	private void UpdateDocAndWindowAfterCheckPrint(string checkNumber)
	{
		try
		{
			_ecrReturnCheckNum.Text = checkNumber;
			PO.ECRReturnCheckNum = checkNumber;
			int num = 1;
			List<PropertyWithValue> list = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list, num);
			CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("ECRReturnCheckNum", PO.ECRReturnCheckNum);
			List<PropertyWithValue> editedProperties = list;
			PO.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, string.Empty, POISN(), editedProperties, PO.TS);
			LoadDataToControls();
			IsChanged = false;
			SetControlsReadonly();
			UpdateDataViewPanel(DVPID);
		}
		catch (Exception)
		{
			throw;
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
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barSubItemJournals.Content = "";
		_barSubItemJournals.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemJournals.LargeGlyph = ImageService.GetImage("report_32.svg");
		_barButtonPartnerOperationJournal = new ReportBarButtonItem(DataProviderTypeSE.PartOpJournal.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
		_barButtonPartnerOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonCashOperationJournal = new ReportBarButtonItem(DataProviderTypeSE.CashOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.KassaOp);
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

	protected override void CreateChildDocument(short childDocType, CashOut baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		if (childDocType == 2)
		{
			CreateBasedOnDocHelper.CreateDocument((DocumentTypeSE)(SmartEnum<DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
		}
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonPartnerOperationJournal.IsEnabled = isEnabled;
		_barButtonCashOperationJournal.IsEnabled = isEnabled;
	}

	protected override Control ControlByName(string name)
	{
		return name switch
		{
			"Date" => (Control)_date, 
			"DocNum" => _docNum, 
			"CashDesk" => _cashDesk, 
			"CurrCode" => _currCode, 
			"Summ" => _summa, 
			"SummNCC" => _summaNCC, 
			"PartnerCode" => _partnerContractContainer.PartnerCode, 
			"Contract" => _partnerContractContainer.Contract, 
			"SummaPart" => _partnerContractContainer.SummaPart, 
			"AccountInBankCode" => _settlAcc, 
			"PayerName" => _receiverName, 
			"NPassport" => _nPassport, 
			"Base" => _base, 
			"Appendix" => _appendix, 
			"Comment" => _comment, 
			_ => null, 
		};
	}
}
