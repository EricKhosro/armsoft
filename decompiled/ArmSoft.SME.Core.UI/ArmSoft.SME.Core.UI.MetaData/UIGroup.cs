using System.Collections.Generic;
using System.Xml;
using ArmSoft.SME.Common.MetaData;

namespace ArmSoft.SME.Core.UI.MetaData;

public class UIGroup : UIElement
{
	public string Name { get; set; }

	public bool IsHorizontal { get; set; }

	public bool IsCollapsible { get; set; }

	public bool IsCollapsed { get; set; }

	public List<UIElement> Elements { get; set; }

	public UIGroup()
	{
		Elements = new List<UIElement>();
		IsHorizontal = true;
		IsCollapsible = false;
		IsCollapsed = false;
	}

	public static UIGroup Parse(XmlElement node, Environment environment, Dictionary<string, DataItem> dataItems)
	{
		UIGroup uIGroup = new UIGroup();
		foreach (XmlAttribute attribute in node.Attributes)
		{
			switch (attribute.Name)
			{
			case "Name":
				uIGroup.Name = attribute.InnerText.Trim();
				break;
			case "CaptionId":
				uIGroup.CaptionId = attribute.InnerText.Trim();
				break;
			case "Caption":
				uIGroup.Caption = attribute.InnerText.Trim();
				break;
			case "isHorisontal":
				uIGroup.IsHorizontal = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "IsCollapsible":
				uIGroup.IsCollapsible = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "IsCollapsed":
				uIGroup.IsCollapsed = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			}
		}
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
					uIElement = Parse(xmlElement, environment, dataItems);
				}
			}
			else
			{
				uIElement = UIField.Parse(xmlElement, environment, dataItems);
			}
			if (uIElement != null)
			{
				uIGroup.Elements.Add(uIElement);
			}
		}
		if (!string.IsNullOrWhiteSpace(uIGroup.CaptionId))
		{
			uIGroup.Caption = environment.GetStringResource(uIGroup.CaptionId);
		}
		return uIGroup;
	}

	public override XmlElement ToXml(XmlDocument xmlDoc)
	{
		XmlElement xmlElement = xmlDoc.CreateElement("UIGroup");
		if (!string.IsNullOrWhiteSpace(Name))
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Name")).InnerText = Name;
		}
		if (!string.IsNullOrWhiteSpace(CaptionId))
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("CaptionId")).InnerText = CaptionId;
		}
		else if (!string.IsNullOrWhiteSpace(Caption))
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Caption")).InnerText = Caption;
		}
		if (!IsHorizontal)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("isHorisontal")).InnerText = "false";
		}
		if (IsCollapsible)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("IsCollapsible")).InnerText = "true";
		}
		if (IsCollapsed)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("IsCollapsed")).InnerText = "true";
		}
		if (Elements.Count > 0)
		{
			foreach (UIElement element in Elements)
			{
				xmlElement.AppendChild(element.ToXml(xmlDoc));
			}
		}
		return xmlElement;
	}
}
