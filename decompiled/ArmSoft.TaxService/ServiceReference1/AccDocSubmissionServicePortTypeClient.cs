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
internal class AccDocSubmissionServicePortTypeClient : ClientBase<AccDocSubmissionServicePortType>, AccDocSubmissionServicePortType
{
	public enum EndpointConfiguration
	{
		AccDocSubmissionServicePort
	}

	public AccDocSubmissionServicePortTypeClient()
		: base(GetDefaultBinding(), GetDefaultEndpointAddress())
	{
		Endpoint.Name = EndpointConfiguration.AccDocSubmissionServicePort.ToString();
	}

	public AccDocSubmissionServicePortTypeClient(EndpointConfiguration endpointConfiguration)
		: base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public AccDocSubmissionServicePortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public AccDocSubmissionServicePortTypeClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), remoteAddress)
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public AccDocSubmissionServicePortTypeClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	registerSignedAccDocResponse1 AccDocSubmissionServicePortType.registerSignedAccDoc(registerSignedAccDocRequest1 request)
	{
		return Channel.registerSignedAccDoc(request);
	}

	public RegisterSignedAccDocResponse registerSignedAccDoc(RegisterSignedAccDocRequest RegisterSignedAccDocRequest)
	{
		registerSignedAccDocRequest1 registerSignedAccDocRequest2 = new registerSignedAccDocRequest1();
		registerSignedAccDocRequest2.RegisterSignedAccDocRequest = RegisterSignedAccDocRequest;
		return ((AccDocSubmissionServicePortType)this).registerSignedAccDoc(registerSignedAccDocRequest2).RegisterSignedAccDocResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<registerSignedAccDocResponse1> AccDocSubmissionServicePortType.registerSignedAccDocAsync(registerSignedAccDocRequest1 request)
	{
		return Channel.registerSignedAccDocAsync(request);
	}

	public Task<registerSignedAccDocResponse1> registerSignedAccDocAsync(RegisterSignedAccDocRequest RegisterSignedAccDocRequest)
	{
		registerSignedAccDocRequest1 registerSignedAccDocRequest2 = new registerSignedAccDocRequest1();
		registerSignedAccDocRequest2.RegisterSignedAccDocRequest = RegisterSignedAccDocRequest;
		return ((AccDocSubmissionServicePortType)this).registerSignedAccDocAsync(registerSignedAccDocRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	resignAccDocBySupplierResponse1 AccDocSubmissionServicePortType.resignAccDocBySupplier(resignAccDocBySupplierRequest1 request)
	{
		return Channel.resignAccDocBySupplier(request);
	}

	public ResignAccDocBySupplierResponse resignAccDocBySupplier(ResignAccDocBySupplierRequest ResignAccDocBySupplierRequest)
	{
		resignAccDocBySupplierRequest1 resignAccDocBySupplierRequest2 = new resignAccDocBySupplierRequest1();
		resignAccDocBySupplierRequest2.ResignAccDocBySupplierRequest = ResignAccDocBySupplierRequest;
		return ((AccDocSubmissionServicePortType)this).resignAccDocBySupplier(resignAccDocBySupplierRequest2).ResignAccDocBySupplierResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<resignAccDocBySupplierResponse1> AccDocSubmissionServicePortType.resignAccDocBySupplierAsync(resignAccDocBySupplierRequest1 request)
	{
		return Channel.resignAccDocBySupplierAsync(request);
	}

	public Task<resignAccDocBySupplierResponse1> resignAccDocBySupplierAsync(ResignAccDocBySupplierRequest ResignAccDocBySupplierRequest)
	{
		resignAccDocBySupplierRequest1 resignAccDocBySupplierRequest2 = new resignAccDocBySupplierRequest1();
		resignAccDocBySupplierRequest2.ResignAccDocBySupplierRequest = ResignAccDocBySupplierRequest;
		return ((AccDocSubmissionServicePortType)this).resignAccDocBySupplierAsync(resignAccDocBySupplierRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	checkInvalidateAccDocResponse1 AccDocSubmissionServicePortType.checkInvalidateAccDoc(checkInvalidateAccDocRequest1 request)
	{
		return Channel.checkInvalidateAccDoc(request);
	}

	public CheckInvalidateAccDocResponse checkInvalidateAccDoc(CheckInvalidateAccDocRequest CheckInvalidateAccDocRequest)
	{
		checkInvalidateAccDocRequest1 checkInvalidateAccDocRequest2 = new checkInvalidateAccDocRequest1();
		checkInvalidateAccDocRequest2.CheckInvalidateAccDocRequest = CheckInvalidateAccDocRequest;
		return ((AccDocSubmissionServicePortType)this).checkInvalidateAccDoc(checkInvalidateAccDocRequest2).CheckInvalidateAccDocResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<checkInvalidateAccDocResponse1> AccDocSubmissionServicePortType.checkInvalidateAccDocAsync(checkInvalidateAccDocRequest1 request)
	{
		return Channel.checkInvalidateAccDocAsync(request);
	}

	public Task<checkInvalidateAccDocResponse1> checkInvalidateAccDocAsync(CheckInvalidateAccDocRequest CheckInvalidateAccDocRequest)
	{
		checkInvalidateAccDocRequest1 checkInvalidateAccDocRequest2 = new checkInvalidateAccDocRequest1();
		checkInvalidateAccDocRequest2.CheckInvalidateAccDocRequest = CheckInvalidateAccDocRequest;
		return ((AccDocSubmissionServicePortType)this).checkInvalidateAccDocAsync(checkInvalidateAccDocRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	checkAccDocDeletePermitionResponse1 AccDocSubmissionServicePortType.checkAccDocDeletePermition(checkAccDocDeletePermitionRequest1 request)
	{
		return Channel.checkAccDocDeletePermition(request);
	}

	public CheckAccDocDeletePermitionResponse checkAccDocDeletePermition(CheckAccDocDeletePermitionRequest CheckAccDocDeletePermitionRequest)
	{
		checkAccDocDeletePermitionRequest1 checkAccDocDeletePermitionRequest2 = new checkAccDocDeletePermitionRequest1();
		checkAccDocDeletePermitionRequest2.CheckAccDocDeletePermitionRequest = CheckAccDocDeletePermitionRequest;
		return ((AccDocSubmissionServicePortType)this).checkAccDocDeletePermition(checkAccDocDeletePermitionRequest2).CheckAccDocDeletePermitionResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<checkAccDocDeletePermitionResponse1> AccDocSubmissionServicePortType.checkAccDocDeletePermitionAsync(checkAccDocDeletePermitionRequest1 request)
	{
		return Channel.checkAccDocDeletePermitionAsync(request);
	}

	public Task<checkAccDocDeletePermitionResponse1> checkAccDocDeletePermitionAsync(CheckAccDocDeletePermitionRequest CheckAccDocDeletePermitionRequest)
	{
		checkAccDocDeletePermitionRequest1 checkAccDocDeletePermitionRequest2 = new checkAccDocDeletePermitionRequest1();
		checkAccDocDeletePermitionRequest2.CheckAccDocDeletePermitionRequest = CheckAccDocDeletePermitionRequest;
		return ((AccDocSubmissionServicePortType)this).checkAccDocDeletePermitionAsync(checkAccDocDeletePermitionRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	invalidateAccDocResponse1 AccDocSubmissionServicePortType.invalidateAccDoc(invalidateAccDocRequest1 request)
	{
		return Channel.invalidateAccDoc(request);
	}

	public InvalidateAccDocResponse invalidateAccDoc(InvalidateAccDocRequest InvalidateAccDocRequest)
	{
		invalidateAccDocRequest1 invalidateAccDocRequest2 = new invalidateAccDocRequest1();
		invalidateAccDocRequest2.InvalidateAccDocRequest = InvalidateAccDocRequest;
		return ((AccDocSubmissionServicePortType)this).invalidateAccDoc(invalidateAccDocRequest2).InvalidateAccDocResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<invalidateAccDocResponse1> AccDocSubmissionServicePortType.invalidateAccDocAsync(invalidateAccDocRequest1 request)
	{
		return Channel.invalidateAccDocAsync(request);
	}

	public Task<invalidateAccDocResponse1> invalidateAccDocAsync(InvalidateAccDocRequest InvalidateAccDocRequest)
	{
		invalidateAccDocRequest1 invalidateAccDocRequest2 = new invalidateAccDocRequest1();
		invalidateAccDocRequest2.InvalidateAccDocRequest = InvalidateAccDocRequest;
		return ((AccDocSubmissionServicePortType)this).invalidateAccDocAsync(invalidateAccDocRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	confirmAccDocInvalidationRequestResponse1 AccDocSubmissionServicePortType.confirmAccDocInvalidationRequest(confirmAccDocInvalidationRequestRequest1 request)
	{
		return Channel.confirmAccDocInvalidationRequest(request);
	}

	public ConfirmAccDocInvalidationRequestResponse confirmAccDocInvalidationRequest(ConfirmAccDocInvalidationRequestRequest ConfirmAccDocInvalidationRequestRequest)
	{
		confirmAccDocInvalidationRequestRequest1 confirmAccDocInvalidationRequestRequest2 = new confirmAccDocInvalidationRequestRequest1();
		confirmAccDocInvalidationRequestRequest2.ConfirmAccDocInvalidationRequestRequest = ConfirmAccDocInvalidationRequestRequest;
		return ((AccDocSubmissionServicePortType)this).confirmAccDocInvalidationRequest(confirmAccDocInvalidationRequestRequest2).ConfirmAccDocInvalidationRequestResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<confirmAccDocInvalidationRequestResponse1> AccDocSubmissionServicePortType.confirmAccDocInvalidationRequestAsync(confirmAccDocInvalidationRequestRequest1 request)
	{
		return Channel.confirmAccDocInvalidationRequestAsync(request);
	}

	public Task<confirmAccDocInvalidationRequestResponse1> confirmAccDocInvalidationRequestAsync(ConfirmAccDocInvalidationRequestRequest ConfirmAccDocInvalidationRequestRequest)
	{
		confirmAccDocInvalidationRequestRequest1 confirmAccDocInvalidationRequestRequest2 = new confirmAccDocInvalidationRequestRequest1();
		confirmAccDocInvalidationRequestRequest2.ConfirmAccDocInvalidationRequestRequest = ConfirmAccDocInvalidationRequestRequest;
		return ((AccDocSubmissionServicePortType)this).confirmAccDocInvalidationRequestAsync(confirmAccDocInvalidationRequestRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	rejectAccDocInvalidationRequestResponse1 AccDocSubmissionServicePortType.rejectAccDocInvalidationRequest(rejectAccDocInvalidationRequestRequest1 request)
	{
		return Channel.rejectAccDocInvalidationRequest(request);
	}

	public RejectAccDocInvalidationRequestResponse rejectAccDocInvalidationRequest(RejectAccDocInvalidationRequestRequest RejectAccDocInvalidationRequestRequest)
	{
		rejectAccDocInvalidationRequestRequest1 rejectAccDocInvalidationRequestRequest2 = new rejectAccDocInvalidationRequestRequest1();
		rejectAccDocInvalidationRequestRequest2.RejectAccDocInvalidationRequestRequest = RejectAccDocInvalidationRequestRequest;
		return ((AccDocSubmissionServicePortType)this).rejectAccDocInvalidationRequest(rejectAccDocInvalidationRequestRequest2).RejectAccDocInvalidationRequestResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<rejectAccDocInvalidationRequestResponse1> AccDocSubmissionServicePortType.rejectAccDocInvalidationRequestAsync(rejectAccDocInvalidationRequestRequest1 request)
	{
		return Channel.rejectAccDocInvalidationRequestAsync(request);
	}

	public Task<rejectAccDocInvalidationRequestResponse1> rejectAccDocInvalidationRequestAsync(RejectAccDocInvalidationRequestRequest RejectAccDocInvalidationRequestRequest)
	{
		rejectAccDocInvalidationRequestRequest1 rejectAccDocInvalidationRequestRequest2 = new rejectAccDocInvalidationRequestRequest1();
		rejectAccDocInvalidationRequestRequest2.RejectAccDocInvalidationRequestRequest = RejectAccDocInvalidationRequestRequest;
		return ((AccDocSubmissionServicePortType)this).rejectAccDocInvalidationRequestAsync(rejectAccDocInvalidationRequestRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	deleteAccDocRequestResponse1 AccDocSubmissionServicePortType.deleteAccDocRequest(deleteAccDocRequestRequest1 request)
	{
		return Channel.deleteAccDocRequest(request);
	}

	public DeleteAccDocRequestResponse deleteAccDocRequest(DeleteAccDocRequestRequest DeleteAccDocRequestRequest)
	{
		deleteAccDocRequestRequest1 deleteAccDocRequestRequest2 = new deleteAccDocRequestRequest1();
		deleteAccDocRequestRequest2.DeleteAccDocRequestRequest = DeleteAccDocRequestRequest;
		return ((AccDocSubmissionServicePortType)this).deleteAccDocRequest(deleteAccDocRequestRequest2).DeleteAccDocRequestResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<deleteAccDocRequestResponse1> AccDocSubmissionServicePortType.deleteAccDocRequestAsync(deleteAccDocRequestRequest1 request)
	{
		return Channel.deleteAccDocRequestAsync(request);
	}

	public Task<deleteAccDocRequestResponse1> deleteAccDocRequestAsync(DeleteAccDocRequestRequest DeleteAccDocRequestRequest)
	{
		deleteAccDocRequestRequest1 deleteAccDocRequestRequest2 = new deleteAccDocRequestRequest1();
		deleteAccDocRequestRequest2.DeleteAccDocRequestRequest = DeleteAccDocRequestRequest;
		return ((AccDocSubmissionServicePortType)this).deleteAccDocRequestAsync(deleteAccDocRequestRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	checkOverdueInvalidateAccDocResponse1 AccDocSubmissionServicePortType.checkOverdueInvalidateAccDoc(checkOverdueInvalidateAccDocRequest1 request)
	{
		return Channel.checkOverdueInvalidateAccDoc(request);
	}

	public CheckOverdueInvalidateAccDocResponse checkOverdueInvalidateAccDoc(CheckOverdueInvalidateAccDocRequest CheckOverdueInvalidateAccDocRequest)
	{
		checkOverdueInvalidateAccDocRequest1 checkOverdueInvalidateAccDocRequest2 = new checkOverdueInvalidateAccDocRequest1();
		checkOverdueInvalidateAccDocRequest2.CheckOverdueInvalidateAccDocRequest = CheckOverdueInvalidateAccDocRequest;
		return ((AccDocSubmissionServicePortType)this).checkOverdueInvalidateAccDoc(checkOverdueInvalidateAccDocRequest2).CheckOverdueInvalidateAccDocResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<checkOverdueInvalidateAccDocResponse1> AccDocSubmissionServicePortType.checkOverdueInvalidateAccDocAsync(checkOverdueInvalidateAccDocRequest1 request)
	{
		return Channel.checkOverdueInvalidateAccDocAsync(request);
	}

	public Task<checkOverdueInvalidateAccDocResponse1> checkOverdueInvalidateAccDocAsync(CheckOverdueInvalidateAccDocRequest CheckOverdueInvalidateAccDocRequest)
	{
		checkOverdueInvalidateAccDocRequest1 checkOverdueInvalidateAccDocRequest2 = new checkOverdueInvalidateAccDocRequest1();
		checkOverdueInvalidateAccDocRequest2.CheckOverdueInvalidateAccDocRequest = CheckOverdueInvalidateAccDocRequest;
		return ((AccDocSubmissionServicePortType)this).checkOverdueInvalidateAccDocAsync(checkOverdueInvalidateAccDocRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	overdueInvalidateAccDocResponse1 AccDocSubmissionServicePortType.overdueInvalidateAccDoc(overdueInvalidateAccDocRequest1 request)
	{
		return Channel.overdueInvalidateAccDoc(request);
	}

	public OverdueInvalidateAccDocResponse overdueInvalidateAccDoc(OverdueInvalidateAccDocRequest OverdueInvalidateAccDocRequest)
	{
		overdueInvalidateAccDocRequest1 overdueInvalidateAccDocRequest2 = new overdueInvalidateAccDocRequest1();
		overdueInvalidateAccDocRequest2.OverdueInvalidateAccDocRequest = OverdueInvalidateAccDocRequest;
		return ((AccDocSubmissionServicePortType)this).overdueInvalidateAccDoc(overdueInvalidateAccDocRequest2).OverdueInvalidateAccDocResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<overdueInvalidateAccDocResponse1> AccDocSubmissionServicePortType.overdueInvalidateAccDocAsync(overdueInvalidateAccDocRequest1 request)
	{
		return Channel.overdueInvalidateAccDocAsync(request);
	}

	public Task<overdueInvalidateAccDocResponse1> overdueInvalidateAccDocAsync(OverdueInvalidateAccDocRequest OverdueInvalidateAccDocRequest)
	{
		overdueInvalidateAccDocRequest1 overdueInvalidateAccDocRequest2 = new overdueInvalidateAccDocRequest1();
		overdueInvalidateAccDocRequest2.OverdueInvalidateAccDocRequest = OverdueInvalidateAccDocRequest;
		return ((AccDocSubmissionServicePortType)this).overdueInvalidateAccDocAsync(overdueInvalidateAccDocRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	checkRevokeAccDocSignatureResponse AccDocSubmissionServicePortType.checkRevokeAccDocSignature(checkRevokeAccDocSignatureRequest1 request)
	{
		return Channel.checkRevokeAccDocSignature(request);
	}

	public CheckAccDocRevokeSingatureResponse checkRevokeAccDocSignature(CheckRevokeAccDocSignatureRequest CheckRevokeAccDocSignatureRequest)
	{
		checkRevokeAccDocSignatureRequest1 checkRevokeAccDocSignatureRequest2 = new checkRevokeAccDocSignatureRequest1();
		checkRevokeAccDocSignatureRequest2.CheckRevokeAccDocSignatureRequest = CheckRevokeAccDocSignatureRequest;
		return ((AccDocSubmissionServicePortType)this).checkRevokeAccDocSignature(checkRevokeAccDocSignatureRequest2).CheckAccDocRevokeSingatureResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<checkRevokeAccDocSignatureResponse> AccDocSubmissionServicePortType.checkRevokeAccDocSignatureAsync(checkRevokeAccDocSignatureRequest1 request)
	{
		return Channel.checkRevokeAccDocSignatureAsync(request);
	}

	public Task<checkRevokeAccDocSignatureResponse> checkRevokeAccDocSignatureAsync(CheckRevokeAccDocSignatureRequest CheckRevokeAccDocSignatureRequest)
	{
		checkRevokeAccDocSignatureRequest1 checkRevokeAccDocSignatureRequest2 = new checkRevokeAccDocSignatureRequest1();
		checkRevokeAccDocSignatureRequest2.CheckRevokeAccDocSignatureRequest = CheckRevokeAccDocSignatureRequest;
		return ((AccDocSubmissionServicePortType)this).checkRevokeAccDocSignatureAsync(checkRevokeAccDocSignatureRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	revokeAccDocSignatureResponse AccDocSubmissionServicePortType.revokeAccDocSignature(revokeAccDocSignatureRequest1 request)
	{
		return Channel.revokeAccDocSignature(request);
	}

	public RevokeAccDocSingatureResponse revokeAccDocSignature(RevokeAccDocSignatureRequest RevokeAccDocSignatureRequest)
	{
		revokeAccDocSignatureRequest1 revokeAccDocSignatureRequest2 = new revokeAccDocSignatureRequest1();
		revokeAccDocSignatureRequest2.RevokeAccDocSignatureRequest = RevokeAccDocSignatureRequest;
		return ((AccDocSubmissionServicePortType)this).revokeAccDocSignature(revokeAccDocSignatureRequest2).RevokeAccDocSingatureResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<revokeAccDocSignatureResponse> AccDocSubmissionServicePortType.revokeAccDocSignatureAsync(revokeAccDocSignatureRequest1 request)
	{
		return Channel.revokeAccDocSignatureAsync(request);
	}

	public Task<revokeAccDocSignatureResponse> revokeAccDocSignatureAsync(RevokeAccDocSignatureRequest RevokeAccDocSignatureRequest)
	{
		revokeAccDocSignatureRequest1 revokeAccDocSignatureRequest2 = new revokeAccDocSignatureRequest1();
		revokeAccDocSignatureRequest2.RevokeAccDocSignatureRequest = RevokeAccDocSignatureRequest;
		return ((AccDocSubmissionServicePortType)this).revokeAccDocSignatureAsync(revokeAccDocSignatureRequest2);
	}

	public new virtual Task OpenAsync()
	{
		return Task.Factory.FromAsync(((ICommunicationObject)this).BeginOpen((AsyncCallback)null, (object)null), ((ICommunicationObject)this).EndOpen);
	}

	private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == EndpointConfiguration.AccDocSubmissionServicePort)
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
		if (endpointConfiguration == EndpointConfiguration.AccDocSubmissionServicePort)
		{
			return new EndpointAddress("http://ews.taxservice.am/taxsystem-fe-ws/taxpayer/accDocSubmissionService");
		}
		throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'.");
	}

	private static Binding GetDefaultBinding()
	{
		return GetBindingForEndpoint(EndpointConfiguration.AccDocSubmissionServicePort);
	}

	private static EndpointAddress GetDefaultEndpointAddress()
	{
		return GetEndpointAddress(EndpointConfiguration.AccDocSubmissionServicePort);
	}
}
