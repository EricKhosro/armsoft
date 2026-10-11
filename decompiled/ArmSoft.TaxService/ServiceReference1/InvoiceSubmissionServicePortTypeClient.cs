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
internal class InvoiceSubmissionServicePortTypeClient : ClientBase<InvoiceSubmissionServicePortType>, InvoiceSubmissionServicePortType
{
	public enum EndpointConfiguration
	{
		InvoiceSubmissionServicePort
	}

	public InvoiceSubmissionServicePortTypeClient()
		: base(GetDefaultBinding(), GetDefaultEndpointAddress())
	{
		Endpoint.Name = EndpointConfiguration.InvoiceSubmissionServicePort.ToString();
	}

	public InvoiceSubmissionServicePortTypeClient(EndpointConfiguration endpointConfiguration)
		: base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public InvoiceSubmissionServicePortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public InvoiceSubmissionServicePortTypeClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), remoteAddress)
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public InvoiceSubmissionServicePortTypeClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	registerSignedInvoiceResponse1 InvoiceSubmissionServicePortType.registerSignedInvoice(registerSignedInvoiceRequest1 request)
	{
		return Channel.registerSignedInvoice(request);
	}

	public RegisterSignedInvoiceResponse registerSignedInvoice(RegisterSignedInvoiceRequest RegisterSignedInvoiceRequest)
	{
		registerSignedInvoiceRequest1 registerSignedInvoiceRequest2 = new registerSignedInvoiceRequest1();
		registerSignedInvoiceRequest2.RegisterSignedInvoiceRequest = RegisterSignedInvoiceRequest;
		return ((InvoiceSubmissionServicePortType)this).registerSignedInvoice(registerSignedInvoiceRequest2).RegisterSignedInvoiceResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<registerSignedInvoiceResponse1> InvoiceSubmissionServicePortType.registerSignedInvoiceAsync(registerSignedInvoiceRequest1 request)
	{
		return Channel.registerSignedInvoiceAsync(request);
	}

	public Task<registerSignedInvoiceResponse1> registerSignedInvoiceAsync(RegisterSignedInvoiceRequest RegisterSignedInvoiceRequest)
	{
		registerSignedInvoiceRequest1 registerSignedInvoiceRequest2 = new registerSignedInvoiceRequest1();
		registerSignedInvoiceRequest2.RegisterSignedInvoiceRequest = RegisterSignedInvoiceRequest;
		return ((InvoiceSubmissionServicePortType)this).registerSignedInvoiceAsync(registerSignedInvoiceRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	resignInvoiceBySupplierResponse1 InvoiceSubmissionServicePortType.resignInvoiceBySupplier(resignInvoiceBySupplierRequest1 request)
	{
		return Channel.resignInvoiceBySupplier(request);
	}

	public ResignInvoiceBySupplierResponse resignInvoiceBySupplier(ResignInvoiceBySupplierRequest ResignInvoiceBySupplierRequest)
	{
		resignInvoiceBySupplierRequest1 resignInvoiceBySupplierRequest2 = new resignInvoiceBySupplierRequest1();
		resignInvoiceBySupplierRequest2.ResignInvoiceBySupplierRequest = ResignInvoiceBySupplierRequest;
		return ((InvoiceSubmissionServicePortType)this).resignInvoiceBySupplier(resignInvoiceBySupplierRequest2).ResignInvoiceBySupplierResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<resignInvoiceBySupplierResponse1> InvoiceSubmissionServicePortType.resignInvoiceBySupplierAsync(resignInvoiceBySupplierRequest1 request)
	{
		return Channel.resignInvoiceBySupplierAsync(request);
	}

	public Task<resignInvoiceBySupplierResponse1> resignInvoiceBySupplierAsync(ResignInvoiceBySupplierRequest ResignInvoiceBySupplierRequest)
	{
		resignInvoiceBySupplierRequest1 resignInvoiceBySupplierRequest2 = new resignInvoiceBySupplierRequest1();
		resignInvoiceBySupplierRequest2.ResignInvoiceBySupplierRequest = ResignInvoiceBySupplierRequest;
		return ((InvoiceSubmissionServicePortType)this).resignInvoiceBySupplierAsync(resignInvoiceBySupplierRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	checkInvalidateInvoiceResponse1 InvoiceSubmissionServicePortType.checkInvalidateInvoice(checkInvalidateInvoiceRequest1 request)
	{
		return Channel.checkInvalidateInvoice(request);
	}

	public CheckInvalidateInvoiceResponse checkInvalidateInvoice(CheckInvalidateInvoiceRequest CheckInvalidateInvoiceRequest)
	{
		checkInvalidateInvoiceRequest1 checkInvalidateInvoiceRequest2 = new checkInvalidateInvoiceRequest1();
		checkInvalidateInvoiceRequest2.CheckInvalidateInvoiceRequest = CheckInvalidateInvoiceRequest;
		return ((InvoiceSubmissionServicePortType)this).checkInvalidateInvoice(checkInvalidateInvoiceRequest2).CheckInvalidateInvoiceResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<checkInvalidateInvoiceResponse1> InvoiceSubmissionServicePortType.checkInvalidateInvoiceAsync(checkInvalidateInvoiceRequest1 request)
	{
		return Channel.checkInvalidateInvoiceAsync(request);
	}

	public Task<checkInvalidateInvoiceResponse1> checkInvalidateInvoiceAsync(CheckInvalidateInvoiceRequest CheckInvalidateInvoiceRequest)
	{
		checkInvalidateInvoiceRequest1 checkInvalidateInvoiceRequest2 = new checkInvalidateInvoiceRequest1();
		checkInvalidateInvoiceRequest2.CheckInvalidateInvoiceRequest = CheckInvalidateInvoiceRequest;
		return ((InvoiceSubmissionServicePortType)this).checkInvalidateInvoiceAsync(checkInvalidateInvoiceRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	checkInvoiceDeletePermitionResponse1 InvoiceSubmissionServicePortType.checkInvoiceDeletePermition(checkInvoiceDeletePermitionRequest1 request)
	{
		return Channel.checkInvoiceDeletePermition(request);
	}

	public CheckInvoiceDeletePermitionResponse checkInvoiceDeletePermition(CheckInvoiceDeletePermitionRequest CheckInvoiceDeletePermitionRequest)
	{
		checkInvoiceDeletePermitionRequest1 checkInvoiceDeletePermitionRequest2 = new checkInvoiceDeletePermitionRequest1();
		checkInvoiceDeletePermitionRequest2.CheckInvoiceDeletePermitionRequest = CheckInvoiceDeletePermitionRequest;
		return ((InvoiceSubmissionServicePortType)this).checkInvoiceDeletePermition(checkInvoiceDeletePermitionRequest2).CheckInvoiceDeletePermitionResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<checkInvoiceDeletePermitionResponse1> InvoiceSubmissionServicePortType.checkInvoiceDeletePermitionAsync(checkInvoiceDeletePermitionRequest1 request)
	{
		return Channel.checkInvoiceDeletePermitionAsync(request);
	}

	public Task<checkInvoiceDeletePermitionResponse1> checkInvoiceDeletePermitionAsync(CheckInvoiceDeletePermitionRequest CheckInvoiceDeletePermitionRequest)
	{
		checkInvoiceDeletePermitionRequest1 checkInvoiceDeletePermitionRequest2 = new checkInvoiceDeletePermitionRequest1();
		checkInvoiceDeletePermitionRequest2.CheckInvoiceDeletePermitionRequest = CheckInvoiceDeletePermitionRequest;
		return ((InvoiceSubmissionServicePortType)this).checkInvoiceDeletePermitionAsync(checkInvoiceDeletePermitionRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	invalidateInvoiceResponse1 InvoiceSubmissionServicePortType.invalidateInvoice(invalidateInvoiceRequest1 request)
	{
		return Channel.invalidateInvoice(request);
	}

	public InvalidateInvoiceResponse invalidateInvoice(InvalidateInvoiceRequest InvalidateInvoiceRequest)
	{
		invalidateInvoiceRequest1 invalidateInvoiceRequest2 = new invalidateInvoiceRequest1();
		invalidateInvoiceRequest2.InvalidateInvoiceRequest = InvalidateInvoiceRequest;
		return ((InvoiceSubmissionServicePortType)this).invalidateInvoice(invalidateInvoiceRequest2).InvalidateInvoiceResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<invalidateInvoiceResponse1> InvoiceSubmissionServicePortType.invalidateInvoiceAsync(invalidateInvoiceRequest1 request)
	{
		return Channel.invalidateInvoiceAsync(request);
	}

	public Task<invalidateInvoiceResponse1> invalidateInvoiceAsync(InvalidateInvoiceRequest InvalidateInvoiceRequest)
	{
		invalidateInvoiceRequest1 invalidateInvoiceRequest2 = new invalidateInvoiceRequest1();
		invalidateInvoiceRequest2.InvalidateInvoiceRequest = InvalidateInvoiceRequest;
		return ((InvoiceSubmissionServicePortType)this).invalidateInvoiceAsync(invalidateInvoiceRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	checkOverdueInvalidateInvoiceResponse1 InvoiceSubmissionServicePortType.checkOverdueInvalidateInvoice(checkOverdueInvalidateInvoiceRequest1 request)
	{
		return Channel.checkOverdueInvalidateInvoice(request);
	}

	public CheckOverdueInvalidateInvoiceResponse checkOverdueInvalidateInvoice(CheckOverdueInvalidateInvoiceRequest CheckOverdueInvalidateInvoiceRequest)
	{
		checkOverdueInvalidateInvoiceRequest1 checkOverdueInvalidateInvoiceRequest2 = new checkOverdueInvalidateInvoiceRequest1();
		checkOverdueInvalidateInvoiceRequest2.CheckOverdueInvalidateInvoiceRequest = CheckOverdueInvalidateInvoiceRequest;
		return ((InvoiceSubmissionServicePortType)this).checkOverdueInvalidateInvoice(checkOverdueInvalidateInvoiceRequest2).CheckOverdueInvalidateInvoiceResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<checkOverdueInvalidateInvoiceResponse1> InvoiceSubmissionServicePortType.checkOverdueInvalidateInvoiceAsync(checkOverdueInvalidateInvoiceRequest1 request)
	{
		return Channel.checkOverdueInvalidateInvoiceAsync(request);
	}

	public Task<checkOverdueInvalidateInvoiceResponse1> checkOverdueInvalidateInvoiceAsync(CheckOverdueInvalidateInvoiceRequest CheckOverdueInvalidateInvoiceRequest)
	{
		checkOverdueInvalidateInvoiceRequest1 checkOverdueInvalidateInvoiceRequest2 = new checkOverdueInvalidateInvoiceRequest1();
		checkOverdueInvalidateInvoiceRequest2.CheckOverdueInvalidateInvoiceRequest = CheckOverdueInvalidateInvoiceRequest;
		return ((InvoiceSubmissionServicePortType)this).checkOverdueInvalidateInvoiceAsync(checkOverdueInvalidateInvoiceRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	overdueInvalidateInvoiceResponse1 InvoiceSubmissionServicePortType.overdueInvalidateInvoice(overdueInvalidateInvoiceRequest1 request)
	{
		return Channel.overdueInvalidateInvoice(request);
	}

	public OverdueInvalidateInvoiceResponse overdueInvalidateInvoice(OverdueInvalidateInvoiceRequest OverdueInvalidateInvoiceRequest)
	{
		overdueInvalidateInvoiceRequest1 overdueInvalidateInvoiceRequest2 = new overdueInvalidateInvoiceRequest1();
		overdueInvalidateInvoiceRequest2.OverdueInvalidateInvoiceRequest = OverdueInvalidateInvoiceRequest;
		return ((InvoiceSubmissionServicePortType)this).overdueInvalidateInvoice(overdueInvalidateInvoiceRequest2).OverdueInvalidateInvoiceResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<overdueInvalidateInvoiceResponse1> InvoiceSubmissionServicePortType.overdueInvalidateInvoiceAsync(overdueInvalidateInvoiceRequest1 request)
	{
		return Channel.overdueInvalidateInvoiceAsync(request);
	}

	public Task<overdueInvalidateInvoiceResponse1> overdueInvalidateInvoiceAsync(OverdueInvalidateInvoiceRequest OverdueInvalidateInvoiceRequest)
	{
		overdueInvalidateInvoiceRequest1 overdueInvalidateInvoiceRequest2 = new overdueInvalidateInvoiceRequest1();
		overdueInvalidateInvoiceRequest2.OverdueInvalidateInvoiceRequest = OverdueInvalidateInvoiceRequest;
		return ((InvoiceSubmissionServicePortType)this).overdueInvalidateInvoiceAsync(overdueInvalidateInvoiceRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	confirmInvoiceInvalidationRequestResponse1 InvoiceSubmissionServicePortType.confirmInvoiceInvalidationRequest(confirmInvoiceInvalidationRequestRequest1 request)
	{
		return Channel.confirmInvoiceInvalidationRequest(request);
	}

	public ConfirmInvoiceInvalidationRequestResponse confirmInvoiceInvalidationRequest(ConfirmInvoiceInvalidationRequestRequest ConfirmInvoiceInvalidationRequestRequest)
	{
		confirmInvoiceInvalidationRequestRequest1 confirmInvoiceInvalidationRequestRequest2 = new confirmInvoiceInvalidationRequestRequest1();
		confirmInvoiceInvalidationRequestRequest2.ConfirmInvoiceInvalidationRequestRequest = ConfirmInvoiceInvalidationRequestRequest;
		return ((InvoiceSubmissionServicePortType)this).confirmInvoiceInvalidationRequest(confirmInvoiceInvalidationRequestRequest2).ConfirmInvoiceInvalidationRequestResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<confirmInvoiceInvalidationRequestResponse1> InvoiceSubmissionServicePortType.confirmInvoiceInvalidationRequestAsync(confirmInvoiceInvalidationRequestRequest1 request)
	{
		return Channel.confirmInvoiceInvalidationRequestAsync(request);
	}

	public Task<confirmInvoiceInvalidationRequestResponse1> confirmInvoiceInvalidationRequestAsync(ConfirmInvoiceInvalidationRequestRequest ConfirmInvoiceInvalidationRequestRequest)
	{
		confirmInvoiceInvalidationRequestRequest1 confirmInvoiceInvalidationRequestRequest2 = new confirmInvoiceInvalidationRequestRequest1();
		confirmInvoiceInvalidationRequestRequest2.ConfirmInvoiceInvalidationRequestRequest = ConfirmInvoiceInvalidationRequestRequest;
		return ((InvoiceSubmissionServicePortType)this).confirmInvoiceInvalidationRequestAsync(confirmInvoiceInvalidationRequestRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	rejectInvoiceInvalidationRequestResponse1 InvoiceSubmissionServicePortType.rejectInvoiceInvalidationRequest(rejectInvoiceInvalidationRequestRequest1 request)
	{
		return Channel.rejectInvoiceInvalidationRequest(request);
	}

	public RejectInvoiceInvalidationRequestResponse rejectInvoiceInvalidationRequest(RejectInvoiceInvalidationRequestRequest RejectInvoiceInvalidationRequestRequest)
	{
		rejectInvoiceInvalidationRequestRequest1 rejectInvoiceInvalidationRequestRequest2 = new rejectInvoiceInvalidationRequestRequest1();
		rejectInvoiceInvalidationRequestRequest2.RejectInvoiceInvalidationRequestRequest = RejectInvoiceInvalidationRequestRequest;
		return ((InvoiceSubmissionServicePortType)this).rejectInvoiceInvalidationRequest(rejectInvoiceInvalidationRequestRequest2).RejectInvoiceInvalidationRequestResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<rejectInvoiceInvalidationRequestResponse1> InvoiceSubmissionServicePortType.rejectInvoiceInvalidationRequestAsync(rejectInvoiceInvalidationRequestRequest1 request)
	{
		return Channel.rejectInvoiceInvalidationRequestAsync(request);
	}

	public Task<rejectInvoiceInvalidationRequestResponse1> rejectInvoiceInvalidationRequestAsync(RejectInvoiceInvalidationRequestRequest RejectInvoiceInvalidationRequestRequest)
	{
		rejectInvoiceInvalidationRequestRequest1 rejectInvoiceInvalidationRequestRequest2 = new rejectInvoiceInvalidationRequestRequest1();
		rejectInvoiceInvalidationRequestRequest2.RejectInvoiceInvalidationRequestRequest = RejectInvoiceInvalidationRequestRequest;
		return ((InvoiceSubmissionServicePortType)this).rejectInvoiceInvalidationRequestAsync(rejectInvoiceInvalidationRequestRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	deleteInvoiceRequestResponse1 InvoiceSubmissionServicePortType.deleteInvoiceRequest(deleteInvoiceRequestRequest1 request)
	{
		return Channel.deleteInvoiceRequest(request);
	}

	public DeleteInvoiceRequestResponse deleteInvoiceRequest(DeleteInvoiceRequestRequest DeleteInvoiceRequestRequest)
	{
		deleteInvoiceRequestRequest1 deleteInvoiceRequestRequest2 = new deleteInvoiceRequestRequest1();
		deleteInvoiceRequestRequest2.DeleteInvoiceRequestRequest = DeleteInvoiceRequestRequest;
		return ((InvoiceSubmissionServicePortType)this).deleteInvoiceRequest(deleteInvoiceRequestRequest2).DeleteInvoiceRequestResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<deleteInvoiceRequestResponse1> InvoiceSubmissionServicePortType.deleteInvoiceRequestAsync(deleteInvoiceRequestRequest1 request)
	{
		return Channel.deleteInvoiceRequestAsync(request);
	}

	public Task<deleteInvoiceRequestResponse1> deleteInvoiceRequestAsync(DeleteInvoiceRequestRequest DeleteInvoiceRequestRequest)
	{
		deleteInvoiceRequestRequest1 deleteInvoiceRequestRequest2 = new deleteInvoiceRequestRequest1();
		deleteInvoiceRequestRequest2.DeleteInvoiceRequestRequest = DeleteInvoiceRequestRequest;
		return ((InvoiceSubmissionServicePortType)this).deleteInvoiceRequestAsync(deleteInvoiceRequestRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	checkRevokeSignatureResponse InvoiceSubmissionServicePortType.checkRevokeSignature(checkRevokeSignatureRequest1 request)
	{
		return Channel.checkRevokeSignature(request);
	}

	public CheckRevokeSingatureResponse checkRevokeSignature(CheckRevokeSignatureRequest CheckRevokeSignatureRequest)
	{
		checkRevokeSignatureRequest1 checkRevokeSignatureRequest2 = new checkRevokeSignatureRequest1();
		checkRevokeSignatureRequest2.CheckRevokeSignatureRequest = CheckRevokeSignatureRequest;
		return ((InvoiceSubmissionServicePortType)this).checkRevokeSignature(checkRevokeSignatureRequest2).CheckRevokeSingatureResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<checkRevokeSignatureResponse> InvoiceSubmissionServicePortType.checkRevokeSignatureAsync(checkRevokeSignatureRequest1 request)
	{
		return Channel.checkRevokeSignatureAsync(request);
	}

	public Task<checkRevokeSignatureResponse> checkRevokeSignatureAsync(CheckRevokeSignatureRequest CheckRevokeSignatureRequest)
	{
		checkRevokeSignatureRequest1 checkRevokeSignatureRequest2 = new checkRevokeSignatureRequest1();
		checkRevokeSignatureRequest2.CheckRevokeSignatureRequest = CheckRevokeSignatureRequest;
		return ((InvoiceSubmissionServicePortType)this).checkRevokeSignatureAsync(checkRevokeSignatureRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	revokeSignatureResponse InvoiceSubmissionServicePortType.revokeSignature(revokeSignatureRequest1 request)
	{
		return Channel.revokeSignature(request);
	}

	public RevokeSingatureResponse revokeSignature(RevokeSignatureRequest RevokeSignatureRequest)
	{
		revokeSignatureRequest1 revokeSignatureRequest2 = new revokeSignatureRequest1();
		revokeSignatureRequest2.RevokeSignatureRequest = RevokeSignatureRequest;
		return ((InvoiceSubmissionServicePortType)this).revokeSignature(revokeSignatureRequest2).RevokeSingatureResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<revokeSignatureResponse> InvoiceSubmissionServicePortType.revokeSignatureAsync(revokeSignatureRequest1 request)
	{
		return Channel.revokeSignatureAsync(request);
	}

	public Task<revokeSignatureResponse> revokeSignatureAsync(RevokeSignatureRequest RevokeSignatureRequest)
	{
		revokeSignatureRequest1 revokeSignatureRequest2 = new revokeSignatureRequest1();
		revokeSignatureRequest2.RevokeSignatureRequest = RevokeSignatureRequest;
		return ((InvoiceSubmissionServicePortType)this).revokeSignatureAsync(revokeSignatureRequest2);
	}

	public new virtual Task OpenAsync()
	{
		return Task.Factory.FromAsync(((ICommunicationObject)this).BeginOpen((AsyncCallback)null, (object)null), ((ICommunicationObject)this).EndOpen);
	}

	private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == EndpointConfiguration.InvoiceSubmissionServicePort)
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
		if (endpointConfiguration == EndpointConfiguration.InvoiceSubmissionServicePort)
		{
			return new EndpointAddress("http://ews.taxservice.am/taxsystem-fe-ws/taxpayer/invoiceSubmissionService");
		}
		throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'.");
	}

	private static Binding GetDefaultBinding()
	{
		return GetBindingForEndpoint(EndpointConfiguration.InvoiceSubmissionServicePort);
	}

	private static EndpointAddress GetDefaultEndpointAddress()
	{
		return GetEndpointAddress(EndpointConfiguration.InvoiceSubmissionServicePort);
	}
}
