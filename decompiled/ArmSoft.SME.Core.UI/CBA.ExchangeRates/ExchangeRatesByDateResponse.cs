using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

namespace CBA.ExchangeRates;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
[MessageContract(WrapperName = "ExchangeRatesByDateResponse", WrapperNamespace = "http://www.cba.am/", IsWrapped = true)]
public class ExchangeRatesByDateResponse
{
	[MessageBodyMember(Namespace = "http://www.cba.am/", Order = 0)]
	public ExchangeRates ExchangeRatesByDateResult;

	public ExchangeRatesByDateResponse()
	{
	}

	public ExchangeRatesByDateResponse(ExchangeRates ExchangeRatesByDateResult)
	{
		this.ExchangeRatesByDateResult = ExchangeRatesByDateResult;
	}
}
