using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace CBA.ExchangeRates;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.cba.am/")]
public class ExchangeRatesByDateRangeByISOResponseExchangeRatesByDateRangeByISOResult
{
	private XmlElement[] anyField;

	private XmlElement any1Field;

	private string namespaceField;

	private string tableTypeNameField;

	[XmlAnyElement(Namespace = "http://www.w3.org/2001/XMLSchema", Order = 0)]
	public XmlElement[] Any
	{
		get
		{
			return anyField;
		}
		set
		{
			anyField = value;
		}
	}

	[XmlAnyElement(Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1", Order = 1)]
	public XmlElement Any1
	{
		get
		{
			return any1Field;
		}
		set
		{
			any1Field = value;
		}
	}

	[XmlAttribute]
	public string @namespace
	{
		get
		{
			return namespaceField;
		}
		set
		{
			namespaceField = value;
		}
	}

	[XmlAttribute]
	public string tableTypeName
	{
		get
		{
			return tableTypeNameField;
		}
		set
		{
			tableTypeNameField = value;
		}
	}

	public ExchangeRatesByDateRangeByISOResponseExchangeRatesByDateRangeByISOResult()
	{
		namespaceField = "http://tempuri.org/ExchangeRatesDS.xsd";
		tableTypeNameField = "ExchangeRatesByRangeDataTable";
	}
}
