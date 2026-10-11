using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

namespace CBA.ExchangeRates;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
[MessageContract(WrapperName = "ExchangeRatesByDateRangeByISOResponse", WrapperNamespace = "http://www.cba.am/", IsWrapped = true)]
public class ExchangeRatesByDateRangeByISOResponse
{
	[MessageBodyMember(Namespace = "http://www.cba.am/", Order = 0)]
	public ExchangeRatesByDateRangeByISOResponseExchangeRatesByDateRangeByISOResult ExchangeRatesByDateRangeByISOResult;

	public ExchangeRatesByDateRangeByISOResponse()
	{
	}

	public ExchangeRatesByDateRangeByISOResponse(ExchangeRatesByDateRangeByISOResponseExchangeRatesByDateRangeByISOResult ExchangeRatesByDateRangeByISOResult)
	{
		this.ExchangeRatesByDateRangeByISOResult = ExchangeRatesByDateRangeByISOResult;
	}
}
