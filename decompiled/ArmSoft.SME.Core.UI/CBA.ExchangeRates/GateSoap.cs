using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CBA.ExchangeRates;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
[ServiceContract(Namespace = "http://www.cba.am/", ConfigurationName = "CBA.ExchangeRates.GateSoap")]
public interface GateSoap
{
	[OperationContract(Action = "http://www.cba.am/ISOCodesDetailed", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	ISOCodesDetailedResponse ISOCodesDetailed(ISOCodesDetailedRequest request);

	[OperationContract(Action = "http://www.cba.am/ISOCodesDetailed", ReplyAction = "*")]
	Task<ISOCodesDetailedResponse> ISOCodesDetailedAsync(ISOCodesDetailedRequest request);

	[OperationContract(Action = "http://www.cba.am/ISOCodes", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	ISOCodesResponse ISOCodes(ISOCodesRequest request);

	[OperationContract(Action = "http://www.cba.am/ISOCodes", ReplyAction = "*")]
	Task<ISOCodesResponse> ISOCodesAsync(ISOCodesRequest request);

	[OperationContract(Action = "http://www.cba.am/ExchangeRatesLatestByISO", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	ExchangeRatesLatestByISOResponse ExchangeRatesLatestByISO(ExchangeRatesLatestByISORequest request);

	[OperationContract(Action = "http://www.cba.am/ExchangeRatesLatestByISO", ReplyAction = "*")]
	Task<ExchangeRatesLatestByISOResponse> ExchangeRatesLatestByISOAsync(ExchangeRatesLatestByISORequest request);

	[OperationContract(Action = "http://www.cba.am/ExchangeRatesByDateByISO", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	ExchangeRatesByDateByISOResponse ExchangeRatesByDateByISO(ExchangeRatesByDateByISORequest request);

	[OperationContract(Action = "http://www.cba.am/ExchangeRatesByDateByISO", ReplyAction = "*")]
	Task<ExchangeRatesByDateByISOResponse> ExchangeRatesByDateByISOAsync(ExchangeRatesByDateByISORequest request);

	[OperationContract(Action = "http://www.cba.am/ExchangeRatesByDate", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	ExchangeRatesByDateResponse ExchangeRatesByDate(ExchangeRatesByDateRequest request);

	[OperationContract(Action = "http://www.cba.am/ExchangeRatesByDate", ReplyAction = "*")]
	Task<ExchangeRatesByDateResponse> ExchangeRatesByDateAsync(ExchangeRatesByDateRequest request);

	[OperationContract(Action = "http://www.cba.am/ExchangeRatesLatest", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	ExchangeRatesLatestResponse ExchangeRatesLatest(ExchangeRatesLatestRequest request);

	[OperationContract(Action = "http://www.cba.am/ExchangeRatesLatest", ReplyAction = "*")]
	Task<ExchangeRatesLatestResponse> ExchangeRatesLatestAsync(ExchangeRatesLatestRequest request);

	[OperationContract(Action = "http://www.cba.am/ExchangeRatesByDateRangeByISO", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	ExchangeRatesByDateRangeByISOResponse ExchangeRatesByDateRangeByISO(ExchangeRatesByDateRangeByISORequest request);

	[OperationContract(Action = "http://www.cba.am/ExchangeRatesByDateRangeByISO", ReplyAction = "*")]
	Task<ExchangeRatesByDateRangeByISOResponse> ExchangeRatesByDateRangeByISOAsync(ExchangeRatesByDateRangeByISORequest request);
}
