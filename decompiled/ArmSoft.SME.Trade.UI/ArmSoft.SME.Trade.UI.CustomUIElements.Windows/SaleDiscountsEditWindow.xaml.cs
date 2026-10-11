using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.Documents.Base;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.Windows;

public partial class SaleDiscountsEditWindow : ThemedWindow, IComponentConnector
{
	private readonly bool _isReturn;

	private readonly bool _touchMode;

	private readonly DateTime _documentDate;

	private readonly string _currCode;

	private readonly MaterialCacheRow _materialCacheRow;

	private readonly string _priceListTypeRoundDgr;

	private SaleWindowHelper.CurrentFocusedDiscountControlField _currentFocusedDiscountElement;

	public BasePOSSaleWindow.SaleRow SaleRow;

	public decimal DiscountPercent { get; private set; }

	public decimal DiscountedSumm { get; private set; }

	public decimal DiscountSumm { get; private set; }

	public decimal DiscountPrice { get; private set; }

	public SaleDiscountsEditWindow(Window windowOwner, BasePOSSaleWindow.SaleRow saleRow, bool isReturn, bool touchMode, string priceListTypeRoundDgr, DateTime documentDate, string currCode)
	{
		InitializeComponent();
		Owner = windowOwner;
		SaleRow = saleRow;
		_isReturn = isReturn;
		_touchMode = touchMode;
		_documentDate = documentDate;
		_currCode = currCode;
		_priceListTypeRoundDgr = priceListTypeRoundDgr;
		_materialCacheRow = TradeSettings.TradeCacheService.Material.GetItemById(SaleRow.ItemID);
		MaterialTextEdit.EditValue = SaleRow.ItemCode + " " + SaleRow.ItemName.Trim();
		DiscountPercent = SaleRow.Discount;
		DiscountSumm = SaleRow.DiscountSumma;
		DiscountedSumm = SaleRow.Summa;
		DiscountPrice = SaleRow.DiscountPrice;
		DiscountPercentTextEdit.EditValue = SaleRow.Discount;
		DiscountPercentSummTextEdit.EditValue = DiscountSumm;
		DiscountedPercentSummTextEdit.EditValue = SaleRow.Summa;
		_currentFocusedDiscountElement = SaleWindowHelper.CurrentFocusedDiscountControlField.DiscountPercent;
		DiscountPercentTextEdit.SelectAll();
		DiscountPercentTextEdit.Focus();
		FillCalcInfoTextBlock();
		SetReadOnly();
		ASFontHelper.SetFontSettings(this);
	}

	private void SetReadOnly()
	{
		DiscountPercentTextEdit.IsReadOnly = !Settings.ParametersService.AllowPOSChangePrice();
		DiscountedPercentSummTextEdit.IsReadOnly = !Settings.ParametersService.AllowPOSChangePrice();
		DiscountPercentSummTextEdit.IsReadOnly = !Settings.ParametersService.AllowPOSChangePrice();
	}

	private void FillCalcInfoTextBlock()
	{
		decimal value = Math.Abs(DiscountSumm);
		string text = ((!(DiscountSumm >= 0m)) ? "+" : "-");
		TextBlock calcInfoTextBlock = CalcInfoTextBlock;
		InlineArray6<object> buffer = default;
		buffer[0] = SaleRow.Quantity.CurrencyFormat(16, Convert.ToInt16((!(SaleRow.Quantity == decimal.Floor(SaleRow.Quantity))) ? 3 : 0));
		buffer[1] = _materialCacheRow.UnitRow.Brief;
		buffer[2] = SaleRow.Price.CurrencyFormat(16, Convert.ToInt16((!(SaleRow.Price == decimal.Floor(SaleRow.Price))) ? 2 : 0));
		buffer[3] = text;
		buffer[4] = value.CurrencyFormat(16, Convert.ToInt16((!(DiscountSumm == decimal.Floor(DiscountSumm))) ? 2 : 0)) + "(" + DiscountPercent.CurrencyFormat(16, Convert.ToInt16((!(DiscountPercent == decimal.Floor(DiscountPercent))) ? 2 : 0)) + "%)";
		buffer[5] = DiscountedSumm.CurrencyFormat(16, Convert.ToInt16((!(DiscountedSumm == decimal.Floor(DiscountedSumm))) ? 2 : 0));
		calcInfoTextBlock.Text = string.Format("{0} {1} x {2} {3} {4} = {5}", (ReadOnlySpan<object?>)buffer);
	}

