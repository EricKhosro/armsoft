using System.Collections.Generic;
using System.Xml;
using ArmSoft.SME.Common.MetaData;

namespace ArmSoft.SME.Core.UI.MetaData;

public class DataViewDefinition
{
	public string Name { get; set; }

	public string CaptionId { get; set; }

	public string Caption { get; set; }

	public string Group { get; set; }

	public bool System { get; set; }

	public bool IsEnabled { get; set; }

	public bool IsDefault { get; set; }

	public bool PreserveColumns { get; set; }

	public ViewLoadIndication Indication { get; set; }

	public string Script { get; set; }

	public Dictionary<string, CalculatedColumn> CalculatedColumns { get; set; }

	public List<ViewColumn> ViewColumns { get; set; }

	public List<ColumnOrder> SortOrder { get; set; }

	public DataViewDialogExtension DialogExtension { get; set; }

	public XmlDocument StoreToXMLDocument()
	{
		XmlDocument xmlDocument = new XmlDocument();
		XmlNode xmlNode = xmlDocument.AppendChild(xmlDocument.CreateElement("View"));
		xmlNode.Attributes.Append(xmlDocument.CreateAttribute("Name")).InnerText = Name;
		xmlNode.Attributes.Append(xmlDocument.CreateAttribute("CaptionId")).InnerText = CaptionId;
		xmlNode.Attributes.Append(xmlDocument.CreateAttribute("Group")).InnerText = Group;
		if (System)
		{
			xmlNode.Attributes.Append(xmlDocument.CreateAttribute("System")).InnerText = "true";
		}
		if (!IsEnabled)
		{
			xmlNode.Attributes.Append(xmlDocument.CreateAttribute("IsEnabled")).InnerText = "false";
		}
		if (!PreserveColumns)
		{
			xmlNode.Attributes.Append(xmlDocument.CreateAttribute("PreserveColumns")).InnerText = "false";
		}
		if (Indication != ViewLoadIndication.Default)
		{
			xmlNode.Attributes.Append(xmlDocument.CreateAttribute("Indication")).InnerText = Indication.ToString();
		}
		if (DialogExtension != null)
		{
			XmlNode xmlNode2 = xmlNode.AppendChild(xmlDocument.CreateElement("DialogExtension"));
			if (DialogExtension.DataItems != null && DialogExtension.DataItems.Count > 0)
			{
				XmlNode xmlNode3 = xmlNode2.AppendChild(xmlDocument.CreateElement("Data"));
				foreach (DataItem value in DialogExtension.DataItems.Values)
				{
					xmlNode3.AppendChild(value.ToXml(xmlDocument));
				}
			}
			if (DialogExtension.Elements != null && DialogExtension.Elements.Count > 0)
			{
				XmlNode xmlNode4 = xmlNode2.AppendChild(xmlDocument.CreateElement("UI"));
				foreach (UIElement element in DialogExtension.Elements)
				{
					xmlNode4.AppendChild(element.ToXml(xmlDocument));
				}
			}
		}
		if (CalculatedColumns.Count > 0)
		{
			XmlNode xmlNode5 = xmlNode.AppendChild(xmlDocument.CreateElement("CalculatedColumns"));
			foreach (CalculatedColumn value2 in CalculatedColumns.Values)
			{
				xmlNode5.AppendChild(value2.ToXml(xmlDocument));
			}
		}
		if (ViewColumns.Count > 0)
		{
			XmlNode xmlNode6 = xmlNode.AppendChild(xmlDocument.CreateElement("ViewColumns"));
			foreach (ViewColumn viewColumn in ViewColumns)
			{
				XmlElement xmlElement = null;
				switch (viewColumn.Action)
				{
				case ViewColumnAction.Add:
					xmlElement = xmlDocument.CreateElement("AddColumn");
					break;
				case ViewColumnAction.Modify:
					xmlElement = xmlDocument.CreateElement("ModifyColumn");
					break;
				case ViewColumnAction.Remove:
					xmlElement = xmlDocument.CreateElement("RemoveColumn");
					break;
				}
				xmlElement.Attributes.Append(xmlDocument.CreateAttribute("Name")).InnerText = viewColumn.Name;
				if (viewColumn.Action != ViewColumnAction.Remove)
				{
					if (!string.IsNullOrWhiteSpace(viewColumn.CaptionId))
					{
						xmlElement.Attributes.Append(xmlDocument.CreateAttribute("CaptionId")).InnerText = viewColumn.CaptionId;
					}
					else if (!string.IsNullOrWhiteSpace(viewColumn.Caption))
					{
						xmlElement.Attributes.Append(xmlDocument.CreateAttribute("Caption")).InnerText = viewColumn.Caption;
					}
					if (!string.IsNullOrWhiteSpace(viewColumn.Format))
					{
						xmlElement.Attributes.Append(xmlDocument.CreateAttribute("Format")).InnerText = viewColumn.Format;
					}
					if (viewColumn.FooterSum)
					{
						xmlElement.Attributes.Append(xmlDocument.CreateAttribute("FooterSum")).InnerText = "true";
					}
					if (!viewColumn.Visible)
					{
						xmlElement.Attributes.Append(xmlDocument.CreateAttribute("Visible")).InnerText = "false";
					}
					if (!string.IsNullOrWhiteSpace(viewColumn.After))
					{
						xmlElement.Attributes.Append(xmlDocument.CreateAttribute("After")).InnerText = viewColumn.After;
					}
					if (!string.IsNullOrWhiteSpace(viewColumn.Before))
					{
						xmlElement.Attributes.Append(xmlDocument.CreateAttribute("Before")).InnerText = viewColumn.Before;
					}
				}
				xmlNode6.AppendChild(xmlElement);
			}
		}
		if (SortOrder.Count > 0)
		{
			XmlNode xmlNode7 = xmlNode.AppendChild(xmlDocument.CreateElement("SortOrder"));
			foreach (ColumnOrder item in SortOrder)
			{
				XmlElement xmlElement2 = xmlDocument.CreateElement("ColumnOrder");
				xmlElement2.Attributes.Append(xmlDocument.CreateAttribute("Name")).InnerText = item.Name;
				if (item.Descending)
				{
					xmlElement2.Attributes.Append(xmlDocument.CreateAttribute("Descending")).InnerText = "true";
				}
				xmlNode7.AppendChild(xmlElement2);
			}
		}
		if (!string.IsNullOrWhiteSpace(Script))
		{
			XmlNode? xmlNode8 = xmlNode.AppendChild(xmlDocument.CreateElement("script"));
			XmlCDataSection newChild = xmlDocument.CreateCDataSection(Script);
			xmlNode8.AppendChild(newChild);
		}
		return xmlDocument;
	}
}
