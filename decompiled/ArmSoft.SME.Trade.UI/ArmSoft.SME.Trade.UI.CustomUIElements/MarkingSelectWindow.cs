using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class MarkingSelectWindow : DialogWindow
{
	public class MarkingRow
	{
		public string Marking { get; set; }

		public string Name { get; set; }

		public MarkingRow(string marking, string name = "")
		{
			Marking = marking;
			Name = name;
		}
	}

	private DocumentGridControl _markingGrid;

	private TableView _markingTableView;

	private TextEditExt _inputValue;

	private Button _add;

	private TextBlock _quantities;

	private readonly HashSet<string> _markings;

	private readonly string _itemCode;

	private readonly string _itemName;

	private readonly decimal _baseQuantity;

	private readonly bool _isReturn;

	private string _realText = "";

	public readonly bool IsViewOnlyMode;

	private readonly bool _isWeight;

	private readonly bool _eMarkReceipt;

	public readonly BindingList<MarkingRow> MarkingList = new BindingList<MarkingRow>();

	public readonly List<string> AllMarkingList;

	public MarkingSelectWindow(Window owner, bool viewMode, bool isRowMarkable, string itemCode, string itemName, HashSet<string> markings, List<string> allMarkings, decimal baseQuantity, bool isReturn = false, bool eMarkReceipt = false, bool forPOS = false)
		: base(owner)
	{
		Title = (eMarkReceipt ? ArmSoft.SME.Trade.Common.Resources.EMarkReceiptDoc : (ArmSoft.SME.Common.Resources.Marking + " - " + itemCode + " " + itemName.Trim()));
		IsViewOnlyMode = viewMode && (isRowMarkable || markings.Count != 0);
		_markings = markings;
		_itemCode = itemCode;
		_itemName = itemName;
		_baseQuantity = baseQuantity;
		_isReturn = isReturn;
		_eMarkReceipt = eMarkReceipt;
		_isWeight = TradeSettings.TradeCacheService.Material.GetItem(_itemCode)?.IsWeight ?? false;
		InnerInitialize();
		foreach (string marking in markings)
		{
			MarkingList.Add(new MarkingRow(marking));
		}
		AllMarkingList = allMarkings;
		ControlsHelper.SetFocus(_inputValue);
		if (IsViewOnlyMode)
		{
			SetReadOnly();
		}
		if (eMarkReceipt & forPOS)
		{
			_okButton.Height = 40.0;
			_cancelButton.Height = 40.0;
		}
	}

	private void InnerInitialize()
	{
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		ShowIcon = false;
		Height = 450.0;
		Width = 700.0;
		_txtStatus.Visibility = Visibility.Visible;
		SizeToContent = SizeToContent.Manual;
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		_inputValue = AddStringControl(container, "Scanner", ArmSoft.SME.Trade.Common.Resources.InputBoxCaption + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillInputValueHotKey()) ? "" : (" (" + Settings.ParametersService.MTBillInputValueHotKey() + ")")), 110);
		_inputValue.HorizontalAlignment = HorizontalAlignment.Stretch;
		_add = AddButton(container, "Add", null, ArmSoft.SME.Common.Resources.Add);
		_add.Width = 100.0;
		LayoutGroupExt layoutGroupExt = AddHorizontalLayoutGroup();
		layoutGroupExt.Margin = new Thickness(10.0, 3.0, 3.0, 0.0);
		if (_eMarkReceipt)
		{
			layoutGroupExt.Children.Add(new DXImage
			{
				Width = 16.0,
				Height = 16.0,
				Margin = new Thickness(0.0, -6.0, 0.0, 0.0),
				VerticalAlignment = VerticalAlignment.Center,
				Source = ImageService.GetImage("information_16.svg")
			});
		}
		_quantities = AddTextBlock(layoutGroupExt, "", TextBlockImage.None);
		_quantities.TextWrapping = TextWrapping.Wrap;
		_markingGrid = AddGridControl("_markingGrid");
		_markingGrid.AllowAdd = false;
		_markingGrid.AllowDuplicate = false;
		_markingGrid.DeletingRow += new SafeEventHandler<DocumentGridDeletingRowEventArgs>(MarkingGrid_DeletingRow).Handler;
		_markingGrid.IsVisibleDuplicateButton = false;
		DocumentGridColumn documentGridColumn = _markingGrid.AddStringColumn("Marking", ArmSoft.SME.Common.Resources.MarkingCodes, 110);
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		documentGridColumn.Width = 220.0;
		DocumentGridColumn documentGridColumn2 = _markingGrid.AddStringColumn("Name", ArmSoft.SME.Common.Resources.Name, 200);
		documentGridColumn2.AllowEditing = DefaultBoolean.False;
		documentGridColumn2.Visible = _eMarkReceipt;
		_markingTableView = _markingGrid.View;
		_markingTableView.AutoWidth = true;
		_markingGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
		_markingGrid.VerticalAlignment = VerticalAlignment.Stretch;
		_markingGrid.ItemsSource = MarkingList;
		Closing += new SafeEventHandler<CancelEventArgs>(MarkingSelectWindow_Closing).Handler;
		_markingGrid.WriteToStatusBarForCurrentRow += new SafeEventHandler<string>(MarkingGrid_WriteToStatusBarForCurrentRow).Handler;
		_add.Click += new SafeEventHandler<RoutedEventArgs>(BE_Add_Click).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(MarkingSelectWindow_PreviewKeyDown).Handler;
		_inputValue.PreviewTextInput += new SafeEventHandler<TextCompositionEventArgs>(InputValue_PreviewTextInput).Handler;
		_inputValue.KeyDown += new SafeEventHandler<KeyEventArgs>(InputValue_KeyDown).Handler;
	}

	private void MarkingGrid_DeletingRow(object sender, DocumentGridDeletingRowEventArgs e)
	{
		MarkingRow markingRow = (MarkingRow)_markingGrid.Grid.GetRow(e.RowHandle);
		if (markingRow != null)
		{
			AllMarkingList.Remove(markingRow.Marking);
		}
	}

	private void SetReadOnly()
	{
		_inputValue.IsReadOnly = true;
		_markingGrid.IsReadOnly = true;
		_add.IsEnabled = false;
	}

	public override void OnScanner(string scannerData)
	{
		if (!IsViewOnlyMode)
		{
			_inputValue.Text = MarkingHelper.CleanScannerSymbols(scannerData);
			AddRow();
		}
	}

	private void InputValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		_realText += e.Text;
	}

	private void InputValue_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 0 && !_inputValue.IsReadOnly && !string.IsNullOrWhiteSpace(_inputValue.Text))
			{
				e.Handled = true;
				_inputValue.Text = MarkingHelper.CleanScannerSymbols(_realText.Contains('\u001d') ? _realText : _inputValue.Text);
				AddRow();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			_realText = "";
		}
	}

	private void AddRow()
	{
		_inputValue.DoValidate();
		if (string.IsNullOrWhiteSpace(_inputValue.Text))
		{
			return;
		}
		byte[] bytes = Encoding.Unicode.GetBytes(_inputValue.Text);
		for (int i = 0; i < bytes.Length; i += 2)
		{
			if (((bytes[i + 1] << 8) | bytes[i]) > 127)
			{
				_inputValue.Text = "";
				_realText = "";
				ControlsHelper.SetFocus(_inputValue);
				throw new ASException(ArmSoft.SME.Common.Resources.KeyboardLanguageForMarkingScanShouldBeEnglish);
			}
		}
		if (AllMarkingList.Any((string c) => c == _inputValue.Text))
		{
			_inputValue.Text = "";
			_realText = "";
			ControlsHelper.SetFocus(_inputValue);
			throw new ASException(ArmSoft.SME.Common.Resources.ValueIsInList);
		}
		if (MarkingList.Any((MarkingRow c) => c.Marking == _inputValue.Text))
		{
			_inputValue.Text = "";
			_realText = "";
			ControlsHelper.SetFocus(_inputValue);
			throw new ASException(ArmSoft.SME.Common.Resources.ValueIsInList);
		}
		MarkingHelper.ValidateMarkingCode(_inputValue.Text);
		GetBarCodeRow(_inputValue.Text, out var materialCacheRow);
		if (_isReturn && (decimal)MarkingList.Count == _baseQuantity)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrQTYCannotBeMore, _itemCode, _itemName.Trim()));
		}
		MarkingList.Add(new MarkingRow(_inputValue.Text, materialCacheRow?.Caption));
		_markingGrid.Grid.RefreshRow(MarkingList.Count - 1);
		_inputValue.Text = "";
		_realText = "";
		ControlsHelper.SetFocus(_inputValue);
	}

	private void MarkingSelectWindow_Closing(object sender, CancelEventArgs e)
	{
		if (DialogResult == false && IsChanged() && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.QuestionWhenCloseWindow, Caption, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, this) == MessageBoxResult.No)
		{
			e.Cancel = true;
		}
	}

	private string GetBarCodeRow(string mark, out MaterialCacheRow materialCacheRow)
	{
		string text = MarkingHelpers.ExtractGTINFromDataMatrix(mark, out var good);
		materialCacheRow = good;
		if (_eMarkReceipt)
		{
			if (good == null)
			{
				_inputValue.Text = "";
				_realText = "";
				ControlsHelper.SetFocus(_inputValue);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodesNotExist, text));
			}
		}
		else if (!_eMarkReceipt && good?.MTCode != _itemCode && Settings.ParametersService.CheckMarkingAndBarCode())
		{
			_inputValue.Text = "";
			_realText = "";
			ControlsHelper.SetFocus(_inputValue);
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.MarkIsWrong, mark, _itemCode));
		}
		return text;
	}

	private bool IsChanged()
	{
		return !MarkingList.Select((MarkingRow r) => r.Marking).SequenceEqual(_markings);
	}

	private void MarkingSelectWindow_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.Key == 91 && (int)Keyboard.Modifiers == 0)
		{
			HotKeyRedirector("F2");
			e.Handled = true;
		}
	}

	private void HotKeyRedirector(string hotKey)
	{
		if (hotKey == Settings.ParametersService.MTBillInputValueHotKey() && !_inputValue.IsReadOnly)
		{
			ControlsHelper.SetFocus(_inputValue);
		}
	}

	private void BE_Add_Click(object sender, RoutedEventArgs e)
	{
		AddRow();
	}

	private void MarkingGrid_WriteToStatusBarForCurrentRow(object sender, string msg)
	{
		if (_eMarkReceipt)
		{
			_quantities.Text = ArmSoft.SME.Trade.Common.Resources.EMarkReceiptText;
			TextBlock quantities = _quantities;
			Brush foreground = (Foreground = ColorsAS.Foreground.BlueBrush);
			quantities.Foreground = foreground;
		}
		else if ((decimal)MarkingList.Count <= _baseQuantity)
		{
			_quantities.Text = string.Format(ArmSoft.SME.Common.Resources.MarkingSelectWindowQuantityInfo, MarkingList.Count, _baseQuantity - (decimal)MarkingList.Count, _baseQuantity);
			TextBlock quantities2 = _quantities;
			Brush foreground = (Foreground = ColorsAS.Foreground.BlueBrush);
			quantities2.Foreground = foreground;
		}
		else if (_isWeight)
		{
			_quantities.Text = "";
		}
		else
		{
			_quantities.Text = ArmSoft.SME.Common.Resources.CodesCountExceedsItemsQuantity;
			TextBlock quantities3 = _quantities;
			Brush foreground = (Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#f07174" : "#dd0326"));
			quantities3.Foreground = foreground;
		}
	}
}
