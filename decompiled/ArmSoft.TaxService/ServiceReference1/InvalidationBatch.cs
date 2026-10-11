using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class InvalidationBatch : SignableData
{
	private InvalidationBatchEntry[] entryField;

	[XmlElement("Entry", Order = 0)]
	public InvalidationBatchEntry[] Entry
	{
		get
		{
			return entryField;
		}
		set
		{
			entryField = value;
		}
	}
}
