using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.PersistableObject;
using ArmSoft.SME.Trade.Common.SmartEnums;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.Documents;

[DocumentWindow(6)]
public class PartnerRemCorrectWindow : TradeDocumentWindow<PartnerRemCorrect>
{
	private PartnerContractGroupControls _partnerContractContainer;

	private ExchangeRateControl _currExChngRate;

	private PopupCalcEditExt _summa;

	private PopupCalcEditExt _summaNCC;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonPartnerOperationJournal;

	private string _currCodeOldValue = string.Empty;

	protected override double WidthValue => 630.0;

	protected override double HeightValue => 370.0;

	protected override double MinWidthValue => 400.0;

	protected override double MinHeightValue => 170.0;

	public PartnerRemCorrectWindow()
	{
	}

	public PartnerRemCorrectWindow(PartnerRemCorrect doc)
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
		_partnerContractContainer = AddPartnerContractGroupControls(layoutGroupExt, (PartnerContractGroupDefinition)Definition.Fields["PartnerContractGroup"], _date, null, null);
		_partnerContractContainer.PartnerCode.IsRequired = true;
		_partnerContractContainer.ContractCurrCode.IsRequired = true;
		_partnerContractContainer.ContractCodeEditValueChanged += new SafeDocumentEventHandler<EventArgs>(PartnerContractEditValueChanged, this).Handler;
		_summa = AddNumericControl(layoutGroupExt, Definition.Fields["Summ"]);
		_summa.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Summa_EditValueChanged, this).Handler;
		_currExChngRate = AddExchangeRateControl(layoutGroupExt, Definition.Fields["ExchangeRate"]);
		_currExChngRate.EditValueChanged += new SafeDocumentEventHandler<RateEditValueChangedEventArgs>(CurrExChngRate_EditValueChanged, this).Handler;
		_summaNCC = AddNumericControl(layoutGroupExt, Definition.Fields["SummNCC"]);
		_summaNCC.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(SummaNCC_EditValueChanged, this).Handler;
		List<string> captionsList = TradeSettings.TradeCacheService.StandardComments.GetCaptionsList();
		_comment = AddComboBoxControl(layoutGroupExt, Definition.Fields["Comment"], captionsList, HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(layoutGroupExt, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
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

	private void CurrExChngRate_EditValueChanged(object sender, RateEditValueChangedEventArgs e)
	{
		if (_currExChngRate.DoValidate() && _summa.Value != 0m && _currExChngRate.Base != 0m)
		{
			_summaNCC.EditValue = PriceHelper.GetSumNNC(_summa.Value, new ExchangeRate(_currExChngRate.Base, _currExChngRate.Rate));
		}
	}

	private void SummaNCC_EditValueChanged(object sender, RoutedEventArgs e)
	{
		if (_summa.Value != 0m && _currExChngRate.Base != 0m)
		{
			_currExChngRate.Rate = ((decimal)_summaNCC.EditValue / _summa.Value * _currExChngRate.Base).RoundAs(4);
			_currExChngRate.DoValidate();
		}
	}

	private void Summa_EditValueChanged(object sender, RoutedEventArgs e)
	{
		if (_currExChngRate.DoValidate())
		{
			if (string.IsNullOrWhiteSpace(_partnerContractContainer.ContractCurrCode.Code) || _partnerContractContainer.ContractCurrCode.Code == "AMD" || !_partnerContractContainer.ContractCurrCode.DoValidate())
			{
				_currExChngRate.Rate = 1m;
				_currExChngRate.Base = 1m;
				_summaNCC.EditValue = _summa.EditValue;
			}
			else if (_currExChngRate.DoValidate() && _currExChngRate.Base != 0m)
			{
				_summaNCC.EditValue = PriceHelper.GetSumNNC(_summa.Value, new ExchangeRate(_currExChngRate.Base, _currExChngRate.Rate));
			}
		}
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
		_setFocusAfterInvalidSave = _date;
	}

	protected override void OnNewAfterSave()
	{
		PO = new PartnerRemCorrect();
		base.OnNewAfterSave();
		_summa.EditValue = 0m;
		_summaNCC.EditValue = 0m;
		_owner.Code = Settings.LoginService.UserName;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.CurrCode = _partnerContractContainer.ContractCurrCode.Code;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_partnerContractContainer.ContractCurrCode.Code = PO.CurrCode;
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

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			bool isReadOnly = _partnerContractContainer.ContractCurrCode.Code == "AMD" || string.IsNullOrWhiteSpace(_partnerContractContainer.ContractCurrCode.Code);
			_currExChngRate.IsReadOnly = isReadOnly;
			_summaNCC.IsReadOnly = isReadOnly;
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
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		BarSubItem barSubItem = new BarSubItem();
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref barSubItem);
		_barSubItemJournals.Content = "";
		_barSubItemJournals.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemJournals.LargeGlyph = ImageService.GetImage("report_32.svg");
		_barButtonPartnerOperationJournal = new ReportBarButtonItem(DataProviderTypeSE.PartOpJournal.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
		_barButtonPartnerOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
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
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonPartnerOperationJournal.IsEnabled = isEnabled;
	}

	protected override bool AllowPrint()
	{
		return true;
	}

	protected override bool AllowPrintOnNew()
	{
		return true;
	}

	private void PartnerContractEditValueChanged(object sender, EventArgs e)
	{
		if (_currCodeOldValue != _partnerContractContainer.ContractCurrCode.Code)
		{
			ContractCurrCode_EditValueChanged();
			_currCodeOldValue = _partnerContractContainer.ContractCurrCode.Code;
		}
	}

	private void ContractCurrCode_EditValueChanged()
	{
		bool flag = false;
		if (_summa.Value != 0m && !string.IsNullOrWhiteSpace(_partnerContractContainer.ContractCurrCode.Code) && _partnerContractContainer.ContractCurrCode.DoValidate() && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ConfirmSummChange, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
		{
			flag = true;
		}
		ChangedCurrCodeOrDate();
		if (flag && _currExChngRate.Rate == 0m)
		{
			_summa.Value = PriceHelper.GetSumCurr(_summaNCC.Value, new ExchangeRate(_currExChngRate.Base, _currExChngRate.Rate));
		}
	}

	private void ChangedCurrCodeOrDate()
	{
		if (string.IsNullOrWhiteSpace(_partnerContractContainer.ContractCurrCode.Code) || _partnerContractContainer.ContractCurrCode.Code == "AMD" || !_partnerContractContainer.ContractCurrCode.DoValidate())
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
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_partnerContractContainer.ContractCurrCode.Code, _date.EditValue.Value);
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
}
