using System;
using System.Xml;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Common.MetaData;

public class NumberDataItem : DataItem
{
	public short Length { get; set; }

	public short Precision { get; set; }

	public bool AllowNegativeNumbers { get; set; }

	public bool UseCurrencyFormat { get; set; }

	public decimal OverriddenDefaultValue { get; set; }

	public override string Type => "Number";

	public NumberDataItem()
	{
		Required = false;
		AllowNegativeNumbers = true;
		UseCurrencyFormat = true;
		Length = 16;
		Precision = 2;
		OverriddenDefaultValue = 0m;
	}

	public NumberDataItem(string name, string caption, short length, short precision)
	{
		Name = name;
		Caption = caption;
		Length = length;
		Precision = precision;
		Required = false;
		AllowNegativeNumbers = false;
		UseCurrencyFormat = true;
		OverriddenDefaultValue = 0m;
	}

	public static NumberDataItem Parse(XmlElement node, Environment environment)
	{
		NumberDataItem numberDataItem = new NumberDataItem();
		numberDataItem.InnerParse(node, environment);
		foreach (XmlAttribute attribute in node.Attributes)
		{
			switch (attribute.Name)
			{
			case "Required":
				numberDataItem.Required = Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "Length":
				numberDataItem.Length = environment.ParseShort(attribute.InnerText.Trim());
				break;
			case "Precision":
				numberDataItem.Precision = environment.ParseShort(attribute.InnerText.Trim());
				break;
			case "AllowNegativeNumbers":
				numberDataItem.AllowNegativeNumbers = Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "UseCurrencyFormat":
				numberDataItem.UseCurrencyFormat = Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "DefaultValue":
				numberDataItem.OverriddenDefaultValue = Environment.ParseDecimal(attribute.InnerText.Trim());
				break;
			}
		}
		if (numberDataItem.Length < 1 || numberDataItem.Length > 28)
		{
			throw new RESTException(string.Format(Resources.InvalidNumberLength, numberDataItem.Name, 1, 28));
		}
		if (numberDataItem.Precision < 0 || numberDataItem.Precision > 6)
		{
			throw new RESTException(string.Format(Resources.InvalidNumberPrecision, numberDataItem.Name, 0, 6));
		}
		if (numberDataItem.Precision + 2 > numberDataItem.Length)
		{
			throw new RESTException(string.Format(Resources.InvalidNumberPrecisionForLength, numberDataItem.Name, numberDataItem.Length, numberDataItem.Precision));
		}
		return numberDataItem;
	}

	public override XmlElement ToXml(XmlDocument xmlDoc)
	{
		XmlElement xmlElement = xmlDoc.CreateElement("Number");
		InnerFill(xmlDoc, xmlElement);
		if (Required)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Required")).InnerText = "true";
		}
		xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Length")).InnerText = Length.ToString();
		if (Precision > 0)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Precision")).InnerText = Precision.ToString();
		}
		if (AllowNegativeNumbers)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("AllowNegativeNumbers")).InnerText = "true";
		}
		if (!UseCurrencyFormat)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("UseCurrencyFormat")).InnerText = "false";
		}
		if (OverriddenDefaultValue != 0m)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("DefaultValue")).InnerText = DefaultValue().ToString();
		}
		return xmlElement;
	}

	public override object FromBody(string value)
	{
		decimal result = 0m;
		if (!string.IsNullOrWhiteSpace(value))
		{
			decimal.TryParse(value, out result);
		}
		return result;
	}

	public override string ToBody(object value)
	{
		decimal num = Convert.ToDecimal(value);
		if (num == 0m)
		{
			return "";
		}
		return Convert.ToString(num);
	}

	public override object DefaultValue()
	{
		return OverriddenDefaultValue;
	}

	public override object Assign(object value)
	{
		return Convert.ToDecimal(value);
	}

	public override bool Validate(object value, out string msg)
	{
		decimal num = (decimal)value;
		msg = string.Empty;
		if (Required && num == 0m)
		{
			msg = string.Format(Resources.FieldIsEmpty, Caption);
			return false;
		}
		if (!AllowNegativeNumbers && num < 0m)
		{
			msg = string.Format(Resources.FieldValueNegative, Caption, num);
			return false;
		}
		return true;
	}

	public override string Print(object value)
	{
		decimal value2 = Convert.ToDecimal(value);
		if (UseCurrencyFormat)
		{
			return value2.CurrencyFormat(Length, Precision);
		}
		return value2.NumberFormat(Length, Precision);
	}
}
