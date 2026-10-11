using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ArmSoft.SME.Client.Models.Directories;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.DPR;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using DevExpress.Utils;
using DevExpress.Xpf.Grid;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

internal class ConfirmPricesWindow : DialogWindow
{
	private BindingList<SelectedMaterialsRow> _rows;

	private DocumentGridControl _priceFormingGrid;

	private TableView _priceFormingTableView;

	private DocumentGridColumn _mtCodeCol;

	private DocumentGridColumn _mtNameCol;

	private DocumentGridColumn _basePriceCol;

	private DocumentGridColumn _oldPriceCol;

	private DocumentGridColumn _percentCol;

	private DocumentGridColumn _newPriceCol;

	private DocumentGridColumn _qntUnitCol;

	private DocumentGridColumn _isCheckedCol;

	private string _priceType = "";

	private string _priceTypeCaption = "";

	private string _comment = "";

	private DateOnly _date;

	private string _currCode = "";

	private readonly string _operType = "";

	private readonly bool _isBasePriceTypePurchaseOrSale;

	private readonly List<PriceFormationRequestProperties> _properties = new List<PriceFormationRequestProperties>();

	public ConfirmPricesWindow(List<SelectedMaterialsRow> rows, string operType, string basePriceType)
	{
		Caption = ArmSoft.SME.Trade.Common.Resources.PricesFormation;
		_rows = new BindingList<SelectedMaterialsRow>();
		_operType = operType;
		_isBasePriceTypePurchaseOrSale = basePriceType == "S4" || basePriceType == "S5" || basePriceType == "S7";
		foreach (SelectedMaterialsRow row in rows)
		{
			if (row.BasePrice != 0m)
			{
				row.Percent = (row.NewPrice * 100m / row.BasePrice - 100m).RoundAs(2);
			}
			_rows.Add(row);
		}
		CompactRows();
		WindowValidation += ConfirmPricesWindow_Validation;
	}

	protected override void RestoreSettings(RegistryKey rk)
	{
		if (double.TryParse(rk.GetValue("Width", "0").ToString(), out var result) && result > 0.0)
		{
			Width = result;
		}
		if (double.TryParse(rk.GetValue("Height", "0").ToString(), out var result2) && result2 > 0.0)
		{
			Height = result2;
		}
		WindowState = WindowState.Normal;
		LoadColumnWidthsFromRegistry(_priceFormingGrid.Grid.Columns, rk.OpenSubKey("PriceFormationColumns", writable: false));
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		if (rk != null && WindowState != WindowState.Minimized)
		{
			rk.SetValue("Width", Width.ToString());
			rk.SetValue("Height", Height.ToString());
			rk.SetValue("WindowState", WindowState);
		}
		SaveColumnWidthsInRegistry(_priceFormingGrid.Grid.Columns, rk.CreateSubKey("PriceFormationColumns"));
	}

