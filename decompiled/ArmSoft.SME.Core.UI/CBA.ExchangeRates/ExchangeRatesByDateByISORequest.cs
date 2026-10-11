using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

namespace CBA.ExchangeRates;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
[MessageContract(WrapperName = "ExchangeRatesByDateByISO", WrapperNamespace = "http://www.cba.am/", IsWrapped = true)]
public class ExchangeRatesByDateByISORequest
{
	[MessageBodyMember(Namespace = "http://www.cba.am/", Order = 0)]
	public DateTime date;

	[MessageBodyMember(Namespace = "http://www.cba.am/", Order = 1)]
	public string ISO;

	public ExchangeRatesByDateByISORequest()
	{
	}

	public ExchangeRatesByDateByISORequest(DateTime date, string ISO)
	{
		this.date = date;
		this.ISO = ISO;
	}
}
