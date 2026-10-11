using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace ServiceReference1;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
internal class existDeletedAccDocsRequest1
{
	[MessageBodyMember(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", Order = 0)]
	public ExistDeletedAccDocsRequest ExistDeletedAccDocsRequest;

	public existDeletedAccDocsRequest1()
	{
	}

	public existDeletedAccDocsRequest1(ExistDeletedAccDocsRequest ExistDeletedAccDocsRequest)
	{
		this.ExistDeletedAccDocsRequest = ExistDeletedAccDocsRequest;
	}
}
