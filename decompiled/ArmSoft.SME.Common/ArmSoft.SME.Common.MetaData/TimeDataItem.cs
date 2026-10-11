using System.Xml;

namespace ArmSoft.SME.Common.MetaData;

public class TimeDataItem : DataItem
{
	public bool ShowSeconds { get; set; }

	public override string Type => "Time";

	public TimeDataItem()
	{
		Required = false;
		ShowSeconds = false;
	}

	public static TimeDataItem Parse(XmlElement node, Environment environment)
	{
		TimeDataItem timeDataItem = new TimeDataItem();
		timeDataItem.InnerParse(node, environment);
		foreach (XmlAttribute attribute in node.Attributes)
		{
			string name = attribute.Name;
			if (!(name == "Required"))
			{
				if (name == "ShowSeconds")
				{
					timeDataItem.ShowSeconds = Environment.ParseBoolean(attribute.InnerText.Trim());
				}
			}
			else
			{
				timeDataItem.Required = Environment.ParseBoolean(attribute.InnerText.Trim());
			}
		}
		return timeDataItem;
	}

	public override XmlElement ToXml(XmlDocument xmlDoc)
	{
		XmlElement xmlElement = xmlDoc.CreateElement("Time");
		InnerFill(xmlDoc, xmlElement);
		if (Required)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Required")).InnerText = "true";
		}
		if (ShowSeconds)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("ShowSeconds")).InnerText = "true";
		}
		return xmlElement;
	}

	public override object FromBody(string value)
	{
		return value;
	}

	public override object DefaultValue()
	{
		return "";
	}

	public override string ToBody(object value)
	{
		return (string)value;
	}

	public override object Assign(object value)
	{
		return value;
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
		return (string)value;
	}
}
