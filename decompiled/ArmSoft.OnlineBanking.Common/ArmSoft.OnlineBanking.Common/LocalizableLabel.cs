using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace ArmSoft.OnlineBanking.Common;

[XmlRoot("labels")]
[TypeConverter(typeof(LocalizableLabelConverter))]
public class LocalizableLabel : ICloneable
{
	[XmlAttribute("defaultText")]
	public string DefaultText { get; set; }

	[XmlElement("label")]
	public List<LocalizedLabel> Labels { get; set; }

	public string GetTextByCulture => GetTextByLanguage(Thread.CurrentThread.CurrentUICulture.Name);

	public string GetTextInEnglishOrArmenian => GetTextByLanguage((Thread.CurrentThread.CurrentUICulture.Name == "ru-RU") ? "en-US" : Thread.CurrentThread.CurrentUICulture.Name);

	public LocalizableLabel()
	{
		DefaultText = string.Empty;
		Labels = new List<LocalizedLabel>();
	}

	public LocalizableLabel(string defaultText)
	{
		DefaultText = defaultText;
		Labels = new List<LocalizedLabel>();
	}

	public LocalizableLabel(string textAm, string textEn, string textRu, string defaultText = null)
	{
		DefaultText = (string.IsNullOrEmpty(defaultText) ? textEn : defaultText);
		Labels = new List<LocalizedLabel>
		{
			new LocalizedLabel(textAm, "hy-AM"),
			new LocalizedLabel(textEn, "en-US"),
			new LocalizedLabel(textRu, "ru-RU")
		};
	}

	public static LocalizableLabel FromResource(ResourceManager resourceManager, string resourceName, string defaultText = null)
	{
		string? textAm = resourceManager.GetString(resourceName, CultureInfo.CreateSpecificCulture("hy-AM"));
		string text = resourceManager.GetString(resourceName, CultureInfo.CreateSpecificCulture("en-US"));
		string textRu = resourceManager.GetString(resourceName, CultureInfo.CreateSpecificCulture("ru-RU"));
		return new LocalizableLabel(textAm, text, textRu, string.IsNullOrEmpty(defaultText) ? text : defaultText);
	}

	public static LocalizableLabel FromResource(Type resourceManager, string resourceName, string defaultText = null)
	{
		MethodInfo method = resourceManager.GetMethod("Get", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		if (method == null)
		{
			throw new MissingMethodException("No method found for Get.");
		}
		string textAm = (string)method.Invoke(null, new object[2] { resourceName, "hy-AM" });
		string text = (string)method.Invoke(null, new object[2] { resourceName, "en-US" });
		string textRu = (string)method.Invoke(null, new object[2] { resourceName, "ru-RU" });
		return new LocalizableLabel(textAm, text, textRu, string.IsNullOrEmpty(defaultText) ? text : defaultText);
	}

	public object Clone()
	{
		LocalizableLabel localizableLabel = new LocalizableLabel
		{
			DefaultText = DefaultText,
			Labels = new List<LocalizedLabel>()
		};
		foreach (LocalizedLabel label in Labels)
		{
			localizableLabel.Labels.Add((LocalizedLabel)label.Clone());
		}
		return localizableLabel;
	}

	public string GetTextByLanguage(string language)
	{
		string result = DefaultText;
		foreach (LocalizedLabel label in Labels)
		{
			if (label.Language == language && !string.IsNullOrEmpty(label.Text))
			{
				result = label.Text;
			}
		}
		return result;
	}

	public LocalizableLabel Replace(string oldValue, string newValue, string language = "")
	{
		if (string.IsNullOrEmpty(language) || language == "en-US")
		{
			DefaultText = DefaultText.Replace(oldValue, newValue);
		}
		foreach (LocalizedLabel label in Labels)
		{
			if (!string.IsNullOrEmpty(label.Text) && (string.IsNullOrEmpty(language) || label.Language == language))
			{
				label.Text = label.Text.Replace(oldValue, newValue);
			}
		}
		return this;
	}

	public override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}
		LocalizableLabel localizableLabel = (LocalizableLabel)obj;
		if (!DefaultText.Equals(localizableLabel.DefaultText))
		{
			return false;
		}
		if (!Labels.Count.Equals(localizableLabel.Labels.Count))
		{
			return false;
		}
		for (int i = 0; i < Labels.Count; i++)
		{
			if (!Labels[i].Equals(localizableLabel.Labels[i]))
			{
				return false;
			}
		}
		return true;
	}

	public override int GetHashCode()
	{
		int num = DefaultText.GetHashCode();
		for (int i = 0; i < Labels.Count; i++)
		{
			num ^= Labels[i].GetHashCode();
		}
		return num;
	}

	public override string ToString()
	{
		string text = null;
		XmlWriterSettings settings = new XmlWriterSettings
		{
			OmitXmlDeclaration = true
		};
		StringBuilder stringBuilder = new StringBuilder();
		using XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings);
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(LocalizableLabel));
		XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
		xmlSerializerNamespaces.Add(string.Empty, string.Empty);
		xmlSerializer.Serialize(xmlWriter, this, xmlSerializerNamespaces);
		return stringBuilder.ToString();
	}
}
