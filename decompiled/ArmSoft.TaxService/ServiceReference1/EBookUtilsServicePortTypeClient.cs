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
internal class EBookUtilsServicePortTypeClient : ClientBase<EBookUtilsServicePortType>, EBookUtilsServicePortType
{
	public enum EndpointConfiguration
	{
		EBookUtilsServicePort
	}

	public EBookUtilsServicePortTypeClient()
		: base(GetDefaultBinding(), GetDefaultEndpointAddress())
	{
		Endpoint.Name = EndpointConfiguration.EBookUtilsServicePort.ToString();
	}

	public EBookUtilsServicePortTypeClient(EndpointConfiguration endpointConfiguration)
		: base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public EBookUtilsServicePortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public EBookUtilsServicePortTypeClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), remoteAddress)
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public EBookUtilsServicePortTypeClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getTpBookListResponse1 EBookUtilsServicePortType.getTpBookList(getTpBookListRequest1 request)
	{
		return Channel.getTpBookList(request);
	}

	public GetTpBookListResponse getTpBookList(GetTpBookListRequest GetTpBookListRequest)
	{
		getTpBookListRequest1 getTpBookListRequest2 = new getTpBookListRequest1();
		getTpBookListRequest2.GetTpBookListRequest = GetTpBookListRequest;
		return ((EBookUtilsServicePortType)this).getTpBookList(getTpBookListRequest2).GetTpBookListResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getTpBookListResponse1> EBookUtilsServicePortType.getTpBookListAsync(getTpBookListRequest1 request)
	{
		return Channel.getTpBookListAsync(request);
	}

	public Task<getTpBookListResponse1> getTpBookListAsync(GetTpBookListRequest GetTpBookListRequest)
	{
		getTpBookListRequest1 getTpBookListRequest2 = new getTpBookListRequest1();
		getTpBookListRequest2.GetTpBookListRequest = GetTpBookListRequest;
		return ((EBookUtilsServicePortType)this).getTpBookListAsync(getTpBookListRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getObjectByOinResponse1 EBookUtilsServicePortType.getObjectByOin(getObjectByOinRequest1 request)
	{
		return Channel.getObjectByOin(request);
	}

	public GetObjectByOinResponse getObjectByOin(GetObjectByOinRequest GetObjectByOinRequest)
	{
		getObjectByOinRequest1 getObjectByOinRequest2 = new getObjectByOinRequest1();
		getObjectByOinRequest2.GetObjectByOinRequest = GetObjectByOinRequest;
		return ((EBookUtilsServicePortType)this).getObjectByOin(getObjectByOinRequest2).GetObjectByOinResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getObjectByOinResponse1> EBookUtilsServicePortType.getObjectByOinAsync(getObjectByOinRequest1 request)
	{
		return Channel.getObjectByOinAsync(request);
	}

	public Task<getObjectByOinResponse1> getObjectByOinAsync(GetObjectByOinRequest GetObjectByOinRequest)
	{
		getObjectByOinRequest1 getObjectByOinRequest2 = new getObjectByOinRequest1();
		getObjectByOinRequest2.GetObjectByOinRequest = GetObjectByOinRequest;
		return ((EBookUtilsServicePortType)this).getObjectByOinAsync(getObjectByOinRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getObjectListResponse1 EBookUtilsServicePortType.getObjectList(getObjectListRequest1 request)
	{
		return Channel.getObjectList(request);
	}

	public GetObjectListResponse getObjectList(GetObjectListRequest GetObjectListRequest)
	{
		getObjectListRequest1 getObjectListRequest2 = new getObjectListRequest1();
		getObjectListRequest2.GetObjectListRequest = GetObjectListRequest;
		return ((EBookUtilsServicePortType)this).getObjectList(getObjectListRequest2).GetObjectListResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getObjectListResponse1> EBookUtilsServicePortType.getObjectListAsync(getObjectListRequest1 request)
	{
		return Channel.getObjectListAsync(request);
	}

	public Task<getObjectListResponse1> getObjectListAsync(GetObjectListRequest GetObjectListRequest)
	{
		getObjectListRequest1 getObjectListRequest2 = new getObjectListRequest1();
		getObjectListRequest2.GetObjectListRequest = GetObjectListRequest;
		return ((EBookUtilsServicePortType)this).getObjectListAsync(getObjectListRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getActiveObjectListResponse1 EBookUtilsServicePortType.getActiveObjectList(getActiveObjectListRequest1 request)
	{
		return Channel.getActiveObjectList(request);
	}

	public GetActiveObjectListResponse getActiveObjectList(GetActiveObjectListRequest GetActiveObjectListRequest)
	{
		getActiveObjectListRequest1 getActiveObjectListRequest2 = new getActiveObjectListRequest1();
		getActiveObjectListRequest2.GetActiveObjectListRequest = GetActiveObjectListRequest;
		return ((EBookUtilsServicePortType)this).getActiveObjectList(getActiveObjectListRequest2).GetActiveObjectListResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getActiveObjectListResponse1> EBookUtilsServicePortType.getActiveObjectListAsync(getActiveObjectListRequest1 request)
	{
		return Channel.getActiveObjectListAsync(request);
	}

	public Task<getActiveObjectListResponse1> getActiveObjectListAsync(GetActiveObjectListRequest GetActiveObjectListRequest)
	{
		getActiveObjectListRequest1 getActiveObjectListRequest2 = new getActiveObjectListRequest1();
		getActiveObjectListRequest2.GetActiveObjectListRequest = GetActiveObjectListRequest;
		return ((EBookUtilsServicePortType)this).getActiveObjectListAsync(getActiveObjectListRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getMarzListResponse1 EBookUtilsServicePortType.getMarzList(getMarzListRequest1 request)
	{
		return Channel.getMarzList(request);
	}

	public GetMarzListResponse getMarzList(GetMarzListRequest GetMarzListRequest)
	{
		getMarzListRequest1 getMarzListRequest2 = new getMarzListRequest1();
		getMarzListRequest2.GetMarzListRequest = GetMarzListRequest;
		return ((EBookUtilsServicePortType)this).getMarzList(getMarzListRequest2).GetMarzListResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getMarzListResponse1> EBookUtilsServicePortType.getMarzListAsync(getMarzListRequest1 request)
	{
		return Channel.getMarzListAsync(request);
	}

	public Task<getMarzListResponse1> getMarzListAsync(GetMarzListRequest GetMarzListRequest)
	{
		getMarzListRequest1 getMarzListRequest2 = new getMarzListRequest1();
		getMarzListRequest2.GetMarzListRequest = GetMarzListRequest;
		return ((EBookUtilsServicePortType)this).getMarzListAsync(getMarzListRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getRegionListByMarzResponse1 EBookUtilsServicePortType.getRegionListByMarz(getRegionListByMarzRequest1 request)
	{
		return Channel.getRegionListByMarz(request);
	}

	public GetRegionListByMarzResponse getRegionListByMarz(GetRegionListByMarzRequest GetRegionListByMarzRequest)
	{
		getRegionListByMarzRequest1 getRegionListByMarzRequest2 = new getRegionListByMarzRequest1();
		getRegionListByMarzRequest2.GetRegionListByMarzRequest = GetRegionListByMarzRequest;
		return ((EBookUtilsServicePortType)this).getRegionListByMarz(getRegionListByMarzRequest2).GetRegionListByMarzResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getRegionListByMarzResponse1> EBookUtilsServicePortType.getRegionListByMarzAsync(getRegionListByMarzRequest1 request)
	{
		return Channel.getRegionListByMarzAsync(request);
	}

	public Task<getRegionListByMarzResponse1> getRegionListByMarzAsync(GetRegionListByMarzRequest GetRegionListByMarzRequest)
	{
		getRegionListByMarzRequest1 getRegionListByMarzRequest2 = new getRegionListByMarzRequest1();
		getRegionListByMarzRequest2.GetRegionListByMarzRequest = GetRegionListByMarzRequest;
		return ((EBookUtilsServicePortType)this).getRegionListByMarzAsync(getRegionListByMarzRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	getTownListResponse1 EBookUtilsServicePortType.getTownList(getTownListRequest1 request)
	{
		return Channel.getTownList(request);
	}

	public GetTownListResponse getTownList(GetTownListRequest GetTownListRequest)
	{
		getTownListRequest1 getTownListRequest2 = new getTownListRequest1();
		getTownListRequest2.GetTownListRequest = GetTownListRequest;
		return ((EBookUtilsServicePortType)this).getTownList(getTownListRequest2).GetTownListResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<getTownListResponse1> EBookUtilsServicePortType.getTownListAsync(getTownListRequest1 request)
	{
		return Channel.getTownListAsync(request);
	}

	public Task<getTownListResponse1> getTownListAsync(GetTownListRequest GetTownListRequest)
	{
		getTownListRequest1 getTownListRequest2 = new getTownListRequest1();
		getTownListRequest2.GetTownListRequest = GetTownListRequest;
		return ((EBookUtilsServicePortType)this).getTownListAsync(getTownListRequest2);
	}

	public new virtual Task OpenAsync()
	{
		return Task.Factory.FromAsync(((ICommunicationObject)this).BeginOpen((AsyncCallback)null, (object)null), ((ICommunicationObject)this).EndOpen);
	}

	private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == EndpointConfiguration.EBookUtilsServicePort)
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
		if (endpointConfiguration == EndpointConfiguration.EBookUtilsServicePort)
		{
			return new EndpointAddress("http://ews.taxservice.am/taxsystem-fe-ws/taxpayer/eBookUtilsService");
		}
		throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'.");
	}

	private static Binding GetDefaultBinding()
	{
		return GetBindingForEndpoint(EndpointConfiguration.EBookUtilsServicePort);
	}

	private static EndpointAddress GetDefaultEndpointAddress()
	{
		return GetEndpointAddress(EndpointConfiguration.EBookUtilsServicePort);
	}
}
