using System.Xml;

namespace ArmSoft.SME.Common.MetaData;

public class DocumentExtensionDefinition : ExtensionDefinitionBase
{
	public short POTypeDef { get; set; }

	public short POType { get; set; }

	protected override string ExtensionType => "DocumentExtension";

	public DocumentExtensionDefinition(string xml, Environment environment)
		: base(xml, environment)
	{
	}

	public DocumentExtensionDefinition(XmlElement element, Environment environment)
		: base(element, environment)
	{
	}

	protected override void ParseAttributes(XmlElement node)
	{
		foreach (XmlAttribute attribute in node.Attributes)
		{
			string name = attribute.Name;
			if (!(name == "POTypeDef"))
			{
				if (name == "POType")
				{
					POType = short.Parse(attribute.InnerText.Trim());
				}
			}
			else
			{
				POTypeDef = short.Parse(attribute.InnerText.Trim());
			}
		}
	}

	protected override void SetAttributes(XmlElement element, XmlDocument xmlDoc)
	{
		element.Attributes.Append(xmlDoc.CreateAttribute("POTypeDef")).InnerText = POTypeDef.ToString();
		element.Attributes.Append(xmlDoc.CreateAttribute("POType")).InnerText = POType.ToString();
	}
}
