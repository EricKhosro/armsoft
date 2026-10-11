using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class File
{
	private byte[] contentField;

	private bool zippedField;

	private bool zippedFieldSpecified;

	[XmlElement(DataType = "base64Binary", Order = 0)]
	public byte[] Content
	{
		get
		{
			return contentField;
		}
		set
		{
			contentField = value;
		}
	}

	[XmlAttribute]
	public bool zipped
	{
		get
		{
			return zippedField;
		}
		set
		{
			zippedField = value;
		}
	}

	[XmlIgnore]
	public bool zippedSpecified
	{
		get
		{
			return zippedFieldSpecified;
		}
		set
		{
			zippedFieldSpecified = value;
		}
	}
}
