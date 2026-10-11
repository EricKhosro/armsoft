using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Trade.UI;

public class TouchGridControlWithCodeCaptionPair : TouchGridBaseControl
{
	private string _codeSearchButtonMode;

	private readonly List<CodeName> _codeName;

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

	public TouchGridControlWithCodeCaptionPair(Window owner, List<CodeName> codeName, string name, string selectedItemCode)
		: base(owner)
	{
		Title = name;
		_codeName = codeName;
		_codeSearchButtonMode = selectedItemCode;
		InitializeComponent();
		LoadRegistrySettings();
		ASFontHelper.SetFontSettings(this);
	}

	protected override void InitializeComponent()
	{
		base.InitializeComponent();
		MinHeight = 240.0;
		MinWidth = 500.0;
		_tableView.ShowSearchPanelMode = ShowSearchPanelMode.Never;
		_gridControl.Columns.Add(new GridColumn
		{
			Name = "Name",
			FieldName = "Name",
			Header = ArmSoft.SME.Common.Resources.Name
		});
		AddingEvents();
	}

	private void AddingEvents()
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
		DialogResult = false;
	}

	private void TouchGridControl_Loaded(object sender, RoutedEventArgs e)
	{
		try
		{
			GridControl.ItemsSource = _codeName;
			GridControl.SelectedItem = ((List<CodeName>)GridControl.ItemsSource).First((CodeName cn) => cn.Code == _codeSearchButtonMode);
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

	public CodeName GetSelectedItem()
	{
		return GridControl.GetFocusedRow() as CodeName;
	}
}
