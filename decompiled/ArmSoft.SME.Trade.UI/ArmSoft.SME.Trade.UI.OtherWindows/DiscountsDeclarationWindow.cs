using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using DevExpress.Utils;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

public class DiscountsDeclarationWindow : DialogWindow
{
	private class DiscountsDeclarationRow
	{
		public string MTCode { get; set; } = "";

		public string MTName { get; set; } = "";

		public string Discount { get; set; } = "";

		public DateOnly DateBegin { get; set; }

		public DateOnly DateEnd { get; set; }
	}

	private DocumentGridControl _mtListGrid;

	private TableView _mtListView;

	private readonly BindingList<DiscountsDeclarationRow> _mtList = new BindingList<DiscountsDeclarationRow>();

	public DiscountsDeclarationWindow(DateOnly dateBegin, DateOnly dateEnd, List<Goods.DataRow> selectedGoods)
	{
		foreach (Goods.DataRow selectedGood in selectedGoods)
		{
			DiscountsDeclarationRow item = new DiscountsDeclarationRow
			{
				MTCode = selectedGood.MTCode,
				MTName = selectedGood.Name,
				Discount = selectedGood.Discount.CurrencyFormat(6, 2),
				DateBegin = dateBegin,
				DateEnd = dateEnd
			};
			_mtList.Add(item);
		}
		InnerInitialize();
	}

	private void InnerInitialize()
	{
		Caption = ArmSoft.SME.Trade.Common.Resources.DiscountsDeclaration;
		Name = "DiscountsDeclaration";
		_mtListGrid = AddGridControl("MTList");
		_mtListGrid.AllowAdd = false;
		_mtListGrid.AllowDelete = false;
		_mtListGrid.AllowDuplicate = false;
		_mtListGrid.AllowDrop = false;
		_mtListGrid.ShowButtons = false;
		_mtListGrid.AllowEdit = true;
		_mtListGrid.View.Height = 300.0;
		_mtListGrid.View.AutoWidth = true;
		_mtListGrid.Width = 770.0;
		DocumentGridColumn documentGridColumn = _mtListGrid.AddStringColumn("MTCode", ArmSoft.SME.Common.Resources.Code, 20);
		documentGridColumn.Width = 125.0;
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		_mtListGrid.AddStringColumn("MTName", ArmSoft.SME.Common.Resources.Name, 200).AllowEditing = DefaultBoolean.False;
		_mtListGrid.AddStringColumn("Discount", ArmSoft.SME.Trade.Common.Resources.DiscountDescription, 250).Width = 200.0;
		_mtListGrid.AddDateColumn("DateBegin", ArmSoft.SME.Common.Resources.BeginOfPeriodShort);
		_mtListGrid.AddDateColumn("DateEnd", ArmSoft.SME.Common.Resources.EndOfPeriodShort);
		_mtListGrid.ItemsSource = _mtList;
		_mtListView = _mtListGrid.View;
		_mtListView.AllowMoveColumnToDropArea = false;
		_mtListView.IsColumnMenuEnabled = false;
		_mtListView.AllowColumnMoving = true;
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		base.DialogValidation(args);
		ExportDiscountDeclarationXML();
	}

	private void ExportDiscountDeclarationXML()
	{
		string fileNameFromExportDialog = Settings.CommonCoreHelper.GetFileNameFromExportDialog(".xml", "XML Files (.xml)|*.xml", "DiscountsExport");
		if (!string.IsNullOrWhiteSpace(fileNameFromExportDialog))
		{
			Mouse.OverrideCursor = Cursors.Wait;
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement xmlElement = xmlDocument.CreateElement("SignedData");
			xmlElement.SetAttribute("Version", "1.0");
			XmlElement xmlElement2 = xmlDocument.CreateElement("Data");
			xmlElement.AppendChild(xmlElement2);
			XmlElement xmlElement3 = xmlDocument.CreateElement("Declaration");
			xmlElement3.SetAttribute("Version", "1.0");
			xmlElement3.SetAttribute("Id", "xy");
			xmlElement2.AppendChild(xmlElement3);
			XmlElement xmlElement4 = xmlDocument.CreateElement("Header");
			xmlElement3.AppendChild(xmlElement4);
			XmlElement xmlElement5 = xmlDocument.CreateElement("Sender");
			xmlElement4.AppendChild(xmlElement5);
			XmlElement xmlElement6 = xmlDocument.CreateElement("Tin");
			xmlElement6.InnerText = Settings.ParametersService.TaxCode();
			xmlElement5.AppendChild(xmlElement6);
			XmlElement xmlElement7 = xmlDocument.CreateElement("Body");
			xmlElement3.AppendChild(xmlElement7);
			XmlElement xmlElement8 = xmlDocument.CreateElement("Table");
			xmlElement7.AppendChild(xmlElement8);
			for (int i = 0; i <= _mtList.Count - 1; i++)
			{
				XmlElement xmlElement9 = xmlDocument.CreateElement("ReliefRow");
				xmlElement9.SetAttribute("Index", (i + 1).ToString());
				xmlElement8.AppendChild(xmlElement9);
				xmlElement6 = xmlDocument.CreateElement("goodName");
				xmlElement6.InnerText = _mtList[i].MTName;
				xmlElement9.AppendChild(xmlElement6);
				xmlElement6 = xmlDocument.CreateElement("reliefDescription");
				xmlElement6.InnerText = _mtList[i].Discount;
				xmlElement9.AppendChild(xmlElement6);
				xmlElement6 = xmlDocument.CreateElement("startDate");
				xmlElement6.InnerText = _mtList[i].DateBegin.ToString("yyyy-MM-dd");
				xmlElement9.AppendChild(xmlElement6);
				xmlElement6 = xmlDocument.CreateElement("endDate");
				xmlElement6.InnerText = _mtList[i].DateEnd.ToString("yyyy-MM-dd");
				xmlElement9.AppendChild(xmlElement6);
			}
			xmlDocument.AppendChild(xmlElement);
			Settings.CommonCoreHelper.SaveXMLToFile(xmlDocument, fileNameFromExportDialog);
			Mouse.OverrideCursor = null;
		}
	}
}
