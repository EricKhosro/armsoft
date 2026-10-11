using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.ASReport;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DataView;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.ReportViewers.Goods;

public class MTAccountReportViewer : AsReport
{
	private class ShowDailyRemsCurrencyRepRowCell : AsRepRowCell
	{
		public ShowDailyRemsCurrencyRepRowCell()
		{
			ValueStyle = AsRepColStyle.CurrencyStyle;
			FontStyle = FontStyles.Italic;
			FontWeight = FontWeights.Bold;
		}
	}

	private class SimpleCurrencyRepRowCell : AsRepRowCell
	{
		public SimpleCurrencyRepRowCell()
		{
			ValueStyle = AsRepColStyle.CurrencyStyle;
			FontWeight = FontWeights.Bold;
		}
	}

	private List<ListOfSelectedMaterialStorage> _selectedRows;

	private const string _reportTableName = "MTTAccount";

	private List<AsRepColumn> _listOfRepRepColumns;

	private bool _isSelectedRows;

	private readonly bool _showPartySupplier;

	private readonly bool _showPartyContract;

	private readonly bool _showPartyDocType;

	private readonly bool _showPartyDocNum;

	private readonly bool _showPartyUseableDate;

	private readonly bool _showPartyCountry;

	private readonly bool _showPartyIncomeDate;

	private readonly bool _showPartyProducer;

	private readonly bool _showPartyProperty1;

	private readonly bool _showPartyProperty2;

	private decimal _totalTurnSaleSum;

	private decimal _totalTurnSaleVATSum;

	private decimal _totalTurnSaleCurrSum;

	private bool _isShowFromTurns;

	private CheckEditExt _groupByCorObject;

	private CheckEditExt _showEachDayRems;

	private CheckEditExt _showParties;

	private DropDownControl _goodGroup;

	private DropDownControl _goodCode;

	private CheckEditExt _showSumm;

	private CheckEditExt _showSummsWithVAT;

	private CheckEditExt _showCurrSumms;

	private CheckEditExt _showPrices;

	public bool ShowFromTurns
	{
		set
		{
			_isShowFromTurns = value;
		}
	}

	public DateOnly StartDate
	{
		get
		{
			return (DateOnly)_parameters["StartDate"].Value;
		}
		set
		{
			_parameters["StartDate"].Value = value;
		}
	}

	public DateOnly EndDate
	{
		get
		{
			return (DateOnly)_parameters["EndDate"].Value;
		}
		set
		{
			_parameters["EndDate"].Value = value;
		}
	}

	public string Storage
	{
		get
		{
			return (string)_parameters["Storage"].Value;
		}
		set
		{
			_parameters["Storage"].Value = value;
		}
	}

	public string GoodGroup
	{
		get
		{
			return (string)_parameters["GoodGroup"].Value;
		}
		set
		{
			_parameters["GoodGroup"].Value = value;
		}
	}

	public string GoodCode
	{
		get
		{
			return (string)_parameters["GoodCode"].Value;
		}
		set
		{
			_parameters["GoodCode"].Value = value;
		}
	}

	public bool ShowAltUnit
	{
		get
		{
			return (bool)_parameters["ShowAltUnit"].Value;
		}
		set
		{
			_parameters["ShowAltUnit"].Value = value;
		}
	}

	public bool ShowSumms
	{
		get
		{
			return (bool)_parameters["ShowSumms"].Value;
		}
		set
		{
			_parameters["ShowSumms"].Value = value;
		}
	}

	public bool ShowSummsWithVAT
	{
		get
		{
			return (bool)_parameters["ShowSummsWithVAT"].Value;
		}
		set
		{
			_parameters["ShowSummsWithVAT"].Value = value;
		}
	}

	public bool ShowCurrSumms
	{
		get
		{
			return (bool)_parameters["ShowCurrSumms"].Value;
		}
		set
		{
			_parameters["ShowCurrSumms"].Value = value;
		}
	}

	public bool ShowParties
	{
		get
		{
			return (bool)_parameters["ShowParties"].Value;
		}
		set
		{
			_parameters["ShowParties"].Value = value;
		}
	}

	public bool ShowSalePrice
	{
		get
		{
			return (bool)_parameters["ShowSalePrice"].Value;
		}
		set
		{
			_parameters["ShowSalePrice"].Value = value;
		}
	}

	public bool ShowEachDayRems
	{
		get
		{
			return (bool)_parameters["ShowEachDayRems"].Value;
		}
		set
		{
			_parameters["ShowEachDayRems"].Value = value;
		}
	}

	public bool GroupByCorObject
	{
		get
		{
			return (bool)_parameters["GroupByCorObject"].Value;
		}
		set
		{
			_parameters["GroupByCorObject"].Value = value;
		}
	}

	public override string Name => "MTTAccount";

	public override bool AllowEdit
	{
		get
		{
			if (AllowOperation)
			{
				return !GroupByCorObject;
			}
			return false;
		}
	}

	public override bool AllowView => !GroupByCorObject;

	public override bool AllowDelete
	{
		get
		{
			if (AllowOperation)
			{
				return !GroupByCorObject;
			}
			return false;
		}
	}

	public override bool AllowCreateCopy
	{
		get
		{
			if (AllowOperation)
			{
				return !GroupByCorObject;
			}
			return false;
		}
	}

	public override bool AllowPreview => !GroupByCorObject;

	public override bool AllowDocumentHistory
	{
		get
		{
			if (AllowOperation)
			{
				return !GroupByCorObject;
			}
			return false;
		}
	}

	private MTAccountReportParam _reportRequest { get; set; }

	protected Dictionary<string, MTAccountReportResult> _reportResult { get; set; }

