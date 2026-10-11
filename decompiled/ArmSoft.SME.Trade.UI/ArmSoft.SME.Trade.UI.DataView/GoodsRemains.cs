using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Converters;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Cache;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.OtherWindows;
using ArmSoft.SME.Trade.UI.ReportViewers.Goods;
using DevExpress.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10049)]
public class GoodsRemains : TradeDataView<GoodsRemainsDataProvider.DataRow, GoodsRemainsDataProvider.Param>
{
	private class DataViewDialog : TradeDataViewParametersDialogWindow
	{
		private readonly GoodsRemains _dataView;

		private readonly DateEditControl _remDate;

		private readonly MultiSelectDropDownControl _storage;

		private readonly DropDownControl _mtGroup;

		private readonly MultiSelectDropDownControl _mtCode;

		private readonly CheckEditExt _showByAditionalQntUnits;

		private readonly CheckEditExt _showSumms;

		private readonly CheckEditExt _showSummsWithVAT;

		private readonly CheckEditExt _showCurrSumms;

		private readonly CheckEditExt _showByDifferentParties;

		private readonly CheckEditExt _detailByStorage;

		private readonly DropDownControl _minQtys;

		private readonly MultiSelectDropDownControl _priceListTypes;

		private readonly CheckEditExt _showLinks;

		private readonly DropDownControl _subTotalType;

		private readonly GoodsFilter _goods;

		public void LoadParams()
		{
			LoadValues();
			ApplyParams(useParamValues: false, isRefreshMode: true);
		}

		public DataViewDialog(GoodsRemains dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			_goods = new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value, "", isShowOnlyInPriceList: true);
			Caption = ArmSoft.SME.Trade.Common.Resources.MatRepAvail;
			bool showForReval = _dataView.ShowForReval;
			bool showAsTransactionPanel = _dataView.ShowAsTransactionPanel;
			Name = _dataView.Name + (showForReval ? "Reval" : "") + (showAsTransactionPanel ? "Panel" : "");
			bool flag = Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode | showAsTransactionPanel)
			{
				AddStoreValuesButton(container);
			}
			if (!showAsTransactionPanel)
			{
				_remDate = AddDateControl(container, "RemDate", ArmSoft.SME.Common.Resources.DateWord, useLongDate: false, isRequired: true, storeValue: true);
				_remDate.EditValue = (useParamValues ? dataView.RemDate : Settings.HostingEnvironment.CurrentDate);
				AddSpecialDateButton(container, _remDate);
			}
			DXTabControl tc = AddTabControl(null);
			LayoutGroup layoutGroup = AddTabedGroup(tc, ArmSoft.SME.Common.Resources.General);
			string storage = ArmSoft.SME.Common.Resources.Storage;
			bool? checkViewOrOperationAccess = true;
			_storage = AddMultiSelectDropDownControl(layoutGroup, "Storage", storage, new ArmSoft.SME.Trade.UI.DropDown.Storages(null, checkViewOrOperationAccess), isRequired: false, storeValue: true);
			_storage.SelectedString = (useParamValues ? dataView.Storage : string.Empty);
			if (!showAsTransactionPanel)
			{
				_mtGroup = AddDropDownControl(layoutGroup, "MTGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: false, allowAdditionButtons: false, storeValue: true);
				_mtGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(GoodsGroup_EditValueChanged).Handler;
				_mtGroup.Code = (useParamValues ? dataView.MTGroup : string.Empty);
				_mtCode = AddMultiSelectDropDownControl(layoutGroup, "MTCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, _goods, isRequired: false, !useParamValues);
				_mtCode.SelectedString = (useParamValues ? dataView.MTCode : string.Empty);
			}
			AddLine(layoutGroup);
			_showByAditionalQntUnits = AddBooleanControl(layoutGroup, "ShowByAditionalQntUnits", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, !showForReval);
			_showByAditionalQntUnits.EditValue = useParamValues && dataView.ShowByAditionalQntUnits;
			_showSumms = AddBooleanControl(layoutGroup, "ShowSumms", ArmSoft.SME.Trade.Common.Resources.ShowNCCSumma, flag);
			_showSumms.EditValue = useParamValues && dataView.ShowSumms;
			_showSumms.IsReadOnly = !flag;
			_showSummsWithVAT = AddBooleanControl(layoutGroup, "ShowSummsWithVAT", ArmSoft.SME.Trade.Common.Resources.ShowNCCSummaWithVAT, flag);
			_showSummsWithVAT.EditValue = useParamValues && dataView.ShowSummsWithVAT;
			_showSummsWithVAT.IsReadOnly = !flag;
			_showCurrSumms = AddBooleanControl(layoutGroup, "ShowCurrSumms", string.Format(ArmSoft.SME.Trade.Common.Resources.ShowCurrSumma, dataView._goodsCurr), flag);
			_showCurrSumms.EditValue = useParamValues && dataView.ShowCurrSumms;
			_showCurrSumms.IsReadOnly = !flag;
			_showByDifferentParties = AddBooleanControl(layoutGroup, "ShowByDifferentParties", ArmSoft.SME.Common.Resources.ByDiffParties, !showForReval);
			_showByDifferentParties.EditValue = useParamValues && dataView.ShowByDifferentParties;
			_showByDifferentParties.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ShowByDifferentParties_EditValueChanged).Handler;
			AddLine(layoutGroup);
			_detailByStorage = AddBooleanControl(layoutGroup, "DetailByStorage", ArmSoft.SME.Trade.Common.Resources.DetailByStorage, !showForReval);
			_detailByStorage.EditValue = useParamValues && dataView.DetailByStorage;
			_detailByStorage.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(DetailByStorage_EditValueChanged).Handler;
			_minQtys = AddDropDownControl(layoutGroup, "MinQtys", ArmSoft.SME.Common.Resources.MinQtys, new SmartEnum(typeof(AvailabilityMinQtysSE)), isRequired: false, allowAdditionButtons: false, !showForReval);
			_minQtys.Code = (useParamValues ? dataView.MinQtys : "");
			_minQtys.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ShowMinQtys_EditValueChanged).Handler;
			AddLine(layoutGroup);
			_priceListTypes = AddMultiSelectDropDownControl(layoutGroup, "PriceListTypes", ArmSoft.SME.Trade.Common.Resources.ShowPricesByPT, new ArmSoft.SME.Trade.UI.DropDown.PriceTypes(), isRequired: false, !showForReval);
			_priceListTypes.SelectedString = (useParamValues ? dataView.PriceListTypes : string.Empty);
			AddLine(layoutGroup);
			_showLinks = AddBooleanControl(layoutGroup, "ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumn, storeValue: true);
			_showLinks.EditValue = useParamValues && dataView.ShowLinks;
			_subTotalType = AddDropDownControl(layoutGroup, "SubTotalType", ArmSoft.SME.Common.Resources.ShowSubTotals, new SmartEnum(typeof(SubTotalTypesSE)), isRequired: false, allowAdditionButtons: false, storeValue: true);
			_subTotalType.Code = (useParamValues ? dataView.SubTotalType : "");
			_subTotalType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(SubTotalType_EditValueChanged).Handler;
			if (!isRefreshMode)
			{
				AddColumnsSelectionTab(tc, layoutGroup, null);
			}
		}

