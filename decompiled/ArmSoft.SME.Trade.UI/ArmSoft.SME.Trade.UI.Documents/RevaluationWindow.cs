using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.Documents;

[DocumentWindow(17)]
public class RevaluationWindow : TradeDocumentWindow<Revaluation>
{
	private DropDownControl _currCode;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	private CheckEditExt _revalCashes;

	private CheckEditExt _revalBankAccs;

	private CheckEditExt _revalPartner;

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonPartnerOperationJournal;

	private ReportBarButtonItem _barButtonCashOperationJournal;

	private ReportBarButtonItem _barButtonBankOperationJournal;

	public RevaluationWindow()
	{
	}

	public RevaluationWindow(Revaluation doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_currCode = AddDropDownControl(null, Definition.Fields["CurrCode"], new Currency(shortMode: true, withOutNCC: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_comment = AddComboBoxControl(null, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(null, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		LayoutGroupExt layoutGroupExt = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Vertical);
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Stretch;
		_revalCashes = AddBooleanControl(layoutGroupExt, Definition.Fields["IsRevalCashes"]);
		_revalBankAccs = AddBooleanControl(layoutGroupExt, Definition.Fields["IsRevalBankAccs"]);
		_revalPartner = AddBooleanControl(layoutGroupExt, Definition.Fields["IsRevalPartners"]);
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString());
		span[1] = new PropertyWithValue("StartDate", PO.DocDate.ToString());
		span[2] = new PropertyWithValue("EndDate", PO.DocDate.ToString());
		span[3] = new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString());
		barButtonJumpTo.PropertyWithValues = list;
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
		_barButtonCashOperationJournal = new ReportBarButtonItem(DataProviderTypeSE.CashOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.KassaOp);
		_barButtonCashOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonBankOperationJournal = new ReportBarButtonItem(DataProviderTypeSE.BankOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.BankOpJournal);
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
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString());
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
		ReportBarButtonItem barButtonBankOperationJournal = _barButtonBankOperationJournal;
		num = 2;
		List<PropertyWithValue> list4 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list4, num);
		Span<PropertyWithValue> span4 = CollectionsMarshal.AsSpan(list4);
		span4[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span4[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonBankOperationJournal.PropertyWithValues = list4;
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
	}

	protected override void OnNewAfterSave()
	{
		PO = new Revaluation();
		base.OnNewAfterSave();
		_revalCashes.IsChecked = false;
		_revalBankAccs.IsChecked = false;
		_revalPartner.IsChecked = false;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonPartnerOperationJournal.IsEnabled = isEnabled;
		_barButtonCashOperationJournal.IsEnabled = isEnabled;
		_barButtonBankOperationJournal.IsEnabled = isEnabled;
	}
}
