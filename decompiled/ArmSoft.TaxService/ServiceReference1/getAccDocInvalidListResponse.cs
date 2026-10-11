using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace ServiceReference1;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
internal class getAccDocInvalidListResponse
{
	[MessageBodyMember(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", Order = 0)]
	public GetAccDocsResponse GetAccDocInvalidListResponse;

	public getAccDocInvalidListResponse()
	{
	}

	public getAccDocInvalidListResponse(GetAccDocsResponse GetAccDocInvalidListResponse)
	{
		this.GetAccDocInvalidListResponse = GetAccDocInvalidListResponse;
	}
}
