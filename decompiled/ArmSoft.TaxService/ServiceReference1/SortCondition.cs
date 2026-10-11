using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class SortCondition
{
	private bool sortAscField;

	private SortConditionSortBy sortByField;

	[XmlElement(Order = 0)]
	public bool SortAsc
	{
		get
		{
			return sortAscField;
		}
		set
		{
			sortAscField = value;
		}
	}

	[XmlElement(Order = 1)]
	public SortConditionSortBy SortBy
	{
		get
		{
			return sortByField;
		}
		set
		{
			sortByField = value;
		}
	}
}