	private void OkButton_Click(object sender, RoutedEventArgs e)
	{
		OnConfirmButtonPress();
	}

	private void ExitButton_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void TabButton_Click(object sender, RoutedEventArgs e)
	{
		if (_currentFocusedDiscountElement == SaleWindowHelper.CurrentFocusedDiscountControlField.DiscountPercent)
		{
			DiscountPercentSummTextEdit.Focus();
		}
		else if (_currentFocusedDiscountElement == SaleWindowHelper.CurrentFocusedDiscountControlField.DiscountSumm)
		{
			DiscountedPercentSummTextEdit.Focus();
		}
		else
		{
			DiscountPercentTextEdit.Focus();
		}
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
			if (HotKeys.Single.Escape.Matches(this, e))
			{
				DialogResult = false;
				Close();
			}
			else if (HotKeys.WithModifier.Control_Enter.Matches(this, e))
			{
				e.Handled = true;
				OnConfirmButtonPress();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void DiscountPercentTextEdit_LostFocus(object sender, RoutedEventArgs e)
	{
		try
		{
			_currentFocusedDiscountElement = SaleWindowHelper.CurrentFocusedDiscountControlField.DiscountPercent;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void DiscountPercentSummTextEdit_LostFocus(object sender, RoutedEventArgs e)
	{
		try
		{
			_currentFocusedDiscountElement = SaleWindowHelper.CurrentFocusedDiscountControlField.DiscountSumm;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void DiscountedPercentSummTextEdit_LostFocus(object sender, RoutedEventArgs e)
	{
		try
		{
			_currentFocusedDiscountElement = SaleWindowHelper.CurrentFocusedDiscountControlField.DiscountedSumm;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void DiscountPercentTextEdit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		try
		{
			DiscountPercentValueChanged();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void DiscountPercentSummTextEdit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		try
		{
			DiscountSummValueChanged();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void DiscountedPercentSummTextEdit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		try
		{
			DiscountedSummValueChanged();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ValidateDiscountPercent()
	{
		bool flag = decimal.TryParse(DiscountPercentTextEdit.Text.Trim(), out var result);
		DiscountPercent = result;
		if (!flag)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
		}
		DiscountPercent = DiscountPercent.RoundAs(2);
		DiscountPercentTextEdit.EditValue = DiscountPercent;
	}

	private void ValidateDiscountSumm()
	{
		bool flag = decimal.TryParse(DiscountPercentSummTextEdit.Text.Trim(), out var result);
		DiscountSumm = result;
		if (!flag)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
		}
		DiscountSumm = DiscountSumm.RoundAs(2);
		DiscountPercentSummTextEdit.EditValue = DiscountSumm;
	}

	private void ValidateDiscountedSumm(bool isEditCompleted = false)
	{
		bool flag = decimal.TryParse(DiscountedPercentSummTextEdit.Text.Trim(), out var result);
		DiscountedSumm = result;
		if (!flag)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
		}
		if (isEditCompleted && DiscountedSumm < 0m)
		{
			DiscountedPercentSummTextEdit.Focus();
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ValueIsNegativeWithName, ArmSoft.SME.Trade.Common.Resources.DiscountedSumm));
		}
		DiscountedSumm = DiscountedSumm.RoundAs(2);
		DiscountedPercentSummTextEdit.EditValue = DiscountedSumm;
	}

	private void DiscountPercentValueChanged()
	{
		if (decimal.TryParse(DiscountPercentTextEdit.Text.Trim(), out var _))
		{
			if (Convert.ToDecimal(DiscountPercentTextEdit.EditValue) != DiscountPercent)
			{
				ValidateDiscountPercent();
				DiscountPrice = Methods.RoundPrice(SaleRow.Price * (100m - DiscountPercent) / 100m, _priceListTypeRoundDgr);
				DiscountedSumm = (SaleRow.Quantity * DiscountPrice).RoundAs(2);
				DiscountSumm = (SaleRow.Quantity * SaleRow.Price).RoundAs(2) - DiscountedSumm;
				DiscountPercentSummTextEdit.EditValue = DiscountSumm;
				DiscountedPercentSummTextEdit.EditValue = DiscountedSumm;
				ValidateDiscountSumm();
				ValidateDiscountedSumm();
			}
			FillCalcInfoTextBlock();
		}
	}

	private void DiscountSummValueChanged()
	{
		if (!decimal.TryParse(DiscountPercentSummTextEdit.Text.Trim(), out var _))
		{
			return;
		}
		if (Convert.ToDecimal(DiscountPercentSummTextEdit.EditValue) != DiscountSumm)
		{
			ValidateDiscountSumm();
			DiscountedPercentSummTextEdit.EditValue = SaleRow.BeginSumma - Convert.ToDecimal(DiscountPercentSummTextEdit.EditValue);
			ValidateDiscountedSumm();
			if (SaleRow.Price != 0m)
			{
				DiscountPercentTextEdit.EditValue = (100m * (SaleRow.Price - DiscountPrice) / SaleRow.Price).RoundAs(2);
			}
		}
		FillCalcInfoTextBlock();
	}

	private void DiscountedSummValueChanged()
	{
		try
		{
			if (!decimal.TryParse(DiscountedPercentSummTextEdit.Text.Trim(), out var _))
			{
				return;
			}
			if (Convert.ToDecimal(DiscountedPercentSummTextEdit.EditValue) != DiscountedSumm)
			{
				ValidateDiscountedSumm();
				DiscountSumm = SaleRow.BeginSumma - Convert.ToDecimal(DiscountedPercentSummTextEdit.EditValue);
				if (SaleRow.Price != 0m)
				{
					DiscountPrice = Methods.RoundPrice((DiscountedSumm / SaleRow.Quantity).RoundAs(2), _priceListTypeRoundDgr);
					DiscountPercentTextEdit.EditValue = (100m * (SaleRow.Price - DiscountPrice) / SaleRow.Price).RoundAs(2);
				}
				ValidateDiscountSumm();
			}
			FillCalcInfoTextBlock();
		}
		catch
		{
		}
	}

	private void ReleaseCButton()
	{
		if (_currentFocusedDiscountElement == SaleWindowHelper.CurrentFocusedDiscountControlField.DiscountPercent)
		{
			if (!DiscountPercentTextEdit.IsReadOnly)
			{
				DiscountPercentTextEdit.EditValue = 0m;
				DiscountPercentTextEdit.Focus();
			}
		}
		else if (_currentFocusedDiscountElement == SaleWindowHelper.CurrentFocusedDiscountControlField.DiscountSumm)
		{
			if (!DiscountPercentSummTextEdit.IsReadOnly)
			{
				DiscountPercentSummTextEdit.EditValue = 0m;
				DiscountPercentSummTextEdit.Focus();
			}
		}
		else if (_currentFocusedDiscountElement == SaleWindowHelper.CurrentFocusedDiscountControlField.DiscountedSumm && !DiscountedPercentSummTextEdit.IsReadOnly)
		{
			DiscountedPercentSummTextEdit.EditValue = 0m;
			DiscountedPercentSummTextEdit.Focus();
		}
	}

	private void ForwardSymbolToFocusedField(string value)
	{
		try
		{
			if (_currentFocusedDiscountElement == SaleWindowHelper.CurrentFocusedDiscountControlField.DiscountPercent)
			{
				if (DiscountPercentTextEdit.IsReadOnly)
				{
					return;
				}
				int num = DiscountPercentTextEdit.CaretIndex;
				int selectionLength = DiscountPercentTextEdit.SelectionLength;
				string text = DiscountPercentTextEdit.Text;
				string displayText = DiscountPercentTextEdit.DisplayText;
				int length = DiscountPercentTextEdit.DisplayText.Length;
				if (selectionLength == 0)
				{
					if (length >= DiscountPercentTextEdit.Mask.Length && (num < displayText.IndexOf(".") || (num == displayText.IndexOf(".") && value != ".")))
					{
						return;
					}
					if (num == length)
					{
						DiscountPercentTextEdit.Text = UIHelper.ConcatFieldNumericTextWithSymbolsLeftToRight(text, value, 2);
					}
					else if (num == 0)
					{
						DiscountPercentTextEdit.Text = UIHelper.ConcatFieldNumericTextWithSymbolsLeftToRight(value, text, 2);
					}
					else
					{
						if (num > length)
						{
							num = length;
						}
						string fieldText = displayText.Substring(0, num);
						string fieldText2 = displayText.Substring(num);
						fieldText = UIHelper.RemoveRedundantCommaSymbolsOfText(fieldText);
						fieldText2 = UIHelper.RemoveRedundantCommaSymbolsOfText(fieldText2);
						DiscountPercentTextEdit.Text = UIHelper.ConcatFieldNumericTextWithSymbolsLeftToRight(UIHelper.ConcatFieldNumericTextWithSymbolsLeftToRight(fieldText, value, 2), fieldText2, 2);
					}
				}
				else
				{
					if (selectionLength < value.Length)
					{
						return;
					}
					DiscountPercentTextEdit.SelectedText = value;
					if (selectionLength > 0)
					{
						DiscountPercentTextEdit.SelectionLength = 0;
					}
				}
				if (DiscountPercentTextEdit.DisplayText[1] == ',' && DiscountPercentTextEdit.DisplayText.Length > length)
				{
					num++;
				}
				DiscountPercentTextEdit.CaretIndex = num + value.Length;
			}
			else if (_currentFocusedDiscountElement == SaleWindowHelper.CurrentFocusedDiscountControlField.DiscountSumm)
			{
				if (DiscountPercentSummTextEdit.IsReadOnly)
				{
					return;
				}
				int num2 = DiscountPercentSummTextEdit.CaretIndex;
				int selectionLength2 = DiscountPercentSummTextEdit.SelectionLength;
				string text2 = DiscountPercentSummTextEdit.Text;
				string displayText2 = DiscountPercentSummTextEdit.DisplayText;
				int length2 = DiscountPercentSummTextEdit.DisplayText.Length;
				if (selectionLength2 == 0)
				{
					if (length2 >= DiscountPercentSummTextEdit.Mask.Length && (num2 < displayText2.IndexOf(".") || (num2 == displayText2.IndexOf(".") && value != ".")))
					{
						return;
					}
					if (num2 == length2)
					{
						DiscountPercentSummTextEdit.Text = UIHelper.ConcatFieldNumericTextWithSymbolsLeftToRight(text2, value, 2);
					}
					else if (num2 == 0)
					{
						DiscountPercentSummTextEdit.Text = UIHelper.ConcatFieldNumericTextWithSymbolsLeftToRight(value, text2, 2);
					}
					else
					{
						if (num2 > length2)
						{
							num2 = length2;
						}
						string fieldText3 = displayText2.Substring(0, num2);
						string fieldText4 = displayText2.Substring(num2);
						fieldText3 = UIHelper.RemoveRedundantCommaSymbolsOfText(fieldText3);
						fieldText4 = UIHelper.RemoveRedundantCommaSymbolsOfText(fieldText4);
						DiscountPercentSummTextEdit.Text = UIHelper.ConcatFieldNumericTextWithSymbolsLeftToRight(UIHelper.ConcatFieldNumericTextWithSymbolsLeftToRight(fieldText3, value, 2), fieldText4, 2);
					}
				}
				else
				{
					if (selectionLength2 < value.Length)
					{
						return;
					}
					DiscountPercentSummTextEdit.SelectedText = value;
					if (selectionLength2 > 0)
					{
						DiscountPercentSummTextEdit.SelectionLength = 0;
					}
				}
				if (DiscountPercentSummTextEdit.DisplayText[1] == ',' && DiscountPercentSummTextEdit.DisplayText.Length > length2)
				{
					num2++;
				}
				DiscountPercentSummTextEdit.CaretIndex = num2 + value.Length;
			}
			else
			{
				if (_currentFocusedDiscountElement != SaleWindowHelper.CurrentFocusedDiscountControlField.DiscountedSumm || DiscountedPercentSummTextEdit.IsReadOnly)
				{
					return;
				}
				int num3 = DiscountedPercentSummTextEdit.CaretIndex;
				int selectionLength3 = DiscountedPercentSummTextEdit.SelectionLength;
				string text3 = DiscountedPercentSummTextEdit.Text;
				string displayText3 = DiscountedPercentSummTextEdit.DisplayText;
				int length3 = DiscountedPercentSummTextEdit.DisplayText.Length;
				if (selectionLength3 == 0)
				{
					if (length3 >= DiscountedPercentSummTextEdit.Mask.Length && (num3 < displayText3.IndexOf(".") || (num3 == displayText3.IndexOf(".") && value != ".")))
					{
						return;
					}
					if (num3 == length3)
					{
						DiscountedPercentSummTextEdit.Text = UIHelper.ConcatFieldNumericTextWithSymbolsLeftToRight(text3, value, 2);
					}
					else if (num3 == 0)
					{
						DiscountedPercentSummTextEdit.Text = UIHelper.ConcatFieldNumericTextWithSymbolsLeftToRight(value, text3, 2);
					}
					else
					{
						if (num3 > length3)
						{
							num3 = length3;
						}
						string fieldText5 = displayText3.Substring(0, num3);
						string fieldText6 = displayText3.Substring(num3);
						fieldText5 = UIHelper.RemoveRedundantCommaSymbolsOfText(fieldText5);
						fieldText6 = UIHelper.RemoveRedundantCommaSymbolsOfText(fieldText6);
						DiscountedPercentSummTextEdit.Text = UIHelper.ConcatFieldNumericTextWithSymbolsLeftToRight(UIHelper.ConcatFieldNumericTextWithSymbolsLeftToRight(fieldText5, value, 2), fieldText6, 2);
					}
				}
				else
				{
					if (selectionLength3 < value.Length)
					{
						return;
					}
					DiscountedPercentSummTextEdit.SelectedText = value;
					if (selectionLength3 > 0)
					{
						DiscountedPercentSummTextEdit.SelectionLength = 0;
					}
				}
				if (DiscountedPercentSummTextEdit.DisplayText[1] == ',' && DiscountedPercentSummTextEdit.DisplayText.Length > length3)
				{
					num3++;
				}
				DiscountedPercentSummTextEdit.CaretIndex = num3 + value.Length;
			}
		}
		catch
		{
		}
	}

	private void OnConfirmButtonPress()
	{
		try
		{
			DiscountPercentValueChanged();
			DiscountSummValueChanged();
			DiscountedSummValueChanged();
			ValidateDiscountPercent();
			ValidateDiscountSumm();
			ValidateDiscountedSumm(isEditCompleted: true);
			SaleRow.DiscountSumma = DiscountSumm;
			SaleRow.Summa = DiscountedSumm;
			SaleRow.Discount = DiscountPercent;
			SaleRow.DiscountPrice = DiscountPrice;
			DialogResult = true;
			Close();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
