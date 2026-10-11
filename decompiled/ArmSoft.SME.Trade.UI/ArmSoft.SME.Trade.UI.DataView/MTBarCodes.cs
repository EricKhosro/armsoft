using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.Directories;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.OtherWindows;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10028)]
public class MTBarCodes : TradeDirectoryDataView<MTBarCodesDataProvider.DataRow, MTBarCodesDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.MTBarCode.Value;

	public string MTCode { get; set; }

	public bool IsShowOnlyComplects { get; set; }

	public string Group { get; set; }

	public string ItemType { get; set; }

	public override string Caption
	{
		get
		{
			string caption = base.Caption;
			if (!string.IsNullOrWhiteSpace(MTCode))
			{
				return caption = caption + ", " + MTCode + " - " + TradeSettings.TradeCacheService.Material.GetItem(MTCode).Caption;
			}
			return caption;
		}
	}

	public override string HelpId => "Purchases_Lists_Productsandservicessublist_Barcodes";

	public MTBarCodes()
	{
		DataProvider = new MTBarCodesDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("MTID", value: false);
		dvp.Set_ColumnVisible("QntUnitCaption", value: false);
		dvp.Set_ColumnVisible("ImageExists", value: false);
		dvp.Set_ColumnVisible("MTCode", string.IsNullOrWhiteSpace(MTCode));
		dvp.Set_ColumnVisible("MTCaption", string.IsNullOrWhiteSpace(MTCode));
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("MTCode");
		}
		Columns.AddDirectoryLinkColumn(DirectoryTypeSE.Goods, "MTCode", "MTCaption", "", ArmSoft.SME.Common.Resources.Product);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.MTCode = MTCode;
	}

	public override void Add()
	{
		ItemBarCodesDialogWindow itemBarCodesDialogWindow = new ItemBarCodesDialogWindow(DialogWindowMode.NewMode)
		{
			DVPId = Settings.HostingEnvironment.CurrentViewPanel.Id
		};
		MTBarCodesDataProvider.DataRow dataRow = (MTBarCodesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			itemBarCodesDialogWindow.Directory.MTCode = dataRow.MTCode;
		}
		else
		{
			itemBarCodesDialogWindow.Directory.MTCode = MTCode;
		}
		itemBarCodesDialogWindow.ShowDialog();
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "PrintLabel", ArmSoft.SME.Trade.Common.Resources.PrintLabel, PrintLabel);
		Settings.HostingEnvironment.CurrentViewPanel.AddContextMenuGroup("Export", ArmSoft.SME.Common.Resources.Export, "export_16.svg");
		Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("Export", "ExportForScale", ArmSoft.SME.Trade.Common.Resources.ByScaleFormat, ExportForScale);
		Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("Export", "ExportForTerminal", ArmSoft.SME.Trade.Common.Resources.ByTerminalFormat, ExportForTerminal);
		if (AllowOperation)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "GenerateNewBarcode", ArmSoft.SME.Trade.Common.Resources.AssignNewBarcode, GenerateNewBarcode);
		}
	}

	private void PrintLabel()
	{
		List<Label.GoodLabelRow> list = new List<Label.GoodLabelRow>();
		foreach (MTBarCodesDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<MTBarCodesDataProvider.DataRow>())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(selectedOrFocusedRow.MTCode);
			if (item != null)
			{
				Label.GoodLabelRow goodLabelRow = InitContextFunctionsHelper.BuildLabelGoodRow(item);
				goodLabelRow.BarCode = selectedOrFocusedRow.BarCode;
				list.Add(goodLabelRow);
			}
		}
		new PrintLabelDialog(null, null, list).Invoke();
	}

	private void ExportForScale()
	{
		List<MaterialCacheRow> list = new List<MaterialCacheRow>();
		foreach (MTBarCodesDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<MTBarCodesDataProvider.DataRow>())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(selectedOrFocusedRow.MTCode);
			if (item != null && item.Type == GoodsTypesSE.Good && item.IsWeight)
			{
				list.Add(item);
			}
		}
		new ElScaleExportWindow(list).ShowDialog();
	}

	private void ExportForTerminal()
	{
		new TerminalExportWindow(GetSourceList()).ShowDialog();
	}

	private static List<TerminalRow> GetSourceList()
	{
		List<TerminalRow> list = new List<TerminalRow>();
		foreach (MTBarCodesDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<MTBarCodesDataProvider.DataRow>())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(selectedOrFocusedRow.MTCode);
			if (item.Type == GoodsTypesSE.Good)
			{
				list.Add(new TerminalRow
				{
					Id = selectedOrFocusedRow.MTID,
					Code = selectedOrFocusedRow.MTCode,
					Name = item.Caption,
					Unit = item.Unit,
					AlternativeUnit = item.AltUnit,
					BarCode = null,
					Quantity = 1
				});
			}
		}
		return list;
	}

	private void GenerateNewBarcode()
	{
		int num = 0;
		try
		{
			Mouse.OverrideCursor = Cursors.Wait;
			List<MTBarCodesDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<MTBarCodesDataProvider.DataRow>();
			string nextBarCode = "";
			if (Settings.ParametersService.InternalBarcodeEANFormat())
			{
				foreach (MTBarCodesDataProvider.DataRow item in selectedOrFocusedRows)
				{
					num += GenerateNewEANBarcodeForOneRow(item.MTCode, ref nextBarCode, selectedOrFocusedRows.Count > 1);
					object key = GetKey(item);
					Settings.HostingEnvironment.CurrentViewPanel.Update(key);
					key = nextBarCode;
					Settings.HostingEnvironment.CurrentViewPanel.Update(key);
				}
			}
			else
			{
				foreach (MTBarCodesDataProvider.DataRow item2 in selectedOrFocusedRows)
				{
					num += GenerateNewBarcodeForOneRow(item2.MTCode, ref nextBarCode, selectedOrFocusedRows.Count > 1);
					object key2 = GetKey(item2);
					Settings.HostingEnvironment.CurrentViewPanel.Update(key2);
					key2 = nextBarCode;
					Settings.HostingEnvironment.CurrentViewPanel.Update(key2);
				}
			}
			foreach (MTBarCodesDataProvider.DataRow item3 in selectedOrFocusedRows)
			{
				_ = item3;
			}
			Mouse.OverrideCursor = null;
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.GenerateBarcodeCompleted, num), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private int GenerateNewEANBarcodeForOneRow(string goodCode, ref string nextBarCode, bool singleRow = false)
	{
		Good good = Settings.DirectoryService.Load<Good>(DirectoryType, goodCode);
		try
		{
			nextBarCode = Settings.ParametersService.InternalBarcodeNext();
			good.BarCode = MaterialHelpers.GenerateInnerBarcode();
			Settings.DirectoryService.Store(good, new DirStoreRequest());
			Settings.ParametersService.InternalBarcodeNext_set(StringExtensions.NextDocNum(Settings.ParametersService.InternalBarcodeNext(), 10));
			return 1;
		}
		catch
		{
			if (singleRow)
			{
				throw;
			}
		}
		return 0;
	}

	private static int GenerateNewBarcodeForOneRow(string goodCode, ref string nextBarCode, bool singleRow = false)
	{
		Good good = Settings.DirectoryService.Load<Good>(DirectoryTypeSE.Goods, goodCode);
		try
		{
			nextBarCode = Settings.ParametersService.InternalBarcodeNext();
			good.BarCode = Settings.ParametersService.InternalBarcodeNext();
			Settings.DirectoryService.Store(good, new DirStoreRequest());
			Settings.ParametersService.InternalBarcodeNext_set(StringExtensions.NextDocNum(good.BarCode, 20));
			return 1;
		}
		catch
		{
			if (singleRow)
			{
				throw;
			}
		}
		return 0;
	}

	public override bool CardViewAllowed()
	{
		return true;
	}

	public override DataTemplate GetCardHeaderTemplate()
	{
		return (DataTemplate)Application.Current.Resources["GoodsQUCardHeaderTemplate"];
	}

	public override DataTemplate GetCardTemplate()
	{
		return (DataTemplate)Application.Current.Resources["GoodsQUCardTemplate"];
	}

	public override DataTemplate GetCardPrintTemplate()
	{
		return (DataTemplate)Application.Current.Resources["GoodsQUCardPrintTemplate"];
	}
}
