using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[ServiceContract(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", ConfigurationName = "ServiceReference1.InvoiceServicePortType")]
internal interface InvoiceServicePortType
{
	[OperationContract(Action = "checkVatPayerStatus", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	checkVatPayerStatusResponse1 checkVatPayerStatus(checkVatPayerStatusRequest1 request);

	[OperationContract(Action = "checkVatPayerStatus", ReplyAction = "*")]
	Task<checkVatPayerStatusResponse1> checkVatPayerStatusAsync(checkVatPayerStatusRequest1 request);

	[OperationContract(Action = "existWaitingInvalidationApprovementInvoices", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	existWaitingInvalidationApprovementInvoicesResponse1 existWaitingInvalidationApprovementInvoices(existWaitingInvalidationApprovementInvoicesRequest1 request);

	[OperationContract(Action = "existWaitingInvalidationApprovementInvoices", ReplyAction = "*")]
	Task<existWaitingInvalidationApprovementInvoicesResponse1> existWaitingInvalidationApprovementInvoicesAsync(existWaitingInvalidationApprovementInvoicesRequest1 request);

	[OperationContract(Action = "existInvalidationApprovedInvoices", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	existInvalidationApprovedInvoicesResponse1 existInvalidationApprovedInvoices(existInvalidationApprovedInvoicesRequest1 request);

	[OperationContract(Action = "existInvalidationApprovedInvoices", ReplyAction = "*")]
	Task<existInvalidationApprovedInvoicesResponse1> existInvalidationApprovedInvoicesAsync(existInvalidationApprovedInvoicesRequest1 request);

	[OperationContract(Action = "existInvalidationApprovedInvoicesForBuyer", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	existInvalidationApprovedInvoicesForBuyerResponse1 existInvalidationApprovedInvoicesForBuyer(existInvalidationApprovedInvoicesForBuyerRequest1 request);

	[OperationContract(Action = "existInvalidationApprovedInvoicesForBuyer", ReplyAction = "*")]
	Task<existInvalidationApprovedInvoicesForBuyerResponse1> existInvalidationApprovedInvoicesForBuyerAsync(existInvalidationApprovedInvoicesForBuyerRequest1 request);

	[OperationContract(Action = "existInvalidationCanceledInvoices", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	existInvalidationCanceledInvoicesResponse1 existInvalidationCanceledInvoices(existInvalidationCanceledInvoicesRequest1 request);

	[OperationContract(Action = "existInvalidationCanceledInvoices", ReplyAction = "*")]
	Task<existInvalidationCanceledInvoicesResponse1> existInvalidationCanceledInvoicesAsync(existInvalidationCanceledInvoicesRequest1 request);

	[OperationContract(Action = "existDeletedInvoices", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	existDeletedInvoicesResponse1 existDeletedInvoices(existDeletedInvoicesRequest1 request);

	[OperationContract(Action = "existDeletedInvoices", ReplyAction = "*")]
	Task<existDeletedInvoicesResponse1> existDeletedInvoicesAsync(existDeletedInvoicesRequest1 request);

	[OperationContract(Action = "existOnBehalfOfSupplierInvoices", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	existOnBehalfOfSupplierInvoicesResponse1 existOnBehalfOfSupplierInvoices(existOnBehalfOfSupplierInvoicesRequest1 request);

	[OperationContract(Action = "existOnBehalfOfSupplierInvoices", ReplyAction = "*")]
	Task<existOnBehalfOfSupplierInvoicesResponse1> existOnBehalfOfSupplierInvoicesAsync(existOnBehalfOfSupplierInvoicesRequest1 request);

	[OperationContract(Action = "existInvalidatedOnBehalfOfSupplierInvoices", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	existInvalidatedOnBehalfOfSupplierInvoicesResponse1 existInvalidatedOnBehalfOfSupplierInvoices(existInvalidatedOnBehalfOfSupplierInvoicesRequest1 request);

	[OperationContract(Action = "existInvalidatedOnBehalfOfSupplierInvoices", ReplyAction = "*")]
	Task<existInvalidatedOnBehalfOfSupplierInvoicesResponse1> existInvalidatedOnBehalfOfSupplierInvoicesAsync(existInvalidatedOnBehalfOfSupplierInvoicesRequest1 request);

	[OperationContract(Action = "resignInvoiceByBuyer", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	resignInvoiceByBuyerResponse1 resignInvoiceByBuyer(resignInvoiceByBuyerRequest1 request);

	[OperationContract(Action = "resignInvoiceByBuyer", ReplyAction = "*")]
	Task<resignInvoiceByBuyerResponse1> resignInvoiceByBuyerAsync(resignInvoiceByBuyerRequest1 request);

	[OperationContract(Action = "checkInvoicePrintStatus", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	checkInvoicePrintStatusResponse1 checkInvoicePrintStatus(checkInvoicePrintStatusRequest1 request);

	[OperationContract(Action = "checkInvoicePrintStatus", ReplyAction = "*")]
	Task<checkInvoicePrintStatusResponse1> checkInvoicePrintStatusAsync(checkInvoicePrintStatusRequest1 request);

	[OperationContract(Action = "getTaxpayerInfo", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getTaxpayerInfoResponse1 getTaxpayerInfo(getTaxpayerInfoRequest1 request);

	[OperationContract(Action = "getTaxpayerInfo", ReplyAction = "*")]
	Task<getTaxpayerInfoResponse1> getTaxpayerInfoAsync(getTaxpayerInfoRequest1 request);

	[OperationContract(Action = "getTaxpayerInfoExtended", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getTaxpayerInfoExtendedResponse getTaxpayerInfoExtended(getTaxpayerInfoExtendedRequest request);

	[OperationContract(Action = "getTaxpayerInfoExtended", ReplyAction = "*")]
	Task<getTaxpayerInfoExtendedResponse> getTaxpayerInfoExtendedAsync(getTaxpayerInfoExtendedRequest request);

	[OperationContract(Action = "getInvoiceNumbers", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getInvoiceNumberResponse1 getInvoiceNumber(getInvoiceNumberRequest1 request);

	[OperationContract(Action = "getInvoiceNumbers", ReplyAction = "*")]
	Task<getInvoiceNumberResponse1> getInvoiceNumberAsync(getInvoiceNumberRequest1 request);

	[OperationContract(Action = "getCertificateStatus", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getCertificateStatusResponse1 getCertificateStatus(getCertificateStatusRequest1 request);

	[OperationContract(Action = "getCertificateStatus", ReplyAction = "*")]
	Task<getCertificateStatusResponse1> getCertificateStatusAsync(getCertificateStatusRequest1 request);

	[OperationContract(Action = "getSubmittedList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getSubmittedListResponse getSubmittedList(getSubmittedListRequest request);

	[OperationContract(Action = "getSubmittedList", ReplyAction = "*")]
	Task<getSubmittedListResponse> getSubmittedListAsync(getSubmittedListRequest request);

	[OperationContract(Action = "getSubmittedListCount", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getSubmittedListCountResponse getSubmittedListCount(getSubmittedListCountRequest request);

	[OperationContract(Action = "getSubmittedListCount", ReplyAction = "*")]
	Task<getSubmittedListCountResponse> getSubmittedListCountAsync(getSubmittedListCountRequest request);

	[OperationContract(Action = "getInvalidList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getInvalidListResponse getInvalidList(getInvalidListRequest request);

	[OperationContract(Action = "getInvalidList", ReplyAction = "*")]
	Task<getInvalidListResponse> getInvalidListAsync(getInvalidListRequest request);

	[OperationContract(Action = "getInvalidListCount", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getInvalidListCountResponse getInvalidListCount(getInvalidListCountRequest request);

	[OperationContract(Action = "getInvalidListCount", ReplyAction = "*")]
	Task<getInvalidListCountResponse> getInvalidListCountAsync(getInvalidListCountRequest request);

	[OperationContract(Action = "getOverdueInvalidList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getOverdueInvalidListResponse getOverdueInvalidList(getOverdueInvalidListRequest request);

	[OperationContract(Action = "getOverdueInvalidList", ReplyAction = "*")]
	Task<getOverdueInvalidListResponse> getOverdueInvalidListAsync(getOverdueInvalidListRequest request);

	[OperationContract(Action = "getOverdueInvalidListCount", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getOverdueInvalidListCountResponse getOverdueInvalidListCount(getOverdueInvalidListCountRequest request);

	[OperationContract(Action = "getOverdueInvalidListCount", ReplyAction = "*")]
	Task<getOverdueInvalidListCountResponse> getOverdueInvalidListCountAsync(getOverdueInvalidListCountRequest request);

	[OperationContract(Action = "getReceivedList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getReceivedListResponse getReceivedList(getReceivedListRequest request);

	[OperationContract(Action = "getReceivedList", ReplyAction = "*")]
	Task<getReceivedListResponse> getReceivedListAsync(getReceivedListRequest request);

	[OperationContract(Action = "getReceivedListCount", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getReceivedListCountResponse getReceivedListCount(getReceivedListCountRequest request);

	[OperationContract(Action = "getReceivedListCount", ReplyAction = "*")]
	Task<getReceivedListCountResponse> getReceivedListCountAsync(getReceivedListCountRequest request);

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

	[OperationContract(Action = "signInvoiceByBuyer", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	signInvoiceByBuyerResponse1 signInvoiceByBuyer(signInvoiceByBuyerRequest1 request);

	[OperationContract(Action = "signInvoiceByBuyer", ReplyAction = "*")]
	Task<signInvoiceByBuyerResponse1> signInvoiceByBuyerAsync(signInvoiceByBuyerRequest1 request);

	[OperationContract(Action = "getReceipt", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getReceiptResponse1 getReceipt(getReceiptRequest1 request);

	[OperationContract(Action = "getReceipt", ReplyAction = "*")]
	Task<getReceiptResponse1> getReceiptAsync(getReceiptRequest1 request);

	[OperationContract(Action = "getInvalidationRequest", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getInvalidationRequestResponse1 getInvalidationRequest(getInvalidationRequestRequest1 request);

	[OperationContract(Action = "getInvalidationRequest", ReplyAction = "*")]
	Task<getInvalidationRequestResponse1> getInvalidationRequestAsync(getInvalidationRequestRequest1 request);

	[OperationContract(Action = "getSignatureRevokationRequest", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getSignatureRevokationRequestResponse1 getSignatureRevokationRequest(getSignatureRevokationRequestRequest1 request);

	[OperationContract(Action = "getSignatureRevokationRequest", ReplyAction = "*")]
	Task<getSignatureRevokationRequestResponse1> getSignatureRevokationRequestAsync(getSignatureRevokationRequestRequest1 request);

	[OperationContract(Action = "getInvoiceNumberStatus", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getInvoiceNumberStatusResponse1 getInvoiceNumberStatus(getInvoiceNumberStatusRequest1 request);

	[OperationContract(Action = "getInvoiceNumberStatus", ReplyAction = "*")]
	Task<getInvoiceNumberStatusResponse1> getInvoiceNumberStatusAsync(getInvoiceNumberStatusRequest1 request);

	[OperationContract(Action = "getTaxFreeGoodsList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getTaxFreeGoodsListResponse1 getTaxFreeGoodsList(getTaxFreeGoodsListRequest1 request);

	[OperationContract(Action = "getTaxFreeGoodsList", ReplyAction = "*")]
	Task<getTaxFreeGoodsListResponse1> getTaxFreeGoodsListAsync(getTaxFreeGoodsListRequest1 request);

	[OperationContract(Action = "getTaxFreeCountryList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getTaxFreeCountryListResponse1 getTaxFreeCountryList(getTaxFreeCountryListRequest1 request);

	[OperationContract(Action = "getTaxFreeCountryList", ReplyAction = "*")]
	Task<getTaxFreeCountryListResponse1> getTaxFreeCountryListAsync(getTaxFreeCountryListRequest1 request);
}
