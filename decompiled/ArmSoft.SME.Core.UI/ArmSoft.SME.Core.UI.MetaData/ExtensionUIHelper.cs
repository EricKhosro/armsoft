using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.MetaData.DocumentExtension;
using ArmSoft.SME.Core.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.MetaData;

internal static class ExtensionUIHelper
{
	private const string _uiKey = "UI";

	private const string _tabKey = "TabedUI";

	internal static void ParseExtensionUI(XmlElement node, ArmSoft.SME.Common.MetaData.Environment env, Dictionary<string, DataItem> dataItems, List<UIElement> elements, List<UITab> tabs)
	{
		foreach (object childNode in node.ChildNodes)
		{
			if (!(childNode is XmlElement xmlElement))
			{
				continue;
			}
			string name = xmlElement.Name;
			if (!(name == "UI"))
			{
				if (name == "TabedUI" && tabs != null)
				{
					ParseTabbedUI(xmlElement, (Environment)env, dataItems, tabs, elements);
				}
			}
			else
			{
				ParseUI(xmlElement, (Environment)env, dataItems, elements);
			}
		}
	}

	private static void ParseTabbedUI(XmlElement node, Environment environment, Dictionary<string, DataItem> dataItems, List<UITab> tabs, List<UIElement> elements)
	{
		foreach (object childNode in node.ChildNodes)
		{
			if (childNode is XmlElement { Name: "UITab" } xmlElement)
			{
				tabs.Add(UITab.Parse(xmlElement, environment, dataItems));
			}
		}
	}

	internal static void ParseUI(XmlElement node, Environment environment, Dictionary<string, DataItem> dataItems, List<UIElement> elements)
	{
		foreach (object childNode in node.ChildNodes)
		{
			if (!(childNode is XmlElement xmlElement))
			{
				continue;
			}
			UIElement uIElement = null;
			string name = xmlElement.Name;
			if (!(name == "UIField"))
			{
				if (name == "UIGroup")
				{
					uIElement = UIGroup.Parse(xmlElement, environment, dataItems);
				}
			}
			else
			{
				uIElement = UIField.Parse(xmlElement, environment, dataItems);
			}
			if (uIElement != null)
			{
				elements.Add(uIElement);
			}
		}
	}

	internal static void StoreToXMLDocument(XmlDocument xmlDoc, List<UIElement> elements, List<UITab> tabs)
	{
		XmlElement documentElement = xmlDoc.DocumentElement;
		if (elements != null && elements.Count > 0)
		{
			XmlElement xmlElement = xmlDoc.CreateElement("UI");
			documentElement.AppendChild(xmlElement);
			foreach (UIElement element in elements)
			{
				xmlElement.AppendChild(element.ToXml(xmlDoc));
			}
		}
		if (tabs == null || tabs.Count <= 0)
		{
			return;
		}
		XmlElement xmlElement2 = xmlDoc.CreateElement("TabedUI");
		documentElement.AppendChild(xmlElement2);
		foreach (UITab tab in tabs)
		{
			xmlElement2.AppendChild(tab.ToXml(xmlDoc));
		}
	}

	internal static void AddExtensionColumns(ViewColumnsCollection columns, UIElement uiElement)
	{
		if (uiElement is UIGroup uIGroup)
		{
			{
				foreach (UIElement element in uIGroup.Elements)
				{
					AddExtensionColumns(columns, element);
				}
				return;
			}
		}
		if (uiElement is UIField { DataItem: var dataItem })
		{
			DataColumnType type = DataColumnType.Unknown;
			if (dataItem is NumberDataItem)
			{
				type = DataColumnType.Decimal;
			}
			else if (dataItem is DateDataItem)
			{
				type = DataColumnType.DateOnly;
			}
			else if (dataItem is BooleanDataItem)
			{
				type = DataColumnType.Boolean;
			}
			else if (dataItem is StringDataItem || dataItem is TreeNodeDataItem || dataItem is TimeDataItem)
			{
				type = DataColumnType.String;
			}
			ArmSoft.SME.Core.UI.DataViews.ViewColumn viewColumn = columns.AddDynamic(dataItem.Name, dataItem.Caption, dataItem.Name, type, footerSum: false, "", "", isExtended: true);
			if (dataItem is DateDataItem)
			{
				viewColumn.Format = "dd/MM/yy HH:mm:ss";
			}
			else if (dataItem is NumberDataItem numberDataItem)
			{
				viewColumn.Format = DecimalExtension.GetCurrencyFormat(numberDataItem.Length, numberDataItem.Precision);
			}
		}
	}

