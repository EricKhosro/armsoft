using System;
using System.Xml;
using ArmSoft.SME.Common;

namespace ArmSoft.SME.Core.UI.MetaData;

public class CalculatedColumn
{
	public string Name { get; set; }

	public string CaptionId { get; set; }

	public string Caption { get; set; }

	public string Expression { get; set; }

	public ColumnType Type { get; set; }

	public string Format { get; set; }

	public string Source { get; set; }

	public static CalculatedColumn Parse(XmlElement node, Environment environment)
	{
		CalculatedColumn calculatedColumn = new CalculatedColumn();
		foreach (XmlAttribute attribute in node.Attributes)
		{
			switch (attribute.Name)
			{
			case "Name":
				calculatedColumn.Name = attribute.InnerText.Trim();
				break;
			case "CaptionId":
				calculatedColumn.CaptionId = attribute.InnerText.Trim();
				break;
			case "Caption":
				calculatedColumn.Caption = attribute.InnerText.Trim();
				break;
			case "Format":
				calculatedColumn.Format = attribute.InnerText.Trim();
				break;
			case "Source":
				calculatedColumn.Source = attribute.InnerText.Trim();
				break;
			case "Type":
				calculatedColumn.Type = (ColumnType)Enum.Parse(typeof(ColumnType), attribute.InnerText.Trim());
				break;
			}
		}
		foreach (object childNode in node.ChildNodes)
		{
			if (childNode is XmlElement { Name: "Expression" } xmlElement)
			{
				calculatedColumn.Expression = xmlElement.InnerText.Trim();
			}
		}
		if (!environment.IsIdentifier(calculatedColumn.Name))
		{
			throw new ASException(string.Format(Resources.InvalidIdentifier, calculatedColumn.Name));
		}
		if (string.IsNullOrWhiteSpace(calculatedColumn.CaptionId))
		{
			if (string.IsNullOrWhiteSpace(calculatedColumn.Caption))
			{
				calculatedColumn.Caption = calculatedColumn.Name;
			}
		}
		else
		{
			if (!environment.IsIdentifier(calculatedColumn.CaptionId))
			{
				throw new ASException(string.Format(Resources.InvalidIdentifier, calculatedColumn.CaptionId));
			}
			calculatedColumn.Caption = environment.GetStringResource(calculatedColumn.CaptionId);
		}
		return calculatedColumn;
	}

	public XmlElement ToXml(XmlDocument xmlDoc)
	{
		XmlElement xmlElement = xmlDoc.CreateElement("CalculatedColumn");
		xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Name")).InnerText = Name;
		if (!string.IsNullOrWhiteSpace(CaptionId))
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("CaptionId")).InnerText = CaptionId;
		}
		else if (!string.IsNullOrWhiteSpace(Caption))
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Caption")).InnerText = Caption;
		}
		xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Type")).InnerText = Type.ToString();
		if (!string.IsNullOrWhiteSpace(Format))
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Format")).InnerText = Format;
		}
		if (!string.IsNullOrWhiteSpace(Source))
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Source")).InnerText = Source;
		}
		if (!string.IsNullOrWhiteSpace(Expression))
		{
			xmlElement.AppendChild(xmlDoc.CreateElement("Expression")).InnerText = Expression;
		}
		return xmlElement;
	}
}