		private void GoodsGroup_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue)
			{
				_goods.Group = (string)e.NewValue;
			}
		}

		private void ShowByDifferentParties_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (_showByDifferentParties.IsChecked.HasValue && _showByDifferentParties.IsChecked.Value)
			{
				_detailByStorage.IsChecked = true;
				_minQtys.Code = "";
			}
		}

		private void DetailByStorage_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (_detailByStorage.IsChecked.HasValue && !_detailByStorage.IsChecked.Value)
			{
				_showByDifferentParties.IsChecked = false;
			}
		}

		private void SubTotalType_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(_subTotalType.Code) && _subTotalType.DoValidate() && _subTotalType.Code == "1")
			{
				_detailByStorage.IsChecked = true;
				_detailByStorage.IsReadOnly = true;
			}
			else
			{
				_detailByStorage.IsReadOnly = false;
			}
		}

		private void ShowMinQtys_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(_minQtys.Code) && _minQtys.DoValidate())
				{
					_showByDifferentParties.IsChecked = false;
				}
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				if (!_dataView.ShowAsTransactionPanel)
				{
					args.isValid = TradeDataViewParametersDialogWindow.CheckPeriodForActualness(_remDate.EditValue.Value);
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
			if (!_dataView.ShowAsTransactionPanel)
			{
				_dataView.RemDate = _remDate.EditValue.Value;
				_dataView.MTGroup = _mtGroup.Code;
				_dataView.MTCode = _mtCode.SelectedString;
			}
			_dataView.Storage = _storage.SelectedString;
			_dataView.ShowByAditionalQntUnits = _showByAditionalQntUnits.IsChecked.Value;
			_dataView.ShowSumms = _showSumms.IsChecked.Value;
			_dataView.ShowSummsWithVAT = _showSummsWithVAT.IsChecked.Value;
			_dataView.ShowCurrSumms = _showCurrSumms.IsChecked.Value;
			_dataView.ShowByDifferentParties = _showByDifferentParties.IsChecked.Value;
			_dataView.DetailByStorage = _detailByStorage.IsChecked.Value;
			_dataView.MinQtys = _minQtys.Code;
			_dataView.PriceListTypes = _priceListTypes.SelectedString;
			_dataView.ShowLinks = _showLinks.IsChecked.Value;
			_dataView.SubTotalType = _subTotalType.Code;
		}
	}

	private readonly string _goodsCurr;

	private List<string> _dynamicColumns = new List<string>();

	private List<ViewColumn> potentiallyColumns;

	public override string HelpId => "Purchases_Reports_Remains";

	public override bool DialogSupported => true;

	public override string Caption => (ShowByDifferentParties ? ArmSoft.SME.Trade.Common.Resources.MatRepAvailByParties : ArmSoft.SME.Trade.Common.Resources.MatRepAvail) + " - " + RemDate.FormatDDMMYY();

	protected override bool AllowFindRowByMTBarcode => true;

	protected override string MTCodeColumnName => "MTCode";

	public override LoadIndication LoadingIndication
	{
		get
		{
			if (!ShowAsTransactionPanel)
			{
				return LoadIndication.AlwaysInside;
			}
			return LoadIndication.None;
		}
	}

	public override List<ViewColumn> PotentiallyColumns => potentiallyColumns;

	public override bool AllowEditSelected => Settings.UserAccessService.GetDirectoryPermission(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods.Value).AllowEdit;

	public DateOnly RemDate
	{
		get
		{
			return (DateOnly)Parameters["RemDate"].Value;
		}
		set
		{
			Parameters["RemDate"].Value = value;
		}
	}

	public string Storage
	{
		get
		{
			return (string)Parameters["Storage"].Value;
		}
		set
		{
			Parameters["Storage"].Value = value;
		}
	}

	public string MTGroup
	{
		get
		{
			return (string)Parameters["MTGroup"].Value;
		}
		set
		{
			Parameters["MTGroup"].Value = value;
		}
	}

	public string MTCode
	{
		get
		{
			return (string)Parameters["MTCode"].Value;
		}
		set
		{
			Parameters["MTCode"].Value = value;
		}
	}

	public bool ShowByAditionalQntUnits
	{
		get
		{
			return (bool)Parameters["ShowByAditionalQntUnits"].Value;
		}
		set
		{
			Parameters["ShowByAditionalQntUnits"].Value = value;
		}
	}

	public bool ShowSumms
	{
		get
		{
			return (bool)Parameters["ShowSumms"].Value;
		}
		set
		{
			Parameters["ShowSumms"].Value = value;
		}
	}

	public bool ShowSummsWithVAT
	{
		get
		{
			return (bool)Parameters["ShowSummsWithVAT"].Value;
		}
		set
		{
			Parameters["ShowSummsWithVAT"].Value = value;
		}
	}

	public bool ShowCurrSumms
	{
		get
		{
			return (bool)Parameters["ShowCurrSumms"].Value;
		}
		set
		{
			Parameters["ShowCurrSumms"].Value = value;
		}
	}

	public bool ShowByDifferentParties
	{
		get
		{
			return (bool)Parameters["ShowByDifferentParties"].Value;
		}
		set
		{
			Parameters["ShowByDifferentParties"].Value = value;
		}
	}

	public bool DetailByStorage
	{
		get
		{
			return (bool)Parameters["DetailByStorage"].Value;
		}
		set
		{
			Parameters["DetailByStorage"].Value = value;
		}
	}

	public bool ShowLinks
	{
		get
		{
			return (bool)Parameters["ShowLinks"].Value;
		}
		set
		{
			Parameters["ShowLinks"].Value = value;
		}
	}

	public string MinQtys
	{
		get
		{
			return (string)Parameters["MinQtys"].Value;
		}
		set
		{
			Parameters["MinQtys"].Value = value;
		}
	}

	public string PriceListTypes
	{
		get
		{
			return (string)Parameters["PriceListTypes"].Value;
		}
		set
		{
			Parameters["PriceListTypes"].Value = value;
		}
	}

	public string SubTotalType
	{
		get
		{
			return (string)Parameters["SubTotalType"].Value;
		}
		set
		{
			Parameters["SubTotalType"].Value = value;
		}
	}

	public bool ShowForReval
	{
		get
		{
			return (bool)Parameters["ShowForReval"].Value;
		}
		set
		{
			Parameters["ShowForReval"].Value = value;
		}
	}

	public GoodsRemains()
	{
		DataProvider = new GoodsRemainsDataProvider(TradeSettings.CreateTradeClient());
		_goodsCurr = Settings.ParametersService.MTCurr();
		AddParameter("RemDate", ArmSoft.SME.Common.Resources.DateWord, Settings.HostingEnvironment.CurrentDate, "dd/MM/yy");
		AddParameter("Storage", ArmSoft.SME.Common.Resources.Storage, string.Empty);
		AddParameter("MTGroup", ArmSoft.SME.Common.Resources.Group, string.Empty);
		AddParameter("MTCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, string.Empty);
		AddParameter("ShowByAditionalQntUnits", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, false);
		AddParameter("ShowSumms", ArmSoft.SME.Trade.Common.Resources.ShowNCCSumma, false);
		AddParameter("ShowSummsWithVAT", ArmSoft.SME.Trade.Common.Resources.ShowNCCSummaWithVAT, false);
		AddParameter("ShowCurrSumms", string.Format(ArmSoft.SME.Trade.Common.Resources.ShowCurrSumma, _goodsCurr), false);
		AddParameter("ShowByDifferentParties", ArmSoft.SME.Common.Resources.ByDiffParties, false);
		AddParameter("DetailByStorage", ArmSoft.SME.Trade.Common.Resources.DetailByStorage, false);
		AddParameter("MinQtys", ArmSoft.SME.Common.Resources.MinQtys, string.Empty);
		AddParameter("PriceListTypes", ArmSoft.SME.Trade.Common.Resources.ShowPricesByPT, string.Empty);
		AddParameter("ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumn, true);
		AddParameter("SubTotalType", ArmSoft.SME.Common.Resources.ShowSubTotals, string.Empty);
		AddParameter("ShowForReval", "", false);
		FillColumnsFromDataProvider();
		Columns["ImageExists"].Converter = new BooleanToImageExists();
		potentiallyColumns = new List<ViewColumn>();
		potentiallyColumns.AddRange(Columns.ViewList);
		potentiallyColumns.AddRange(MaterialCache.GetDataViewColumns(ArmSoft.SME.Trade.Common.Resources.MaterialValue, "", ""));
		potentiallyColumns.AddRange(ProducerCache.GetDataViewColumns(ArmSoft.SME.Common.Resources.Producer, "", ""));
	}

	protected override void SetColumnsProperties()
	{
		base.SetColumnsProperties();
		foreach (ViewColumn view in Columns.ViewList)
		{
			if (view.Name.Substring(0, 3) == "PT_" || view.Name.Substring(0, 3) == "PS_")
			{
				Columns[view.Name].IsSystem = true;
				Columns[view.Name].FooterSum = view.Name.Substring(0, 3) == "PS_";
			}
		}
		Columns["UnitCode"].IsSystem = true;
		Columns["Storage"].IsSystem = true;
		Columns["StorageCaption"].IsSystem = true;
		Columns["MinQty"].IsSystem = true;
		Columns["DiffQty"].IsSystem = true;
		Columns["MaxQty"].IsSystem = true;
		Columns["OrderQty"].IsSystem = true;
		Columns["OrderComment"].IsSystem = true;
		Columns["Cost"].IsSystem = true;
		Columns["CostSumm"].IsSystem = true;
		Columns["CostWithVAT"].IsSystem = true;
		Columns["CostSummWithVAT"].IsSystem = true;
		Columns["CurrCost"].IsSystem = true;
		Columns["CurrCostSumm"].IsSystem = true;
		Columns["GroupCodeCaption"].IsSystem = true;
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

	public void LoadParams()
	{
		((DataViewDialog)CreateDialog(useParamValues: false, isRefreshMode: true)).LoadParams();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		_dynamicColumns = PriceListTypes.SplitSelected().SelectMany((string priceType) => new string[2]
		{
			"PT_" + priceType,
			"PS_" + priceType
		}).ToList();
		foreach (ViewColumn view in Columns.ViewList)
		{
			if (view.Name.Substring(0, 3) == "PT_" || view.Name.Substring(0, 3) == "PS_")
			{
				dvp.Set_ColumnVisible(view.Name, _dynamicColumns.Contains(view.Name));
			}
		}
		dvp.RemoveGrouping();
		dvp.Set_ColumnVisible("Cost", ShowSumms);
		dvp.Set_ColumnVisible("CostSumm", ShowSumms);
		dvp.Set_ColumnVisible("CostWithVAT", ShowSummsWithVAT);
		dvp.Set_ColumnVisible("CostSummWithVAT", ShowSummsWithVAT);
		dvp.Set_ColumnVisible("CurrCost", ShowCurrSumms);
		dvp.Set_ColumnVisible("CurrCostSumm", ShowCurrSumms);
		dvp.Set_ColumnVisible("Storage", DetailByStorage);
		dvp.Set_ColumnVisible("StorageCaption", DetailByStorage);
		dvp.Set_ColumnVisible("MTCode", !ShowAsTransactionPanel);
		dvp.Set_ColumnVisible("Caption", !ShowAsTransactionPanel);
		dvp.Set_ColumnVisible("Image", !ShowAsTransactionPanel);
		dvp.Set_ColumnVisible("ImageExists", !ShowAsTransactionPanel);
		dvp.Set_ColumnVisible("QuantityInTransit", Settings.ParametersService.MTMoveWithConfirmation());
		dvp.Set_ColumnVisible("IncomeDate", ShowByDifferentParties && Settings.ParametersService.ShowPartyIncomeDate());
		dvp.Set_ColumnVisible("PartyPartCaption", ShowByDifferentParties && Settings.ParametersService.ShowPartySUPPLIER());
		dvp.Set_ColumnVisible("PartyContractCaption", ShowByDifferentParties && Settings.ParametersService.ShowPartyCONTRACT());
		dvp.Set_ColumnVisible("PartyPartTaxCode", Settings.ParametersService.ShowPartnerTaxCode() && ShowByDifferentParties && Settings.ParametersService.ShowPartyCONTRACT());
		dvp.Set_ColumnVisible("PartyDocType", ShowByDifferentParties && Settings.ParametersService.ShowPartyDocType());
		dvp.Set_ColumnVisible("PartyDocNum", ShowByDifferentParties && Settings.ParametersService.ShowPartyDocNum());
		dvp.Set_ColumnVisible("PartyDocUsableDate", ShowByDifferentParties && Settings.ParametersService.ShowPartyUSEABLEDATE());
		dvp.Set_ColumnVisible("PartyDocCountry", ShowByDifferentParties && Settings.ParametersService.ShowPartyCountry());
		dvp.Set_ColumnVisible("PartyDocProducer", ShowByDifferentParties && Settings.ParametersService.ShowPartyProducer());
		dvp.Set_ColumnVisible("PartyDocPropertyType1", ShowByDifferentParties && Settings.ParametersService.ShowPartyProperty1());
		dvp.Set_ColumnVisible("PartyDocPropertyValue1", ShowByDifferentParties && Settings.ParametersService.ShowPartyProperty1());
		dvp.Set_ColumnVisible("PartyDocPropertyType2", ShowByDifferentParties && Settings.ParametersService.ShowPartyProperty2());
		dvp.Set_ColumnVisible("PartyDocPropertyValue2", ShowByDifferentParties && Settings.ParametersService.ShowPartyProperty2());
		dvp.Set_ColumnVisible("GroupCodeCaption", value: false);
		dvp.Set_ColumnVisible("MinQty", !string.IsNullOrWhiteSpace(MinQtys));
		dvp.Set_ColumnVisible("MaxQty", !string.IsNullOrWhiteSpace(MinQtys));
		dvp.Set_ColumnVisible("OrderQty", !string.IsNullOrWhiteSpace(MinQtys));
		dvp.Set_ColumnVisible("OrderComment", !string.IsNullOrWhiteSpace(MinQtys));
		dvp.Set_ColumnVisible("DiffQty", !string.IsNullOrWhiteSpace(MinQtys));
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("MTCode");
			dvp.Set_ColumnSortOrder("Storage");
		}
		if (!string.IsNullOrWhiteSpace(SubTotalType))
		{
			dvp.Set_ColumnGroupSummary("Quantity", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("CostSumm", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("CostSummWithVAT", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("CurrCostSumm", SummaryItemType.Sum);
			switch (SubTotalType)
			{
			case "1":
				dvp.GroupByColumns("Storage");
				break;
			case "2":
				dvp.GroupByColumns("GroupCodeCaption");
				break;
			case "3":
				dvp.GroupByColumns("MTCode");
				break;
			}
		}
		Columns.AddDirectoryLinkColumn(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods, "MTCode", "Caption", "", ArmSoft.SME.Common.Resources.Product, ShowLinks);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.RemDate = RemDate;
		ParametersProvider.Storage = Storage;
		ParametersProvider.MTGroup = MTGroup;
		ParametersProvider.MTCode = MTCode;
		ParametersProvider.ShowByAditionalQntUnits = ShowByAditionalQntUnits;
		ParametersProvider.ShowSumms = ShowSumms;
		ParametersProvider.ShowSummsWithVAT = ShowSummsWithVAT;
		ParametersProvider.ShowCurrSumms = ShowCurrSumms;
		ParametersProvider.ShowByDifferentParties = ShowByDifferentParties;
		ParametersProvider.DetailByStorage = DetailByStorage;
		ParametersProvider.MinQtys = MinQtys;
		ParametersProvider.PriceListTypes = PriceListTypes;
		ParametersProvider.SubTotalType = SubTotalType;
	}

	protected override void AddControlsInEditSelectedDialog(DialogWindow dialog, out bool doRefresh)
	{
		base.AddControlsInEditSelectedDialog(dialog, out doRefresh);
		EditSelectedHelper.AddGoodsControls(dialog);
	}

	protected override void EditSelectedRows(List<PropertyWithValue> properties, bool doRefresh, bool withoutValidation = false)
	{
		List<GoodsRemainsDataProvider.DataRow> selectedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<GoodsRemainsDataProvider.DataRow>();
		List<int> list = new List<int>();
		foreach (GoodsRemainsDataProvider.DataRow item in selectedRows)
		{
			list.Add(item.MTID);
		}
		EditSelectedDirectories(properties, list, null, ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods.Value, out var errorRepViewer, withoutValidation);
		if (errorRepViewer.HasErrors)
		{
			errorRepViewer.Show();
		}
	}

	public override string GetDefaultFunction()
	{
		return "ShowMTTAccount";
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepTAccount).AllowView)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowMTTAccount", ArmSoft.SME.Trade.Common.Resources.TAccount, ShowMTTAccount, "", "", storeOpContext: true);
			}
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MTPrices).AllowView)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowPricesFormation", ArmSoft.SME.Trade.Common.Resources.PricesFormation, ShowPricesFormation);
			}
			if (DetailByStorage || ShowByDifferentParties)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "CreateDoc", ArmSoft.SME.Common.Resources.CreateDoc, CreateDoc, "doc_new_16.svg");
			}
			else
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowCreatePurchase", ArmSoft.SME.Common.Resources.CreateDoc, ShowCreatePurchase, "doc_new_16.svg");
			}
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "PrintLabel", ArmSoft.SME.Trade.Common.Resources.PrintLabel, PrintLabel);
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsBySegment).AllowOperation)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ChangeGoodsBySegments", ArmSoft.SME.Trade.Common.Resources.AddToSegment, ChangeGoodsBySegments);
			}
			Settings.HostingEnvironment.CurrentViewPanel.AddContextMenuGroup("Export", ArmSoft.SME.Common.Resources.Export, "export_16.svg");
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("Export", "ExportForTerminal", ArmSoft.SME.Trade.Common.Resources.ByTerminalFormat, ExportForTerminal, "", "", storeOpContext: true);
			if (!ShowByDifferentParties && !DetailByStorage)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("Export", "FuncExportForScale", ArmSoft.SME.Trade.Common.Resources.ByScaleFormat, FuncExportForScale, "", "", storeOpContext: true);
			}
			Settings.HostingEnvironment.CurrentViewPanel.AddContextSeparator("Export");
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("Export", "GroupExportForAll", ArmSoft.SME.Trade.Common.Resources.GroupElScalesAndTerminals, GroupExportForAll);
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.OrderMinMax).AllowView)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextSeparator("MinMax");
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("MinMax", "FuncOrderMinMax", ArmSoft.SME.Trade.Common.Resources.MTOrderMinMax, FuncOrderMinMax);
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("MinMax", "EditOrderMinMax", ArmSoft.SME.Trade.Common.Resources.EditOrderMinMax, EditOrderMinMax);
				Settings.HostingEnvironment.CurrentViewPanel.AddContextSeparator("MinMax");
			}
		}
	}

	private void GroupExportForAll()
	{
		new GroupElScaleAndTerminalExportWindow(GetSourceListForElScales(), GetSourceListForTerminals(), (!ShowByDifferentParties && !DetailByStorage) ? null : DeviceTypesSE.Terminal.Value).ShowDialog();
	}

	private List<MaterialCacheRow> GetSourceListForElScales()
	{
		List<MaterialCacheRow> list = new List<MaterialCacheRow>();
		foreach (GoodsRemainsDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRemainsDataProvider.DataRow>())
		{
			MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(selectedOrFocusedRow.MTID);
			if (itemById != null && itemById.Type == ((short)1).ToString() && itemById.IsWeight)
			{
				list.Add(itemById);
			}
		}
		return list;
	}

	private List<TerminalRow> GetSourceListForTerminals()
	{
		List<TerminalRow> list = new List<TerminalRow>();
		foreach (GoodsRemainsDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRemainsDataProvider.DataRow>())
		{
			TerminalRow item = new TerminalRow
			{
				Id = selectedOrFocusedRow.MTID,
				Code = selectedOrFocusedRow.MTCode,
				Name = selectedOrFocusedRow.Caption,
				Unit = selectedOrFocusedRow.UnitCode,
				AlternativeUnit = string.Empty,
				BarCode = null,
				Quantity = selectedOrFocusedRow.Quantity
			};
			list.Add(item);
		}
		return list;
	}

	protected override void SetCacheRow(ExtendableRow row)
	{
		GoodsRemainsDataProvider.DataRow dataRow = row as GoodsRemainsDataProvider.DataRow;
		dataRow.MaterialCacheRow = TradeSettings.TradeCacheService.Material.GetItemById(dataRow.MTID);
		dataRow.ProducerCacheRow = TradeSettings.TradeCacheService.Producer.GetItem(dataRow.MaterialCacheRow?.Producer);
	}

	private void PublishToB2B()
	{
	}

	private void ShowMTTAccount()
	{
		List<GoodsRemainsDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRemainsDataProvider.DataRow>();
		MTAccountReportViewer mTAccountReportViewer;
		if (selectedOrFocusedRows.Count == 1)
		{
			mTAccountReportViewer = new MTAccountReportViewer();
			mTAccountReportViewer.GoodCode = selectedOrFocusedRows[0].MTCode;
			if (DetailByStorage)
			{
				mTAccountReportViewer.Storage = selectedOrFocusedRows[0].Storage;
			}
		}
		else
		{
			List<ListOfSelectedMaterialStorage> list = new List<ListOfSelectedMaterialStorage>();
			foreach (GoodsRemainsDataProvider.DataRow item in selectedOrFocusedRows)
			{
				ListOfSelectedMaterialStorage listOfSelectedMaterialStorage = new ListOfSelectedMaterialStorage();
				listOfSelectedMaterialStorage.GoodCode = item.MTCode;
				if (DetailByStorage)
				{
					listOfSelectedMaterialStorage.Storage = item.Storage;
				}
				list.Add(listOfSelectedMaterialStorage);
			}
			mTAccountReportViewer = new MTAccountReportViewer(isForSelectedRows: true, list);
		}
		mTAccountReportViewer.ShowFromTurns = true;
		mTAccountReportViewer.ShowAltUnit = ShowByAditionalQntUnits;
		mTAccountReportViewer.ShowSumms = ShowSumms;
		mTAccountReportViewer.ShowSummsWithVAT = ShowSummsWithVAT;
		mTAccountReportViewer.ShowCurrSumms = ShowCurrSumms;
		mTAccountReportViewer.ShowParties = ShowByDifferentParties;
		mTAccountReportViewer.StartDate = Settings.HostingEnvironment.DateBegin;
		mTAccountReportViewer.EndDate = Settings.HostingEnvironment.DateEnd;
		mTAccountReportViewer.Show(useParamValues: true);
	}

	private void ShowPricesFormation()
	{
		List<PricesFormationWindow.GoodsRow> list = new List<PricesFormationWindow.GoodsRow>();
		foreach (GoodsRemainsDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRemainsDataProvider.DataRow>())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(selectedOrFocusedRow.MTCode, throwExceptionNotExits: true);
			list.Add(new PricesFormationWindow.GoodsRow(selectedOrFocusedRow.MTID, selectedOrFocusedRow.MTCode, item.Unit, item.AltUnit));
		}
		new PricesFormationWindow(list, "PrF_" + Name).Invoke();
	}

	private void PrintLabel()
	{
		List<ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow> list = new List<ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow>();
		ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow goodLabelRow = new ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow();
		foreach (GoodsRemainsDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRemainsDataProvider.DataRow>())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(selectedOrFocusedRow.MTCode);
			if (item != null && item.Type == GoodsTypesSE.Good)
			{
				goodLabelRow = InitContextFunctionsHelper.BuildLabelGoodRow(item, selectedOrFocusedRow.PartyDocPropertyValue1Code, selectedOrFocusedRow.PartyDocPropertyValue1, selectedOrFocusedRow.PartyDocPropertyValue2Code, selectedOrFocusedRow.PartyDocPropertyValue2);
				goodLabelRow.Quantity = selectedOrFocusedRow.Quantity;
				list.Add(goodLabelRow);
			}
		}
		new PrintLabelDialog(null, null, list, isFromGoodsAvailability: true).Invoke();
	}

	private void ChangeGoodsBySegments()
	{
		InitContextFunctionsHelper.ChangeGoodsBySegments((from p in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRemainsDataProvider.DataRow>()
			select p.MTCode).ToList());
	}

	private void FuncExportForScale()
	{
		List<MaterialCacheRow> list = new List<MaterialCacheRow>();
		if (!Settings.HostingEnvironment.CurrentViewPanel.GetSelectedAny())
		{
			GoodsRemainsDataProvider.DataRow dataRow = (GoodsRemainsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(dataRow.MTCode);
			if (item != null && item.Type == GoodsTypesSE.Good && item.IsWeight)
			{
				list.Add(item);
			}
		}
		else
		{
			foreach (GoodsRemainsDataProvider.DataRow selectedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<GoodsRemainsDataProvider.DataRow>())
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(selectedRow.MTCode);
				if (item != null && item.Type == GoodsTypesSE.Good && item.IsWeight)
				{
					list.Add(item);
				}
			}
		}
		new ElScaleExportWindow(list).ShowDialog();
	}

	private void FuncOrderMinMax()
	{
		GoodsRemainsDataProvider.DataRow dataRow = (GoodsRemainsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			OrderMinMax orderMinMax = new OrderMinMax();
			orderMinMax.MTCode = dataRow.MTCode;
			orderMinMax.Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
		}
	}

	private void EditOrderMinMax()
	{
		GoodsRemainsDataProvider.DataRow dataRow = (GoodsRemainsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow == null)
		{
			return;
		}
		if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(dataRow.Storage))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, dataRow.Storage), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return;
		}
		string storageCode = ((!string.IsNullOrWhiteSpace(dataRow.Storage) || Storage.Contains(",")) ? dataRow.Storage : Storage);
		OrderMinMaxDataProvider.DataRow.OrderMinMaxRowKey value = new OrderMinMaxDataProvider.DataRow.OrderMinMaxRowKey
		{
			StorageCode = storageCode,
			MTID = TradeSettings.TradeCacheService.Material.GetItem(dataRow.MTCode, throwExceptionNotExits: true).Id
		};
		ArmSoft.SME.Trade.Common.Models.Directories.OrderMinMax orderMinMax = Settings.DirectoryService.Load<ArmSoft.SME.Trade.Common.Models.Directories.OrderMinMax>(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.OrderMinMax, JsonSerializer.Serialize(value), withoutAccessCheck: false, throwExceptionIfDoesNotExist: false);
		if (orderMinMax == null)
		{
			orderMinMax = (ArmSoft.SME.Trade.Common.Models.Directories.OrderMinMax)Settings.DirectoryService.Create(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.OrderMinMax);
			orderMinMax.StorageCode = storageCode;
			orderMinMax.Code = dataRow.MTCode;
			Settings.DirectoryService.ShowWindow(orderMinMax, isPOWindow: false, WindowViewMode.NewCreate);
		}
		else
		{
			Settings.DirectoryService.ShowWindow(orderMinMax, isPOWindow: false, WindowViewMode.Edit);
		}
	}

	private void CreateDoc()
	{
		if (!ShowByDifferentParties && !DetailByStorage)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidCreateDocFromGoodsAvail);
		}
		int num = 16;
		List<short> list = new List<short>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<short> span = CollectionsMarshal.AsSpan(list);
		span[0] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value;
		span[1] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value;
		span[2] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value;
		span[3] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder.Value;
		span[4] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value;
		span[5] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value;
		span[6] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value;
		span[7] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTExpense.Value;
		span[8] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value;
		span[9] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value;
		span[10] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveOrder.Value;
		span[11] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTaking.Value;
		span[12] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTakingRetail.Value;
		span[13] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTRevaluation.Value;
		span[14] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.InvoiceForPay.Value;
		span[15] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value;
		List<short> list2 = list;
		if (!Settings.ParametersService.MTMoveWithConfirmation())
		{
			list2.Remove(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value);
		}
		if (ShowByDifferentParties)
		{
			list2.Remove(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTaking.Value);
			list2.Remove(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTakingRetail.Value);
		}
		if (!ShowByDifferentParties)
		{
			list2.Remove(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTRevaluation.Value);
		}
		ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE documentTypeSE = InitContextFunctionsHelper.ChooseDocTypeFromMenu(list2);
		if (!(documentTypeSE == null))
		{
			bool fillAvailQty = InitContextFunctionsHelper.FillAllAvailabilityAvailabilityOrTurnOver(documentTypeSE);
			if ((short)documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value)
			{
				InitContextFunctionsHelper.CreateNewMTIncomeDoc(RemDate);
			}
			else if ((short)documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value)
			{
				InitContextFunctionsHelper.CreateNewMTIncomeRetailDoc(RemDate, isFromRetail: false);
			}
			else if ((short)documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value)
			{
				InitContextFunctionsHelper.CreateNewMTReturnToSupplierDoc(fillAvailQty, RemDate, ShowByDifferentParties);
			}
			else if ((short)documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder.Value)
			{
				CreateNewMTPurchaseDoc();
			}
			else if ((short)documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value)
			{
				InitContextFunctionsHelper.CreateNewMTInvoiceDoc(fillAvailQty, RemDate, ShowByDifferentParties);
			}
			else if ((short)documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
			{
				InitContextFunctionsHelper.CreateNewMTBillDoc(fillAvailQty, RemDate, ShowByDifferentParties);
			}
			else if ((short)documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value)
			{
				InitContextFunctionsHelper.CreateNewMTInvoiceRetailDoc(fillAvailQty, RemDate, ShowByDifferentParties);
			}
			else if ((short)documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTExpense.Value)
			{
				InitContextFunctionsHelper.CreateNewMTExpenseDoc(fillAvailQty, RemDate, ShowByDifferentParties);
			}
			else if ((short)documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value)
			{
				InitContextFunctionsHelper.CreateNewMTMoveDoc(fillAvailQty, RemDate, ShowByDifferentParties);
			}
			else if ((short)documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value)
			{
				InitContextFunctionsHelper.CreateNewMTMoveWithConfirmationDoc(fillAvailQty, RemDate, ShowByDifferentParties);
			}
			else if ((short)documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveOrder.Value)
			{
				InitContextFunctionsHelper.CreateNewMTMoveOrderDoc(fillAvailQty, RemDate, ShowByDifferentParties, isFromTurnOver: false, MinQtys);
			}
			else if ((short)documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTaking.Value)
			{
				InitContextFunctionsHelper.CreateNewMTStockTakingDoc(fillAvailQty, RemDate);
			}
			else if ((short)documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTakingRetail.Value)
			{
				InitContextFunctionsHelper.CreateNewMTStockTakingRetailDoc(fillAvailQty, RemDate);
			}
			else if ((short)documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTRevaluation.Value)
			{
				InitContextFunctionsHelper.CreateNewMTRevaluationDoc();
			}
			else if ((short)documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.InvoiceForPay.Value)
			{
				InitContextFunctionsHelper.CreateNewInvoiceForPayDoc(fillAvailQty, isFromTurnOver: false);
			}
			else if ((short)documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value)
			{
				InitContextFunctionsHelper.CreateNewMTComplectionDoc(RemDate);
			}
		}
	}

	private void ShowCreatePurchase()
	{
		try
		{
			ContextMenu contextMenu = new ContextMenu();
			contextMenu.FontFamily = Settings.FontFamily;
			contextMenu.FontSize = Settings.GetFontSize();
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PurchaseOrdersDocumentsJournal.Value).AllowView)
			{
				MenuItem menuItem = new MenuItem
				{
					Header = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder.Caption,
					Tag = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder.Value
				};
				menuItem.Click += CreateNewMTPurchaseOrderDoc;
				contextMenu.Items.Add(menuItem);
				menuItem = new MenuItem
				{
					Header = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder.Caption + " " + ArmSoft.SME.Trade.Common.Resources.BySuppliers,
					Tag = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder.Value
				};
				menuItem.Click += CreateNewMTPurchaseOrdersBySuppliersDoc;
				contextMenu.Items.Add(menuItem);
			}
			contextMenu.ToolTip = ArmSoft.SME.Common.Resources.NewDocuments;
			contextMenu.PlacementTarget = Settings.HostingEnvironment.LayoutManager;
			contextMenu.Placement = PlacementMode.Center;
			contextMenu.VerticalOffset = 0.0;
			contextMenu.HorizontalOffset = 0.0;
			contextMenu.IsOpen = contextMenu.Items.Count > 0;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CreateNewMTPurchaseOrderDoc(object sender, RoutedEventArgs e)
	{
		try
		{
			CreateNewMTPurchaseDoc();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public void CreateNewMTPurchaseDoc()
	{
		MTPurchaseOrder mTPurchaseOrder = Settings.DocumentService.Create<MTPurchaseOrder>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder);
		mTPurchaseOrder.DocDate = Settings.HostingEnvironment.CurrentDate;
		mTPurchaseOrder.DocNum = Settings.ParametersService.MTPurchaseOrderNumber();
		mTPurchaseOrder.OwnerID = Settings.LoginService.UserId;
		mTPurchaseOrder.Owner = Settings.LoginService.UserName;
		List<GoodsRemainsDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRemainsDataProvider.DataRow>();
		IEnumerable<GoodsRemainsDataProvider.DataRow> enumerable2;
		if (!string.IsNullOrWhiteSpace(MinQtys))
		{
			IEnumerable<GoodsRemainsDataProvider.DataRow> enumerable = selectedOrFocusedRows;
			enumerable2 = enumerable;
		}
		else
		{
			enumerable2 = from r in selectedOrFocusedRows
				group r by new { r.MTCode, r.UnitCode } into g
				select g.First();
		}
		foreach (GoodsRemainsDataProvider.DataRow item in enumerable2)
		{
			AddCurrentRowToMTPurchaseOrderList(mTPurchaseOrder, item);
		}
		Settings.DocumentService.CreateWindow(mTPurchaseOrder).Show();
	}

	private void CreateNewMTPurchaseOrdersBySuppliersDoc(object sender, RoutedEventArgs e)
	{
		try
		{
			if (Settings.HostingEnvironment.CurrentViewPanel != null)
			{
				CreateMTPurchaseOrdersBySuppliers();
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OpenPOJournal, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					PurchaseOrdersDocumentsJournalDataView purchaseOrdersDocumentsJournalDataView = new PurchaseOrdersDocumentsJournalDataView();
					purchaseOrdersDocumentsJournalDataView.StartDate = Settings.HostingEnvironment.CurrentDate;
					purchaseOrdersDocumentsJournalDataView.EndDate = Settings.HostingEnvironment.CurrentDate;
					purchaseOrdersDocumentsJournalDataView.DocOwner = Settings.CoreCacheService.User.GetNameById(Settings.LoginService.UserId);
					purchaseOrdersDocumentsJournalDataView.Show(useParamValues: true, openAsChild: false);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CreateMTPurchaseOrdersBySuppliers()
	{
		Dictionary<string, MTPurchaseOrder> dictionary = new Dictionary<string, MTPurchaseOrder>();
		string text = string.Empty;
		string text2 = Settings.ParametersService.MTPurchaseOrderNumber();
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		foreach (GoodsRemainsDataProvider.DataRow row in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRemainsDataProvider.DataRow>())
		{
			IReadOnlyList<PartnerGoodCacheRow> list = TradeSettings.TradeCacheService.PartnerGood.GetList((PartnerGoodCacheRow p) => p.MTID == row.MTID);
			if (list.Count > 0)
			{
				text = TradeSettings.TradeCacheService.Partner.GetItemById(list[0].PartID)?.PartCode;
			}
			MTPurchaseOrder doc;
			if (dictionary.TryGetValue(text, out var value))
			{
				doc = value;
				AddCurrentRowToMTPurchaseOrderList(doc, row);
				continue;
			}
			doc = Settings.DocumentService.Create<MTPurchaseOrder>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder);
			doc.DocDate = Settings.HostingEnvironment.CurrentDate;
			doc.DocNum = text2;
			doc.OwnerID = Settings.LoginService.UserId;
			doc.Owner = Settings.LoginService.UserName;
			if (string.IsNullOrWhiteSpace(text))
			{
				doc.POStates = POStatesSE.EmptySupplier;
			}
			else
			{
				PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(text);
				if (item != null)
				{
					doc.PartnerID = item.Id;
					doc.PartnerContractGroup.PartnerCode = item.PartCode;
					doc.PartnerName = item.Caption;
					doc.SupplierEMail = item.Email;
				}
			}
			text2 = ArmSoft.SME.Common.Extensions.StringExtensions.NextDocNum(doc.DocNum, 12);
			AddCurrentRowToMTPurchaseOrderList(doc, row);
			dictionary.Add(doc.PartnerContractGroup.PartnerCode, doc);
		}
		foreach (KeyValuePair<string, MTPurchaseOrder> item2 in dictionary)
		{
			MTPurchaseOrder doc = item2.Value;
			if (doc.MTList.Count > 0)
			{
				Settings.DocumentService.Store(doc, new StoreRequest
				{
					LogComment = ArmSoft.SME.Common.Resources.AutoCreatePurchase
				});
			}
		}
		Settings.ParametersService.MTPurchaseOrderNumber_set(text2);
	}

	private void AddCurrentRowToMTPurchaseOrderList(MTPurchaseOrder doc, GoodsRemainsDataProvider.DataRow sourceRow)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(sourceRow.MTCode);
		if (item != null)
		{
			doc.MTList.Add(new MTPurchaseOrder.MTPurchaseOrderSpecificationRow
			{
				RowID = doc.MaxRowId,
				MTCode = item.MTCode,
				MTName = item.Caption,
				MTID = item.Id,
				UnitCode = sourceRow.UnitCode,
				Quantity = (string.IsNullOrWhiteSpace(MinQtys) ? 1m : Math.Abs(sourceRow.OrderQty)),
				UnitBrief = sourceRow.UnitBrief
			});
			doc.MaxRowId++;
		}
	}

	private void ExportForTerminal()
	{
		new TerminalExportWindow(GetSourceList()).ShowDialog();
	}

	private List<TerminalRow> GetSourceList()
	{
		List<TerminalRow> list = new List<TerminalRow>();
		foreach (GoodsRemainsDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRemainsDataProvider.DataRow>())
		{
			list.Add(new TerminalRow
			{
				Id = selectedOrFocusedRow.MTID,
				Code = selectedOrFocusedRow.MTCode,
				Name = selectedOrFocusedRow.Caption,
				Unit = selectedOrFocusedRow.UnitCode,
				AlternativeUnit = string.Empty,
				BarCode = null,
				Quantity = selectedOrFocusedRow.Quantity
			});
		}
		return list;
	}

	public override bool CardViewAllowed()
	{
		return true;
	}

	public override DataTemplate GetCardTemplate()
	{
		if (ShowSumms)
		{
			return (DataTemplate)Application.Current.Resources["MtAvailCardTemplate"];
		}
		return (DataTemplate)Application.Current.Resources["MtAvailCardWithoutCostPriceTemplate"];
	}

	public override DataTemplate GetCardPrintTemplate()
	{
		return (DataTemplate)Application.Current.Resources["MtAvailCardPrintTemplate"];
	}

	public override ToolTip ToolTipContent(object sender)
	{
		GoodsRemainsDataProvider.DataRow dataRow = (GoodsRemainsDataProvider.DataRow)((Image)sender).Tag;
		if (dataRow == null)
		{
			return null;
		}
		int mTID = dataRow.MTID;
		if (dataRow.ImageExists && dataRow.Image == null)
		{
			dataRow.Image = Settings.ImagesService.GetItemImage(ArmSoft.SME.Trade.Common.SmartEnums.ImageItemTypeSE.Materials.Value, TradeSettings.TradeCacheService.Material.GetQntUnit(mTID, dataRow.UnitCode)?.ImageGUID.ToString());
		}
		if (dataRow.Image != null && dataRow.Image.Length != 0)
		{
			LayoutItemWithRequired layoutItemWithRequired = new LayoutItemWithRequired();
			layoutItemWithRequired.LabelPosition = LayoutItemLabelPosition.Top;
			layoutItemWithRequired.Label = dataRow.Caption;
			ImageEdit imageEdit = new ImageEdit();
			imageEdit.IsReadOnly = true;
			imageEdit.ShowLoadDialogOnClickMode = ShowLoadDialogOnClickMode.Never;
			imageEdit.EditMode = EditMode.InplaceInactive;
			imageEdit.EditValue = dataRow.Image;
			imageEdit.Width = 200.0;
			imageEdit.Height = 200.0;
			imageEdit.VerticalAlignment = VerticalAlignment.Stretch;
			imageEdit.HorizontalAlignment = HorizontalAlignment.Stretch;
			layoutItemWithRequired.Content = imageEdit;
			return new ToolTip
			{
				Content = layoutItemWithRequired
			};
		}
		return new ToolTip
		{
			Content = ArmSoft.SME.Common.Resources.NoImage
		};
	}
}
