using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Helpers;
using Microsoft.Office.Interop.Excel;

namespace ArmSoft.SME.Trade.UI.DataView.Reports;

public class ExcelReports
{
	public static bool ExportPrice(DateOnly date, string type, string group, string mtCode, string itemsShowMode, string priceType, bool showAvailables)
	{
		try
		{
			int num = 0;
			int num2 = 1;
			int num3 = 2;
			int num4 = 3;
			int num5 = 4;
			int num6 = 5;
			int num7 = 6;
			int num8 = 7;
			int num9 = 8;
			int num10 = 0;
			int num11 = 0;
			int num12 = 1;
			int num13 = 2;
			int num14 = 3;
			int num15 = 4;
			Mouse.OverrideCursor = Cursors.Wait;
			Application application = new ApplicationClass();
			string[] array = new string[5];
			List<string> list = priceType.SplitSelected();
			int num16 = 9 + list.Count;
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
			Dictionary<string, string> priceTypesForPriceListColumns = PriceHelper.GetPriceTypesForPriceListColumns();
			_ = item?.Id;
			PriceListDataProvider.Param param = new PriceListDataProvider.Param
			{
				PriceListDate = date,
				Type = type,
				Group = group,
				MTCode = mtCode,
				ItemsShowMode = itemsShowMode,
				PriceTypes = priceType,
				ShowAvailables = showAvailables
			};
			List<PriceListDataProvider.DataRow> rows = new PriceListDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
			if (rows.Count == 0)
			{
				Mouse.OverrideCursor = null;
				return false;
			}
			object[,] array2 = new object[num16 + 1, rows.Count + 1];
			array2[num, num10] = ArmSoft.SME.Common.Resources.Name;
			array2[num2, num10] = ArmSoft.SME.Trade.Common.Resources.QntUnit;
			array2[num3, num10] = ArmSoft.SME.Common.Resources.Code;
			array2[num4, num10] = ArmSoft.SME.Common.Resources.Specification;
			array2[num5, num10] = ArmSoft.SME.Common.Resources.FullCaption;
			array2[num6, num10] = ArmSoft.SME.Common.Resources.Group;
			array2[num7, num10] = ArmSoft.SME.Common.Resources.Country;
			array2[num8, num10] = ArmSoft.SME.Common.Resources.Producer;
			array2[num9, num10] = ArmSoft.SME.Common.Resources.BarCode;
			int num17 = 9;
			foreach (string item2 in list)
			{
				if (priceTypesForPriceListColumns.TryGetValue(item2, out var value))
				{
					array2[num17, num10] = value;
				}
				num17++;
			}
			Dictionary<string, int> dictionary = (from p in TradeSettings.TradeCacheService.PriceType.GetList()
				orderby p.Code
				select p).ToList().Select((PriceTypeCacheRow pt, int idx) => new
			{
				Code = pt.Code,
				Index = idx
			}).ToDictionary(x => x.Code, x => x.Index * 2 + 1);
			for (int num18 = 0; num18 < rows.Count; num18++)
			{
				array2[num, num18 + 1] = rows[num18].Caption;
				array2[num2, num18 + 1] = rows[num18].UnitCaption;
				array2[num3, num18 + 1] = rows[num18].MTCode;
				array2[num4, num18 + 1] = rows[num18].Descr;
				array2[num5, num18 + 1] = rows[num18].FullCaption;
				array2[num6, num18 + 1] = rows[num18].Group;
				array2[num7, num18 + 1] = rows[num18].Country;
				array2[num8, num18 + 1] = rows[num18].Producer;
				array2[num9, num18 + 1] = rows[num18].BarCode;
				num17 = 9;
				foreach (string item3 in list)
				{
					if (dictionary.TryGetValue(item3, out var value2))
					{
						array2[num17, num18 + 1] = rows[num18].Values[value2];
						num17++;
					}
				}
			}
			array[num11] = Settings.ParametersService.CompanyName();
			array[num12] = Settings.ParametersService.CompanyAddress();
			array[num13] = Settings.ParametersService.PhoneNumber();
			array[num14] = DateExtension.DateToSymbol(date.ToDateTime());
			array[num15] = ArmSoft.SME.Common.Resources.PriceList;
			string tempFileNameByPrephix = Settings.GetTempFileNameByPrephix("Prices_" + date.FormatDDMMYY().Replace("/", ""));
			byte[] bytes = Settings.CreateApiClient().TemplateRoutes.ExportFile(Settings.CoreCacheService.Template.GetDefaultPrintTemplate(short.Parse(ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.PriceList)).Name, PrintTemplateTypeSE.Doc.Value);
			File.WriteAllBytes(tempFileNameByPrephix, bytes);
			Workbook workbook = application.Workbooks.Open(tempFileNameByPrephix, Type.Missing, true, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
			workbook.Application.DisplayAlerts = false;
			workbook.Application.Run(workbook.Name + "!ModCommon.Import", array2, array, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
			workbook.Application.DisplayAlerts = true;
			workbook.Application.ActiveWindow.WindowState = XlWindowState.xlMaximized;
			workbook.Application.Visible = true;
			Mouse.OverrideCursor = null;
			return true;
		}
		catch
		{
			Mouse.OverrideCursor = null;
			return false;
		}
	}

	public static void ExportDiscountsDeclarationToExel(DateOnly dateBegin, DateOnly dateEnd, List<ArmSoft.SME.Trade.Client.Data.Directories.Goods.DataRow> selectedGoods)
	{
		try
		{
			Mouse.OverrideCursor = Cursors.Wait;
			string[,] array = new string[selectedGoods.Count, 4];
			int num = 0;
			int num2 = 1;
			int num3 = 2;
			int num4 = 3;
			for (int i = 0; i < selectedGoods.Count; i++)
			{
				array[i, num] = selectedGoods[i].Name;
				array[i, num2] = selectedGoods[i].Discount.CurrencyFormat(6, 2);
				array[i, num3] = dateBegin.FormatDDMMYY();
				array[i, num4] = dateEnd.FormatDDMMYY();
			}
			ApplicationClass applicationClass = new ApplicationClass();
			string tempFileNameByPrephix = Settings.GetTempFileNameByPrephix("DiscountsDeclaration_" + DateTime.Now.FormatDDMMYY().Replace("/", ""));
			byte[] bytes = Settings.CreateApiClient().TemplateRoutes.ExportFile(Settings.CoreCacheService.Template.GetDefaultPrintTemplate(short.Parse(ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.DiscountsDeclaration)).Name, PrintTemplateTypeSE.Doc.Value);
			File.WriteAllBytes(tempFileNameByPrephix, bytes);
			Workbook workbook = ((_Application)applicationClass).Workbooks.Open(tempFileNameByPrephix, Type.Missing, true, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
			workbook.Application.DisplayAlerts = false;
			workbook.Application.Run(workbook.Name + "!modCommon.Import", array, Settings.ParametersService.CompanyName(), Settings.ParametersService.TaxCode(), Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
			workbook.Application.DisplayAlerts = true;
			workbook.Application.ActiveWindow.WindowState = XlWindowState.xlMaximized;
			workbook.Application.Visible = true;
			Mouse.OverrideCursor = null;
		}
		catch (Exception ex)
		{
			Mouse.OverrideCursor = null;
			Settings.ErrorHandler(ex);
		}
	}
}
