using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class BatchResponse : SignableData
{
	private int batchIdField;

	private BatchError[] batchErrorsField;

	[XmlElement(Order = 0)]
	public int BatchId
	{
		get
		{
			return batchIdField;
		}
		set
		{
			batchIdField = value;
		}
	}

	[XmlArray(Order = 1)]
	[XmlArrayItem(IsNullable = false)]
	public BatchError[] BatchErrors
	{
		get
		{
			return batchErrorsField;
		}
		set
		{
			batchErrorsField = value;
		}
	}
}
