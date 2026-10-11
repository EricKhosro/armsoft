using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[ServiceContract(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", ConfigurationName = "ServiceReference1.AccDocSubmissionServicePortType")]
internal interface AccDocSubmissionServicePortType
{
	[OperationContract(Action = "registerSignedAccDoc", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	registerSignedAccDocResponse1 registerSignedAccDoc(registerSignedAccDocRequest1 request);

	[OperationContract(Action = "registerSignedAccDoc", ReplyAction = "*")]
	Task<registerSignedAccDocResponse1> registerSignedAccDocAsync(registerSignedAccDocRequest1 request);

	[OperationContract(Action = "resignAccDocBySupplier", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	resignAccDocBySupplierResponse1 resignAccDocBySupplier(resignAccDocBySupplierRequest1 request);

	[OperationContract(Action = "resignAccDocBySupplier", ReplyAction = "*")]
	Task<resignAccDocBySupplierResponse1> resignAccDocBySupplierAsync(resignAccDocBySupplierRequest1 request);

	[OperationContract(Action = "checkInvalidateAccDoc", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	checkInvalidateAccDocResponse1 checkInvalidateAccDoc(checkInvalidateAccDocRequest1 request);

	[OperationContract(Action = "checkInvalidateAccDoc", ReplyAction = "*")]
	Task<checkInvalidateAccDocResponse1> checkInvalidateAccDocAsync(checkInvalidateAccDocRequest1 request);

	[OperationContract(Action = "checkAccDocDeletePermition", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	checkAccDocDeletePermitionResponse1 checkAccDocDeletePermition(checkAccDocDeletePermitionRequest1 request);

	[OperationContract(Action = "checkAccDocDeletePermition", ReplyAction = "*")]
	Task<checkAccDocDeletePermitionResponse1> checkAccDocDeletePermitionAsync(checkAccDocDeletePermitionRequest1 request);

	[OperationContract(Action = "invalidateAccDoc", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	invalidateAccDocResponse1 invalidateAccDoc(invalidateAccDocRequest1 request);

	[OperationContract(Action = "invalidateAccDoc", ReplyAction = "*")]
	Task<invalidateAccDocResponse1> invalidateAccDocAsync(invalidateAccDocRequest1 request);

	[OperationContract(Action = "confirmAccDocInvalidationRequest", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	confirmAccDocInvalidationRequestResponse1 confirmAccDocInvalidationRequest(confirmAccDocInvalidationRequestRequest1 request);

	[OperationContract(Action = "confirmAccDocInvalidationRequest", ReplyAction = "*")]
	Task<confirmAccDocInvalidationRequestResponse1> confirmAccDocInvalidationRequestAsync(confirmAccDocInvalidationRequestRequest1 request);

	[OperationContract(Action = "rejectAccDocInvalidationRequest", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	rejectAccDocInvalidationRequestResponse1 rejectAccDocInvalidationRequest(rejectAccDocInvalidationRequestRequest1 request);

	[OperationContract(Action = "rejectAccDocInvalidationRequest", ReplyAction = "*")]
	Task<rejectAccDocInvalidationRequestResponse1> rejectAccDocInvalidationRequestAsync(rejectAccDocInvalidationRequestRequest1 request);

	[OperationContract(Action = "deleteAccDocRequest", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	deleteAccDocRequestResponse1 deleteAccDocRequest(deleteAccDocRequestRequest1 request);

	[OperationContract(Action = "deleteAccDocRequest", ReplyAction = "*")]
	Task<deleteAccDocRequestResponse1> deleteAccDocRequestAsync(deleteAccDocRequestRequest1 request);

	[OperationContract(Action = "checkOverdueInvalidateAccDoc", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	checkOverdueInvalidateAccDocResponse1 checkOverdueInvalidateAccDoc(checkOverdueInvalidateAccDocRequest1 request);

	[OperationContract(Action = "checkOverdueInvalidateAccDoc", ReplyAction = "*")]
	Task<checkOverdueInvalidateAccDocResponse1> checkOverdueInvalidateAccDocAsync(checkOverdueInvalidateAccDocRequest1 request);

	[OperationContract(Action = "overdueInvalidateAccDoc", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	overdueInvalidateAccDocResponse1 overdueInvalidateAccDoc(overdueInvalidateAccDocRequest1 request);

	[OperationContract(Action = "overdueInvalidateAccDoc", ReplyAction = "*")]
	Task<overdueInvalidateAccDocResponse1> overdueInvalidateAccDocAsync(overdueInvalidateAccDocRequest1 request);

	[OperationContract(Action = "checkRevokeAccDocSignature", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	checkRevokeAccDocSignatureResponse checkRevokeAccDocSignature(checkRevokeAccDocSignatureRequest1 request);

	[OperationContract(Action = "checkRevokeAccDocSignature", ReplyAction = "*")]
	Task<checkRevokeAccDocSignatureResponse> checkRevokeAccDocSignatureAsync(checkRevokeAccDocSignatureRequest1 request);

	[OperationContract(Action = "revokeAccDocSignature", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	revokeAccDocSignatureResponse revokeAccDocSignature(revokeAccDocSignatureRequest1 request);

	[OperationContract(Action = "revokeAccDocSignature", ReplyAction = "*")]
	Task<revokeAccDocSignatureResponse> revokeAccDocSignatureAsync(revokeAccDocSignatureRequest1 request);
}