	internal static DialogWindow CreateDocumentExtensionMultipleFieldsEditDialog(DocumentExtensionDefinition extensionDefinition)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = Resources.EditUserDefinedFields,
			Name = "EditSelectedExtensionFields",
			MaxHeight = Screen.PrimaryScreen.WorkingArea.Height
		};
		foreach (UIField uIField in extensionDefinition.GetUIFields())
		{
			DataItem dataItem = uIField.DataItem;
			if (dataItem is StringDataItem stringDataItem)
			{
				DropDown dropDownItem = uIField.DropDownItem;
				if (dropDownItem == null)
				{
					DialogChangeDemandControls.AddChangeDemandStringControl(dialogWindow, dataItem.Name, dataItem.Caption, stringDataItem.Length, stringDataItem.Required, uIField.StoreValue);
					continue;
				}
				IDropDownView idv = Settings.ServiceProvider.GetRequiredService<IDropDownService>().CreateDropDown(dropDownItem.Group);
				if (dropDownItem.IsDropDownComment)
				{
					DialogChangeDemandControls.AddChangeDemandDropDownCommentControl(dialogWindow, dataItem.Name, dataItem.Caption, idv, stringDataItem.Required, uIField.StoreValue, 50);
				}
				else if (dropDownItem.IsMultiSelect)
				{
					DialogChangeDemandControls.AddChangeDemandMultiSelectDropDownControl(dialogWindow, dataItem.Name, dataItem.Caption, idv, stringDataItem.Required, uIField.StoreValue);
				}
				else
				{
					DialogChangeDemandControls.AddChangeDemandDropDownControl(dialogWindow, dataItem.Name, dataItem.Caption, idv, stringDataItem.Required, uIField.StoreValue);
				}
			}
			else if (dataItem is BooleanDataItem)
			{
				DialogChangeDemandControls.AddChangeDemandBooleanControl(dialogWindow, dataItem.Name, dataItem.Caption, uIField.StoreValue);
			}
			else if (dataItem is NumberDataItem numberDataItem)
			{
				DialogChangeDemandControls.AddChangeDemandNumericControl(dialogWindow, dataItem.Name, dataItem.Caption, numberDataItem.Length, numberDataItem.Precision, numberDataItem.Required, numberDataItem.AllowNegativeNumbers, numberDataItem.UseCurrencyFormat, uIField.StoreValue);
			}
			else if (dataItem is DateDataItem dateDataItem)
			{
				DialogChangeDemandControls.AddChangeDemandDateEditControl(dialogWindow, dataItem.Name, dataItem.Caption, dateDataItem.UseLongDate, dateDataItem.Required, uIField.StoreValue);
			}
			else if (dataItem is TimeDataItem timeDataItem)
			{
				DialogChangeDemandControls.AddChangeDemandTimeControl(dialogWindow, dataItem.Name, dataItem.Caption, timeDataItem.ShowSeconds, timeDataItem.Required, uIField.StoreValue);
			}
			else if (dataItem is TreeNodeDataItem treeNodeDataItem)
			{
				TreeNodes idv2 = new TreeNodes(treeNodeDataItem.Tree.Id);
				DialogChangeDemandControls.AddChangeDemandDropDownControl(dialogWindow, dataItem.Name, dataItem.Caption, idv2, treeNodeDataItem.Required, uIField.StoreValue);
			}
		}
		return dialogWindow;
	}

	public static void CreatePOExtensionBySimpleWindow(POTypeSE poTypeDef, short poType, Window owner = null)
	{
		if (!Settings.LoginService.IsAdmin)
		{
			throw new ASException(Resources.AdminHaveAccessForWork);
		}
		IDocumentExtensionService requiredService = Settings.ServiceProvider.GetRequiredService<IDocumentExtensionService>();
		DocumentExtensionModel documentExtensionModel = requiredService.Load(new DocumentExtensionRequest(poTypeDef, poType));
		DocumentExtensionsWindowSimple documentExtensionsWindowSimple = new DocumentExtensionsWindowSimple((documentExtensionModel == null) ? DialogWindowMode.NewMode : DialogWindowMode.EditMode, poTypeDef, poType, documentExtensionModel, owner);
		if (documentExtensionsWindowSimple.ShowDialog().Value)
		{
			DocumentExtensionModel documentDefinition = new DocumentExtensionModel
			{
				POTypeDef = documentExtensionsWindowSimple.DocumentExtensionDefinition.POTypeDef,
				POType = documentExtensionsWindowSimple.DocumentExtensionDefinition.POType,
				ExtensionXml = documentExtensionsWindowSimple.DocumentExtensionDefinition.ToXmlString()
			};
			requiredService.Store(documentDefinition);
		}
	}
}
