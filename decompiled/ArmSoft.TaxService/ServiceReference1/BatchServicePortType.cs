using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[ServiceContract(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", ConfigurationName = "ServiceReference1.BatchServicePortType")]
internal interface BatchServicePortType
{
	[OperationContract(Action = "getInvoiceNumberBatch", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getInvoiceNumberBatchResponse1 getInvoiceNumberBatch(getInvoiceNumberBatchRequest1 request);

	[OperationContract(Action = "getInvoiceNumberBatch", ReplyAction = "*")]
	Task<getInvoiceNumberBatchResponse1> getInvoiceNumberBatchAsync(getInvoiceNumberBatchRequest1 request);

	[OperationContract(Action = "registerInvoiceBatch", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	registerInvoiceBatchResponse1 registerInvoiceBatch(registerInvoiceBatchRequest1 request);

	[OperationContract(Action = "registerInvoiceBatch", ReplyAction = "*")]
	Task<registerInvoiceBatchResponse1> registerInvoiceBatchAsync(registerInvoiceBatchRequest1 request);

	[OperationContract(Action = "invalidateInvoices", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	invalidateInvoicesResponse1 invalidateInvoices(invalidateInvoicesRequest1 request);

	[OperationContract(Action = "invalidateInvoices", ReplyAction = "*")]
	Task<invalidateInvoicesResponse1> invalidateInvoicesAsync(invalidateInvoicesRequest1 request);

	[OperationContract(Action = "getAccDocNumberBatch", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getAccDocNumberBatchResponse1 getAccDocNumberBatch(getAccDocNumberBatchRequest1 request);

	[OperationContract(Action = "getAccDocNumberBatch", ReplyAction = "*")]
	Task<getAccDocNumberBatchResponse1> getAccDocNumberBatchAsync(getAccDocNumberBatchRequest1 request);

	[OperationContract(Action = "registerAccDocBatch", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	registerAccDocBatchResponse1 registerAccDocBatch(registerAccDocBatchRequest1 request);

	[OperationContract(Action = "registerAccDocBatch", ReplyAction = "*")]
	Task<registerAccDocBatchResponse1> registerAccDocBatchAsync(registerAccDocBatchRequest1 request);

	[OperationContract(Action = "invalidateAccDocs", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	invalidateAccDocsResponse1 invalidateAccDocs(invalidateAccDocsRequest1 request);

	[OperationContract(Action = "invalidateAccDocs", ReplyAction = "*")]
	Task<invalidateAccDocsResponse1> invalidateAccDocsAsync(invalidateAccDocsRequest1 request);
}
