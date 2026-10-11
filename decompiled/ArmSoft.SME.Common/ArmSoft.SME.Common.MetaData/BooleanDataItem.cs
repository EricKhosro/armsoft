using System;
using System.Xml;

namespace ArmSoft.SME.Common.MetaData;

public class BooleanDataItem : DataItem
{
	public bool OverriddenDefaultValue { get; set; }

	public override string Type => "Boolean";

	public BooleanDataItem()
	{
		OverriddenDefaultValue = false;
	}

	public BooleanDataItem(string name, string caption)
	{
		Name = name;
		Caption = caption;
		OverriddenDefaultValue = false;
	}

	public static BooleanDataItem Parse(XmlElement node, Environment environment)
	{
		BooleanDataItem booleanDataItem = new BooleanDataItem();
		booleanDataItem.InnerParse(node, environment);
		foreach (XmlAttribute attribute in node.Attributes)
		{
			if (attribute.Name == "DefaultValue")
			{
				booleanDataItem.OverriddenDefaultValue = Environment.ParseBoolean(attribute.InnerText.Trim());
			}
		}
		return booleanDataItem;
	}

	public override XmlElement ToXml(XmlDocument xmlDoc)
	{
		XmlElement xmlElement = xmlDoc.CreateElement("Boolean");
		InnerFill(xmlDoc, xmlElement);
		if (OverriddenDefaultValue)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("DefaultValue")).InnerText = "true";
		}
		return xmlElement;
	}

	public override object FromBody(string value)
	{
		return value == "1";
	}

	public override object DefaultValue()
	{
		return OverriddenDefaultValue;
	}

	public override string ToBody(object value)
	{
		if (Convert.ToBoolean(value))
		{
			return "1";
		}
		return "0";
	}

	public override object Assign(object value)
	{
		return Convert.ToBoolean(value);
	}

	public override bool Validate(object value, out string msg)
	{
		msg = string.Empty;
		return true;
	}

	public override string Print(object value)
	{
		if (Convert.ToBoolean(value))
		{
			return Resources.Yes;
		}
		return Resources.No;
	}
}
