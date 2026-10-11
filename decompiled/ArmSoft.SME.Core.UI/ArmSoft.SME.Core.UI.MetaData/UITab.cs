using System.Collections.Generic;
using System.Xml;
using ArmSoft.SME.Common.MetaData;

namespace ArmSoft.SME.Core.UI.MetaData;

public class UITab : UIElement
{
	public List<UIElement> Elements { get; set; }

	public UITab()
	{
		Elements = new List<UIElement>();
	}

	public static UITab Parse(XmlElement node, Environment env, Dictionary<string, DataItem> dataItems)
	{
		UITab uITab = new UITab();
		foreach (XmlAttribute attribute in node.Attributes)
		{
			string name = attribute.Name;
			if (!(name == "CaptionId"))
			{
				if (name == "Caption")
				{
					uITab.Caption = attribute.InnerText.Trim();
				}
			}
			else
			{
				uITab.CaptionId = attribute.InnerText.Trim();
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
					uIElement = UIGroup.Parse(xmlElement, env, dataItems);
				}
			}
			else
			{
				uIElement = UIField.Parse(xmlElement, env, dataItems);
			}
			if (uIElement != null)
			{
				uITab.Elements.Add(uIElement);
			}
		}
		if (!string.IsNullOrWhiteSpace(uITab.CaptionId))
		{
			uITab.Caption = env.GetStringResource(uITab.CaptionId);
		}
		return uITab;
	}

	public override XmlElement ToXml(XmlDocument xmlDoc)
	{
		XmlElement xmlElement = xmlDoc.CreateElement("UITab");
		if (!string.IsNullOrWhiteSpace(CaptionId))
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("CaptionId")).InnerText = CaptionId;
		}
		else if (!string.IsNullOrWhiteSpace(Caption))
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Caption")).InnerText = Caption;
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
