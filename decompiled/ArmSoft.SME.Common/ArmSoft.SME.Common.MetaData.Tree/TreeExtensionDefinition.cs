using System.Xml;

namespace ArmSoft.SME.Common.MetaData.Tree;

public class TreeExtensionDefinition : ExtensionDefinitionBase
{
	public string Name { get; set; }

	protected override string ExtensionType => "TreeExtension";

	public TreeExtensionDefinition(string xml, Environment environment)
		: base(xml, environment)
	{
	}

	public TreeExtensionDefinition(XmlElement element, Environment environment)
		: base(element, environment)
	{
	}

	protected override void ParseAttributes(XmlElement node)
	{
		foreach (XmlAttribute attribute in node.Attributes)
		{
			if (attribute.Name == "Name")
			{
				Name = attribute.InnerText.Trim();
			}
		}
	}

	protected override void SetAttributes(XmlElement element, XmlDocument xmlDoc)
	{
		element.Attributes.Append(xmlDoc.CreateAttribute("Name")).InnerText = Name;
	}
}
