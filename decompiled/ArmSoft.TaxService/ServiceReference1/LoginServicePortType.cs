using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[ServiceContract(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", ConfigurationName = "ServiceReference1.LoginServicePortType")]
internal interface LoginServicePortType
{
	[OperationContract(Action = "login", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	loginResponse1 login(loginRequest1 request);

	[OperationContract(Action = "login", ReplyAction = "*")]
	Task<loginResponse1> loginAsync(loginRequest1 request);

	[OperationContract(Action = "loginWeb", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	loginWebResponse1 loginWeb(loginWebRequest1 request);

	[OperationContract(Action = "loginWeb", ReplyAction = "*")]
	Task<loginWebResponse1> loginWebAsync(loginWebRequest1 request);

	[OperationContract(Action = "sendMobileLogin", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	sendMobileLoginResponse1 sendMobileLogin(sendMobileLoginRequest1 request);

	[OperationContract(Action = "sendMobileLogin", ReplyAction = "*")]
	Task<sendMobileLoginResponse1> sendMobileLoginAsync(sendMobileLoginRequest1 request);

	[OperationContract(Action = "checkMobileLogin", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	checkMobileLoginResponse1 checkMobileLogin(checkMobileLoginRequest1 request);

	[OperationContract(Action = "checkMobileLogin", ReplyAction = "*")]
	Task<checkMobileLoginResponse1> checkMobileLoginAsync(checkMobileLoginRequest1 request);

	[OperationContract(Action = "stopPending", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	stopPendingResponse1 stopPending(stopPendingRequest1 request);

	[OperationContract(Action = "stopPending", ReplyAction = "*")]
	Task<stopPendingResponse1> stopPendingAsync(stopPendingRequest1 request);

	[OperationContract(Action = "checkLoginStatus", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	checkLoginStatusResponse1 checkLoginStatus(checkLoginStatusRequest1 request);

	[OperationContract(Action = "checkLoginStatus", ReplyAction = "*")]
	Task<checkLoginStatusResponse1> checkLoginStatusAsync(checkLoginStatusRequest1 request);

	[OperationContract(Action = "logout", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	logoutResponse1 logout(logoutRequest1 request);

	[OperationContract(Action = "logout", ReplyAction = "*")]
	Task<logoutResponse1> logoutAsync(logoutRequest1 request);
}
