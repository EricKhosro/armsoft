using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

namespace CBA.ExchangeRates;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
[MessageContract(WrapperName = "ISOCodesDetailedResponse", WrapperNamespace = "http://www.cba.am/", IsWrapped = true)]
public class ISOCodesDetailedResponse
{
	[MessageBodyMember(Namespace = "http://www.cba.am/", Order = 0)]
	public ISOCodesDetailedResponseISOCodesDetailedResult ISOCodesDetailedResult;

	public ISOCodesDetailedResponse()
	{
	}

	public ISOCodesDetailedResponse(ISOCodesDetailedResponseISOCodesDetailedResult ISOCodesDetailedResult)
	{
		this.ISOCodesDetailedResult = ISOCodesDetailedResult;
	}
}
