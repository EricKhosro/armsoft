using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Cache;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10057)]
public class SalesAnalysis : TradeDataView<SalesAnalysisDataProvider.DataRow, SalesAnalysisDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly SalesAnalysis _dataView;

		private readonly DateEditControl _startDate;

		private readonly DateEditControl _endDate;

		private readonly MultiSelectDropDownControl _storage;

		private readonly DropDownControl _itemType;

		private readonly DropDownControl _goodGroup;

		private readonly DropDownControl _good;

		private readonly DropDownControl _partnerGroup;

		private readonly DropDownControl _partnerCode;

		private readonly CheckEditExt _showAltUnit;

		private readonly CheckEditExt _showNccSumm;

		private readonly CheckEditExt _showNCCVATSumm;

		private readonly CheckEditExt _showCurrSumm;

		private readonly CheckEditExt _showParties;

		private readonly CheckEditExt _showPrices;

		private readonly CheckEditExt _showDiscount;

		private readonly CheckEditExt _showBonusPoints;

		private readonly CheckEditExt _showVATSumms;

		private readonly CheckEditExt _showLinks;

		private readonly DropDownControl _groupBy;

		private readonly GoodsFilter _goods;

		private readonly PartnersFilter _partners;

		public DataViewDialog(SalesAnalysis dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = ArmSoft.SME.Trade.Common.Resources.MTRepSalesAnalysis;
			_goods = new GoodsFilter(shortMode: false, "0", "", isShowOnlyInPriceList: true);
			_partners = new PartnersFilter(PartnersFilter.ShowPartnersModeType.CodeCaption, "", isFilterBySuppliers: false, isFilterByCustomers: true);
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_startDate = AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_startDate.EditValue = (useParamValues ? dataView.StartDate : new DateOnly?(Settings.HostingEnvironment.DateBegin));
			_endDate = AddDateControl(container, "EndDate", string.Empty, useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_endDate.EditValue = (useParamValues ? dataView.EndDate : new DateOnly?(Settings.HostingEnvironment.DateEnd));
			AddSpecialDateButton(container, _startDate, _endDate);
			DXTabControl tc = null;
			LayoutGroup layoutGroup = null;
			if (dataView.DocISN != Guid.Empty)
			{
				_startDate.IsRequired = false;
				((Control)(object)_startDate.Parent).Visibility = Visibility.Collapsed;
				_endDate.IsRequired = false;
				((Control)(object)_endDate.Parent).Visibility = Visibility.Collapsed;
			}
			else
			{
				tc = AddTabControl(null);
				layoutGroup = AddTabedGroup(tc, ArmSoft.SME.Common.Resources.General);
			}
			LayoutGroup container2 = layoutGroup;
			string storage = ArmSoft.SME.Common.Resources.Storage;
			bool? checkViewOrOperationAccess = true;
			_storage = AddMultiSelectDropDownControl(container2, "Storage", storage, new ArmSoft.SME.Trade.UI.DropDown.Storages(null, checkViewOrOperationAccess), isRequired: false, !useParamValues);
			_storage.SelectedString = (useParamValues ? dataView.Storage : string.Empty);
			_itemType = AddDropDownControl(layoutGroup, "ItemType", ArmSoft.SME.Common.Resources.TypeWord, new SmartEnum(typeof(GoodsTypesSE)), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_itemType.Code = (useParamValues ? dataView.ItemType : string.Empty);
			_itemType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ItemType_EditValueChanged).Handler;
			_goodGroup = AddDropDownControl(layoutGroup, "GoodGroup", ArmSoft.SME.Trade.Common.Resources.MTGpDialogCaption, new GoodsGroups(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_goodGroup.Code = (useParamValues ? dataView.GoodGroup : string.Empty);
			_goodGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(GoodsGroup_EditValueChanged).Handler;
			_good = AddDropDownControl(layoutGroup, "GoodCode", ArmSoft.SME.Trade.Common.Resources.MaterialOrService, _goods, isRequired: false, allowAdditionButtons: false, !useParamValues);
			_good.Code = (useParamValues ? dataView.Good : string.Empty);
			_partnerGroup = AddDropDownControl(layoutGroup, "PartnerType", ArmSoft.SME.Common.Resources.PartnersGroup, new PartnerGroups(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_partnerGroup.Code = (useParamValues ? dataView.PartnerGroup : string.Empty);
			_partnerGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(PartnerGroup_EditValueChanged).Handler;
			_partnerCode = AddDropDownControl(layoutGroup, "PartnerCode", ArmSoft.SME.Common.Resources.Partner, _partners, isRequired: false, allowAdditionButtons: false, !useParamValues);
			_partnerCode.Code = (useParamValues ? dataView.PartnerCode : string.Empty);
			AddLine(layoutGroup);
			_showAltUnit = AddBooleanControl(layoutGroup, "ShowAltUnit", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, !useParamValues);
			_showAltUnit.IsChecked = useParamValues && dataView.ShowAltUnit;
			_showNccSumm = AddBooleanControl(layoutGroup, "ShowNccSumm", ArmSoft.SME.Trade.Common.Resources.ShowNCCSumma, !useParamValues);
			_showNccSumm.IsChecked = useParamValues && dataView.ShowNccSumm;
			_showNccSumm.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ShowNccSumm_EditValueChanged).Handler;
			_showNCCVATSumm = AddBooleanControl(layoutGroup, "ShowNCCVATSumm", ArmSoft.SME.Trade.Common.Resources.ShowNCCSummaWithVAT, !useParamValues);
			_showNCCVATSumm.IsChecked = useParamValues && dataView.ShowNCCVATSumm;
			_showNCCVATSumm.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ShowNCCVATSumm_EditValueChanged).Handler;
			_showCurrSumm = AddBooleanControl(layoutGroup, "ShowCurrSumm", string.Format(ArmSoft.SME.Trade.Common.Resources.ShowCurrSumma, _mtCurr), !useParamValues);
			_showCurrSumm.IsChecked = useParamValues && dataView.ShowCurrSumm;
			_showCurrSumm.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ShowCurrSumm_EditValueChanged).Handler;
			_showParties = AddBooleanControl(layoutGroup, "ShowParties", ArmSoft.SME.Common.Resources.ShowParties, !useParamValues);
			_showParties.IsChecked = useParamValues && dataView.ShowParties;
			AddLine(layoutGroup);
			_showPrices = AddBooleanControl(layoutGroup, "ShowPrices", ArmSoft.SME.Trade.Common.Resources.ShowCostAndSalePrices, !useParamValues);
			_showPrices.IsChecked = useParamValues && dataView.ShowPrices;
			_showDiscount = AddBooleanControl(layoutGroup, "ShowDiscount", ArmSoft.SME.Trade.Common.Resources.ShowDiscountSumma, !useParamValues);
			_showDiscount.IsChecked = useParamValues && dataView.ShowDiscount;
			if (_useBonus)
			{
				_showBonusPoints = AddBooleanControl(layoutGroup, "ShowBonusPoints", ArmSoft.SME.Trade.Common.Resources.ShowBonusPoints, !useParamValues);
				_showBonusPoints.IsChecked = useParamValues && dataView.ShowBonusPoints;
			}
			_showVATSumms = AddBooleanControl(layoutGroup, "ShowVATSumms", ArmSoft.SME.Trade.Common.Resources.ShowVATSumms, !useParamValues);
			_showVATSumms.IsChecked = useParamValues && dataView.ShowVATSumms;
			AddLine(layoutGroup);
			_showLinks = AddBooleanControl(layoutGroup, "ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumns, !useParamValues);
			_showLinks.IsChecked = useParamValues && dataView.ShowLinks;
			_groupBy = AddDropDownControl(layoutGroup, "GroupBy", ArmSoft.SME.Trade.Common.Resources.GroupBy, new SmartEnum(typeof(GroupBySE)), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_groupBy.Code = (useParamValues ? dataView.GroupBy : string.Empty);
			_groupBy.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(GroupBy_EditValueChanged).Handler;
			if (!isRefreshMode)
			{
				AddColumnsSelectionTab(tc, layoutGroup, null);
			}
			else
			{
				GroupBy_EditValueChangedForRefresh();
			}
			UpdateShowDiscountControl();
			UpdateShowBonusPointsControl();
			UpdateShowPricesControl();
		}

		private void ShowNccSumm_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			UpdateShowDiscountControl();
			UpdateShowBonusPointsControl();
			UpdateShowPricesControl();
		}

		private void ShowNCCVATSumm_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			UpdateShowDiscountControl();
			UpdateShowBonusPointsControl();
			UpdateShowPricesControl();
		}

		private void ShowCurrSumm_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			UpdateShowDiscountControl();
			UpdateShowBonusPointsControl();
			UpdateShowPricesControl();
		}

		private void UpdateShowDiscountControl()
		{
			_showDiscount.IsReadOnly = !_showNccSumm.IsChecked.Value && !_showNCCVATSumm.IsChecked.Value && !_showCurrSumm.IsChecked.Value;
			if (_showDiscount.IsReadOnly)
			{
				_showDiscount.IsChecked = false;
			}
		}

		private void UpdateShowBonusPointsControl()
		{
			if (_useBonus)
			{
				_showBonusPoints.IsReadOnly = !_showNccSumm.IsChecked.Value && !_showNCCVATSumm.IsChecked.Value && !_showCurrSumm.IsChecked.Value;
				if (_showBonusPoints.IsReadOnly)
				{
					_showBonusPoints.IsChecked = false;
				}
			}
		}

		private void UpdateShowPricesControl()
		{
			_showPrices.IsReadOnly = (!_showNccSumm.IsChecked.Value && !_showNCCVATSumm.IsChecked.Value && !_showCurrSumm.IsChecked.Value) || _groupBy.Code == GroupBySE.ByPartner.Value;
			if (_showPrices.IsReadOnly)
			{
				_showPrices.IsChecked = false;
			}
		}

		private void GoodsGroup_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue)
			{
				_goods.Group = (string)e.NewValue;
			}
		}

		private void GroupBy_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			GroupBy_EditValueChangedForRefresh();
		}

		private void GroupBy_EditValueChangedForRefresh()
		{
			if (!string.IsNullOrWhiteSpace(_groupBy.Code))
			{
				_showParties.IsChecked = false;
				if (_groupBy.Code == GroupBySE.ByPartner.Value)
				{
					_showAltUnit.IsChecked = false;
				}
			}
			_showParties.IsReadOnly = !string.IsNullOrWhiteSpace(_groupBy.Code);
			_showAltUnit.IsReadOnly = Convert.ToBoolean(_groupBy.Code == GroupBySE.ByPartner.Value);
			UpdateShowPricesControl();
		}

		private void ItemType_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue && SmartEnum<ItemTypesSE, string>.TryFromValue((string)e.NewValue, out var result))
			{
				_goods.ItemType = result;
			}
			if (!string.IsNullOrWhiteSpace(_itemType.Code))
			{
				if (!_itemType.DoValidate())
				{
					return;
				}
				if (_itemType.Code == ItemTypesSE.Goods)
				{
					_storage.IsReadOnly = false;
					_good.DataView = new GoodsFilter(shortMode: false, ItemTypesSE.Goods);
				}
				else
				{
					_storage.IsReadOnly = true;
					_storage.SelectedString = "";
					_good.DataView = new GoodsFilter(shortMode: false, ItemTypesSE.Services);
				}
				if (!string.IsNullOrWhiteSpace(_good.Code))
				{
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_good.Code);
					if (item != null && item.Type != _itemType.Code)
					{
						_good.Code = "";
						_good.DoValidate();
					}
				}
			}
			else
			{
				_storage.IsReadOnly = false;
				_good.DataView = new GoodsFilter(shortMode: false);
			}
		}

		private void PartnerGroup_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue)
			{
				_partners.PartType = (string)e.NewValue;
			}
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				if (_dataView.DocISN == Guid.Empty)
				{
					ArmSoft.SME.Core.UI.ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
					args.isValid = TradeDataViewParametersDialogWindow.CheckPeriodForActualness(_endDate.EditValue.Value);
				}
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			if (_dataView.DocISN == Guid.Empty)
			{
				_dataView.StartDate = _startDate.EditValue.Value;
				_dataView.EndDate = _endDate.EditValue.Value;
			}
			_dataView.Storage = _storage.SelectedString;
			_dataView.ItemType = _itemType.Code;
			_dataView.GoodGroup = _goodGroup.Code;
			_dataView.Good = _good.Code;
			_dataView.PartnerGroup = _partnerGroup.Code;
			_dataView.PartnerCode = _partnerCode.Code;
			_dataView.ShowAltUnit = _showAltUnit.IsChecked.Value;
			_dataView.ShowNccSumm = _showNccSumm.IsChecked.Value;
			_dataView.ShowNCCVATSumm = _showNCCVATSumm.IsChecked.Value;
			_dataView.ShowCurrSumm = _showCurrSumm.IsChecked.Value;
			_dataView.ShowParties = _showParties.IsChecked.Value;
			_dataView.ShowDiscount = _showDiscount.IsChecked.Value;
			if (_useBonus)
			{
				_dataView.ShowBonusPoints = _showBonusPoints.IsChecked.Value;
			}
			_dataView.ShowVATSumms = _showVATSumms.IsChecked.Value;
			_dataView.ShowPrices = _showPrices.IsChecked.Value;
			_dataView.ShowLinks = _showLinks.IsChecked.Value;
			_dataView.GroupBy = _groupBy.Code;
		}
	}

	private static readonly string _mtCurr = Settings.ParametersService.MTCurr();

	private static readonly bool _useBonus = Settings.ParametersService.UseBonus();

	private readonly bool _showSumms;

	private readonly List<ViewColumn> _potentiallyColumns;

	public override LoadIndication LoadingIndication => LoadIndication.AlwaysInside;

	public override string HelpId => "Sales_Reports_SalesAnalysis";

	public override bool DialogSupported => true;

	public override List<ViewColumn> PotentiallyColumns => _potentiallyColumns;

	public override string Caption
	{
		get
		{
			if (DocISN == Guid.Empty)
			{
				return $"{ArmSoft.SME.Trade.Common.Resources.MTRepSalesAnalysis} - {StartDate.FormatDDMMYY()} - {EndDate.FormatDDMMYY()}";
			}
			return ArmSoft.SME.Trade.Common.Resources.MTRepSalesAnalysis + " - " + AdditionalCaption;
		}
	}

	public override bool AllowEdit
	{
		get
		{
			if (AllowOperation)
			{
				return string.IsNullOrWhiteSpace(GroupBy);
			}
			return false;
		}
	}

	public override bool AllowPreview => string.IsNullOrWhiteSpace(GroupBy);

	public override bool AllowView => string.IsNullOrWhiteSpace(GroupBy);

	public override bool AllowCreateCopy
	{
		get
		{
			if (string.IsNullOrWhiteSpace(GroupBy))
			{
				return AllowOperation;
			}
			return false;
		}
	}

	protected override bool AllowFindRowByMTBarcode => string.IsNullOrWhiteSpace(GroupBy);

	protected override string MTCodeColumnName => "GoodCode";

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

	public string GoodGroup
	{
		get
		{
			return Convert.ToString(Parameters["GoodGroup"].Value);
		}
		set
		{
			Parameters["GoodGroup"].Value = value;
		}
	}

	public string Good
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

	public string PartnerGroup
	{
		get
		{
			return Convert.ToString(Parameters["PartnerType"].Value);
		}
		set
		{
			Parameters["PartnerType"].Value = value;
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

	public bool ShowAltUnit
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowAltUnit"].Value);
		}
		set
		{
			Parameters["ShowAltUnit"].Value = value;
		}
	}

	public bool ShowNCCVATSumm
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowNCCVATSumm"].Value);
		}
		set
		{
			Parameters["ShowNCCVATSumm"].Value = value;
		}
	}

	public bool ShowNccSumm
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowNccSumm"].Value);
		}
		set
		{
			Parameters["ShowNccSumm"].Value = value;
		}
	}

	public bool ShowCurrSumm
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowCurrSumm"].Value);
		}
		set
		{
			Parameters["ShowCurrSumm"].Value = value;
		}
	}

	public bool ShowParties
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowParties"].Value);
		}
		set
		{
			Parameters["ShowParties"].Value = value;
		}
	}

	public bool ShowPrices
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowPrices"].Value);
		}
		set
		{
			Parameters["ShowPrices"].Value = value;
		}
	}

	public bool ShowDiscount
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowDiscount"].Value);
		}
		set
		{
			Parameters["ShowDiscount"].Value = value;
		}
	}

	public bool ShowBonusPoints
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowBonusPoints"].Value);
		}
		set
		{
			Parameters["ShowBonusPoints"].Value = value;
		}
	}

	public bool ShowVATSumms
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowVATSumms"].Value);
		}
		set
		{
			Parameters["ShowVATSumms"].Value = value;
		}
	}

	public bool ShowLinks
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowLinks"].Value);
		}
		set
		{
			Parameters["ShowLinks"].Value = value;
		}
	}

	public string GroupBy
	{
		get
		{
			return Convert.ToString(Parameters["GroupBy"].Value);
		}
		set
		{
			Parameters["GroupBy"].Value = value;
		}
	}

	public string Storage
	{
		get
		{
			return Convert.ToString(Parameters["Storage"].Value);
		}
		set
		{
			Parameters["Storage"].Value = value;
		}
	}

	public string ItemType
	{
		get
		{
			return Convert.ToString(Parameters["ItemType"].Value);
		}
		set
		{
			Parameters["ItemType"].Value = value;
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

	public string AdditionalCaption
	{
		get
		{
			return (string)Parameters["AdditionalCaption"].Value;
		}
		set
		{
			Parameters["AdditionalCaption"].Value = value;
		}
	}

	public SalesAnalysis()
	{
		DataProvider = new SalesAnalysisDataProvider(TradeSettings.CreateTradeClient());
		_showSumms = Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, null, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, null, "dd/MM/yy");
		AddParameter("Storage", ArmSoft.SME.Common.Resources.Storage, string.Empty);
		AddParameter("ItemType", ArmSoft.SME.Common.Resources.TypeWord, string.Empty);
		AddParameter("GoodGroup", ArmSoft.SME.Common.Resources.Group, string.Empty);
		AddParameter("GoodCode", ArmSoft.SME.Trade.Common.Resources.MaterialOrService, string.Empty);
		AddParameter("PartnerType", ArmSoft.SME.Common.Resources.PartnersGroup, string.Empty);
		AddParameter("PartnerCode", ArmSoft.SME.Common.Resources.Buyer, string.Empty);
		AddParameter("ShowAltUnit", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, false);
		AddParameter("ShowNccSumm", ArmSoft.SME.Trade.Common.Resources.ShowNCCSumma, false);
		AddParameter("ShowNCCVATSumm", ArmSoft.SME.Trade.Common.Resources.ShowNCCSummaWithVAT, false);
		AddParameter("ShowCurrSumm", string.Format(ArmSoft.SME.Trade.Common.Resources.ShowCurrSumma, _mtCurr), false);
		AddParameter("ShowParties", ArmSoft.SME.Common.Resources.ByDiffParties, false);
		AddParameter("ShowPrices", ArmSoft.SME.Trade.Common.Resources.ShowPrices, false);
		AddParameter("ShowDiscount", ArmSoft.SME.Trade.Common.Resources.ShowDiscountSumma, false);
		AddParameter("ShowBonusPoints", ArmSoft.SME.Trade.Common.Resources.ShowBonusPoints, false);
		AddParameter("ShowVATSumms", ArmSoft.SME.Trade.Common.Resources.ShowVATSumms, false);
		AddParameter("ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumns, false);
		AddParameter("GroupBy", ArmSoft.SME.Trade.Common.Resources.GroupBy, string.Empty);
		AddParameter("DocISN", ArmSoft.SME.Common.Resources.DocumentISN, Guid.Empty);
		AddParameter("AdditionalCaption", ArmSoft.SME.Trade.Common.Resources.AdditionalCaption, string.Empty);
		FillColumnsFromDataProvider();
		if (_useBonus)
		{
			Columns["UsedBonus"].IsSystem = true;
			Columns["AccumulatedBonus"].IsSystem = true;
		}
		_potentiallyColumns = new List<ViewColumn>();
		_potentiallyColumns.AddRange(Columns.ViewList);
		_potentiallyColumns.AddRange(PartnerCache.GetDataViewColumns(ArmSoft.SME.Common.Resources.Buyer, "", "", includeCode: false, includeCaption: true));
		_potentiallyColumns.AddRange(MaterialCache.GetDataViewColumns(ArmSoft.SME.Trade.Common.Resources.MaterialValue, "", ""));
		_potentiallyColumns.AddRange(ProducerCache.GetDataViewColumns(ArmSoft.SME.Common.Resources.Producer, "", ""));
	}

	protected override void SetColumnsProperties()
	{
		base.SetColumnsProperties();
		Columns["DocDate"].IsSystem = true;
		Columns["DocNum"].IsSystem = true;
		Columns["ECRCheckNum"].IsSystem = true;
		Columns["OpName"].IsSystem = true;
		Columns["ItemType"].IsSystem = true;
		Columns["GoodCode"].IsSystem = true;
		Columns["GoodCaption"].IsSystem = true;
		Columns["UnitCode"].IsSystem = true;
		Columns["UnitBrief"].IsSystem = true;
		Columns["Storage"].IsSystem = true;
		Columns["StorageCaption"].IsSystem = true;
		Columns["CashDesk"].IsSystem = true;
		Columns["CashDeskCaption"].IsSystem = true;
		Columns["ShiftNumber"].IsSystem = true;
		Columns["PartnerCode"].IsSystem = true;
		Columns["PartnerCaption"].IsSystem = true;
		Columns["Quantity"].IsSystem = true;
		Columns["Summ"].IsSystem = true;
		Columns["CostPrice"].IsSystem = true;
		Columns["SalePrice"].IsSystem = true;
		Columns["SaleSumm"].IsSystem = true;
		Columns["Profit"].IsSystem = true;
		Columns["ProfitPercentByCost"].IsSystem = true;
		Columns["ProfitPercentBySale"].IsSystem = true;
		Columns["DiscountSum"].IsSystem = true;
		Columns["DiscountPercent"].IsSystem = true;
		Columns["CostVATSumm"].IsSystem = true;
		Columns["CostVATPrice"].IsSystem = true;
		Columns["SummWithVAT"].IsSystem = true;
		Columns["SaleSummWithVAT"].IsSystem = true;
		Columns["SalePriceWithVAT"].IsSystem = true;
		Columns["SaleVATSumm"].IsSystem = true;
		Columns["ProfitWithVAT"].IsSystem = true;
		Columns["ProfitPercentBySaleWithVAT"].IsSystem = true;
		Columns["ProfitPercentByCostWithVAT"].IsSystem = true;
		Columns["DiscountSummWithVAT"].IsSystem = true;
		Columns["DiscountPercentWithVAT"].IsSystem = true;
		Columns["DocVATTypeCaption"].IsSystem = true;
		Columns["CostVATPrice"].IsSystem = true;
		Columns["CostPriceWithVAT"].IsSystem = true;
		Columns["SaleVATSumm"].IsSystem = true;
		Columns["DiscountVATSumm"].IsSystem = true;
		Columns["CurSumm"].IsSystem = true;
		Columns["CurrCostPrice"].IsSystem = true;
		Columns["SaleCurPrice"].IsSystem = true;
		Columns["SaleCurSumm"].IsSystem = true;
		Columns["CurProfit"].IsSystem = true;
		Columns["CurProfitPercentBySale"].IsSystem = true;
		Columns["CurProfitPercentByCost"].IsSystem = true;
		Columns["DiscountCurSum"].IsSystem = true;
		Columns["DiscountCurPercent"].IsSystem = true;
		if (_useBonus)
		{
			Columns["UsedBonus"].IsSystem = true;
			Columns["AccumulatedBonus"].IsSystem = true;
		}
		Columns["Group"].IsSystem = true;
		Columns["SalesConsultantCode"].IsSystem = true;
		Columns["SalesConsultantCaption"].IsSystem = true;
		Columns["GoodDescription"].IsSystem = true;
		Columns["BaseDocDate"].IsSystem = true;
		Columns["BaseDocNum"].IsSystem = true;
		Columns["IncomeDate"].IsSystem = true;
		Columns["PartyPartCaption"].IsSystem = true;
		Columns["PartyPartTaxCode"].IsSystem = true;
		Columns["PartyContractCaption"].IsSystem = true;
		Columns["PartyDocType"].IsSystem = true;
		Columns["PartyDocNum"].IsSystem = true;
		Columns["PartyDocUsableDate"].IsSystem = true;
		Columns["PartyDocCountry"].IsSystem = true;
		Columns["PartyDocProducer"].IsSystem = true;
		Columns["PartyDocPropertyType1"].IsSystem = true;
		Columns["PartyDocPropertyValue1"].IsSystem = true;
		Columns["PartyDocPropertyType2"].IsSystem = true;
		Columns["PartyDocPropertyValue2"].IsSystem = true;
		Columns["DocType"].Visible = false;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("DocDate");
			dvp.Set_ColumnSortOrder("DocNum");
			dvp.Set_ColumnSortOrder("Storage");
		}
		Columns.AddDirectoryLinkColumn(DirectoryTypeSE.Goods, "GoodCode", "GoodCaption", "ItemType", "", ShowLinks);
		Columns.AddDirectoryLinkColumn(DirectoryTypeSE.Partner, "PartnerCode", "PartnerCaption", "", "", ShowLinks);
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		SalesAnalysisDataProvider.DataRow dataRow = (SalesAnalysisDataProvider.DataRow)currentViewPanel.FocusedRow();
		if (dataRow != null && AllowOperation && string.IsNullOrWhiteSpace(GroupBy))
		{
			if (dataRow.DocType != (short)DocumentTypeSE.MTBillReturn && dataRow.DocType != (short)DocumentTypeSE.MTReturnFromCustomer)
			{
				currentViewPanel.AddContextFunction("ContextFunctions", "CreateDoc", ArmSoft.SME.Common.Resources.CreateDoc, CreateDoc, "doc_new_16.svg");
			}
			DefinitionMappingsService.TryResolveDocumentDefinition(dataRow.DocType, out var definition);
			if (definition.AllowStoreAsDraft || (currentViewPanel.GetSelectedAny() && Settings.ParametersService.DraftSelected()))
			{
				currentViewPanel.AddContextFunction("ContextFunctions", "SetDocDraftState", ArmSoft.SME.Common.Resources.SetDocDraft, SetDocDraftState, "", "", storeOpContext: true);
			}
		}
	}

	private void SetDocDraftState()
	{
		InitContextFunctionsHelper.ChangeDocState((from p in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<SalesAnalysisDataProvider.DataRow>()
			select p.GetDocISN()).ToList(), DocumentStateSE.Draft);
	}

	private void CreateDoc()
	{
		int num = 4;
		List<short> list = new List<short>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<short> span = CollectionsMarshal.AsSpan(list);
		span[0] = DocumentTypeSE.MTIncome.Value;
		span[1] = DocumentTypeSE.MTIncomeRetail.Value;
		span[2] = DocumentTypeSE.MTReturnFromCustomer.Value;
		span[3] = DocumentTypeSE.MTBillReturn.Value;
		DocumentTypeSE documentTypeSE = InitContextFunctionsHelper.ChooseDocTypeFromMenu(list);
		if (documentTypeSE == DocumentTypeSE.MTIncome)
		{
			InitContextFunctionsHelper.CreateNewMTIncomeDoc(EndDate.Value);
		}
		else if (documentTypeSE == DocumentTypeSE.MTIncomeRetail)
		{
			InitContextFunctionsHelper.CreateNewMTIncomeRetailDoc(EndDate.Value);
		}
		else if (documentTypeSE == DocumentTypeSE.MTReturnFromCustomer)
		{
			CreateNewMTReturnFromCustomerDoc();
		}
		else if (documentTypeSE == DocumentTypeSE.MTBillReturn)
		{
			CreateNewMTBillReturnDoc();
		}
	}

	private static void CreateNewMTReturnFromCustomerDoc()
	{
		try
		{
			List<SalesAnalysisDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<SalesAnalysisDataProvider.DataRow>();
			string text = string.Empty;
			string text2 = string.Empty;
			string text3 = ((selectedOrFocusedRows.Select((SalesAnalysisDataProvider.DataRow row) => row.PartnerCode).Distinct().Count() > 1) ? string.Empty : selectedOrFocusedRows[0].PartnerCode);
			string contractName = ((selectedOrFocusedRows.Select((SalesAnalysisDataProvider.DataRow row) => row.Contract).Distinct().Count() > 1) ? string.Empty : selectedOrFocusedRows[0].Contract);
			string salesConsultant = ((selectedOrFocusedRows.Select((SalesAnalysisDataProvider.DataRow row) => row.SalesConsultantCode).Distinct().Count() > 1) ? string.Empty : selectedOrFocusedRows[0].SalesConsultantCode);
			foreach (SalesAnalysisDataProvider.DataRow item in selectedOrFocusedRows)
			{
				if (item.DocType == (short)DocumentTypeSE.MTReturnFromCustomer || item.DocType == (short)DocumentTypeSE.MTBillReturn)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrInvalidBaseDocTypeForMTReturn);
				}
				if (!string.IsNullOrWhiteSpace(text) && text != item.DocCurrCode)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrDifferentCurrNotAllowedForMTReturn);
				}
				text = item.DocCurrCode;
				if (!string.IsNullOrWhiteSpace(text2) && text2 != item.DocVATType)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrDifferentVATTypeNotAllowedForMTReturn);
				}
				text2 = item.DocVATType;
			}
			MTReturnFromCustomer mTReturnFromCustomer = Settings.DocumentService.Create<MTReturnFromCustomer>(DocumentTypeSE.MTReturnFromCustomer);
			int num = 0;
			mTReturnFromCustomer.PartnerContractGroup.PartnerCode = text3;
			if (!string.IsNullOrWhiteSpace(text3) && !string.IsNullOrWhiteSpace(contractName))
			{
				PartnerCacheRow.ContractRow contractRow = TradeSettings.TradeCacheService.Partner.GetItem(text3).Contracts.FirstOrDefault((PartnerCacheRow.ContractRow p) => p.Caption == contractName);
				mTReturnFromCustomer.PartnerContractGroup.ContractCode = contractRow?.Contract ?? "";
			}
			mTReturnFromCustomer.SalesConsultant = salesConsultant;
			mTReturnFromCustomer.CurrCode = text;
			if (text != "AMD")
			{
				TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(text, Settings.HostingEnvironment.CurrentDate);
			}
			mTReturnFromCustomer.VATType = text2;
			string text4 = Settings.ParametersService.DefaultStorage();
			foreach (SalesAnalysisDataProvider.DataRow item2 in selectedOrFocusedRows)
			{
				MTReturnFromCustomerBase.MTRetFromCustSpecificationRow returnRow = GetReturnRow(mTReturnFromCustomer, item2);
				decimal? quantity = returnRow.Quantity;
				if (!((quantity.GetValueOrDefault() == 0m) & quantity.HasValue) && returnRow.ItemType == Convert.ToString(ItemTypesSE.Goods))
				{
					returnRow.CostPrice = (item2.Summ / returnRow.Quantity.Value).RoundAs(2);
					returnRow.CostPriceVAT = (item2.CostVATSumm / returnRow.Quantity.Value).RoundAs(2);
					returnRow.CurrCostPrice = (item2.CurSumm / returnRow.Quantity.Value).RoundAs(2);
				}
				returnRow.RowID = num;
				if (!string.IsNullOrWhiteSpace(text4) && returnRow.ItemType == Convert.ToString(ItemTypesSE.Goods))
				{
					returnRow.Storage = text4;
				}
				mTReturnFromCustomer.MTList.Add(returnRow);
				num++;
			}
			mTReturnFromCustomer.MaxRowID = num;
			if (mTReturnFromCustomer.VATType == "1")
			{
				mTReturnFromCustomer.Summ += (decimal?)mTReturnFromCustomer.SummVAT;
			}
			mTReturnFromCustomer.PriceType = Settings.ParametersService.DefaultSalePrice();
			Settings.DocumentService.CreateWindow(mTReturnFromCustomer).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public static void CreateNewMTBillReturnDoc()
	{
		try
		{
			List<SalesAnalysisDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<SalesAnalysisDataProvider.DataRow>();
			Guid guid = Guid.Empty;
			foreach (SalesAnalysisDataProvider.DataRow item in selectedOrFocusedRows)
			{
				if (item.DocType != (short)DocumentTypeSE.MTBill)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrInvalidBaseDocTypeForMTBillReturn);
				}
				if (guid != Guid.Empty && guid != item.DocISN)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.MTBillRetDiffDocError);
				}
				guid = item.DocISN;
			}
			MTBillReturn mTBillReturn = Settings.DocumentService.Create<MTBillReturn>(DocumentTypeSE.MTBillReturn.Value);
			MTBill mTBill = (MTBill)Settings.DocumentService.Load(selectedOrFocusedRows[0].DocISN, selectedOrFocusedRows[0].DocType);
			mTBillReturn.BaseISN = selectedOrFocusedRows[0].DocISN;
			mTBillReturn.PartnerContractGroup.PartnerCode = selectedOrFocusedRows[0].PartnerCode;
			mTBillReturn.PartnerContractGroup.ContractCode = selectedOrFocusedRows[0].Contract;
			mTBillReturn.SalesConsultant = selectedOrFocusedRows[0].SalesConsultantCode;
			mTBillReturn.CurrCode = selectedOrFocusedRows[0].DocCurrCode;
			if (mTBillReturn.CurrCode != "AMD")
			{
				TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(mTBillReturn.CurrCode, Settings.HostingEnvironment.CurrentDate);
			}
			mTBillReturn.VATType = mTBill.VATType;
			foreach (SalesAnalysisDataProvider.DataRow row in selectedOrFocusedRows)
			{
				MTInvoiceBase.MTInvoiceSpecificationRow mTInvoiceSpecificationRow = mTBill.MTList.FirstOrDefault((MTInvoiceBase.MTInvoiceSpecificationRow x) => x.RowID == row.RowId);
				if (mTInvoiceSpecificationRow != null)
				{
					mTBillReturn.MTList.Add(new MTReturnFromCustomerBase.MTRetFromCustSpecificationRow
					{
						RowID = mTInvoiceSpecificationRow.RowID,
						ItemID = mTInvoiceSpecificationRow.ItemID,
						ItemType = mTInvoiceSpecificationRow.ItemType,
						Storage = mTInvoiceSpecificationRow.Storage,
						BarCode = mTInvoiceSpecificationRow.BarCode,
						ItemCode = mTInvoiceSpecificationRow.ItemCode,
						ItemName = mTInvoiceSpecificationRow.ItemName,
						UnitCode = mTInvoiceSpecificationRow.UnitCode,
						UnitBrief = mTInvoiceSpecificationRow.UnitBrief,
						Quantity = mTInvoiceSpecificationRow.Quantity,
						Price = mTInvoiceSpecificationRow.Price,
						Discount = mTInvoiceSpecificationRow.Discount,
						DiscountPrice = mTInvoiceSpecificationRow.DiscountPrice,
						Summa = mTInvoiceSpecificationRow.Summa,
						DiscountSumma = mTInvoiceSpecificationRow.DiscountSumma,
						IsRowMarkable = mTInvoiceSpecificationRow.IsRowMarkable,
						VAT = (mTInvoiceSpecificationRow.VAT.HasValue && mTInvoiceSpecificationRow.VAT.Value),
						AccumulatedBonus = mTInvoiceSpecificationRow.AccumulatedBonus,
						RowDescription = mTInvoiceSpecificationRow.RowDescription,
						MarkingStatus = mTInvoiceSpecificationRow.MarkingStatus,
						MarkingList = mTInvoiceSpecificationRow.MarkingList,
						IsGift = mTInvoiceSpecificationRow.IsGift,
						IsDeleted = mTInvoiceSpecificationRow.IsDeleted,
						IsStorageRequired = mTInvoiceSpecificationRow.IsStorageRequired,
						IsPartySelected = (mTInvoiceSpecificationRow.IsPartySelected.HasValue && mTInvoiceSpecificationRow.IsPartySelected.Value),
						UsedBonus = mTInvoiceSpecificationRow.UsedBonus
					});
				}
			}
			if (mTBillReturn.VATType == "1")
			{
				mTBillReturn.Summ += (decimal?)mTBillReturn.SummVAT;
			}
			Settings.DocumentService.CreateWindow(mTBillReturn).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static MTReturnFromCustomerBase.MTRetFromCustSpecificationRow GetReturnRow(MTReturnFromCustomer doc, SalesAnalysisDataProvider.DataRow sourceRow)
	{
		MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = new MTReturnFromCustomerBase.MTRetFromCustSpecificationRow
		{
			ItemType = sourceRow.ItemTypeCode,
			Storage = sourceRow.Storage,
			ItemID = sourceRow.GoodID,
			ItemCode = sourceRow.GoodCode,
			ItemName = sourceRow.GoodCaption,
			UnitCode = sourceRow.UnitCode,
			UnitBrief = sourceRow.UnitBrief
		};
		DataFromInvoiceListRow dataFromInvoiceListRow = TradeSettings.CreateTradeClient().CommonServices.GetDataFromInvoiceListRow(sourceRow.DocISN, sourceRow.RowId);
		mTRetFromCustSpecificationRow.Quantity = dataFromInvoiceListRow.Quantity;
		mTRetFromCustSpecificationRow.Price = dataFromInvoiceListRow.SalePrice;
		mTRetFromCustSpecificationRow.Summa = dataFromInvoiceListRow.SaleSumm;
		mTRetFromCustSpecificationRow.VAT = dataFromInvoiceListRow.VAT;
		mTRetFromCustSpecificationRow.DealType = dataFromInvoiceListRow.DealType;
		decimal? quantity = mTRetFromCustSpecificationRow.Quantity;
		decimal quantity2 = sourceRow.Quantity;
		if (!((quantity.GetValueOrDefault() == quantity2) & quantity.HasValue))
		{
			quantity = mTRetFromCustSpecificationRow.Quantity;
			if (!((quantity.GetValueOrDefault() == 0m) & quantity.HasValue))
			{
				mTRetFromCustSpecificationRow.Price = (mTRetFromCustSpecificationRow.Summa.Value / mTRetFromCustSpecificationRow.Quantity.Value).RoundAs(2);
			}
			mTRetFromCustSpecificationRow.Quantity = sourceRow.Quantity;
			mTRetFromCustSpecificationRow.Summa = (mTRetFromCustSpecificationRow.Quantity.Value * mTRetFromCustSpecificationRow.Price.Value).RoundAs(2);
		}
		doc.Summ += mTRetFromCustSpecificationRow.Summa;
		doc.SummVAT += PriceHelper.CalcVATAmountByType(mTRetFromCustSpecificationRow.Summa.Value, doc.VATType, mTRetFromCustSpecificationRow.VAT.Value);
		return mTRetFromCustSpecificationRow;
	}

	protected override void SetCacheRow(ExtendableRow row)
	{
		SalesAnalysisDataProvider.DataRow dataRow = row as SalesAnalysisDataProvider.DataRow;
		dataRow.MaterialCacheRow = TradeSettings.TradeCacheService.Material.GetItem(dataRow.GoodCode);
		dataRow.PartnerCacheRow = TradeSettings.TradeCacheService.Partner.GetItem(dataRow.PartnerCode);
		dataRow.ProducerCacheRow = TradeSettings.TradeCacheService.Producer.GetItem(dataRow.MaterialCacheRow?.Producer ?? "");
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		if (!TradeSettings.CreateTradeClient().CommonServices.ExistsAvailableStorage())
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.AccessToStoragesDenied, Settings.LoginService.UserName), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return null;
		}
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.Storage = Storage;
		ParametersProvider.ItemType = ItemType;
		ParametersProvider.GoodGroup = GoodGroup;
		ParametersProvider.GoodCode = Good;
		ParametersProvider.PartnerType = PartnerGroup;
		ParametersProvider.PartnerCode = PartnerCode;
		ParametersProvider.ShowAltUnit = ShowAltUnit;
		ParametersProvider.ShowNccSumm = ShowNccSumm;
		ParametersProvider.ShowNCCVATSumm = ShowNCCVATSumm;
		ParametersProvider.ShowCurrSumm = ShowCurrSumm;
		ParametersProvider.ShowParties = ShowParties;
		ParametersProvider.ShowDiscount = ShowDiscount;
		if (_useBonus)
		{
			ParametersProvider.ShowBonusPoints = ShowBonusPoints;
		}
		ParametersProvider.ShowVATSumms = ShowVATSumms;
		ParametersProvider.ShowPrices = ShowPrices;
		ParametersProvider.GroupBy = GroupBy;
		ParametersProvider.DocISN = DocISN;
	}

	protected override void SetDataProviderColumns()
	{
		bool flag = GroupBy.Trim() != GroupBySE.ByGood.Value;
		bool flag2 = GroupBy.Trim() != GroupBySE.ByPartner.Value;
		ColumnsProvider.Add("GoodID");
		ColumnsProvider.Add("DocISN");
		ColumnsProvider.Add("DocType");
		ColumnsProvider.Add("DocCurrCode");
		ColumnsProvider.Add("ItemTypeCode");
		if (string.IsNullOrWhiteSpace(GroupBy.Trim()))
		{
			ColumnsProvider.Add("RowId");
			ColumnsProvider.Add("DocVATType");
			ColumnsProvider.Add("DocVATTypeCaption");
			ColumnsProvider.Add("DocDate");
			ColumnsProvider.Add("DocNum");
			ColumnsProvider.Add("ECRCheckNum");
			ColumnsProvider.Add("OpName");
			ColumnsProvider.Add("SalesConsultantCode");
			ColumnsProvider.Add("SalesConsultantCaption");
			ColumnsProvider.Add("Comment");
			ColumnsProvider.Add("Storage");
			ColumnsProvider.Add("StorageCaption");
			ColumnsProvider.Add("CashDesk");
			ColumnsProvider.Add("CashDeskCaption");
			ColumnsProvider.Add("ShiftNumber");
			ColumnsProvider.Add("RowDescription");
			ColumnsProvider.Add("TaxExportCaption");
			ColumnsProvider.Add("BaseDocDate");
			ColumnsProvider.Add("BaseDocNum");
			ColumnsProvider.Add("BuyerBussAddress");
		}
		if (flag2)
		{
			ColumnsProvider.Add("GoodCode");
			ColumnsProvider.Add("ItemType");
			ColumnsProvider.Add("GoodCaption");
			ColumnsProvider.Add("GoodDescription");
			ColumnsProvider.Add("UnitCode");
			ColumnsProvider.Add("UnitBrief");
			ColumnsProvider.Add("Group");
			ColumnsProvider.Add("Quantity");
		}
		if (flag)
		{
			ColumnsProvider.Add("PartnerCode");
			ColumnsProvider.Add("PartnerCaption");
		}
		if (flag & flag2)
		{
			ColumnsProvider.Add("Contract");
		}
		if (ShowNccSumm && _showSumms)
		{
			ColumnsProvider.Add("Summ");
			ColumnsProvider.Add("Profit");
			ColumnsProvider.Add("ProfitPercentBySale");
			ColumnsProvider.Add("ProfitPercentByCost");
		}
		if (ShowNccSumm && ShowPrices && _showSumms)
		{
			ColumnsProvider.Add("CostPrice");
		}
		if (ShowNccSumm && ShowPrices)
		{
			ColumnsProvider.Add("SalePrice");
		}
		if (ShowNccSumm)
		{
			ColumnsProvider.Add("SaleSumm");
		}
		if (ShowPrices && ShowNccSumm && ShowDiscount)
		{
			ColumnsProvider.Add("InitialPrice");
		}
		if (ShowNccSumm && ShowDiscount)
		{
			ColumnsProvider.Add("InitialSumm");
			ColumnsProvider.Add("DiscountSum");
		}
		if (ShowNccSumm && ShowDiscount && string.IsNullOrWhiteSpace(GroupBy))
		{
			ColumnsProvider.Add("DiscountPercent");
		}
		if (ShowNCCVATSumm && _showSumms)
		{
			ColumnsProvider.Add("SummWithVAT");
		}
		if (ShowNCCVATSumm && ShowPrices && _showSumms)
		{
			ColumnsProvider.Add("CostPriceWithVAT");
		}
		if (ShowNCCVATSumm && ShowPrices)
		{
			ColumnsProvider.Add("SalePriceWithVAT");
		}
		if (ShowNCCVATSumm)
		{
			ColumnsProvider.Add("SaleSummWithVAT");
		}
		if (ShowNCCVATSumm && _showSumms)
		{
			ColumnsProvider.Add("ProfitWithVAT");
			ColumnsProvider.Add("ProfitPercentBySaleWithVAT");
			ColumnsProvider.Add("ProfitPercentByCostWithVAT");
		}
		if (ShowNCCVATSumm && ShowDiscount)
		{
			ColumnsProvider.Add("DiscountSummWithVAT");
		}
		if (ShowNCCVATSumm && ShowDiscount && string.IsNullOrWhiteSpace(GroupBy))
		{
			ColumnsProvider.Add("DiscountPercentWithVAT");
		}
		if (ShowVATSumms && _showSumms)
		{
			ColumnsProvider.Add("CostVATSumm");
		}
		if (ShowVATSumms && ShowPrices && _showSumms)
		{
			ColumnsProvider.Add("CostVATPrice");
		}
		if (ShowVATSumms)
		{
			ColumnsProvider.Add("SaleVATSumm");
		}
		if (ShowPrices && ShowDiscount)
		{
			ColumnsProvider.Add("InitialPriceVAT");
		}
		if (ShowNCCVATSumm && ShowDiscount)
		{
			ColumnsProvider.Add("InitialSummVAT");
			ColumnsProvider.Add("DiscountVATSumm");
		}
		if ((ShowCurrSumm || ShowNCCVATSumm || ShowNccSumm) && !ShowParties)
		{
			ColumnsProvider.Add("EnvFeeSumma");
		}
		if (ShowCurrSumm && _showSumms)
		{
			ColumnsProvider.Add("CurSumm");
		}
		if (ShowCurrSumm && ShowPrices && _showSumms)
		{
			ColumnsProvider.Add("CurrCostPrice");
		}
		if (ShowCurrSumm && ShowPrices)
		{
			ColumnsProvider.Add("SaleCurPrice");
		}
		if (ShowCurrSumm)
		{
			ColumnsProvider.Add("SaleCurSumm");
		}
		if (ShowCurrSumm && _showSumms)
		{
			ColumnsProvider.Add("CurProfit");
		}
		if (ShowCurrSumm && _showSumms)
		{
			ColumnsProvider.Add("CurProfitPercentBySale");
			ColumnsProvider.Add("CurProfitPercentByCost");
		}
		if (ShowPrices && ShowDiscount)
		{
			ColumnsProvider.Add("InitialPriceCurr");
		}
		if (ShowCurrSumm && ShowDiscount)
		{
			ColumnsProvider.Add("InitialSummCurr");
			ColumnsProvider.Add("DiscountCurSum");
		}
		if (ShowCurrSumm && ShowDiscount && string.IsNullOrWhiteSpace(GroupBy))
		{
			ColumnsProvider.Add("DiscountCurPercent");
		}
		if (_useBonus && ShowBonusPoints && (ShowCurrSumm || ShowNCCVATSumm || ShowNccSumm))
		{
			ColumnsProvider.Add("UsedBonus");
			ColumnsProvider.Add("AccumulatedBonus");
		}
		if (ShowParties && Settings.ParametersService.ShowPartyIncomeDate())
		{
			ColumnsProvider.Add("IncomeDate");
		}
		if (ShowParties && Settings.ParametersService.ShowPartySUPPLIER())
		{
			ColumnsProvider.Add("PartyPartCaption");
		}
		if (ShowParties && Settings.ParametersService.ShowPartnerTaxCode())
		{
			ColumnsProvider.Add("PartyPartTaxCode");
		}
		if (ShowParties && Settings.ParametersService.ShowPartySUPPLIER())
		{
			ColumnsProvider.Add("PartyPartCaption");
		}
		if (ShowParties && Settings.ParametersService.ShowPartyCONTRACT())
		{
			ColumnsProvider.Add("PartyContractCaption");
		}
		if (ShowParties && Settings.ParametersService.ShowPartyDocType())
		{
			ColumnsProvider.Add("PartyDocType");
		}
		if (ShowParties && Settings.ParametersService.ShowPartyDocNum())
		{
			ColumnsProvider.Add("PartyDocNum");
		}
		if (ShowParties && Settings.ParametersService.ShowPartyUSEABLEDATE())
		{
			ColumnsProvider.Add("PartyDocUsableDate");
		}
		if (ShowParties && Settings.ParametersService.ShowPartyCountry())
		{
			ColumnsProvider.Add("PartyDocCountry");
		}
		if (ShowParties && Settings.ParametersService.ShowPartyProducer())
		{
			ColumnsProvider.Add("PartyDocProducer");
		}
		if (ShowParties && Settings.ParametersService.ShowPartyProperty1())
		{
			ColumnsProvider.Add("PartyDocPropertyType1");
			ColumnsProvider.Add("PartyDocPropertyValue1");
		}
		if (ShowParties && Settings.ParametersService.ShowPartyProperty2())
		{
			ColumnsProvider.Add("PartyDocPropertyType2");
			ColumnsProvider.Add("PartyDocPropertyValue2");
		}
	}
}
