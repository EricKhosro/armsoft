using System.Xml;
using ArmSoft.SME.Common.Exceptions;

namespace ArmSoft.SME.Common.MetaData;

public abstract class DataItem
{
	public string Name { get; set; }

	public string CaptionId { get; set; }

	public string Caption { get; set; }

	public bool Required { get; set; }

	public string Expression { get; set; }

	public bool SaveValueOnCopy { get; set; }

	public string Source { get; set; }

	public string ValidatorName { get; set; }

	public abstract string Type { get; }

	public bool IsCalculated
	{
		get
		{
			if (string.IsNullOrWhiteSpace(Source))
			{
				return !string.IsNullOrWhiteSpace(Expression);
			}
			return true;
		}
	}

	public abstract XmlElement ToXml(XmlDocument xmlDoc);

	public abstract object FromBody(string value);

	public abstract object DefaultValue();

	public abstract string ToBody(object value);

	public abstract object Assign(object value);

	public abstract bool Validate(object value, out string msg);

	public abstract string Print(object value);

	protected void InnerParse(XmlElement node, Environment environment)
	{
		foreach (XmlAttribute attribute in node.Attributes)
		{
			switch (attribute.Name)
			{
			case "Name":
				Name = attribute.InnerText.Trim();
				break;
			case "CaptionId":
				CaptionId = attribute.InnerText.Trim();
				break;
			case "Caption":
				Caption = attribute.InnerText.Trim();
				break;
			case "Validator":
				ValidatorName = attribute.InnerText.Trim();
				break;
			case "SaveValueOnCopy":
				SaveValueOnCopy = Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "Source":
				Source = attribute.InnerText.Trim();
				break;
			}
		}
		foreach (object childNode in node.ChildNodes)
		{
			if (childNode is XmlElement { Name: "Expression" } xmlElement)
			{
				Expression = xmlElement.InnerText.Trim();
			}
		}
		if (!environment.IsIdentifier(Name))
		{
			throw new RESTException(string.Format(Resources.InvalidIdentifier, Name));
		}
		if (string.IsNullOrWhiteSpace(CaptionId))
		{
			if (string.IsNullOrWhiteSpace(Caption))
			{
				Caption = Name;
			}
		}
		else if (!environment.IsIdentifier(CaptionId))
		{
			Caption = CaptionId;
		}
		else
		{
			Caption = environment.GetStringResource(CaptionId);
		}
	}

	protected void InnerFill(XmlDocument xmlDoc, XmlElement xmlEl)
	{
		xmlEl.Attributes.Append(xmlDoc.CreateAttribute("Name")).InnerText = Name;
		if (!string.IsNullOrWhiteSpace(CaptionId))
		{
			xmlEl.Attributes.Append(xmlDoc.CreateAttribute("CaptionId")).InnerText = CaptionId;
		}
		else if (!string.IsNullOrWhiteSpace(Caption))
		{
			xmlEl.Attributes.Append(xmlDoc.CreateAttribute("Caption")).InnerText = Caption;
		}
		if (!string.IsNullOrWhiteSpace(ValidatorName))
		{
			xmlEl.Attributes.Append(xmlDoc.CreateAttribute("Validator")).InnerText = ValidatorName;
		}
		if (!string.IsNullOrWhiteSpace(Source))
		{
			xmlEl.Attributes.Append(xmlDoc.CreateAttribute("Source")).InnerText = Source;
		}
		if (SaveValueOnCopy)
		{
			xmlEl.Attributes.Append(xmlDoc.CreateAttribute("SaveValueOnCopy")).InnerText = "true";
		}
		if (!string.IsNullOrWhiteSpace(Expression))
		{
			xmlEl.AppendChild(xmlDoc.CreateElement("Expression")).InnerText = Expression;
		}
	}
}
