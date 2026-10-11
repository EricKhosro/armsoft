using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Documents.Banks_and_Cashes;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.Documents;

[DocumentWindow(30)]
public class BonusGroupCorrectWindow : PartnerBonusGroupCorrectBaseWindow<BonusGroupCorrect>
{
	protected DropDownControl _cashDesk;

	protected TextEditExt _cashierShift;

	private ReportBarButtonItem _barButtonBonusOperationJournal;

	protected override double WidthValue => 630.0;

	protected override double HeightValue => 370.0;

	protected override double MinWidthValue => 400.0;

	protected override double MinHeightValue => 170.0;

	public BonusGroupCorrectWindow()
	{
	}

	public BonusGroupCorrectWindow(BonusGroupCorrect doc)
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
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_cashDesk = AddDropDownControl(null, Definition.Fields["CashDesk"], new Cashes(withoutCheckAccess: true));
		_cashDesk.EditValueChanged += CashDesk_EditValueChanged;
		_cashierShift = AddStringControl(null, Definition.Fields["CashierShiftNumber"]);
		_currCode = AddDropDownControl(null, Definition.Fields["CurrCode"], new Currency(shortMode: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_currExChngRate = AddExchangeRateControl(null, Definition.Fields["ExchangeRate"]);
		_partRemGrid = AddGrid(null, Definition.Grids["PartRemList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_partRemGrid.IsRequired = true;
		_partRemTableView = _partRemGrid.View;
		_partRemGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(PartRemGrid_AddingRow, this).Handler;
		_partRemGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(PartRemGrid_DuplicatingRow, this).Handler;
		_partRemGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(PartRemGrid_ClickDropDown, this).Handler;
		_partRemGrid.AddingRowWithDialog += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(PartRemGrid_AddingRowWithDialog, this).Handler;
		_partRemGrid.DuplicatingRowWithDialog += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(PartRemGrid_DuplicatingRowWithDialog, this).Handler;
		_partRemGrid.EditingRowWithDialog += new SafeDocumentEventHandler<DocumentGridEditingRowEventArgs>(PartRemGrid_EditingRowWithDialog, this).Handler;
		_partRemTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(PartRemTableView_CellValueChanged, this).Handler;
		_partRemTableView.FocusedRowHandleChanged += new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(PartRemTableView_FocusedRowHandleChangedEventArgs, this).Handler;
		_partRemTableView.ShowingEditor += new SafeDocumentEventHandler<ShowingEditorEventArgs>(PartRemTableView_ShowingEditor, this).Handler;
		_partCodeCol = _partRemGrid.AddDropDownColumn(Definition.Grids["PartRemList"].Columns["PartCode"], new PartnersFilter(), allowAdditionButtons: true);
		DocumentGridColumn documentGridColumn = _partRemGrid.AddStringColumn(Definition.Grids["PartRemList"].Columns["PartName"]);
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		documentGridColumn.Width = 225.0;
		_contractCol = _partRemGrid.AddDropDownColumn(Definition.Grids["PartRemList"].Columns["Contract"], new Contracts(), allowAdditionButtons: false, isOwnProcessingClickDropDown: true);
		_summaCol = _partRemGrid.AddNumberColumn(Definition.Grids["PartRemList"].Columns["Summa"], showTotal: true);
		_partRemList = new ObservableCollection<PartBonusGroupCorrectSpecificationRow>();
		List<string> captionsList = TradeSettings.TradeCacheService.StandardComments.GetCaptionsList();
		_comment = AddComboBoxControl(null, Definition.Fields["Comment"], captionsList, HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(null, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
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

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			_cashierShift.IsReadOnly = true;
			_currCode.IsReadOnly = true;
			_currExChngRate.IsReadOnly = true;
		}
	}

	private void CashDesk_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_cashDesk.Code) && TradeSettings.TradeCacheService.Cash.GetItem(_cashDesk.Code) != null && !TradeSettings.CreateTradeClient().CommonServices.IsCashDeskAvailable(Convert.ToString(e.NewValue)))
		{
			e.Handled = true;
			_cashDesk.Code = Convert.ToString(e.OldValue);
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.CashDeskNotAvailForUser, Settings.LoginService.UserName, Convert.ToString(e.NewValue)), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
		else
		{
			SetCashierShiftNumber();
		}
	}

	private void SetCashierShiftNumber()
	{
		if (!string.IsNullOrWhiteSpace(_cashDesk.Code))
		{
			_cashierShift.Text = TradeSettings.CreateTradeClient().CommonServices.GetOpenCashierShiftNumberByCashDesk(_cashDesk.Code);
		}
		else
		{
			_cashierShift.Text = string.Empty;
		}
	}

	protected void PartRemGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		if (string.IsNullOrWhiteSpace(_currCode.Code))
		{
			ControlsHelper.SetFocus(_currCode);
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidateEmptyCurrCode);
		}
		if (Settings.CoreCacheService.Currency.GetItem(_currCode.Code) == null)
		{
			ControlsHelper.SetFocus(_currCode);
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.NotInCurr, _currCode.Code));
		}
		_partRemList.Add(new PartBonusGroupCorrectSpecificationRow());
		e.RowNumber = _partRemList.Count - 1;
	}

	private void PartRemGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			PartBonusGroupCorrectSpecificationRow partBonusGroupCorrectSpecificationRow = (PartBonusGroupCorrectSpecificationRow)_partRemGrid.Grid.GetRow(e.RowHandle);
			if (e.Column == _contractCol && partBonusGroupCorrectSpecificationRow != null)
			{
				Contracts contracts = new Contracts(partBonusGroupCorrectSpecificationRow.PartCode, _currCode.Code, isExceptCurrCodeNCC: false, isFilterBySuppliers: false, isFilterByCustomers: false, isValueMemberPartner: false, showOnlyBonusCards: true);
				ModalBrowser modalBrowser = new ModalBrowser(contracts);
				modalBrowser.LookUpValue = e.Value;
				modalBrowser.Owner = e.Owner;
				modalBrowser.Left = e.Left;
				modalBrowser.Top = e.Top;
				if (modalBrowser.Show())
				{
					e.NewValue = contracts.ValueMember(modalBrowser.SelectedRowIndex);
					if (string.IsNullOrWhiteSpace(partBonusGroupCorrectSpecificationRow.PartCode))
					{
						ContractDataProvider.DataRow.Key key = (ContractDataProvider.DataRow.Key)contracts.KeyMember(modalBrowser.SelectedRowIndex);
						if (key != null && TradeSettings.TradeCacheService.Partner.GetContract(key.PartCode, key.Contract, out var partnerCacheRow) != null)
						{
							partBonusGroupCorrectSpecificationRow.PartCode = partnerCacheRow.PartCode;
							partBonusGroupCorrectSpecificationRow.PartName = partnerCacheRow.Caption;
						}
					}
				}
			}
			_partRemGrid.Grid.RefreshData();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void PartRemTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		PartBonusGroupCorrectSpecificationRow partBonusGroupCorrectSpecificationRow = (PartBonusGroupCorrectSpecificationRow)_partRemGrid.Grid.GetRow(e.RowHandle);
		if (partBonusGroupCorrectSpecificationRow == null)
		{
			return;
		}
		if (e.Column == _partCodeCol)
		{
			try
			{
				if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					partBonusGroupCorrectSpecificationRow.PartCode = "";
					partBonusGroupCorrectSpecificationRow.PartName = "";
					partBonusGroupCorrectSpecificationRow.Contract = "";
					partBonusGroupCorrectSpecificationRow.Summa = 0m;
					_contractCol.DataView = new Contracts(partBonusGroupCorrectSpecificationRow.PartCode, _currCode.Code, isExceptCurrCodeNCC: false, isFilterBySuppliers: false, isFilterByCustomers: false, isValueMemberPartner: false, showOnlyBonusCards: true);
				}
				else
				{
					string key = e.Value.ToString();
					PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(key, throwExceptionNotExits: true);
					partBonusGroupCorrectSpecificationRow.PartCode = item.PartCode;
					partBonusGroupCorrectSpecificationRow.PartName = item.Caption;
					_contractCol.DataView = new Contracts(item.PartCode, _currCode.Code, isExceptCurrCodeNCC: false, isFilterBySuppliers: false, isFilterByCustomers: false, isValueMemberPartner: false, showOnlyBonusCards: true);
					partBonusGroupCorrectSpecificationRow.Summa = 0m;
				}
			}
			catch (Exception ex)
			{
				partBonusGroupCorrectSpecificationRow.PartCode = "";
				partBonusGroupCorrectSpecificationRow.PartName = "";
				partBonusGroupCorrectSpecificationRow.Contract = "";
				partBonusGroupCorrectSpecificationRow.Summa = 0m;
				_contractCol.DataView = new Contracts(partBonusGroupCorrectSpecificationRow.PartCode, _currCode.Code, isExceptCurrCodeNCC: false, isFilterBySuppliers: false, isFilterByCustomers: false, isValueMemberPartner: false, showOnlyBonusCards: true);
				Settings.ErrorHandler(ex);
			}
		}
		else if (e.Column == _summaNCCCol)
		{
			if (_currExChngRate.DoValidate() && _currCode.Code != "AMD")
			{
				partBonusGroupCorrectSpecificationRow.Summa = PriceHelper.GetSumCurr(Convert.ToDecimal(e.Value), new ExchangeRate(_currExChngRate.Base, _currExChngRate.Rate));
			}
			else
			{
				partBonusGroupCorrectSpecificationRow.Summa = Convert.ToDecimal(e.Value);
			}
		}
		_partRemGrid.Grid.RefreshData();
	}

	protected void PartRemTableView_FocusedRowHandleChangedEventArgs(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _partRemTableView.FocusedRowHandle;
		PartBonusGroupCorrectSpecificationRow partBonusGroupCorrectSpecificationRow = (PartBonusGroupCorrectSpecificationRow)_partRemGrid.Grid.GetRow(focusedRowHandle);
		if (partBonusGroupCorrectSpecificationRow != null)
		{
			ChangeColumnAdditonalView(partBonusGroupCorrectSpecificationRow.PartCode);
		}
	}

	private void ChangeColumnAdditonalView(string partnerCode)
	{
		_contractCol.DataView = new Contracts(partnerCode, _currCode.Code);
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
		_barButtonBonusOperationJournal = new ReportBarButtonItem(DataProviderTypeSE.BonusOpJournal.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.BonusOp);
		_barButtonBonusOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
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
		ReportBarButtonItem barButtonBonusOperationJournal = _barButtonBonusOperationJournal;
		num = 2;
		List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<PropertyWithValue> span2 = CollectionsMarshal.AsSpan(list2);
		span2[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span2[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonBonusOperationJournal.PropertyWithValues = list2;
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonBonusOperationJournal.IsEnabled = isEnabled;
	}
}
