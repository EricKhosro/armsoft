using System;
using System.Xml;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Common.MetaData;

public class DateDataItem : DataItem
{
	public bool UseLongDate { get; set; }

	public override string Type => "Date";

	public DateDataItem()
	{
		Required = false;
		UseLongDate = false;
	}

	public static DateDataItem Parse(XmlElement node, Environment environment)
	{
		DateDataItem dateDataItem = new DateDataItem();
		dateDataItem.InnerParse(node, environment);
		foreach (XmlAttribute attribute in node.Attributes)
		{
			string name = attribute.Name;
			if (!(name == "Required"))
			{
				if (name == "UseLongDate")
				{
					dateDataItem.UseLongDate = Environment.ParseBoolean(attribute.InnerText.Trim());
				}
			}
			else
			{
				dateDataItem.Required = Environment.ParseBoolean(attribute.InnerText.Trim());
			}
		}
		return dateDataItem;
	}

	public override XmlElement ToXml(XmlDocument xmlDoc)
	{
		XmlElement xmlElement = xmlDoc.CreateElement("Date");
		InnerFill(xmlDoc, xmlElement);
		if (Required)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Required")).InnerText = "true";
		}
		if (UseLongDate)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("UseLongDate")).InnerText = "true";
		}
		return xmlElement;
	}

	public override object FromBody(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return null;
		}
		if (value == "00000000")
		{
			return null;
		}
		return value.YYYYMMDDToDate();
	}

	public override object DefaultValue()
	{
		return null;
	}

	public override string ToBody(object value)
	{
		if (value == null)
		{
			return "";
		}
		DateOnly? value2 = null;
		if (value is DateOnly value3)
		{
			value2 = value3;
		}
		else if (!string.IsNullOrWhiteSpace(value?.ToString()))
		{
			value2 = Convert.ToDateTime(value).ToDateOnly();
		}
		if (value2.HasValue)
		{
			return value2.FormatYYYYMMDD();
		}
		return "";
	}

	public override object Assign(object value)
	{
		DateOnly? dateOnly = ((!(value is DateOnly value2)) ? ((DateOnly?)value) : new DateOnly?(value2));
		if (dateOnly.HasValue)
		{
			dateOnly = new DateOnly(dateOnly.Value.Year, dateOnly.Value.Month, dateOnly.Value.Day);
		}
		return dateOnly;
	}

	public override bool Validate(object value, out string msg)
	{
		msg = string.Empty;
		DateOnly? dateOnly = ((!(value is DateOnly value2)) ? ((DateOnly?)value) : new DateOnly?(value2));
		if (Required && !dateOnly.HasValue)
		{
			msg = string.Format(Resources.FieldIsEmpty, Caption);
			return false;
		}
		return true;
	}

	public override string Print(object value)
	{
		DateOnly? value2 = ((!(value is DateOnly value3)) ? ((DateOnly?)value) : new DateOnly?(value3));
		if (value2.HasValue)
		{
			value2 = new DateOnly(value2.Value.Year, value2.Value.Month, value2.Value.Day);
		}
		if (value2.HasValue)
		{
			if (UseLongDate)
			{
				return value2.FormatDDMMYYYY();
			}
			return value2.FormatDDMMYY();
		}
		return "";
	}
}
