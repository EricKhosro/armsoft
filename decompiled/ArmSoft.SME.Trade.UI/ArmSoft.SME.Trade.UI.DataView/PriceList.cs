using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Converters;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DataView.Reports;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10060)]
public class PriceList : TradeDataView<PriceListDataProvider.DataRow, PriceListDataProvider.Param>
{
	private class DataViewDialog : TradeDataViewParametersDialogWindow
	{
		private readonly PriceList _dataView;

		private readonly DateEditControl _date;

		private readonly MultiSelectDropDownControl _priceType;

		private readonly DropDownControl _type;

		private readonly MultiSelectDropDownControl _group;

		private readonly DropDownControl _item;

		private readonly DropDownControl _itemsShowMode;

		private readonly CheckEditExt _showAvailable;

		private readonly CheckEditExt _exportExcel;

		private readonly GoodsFilter _goods;

		private readonly bool _isRefreshMode;

		public DataViewDialog(PriceList dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = ArmSoft.SME.Common.Resources.PriceList;
			_goods = new GoodsFilter(shortMode: false);
			_isRefreshMode = isRefreshMode;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_date = AddDateControl(container, "PriceListDate", ArmSoft.SME.Common.Resources.DateWord, useLongDate: false, isRequired: true, !useParamValues);
			_date.EditValue = (useParamValues ? dataView.PriceListDate : Settings.HostingEnvironment.CurrentDate);
			AddSpecialDateButton(container, _date);
			_priceType = AddMultiSelectDropDownControl("PriceTypes", ArmSoft.SME.Common.Resources.PriceListTypes, new ArmSoft.SME.Trade.UI.DropDown.PriceTypes(), isRequired: true, !useParamValues);
			_priceType.SelectedString = (useParamValues ? dataView.PriceType : string.Empty);
			_type = AddDropDownControl("Type", ArmSoft.SME.Common.Resources.TypeWord, new SmartEnum(typeof(GoodsTypesSE)), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_type.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ItemType_EditValueChanged).Handler;
			_type.Code = (useParamValues ? dataView.Type : string.Empty);
			_group = AddMultiSelectDropDownControl("Group", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: false, !useParamValues);
			_group.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(GoodsGroup_EditValueChanged).Handler;
			_group.SelectedString = (useParamValues ? dataView.Group : string.Empty);
			_item = AddDropDownControl("MTCode", ArmSoft.SME.Common.Resources.Code, _goods, isRequired: false, allowAdditionButtons: false, !useParamValues);
			_item.Code = (useParamValues ? dataView.MTCode : string.Empty);
			_itemsShowMode = AddDropDownControl("ItemsShowMode", ArmSoft.SME.Trade.Common.Resources.Show, new SmartEnum(typeof(ShowInPriceListSE)), isRequired: true, allowAdditionButtons: false, !useParamValues);
			_itemsShowMode.Code = ShowInPriceListSE.IncludedInPriceList.Value;
			_showAvailable = AddBooleanControl("_showAvailable", ArmSoft.SME.Trade.Common.Resources.ShowAvailables, !useParamValues);
			_showAvailable.IsChecked = useParamValues && dataView.ShowAvailable;
			if (!isRefreshMode)
			{
				_exportExcel = AddBooleanControl("ExportExcel", ArmSoft.SME.Common.Resources.ExportToExcel, storeValue: true);
			}
		}

