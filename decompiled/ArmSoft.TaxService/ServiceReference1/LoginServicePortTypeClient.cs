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
internal class LoginServicePortTypeClient : ClientBase<LoginServicePortType>, LoginServicePortType
{
	public enum EndpointConfiguration
	{
		LoginServicePort
	}

	public LoginServicePortTypeClient()
		: base(GetDefaultBinding(), GetDefaultEndpointAddress())
	{
		Endpoint.Name = EndpointConfiguration.LoginServicePort.ToString();
	}

	public LoginServicePortTypeClient(EndpointConfiguration endpointConfiguration)
		: base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public LoginServicePortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public LoginServicePortTypeClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), remoteAddress)
	{
		Endpoint.Name = endpointConfiguration.ToString();
	}

	public LoginServicePortTypeClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	loginResponse1 LoginServicePortType.login(loginRequest1 request)
	{
		return Channel.login(request);
	}

	public LoginResponse login(LoginRequest LoginRequest)
	{
		loginRequest1 loginRequest2 = new loginRequest1();
		loginRequest2.LoginRequest = LoginRequest;
		return ((LoginServicePortType)this).login(loginRequest2).LoginResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<loginResponse1> LoginServicePortType.loginAsync(loginRequest1 request)
	{
		return Channel.loginAsync(request);
	}

	public Task<loginResponse1> loginAsync(LoginRequest LoginRequest)
	{
		loginRequest1 loginRequest2 = new loginRequest1();
		loginRequest2.LoginRequest = LoginRequest;
		return ((LoginServicePortType)this).loginAsync(loginRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	loginWebResponse1 LoginServicePortType.loginWeb(loginWebRequest1 request)
	{
		return Channel.loginWeb(request);
	}

	public LoginWebResponse loginWeb(LoginWebRequest LoginWebRequest)
	{
		loginWebRequest1 loginWebRequest2 = new loginWebRequest1();
		loginWebRequest2.LoginWebRequest = LoginWebRequest;
		return ((LoginServicePortType)this).loginWeb(loginWebRequest2).LoginWebResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<loginWebResponse1> LoginServicePortType.loginWebAsync(loginWebRequest1 request)
	{
		return Channel.loginWebAsync(request);
	}

	public Task<loginWebResponse1> loginWebAsync(LoginWebRequest LoginWebRequest)
	{
		loginWebRequest1 loginWebRequest2 = new loginWebRequest1();
		loginWebRequest2.LoginWebRequest = LoginWebRequest;
		return ((LoginServicePortType)this).loginWebAsync(loginWebRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	sendMobileLoginResponse1 LoginServicePortType.sendMobileLogin(sendMobileLoginRequest1 request)
	{
		return Channel.sendMobileLogin(request);
	}

	public SendMobileLoginResponse sendMobileLogin(SendMobileLoginRequest SendMobileLoginRequest)
	{
		sendMobileLoginRequest1 sendMobileLoginRequest2 = new sendMobileLoginRequest1();
		sendMobileLoginRequest2.SendMobileLoginRequest = SendMobileLoginRequest;
		return ((LoginServicePortType)this).sendMobileLogin(sendMobileLoginRequest2).SendMobileLoginResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<sendMobileLoginResponse1> LoginServicePortType.sendMobileLoginAsync(sendMobileLoginRequest1 request)
	{
		return Channel.sendMobileLoginAsync(request);
	}

	public Task<sendMobileLoginResponse1> sendMobileLoginAsync(SendMobileLoginRequest SendMobileLoginRequest)
	{
		sendMobileLoginRequest1 sendMobileLoginRequest2 = new sendMobileLoginRequest1();
		sendMobileLoginRequest2.SendMobileLoginRequest = SendMobileLoginRequest;
		return ((LoginServicePortType)this).sendMobileLoginAsync(sendMobileLoginRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	checkMobileLoginResponse1 LoginServicePortType.checkMobileLogin(checkMobileLoginRequest1 request)
	{
		return Channel.checkMobileLogin(request);
	}

	public CheckMobileLoginResponse checkMobileLogin(CheckMobileLoginRequest CheckMobileLoginRequest)
	{
		checkMobileLoginRequest1 checkMobileLoginRequest2 = new checkMobileLoginRequest1();
		checkMobileLoginRequest2.CheckMobileLoginRequest = CheckMobileLoginRequest;
		return ((LoginServicePortType)this).checkMobileLogin(checkMobileLoginRequest2).CheckMobileLoginResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<checkMobileLoginResponse1> LoginServicePortType.checkMobileLoginAsync(checkMobileLoginRequest1 request)
	{
		return Channel.checkMobileLoginAsync(request);
	}

	public Task<checkMobileLoginResponse1> checkMobileLoginAsync(CheckMobileLoginRequest CheckMobileLoginRequest)
	{
		checkMobileLoginRequest1 checkMobileLoginRequest2 = new checkMobileLoginRequest1();
		checkMobileLoginRequest2.CheckMobileLoginRequest = CheckMobileLoginRequest;
		return ((LoginServicePortType)this).checkMobileLoginAsync(checkMobileLoginRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	stopPendingResponse1 LoginServicePortType.stopPending(stopPendingRequest1 request)
	{
		return Channel.stopPending(request);
	}

	public StopPendingResponse stopPending(StopPendingRequest StopPendingRequest)
	{
		stopPendingRequest1 stopPendingRequest2 = new stopPendingRequest1();
		stopPendingRequest2.StopPendingRequest = StopPendingRequest;
		return ((LoginServicePortType)this).stopPending(stopPendingRequest2).StopPendingResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<stopPendingResponse1> LoginServicePortType.stopPendingAsync(stopPendingRequest1 request)
	{
		return Channel.stopPendingAsync(request);
	}

	public Task<stopPendingResponse1> stopPendingAsync(StopPendingRequest StopPendingRequest)
	{
		stopPendingRequest1 stopPendingRequest2 = new stopPendingRequest1();
		stopPendingRequest2.StopPendingRequest = StopPendingRequest;
		return ((LoginServicePortType)this).stopPendingAsync(stopPendingRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	checkLoginStatusResponse1 LoginServicePortType.checkLoginStatus(checkLoginStatusRequest1 request)
	{
		return Channel.checkLoginStatus(request);
	}

	public CheckLoginStatusResponse checkLoginStatus(CheckLoginStatusRequest CheckLoginStatusRequest)
	{
		checkLoginStatusRequest1 checkLoginStatusRequest2 = new checkLoginStatusRequest1();
		checkLoginStatusRequest2.CheckLoginStatusRequest = CheckLoginStatusRequest;
		return ((LoginServicePortType)this).checkLoginStatus(checkLoginStatusRequest2).CheckLoginStatusResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<checkLoginStatusResponse1> LoginServicePortType.checkLoginStatusAsync(checkLoginStatusRequest1 request)
	{
		return Channel.checkLoginStatusAsync(request);
	}

	public Task<checkLoginStatusResponse1> checkLoginStatusAsync(CheckLoginStatusRequest CheckLoginStatusRequest)
	{
		checkLoginStatusRequest1 checkLoginStatusRequest2 = new checkLoginStatusRequest1();
		checkLoginStatusRequest2.CheckLoginStatusRequest = CheckLoginStatusRequest;
		return ((LoginServicePortType)this).checkLoginStatusAsync(checkLoginStatusRequest2);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	logoutResponse1 LoginServicePortType.logout(logoutRequest1 request)
	{
		return Channel.logout(request);
	}

	public LogoutResponse logout(LogoutRequest LogoutRequest)
	{
		logoutRequest1 logoutRequest2 = new logoutRequest1();
		logoutRequest2.LogoutRequest = LogoutRequest;
		return ((LoginServicePortType)this).logout(logoutRequest2).LogoutResponse;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<logoutResponse1> LoginServicePortType.logoutAsync(logoutRequest1 request)
	{
		return Channel.logoutAsync(request);
	}

	public Task<logoutResponse1> logoutAsync(LogoutRequest LogoutRequest)
	{
		logoutRequest1 logoutRequest2 = new logoutRequest1();
		logoutRequest2.LogoutRequest = LogoutRequest;
		return ((LoginServicePortType)this).logoutAsync(logoutRequest2);
	}

	public new virtual Task OpenAsync()
	{
		return Task.Factory.FromAsync(((ICommunicationObject)this).BeginOpen((AsyncCallback)null, (object)null), ((ICommunicationObject)this).EndOpen);
	}

	private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == EndpointConfiguration.LoginServicePort)
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
		if (endpointConfiguration == EndpointConfiguration.LoginServicePort)
		{
			return new EndpointAddress("http://ews.taxservice.am/taxsystem-fe-ws/taxpayer/loginService");
		}
		throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'.");
	}

	private static Binding GetDefaultBinding()
	{
		return GetBindingForEndpoint(EndpointConfiguration.LoginServicePort);
	}

	private static EndpointAddress GetDefaultEndpointAddress()
	{
		return GetEndpointAddress(EndpointConfiguration.LoginServicePort);
	}
}
