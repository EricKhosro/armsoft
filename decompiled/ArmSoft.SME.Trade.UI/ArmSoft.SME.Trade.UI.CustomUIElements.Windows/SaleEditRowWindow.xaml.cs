using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.Documents.Base;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.Windows;

public partial class SaleEditRowWindow : ThemedWindow, IScanable, IComponentConnector, IStyleConnector
{
	private readonly bool _isReturn;

	private readonly bool _touchMode;

	private readonly bool _isRetail;

	private readonly string _priceListType;

	private readonly string _priceListTypeRoundDgr;

	private readonly string _vatType;

	private readonly DateTime _documentDate;

	private readonly string _currCode;

	private readonly MaterialCacheRow _materialCacheRow;

	private readonly bool _allowPOSDecreaseQuantity = Settings.ParametersService.AllowPOSDecreaseQuantity();

	private readonly bool _isMarkingListVisible;

	private readonly bool _useDiscount = Settings.ParametersService.DiscountUse();

	private readonly bool _allowPOSChangePrice = Settings.ParametersService.AllowPOSChangePrice();

	private readonly bool _priceRecalcFromSum = Settings.ParametersService.PriceRecalcFromSumm();

	private readonly string _mtBillInputValueHotKey = Settings.ParametersService.MTBillInputValueHotKey();

	private SaleWindowHelper.CurrentFocusedField _currentFocusedElement;

	private string _realText = "";

	private readonly bool _weightButtonVisible = !string.IsNullOrWhiteSpace(ElScaleInfo.GetInstance().ElScaleType);

	private readonly DispatcherTimer _scaleTimer;

	private readonly Brush _foregroundBlue = ColorsAS.Foreground.BlueBrush;

	private readonly Brush _foregroundRed = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#f07174" : "#dd0326");

	private readonly Brush _foregroundGreen = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#4fad77" : "#217346");

	public BasePOSSaleWindow.SaleRow SaleRow;

	public readonly BindingList<string> MarkingList = new BindingList<string>();

	public readonly List<string> AllMarkingList;

	public decimal Quantity { get; private set; }

	public decimal Price { get; private set; }

	public decimal Summ { get; private set; }

	public decimal DiscountPercent { get; private set; }

	public decimal DiscountPrice { get; private set; }

	public decimal DiscountSumm { get; private set; }

	public string UnitCode { get; private set; }

