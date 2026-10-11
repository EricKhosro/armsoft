using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.StandartWindow;

public partial class HistoryPanel : LayoutPanel, IComponentConnector
{
	public class HistoryIconToImage : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (value == null)
			{
				return null;
			}
			return ImageService.GetImage(value.ToString());
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	private readonly HistoryController _controller;

	private Dictionary<HistoryItem.ItemCategory, HistoryCategory> _categories;

	public List<HistoryCategory> Categories
	{
		set
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			_categories = new Dictionary<HistoryItem.ItemCategory, HistoryCategory>();
			foreach (HistoryCategory item in value)
			{
				BarCheckItem barCheckItem = new BarCheckItem
				{
					Glyph = ImageService.GetImage(item.Icon),
					Content = item.Caption,
					Tag = item.ID
				};
				CategoriesToolbar.Items.Add(barCheckItem);
				_categories.Add(item.ID, item);
				barCheckItem.CheckedChanged += new SafeEventHandler<ItemClickEventArgs>(FilterGrid1).Handler;
			}
			GridColumn gridColumn = xGrid.Columns["Icon"];
			Binding binding = new Binding("Icon")
			{
				Converter = new HistoryIconToImage(),
				Mode = BindingMode.OneWay
			};
			gridColumn.Binding = binding;
			gridColumn.CellTemplate = Resources["ColumnToImage"] as DataTemplate;
			gridColumn.UnboundType = UnboundColumnType.Object;
			gridColumn.AllowEditing = DefaultBoolean.False;
		}
	}

	public HistoryPanel(HistoryController controller)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		_controller = controller;
		xGrid.ItemsSource = controller.ItemsList;
		xView.UseLightweightTemplates = UseLightweightTemplates.None;
		_controller.ItemsChanged += RefreshGrid;
		Caption = ArmSoft.SME.Common.Resources.History;
		btnClearList.ToolTip = ArmSoft.SME.Common.Resources.ClearWord;
		btnClearList.Content = btnClearList.ToolTip;
		btnClearList.Glyph = ImageService.GetImage("delete_16.svg");
		xView.RowStyle = (Style)Resources["NewRowStyle"];
		xView.RowDoubleClick += new SafeEventHandler<RowDoubleClickEventArgs>(RowClick).Handler;
		xView.MouseLeftButtonDown += new SafeEventHandler<MouseButtonEventArgs>(View_MouseLeftButtonDown).Handler;
		xView.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(View_PreviewKeyDown).Handler;
		ASFontHelper.SetFontSettings(this);
	}

	internal void RefreshGrid()
	{
		xGrid.RefreshData();
		if (xGrid.VisibleRowCount > 0)
		{
			xGrid.SelectedItem = xGrid.GetRow(xGrid.GetRowHandleByVisibleIndex(0));
		}
	}

	public void ClearList(object sender, ItemClickEventArgs e)
	{
		_controller.Clear();
	}

	private void FilterGrid1(object sender, ItemClickEventArgs e)
	{
		string text = "";
		foreach (IBarItem item in CategoriesToolbar.Items)
		{
			if (item is BarCheckItem)
			{
				BarCheckItem barCheckItem = (BarCheckItem)item;
				if (Convert.ToBoolean(barCheckItem.IsChecked))
				{
					text = ((!(text == "")) ? (text + " ,'" + barCheckItem.Tag.ToString() + "'") : ("[Category] in ('" + barCheckItem.Tag.ToString() + "'"));
				}
			}
		}
		if (text != "")
		{
			text += ")";
		}
		xGrid.FilterString = text;
	}

	public void UpdateItems()
	{
		_controller.RefreshItems();
		RefreshGrid();
	}

	private void View_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		int rowHandleByMouseEventArgs = xView.GetRowHandleByMouseEventArgs(e);
		if (rowHandleByMouseEventArgs != int.MinValue)
		{
			((HistoryItem)xGrid.GetRow(rowHandleByMouseEventArgs)).IsNewItem = false;
			xGrid.RefreshRow(rowHandleByMouseEventArgs);
		}
	}

	private void View_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 0)
			{
				RowClickOrEnter();
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void RowClick(object sender, RowDoubleClickEventArgs e)
	{
		RowClickOrEnter();
	}

	private void RowClickOrEnter()
	{
		if (Settings.SystemType != SystemTypes.Trade || Settings.ParametersService.AllowOpFromHistoryPanel())
		{
			if (xGrid.GetFocusedRow() is HistoryItem historyItem)
			{
				((DispatcherObject)this).Dispatcher.Invoke((Delegate)new Action<HistoryItem>(Settings.HostingEnvironment.HistoryItemClick), new object[1] { historyItem });
			}
		}
		else
		{
			MessageBox.Show(ArmSoft.SME.Common.Resources.NotPermissionEditHistoryPanelRow);
		}
	}
}
