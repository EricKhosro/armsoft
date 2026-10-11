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
internal class EBookServicePortTypeClient : ClientBase<EBookServicePortType>, EBookServicePortType
{
	public enum EndpointConfiguration
	{
		EBookServicePort
	}

	public EBookServicePortTypeClient()
		: base(GetDefaultBinding(), GetDefaultEndpointAddress())
	{
		Endpoint.Name = EndpointConfiguration.EBookServicePort.ToString();
	}

	public EBookServicePortTypeClient(EndpointConfiguration endpointConfiguration)
		: base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public EBookServicePortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public EBookServicePortTypeClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), remoteAddress)
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public EBookServicePortTypeClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getEbookListResponse1 EBookServicePortType.getEbookList(getEbookListRequest1 request)
	{
		return Channel.getEbookList(request);
	}

	public GetEBookListResponse getEbookList(GetEBookListRequest GetEBookListRequest)
	{
		getEbookListRequest1 getEbookListRequest2 = new getEbookListRequest1();
		getEbookListRequest2.GetEBookListRequest = GetEBookListRequest;
		return ((EBookServicePortType)this).getEbookList(getEbookListRequest2).GetEBookListResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getEbookListResponse1> EBookServicePortType.getEbookListAsync(getEbookListRequest1 request)
	{
		return Channel.getEbookListAsync(request);
	}

	public Task<getEbookListResponse1> getEbookListAsync(GetEBookListRequest GetEBookListRequest)
	{
		getEbookListRequest1 getEbookListRequest2 = new getEbookListRequest1();
		getEbookListRequest2.GetEBookListRequest = GetEBookListRequest;
		return ((EBookServicePortType)this).getEbookListAsync(getEbookListRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getEbookListCountResponse1 EBookServicePortType.getEbookListCount(getEbookListCountRequest1 request)
	{
		return Channel.getEbookListCount(request);
	}

	public GetEBookListCountResponse getEbookListCount(GetEBookListCountRequest GetEBookListCountRequest)
	{
		getEbookListCountRequest1 getEbookListCountRequest2 = new getEbookListCountRequest1();
		getEbookListCountRequest2.GetEBookListCountRequest = GetEBookListCountRequest;
		return ((EBookServicePortType)this).getEbookListCount(getEbookListCountRequest2).GetEBookListCountResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getEbookListCountResponse1> EBookServicePortType.getEbookListCountAsync(getEbookListCountRequest1 request)
	{
		return Channel.getEbookListCountAsync(request);
	}

	public Task<getEbookListCountResponse1> getEbookListCountAsync(GetEBookListCountRequest GetEBookListCountRequest)
	{
		getEbookListCountRequest1 getEbookListCountRequest2 = new getEbookListCountRequest1();
		getEbookListCountRequest2.GetEBookListCountRequest = GetEBookListCountRequest;
		return ((EBookServicePortType)this).getEbookListCountAsync(getEbookListCountRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getEbookResponse1 EBookServicePortType.getEbook(getEbookRequest1 request)
	{
		return Channel.getEbook(request);
	}

	public GetEBookResponse getEbook(GetEBookRequest GetEBookRequest)
	{
		getEbookRequest1 getEbookRequest2 = new getEbookRequest1();
		getEbookRequest2.GetEBookRequest = GetEBookRequest;
		return ((EBookServicePortType)this).getEbook(getEbookRequest2).GetEBookResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getEbookResponse1> EBookServicePortType.getEbookAsync(getEbookRequest1 request)
	{
		return Channel.getEbookAsync(request);
	}

	public Task<getEbookResponse1> getEbookAsync(GetEBookRequest GetEBookRequest)
	{
		getEbookRequest1 getEbookRequest2 = new getEbookRequest1();
		getEbookRequest2.GetEBookRequest = GetEBookRequest;
		return ((EBookServicePortType)this).getEbookAsync(getEbookRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	cancelEbookRowsResponse1 EBookServicePortType.cancelEbookRows(cancelEbookRowsRequest1 request)
	{
		return Channel.cancelEbookRows(request);
	}

	public CancelEbookRowsResponse cancelEbookRows(CancelEbookRowsRequest CancelEbookRowsRequest)
	{
		cancelEbookRowsRequest1 cancelEbookRowsRequest2 = new cancelEbookRowsRequest1();
		cancelEbookRowsRequest2.CancelEbookRowsRequest = CancelEbookRowsRequest;
		return ((EBookServicePortType)this).cancelEbookRows(cancelEbookRowsRequest2).CancelEbookRowsResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<cancelEbookRowsResponse1> EBookServicePortType.cancelEbookRowsAsync(cancelEbookRowsRequest1 request)
	{
		return Channel.cancelEbookRowsAsync(request);
	}

	public Task<cancelEbookRowsResponse1> cancelEbookRowsAsync(CancelEbookRowsRequest CancelEbookRowsRequest)
	{
		cancelEbookRowsRequest1 cancelEbookRowsRequest2 = new cancelEbookRowsRequest1();
		cancelEbookRowsRequest2.CancelEbookRowsRequest = CancelEbookRowsRequest;
		return ((EBookServicePortType)this).cancelEbookRowsAsync(cancelEbookRowsRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	postEbookRowsResponse1 EBookServicePortType.postEbookRows(postEbookRowsRequest1 request)
	{
		return Channel.postEbookRows(request);
	}

	public PostEbookRowsResponse postEbookRows(PostEbookRowsRequest PostEbookRowsRequest)
	{
		postEbookRowsRequest1 postEbookRowsRequest2 = new postEbookRowsRequest1();
		postEbookRowsRequest2.PostEbookRowsRequest = PostEbookRowsRequest;
		return ((EBookServicePortType)this).postEbookRows(postEbookRowsRequest2).PostEbookRowsResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<postEbookRowsResponse1> EBookServicePortType.postEbookRowsAsync(postEbookRowsRequest1 request)
	{
		return Channel.postEbookRowsAsync(request);
	}

	public Task<postEbookRowsResponse1> postEbookRowsAsync(PostEbookRowsRequest PostEbookRowsRequest)
	{
		postEbookRowsRequest1 postEbookRowsRequest2 = new postEbookRowsRequest1();
		postEbookRowsRequest2.PostEbookRowsRequest = PostEbookRowsRequest;
		return ((EBookServicePortType)this).postEbookRowsAsync(postEbookRowsRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getEbookDetailListCountResponse EBookServicePortType.getEbookDetailListCount(getEbookDetailListCountRequest request)
	{
		return Channel.getEbookDetailListCount(request);
	}

	public GetEbookDetailListCountRequestResponse getEbookDetailListCount(GetEbookDetailListCountRequestRequest GetEbookDetailListCountRequestRequest)
	{
		getEbookDetailListCountRequest getEbookDetailListCountRequest2 = new getEbookDetailListCountRequest();
		getEbookDetailListCountRequest2.GetEbookDetailListCountRequestRequest = GetEbookDetailListCountRequestRequest;
		return ((EBookServicePortType)this).getEbookDetailListCount(getEbookDetailListCountRequest2).GetEbookDetailListCountRequestResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getEbookDetailListCountResponse> EBookServicePortType.getEbookDetailListCountAsync(getEbookDetailListCountRequest request)
	{
		return Channel.getEbookDetailListCountAsync(request);
	}

	public Task<getEbookDetailListCountResponse> getEbookDetailListCountAsync(GetEbookDetailListCountRequestRequest GetEbookDetailListCountRequestRequest)
	{
		getEbookDetailListCountRequest getEbookDetailListCountRequest2 = new getEbookDetailListCountRequest();
		getEbookDetailListCountRequest2.GetEbookDetailListCountRequestRequest = GetEbookDetailListCountRequestRequest;
		return ((EBookServicePortType)this).getEbookDetailListCountAsync(getEbookDetailListCountRequest2);
	}

	public new virtual Task OpenAsync()
	{
		return Task.Factory.FromAsync(((ICommunicationObject)this).BeginOpen((AsyncCallback)null, (object)null), ((ICommunicationObject)this).EndOpen);
	}

	private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == EndpointConfiguration.EBookServicePort)
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
		if (endpointConfiguration == EndpointConfiguration.EBookServicePort)
		{
			return new EndpointAddress("http://ews.taxservice.am/taxsystem-fe-ws/taxpayer/eBookService");
		}
		throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'.");
	}

	private static Binding GetDefaultBinding()
	{
		return GetBindingForEndpoint(EndpointConfiguration.EBookServicePort);
	}

	private static EndpointAddress GetDefaultEndpointAddress()
	{
		return GetEndpointAddress(EndpointConfiguration.EBookServicePort);
	}
}
