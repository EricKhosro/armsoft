using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Xml;

namespace ServiceReference1;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
internal class BatchServicePortTypeClient : ClientBase<BatchServicePortType>, BatchServicePortType
{
	public enum EndpointConfiguration
	{
		BatchServicePort
	}

	public BatchServicePortTypeClient()
		: base(GetDefaultBinding(), GetDefaultEndpointAddress())
	{
		Endpoint.Name = EndpointConfiguration.BatchServicePort.ToString();
	}

	public BatchServicePortTypeClient(EndpointConfiguration endpointConfiguration)
		: base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public BatchServicePortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public BatchServicePortTypeClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), remoteAddress)
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public BatchServicePortTypeClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getInvoiceNumberBatchResponse1 BatchServicePortType.getInvoiceNumberBatch(getInvoiceNumberBatchRequest1 request)
	{
		return Channel.getInvoiceNumberBatch(request);
	}

	public GetInvoiceNumberBatchResponse getInvoiceNumberBatch(GetInvoiceNumberBatchRequest GetInvoiceNumberBatchRequest)
	{
		getInvoiceNumberBatchRequest1 getInvoiceNumberBatchRequest2 = new getInvoiceNumberBatchRequest1();
		getInvoiceNumberBatchRequest2.GetInvoiceNumberBatchRequest = GetInvoiceNumberBatchRequest;
		return ((BatchServicePortType)this).getInvoiceNumberBatch(getInvoiceNumberBatchRequest2).GetInvoiceNumberBatchResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getInvoiceNumberBatchResponse1> BatchServicePortType.getInvoiceNumberBatchAsync(getInvoiceNumberBatchRequest1 request)
	{
		return Channel.getInvoiceNumberBatchAsync(request);
	}

	public Task<getInvoiceNumberBatchResponse1> getInvoiceNumberBatchAsync(GetInvoiceNumberBatchRequest GetInvoiceNumberBatchRequest)
	{
		getInvoiceNumberBatchRequest1 getInvoiceNumberBatchRequest2 = new getInvoiceNumberBatchRequest1();
		getInvoiceNumberBatchRequest2.GetInvoiceNumberBatchRequest = GetInvoiceNumberBatchRequest;
		return ((BatchServicePortType)this).getInvoiceNumberBatchAsync(getInvoiceNumberBatchRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	registerInvoiceBatchResponse1 BatchServicePortType.registerInvoiceBatch(registerInvoiceBatchRequest1 request)
	{
		return Channel.registerInvoiceBatch(request);
	}

	public RegisterInvoiceBatchResponse registerInvoiceBatch(RegisterInvoiceBatchRequest RegisterInvoiceBatchRequest)
	{
		registerInvoiceBatchRequest1 registerInvoiceBatchRequest2 = new registerInvoiceBatchRequest1();
		registerInvoiceBatchRequest2.RegisterInvoiceBatchRequest = RegisterInvoiceBatchRequest;
		return ((BatchServicePortType)this).registerInvoiceBatch(registerInvoiceBatchRequest2).RegisterInvoiceBatchResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<registerInvoiceBatchResponse1> BatchServicePortType.registerInvoiceBatchAsync(registerInvoiceBatchRequest1 request)
	{
		return Channel.registerInvoiceBatchAsync(request);
	}

	public Task<registerInvoiceBatchResponse1> registerInvoiceBatchAsync(RegisterInvoiceBatchRequest RegisterInvoiceBatchRequest)
	{
		registerInvoiceBatchRequest1 registerInvoiceBatchRequest2 = new registerInvoiceBatchRequest1();
		registerInvoiceBatchRequest2.RegisterInvoiceBatchRequest = RegisterInvoiceBatchRequest;
		return ((BatchServicePortType)this).registerInvoiceBatchAsync(registerInvoiceBatchRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	invalidateInvoicesResponse1 BatchServicePortType.invalidateInvoices(invalidateInvoicesRequest1 request)
	{
		return Channel.invalidateInvoices(request);
	}

	public InvalidateInvoicesResponse invalidateInvoices(InvalidateInvoicesRequest InvalidateInvoicesRequest)
	{
		invalidateInvoicesRequest1 invalidateInvoicesRequest2 = new invalidateInvoicesRequest1();
		invalidateInvoicesRequest2.InvalidateInvoicesRequest = InvalidateInvoicesRequest;
		return ((BatchServicePortType)this).invalidateInvoices(invalidateInvoicesRequest2).InvalidateInvoicesResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<invalidateInvoicesResponse1> BatchServicePortType.invalidateInvoicesAsync(invalidateInvoicesRequest1 request)
	{
		return Channel.invalidateInvoicesAsync(request);
	}

	public Task<invalidateInvoicesResponse1> invalidateInvoicesAsync(InvalidateInvoicesRequest InvalidateInvoicesRequest)
	{
		invalidateInvoicesRequest1 invalidateInvoicesRequest2 = new invalidateInvoicesRequest1();
		invalidateInvoicesRequest2.InvalidateInvoicesRequest = InvalidateInvoicesRequest;
		return ((BatchServicePortType)this).invalidateInvoicesAsync(invalidateInvoicesRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getAccDocNumberBatchResponse1 BatchServicePortType.getAccDocNumberBatch(getAccDocNumberBatchRequest1 request)
	{
		return Channel.getAccDocNumberBatch(request);
	}

	public GetAccDocNumberBatchResponse getAccDocNumberBatch(GetAccDocNumberBatchRequest GetAccDocNumberBatchRequest)
	{
		getAccDocNumberBatchRequest1 getAccDocNumberBatchRequest2 = new getAccDocNumberBatchRequest1();
		getAccDocNumberBatchRequest2.GetAccDocNumberBatchRequest = GetAccDocNumberBatchRequest;
		return ((BatchServicePortType)this).getAccDocNumberBatch(getAccDocNumberBatchRequest2).GetAccDocNumberBatchResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getAccDocNumberBatchResponse1> BatchServicePortType.getAccDocNumberBatchAsync(getAccDocNumberBatchRequest1 request)
	{
		return Channel.getAccDocNumberBatchAsync(request);
	}

	public Task<getAccDocNumberBatchResponse1> getAccDocNumberBatchAsync(GetAccDocNumberBatchRequest GetAccDocNumberBatchRequest)
	{
		getAccDocNumberBatchRequest1 getAccDocNumberBatchRequest2 = new getAccDocNumberBatchRequest1();
		getAccDocNumberBatchRequest2.GetAccDocNumberBatchRequest = GetAccDocNumberBatchRequest;
		return ((BatchServicePortType)this).getAccDocNumberBatchAsync(getAccDocNumberBatchRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	registerAccDocBatchResponse1 BatchServicePortType.registerAccDocBatch(registerAccDocBatchRequest1 request)
	{
		return Channel.registerAccDocBatch(request);
	}

	public RegisterAccDocBatchResponse registerAccDocBatch(RegisterAccDocBatchRequest RegisterAccDocBatchRequest)
	{
		registerAccDocBatchRequest1 registerAccDocBatchRequest2 = new registerAccDocBatchRequest1();
		registerAccDocBatchRequest2.RegisterAccDocBatchRequest = RegisterAccDocBatchRequest;
		return ((BatchServicePortType)this).registerAccDocBatch(registerAccDocBatchRequest2).RegisterAccDocBatchResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<registerAccDocBatchResponse1> BatchServicePortType.registerAccDocBatchAsync(registerAccDocBatchRequest1 request)
	{
		return Channel.registerAccDocBatchAsync(request);
	}

	public Task<registerAccDocBatchResponse1> registerAccDocBatchAsync(RegisterAccDocBatchRequest RegisterAccDocBatchRequest)
	{
		registerAccDocBatchRequest1 registerAccDocBatchRequest2 = new registerAccDocBatchRequest1();
		registerAccDocBatchRequest2.RegisterAccDocBatchRequest = RegisterAccDocBatchRequest;
		return ((BatchServicePortType)this).registerAccDocBatchAsync(registerAccDocBatchRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	invalidateAccDocsResponse1 BatchServicePortType.invalidateAccDocs(invalidateAccDocsRequest1 request)
	{
		return Channel.invalidateAccDocs(request);
	}

	public InvalidateAccDocsResponse invalidateAccDocs(InvalidateAccDocsRequest InvalidateAccDocsRequest)
	{
		invalidateAccDocsRequest1 invalidateAccDocsRequest2 = new invalidateAccDocsRequest1();
		invalidateAccDocsRequest2.InvalidateAccDocsRequest = InvalidateAccDocsRequest;
		return ((BatchServicePortType)this).invalidateAccDocs(invalidateAccDocsRequest2).InvalidateAccDocsResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<invalidateAccDocsResponse1> BatchServicePortType.invalidateAccDocsAsync(invalidateAccDocsRequest1 request)
	{
		return Channel.invalidateAccDocsAsync(request);
	}

	public Task<invalidateAccDocsResponse1> invalidateAccDocsAsync(InvalidateAccDocsRequest InvalidateAccDocsRequest)
	{
		invalidateAccDocsRequest1 invalidateAccDocsRequest2 = new invalidateAccDocsRequest1();
		invalidateAccDocsRequest2.InvalidateAccDocsRequest = InvalidateAccDocsRequest;
		return ((BatchServicePortType)this).invalidateAccDocsAsync(invalidateAccDocsRequest2);
	}

	public new virtual Task OpenAsync()
	{
		return Task.Factory.FromAsync(((ICommunicationObject)this).BeginOpen((AsyncCallback)null, (object)null), ((ICommunicationObject)this).EndOpen);
	}

	private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == EndpointConfiguration.BatchServicePort)
		{
			return new BasicHttpBinding
			{
				MaxBufferSize = int.MaxValue,
				ReaderQuotas = XmlDictionaryReaderQuotas.Max,
				MaxReceivedMessageSize = 2147483647L,
				AllowCookies = true
			};
		}
		throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'.");
	}

	private static EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == EndpointConfiguration.BatchServicePort)
		{
			return new EndpointAddress("http://ews.taxservice.am/taxsystem-fe-ws/taxpayer/batchService");
		}
		throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'.");
	}

	private static Binding GetDefaultBinding()
	{
		return GetBindingForEndpoint(EndpointConfiguration.BatchServicePort);
	}

	private static EndpointAddress GetDefaultEndpointAddress()
	{
		return GetEndpointAddress(EndpointConfiguration.BatchServicePort);
	}
}
