using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ArmSoft.OnlineBanking.Common;

public class LocalizableLabelConverter : TypeConverter
{
	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		object result = null;
		if (value is string s)
		{
			using StringReader textReader = new StringReader(s);
			result = new XmlSerializer(typeof(LocalizableLabel)).Deserialize(textReader);
		}
		else
		{
			result = base.ConvertFrom(context, culture, value);
		}
		return result;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		object result = null;
		if (destinationType.Equals(typeof(string)))
		{
			XmlWriterSettings settings = new XmlWriterSettings
			{
				OmitXmlDeclaration = true
			};
			StringBuilder stringBuilder = new StringBuilder();
			using XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(LocalizableLabel));
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add(string.Empty, string.Empty);
			xmlSerializer.Serialize(xmlWriter, value, xmlSerializerNamespaces);
			result = stringBuilder.ToString();
		}
		else
		{
			result = base.ConvertTo(context, culture, value, destinationType);
		}
		return result;
	}
}
