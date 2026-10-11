using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[ServiceContract(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", ConfigurationName = "ServiceReference1.BookDeclServicePortType")]
internal interface BookDeclServicePortType
{
	[OperationContract(Action = "getSubmittedDeclsList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getSubmittedDeclsListResponse getSubmittedDeclsList(getSubmittedDeclsListRequest request);

	[OperationContract(Action = "getSubmittedDeclsList", ReplyAction = "*")]
	Task<getSubmittedDeclsListResponse> getSubmittedDeclsListAsync(getSubmittedDeclsListRequest request);

	[OperationContract(Action = "getSubmittedDeclsListCount", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getSubmittedDeclsListCountResponse getSubmittedDeclsListCount(getSubmittedDeclsListCountRequest request);

	[OperationContract(Action = "getSubmittedDeclsListCount", ReplyAction = "*")]
	Task<getSubmittedDeclsListCountResponse> getSubmittedDeclsListCountAsync(getSubmittedDeclsListCountRequest request);

	[OperationContract(Action = "getBookDeclByDln", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getBookDeclByDlnResponse1 getBookDeclByDln(getBookDeclByDlnRequest1 request);

	[OperationContract(Action = "getBookDeclByDln", ReplyAction = "*")]
	Task<getBookDeclByDlnResponse1> getBookDeclByDlnAsync(getBookDeclByDlnRequest1 request);

	[OperationContract(Action = "getNotifPDF", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getNotifPDFResponse1 getNotifPDF(getNotifPDFRequest1 request);

	[OperationContract(Action = "getNotifPDF", ReplyAction = "*")]
	Task<getNotifPDFResponse1> getNotifPDFAsync(getNotifPDFRequest1 request);
}