	internal bool Invoke()
	{
		_priceType = Properties["PriceType"].ToString().Trim();
		_currCode = Properties["CurrCode"].ToString().Trim();
		_date = (DateOnly)Properties["Date"];
		_priceTypeCaption = Properties["PriceTypeCaption"].ToString().Trim();
		_comment = Properties["Comment"].ToString().Trim();
		string text = Properties["VAT"].ToString().Trim();
		string text2 = Properties["BasePriceType"].ToString().Trim();
		string arg = Properties["BasePriceCurrCode"].ToString().Trim();
		string text3 = Properties["BasePriceVAT"].ToString().Trim();
		bool flag = (bool)Properties["AutoForming"];
		Caption = InnerCaption();
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		_priceFormingGrid = AddGridControl(container, "PriceForming");
		_priceFormingGrid.Width = SizeCalculator.Width(860.0);
		_priceFormingGrid.Height = SizeCalculator.Height(400.0);
		_priceFormingGrid.AllowAdd = false;
		_priceFormingGrid.AllowDelete = false;
		_priceFormingGrid.AllowDuplicate = false;
		_priceFormingGrid.ShowButtons = false;
		_priceFormingGrid.ItemsSource = _rows;
		_priceFormingTableView = _priceFormingGrid.View;
		_priceFormingTableView.AllowMoveColumnToDropArea = false;
		_priceFormingTableView.IsColumnMenuEnabled = false;
		_priceFormingTableView.AllowColumnMoving = true;
		_priceFormingTableView.AutoWidth = true;
		_priceFormingGrid.KeyDown += new SafeEventHandler<KeyEventArgs>(PriceFormingGrid_KeyDown).Handler;
		_priceFormingTableView.ColumnHeaderClick += new SafeEventHandler<ColumnHeaderClickEventArgs>(PriceFormingTableView_ColumnHeaderClick).Handler;
		_priceFormingTableView.CellValueChanging += new SafeEventHandler<CellValueChangedEventArgs>(PriceFormingTableView_CellValueChanging).Handler;
		_isCheckedCol = _priceFormingGrid.AddBooleanColumn("IsChecked", string.Empty);
		_isCheckedCol.Width = 50.0;
		_isCheckedCol.AllowEditing = DefaultBoolean.True;
		_isCheckedCol.AllowSorting = DefaultBoolean.True;
		_mtCodeCol = _priceFormingGrid.AddStringColumn("MTCode", ArmSoft.SME.Common.Resources.Code, 20);
		_mtCodeCol.Width = 100.0;
		_mtCodeCol.AllowEditing = DefaultBoolean.False;
		_mtCodeCol.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
		_mtCodeCol.AllowFocus = false;
		_mtCodeCol.AllowSorting = DefaultBoolean.True;
		_mtNameCol = _priceFormingGrid.AddStringColumn("MTName", ArmSoft.SME.Common.Resources.Name, 50);
		_mtNameCol.AllowEditing = DefaultBoolean.False;
		_mtNameCol.Width = 280.0;
		_mtNameCol.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
		_mtNameCol.AllowFocus = false;
		_mtNameCol.AllowSorting = DefaultBoolean.True;
		_qntUnitCol = _priceFormingGrid.AddStringColumn("QntUnitName", ArmSoft.SME.Trade.Common.Resources.QU, 3);
		_qntUnitCol.Width = 50.0;
		_qntUnitCol.AllowEditing = DefaultBoolean.False;
		_qntUnitCol.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
		_qntUnitCol.AllowFocus = false;
		_qntUnitCol.AllowSorting = DefaultBoolean.True;
		string caption;
		if (flag)
		{
			switch (text2)
			{
			case "S4":
			case "S5":
			case "S6":
			case "S7":
				text3 = "";
				break;
			default:
				text3 = ", " + text3;
				break;
			}
			caption = string.Format(ArmSoft.SME.Trade.Common.Resources.BasePriceCurrVAT, GetPriceTypeCaption(text2), arg, text3);
		}
		else
		{
			caption = string.Format(ArmSoft.SME.Trade.Common.Resources.BasePriceCurrVAT, GetPriceTypeCaption(_priceType), _currCode, ", " + text);
		}
		_basePriceCol = _priceFormingGrid.AddNumberColumn("BasePrice", caption, 16, 2);
		_basePriceCol.AllowEditing = DefaultBoolean.False;
		_basePriceCol.Width = 155.0;
		_basePriceCol.Visible = flag;
		if (!_basePriceCol.Visible)
		{
			_mtNameCol.Width = _mtNameCol.Width.Value + _basePriceCol.Width.Value;
		}
		_basePriceCol.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
		_basePriceCol.AllowFocus = false;
		_basePriceCol.AllowSorting = DefaultBoolean.True;
		_oldPriceCol = _priceFormingGrid.AddNumberColumn("OldPrice", ArmSoft.SME.Trade.Common.Resources.FormingPriceOldValues, 16, 2);
		_oldPriceCol.AllowEditing = DefaultBoolean.False;
		_oldPriceCol.Width = 155.0;
		_oldPriceCol.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
		_oldPriceCol.AllowFocus = false;
		_oldPriceCol.AllowSorting = DefaultBoolean.True;
		if (_operType != PriceFormationOpTypeSE.PricesInputing.Value)
		{
			_percentCol = _priceFormingGrid.AddNumberColumn("Percent", "%", 6, 2, allowNegativeNumbers: true);
			_percentCol.AllowEditing = DefaultBoolean.True;
			_percentCol.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
			_percentCol.AllowSorting = DefaultBoolean.True;
		}
		_newPriceCol = _priceFormingGrid.AddNumberColumn("NewPrice", string.Format(ArmSoft.SME.Trade.Common.Resources.FormingPriceCurrVAT, GetPriceTypeCaption(_priceType), _currCode, text), 16, 2);
		_newPriceCol.AllowEditing = DefaultBoolean.True;
		_newPriceCol.Width = 155.0;
		_newPriceCol.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
		_newPriceCol.AllowSorting = DefaultBoolean.True;
		DialogWindow.SelectCellInGrid(_priceFormingTableView, _priceFormingGrid, 0, _newPriceCol.Name);
		_registryKey = "PriceFormationColumns";
		Closing += ConfirmClosing;
		return ShowDialog().Value;
	}

