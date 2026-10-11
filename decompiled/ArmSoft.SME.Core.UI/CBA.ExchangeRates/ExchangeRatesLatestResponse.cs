using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

namespace CBA.ExchangeRates;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
[MessageContract(WrapperName = "ExchangeRatesLatestResponse", WrapperNamespace = "http://www.cba.am/", IsWrapped = true)]
public class ExchangeRatesLatestResponse
{
	[MessageBodyMember(Namespace = "http://www.cba.am/", Order = 0)]
	public ExchangeRates ExchangeRatesLatestResult;

	public ExchangeRatesLatestResponse()
	{
	}

	public ExchangeRatesLatestResponse(ExchangeRates ExchangeRatesLatestResult)
	{
		this.ExchangeRatesLatestResult = ExchangeRatesLatestResult;
	}
}
