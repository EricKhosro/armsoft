using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class GetAccDocListCountResponse : GenericResponse
{
	private AccDocTotalSumDto accDocTotalSumDtoField;

	private long countField;

	private double totalField;

	[XmlElement(Order = 0)]
	public AccDocTotalSumDto AccDocTotalSumDto
	{
		get
		{
			return accDocTotalSumDtoField;
		}
		set
		{
			accDocTotalSumDtoField = value;
		}
	}

	[XmlElement(Order = 1)]
	public long Count
	{
		get
		{
			return countField;
		}
		set
		{
			countField = value;
		}
	}

	[XmlElement(Order = 2)]
	public double Total
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
