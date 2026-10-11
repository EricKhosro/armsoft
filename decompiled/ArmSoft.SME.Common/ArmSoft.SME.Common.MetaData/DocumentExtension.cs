using System.Collections.Generic;
using System.IO;
using System.Xml;
using ArmSoft.SME.Common.Exceptions;

namespace ArmSoft.SME.Common.MetaData;

public class DocumentExtension
{
	private Dictionary<string, DataItem> _dataItems;

	public Dictionary<string, object> Values { get; set; }

	public object this[string name]
	{
		get
		{
			if (_dataItems == null)
			{
				throw new RESTException(string.Format(Resources.DataItemNotExists, name));
			}
			if (Values != null && Values.TryGetValue(name, out var value))
			{
				return value;
			}
			if (_dataItems.TryGetValue(name, out var value2))
			{
				return value2.DefaultValue();
			}
			throw new RESTException(string.Format(Resources.DataItemNotExists, name));
		}
		set
		{
			if (Values == null)
			{
				Dictionary<string, object> dictionary = (Values = new Dictionary<string, object>());
			}
			if (_dataItems == null)
			{
				throw new RESTException(string.Format(Resources.DataItemNotExists, name));
			}
			if (Values.ContainsKey(name))
			{
				Values[name] = _dataItems[name].Assign(value);
				return;
			}
			if (_dataItems.TryGetValue(name, out var value2))
			{
				Values.Add(name, value2.Assign(value));
				return;
			}
			throw new RESTException(string.Format(Resources.DataItemNotExists, name));
		}
	}

	public DocumentExtension(ExtensionDefinitionBase definition)
	{
		if (definition == null)
		{
			_dataItems = null;
		}
		else
		{
			_dataItems = definition.DataItems;
		}
		Values = new Dictionary<string, object>();
	}

	public DocumentExtension(Dictionary<string, DataItem> dataItems)
	{
		_dataItems = dataItems;
		Values = null;
	}

	public void LoadFromXML(string value)
	{
		if (_dataItems == null)
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(value))
		{
			Values = null;
			return;
		}
		if (Values == null)
		{
			Values = new Dictionary<string, object>();
		}
		else
		{
			Values.Clear();
		}
		using StringReader input = new StringReader(value);
		using XmlTextReader xmlTextReader = new XmlTextReader(input);
		xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
		DataItem value2 = null;
		while (xmlTextReader.Read())
		{
			switch (xmlTextReader.NodeType)
			{
			case XmlNodeType.Element:
				if (xmlTextReader.Depth == 1)
				{
					_dataItems.TryGetValue(xmlTextReader.Name, out value2);
				}
				else
				{
					value2 = null;
				}
				break;
			case XmlNodeType.Text:
				if ((xmlTextReader.Depth == 2) & (value2 != null))
				{
					Values.Add(value2.Name, value2.FromBody(xmlTextReader.Value));
				}
				break;
			}
		}
	}

	public string ToXML(Dictionary<string, object> values)
	{
		if (_dataItems == null)
		{
			return "";
		}
		if (Values == null && values != null)
		{
			Values = values;
		}
		using StringWriter stringWriter = new StringWriter();
		XmlWriterSettings settings = new XmlWriterSettings
		{
			Indent = false,
			OmitXmlDeclaration = true
		};
		using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
		{
			xmlWriter.WriteStartElement("data");
			foreach (DataItem value2 in _dataItems.Values)
			{
				string text = null;
				text = ((Values == null || !Values.TryGetValue(value2.Name, out var value)) ? value2.ToBody(value2.DefaultValue()) : value2.ToBody(value));
				if (!string.IsNullOrWhiteSpace(text))
				{
					xmlWriter.WriteStartElement(value2.Name);
					xmlWriter.WriteString(text);
					xmlWriter.WriteEndElement();
				}
			}
			xmlWriter.WriteEndElement();
		}
		return stringWriter.ToString();
	}

	public XmlNode[] ToExchange(string xmlNS)
	{
		if (_dataItems == null)
		{
			return null;
		}
		List<XmlNode> list = new List<XmlNode>(_dataItems.Count);
		XmlDocument xmlDocument = new XmlDocument();
		foreach (DataItem value2 in _dataItems.Values)
		{
			if (!value2.IsCalculated)
			{
				string text = ((Values == null || !Values.TryGetValue(value2.Name, out var value)) ? value2.ToBody(value2.DefaultValue()) : value2.ToBody(value));
				if (!string.IsNullOrWhiteSpace(text))
				{
					XmlElement xmlElement = xmlDocument.CreateElement(value2.Name, xmlNS);
					xmlElement.InnerText = text;
					list.Add(xmlElement);
				}
			}
		}
		return list.ToArray();
	}

	public void FromExchange(XmlNode[] data, bool edit)
	{
		if (_dataItems == null)
		{
			return;
		}
		if (Values == null)
		{
			Values = new Dictionary<string, object>();
		}
		else if (!edit)
		{
			Values.Clear();
		}
		if (data == null)
		{
			return;
		}
		foreach (XmlNode xmlNode in data)
		{
			if (xmlNode is XmlElement xmlElement && _dataItems.TryGetValue(xmlNode.Name, out var value))
			{
				if (edit && Values.ContainsKey(value.Name))
				{
					Values[value.Name] = value.FromBody(xmlElement.InnerText);
				}
				else
				{
					Values.Add(value.Name, value.FromBody(xmlElement.InnerText));
				}
			}
		}
	}

	public DocumentExtension Clone()
	{
		DocumentExtension documentExtension = new DocumentExtension(_dataItems);
		if (Values != null)
		{
			documentExtension.Values = new Dictionary<string, object>();
			foreach (KeyValuePair<string, object> value in Values)
			{
				documentExtension.Values.Add(value.Key, value.Value);
			}
		}
		return documentExtension;
	}

	private void Validate()
	{
		if (_dataItems == null)
		{
			return;
		}
		foreach (DataItem value in _dataItems.Values)
		{
			if (!value.IsCalculated && !value.Validate(this[value.Name], out var msg))
			{
				throw new RESTException(msg);
			}
		}
		foreach (DataItem value2 in _dataItems.Values)
		{
			_ = value2;
		}
	}

	public bool EvaluateExtensionUIReadOnly(string readOnlySource)
	{
		return false;
	}
}