	public MTAccountReportViewer(bool isForSelectedRows = false, List<ListOfSelectedMaterialStorage> selectedRows = null)
	{
		_isSelectedRows = isForSelectedRows;
		_selectedRows = selectedRows;
		DialogSupported = true;
		HelpId = "Purchases_Reports_ProductCard";
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("Storage", ArmSoft.SME.Common.Resources.Storage, "");
		AddParameter("GoodGroup", ArmSoft.SME.Common.Resources.Group, "");
		AddParameter("GoodCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, "");
		AddParameter("ShowAltUnit", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, false);
		AddParameter("ShowSumms", ArmSoft.SME.Trade.Common.Resources.ShowNCCSumma, false);
		AddParameter("ShowSummsWithVAT", ArmSoft.SME.Trade.Common.Resources.ShowNCCSummaWithVAT, false);
		AddParameter("ShowCurrSumms", string.Format(ArmSoft.SME.Trade.Common.Resources.ShowCurrSumma, Settings.ParametersService.MTCurr()), false);
		AddParameter("ShowParties", ArmSoft.SME.Common.Resources.ShowParties, false);
		AddParameter("ShowSalePrice", ArmSoft.SME.Trade.Common.Resources.ShowPrices, false);
		AddParameter("ShowEachDayRems", ArmSoft.SME.Trade.Common.Resources.MTTAccDlgShowDailyRem, false);
		AddParameter("GroupByCorObject", ArmSoft.SME.Trade.Common.Resources.MTTAccDlgGroupByCorObj, false);
		_showPartySupplier = Settings.ParametersService.ShowPartySUPPLIER();
		_showPartyContract = Settings.ParametersService.ShowPartyCONTRACT();
		_showPartyDocType = Settings.ParametersService.ShowPartyDocType();
		_showPartyDocNum = Settings.ParametersService.ShowPartyDocNum();
		_showPartyUseableDate = Settings.ParametersService.ShowPartyUSEABLEDATE();
		_showPartyIncomeDate = Settings.ParametersService.ShowPartyIncomeDate();
		_showPartyCountry = Settings.ParametersService.ShowPartyCountry();
		_showPartyProducer = Settings.ParametersService.ShowPartyProducer();
		_showPartyProperty1 = Settings.ParametersService.ShowPartyProperty1();
		_showPartyProperty2 = Settings.ParametersService.ShowPartyProperty2();
	}

	public override string Caption()
	{
		if (!_isSelectedRows)
		{
			return ArmSoft.SME.Trade.Common.Resources.TAccount + " - " + GoodCode.Trim();
		}
		return ArmSoft.SME.Trade.Common.Resources.TAccount;
	}

	public override bool ShowDialog(bool useParamValues, bool isRefreshMode)
	{
		if (!TradeSettings.CreateTradeClient().CommonServices.ExistsAvailableStorage())
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.AccessToStoragesDenied, Settings.LoginService.UserName), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return false;
		}
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = ArmSoft.SME.Trade.Common.Resources.TAccount,
			Name = Name,
			HelpId = HelpId
		};
		LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
		if (!isRefreshMode)
		{
			dialogWindow.AddStoreValuesButton(container);
		}
		DateEditControl dateEditControl = dialogWindow.AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, !_isShowFromTurns, ArmSoft.SME.Common.Resources.DatePeriodBegin);
		dateEditControl.EditValue = (useParamValues ? StartDate : Settings.HostingEnvironment.DateBegin);
		DateEditControl dateEditControl2 = dialogWindow.AddDateControl(container, "EndDate", "", useLongDate: false, isRequired: true, !_isShowFromTurns, ArmSoft.SME.Common.Resources.DatePeriodEnd);
		dateEditControl2.EditValue = (useParamValues ? EndDate : Settings.HostingEnvironment.DateEnd);
		dialogWindow.AddSpecialDateButton(container, dateEditControl, dateEditControl2);
		if (!_isSelectedRows)
		{
			string storage = ArmSoft.SME.Common.Resources.Storage;
			bool? checkViewOrOperationAccess = true;
			dialogWindow.AddMultiSelectDropDownControl("Storage", storage, new ArmSoft.SME.Trade.UI.DropDown.Storages(null, checkViewOrOperationAccess), isRequired: false, !_isShowFromTurns).SelectedString = (useParamValues ? Storage : string.Empty);
			_goodGroup = dialogWindow.AddDropDownControl("GoodGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: false, allowAdditionButtons: false, storeValue: true);
			_goodGroup.Code = (useParamValues ? GoodGroup : string.Empty);
			_goodCode = dialogWindow.AddDropDownControl("GoodCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value, "", isShowOnlyInPriceList: true), isRequired: true, allowAdditionButtons: false, !useParamValues, isShowComment: true, 8);
			_goodCode.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(GoodCode_ClickDropDown).Handler;
			_goodCode.Code = (useParamValues ? GoodCode : string.Empty);
		}
		dialogWindow.AddLine();
		dialogWindow.AddBooleanControl("ShowAltUnit", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, !_isShowFromTurns).IsChecked = useParamValues && ShowAltUnit;
		_showSumm = dialogWindow.AddBooleanControl("ShowSumms", ArmSoft.SME.Trade.Common.Resources.ShowNCCSumma, Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
		_showSumm.IsReadOnly = !Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
		_showSumm.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ShowNccSumm_EditValueChanged).Handler;
		_showSummsWithVAT = dialogWindow.AddBooleanControl("ShowSummsWithVAT", ArmSoft.SME.Trade.Common.Resources.ShowNCCSummaWithVAT, Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
		_showSummsWithVAT.IsReadOnly = !Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
		_showSummsWithVAT.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ShowNCCVATSumm_EditValueChanged).Handler;
		_showCurrSumms = dialogWindow.AddBooleanControl("ShowCurrSumms", string.Format(ArmSoft.SME.Trade.Common.Resources.ShowCurrSumma, Settings.ParametersService.MTCurr()), Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
		_showCurrSumms.IsReadOnly = !Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
		_showCurrSumms.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ShowCurrSumm_EditValueChanged).Handler;
		_showParties = dialogWindow.AddBooleanControl("ShowParties", ArmSoft.SME.Common.Resources.ShowParties, !_isShowFromTurns);
		dialogWindow.AddLine();
		_showPrices = dialogWindow.AddBooleanControl("ShowPrices", ArmSoft.SME.Trade.Common.Resources.ShowCostAndSalePrices, Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
		_showPrices.IsReadOnly = !Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
		_showSumm.IsChecked = Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && useParamValues && ShowSumms;
		_showSummsWithVAT.IsChecked = Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && useParamValues && ShowSummsWithVAT;
		_showCurrSumms.IsChecked = Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && useParamValues && ShowCurrSumms;
		_showPrices.IsChecked = Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && useParamValues && ShowSalePrice;
		UpdateShowPricesControl();
		_showEachDayRems = dialogWindow.AddBooleanControl("ShowEachDayRems", ArmSoft.SME.Trade.Common.Resources.MTTAccDlgShowDailyRem, storeValue: true);
		_groupByCorObject = dialogWindow.AddBooleanControl("GroupByCorObject", ArmSoft.SME.Trade.Common.Resources.MTTAccDlgGroupByCorObj, storeValue: true);
		_groupByCorObject.EditValueChanged += CE_GroupByCorObject_EditValueChanged;
		_showEachDayRems.EditValueChanged += CE_ShowEachDayRems_EditValueChanged;
		_showParties.EditValueChanged += CE_ShowParties_EditValueChanged;
		_showParties.IsChecked = useParamValues && ShowParties;
		_groupByCorObject.IsChecked = useParamValues && GroupByCorObject;
		_showEachDayRems.IsChecked = useParamValues && ShowEachDayRems;
		dialogWindow.WindowValidation += ParametersDialogValidation;
		if (dialogWindow.ShowDialog().Value)
		{
			StartDate = dateEditControl.EditValue.Value;
			EndDate = dateEditControl2.EditValue.Value;
			if (!_isSelectedRows)
			{
				Storage = (string)dialogWindow["Storage"];
				GoodGroup = _goodGroup.Code;
				GoodCode = _goodCode.Code;
			}
			ShowAltUnit = (bool)dialogWindow["ShowAltUnit"];
			ShowSumms = _showSumm.IsChecked.Value;
			ShowSummsWithVAT = _showSummsWithVAT.IsChecked.Value;
			ShowCurrSumms = _showCurrSumms.IsChecked.Value;
			ShowParties = (bool)dialogWindow["ShowParties"];
			ShowSalePrice = _showPrices.IsChecked.Value;
			GroupByCorObject = (bool)dialogWindow["GroupByCorObject"];
			ShowEachDayRems = (bool)dialogWindow["ShowEachDayRems"];
			return true;
		}
		return false;
	}

	private void ShowNccSumm_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		UpdateShowPricesControl();
	}

	private void ShowNCCVATSumm_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		UpdateShowPricesControl();
	}

	private void ShowCurrSumm_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		UpdateShowPricesControl();
	}

	private void UpdateShowPricesControl()
	{
		_showPrices.IsReadOnly = !_showSumm.IsChecked.Value && !_showSummsWithVAT.IsChecked.Value && !_showCurrSumms.IsChecked.Value;
		if (_showPrices.IsReadOnly)
		{
			_showPrices.IsChecked = false;
		}
	}

	private void GoodCode_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, ItemTypesSE.Goods, _goodGroup.DoValidate() ? _goodGroup.Code : "", isShowOnlyInPriceList: true);
		ModalBrowser modalBrowser = new ModalBrowser(goodsFilter)
		{
			LookUpValue = _goodCode.Code,
			Owner = e.Owner,
			Left = e.Left,
			Top = e.Top
		};
		if (modalBrowser.Show())
		{
			_goodCode.Code = goodsFilter.ValueMember(modalBrowser.SelectedRowIndex);
		}
		e.Cancel = true;
	}

	private void ParametersDialogValidation(DialogWindow dialog, DialogWindowValidationArgs args)
	{
		ArmSoft.SME.Core.UI.ValidationService.ValidateWrongPeriod(((DateOnly)dialog["StartDate"]).ToDateTime(), ((DateOnly)dialog["EndDate"]).ToDateTime());
		args.isValid = TradeDataViewParametersDialogWindow.CheckPeriodForActualness((DateOnly)dialog["EndDate"]);
	}

	private void CE_ShowParties_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_showParties.IsChecked.Value)
		{
			_groupByCorObject.IsChecked = false;
		}
	}

	private void CE_ShowEachDayRems_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_showEachDayRems.IsChecked.Value)
		{
			_groupByCorObject.IsChecked = false;
		}
	}

	private void CE_GroupByCorObject_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_groupByCorObject.IsChecked.Value)
		{
			_showEachDayRems.IsChecked = false;
			_showParties.IsChecked = false;
		}
	}

	public override void ExecuteDPR()
	{
		_reportRequest = new MTAccountReportParam
		{
			StartDate = StartDate,
			EndDate = EndDate,
			GoodCode = GoodCode,
			Storage = Storage,
			ShowParties = ShowParties,
			ShowSumms = ShowSumms,
			ShowSummsWithVAT = ShowSummsWithVAT,
			ShowCurrSumms = ShowCurrSumms,
			ShowSaleSumms = ShowSalePrice,
			ShowAltUnit = ShowAltUnit
		};
		_reportResult = new Dictionary<string, MTAccountReportResult>();
		if (!_isSelectedRows)
		{
			_reportResult.Add(GoodCode.PadLeft(20) + Storage.PadLeft(5), DataProcessingRequestHelper.Execute<MTAccountReportParam, MTAccountReportResult>(DPRType.Report, Name, _reportRequest));
			return;
		}
		foreach (ListOfSelectedMaterialStorage selectedRow in _selectedRows)
		{
			_reportRequest.GoodCode = selectedRow.GoodCode;
			_reportRequest.Storage = selectedRow.Storage;
			string key = selectedRow.GoodCode.PadLeft(20) + selectedRow.Storage.PadLeft(5);
			if (!_reportResult.ContainsKey(key))
			{
				_reportResult.Add(key, DataProcessingRequestHelper.ExecuteThroughHandler<MTAccountReportParam, MTAccountReportResult>(DPRType.Report, Name, _reportRequest).Item2);
			}
		}
	}

	public override bool BeforeShowReport()
	{
		ReportPanel.ClearAll();
		_listOfRepRepColumns = GetListOfRepColumns();
		ReportPanel.AddTableDefinition("MTTAccount", _listOfRepRepColumns);
		if (!_isSelectedRows)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_goodCode.Code);
			MTTAccountForOneMT(item, Storage);
		}
		else
		{
			foreach (ListOfSelectedMaterialStorage selectedRow in _selectedRows)
			{
				MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(selectedRow.GoodCode);
				MTTAccountForOneMT(item2, selectedRow.Storage);
				if (selectedRow != _selectedRows[_selectedRows.Count - 1])
				{
					ReportPanel.AddString("");
					ReportPanel.AddPageBreak(item2.MTCode + " / " + selectedRow.Storage + " " + item2.Caption);
				}
			}
		}
		return true;
	}

	private void MTTAccountForOneMT(MaterialCacheRow TAccGoodDoc, string TAccStorage)
	{
		int num = 0;
		decimal num2 = 0m;
		DateOnly dateOnly = default;
		decimal num3 = 0m;
		decimal num4 = 0m;
		decimal num5 = 0m;
		decimal num6 = 0m;
		decimal num7 = 0m;
		decimal num8 = 0m;
		decimal num9 = 0m;
		_totalTurnSaleSum = 0m;
		_totalTurnSaleVATSum = 0m;
		_totalTurnSaleCurrSum = 0m;
		string key = TAccGoodDoc.MTCode.PadLeft(20) + TAccStorage.PadLeft(5);
		GoodsTurnOverDataProvider.DataRow dataRow = new GoodsTurnOverDataProvider.DataRow();
		foreach (MTAccountReportResult.TurnRow turnRow in _reportResult[key].TurnRows)
		{
			dataRow.QtyIn += turnRow.QtyIn;
			dataRow.SumIn += turnRow.SumIn;
			dataRow.SumVATIn += turnRow.SumVATIn;
			dataRow.SumCurrIn += turnRow.SumCurrIn;
			dataRow.QtyOut += turnRow.QtyOut;
			dataRow.SumOut += turnRow.SumOut;
			dataRow.SumVATOut += turnRow.SumVATOut;
			dataRow.SumCurrOut += turnRow.SumCurrOut;
			dataRow.QtyBegin += turnRow.QtyBegin;
			dataRow.SumBegin += turnRow.SumBegin;
			dataRow.SumVATBegin += turnRow.SumVATBegin;
			dataRow.SumCurrBegin += turnRow.SumCurrBegin;
			dataRow.SaleSumBegin += turnRow.SaleSumBegin;
			dataRow.SaleSumVATBegin += turnRow.SaleSumVATBegin;
			dataRow.SaleSumCurrBegin += turnRow.SaleSumCurrBegin;
		}
		foreach (MTAccountReportResult.OperationRow operationRow in _reportResult[key].OperationRows)
		{
			switch (operationRow.DocType)
			{
			case 7:
			case 8:
			case 13:
			case 14:
			case 15:
			case 19:
			case 20:
			case 21:
			case 38:
				if (operationRow.DocPartID != -1)
				{
					PartnerCacheRow itemById = TradeSettings.TradeCacheService.Partner.GetItemById(operationRow.DocPartID);
					if (itemById != null)
					{
						operationRow.CorrObjectCode = itemById.PartCode;
						operationRow.CorrObjectCaption = itemById.Caption;
					}
				}
				break;
			case 10:
				operationRow.CorrObjectCode = operationRow.Storage;
				operationRow.CorrObjectCaption = TradeSettings.TradeCacheService.Storage.GetItem(operationRow.Storage)?.Caption;
				break;
			}
			if (operationRow.Quantity >= 0m)
			{
				operationRow.QuantityDb += operationRow.Quantity;
			}
			else
			{
				operationRow.QuantityCr = operationRow.Quantity;
			}
			if (ShowSumms)
			{
				if (operationRow.CostSumm >= 0m)
				{
					operationRow.CostSummDb = operationRow.CostSumm;
				}
				else
				{
					operationRow.CostSummCr = operationRow.CostSumm;
				}
			}
			if (ShowSummsWithVAT)
			{
				if (operationRow.CostSummWithVat >= 0m)
				{
					operationRow.CostSummWithVATDb = operationRow.CostSummWithVat;
				}
				else
				{
					operationRow.CostSummWithVATCr = operationRow.CostSummWithVat;
				}
			}
			if (ShowCurrSumms)
			{
				if (operationRow.CurrCostSumm >= 0m)
				{
					operationRow.CurrCostSummDb = operationRow.CurrCostSumm;
				}
				else
				{
					operationRow.CurrCostSummCr = operationRow.CurrCostSumm;
				}
			}
			if (ShowSalePrice)
			{
				_totalTurnSaleSum += operationRow.SaleSumm;
				_totalTurnSaleVATSum += operationRow.VATSaleSumm;
				_totalTurnSaleCurrSum += operationRow.SaleCurSumm;
			}
		}
		AddHeaderLines(TAccGoodDoc, TAccStorage);
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		AsRepRowCell asRepRowCell = new AsRepRowCell
		{
			ColumnSpan = (GroupByCorObject ? 2 : 4),
			Value = string.Format(ArmSoft.SME.Common.Resources.MTTAccCaptionUnit, ShowAltUnit ? TAccGoodDoc.AltUnitRow.Brief.Trim() : TAccGoodDoc.UnitRow.Brief.Trim()),
			FontWeight = FontWeights.Bold,
			FontSize = (int)Settings.GetFontSize() + 1
		};
		num += asRepRowCell.ColumnSpan;
		list.Add(asRepRowCell);
		asRepRowCell = new AsRepRowCell
		{
			ColumnSpan = 2,
			Value = ArmSoft.SME.Trade.Common.Resources.MTTAccQuantitative
		};
		asRepRowCell.Borders.Add(_isSelectedRows ? BorderType.BottomBorder : BorderType.None);
		asRepRowCell.Alignment = TextAlignment.Center;
		asRepRowCell.FontWeight = FontWeights.Bold;
		asRepRowCell.FontSize = (int)Settings.GetFontSize() + 1;
		num += asRepRowCell.ColumnSpan;
		list.Add(asRepRowCell);
		if (!GroupByCorObject)
		{
			asRepRowCell = new AsRepRowCell
			{
				ColumnSpan = 1,
				Value = ""
			};
			num += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
		}
		if (ShowSumms)
		{
			if (!GroupByCorObject)
			{
				asRepRowCell = new AsRepRowCell
				{
					ColumnSpan = 1,
					Value = ""
				};
				num += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
			asRepRowCell = new AsRepRowCell
			{
				ColumnSpan = 2,
				Value = ArmSoft.SME.Trade.Common.Resources.ShortCostSumma + " " + ArmSoft.SME.Trade.Common.Resources.WithoutNDS,
				Alignment = TextAlignment.Center
			};
			asRepRowCell.Borders.Add(_isSelectedRows ? BorderType.BottomBorder : BorderType.None);
			asRepRowCell.FontWeight = FontWeights.Bold;
			asRepRowCell.FontSize = (int)Settings.GetFontSize() + 1;
			num += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
			if (ShowSalePrice)
			{
				if (!GroupByCorObject)
				{
					asRepRowCell = new AsRepRowCell
					{
						ColumnSpan = 1,
						Value = ""
					};
					num += asRepRowCell.ColumnSpan;
					list.Add(asRepRowCell);
				}
				asRepRowCell = new AsRepRowCell
				{
					ColumnSpan = 1,
					Value = ""
				};
				num += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
		}
		if (ShowSummsWithVAT)
		{
			if (!GroupByCorObject)
			{
				asRepRowCell = new AsRepRowCell
				{
					ColumnSpan = 1,
					Value = ""
				};
				num += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
			asRepRowCell = new AsRepRowCell
			{
				ColumnSpan = 2,
				Value = ArmSoft.SME.Trade.Common.Resources.ShortCostSumma + " " + ArmSoft.SME.Trade.Common.Resources.WithNDS,
				Alignment = TextAlignment.Center
			};
			asRepRowCell.Borders.Add(_isSelectedRows ? BorderType.BottomBorder : BorderType.None);
			asRepRowCell.FontWeight = FontWeights.Bold;
			asRepRowCell.FontSize = (int)Settings.GetFontSize() + 1;
			num += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
			if (ShowSalePrice)
			{
				if (!GroupByCorObject)
				{
					asRepRowCell = new AsRepRowCell
					{
						ColumnSpan = 1,
						Value = ""
					};
					num += asRepRowCell.ColumnSpan;
					list.Add(asRepRowCell);
				}
				asRepRowCell = new AsRepRowCell
				{
					ColumnSpan = 1,
					Value = ""
				};
				num += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
		}
		if (ShowCurrSumms)
		{
			if (!GroupByCorObject)
			{
				asRepRowCell = new AsRepRowCell
				{
					ColumnSpan = 1,
					Value = ""
				};
				num += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
			asRepRowCell = new AsRepRowCell
			{
				ColumnSpan = 2,
				Value = string.Format(ArmSoft.SME.Trade.Common.Resources.ShortCostSummaCurr, Settings.ParametersService.MTCurr()),
				Alignment = TextAlignment.Center
			};
			asRepRowCell.Borders.Add(_isSelectedRows ? BorderType.BottomBorder : BorderType.None);
			asRepRowCell.FontWeight = FontWeights.Bold;
			asRepRowCell.FontSize = (int)Settings.GetFontSize() + 1;
			num += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
			if (ShowSalePrice)
			{
				if (!GroupByCorObject)
				{
					asRepRowCell = new AsRepRowCell
					{
						ColumnSpan = 1,
						Value = ""
					};
					num += asRepRowCell.ColumnSpan;
					list.Add(asRepRowCell);
				}
				asRepRowCell = new AsRepRowCell
				{
					ColumnSpan = 1,
					Value = ""
				};
				num += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
		}
		if (_listOfRepRepColumns.Count - num > 0)
		{
			asRepRowCell = new AsRepRowCell
			{
				ColumnSpan = _listOfRepRepColumns.Count - num,
				Value = ""
			};
			list.Add(asRepRowCell);
		}
		ReportPanel.AddTableSpanRow("MTTAccount", list);
		ReportPanel.UnmovableHeaderLinesCount += ((!_isSelectedRows) ? 1 : 0);
		num = 0;
		list = new List<AsRepRowCell>();
		asRepRowCell = new SimpleCurrencyRepRowCell();
		asRepRowCell.ColumnSpan = (GroupByCorObject ? 2 : 4);
		asRepRowCell.Value = string.Format(ArmSoft.SME.Common.Resources.RemInStartDay, DateExtension.DateToSymbol(StartDate.ToDateTime(), shortMode: false, isPrintMode: false));
		asRepRowCell.Alignment = TextAlignment.Left;
		num += asRepRowCell.ColumnSpan;
		list.Add(asRepRowCell);
		num2 = dataRow.QtyBegin;
		bool flag = num2 >= 0m;
		asRepRowCell = new SimpleCurrencyRepRowCell
		{
			Value = (flag ? ((object)num2) : "")
		};
		asRepRowCell.Borders.Add(BorderType.None);
		num += asRepRowCell.ColumnSpan;
		list.Add(asRepRowCell);
		asRepRowCell = new SimpleCurrencyRepRowCell
		{
			Value = (flag ? "" : ((object)num2))
		};
		asRepRowCell.Borders.Add(BorderType.None);
		num += asRepRowCell.ColumnSpan;
		list.Add(asRepRowCell);
		if (!GroupByCorObject)
		{
			asRepRowCell = new AsRepRowCell
			{
				ColumnSpan = 1,
				Value = ""
			};
			num += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
		}
		if (ShowSumms)
		{
			if (!GroupByCorObject)
			{
				asRepRowCell = new AsRepRowCell
				{
					ColumnSpan = 1,
					Value = ""
				};
				num += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
			num2 = dataRow.SumBegin;
			flag = num2 >= 0m;
			asRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = (flag ? ((object)num2) : "")
			};
			asRepRowCell.Borders.Add(BorderType.None);
			num += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
			asRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = (flag ? "" : ((object)num2))
			};
			asRepRowCell.Borders.Add(BorderType.None);
			num += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
			if (ShowSalePrice)
			{
				if (!GroupByCorObject)
				{
					asRepRowCell = new AsRepRowCell
					{
						ColumnSpan = 1,
						Value = ""
					};
					num += asRepRowCell.ColumnSpan;
					list.Add(asRepRowCell);
				}
				asRepRowCell = new SimpleCurrencyRepRowCell
				{
					ColumnSpan = 1,
					Value = dataRow.SaleSumBegin
				};
				asRepRowCell.Borders.Add(BorderType.None);
				num += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
		}
		if (ShowSummsWithVAT)
		{
			if (!GroupByCorObject)
			{
				asRepRowCell = new AsRepRowCell
				{
					ColumnSpan = 1,
					Value = ""
				};
				num += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
			num2 = dataRow.SumVATBegin;
			flag = num2 >= 0m;
			asRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = (flag ? ((object)num2) : "")
			};
			asRepRowCell.Borders.Add(BorderType.None);
			num += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
			asRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = (flag ? "" : ((object)num2))
			};
			asRepRowCell.Borders.Add(BorderType.None);
			num += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
			if (ShowSalePrice)
			{
				if (!GroupByCorObject)
				{
					asRepRowCell = new AsRepRowCell
					{
						ColumnSpan = 1,
						Value = ""
					};
					num += asRepRowCell.ColumnSpan;
					list.Add(asRepRowCell);
				}
				asRepRowCell = new SimpleCurrencyRepRowCell
				{
					ColumnSpan = 1,
					Value = dataRow.SaleSumVATBegin
				};
				asRepRowCell.Borders.Add(BorderType.None);
				num += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
		}
		if (ShowCurrSumms)
		{
			if (!GroupByCorObject)
			{
				asRepRowCell = new AsRepRowCell
				{
					ColumnSpan = 1,
					Value = ""
				};
				num += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
			num2 = dataRow.SumCurrBegin;
			flag = num2 >= 0m;
			asRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = (flag ? ((object)num2) : "")
			};
			asRepRowCell.Borders.Add(BorderType.None);
			num += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
			asRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = (flag ? "" : ((object)num2))
			};
			asRepRowCell.Borders.Add(BorderType.None);
			num += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
			if (ShowSalePrice)
			{
				if (!GroupByCorObject)
				{
					asRepRowCell = new AsRepRowCell
					{
						ColumnSpan = 1,
						Value = ""
					};
					num += asRepRowCell.ColumnSpan;
					list.Add(asRepRowCell);
				}
				asRepRowCell = new SimpleCurrencyRepRowCell
				{
					ColumnSpan = 1,
					Value = dataRow.SaleSumCurrBegin
				};
				asRepRowCell.Borders.Add(BorderType.None);
				num += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
		}
		if (_listOfRepRepColumns.Count - num > 0)
		{
			asRepRowCell = new AsRepRowCell
			{
				ColumnSpan = _listOfRepRepColumns.Count - num,
				Value = ""
			};
			list.Add(asRepRowCell);
		}
		ReportPanel.AddTableSpanRow("MTTAccount", list);
		ReportPanel.AddTableColumnsHeader("MTTAccount");
		if (_reportResult[key].OperationRows == null || _reportResult[key].OperationRows.Count == 0)
		{
			asRepRowCell = new AsRepRowCell
			{
				ColumnSpan = _listOfRepRepColumns.Count,
				Value = ""
			};
			int num10 = 1;
			List<AsRepRowCell> list2 = new List<AsRepRowCell>(num10);
			CollectionsMarshal.SetCount(list2, num10);
			CollectionsMarshal.AsSpan(list2)[0] = asRepRowCell;
			list = list2;
			ReportPanel.AddTableSpanRow("MTTAccount", list);
			ReportPanel.AddString("");
		}
		else
		{
			if (ShowEachDayRems)
			{
				dateOnly = _reportResult[key].OperationRows[0].OpDate;
				num3 = dataRow.QtyBegin;
				num4 = dataRow.SumBegin;
				num5 = dataRow.SumVATBegin;
				num6 = dataRow.SumCurrBegin;
				if (ShowSalePrice)
				{
					num7 = dataRow.SaleSumBegin;
					num8 = dataRow.SaleSumVATBegin;
					num9 = dataRow.SaleSumCurrBegin;
				}
			}
			if (GroupByCorObject)
			{
				foreach (GoodsOperationsDataProvider.DataRow item in from row in _reportResult[key].OperationRows
					group row by new { row.CorrObjectCode, row.CorrObjectCaption, row.GoodCode } into opRow
					select new GoodsOperationsDataProvider.DataRow
					{
						CorrObjectCode = opRow.Key.CorrObjectCode,
						CorrObjectCaption = opRow.Key.CorrObjectCaption,
						GoodCode = opRow.Key.GoodCode,
						QuantityDb = opRow.Sum((MTAccountReportResult.OperationRow p) => p.QuantityDb),
						CostSummDb = opRow.Sum((MTAccountReportResult.OperationRow p) => p.CostSummDb),
						CostSummWithVATDb = opRow.Sum((MTAccountReportResult.OperationRow p) => p.CostSummWithVATDb),
						CurrCostSummDb = opRow.Sum((MTAccountReportResult.OperationRow p) => p.CurrCostSummDb),
						QuantityCr = opRow.Sum((MTAccountReportResult.OperationRow p) => p.QuantityCr),
						CostSummCr = opRow.Sum((MTAccountReportResult.OperationRow p) => p.CostSummCr),
						CostSummWithVATCr = opRow.Sum((MTAccountReportResult.OperationRow p) => p.CostSummWithVATCr),
						CurrCostSummCr = opRow.Sum((MTAccountReportResult.OperationRow p) => p.CurrCostSummCr),
						SaleSumm = opRow.Sum((MTAccountReportResult.OperationRow p) => p.SaleSumm),
						VATSaleSumm = opRow.Sum((MTAccountReportResult.OperationRow p) => p.VATSaleSumm),
						SaleCurSumm = opRow.Sum((MTAccountReportResult.OperationRow p) => p.SaleCurSumm)
					})
				{
					int num10 = 4;
					List<object> list3 = new List<object>(num10);
					CollectionsMarshal.SetCount(list3, num10);
					Span<object> span = CollectionsMarshal.AsSpan(list3);
					span[0] = item.CorrObjectCode;
					span[1] = item.CorrObjectCaption;
					span[2] = item.QuantityDb;
					span[3] = -item.QuantityCr;
					List<object> list4 = list3;
					if (ShowSumms)
					{
						list4.Add(item.CostSummDb);
						list4.Add(-item.CostSummCr);
						if (ShowSalePrice)
						{
							list4.Add(item.SaleSumm);
						}
					}
					if (ShowSummsWithVAT)
					{
						list4.Add(item.CostSummWithVATDb);
						list4.Add(-item.CostSummWithVATCr);
						if (ShowSalePrice)
						{
							list4.Add(item.VATSaleSumm);
						}
					}
					if (ShowCurrSumms)
					{
						list4.Add(item.CurrCostSummDb);
						list4.Add(-item.CurrCostSummCr);
						if (ShowSalePrice)
						{
							list4.Add(item.SaleCurSumm);
						}
					}
					ReportPanel.AddTableRow("MTTAccount", list4, item);
				}
			}
			else
			{
				foreach (MTAccountReportResult.OperationRow operationRow2 in _reportResult[key].OperationRows)
				{
					if (ShowEachDayRems)
					{
						if (dateOnly < operationRow2.OpDate)
						{
							List<AsRepRowCell> list5 = new List<AsRepRowCell>();
							asRepRowCell = new ShowDailyRemsCurrencyRepRowCell
							{
								ColumnSpan = 4,
								Value = string.Format(ArmSoft.SME.Common.Resources.RemInEndDay, dateOnly.FormatDDMMYY())
							};
							list5.Add(asRepRowCell);
							asRepRowCell = new ShowDailyRemsCurrencyRepRowCell
							{
								Value = ((num3 >= 0m) ? ((object)num3) : "")
							};
							list5.Add(asRepRowCell);
							asRepRowCell = new ShowDailyRemsCurrencyRepRowCell
							{
								Value = ((num3 >= 0m) ? "" : ((object)Math.Abs(num3)))
							};
							list5.Add(asRepRowCell);
							asRepRowCell = new ShowDailyRemsCurrencyRepRowCell
							{
								ColumnSpan = 1,
								Value = ""
							};
							list5.Add(asRepRowCell);
							num = 7;
							if (ShowSumms)
							{
								asRepRowCell = new AsRepRowCell
								{
									ColumnSpan = 1,
									Value = ""
								};
								num += asRepRowCell.ColumnSpan;
								list5.Add(asRepRowCell);
								num2 = dataRow.SumBegin;
								flag = num2 >= 0m;
								asRepRowCell = new ShowDailyRemsCurrencyRepRowCell
								{
									Value = ((num4 >= 0m) ? ((object)num4) : "")
								};
								num += asRepRowCell.ColumnSpan;
								list5.Add(asRepRowCell);
								asRepRowCell = new ShowDailyRemsCurrencyRepRowCell
								{
									Value = ((num4 >= 0m) ? "" : ((object)Math.Abs(num4)))
								};
								num += asRepRowCell.ColumnSpan;
								list5.Add(asRepRowCell);
								if (ShowSalePrice)
								{
									asRepRowCell = new AsRepRowCell
									{
										ColumnSpan = 1,
										Value = ""
									};
									num += asRepRowCell.ColumnSpan;
									list5.Add(asRepRowCell);
									asRepRowCell = new ShowDailyRemsCurrencyRepRowCell
									{
										ColumnSpan = 1,
										Value = num7
									};
									num += asRepRowCell.ColumnSpan;
									list5.Add(asRepRowCell);
								}
							}
							if (ShowSummsWithVAT)
							{
								asRepRowCell = new AsRepRowCell
								{
									ColumnSpan = 1,
									Value = ""
								};
								num += asRepRowCell.ColumnSpan;
								list5.Add(asRepRowCell);
								num2 = dataRow.SumBegin;
								flag = num2 >= 0m;
								asRepRowCell = new ShowDailyRemsCurrencyRepRowCell
								{
									Value = ((num5 >= 0m) ? ((object)num5) : "")
								};
								num += asRepRowCell.ColumnSpan;
								list5.Add(asRepRowCell);
								asRepRowCell = new ShowDailyRemsCurrencyRepRowCell
								{
									Value = ((num5 >= 0m) ? "" : ((object)Math.Abs(num5)))
								};
								num += asRepRowCell.ColumnSpan;
								list5.Add(asRepRowCell);
								if (ShowSalePrice)
								{
									asRepRowCell = new AsRepRowCell
									{
										ColumnSpan = 1,
										Value = ""
									};
									num += asRepRowCell.ColumnSpan;
									list5.Add(asRepRowCell);
									asRepRowCell = new ShowDailyRemsCurrencyRepRowCell
									{
										ColumnSpan = 1,
										Value = num8
									};
									num += asRepRowCell.ColumnSpan;
									list5.Add(asRepRowCell);
								}
							}
							if (ShowCurrSumms)
							{
								asRepRowCell = new AsRepRowCell
								{
									ColumnSpan = 1,
									Value = ""
								};
								num += asRepRowCell.ColumnSpan;
								list5.Add(asRepRowCell);
								num2 = dataRow.SumBegin;
								flag = num2 >= 0m;
								asRepRowCell = new ShowDailyRemsCurrencyRepRowCell
								{
									Value = ((num6 >= 0m) ? ((object)num6) : "")
								};
								num += asRepRowCell.ColumnSpan;
								list5.Add(asRepRowCell);
								asRepRowCell = new ShowDailyRemsCurrencyRepRowCell
								{
									Value = ((num6 >= 0m) ? "" : ((object)Math.Abs(num6)))
								};
								num += asRepRowCell.ColumnSpan;
								list5.Add(asRepRowCell);
								if (ShowSalePrice)
								{
									asRepRowCell = new AsRepRowCell
									{
										ColumnSpan = 1,
										Value = ""
									};
									num += asRepRowCell.ColumnSpan;
									list5.Add(asRepRowCell);
									asRepRowCell = new ShowDailyRemsCurrencyRepRowCell
									{
										ColumnSpan = 1,
										Value = num9
									};
									num += asRepRowCell.ColumnSpan;
									list5.Add(asRepRowCell);
								}
							}
							if (_listOfRepRepColumns.Count - num > 0)
							{
								asRepRowCell = new ShowDailyRemsCurrencyRepRowCell
								{
									ColumnSpan = 2,
									Value = ""
								};
								list5.Add(asRepRowCell);
							}
							ReportPanel.AddTableSpanRow("MTTAccount", list5, AsRepRowStyle.TableCaption);
							dateOnly = operationRow2.OpDate;
						}
						num3 += operationRow2.Quantity;
						num4 += operationRow2.CostSumm;
						num5 += operationRow2.CostSummWithVat;
						num6 += operationRow2.CurrCostSumm;
						if (ShowSalePrice)
						{
							num7 += operationRow2.SaleSumm;
							num8 += operationRow2.VATSaleSumm;
							num9 += operationRow2.SaleCurSumm;
						}
					}
					int num10 = 4;
					List<object> list6 = new List<object>(num10);
					CollectionsMarshal.SetCount(list6, num10);
					Span<object> span2 = CollectionsMarshal.AsSpan(list6);
					span2[0] = operationRow2.OpDate.FormatDDMMYY();
					span2[1] = operationRow2.CorrObjectCode;
					span2[2] = operationRow2.CorrObjectCaption;
					span2[3] = operationRow2.Storage;
					List<object> list4 = list6;
					if (operationRow2.Quantity >= 0m)
					{
						list4.Add(operationRow2.Quantity);
						list4.Add("");
					}
					else
					{
						list4.Add("");
						list4.Add(operationRow2.Quantity * -1m);
					}
					list4.Add(operationRow2.DocNum);
					if (ShowSumms)
					{
						list4.Add(operationRow2.Cost);
						if (operationRow2.CostSumm >= 0m)
						{
							list4.Add(operationRow2.CostSumm);
							list4.Add("");
						}
						else
						{
							list4.Add("");
							list4.Add(operationRow2.CostSumm * -1m);
						}
						if (ShowSalePrice)
						{
							if (operationRow2.SalePrice >= 0m)
							{
								list4.Add(operationRow2.SalePrice);
							}
							else
							{
								list4.Add(operationRow2.SalePrice * -1m);
							}
							list4.Add(operationRow2.SaleSumm);
						}
					}
					if (ShowSummsWithVAT)
					{
						list4.Add(operationRow2.CostWithVat);
						if (operationRow2.CostSummWithVat >= 0m)
						{
							list4.Add(operationRow2.CostSummWithVat);
							list4.Add("");
						}
						else
						{
							list4.Add("");
							list4.Add(operationRow2.CostSummWithVat * -1m);
						}
						if (ShowSalePrice)
						{
							if (operationRow2.VATSalePrice >= 0m)
							{
								list4.Add(operationRow2.VATSalePrice);
							}
							else
							{
								list4.Add(operationRow2.VATSalePrice * -1m);
							}
							list4.Add(operationRow2.VATSaleSumm);
						}
					}
					if (ShowCurrSumms)
					{
						list4.Add(operationRow2.CurrCost);
						if (operationRow2.CurrCostSumm >= 0m)
						{
							list4.Add(operationRow2.CurrCostSumm);
							list4.Add("");
						}
						else
						{
							list4.Add("");
							list4.Add(operationRow2.CurrCostSumm * -1m);
						}
						if (ShowSalePrice)
						{
							if (operationRow2.SaleCurPrice >= 0m)
							{
								list4.Add(operationRow2.SaleCurPrice);
							}
							else
							{
								list4.Add(operationRow2.SaleCurPrice * -1m);
							}
							list4.Add(operationRow2.SaleCurSumm);
						}
					}
					list4.Add(operationRow2.Comment);
					list4.Add(operationRow2.DocTypeCaption);
					if (ShowParties)
					{
						if (_showPartySupplier)
						{
							list4.Add(operationRow2.PartyPartCaption);
						}
						if (_showPartyContract)
						{
							list4.Add(operationRow2.PartyContractCaption);
						}
						if (_showPartyDocType && SmartEnum<DocumentTypeSE, short>.TryFromValue(operationRow2.PartyDocType, out var result))
						{
							list4.Add(result.Caption);
						}
						if (_showPartyDocNum)
						{
							list4.Add(operationRow2.PartyDocNum);
						}
						if (_showPartyUseableDate)
						{
							if (!operationRow2.PartyDocUsableDate.HasValue)
							{
								list4.Add("");
							}
							else
							{
								list4.Add(operationRow2.PartyDocUsableDate);
							}
						}
						if (_showPartyIncomeDate)
						{
							list4.Add(operationRow2.IncomeDate);
						}
						if (_showPartyCountry)
						{
							list4.Add(operationRow2.PartyDocCountry);
						}
						if (_showPartyProducer)
						{
							list4.Add(operationRow2.PartyDocProducer);
						}
						if (_showPartyProperty1)
						{
							list4.Add(operationRow2.PartyDocPropertyType1);
							list4.Add(operationRow2.PartyDocPropertyValue1);
						}
						if (_showPartyProperty2)
						{
							list4.Add(operationRow2.PartyDocPropertyType2);
							list4.Add(operationRow2.PartyDocPropertyValue2);
						}
					}
					ReportPanel.AddTableRow("MTTAccount", list4, operationRow2);
				}
			}
		}
		AddTotalTurn(dataRow);
		AddFinalRemain(dataRow);
	}

	private void AddTotalTurn(GoodsTurnOverDataProvider.DataRow MTreportResult)
	{
		int num = 0;
		ReportPanel.UnmovableFooterLinesCount = ((!_isSelectedRows) ? 2 : 0);
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		SimpleCurrencyRepRowCell simpleCurrencyRepRowCell = new SimpleCurrencyRepRowCell
		{
			ColumnSpan = (GroupByCorObject ? 2 : 4),
			Value = ArmSoft.SME.Common.Resources.TheTurn,
			Alignment = TextAlignment.Right
		};
		num += simpleCurrencyRepRowCell.ColumnSpan;
		list.Add(simpleCurrencyRepRowCell);
		simpleCurrencyRepRowCell = new SimpleCurrencyRepRowCell
		{
			Value = MTreportResult.QtyIn
		};
		simpleCurrencyRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
		simpleCurrencyRepRowCell.Borders.Add(_isSelectedRows ? BorderType.BottomBorder : BorderType.None);
		num += simpleCurrencyRepRowCell.ColumnSpan;
		list.Add(simpleCurrencyRepRowCell);
		simpleCurrencyRepRowCell = new SimpleCurrencyRepRowCell
		{
			Value = MTreportResult.QtyOut
		};
		simpleCurrencyRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
		simpleCurrencyRepRowCell.Borders.Add(_isSelectedRows ? BorderType.BottomBorder : BorderType.None);
		num += simpleCurrencyRepRowCell.ColumnSpan;
		list.Add(simpleCurrencyRepRowCell);
		if (!GroupByCorObject)
		{
			AsRepRowCell asRepRowCell = new AsRepRowCell
			{
				ColumnSpan = 1,
				Value = ""
			};
			num += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
		}
		if (ShowSumms)
		{
			if (!GroupByCorObject)
			{
				AsRepRowCell asRepRowCell2 = new AsRepRowCell
				{
					ColumnSpan = 1,
					Value = ""
				};
				num += asRepRowCell2.ColumnSpan;
				list.Add(asRepRowCell2);
			}
			simpleCurrencyRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = MTreportResult.SumIn
			};
			simpleCurrencyRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
			simpleCurrencyRepRowCell.Borders.Add(_isSelectedRows ? BorderType.BottomBorder : BorderType.None);
			num += simpleCurrencyRepRowCell.ColumnSpan;
			list.Add(simpleCurrencyRepRowCell);
			simpleCurrencyRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = MTreportResult.SumOut
			};
			simpleCurrencyRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
			simpleCurrencyRepRowCell.Borders.Add(_isSelectedRows ? BorderType.BottomBorder : BorderType.None);
			num += simpleCurrencyRepRowCell.ColumnSpan;
			list.Add(simpleCurrencyRepRowCell);
			if (ShowSalePrice)
			{
				if (!GroupByCorObject)
				{
					AsRepRowCell asRepRowCell3 = new AsRepRowCell
					{
						ColumnSpan = 1,
						Value = ""
					};
					num += asRepRowCell3.ColumnSpan;
					list.Add(asRepRowCell3);
				}
				AsRepRowCell asRepRowCell4 = new AsRepRowCell
				{
					ColumnSpan = 1,
					Value = ""
				};
				num += asRepRowCell4.ColumnSpan;
				list.Add(asRepRowCell4);
			}
		}
		if (ShowSummsWithVAT)
		{
			if (!GroupByCorObject)
			{
				AsRepRowCell asRepRowCell5 = new AsRepRowCell
				{
					ColumnSpan = 1,
					Value = ""
				};
				num += asRepRowCell5.ColumnSpan;
				list.Add(asRepRowCell5);
			}
			simpleCurrencyRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = MTreportResult.SumVATIn
			};
			simpleCurrencyRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
			simpleCurrencyRepRowCell.Borders.Add(_isSelectedRows ? BorderType.BottomBorder : BorderType.None);
			num += simpleCurrencyRepRowCell.ColumnSpan;
			list.Add(simpleCurrencyRepRowCell);
			simpleCurrencyRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = MTreportResult.SumVATOut
			};
			simpleCurrencyRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
			simpleCurrencyRepRowCell.Borders.Add(_isSelectedRows ? BorderType.BottomBorder : BorderType.None);
			num += simpleCurrencyRepRowCell.ColumnSpan;
			list.Add(simpleCurrencyRepRowCell);
			if (ShowSalePrice)
			{
				if (!GroupByCorObject)
				{
					AsRepRowCell asRepRowCell6 = new AsRepRowCell
					{
						ColumnSpan = 1,
						Value = ""
					};
					num += asRepRowCell6.ColumnSpan;
					list.Add(asRepRowCell6);
				}
				AsRepRowCell asRepRowCell7 = new AsRepRowCell
				{
					ColumnSpan = 1,
					Value = ""
				};
				num += asRepRowCell7.ColumnSpan;
				list.Add(asRepRowCell7);
			}
		}
		if (ShowCurrSumms)
		{
			if (!GroupByCorObject)
			{
				AsRepRowCell asRepRowCell8 = new AsRepRowCell();
				asRepRowCell8.ColumnSpan = 1;
				asRepRowCell8.Value = "";
				num += asRepRowCell8.ColumnSpan;
				list.Add(asRepRowCell8);
			}
			simpleCurrencyRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = MTreportResult.SumCurrIn
			};
			simpleCurrencyRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
			simpleCurrencyRepRowCell.Borders.Add(_isSelectedRows ? BorderType.BottomBorder : BorderType.None);
			num += simpleCurrencyRepRowCell.ColumnSpan;
			list.Add(simpleCurrencyRepRowCell);
			simpleCurrencyRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = MTreportResult.SumCurrOut
			};
			simpleCurrencyRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
			simpleCurrencyRepRowCell.Borders.Add(_isSelectedRows ? BorderType.BottomBorder : BorderType.None);
			num += simpleCurrencyRepRowCell.ColumnSpan;
			list.Add(simpleCurrencyRepRowCell);
			if (ShowSalePrice)
			{
				if (!GroupByCorObject)
				{
					AsRepRowCell asRepRowCell9 = new AsRepRowCell
					{
						ColumnSpan = 1,
						Value = ""
					};
					num += asRepRowCell9.ColumnSpan;
					list.Add(asRepRowCell9);
				}
				AsRepRowCell asRepRowCell10 = new AsRepRowCell
				{
					ColumnSpan = 1,
					Value = ""
				};
				num += asRepRowCell10.ColumnSpan;
				list.Add(asRepRowCell10);
			}
		}
		if (_listOfRepRepColumns.Count - num > 0)
		{
			AsRepRowCell item = new AsRepRowCell
			{
				ColumnSpan = _listOfRepRepColumns.Count - num,
				Value = ""
			};
			list.Add(item);
		}
		ReportPanel.AddTableSpanRow("MTTAccount", list);
	}

	private void AddFinalRemain(GoodsTurnOverDataProvider.DataRow MTreportResult)
	{
		decimal num = 0m;
		int num2 = 0;
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		AsRepRowCell asRepRowCell = new SimpleCurrencyRepRowCell
		{
			ColumnSpan = (GroupByCorObject ? 2 : 4),
			Value = string.Format(ArmSoft.SME.Common.Resources.RemInEndDay, DateExtension.DateToSymbol(EndDate.ToDateTime(), shortMode: false, isPrintMode: false))
		};
		num2 += asRepRowCell.ColumnSpan;
		asRepRowCell.Alignment = TextAlignment.Right;
		list.Add(asRepRowCell);
		num = MTreportResult.QtyEnd;
		bool flag = num >= 0m;
		asRepRowCell = new SimpleCurrencyRepRowCell
		{
			Value = (flag ? ((object)num) : "")
		};
		asRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
		num2 += asRepRowCell.ColumnSpan;
		list.Add(asRepRowCell);
		asRepRowCell = new SimpleCurrencyRepRowCell
		{
			Value = (flag ? "" : ((object)num))
		};
		asRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
		num2 += asRepRowCell.ColumnSpan;
		list.Add(asRepRowCell);
		if (!GroupByCorObject)
		{
			asRepRowCell = new AsRepRowCell
			{
				ColumnSpan = 1,
				Value = ""
			};
			num2 += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
		}
		if (ShowSumms)
		{
			if (!GroupByCorObject)
			{
				asRepRowCell = new AsRepRowCell
				{
					ColumnSpan = 1,
					Value = ""
				};
				num2 += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
			num = MTreportResult.SumEnd;
			flag = num >= 0m;
			asRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = (flag ? ((object)num) : "")
			};
			asRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
			num2 += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
			asRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = (flag ? "" : ((object)num))
			};
			asRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
			num2 += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
			if (ShowSalePrice)
			{
				if (!GroupByCorObject)
				{
					asRepRowCell = new AsRepRowCell
					{
						ColumnSpan = 1,
						Value = ""
					};
					num2 += asRepRowCell.ColumnSpan;
					list.Add(asRepRowCell);
				}
				asRepRowCell = new SimpleCurrencyRepRowCell
				{
					ColumnSpan = 1,
					Value = MTreportResult.SaleSumBegin + _totalTurnSaleSum
				};
				asRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
				num2 += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
		}
		if (ShowSummsWithVAT)
		{
			if (!GroupByCorObject)
			{
				asRepRowCell = new AsRepRowCell
				{
					ColumnSpan = 1,
					Value = ""
				};
				num2 += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
			num = MTreportResult.SumVATEnd;
			flag = num >= 0m;
			asRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = (flag ? ((object)num) : "")
			};
			asRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
			num2 += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
			asRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = (flag ? "" : ((object)num))
			};
			asRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
			num2 += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
			if (ShowSalePrice)
			{
				if (!GroupByCorObject)
				{
					asRepRowCell = new AsRepRowCell
					{
						ColumnSpan = 1,
						Value = ""
					};
					num2 += asRepRowCell.ColumnSpan;
					list.Add(asRepRowCell);
				}
				asRepRowCell = new SimpleCurrencyRepRowCell
				{
					ColumnSpan = 1,
					Value = MTreportResult.SaleSumVATBegin + _totalTurnSaleVATSum
				};
				asRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
				num2 += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
		}
		if (ShowCurrSumms)
		{
			if (!GroupByCorObject)
			{
				asRepRowCell = new AsRepRowCell
				{
					ColumnSpan = 1,
					Value = ""
				};
				num2 += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
			num = MTreportResult.SumCurrEnd;
			flag = num >= 0m;
			asRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = (flag ? ((object)num) : "")
			};
			asRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
			num2 += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
			asRepRowCell = new SimpleCurrencyRepRowCell
			{
				Value = (flag ? "" : ((object)num))
			};
			asRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
			num2 += asRepRowCell.ColumnSpan;
			list.Add(asRepRowCell);
			if (ShowSalePrice)
			{
				if (!GroupByCorObject)
				{
					asRepRowCell = new AsRepRowCell
					{
						ColumnSpan = 1,
						Value = ""
					};
					num2 += asRepRowCell.ColumnSpan;
					list.Add(asRepRowCell);
				}
				asRepRowCell = new SimpleCurrencyRepRowCell
				{
					ColumnSpan = 1,
					Value = MTreportResult.SaleSumCurrBegin + _totalTurnSaleCurrSum
				};
				asRepRowCell.Borders.Add(_isSelectedRows ? BorderType.TopBorder : BorderType.None);
				num2 += asRepRowCell.ColumnSpan;
				list.Add(asRepRowCell);
			}
		}
		if (_listOfRepRepColumns.Count - num2 > 0)
		{
			asRepRowCell = new AsRepRowCell
			{
				ColumnSpan = _listOfRepRepColumns.Count - num2,
				Value = ""
			};
			list.Add(asRepRowCell);
		}
		ReportPanel.AddTableSpanRow("MTTAccount", list);
	}

	private void AddHeaderLines(MaterialCacheRow TAccGoodDoc, string TAccStorage)
	{
		short num = (short)(GroupByCorObject ? 4 : 7);
		ReportPanel.UnmovableHeaderLinesCount = ((!_isSelectedRows) ? 2 : 0);
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		AsRepRowCell item = new AsRepRowCell
		{
			ColumnSpan = num,
			Value = string.Format(ArmSoft.SME.Trade.Common.Resources.MTTAccCaptionMaterial, TAccGoodDoc.Caption.Trim(), TAccGoodDoc.MTCode.Trim()),
			Alignment = TextAlignment.Center,
			FontWeight = FontWeights.Bold,
			FontSize = (int)Settings.GetFontSize() + 4
		};
		list.Add(item);
		if (GetListOfRepColumns().Count - num > 0)
		{
			item = new AsRepRowCell
			{
				ColumnSpan = _listOfRepRepColumns.Count - num,
				Value = ""
			};
			list.Add(item);
		}
		ReportPanel.AddTableSpanRow("MTTAccount", list);
		ReportPanel.UnmovableHeaderLinesCount += ((!_isSelectedRows) ? 1 : 0);
		list = new List<AsRepRowCell>();
		item = new AsRepRowCell
		{
			ColumnSpan = num
		};
		if (string.IsNullOrWhiteSpace(TAccStorage))
		{
			item.Value = ArmSoft.SME.Trade.Common.Resources.MTTAccByAllStorages;
		}
		else if (TAccStorage.Split(",").Length == 1)
		{
			item.Value = string.Format(ArmSoft.SME.Trade.Common.Resources.MTTAccByTheStorage, TAccStorage.Trim(), TradeSettings.TradeCacheService.Storage.ItemsWithKeys.ContainsKey(TAccStorage) ? TradeSettings.TradeCacheService.Storage.ItemsWithKeys[TAccStorage].Caption : "");
		}
		else
		{
			item.Value = ArmSoft.SME.Common.Resources.Storages + " - " + TAccStorage;
		}
		item.Alignment = TextAlignment.Center;
		item.FontWeight = FontWeights.Bold;
		item.FontSize = (int)Settings.GetFontSize() + 3;
		list.Add(item);
		if (GetListOfRepColumns().Count - num > 0)
		{
			item = new AsRepRowCell
			{
				ColumnSpan = GetListOfRepColumns().Count - num,
				Value = ""
			};
			list.Add(item);
		}
		ReportPanel.AddTableSpanRow("MTTAccount", list);
		ReportPanel.UnmovableHeaderLinesCount += ((!_isSelectedRows) ? 1 : 0);
	}

	private List<AsRepColumn> GetListOfRepColumns()
	{
		List<AsRepColumn> list = new List<AsRepColumn>();
		AsRepColumn item;
		if (!GroupByCorObject)
		{
			item = new AsRepColumn(ArmSoft.SME.Common.Resources.DateWord, 120.0)
			{
				ValuesStyle = AsRepColStyle.DateStyle,
				CaptionAlignment = TextAlignment.Left,
				DataAlignment = TextAlignment.Left
			};
			list.Add(item);
		}
		item = new AsRepColumn(ArmSoft.SME.Common.Resources.Code, 200.0);
		list.Add(item);
		item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.MTTAccCorObjectName, 150.0);
		list.Add(item);
		if (!GroupByCorObject)
		{
			item = new AsRepColumn(ArmSoft.SME.Common.Resources.Storage, 105.0);
			list.Add(item);
		}
		item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.MTIn, 160.0)
		{
			ValuesStyle = AsRepColStyle.CurrencyStyle,
			CaptionAlignment = TextAlignment.Right,
			DataBorders = new ObservableCollection<BorderType> { BorderType.RightBorder },
			CurrencyPrecision = 3
		};
		list.Add(item);
		item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.MTOut, 160.0)
		{
			ValuesStyle = AsRepColStyle.CurrencyStyle,
			CaptionAlignment = TextAlignment.Right,
			DataBorders = new ObservableCollection<BorderType> { BorderType.LeftBorder },
			CurrencyPrecision = 3
		};
		list.Add(item);
		if (!GroupByCorObject)
		{
			item = new AsRepColumn(ArmSoft.SME.Common.Resources.DocNumber, 120.0);
			list.Add(item);
		}
		if (ShowSumms)
		{
			if (!GroupByCorObject)
			{
				item = new AsRepColumn(ArmSoft.SME.Common.Resources.Price, 160.0)
				{
					ValuesStyle = AsRepColStyle.CurrencyStyle,
					CaptionAlignment = TextAlignment.Right,
					CurrencyPrecision = 2
				};
				list.Add(item);
			}
			item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.MTIn, 160.0)
			{
				ValuesStyle = AsRepColStyle.CurrencyStyle,
				CaptionAlignment = TextAlignment.Right,
				DataBorders = new ObservableCollection<BorderType> { BorderType.RightBorder }
			};
			list.Add(item);
			item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.MTOut, 160.0)
			{
				ValuesStyle = AsRepColStyle.CurrencyStyle,
				CaptionAlignment = TextAlignment.Right,
				DataBorders = new ObservableCollection<BorderType> { BorderType.LeftBorder }
			};
			list.Add(item);
			if (ShowSalePrice)
			{
				if (!GroupByCorObject)
				{
					item = new AsRepColumn(ArmSoft.SME.Common.Resources.MTTAccSalePrice, 160.0)
					{
						ValuesStyle = AsRepColStyle.CurrencyStyle,
						CaptionAlignment = TextAlignment.Right,
						CurrencyPrecision = 2
					};
					list.Add(item);
				}
				item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.MatRepSaleTotalSum, 160.0)
				{
					ValuesStyle = AsRepColStyle.CurrencyStyle,
					CaptionAlignment = TextAlignment.Right
				};
				list.Add(item);
			}
		}
		if (ShowSummsWithVAT)
		{
			if (!GroupByCorObject)
			{
				item = new AsRepColumn(ArmSoft.SME.Common.Resources.Price, 160.0)
				{
					ValuesStyle = AsRepColStyle.CurrencyStyle,
					CaptionAlignment = TextAlignment.Right,
					CurrencyPrecision = 2
				};
				list.Add(item);
			}
			item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.MTIn, 160.0)
			{
				ValuesStyle = AsRepColStyle.CurrencyStyle,
				CaptionAlignment = TextAlignment.Right,
				DataBorders = new ObservableCollection<BorderType> { BorderType.RightBorder }
			};
			list.Add(item);
			item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.MTOut, 160.0)
			{
				ValuesStyle = AsRepColStyle.CurrencyStyle,
				CaptionAlignment = TextAlignment.Right,
				DataBorders = new ObservableCollection<BorderType> { BorderType.LeftBorder }
			};
			list.Add(item);
			if (ShowSalePrice)
			{
				if (!GroupByCorObject)
				{
					item = new AsRepColumn(ArmSoft.SME.Common.Resources.MTTAccSalePrice, 160.0)
					{
						ValuesStyle = AsRepColStyle.CurrencyStyle,
						CaptionAlignment = TextAlignment.Right,
						CurrencyPrecision = 2
					};
					list.Add(item);
				}
				item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.MatRepSaleTotalSum, 160.0)
				{
					ValuesStyle = AsRepColStyle.CurrencyStyle,
					CaptionAlignment = TextAlignment.Right
				};
				list.Add(item);
			}
		}
		if (ShowCurrSumms)
		{
			if (!GroupByCorObject)
			{
				item = new AsRepColumn(ArmSoft.SME.Common.Resources.Price, 160.0)
				{
					ValuesStyle = AsRepColStyle.CurrencyStyle,
					CaptionAlignment = TextAlignment.Right,
					CurrencyPrecision = 2
				};
				list.Add(item);
			}
			item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.MTIn, 160.0)
			{
				ValuesStyle = AsRepColStyle.CurrencyStyle,
				CaptionAlignment = TextAlignment.Right,
				DataBorders = new ObservableCollection<BorderType> { BorderType.RightBorder }
			};
			list.Add(item);
			item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.MTOut, 160.0)
			{
				ValuesStyle = AsRepColStyle.CurrencyStyle,
				CaptionAlignment = TextAlignment.Right,
				DataBorders = new ObservableCollection<BorderType> { BorderType.LeftBorder }
			};
			list.Add(item);
			if (ShowSalePrice)
			{
				if (!GroupByCorObject)
				{
					item = new AsRepColumn(ArmSoft.SME.Common.Resources.MTTAccSalePrice, 160.0)
					{
						ValuesStyle = AsRepColStyle.CurrencyStyle,
						CaptionAlignment = TextAlignment.Right,
						CurrencyPrecision = 2
					};
					list.Add(item);
				}
				item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.MatRepSaleTotalSum, 160.0)
				{
					ValuesStyle = AsRepColStyle.CurrencyStyle,
					CaptionAlignment = TextAlignment.Right
				};
				list.Add(item);
			}
		}
		if (!GroupByCorObject)
		{
			item = new AsRepColumn(ArmSoft.SME.Common.Resources.Comment, 250.0);
			list.Add(item);
			item = new AsRepColumn(ArmSoft.SME.Common.Resources.Operation, 150.0);
			list.Add(item);
		}
		if (ShowParties)
		{
			if (_showPartySupplier)
			{
				item = new AsRepColumn(ArmSoft.SME.Common.Resources.Supplier, 150.0);
				list.Add(item);
			}
			if (_showPartyContract)
			{
				item = new AsRepColumn(ArmSoft.SME.Common.Resources.Contract, 150.0);
				list.Add(item);
			}
			if (_showPartyDocType)
			{
				item = new AsRepColumn(ArmSoft.SME.Common.Resources.DocumentType, 150.0);
				list.Add(item);
			}
			if (_showPartyDocNum)
			{
				item = new AsRepColumn(ArmSoft.SME.Common.Resources.DocNumber, 120.0);
				list.Add(item);
			}
			if (_showPartyUseableDate)
			{
				item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.UseLifeLong, 100.0)
				{
					ValuesStyle = AsRepColStyle.DateStyle,
					CaptionAlignment = TextAlignment.Left,
					DataAlignment = TextAlignment.Left
				};
				list.Add(item);
			}
			if (_showPartyIncomeDate)
			{
				item = new AsRepColumn(ArmSoft.SME.Common.Resources.IncomeDateShort, 120.0)
				{
					ValuesStyle = AsRepColStyle.DateStyle,
					CaptionAlignment = TextAlignment.Left,
					DataAlignment = TextAlignment.Left
				};
				list.Add(item);
			}
			if (_showPartyCountry)
			{
				item = new AsRepColumn(ArmSoft.SME.Common.Resources.Country, 150.0);
				list.Add(item);
			}
			if (_showPartyProducer)
			{
				item = new AsRepColumn(ArmSoft.SME.Common.Resources.Producer, 150.0);
				list.Add(item);
			}
			if (_showPartyProperty1)
			{
				item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.Attribute1, 200.0);
				list.Add(item);
				item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.AttributeValue + " 1 ", 200.0);
				list.Add(item);
			}
			if (_showPartyProperty2)
			{
				item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.Attribute2, 200.0);
				list.Add(item);
				item = new AsRepColumn(ArmSoft.SME.Trade.Common.Resources.AttributeValue + " 2 ", 200.0);
				list.Add(item);
			}
		}
		return list;
	}
}
