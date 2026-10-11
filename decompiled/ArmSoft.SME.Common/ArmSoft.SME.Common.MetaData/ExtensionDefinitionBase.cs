using System.Collections.Generic;
using System.Xml;
using ArmSoft.SME.Common.Exceptions;

namespace ArmSoft.SME.Common.MetaData;

public abstract class ExtensionDefinitionBase
{
	private const string _dataKey = "Data";

	private const string _xmlNamespace = "xmlns";

	protected abstract string ExtensionType { get; }

	public Dictionary<string, DataItem> DataItems { get; set; }

	public ExtensionDefinitionBase(string xml, Environment environment)
	{
		if (string.IsNullOrWhiteSpace(xml))
		{
			return;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlElement xmlElement = null;
		foreach (object childNode in xmlDocument.ChildNodes)
		{
			if (childNode is XmlElement xmlElement2 && xmlElement2.Name == ExtensionType)
			{
				xmlElement = xmlElement2;
				break;
			}
		}
		if (xmlElement != null)
		{
			InnerParse(xmlElement, environment);
		}
	}

	public ExtensionDefinitionBase(XmlElement element, Environment environment)
	{
		InnerParse(element, environment);
	}

	protected virtual void InnerParse(XmlElement node, Environment environment)
	{
		DataItems = new Dictionary<string, DataItem>();
		ParseAttributes(node);
		foreach (object childNode in node.ChildNodes)
		{
			if (childNode is XmlElement { Name: "Data" } xmlElement)
			{
				ParseData(xmlElement, environment, DataItems);
			}
		}
	}

	protected abstract void ParseAttributes(XmlElement node);

	public static void ParseData(XmlElement node, Environment environment, Dictionary<string, DataItem> dataItems)
	{
		foreach (object childNode in node.ChildNodes)
		{
			if (!(childNode is XmlElement xmlElement))
			{
				continue;
			}
			DataItem dataItem = null;
			switch (xmlElement.Name)
			{
			case "String":
				dataItem = StringDataItem.Parse(xmlElement, environment);
				break;
			case "Number":
				dataItem = NumberDataItem.Parse(xmlElement, environment);
				break;
			case "Date":
				dataItem = DateDataItem.Parse(xmlElement, environment);
				break;
			case "Boolean":
				dataItem = BooleanDataItem.Parse(xmlElement, environment);
				break;
			case "Time":
				dataItem = TimeDataItem.Parse(xmlElement, environment);
				break;
			case "TreeNode":
				dataItem = TreeNodeDataItem.Parse(xmlElement, environment);
				break;
			}
			if (dataItem != null)
			{
				if (dataItems.ContainsKey(dataItem.Name))
				{
					throw new RESTException(string.Format(Resources.DataItemHasMultipleDefinitions, dataItem.Name));
				}
				dataItems.Add(dataItem.Name, dataItem);
			}
		}
	}

	protected abstract void SetAttributes(XmlElement element, XmlDocument xmlDoc);

	public virtual XmlDocument StoreToXMLDocument()
	{
		XmlDocument xmlDocument = new XmlDocument();
		XmlElement xmlElement = xmlDocument.CreateElement(ExtensionType);
		xmlDocument.AppendChild(xmlElement);
		SetAttributes(xmlElement, xmlDocument);
		xmlElement.SetAttribute("xmlns", "http://armsoft.am/META/6");
		if (DataItems != null && DataItems.Count > 0)
		{
			XmlElement xmlElement2 = xmlDocument.CreateElement("Data");
			xmlElement.AppendChild(xmlElement2);
			foreach (DataItem value in DataItems.Values)
			{
				xmlElement2.AppendChild(value.ToXml(xmlDocument));
			}
		}
		return xmlDocument;
	}

	public virtual string ToXmlString()
	{
		return StoreToXMLDocument().OuterXml;
	}
}
