using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Trade.UI;

public class TouchGridControl<T> : TouchGridBaseControl
{
	private readonly IDropDownView _dropDownView;

	private T _row;

	protected override TableView TableView
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		get
		{
			return _tableView;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			if (_tableView != null)
			{
				_tableView.PreviewMouseDoubleClick -= TableView_PreviewMouseDoubleClick;
			}
			_tableView = value;
			if (_tableView != null)
			{
				_tableView.PreviewMouseDoubleClick += TableView_PreviewMouseDoubleClick;
			}
		}
	}

	public T SelectedRow => _row;

	public TouchGridControl(Window owner, IDropDownView dropDownView, string name)
		: base(owner)
	{
		Name = name;
		Title = dropDownView.Caption;
		_dropDownView = dropDownView;
		InitializeComponent();
		LoadRegistrySettings();
	}

	protected override void InitializeComponent()
	{
		base.InitializeComponent();
		foreach (ViewColumn view in _dropDownView.Columns.ViewList)
		{
			_gridControl.Columns.Add(Mapper.Convert(view, _dropDownView.GetColumnType));
		}
		AddEvents();
		_gridControl.View.ShowSearchPanel(moveFocusInSearchPanel: true);
	}

	private void AddEvents()
	{
		_tableView.PreviewMouseDoubleClick += TableView_PreviewMouseDoubleClick;
		Loaded += TouchGridControl_Loaded;
		_okButton.Click += OKButton_Click;
		_exitButton.Click += ExitButton_Click;
	}

	private void OKButton_Click(object sender, RoutedEventArgs e)
	{
		ProcessOK();
	}

	private void ExitButton_Click(object sender, RoutedEventArgs e)
	{
		DialogResult = true;
	}

	private void TouchGridControl_Loaded(object sender, RoutedEventArgs e)
	{
		try
		{
			_dropDownView.BeforeLoadData(GridControl);
			_dropDownView.LoadData();
			GridControl.ItemsSource = _dropDownView.Data;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void TableView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		try
		{
			int rowHandleByMouseEventArgs = TableView.GetRowHandleByMouseEventArgs(e);
			if (rowHandleByMouseEventArgs != int.MinValue && rowHandleByMouseEventArgs != -2147483645)
			{
				ProcessOK();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void ProcessOK()
	{
		try
		{
			object focusedRow = GridControl.GetFocusedRow();
			if (focusedRow != null)
			{
				_row = (T)focusedRow;
			}
			DialogResult = true;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
