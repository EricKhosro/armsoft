using System.Xml;

namespace ArmSoft.SME.Common.MetaData;

public class AWPNode
{
	public short Id { get; set; }

	public string CaptionId { get; set; }

	public string Caption { get; set; }

	public string TooltipId { get; set; }

	public string Tooltip { get; set; }

	public string Icon { get; set; }

	public AWPCommandType CommandType { get; set; }

	public string Command { get; set; }

	public XmlElement NodeToXml(XmlDocument xmldoc, XmlElement node, short id)
	{
		CommonAttributesAppendToXmlNode(xmldoc, node);
		node.Attributes.Append(xmldoc.CreateAttribute("Id")).InnerText = Id.ToString();
		return node;
	}

	public XmlElement LeafToXml(XmlDocument xmldoc)
	{
		XmlElement xmlElement = xmldoc.CreateElement("AWPLeaf");
		if (CommandType == AWPCommandType.UserReport)
		{
			xmlElement.Attributes.Append(xmldoc.CreateAttribute("CommandType")).InnerText = AWPCommandType.UserReport.ToString();
		}
		else if (CommandType == AWPCommandType.Tree)
		{
			xmlElement.Attributes.Append(xmldoc.CreateAttribute("CommandType")).InnerText = AWPCommandType.Tree.ToString();
		}
		if (!string.IsNullOrWhiteSpace(Command))
		{
			if (short.TryParse(Command, out var result))
			{
				xmlElement.Attributes.Append(xmldoc.CreateAttribute("Command")).InnerText = result.ToString();
			}
			else
			{
				xmlElement.Attributes.Append(xmldoc.CreateAttribute("Command")).InnerText = Command;
			}
		}
		CommonAttributesAppendToXmlNode(xmldoc, xmlElement);
		return xmlElement;
	}

	private void CommonAttributesAppendToXmlNode(XmlDocument xmldoc, XmlElement node)
	{
		if (!string.IsNullOrWhiteSpace(CaptionId))
		{
			node.Attributes.Append(xmldoc.CreateAttribute("CaptionId")).InnerText = CaptionId;
		}
		else if (!string.IsNullOrWhiteSpace(Caption))
		{
			node.Attributes.Append(xmldoc.CreateAttribute("Caption")).InnerText = Caption;
		}
		if (!string.IsNullOrWhiteSpace(TooltipId))
		{
			if (!(TooltipId.Trim() == CaptionId.Trim()))
			{
				node.Attributes.Append(xmldoc.CreateAttribute("TooltipId")).InnerText = TooltipId;
			}
		}
		else if (!string.IsNullOrWhiteSpace(Tooltip) && !(Tooltip.Trim() == Caption.Trim()))
		{
			node.Attributes.Append(xmldoc.CreateAttribute("Tooltip")).InnerText = Tooltip;
		}
		if (!string.IsNullOrWhiteSpace(Icon))
		{
			node.Attributes.Append(xmldoc.CreateAttribute("Icon")).InnerText = Icon;
		}
	}
}
