using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.CustomWindowControls;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI;

public class TouchGridBaseControl : ThemedWindow
{
	protected GridControl _gridControl;

	protected TableView _tableView;

	protected CWCSimpleButton _okButton;

	protected CWCSimpleButton _exitButton;

	protected RowDefinition EmptyRow { get; set; }

	protected Grid ButtonsPanelGrid { get; set; }

	protected virtual GridControl GridControl
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		get
		{
			return _gridControl;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			_gridControl = value;
		}
	}

	protected virtual TableView TableView
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		get
		{
			return _tableView;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
		}
	}

	public TouchGridBaseControl(Window owner)
	{
		Owner = owner;
		ASFontHelper.SetFontSettings(this);
	}

	protected virtual void InitializeComponent()
	{
		Height = 400.0;
		Width = 820.0;
		MinHeight = 240.0;
		MinWidth = 500.0;
		WindowState = WindowState.Normal;
		WindowStartupLocation = WindowStartupLocation.CenterOwner;
		ResizeMode = ResizeMode.CanResize;
		UseLayoutRounding = true;
		ShowIcon = false;
		ShowInTaskbar = false;
		DockPanel dockPanel = (DockPanel)(Content = new DockPanel
		{
			LastChildFill = true
		});
		ButtonsPanelGrid = new Grid();
		dockPanel.Children.Add(ButtonsPanelGrid);
		DockPanel.SetDock(ButtonsPanelGrid, Dock.Right);
		ButtonsPanelGrid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		ButtonsPanelGrid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(100.0)
		});
		ButtonsPanelGrid.RowDefinitions.Add(new RowDefinition
		{
			Name = "OKButtonRow",
			Height = new GridLength(1.0, GridUnitType.Auto)
		});
		ButtonsPanelGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(30.0)
		});
		ButtonsPanelGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Auto)
		});
		ButtonsPanelGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Auto)
		});
		ButtonsPanelGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Auto)
		});
		ButtonsPanelGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Auto)
		});
		ButtonsPanelGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Auto)
		});
		EmptyRow = new RowDefinition
		{
			Height = new GridLength(0.0)
		};
		ButtonsPanelGrid.RowDefinitions.Add(EmptyRow);
		ButtonsPanelGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Auto)
		});
		ButtonsPanelGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Auto)
		});
		AddOKButton();
		AddCancelButton();
		_gridControl = new GridControl
		{
			Name = "GridControl",
			AllowLiveDataShaping = true,
			Margin = new Thickness(5.0, 12.0, 5.0, 10.0),
			SelectionMode = MultiSelectMode.Cell
		};
		_tableView = new TableView
		{
			Name = "InnerView",
			ShowTotalSummary = false,
			IsRowCellMenuEnabled = true,
			IsTotalSummaryMenuEnabled = false,
			IsGroupPanelMenuEnabled = false,
			ShowFocusedRectangle = false,
			ShowGroupPanel = false,
			SearchPanelAllowFilter = true,
			SearchPanelNullText = "",
			ShowGroupedColumns = false,
			ShowSearchPanelMode = ShowSearchPanelMode.Always,
			SearchPanelHorizontalAlignment = HorizontalAlignment.Stretch,
			AllowBestFit = true,
			NewItemRowPosition = NewItemRowPosition.None,
			NavigationStyle = GridViewNavigationStyle.Row,
			AllowColumnFiltering = false,
			EditorButtonShowMode = EditorButtonShowMode.ShowForFocusedCell,
			EditorShowMode = EditorShowMode.MouseDown,
			EnterMoveNextColumn = true,
			AllowLeaveFocusOnTab = true,
			VerticalScrollbarVisibility = ScrollBarVisibility.Auto,
			AllowEditing = false,
			AllowMoveColumnToDropArea = false,
			AllowGrouping = false,
			AllowColumnMoving = false,
			AutoWidth = true,
			UseEvenRowBackground = true,
			RowMinHeight = 40.0,
			HeaderPanelMinHeight = 30.0,
			RowStyle = StyleHelper.GetTableRowWithSelectionStyle()
		};
		_gridControl.View = _tableView;
		dockPanel.Children.Add(_gridControl);
		AddingEvents();
	}

	private void AddingEvents()
	{
		Activated += TouchGridControl_Activated;
		Closing += TouchGridControl_Closing;
	}

	private void AddOKButton()
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		_okButton = new CWCSimpleButton
		{
			Name = "OKButton",
			Height = 40.0,
			Margin = new Thickness(5.0, 12.0, 5.0, 0.0),
			Padding = new Thickness(0.0, 0.0, 0.0, 0.0),
			IsDefault = true,
			UseLayoutRounding = true
		};
		DXImage element = new DXImage
		{
			Source = ImageService.GetImage("accept_16.svg"),
			Width = 24.0,
			Height = 24.0,
			Margin = new Thickness(0.0, -3.0, 0.0, 0.0),
			UseLayoutRounding = true,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center
		};
		Grid grid = new Grid
		{
			Margin = new Thickness(0.0, 0.0, 0.0, -1.0),
			Width = 88.0,
			Height = 39.0,
			UseLayoutRounding = true
		};
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(3.0)
		});
		Grid.SetColumn(element, 0);
		Grid.SetRow(element, 0);
		Grid.SetRowSpan(element, 2);
		grid.Children.Add(element);
		Rectangle element2 = new Rectangle
		{
			Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#4fad77" : "#3E7337"),
			UseLayoutRounding = true
		};
		Grid.SetColumn(element2, 0);
		Grid.SetRow(element2, 1);
		grid.Children.Add(element2);
		_okButton.Content = grid;
		Grid.SetColumn(_okButton, 1);
		Grid.SetRow(_okButton, 0);
		ButtonsPanelGrid.Children.Add(_okButton);
	}

	private void AddCancelButton()
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		_exitButton = new CWCSimpleButton
		{
			Name = "ExitButton",
			Height = 40.0,
			Margin = new Thickness(5.0, 0.0, 5.0, 10.0),
			Padding = new Thickness(0.0, 0.0, 0.0, 0.0),
			IsCancel = true,
			UseLayoutRounding = true
		};
		DXImage element = new DXImage
		{
			Source = ImageService.GetImage("cancel_16.svg"),
			Width = 24.0,
			Height = 24.0,
			Margin = new Thickness(0.0, -3.0, 0.0, 0.0),
			UseLayoutRounding = true,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center
		};
		Grid grid = new Grid
		{
			Margin = new Thickness(0.0, 0.0, 0.0, -1.0),
			Width = 88.0,
			Height = 39.0,
			UseLayoutRounding = true
		};
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(3.0)
		});
		Grid.SetColumn(element, 0);
		Grid.SetRow(element, 0);
		Grid.SetRowSpan(element, 2);
		grid.Children.Add(element);
		Rectangle element2 = new Rectangle
		{
			Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#fb7376" : "#dd0326"),
			UseLayoutRounding = true
		};
		Grid.SetColumn(element2, 0);
		Grid.SetRow(element2, 1);
		grid.Children.Add(element2);
		_exitButton.Content = grid;
		Grid.SetColumn(_exitButton, 1);
		Grid.SetRow(_exitButton, 8);
		ButtonsPanelGrid.Children.Add(_exitButton);
	}

	protected void LoadRegistrySettings()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\" + Name, writable: false);
			if (registryKey == null)
			{
				return;
			}
			if (double.TryParse(registryKey.GetValue("Width", "0").ToString(), out var result) && result > 0.0)
			{
				Width = result;
			}
			result = 0.0;
			if (double.TryParse(registryKey.GetValue("Height", "0").ToString(), out result) && result > 0.0)
			{
				Height = result;
			}
			foreach (GridColumn column in GridControl.Columns)
			{
				result = 0.0;
				if (double.TryParse(registryKey.GetValue(column.Name.Trim(), "0").ToString(), out result) && result > 0.0)
				{
					column.Width = result;
				}
			}
		}
		catch
		{
		}
	}

	private void TouchGridControl_Activated(object sender, EventArgs e)
	{
		try
		{
			GridControl.Focus();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void TouchGridControl_Closing(object sender, CancelEventArgs e)
	{
		try
		{
			RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\" + Name);
			if (registryKey == null)
			{
				return;
			}
			registryKey.SetValue("Width", Width);
			registryKey.SetValue("Height", Height);
			foreach (GridColumn column in GridControl.Columns)
			{
				string text = column.ActualWidth.ToString();
				if (text != "NaN")
				{
					registryKey.SetValue(column.Name.Trim(), text);
				}
			}
			registryKey.Close();
		}
		catch
		{
		}
	}

	protected virtual void ProcessOK()
	{
		DialogResult = true;
	}
}
