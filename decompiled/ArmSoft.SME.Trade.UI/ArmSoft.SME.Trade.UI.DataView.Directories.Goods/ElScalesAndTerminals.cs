using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.DataProviders.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DataView.Reports;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;

namespace ArmSoft.SME.Trade.UI.DataView.Directories.Goods;

[DataView(10123)]
public class ElScalesAndTerminals : TradeDirectoryDataView<ElScalesAndTerminalsDataProvider.DataRow, ElScalesAndTerminalsDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly ElScalesAndTerminals _dataView;

		private readonly DropDownControl _elType;

		public DataViewDialog(ElScalesAndTerminals dataView, bool useParamValues)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			_elType = AddDropDownControl("ElType", ArmSoft.SME.Trade.Common.Resources.DeviceType, _dataView._dropDown);
			_elType.Code = (useParamValues ? dataView.ElType : string.Empty);
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.ElType = _elType.Code;
		}
	}

	private readonly IDropDownView _dropDown = new SmartEnum(typeof(DeviceTypesSE));

	public override short DirectoryType => DirectoryTypeSE.ElScaleOrTerminal.Value;

	protected override bool IsPOWindow => true;

	public override string HelpId => "Sales_lists_el_scales_terminals(off-line)";

	public override bool DialogSupported => true;

	public override bool AllowAdd => AllowOperation;

	public override bool AllowEdit => AllowOperation;

	public override bool AllowView => AllowOperation;

	public override bool AllowDelete => AllowOperation;

	public override bool AllowCreateCopy => true;

	public string ElType
	{
		get
		{
			return (string)Parameters["ElType"].Value;
		}
		set
		{
			Parameters["ElType"].Value = value;
		}
	}

	public ElScalesAndTerminals()
	{
		DataProvider = new ElScalesAndTerminalsDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("ElType", ArmSoft.SME.Trade.Common.Resources.DeviceType, "", "", includeCaptionToolTip: true, _dropDown);
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("ElType", value: false);
		dvp.Set_ColumnVisible("ElModel", value: false);
		dvp.Set_ColumnVisible("AsNameChoose", value: false);
		dvp.Set_ColumnVisible("AsCodeExport", value: false);
		dvp.Set_ColumnVisible("PriceList", value: false);
		dvp.Set_ColumnVisible("ExportForAdditionalQntUnit", value: false);
		dvp.Set_ColumnVisible("DeleteAllItemsBeforeExporting", value: false);
		dvp.Set_ColumnVisible("IsSelected", value: false);
		dvp.Set_ColumnVisible("WeightLabelN", value: false);
		dvp.Set_ColumnVisible("WeightBarCodeN", value: false);
		dvp.Set_ColumnVisible("PieceLabelN", value: false);
		dvp.Set_ColumnVisible("PieceBarCodeN", value: false);
		dvp.Set_ColumnVisible("Password", value: false);
		HyperlinkEditSettings hyperlinkEditSettings = new HyperlinkEditSettings();
		hyperlinkEditSettings.TextWrapping = TextWrapping.NoWrap;
		hyperlinkEditSettings.RequestNavigation += Hyperlink_RequestNavigation;
		dvp.InnerGrid().Columns["ExportFile"].EditSettings = hyperlinkEditSettings;
	}

	public static void Hyperlink_RequestNavigation(object sender, HyperlinkEditRequestNavigationEventArgs e)
	{
		if (string.IsNullOrWhiteSpace(e.NavigationUrl))
		{
			return;
		}
		Match match = Regex.Match(e.NavigationUrl, "^(?<ip>(\\d{1,3}\\.){3}\\d{1,3}):(?<port>\\d+)$");
		if (!match.Success)
		{
			if (File.Exists(e.NavigationUrl))
			{
				Process.Start("explorer.exe", "/select,\"" + e.NavigationUrl + "\"");
			}
			else if (System.IO.Directory.Exists(e.NavigationUrl))
			{
				Process.Start("explorer.exe", "\"" + e.NavigationUrl + "\"");
			}
			else
			{
				string directoryName = Path.GetDirectoryName(e.NavigationUrl);
				if (directoryName == null || !System.IO.Directory.Exists(directoryName))
				{
					return;
				}
				Process.Start("explorer.exe", "\"" + directoryName + "\"");
			}
		}
		string value = match.Groups["ip"].Value;
		if (!int.TryParse(match.Groups["port"].Value, out var result))
		{
			return;
		}
		try
		{
			using TcpClient tcpClient = new TcpClient();
			IAsyncResult asyncResult = tcpClient.BeginConnect(value, result, null, null);
			if (asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2L)) && tcpClient.Connected)
			{
				tcpClient.EndConnect(asyncResult);
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.Connected, "", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
			}
			else
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.Disconnected, "", MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK);
			}
		}
		catch
		{
		}
	}

	public override void AfterLoadData(DataViewPanel dvp)
	{
		base.AfterLoadData(dvp);
		foreach (ElScalesAndTerminalsDataProvider.DataRow datum in Data)
		{
			TerminalTypesSE result2;
			if (datum.ElType == DeviceTypesSE.ElScale.Value)
			{
				if (SmartEnum<ElScaleTypesSE, string>.TryFromValue(datum.ElModel.Trim(), out var result))
				{
					datum.ElModelCaption = result.Caption;
				}
				else
				{
					datum.ElModelCaption = (ElScaleAndTerminalHelper.ElScaleTypes.TryGetValue(datum.ElModel.Trim(), out var value) ? value.ScaleType : "");
				}
			}
			else if (SmartEnum<TerminalTypesSE, string>.TryFromValue(datum.ElModel.Trim(), out result2))
			{
				datum.ElModelCaption = result2.Caption;
			}
			else
			{
				datum.ElModelCaption = ElScaleAndTerminalHelper.TerminalTypes[datum.ElModel.Trim()].Brand;
			}
		}
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		ElScalesAndTerminalsDataProvider.DataRow dataRow = (ElScalesAndTerminalsDataProvider.DataRow)currentViewPanel.FocusedRow();
		if (dataRow != null && AllowOperation)
		{
			currentViewPanel.AddContextFunction("ContextFunctions", "ExportForElScaleOrTerminal", ArmSoft.SME.Trade.Common.Resources.ExportForElScaleAndTerminal, ExportForElScaleOrTerminal, "scale_import_16.svg");
			if (dataRow.ElModel == ElScaleTypesSE.MassaK || dataRow.ElModel == ElScaleTypesSE.CasDirect || dataRow.ElModel == ElScaleTypesSE.DigiDirect || dataRow.ElModel == ElScaleTypesSE.TMADirect || dataRow.ElModel == ElScaleTypesSE.MertechDirect)
			{
				currentViewPanel.AddContextFunction("ContextFunctions", "DirectImportFromElScale", ArmSoft.SME.Trade.Common.Resources.ViewScaleItemList, ElScaleAndTerminalHelper.DirectImportFromElScale, "scale_view_16.svg");
				currentViewPanel.AddContextSeparator("ContextFunctions");
				currentViewPanel.AddContextFunction("ContextFunctions", "EraseItemsFromScale", ArmSoft.SME.Trade.Common.Resources.EraseItemsFromScale, EraseItemsFromScale);
			}
		}
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues);
	}

	private void EraseItemsFromScale()
	{
		if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ConfirmErasingItemsFromScale, ArmSoft.SME.Common.Resources.Delete, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			ElScaleAndTerminalHelper.EraseItemsFromScale();
		}
	}

	private void ExportForElScaleOrTerminal()
	{
		_ = Settings.HostingEnvironment.CurrentViewPanel;
		List<ElScalesAndTerminalsDataProvider.DataRow> selectedOrFocusedRows = ParentViewPanel.GetSelectedOrFocusedRows<ElScalesAndTerminalsDataProvider.DataRow>();
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = ArmSoft.SME.Trade.Common.Resources.ExportProducts
		};
		dialogWindow.AddDateControl("Date", ArmSoft.SME.Common.Resources.DateWord, useLongDate: true).EditValue = DateTime.Today.ToDateOnly();
		if (!dialogWindow.ShowDialog().Value)
		{
			return;
		}
		DateTime expDate = dialogWindow["Date"].ToString().DDMMYYYYToDate();
		IReadOnlyList<MaterialCacheRow> readOnlyList = TradeSettings.TradeCacheService.Material.GetList((MaterialCacheRow x) => x.Type == GoodsTypesSE.Good.Value && x.IsWeight);
		List<TerminalRow> sourceList = DocumentHelpers.GetSourceList();
		MessageWithHeaderReport messageWithHeaderReport = new MessageWithHeaderReport(ArmSoft.SME.Common.Resources.Export);
		foreach (ElScalesAndTerminalsDataProvider.DataRow item in selectedOrFocusedRows)
		{
			try
			{
				if (item.ElType == DeviceTypesSE.ElScale)
				{
					if (!string.IsNullOrWhiteSpace(item.Group))
					{
						readOnlyList = (from x in TradeSettings.TradeCacheService.Material.GetItems(item.Group)
							where x.Type == GoodsTypesSE.Good.Value && x.IsWeight
							select x).ToList();
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
						list.AddRange(readOnlyList.Where((MaterialCacheRow g) => myCodes.Contains(g.MTCode)));
						readOnlyList = new _003C_003Ez__ReadOnlyList<MaterialCacheRow>(list);
					}
					if (item.ElModel == ElScaleTypesSE.MassaK.Value)
					{
						ElScaleAndTerminalHelper.ExportGoodsForMassak(expDate, readOnlyList, item);
					}
					else if (item.ElModel == ElScaleTypesSE.CasDirect.Value)
					{
						ElScaleAndTerminalHelper.ExportForCAS(expDate, readOnlyList, item);
					}
					else if (item.ElModel == ElScaleTypesSE.DigiDirect.Value)
					{
						ElScaleAndTerminalHelper.ExportForDigi(expDate, readOnlyList, item);
					}
					else if (item.ElModel == ElScaleTypesSE.TMADirect.Value)
					{
						ElScaleAndTerminalHelper.ExportForTMA(expDate, readOnlyList, item);
					}
					else if (item.ElModel == ElScaleTypesSE.MertechDirect.Value)
					{
						ElScaleAndTerminalHelper.ExportForMertech(expDate, readOnlyList, item);
					}
					else
					{
						ElScaleAndTerminalHelper.ExportGoodsForElScales(expDate, readOnlyList, messageWithHeaderReport, item);
					}
				}
				else
				{
					ElScaleAndTerminalHelper.ExportGoodsForTerminals(expDate, sourceList, messageWithHeaderReport, item);
				}
				Settings.HostingEnvironment.FindDataViewPanel(DVP.Id).Update(item.Code);
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

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.ElType = ElType;
	}
}
