using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

namespace CBA.ExchangeRates;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
[MessageContract(WrapperName = "ExchangeRatesByDateRangeByISO", WrapperNamespace = "http://www.cba.am/", IsWrapped = true)]
public class ExchangeRatesByDateRangeByISORequest
{
	[MessageBodyMember(Namespace = "http://www.cba.am/", Order = 0)]
	public string ISOCodes;

	[MessageBodyMember(Namespace = "http://www.cba.am/", Order = 1)]
	public DateTime DateFrom;

	[MessageBodyMember(Namespace = "http://www.cba.am/", Order = 2)]
	public DateTime DateTo;

	public ExchangeRatesByDateRangeByISORequest()
	{
	}

	public ExchangeRatesByDateRangeByISORequest(string ISOCodes, DateTime DateFrom, DateTime DateTo)
	{
		this.ISOCodes = ISOCodes;
		this.DateFrom = DateFrom;
		this.DateTo = DateTo;
	}
}
