using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace ServiceReference1;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
internal class getNotifPDFResponse1
{
	[MessageBodyMember(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", Order = 0)]
	public GetNotifPDFResponse GetNotifPDFResponse;

	public getNotifPDFResponse1()
	{
	}

	public getNotifPDFResponse1(GetNotifPDFResponse GetNotifPDFResponse)
	{
		this.GetNotifPDFResponse = GetNotifPDFResponse;
	}
}
