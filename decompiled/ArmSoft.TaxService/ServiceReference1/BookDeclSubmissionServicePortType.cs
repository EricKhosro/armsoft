using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[ServiceContract(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", ConfigurationName = "ServiceReference1.BookDeclSubmissionServicePortType")]
internal interface BookDeclSubmissionServicePortType
{
	[OperationContract(Action = "registerSignedBookDecl", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	registerSignedBookDeclResponse1 registerSignedBookDecl(registerSignedBookDeclRequest1 request);

	[OperationContract(Action = "registerSignedBookDecl", ReplyAction = "*")]
	Task<registerSignedBookDeclResponse1> registerSignedBookDeclAsync(registerSignedBookDeclRequest1 request);

	[OperationContract(Action = "registerInvalidationDecl", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	registerInvalidationDeclResponse1 registerInvalidationDecl(registerInvalidationDeclRequest1 request);

	[OperationContract(Action = "registerInvalidationDecl", ReplyAction = "*")]
	Task<registerInvalidationDeclResponse1> registerInvalidationDeclAsync(registerInvalidationDeclRequest1 request);

	[OperationContract(Action = "registerOverdueDecl", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	registerOverdueDeclResponse1 registerOverdueDecl(registerOverdueDeclRequest1 request);

	[OperationContract(Action = "registerOverdueDecl", ReplyAction = "*")]
	Task<registerOverdueDeclResponse1> registerOverdueDeclAsync(registerOverdueDeclRequest1 request);
}
