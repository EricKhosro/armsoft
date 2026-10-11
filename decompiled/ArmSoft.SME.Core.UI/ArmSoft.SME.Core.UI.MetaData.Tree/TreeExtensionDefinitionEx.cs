using System.Collections.Generic;
using System.Xml;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.MetaData.Tree;

namespace ArmSoft.SME.Core.UI.MetaData.Tree;

public class TreeExtensionDefinitionEx : TreeExtensionDefinition
{
	public List<UIElement> Elements { get; set; }

	public TreeExtensionDefinitionEx(string xml, ArmSoft.SME.Common.MetaData.Environment environment)
		: base(xml, environment)
	{
	}

	public TreeExtensionDefinitionEx(XmlElement element, ArmSoft.SME.Common.MetaData.Environment environment)
		: base(element, environment)
	{
	}

	protected override void InnerParse(XmlElement node, ArmSoft.SME.Common.MetaData.Environment env)
	{
		Elements = new List<UIElement>();
		base.InnerParse(node, env);
		ExtensionUIHelper.ParseExtensionUI(node, env, DataItems, Elements, null);
	}

	public override XmlDocument StoreToXMLDocument()
	{
		XmlDocument xmlDocument = base.StoreToXMLDocument();
		ExtensionUIHelper.StoreToXMLDocument(xmlDocument, Elements, null);
		return xmlDocument;
	}

	public override string ToXmlString()
	{
		return StoreToXMLDocument().OuterXml;
	}
}
