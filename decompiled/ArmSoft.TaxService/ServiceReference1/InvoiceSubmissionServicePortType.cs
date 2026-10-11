using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[ServiceContract(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", ConfigurationName = "ServiceReference1.InvoiceSubmissionServicePortType")]
internal interface InvoiceSubmissionServicePortType
{
	[OperationContract(Action = "registerSignedInvoice", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	registerSignedInvoiceResponse1 registerSignedInvoice(registerSignedInvoiceRequest1 request);

	[OperationContract(Action = "registerSignedInvoice", ReplyAction = "*")]
	Task<registerSignedInvoiceResponse1> registerSignedInvoiceAsync(registerSignedInvoiceRequest1 request);

	[OperationContract(Action = "resignInvoiceBySupplier", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	resignInvoiceBySupplierResponse1 resignInvoiceBySupplier(resignInvoiceBySupplierRequest1 request);

	[OperationContract(Action = "resignInvoiceBySupplier", ReplyAction = "*")]
	Task<resignInvoiceBySupplierResponse1> resignInvoiceBySupplierAsync(resignInvoiceBySupplierRequest1 request);

	[OperationContract(Action = "checkInvalidateInvoice", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	checkInvalidateInvoiceResponse1 checkInvalidateInvoice(checkInvalidateInvoiceRequest1 request);

	[OperationContract(Action = "checkInvalidateInvoice", ReplyAction = "*")]
	Task<checkInvalidateInvoiceResponse1> checkInvalidateInvoiceAsync(checkInvalidateInvoiceRequest1 request);

	[OperationContract(Action = "checkInvoiceDeletePermition", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	checkInvoiceDeletePermitionResponse1 checkInvoiceDeletePermition(checkInvoiceDeletePermitionRequest1 request);

	[OperationContract(Action = "checkInvoiceDeletePermition", ReplyAction = "*")]
	Task<checkInvoiceDeletePermitionResponse1> checkInvoiceDeletePermitionAsync(checkInvoiceDeletePermitionRequest1 request);

	[OperationContract(Action = "invalidateInvoice", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	invalidateInvoiceResponse1 invalidateInvoice(invalidateInvoiceRequest1 request);

	[OperationContract(Action = "invalidateInvoice", ReplyAction = "*")]
	Task<invalidateInvoiceResponse1> invalidateInvoiceAsync(invalidateInvoiceRequest1 request);

	[OperationContract(Action = "checkOverdueInvalidateInvoice", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	checkOverdueInvalidateInvoiceResponse1 checkOverdueInvalidateInvoice(checkOverdueInvalidateInvoiceRequest1 request);

	[OperationContract(Action = "checkOverdueInvalidateInvoice", ReplyAction = "*")]
	Task<checkOverdueInvalidateInvoiceResponse1> checkOverdueInvalidateInvoiceAsync(checkOverdueInvalidateInvoiceRequest1 request);

	[OperationContract(Action = "overdueInvalidateInvoice", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	overdueInvalidateInvoiceResponse1 overdueInvalidateInvoice(overdueInvalidateInvoiceRequest1 request);

	[OperationContract(Action = "overdueInvalidateInvoice", ReplyAction = "*")]
	Task<overdueInvalidateInvoiceResponse1> overdueInvalidateInvoiceAsync(overdueInvalidateInvoiceRequest1 request);

	[OperationContract(Action = "confirmInvoiceInvalidationRequest", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	confirmInvoiceInvalidationRequestResponse1 confirmInvoiceInvalidationRequest(confirmInvoiceInvalidationRequestRequest1 request);

	[OperationContract(Action = "confirmInvoiceInvalidationRequest", ReplyAction = "*")]
	Task<confirmInvoiceInvalidationRequestResponse1> confirmInvoiceInvalidationRequestAsync(confirmInvoiceInvalidationRequestRequest1 request);

	[OperationContract(Action = "rejectInvoiceInvalidationRequest", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	rejectInvoiceInvalidationRequestResponse1 rejectInvoiceInvalidationRequest(rejectInvoiceInvalidationRequestRequest1 request);

	[OperationContract(Action = "rejectInvoiceInvalidationRequest", ReplyAction = "*")]
	Task<rejectInvoiceInvalidationRequestResponse1> rejectInvoiceInvalidationRequestAsync(rejectInvoiceInvalidationRequestRequest1 request);

	[OperationContract(Action = "deleteInvoiceRequest", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	deleteInvoiceRequestResponse1 deleteInvoiceRequest(deleteInvoiceRequestRequest1 request);

	[OperationContract(Action = "deleteInvoiceRequest", ReplyAction = "*")]
	Task<deleteInvoiceRequestResponse1> deleteInvoiceRequestAsync(deleteInvoiceRequestRequest1 request);

	[OperationContract(Action = "checkRevokeSignature", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	checkRevokeSignatureResponse checkRevokeSignature(checkRevokeSignatureRequest1 request);

	[OperationContract(Action = "checkRevokeSignature", ReplyAction = "*")]
	Task<checkRevokeSignatureResponse> checkRevokeSignatureAsync(checkRevokeSignatureRequest1 request);

	[OperationContract(Action = "revokeSignature", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	revokeSignatureResponse revokeSignature(revokeSignatureRequest1 request);

	[OperationContract(Action = "revokeSignature", ReplyAction = "*")]
	Task<revokeSignatureResponse> revokeSignatureAsync(revokeSignatureRequest1 request);
}
