using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace ServiceReference1;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
internal class getDocByInvoiceNumberResponse1
{
	[MessageBodyMember(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", Order = 0)]
	public GetDocByInvoiceNumberResponse GetDocByInvoiceNumberResponse;

	public getDocByInvoiceNumberResponse1()
	{
	}

	public getDocByInvoiceNumberResponse1(GetDocByInvoiceNumberResponse GetDocByInvoiceNumberResponse)
	{
		this.GetDocByInvoiceNumberResponse = GetDocByInvoiceNumberResponse;
	}
}
