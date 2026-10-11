using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace CBA.ExchangeRates;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.cba.am/")]
public class ExchangeRates
{
	private DateTime currentDateField;

	private DateTime? nextAvailableDateField;

	private DateTime? previousAvailableDateField;

	private ExchangeRate[] ratesField;

	[XmlElement(Order = 0)]
	public DateTime CurrentDate
	{
		get
		{
			return currentDateField;
		}
		set
		{
			currentDateField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 1)]
	public DateTime? NextAvailableDate
	{
		get
		{
			return nextAvailableDateField;
		}
		set
		{
			nextAvailableDateField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 2)]
	public DateTime? PreviousAvailableDate
	{
		get
		{
			return previousAvailableDateField;
		}
		set
		{
			previousAvailableDateField = value;
		}
	}

	[XmlArray(Order = 3)]
	public ExchangeRate[] Rates
	{
		get
		{
			return ratesField;
		}
		set
		{
			ratesField = value;
		}
	}
}
