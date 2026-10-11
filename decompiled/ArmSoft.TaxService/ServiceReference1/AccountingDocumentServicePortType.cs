using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[ServiceContract(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", ConfigurationName = "ServiceReference1.AccountingDocumentServicePortType")]
internal interface AccountingDocumentServicePortType
{
	[OperationContract(Action = "getAccDocTaxpayerInfo", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getAccDocTaxpayerInfoResponse1 getAccDocTaxpayerInfo(getAccDocTaxpayerInfoRequest1 request);

	[OperationContract(Action = "getAccDocTaxpayerInfo", ReplyAction = "*")]
	Task<getAccDocTaxpayerInfoResponse1> getAccDocTaxpayerInfoAsync(getAccDocTaxpayerInfoRequest1 request);

	[OperationContract(Action = "resignAccDocByBuyer", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	resignAccDocByBuyerResponse1 resignAccDocByBuyer(resignAccDocByBuyerRequest1 request);

	[OperationContract(Action = "resignAccDocByBuyer", ReplyAction = "*")]
	Task<resignAccDocByBuyerResponse1> resignAccDocByBuyerAsync(resignAccDocByBuyerRequest1 request);

	[OperationContract(Action = "getInvoiceNumbers", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getInvoiceNumberResponse1 getInvoiceNumber(getInvoiceNumberRequest1 request);

	[OperationContract(Action = "getInvoiceNumbers", ReplyAction = "*")]
	Task<getInvoiceNumberResponse1> getInvoiceNumberAsync(getInvoiceNumberRequest1 request);

	[OperationContract(Action = "getAccDocSubmittedList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getAccDocSubmittedListResponse getAccDocSubmittedList(getAccDocSubmittedListRequest request);

	[OperationContract(Action = "getAccDocSubmittedList", ReplyAction = "*")]
	Task<getAccDocSubmittedListResponse> getAccDocSubmittedListAsync(getAccDocSubmittedListRequest request);

	[OperationContract(Action = "getAccDocSubmittedListCount", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getAccDocSubmittedListCountResponse getAccDocSubmittedListCount(getAccDocSubmittedListCountRequest request);

	[OperationContract(Action = "getAccDocSubmittedListCount", ReplyAction = "*")]
	Task<getAccDocSubmittedListCountResponse> getAccDocSubmittedListCountAsync(getAccDocSubmittedListCountRequest request);

	[OperationContract(Action = "getAccDocInvalidList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getAccDocInvalidListResponse getAccDocInvalidList(getAccDocInvalidListRequest request);

	[OperationContract(Action = "getAccDocInvalidList", ReplyAction = "*")]
	Task<getAccDocInvalidListResponse> getAccDocInvalidListAsync(getAccDocInvalidListRequest request);

	[OperationContract(Action = "getAccDocInvalidListCount", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getAccDocInvalidListCountResponse getAccDocInvalidListCount(getAccDocInvalidListCountRequest request);

	[OperationContract(Action = "getAccDocInvalidListCount", ReplyAction = "*")]
	Task<getAccDocInvalidListCountResponse> getAccDocInvalidListCountAsync(getAccDocInvalidListCountRequest request);

	[OperationContract(Action = "getAccDocOverdueInvalidList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getAccDocOverdueInvalidListResponse getAccDocOverdueInvalidList(getAccDocOverdueInvalidListRequest request);

	[OperationContract(Action = "getAccDocOverdueInvalidList", ReplyAction = "*")]
	Task<getAccDocOverdueInvalidListResponse> getAccDocOverdueInvalidListAsync(getAccDocOverdueInvalidListRequest request);

	[OperationContract(Action = "getAccDocOverdueInvalidListCount", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getAccDocOverdueInvalidListCountResponse getAccDocOverdueInvalidListCount(getAccDocOverdueInvalidListCountRequest request);

	[OperationContract(Action = "getAccDocOverdueInvalidListCount", ReplyAction = "*")]
	Task<getAccDocOverdueInvalidListCountResponse> getAccDocOverdueInvalidListCountAsync(getAccDocOverdueInvalidListCountRequest request);

	[OperationContract(Action = "getAccDocReceivedList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getAccDocReceivedListResponse getAccDocReceivedList(getAccDocReceivedListRequest request);

	[OperationContract(Action = "getAccDocReceivedList", ReplyAction = "*")]
	Task<getAccDocReceivedListResponse> getAccDocReceivedListAsync(getAccDocReceivedListRequest request);

	[OperationContract(Action = "getAccDocReceivedListCount", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getAccDocReceivedListCountResponse getAccDocReceivedListCount(getAccDocReceivedListCountRequest request);

	[OperationContract(Action = "getAccDocReceivedListCount", ReplyAction = "*")]
	Task<getAccDocReceivedListCountResponse> getAccDocReceivedListCountAsync(getAccDocReceivedListCountRequest request);

	[OperationContract(Action = "getDocByInvoiceNumber", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getDocByInvoiceNumberResponse1 getDocByInvoiceNumber(getDocByInvoiceNumberRequest1 request);

	[OperationContract(Action = "getDocByInvoiceNumber", ReplyAction = "*")]
	Task<getDocByInvoiceNumberResponse1> getDocByInvoiceNumberAsync(getDocByInvoiceNumberRequest1 request);

	[OperationContract(Action = "getParentDocByInvoiceNumber", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getParentDocByInvoiceNumberResponse1 getParentDocByInvoiceNumber(getParentDocByInvoiceNumberRequest1 request);

	[OperationContract(Action = "getParentDocByInvoiceNumber", ReplyAction = "*")]
	Task<getParentDocByInvoiceNumberResponse1> getParentDocByInvoiceNumberAsync(getParentDocByInvoiceNumberRequest1 request);

	[OperationContract(Action = "signAccDocByBuyer", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	signAccDocByBuyerResponse1 signAccDocByBuyer(signAccDocByBuyerRequest1 request);

	[OperationContract(Action = "signAccDocByBuyer", ReplyAction = "*")]
	Task<signAccDocByBuyerResponse1> signAccDocByBuyerAsync(signAccDocByBuyerRequest1 request);

	[OperationContract(Action = "getAccDocReceipt", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getAccDocReceiptResponse1 getAccDocReceipt(getAccDocReceiptRequest1 request);

	[OperationContract(Action = "getAccDocReceipt", ReplyAction = "*")]
	Task<getAccDocReceiptResponse1> getAccDocReceiptAsync(getAccDocReceiptRequest1 request);

	[OperationContract(Action = "getAccDocInvalidationRequest", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getAccDocInvalidationRequestResponse getAccDocInvalidationRequest(getAccDocInvalidationRequestRequest request);

	[OperationContract(Action = "getAccDocInvalidationRequest", ReplyAction = "*")]
	Task<getAccDocInvalidationRequestResponse> getAccDocInvalidationRequestAsync(getAccDocInvalidationRequestRequest request);

	[OperationContract(Action = "getAccDocSignatureRevokationRequest", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getAccDocSignatureRevokationRequestResponse getAccDocSignatureRevokationRequest(getAccDocSignatureRevokationRequestRequest request);

	[OperationContract(Action = "getAccDocSignatureRevokationRequest", ReplyAction = "*")]
	Task<getAccDocSignatureRevokationRequestResponse> getAccDocSignatureRevokationRequestAsync(getAccDocSignatureRevokationRequestRequest request);

	[OperationContract(Action = "existWaitingInvalidationApprovementAccDocs", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	existWaitingInvalidationApprovementAccDocsResponse1 existWaitingInvalidationApprovementAccDocs(existWaitingInvalidationApprovementAccDocsRequest1 request);

	[OperationContract(Action = "existWaitingInvalidationApprovementAccDocs", ReplyAction = "*")]
	Task<existWaitingInvalidationApprovementAccDocsResponse1> existWaitingInvalidationApprovementAccDocsAsync(existWaitingInvalidationApprovementAccDocsRequest1 request);

	[OperationContract(Action = "existInvalidationApprovedAccDocs", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	existInvalidationApprovedAccDocsResponse1 existInvalidationApprovedAccDocs(existInvalidationApprovedAccDocsRequest1 request);

	[OperationContract(Action = "existInvalidationApprovedAccDocs", ReplyAction = "*")]
	Task<existInvalidationApprovedAccDocsResponse1> existInvalidationApprovedAccDocsAsync(existInvalidationApprovedAccDocsRequest1 request);

	[OperationContract(Action = "existInvalidationApprovedAccDocsForBuyer", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	existInvalidationApprovedAccDocsForBuyerResponse1 existInvalidationApprovedAccDocsForBuyer(existInvalidationApprovedAccDocsForBuyerRequest1 request);

	[OperationContract(Action = "existInvalidationApprovedAccDocsForBuyer", ReplyAction = "*")]
	Task<existInvalidationApprovedAccDocsForBuyerResponse1> existInvalidationApprovedAccDocsForBuyerAsync(existInvalidationApprovedAccDocsForBuyerRequest1 request);

	[OperationContract(Action = "existInvalidationCanceledAccDocs", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	existInvalidationCanceledAccDocsResponse1 existInvalidationCanceledAccDocs(existInvalidationCanceledAccDocsRequest1 request);

	[OperationContract(Action = "existInvalidationCanceledAccDocs", ReplyAction = "*")]
	Task<existInvalidationCanceledAccDocsResponse1> existInvalidationCanceledAccDocsAsync(existInvalidationCanceledAccDocsRequest1 request);

	[OperationContract(Action = "existDeletedAccDocs", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	existDeletedAccDocsResponse1 existDeletedAccDocs(existDeletedAccDocsRequest1 request);

	[OperationContract(Action = "existDeletedAccDocs", ReplyAction = "*")]
	Task<existDeletedAccDocsResponse1> existDeletedAccDocsAsync(existDeletedAccDocsRequest1 request);

	[OperationContract(Action = "existOnBehalfOfSupplierAccDocs", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	existOnBehalfOfSupplierAccDocsResponse1 existOnBehalfOfSupplierAccDocs(existOnBehalfOfSupplierAccDocsRequest1 request);

	[OperationContract(Action = "existOnBehalfOfSupplierAccDocs", ReplyAction = "*")]
	Task<existOnBehalfOfSupplierAccDocsResponse1> existOnBehalfOfSupplierAccDocsAsync(existOnBehalfOfSupplierAccDocsRequest1 request);

	[OperationContract(Action = "existInvalidatedOnBehalfOfSupplierAccDocs", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	existInvalidatedOnBehalfOfSupplierAccDocsResponse1 existInvalidatedOnBehalfOfSupplierAccDocs(existInvalidatedOnBehalfOfSupplierAccDocsRequest1 request);

	[OperationContract(Action = "existInvalidatedOnBehalfOfSupplierAccDocs", ReplyAction = "*")]
	Task<existInvalidatedOnBehalfOfSupplierAccDocsResponse1> existInvalidatedOnBehalfOfSupplierAccDocsAsync(existInvalidatedOnBehalfOfSupplierAccDocsRequest1 request);
}
