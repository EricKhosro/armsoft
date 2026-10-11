using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[ServiceContract(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", ConfigurationName = "ServiceReference1.SignServicePortType")]
internal interface SignServicePortType
{
	[OperationContract(Action = "login", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	signResponse1 sign(signRequest1 request);

	[OperationContract(Action = "login", ReplyAction = "*")]
	Task<signResponse1> signAsync(signRequest1 request);
}
