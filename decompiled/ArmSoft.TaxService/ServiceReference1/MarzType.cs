using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class MarzType
{
	private string marzCodeField;

	private string marzNameField;

	[XmlElement(Order = 0)]
	public string MarzCode
	{
		get
		{
			return marzCodeField;
		}
		set
		{
			marzCodeField = value;
		}
	}

	[XmlElement(Order = 1)]
	public string MarzName
	{
		get
		{
			return marzNameField;
		}
		set
		{
			marzNameField = value;
		}
	}
}
