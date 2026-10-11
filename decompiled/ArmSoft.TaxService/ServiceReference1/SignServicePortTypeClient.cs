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
internal class SignServicePortTypeClient : ClientBase<SignServicePortType>, SignServicePortType
{
	public enum EndpointConfiguration
	{
		SignServicePort
	}

	public SignServicePortTypeClient()
		: base(GetDefaultBinding(), GetDefaultEndpointAddress())
	{
		Endpoint.Name = EndpointConfiguration.SignServicePort.ToString();
	}

	public SignServicePortTypeClient(EndpointConfiguration endpointConfiguration)
		: base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public SignServicePortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public SignServicePortTypeClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), remoteAddress)
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public SignServicePortTypeClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	signResponse1 SignServicePortType.sign(signRequest1 request)
	{
		return Channel.sign(request);
	}

	public SignResponse sign(SignRequest SignRequest)
	{
		signRequest1 signRequest2 = new signRequest1();
		signRequest2.SignRequest = SignRequest;
		return ((SignServicePortType)this).sign(signRequest2).SignResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<signResponse1> SignServicePortType.signAsync(signRequest1 request)
	{
		return Channel.signAsync(request);
	}

	public Task<signResponse1> signAsync(SignRequest SignRequest)
	{
		signRequest1 signRequest2 = new signRequest1();
		signRequest2.SignRequest = SignRequest;
		return ((SignServicePortType)this).signAsync(signRequest2);
	}

	public new virtual Task OpenAsync()
	{
		return Task.Factory.FromAsync(((ICommunicationObject)this).BeginOpen((AsyncCallback)null, (object)null), ((ICommunicationObject)this).EndOpen);
	}

	private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == EndpointConfiguration.SignServicePort)
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
		if (endpointConfiguration == EndpointConfiguration.SignServicePort)
		{
			return new EndpointAddress("http://ews.taxservice.am/taxsystem-fe-ws/taxpayer/signService");
		}
		throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'.");
	}

	private static Binding GetDefaultBinding()
	{
		return GetBindingForEndpoint(EndpointConfiguration.SignServicePort);
	}

	private static EndpointAddress GetDefaultEndpointAddress()
	{
		return GetEndpointAddress(EndpointConfiguration.SignServicePort);
	}
}