	private void PriceFormingGrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)e.Key == 6 && _priceFormingTableView.Grid.CurrentColumn.Name == _newPriceCol.Name)
		{
			_priceFormingTableView.CommitEditing();
			int num = _priceFormingTableView.Grid.FindRow(_priceFormingTableView.Grid.CurrentItem);
			if (num != _priceFormingGrid.Grid.VisibleRowCount - 1)
			{
				int rowHandleByVisibleIndex = _priceFormingGrid.Grid.GetRowHandleByVisibleIndex(num + 1);
				_priceFormingGrid.Grid.RefreshRow(rowHandleByVisibleIndex);
				DialogWindow.SelectCellInGrid(_priceFormingTableView, _priceFormingGrid, rowHandleByVisibleIndex, _newPriceCol.Name);
			}
			e.Handled = true;
		}
	}

	private void PriceFormingTableView_ColumnHeaderClick(object sender, ColumnHeaderClickEventArgs e)
	{
		if (e.Column == _isCheckedCol && _priceFormingGrid.Grid.VisibleRowCount != 0)
		{
			SelectDeselect();
			e.Handled = true;
		}
	}

	private void SelectDeselect()
	{
		if (_isCheckedCol.AllowEditing == DefaultBoolean.False)
		{
			return;
		}
		bool flag = true;
		foreach (SelectedMaterialsRow row in _rows)
		{
			if (!row.IsChecked)
			{
				flag = false;
				break;
			}
		}
		int i = 0;
		for (int num = _priceFormingGrid.Grid.VisibleRowCount - 1; i <= num; i++)
		{
			int rowHandleByVisibleIndex = _priceFormingGrid.Grid.GetRowHandleByVisibleIndex(i);
			SelectedMaterialsRow selectedMaterialsRow = (SelectedMaterialsRow)_priceFormingGrid.Grid.GetRow(rowHandleByVisibleIndex);
			if (selectedMaterialsRow != null)
			{
				if (flag)
				{
					selectedMaterialsRow.IsChecked = false;
				}
				else
				{
					selectedMaterialsRow.IsChecked = true;
				}
			}
		}
		_priceFormingGrid.Grid.RefreshData();
	}

	private void ConfirmClosing(object sender, CancelEventArgs e)
	{
		if (!Convert.ToBoolean(DialogResult) && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.QuestionWhenCloseWindow, ArmSoft.SME.Trade.Common.Resources.PricesFormation, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
		{
			e.Cancel = true;
		}
	}

	private void ConfirmPricesWindow_Validation(DialogWindow window, DialogWindowValidationArgs args)
	{
		try
		{
			ConfirmPricesWindowValidation();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
		}
	}

	private void PriceFormingTableView_CellValueChanging(object sender, CellValueChangedEventArgs e)
	{
		if (_operType == PriceFormationOpTypeSE.AutoForming.Value)
		{
			SelectedMaterialsRow selectedMaterialsRow = (SelectedMaterialsRow)_priceFormingGrid.Grid.GetRow(e.RowHandle);
			if (selectedMaterialsRow == null)
			{
				return;
			}
			if (e.Column == _percentCol)
			{
				selectedMaterialsRow.NewPrice = (selectedMaterialsRow.BasePrice * (1m + (decimal)e.Value / 100m)).RoundAs(2);
			}
			else if (e.Column == _newPriceCol && selectedMaterialsRow.BasePrice != 0m)
			{
				selectedMaterialsRow.Percent = ((decimal)e.Value * 100m / selectedMaterialsRow.BasePrice - 100m).RoundAs(2);
			}
		}
		_priceFormingGrid.View.CommitEditing();
		_priceFormingGrid.Grid.RefreshRow(e.RowHandle);
	}

	private void ConfirmPricesWindowValidation()
	{
		_priceFormingGrid.View.CommitEditing();
		bool flag = false;
		bool flag2 = true;
		PricesDataProvider.DataRow priceRow = new PricesDataProvider.DataRow();
		List<PricesDataProvider.DataRow> rows = new PricesDataProvider(TradeSettings.CreateTradeClient()).GetRows(new PricesDataProvider.Param
		{
			StartDate = _date,
			EndDate = _date,
			PriceType = _priceType
		}, withoutAccessCheck: true);
		foreach (SelectedMaterialsRow row in _rows)
		{
			if (!row.IsChecked || !(row.NewPrice > 0m) || !(row.OldPrice != row.NewPrice))
			{
				continue;
			}
			if (IsPriceAlreadyExist(rows, row.MTID, row.QntUnit, ref priceRow))
			{
				if (!flag2)
				{
					continue;
				}
				if (!flag)
				{
					if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.OverwritePrices, _date.FormatDDMMYY()), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
					{
						flag = true;
						ProcessRow(row);
					}
					else
					{
						flag2 = false;
					}
				}
				else
				{
					ProcessRow(row);
				}
			}
			else
			{
				ProcessRow(row, editMode: false);
			}
		}
		if (_properties.Count > 0)
		{
			PriceFormationRequestModel request = new PriceFormationRequestModel
			{
				Properties = _properties
			};
			DataProcessingRequestHelper.Execute<PriceFormationRequestModel, DirectoryStoreRequestModel<Price>>(DPRType.Other, "PriceFormation", request);
			TradeSettings.TradeCacheService.CurrentPrices.Refresh();
		}
	}

	private void ProcessRow(SelectedMaterialsRow row, bool editMode = true)
	{
		if (editMode)
		{
			if (_isBasePriceTypePurchaseOrSale)
			{
				if (IsLastUnSkippedRow(row))
				{
					_properties.Add(new PriceFormationRequestProperties
					{
						MTID = row.MTID,
						MTCode = row.MTCode,
						PriceType = _priceType,
						QntUnit = row.QntUnit,
						PriceDate = _date,
						Curr = _currCode,
						ItemPrice = row.NewPrice,
						Comment = _comment,
						MTCodeOld = row.MTCode,
						QntUnitOld = row.QntUnit,
						PriceTypeOld = _priceType,
						PriceDateOld = _date
					});
				}
				else
				{
					row.Skipped = true;
				}
			}
			else
			{
				_properties.Add(new PriceFormationRequestProperties
				{
					MTID = row.MTID,
					MTCode = row.MTCode,
					PriceType = _priceType,
					QntUnit = row.QntUnit,
					PriceDate = _date,
					Curr = _currCode,
					ItemPrice = row.NewPrice,
					Comment = _comment,
					MTCodeOld = row.MTCode,
					QntUnitOld = row.QntUnit,
					PriceTypeOld = _priceType,
					PriceDateOld = _date
				});
			}
		}
		else
		{
			_properties.Add(new PriceFormationRequestProperties
			{
				MTID = row.MTID,
				MTCode = row.MTCode,
				PriceType = _priceType,
				QntUnit = row.QntUnit,
				PriceDate = _date,
				Curr = _currCode,
				ItemPrice = row.NewPrice,
				Comment = _comment
			});
		}
	}

	private bool IsLastUnSkippedRow(SelectedMaterialsRow row)
	{
		using (IEnumerator<SelectedMaterialsRow> enumerator = _rows.Where((SelectedMaterialsRow x) => x.MTID == row.MTID && x.QntUnit == row.QntUnit && x.BasePrice != row.BasePrice && !x.Skipped).GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				SelectedMaterialsRow current = enumerator.Current;
				row = current;
				return false;
			}
		}
		return true;
	}

	private static bool IsPriceAlreadyExist(List<PricesDataProvider.DataRow> pricesList, int mtID, string qntUnit, ref PricesDataProvider.DataRow priceRow)
	{
		qntUnit = qntUnit.Trim();
		IEnumerable<PricesDataProvider.DataRow> source = pricesList.Where((PricesDataProvider.DataRow row) => row.MTID == mtID && row.QntUnit.Trim() == qntUnit);
		if (source.Any())
		{
			priceRow = source.ElementAtOrDefault(0);
			return true;
		}
		return false;
	}

	private string InnerCaption()
	{
		string text = "";
		string text2 = Properties["Group"].ToString().Trim();
		int num = Convert.ToInt32(Properties["MTID"]);
		bool flag = _rows.Count > 1;
		if (!string.IsNullOrWhiteSpace(text2) && num == -1)
		{
			string arg = "";
			MTGroupCacheRow item = TradeSettings.TradeCacheService.MTGroup.GetItem(text2);
			if (item != null)
			{
				arg = item.Caption.Trim();
			}
			text = $", {ArmSoft.SME.Common.Resources.Group}` {text2} - {arg}";
		}
		else if (num == -1 && !flag)
		{
			text = $", {ArmSoft.SME.Common.Resources.Group}` {ArmSoft.SME.Common.Resources.All}";
		}
		InlineArray6<object> buffer = default;
		buffer[0] = ArmSoft.SME.Trade.Common.Resources.PricesFormation;
		buffer[1] = _date.FormatDDMMYY();
		buffer[2] = GetPriceTypeCaption(_priceType);
		buffer[3] = ArmSoft.SME.Common.Resources.Curr;
		buffer[4] = _currCode;
		buffer[5] = text;
		return string.Format("{0} - {1} ({2}, {3}` - {4}{5})", (ReadOnlySpan<object?>)buffer);
	}

	private string GetPriceTypeCaption(string priceType)
	{
		switch (priceType)
		{
		case "S1":
			return ArmSoft.SME.Trade.Common.Resources.CostSummaNCC + " " + ArmSoft.SME.Trade.Common.Resources.WithoutNDS;
		case "S2":
			return ArmSoft.SME.Trade.Common.Resources.CostSummaNCC + " " + ArmSoft.SME.Trade.Common.Resources.WithNDS;
		case "S3":
			return string.Format(ArmSoft.SME.Trade.Common.Resources.CostSummaCurr, Settings.ParametersService.MTCurr());
		case "S4":
			return ArmSoft.SME.Trade.Common.Resources.PurchasePrice;
		case "S5":
			return ArmSoft.SME.Trade.Common.Resources.SalePrice;
		case "S6":
			return ArmSoft.SME.Trade.Common.Resources.RetailAvailabilityPrice;
		case "S7":
			return ArmSoft.SME.Trade.Common.Resources.DocDiscountPrice;
		default:
		{
			string text = priceType + " - ";
			if (priceType == _priceType)
			{
				return text + _priceTypeCaption;
			}
			return text + TradeSettings.TradeCacheService.PriceType.GetItem(priceType)?.Caption;
		}
		}
	}

	private void CompactRows()
	{
		BindingList<SelectedMaterialsRow> bindingList = new BindingList<SelectedMaterialsRow>();
		if (_rows != null && _rows.Count > 0)
		{
			foreach (var item2 in from row in _rows
				orderby row.RowNum
				group row by new { row.MTID, row.MTCode, row.MTName, row.BasePrice, row.Percent, row.OldPrice, row.NewPrice, row.QntUnit, row.QntUnitName, row.Comment })
			{
				SelectedMaterialsRow item = new SelectedMaterialsRow
				{
					MTID = item2.Key.MTID,
					MTCode = item2.Key.MTCode,
					MTName = item2.Key.MTName,
					BasePrice = item2.Key.BasePrice,
					OldPrice = item2.Key.OldPrice,
					Percent = item2.Key.Percent,
					NewPrice = item2.Key.NewPrice,
					QntUnit = item2.Key.QntUnit,
					QntUnitName = item2.Key.QntUnitName,
					Comment = item2.Key.Comment
				};
				bindingList.Add(item);
			}
		}
		_rows = bindingList;
	}

	private static void LoadColumnWidthsFromRegistry(GridColumnCollection columns, RegistryKey registryKey)
	{
		if (registryKey == null)
		{
			return;
		}
		foreach (GridColumn column in columns)
		{
			if (double.TryParse(registryKey.GetValue(column.Name.Trim(), "0").ToString(), out var result) && result > 0.0)
			{
				column.Width = result;
			}
		}
		registryKey.Close();
	}

	private static void SaveColumnWidthsInRegistry(GridColumnCollection columns, RegistryKey registryKey)
	{
		if (registryKey == null)
		{
			return;
		}
		foreach (GridColumn column in columns)
		{
			string text = column.ActualWidth.ToString();
			if (text != "NaN")
			{
				registryKey.SetValue(column.Name.Trim(), text);
			}
		}
		registryKey.Close();
	}
}