		private void GoodsGroup_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue)
			{
				_goods.Group = (string)e.NewValue;
			}
		}

		private void ItemType_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue && SmartEnum<ItemTypesSE, string>.TryFromValue((string)e.NewValue, out var result))
			{
				_goods.ItemType = result;
			}
			if (!string.IsNullOrWhiteSpace(_type.Code))
			{
				if (!_type.DoValidate())
				{
					return;
				}
				if (_type.Code == ItemTypesSE.Goods)
				{
					_item.DataView = new GoodsFilter(shortMode: false, ItemTypesSE.Goods);
				}
				else
				{
					_item.DataView = new GoodsFilter(shortMode: false, ItemTypesSE.Services);
				}
				if (!string.IsNullOrWhiteSpace(_item.Code))
				{
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_item.Code);
					if (item != null && item.Type != _type.Code)
					{
						_item.Code = "";
						_item.DoValidate();
					}
				}
			}
			else
			{
				_item.DataView = new GoodsFilter(shortMode: false);
			}
		}

		private void ItemType_ClickDropDown(object sender, EditValueChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue && SmartEnum<ItemTypesSE, string>.TryFromValue((string)e.NewValue, out var result))
			{
				_goods.ItemType = result;
			}
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				if (!_isRefreshMode && _exportExcel.IsChecked.Value)
				{
					if (!ExcelReports.ExportPrice(_date.EditValue.Value, _type.Code, _group.SelectedString, _item.Code, _itemsShowMode.Code, _priceType.SelectedString, _showAvailable.IsChecked.Value))
					{
						ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.NoPricesToExport);
						args.isValid = false;
						args.SetFocusWhenInvalidSave = _date;
					}
					else
					{
						args.isValid = false;
						Close();
					}
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
			_dataView.PriceListDate = _date.EditValue.Value;
			_dataView.PriceType = _priceType.SelectedString;
			_dataView.Type = _type.Code;
			_dataView.Group = _group.SelectedString;
			_dataView.MTCode = _item.Code;
			_dataView.ItemsShowMode = _itemsShowMode.Code;
			_dataView.ShowAvailable = _showAvailable.IsChecked.Value;
		}
	}

	private readonly bool _isShowForPanel;

	private List<string> _arrPriceTypes = new List<string>();

	public override string HelpId => "Purchases_Reports_PriceList";

	public override bool DialogSupported => true;

	public override string Caption => ArmSoft.SME.Common.Resources.PriceList + " - " + PriceListDate.FormatDDMMYY();

	public override LoadIndication LoadingIndication => LoadIndication.Always;

	private DataViewPanel ParentViewPanel
	{
		get
		{
			if (!_isShowForPanel)
			{
				return Settings.HostingEnvironment.CurrentViewPanel;
			}
			return (DataViewPanel)Settings.HostingEnvironment.CurrentViewPanel.TransactionsControl;
		}
	}

	public DateOnly PriceListDate
	{
		get
		{
			return (DateOnly)Parameters["PriceListDate"].Value;
		}
		set
		{
			Parameters["PriceListDate"].Value = value;
		}
	}

	public string PriceType
	{
		get
		{
			return (string)Parameters["PriceTypes"].Value;
		}
		set
		{
			Parameters["PriceTypes"].Value = value;
		}
	}

	public string Type
	{
		get
		{
			return (string)Parameters["Type"].Value;
		}
		set
		{
			Parameters["Type"].Value = value;
		}
	}

	public string Group
	{
		get
		{
			return (string)Parameters["Group"].Value;
		}
		set
		{
			Parameters["Group"].Value = value;
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

	public string ItemsShowMode
	{
		get
		{
			return (string)Parameters["ItemsShowMode"].Value;
		}
		set
		{
			Parameters["ItemsShowMode"].Value = value;
		}
	}

	public bool ShowAvailable
	{
		get
		{
			return (bool)Parameters["ShowAvailables"].Value;
		}
		set
		{
			Parameters["ShowAvailables"].Value = value;
		}
	}

	public PriceList()
	{
		DataProvider = new PriceListDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("PriceListDate", ArmSoft.SME.Common.Resources.DateWord, Settings.HostingEnvironment.CurrentDate, "dd/MM/yy");
		AddParameter("PriceTypes", ArmSoft.SME.Common.Resources.PriceListTypes, string.Empty);
		AddParameter("Type", ArmSoft.SME.Common.Resources.TypeWord, string.Empty);
		AddParameter("Group", ArmSoft.SME.Common.Resources.Group, string.Empty);
		AddParameter("MTCode", ArmSoft.SME.Common.Resources.Code, string.Empty);
		AddParameter("ItemsShowMode", ArmSoft.SME.Trade.Common.Resources.Show, string.Empty);
		AddParameter("ShowAvailables", ArmSoft.SME.Trade.Common.Resources.ShowAvailables, string.Empty);
		FillColumnsFromDataProvider();
		Columns["ImageExists"].Converter = new BooleanToImageExists();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		List<string> list = PriceType.SplitSelected().SelectMany((string priceType) => new string[2]
		{
			"PD_" + priceType,
			"PT_" + priceType
		}).ToList();
		foreach (ViewColumn view in Columns.ViewList)
		{
			if (view.Name.Substring(0, 3) == "PD_" || view.Name.Substring(0, 3) == "PT_")
			{
				dvp.Set_ColumnVisible(view.Name, list.Contains(view.Name));
			}
		}
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("MTCode");
		}
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		PriceListDataProvider.DataRow dataRow = (PriceListDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (AllowOperation && dataRow != null)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowPrices", ArmSoft.SME.Common.Resources.Prices, ShowPrices);
		}
	}

	private void ShowPrices()
	{
		List<string> list = new List<string>();
		foreach (PriceListDataProvider.DataRow selectedOrFocusedRow in ParentViewPanel.GetSelectedOrFocusedRows<PriceListDataProvider.DataRow>())
		{
			list.Add(selectedOrFocusedRow.MTCode);
		}
		_arrPriceTypes = PriceType.SplitSelected();
		Prices prices = new Prices(list);
		if (_arrPriceTypes.Count == 1)
		{
			prices.PriceType = _arrPriceTypes[0];
		}
		prices.Show(useParamValues: true);
	}

	public override string GetDefaultFunction()
	{
		return "ShowPrices";
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.PriceListDate = PriceListDate;
		ParametersProvider.PriceTypes = PriceType;
		ParametersProvider.Type = Type;
		ParametersProvider.MTCode = MTCode;
		ParametersProvider.Group = Group;
		ParametersProvider.ItemsShowMode = ItemsShowMode;
		ParametersProvider.ShowAvailables = ShowAvailable;
	}

	public override bool CardViewAllowed()
	{
		return true;
	}

	public override DataTemplate GetCardHeaderTemplate()
	{
		return (DataTemplate)Application.Current.Resources["GoodsPriceListCardHeaderTemplate"];
	}

	public override DataTemplate GetCardTemplate()
	{
		return (DataTemplate)Application.Current.Resources["GoodsPriceListCardTemplate"];
	}

	public override DataTemplate GetCardPrintTemplate()
	{
		return (DataTemplate)Application.Current.Resources["GoodsPriceListCardPrintTemplate"];
	}

	public override ToolTip ToolTipContent(object sender)
	{
		PriceListDataProvider.DataRow dataRow = (PriceListDataProvider.DataRow)((Image)sender).Tag;
		if (dataRow == null)
		{
			return null;
		}
		int mTID = dataRow.MTID;
		if (dataRow.ImageExists && dataRow.Image == null)
		{
			dataRow.Image = Settings.ImagesService.GetItemImage(ImageItemTypeSE.Materials.Value, TradeSettings.TradeCacheService.Material.GetQntUnit(mTID, dataRow.Unit)?.ImageGUID.ToString());
		}
		if (dataRow.Image != null && dataRow.Image.Length != 0)
		{
			LayoutItemWithRequired layoutItemWithRequired = new LayoutItemWithRequired
			{
				LabelPosition = LayoutItemLabelPosition.Top,
				Label = dataRow.Caption
			};
			ImageEdit content = new ImageEdit
			{
				IsReadOnly = true,
				ShowLoadDialogOnClickMode = ShowLoadDialogOnClickMode.Never,
				EditMode = EditMode.InplaceInactive,
				EditValue = dataRow.Image,
				Width = 200.0,
				Height = 200.0,
				VerticalAlignment = VerticalAlignment.Stretch,
				HorizontalAlignment = HorizontalAlignment.Stretch
			};
			layoutItemWithRequired.Content = content;
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
