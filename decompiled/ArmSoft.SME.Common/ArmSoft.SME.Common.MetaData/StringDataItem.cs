using System.Xml;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Common.MetaData;

public class StringDataItem : DataItem
{
	public short Length { get; set; }

	public bool LeftAlign { get; set; }

	public bool AddTrailingSpaces { get; set; }

	public string OverriddenDefaultValue { get; set; }

	public override string Type => "String";

	public StringDataItem()
	{
		Required = false;
		Length = 150;
		LeftAlign = true;
		AddTrailingSpaces = false;
		OverriddenDefaultValue = "";
	}

	public StringDataItem(string name, string caption, short length)
	{
		Name = name;
		Caption = caption;
		Length = length;
		Required = false;
		LeftAlign = true;
		AddTrailingSpaces = false;
		OverriddenDefaultValue = "";
	}

	public static StringDataItem Parse(XmlElement node, Environment environment)
	{
		StringDataItem stringDataItem = new StringDataItem();
		stringDataItem.InnerParse(node, environment);
		foreach (XmlAttribute attribute in node.Attributes)
		{
			switch (attribute.Name)
			{
			case "Required":
				stringDataItem.Required = Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "Length":
				stringDataItem.Length = environment.ParseShort(attribute.InnerText.Trim());
				break;
			case "LeftAlign":
				stringDataItem.LeftAlign = Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "AddTrailingSpaces":
				stringDataItem.AddTrailingSpaces = Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "DefaultValue":
				stringDataItem.OverriddenDefaultValue = attribute.InnerText;
				break;
			}
		}
		if (stringDataItem.Length < 1 || stringDataItem.Length > 512)
		{
			throw new RESTException(string.Format(Resources.InvalidStringLength, stringDataItem.Name, 1, (short)512));
		}
		if (stringDataItem.OverriddenDefaultValue.Length > stringDataItem.Length)
		{
			stringDataItem.OverriddenDefaultValue = stringDataItem.OverriddenDefaultValue.Left(stringDataItem.Length);
		}
		return stringDataItem;
	}

	public override XmlElement ToXml(XmlDocument xmlDoc)
	{
		XmlElement xmlElement = xmlDoc.CreateElement("String");
		InnerFill(xmlDoc, xmlElement);
		if (Required)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Required")).InnerText = "true";
		}
		if (!LeftAlign)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("LeftAlign")).InnerText = "false";
		}
		if (AddTrailingSpaces)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("AddTrailingSpaces")).InnerText = "true";
		}
		xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Length")).InnerText = Length.ToString();
		if (!string.IsNullOrWhiteSpace(OverriddenDefaultValue))
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("DefaultValue")).InnerText = OverriddenDefaultValue;
		}
		return xmlElement;
	}

	public override object FromBody(string value)
	{
		return Format(value);
	}

	public string Format(string value)
	{
		if (value == null)
		{
			value = string.Empty;
		}
		if (LeftAlign)
		{
			value = value.TrimStart();
		}
		if (value.Length > Length)
		{
			value = value.Left(Length);
		}
		if (AddTrailingSpaces && value.Length < Length)
		{
			value += new string(' ', Length - value.Length);
		}
		return value;
	}

	public override string ToBody(object value)
	{
		if (value == null)
		{
			return null;
		}
		return ((string)value).TrimEnd();
	}

	public override object DefaultValue()
	{
		return OverriddenDefaultValue;
	}

	public override object Assign(object value)
	{
		if (value == null)
		{
			return string.Empty;
		}
		return Format(value.ToString());
	}

	public override bool Validate(object value, out string msg)
	{
		string value2 = (string)value;
		msg = string.Empty;
		if (Required && string.IsNullOrWhiteSpace(value2))
		{
			msg = string.Format(Resources.FieldIsEmpty, Caption);
			return false;
		}
		return true;
	}

	public override string Print(object value)
	{
		if (value == null)
		{
			return string.Empty;
		}
		return Format(value.ToString()).TrimEnd();
	}
}
