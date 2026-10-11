using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace ServiceReference1;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
internal class existInvalidationCanceledInvoicesResponse1
{
	[MessageBodyMember(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", Order = 0)]
	public ExistInvalidationCanceledInvoicesResponse ExistInvalidationCanceledInvoicesResponse;

	public existInvalidationCanceledInvoicesResponse1()
	{
	}

	public existInvalidationCanceledInvoicesResponse1(ExistInvalidationCanceledInvoicesResponse ExistInvalidationCanceledInvoicesResponse)
	{
		this.ExistInvalidationCanceledInvoicesResponse = ExistInvalidationCanceledInvoicesResponse;
	}
}
