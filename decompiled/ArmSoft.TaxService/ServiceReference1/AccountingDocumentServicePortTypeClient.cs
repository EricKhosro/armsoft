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
internal class AccountingDocumentServicePortTypeClient : ClientBase<AccountingDocumentServicePortType>, AccountingDocumentServicePortType
{
	public enum EndpointConfiguration
	{
		AccountingDocumentServicePort
	}

	public AccountingDocumentServicePortTypeClient()
		: base(GetDefaultBinding(), GetDefaultEndpointAddress())
	{
		Endpoint.Name = EndpointConfiguration.AccountingDocumentServicePort.ToString();
	}

	public AccountingDocumentServicePortTypeClient(EndpointConfiguration endpointConfiguration)
		: base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public AccountingDocumentServicePortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public AccountingDocumentServicePortTypeClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), remoteAddress)
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public AccountingDocumentServicePortTypeClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getAccDocTaxpayerInfoResponse1 AccountingDocumentServicePortType.getAccDocTaxpayerInfo(getAccDocTaxpayerInfoRequest1 request)
	{
		return Channel.getAccDocTaxpayerInfo(request);
	}

	public GetAccDocTaxpayerInfoResponse getAccDocTaxpayerInfo(GetAccDocTaxpayerInfoRequest GetAccDocTaxpayerInfoRequest)
	{
		getAccDocTaxpayerInfoRequest1 getAccDocTaxpayerInfoRequest2 = new getAccDocTaxpayerInfoRequest1();
		getAccDocTaxpayerInfoRequest2.GetAccDocTaxpayerInfoRequest = GetAccDocTaxpayerInfoRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocTaxpayerInfo(getAccDocTaxpayerInfoRequest2).GetAccDocTaxpayerInfoResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getAccDocTaxpayerInfoResponse1> AccountingDocumentServicePortType.getAccDocTaxpayerInfoAsync(getAccDocTaxpayerInfoRequest1 request)
	{
		return Channel.getAccDocTaxpayerInfoAsync(request);
	}

	public Task<getAccDocTaxpayerInfoResponse1> getAccDocTaxpayerInfoAsync(GetAccDocTaxpayerInfoRequest GetAccDocTaxpayerInfoRequest)
	{
		getAccDocTaxpayerInfoRequest1 getAccDocTaxpayerInfoRequest2 = new getAccDocTaxpayerInfoRequest1();
		getAccDocTaxpayerInfoRequest2.GetAccDocTaxpayerInfoRequest = GetAccDocTaxpayerInfoRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocTaxpayerInfoAsync(getAccDocTaxpayerInfoRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	resignAccDocByBuyerResponse1 AccountingDocumentServicePortType.resignAccDocByBuyer(resignAccDocByBuyerRequest1 request)
	{
		return Channel.resignAccDocByBuyer(request);
	}

	public ResignAccDocByBuyerResponse resignAccDocByBuyer(ResignAccDocByBuyerRequest ResignAccDocByBuyerRequest)
	{
		resignAccDocByBuyerRequest1 resignAccDocByBuyerRequest2 = new resignAccDocByBuyerRequest1();
		resignAccDocByBuyerRequest2.ResignAccDocByBuyerRequest = ResignAccDocByBuyerRequest;
		return ((AccountingDocumentServicePortType)this).resignAccDocByBuyer(resignAccDocByBuyerRequest2).ResignAccDocByBuyerResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<resignAccDocByBuyerResponse1> AccountingDocumentServicePortType.resignAccDocByBuyerAsync(resignAccDocByBuyerRequest1 request)
	{
		return Channel.resignAccDocByBuyerAsync(request);
	}

	public Task<resignAccDocByBuyerResponse1> resignAccDocByBuyerAsync(ResignAccDocByBuyerRequest ResignAccDocByBuyerRequest)
	{
		resignAccDocByBuyerRequest1 resignAccDocByBuyerRequest2 = new resignAccDocByBuyerRequest1();
		resignAccDocByBuyerRequest2.ResignAccDocByBuyerRequest = ResignAccDocByBuyerRequest;
		return ((AccountingDocumentServicePortType)this).resignAccDocByBuyerAsync(resignAccDocByBuyerRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getInvoiceNumberResponse1 AccountingDocumentServicePortType.getInvoiceNumber(getInvoiceNumberRequest1 request)
	{
		return Channel.getInvoiceNumber(request);
	}

	public GetInvoiceNumberResponse getInvoiceNumber(GetInvoiceNumberRequest GetInvoiceNumberRequest)
	{
		getInvoiceNumberRequest1 getInvoiceNumberRequest2 = new getInvoiceNumberRequest1();
		getInvoiceNumberRequest2.GetInvoiceNumberRequest = GetInvoiceNumberRequest;
		return ((AccountingDocumentServicePortType)this).getInvoiceNumber(getInvoiceNumberRequest2).GetInvoiceNumberResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getInvoiceNumberResponse1> AccountingDocumentServicePortType.getInvoiceNumberAsync(getInvoiceNumberRequest1 request)
	{
		return Channel.getInvoiceNumberAsync(request);
	}

	public Task<getInvoiceNumberResponse1> getInvoiceNumberAsync(GetInvoiceNumberRequest GetInvoiceNumberRequest)
	{
		getInvoiceNumberRequest1 getInvoiceNumberRequest2 = new getInvoiceNumberRequest1();
		getInvoiceNumberRequest2.GetInvoiceNumberRequest = GetInvoiceNumberRequest;
		return ((AccountingDocumentServicePortType)this).getInvoiceNumberAsync(getInvoiceNumberRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getAccDocSubmittedListResponse AccountingDocumentServicePortType.getAccDocSubmittedList(getAccDocSubmittedListRequest request)
	{
		return Channel.getAccDocSubmittedList(request);
	}

	public GetAccDocsResponse getAccDocSubmittedList(GetAccDocsRequest GetAccDocSubmittedListRequest)
	{
		getAccDocSubmittedListRequest getAccDocSubmittedListRequest2 = new getAccDocSubmittedListRequest();
		getAccDocSubmittedListRequest2.GetAccDocSubmittedListRequest = GetAccDocSubmittedListRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocSubmittedList(getAccDocSubmittedListRequest2).GetAccDocSubmittedListResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getAccDocSubmittedListResponse> AccountingDocumentServicePortType.getAccDocSubmittedListAsync(getAccDocSubmittedListRequest request)
	{
		return Channel.getAccDocSubmittedListAsync(request);
	}

	public Task<getAccDocSubmittedListResponse> getAccDocSubmittedListAsync(GetAccDocsRequest GetAccDocSubmittedListRequest)
	{
		getAccDocSubmittedListRequest getAccDocSubmittedListRequest2 = new getAccDocSubmittedListRequest();
		getAccDocSubmittedListRequest2.GetAccDocSubmittedListRequest = GetAccDocSubmittedListRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocSubmittedListAsync(getAccDocSubmittedListRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getAccDocSubmittedListCountResponse AccountingDocumentServicePortType.getAccDocSubmittedListCount(getAccDocSubmittedListCountRequest request)
	{
		return Channel.getAccDocSubmittedListCount(request);
	}

	public GetAccDocListCountResponse getAccDocSubmittedListCount(GetAccDocsRequest GetAccDocSubmittedListCountRequest)
	{
		getAccDocSubmittedListCountRequest getAccDocSubmittedListCountRequest2 = new getAccDocSubmittedListCountRequest();
		getAccDocSubmittedListCountRequest2.GetAccDocSubmittedListCountRequest = GetAccDocSubmittedListCountRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocSubmittedListCount(getAccDocSubmittedListCountRequest2).GetAccDocSubmittedListCountResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getAccDocSubmittedListCountResponse> AccountingDocumentServicePortType.getAccDocSubmittedListCountAsync(getAccDocSubmittedListCountRequest request)
	{
		return Channel.getAccDocSubmittedListCountAsync(request);
	}

	public Task<getAccDocSubmittedListCountResponse> getAccDocSubmittedListCountAsync(GetAccDocsRequest GetAccDocSubmittedListCountRequest)
	{
		getAccDocSubmittedListCountRequest getAccDocSubmittedListCountRequest2 = new getAccDocSubmittedListCountRequest();
		getAccDocSubmittedListCountRequest2.GetAccDocSubmittedListCountRequest = GetAccDocSubmittedListCountRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocSubmittedListCountAsync(getAccDocSubmittedListCountRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getAccDocInvalidListResponse AccountingDocumentServicePortType.getAccDocInvalidList(getAccDocInvalidListRequest request)
	{
		return Channel.getAccDocInvalidList(request);
	}

	public GetAccDocsResponse getAccDocInvalidList(GetAccDocsRequest GetAccDocInvalidListRequest)
	{
		getAccDocInvalidListRequest getAccDocInvalidListRequest2 = new getAccDocInvalidListRequest();
		getAccDocInvalidListRequest2.GetAccDocInvalidListRequest = GetAccDocInvalidListRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocInvalidList(getAccDocInvalidListRequest2).GetAccDocInvalidListResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getAccDocInvalidListResponse> AccountingDocumentServicePortType.getAccDocInvalidListAsync(getAccDocInvalidListRequest request)
	{
		return Channel.getAccDocInvalidListAsync(request);
	}

	public Task<getAccDocInvalidListResponse> getAccDocInvalidListAsync(GetAccDocsRequest GetAccDocInvalidListRequest)
	{
		getAccDocInvalidListRequest getAccDocInvalidListRequest2 = new getAccDocInvalidListRequest();
		getAccDocInvalidListRequest2.GetAccDocInvalidListRequest = GetAccDocInvalidListRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocInvalidListAsync(getAccDocInvalidListRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getAccDocInvalidListCountResponse AccountingDocumentServicePortType.getAccDocInvalidListCount(getAccDocInvalidListCountRequest request)
	{
		return Channel.getAccDocInvalidListCount(request);
	}

	public GetAccDocListCountResponse getAccDocInvalidListCount(GetAccDocsRequest GetAccDocInvalidListCountRequest)
	{
		getAccDocInvalidListCountRequest getAccDocInvalidListCountRequest2 = new getAccDocInvalidListCountRequest();
		getAccDocInvalidListCountRequest2.GetAccDocInvalidListCountRequest = GetAccDocInvalidListCountRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocInvalidListCount(getAccDocInvalidListCountRequest2).GetAccDocInvalidListCountResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getAccDocInvalidListCountResponse> AccountingDocumentServicePortType.getAccDocInvalidListCountAsync(getAccDocInvalidListCountRequest request)
	{
		return Channel.getAccDocInvalidListCountAsync(request);
	}

	public Task<getAccDocInvalidListCountResponse> getAccDocInvalidListCountAsync(GetAccDocsRequest GetAccDocInvalidListCountRequest)
	{
		getAccDocInvalidListCountRequest getAccDocInvalidListCountRequest2 = new getAccDocInvalidListCountRequest();
		getAccDocInvalidListCountRequest2.GetAccDocInvalidListCountRequest = GetAccDocInvalidListCountRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocInvalidListCountAsync(getAccDocInvalidListCountRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getAccDocOverdueInvalidListResponse AccountingDocumentServicePortType.getAccDocOverdueInvalidList(getAccDocOverdueInvalidListRequest request)
	{
		return Channel.getAccDocOverdueInvalidList(request);
	}

	public GetAccDocsResponse getAccDocOverdueInvalidList(GetAccDocsRequest GetAccDocOverdueInvalidListRequest)
	{
		getAccDocOverdueInvalidListRequest getAccDocOverdueInvalidListRequest2 = new getAccDocOverdueInvalidListRequest();
		getAccDocOverdueInvalidListRequest2.GetAccDocOverdueInvalidListRequest = GetAccDocOverdueInvalidListRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocOverdueInvalidList(getAccDocOverdueInvalidListRequest2).GetAccDocOverdueInvalidListResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getAccDocOverdueInvalidListResponse> AccountingDocumentServicePortType.getAccDocOverdueInvalidListAsync(getAccDocOverdueInvalidListRequest request)
	{
		return Channel.getAccDocOverdueInvalidListAsync(request);
	}

	public Task<getAccDocOverdueInvalidListResponse> getAccDocOverdueInvalidListAsync(GetAccDocsRequest GetAccDocOverdueInvalidListRequest)
	{
		getAccDocOverdueInvalidListRequest getAccDocOverdueInvalidListRequest2 = new getAccDocOverdueInvalidListRequest();
		getAccDocOverdueInvalidListRequest2.GetAccDocOverdueInvalidListRequest = GetAccDocOverdueInvalidListRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocOverdueInvalidListAsync(getAccDocOverdueInvalidListRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getAccDocOverdueInvalidListCountResponse AccountingDocumentServicePortType.getAccDocOverdueInvalidListCount(getAccDocOverdueInvalidListCountRequest request)
	{
		return Channel.getAccDocOverdueInvalidListCount(request);
	}

	public GetAccDocListCountResponse getAccDocOverdueInvalidListCount(GetAccDocsRequest GetAccDocOverdueInvalidListCountRequest)
	{
		getAccDocOverdueInvalidListCountRequest getAccDocOverdueInvalidListCountRequest2 = new getAccDocOverdueInvalidListCountRequest();
		getAccDocOverdueInvalidListCountRequest2.GetAccDocOverdueInvalidListCountRequest = GetAccDocOverdueInvalidListCountRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocOverdueInvalidListCount(getAccDocOverdueInvalidListCountRequest2).GetAccDocOverdueInvalidListCountResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getAccDocOverdueInvalidListCountResponse> AccountingDocumentServicePortType.getAccDocOverdueInvalidListCountAsync(getAccDocOverdueInvalidListCountRequest request)
	{
		return Channel.getAccDocOverdueInvalidListCountAsync(request);
	}

	public Task<getAccDocOverdueInvalidListCountResponse> getAccDocOverdueInvalidListCountAsync(GetAccDocsRequest GetAccDocOverdueInvalidListCountRequest)
	{
		getAccDocOverdueInvalidListCountRequest getAccDocOverdueInvalidListCountRequest2 = new getAccDocOverdueInvalidListCountRequest();
		getAccDocOverdueInvalidListCountRequest2.GetAccDocOverdueInvalidListCountRequest = GetAccDocOverdueInvalidListCountRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocOverdueInvalidListCountAsync(getAccDocOverdueInvalidListCountRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getAccDocReceivedListResponse AccountingDocumentServicePortType.getAccDocReceivedList(getAccDocReceivedListRequest request)
	{
		return Channel.getAccDocReceivedList(request);
	}

	public GetAccDocsResponse getAccDocReceivedList(GetAccDocsRequest GetAccDocReceivedListRequest)
	{
		getAccDocReceivedListRequest getAccDocReceivedListRequest2 = new getAccDocReceivedListRequest();
		getAccDocReceivedListRequest2.GetAccDocReceivedListRequest = GetAccDocReceivedListRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocReceivedList(getAccDocReceivedListRequest2).GetAccDocReceivedListResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getAccDocReceivedListResponse> AccountingDocumentServicePortType.getAccDocReceivedListAsync(getAccDocReceivedListRequest request)
	{
		return Channel.getAccDocReceivedListAsync(request);
	}

	public Task<getAccDocReceivedListResponse> getAccDocReceivedListAsync(GetAccDocsRequest GetAccDocReceivedListRequest)
	{
		getAccDocReceivedListRequest getAccDocReceivedListRequest2 = new getAccDocReceivedListRequest();
		getAccDocReceivedListRequest2.GetAccDocReceivedListRequest = GetAccDocReceivedListRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocReceivedListAsync(getAccDocReceivedListRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getAccDocReceivedListCountResponse AccountingDocumentServicePortType.getAccDocReceivedListCount(getAccDocReceivedListCountRequest request)
	{
		return Channel.getAccDocReceivedListCount(request);
	}

	public GetAccDocListCountResponse getAccDocReceivedListCount(GetAccDocsRequest GetAccDocReceivedListCountRequest)
	{
		getAccDocReceivedListCountRequest getAccDocReceivedListCountRequest2 = new getAccDocReceivedListCountRequest();
		getAccDocReceivedListCountRequest2.GetAccDocReceivedListCountRequest = GetAccDocReceivedListCountRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocReceivedListCount(getAccDocReceivedListCountRequest2).GetAccDocReceivedListCountResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getAccDocReceivedListCountResponse> AccountingDocumentServicePortType.getAccDocReceivedListCountAsync(getAccDocReceivedListCountRequest request)
	{
		return Channel.getAccDocReceivedListCountAsync(request);
	}

	public Task<getAccDocReceivedListCountResponse> getAccDocReceivedListCountAsync(GetAccDocsRequest GetAccDocReceivedListCountRequest)
	{
		getAccDocReceivedListCountRequest getAccDocReceivedListCountRequest2 = new getAccDocReceivedListCountRequest();
		getAccDocReceivedListCountRequest2.GetAccDocReceivedListCountRequest = GetAccDocReceivedListCountRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocReceivedListCountAsync(getAccDocReceivedListCountRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getDocByInvoiceNumberResponse1 AccountingDocumentServicePortType.getDocByInvoiceNumber(getDocByInvoiceNumberRequest1 request)
	{
		return Channel.getDocByInvoiceNumber(request);
	}

	public GetDocByInvoiceNumberResponse getDocByInvoiceNumber(GetDocByInvoiceNumberRequest GetDocByInvoiceNumberRequest)
	{
		getDocByInvoiceNumberRequest1 getDocByInvoiceNumberRequest2 = new getDocByInvoiceNumberRequest1();
		getDocByInvoiceNumberRequest2.GetDocByInvoiceNumberRequest = GetDocByInvoiceNumberRequest;
		return ((AccountingDocumentServicePortType)this).getDocByInvoiceNumber(getDocByInvoiceNumberRequest2).GetDocByInvoiceNumberResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getDocByInvoiceNumberResponse1> AccountingDocumentServicePortType.getDocByInvoiceNumberAsync(getDocByInvoiceNumberRequest1 request)
	{
		return Channel.getDocByInvoiceNumberAsync(request);
	}

	public Task<getDocByInvoiceNumberResponse1> getDocByInvoiceNumberAsync(GetDocByInvoiceNumberRequest GetDocByInvoiceNumberRequest)
	{
		getDocByInvoiceNumberRequest1 getDocByInvoiceNumberRequest2 = new getDocByInvoiceNumberRequest1();
		getDocByInvoiceNumberRequest2.GetDocByInvoiceNumberRequest = GetDocByInvoiceNumberRequest;
		return ((AccountingDocumentServicePortType)this).getDocByInvoiceNumberAsync(getDocByInvoiceNumberRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getParentDocByInvoiceNumberResponse1 AccountingDocumentServicePortType.getParentDocByInvoiceNumber(getParentDocByInvoiceNumberRequest1 request)
	{
		return Channel.getParentDocByInvoiceNumber(request);
	}

	public GetParentDocByInvoiceNumberResponse getParentDocByInvoiceNumber(GetParentDocByInvoiceNumberRequest GetParentDocByInvoiceNumberRequest)
	{
		getParentDocByInvoiceNumberRequest1 getParentDocByInvoiceNumberRequest2 = new getParentDocByInvoiceNumberRequest1();
		getParentDocByInvoiceNumberRequest2.GetParentDocByInvoiceNumberRequest = GetParentDocByInvoiceNumberRequest;
		return ((AccountingDocumentServicePortType)this).getParentDocByInvoiceNumber(getParentDocByInvoiceNumberRequest2).GetParentDocByInvoiceNumberResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getParentDocByInvoiceNumberResponse1> AccountingDocumentServicePortType.getParentDocByInvoiceNumberAsync(getParentDocByInvoiceNumberRequest1 request)
	{
		return Channel.getParentDocByInvoiceNumberAsync(request);
	}

	public Task<getParentDocByInvoiceNumberResponse1> getParentDocByInvoiceNumberAsync(GetParentDocByInvoiceNumberRequest GetParentDocByInvoiceNumberRequest)
	{
		getParentDocByInvoiceNumberRequest1 getParentDocByInvoiceNumberRequest2 = new getParentDocByInvoiceNumberRequest1();
		getParentDocByInvoiceNumberRequest2.GetParentDocByInvoiceNumberRequest = GetParentDocByInvoiceNumberRequest;
		return ((AccountingDocumentServicePortType)this).getParentDocByInvoiceNumberAsync(getParentDocByInvoiceNumberRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	signAccDocByBuyerResponse1 AccountingDocumentServicePortType.signAccDocByBuyer(signAccDocByBuyerRequest1 request)
	{
		return Channel.signAccDocByBuyer(request);
	}

	public SignAccDocByBuyerResponse signAccDocByBuyer(SignAccDocByBuyerRequest SignAccDocByBuyerRequest)
	{
		signAccDocByBuyerRequest1 signAccDocByBuyerRequest2 = new signAccDocByBuyerRequest1();
		signAccDocByBuyerRequest2.SignAccDocByBuyerRequest = SignAccDocByBuyerRequest;
		return ((AccountingDocumentServicePortType)this).signAccDocByBuyer(signAccDocByBuyerRequest2).SignAccDocByBuyerResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<signAccDocByBuyerResponse1> AccountingDocumentServicePortType.signAccDocByBuyerAsync(signAccDocByBuyerRequest1 request)
	{
		return Channel.signAccDocByBuyerAsync(request);
	}

	public Task<signAccDocByBuyerResponse1> signAccDocByBuyerAsync(SignAccDocByBuyerRequest SignAccDocByBuyerRequest)
	{
		signAccDocByBuyerRequest1 signAccDocByBuyerRequest2 = new signAccDocByBuyerRequest1();
		signAccDocByBuyerRequest2.SignAccDocByBuyerRequest = SignAccDocByBuyerRequest;
		return ((AccountingDocumentServicePortType)this).signAccDocByBuyerAsync(signAccDocByBuyerRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getAccDocReceiptResponse1 AccountingDocumentServicePortType.getAccDocReceipt(getAccDocReceiptRequest1 request)
	{
		return Channel.getAccDocReceipt(request);
	}

	public GetAccDocReceiptResponse getAccDocReceipt(GetAccDocReceiptRequest GetAccDocReceiptRequest)
	{
		getAccDocReceiptRequest1 getAccDocReceiptRequest2 = new getAccDocReceiptRequest1();
		getAccDocReceiptRequest2.GetAccDocReceiptRequest = GetAccDocReceiptRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocReceipt(getAccDocReceiptRequest2).GetAccDocReceiptResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getAccDocReceiptResponse1> AccountingDocumentServicePortType.getAccDocReceiptAsync(getAccDocReceiptRequest1 request)
	{
		return Channel.getAccDocReceiptAsync(request);
	}

	public Task<getAccDocReceiptResponse1> getAccDocReceiptAsync(GetAccDocReceiptRequest GetAccDocReceiptRequest)
	{
		getAccDocReceiptRequest1 getAccDocReceiptRequest2 = new getAccDocReceiptRequest1();
		getAccDocReceiptRequest2.GetAccDocReceiptRequest = GetAccDocReceiptRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocReceiptAsync(getAccDocReceiptRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getAccDocInvalidationRequestResponse AccountingDocumentServicePortType.getAccDocInvalidationRequest(getAccDocInvalidationRequestRequest request)
	{
		return Channel.getAccDocInvalidationRequest(request);
	}

	public GetInvalidationRequestResponse getAccDocInvalidationRequest(GetInvalidationRequestRequest GetInvalidationRequestRequest)
	{
		getAccDocInvalidationRequestRequest getAccDocInvalidationRequestRequest2 = new getAccDocInvalidationRequestRequest();
		getAccDocInvalidationRequestRequest2.GetInvalidationRequestRequest = GetInvalidationRequestRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocInvalidationRequest(getAccDocInvalidationRequestRequest2).GetInvalidationRequestResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getAccDocInvalidationRequestResponse> AccountingDocumentServicePortType.getAccDocInvalidationRequestAsync(getAccDocInvalidationRequestRequest request)
	{
		return Channel.getAccDocInvalidationRequestAsync(request);
	}

	public Task<getAccDocInvalidationRequestResponse> getAccDocInvalidationRequestAsync(GetInvalidationRequestRequest GetInvalidationRequestRequest)
	{
		getAccDocInvalidationRequestRequest getAccDocInvalidationRequestRequest2 = new getAccDocInvalidationRequestRequest();
		getAccDocInvalidationRequestRequest2.GetInvalidationRequestRequest = GetInvalidationRequestRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocInvalidationRequestAsync(getAccDocInvalidationRequestRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getAccDocSignatureRevokationRequestResponse AccountingDocumentServicePortType.getAccDocSignatureRevokationRequest(getAccDocSignatureRevokationRequestRequest request)
	{
		return Channel.getAccDocSignatureRevokationRequest(request);
	}

	public GetSignatureRevokationRequestResponse getAccDocSignatureRevokationRequest(GetSignatureRevokationRequestRequest GetSignatureRevokationRequestRequest)
	{
		getAccDocSignatureRevokationRequestRequest getAccDocSignatureRevokationRequestRequest2 = new getAccDocSignatureRevokationRequestRequest();
		getAccDocSignatureRevokationRequestRequest2.GetSignatureRevokationRequestRequest = GetSignatureRevokationRequestRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocSignatureRevokationRequest(getAccDocSignatureRevokationRequestRequest2).GetSignatureRevokationRequestResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getAccDocSignatureRevokationRequestResponse> AccountingDocumentServicePortType.getAccDocSignatureRevokationRequestAsync(getAccDocSignatureRevokationRequestRequest request)
	{
		return Channel.getAccDocSignatureRevokationRequestAsync(request);
	}

	public Task<getAccDocSignatureRevokationRequestResponse> getAccDocSignatureRevokationRequestAsync(GetSignatureRevokationRequestRequest GetSignatureRevokationRequestRequest)
	{
		getAccDocSignatureRevokationRequestRequest getAccDocSignatureRevokationRequestRequest2 = new getAccDocSignatureRevokationRequestRequest();
		getAccDocSignatureRevokationRequestRequest2.GetSignatureRevokationRequestRequest = GetSignatureRevokationRequestRequest;
		return ((AccountingDocumentServicePortType)this).getAccDocSignatureRevokationRequestAsync(getAccDocSignatureRevokationRequestRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	existWaitingInvalidationApprovementAccDocsResponse1 AccountingDocumentServicePortType.existWaitingInvalidationApprovementAccDocs(existWaitingInvalidationApprovementAccDocsRequest1 request)
	{
		return Channel.existWaitingInvalidationApprovementAccDocs(request);
	}

	public ExistWaitingInvalidationApprovementAccDocsResponse existWaitingInvalidationApprovementAccDocs(ExistWaitingInvalidationApprovementAccDocsRequest ExistWaitingInvalidationApprovementAccDocsRequest)
	{
		existWaitingInvalidationApprovementAccDocsRequest1 existWaitingInvalidationApprovementAccDocsRequest2 = new existWaitingInvalidationApprovementAccDocsRequest1();
		existWaitingInvalidationApprovementAccDocsRequest2.ExistWaitingInvalidationApprovementAccDocsRequest = ExistWaitingInvalidationApprovementAccDocsRequest;
		return ((AccountingDocumentServicePortType)this).existWaitingInvalidationApprovementAccDocs(existWaitingInvalidationApprovementAccDocsRequest2).ExistWaitingInvalidationApprovementAccDocsResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<existWaitingInvalidationApprovementAccDocsResponse1> AccountingDocumentServicePortType.existWaitingInvalidationApprovementAccDocsAsync(existWaitingInvalidationApprovementAccDocsRequest1 request)
	{
		return Channel.existWaitingInvalidationApprovementAccDocsAsync(request);
	}

	public Task<existWaitingInvalidationApprovementAccDocsResponse1> existWaitingInvalidationApprovementAccDocsAsync(ExistWaitingInvalidationApprovementAccDocsRequest ExistWaitingInvalidationApprovementAccDocsRequest)
	{
		existWaitingInvalidationApprovementAccDocsRequest1 existWaitingInvalidationApprovementAccDocsRequest2 = new existWaitingInvalidationApprovementAccDocsRequest1();
		existWaitingInvalidationApprovementAccDocsRequest2.ExistWaitingInvalidationApprovementAccDocsRequest = ExistWaitingInvalidationApprovementAccDocsRequest;
		return ((AccountingDocumentServicePortType)this).existWaitingInvalidationApprovementAccDocsAsync(existWaitingInvalidationApprovementAccDocsRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	existInvalidationApprovedAccDocsResponse1 AccountingDocumentServicePortType.existInvalidationApprovedAccDocs(existInvalidationApprovedAccDocsRequest1 request)
	{
		return Channel.existInvalidationApprovedAccDocs(request);
	}

	public ExistInvalidationApprovedAccDocsResponse existInvalidationApprovedAccDocs(ExistInvalidationApprovedAccDocsRequest ExistInvalidationApprovedAccDocsRequest)
	{
		existInvalidationApprovedAccDocsRequest1 existInvalidationApprovedAccDocsRequest2 = new existInvalidationApprovedAccDocsRequest1();
		existInvalidationApprovedAccDocsRequest2.ExistInvalidationApprovedAccDocsRequest = ExistInvalidationApprovedAccDocsRequest;
		return ((AccountingDocumentServicePortType)this).existInvalidationApprovedAccDocs(existInvalidationApprovedAccDocsRequest2).ExistInvalidationApprovedAccDocsResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<existInvalidationApprovedAccDocsResponse1> AccountingDocumentServicePortType.existInvalidationApprovedAccDocsAsync(existInvalidationApprovedAccDocsRequest1 request)
	{
		return Channel.existInvalidationApprovedAccDocsAsync(request);
	}

	public Task<existInvalidationApprovedAccDocsResponse1> existInvalidationApprovedAccDocsAsync(ExistInvalidationApprovedAccDocsRequest ExistInvalidationApprovedAccDocsRequest)
	{
		existInvalidationApprovedAccDocsRequest1 existInvalidationApprovedAccDocsRequest2 = new existInvalidationApprovedAccDocsRequest1();
		existInvalidationApprovedAccDocsRequest2.ExistInvalidationApprovedAccDocsRequest = ExistInvalidationApprovedAccDocsRequest;
		return ((AccountingDocumentServicePortType)this).existInvalidationApprovedAccDocsAsync(existInvalidationApprovedAccDocsRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	existInvalidationApprovedAccDocsForBuyerResponse1 AccountingDocumentServicePortType.existInvalidationApprovedAccDocsForBuyer(existInvalidationApprovedAccDocsForBuyerRequest1 request)
	{
		return Channel.existInvalidationApprovedAccDocsForBuyer(request);
	}

	public ExistInvalidationApprovedAccDocsForBuyerResponse existInvalidationApprovedAccDocsForBuyer(ExistInvalidationApprovedAccDocsForBuyerRequest ExistInvalidationApprovedAccDocsForBuyerRequest)
	{
		existInvalidationApprovedAccDocsForBuyerRequest1 existInvalidationApprovedAccDocsForBuyerRequest2 = new existInvalidationApprovedAccDocsForBuyerRequest1();
		existInvalidationApprovedAccDocsForBuyerRequest2.ExistInvalidationApprovedAccDocsForBuyerRequest = ExistInvalidationApprovedAccDocsForBuyerRequest;
		return ((AccountingDocumentServicePortType)this).existInvalidationApprovedAccDocsForBuyer(existInvalidationApprovedAccDocsForBuyerRequest2).ExistInvalidationApprovedAccDocsForBuyerResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<existInvalidationApprovedAccDocsForBuyerResponse1> AccountingDocumentServicePortType.existInvalidationApprovedAccDocsForBuyerAsync(existInvalidationApprovedAccDocsForBuyerRequest1 request)
	{
		return Channel.existInvalidationApprovedAccDocsForBuyerAsync(request);
	}

	public Task<existInvalidationApprovedAccDocsForBuyerResponse1> existInvalidationApprovedAccDocsForBuyerAsync(ExistInvalidationApprovedAccDocsForBuyerRequest ExistInvalidationApprovedAccDocsForBuyerRequest)
	{
		existInvalidationApprovedAccDocsForBuyerRequest1 existInvalidationApprovedAccDocsForBuyerRequest2 = new existInvalidationApprovedAccDocsForBuyerRequest1();
		existInvalidationApprovedAccDocsForBuyerRequest2.ExistInvalidationApprovedAccDocsForBuyerRequest = ExistInvalidationApprovedAccDocsForBuyerRequest;
		return ((AccountingDocumentServicePortType)this).existInvalidationApprovedAccDocsForBuyerAsync(existInvalidationApprovedAccDocsForBuyerRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	existInvalidationCanceledAccDocsResponse1 AccountingDocumentServicePortType.existInvalidationCanceledAccDocs(existInvalidationCanceledAccDocsRequest1 request)
	{
		return Channel.existInvalidationCanceledAccDocs(request);
	}

	public ExistInvalidationCanceledAccDocsResponse existInvalidationCanceledAccDocs(ExistInvalidationCanceledAccDocsRequest ExistInvalidationCanceledAccDocsRequest)
	{
		existInvalidationCanceledAccDocsRequest1 existInvalidationCanceledAccDocsRequest2 = new existInvalidationCanceledAccDocsRequest1();
		existInvalidationCanceledAccDocsRequest2.ExistInvalidationCanceledAccDocsRequest = ExistInvalidationCanceledAccDocsRequest;
		return ((AccountingDocumentServicePortType)this).existInvalidationCanceledAccDocs(existInvalidationCanceledAccDocsRequest2).ExistInvalidationCanceledAccDocsResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<existInvalidationCanceledAccDocsResponse1> AccountingDocumentServicePortType.existInvalidationCanceledAccDocsAsync(existInvalidationCanceledAccDocsRequest1 request)
	{
		return Channel.existInvalidationCanceledAccDocsAsync(request);
	}

	public Task<existInvalidationCanceledAccDocsResponse1> existInvalidationCanceledAccDocsAsync(ExistInvalidationCanceledAccDocsRequest ExistInvalidationCanceledAccDocsRequest)
	{
		existInvalidationCanceledAccDocsRequest1 existInvalidationCanceledAccDocsRequest2 = new existInvalidationCanceledAccDocsRequest1();
		existInvalidationCanceledAccDocsRequest2.ExistInvalidationCanceledAccDocsRequest = ExistInvalidationCanceledAccDocsRequest;
		return ((AccountingDocumentServicePortType)this).existInvalidationCanceledAccDocsAsync(existInvalidationCanceledAccDocsRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	existDeletedAccDocsResponse1 AccountingDocumentServicePortType.existDeletedAccDocs(existDeletedAccDocsRequest1 request)
	{
		return Channel.existDeletedAccDocs(request);
	}

	public ExistDeletedAccDocsResponse existDeletedAccDocs(ExistDeletedAccDocsRequest ExistDeletedAccDocsRequest)
	{
		existDeletedAccDocsRequest1 existDeletedAccDocsRequest2 = new existDeletedAccDocsRequest1();
		existDeletedAccDocsRequest2.ExistDeletedAccDocsRequest = ExistDeletedAccDocsRequest;
		return ((AccountingDocumentServicePortType)this).existDeletedAccDocs(existDeletedAccDocsRequest2).ExistDeletedAccDocsResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<existDeletedAccDocsResponse1> AccountingDocumentServicePortType.existDeletedAccDocsAsync(existDeletedAccDocsRequest1 request)
	{
		return Channel.existDeletedAccDocsAsync(request);
	}

	public Task<existDeletedAccDocsResponse1> existDeletedAccDocsAsync(ExistDeletedAccDocsRequest ExistDeletedAccDocsRequest)
	{
		existDeletedAccDocsRequest1 existDeletedAccDocsRequest2 = new existDeletedAccDocsRequest1();
		existDeletedAccDocsRequest2.ExistDeletedAccDocsRequest = ExistDeletedAccDocsRequest;
		return ((AccountingDocumentServicePortType)this).existDeletedAccDocsAsync(existDeletedAccDocsRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	existOnBehalfOfSupplierAccDocsResponse1 AccountingDocumentServicePortType.existOnBehalfOfSupplierAccDocs(existOnBehalfOfSupplierAccDocsRequest1 request)
	{
		return Channel.existOnBehalfOfSupplierAccDocs(request);
	}

	public ExistOnBehalfOfSupplierAccDocsResponse existOnBehalfOfSupplierAccDocs(ExistOnBehalfOfSupplierAccDocsRequest ExistOnBehalfOfSupplierAccDocsRequest)
	{
		existOnBehalfOfSupplierAccDocsRequest1 existOnBehalfOfSupplierAccDocsRequest2 = new existOnBehalfOfSupplierAccDocsRequest1();
		existOnBehalfOfSupplierAccDocsRequest2.ExistOnBehalfOfSupplierAccDocsRequest = ExistOnBehalfOfSupplierAccDocsRequest;
		return ((AccountingDocumentServicePortType)this).existOnBehalfOfSupplierAccDocs(existOnBehalfOfSupplierAccDocsRequest2).ExistOnBehalfOfSupplierAccDocsResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<existOnBehalfOfSupplierAccDocsResponse1> AccountingDocumentServicePortType.existOnBehalfOfSupplierAccDocsAsync(existOnBehalfOfSupplierAccDocsRequest1 request)
	{
		return Channel.existOnBehalfOfSupplierAccDocsAsync(request);
	}

	public Task<existOnBehalfOfSupplierAccDocsResponse1> existOnBehalfOfSupplierAccDocsAsync(ExistOnBehalfOfSupplierAccDocsRequest ExistOnBehalfOfSupplierAccDocsRequest)
	{
		existOnBehalfOfSupplierAccDocsRequest1 existOnBehalfOfSupplierAccDocsRequest2 = new existOnBehalfOfSupplierAccDocsRequest1();
		existOnBehalfOfSupplierAccDocsRequest2.ExistOnBehalfOfSupplierAccDocsRequest = ExistOnBehalfOfSupplierAccDocsRequest;
		return ((AccountingDocumentServicePortType)this).existOnBehalfOfSupplierAccDocsAsync(existOnBehalfOfSupplierAccDocsRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	existInvalidatedOnBehalfOfSupplierAccDocsResponse1 AccountingDocumentServicePortType.existInvalidatedOnBehalfOfSupplierAccDocs(existInvalidatedOnBehalfOfSupplierAccDocsRequest1 request)
	{
		return Channel.existInvalidatedOnBehalfOfSupplierAccDocs(request);
	}

	public ExistInvalidatedOnBehalfOfSupplierAccDocsResponse existInvalidatedOnBehalfOfSupplierAccDocs(ExistInvalidatedOnBehalfOfSupplierAccDocsRequest ExistInvalidatedOnBehalfOfSupplierAccDocsRequest)
	{
		existInvalidatedOnBehalfOfSupplierAccDocsRequest1 existInvalidatedOnBehalfOfSupplierAccDocsRequest2 = new existInvalidatedOnBehalfOfSupplierAccDocsRequest1();
		existInvalidatedOnBehalfOfSupplierAccDocsRequest2.ExistInvalidatedOnBehalfOfSupplierAccDocsRequest = ExistInvalidatedOnBehalfOfSupplierAccDocsRequest;
		return ((AccountingDocumentServicePortType)this).existInvalidatedOnBehalfOfSupplierAccDocs(existInvalidatedOnBehalfOfSupplierAccDocsRequest2).ExistInvalidatedOnBehalfOfSupplierAccDocsResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<existInvalidatedOnBehalfOfSupplierAccDocsResponse1> AccountingDocumentServicePortType.existInvalidatedOnBehalfOfSupplierAccDocsAsync(existInvalidatedOnBehalfOfSupplierAccDocsRequest1 request)
	{
		return Channel.existInvalidatedOnBehalfOfSupplierAccDocsAsync(request);
	}

	public Task<existInvalidatedOnBehalfOfSupplierAccDocsResponse1> existInvalidatedOnBehalfOfSupplierAccDocsAsync(ExistInvalidatedOnBehalfOfSupplierAccDocsRequest ExistInvalidatedOnBehalfOfSupplierAccDocsRequest)
	{
		existInvalidatedOnBehalfOfSupplierAccDocsRequest1 existInvalidatedOnBehalfOfSupplierAccDocsRequest2 = new existInvalidatedOnBehalfOfSupplierAccDocsRequest1();
		existInvalidatedOnBehalfOfSupplierAccDocsRequest2.ExistInvalidatedOnBehalfOfSupplierAccDocsRequest = ExistInvalidatedOnBehalfOfSupplierAccDocsRequest;
		return ((AccountingDocumentServicePortType)this).existInvalidatedOnBehalfOfSupplierAccDocsAsync(existInvalidatedOnBehalfOfSupplierAccDocsRequest2);
	}

	public new virtual Task OpenAsync()
	{
		return Task.Factory.FromAsync(((ICommunicationObject)this).BeginOpen((AsyncCallback)null, (object)null), ((ICommunicationObject)this).EndOpen);
	}

	private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == EndpointConfiguration.AccountingDocumentServicePort)
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
		if (endpointConfiguration == EndpointConfiguration.AccountingDocumentServicePort)
		{
			return new EndpointAddress("http://ews.taxservice.am/taxsystem-fe-ws/taxpayer/accDocService");
		}
		throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'.");
	}

	private static Binding GetDefaultBinding()
	{
		return GetBindingForEndpoint(EndpointConfiguration.AccountingDocumentServicePort);
	}

	private static EndpointAddress GetDefaultEndpointAddress()
	{
		return GetEndpointAddress(EndpointConfiguration.AccountingDocumentServicePort);
	}
}
