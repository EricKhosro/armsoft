using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.DataProviders.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DataView.Directories.Goods;
using ArmSoft.SME.Trade.UI.DataView.Reports;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Utils;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

public class GroupElScaleAndTerminalExportWindow : DialogWindow
{
	private DateEditControl _date;

	private DocumentGridControl _elScalesAndTerminalsGrid;

	private TableView _elScalesAndTerminalsTableView;

	private readonly List<ElScalesAndTerminalsDataProvider.DataRow> _scaleList;

	private readonly List<TerminalRow> _sourceDelegate;

	private List<MaterialCacheRow> _goods;

	private DocumentGridColumn _selectCol;

	private DocumentGridColumn _nameCol;

	private DocumentGridColumn _typeCol;

	private DocumentGridColumn _modelCol;

	private DocumentGridColumn _exportFileCol;

	public GroupElScaleAndTerminalExportWindow(List<MaterialCacheRow> goods, List<TerminalRow> sourceDelegate, string elType = null)
	{
		HelpId = "Sale_Operations_Export(El_Scale_Terminal)";
		Caption = ArmSoft.SME.Trade.Common.Resources.ExportProducts;
		List<ElScalesAndTerminalsDataProvider.DataRow> rows = new ElScalesAndTerminalsDataProvider(TradeSettings.CreateTradeClient()).GetRows(new ElScalesAndTerminalsDataProvider.Param
		{
			ElType = elType,
			ShowOnlyActives = true
		});
		_scaleList = rows;
		_sourceDelegate = sourceDelegate;
		_goods = goods;
		Initialize();
		InnerInitialize();
	}

	private void Initialize()
	{
		foreach (ElScalesAndTerminalsDataProvider.DataRow scale in _scaleList)
		{
			TerminalTypesSE result2;
			if (scale.ElType == DeviceTypesSE.ElScale.Value)
			{
				if (SmartEnum<ElScaleTypesSE, string>.TryFromValue(scale.ElModel.Trim(), out var result))
				{
					scale.ElModelCaption = result.Caption;
				}
				else
				{
					scale.ElModelCaption = (ElScaleAndTerminalHelper.ElScaleTypes.TryGetValue(scale.ElModel.Trim(), out var value) ? value.ScaleType : "");
				}
			}
			else if (SmartEnum<TerminalTypesSE, string>.TryFromValue(scale.ElModel.Trim(), out result2))
			{
				scale.ElModelCaption = result2.Caption;
			}
			else
			{
				scale.ElModelCaption = ElScaleAndTerminalHelper.TerminalTypes[scale.ElModel.Trim()].Brand;
			}
		}
	}

