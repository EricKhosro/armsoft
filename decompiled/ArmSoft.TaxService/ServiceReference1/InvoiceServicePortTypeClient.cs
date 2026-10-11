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
internal class InvoiceServicePortTypeClient : ClientBase<InvoiceServicePortType>, InvoiceServicePortType
{
	public enum EndpointConfiguration
	{
		InvoiceServicePort
	}

	public InvoiceServicePortTypeClient()
		: base(GetDefaultBinding(), GetDefaultEndpointAddress())
	{
		Endpoint.Name = EndpointConfiguration.InvoiceServicePort.ToString();
	}

	public InvoiceServicePortTypeClient(EndpointConfiguration endpointConfiguration)
		: base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public InvoiceServicePortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public InvoiceServicePortTypeClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), remoteAddress)
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public InvoiceServicePortTypeClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	checkVatPayerStatusResponse1 InvoiceServicePortType.checkVatPayerStatus(checkVatPayerStatusRequest1 request)
	{
		return Channel.checkVatPayerStatus(request);
	}

	public CheckVatPayerStatusResponse checkVatPayerStatus(CheckVatPayerStatusRequest CheckVatPayerStatusRequest)
	{
		checkVatPayerStatusRequest1 checkVatPayerStatusRequest2 = new checkVatPayerStatusRequest1();
		checkVatPayerStatusRequest2.CheckVatPayerStatusRequest = CheckVatPayerStatusRequest;
		return ((InvoiceServicePortType)this).checkVatPayerStatus(checkVatPayerStatusRequest2).CheckVatPayerStatusResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<checkVatPayerStatusResponse1> InvoiceServicePortType.checkVatPayerStatusAsync(checkVatPayerStatusRequest1 request)
	{
		return Channel.checkVatPayerStatusAsync(request);
	}

	public Task<checkVatPayerStatusResponse1> checkVatPayerStatusAsync(CheckVatPayerStatusRequest CheckVatPayerStatusRequest)
	{
		checkVatPayerStatusRequest1 checkVatPayerStatusRequest2 = new checkVatPayerStatusRequest1();
		checkVatPayerStatusRequest2.CheckVatPayerStatusRequest = CheckVatPayerStatusRequest;
		return ((InvoiceServicePortType)this).checkVatPayerStatusAsync(checkVatPayerStatusRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	existWaitingInvalidationApprovementInvoicesResponse1 InvoiceServicePortType.existWaitingInvalidationApprovementInvoices(existWaitingInvalidationApprovementInvoicesRequest1 request)
	{
		return Channel.existWaitingInvalidationApprovementInvoices(request);
	}

	public ExistWaitingInvalidationApprovementInvoicesResponse existWaitingInvalidationApprovementInvoices(ExistWaitingInvalidationApprovementInvoicesRequest ExistWaitingInvalidationApprovementInvoicesRequest)
	{
		existWaitingInvalidationApprovementInvoicesRequest1 existWaitingInvalidationApprovementInvoicesRequest2 = new existWaitingInvalidationApprovementInvoicesRequest1();
		existWaitingInvalidationApprovementInvoicesRequest2.ExistWaitingInvalidationApprovementInvoicesRequest = ExistWaitingInvalidationApprovementInvoicesRequest;
		return ((InvoiceServicePortType)this).existWaitingInvalidationApprovementInvoices(existWaitingInvalidationApprovementInvoicesRequest2).ExistWaitingInvalidationApprovementInvoicesResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<existWaitingInvalidationApprovementInvoicesResponse1> InvoiceServicePortType.existWaitingInvalidationApprovementInvoicesAsync(existWaitingInvalidationApprovementInvoicesRequest1 request)
	{
		return Channel.existWaitingInvalidationApprovementInvoicesAsync(request);
	}

	public Task<existWaitingInvalidationApprovementInvoicesResponse1> existWaitingInvalidationApprovementInvoicesAsync(ExistWaitingInvalidationApprovementInvoicesRequest ExistWaitingInvalidationApprovementInvoicesRequest)
	{
		existWaitingInvalidationApprovementInvoicesRequest1 existWaitingInvalidationApprovementInvoicesRequest2 = new existWaitingInvalidationApprovementInvoicesRequest1();
		existWaitingInvalidationApprovementInvoicesRequest2.ExistWaitingInvalidationApprovementInvoicesRequest = ExistWaitingInvalidationApprovementInvoicesRequest;
		return ((InvoiceServicePortType)this).existWaitingInvalidationApprovementInvoicesAsync(existWaitingInvalidationApprovementInvoicesRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	existInvalidationApprovedInvoicesResponse1 InvoiceServicePortType.existInvalidationApprovedInvoices(existInvalidationApprovedInvoicesRequest1 request)
	{
		return Channel.existInvalidationApprovedInvoices(request);
	}

	public ExistInvalidationApprovedInvoicesResponse existInvalidationApprovedInvoices(ExistInvalidationApprovedInvoicesRequest ExistInvalidationApprovedInvoicesRequest)
	{
		existInvalidationApprovedInvoicesRequest1 existInvalidationApprovedInvoicesRequest2 = new existInvalidationApprovedInvoicesRequest1();
		existInvalidationApprovedInvoicesRequest2.ExistInvalidationApprovedInvoicesRequest = ExistInvalidationApprovedInvoicesRequest;
		return ((InvoiceServicePortType)this).existInvalidationApprovedInvoices(existInvalidationApprovedInvoicesRequest2).ExistInvalidationApprovedInvoicesResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<existInvalidationApprovedInvoicesResponse1> InvoiceServicePortType.existInvalidationApprovedInvoicesAsync(existInvalidationApprovedInvoicesRequest1 request)
	{
		return Channel.existInvalidationApprovedInvoicesAsync(request);
	}

	public Task<existInvalidationApprovedInvoicesResponse1> existInvalidationApprovedInvoicesAsync(ExistInvalidationApprovedInvoicesRequest ExistInvalidationApprovedInvoicesRequest)
	{
		existInvalidationApprovedInvoicesRequest1 existInvalidationApprovedInvoicesRequest2 = new existInvalidationApprovedInvoicesRequest1();
		existInvalidationApprovedInvoicesRequest2.ExistInvalidationApprovedInvoicesRequest = ExistInvalidationApprovedInvoicesRequest;
		return ((InvoiceServicePortType)this).existInvalidationApprovedInvoicesAsync(existInvalidationApprovedInvoicesRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	existInvalidationApprovedInvoicesForBuyerResponse1 InvoiceServicePortType.existInvalidationApprovedInvoicesForBuyer(existInvalidationApprovedInvoicesForBuyerRequest1 request)
	{
		return Channel.existInvalidationApprovedInvoicesForBuyer(request);
	}

	public ExistInvalidationApprovedInvoicesForBuyerResponse existInvalidationApprovedInvoicesForBuyer(ExistInvalidationApprovedInvoicesForBuyerRequest ExistInvalidationApprovedInvoicesForBuyerRequest)
	{
		existInvalidationApprovedInvoicesForBuyerRequest1 existInvalidationApprovedInvoicesForBuyerRequest2 = new existInvalidationApprovedInvoicesForBuyerRequest1();
		existInvalidationApprovedInvoicesForBuyerRequest2.ExistInvalidationApprovedInvoicesForBuyerRequest = ExistInvalidationApprovedInvoicesForBuyerRequest;
		return ((InvoiceServicePortType)this).existInvalidationApprovedInvoicesForBuyer(existInvalidationApprovedInvoicesForBuyerRequest2).ExistInvalidationApprovedInvoicesForBuyerResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<existInvalidationApprovedInvoicesForBuyerResponse1> InvoiceServicePortType.existInvalidationApprovedInvoicesForBuyerAsync(existInvalidationApprovedInvoicesForBuyerRequest1 request)
	{
		return Channel.existInvalidationApprovedInvoicesForBuyerAsync(request);
	}

	public Task<existInvalidationApprovedInvoicesForBuyerResponse1> existInvalidationApprovedInvoicesForBuyerAsync(ExistInvalidationApprovedInvoicesForBuyerRequest ExistInvalidationApprovedInvoicesForBuyerRequest)
	{
		existInvalidationApprovedInvoicesForBuyerRequest1 existInvalidationApprovedInvoicesForBuyerRequest2 = new existInvalidationApprovedInvoicesForBuyerRequest1();
		existInvalidationApprovedInvoicesForBuyerRequest2.ExistInvalidationApprovedInvoicesForBuyerRequest = ExistInvalidationApprovedInvoicesForBuyerRequest;
		return ((InvoiceServicePortType)this).existInvalidationApprovedInvoicesForBuyerAsync(existInvalidationApprovedInvoicesForBuyerRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	existInvalidationCanceledInvoicesResponse1 InvoiceServicePortType.existInvalidationCanceledInvoices(existInvalidationCanceledInvoicesRequest1 request)
	{
		return Channel.existInvalidationCanceledInvoices(request);
	}

	public ExistInvalidationCanceledInvoicesResponse existInvalidationCanceledInvoices(ExistInvalidationCanceledInvoicesRequest ExistInvalidationCanceledInvoicesRequest)
	{
		existInvalidationCanceledInvoicesRequest1 existInvalidationCanceledInvoicesRequest2 = new existInvalidationCanceledInvoicesRequest1();
		existInvalidationCanceledInvoicesRequest2.ExistInvalidationCanceledInvoicesRequest = ExistInvalidationCanceledInvoicesRequest;
		return ((InvoiceServicePortType)this).existInvalidationCanceledInvoices(existInvalidationCanceledInvoicesRequest2).ExistInvalidationCanceledInvoicesResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<existInvalidationCanceledInvoicesResponse1> InvoiceServicePortType.existInvalidationCanceledInvoicesAsync(existInvalidationCanceledInvoicesRequest1 request)
	{
		return Channel.existInvalidationCanceledInvoicesAsync(request);
	}

	public Task<existInvalidationCanceledInvoicesResponse1> existInvalidationCanceledInvoicesAsync(ExistInvalidationCanceledInvoicesRequest ExistInvalidationCanceledInvoicesRequest)
	{
		existInvalidationCanceledInvoicesRequest1 existInvalidationCanceledInvoicesRequest2 = new existInvalidationCanceledInvoicesRequest1();
		existInvalidationCanceledInvoicesRequest2.ExistInvalidationCanceledInvoicesRequest = ExistInvalidationCanceledInvoicesRequest;
		return ((InvoiceServicePortType)this).existInvalidationCanceledInvoicesAsync(existInvalidationCanceledInvoicesRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	existDeletedInvoicesResponse1 InvoiceServicePortType.existDeletedInvoices(existDeletedInvoicesRequest1 request)
	{
		return Channel.existDeletedInvoices(request);
	}

	public ExistDeletedInvoicesResponse existDeletedInvoices(ExistDeletedInvoicesRequest ExistDeletedInvoicesRequest)
	{
		existDeletedInvoicesRequest1 existDeletedInvoicesRequest2 = new existDeletedInvoicesRequest1();
		existDeletedInvoicesRequest2.ExistDeletedInvoicesRequest = ExistDeletedInvoicesRequest;
		return ((InvoiceServicePortType)this).existDeletedInvoices(existDeletedInvoicesRequest2).ExistDeletedInvoicesResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<existDeletedInvoicesResponse1> InvoiceServicePortType.existDeletedInvoicesAsync(existDeletedInvoicesRequest1 request)
	{
		return Channel.existDeletedInvoicesAsync(request);
	}

	public Task<existDeletedInvoicesResponse1> existDeletedInvoicesAsync(ExistDeletedInvoicesRequest ExistDeletedInvoicesRequest)
	{
		existDeletedInvoicesRequest1 existDeletedInvoicesRequest2 = new existDeletedInvoicesRequest1();
		existDeletedInvoicesRequest2.ExistDeletedInvoicesRequest = ExistDeletedInvoicesRequest;
		return ((InvoiceServicePortType)this).existDeletedInvoicesAsync(existDeletedInvoicesRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	existOnBehalfOfSupplierInvoicesResponse1 InvoiceServicePortType.existOnBehalfOfSupplierInvoices(existOnBehalfOfSupplierInvoicesRequest1 request)
	{
		return Channel.existOnBehalfOfSupplierInvoices(request);
	}

	public ExistOnBehalfOfSupplierInvoicesResponse existOnBehalfOfSupplierInvoices(ExistOnBehalfOfSupplierInvoicesRequest ExistOnBehalfOfSupplierInvoicesRequest)
	{
		existOnBehalfOfSupplierInvoicesRequest1 existOnBehalfOfSupplierInvoicesRequest2 = new existOnBehalfOfSupplierInvoicesRequest1();
		existOnBehalfOfSupplierInvoicesRequest2.ExistOnBehalfOfSupplierInvoicesRequest = ExistOnBehalfOfSupplierInvoicesRequest;
		return ((InvoiceServicePortType)this).existOnBehalfOfSupplierInvoices(existOnBehalfOfSupplierInvoicesRequest2).ExistOnBehalfOfSupplierInvoicesResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<existOnBehalfOfSupplierInvoicesResponse1> InvoiceServicePortType.existOnBehalfOfSupplierInvoicesAsync(existOnBehalfOfSupplierInvoicesRequest1 request)
	{
		return Channel.existOnBehalfOfSupplierInvoicesAsync(request);
	}

	public Task<existOnBehalfOfSupplierInvoicesResponse1> existOnBehalfOfSupplierInvoicesAsync(ExistOnBehalfOfSupplierInvoicesRequest ExistOnBehalfOfSupplierInvoicesRequest)
	{
		existOnBehalfOfSupplierInvoicesRequest1 existOnBehalfOfSupplierInvoicesRequest2 = new existOnBehalfOfSupplierInvoicesRequest1();
		existOnBehalfOfSupplierInvoicesRequest2.ExistOnBehalfOfSupplierInvoicesRequest = ExistOnBehalfOfSupplierInvoicesRequest;
		return ((InvoiceServicePortType)this).existOnBehalfOfSupplierInvoicesAsync(existOnBehalfOfSupplierInvoicesRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	existInvalidatedOnBehalfOfSupplierInvoicesResponse1 InvoiceServicePortType.existInvalidatedOnBehalfOfSupplierInvoices(existInvalidatedOnBehalfOfSupplierInvoicesRequest1 request)
	{
		return Channel.existInvalidatedOnBehalfOfSupplierInvoices(request);
	}

	public ExistInvalidatedOnBehalfOfSupplierInvoicesResponse existInvalidatedOnBehalfOfSupplierInvoices(ExistInvalidatedOnBehalfOfSupplierInvoicesRequest ExistInvalidatedOnBehalfOfSupplierInvoicesRequest)
	{
		existInvalidatedOnBehalfOfSupplierInvoicesRequest1 existInvalidatedOnBehalfOfSupplierInvoicesRequest2 = new existInvalidatedOnBehalfOfSupplierInvoicesRequest1();
		existInvalidatedOnBehalfOfSupplierInvoicesRequest2.ExistInvalidatedOnBehalfOfSupplierInvoicesRequest = ExistInvalidatedOnBehalfOfSupplierInvoicesRequest;
		return ((InvoiceServicePortType)this).existInvalidatedOnBehalfOfSupplierInvoices(existInvalidatedOnBehalfOfSupplierInvoicesRequest2).ExistInvalidatedOnBehalfOfSupplierInvoicesResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<existInvalidatedOnBehalfOfSupplierInvoicesResponse1> InvoiceServicePortType.existInvalidatedOnBehalfOfSupplierInvoicesAsync(existInvalidatedOnBehalfOfSupplierInvoicesRequest1 request)
	{
		return Channel.existInvalidatedOnBehalfOfSupplierInvoicesAsync(request);
	}

	public Task<existInvalidatedOnBehalfOfSupplierInvoicesResponse1> existInvalidatedOnBehalfOfSupplierInvoicesAsync(ExistInvalidatedOnBehalfOfSupplierInvoicesRequest ExistInvalidatedOnBehalfOfSupplierInvoicesRequest)
	{
		existInvalidatedOnBehalfOfSupplierInvoicesRequest1 existInvalidatedOnBehalfOfSupplierInvoicesRequest2 = new existInvalidatedOnBehalfOfSupplierInvoicesRequest1();
		existInvalidatedOnBehalfOfSupplierInvoicesRequest2.ExistInvalidatedOnBehalfOfSupplierInvoicesRequest = ExistInvalidatedOnBehalfOfSupplierInvoicesRequest;
		return ((InvoiceServicePortType)this).existInvalidatedOnBehalfOfSupplierInvoicesAsync(existInvalidatedOnBehalfOfSupplierInvoicesRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	resignInvoiceByBuyerResponse1 InvoiceServicePortType.resignInvoiceByBuyer(resignInvoiceByBuyerRequest1 request)
	{
		return Channel.resignInvoiceByBuyer(request);
	}

	public ResignInvoiceByBuyerResponse resignInvoiceByBuyer(ResignInvoiceByBuyerRequest ResignInvoiceByBuyerRequest)
	{
		resignInvoiceByBuyerRequest1 resignInvoiceByBuyerRequest2 = new resignInvoiceByBuyerRequest1();
		resignInvoiceByBuyerRequest2.ResignInvoiceByBuyerRequest = ResignInvoiceByBuyerRequest;
		return ((InvoiceServicePortType)this).resignInvoiceByBuyer(resignInvoiceByBuyerRequest2).ResignInvoiceByBuyerResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<resignInvoiceByBuyerResponse1> InvoiceServicePortType.resignInvoiceByBuyerAsync(resignInvoiceByBuyerRequest1 request)
	{
		return Channel.resignInvoiceByBuyerAsync(request);
	}

	public Task<resignInvoiceByBuyerResponse1> resignInvoiceByBuyerAsync(ResignInvoiceByBuyerRequest ResignInvoiceByBuyerRequest)
	{
		resignInvoiceByBuyerRequest1 resignInvoiceByBuyerRequest2 = new resignInvoiceByBuyerRequest1();
		resignInvoiceByBuyerRequest2.ResignInvoiceByBuyerRequest = ResignInvoiceByBuyerRequest;
		return ((InvoiceServicePortType)this).resignInvoiceByBuyerAsync(resignInvoiceByBuyerRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	checkInvoicePrintStatusResponse1 InvoiceServicePortType.checkInvoicePrintStatus(checkInvoicePrintStatusRequest1 request)
	{
		return Channel.checkInvoicePrintStatus(request);
	}

	public CheckInvoicePrintStatusResponse checkInvoicePrintStatus(CheckInvoicePrintStatusRequest CheckInvoicePrintStatusRequest)
	{
		checkInvoicePrintStatusRequest1 checkInvoicePrintStatusRequest2 = new checkInvoicePrintStatusRequest1();
		checkInvoicePrintStatusRequest2.CheckInvoicePrintStatusRequest = CheckInvoicePrintStatusRequest;
		return ((InvoiceServicePortType)this).checkInvoicePrintStatus(checkInvoicePrintStatusRequest2).CheckInvoicePrintStatusResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<checkInvoicePrintStatusResponse1> InvoiceServicePortType.checkInvoicePrintStatusAsync(checkInvoicePrintStatusRequest1 request)
	{
		return Channel.checkInvoicePrintStatusAsync(request);
	}

	public Task<checkInvoicePrintStatusResponse1> checkInvoicePrintStatusAsync(CheckInvoicePrintStatusRequest CheckInvoicePrintStatusRequest)
	{
		checkInvoicePrintStatusRequest1 checkInvoicePrintStatusRequest2 = new checkInvoicePrintStatusRequest1();
		checkInvoicePrintStatusRequest2.CheckInvoicePrintStatusRequest = CheckInvoicePrintStatusRequest;
		return ((InvoiceServicePortType)this).checkInvoicePrintStatusAsync(checkInvoicePrintStatusRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getTaxpayerInfoResponse1 InvoiceServicePortType.getTaxpayerInfo(getTaxpayerInfoRequest1 request)
	{
		return Channel.getTaxpayerInfo(request);
	}

	public GetTaxpayerInfoResponse getTaxpayerInfo(GetTaxpayerInfoRequest GetTaxpayerInfoRequest)
	{
		getTaxpayerInfoRequest1 getTaxpayerInfoRequest2 = new getTaxpayerInfoRequest1();
		getTaxpayerInfoRequest2.GetTaxpayerInfoRequest = GetTaxpayerInfoRequest;
		return ((InvoiceServicePortType)this).getTaxpayerInfo(getTaxpayerInfoRequest2).GetTaxpayerInfoResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getTaxpayerInfoResponse1> InvoiceServicePortType.getTaxpayerInfoAsync(getTaxpayerInfoRequest1 request)
	{
		return Channel.getTaxpayerInfoAsync(request);
	}

	public Task<getTaxpayerInfoResponse1> getTaxpayerInfoAsync(GetTaxpayerInfoRequest GetTaxpayerInfoRequest)
	{
		getTaxpayerInfoRequest1 getTaxpayerInfoRequest2 = new getTaxpayerInfoRequest1();
		getTaxpayerInfoRequest2.GetTaxpayerInfoRequest = GetTaxpayerInfoRequest;
		return ((InvoiceServicePortType)this).getTaxpayerInfoAsync(getTaxpayerInfoRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getTaxpayerInfoExtendedResponse InvoiceServicePortType.getTaxpayerInfoExtended(getTaxpayerInfoExtendedRequest request)
	{
		return Channel.getTaxpayerInfoExtended(request);
	}

	public GetTaxpayerInfoResponseExtended getTaxpayerInfoExtended(GetTaxpayerInfoRequestExtended GetTaxpayerInfoRequestExtended)
	{
		getTaxpayerInfoExtendedRequest getTaxpayerInfoExtendedRequest2 = new getTaxpayerInfoExtendedRequest();
		getTaxpayerInfoExtendedRequest2.GetTaxpayerInfoRequestExtended = GetTaxpayerInfoRequestExtended;
		return ((InvoiceServicePortType)this).getTaxpayerInfoExtended(getTaxpayerInfoExtendedRequest2).GetTaxpayerInfoResponseExtended;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getTaxpayerInfoExtendedResponse> InvoiceServicePortType.getTaxpayerInfoExtendedAsync(getTaxpayerInfoExtendedRequest request)
	{
		return Channel.getTaxpayerInfoExtendedAsync(request);
	}

	public Task<getTaxpayerInfoExtendedResponse> getTaxpayerInfoExtendedAsync(GetTaxpayerInfoRequestExtended GetTaxpayerInfoRequestExtended)
	{
		getTaxpayerInfoExtendedRequest getTaxpayerInfoExtendedRequest2 = new getTaxpayerInfoExtendedRequest();
		getTaxpayerInfoExtendedRequest2.GetTaxpayerInfoRequestExtended = GetTaxpayerInfoRequestExtended;
		return ((InvoiceServicePortType)this).getTaxpayerInfoExtendedAsync(getTaxpayerInfoExtendedRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getInvoiceNumberResponse1 InvoiceServicePortType.getInvoiceNumber(getInvoiceNumberRequest1 request)
	{
		return Channel.getInvoiceNumber(request);
	}

	public GetInvoiceNumberResponse getInvoiceNumber(GetInvoiceNumberRequest GetInvoiceNumberRequest)
	{
		getInvoiceNumberRequest1 getInvoiceNumberRequest2 = new getInvoiceNumberRequest1();
		getInvoiceNumberRequest2.GetInvoiceNumberRequest = GetInvoiceNumberRequest;
		return ((InvoiceServicePortType)this).getInvoiceNumber(getInvoiceNumberRequest2).GetInvoiceNumberResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getInvoiceNumberResponse1> InvoiceServicePortType.getInvoiceNumberAsync(getInvoiceNumberRequest1 request)
	{
		return Channel.getInvoiceNumberAsync(request);
	}

	public Task<getInvoiceNumberResponse1> getInvoiceNumberAsync(GetInvoiceNumberRequest GetInvoiceNumberRequest)
	{
		getInvoiceNumberRequest1 getInvoiceNumberRequest2 = new getInvoiceNumberRequest1();
		getInvoiceNumberRequest2.GetInvoiceNumberRequest = GetInvoiceNumberRequest;
		return ((InvoiceServicePortType)this).getInvoiceNumberAsync(getInvoiceNumberRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getCertificateStatusResponse1 InvoiceServicePortType.getCertificateStatus(getCertificateStatusRequest1 request)
	{
		return Channel.getCertificateStatus(request);
	}

	public GetCertificateStatusResponse getCertificateStatus(GetCertificateStatusRequest GetCertificateStatusRequest)
	{
		getCertificateStatusRequest1 getCertificateStatusRequest2 = new getCertificateStatusRequest1();
		getCertificateStatusRequest2.GetCertificateStatusRequest = GetCertificateStatusRequest;
		return ((InvoiceServicePortType)this).getCertificateStatus(getCertificateStatusRequest2).GetCertificateStatusResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getCertificateStatusResponse1> InvoiceServicePortType.getCertificateStatusAsync(getCertificateStatusRequest1 request)
	{
		return Channel.getCertificateStatusAsync(request);
	}

	public Task<getCertificateStatusResponse1> getCertificateStatusAsync(GetCertificateStatusRequest GetCertificateStatusRequest)
	{
		getCertificateStatusRequest1 getCertificateStatusRequest2 = new getCertificateStatusRequest1();
		getCertificateStatusRequest2.GetCertificateStatusRequest = GetCertificateStatusRequest;
		return ((InvoiceServicePortType)this).getCertificateStatusAsync(getCertificateStatusRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getSubmittedListResponse InvoiceServicePortType.getSubmittedList(getSubmittedListRequest request)
	{
		return Channel.getSubmittedList(request);
	}

	public GetInvoicesResponse getSubmittedList(GetInvoicesRequest GetSubmittedListRequest)
	{
		getSubmittedListRequest getSubmittedListRequest2 = new getSubmittedListRequest();
		getSubmittedListRequest2.GetSubmittedListRequest = GetSubmittedListRequest;
		return ((InvoiceServicePortType)this).getSubmittedList(getSubmittedListRequest2).GetSubmittedListResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getSubmittedListResponse> InvoiceServicePortType.getSubmittedListAsync(getSubmittedListRequest request)
	{
		return Channel.getSubmittedListAsync(request);
	}

	public Task<getSubmittedListResponse> getSubmittedListAsync(GetInvoicesRequest GetSubmittedListRequest)
	{
		getSubmittedListRequest getSubmittedListRequest2 = new getSubmittedListRequest();
		getSubmittedListRequest2.GetSubmittedListRequest = GetSubmittedListRequest;
		return ((InvoiceServicePortType)this).getSubmittedListAsync(getSubmittedListRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getSubmittedListCountResponse InvoiceServicePortType.getSubmittedListCount(getSubmittedListCountRequest request)
	{
		return Channel.getSubmittedListCount(request);
	}

	public GetListCountResponse getSubmittedListCount(GetInvoicesRequest GetSubmittedListCountRequest)
	{
		getSubmittedListCountRequest getSubmittedListCountRequest2 = new getSubmittedListCountRequest();
		getSubmittedListCountRequest2.GetSubmittedListCountRequest = GetSubmittedListCountRequest;
		return ((InvoiceServicePortType)this).getSubmittedListCount(getSubmittedListCountRequest2).GetSubmittedListCountResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getSubmittedListCountResponse> InvoiceServicePortType.getSubmittedListCountAsync(getSubmittedListCountRequest request)
	{
		return Channel.getSubmittedListCountAsync(request);
	}

	public Task<getSubmittedListCountResponse> getSubmittedListCountAsync(GetInvoicesRequest GetSubmittedListCountRequest)
	{
		getSubmittedListCountRequest getSubmittedListCountRequest2 = new getSubmittedListCountRequest();
		getSubmittedListCountRequest2.GetSubmittedListCountRequest = GetSubmittedListCountRequest;
		return ((InvoiceServicePortType)this).getSubmittedListCountAsync(getSubmittedListCountRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getInvalidListResponse InvoiceServicePortType.getInvalidList(getInvalidListRequest request)
	{
		return Channel.getInvalidList(request);
	}

	public GetInvoicesResponse getInvalidList(GetInvoicesRequest GetInvalidListRequest)
	{
		getInvalidListRequest getInvalidListRequest2 = new getInvalidListRequest();
		getInvalidListRequest2.GetInvalidListRequest = GetInvalidListRequest;
		return ((InvoiceServicePortType)this).getInvalidList(getInvalidListRequest2).GetInvalidListResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getInvalidListResponse> InvoiceServicePortType.getInvalidListAsync(getInvalidListRequest request)
	{
		return Channel.getInvalidListAsync(request);
	}

	public Task<getInvalidListResponse> getInvalidListAsync(GetInvoicesRequest GetInvalidListRequest)
	{
		getInvalidListRequest getInvalidListRequest2 = new getInvalidListRequest();
		getInvalidListRequest2.GetInvalidListRequest = GetInvalidListRequest;
		return ((InvoiceServicePortType)this).getInvalidListAsync(getInvalidListRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getInvalidListCountResponse InvoiceServicePortType.getInvalidListCount(getInvalidListCountRequest request)
	{
		return Channel.getInvalidListCount(request);
	}

	public GetListCountResponse getInvalidListCount(GetInvoicesRequest GetInvalidListCountRequest)
	{
		getInvalidListCountRequest getInvalidListCountRequest2 = new getInvalidListCountRequest();
		getInvalidListCountRequest2.GetInvalidListCountRequest = GetInvalidListCountRequest;
		return ((InvoiceServicePortType)this).getInvalidListCount(getInvalidListCountRequest2).GetInvalidListCountResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getInvalidListCountResponse> InvoiceServicePortType.getInvalidListCountAsync(getInvalidListCountRequest request)
	{
		return Channel.getInvalidListCountAsync(request);
	}

	public Task<getInvalidListCountResponse> getInvalidListCountAsync(GetInvoicesRequest GetInvalidListCountRequest)
	{
		getInvalidListCountRequest getInvalidListCountRequest2 = new getInvalidListCountRequest();
		getInvalidListCountRequest2.GetInvalidListCountRequest = GetInvalidListCountRequest;
		return ((InvoiceServicePortType)this).getInvalidListCountAsync(getInvalidListCountRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getOverdueInvalidListResponse InvoiceServicePortType.getOverdueInvalidList(getOverdueInvalidListRequest request)
	{
		return Channel.getOverdueInvalidList(request);
	}

	public GetInvoicesResponse getOverdueInvalidList(GetInvoicesRequest GetOverdueInvalidListRequest)
	{
		getOverdueInvalidListRequest getOverdueInvalidListRequest2 = new getOverdueInvalidListRequest();
		getOverdueInvalidListRequest2.GetOverdueInvalidListRequest = GetOverdueInvalidListRequest;
		return ((InvoiceServicePortType)this).getOverdueInvalidList(getOverdueInvalidListRequest2).GetOverdueInvalidListResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getOverdueInvalidListResponse> InvoiceServicePortType.getOverdueInvalidListAsync(getOverdueInvalidListRequest request)
	{
		return Channel.getOverdueInvalidListAsync(request);
	}

	public Task<getOverdueInvalidListResponse> getOverdueInvalidListAsync(GetInvoicesRequest GetOverdueInvalidListRequest)
	{
		getOverdueInvalidListRequest getOverdueInvalidListRequest2 = new getOverdueInvalidListRequest();
		getOverdueInvalidListRequest2.GetOverdueInvalidListRequest = GetOverdueInvalidListRequest;
		return ((InvoiceServicePortType)this).getOverdueInvalidListAsync(getOverdueInvalidListRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getOverdueInvalidListCountResponse InvoiceServicePortType.getOverdueInvalidListCount(getOverdueInvalidListCountRequest request)
	{
		return Channel.getOverdueInvalidListCount(request);
	}

	public GetListCountResponse getOverdueInvalidListCount(GetInvoicesRequest GetOverdueInvalidListCountRequest)
	{
		getOverdueInvalidListCountRequest getOverdueInvalidListCountRequest2 = new getOverdueInvalidListCountRequest();
		getOverdueInvalidListCountRequest2.GetOverdueInvalidListCountRequest = GetOverdueInvalidListCountRequest;
		return ((InvoiceServicePortType)this).getOverdueInvalidListCount(getOverdueInvalidListCountRequest2).GetOverdueInvalidListCountResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getOverdueInvalidListCountResponse> InvoiceServicePortType.getOverdueInvalidListCountAsync(getOverdueInvalidListCountRequest request)
	{
		return Channel.getOverdueInvalidListCountAsync(request);
	}

	public Task<getOverdueInvalidListCountResponse> getOverdueInvalidListCountAsync(GetInvoicesRequest GetOverdueInvalidListCountRequest)
	{
		getOverdueInvalidListCountRequest getOverdueInvalidListCountRequest2 = new getOverdueInvalidListCountRequest();
		getOverdueInvalidListCountRequest2.GetOverdueInvalidListCountRequest = GetOverdueInvalidListCountRequest;
		return ((InvoiceServicePortType)this).getOverdueInvalidListCountAsync(getOverdueInvalidListCountRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getReceivedListResponse InvoiceServicePortType.getReceivedList(getReceivedListRequest request)
	{
		return Channel.getReceivedList(request);
	}

	public GetInvoicesResponse getReceivedList(GetInvoicesRequest GetReceivedListRequest)
	{
		getReceivedListRequest getReceivedListRequest2 = new getReceivedListRequest();
		getReceivedListRequest2.GetReceivedListRequest = GetReceivedListRequest;
		return ((InvoiceServicePortType)this).getReceivedList(getReceivedListRequest2).GetReceivedListResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getReceivedListResponse> InvoiceServicePortType.getReceivedListAsync(getReceivedListRequest request)
	{
		return Channel.getReceivedListAsync(request);
	}

	public Task<getReceivedListResponse> getReceivedListAsync(GetInvoicesRequest GetReceivedListRequest)
	{
		getReceivedListRequest getReceivedListRequest2 = new getReceivedListRequest();
		getReceivedListRequest2.GetReceivedListRequest = GetReceivedListRequest;
		return ((InvoiceServicePortType)this).getReceivedListAsync(getReceivedListRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getReceivedListCountResponse InvoiceServicePortType.getReceivedListCount(getReceivedListCountRequest request)
	{
		return Channel.getReceivedListCount(request);
	}

	public GetListCountResponse getReceivedListCount(GetInvoicesRequest GetReceivedListCountRequest)
	{
		getReceivedListCountRequest getReceivedListCountRequest2 = new getReceivedListCountRequest();
		getReceivedListCountRequest2.GetReceivedListCountRequest = GetReceivedListCountRequest;
		return ((InvoiceServicePortType)this).getReceivedListCount(getReceivedListCountRequest2).GetReceivedListCountResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getReceivedListCountResponse> InvoiceServicePortType.getReceivedListCountAsync(getReceivedListCountRequest request)
	{
		return Channel.getReceivedListCountAsync(request);
	}

	public Task<getReceivedListCountResponse> getReceivedListCountAsync(GetInvoicesRequest GetReceivedListCountRequest)
	{
		getReceivedListCountRequest getReceivedListCountRequest2 = new getReceivedListCountRequest();
		getReceivedListCountRequest2.GetReceivedListCountRequest = GetReceivedListCountRequest;
		return ((InvoiceServicePortType)this).getReceivedListCountAsync(getReceivedListCountRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getDocByInvoiceNumberResponse1 InvoiceServicePortType.getDocByInvoiceNumber(getDocByInvoiceNumberRequest1 request)
	{
		return Channel.getDocByInvoiceNumber(request);
	}

	public GetDocByInvoiceNumberResponse getDocByInvoiceNumber(GetDocByInvoiceNumberRequest GetDocByInvoiceNumberRequest)
	{
		getDocByInvoiceNumberRequest1 getDocByInvoiceNumberRequest2 = new getDocByInvoiceNumberRequest1();
		getDocByInvoiceNumberRequest2.GetDocByInvoiceNumberRequest = GetDocByInvoiceNumberRequest;
		return ((InvoiceServicePortType)this).getDocByInvoiceNumber(getDocByInvoiceNumberRequest2).GetDocByInvoiceNumberResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getDocByInvoiceNumberResponse1> InvoiceServicePortType.getDocByInvoiceNumberAsync(getDocByInvoiceNumberRequest1 request)
	{
		return Channel.getDocByInvoiceNumberAsync(request);
	}

	public Task<getDocByInvoiceNumberResponse1> getDocByInvoiceNumberAsync(GetDocByInvoiceNumberRequest GetDocByInvoiceNumberRequest)
	{
		getDocByInvoiceNumberRequest1 getDocByInvoiceNumberRequest2 = new getDocByInvoiceNumberRequest1();
		getDocByInvoiceNumberRequest2.GetDocByInvoiceNumberRequest = GetDocByInvoiceNumberRequest;
		return ((InvoiceServicePortType)this).getDocByInvoiceNumberAsync(getDocByInvoiceNumberRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getParentDocByInvoiceNumberResponse1 InvoiceServicePortType.getParentDocByInvoiceNumber(getParentDocByInvoiceNumberRequest1 request)
	{
		return Channel.getParentDocByInvoiceNumber(request);
	}

	public GetParentDocByInvoiceNumberResponse getParentDocByInvoiceNumber(GetParentDocByInvoiceNumberRequest GetParentDocByInvoiceNumberRequest)
	{
		getParentDocByInvoiceNumberRequest1 getParentDocByInvoiceNumberRequest2 = new getParentDocByInvoiceNumberRequest1();
		getParentDocByInvoiceNumberRequest2.GetParentDocByInvoiceNumberRequest = GetParentDocByInvoiceNumberRequest;
		return ((InvoiceServicePortType)this).getParentDocByInvoiceNumber(getParentDocByInvoiceNumberRequest2).GetParentDocByInvoiceNumberResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getParentDocByInvoiceNumberResponse1> InvoiceServicePortType.getParentDocByInvoiceNumberAsync(getParentDocByInvoiceNumberRequest1 request)
	{
		return Channel.getParentDocByInvoiceNumberAsync(request);
	}

	public Task<getParentDocByInvoiceNumberResponse1> getParentDocByInvoiceNumberAsync(GetParentDocByInvoiceNumberRequest GetParentDocByInvoiceNumberRequest)
	{
		getParentDocByInvoiceNumberRequest1 getParentDocByInvoiceNumberRequest2 = new getParentDocByInvoiceNumberRequest1();
		getParentDocByInvoiceNumberRequest2.GetParentDocByInvoiceNumberRequest = GetParentDocByInvoiceNumberRequest;
		return ((InvoiceServicePortType)this).getParentDocByInvoiceNumberAsync(getParentDocByInvoiceNumberRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	signInvoiceByBuyerResponse1 InvoiceServicePortType.signInvoiceByBuyer(signInvoiceByBuyerRequest1 request)
	{
		return Channel.signInvoiceByBuyer(request);
	}

	public SignInvoiceByBuyerResponse signInvoiceByBuyer(SignInvoiceByBuyerRequest SignInvoiceByBuyerRequest)
	{
		signInvoiceByBuyerRequest1 signInvoiceByBuyerRequest2 = new signInvoiceByBuyerRequest1();
		signInvoiceByBuyerRequest2.SignInvoiceByBuyerRequest = SignInvoiceByBuyerRequest;
		return ((InvoiceServicePortType)this).signInvoiceByBuyer(signInvoiceByBuyerRequest2).SignInvoiceByBuyerResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<signInvoiceByBuyerResponse1> InvoiceServicePortType.signInvoiceByBuyerAsync(signInvoiceByBuyerRequest1 request)
	{
		return Channel.signInvoiceByBuyerAsync(request);
	}

	public Task<signInvoiceByBuyerResponse1> signInvoiceByBuyerAsync(SignInvoiceByBuyerRequest SignInvoiceByBuyerRequest)
	{
		signInvoiceByBuyerRequest1 signInvoiceByBuyerRequest2 = new signInvoiceByBuyerRequest1();
		signInvoiceByBuyerRequest2.SignInvoiceByBuyerRequest = SignInvoiceByBuyerRequest;
		return ((InvoiceServicePortType)this).signInvoiceByBuyerAsync(signInvoiceByBuyerRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getReceiptResponse1 InvoiceServicePortType.getReceipt(getReceiptRequest1 request)
	{
		return Channel.getReceipt(request);
	}

	public GetReceiptResponse getReceipt(GetReceiptRequest GetReceiptRequest)
	{
		getReceiptRequest1 getReceiptRequest2 = new getReceiptRequest1();
		getReceiptRequest2.GetReceiptRequest = GetReceiptRequest;
		return ((InvoiceServicePortType)this).getReceipt(getReceiptRequest2).GetReceiptResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getReceiptResponse1> InvoiceServicePortType.getReceiptAsync(getReceiptRequest1 request)
	{
		return Channel.getReceiptAsync(request);
	}

	public Task<getReceiptResponse1> getReceiptAsync(GetReceiptRequest GetReceiptRequest)
	{
		getReceiptRequest1 getReceiptRequest2 = new getReceiptRequest1();
		getReceiptRequest2.GetReceiptRequest = GetReceiptRequest;
		return ((InvoiceServicePortType)this).getReceiptAsync(getReceiptRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getInvalidationRequestResponse1 InvoiceServicePortType.getInvalidationRequest(getInvalidationRequestRequest1 request)
	{
		return Channel.getInvalidationRequest(request);
	}

	public GetInvalidationRequestResponse getInvalidationRequest(GetInvalidationRequestRequest GetInvalidationRequestRequest)
	{
		getInvalidationRequestRequest1 getInvalidationRequestRequest2 = new getInvalidationRequestRequest1();
		getInvalidationRequestRequest2.GetInvalidationRequestRequest = GetInvalidationRequestRequest;
		return ((InvoiceServicePortType)this).getInvalidationRequest(getInvalidationRequestRequest2).GetInvalidationRequestResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getInvalidationRequestResponse1> InvoiceServicePortType.getInvalidationRequestAsync(getInvalidationRequestRequest1 request)
	{
		return Channel.getInvalidationRequestAsync(request);
	}

	public Task<getInvalidationRequestResponse1> getInvalidationRequestAsync(GetInvalidationRequestRequest GetInvalidationRequestRequest)
	{
		getInvalidationRequestRequest1 getInvalidationRequestRequest2 = new getInvalidationRequestRequest1();
		getInvalidationRequestRequest2.GetInvalidationRequestRequest = GetInvalidationRequestRequest;
		return ((InvoiceServicePortType)this).getInvalidationRequestAsync(getInvalidationRequestRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getSignatureRevokationRequestResponse1 InvoiceServicePortType.getSignatureRevokationRequest(getSignatureRevokationRequestRequest1 request)
	{
		return Channel.getSignatureRevokationRequest(request);
	}

	public GetSignatureRevokationRequestResponse getSignatureRevokationRequest(GetSignatureRevokationRequestRequest GetSignatureRevokationRequestRequest)
	{
		getSignatureRevokationRequestRequest1 getSignatureRevokationRequestRequest2 = new getSignatureRevokationRequestRequest1();
		getSignatureRevokationRequestRequest2.GetSignatureRevokationRequestRequest = GetSignatureRevokationRequestRequest;
		return ((InvoiceServicePortType)this).getSignatureRevokationRequest(getSignatureRevokationRequestRequest2).GetSignatureRevokationRequestResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getSignatureRevokationRequestResponse1> InvoiceServicePortType.getSignatureRevokationRequestAsync(getSignatureRevokationRequestRequest1 request)
	{
		return Channel.getSignatureRevokationRequestAsync(request);
	}

	public Task<getSignatureRevokationRequestResponse1> getSignatureRevokationRequestAsync(GetSignatureRevokationRequestRequest GetSignatureRevokationRequestRequest)
	{
		getSignatureRevokationRequestRequest1 getSignatureRevokationRequestRequest2 = new getSignatureRevokationRequestRequest1();
		getSignatureRevokationRequestRequest2.GetSignatureRevokationRequestRequest = GetSignatureRevokationRequestRequest;
		return ((InvoiceServicePortType)this).getSignatureRevokationRequestAsync(getSignatureRevokationRequestRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getInvoiceNumberStatusResponse1 InvoiceServicePortType.getInvoiceNumberStatus(getInvoiceNumberStatusRequest1 request)
	{
		return Channel.getInvoiceNumberStatus(request);
	}

	public GetInvoiceNumberStatusResponse getInvoiceNumberStatus(GetInvoiceNumberStatusRequest GetInvoiceNumberStatusRequest)
	{
		getInvoiceNumberStatusRequest1 getInvoiceNumberStatusRequest2 = new getInvoiceNumberStatusRequest1();
		getInvoiceNumberStatusRequest2.GetInvoiceNumberStatusRequest = GetInvoiceNumberStatusRequest;
		return ((InvoiceServicePortType)this).getInvoiceNumberStatus(getInvoiceNumberStatusRequest2).GetInvoiceNumberStatusResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getInvoiceNumberStatusResponse1> InvoiceServicePortType.getInvoiceNumberStatusAsync(getInvoiceNumberStatusRequest1 request)
	{
		return Channel.getInvoiceNumberStatusAsync(request);
	}

	public Task<getInvoiceNumberStatusResponse1> getInvoiceNumberStatusAsync(GetInvoiceNumberStatusRequest GetInvoiceNumberStatusRequest)
	{
		getInvoiceNumberStatusRequest1 getInvoiceNumberStatusRequest2 = new getInvoiceNumberStatusRequest1();
		getInvoiceNumberStatusRequest2.GetInvoiceNumberStatusRequest = GetInvoiceNumberStatusRequest;
		return ((InvoiceServicePortType)this).getInvoiceNumberStatusAsync(getInvoiceNumberStatusRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getTaxFreeGoodsListResponse1 InvoiceServicePortType.getTaxFreeGoodsList(getTaxFreeGoodsListRequest1 request)
	{
		return Channel.getTaxFreeGoodsList(request);
	}

	public GetTaxFreeGoodsListResponse getTaxFreeGoodsList(GetTaxFreeGoodsListRequest GetTaxFreeGoodsListRequest)
	{
		getTaxFreeGoodsListRequest1 getTaxFreeGoodsListRequest2 = new getTaxFreeGoodsListRequest1();
		getTaxFreeGoodsListRequest2.GetTaxFreeGoodsListRequest = GetTaxFreeGoodsListRequest;
		return ((InvoiceServicePortType)this).getTaxFreeGoodsList(getTaxFreeGoodsListRequest2).GetTaxFreeGoodsListResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getTaxFreeGoodsListResponse1> InvoiceServicePortType.getTaxFreeGoodsListAsync(getTaxFreeGoodsListRequest1 request)
	{
		return Channel.getTaxFreeGoodsListAsync(request);
	}

	public Task<getTaxFreeGoodsListResponse1> getTaxFreeGoodsListAsync(GetTaxFreeGoodsListRequest GetTaxFreeGoodsListRequest)
	{
		getTaxFreeGoodsListRequest1 getTaxFreeGoodsListRequest2 = new getTaxFreeGoodsListRequest1();
		getTaxFreeGoodsListRequest2.GetTaxFreeGoodsListRequest = GetTaxFreeGoodsListRequest;
		return ((InvoiceServicePortType)this).getTaxFreeGoodsListAsync(getTaxFreeGoodsListRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getTaxFreeCountryListResponse1 InvoiceServicePortType.getTaxFreeCountryList(getTaxFreeCountryListRequest1 request)
	{
		return Channel.getTaxFreeCountryList(request);
	}

	public GetTaxFreeCountryListResponse getTaxFreeCountryList(GetTaxFreeCountryListRequest GetTaxFreeCountryListRequest)
	{
		getTaxFreeCountryListRequest1 getTaxFreeCountryListRequest2 = new getTaxFreeCountryListRequest1();
		getTaxFreeCountryListRequest2.GetTaxFreeCountryListRequest = GetTaxFreeCountryListRequest;
		return ((InvoiceServicePortType)this).getTaxFreeCountryList(getTaxFreeCountryListRequest2).GetTaxFreeCountryListResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getTaxFreeCountryListResponse1> InvoiceServicePortType.getTaxFreeCountryListAsync(getTaxFreeCountryListRequest1 request)
	{
		return Channel.getTaxFreeCountryListAsync(request);
	}

	public Task<getTaxFreeCountryListResponse1> getTaxFreeCountryListAsync(GetTaxFreeCountryListRequest GetTaxFreeCountryListRequest)
	{
		getTaxFreeCountryListRequest1 getTaxFreeCountryListRequest2 = new getTaxFreeCountryListRequest1();
		getTaxFreeCountryListRequest2.GetTaxFreeCountryListRequest = GetTaxFreeCountryListRequest;
		return ((InvoiceServicePortType)this).getTaxFreeCountryListAsync(getTaxFreeCountryListRequest2);
	}

	public new virtual Task OpenAsync()
	{
		return Task.Factory.FromAsync(((ICommunicationObject)this).BeginOpen((AsyncCallback)null, (object)null), ((ICommunicationObject)this).EndOpen);
	}

	private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == EndpointConfiguration.InvoiceServicePort)
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
		if (endpointConfiguration == EndpointConfiguration.InvoiceServicePort)
		{
			return new EndpointAddress("http://ews.taxservice.am/taxsystem-fe-ws/taxpayer/invoiceService");
		}
		throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'.");
	}

	private static Binding GetDefaultBinding()
	{
		return GetBindingForEndpoint(EndpointConfiguration.InvoiceServicePort);
	}

	private static EndpointAddress GetDefaultEndpointAddress()
	{
		return GetEndpointAddress(EndpointConfiguration.InvoiceServicePort);
	}
}
