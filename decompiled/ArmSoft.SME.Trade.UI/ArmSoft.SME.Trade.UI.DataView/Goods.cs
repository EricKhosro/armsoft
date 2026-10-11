using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Converters;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.Data.DropDowns;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Cache;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.DataView.Reports;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.OtherWindows;
using ArmSoft.SME.Trade.UI.ReportViewers.Goods;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10003)]
public class Goods : TradeDirectoryDataView<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow, ArmSoft.SME.Trade.Client.Data.Directories.Goods.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly Goods _dataView;

		private readonly DropDownControl _itemType;

		private readonly DropDownControl _goodsGroup;

		private readonly MultiSelectDropDownControl _good;

		private readonly DropDownControl _showAs;

		private readonly MultiSelectDropDownControl _priceType;

		private readonly CheckEditExt _extended;

		private readonly CheckEditExt _showRems;

		private readonly ArmSoft.SME.Trade.UI.DropDown.GoodsFilter _goods;

		public void LoadParams()
		{
			LoadValues();
			ApplyParams(useParamValues: false, isRefreshMode: true);
		}

		public DataViewDialog(Goods dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			bool showAsTransactionPanel = _dataView.ShowAsTransactionPanel;
			Name = _dataView.Name + (showAsTransactionPanel ? "Panel" : "");
			_goods = new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: false);
			LayoutGroupExt layoutGroupExt = AddHorizontalLayoutGroup();
			DXTabControl tc = (_dataView._hasExtension ? AddTabControl(null) : null);
			LayoutGroup layoutGroup = (_dataView._hasExtension ? AddTabedGroup(tc, ArmSoft.SME.Common.Resources.General) : null);
			if (!isRefreshMode | showAsTransactionPanel)
			{
				AddStoreValuesButton(layoutGroupExt);
			}
			_itemType = AddDropDownControl(_dataView._hasExtension ? layoutGroup : layoutGroupExt, "ItemType", ArmSoft.SME.Common.Resources.TypeWord, new SmartEnum(typeof(GoodsTypesSE)), isRequired: false, allowAdditionButtons: false, storeValue: true);
			_itemType.Code = (useParamValues ? _dataView.ItemType : "");
			_itemType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ItemType_EditValueChanged).Handler;
			if (!showAsTransactionPanel)
			{
				_goodsGroup = AddDropDownControl(layoutGroup, "Group", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: false, allowAdditionButtons: false, storeValue: true);
				_goodsGroup.Code = (useParamValues ? _dataView.Group : "");
				_goodsGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(GoodsGroup_EditValueChanged).Handler;
				_good = AddMultiSelectDropDownControl(layoutGroup, "MTCode", ArmSoft.SME.Trade.Common.Resources.MaterialOrService, _goods, isRequired: false, !useParamValues);
				_good.SelectedString = (useParamValues ? _dataView.MtCode : "");
			}
			_showAs = AddDropDownControl(layoutGroup, "Show", ArmSoft.SME.Trade.Common.Resources.Show, new SmartEnum(typeof(PriceListTypeCheckSE)), isRequired: true, allowAdditionButtons: false, storeValue: true);
			_showAs.Code = (useParamValues ? dataView.ShowAs : PriceListTypeCheckSE.All.Value.ToString());
			AddLine(layoutGroup);
			_priceType = AddMultiSelectDropDownControl(layoutGroup, "PriceType", ArmSoft.SME.Common.Resources.PriceListTypes, new ArmSoft.SME.Trade.UI.DropDown.PriceTypes(), isRequired: false, storeValue: true);
			_priceType.SelectedString = (useParamValues ? _dataView.PriceType : "");
			_extended = AddBooleanControl(layoutGroup, "ShowExtended", ArmSoft.SME.Common.Resources.Extended, storeValue: true);
			_extended.IsChecked = useParamValues && _dataView.Extended;
			if (!isRefreshMode)
			{
				AddLine(layoutGroup);
				_showRems = AddBooleanControl(layoutGroup, "ShowRems", ArmSoft.SME.Trade.Common.Resources.ShowRems, storeValue: true);
				_showRems.IsChecked = useParamValues && _dataView.ShowRems;
				if (_dataView._hasExtension)
				{
					AddColumnsSelectionTab(tc, layoutGroup, null);
				}
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
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.ItemType = _itemType.Code;
			if (!_dataView.ShowAsTransactionPanel)
			{
				_dataView.Group = _goodsGroup.Code;
				_dataView.MtCode = _good.SelectedString;
			}
			_dataView.ShowAs = _showAs.Code;
			_dataView.PriceType = _priceType.SelectedString;
			_dataView.Extended = _extended.IsChecked.Value;
			if (!isRefreshMode)
			{
				_dataView.ShowRems = _showRems.IsChecked.Value;
			}
		}
	}

	private GoodsRemains _goodsRemains;

	private readonly bool _hasExtension;

	private readonly List<ArmSoft.SME.Core.UI.DataViews.ViewColumn> _potentiallyColumns;

	public override short DirectoryType => ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods.Value;

	protected override bool IsPOWindow => true;

	public override bool AllowEditCodeOfDirectories => Settings.ParametersService.AllowEditCodeOfDirectories();

	public override List<ArmSoft.SME.Core.UI.DataViews.ViewColumn> PotentiallyColumns => _potentiallyColumns;

	public override string HelpId => "Purchases_Lists_ProductsandServices";

	public override bool DialogSupported => true;

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

	public override bool AllowShowTransactions
	{
		get
		{
			if (!Settings.LoginService.IsAdmin)
			{
				if (!Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepAvailability.Value).AllowView)
				{
					return Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepAvailabilityTreeView).AllowView;
				}
				return true;
			}
			return true;
		}
	}

	public override bool ShowTransactionsOnOpen
	{
		get
		{
			if (AllowOperation)
			{
				if (!Settings.LoginService.IsAdmin)
				{
					return ShowRems;
				}
				return true;
			}
			return false;
		}
	}

	public override string TransactionsPanelCaption
	{
		get
		{
			if (!ShowRems)
			{
				return ArmSoft.SME.Common.Resources.b2bSystem;
			}
			return ArmSoft.SME.Trade.Common.Resources.GoodsRem;
		}
	}

	public override bool AllowEditSelected => base.AllowEdit;

	protected override bool AllowFindRowByMTBarcode => true;

	protected override string MTCodeColumnName => "MTCode";

	public string MtCode
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

	public string ItemType
	{
		get
		{
			return (string)Parameters["ItemType"].Value;
		}
		set
		{
			Parameters["ItemType"].Value = value;
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

	public string PriceType
	{
		get
		{
			return (string)Parameters["PriceType"].Value;
		}
		set
		{
			Parameters["PriceType"].Value = value;
		}
	}

	public bool Extended
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowExtended"].Value);
		}
		set
		{
			Parameters["ShowExtended"].Value = value;
		}
	}

	public bool ShowRems
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowRems"].Value);
		}
		set
		{
			Parameters["ShowRems"].Value = value;
		}
	}

	public string ShowAs
	{
		get
		{
			return (string)Parameters["Show"].Value;
		}
		set
		{
			Parameters["Show"].Value = value;
		}
	}

	public Goods()
	{
		DocumentExtensionDefinition documentExtensionDefinition = Settings.ServiceProvider.GetRequiredService<IDocumentExtensionDefinitionService>().Load(POTypeSE.Directory.Value, ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods.Value);
		_hasExtension = documentExtensionDefinition != null && documentExtensionDefinition.DataItems?.Count > 0;
		DataProvider = new ArmSoft.SME.Trade.Client.Data.Directories.Goods(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
		Columns["ImageExists"].Converter = new BooleanToImageExists();
		AddParameter("ItemType", ArmSoft.SME.Common.Resources.Type, string.Empty);
		AddParameter("Group", ArmSoft.SME.Common.Resources.Group, string.Empty);
		AddParameter("MTCode", ArmSoft.SME.Trade.Common.Resources.MaterialCode, string.Empty);
		AddParameter("Show", ArmSoft.SME.Common.Resources.ShowASInfo, "", "", includeCaptionToolTip: false, new SmartEnum(typeof(ArmSoft.SME.Common.SmartEnums.DocumentStateSE)));
		AddParameter("PriceType", ArmSoft.SME.Common.Resources.PriceListType, string.Empty);
		AddParameter("ShowExtended", ArmSoft.SME.Common.Resources.Extended, false);
		AddParameter("ShowRems", ArmSoft.SME.Trade.Common.Resources.ShowRems, false);
		_potentiallyColumns = new List<ArmSoft.SME.Core.UI.DataViews.ViewColumn>();
		_potentiallyColumns.AddRange(Columns.ViewList);
		_potentiallyColumns.AddRange(MaterialCache.GetDataViewColumns(ArmSoft.SME.Trade.Common.Resources.MaterialOrService, "", "", includeCode: false, includeCaption: false, includeUnitBrief: false, includeGroup: true, includeSpecification: true, onlyExtendedFields: true));
	}

	public override void SetTransactionsPanelsDefaultSize(ref double totalsPanelHeight, ref double totalsPanelRightWidth)
	{
		if (!ShowRems)
		{
			totalsPanelHeight = 60.0;
			totalsPanelRightWidth = 0.0;
		}
	}

	public override Control CreateTransactionsControl()
	{
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		if (ShowRems)
		{
			_goodsRemains = new GoodsRemains
			{
				ShowAsTransactionPanel = true
			};
			_goodsRemains.LoadParams();
			return new DataViewPanel(_goodsRemains);
		}
		LayoutGroup layoutGroup = new LayoutGroup
		{
			Orientation = Orientation.Horizontal,
			HorizontalAlignment = System.Windows.HorizontalAlignment.Left
		};
		LayoutGroup layoutGroup2 = new LayoutGroup
		{
			Orientation = Orientation.Horizontal,
			Margin = new Thickness(30.0, 10.0, 0.0, 0.0)
		};
		LayoutItemWithRequired layoutItemWithRequired = new LayoutItemWithRequired();
		TextBlock textBlock = new TextBlock
		{
			TextWrapping = TextWrapping.NoWrap,
			HorizontalAlignment = System.Windows.HorizontalAlignment.Left
		};
		textBlock.Inlines.Add(ArmSoft.SME.Common.Resources.AddBarCodeAndGetGoodsDataFromB2b);
		layoutItemWithRequired.Content = textBlock;
		layoutGroup2.Children.Add(layoutItemWithRequired);
		Hyperlink hyperlink = new Hyperlink(new Run(ArmSoft.SME.Common.Resources.AddNewMT))
		{
			Tag = ArmSoft.SME.Common.Resources.AddNewMT,
			Name = "CreateNewGood"
		};
		hyperlink.Click += new SafeEventHandler<RoutedEventArgs>(NavigateACSUnitedProducts).Handler;
		layoutItemWithRequired = new LayoutItemWithRequired();
		textBlock = new TextBlock();
		textBlock.Inlines.Add(hyperlink);
		textBlock.TextWrapping = TextWrapping.Wrap;
		textBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
		layoutItemWithRequired.Content = textBlock;
		layoutGroup2.Children.Add(layoutItemWithRequired);
		LayoutGroup layoutGroup3 = new LayoutGroup
		{
			Orientation = Orientation.Horizontal,
			Margin = new Thickness(50.0, 10.0, 0.0, 0.0),
			Width = 250.0
		};
		Hyperlink hyperlink2 = new Hyperlink(new Run(ArmSoft.SME.Trade.Common.Resources.GoodAddMethod))
		{
			Tag = ArmSoft.SME.Common.Resources.AddNewMT,
			Name = "AddNewItemMode"
		};
		hyperlink2.Click += new SafeEventHandler<RoutedEventArgs>(NavigateGoodAddMethods).Handler;
		TextBlock textBlock2 = new TextBlock
		{
			HorizontalAlignment = System.Windows.HorizontalAlignment.Left
		};
		textBlock2.Inlines.Add(hyperlink2);
		layoutGroup3.Children.Add(new ImageEdit
		{
			Source = ImageService.GetImage("options_16.svg"),
			IsEnabled = false,
			ShowBorder = false,
			Stretch = Stretch.Fill,
			VerticalAlignment = VerticalAlignment.Top,
			Height = 16.0,
			Width = 16.0
		});
		layoutGroup3.Children.Add(new LayoutItemWithRequired
		{
			Content = textBlock2
		});
		layoutGroup.Children.Add(layoutGroup2);
		layoutGroup.Children.Add(layoutGroup3);
		return new LayoutItem
		{
			VerticalAlignment = VerticalAlignment.Stretch,
			Content = new ScrollViewer
			{
				HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
				VerticalAlignment = VerticalAlignment.Stretch,
				Content = layoutGroup
			}
		};
	}

	private void NavigateGoodAddMethods(object sender, RoutedEventArgs e)
	{
		try
		{
			DialogWindow dialogWindow = new DialogWindow
			{
				Name = "AddMethods",
				Caption = ArmSoft.SME.Common.Resources.AddNewMT
			};
			CheckEditExt checkEditExt = dialogWindow.AddBooleanControl("SimpleMode", ArmSoft.SME.Trade.Common.Resources.GoodAddSimpleMode);
			checkEditExt.IsChecked = Settings.ParametersService.GoodAddSimpleMode();
			if (dialogWindow.ShowDialog().Value)
			{
				Settings.ParametersService.GoodAddSimpleMode_set(checkEditExt.IsChecked.Value);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void NavigateACSUnitedProducts(object sender, RoutedEventArgs e)
	{
		try
		{
			AddByb2bCloudBarCode();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public override void ShowTransactions(bool showAtRight)
	{
		base.ShowTransactions(showAtRight);
		if (ShowRems)
		{
			if (Settings.HostingEnvironment.CurrentViewPanel.TransactionsControl is DataViewPanel dataViewPanel)
			{
				ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow dataRow = (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow)ParentViewPanel.FocusedRow();
				if (dataRow != null)
				{
					_goodsRemains.MTCode = dataRow.MTCode;
				}
				_goodsRemains.BeforeLoadData(dataViewPanel);
				_goodsRemains.LoadData();
				dataViewPanel.BindData();
				dataViewPanel.SetBarStates();
				dataViewPanel.FitColumns();
			}
		}
		else
		{
			Settings.HostingEnvironment.CurrentViewPanel.TotalsPanelHeight = 60.0;
		}
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		List<string> list = PriceType.SplitSelected().SelectMany((string priceType) => new string[2]
		{
			"PD_" + priceType,
			"PT_" + priceType
		}).ToList();
		foreach (ArmSoft.SME.Core.UI.DataViews.ViewColumn view in Columns.ViewList)
		{
			if (view.Name.Substring(0, 3) == "PD_" || view.Name.Substring(0, 3) == "PT_")
			{
				dvp.Set_ColumnVisible(view.Name, list.Contains(view.Name));
			}
		}
		bool flag = ItemType == GoodsTypesSE.Service.Value.ToString();
		dvp.RemoveGrouping();
		dvp.Set_ColumnVisible("Bonus", value: false);
		dvp.Set_ColumnVisible("Point", value: false);
		dvp.Set_ColumnVisible("IgnoreBonus", value: false);
		dvp.Set_ColumnVisible("TypeName", string.IsNullOrWhiteSpace(ItemType));
		dvp.Set_ColumnVisible("ExpenseMethodName", !flag);
		dvp.Set_ColumnVisible("IsWeight", !flag);
		dvp.Set_ColumnVisible("PLUCode", !flag);
		dvp.Set_ColumnVisible("CheckMarking", Extended && !flag && Settings.ParametersService.UseMarking());
		dvp.Set_ColumnVisible("LabelUnitBrief", Extended);
		dvp.Set_ColumnVisible("LabelCoeff", Extended);
		dvp.Set_ColumnVisible("MainSupplier", Extended);
		dvp.Set_ColumnVisible("Country", Extended);
		dvp.Set_ColumnVisible("CountryName", Extended);
		dvp.Set_ColumnVisible("Producer", Extended);
		dvp.Set_ColumnVisible("ProducerName", Extended);
		dvp.Set_ColumnVisible("Property1", Extended && !flag);
		dvp.Set_ColumnVisible("Property2", Extended && !flag);
		dvp.Set_ColumnVisible("MinQty", Extended && !flag);
		dvp.Set_ColumnVisible("MaxQty", Extended && !flag);
		dvp.Set_ColumnVisible("Extra", Extended && !flag);
		dvp.Set_ColumnVisible("BarCode", Extended);
		dvp.Set_ColumnVisible("EcoFeeRate", Settings.ParametersService.EnvironmentalFeePayer());
		if (Settings.ParametersService.UseBonus())
		{
			if (Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent.Value)
			{
				dvp.Set_ColumnVisible("Bonus", Extended);
			}
			else
			{
				dvp.Set_ColumnVisible("Point", Extended);
			}
			dvp.Set_ColumnVisible("IgnoreBonus", Extended);
		}
		dvp.Set_ColumnVisible("Coeff", Extended);
		dvp.Set_ColumnVisible("Discount", Extended);
		if (Settings.ParametersService.EnableSubstituteItems())
		{
			dvp.Set_ColumnVisible("SubstituteItemsGroupCode", Extended);
			dvp.Set_ColumnVisible("SubstituteItemsGroupCaption", Extended);
		}
		dvp.Set_ColumnVisible("ExternalCode", Extended);
		dvp.Set_ColumnVisible("CaptionEN", Extended);
		dvp.Set_ColumnVisible("CaptionRU", Extended);
		dvp.Set_ColumnVisible("AdditionalDescr1", Extended);
		dvp.Set_ColumnVisible("AdditionalDescr2", Extended);
		dvp.Set_ColumnVisible("CreationDate", Extended);
		dvp.Set_ColumnVisible("LastModifyDate", Extended);
		dvp.Set_ColumnVisible("LastModifierName", Extended);
		dvp.Set_ColumnVisible("CreatorName", Extended);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("MTCode");
		}
	}

	protected override void SetCacheRow(ExtendableRow row)
	{
		ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow dataRow = row as ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow;
		if (dataRow.MTCode != null)
		{
			dataRow.MaterialRow = TradeSettings.TradeCacheService.Material.GetItem(dataRow.MTCode);
		}
	}

	public override void Add()
	{
		bool flag = true;
		RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey);
		if (registryKey != null && MemoryExtensions.Contains<string>((ReadOnlySpan<string>)registryKey.GetValueNames(), "ShowGoodsUnifiedAdditionWindow"))
		{
			flag = !Convert.ToBoolean(registryKey.GetValue("ShowGoodsUnifiedAdditionWindow"));
			registryKey.Close();
		}
		if (flag)
		{
			AddByb2bCloudBarCode();
			return;
		}
		Good directory = Settings.DirectoryService.Create<Good>(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods.Value);
		Settings.DirectoryService.ShowWindow(directory, isPOWindow: true, WindowViewMode.NewCreate, ParentViewPanel?.Id ?? (-1));
	}

	private void AddByb2bCloudBarCode()
	{
		BarcodeFromb2bCloudDialogWindow barcodeFromb2bCloudDialogWindow = new BarcodeFromb2bCloudDialogWindow();
		if (barcodeFromb2bCloudDialogWindow.ShowDialog().Value)
		{
			Good good = (Good)Settings.DirectoryService.Create(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods);
			if (barcodeFromb2bCloudDialogWindow.CloudUnitedProduct != null)
			{
				good.Name = barcodeFromb2bCloudDialogWindow.CloudUnitedProduct.Name;
				good.FullName = barcodeFromb2bCloudDialogWindow.CloudUnitedProduct.FullName;
				good.Unit = (barcodeFromb2bCloudDialogWindow.CloudUnitedProduct.IsWeight ? "303" : "001");
				good.AltUnit = good.Unit;
				good.CPACode = barcodeFromb2bCloudDialogWindow.CloudUnitedProduct.CPACode;
				good.VAT = barcodeFromb2bCloudDialogWindow.CloudUnitedProduct.IsVatTaxable;
				good.IsWeight = barcodeFromb2bCloudDialogWindow.CloudUnitedProduct.IsWeight;
				good.BarCode = barcodeFromb2bCloudDialogWindow.CloudUnitedProduct.Barcode;
			}
			else
			{
				good.BarCode = barcodeFromb2bCloudDialogWindow.BarCode;
			}
			Settings.DirectoryService.ShowWindow(good, isPOWindow: true, WindowViewMode.NewCreate, ParentViewPanel?.Id ?? (-1));
		}
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	public void LoadParams()
	{
		((DataViewDialog)CreateDialog(useParamValues: false, isRefreshMode: true)).LoadParams();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.ItemType = ItemType;
		ParametersProvider.Group = Group;
		ParametersProvider.MTCode = MtCode;
		ParametersProvider.Show = ShowAs;
		ParametersProvider.PriceType = PriceType;
		ParametersProvider.ShowExtended = Extended;
		ParametersProvider.ShowRems = ShowRems;
	}

	protected override void AddControlsInEditSelectedDialog(DialogWindow dialog, out bool doRefresh)
	{
		base.AddControlsInEditSelectedDialog(dialog, out doRefresh);
		EditSelectedHelper.AddGoodsControls(dialog);
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow dataRow = (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow)ParentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MTPrices).AllowView)
			{
				ParentViewPanel.AddContextFunction("ContextFunctions", "ShowPrices", ArmSoft.SME.Common.Resources.Prices, ShowPrices);
			}
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MTPrices).AllowOperation)
			{
				ParentViewPanel.AddContextFunction("ContextFunctions", "ShowPricesFormation", ArmSoft.SME.Trade.Common.Resources.PricesFormation, ShowPricesFormation);
			}
			ParentViewPanel.AddContextFunction("ContextFunctions", "DiscountsDeclaration", ArmSoft.SME.Trade.Common.Resources.DiscountsDeclaration, DiscountsDeclaration);
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MTQntUnits).AllowView)
			{
				ParentViewPanel.AddContextFunction("ContextFunctions", "ShowMTQntUnits", ArmSoft.SME.Common.Resources.MeasureUnits, ShowMTQntUnits);
			}
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MTBarCodes).AllowView)
			{
				ParentViewPanel.AddContextFunction("ContextFunctions", "ShowMTBarCodes", ArmSoft.SME.Common.Resources.BarCodes, ShowMTBarCodes);
			}
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MTComplectsParts).AllowView && dataRow.Type == GoodsTypesSE.Good)
			{
				ParentViewPanel.AddContextFunction("ContextFunctions", "ShowComplectPart", ArmSoft.SME.Trade.Common.Resources.MTComplectation, ShowComplectPart);
			}
			ParentViewPanel.AddContextFunction("ContextFunctions", "PrintLabel", ArmSoft.SME.Trade.Common.Resources.PrintLabel, PrintLabel);
			if (ParentViewPanel.GetSelectedAny() || dataRow.Type == GoodsTypesSE.Good)
			{
				ParentViewPanel.AddContextMenuGroup("Export", ArmSoft.SME.Common.Resources.Export, "export_16.svg");
				ParentViewPanel.AddContextFunction("Export", "ExportForScale", ArmSoft.SME.Trade.Common.Resources.ByScaleFormat, ExportForScale);
				ParentViewPanel.AddContextFunction("Export", "ExportForTerminal", ArmSoft.SME.Trade.Common.Resources.ByTerminalFormat, ExportForTerminal);
				ParentViewPanel.AddContextSeparator("Export");
				ParentViewPanel.AddContextFunction("Export", "GroupExportForAll", ArmSoft.SME.Trade.Common.Resources.GroupElScalesAndTerminals, GroupExportForAll);
			}
			if (AllowOperation)
			{
				ParentViewPanel.AddContextFunction("ContextFunctions", "CreateDoc", ArmSoft.SME.Common.Resources.CreateDoc, CreateDoc, "doc_new_16.svg");
			}
			ParentViewPanel.AddContextSeparator("ContextFunctions");
			ParentViewPanel.AddContextFunction("ContextFunctions", "GoodsReports", ArmSoft.SME.Common.Resources.Reports, GoodsReports, "report_16.svg", "", storeOpContext: true);
			if (AllowOperation)
			{
				ParentViewPanel.AddContextFunction("ContextFunctions", "GenerateNewBarcode", ArmSoft.SME.Trade.Common.Resources.GenerateBarcode, GenerateNewBarcode);
			}
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartnersMT).AllowView)
			{
				ParentViewPanel.AddContextFunction("ContextFunctions", "ShowPartnersMT", ArmSoft.SME.Common.Resources.Suppliers, ShowPartnersMT);
			}
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.OrderMinMax).AllowView)
			{
				ParentViewPanel.AddContextFunction("ContextFunctions", "OrderMinMax", ArmSoft.SME.Trade.Common.Resources.MTOrderMinMax, OrderMinMax);
			}
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsBySegment).AllowView)
			{
				ParentViewPanel.AddContextFunction("ContextFunctions", "Segments", ArmSoft.SME.Trade.Common.Resources.Segments, ShowSegments);
			}
			if (Settings.UserAccessService.GetDirectoryPermission(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Segments).AllowAdd)
			{
				ParentViewPanel.AddContextFunction("ContextFunctions", "ChangeGoodsBySegments", ArmSoft.SME.Trade.Common.Resources.AddToSegment, ChangeGoodsBySegments);
			}
			ParentViewPanel.AddContextFunction("ContextFunctions", "AddToMTFavoritesDialog", ArmSoft.SME.Trade.Common.Resources.AddMTFavToUser, AddToMTFavoritesDialog, "", "", storeOpContext: true);
			ParentViewPanel.AddContextFunction("ContextFunctions", "ShowMTFavorites", ArmSoft.SME.Trade.Common.Resources.MTFavorites, ShowMTFavorites, "favorite_16.svg");
		}
	}

	private void ShowPrices()
	{
		List<string> list = new List<string>();
		foreach (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow selectedOrFocusedRow in ParentViewPanel.GetSelectedOrFocusedRows<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>())
		{
			list.Add(selectedOrFocusedRow.MTCode);
		}
		new Prices(list).Show(useParamValues: true);
	}

	private void ShowPricesFormation()
	{
		List<PricesFormationWindow.GoodsRow> list = new List<PricesFormationWindow.GoodsRow>();
		foreach (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow selectedOrFocusedRow in ParentViewPanel.GetSelectedOrFocusedRows<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>())
		{
			list.Add(new PricesFormationWindow.GoodsRow(selectedOrFocusedRow.MTID, selectedOrFocusedRow.MTCode, selectedOrFocusedRow.Unit, selectedOrFocusedRow.AltUnit));
		}
		new PricesFormationWindow(list, "PricesFormation" + Name).Invoke();
	}

	private void DiscountsDeclaration()
	{
		List<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow> list = new List<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>();
		foreach (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow selectedOrFocusedRow in ParentViewPanel.GetSelectedOrFocusedRows<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>())
		{
			if (selectedOrFocusedRow.Discount != 0m)
			{
				list.Add(selectedOrFocusedRow);
			}
		}
		if (list.Count > 0)
		{
			DialogWindow dialogWindow = new DialogWindow
			{
				Caption = ArmSoft.SME.Trade.Common.Resources.DiscountsDeclaration,
				Name = "DiscountsDeclaration"
			};
			LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
			DateEditControl dateEditControl = dialogWindow.AddDateControl(container, "DateBegin", ArmSoft.SME.Trade.Common.Resources.DiscountPeriod, useLongDate: false, isRequired: true);
			dateEditControl.EditValue = Settings.HostingEnvironment.DateBegin;
			DateEditControl dateEditControl2 = dialogWindow.AddDateControl(container, "DateEnd", "", useLongDate: false, isRequired: true);
			dateEditControl2.EditValue = Settings.HostingEnvironment.DateEnd;
			dialogWindow.AddSpecialDateButton(container, dateEditControl, dateEditControl2);
			dialogWindow.AddBooleanControl("ExportToExcel", ArmSoft.SME.Common.Resources.ExportToExcel);
			if (dialogWindow.ShowDialog().Value)
			{
				DateOnly dateBegin = (DateOnly)dialogWindow["DateBegin"];
				DateOnly dateEnd = (DateOnly)dialogWindow["DateEnd"];
				if ((bool)dialogWindow["ExportToExcel"])
				{
					ExcelReports.ExportDiscountsDeclarationToExel(dateBegin, dateEnd, list);
				}
				else
				{
					new DiscountsDeclarationWindow(dateBegin, dateEnd, list).ShowDialog();
				}
			}
		}
		else
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.DiscountsForDeclarationNotFound, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
		}
	}

	private void ShowMTQntUnits()
	{
		new MTQntUnits(((ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow)ParentViewPanel.FocusedRow()).MTCode).Show();
	}

	private void ShowMTBarCodes()
	{
		ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow dataRow = (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow)ParentViewPanel.FocusedRow();
		MTBarCodes mTBarCodes = new MTBarCodes();
		mTBarCodes.MTCode = dataRow.MTCode;
		mTBarCodes.Show();
	}

	private void ShowComplectPart()
	{
		ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow dataRow = (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow)ParentViewPanel.FocusedRow();
		MTComplectsParts mTComplectsParts = new MTComplectsParts();
		mTComplectsParts.ComplectCode = dataRow.MTCode;
		mTComplectsParts.Show();
	}

	private void PrintLabel()
	{
		List<ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow> list = new List<ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow>();
		foreach (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow selectedOrFocusedRow in ParentViewPanel.GetSelectedOrFocusedRows<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(selectedOrFocusedRow.MTCode);
			if (item != null)
			{
				ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow item2 = InitContextFunctionsHelper.BuildLabelGoodRow(item);
				list.Add(item2);
			}
		}
		new PrintLabelDialog(null, null, list).Invoke();
	}

	private void GoodsReports()
	{
		try
		{
			ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow dataRow = (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow)ParentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				GoodReports goodReports = new GoodReports();
				ModalBrowser modalBrowser = new ModalBrowser(goodReports)
				{
					Owner = Settings.HostingEnvironment,
					WindowStartupLocation = WindowStartupLocation.CenterScreen
				};
				if (modalBrowser.Show())
				{
					CreateGoodReport(short.Parse(goodReports.ValueMember(modalBrowser.SelectedRowIndex)), dataRow.MTCode);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static void CreateGoodReport(short reportType, string mtCode)
	{
		switch (reportType)
		{
		case 10057:
		{
			SalesAnalysis salesAnalysis = new SalesAnalysis();
			salesAnalysis.Good = mtCode;
			salesAnalysis.StartDate = Settings.HostingEnvironment.DateBegin;
			salesAnalysis.EndDate = Settings.HostingEnvironment.DateEnd;
			salesAnalysis.Show(useParamValues: true);
			break;
		}
		case 10049:
		{
			GoodsRemains goodsRemains = new GoodsRemains();
			goodsRemains.MTCode = mtCode;
			goodsRemains.RemDate = Settings.HostingEnvironment.CurrentDate;
			goodsRemains.Show(useParamValues: true);
			break;
		}
		case 10041:
		{
			GoodsTurnOver goodsTurnOver = new GoodsTurnOver();
			goodsTurnOver.Good = mtCode;
			goodsTurnOver.StartDate = Settings.HostingEnvironment.DateBegin;
			goodsTurnOver.EndDate = Settings.HostingEnvironment.DateEnd;
			goodsTurnOver.Show(useParamValues: true);
			break;
		}
		case 10067:
		{
			GoodsOperationsJournal goodsOperationsJournal = new GoodsOperationsJournal();
			goodsOperationsJournal.GoodCode = mtCode;
			goodsOperationsJournal.StartDate = Settings.HostingEnvironment.DateBegin;
			goodsOperationsJournal.EndDate = Settings.HostingEnvironment.DateEnd;
			goodsOperationsJournal.Show(useParamValues: true);
			break;
		}
		case 10085:
		{
			MTAccountReportViewer mTAccountReportViewer = new MTAccountReportViewer();
			mTAccountReportViewer.GoodCode = mtCode;
			mTAccountReportViewer.StartDate = Settings.HostingEnvironment.DateBegin;
			mTAccountReportViewer.EndDate = Settings.HostingEnvironment.DateEnd;
			mTAccountReportViewer.Show(useParamValues: true);
			break;
		}
		case 10045:
		{
			GoodsRetailAvailability goodsRetailAvailability = new GoodsRetailAvailability();
			goodsRetailAvailability.Good = mtCode;
			goodsRetailAvailability.AsOfDate = Settings.HostingEnvironment.CurrentDate;
			goodsRetailAvailability.Show(useParamValues: true);
			break;
		}
		case 10048:
		{
			GoodsRetailTurnOver goodsRetailTurnOver = new GoodsRetailTurnOver();
			goodsRetailTurnOver.Good = mtCode;
			goodsRetailTurnOver.StartDate = Settings.HostingEnvironment.DateBegin;
			goodsRetailTurnOver.EndDate = Settings.HostingEnvironment.DateEnd;
			goodsRetailTurnOver.Show(useParamValues: true);
			break;
		}
		case 10059:
		{
			GoodsRetailOperationsJournal goodsRetailOperationsJournal = new GoodsRetailOperationsJournal();
			goodsRetailOperationsJournal.GoodCode = mtCode;
			goodsRetailOperationsJournal.StartDate = Settings.HostingEnvironment.DateBegin;
			goodsRetailOperationsJournal.EndDate = Settings.HostingEnvironment.DateEnd;
			goodsRetailOperationsJournal.Show(useParamValues: true);
			break;
		}
		}
	}

	private void CreateDoc()
	{
		int num = 10;
		List<short> list = new List<short>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<short> span = CollectionsMarshal.AsSpan(list);
		span[0] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value;
		span[1] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value;
		span[2] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveOrder.Value;
		span[3] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value;
		span[4] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value;
		span[5] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeImport.Value;
		span[6] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTExpense.Value;
		span[7] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value;
		span[8] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value;
		span[9] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value;
		List<short> list2 = list;
		if (!Settings.ParametersService.MTMoveWithConfirmation())
		{
			list2.Remove(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value);
		}
		ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE documentTypeSE = InitContextFunctionsHelper.ChooseDocTypeFromMenu(list2);
		if (documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome)
		{
			CreateNewMTIncomeDoc();
		}
		else if (documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail)
		{
			CreateNewMTIncomeRetailDoc();
		}
		else if (documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeImport)
		{
			CreateNewMTIncomeImportDoc();
		}
		else if (documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice)
		{
			CreateNewMTInvoiceDoc();
		}
		else if (documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail)
		{
			CreateNewMTInvoiceRetailDoc();
		}
		else if (documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill)
		{
			CreateNewMTBillDoc();
		}
		else if (documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTExpense)
		{
			CreateNewMTExpensedoc();
		}
		else if (documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove)
		{
			CreateNewMTMoveDoc();
		}
		else if (documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation)
		{
			CreateNewMTMoveWithConfirmationDoc();
		}
		else if (documentTypeSE == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveOrder)
		{
			CreateNewMTMoveOrderDoc();
		}
	}

	private void CreateNewMTIncomeDoc()
	{
		try
		{
			MTIncome doc = Settings.DocumentService.Create<MTIncome>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome);
			FillMTIncomeBaseDoc(doc);
			Settings.DocumentService.ShowWindow(doc, WindowViewMode.NewCreate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CreateNewMTIncomeRetailDoc()
	{
		try
		{
			MTIncomeRetail doc = Settings.DocumentService.Create<MTIncomeRetail>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail);
			FillMTIncomeBaseDoc(doc, isMTIncomeRetail: true);
			Settings.DocumentService.ShowWindow(doc, WindowViewMode.NewCreate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void FillMTIncomeBaseDoc(MTIncomeBase doc, bool isMTIncomeRetail = false)
	{
		int num = 0;
		string text = Settings.ParametersService.DefaultStorage();
		doc.DocDate = Settings.HostingEnvironment.CurrentDate;
		if (!string.IsNullOrWhiteSpace(text) && (!(TradeSettings.TradeCacheService.Storage.GetItem(text)?.Retail ?? false) | isMTIncomeRetail) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
		{
			doc.Storage = text;
		}
		string text2 = Settings.ParametersService.DefaultSupplier();
		if (!string.IsNullOrWhiteSpace(text2))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(text2);
			if (item != null && item.Customer)
			{
				doc.PartnerContractGroup.PartnerCode = text2;
				PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(item);
				if (defaultContract != null)
				{
					doc.PartnerContractGroup.ContractCode = defaultContract.Contract;
				}
			}
		}
		doc.PriceType = Settings.ParametersService.DefaultBuyingPrice();
		foreach (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow selectedOrFocusedRow in ParentViewPanel.GetSelectedOrFocusedRows<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>())
		{
			if (selectedOrFocusedRow.Type == GoodsTypesSE.Good)
			{
				doc.MTList.Add(CreateMTIncomeMTListRow(doc, selectedOrFocusedRow, num));
				num++;
			}
		}
		doc.MaxRowID = num;
		doc.Chief = Settings.ParametersService.ChiefAccountantName();
		doc.SummVAT = doc.MTList.Sum((MTIncomeBase.MTIncomeSpecificationRow p) => ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(p.Summa, doc.VATType, Convert.ToBoolean(p.VAT))).RoundAs(2);
		doc.Summ = (doc.MTList.Sum((MTIncomeBase.MTIncomeSpecificationRow p) => p.Summa) + doc.SummVAT).RoundAs(2);
	}

	private static MTIncomeBase.MTIncomeSpecificationRow CreateMTIncomeMTListRow(MTIncomeBase doc, ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow currentRow, int rowId)
	{
		MTIncomeBase.MTIncomeSpecificationRow mTIncomeSpecificationRow = new MTIncomeBase.MTIncomeSpecificationRow
		{
			RowID = rowId,
			MTID = currentRow.MTID,
			MTCode = currentRow.MTCode,
			MTName = currentRow.Name
		};
		mTIncomeSpecificationRow.UnitCode = TradeSettings.CreateTradeClient().CommonServices.GetMTQntUnitByMTIDPriceTypeAndDocDate(mTIncomeSpecificationRow.MTID, doc.PriceType, Settings.HostingEnvironment.CurrentDate);
		if (string.IsNullOrWhiteSpace(mTIncomeSpecificationRow.UnitCode))
		{
			mTIncomeSpecificationRow.UnitCode = currentRow.Unit;
			mTIncomeSpecificationRow.UnitBrief = currentRow.UnitBrief;
		}
		else
		{
			mTIncomeSpecificationRow.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(mTIncomeSpecificationRow.UnitCode, throwExceptionNotExits: true).Brief;
		}
		mTIncomeSpecificationRow.Quantity = 1m;
		mTIncomeSpecificationRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(doc.PriceType, doc.VATType, mTIncomeSpecificationRow.MTID, mTIncomeSpecificationRow.UnitCode, Settings.HostingEnvironment.CurrentDate, "AMD");
		if (Settings.ParametersService.DiscountUse())
		{
			mTIncomeSpecificationRow.Discount = doc.Discount;
			if (mTIncomeSpecificationRow.Discount == 0m)
			{
				mTIncomeSpecificationRow.Discount = currentRow.Discount;
			}
			else if (currentRow.Discount != 0m)
			{
				mTIncomeSpecificationRow.Discount = currentRow.Discount;
			}
		}
		mTIncomeSpecificationRow.DiscountPrice = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateDiscountPrice(mTIncomeSpecificationRow.Price, mTIncomeSpecificationRow.Discount);
		mTIncomeSpecificationRow.VAT = currentRow.VAT;
		mTIncomeSpecificationRow.Country = currentRow.Country;
		mTIncomeSpecificationRow.Producer = currentRow.Producer;
		mTIncomeSpecificationRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mTIncomeSpecificationRow.DiscountPrice, mTIncomeSpecificationRow.Quantity);
		return mTIncomeSpecificationRow;
	}

	private void CreateNewMTIncomeImportDoc()
	{
		try
		{
			MTIncomeImport doc = Settings.DocumentService.Create<MTIncomeImport>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeImport.Value);
			doc.DocDate = Settings.HostingEnvironment.CurrentDate;
			doc.PriceType = Settings.ParametersService.DefaultBuyingPrice();
			int num = 0;
			decimal num2 = Settings.ParametersService.VAT();
			foreach (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow selectedOrFocusedRow in ParentViewPanel.GetSelectedOrFocusedRows<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>())
			{
				if (selectedOrFocusedRow.Type == GoodsTypesSE.Good)
				{
					MTIncomeImport.MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow = new MTIncomeImport.MTIncomeImportSpecificationRow
					{
						RowID = num,
						MTID = selectedOrFocusedRow.MTID,
						MTCode = selectedOrFocusedRow.MTCode,
						MTName = selectedOrFocusedRow.Name,
						UnitCode = selectedOrFocusedRow.Unit,
						UnitBrief = selectedOrFocusedRow.UnitBrief,
						Quantity = 1m,
						Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(doc.PriceType, "1", selectedOrFocusedRow.MTID, selectedOrFocusedRow.Unit, doc.DocDate, "AMD"),
						VAT = selectedOrFocusedRow.VAT,
						Country = selectedOrFocusedRow.Country,
						Producer = selectedOrFocusedRow.Producer
					};
					decimal num3 = (mTIncomeImportSpecificationRow.VAT ? num2 : 0m);
					mTIncomeImportSpecificationRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mTIncomeImportSpecificationRow.Price, mTIncomeImportSpecificationRow.Quantity);
					mTIncomeImportSpecificationRow.CustomSumma = (mTIncomeImportSpecificationRow.Summa * doc.ExchangeRate.Rate / doc.ExchangeRate.Base).RoundAs(2);
					mTIncomeImportSpecificationRow.VATSumma = ((mTIncomeImportSpecificationRow.CustomSumma + mTIncomeImportSpecificationRow.CustomDuty) * num3 / 100m).RoundAs(2);
					doc.MTList.Add(mTIncomeImportSpecificationRow);
					num++;
				}
			}
			doc.MaxRowID = num;
			doc.SummVAT = doc.MTList.Sum((MTIncomeImport.MTIncomeImportSpecificationRow p) => ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(p.Summa, doc.VATType, p.VAT)).RoundAs(2);
			doc.Summ = doc.MTList.Sum((MTIncomeImport.MTIncomeImportSpecificationRow p) => p.Summa).RoundAs(2);
			Settings.DocumentService.ShowWindow(doc, WindowViewMode.NewCreate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CreateNewMTExpensedoc()
	{
		MTExpense mTExpense = Settings.DocumentService.Create<MTExpense>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTExpense);
		int num = 0;
		try
		{
			string text = Settings.ParametersService.DefaultStorage();
			mTExpense.DocDate = Settings.HostingEnvironment.CurrentDate;
			if (!string.IsNullOrWhiteSpace(text) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
			{
				mTExpense.Storage = text;
			}
			foreach (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow selectedOrFocusedRow in ParentViewPanel.GetSelectedOrFocusedRows<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>())
			{
				if (selectedOrFocusedRow.Type == GoodsTypesSE.Good)
				{
					mTExpense.MTList.Add(new MTExpense.MTExpenseSpecificationRow
					{
						RowID = num,
						MTID = selectedOrFocusedRow.MTID,
						MTCode = selectedOrFocusedRow.MTCode,
						MTName = selectedOrFocusedRow.Name,
						IsPartySelected = false,
						UnitCode = selectedOrFocusedRow.Unit,
						UnitBrief = selectedOrFocusedRow.UnitBrief,
						Quantity = 1m
					});
					num++;
				}
			}
			mTExpense.MaxRowID = num;
			mTExpense.Chief = Settings.ParametersService.ChiefAccountantName();
			Settings.DocumentService.ShowWindow(mTExpense, WindowViewMode.NewCreate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CreateNewMTMoveDoc()
	{
		MTMove mTMove = Settings.DocumentService.Create<MTMove>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove);
		try
		{
			FillNewMTMoveBaseDoc(mTMove);
			mTMove.Chief = Settings.ParametersService.ChiefAccountantName();
			Settings.DocumentService.ShowWindow(mTMove, WindowViewMode.NewCreate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CreateNewMTMoveWithConfirmationDoc()
	{
		MTMoveWithConfirmation mTMoveWithConfirmation = Settings.DocumentService.Create<MTMoveWithConfirmation>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation);
		try
		{
			FillNewMTMoveBaseDoc(mTMoveWithConfirmation);
			mTMoveWithConfirmation.Chief = Settings.ParametersService.ChiefAccountantName();
			Settings.DocumentService.ShowWindow(mTMoveWithConfirmation, WindowViewMode.NewCreate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CreateNewMTMoveOrderDoc()
	{
		MTMoveOrder doc = Settings.DocumentService.Create<MTMoveOrder>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveOrder);
		try
		{
			FillNewMTMoveBaseDoc(doc);
			Settings.DocumentService.ShowWindow(doc, WindowViewMode.NewCreate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void FillNewMTMoveBaseDoc(MTMoveBase doc)
	{
		int num = 0;
		string text = Settings.ParametersService.DefaultStorage();
		doc.DocDate = Settings.HostingEnvironment.CurrentDate;
		if (!string.IsNullOrWhiteSpace(text) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
		{
			doc.Storage = text;
		}
		foreach (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow selectedOrFocusedRow in ParentViewPanel.GetSelectedOrFocusedRows<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>())
		{
			if (selectedOrFocusedRow.Type == GoodsTypesSE.Good)
			{
				doc.MTList.Add(new MTMoveBase.MTMoveSpecificationRow
				{
					RowID = num,
					MTID = selectedOrFocusedRow.MTID,
					MTCode = selectedOrFocusedRow.MTCode,
					MTName = selectedOrFocusedRow.Name,
					IsPartySelected = false,
					UnitCode = selectedOrFocusedRow.Unit,
					UnitBrief = selectedOrFocusedRow.UnitBrief,
					Quantity = 1m
				});
				num++;
			}
		}
		doc.MaxRowID = num;
	}

	private void CreateNewMTInvoiceDoc()
	{
		MTInvoice mTInvoice = Settings.DocumentService.Create<MTInvoice>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice);
		try
		{
			string text = Settings.ParametersService.DefaultStorage();
			mTInvoice.SupplierBusinessAddress = Settings.ParametersService.CompanyBusinessAddress();
			if (!string.IsNullOrWhiteSpace(text))
			{
				StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(text);
				if ((item == null || !item.Retail) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
				{
					mTInvoice.Storage = text;
					StoragesCacheRow item2 = TradeSettings.TradeCacheService.Storage.GetItem(text);
					if (item2 != null && !string.IsNullOrWhiteSpace(item2.Address))
					{
						mTInvoice.SupplierBusinessAddress = item2.Address;
					}
				}
			}
			mTInvoice.PriceListType = Settings.ParametersService.DefaultSalePrice();
			mTInvoice.SupplierName = Settings.ParametersService.CompanyName();
			mTInvoice.SupplierLegalAddress = Settings.ParametersService.CompanyAddress();
			mTInvoice.SupplierSettlAcc = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount(isSettlAccount: false);
			mTInvoice.SupplierTaxCode = Settings.ParametersService.TaxCode();
			mTInvoice.SupplierMngPost = Settings.ParametersService.ManagerPost();
			mTInvoice.SupplierMngName = Settings.ParametersService.ManagerName();
			mTInvoice.SupplierChiefAccPost = Settings.ParametersService.ChiefAccountantPost();
			mTInvoice.SupplierChiefAccName = Settings.ParametersService.ChiefAccountantName();
			FillMTInvoiceBaseDoc(mTInvoice, mTInvoice.PriceListType);
			Settings.DocumentService.ShowWindow(mTInvoice, WindowViewMode.NewCreate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void FillMTInvoiceBaseDoc(MTInvoiceBase doc, string priceListType = null)
	{
		int num = 0;
		doc.DocDate = Settings.HostingEnvironment.CurrentDate;
		doc.SalesConsultant = Settings.ParametersService.DefaultSalesConsultant();
		string text = Settings.ParametersService.DefaultCustomer();
		if (!string.IsNullOrWhiteSpace(text))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(text);
			if (item != null && item.Customer)
			{
				doc.PartnerContractGroup.PartnerCode = text;
				PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(item);
				if (defaultContract != null)
				{
					doc.PartnerContractGroup.ContractCode = defaultContract.Contract;
					doc.Discount = defaultContract.Percent;
				}
			}
		}
		foreach (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow selectedOrFocusedRow in ParentViewPanel.GetSelectedOrFocusedRows<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>())
		{
			doc.MTList.Add(CreateMTInvoiceMTListRow(doc, selectedOrFocusedRow, num, priceListType));
			num++;
		}
		doc.MaxRowID = num;
		doc.SummVAT = doc.MTList.Sum((MTInvoiceBase.MTInvoiceSpecificationRow p) => ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(p.Summa, doc.VATType, Convert.ToBoolean(p.VAT.HasValue && p.VAT.Value))).RoundAs(2);
		doc.Summ = doc.MTList.Sum((MTInvoiceBase.MTInvoiceSpecificationRow p) => p.Summa).RoundAs(2);
	}

	private void CreateNewMTBillDoc()
	{
		MTBill mTBill = Settings.DocumentService.Create<MTBill>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill);
		string text = Settings.ParametersService.DefaultMTCheckPriceType();
		string text2 = Settings.ParametersService.DefaultMTCheckDiscountPriceType();
		_ = string.Empty;
		string priceListRowVATType = string.Empty;
		string priceListRowCurr = string.Empty;
		try
		{
			string text3 = Settings.ParametersService.DefaultStorage();
			if (!string.IsNullOrWhiteSpace(text3) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text3))
			{
				mTBill.Storage = text3;
			}
			if (Settings.ParametersService.DiscountUse() && !string.IsNullOrWhiteSpace(text2))
			{
				TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(text2);
				ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(text2, out priceListRowVATType, out priceListRowCurr);
				if (priceListRowCurr != mTBill.CurrCode && priceListRowCurr != "AMD")
				{
					ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(priceListRowCurr, mTBill.DocDate);
					_ = exchangeRates.Rate;
					_ = exchangeRates.Base;
				}
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(text);
			}
			FillMTInvoiceBaseDoc(mTBill, mTBill.PriceListType);
			mTBill.SummaChange = mTBill.Summ.Value;
			Settings.DocumentService.ShowWindow(mTBill, WindowViewMode.NewCreate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static MTInvoiceBase.MTInvoiceSpecificationRow CreateMTInvoiceMTListRow(MTInvoiceBase doc, ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow goodsRow, int rowId, string defaultSalePrice)
	{
		MTInvoiceBase.MTInvoiceSpecificationRow mTInvoiceSpecificationRow = new MTInvoiceBase.MTInvoiceSpecificationRow
		{
			RowID = rowId,
			ItemType = goodsRow.Type,
			Storage = doc.Storage,
			ItemID = goodsRow.MTID,
			ItemCode = goodsRow.MTCode,
			ItemName = goodsRow.Name,
			IsPartySelected = false,
			BarCode = goodsRow.BarCode,
			Quantity = 1m
		};
		if (Settings.ParametersService.DiscountUse())
		{
			mTInvoiceSpecificationRow.Discount = doc.Discount;
			if (mTInvoiceSpecificationRow.Discount == 0m)
			{
				mTInvoiceSpecificationRow.Discount = goodsRow.Discount;
			}
			else if (goodsRow.Discount != 0m)
			{
				mTInvoiceSpecificationRow.Discount = goodsRow.Discount;
			}
		}
		mTInvoiceSpecificationRow.UnitCode = goodsRow.Unit;
		mTInvoiceSpecificationRow.UnitBrief = goodsRow.UnitBrief;
		mTInvoiceSpecificationRow.VAT = goodsRow.VAT;
		mTInvoiceSpecificationRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(defaultSalePrice, doc.VATType, mTInvoiceSpecificationRow.ItemID, mTInvoiceSpecificationRow.UnitCode, Settings.HostingEnvironment.CurrentDate, "AMD");
		mTInvoiceSpecificationRow.DiscountPrice = (mTInvoiceSpecificationRow.Price - mTInvoiceSpecificationRow.Price * mTInvoiceSpecificationRow.Discount / 100m).RoundAs(2);
		mTInvoiceSpecificationRow.Summa = (mTInvoiceSpecificationRow.DiscountPrice * mTInvoiceSpecificationRow.Quantity).RoundAs(2);
		if (doc.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value && Settings.ParametersService.EnvironmentalFeePayer())
		{
			mTInvoiceSpecificationRow.EnvFeePercent = goodsRow.EcoFeeRate;
			mTInvoiceSpecificationRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mTInvoiceSpecificationRow.Summa, mTInvoiceSpecificationRow.EnvFeePercent, mTInvoiceSpecificationRow.VAT.Value, doc.VATType, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
		}
		return mTInvoiceSpecificationRow;
	}

	private void CreateNewMTInvoiceRetailDoc()
	{
		MTInvoiceRetail doc = Settings.DocumentService.Create<MTInvoiceRetail>(21);
		int num = 0;
		try
		{
			string text = Settings.ParametersService.DefaultStorage();
			if (!string.IsNullOrWhiteSpace(text))
			{
				StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(text);
				if (item != null && item.Retail && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
				{
					doc.Storage = text;
				}
			}
			doc.SalesConsultant = Settings.ParametersService.DefaultSalesConsultant();
			string text2 = Settings.ParametersService.DefaultCustomer();
			if (!string.IsNullOrWhiteSpace(text2))
			{
				PartnerCacheRow item2 = TradeSettings.TradeCacheService.Partner.GetItem(text2);
				if (item2 != null && item2.Customer)
				{
					doc.PartnerContractGroup.PartnerCode = text2;
					PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(item2);
					if (defaultContract != null)
					{
						doc.PartnerContractGroup.ContractCode = defaultContract.Contract;
						doc.Discount = defaultContract.Percent;
					}
				}
			}
			string defaultPrice = Settings.ParametersService.DefaultSalePrice();
			foreach (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow selectedOrFocusedRow in ParentViewPanel.GetSelectedOrFocusedRows<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>())
			{
				doc.MTList.Add(CreateMTInvoiceRetailMTListRow(doc, selectedOrFocusedRow, num, defaultPrice));
				num++;
			}
			doc.MaxRowID = num;
			doc.SummVAT = doc.MTList.Sum((MTInvoiceBase.MTInvoiceSpecificationRow p) => ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(p.Summa, doc.VATType, Convert.ToBoolean(p.VAT.HasValue && p.VAT.Value))).RoundAs(2);
			doc.Summ = doc.MTList.Sum((MTInvoiceBase.MTInvoiceSpecificationRow p) => p.Summa).RoundAs(2);
			Settings.DocumentService.ShowWindow(doc, WindowViewMode.NewCreate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static MTInvoiceBase.MTInvoiceSpecificationRow CreateMTInvoiceRetailMTListRow(MTInvoiceRetail doc, ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow currentRow, int rowId, string defaultPrice)
	{
		MTInvoiceBase.MTInvoiceSpecificationRow mTInvoiceSpecificationRow = new MTInvoiceBase.MTInvoiceSpecificationRow
		{
			RowID = rowId,
			ItemType = currentRow.Type,
			Storage = doc.Storage,
			ItemID = currentRow.MTID,
			ItemCode = currentRow.MTCode,
			ItemName = currentRow.Name,
			UnitCode = currentRow.Unit,
			UnitBrief = currentRow.UnitBrief,
			Quantity = 1m
		};
		if (mTInvoiceSpecificationRow.ItemType == GoodsTypesSE.Good.Value)
		{
			mTInvoiceSpecificationRow.IsPartySelected = false;
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mTInvoiceSpecificationRow.ItemCode, mTInvoiceSpecificationRow.UnitCode);
			if (qntUnit != null)
			{
				MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
				{
					MTCode = mTInvoiceSpecificationRow.ItemCode,
					UnitCoef = qntUnit.Coef,
					Storage = doc.Storage,
					Date = Settings.HostingEnvironment.CurrentDate
				};
				List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
				if (rows.Count > 0)
				{
					mTInvoiceSpecificationRow.Price = rows[0].Price;
				}
			}
		}
		else
		{
			mTInvoiceSpecificationRow.IsPartySelected = null;
			if (TradeSettings.TradeCacheService.Material.GetQntUnit(mTInvoiceSpecificationRow.ItemCode, mTInvoiceSpecificationRow.UnitCode) != null)
			{
				mTInvoiceSpecificationRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(defaultPrice, doc.VATType, mTInvoiceSpecificationRow.ItemID, mTInvoiceSpecificationRow.UnitCode, Settings.HostingEnvironment.CurrentDate, "AMD");
			}
		}
		if (Settings.ParametersService.DiscountUse())
		{
			mTInvoiceSpecificationRow.Discount = doc.Discount;
			if (mTInvoiceSpecificationRow.Discount == 0m)
			{
				mTInvoiceSpecificationRow.Discount = currentRow.Discount;
			}
			else if (currentRow.Discount != 0m)
			{
				mTInvoiceSpecificationRow.Discount = currentRow.Discount;
			}
		}
		mTInvoiceSpecificationRow.DiscountPrice = (mTInvoiceSpecificationRow.Price * (1m - mTInvoiceSpecificationRow.Discount / 100m)).RoundAs(2);
		mTInvoiceSpecificationRow.Summa = (mTInvoiceSpecificationRow.Quantity * mTInvoiceSpecificationRow.DiscountPrice).RoundAs(2);
		mTInvoiceSpecificationRow.VAT = currentRow.VAT;
		mTInvoiceSpecificationRow.IsPartySelected = false;
		if (Settings.ParametersService.EnvironmentalFeePayer())
		{
			mTInvoiceSpecificationRow.EnvFeePercent = currentRow.EcoFeeRate;
			mTInvoiceSpecificationRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mTInvoiceSpecificationRow.Summa, mTInvoiceSpecificationRow.EnvFeePercent, mTInvoiceSpecificationRow.VAT.Value, doc.VATType, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
		}
		return mTInvoiceSpecificationRow;
	}

	private void ExportForScale()
	{
		List<MaterialCacheRow> list = new List<MaterialCacheRow>();
		foreach (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow selectedOrFocusedRow in ParentViewPanel.GetSelectedOrFocusedRows<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(selectedOrFocusedRow.MTCode);
			if (item != null && item.Type == GoodsTypesSE.Good && item.IsWeight)
			{
				list.Add(item);
			}
		}
		new ElScaleExportWindow(list).ShowDialog();
	}

	private void ExportForTerminal()
	{
		new TerminalExportWindow(GetSourceList(), allowChangeUnit: true).ShowDialog();
	}

	private List<TerminalRow> GetSourceList()
	{
		List<TerminalRow> list = new List<TerminalRow>();
		foreach (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow selectedOrFocusedRow in ParentViewPanel.GetSelectedOrFocusedRows<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>())
		{
			if (selectedOrFocusedRow.Type == GoodsTypesSE.Good)
			{
				list.Add(new TerminalRow
				{
					Id = selectedOrFocusedRow.MTID,
					Code = selectedOrFocusedRow.MTCode,
					Name = selectedOrFocusedRow.Name,
					Unit = selectedOrFocusedRow.Unit,
					AlternativeUnit = selectedOrFocusedRow.AltUnit,
					BarCode = null,
					Quantity = 1
				});
			}
		}
		return list;
	}

	private void GroupExportForAll()
	{
		new GroupElScaleAndTerminalExportWindow(GetSourceListForElScales(), GetSourceListForTerminals()).ShowDialog();
	}

	private List<MaterialCacheRow> GetSourceListForElScales()
	{
		List<MaterialCacheRow> list = new List<MaterialCacheRow>();
		foreach (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow selectedOrFocusedRow in ParentViewPanel.GetSelectedOrFocusedRows<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>())
		{
			MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(selectedOrFocusedRow.MTID);
			if (itemById != null && itemById.Type == GoodsTypesSE.Good.Value && itemById.IsWeight)
			{
				list.Add(itemById);
			}
		}
		return list;
	}

	private List<TerminalRow> GetSourceListForTerminals()
	{
		List<TerminalRow> list = new List<TerminalRow>();
		foreach (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow selectedOrFocusedRow in ParentViewPanel.GetSelectedOrFocusedRows<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>())
		{
			if (selectedOrFocusedRow.Type == GoodsTypesSE.Good.Value)
			{
				TerminalRow item = new TerminalRow
				{
					Id = selectedOrFocusedRow.MTID,
					Code = selectedOrFocusedRow.MTCode,
					Name = selectedOrFocusedRow.Name,
					Unit = selectedOrFocusedRow.Unit,
					AlternativeUnit = selectedOrFocusedRow.AltUnit,
					BarCode = null,
					Quantity = 1m
				};
				list.Add(item);
			}
		}
		return list;
	}

	private void GenerateNewBarcode()
	{
		int num = 0;
		try
		{
			Mouse.OverrideCursor = Cursors.Wait;
			List<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow> selectedOrFocusedRows = ParentViewPanel.GetSelectedOrFocusedRows<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>();
			if (Settings.ParametersService.InternalBarcodeEANFormat())
			{
				foreach (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow item in selectedOrFocusedRows)
				{
					num += GenerateNewEANBarcodeForOneRow(item.MTCode, selectedOrFocusedRows.Count > 1);
				}
			}
			else
			{
				foreach (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow item2 in selectedOrFocusedRows)
				{
					num += GenerateNewBarcodeForOneRow(item2.MTCode, selectedOrFocusedRows.Count > 1);
				}
			}
			Mouse.OverrideCursor = null;
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.GenerateBarcodeCompleted, num), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private int GenerateNewEANBarcodeForOneRow(string goodCode, bool singleRow = false)
	{
		Good good = Settings.DirectoryService.Load<Good>(DirectoryType, goodCode);
		try
		{
			if (string.IsNullOrWhiteSpace(good.BarCode))
			{
				good.BarCode = MaterialHelpers.GenerateInnerBarcode();
				Settings.DirectoryService.Store(good, new DirStoreRequest());
				Settings.ParametersService.InternalBarcodeNext_set(ArmSoft.SME.Common.Extensions.StringExtensions.NextDocNum(Settings.ParametersService.InternalBarcodeNext(), 10));
				return 1;
			}
		}
		catch
		{
			if (singleRow)
			{
				throw;
			}
		}
		return 0;
	}

	private int GenerateNewBarcodeForOneRow(string goodCode, bool singleRow = false)
	{
		Good good = Settings.DirectoryService.Load<Good>(DirectoryType, goodCode);
		try
		{
			if (string.IsNullOrWhiteSpace(good.BarCode))
			{
				good.BarCode = Settings.ParametersService.InternalBarcodeNext();
				Settings.DirectoryService.Store(good, new DirStoreRequest());
				Settings.ParametersService.InternalBarcodeNext_set(ArmSoft.SME.Common.Extensions.StringExtensions.NextDocNum(good.BarCode, 20));
				return 1;
			}
		}
		catch
		{
			if (singleRow)
			{
				throw;
			}
		}
		return 0;
	}

	private void ShowPartnersMT()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow dataRow = (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				PartnersGoods partnersGoods = new PartnersGoods();
				partnersGoods.MaterialCode = dataRow.MTCode;
				partnersGoods.Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
			}
		}
	}

	private void OrderMinMax()
	{
		ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow dataRow = (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow)ParentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			OrderMinMax orderMinMax = new OrderMinMax();
			orderMinMax.MTCode = dataRow.MTCode;
			orderMinMax.Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
		}
	}

	private void ShowSegments()
	{
		ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow dataRow = (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow)ParentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			GoodsBySegments goodsBySegments = new GoodsBySegments();
			goodsBySegments.GoodCode = dataRow.MTCode;
			goodsBySegments.Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
		}
	}

	private void ChangeGoodsBySegments()
	{
		InitContextFunctionsHelper.ChangeGoodsBySegments((from p in ParentViewPanel.GetSelectedOrFocusedRows<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>()
			select p.MTCode).ToList());
	}

	private void AddToMTFavoritesDialog()
	{
		try
		{
			if ((ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow)ParentViewPanel.FocusedRow() == null)
			{
				return;
			}
			DialogWindow dialogWindow = new DialogWindow
			{
				Caption = ArmSoft.SME.Trade.Common.Resources.AddMTFavToUser
			};
			DropDownControl dropDownControl = dialogWindow.AddDropDownControl("UserName", ArmSoft.SME.Common.Resources.User, new UsersFilter(), isRequired: true);
			dropDownControl.Code = Settings.LoginService.UserName;
			DropDownControl mtFavGroup = dialogWindow.AddDropDownControl("Group", ArmSoft.SME.Common.Resources.Group, new ArmSoft.SME.Trade.UI.DropDown.MTFavorites(Settings.LoginService.UserId, onlyMaterials: false, onlyGroups: true));
			dialogWindow.SetFocusOnActivation = mtFavGroup;
			dropDownControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>((object? s, EditValueChangedEventArgs e) =>
			{
				User_EditValueChanged(s, mtFavGroup);
			}).Handler;
			if (!dialogWindow.ShowDialog().Value)
			{
				return;
			}
			List<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow> selectedOrFocusedRows = ParentViewPanel.GetSelectedOrFocusedRows<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow>();
			int num = 0;
			foreach (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow item in selectedOrFocusedRows)
			{
				try
				{
					MTFavoriteMaterial mTFavoriteMaterial = Settings.DirectoryService.Create<MTFavoriteMaterial>(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.MTFavoriteMaterial.Value);
					mTFavoriteMaterial.Id = item.MTID;
					mTFavoriteMaterial.Code = item.MTCode;
					mTFavoriteMaterial.QntUnit = item.Unit;
					mTFavoriteMaterial.GroupCode = mtFavGroup.Code;
					mTFavoriteMaterial.UserId = Settings.CoreCacheService.User.GetItemByName(dropDownControl.Code).UserId;
					Settings.DirectoryService.Store(mTFavoriteMaterial, new DirStoreRequest());
					num++;
				}
				catch (ASException ex) when (ex.ErrorCode == 20005)
				{
				}
			}
			if (num == 0)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.SelectedMaterialsAlreadyExists, dropDownControl.Code), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			}
			else
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.SelectedMaterialsAddToSelectedUser, dropDownControl.Code), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			}
		}
		catch (Exception ex2)
		{
			Settings.ErrorHandler(ex2);
		}
	}

	private void ShowMTFavorites()
	{
		MTFavorites mTFavorites = new MTFavorites(Settings.LoginService.UserId);
		mTFavorites.UserId = Settings.LoginService.UserId;
		mTFavorites.UserName = (string.IsNullOrWhiteSpace(Settings.LoginService.UserDescription) ? Settings.LoginService.UserName : Settings.LoginService.UserDescription);
		mTFavorites.Show();
	}

	private static void User_EditValueChanged(object sender, DropDownControl favGroup)
	{
		favGroup.Code = "";
		int userId = Settings.CoreCacheService.User.GetItemByName(((DropDownControl)sender).Code).UserId;
		favGroup.DataView = new ArmSoft.SME.Trade.UI.DropDown.MTFavorites(userId, onlyMaterials: false, onlyGroups: true);
	}

	public override bool CardViewAllowed()
	{
		return true;
	}

	public override DataTemplate GetCardHeaderTemplate()
	{
		return (DataTemplate)Application.Current.Resources["GoodsCardHeaderTemplate"];
	}

	public override DataTemplate GetCardTemplate()
	{
		return (DataTemplate)Application.Current.Resources["GoodsCardTemplate"];
	}

	public override DataTemplate GetCardPrintTemplate()
	{
		return (DataTemplate)Application.Current.Resources["GoodsCardPrintTemplate"];
	}

	public override ToolTip ToolTipContent(object sender)
	{
		ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow dataRow = (ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow)((Image)sender).Tag;
		if (dataRow == null)
		{
			return null;
		}
		int mTID = dataRow.MTID;
		if (dataRow.ImageExists && dataRow.Image == null)
		{
			dataRow.Image = Settings.ImagesService.GetItemImage(ArmSoft.SME.Trade.Common.SmartEnums.ImageItemTypeSE.Materials.Value, TradeSettings.TradeCacheService.Material.GetQntUnit(mTID, dataRow.Unit)?.ImageGUID.ToString());
		}
		if (dataRow.Image != null && dataRow.Image.Length != 0)
		{
			LayoutItemWithRequired layoutItemWithRequired = new LayoutItemWithRequired
			{
				LabelPosition = LayoutItemLabelPosition.Top,
				Label = dataRow.Name
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
				HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
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
