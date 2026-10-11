using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class AccDocGoodsInfo
{
	private Good[] goodField;

	private AccDocTotal totalField;

	[XmlElement("Good", Order = 0)]
	public Good[] Good
	{
		get
		{
			return goodField;
		}
		set
		{
			goodField = value;
		}
	}

	[XmlElement(Order = 1)]
	public AccDocTotal Total
	{
		get
		{
			return totalField;
		}
		set
		{
			totalField = value;
		}
	}
}
