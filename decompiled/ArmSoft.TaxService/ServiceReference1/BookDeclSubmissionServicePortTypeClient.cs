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
internal class BookDeclSubmissionServicePortTypeClient : ClientBase<BookDeclSubmissionServicePortType>, BookDeclSubmissionServicePortType
{
	public enum EndpointConfiguration
	{
		BookDeclSubmissionServicePort
	}

	public BookDeclSubmissionServicePortTypeClient()
		: base(GetDefaultBinding(), GetDefaultEndpointAddress())
	{
		Endpoint.Name = EndpointConfiguration.BookDeclSubmissionServicePort.ToString();
	}

	public BookDeclSubmissionServicePortTypeClient(EndpointConfiguration endpointConfiguration)
		: base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public BookDeclSubmissionServicePortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public BookDeclSubmissionServicePortTypeClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), remoteAddress)
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public BookDeclSubmissionServicePortTypeClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	registerSignedBookDeclResponse1 BookDeclSubmissionServicePortType.registerSignedBookDecl(registerSignedBookDeclRequest1 request)
	{
		return Channel.registerSignedBookDecl(request);
	}

	public RegisterSignedBookDeclResponse registerSignedBookDecl(RegisterSignedBookDeclRequest RegisterSignedBookDeclRequest)
	{
		registerSignedBookDeclRequest1 registerSignedBookDeclRequest2 = new registerSignedBookDeclRequest1();
		registerSignedBookDeclRequest2.RegisterSignedBookDeclRequest = RegisterSignedBookDeclRequest;
		return ((BookDeclSubmissionServicePortType)this).registerSignedBookDecl(registerSignedBookDeclRequest2).RegisterSignedBookDeclResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<registerSignedBookDeclResponse1> BookDeclSubmissionServicePortType.registerSignedBookDeclAsync(registerSignedBookDeclRequest1 request)
	{
		return Channel.registerSignedBookDeclAsync(request);
	}

	public Task<registerSignedBookDeclResponse1> registerSignedBookDeclAsync(RegisterSignedBookDeclRequest RegisterSignedBookDeclRequest)
	{
		registerSignedBookDeclRequest1 registerSignedBookDeclRequest2 = new registerSignedBookDeclRequest1();
		registerSignedBookDeclRequest2.RegisterSignedBookDeclRequest = RegisterSignedBookDeclRequest;
		return ((BookDeclSubmissionServicePortType)this).registerSignedBookDeclAsync(registerSignedBookDeclRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	registerInvalidationDeclResponse1 BookDeclSubmissionServicePortType.registerInvalidationDecl(registerInvalidationDeclRequest1 request)
	{
		return Channel.registerInvalidationDecl(request);
	}

	public RegisterInvalidationDeclResponse registerInvalidationDecl(RegisterInvalidationDeclRequest RegisterInvalidationDeclRequest)
	{
		registerInvalidationDeclRequest1 registerInvalidationDeclRequest2 = new registerInvalidationDeclRequest1();
		registerInvalidationDeclRequest2.RegisterInvalidationDeclRequest = RegisterInvalidationDeclRequest;
		return ((BookDeclSubmissionServicePortType)this).registerInvalidationDecl(registerInvalidationDeclRequest2).RegisterInvalidationDeclResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<registerInvalidationDeclResponse1> BookDeclSubmissionServicePortType.registerInvalidationDeclAsync(registerInvalidationDeclRequest1 request)
	{
		return Channel.registerInvalidationDeclAsync(request);
	}

	public Task<registerInvalidationDeclResponse1> registerInvalidationDeclAsync(RegisterInvalidationDeclRequest RegisterInvalidationDeclRequest)
	{
		registerInvalidationDeclRequest1 registerInvalidationDeclRequest2 = new registerInvalidationDeclRequest1();
		registerInvalidationDeclRequest2.RegisterInvalidationDeclRequest = RegisterInvalidationDeclRequest;
		return ((BookDeclSubmissionServicePortType)this).registerInvalidationDeclAsync(registerInvalidationDeclRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	registerOverdueDeclResponse1 BookDeclSubmissionServicePortType.registerOverdueDecl(registerOverdueDeclRequest1 request)
	{
		return Channel.registerOverdueDecl(request);
	}

	public RegisterOverdueDeclResponse registerOverdueDecl(RegisterOverdueDeclRequest RegisterOverdueDeclRequest)
	{
		registerOverdueDeclRequest1 registerOverdueDeclRequest2 = new registerOverdueDeclRequest1();
		registerOverdueDeclRequest2.RegisterOverdueDeclRequest = RegisterOverdueDeclRequest;
		return ((BookDeclSubmissionServicePortType)this).registerOverdueDecl(registerOverdueDeclRequest2).RegisterOverdueDeclResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<registerOverdueDeclResponse1> BookDeclSubmissionServicePortType.registerOverdueDeclAsync(registerOverdueDeclRequest1 request)
	{
		return Channel.registerOverdueDeclAsync(request);
	}

	public Task<registerOverdueDeclResponse1> registerOverdueDeclAsync(RegisterOverdueDeclRequest RegisterOverdueDeclRequest)
	{
		registerOverdueDeclRequest1 registerOverdueDeclRequest2 = new registerOverdueDeclRequest1();
		registerOverdueDeclRequest2.RegisterOverdueDeclRequest = RegisterOverdueDeclRequest;
		return ((BookDeclSubmissionServicePortType)this).registerOverdueDeclAsync(registerOverdueDeclRequest2);
	}

	public new virtual Task OpenAsync()
	{
		return Task.Factory.FromAsync(((ICommunicationObject)this).BeginOpen((AsyncCallback)null, (object)null), ((ICommunicationObject)this).EndOpen);
	}

	private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == EndpointConfiguration.BookDeclSubmissionServicePort)
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
		if (endpointConfiguration == EndpointConfiguration.BookDeclSubmissionServicePort)
		{
			return new EndpointAddress("http://ews.taxservice.am/taxsystem-fe-ws/taxpayer/bookDeclSubmissionService");
		}
		throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'.");
	}

	private static Binding GetDefaultBinding()
	{
		return GetBindingForEndpoint(EndpointConfiguration.BookDeclSubmissionServicePort);
	}

	private static EndpointAddress GetDefaultEndpointAddress()
	{
		return GetEndpointAddress(EndpointConfiguration.BookDeclSubmissionServicePort);
	}
}
