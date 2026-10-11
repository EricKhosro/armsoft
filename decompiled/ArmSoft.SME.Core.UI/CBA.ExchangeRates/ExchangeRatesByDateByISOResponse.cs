using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

namespace CBA.ExchangeRates;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
[MessageContract(WrapperName = "ExchangeRatesByDateByISOResponse", WrapperNamespace = "http://www.cba.am/", IsWrapped = true)]
public class ExchangeRatesByDateByISOResponse
{
	[MessageBodyMember(Namespace = "http://www.cba.am/", Order = 0)]
	public ExchangeRates ExchangeRatesByDateByISOResult;

	public ExchangeRatesByDateByISOResponse()
	{
	}

	public ExchangeRatesByDateByISOResponse(ExchangeRates ExchangeRatesByDateByISOResult)
	{
		this.ExchangeRatesByDateByISOResult = ExchangeRatesByDateByISOResult;
	}
}
