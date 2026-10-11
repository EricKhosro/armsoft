using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[ServiceContract(Namespace = "http://www.taxservice.am/tp3/invoice/definitions", ConfigurationName = "ServiceReference1.EBookUtilsServicePortType")]
internal interface EBookUtilsServicePortType
{
	[OperationContract(Action = "getTpBookList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getTpBookListResponse1 getTpBookList(getTpBookListRequest1 request);

	[OperationContract(Action = "getTpBookList", ReplyAction = "*")]
	Task<getTpBookListResponse1> getTpBookListAsync(getTpBookListRequest1 request);

	[OperationContract(Action = "getObjectByOin", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getObjectByOinResponse1 getObjectByOin(getObjectByOinRequest1 request);

	[OperationContract(Action = "getObjectByOin", ReplyAction = "*")]
	Task<getObjectByOinResponse1> getObjectByOinAsync(getObjectByOinRequest1 request);

	[OperationContract(Action = "getObjectList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getObjectListResponse1 getObjectList(getObjectListRequest1 request);

	[OperationContract(Action = "getObjectList", ReplyAction = "*")]
	Task<getObjectListResponse1> getObjectListAsync(getObjectListRequest1 request);

	[OperationContract(Action = "getActiveObjectList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getActiveObjectListResponse1 getActiveObjectList(getActiveObjectListRequest1 request);

	[OperationContract(Action = "getActiveObjectList", ReplyAction = "*")]
	Task<getActiveObjectListResponse1> getActiveObjectListAsync(getActiveObjectListRequest1 request);

	[OperationContract(Action = "getMarzList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getMarzListResponse1 getMarzList(getMarzListRequest1 request);

	[OperationContract(Action = "getMarzList", ReplyAction = "*")]
	Task<getMarzListResponse1> getMarzListAsync(getMarzListRequest1 request);

	[OperationContract(Action = "getRegionListByMarz", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getRegionListByMarzResponse1 getRegionListByMarz(getRegionListByMarzRequest1 request);

	[OperationContract(Action = "getRegionListByMarz", ReplyAction = "*")]
	Task<getRegionListByMarzResponse1> getRegionListByMarzAsync(getRegionListByMarzRequest1 request);

	[OperationContract(Action = "getTownList", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	[ServiceKnownType(typeof(GenericAuthenticatedRequest))]
	[ServiceKnownType(typeof(GenericResponse))]
	getTownListResponse1 getTownList(getTownListRequest1 request);

	[OperationContract(Action = "getTownList", ReplyAction = "*")]
	Task<getTownListResponse1> getTownListAsync(getTownListRequest1 request);
}
