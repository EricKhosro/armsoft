using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

namespace CBA.ExchangeRates;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
[MessageContract(WrapperName = "ExchangeRatesLatestByISOResponse", WrapperNamespace = "http://www.cba.am/", IsWrapped = true)]
public class ExchangeRatesLatestByISOResponse
{
	[MessageBodyMember(Namespace = "http://www.cba.am/", Order = 0)]
	public ExchangeRates ExchangeRatesLatestByISOResult;

	public ExchangeRatesLatestByISOResponse()
	{
	}

	public ExchangeRatesLatestByISOResponse(ExchangeRates ExchangeRatesLatestByISOResult)
	{
		this.ExchangeRatesLatestByISOResult = ExchangeRatesLatestByISOResult;
	}
}
