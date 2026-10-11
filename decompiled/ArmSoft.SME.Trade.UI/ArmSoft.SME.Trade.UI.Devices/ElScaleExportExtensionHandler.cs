using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using Microsoft.Office.Interop.Excel;

namespace ArmSoft.SME.Trade.UI.Devices;

public abstract class ElScaleExportExtensionHandler
{
	private List<PricesDataProvider.DataRow> Prices { get; set; }

	protected List<MaterialCacheRow> Goods { get; set; }

	protected string NameInterpretation { get; set; }

	protected string WeightItemUnit { get; set; }

	protected string WeightItemPrefix { get; set; }

	protected string PieceItemUnit { get; set; }

	protected string PieceItemPrefix { get; set; }

	protected string ExportedFileFullPath { get; set; }

	protected string ExportNextCode { get; set; }

	public virtual string ExportedFileExtension => ".txt";

	public virtual string ExportedFileDescription => "Text Documents";

	public virtual string ExportedFileName => "ExportedForScale";

	public abstract string ScaleType { get; }

	public bool Export(IExtensionUIComponents uiCompontents, List<MaterialCacheRow> goods, List<PricesDataProvider.DataRow> prices, string nameInterpretation, string weightItemUnit, string weightItemPrefix, string pieceItemUnit, string pieceItemPrefix, string exportNextCode, string filePath = "")
	{
		Goods = goods;
		Prices = prices;
		NameInterpretation = nameInterpretation;
		WeightItemUnit = weightItemUnit;
		WeightItemPrefix = weightItemPrefix;
		PieceItemUnit = pieceItemUnit;
		PieceItemPrefix = pieceItemPrefix;
		ExportNextCode = exportNextCode;
		if (string.IsNullOrEmpty(filePath))
		{
			ExportedFileFullPath = uiCompontents.ShowSaveFileDialog(ExportedFileExtension, $"{ExportedFileDescription}|*{ExportedFileExtension}", ExportedFileName);
		}
		else
		{
			ExportedFileFullPath = filePath;
		}
		if (string.IsNullOrWhiteSpace(ExportedFileFullPath))
		{
			return false;
		}
		string serialized = AddHeader() + AddBody() + AddFooter();
		WriteToFile(serialized);
		return true;
	}

	protected virtual string AddHeader()
	{
		return string.Empty;
	}

	protected virtual string AddFooter()
	{
		return string.Empty;
	}

	protected virtual string AddBody()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (MaterialCacheRow good in Goods)
		{
			stringBuilder.AppendLine(SerializeGood(good));
		}
		return stringBuilder.ToString();
	}

	protected abstract string SerializeGood(MaterialCacheRow good);

	protected virtual void WriteToFile(string serialized)
	{
		switch (ExportedFileExtension ?? "")
		{
		case ".txt":
			WriteToText(serialized);
			break;
		case ".xls":
		case ".xlsx":
			WriteToExcel(serialized);
			break;
		case ".xml":
			WriteToXML(serialized);
			break;
		default:
			throw new NotImplementedException();
		}
	}

	private void WriteToText(string serialized)
	{
		using StreamWriter streamWriter = File.CreateText(ExportedFileFullPath);
		streamWriter.Write(serialized);
	}

	private void WriteToExcel(string serialized)
	{
		ApplicationClass applicationClass = new ApplicationClass();
		Workbook workbook = ((_Application)applicationClass).Workbooks.Add(Type.Missing);
		Worksheet worksheet = (Worksheet)workbook.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
		worksheet.Name = "PLU";
		worksheet.Cells.NumberFormat = "@";
		Clipboard.Clear();
		Clipboard.SetText(serialized);
		worksheet.Paste(Type.Missing, Type.Missing);
		Clipboard.Clear();
		worksheet.Columns.AutoFit();
		((_Worksheet)worksheet).get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, worksheet.UsedRange.Columns.Count]).Font.Bold = true;
		if (File.Exists(ExportedFileFullPath))
		{
			File.Delete(ExportedFileFullPath);
		}
		worksheet.SaveAs(ExportedFileFullPath, XlFileFormat.xlExcel8, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
		workbook.Close(Type.Missing, Type.Missing, Type.Missing);
		((_Application)applicationClass).Quit();
	}

	private void WriteToXML(string serialized)
	{
		using FileStream output = new FileStream(ExportedFileFullPath, FileMode.Create);
		XmlWriterSettings settings = new XmlWriterSettings
		{
			Encoding = Encoding.UTF8,
			NamespaceHandling = NamespaceHandling.OmitDuplicates,
			Indent = true,
			OmitXmlDeclaration = false
		};
		using XmlWriter w = XmlWriter.Create(output, settings);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(serialized);
		xmlDocument.InsertBefore(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "no"), xmlDocument.DocumentElement);
		xmlDocument.Save(w);
	}

	protected decimal GetPriceByGoodIDAndUnit(int goodID, string unit)
	{
		return Prices.LastOrDefault((PricesDataProvider.DataRow p) => p.MTID == goodID && p.QntUnit.Trim().Equals(unit))?.Price ?? 0m;
	}

	protected string GetBarCodeByGoodIDAndUnit(int goodId, string unit)
	{
		MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(goodId, unit);
		if (barCode != null)
		{
			return barCode.BarCode;
		}
		return string.Empty;
	}

	protected string GetPLUCode(MaterialCacheRow good)
	{
		string exportNextCode = ExportNextCode;
		if (!(exportNextCode == "2"))
		{
			if (exportNextCode == "3")
			{
				return GetBarCodeByGoodIDAndUnit(good.Id, good.Unit);
			}
			return good.MTCode;
		}
		if (good.PLUCode.HasValue)
		{
			return good.PLUCode.Value.ToString();
		}
		return string.Empty;
	}

	protected string GetCaption(MaterialCacheRow good)
	{
		string nameInterpretation = NameInterpretation;
		if (!(nameInterpretation == "1"))
		{
			if (nameInterpretation == "2")
			{
				return good.FullCaption;
			}
			return good.Description;
		}
		return good.Caption;
	}
}