	public SaleEditRowWindow(Window windowOwner, BasePOSSaleWindow.SaleRow saleRow, bool isReturn, bool touchMode, bool isRetail, string priceListType, string priceListTypeRoundDgr, string vatType, DateTime docuemntDate, string currCode, SaleWindowHelper.CurrentFocusedField selectedField, List<string> allMarkings, bool fromSamplePOS, bool editMode = true)
	{
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Expected Obj, but got Unknown
		InitializeComponent();
		Owner = windowOwner;
		SaleRow = saleRow;
		_isReturn = isReturn;
		_touchMode = touchMode;
		_isRetail = isRetail;
		_priceListType = priceListType;
		_priceListTypeRoundDgr = priceListTypeRoundDgr;
		_vatType = vatType;
		_documentDate = docuemntDate;
		_currCode = currCode;
		_materialCacheRow = TradeSettings.TradeCacheService.Material.GetItemById(SaleRow.ItemID);
		MarkingTextEdit.NullText = ArmSoft.SME.Trade.Common.Resources.InputBoxCaption + (string.IsNullOrWhiteSpace(_mtBillInputValueHotKey) ? "" : (" (" + _mtBillInputValueHotKey + ")"));
		TE_Item.EditValue = SaleRow.ItemCode + " " + SaleRow.ItemName.Trim();
		UnitCode = SaleRow.UnitCode;
		Quantity = SaleRow.Quantity;
		QuantityTextEdit.EditValue = Quantity;
		Price = SaleRow.Price;
		DiscountPrice = SaleRow.DiscountPrice;
		Summ = SaleRow.Summa;
		DiscountPercent = SaleRow.Discount;
		DiscountSumm = SaleRow.DiscountSumma;
		decimal num = decimal.Truncate(Price);
		PriceTextEdit.EditValue = ((num == Price) ? num : Price);
		TE_Unit.EditValue = SaleRow.UnitBrief;
		decimal num2 = decimal.Truncate(SaleRow.Summa);
		SummTextEdit.EditValue = ((num2 == SaleRow.Summa) ? num2 : SaleRow.Summa);
		SaleRow.IsRowMarkable = MarkingHelpers.IsGoodMarkable(SaleRow.ItemCode, Settings.ParametersService.UseMarking(), SaleRow.MarkingList.Count != 0, _documentDate.ToDateOnly(), out var _);
		_isMarkingListVisible = Settings.ParametersService.UseMarking() && SaleRow.IsRowMarkable;
		if (_isMarkingListVisible)
		{
			foreach (string marking in SaleRow.MarkingList)
			{
				MarkingList.Add(marking);
			}
		}
		else
		{
			HideMarkingFields();
		}
		AllMarkingList = allMarkings;
		MarkingGrid.ItemsSource = MarkingList;
		if (fromSamplePOS)
		{
			SalePriceTitleTextBlock.Text = ArmSoft.SME.Common.Resources.Price;
		}
		else
		{
			SalePriceTitleTextBlock.Text = ArmSoft.SME.Common.Resources.MTTAccSalePrice;
		}
		if (_useDiscount)
		{
			SummTextEditLabel.Text = ArmSoft.SME.Trade.Common.Resources.DiscountedSumm;
		}
		else
		{
			SummTextEditLabel.Text = ArmSoft.SME.Trade.Common.Resources.SaleSum;
		}
		_currentFocusedElement = SaleWindowHelper.CurrentFocusedField.None;
		switch (selectedField)
		{
		case SaleWindowHelper.CurrentFocusedField.Quantity:
			QuantityTextEdit.SelectAll();
			QuantityTextEdit.Focus();
			break;
		case SaleWindowHelper.CurrentFocusedField.SalePrice:
			PriceTextEdit.SelectAll();
			PriceTextEdit.Focus();
			break;
		case SaleWindowHelper.CurrentFocusedField.SaleSumm:
			SummTextEdit.SelectAll();
			SummTextEdit.Focus();
			break;
		case SaleWindowHelper.CurrentFocusedField.Marking:
			MarkingTextEdit.SelectAll();
			MarkingTextEdit.Focus();
			break;
		}
		if (!editMode && _materialCacheRow != null && _materialCacheRow.IsWeight && _weightButtonVisible)
		{
			_scaleTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(250L)
			};
			_scaleTimer.Tick += ScaleTimer_TickAsync;
			_scaleTimer.Start();
		}
		SetReadOnly();
		ASFontHelper.SetFontSettings(this);
	}

	private void SetReadOnly()
	{
		bool flag = _scaleTimer != null;
		PriceTextEdit.IsReadOnly = !_allowPOSChangePrice | flag;
		SummTextEdit.IsReadOnly = !_allowPOSChangePrice | flag;
		QuantityTextEdit.IsReadOnly = !_allowPOSDecreaseQuantity | flag;
		if (Settings.ParametersService.CalcDiscountAuto() || Settings.ParametersService.AlwaysCalcDiscountAuto())
		{
			SummTextEdit.Visibility = Visibility.Collapsed;
			SummTextEditLabel.Visibility = Visibility.Collapsed;
		}
		else
		{
			SummTextEdit.Visibility = Visibility.Visible;
			SummTextEditLabel.Visibility = Visibility.Visible;
			SummTextEdit.IsReadOnly = (!_allowPOSChangePrice || !Settings.ParametersService.AllowChangeDiscountPrice()) | flag;
		}
		QuantityDecreaseButton.IsEnabled = !flag;
		QuantityIncreaseButton.IsEnabled = !flag;
		UnitButton.IsEnabled = !flag;
	}

	private void FillCalcInfoTextBlock()
	{
		decimal num = Math.Abs(DiscountSumm);
		string text = ((!(DiscountSumm >= 0m)) ? "+" : "-");
		TextBlock calcInfoTextBlock = CalcInfoTextBlock;
		InlineArray6<object> buffer = default;
		buffer[0] = Quantity.CurrencyFormat(16, Convert.ToInt16((!(Quantity == decimal.Floor(Quantity))) ? 3 : 0));
		buffer[1] = TradeSettings.TradeCacheService.QntUnit.GetItem(UnitCode).Brief;
		buffer[2] = Price.CurrencyFormat(16, Convert.ToInt16((!(Price == decimal.Floor(Price))) ? 2 : 0));
		buffer[3] = text;
		buffer[4] = num.CurrencyFormat(16, Convert.ToInt16((!(num == decimal.Floor(num))) ? 2 : 0)) + "(" + DiscountPercent.CurrencyFormat(16, Convert.ToInt16((!(DiscountPercent == decimal.Floor(DiscountPercent))) ? 2 : 0)) + "%)";
		buffer[5] = Summ.CurrencyFormat(16, Convert.ToInt16((!(Summ == decimal.Floor(Summ))) ? 2 : 0));
		calcInfoTextBlock.Text = string.Format("{0} {1} x {2} {3} {4} = {5}", (ReadOnlySpan<object?>)buffer);
	}

	private void UnitButton_Click(object sender, RoutedEventArgs e)
	{
		MTQntUnits mTQntUnits = new MTQntUnits(SaleRow.ItemCode, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", "0", showImageExists: false);
		if (_touchMode)
		{
			TouchGridControlWithExtraButtons<MTQntUnitDataProvider.DataRow> touchGridControlWithExtraButtons = new TouchGridControlWithExtraButtons<MTQntUnitDataProvider.DataRow>(this, mTQntUnits, "UnitSelectTouchDialog");
			if (touchGridControlWithExtraButtons.ShowDialog().Value && touchGridControlWithExtraButtons.SelectedRow != null)
			{
				TE_Unit.EditValue = touchGridControlWithExtraButtons.SelectedRow.QntUnitBrief;
				UnitChanged(touchGridControlWithExtraButtons.SelectedRow.QntUnit, UnitCode);
			}
			return;
		}
		ModalBrowser modalBrowser = new ModalBrowser(mTQntUnits);
		modalBrowser.Owner = Owner;
		modalBrowser.LookUpValue = UnitCode;
		modalBrowser.WindowStartupLocation = WindowStartupLocation.CenterOwner;
		if (modalBrowser.Show())
		{
			string qntUnit = mTQntUnits.ValueMember(modalBrowser.SelectedRowIndex);
			MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(SaleRow.ItemCode, qntUnit);
			TE_Unit.EditValue = TradeSettings.TradeCacheService.QntUnit.GetItem(qntUnit2.QntUnit).Brief;
			UnitChanged(qntUnit2.QntUnit, UnitCode);
		}
	}

	private void QuantityDecreaseButton_Click(object sender, RoutedEventArgs e)
	{
		if (_allowPOSDecreaseQuantity)
		{
			IncreaseDecreaseQuantity(increase: false);
		}
		else if (new UserAuthenticationDialogWindow(this, Definitions.AllowPOSDecreaseQuantity.Id).ShowDialog().Value)
		{
			IncreaseDecreaseQuantity(increase: false);
		}
	}

	private void QuantityIncreaseButton_Click(object sender, RoutedEventArgs e)
	{
		IncreaseDecreaseQuantity();
	}

	private void OKButton_Click(object sender, RoutedEventArgs e)
	{
		OnConfirmButtonPress();
	}

	private void ExitButton_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void TabButton_Click(object sender, RoutedEventArgs e)
	{
		if (_currentFocusedElement == SaleWindowHelper.CurrentFocusedField.Quantity)
		{
			PriceTextEdit.Focus();
		}
		else if (_currentFocusedElement == SaleWindowHelper.CurrentFocusedField.SalePrice)
		{
			SummTextEdit.Focus();
		}
		else if (_currentFocusedElement == SaleWindowHelper.CurrentFocusedField.SaleSumm)
		{
			if (_isMarkingListVisible)
			{
				MarkingTextEdit.Focus();
			}
			else
			{
				QuantityTextEdit.Focus();
			}
		}
		else if (_currentFocusedElement == SaleWindowHelper.CurrentFocusedField.Marking)
		{
			QuantityTextEdit.Focus();
		}
		else
		{
			QuantityTextEdit.Focus();
		}
	}

	private void DeleteRowButton_Click(object sender, RoutedEventArgs e)
	{
		ValidateQuantity();
		if (_allowPOSDecreaseQuantity || new UserAuthenticationDialogWindow(this, Definitions.AllowPOSDecreaseQuantity.Id).ShowDialog().Value)
		{
			int focusedRowHandle = MarkingTableView.FocusedRowHandle;
			string item = (string)MarkingGrid.GetRow(focusedRowHandle);
			if (Quantity < (decimal)MarkingList.Count && (_materialCacheRow == null || !_materialCacheRow.IsWeight))
			{
				IncreaseDecreaseQuantity(increase: false);
			}
			MarkingList.Remove(item);
			AllMarkingList.Remove(item);
			MarkingGrid.RefreshData();
		}
	}

	private void MarkingButton_Click(object sender, RoutedEventArgs e)
	{
		AddMarkingRow();
	}

	private void NumericTouchPadControlButton_Click(object sender, SaleWindowHelper.ButtonClickEventArgs e)
	{
		if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.C || e.ButtonCommand == SaleWindowHelper.ButtonCommands.BackSpace)
		{
			ReleaseCButton();
		}
		else if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.Numeric)
		{
			ForwardSymbolToFocusedField(((SaleWindowHelper.ButtonNumericClickEventArgs)e).ButtonText);
		}
	}

	private void ThemedWindow_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (HotKeys.Functional.F2.Matches(this, e))
			{
				HotKeyRedirector("F2");
				e.Handled = true;
			}
			else if (HotKeys.Single.Escape.Matches(this, e))
			{
				DialogResult = false;
				Close();
			}
			else if (HotKeys.WithModifier.Control_Enter.Matches(this, e))
			{
				e.Handled = true;
				OnConfirmButtonPress();
			}
			else if (HotKeys.Single.Enter.Matches(this, e))
			{
				e.Handled = true;
				if (!MarkingTextEdit.IsReadOnly && !string.IsNullOrWhiteSpace(MarkingTextEdit.Text) && MarkingTextEdit.IsKeyboardFocusWithin)
				{
					MarkingTextEdit.Text = MarkingHelper.CleanScannerSymbols(_realText.Contains('\u001d') ? _realText : MarkingTextEdit.Text);
					AddMarkingRow();
				}
				else
				{
					OnConfirmButtonPress();
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void QuantityTextEdit_GotFocus(object sender, RoutedEventArgs e)
	{
		try
		{
			_currentFocusedElement = SaleWindowHelper.CurrentFocusedField.Quantity;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void PriceTextEdit_GotFocus(object sender, RoutedEventArgs e)
	{
		try
		{
			_currentFocusedElement = SaleWindowHelper.CurrentFocusedField.SalePrice;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void SummTextEdit_GotFocus(object sender, RoutedEventArgs e)
	{
		try
		{
			_currentFocusedElement = SaleWindowHelper.CurrentFocusedField.SaleSumm;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MarkingTextEdit_GotFocus(object sender, RoutedEventArgs e)
	{
		try
		{
			_currentFocusedElement = SaleWindowHelper.CurrentFocusedField.Marking;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MarkingTextEdit_PreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		try
		{
			_realText += e.Text;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			_realText = "";
		}
	}

	private void QuantityTextEdit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		try
		{
			QuantityValueChanged();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void PriceTextEdit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		try
		{
			PriceValueChanged();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void SummTextEdit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		try
		{
			SummValueChanged();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private async void ScaleTimer_TickAsync(object sender, EventArgs e)
	{
		await GetScaleWeightAsync();
	}

	private async Task GetScaleWeightAsync()
	{
		try
		{
			_scaleTimer.Stop();
			decimal num = await Task.Run(() => ScaleConnector.GetWeight(showException: false));
			decimal.TryParse(QuantityTextEdit.Text.Trim(), out var result);
			if (num == 0m)
			{
				ScaleProcessText.Text = ArmSoft.SME.Trade.Common.Resources.PlaceProductOnScale;
				ScaleProcessText.Foreground = _foregroundBlue;
				((DependencyObject)QuantityTextEdit).ClearValue(TextBlock.ForegroundProperty);
			}
			else if (num - result > 0.001m)
			{
				ScaleProcessText.Text = ArmSoft.SME.Trade.Common.Resources.WeightReading;
				QuantityTextEdit.Foreground = _foregroundRed;
				ScaleProcessText.Foreground = QuantityTextEdit.Foreground;
			}
			else
			{
				ScaleProcessText.Text = ArmSoft.SME.Trade.Common.Resources.WeightAssigned;
				QuantityTextEdit.Foreground = _foregroundGreen;
				ScaleProcessText.Foreground = QuantityTextEdit.Foreground;
			}
			QuantityTextEdit.EditValue = num;
		}
		catch
		{
		}
		finally
		{
			if (IsVisible)
			{
				_scaleTimer.Start();
			}
		}
	}

	private void QuantityValueChanged()
	{
		if (decimal.TryParse(QuantityTextEdit.Text.Trim(), out var result))
		{
			if (result != Quantity)
			{
				ValidateQuantity();
				Summ = (Quantity * DiscountPrice).RoundAs(2);
				CalculateSumAndDiscountSum(calcDiscountPrice: false);
			}
			FillCalcInfoTextBlock();
		}
	}

	private void ReleaseCButton()
	{
		if (_currentFocusedElement == SaleWindowHelper.CurrentFocusedField.Quantity)
		{
			if (!QuantityTextEdit.IsReadOnly)
			{
				QuantityTextEdit.EditValue = 0m;
				QuantityTextEdit.Focus();
			}
		}
		else if (_currentFocusedElement == SaleWindowHelper.CurrentFocusedField.SalePrice)
		{
			if (!PriceTextEdit.IsReadOnly)
			{
				PriceTextEdit.EditValue = 0m;
				PriceTextEdit.Focus();
			}
		}
		else if (_currentFocusedElement == SaleWindowHelper.CurrentFocusedField.SaleSumm)
		{
			if (!SummTextEdit.IsReadOnly)
			{
				SummTextEdit.EditValue = 0m;
				SummTextEdit.Focus();
			}
		}
		else if (_currentFocusedElement == SaleWindowHelper.CurrentFocusedField.Marking && !MarkingTextEdit.IsReadOnly)
		{
			MarkingTextEdit.EditValue = "";
			MarkingTextEdit.Focus();
		}
	}

	private void HideMarkingFields()
	{
		MarkingTextEdit.IsReadOnly = true;
		TB_Marking.Visibility = Visibility.Collapsed;
		MarkingContainerGrid.Visibility = Visibility.Collapsed;
		MarkingGrid.Visibility = Visibility.Collapsed;
	}

	private void UnitChanged(string newUnitCode, string oldUnitCode)
	{
		QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(newUnitCode);
		if (item != null)
		{
			MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(SaleRow.ItemID);
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(itemById, item.Code);
			MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(itemById, item.Code);
			if (!_isRetail || itemById.Type == ItemTypesSE.Services.Value)
			{
				if (!string.IsNullOrWhiteSpace(_priceListType))
				{
					decimal num = ((!Settings.IsOffline) ? PriceHelper.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, SaleRow.ItemID, item.Code, _documentDate.ToDateOnly(), _currCode) : PriceHelper.GetPriceForGivenPriceListAndVATTypeFromCache(_priceListType, _vatType, itemById.MTCode, item.Code));
					if (num == 0m)
					{
						num = PriceHelper.GetPriceForUnit(oldUnitCode, newUnitCode, Price, SaleRow.ItemCode);
					}
					Price = Methods.RoundPrice(num, _priceListTypeRoundDgr);
				}
				else
				{
					MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(SaleRow.ItemCode, oldUnitCode.ToString());
					if (barCode != null && qntUnit2 != null)
					{
						Price = (Price / qntUnit2.Coef * qntUnit.Coef).RoundAs(2);
						Price = Methods.RoundPrice(Price, _priceListTypeRoundDgr);
					}
				}
			}
			else
			{
				Price = PriceHelper.GetPriceForUnit(oldUnitCode, newUnitCode, Price, SaleRow.ItemCode);
			}
		}
		UnitCode = newUnitCode;
		PriceTextEdit.EditValue = Price;
		decimal num2 = Quantity * Price;
		SummTextEdit.EditValue = (num2 - (num2 * DiscountPercent / 100m).RoundAs(2)).RoundAs(2);
		DiscountSumm = (num2 - Convert.ToDecimal(SummTextEdit.EditValue)).RoundAs(2);
	}

	private void IncreaseDecreaseQuantity(bool increase = true)
	{
		try
		{
			if (!decimal.TryParse(QuantityTextEdit.Text.Trim(), out var result))
			{
				Quantity = 1m;
			}
			Quantity = result;
			if (increase)
			{
				QuantityTextEdit.EditValue = Quantity + 1m;
			}
			else if (!(Quantity == 1m))
			{
				QuantityTextEdit.EditValue = Quantity - 1m;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void HotKeyRedirector(string hotKey)
	{
		if (hotKey == _mtBillInputValueHotKey && !MarkingTextEdit.IsReadOnly)
		{
			MarkingTextEdit.Focus();
		}
	}

	private void OnConfirmButtonPress()
	{
		try
		{
			QuantityValueChanged();
			PriceValueChanged();
			SummValueChanged();
			ValidateQuantity(editComplete: true);
			ValidatePrice(editComplete: true);
			ValidateSumm(editComplete: true);
			SaleRow.DiscountSumma = DiscountSumm;
			SaleRow.Summa = Summ;
			SaleRow.Discount = DiscountPercent;
			SaleRow.Quantity = Quantity;
			SaleRow.Price = Price;
			SaleRow.BeginSumma = (Quantity * Price).RoundAs(2);
			SaleRow.DiscountPrice = DiscountPrice;
			DialogResult = true;
			Close();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ForwardSymbolToFocusedField(string value)
	{
		try
		{
			if (_currentFocusedElement == SaleWindowHelper.CurrentFocusedField.Quantity)
			{
				if (!QuantityTextEdit.IsReadOnly)
				{
					UIHelper.ForwardSymbolFromPadToTextEditPanel(QuantityTextEdit, value, 3);
				}
			}
			else if (_currentFocusedElement == SaleWindowHelper.CurrentFocusedField.SalePrice)
			{
				if (!PriceTextEdit.IsReadOnly)
				{
					UIHelper.ForwardSymbolFromPadToTextEditPanel(PriceTextEdit, value, 2);
				}
			}
			else if (_currentFocusedElement == SaleWindowHelper.CurrentFocusedField.SaleSumm && !SummTextEdit.IsReadOnly)
			{
				UIHelper.ForwardSymbolFromPadToTextEditPanel(SummTextEdit, value, 2);
			}
		}
		catch
		{
		}
	}

	public void OnScanner(string scannerData)
	{
		MarkingTextEdit.Text = MarkingHelper.CleanScannerSymbols(scannerData);
		AddMarkingRow();
	}

	private void AddMarkingRow()
	{
		try
		{
			ValidateQuantity();
			if (!SaleRow.IsRowMarkable)
			{
				return;
			}
			MarkingTextEdit.DoValidate();
			if (string.IsNullOrWhiteSpace(MarkingTextEdit.Text))
			{
				return;
			}
			byte[] bytes = Encoding.Unicode.GetBytes(MarkingTextEdit.Text);
			for (int i = 0; i <= bytes.Length - 1; i += 2)
			{
				if (((bytes[i + 1] << 8) | bytes[i]) > 127)
				{
					throw new ASException(ArmSoft.SME.Common.Resources.KeyboardLanguageForMarkingScanShouldBeEnglish);
				}
			}
			MarkingHelper.ValidateMarkingCode(MarkingTextEdit.Text);
			if (AllMarkingList.Any((string c) => c == MarkingTextEdit.Text))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ValueIsInList);
			}
			if (MarkingList.Any((string c) => c == MarkingTextEdit.Text))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ValueIsInList);
			}
			GetBarCodeRow(MarkingTextEdit.Text);
			if (_isReturn && (decimal)MarkingList.Count == SaleRow.Quantity)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrQTYCannotBeMore, SaleRow.ItemCode, SaleRow.ItemName.Trim()));
			}
			MarkingList.Add(MarkingTextEdit.Text);
			if (Quantity < (decimal)MarkingList.Count && (_materialCacheRow == null || !_materialCacheRow.IsWeight))
			{
				IncreaseDecreaseQuantity();
			}
			MarkingGrid.RefreshRow(MarkingList.Count - 1);
			MarkingTextEdit.Text = "";
			_realText = "";
			MarkingTextEdit.Focus();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			_realText = "";
		}
	}

	private string GetBarCodeRow(string mark)
	{
		string result = MarkingHelpers.ExtractGTINFromDataMatrix(mark, out var good);
		if (good?.MTCode != SaleRow.ItemCode && Settings.ParametersService.CheckMarkingAndBarCode())
		{
			MarkingTextEdit.Text = "";
			_realText = "";
			MarkingTextEdit.Focus();
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.MarkIsWrong, mark, SaleRow.ItemCode));
		}
		return result;
	}

	private void ValidateQuantity(bool editComplete = false)
	{
		if (!decimal.TryParse(QuantityTextEdit.Text.Trim(), out var result))
		{
			Quantity = result;
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
		}
		Quantity = result;
		if (editComplete && Quantity <= 0m)
		{
			QuantityTextEdit.Focus();
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldValueGraterZero, ArmSoft.SME.Common.Resources.Quantity));
		}
		Quantity = Quantity.RoundAs(3);
		QuantityTextEdit.EditValue = Quantity;
	}

	private void ValidatePrice(bool editComplete = false)
	{
		if (!decimal.TryParse(PriceTextEdit.Text.Trim(), out var result))
		{
			Price = result;
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
		}
		Price = result;
		if (editComplete && Price <= 0m)
		{
			PriceTextEdit.Focus();
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ValueIsNegativeWithName, ArmSoft.SME.Common.Resources.MTTAccSalePrice));
		}
		Price = Methods.RoundPrice(Price, _priceListTypeRoundDgr);
		PriceTextEdit.EditValue = Price;
	}

	private void ValidateSumm(bool editComplete = false)
	{
		if (!decimal.TryParse(SummTextEdit.Text.Trim(), out var result))
		{
			Summ = result;
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
		}
		Summ = result;
		if (editComplete && Summ < 0m)
		{
			SummTextEdit.Focus();
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ValueIsNegativeWithName, ArmSoft.SME.Trade.Common.Resources.SaleSum));
		}
		Summ = Summ.RoundAs(2);
		SummTextEdit.EditValue = Summ;
	}

	private void PriceValueChanged()
	{
		if (decimal.TryParse(PriceTextEdit.Text.Trim(), out var result))
		{
			if (result != Price)
			{
				ValidatePrice();
				CalculateSumAndDiscountSum();
			}
			FillCalcInfoTextBlock();
		}
	}

	private void SummValueChanged()
	{
		if (!decimal.TryParse(SummTextEdit.Text.Trim(), out var result))
		{
			return;
		}
		if (result != Summ)
		{
			ValidateQuantity();
			ValidatePrice();
			ValidateSumm();
			if (_allowPOSChangePrice && _priceRecalcFromSum)
			{
				if (Price != 0m)
				{
					if (_materialCacheRow != null && _materialCacheRow.IsWeight)
					{
						QuantityTextEdit.EditValue = (Summ / Price).RoundAs(3);
						DiscountPercent = SaleRow.Discount;
					}
					else if (Quantity != 0m)
					{
						DiscountPrice = (Summ / Quantity).RoundAs(2);
						DiscountPercent = (100m * (Price - DiscountPrice) / Price).RoundAs(2);
					}
				}
			}
			else
			{
				if (Quantity != 0m)
				{
					DiscountPrice = Methods.RoundPrice((Summ / Quantity).RoundAs(2), _priceListTypeRoundDgr);
					if (!_useDiscount)
					{
						Price = DiscountPrice;
					}
				}
				if (Price != 0m && _useDiscount)
				{
					DiscountPercent = (100m * (Price - DiscountPrice) / Price).RoundAs(2);
					DiscountSumm = ((Quantity * Price).RoundAs(2) - Summ).RoundAs(2);
				}
				if (!_useDiscount && Quantity != 0m)
				{
					Price = (Summ / Quantity).RoundAs(2);
					Price = Methods.RoundPrice(Price, _priceListTypeRoundDgr);
				}
			}
			ValidateQuantity();
			ValidatePrice();
			ValidateSumm();
			DiscountSumm = ((DiscountPercent == 0m) ? 0m : ((Quantity * Price).RoundAs(2) - Summ).RoundAs(2));
		}
		FillCalcInfoTextBlock();
	}

	private void CalculateSumAndDiscountSum(bool calcDiscountPrice = true)
	{
		if (_useDiscount)
		{
			if (calcDiscountPrice)
			{
				if (!string.IsNullOrWhiteSpace(_priceListTypeRoundDgr))
				{
					DiscountPrice = Methods.RoundPrice(Price * (100m - DiscountPercent) / 100m, _priceListTypeRoundDgr);
				}
				else
				{
					DiscountPrice = (Price * (100m - DiscountPercent) / 100m).RoundAs(2);
				}
			}
			Summ = (Quantity * DiscountPrice).RoundAs(2);
			DiscountSumm = ((Quantity * Price).RoundAs(2) - Summ).RoundAs(2);
		}
		else
		{
			DiscountPrice = Price;
			Summ = (Quantity * Price).RoundAs(2);
			DiscountSumm = 0m;
		}
		SummTextEdit.EditValue = Summ;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