	private void InnerInitialize()
	{
		_date = AddDateControl("_date", ArmSoft.SME.Common.Resources.DateWord, useLongDate: false, isRequired: true);
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_elScalesAndTerminalsGrid = AddGridControl("ScaleList");
		_elScalesAndTerminalsGrid.Width = SizeCalculator.Width(880.0);
		_elScalesAndTerminalsGrid.Height = SizeCalculator.Height(250.0);
		_elScalesAndTerminalsGrid.AllowAdd = false;
		_elScalesAndTerminalsGrid.AllowDelete = false;
		_elScalesAndTerminalsGrid.AllowDuplicate = false;
		_elScalesAndTerminalsGrid.ShowButtons = false;
		_elScalesAndTerminalsGrid.AllowDrop = false;
		_elScalesAndTerminalsGrid.AllowEdit = true;
		_elScalesAndTerminalsGrid.IsRequired = true;
		_elScalesAndTerminalsGrid.Caption = ArmSoft.SME.Trade.Common.Resources.ElScalesAndTerminals;
		_elScalesAndTerminalsTableView = _elScalesAndTerminalsGrid.View;
		_elScalesAndTerminalsTableView.AllowMoveColumnToDropArea = false;
		_elScalesAndTerminalsTableView.IsColumnMenuEnabled = false;
		_elScalesAndTerminalsTableView.AutoWidth = true;
		_elScalesAndTerminalsTableView.ColumnHeaderClick += MTListView_ColumnHeaderClick;
		_selectCol = _elScalesAndTerminalsGrid.AddBooleanColumn("IsSelected", "");
		_selectCol.Width = 50.0;
		_selectCol.AllowEditing = DefaultBoolean.True;
		_selectCol.AllowSorting = DefaultBoolean.True;
		_nameCol = _elScalesAndTerminalsGrid.AddStringColumn("Name", ArmSoft.SME.Common.Resources.Name, 15);
		_nameCol.AllowEditing = DefaultBoolean.False;
		_typeCol = _elScalesAndTerminalsGrid.AddStringColumn("ElTypeCaption", ArmSoft.SME.Trade.Common.Resources.DeviceType, 15);
		_typeCol.AllowEditing = DefaultBoolean.False;
		_modelCol = _elScalesAndTerminalsGrid.AddStringColumn("ElModelCaption", ArmSoft.SME.Trade.Common.Resources.DeviceModel, 15);
		_modelCol.AllowEditing = DefaultBoolean.False;
		_exportFileCol = _elScalesAndTerminalsGrid.AddStringColumn("ExportFile", ArmSoft.SME.Trade.Common.Resources.FileToExport + " / " + ArmSoft.SME.Common.Resources.IPAddress, 15);
		_exportFileCol.AllowEditing = DefaultBoolean.False;
		HyperlinkEditSettings hyperlinkEditSettings = new HyperlinkEditSettings();
		hyperlinkEditSettings.TextWrapping = TextWrapping.NoWrap;
		hyperlinkEditSettings.RequestNavigation += ElScalesAndTerminals.Hyperlink_RequestNavigation;
		_exportFileCol.EditSettings = hyperlinkEditSettings;
		_elScalesAndTerminalsGrid.ItemsSource = _scaleList;
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		base.DialogValidation(args);
		IEnumerable<ElScalesAndTerminalsDataProvider.DataRow> enumerable = _scaleList.Where((ElScalesAndTerminalsDataProvider.DataRow x) => x.IsSelected);
		MessageWithHeaderReport messageWithHeaderReport = new MessageWithHeaderReport(ArmSoft.SME.Common.Resources.Export);
		IEnumerable<MaterialCacheRow> source = from x in TradeSettings.TradeCacheService.Material.GetList()
			where x.Type == GoodsTypesSE.Good.Value && x.IsWeight
			select x;
		foreach (ElScalesAndTerminalsDataProvider.DataRow item in enumerable)
		{
			try
			{
				if (item.ElType == DeviceTypesSE.ElScale.Value)
				{
					if (_goods == null)
					{
						if (!string.IsNullOrWhiteSpace(item.Group))
						{
							source = from x in TradeSettings.TradeCacheService.Material.GetItems(item.Group)
								where x.Type == GoodsTypesSE.Good.Value && x.IsWeight
								select x;
						}
						if (!string.IsNullOrWhiteSpace(item.Segment))
						{
							GoodsBySegmentDataProvider.Param param = new GoodsBySegmentDataProvider.Param
							{
								SegmentCode = item.Segment
							};
							List<GoodsBySegmentDataProvider.DataRow> rows = new GoodsBySegmentDataProvider(TradeSettings.CreateTradeClient()).Execute(param, new ExecutionSettings()).Rows;
							HashSet<string> myCodes = rows.Select((GoodsBySegmentDataProvider.DataRow x) => x.MTCode).ToHashSet();
							List<MaterialCacheRow> list = new List<MaterialCacheRow>();
							list.AddRange(source.Where((MaterialCacheRow g) => myCodes.Contains(g.MTCode)));
							source = new _003C_003Ez__ReadOnlyList<MaterialCacheRow>(list);
						}
						_goods = source.ToList();
					}
					if (item.ElModel == ElScaleTypesSE.MassaK)
					{
						ElScaleAndTerminalHelper.ExportGoodsForMassak(_date.EditValue.Value.ToDateTime(), _goods, item);
					}
					else if (item.ElModel == ElScaleTypesSE.CasDirect.Value)
					{
						ElScaleAndTerminalHelper.ExportForCAS(_date.EditValue.Value.ToDateTime(), _goods, item);
					}
					else if (item.ElModel == ElScaleTypesSE.DigiDirect.Value)
					{
						ElScaleAndTerminalHelper.ExportForDigi(_date.EditValue.Value.ToDateTime(), _goods, item);
					}
					else if (item.ElModel == ElScaleTypesSE.TMADirect.Value)
					{
						ElScaleAndTerminalHelper.ExportForTMA(_date.EditValue.Value.ToDateTime(), _goods, item);
					}
					else if (item.ElModel == ElScaleTypesSE.MertechDirect.Value)
					{
						ElScaleAndTerminalHelper.ExportForMertech(_date.EditValue.Value.ToDateTime(), _goods, item);
					}
					else
					{
						ElScaleAndTerminalHelper.ExportGoodsForElScales(_date.EditValue.Value.ToDateTime(), _goods, messageWithHeaderReport, item);
					}
				}
				else
				{
					ElScaleAndTerminalHelper.ExportGoodsForTerminals(_date.EditValue.Value.ToDateTime(), _sourceDelegate, messageWithHeaderReport, item);
				}
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
		}
		if (messageWithHeaderReport.RowsCount != 0)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OlapExpDone, ArmSoft.SME.Common.Resources.Export, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			messageWithHeaderReport.Show();
		}
	}

	private void MTListView_ColumnHeaderClick(object sender, ColumnHeaderClickEventArgs e)
	{
		if (e.Column == _selectCol && _elScalesAndTerminalsGrid.Grid.VisibleRowCount != 0)
		{
			SelectDeselect();
			_elScalesAndTerminalsGrid.Grid.RefreshData();
			e.Handled = true;
		}
	}

	private void SelectDeselect()
	{
		if (_selectCol.AllowEditing == DefaultBoolean.False)
		{
			return;
		}
		bool flag = true;
		foreach (ElScalesAndTerminalsDataProvider.DataRow scale in _scaleList)
		{
			if (!scale.IsSelected)
			{
				flag = false;
				break;
			}
		}
		for (int i = 0; i <= _elScalesAndTerminalsGrid.Grid.VisibleRowCount - 1; i++)
		{
			int rowHandleByVisibleIndex = _elScalesAndTerminalsGrid.Grid.GetRowHandleByVisibleIndex(i);
			ElScalesAndTerminalsDataProvider.DataRow dataRow = (ElScalesAndTerminalsDataProvider.DataRow)_elScalesAndTerminalsGrid.Grid.GetRow(rowHandleByVisibleIndex);
			if (dataRow != null)
			{
				if (flag)
				{
					dataRow.IsSelected = false;
				}
				else
				{
					dataRow.IsSelected = true;
				}
			}
		}
		_elScalesAndTerminalsGrid.Grid.RefreshData();
	}
}
