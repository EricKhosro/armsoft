using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Xml;

namespace CBA.ExchangeRates;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
public class GateSoapClient : ClientBase<GateSoap>, GateSoap
{
	public enum EndpointConfiguration
	{
		GateSoap,
		GateSoap12
	}

	public GateSoapClient(EndpointConfiguration endpointConfiguration)
		: base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public GateSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public GateSoapClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), remoteAddress)
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public GateSoapClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public ISOCodesDetailedResponse ISOCodesDetailed(ISOCodesDetailedRequest request)
	{
		return Channel.ISOCodesDetailed(request);
	}

	public Task<ISOCodesDetailedResponse> ISOCodesDetailedAsync(ISOCodesDetailedRequest request)
	{
		return Channel.ISOCodesDetailedAsync(request);
	}

	public ISOCodesResponse ISOCodes(ISOCodesRequest request)
	{
		return Channel.ISOCodes(request);
	}

	public Task<ISOCodesResponse> ISOCodesAsync(ISOCodesRequest request)
	{
		return Channel.ISOCodesAsync(request);
	}

	public ExchangeRatesLatestByISOResponse ExchangeRatesLatestByISO(ExchangeRatesLatestByISORequest request)
	{
		return Channel.ExchangeRatesLatestByISO(request);
	}

	public Task<ExchangeRatesLatestByISOResponse> ExchangeRatesLatestByISOAsync(ExchangeRatesLatestByISORequest request)
	{
		return Channel.ExchangeRatesLatestByISOAsync(request);
	}

	public ExchangeRatesByDateByISOResponse ExchangeRatesByDateByISO(ExchangeRatesByDateByISORequest request)
	{
		return Channel.ExchangeRatesByDateByISO(request);
	}

	public Task<ExchangeRatesByDateByISOResponse> ExchangeRatesByDateByISOAsync(ExchangeRatesByDateByISORequest request)
	{
		return Channel.ExchangeRatesByDateByISOAsync(request);
	}

	public ExchangeRatesByDateResponse ExchangeRatesByDate(ExchangeRatesByDateRequest request)
	{
		return Channel.ExchangeRatesByDate(request);
	}

	public Task<ExchangeRatesByDateResponse> ExchangeRatesByDateAsync(ExchangeRatesByDateRequest request)
	{
		return Channel.ExchangeRatesByDateAsync(request);
	}

	public ExchangeRatesLatestResponse ExchangeRatesLatest(ExchangeRatesLatestRequest request)
	{
		return Channel.ExchangeRatesLatest(request);
	}

	public Task<ExchangeRatesLatestResponse> ExchangeRatesLatestAsync(ExchangeRatesLatestRequest request)
	{
		return Channel.ExchangeRatesLatestAsync(request);
	}

	public ExchangeRatesByDateRangeByISOResponse ExchangeRatesByDateRangeByISO(ExchangeRatesByDateRangeByISORequest request)
	{
		return Channel.ExchangeRatesByDateRangeByISO(request);
	}

	public Task<ExchangeRatesByDateRangeByISOResponse> ExchangeRatesByDateRangeByISOAsync(ExchangeRatesByDateRangeByISORequest request)
	{
		return Channel.ExchangeRatesByDateRangeByISOAsync(request);
	}

	public new virtual Task OpenAsync()
	{
		return Task.Factory.FromAsync(((ICommunicationObject)this).BeginOpen((AsyncCallback)null, (object)null), ((ICommunicationObject)this).EndOpen);
	}

	private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
	{
		switch (endpointConfiguration)
		{
		case EndpointConfiguration.GateSoap:
			return new BasicHttpBinding
			{
				MaxBufferSize = int.MaxValue,
				ReaderQuotas = XmlDictionaryReaderQuotas.Max,
				MaxReceivedMessageSize = 2147483647L,
				AllowCookies = true,
				Security = 
				{
					Mode = BasicHttpSecurityMode.Transport
				}
			};
		case EndpointConfiguration.GateSoap12:
		{
			CustomBinding customBinding = new CustomBinding();
			TextMessageEncodingBindingElement item = new TextMessageEncodingBindingElement
			{
				MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None)
			};
			customBinding.Elements.Add(item);
			HttpsTransportBindingElement item2 = new HttpsTransportBindingElement
			{
				AllowCookies = true,
				MaxBufferSize = int.MaxValue,
				MaxReceivedMessageSize = 2147483647L
			};
			customBinding.Elements.Add(item2);
			return customBinding;
		}
		default:
			throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'.");
		}
	}

	private static EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
	{
		return endpointConfiguration switch
		{
			EndpointConfiguration.GateSoap => new EndpointAddress("https://api.cba.am/exchangerates.asmx"), 
			EndpointConfiguration.GateSoap12 => new EndpointAddress("https://api.cba.am/exchangerates.asmx"), 
			_ => throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'."), 
		};
	}
}
