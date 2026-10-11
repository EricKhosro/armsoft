using System;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.CustomWindowControls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.UI.CustomUIElements.Windows;
using ArmSoft.SME.Trade.UI.Enums;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI;

public class TouchGridControlWithExtraButtons<T> : TouchGridControl<T>, IScanable
{
	private CWCSimpleButton _searchButton;

	private CWCSimpleButton _clearButton;

	private CWCSimpleButton _pageUPButton;

	private CWCSimpleButton _pageDownButton;

	private CWCSimpleButton _barCodeButton;

	private bool _allowFindAndFilterByBarCode;

	private const string _filterFieldName = "MTCode";

	public bool AllowFindAndFilterByBarCode
	{
		set
		{
			_allowFindAndFilterByBarCode = value;
			if (value)
			{
				AddBarCodeButton();
			}
		}
	}

	public TouchGridControlWithExtraButtons(Window owner, IDropDownView dropDownView, string name)
		: base(owner, dropDownView, name)
	{
	}

	protected override void InitializeComponent()
	{
		base.InitializeComponent();
		AddSearchButton();
		AddClearButton();
		AddPageUpButton();
		AddPageDownButton();
		EmptyRow.Height = new GridLength(30.0);
	}

	private void AddSearchButton()
	{
		_searchButton = new CWCSimpleButton
		{
			Name = "SearchButton",
			Height = 40.0,
			Margin = new Thickness(5.0, 0.0, 5.0, 0.0),
			UseLayoutRounding = true,
			Content = ArmSoft.SME.Common.Resources.Find
		};
		_searchButton.Click += SearchButton_Click;
		Grid.SetColumn(_searchButton, 1);
		Grid.SetRow(_searchButton, 2);
		ButtonsPanelGrid.Children.Add(_searchButton);
	}

	private void AddClearButton()
	{
		_clearButton = new CWCSimpleButton
		{
			Name = "ClearButton",
			Height = 40.0,
			Margin = new Thickness(5.0, 10.0, 5.0, 0.0),
			UseLayoutRounding = true,
			Content = ArmSoft.SME.Common.Resources.ClearWord
		};
		_clearButton.Click += ClearButton_Click;
		Grid.SetColumn(_clearButton, 1);
		Grid.SetRow(_clearButton, 4);
		ButtonsPanelGrid.Children.Add(_clearButton);
	}

	private void AddPageUpButton()
	{
		_pageUPButton = new CWCSimpleButton
		{
			Name = "PageUpButton",
			Height = 40.0,
			Margin = new Thickness(5.0, 10.0, 5.0, 0.0),
			UseLayoutRounding = true,
			Content = ArmSoft.SME.Trade.Common.Resources.PageUp
		};
		_pageUPButton.Click += PageUpButton_Click;
		Grid.SetColumn(_pageUPButton, 1);
		Grid.SetRow(_pageUPButton, 5);
		ButtonsPanelGrid.Children.Add(_pageUPButton);
	}

	private void AddPageDownButton()
	{
		_pageDownButton = new CWCSimpleButton
		{
			Name = "PageDownButton",
			Height = 40.0,
			Margin = new Thickness(5.0, 10.0, 5.0, 0.0),
			UseLayoutRounding = true,
			Content = ArmSoft.SME.Trade.Common.Resources.PageDown
		};
		_pageDownButton.Click += PageDownButton_Click;
		Grid.SetColumn(_pageDownButton, 1);
		Grid.SetRow(_pageDownButton, 6);
		ButtonsPanelGrid.Children.Add(_pageDownButton);
	}

	private void AddBarCodeButton()
	{
		_barCodeButton = new CWCSimpleButton
		{
			Name = "BarCodeButton",
			Height = 40.0,
			Margin = new Thickness(5.0, 10.0, 5.0, 0.0),
			UseLayoutRounding = true,
			Content = ArmSoft.SME.Common.Resources.SearchByBarCodeShort
		};
		_barCodeButton.Click += BarCodeButton_Click;
		Grid.SetColumn(_barCodeButton, 1);
		Grid.SetRow(_barCodeButton, 3);
		ButtonsPanelGrid.Children.Add(_barCodeButton);
	}

	private void SearchButton_Click(object sender, RoutedEventArgs e)
	{
		CustomSearchWindow customSearchWindow = new CustomSearchWindow(this);
		if (TableView.SearchControl != null)
		{
			customSearchWindow.SearchControl.SearchText = TableView.SearchControl.SearchText;
		}
		TableView.SearchControl = customSearchWindow.SearchControl;
		customSearchWindow.ShowDialog();
	}

	private void ClearButton_Click(object sender, RoutedEventArgs e)
	{
		if (TableView.SearchControl != null)
		{
			TableView.SearchControl.SearchText = string.Empty;
		}
		if (_allowFindAndFilterByBarCode)
		{
			GridControl.ClearColumnFilter("MTCode");
		}
	}

	private void PageUpButton_Click(object sender, RoutedEventArgs e)
	{
		TableView.MovePrevPage();
	}

	private void PageDownButton_Click(object sender, RoutedEventArgs e)
	{
		TableView.MoveNextPage();
	}

	private void BarCodeButton_Click(object sender, RoutedEventArgs e)
	{
		CustomBarCodeOrTINSearchWindow customBarCodeOrTINSearchWindow = new CustomBarCodeOrTINSearchWindow(this, SearchModes.BarCode);
		if (customBarCodeOrTINSearchWindow.ShowDialog().Value)
		{
			FindAndFilterByBarCode(customBarCodeOrTINSearchWindow.SearchText);
		}
	}

	public void OnScanner(string scannerData)
	{
		try
		{
			if (_allowFindAndFilterByBarCode)
			{
				FindAndFilterByBarCode(scannerData);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void FindAndFilterByBarCode(string barcode)
	{
		MaterialCacheRow materialCacheRow = DocumentHelpers.TryGetItemByBarCode(ref barcode, out var _, out var _, out var _);
		if (materialCacheRow != null)
		{
			GridControl.ClearColumnFilter("MTCode");
			GridControl.FilterString = string.Format(" ( [{0}] = '{1}')", "MTCode", materialCacheRow.MTCode);
		}
		else
		{
			GridControl.ClearColumnFilter("MTCode");
			GridControl.FilterString = string.Format(" ( [{0}] = '{1}')", "MTCode", "");
		}
	}
}
