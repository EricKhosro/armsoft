using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

namespace CBA.ExchangeRates;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
[MessageContract(WrapperName = "ExchangeRatesByDate", WrapperNamespace = "http://www.cba.am/", IsWrapped = true)]
public class ExchangeRatesByDateRequest
{
	[MessageBodyMember(Namespace = "http://www.cba.am/", Order = 0)]
	public DateTime date;

	public ExchangeRatesByDateRequest()
	{
	}

	public ExchangeRatesByDateRequest(DateTime date)
	{
		this.date = date;
	}
}
