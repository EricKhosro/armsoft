using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.Documents.Goods;

public partial class MTSelectPartyWindow : ThemedWindow, IComponentConnector
{
	private bool _isReadOnly;

	private DropDownControl _storage;

	private DropDownControl _mtCode;

	private TextEditExt _qntUnit;

	private DocumentGridControl _partiesGrid;

	private DocumentGridColumn _quantityCol;

	private TableView _partiesTableView;

	private string _expMethod;

	private bool _isStorageAllowNegExp;

	private bool _baseUnit;

	private List<PartyRow> _mtCodeSelectedPartiesList = new List<PartyRow>();

	private int _gridRow;

	private const double DefaultWidth = 870.0;

	private const double DefaultHeight = 560.0;

	public string Storage { get; set; }

	public DateOnly Date { get; set; }

	public string MTCode { get; set; }

	public string UnitBrief { get; set; }

	public string UnitCode { get; set; }

	public int RowID { get; set; }

	public bool? IsPartySelected { get; set; }

	public decimal TotalExpQty { get; set; }

	public decimal TotalExpSumm { get; set; }

	public List<PartyRow> MTExpDocSelectedPartiesList { get; set; } = new List<PartyRow>();

	public MTSelectPartyWindow(string storage, DateOnly date, string mtCode, string unitCode, decimal quantity, int rowID, bool isReadOnly)
	{
		InitializeComponent();
		LayoutControl.ScrollBars = ScrollBars.Auto;
		Title = ArmSoft.SME.Common.Resources.ExpSpecification + " [" + date.FormatDDMMYY() + "]";
		ImageSource icon = new ImageSourceConverter().ConvertFromString(Settings.AppIconPath) as ImageSource;
		Icon = icon;
		OkButton.Content = ArmSoft.SME.Common.Resources.Ok;
		CancelButton.Content = ArmSoft.SME.Common.Resources.Cancel;
		AutoSelectButton.Content = ArmSoft.SME.Common.Resources.AutoSelect;
		Storage = storage;
		Date = date;
		MTCode = mtCode;
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
		if (item == null)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, mtCode));
		}
		_baseUnit = item.Unit == unitCode;
		_expMethod = item.ExpenseMethod;
		_isStorageAllowNegExp = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(Storage);
		if (_baseUnit)
		{
			UnitBrief = item.UnitRow.Brief;
		}
		else if (item.AltUnit == unitCode)
		{
			UnitBrief = item.AltUnitRow.Brief;
		}
		else
		{
			QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(unitCode);
			UnitBrief = ((item2 != null) ? item2.Brief : string.Empty);
		}
		UnitCode = unitCode;
		TotalExpQty = quantity;
		RowID = rowID;
		_isReadOnly = isReadOnly;
		_storage = new DropDownControl();
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.Storage);
		CommonEventHandlers.AddIsRequiredBinding(_storage, layoutItemWithRequired);
		_storage.AllowAdditionButtons = false;
		_storage.DataView = new Storages();
		_storage.Owner = this;
		_storage.Code = storage;
		_storage.Width = _storage.RecommendedWidth;
		_storage.IsRequired = true;
		_storage.IsReadOnly = true;
		layoutItemWithRequired.Content = _storage;
		_mtCode = new DropDownControl();
		layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.Code);
		CommonEventHandlers.AddIsRequiredBinding(_mtCode, layoutItemWithRequired);
		_mtCode.AllowAdditionButtons = false;
		_mtCode.CodeWidth = 8;
		_mtCode.DataView = new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value, "", isShowOnlyInPriceList: true);
		_mtCode.Owner = this;
		_mtCode.Code = mtCode;
		_mtCode.Width = _mtCode.RecommendedWidth;
		_mtCode.IsRequired = true;
		_mtCode.IsReadOnly = true;
		layoutItemWithRequired.Content = _mtCode;
		layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Trade.Common.Resources.QntUnit);
		_qntUnit = new TextEditExt();
		CommonEventHandlers.AddIsRequiredBinding(_qntUnit, layoutItemWithRequired);
		_qntUnit.IsRequired = true;
		_qntUnit.IsReadOnly = true;
		_qntUnit.MaxLength = 6;
		_qntUnit.MinWidth = 5.0;
		_qntUnit.Text = UnitBrief;
		_qntUnit.Width = SizeCalculator.WidthForString(6);
		layoutItemWithRequired.Content = _qntUnit;
		_partiesGrid = new DocumentGridControl();
		_partiesGrid.Owner = this;
		LayoutControl.Children.Add(_partiesGrid);
		_partiesGrid.AllowAdd = false;
		_partiesGrid.AllowDelete = false;
		_partiesGrid.AllowDuplicate = false;
		_partiesGrid.ShowButtons = false;
		DocumentGridColumn documentGridColumn = _partiesGrid.AddDateColumn("IncomeDate", ArmSoft.SME.Common.Resources.IncomeDateShort);
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		documentGridColumn.Visible = Settings.ParametersService.ShowPartyIncomeDate();
		DocumentGridColumn documentGridColumn2 = _partiesGrid.AddNumberColumn("Cost", ArmSoft.SME.Common.Resources.MtCurCost, 16, 2);
		documentGridColumn2.AllowEditing = DefaultBoolean.False;
		documentGridColumn2.Visible = Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
		_partiesGrid.AddNumberColumn("Qty", ArmSoft.SME.Common.Resources.Availability, 16, 3, allowNegativeNumbers: false, showTotal: true).AllowEditing = DefaultBoolean.False;
		_quantityCol = _partiesGrid.AddNumberColumn("ExpQty", ArmSoft.SME.Common.Resources.Quantity, 13, 3, allowNegativeNumbers: false, showTotal: true);
		DocumentGridColumn documentGridColumn3 = _partiesGrid.AddStringColumn("PartnerCode", ArmSoft.SME.Common.Resources.Supplier, 20);
		documentGridColumn3.Width = 100.0;
		documentGridColumn3.ReadOnly = true;
		documentGridColumn3.Visible = Settings.ParametersService.ShowPartySUPPLIER();
		DocumentGridColumn documentGridColumn4 = _partiesGrid.AddStringColumn("PartnerCaption", ArmSoft.SME.Common.Resources.SupplierName, 20);
		documentGridColumn4.Width = 100.0;
		documentGridColumn4.ReadOnly = true;
		documentGridColumn4.Visible = Settings.ParametersService.ShowPartySUPPLIER();
		DocumentGridColumn documentGridColumn5 = _partiesGrid.AddStringColumn("Contract", ArmSoft.SME.Common.Resources.Contract, 50);
		documentGridColumn5.Width = 100.0;
		documentGridColumn5.ReadOnly = true;
		documentGridColumn5.Visible = Settings.ParametersService.ShowPartyCONTRACT();
		DocumentGridColumn documentGridColumn6 = _partiesGrid.AddStringColumn("Comment", ArmSoft.SME.Common.Resources.IncomeDoc, 50);
		documentGridColumn6.Width = 100.0;
		documentGridColumn6.ReadOnly = true;
		DocumentGridColumn documentGridColumn7 = _partiesGrid.AddDateColumn("UseableDate", ArmSoft.SME.Trade.Common.Resources.UseLife);
		documentGridColumn7.ReadOnly = true;
		documentGridColumn7.Visible = Settings.ParametersService.ShowPartyUSEABLEDATE();
		DocumentGridColumn documentGridColumn8 = _partiesGrid.AddStringColumn("Country", ArmSoft.SME.Common.Resources.Country, 50);
		documentGridColumn8.Width = 100.0;
		documentGridColumn8.ReadOnly = true;
		documentGridColumn8.Visible = Settings.ParametersService.ShowPartyCountry();
		DocumentGridColumn documentGridColumn9 = _partiesGrid.AddStringColumn("Producer", ArmSoft.SME.Common.Resources.Producer, 50);
		documentGridColumn9.Width = 100.0;
		documentGridColumn9.ReadOnly = true;
		documentGridColumn9.Visible = Settings.ParametersService.ShowPartyProducer();
		DocumentGridColumn documentGridColumn10 = _partiesGrid.AddStringColumn("Property1", ArmSoft.SME.Trade.Common.Resources.Properties, 50);
		documentGridColumn10.Width = 100.0;
		documentGridColumn10.ReadOnly = true;
		documentGridColumn10.Visible = Settings.ParametersService.ShowPartyProperty1();
		DocumentGridColumn documentGridColumn11 = _partiesGrid.AddStringColumn("Property2", ArmSoft.SME.Trade.Common.Resources.Properties, 50);
		documentGridColumn11.Width = 100.0;
		documentGridColumn11.ReadOnly = true;
		documentGridColumn11.Visible = Settings.ParametersService.ShowPartyProperty2();
		_partiesTableView = _partiesGrid.View;
		_partiesGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
		_partiesGrid.VerticalAlignment = VerticalAlignment.Stretch;
		MinWidth = SizeCalculator.Width(620.0);
		MinHeight = SizeCalculator.Height(250.0);
		if (_isReadOnly)
		{
			_mtCode.IsReadOnly = true;
			_partiesGrid.IsReadOnly = true;
			_partiesGrid.Grid.Columns["Qty"].Visible = false;
			OkButton.Visibility = Visibility.Collapsed;
			AutoSelectButton.Visibility = Visibility.Collapsed;
		}
		if (_expMethod == "3")
		{
			_partiesGrid.Grid.Columns["PartnerCode"].Visible = false;
			_partiesGrid.Grid.Columns["PartnerCaption"].Visible = false;
			_partiesGrid.Grid.Columns["Contract"].Visible = false;
			_partiesGrid.Grid.Columns["Comment"].Visible = false;
			GridColumn gridColumn = _partiesGrid.Grid.Columns.FirstOrDefault((GridColumn c) => c.Name.Contains("UseableDate"));
			if (gridColumn != null)
			{
				gridColumn.Visible = false;
			}
			_partiesGrid.Grid.Columns["Country"].Visible = false;
			_partiesGrid.Grid.Columns["Producer"].Visible = false;
			_partiesGrid.Grid.Columns["Property1"].Visible = false;
			_partiesGrid.Grid.Columns["Property2"].Visible = false;
		}
		_partiesGrid.ControlEnterPressed += new SafeEventHandler<EventArgs>(OnGridControlEnterPressed).Handler;
		_partiesTableView.CellValueChanged += new SafeEventHandler<CellValueChangedEventArgs>(PartiesTableView_CellValueChanged).Handler;
		_partiesGrid.EscapePressed += new SafeEventHandler<EventArgs>(PartiesGrid_EscapePressed).Handler;
		_mtCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(DDMT_EditValueChanged).Handler;
		OkButton.Click += new SafeEventHandler<RoutedEventArgs>(OkButton_Click).Handler;
		AutoSelectButton.Click += new SafeEventHandler<RoutedEventArgs>(AutoSelectButton_Click).Handler;
		CancelButton.Click += new SafeEventHandler<RoutedEventArgs>(CancelButton_Click).Handler;
		_storage.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(MTSelectPartyWindow_Closing).Handler;
		Loaded += new SafeEventHandler<RoutedEventArgs>(MTSelectPartyWindow_Loaded).Handler;
		_partiesTableView.CellValueChanging += new SafeEventHandler<CellValueChangedEventArgs>(PartiesTableView_CellValueChanging).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(MTSelectPartyWindow_PreviewKeyDown).Handler;
		ASFontHelper.SetFontSettings(this);
	}

	public void Load()
	{
		if (!_isReadOnly)
		{
			decimal num = TotalExpQty;
			GoodsEngineServicesRoutes.PartyRowAndExpMethod parties = TradeSettings.CreateTradeClient().GoodsEngineServices.GetParties(Storage, MTCode, Date);
			_mtCodeSelectedPartiesList = parties.partyRows;
			_expMethod = parties.expMethod;
			if (!_baseUnit)
			{
				foreach (PartyRow mtCodeSelectedParties in _mtCodeSelectedPartiesList)
				{
					string mTCode = TradeSettings.TradeCacheService.Material.GetItemById(mtCodeSelectedParties.GoodID).MTCode;
					mtCodeSelectedParties.Qty = (mtCodeSelectedParties.Qty / PriceHelper.GetMTQntUnitCoeff(mTCode, UnitCode)).RoundAs(3);
				}
			}
			_gridRow = RowID;
			IEnumerable<PartyRow> enumerable = MTExpDocSelectedPartiesList.Where((PartyRow x) => x.GridRow == _gridRow);
			if (enumerable.Count() > 0)
			{
				foreach (PartyRow Row in enumerable)
				{
					PartyRow partyRow = _mtCodeSelectedPartiesList.FirstOrDefault((PartyRow x) => x.Base == Row.Base && x.Row == Row.Row);
					if (partyRow == null)
					{
						continue;
					}
					if (!_isStorageAllowNegExp)
					{
						if (partyRow.Qty >= Row.ExpQty)
						{
							partyRow.ExpQty = Row.ExpQty;
						}
						else
						{
							partyRow.ExpQty = partyRow.Qty;
						}
					}
					else
					{
						partyRow.ExpQty = Row.ExpQty;
					}
				}
			}
			else
			{
				decimal num2 = 0m;
				foreach (PartyRow mtCodeSelectedParties2 in _mtCodeSelectedPartiesList)
				{
					num2 += mtCodeSelectedParties2.Qty;
				}
				foreach (PartyRow mtCodeSelectedParties3 in _mtCodeSelectedPartiesList)
				{
					if (!_isStorageAllowNegExp)
					{
						if (mtCodeSelectedParties3.Qty > 0m)
						{
							if (num >= mtCodeSelectedParties3.Qty)
							{
								mtCodeSelectedParties3.ExpQty = mtCodeSelectedParties3.Qty;
								num -= mtCodeSelectedParties3.Qty;
							}
							else
							{
								mtCodeSelectedParties3.ExpQty = num;
								num = 0m;
							}
						}
					}
					else if (num >= mtCodeSelectedParties3.Qty)
					{
						if (mtCodeSelectedParties3 == _mtCodeSelectedPartiesList[_mtCodeSelectedPartiesList.Count - 1])
						{
							mtCodeSelectedParties3.ExpQty = num;
							num = 0m;
						}
						else
						{
							mtCodeSelectedParties3.ExpQty = mtCodeSelectedParties3.Qty;
							num -= mtCodeSelectedParties3.Qty;
						}
					}
					else
					{
						mtCodeSelectedParties3.ExpQty = num;
						num = 0m;
					}
				}
			}
			_partiesGrid.ItemsSource = _mtCodeSelectedPartiesList;
		}
		else
		{
			_gridRow = RowID;
			if (MTExpDocSelectedPartiesList.Count > 0)
			{
				IEnumerable<PartyRow> enumerable2 = MTExpDocSelectedPartiesList.Where((PartyRow x) => x.GridRow == _gridRow);
				if (enumerable2 != null)
				{
					_partiesGrid.ItemsSource = enumerable2;
				}
			}
		}
		WindowStartupLocation = WindowStartupLocation.CenterOwner;
	}

	private LayoutItemWithRequired AddLayoutItem(string caption)
	{
		LayoutItemWithRequired layoutItemWithRequired = new LayoutItemWithRequired();
		layoutItemWithRequired.Label = caption;
		LayoutControl.Children.Add(layoutItemWithRequired);
		layoutItemWithRequired.HorizontalAlignment = HorizontalAlignment.Left;
		return layoutItemWithRequired;
	}

	private void PartiesTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		if (e.Column != _quantityCol)
		{
			return;
		}
		PartyRow partyRow = (PartyRow)_partiesGrid.Grid.GetRow(e.RowHandle);
		if (!_isStorageAllowNegExp)
		{
			if (Convert.ToDecimal(e.Value) > partyRow.Qty || Convert.ToDecimal(e.Value) < 0m)
			{
				partyRow.ExpQty = partyRow.Qty;
			}
		}
		else if (Convert.ToDecimal(e.Value) < 0m)
		{
			partyRow.ExpQty = partyRow.Qty;
		}
		_partiesGrid.Grid.RefreshRow(e.RowHandle);
	}

	private void PartiesGrid_EscapePressed(object sender, EventArgs e)
	{
		DialogResult = false;
	}

	private void DDMT_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_mtCode.Code != "" && _mtCode.Code != MTCode)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code);
			if (item != null)
			{
				_qntUnit.Text = item.UnitRow.Brief;
				_expMethod = item.ExpenseMethod;
				GoodsEngineServicesRoutes.PartyRowAndExpMethod parties = TradeSettings.CreateTradeClient().GoodsEngineServices.GetParties(Storage, _mtCode.Code, Date);
				_mtCodeSelectedPartiesList = parties.partyRows;
				_expMethod = parties.expMethod;
				_partiesGrid.ItemsSource = _mtCodeSelectedPartiesList;
				MTCode = _mtCode.Code;
			}
			else
			{
				MTCode = "";
			}
		}
	}

	private void OkButton_Click(object sender, RoutedEventArgs e)
	{
		Save();
	}

	protected void Save()
	{
		try
		{
			if (!Validate())
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldValueGraterZero, ArmSoft.SME.Common.Resources.Quantity));
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			return;
		}
		MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == _gridRow);
		foreach (PartyRow mtCodeSelectedParties in _mtCodeSelectedPartiesList)
		{
			PartyRow partyRow = new PartyRow
			{
				GoodID = mtCodeSelectedParties.GoodID,
				GridRow = RowID,
				Base = mtCodeSelectedParties.Base,
				Row = mtCodeSelectedParties.Row,
				Qty = mtCodeSelectedParties.Qty,
				ExpQty = mtCodeSelectedParties.ExpQty,
				Comment = mtCodeSelectedParties.Comment,
				Cost = mtCodeSelectedParties.Cost,
				ExpSumma = GetExpSumma(mtCodeSelectedParties),
				IncomeDate = mtCodeSelectedParties.IncomeDate
			};
			if (mtCodeSelectedParties.ExpQty > 0m)
			{
				TotalExpSumm += partyRow.ExpSumma;
			}
			MTExpDocSelectedPartiesList.Add(partyRow);
		}
		TotalExpQty = Convert.ToDecimal(_partiesGrid.Grid.Columns["ExpQty"].TotalSummaries[0].Value);
		IsPartySelected = true;
		DialogResult = true;
		Close();
	}

	private decimal GetExpSumma(PartyRow tmpEl)
	{
		if (tmpEl.ExpQty == tmpEl.Qty)
		{
			return tmpEl.Summa;
		}
		if (!_isStorageAllowNegExp)
		{
			return (tmpEl.Cost * tmpEl.ExpQty).RoundAs(2);
		}
		if (tmpEl.Cost * tmpEl.ExpQty < tmpEl.Qty)
		{
			return tmpEl.ExpQty.RoundAs(2);
		}
		return tmpEl.Qty.RoundAs(2);
	}

	protected bool Validate()
	{
		return Convert.ToDecimal(_partiesGrid.Grid.Columns["ExpQty"].TotalSummaries[0].Value) != 0m;
	}

	private void AutoSelectButton_Click(object sender, RoutedEventArgs e)
	{
		MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == _gridRow);
		TotalExpQty = Convert.ToDecimal(_partiesGrid.Grid.Columns["ExpQty"].TotalSummaries[0].Value);
		IsPartySelected = false;
		DialogResult = true;
		Close();
	}

	private void CancelButton_Click(object sender, RoutedEventArgs e)
	{
		DialogResult = false;
		IsPartySelected = null;
	}

	private void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_storage.Code) && _storage.Code != Storage)
		{
			_mtCode.DoValidate();
			Storage = _storage.Code;
		}
	}

	private void OnGridControlEnterPressed(object sender, EventArgs e)
	{
		_partiesTableView.CommitEditing();
		Save();
	}

	protected void RestoreSettings()
	{
		bool flag = false;
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\SelectPartyWindow", writable: false);
			if (registryKey == null)
			{
				flag = true;
			}
			else
			{
				Settings.RestoreWindowSize(this, registryKey, 870.0, 560.0);
				if (registryKey.GetValue("State", "1").ToString() == "0")
				{
					WindowState = WindowState.Maximized;
				}
				else
				{
					WindowState = WindowState.Normal;
				}
			}
		}
		catch (Exception)
		{
			flag = true;
		}
		if (flag)
		{
			Width = SizeCalculator.Width(870.0);
			Height = SizeCalculator.Height(560.0);
		}
	}

	protected void StoreSettings()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\SelectPartyWindow");
			if (registryKey != null)
			{
				if (WindowState == WindowState.Normal)
				{
					registryKey.SetValue("Width", Width.ToString());
					registryKey.SetValue("Height", Height.ToString());
					registryKey.SetValue("State", "1");
				}
				else if (WindowState == WindowState.Maximized)
				{
					registryKey.SetValue("State", "0");
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private void MTSelectPartyWindow_Closing(object sender, CancelEventArgs e)
	{
		if (!e.Cancel)
		{
			StoreSettings();
		}
	}

	private void MTSelectPartyWindow_Loaded(object sender, RoutedEventArgs e)
	{
		RestoreSettings();
		if (!_isReadOnly && (_expMethod == "1" || _expMethod == "2"))
		{
			_partiesTableView.Grid.CurrentColumn = _partiesGrid.Grid.Columns["ExpQty"];
			_partiesTableView.ShowEditor();
		}
	}

	private void PartiesTableView_CellValueChanging(object sender, CellValueChangedEventArgs e)
	{
		_partiesTableView.CommitEditing();
	}

	private void MTSelectPartyWindow_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 2)
		{
			if (_isReadOnly)
			{
				e.Handled = true;
				Close();
			}
			else
			{
				OnGridControlEnterPressed(sender, e);
				e.Handled = true;
			}
		}
	}
}
