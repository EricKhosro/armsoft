using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class TpBookListElement
{
	private string tpBookNumberField;

	private string tpStumpNumberField;

	private long oinField;

	[XmlElement(Order = 0)]
	public string TpBookNumber
	{
		get
		{
			return tpBookNumberField;
		}
		set
		{
			tpBookNumberField = value;
		}
	}

	[XmlElement(Order = 1)]
	public string TpStumpNumber
	{
		get
		{
			return tpStumpNumberField;
		}
		set
		{
			tpStumpNumberField = value;
		}
	}

	[XmlElement(Order = 2)]
	public long Oin
	{
		get
		{
			return oinField;
		}
		set
		{
			oinField = value;
		}
	}
}
