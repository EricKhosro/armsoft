using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[ServiceContract(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", ConfigurationName = "ServiceReference1.EBookServicePortType")]
internal interface EBookServicePortType
{
	[OperationContract(Action = "getEbookList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getEbookListResponse1 getEbookList(getEbookListRequest1 request);

	[OperationContract(Action = "getEbookList", ReplyAction = "*")]
	Task<getEbookListResponse1> getEbookListAsync(getEbookListRequest1 request);

	[OperationContract(Action = "getEbookListCount", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getEbookListCountResponse1 getEbookListCount(getEbookListCountRequest1 request);

	[OperationContract(Action = "getEbookListCount", ReplyAction = "*")]
	Task<getEbookListCountResponse1> getEbookListCountAsync(getEbookListCountRequest1 request);

	[OperationContract(Action = "getEbook", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getEbookResponse1 getEbook(getEbookRequest1 request);

	[OperationContract(Action = "getEbook", ReplyAction = "*")]
	Task<getEbookResponse1> getEbookAsync(getEbookRequest1 request);

	[OperationContract(Action = "cancelEbookRows", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	cancelEbookRowsResponse1 cancelEbookRows(cancelEbookRowsRequest1 request);

	[OperationContract(Action = "cancelEbookRows", ReplyAction = "*")]
	Task<cancelEbookRowsResponse1> cancelEbookRowsAsync(cancelEbookRowsRequest1 request);

	[OperationContract(Action = "postEbookRows", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	postEbookRowsResponse1 postEbookRows(postEbookRowsRequest1 request);

	[OperationContract(Action = "postEbookRows", ReplyAction = "*")]
	Task<postEbookRowsResponse1> postEbookRowsAsync(postEbookRowsRequest1 request);

	[OperationContract(Action = "getEbookDetailListCount", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getEbookDetailListCountResponse getEbookDetailListCount(getEbookDetailListCountRequest request);

	[OperationContract(Action = "getEbookDetailListCount", ReplyAction = "*")]
	Task<getEbookDetailListCountResponse> getEbookDetailListCountAsync(getEbookDetailListCountRequest request);
}
