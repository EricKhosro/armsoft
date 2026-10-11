using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ArmSoft.SME.Common.MetaData;

namespace ArmSoft.SME.Core.UI.MetaData;

public class DocumentExtensionDefinition : ArmSoft.SME.Common.MetaData.DocumentExtensionDefinition
{
	public List<UIElement> Elements { get; set; }

	public List<UITab> Tabs { get; set; }

	public DocumentExtensionDefinition(string xml, ArmSoft.SME.Common.MetaData.Environment environment)
		: base(xml, environment)
	{
	}

	public DocumentExtensionDefinition(XmlElement element, ArmSoft.SME.Common.MetaData.Environment environment)
		: base(element, environment)
	{
	}

	protected override void InnerParse(XmlElement node, ArmSoft.SME.Common.MetaData.Environment env)
	{
		Elements = new List<UIElement>();
		Tabs = new List<UITab>();
		base.InnerParse(node, env);
		ExtensionUIHelper.ParseExtensionUI(node, env, DataItems, Elements, Tabs);
	}

	public override XmlDocument StoreToXMLDocument()
	{
		XmlDocument xmlDocument = base.StoreToXMLDocument();
		ExtensionUIHelper.StoreToXMLDocument(xmlDocument, Elements, Tabs);
		return xmlDocument;
	}

	public override string ToXmlString()
	{
		return StoreToXMLDocument().OuterXml;
	}

	public List<UIField> GetUIFields()
	{
		List<UIField> uIFields = GetUIFields(Elements);
		foreach (UITab tab in Tabs)
		{
			uIFields.AddRange(GetUIFields(tab.Elements));
		}
		return uIFields;
	}

	public UIField GetUIField(string filedName)
	{
		return GetUIFields().FirstOrDefault((UIField p) => p.Name == filedName);
	}

	private static List<UIField> GetUIFields(List<UIElement> elements)
	{
		List<UIField> list = elements.Where((UIElement el) => el is UIField).Cast<UIField>().ToList();
		foreach (UIGroup item in elements.Where((UIElement el) => el is UIGroup).Cast<UIGroup>())
		{
			list.AddRange(GetUIFields(item.Elements));
		}
		return list;
	}
}
