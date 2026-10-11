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
internal class BookDeclServicePortTypeClient : ClientBase<BookDeclServicePortType>, BookDeclServicePortType
{
	public enum EndpointConfiguration
	{
		BookDeclServicePort
	}

	public BookDeclServicePortTypeClient()
		: base(GetDefaultBinding(), GetDefaultEndpointAddress())
	{
		Endpoint.Name = EndpointConfiguration.BookDeclServicePort.ToString();
	}

	public BookDeclServicePortTypeClient(EndpointConfiguration endpointConfiguration)
		: base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public BookDeclServicePortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public BookDeclServicePortTypeClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), remoteAddress)
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public BookDeclServicePortTypeClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getSubmittedDeclsListResponse BookDeclServicePortType.getSubmittedDeclsList(getSubmittedDeclsListRequest request)
	{
		return Channel.getSubmittedDeclsList(request);
	}

	public GetBookDeclsResponse getSubmittedDeclsList(GetBookDeclsRequest GetSubmittedDeclsListRequest)
	{
		getSubmittedDeclsListRequest getSubmittedDeclsListRequest2 = new getSubmittedDeclsListRequest();
		getSubmittedDeclsListRequest2.GetSubmittedDeclsListRequest = GetSubmittedDeclsListRequest;
		return ((BookDeclServicePortType)this).getSubmittedDeclsList(getSubmittedDeclsListRequest2).GetSubmittedDeclsListResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getSubmittedDeclsListResponse> BookDeclServicePortType.getSubmittedDeclsListAsync(getSubmittedDeclsListRequest request)
	{
		return Channel.getSubmittedDeclsListAsync(request);
	}

	public Task<getSubmittedDeclsListResponse> getSubmittedDeclsListAsync(GetBookDeclsRequest GetSubmittedDeclsListRequest)
	{
		getSubmittedDeclsListRequest getSubmittedDeclsListRequest2 = new getSubmittedDeclsListRequest();
		getSubmittedDeclsListRequest2.GetSubmittedDeclsListRequest = GetSubmittedDeclsListRequest;
		return ((BookDeclServicePortType)this).getSubmittedDeclsListAsync(getSubmittedDeclsListRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getSubmittedDeclsListCountResponse BookDeclServicePortType.getSubmittedDeclsListCount(getSubmittedDeclsListCountRequest request)
	{
		return Channel.getSubmittedDeclsListCount(request);
	}

	public GetBookDeclsCountResponse getSubmittedDeclsListCount(GetBookDeclsRequest GetSubmittedDeclsListCountRequest)
	{
		getSubmittedDeclsListCountRequest getSubmittedDeclsListCountRequest2 = new getSubmittedDeclsListCountRequest();
		getSubmittedDeclsListCountRequest2.GetSubmittedDeclsListCountRequest = GetSubmittedDeclsListCountRequest;
		return ((BookDeclServicePortType)this).getSubmittedDeclsListCount(getSubmittedDeclsListCountRequest2).GetSubmittedDeclsListCountResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getSubmittedDeclsListCountResponse> BookDeclServicePortType.getSubmittedDeclsListCountAsync(getSubmittedDeclsListCountRequest request)
	{
		return Channel.getSubmittedDeclsListCountAsync(request);
	}

	public Task<getSubmittedDeclsListCountResponse> getSubmittedDeclsListCountAsync(GetBookDeclsRequest GetSubmittedDeclsListCountRequest)
	{
		getSubmittedDeclsListCountRequest getSubmittedDeclsListCountRequest2 = new getSubmittedDeclsListCountRequest();
		getSubmittedDeclsListCountRequest2.GetSubmittedDeclsListCountRequest = GetSubmittedDeclsListCountRequest;
		return ((BookDeclServicePortType)this).getSubmittedDeclsListCountAsync(getSubmittedDeclsListCountRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getBookDeclByDlnResponse1 BookDeclServicePortType.getBookDeclByDln(getBookDeclByDlnRequest1 request)
	{
		return Channel.getBookDeclByDln(request);
	}

	public GetBookDeclByDlnResponse getBookDeclByDln(GetBookDeclByDlnRequest GetBookDeclByDlnRequest)
	{
		getBookDeclByDlnRequest1 getBookDeclByDlnRequest2 = new getBookDeclByDlnRequest1();
		getBookDeclByDlnRequest2.GetBookDeclByDlnRequest = GetBookDeclByDlnRequest;
		return ((BookDeclServicePortType)this).getBookDeclByDln(getBookDeclByDlnRequest2).GetBookDeclByDlnResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getBookDeclByDlnResponse1> BookDeclServicePortType.getBookDeclByDlnAsync(getBookDeclByDlnRequest1 request)
	{
		return Channel.getBookDeclByDlnAsync(request);
	}

	public Task<getBookDeclByDlnResponse1> getBookDeclByDlnAsync(GetBookDeclByDlnRequest GetBookDeclByDlnRequest)
	{
		getBookDeclByDlnRequest1 getBookDeclByDlnRequest2 = new getBookDeclByDlnRequest1();
		getBookDeclByDlnRequest2.GetBookDeclByDlnRequest = GetBookDeclByDlnRequest;
		return ((BookDeclServicePortType)this).getBookDeclByDlnAsync(getBookDeclByDlnRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getNotifPDFResponse1 BookDeclServicePortType.getNotifPDF(getNotifPDFRequest1 request)
	{
		return Channel.getNotifPDF(request);
	}

	public GetNotifPDFResponse getNotifPDF(GetNotifPDFRequest GetNotifPDFRequest)
	{
		getNotifPDFRequest1 getNotifPDFRequest2 = new getNotifPDFRequest1();
		getNotifPDFRequest2.GetNotifPDFRequest = GetNotifPDFRequest;
		return ((BookDeclServicePortType)this).getNotifPDF(getNotifPDFRequest2).GetNotifPDFResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getNotifPDFResponse1> BookDeclServicePortType.getNotifPDFAsync(getNotifPDFRequest1 request)
	{
		return Channel.getNotifPDFAsync(request);
	}

	public Task<getNotifPDFResponse1> getNotifPDFAsync(GetNotifPDFRequest GetNotifPDFRequest)
	{
		getNotifPDFRequest1 getNotifPDFRequest2 = new getNotifPDFRequest1();
		getNotifPDFRequest2.GetNotifPDFRequest = GetNotifPDFRequest;
		return ((BookDeclServicePortType)this).getNotifPDFAsync(getNotifPDFRequest2);
	}

	public new virtual Task OpenAsync()
	{
		return Task.Factory.FromAsync(((ICommunicationObject)this).BeginOpen((AsyncCallback)null, (object)null), ((ICommunicationObject)this).EndOpen);
	}

	private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == EndpointConfiguration.BookDeclServicePort)
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
		if (endpointConfiguration == EndpointConfiguration.BookDeclServicePort)
		{
			return new EndpointAddress("http://ews.taxservice.am/taxsystem-fe-ws/taxpayer/bookDeclService");
		}
		throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'.");
	}

	private static Binding GetDefaultBinding()
	{
		return GetBindingForEndpoint(EndpointConfiguration.BookDeclServicePort);
	}

	private static EndpointAddress GetDefaultEndpointAddress()
	{
		return GetEndpointAddress(EndpointConfiguration.BookDeclServicePort);
	}
}
