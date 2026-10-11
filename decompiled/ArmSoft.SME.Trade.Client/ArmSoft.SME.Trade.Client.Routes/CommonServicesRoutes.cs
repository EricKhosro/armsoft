using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.CustomTypes;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Models.Other;

namespace ArmSoft.SME.Trade.Client.Routes;

public class CommonServicesRoutes
{
	private readonly TradeClient _client;

	internal CommonServicesRoutes(TradeClient client)
	{
		_client = client;
	}

	public decimal GetPartSummaByDocSumma(string docCurrCode, decimal docSumma, decimal docSummaNCC, DateOnly docDate, string contractCurrCode)
	{
		return AsyncHelper.RunSync(async () => await GetPartSummaByDocSummaAsync(docCurrCode, docSumma, docSummaNCC, docDate, contractCurrCode));
	}

	public async Task<decimal> GetPartSummaByDocSummaAsync(string docCurrCode, decimal docSumma, decimal docSummaNCC, DateOnly docDate, string contractCurrCode, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("docCurrCode", docCurrCode);
		urlQuery.Add("docSumma", docSumma);
		urlQuery.Add("docSummaNCC", docSummaNCC);
		urlQuery.Add("docDate", docDate);
		urlQuery.Add("contractCurrCode", contractCurrCode);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/PartSummaByDocSumma", urlQuery);
		return await _client.ExecuteAsync<decimal>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public decimal GetMTSaleSumByPartnerAndContract(CommonConst.SummaTypes summaType, DateOnly startDate, DateOnly endDate, int partID, string contract, Guid docISN)
	{
		return AsyncHelper.RunSync(async () => await GetMTSaleSumByPartnerAndContractAsync(summaType, startDate, endDate, partID, contract, docISN));
	}

	public async Task<decimal> GetMTSaleSumByPartnerAndContractAsync(CommonConst.SummaTypes summaType, DateOnly startDate, DateOnly endDate, int partID, string contract, Guid docISN, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("summaType", (int)summaType);
		urlQuery.Add("startDate", startDate);
		urlQuery.Add("endDate", endDate);
		urlQuery.Add("partID", partID);
		urlQuery.Add("contract", contract);
		urlQuery.Add("docISN", docISN);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/MTSaleSumByPartnerAndContract", urlQuery);
		return await _client.ExecuteAsync<decimal>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public decimal GetBonusRemByPartnerAndContract(int intPartID, string contractCode)
	{
		return AsyncHelper.RunSync(async () => await GetBonusRemByPartnerAndContractAsync(intPartID, contractCode));
	}

	public Task<decimal> GetBonusRemByPartnerAndContractAsync(int intPartID, string contractCode, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("intPartID", intPartID);
		urlQuery.Add("contractCode", contractCode);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/BonusRemByPartnerAndContract", urlQuery);
		return _client.ExecuteAsync<decimal>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public SummCurrSummNccPair GetCurrentPartnerRemByContract(int intPartID, string contractCode, string currCode, Guid? isn = null)
	{
		return AsyncHelper.RunSync(async () => await GetCurrentPartnerRemByContractAsync(intPartID, contractCode, currCode, isn));
	}

	public Task<SummCurrSummNccPair> GetCurrentPartnerRemByContractAsync(int intPartID, string contractCode, string currCode, Guid? isn = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("intPartID", intPartID);
		urlQuery.Add("contractCode", contractCode);
		urlQuery.Add("currCode", currCode);
		urlQuery.Add("isn", isn);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/CurrentPartnerRemByContract", urlQuery);
		return _client.ExecuteAsync<SummCurrSummNccPair>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public bool CheckEntriesExistenceByContract(string contractCode, int intPartID)
	{
		return AsyncHelper.RunSync(async () => await CheckEntriesExistenceByContractAsync(contractCode, intPartID));
	}

	public Task<bool> CheckEntriesExistenceByContractAsync(string contractCode, int intPartID, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("contractCode", contractCode);
		urlQuery.Add("intPartID", intPartID);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/CheckEntriesExistenceByContract", urlQuery);
		return _client.ExecuteAsync<bool>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public ExchangeRate GetExchangeRates(string currCode, DateOnly date)
	{
		return AsyncHelper.RunSync(async () => await GetExchangeRatesAsync(currCode, date));
	}

	public Task<ExchangeRate> GetExchangeRatesAsync(string currCode, DateOnly date, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("currCode", currCode);
		urlQuery.Add("date", date);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/ExchangeRate", urlQuery);
		return _client.ExecuteAsync<ExchangeRate>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public bool IsStorageAvailable(string code, int userID = -1, bool isViewAccess = false)
	{
		return AsyncHelper.RunSync(async () => await IsStorageAvailableAsync(code, userID, isViewAccess));
	}

	public Task<bool> IsStorageAvailableAsync(string code, int userID = -1, bool isViewAccess = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("code", code);
		urlQuery.Add("userID", userID);
		urlQuery.Add("isViewAccess", isViewAccess);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/IsStorageAvailable", urlQuery);
		return _client.ExecuteAsync<bool>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public bool IsCashDeskAvailable(string code, int userID = -1)
	{
		return AsyncHelper.RunSync(async () => await IsCashDeskAvailableAsync(code, userID));
	}

	public Task<bool> IsCashDeskAvailableAsync(string code, int userID = -1, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("code", code);
		urlQuery.Add("userID", userID);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/IsCashDeskAvailable", urlQuery);
		return _client.ExecuteAsync<bool>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public bool ExistsAvailableCashDesk()
	{
		return AsyncHelper.RunSync(async () => await ExistsAvailableCashDeskAsync());
	}

	public Task<bool> ExistsAvailableCashDeskAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery query = new UrlQuery();
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/ExistsAvailableCashDesk", query);
		return _client.ExecuteAsync<bool>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public bool ExistsAvailableStorage()
	{
		return AsyncHelper.RunSync(async () => await ExistsAvailableStorageAsync());
	}

	public Task<bool> ExistsAvailableStorageAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery query = new UrlQuery();
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/ExistsAvailableStorage", query);
		return _client.ExecuteAsync<bool>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public bool IsStorageRetail(string code)
	{
		return AsyncHelper.RunSync(async () => await IsStorageRetailAsync(code));
	}

	public Task<bool> IsStorageRetailAsync(string code, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("code", code);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/IsStorageRetail", urlQuery);
		return _client.ExecuteAsync<bool>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public bool IsNegativExpAllow(string code)
	{
		return AsyncHelper.RunSync(async () => await IsNegativExpAllowAsync(code));
	}

	public Task<bool> IsNegativExpAllowAsync(string storage, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("storage", storage);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/IsNegativExpAllow", urlQuery);
		return _client.ExecuteAsync<bool>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public decimal GetPriceForGivenPriceListAndVATType(string priceListType, string vatType, int mtId, string unit, DateOnly date, string currency)
	{
		return AsyncHelper.RunSync(async () => await GetPriceForGivenPriceListAndVATTypeAsync(priceListType, vatType, mtId, unit, date, currency));
	}

	public Task<decimal> GetPriceForGivenPriceListAndVATTypeAsync(string priceListType, string vatType, int mtId, string unit, DateOnly date, string currency, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("priceListType", priceListType);
		urlQuery.Add("vatType", vatType);
		urlQuery.Add("mtId", mtId);
		urlQuery.Add("unit", unit);
		urlQuery.Add("date", date);
		urlQuery.Add("currency", currency);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/PriceForGivenPriceListAndVATType", urlQuery);
		return _client.ExecuteAsync<decimal>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public string GetMTQntUnitByMTIDPriceTypeAndDocDate(int servID, string code, DateOnly docDate)
	{
		return AsyncHelper.RunSync(async () => await GetMTQntUnitByMTIDPriceTypeAndDocDateAsync(servID, code, docDate));
	}

	public Task<string> GetMTQntUnitByMTIDPriceTypeAndDocDateAsync(int servID, string code, DateOnly docDate, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("servID", servID);
		urlQuery.Add("code", code);
		urlQuery.Add("docDate", docDate);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/MTQntUnitByMTIDPriceTypeAndDocDate", urlQuery);
		return _client.ExecuteAsync<string>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public string GetOpenCashierShiftNumberByCashDesk(string cashDaskCode)
	{
		return AsyncHelper.RunSync(async () => await GetOpenCashierShiftNumberByCashDeskAsync(cashDaskCode));
	}

	public Task<string> GetOpenCashierShiftNumberByCashDeskAsync(string cashDaskCode, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("cashDaskCode", cashDaskCode);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetOpenCashierShiftNumberByCashDesk", urlQuery);
		return _client.ExecuteAsync<string>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public bool ExistPostponedSaleDocsWithCashierShift(string cashDaskCode, string shiftNumber)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("cashDaskCode", cashDaskCode);
		urlQuery.Add("shiftNumber", shiftNumber);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/ExistPostponedSaleDocsWithCashierShift", urlQuery);
		return _client.Execute<bool>(HttpMethod.Get, stringBuilder.ToString());
	}

	public PricesDataProvider.DataRow GetPrice(string priceType, int mtID, string unit, DateOnly date, string curr = "")
	{
		return AsyncHelper.RunSync(async () => await GetPriceAsync(priceType, mtID, unit, date, curr));
	}

	public Task<PricesDataProvider.DataRow> GetPriceAsync(string priceType, int mtID, string unit, DateOnly date, string curr = "", CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("priceType", priceType);
		urlQuery.Add("mtID", mtID);
		urlQuery.Add("unit", unit);
		urlQuery.Add("date", date);
		urlQuery.Add("curr", curr);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/Price", urlQuery);
		return _client.ExecuteAsync<PricesDataProvider.DataRow>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public DateTime GetServerDate()
	{
		return AsyncHelper.RunSync(async () => await GetServerDateAsync());
	}

	public Task<DateTime> GetServerDateAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/ServerDate");
		return _client.ExecuteAsync<DateTime>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public bool CheckEntriesExistenceByBonusContract(string contract, int intPartID)
	{
		return AsyncHelper.RunSync(async () => await CheckEntriesExistenceByBonusContractAsync(contract, intPartID));
	}

	public Task<bool> CheckEntriesExistenceByBonusContractAsync(string contract, int intPartID, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("contract", contract);
		urlQuery.Add("intPartID", intPartID);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/CheckEntriesExistenceByBonusContract", urlQuery);
		return _client.ExecuteAsync<bool>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public Dictionary<string, MTPrice> GetMTPricesByTypesAndBaseUnit(int mtID, List<string> priceTypes = null)
	{
		return AsyncHelper.RunSync(async () => await GetMTPricesByTypesAndBaseUnitAsync(mtID, priceTypes));
	}

	public Task<Dictionary<string, MTPrice>> GetMTPricesByTypesAndBaseUnitAsync(int mtID, List<string> priceTypes = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtID", mtID);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetMTPricesByTypesAndBaseUnit", urlQuery);
		return _client.ExecuteAsync<Dictionary<string, MTPrice>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, priceTypes);
	}

	public Dictionary<string, MTPrice> GetMTPricesByTypesAndAltUnit(int mtID, List<string> priceTypes = null)
	{
		return AsyncHelper.RunSync(async () => await GetMTPricesByTypesAndAltUnitAsync(mtID, priceTypes));
	}

	public Task<Dictionary<string, MTPrice>> GetMTPricesByTypesAndAltUnitAsync(int mtID, List<string> priceTypes = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtID", mtID);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetMTPricesByTypesAndAltUnit", urlQuery);
		return _client.ExecuteAsync<Dictionary<string, MTPrice>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, priceTypes);
	}

	public SummCurrSummNccPair GetAccRem(CommonConst.AccountingsTypes accType, string accCode, int intPartID, string currCode, DateOnly dRemDate)
	{
		return AsyncHelper.RunSync(async () => await GetAccRemAsync(accType, accCode, intPartID, currCode, dRemDate));
	}

	public Task<SummCurrSummNccPair> GetAccRemAsync(CommonConst.AccountingsTypes accType, string accCode, int intPartID, string currCode, DateOnly dRemDate, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("accType", (int)accType);
		urlQuery.Add("accCode", accCode);
		urlQuery.Add("intPartID", intPartID);
		urlQuery.Add("currCode", currCode);
		urlQuery.Add("dRemDate", dRemDate);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/AccRem", urlQuery);
		return _client.ExecuteAsync<SummCurrSummNccPair>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public TurnSumms GetAccTurn(CommonConst.AccountingsTypes accType, string accCode, int intPartID, string currCode, DateOnly startDate, DateOnly endDate)
	{
		return AsyncHelper.RunSync(async () => await GetAccTurnAsync(accType, accCode, intPartID, currCode, startDate, endDate));
	}

	public Task<TurnSumms> GetAccTurnAsync(CommonConst.AccountingsTypes accType, string accCode, int intPartID, string currCode, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("accType", (int)accType);
		urlQuery.Add("accCode", accCode);
		urlQuery.Add("intPartID", intPartID);
		urlQuery.Add("currCode", currCode);
		urlQuery.Add("startDate", startDate);
		urlQuery.Add("endDate", endDate);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetAccTurn", urlQuery);
		return _client.ExecuteAsync<TurnSumms>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public SummCurrSummNccPair GetPartRemByPartIDByDate(CommonConst.AccountingsTypes accType, DateOnly dRemDate, int intPartID, string currCode)
	{
		return AsyncHelper.RunSync(async () => await GetPartRemByPartIDByDateAsync(accType, dRemDate, intPartID, currCode));
	}

	public Task<SummCurrSummNccPair> GetPartRemByPartIDByDateAsync(CommonConst.AccountingsTypes accType, DateOnly dRemDate, int intPartID, string currCode, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("accType", (int)accType);
		urlQuery.Add("intPartID", intPartID);
		urlQuery.Add("currCode", currCode);
		urlQuery.Add("dRemDate", dRemDate);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/PartRemByPartIDByDate", urlQuery);
		return _client.ExecuteAsync<SummCurrSummNccPair>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<PartCashBankAccOperJournalShortRow> GetPartnerOperJournal(DateOnly periodStart, DateOnly periodEnd, int partId, string contractCode)
	{
		return AsyncHelper.RunSync(async () => await GetPartnerOperJournalAsync(periodStart, periodEnd, partId, contractCode));
	}

	public Task<List<PartCashBankAccOperJournalShortRow>> GetPartnerOperJournalAsync(DateOnly periodStart, DateOnly periodEnd, int partId, string contractCode, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("periodStart", periodStart);
		urlQuery.Add("periodEnd", periodEnd);
		urlQuery.Add("partId", partId);
		urlQuery.Add("contractCode", contractCode);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/PartnerOperJournal", urlQuery);
		return _client.ExecuteAsync<List<PartCashBankAccOperJournalShortRow>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public string GetDocumentDescription(Guid isn, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode mode)
	{
		return AsyncHelper.RunSync(async () => await GetDocumentDescriptionAsync(isn, mode));
	}

	public string GetDocumentAdditionalCaptionForReports(Guid isn)
	{
		return AsyncHelper.RunSync(async () => await GetDocumentAdditionalCaptionForReportsAsync(isn));
	}

	public Task<string> GetDocumentDescriptionAsync(Guid isn, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode mode, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("isn", isn);
		urlQuery.Add("mode", (byte)mode);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/DocumentDescription", urlQuery);
		return _client.ExecuteAsync<string>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public Task<string> GetDocumentAdditionalCaptionForReportsAsync(Guid isn, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("isn", isn);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetDocumentAdditionalCaptionForReports", urlQuery);
		return _client.ExecuteAsync<string>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<PartyRow> GetMTDocRowsInMTHI(Guid isn)
	{
		return AsyncHelper.RunSync(async () => await GetMTDocRowsInMTHIAsync(isn));
	}

	public Task<List<PartyRow>> GetMTDocRowsInMTHIAsync(Guid isn, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("isn", isn);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/MTDocRowsInMTHI", urlQuery);
		return _client.ExecuteAsync<List<PartyRow>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public MTPriceForUnit GetMTIncomeImportPrice(int mtId, string unitCode, Guid isn, string currCode, int partId, DateOnly date)
	{
		return AsyncHelper.RunSync(async () => await GetMTIncomeImportPriceAsync(mtId, unitCode, isn, currCode, partId, date));
	}

	public Task<MTPriceForUnit> GetMTIncomeImportPriceAsync(int mtId, string unitCode, Guid isn, string currCode, int partId, DateOnly date, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtId", mtId);
		urlQuery.Add("unitCode", unitCode);
		urlQuery.Add("isn", isn);
		urlQuery.Add("currCode", currCode);
		urlQuery.Add("partId", partId);
		urlQuery.Add("date", date);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/MTIncomeImportPrice", urlQuery);
		return _client.ExecuteAsync<MTPriceForUnit>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public bool BonusCardValidation(string partnerCode, string contractCode, DateOnly? docDate, bool connectTob2barmsoftDiscountServer)
	{
		return AsyncHelper.RunSync(async () => await BonusCardValidationAsync(partnerCode, contractCode, docDate, connectTob2barmsoftDiscountServer));
	}

	public Task<bool> BonusCardValidationAsync(string partnerCode, string contractCode, DateOnly? docDate, bool connectTob2barmsoftDiscountServer, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("partnerCode", partnerCode);
		urlQuery.Add("contractCode", contractCode);
		urlQuery.Add("docDate", docDate);
		urlQuery.Add("connectTob2barmsoftDiscountServer", connectTob2barmsoftDiscountServer);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/BonusCardValidation", urlQuery);
		return _client.ExecuteAsync<bool>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public string GetPriceTypeRoungDgr(string priceType)
	{
		return AsyncHelper.RunSync(async () => await GetPriceTypeRoungDgrAsync(priceType));
	}

	public Task<string> GetPriceTypeRoungDgrAsync(string priceType, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("priceType", priceType);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/PriceTypeRoungDgr", urlQuery);
		return _client.ExecuteAsync<string>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public PartnersGoodsDataProvider.DataRow GetMTFromMapping(int partId, string partMTCode, string unitName)
	{
		return AsyncHelper.RunSync(async () => await GetMTFromMappingAsync(partId, partMTCode, unitName));
	}

	public Task<PartnersGoodsDataProvider.DataRow> GetMTFromMappingAsync(int partId, string partMTCode, string unitName, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("partId", partId);
		urlQuery.Add("partMTCode", partMTCode);
		urlQuery.Add("unitName", unitName);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/MTFromMapping", urlQuery);
		return _client.ExecuteAsync<PartnersGoodsDataProvider.DataRow>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public decimal GetGoodAvailability(int goodId, string storage = "")
	{
		return AsyncHelper.RunSync(async () => await GetGoodAvailabilityAsync(goodId, storage));
	}

	public Task<decimal> GetGoodAvailabilityAsync(int goodId, string storage = "", CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("goodId", goodId);
		urlQuery.Add("storage", storage);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GoodAvailability", urlQuery);
		return _client.ExecuteAsync<decimal>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public MaterialIncomeDiscountPrices GetMTIncomePriceAndDiscountPrice(int mtId, string unitCode, Guid isn, string currCode, int partId, DateOnly date)
	{
		return AsyncHelper.RunSync(async () => await GetMTIncomePriceAndDiscountPriceAsync(mtId, unitCode, isn, currCode, partId, date));
	}

	public Task<MaterialIncomeDiscountPrices> GetMTIncomePriceAndDiscountPriceAsync(int mtId, string unitCode, Guid isn, string currCode, int partId, DateOnly date, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtId", mtId);
		urlQuery.Add("unitCode", unitCode);
		urlQuery.Add("isn", isn);
		urlQuery.Add("currCode", currCode);
		urlQuery.Add("partId", partId);
		urlQuery.Add("date", date);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetMTIncomePriceAndDiscountPrice", urlQuery);
		return _client.ExecuteAsync<MaterialIncomeDiscountPrices>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<MaterialIncomeDiscountPrices> GetMTIncomePriceAndDiscountPriceForList(MTIncomePriceAndDiscountPriceForListRequest request)
	{
		return AsyncHelper.RunSync(async () => await GetMTIncomePriceAndDiscountPriceForListAsync(request));
	}

	public Task<List<MaterialIncomeDiscountPrices>> GetMTIncomePriceAndDiscountPriceForListAsync(MTIncomePriceAndDiscountPriceForListRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetMTIncomePriceAndDiscountPriceForList");
		return _client.ExecuteAsync<List<MaterialIncomeDiscountPrices>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public decimal GetMTSaleSumByPartner(bool getQty, CommonConst.SummaTypes summaType, CommonConst.SaleSumTypes saleSumType, int mtId, string mtGroup, DateOnly startDate, DateOnly endDate, int partId, bool getByDescr = false, string description = "", string segment = "", bool segmentNegation = false)
	{
		return AsyncHelper.RunSync(async () => await GetMTSaleSumByPartnerAsync(getQty, summaType, saleSumType, mtId, mtGroup, startDate, endDate, partId, getByDescr, description, segment, segmentNegation));
	}

	public Task<decimal> GetMTSaleSumByPartnerAsync(bool getQty, CommonConst.SummaTypes summaType, CommonConst.SaleSumTypes saleSumType, int mtId, string mtGroup, DateOnly startDate, DateOnly endDate, int partId, bool getByDescr = false, string description = "", string segment = "", bool segmentNegation = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("getQty", getQty);
		urlQuery.Add("summaType", (int)summaType);
		urlQuery.Add("saleSumType", (int)saleSumType);
		urlQuery.Add("mtId", mtId);
		urlQuery.Add("mtGroup", mtGroup);
		urlQuery.Add("startDate", startDate);
		urlQuery.Add("endDate", endDate);
		urlQuery.Add("partId", partId);
		urlQuery.Add("getByDescr", getByDescr);
		urlQuery.Add("description", description);
		urlQuery.Add("segment", segment);
		urlQuery.Add("segmentNegation", segmentNegation);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/MTSaleSumByPartner", urlQuery);
		return _client.ExecuteAsync<decimal>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public decimal CalculateDiscountByScale(decimal summa, string discountCode = "001")
	{
		return AsyncHelper.RunSync(async () => await CalculateDiscountByScaleAsync(summa, discountCode));
	}

	public Task<decimal> CalculateDiscountByScaleAsync(decimal summa, string discountCode = "001", CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("summa", summa);
		urlQuery.Add("discountCode", discountCode);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/CalculateDiscountByScale", urlQuery);
		return _client.ExecuteAsync<decimal>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public bool InnerIsActualnessDateCorrect()
	{
		return AsyncHelper.RunSync(async () => await InnerIsActualnessDateCorrectAsync());
	}

	public Task<bool> InnerIsActualnessDateCorrectAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/InnerIsActualnessDateCorrect");
		return _client.ExecuteAsync<bool>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public DataFromInvoiceListRow GetDataFromInvoiceListRow(Guid iSN, int rowID)
	{
		return AsyncHelper.RunSync(async () => await GetDataFromInvoiceListRowAsync(iSN, rowID));
	}

	public Task<DataFromInvoiceListRow> GetDataFromInvoiceListRowAsync(Guid iSN, int rowID, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("iSN", iSN);
		urlQuery.Add("rowID", rowID);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetDataFromInvoiceListRow", urlQuery);
		return _client.ExecuteAsync<DataFromInvoiceListRow>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public decimal GetSelfCostsSummByDocMTAndRow(Guid isn, int mtId, int rowID, bool isMTMoveDoc)
	{
		return AsyncHelper.RunSync(async () => await GetSelfCostsSummByDocMTAndRowAsync(isn, mtId, rowID, isMTMoveDoc));
	}

	public Task<decimal> GetSelfCostsSummByDocMTAndRowAsync(Guid isn, int mtId, int rowID, bool isMTMoveDoc, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("isn", isn);
		urlQuery.Add("mtId", mtId);
		urlQuery.Add("rowID", rowID);
		urlQuery.Add("isMTMoveDoc", isMTMoveDoc);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/SelfCostsSummByDocMTAndRow", urlQuery);
		return _client.ExecuteAsync<decimal>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public CostAmountsFromLastPurchase GetCostAmountsFromLastPurchase(int mtID, DateOnly date)
	{
		return AsyncHelper.RunSync(async () => await GetCostAmountsFromLastPurchaseAsync(mtID, date));
	}

	public Task<CostAmountsFromLastPurchase> GetCostAmountsFromLastPurchaseAsync(int mtID, DateOnly date, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtID", mtID);
		urlQuery.Add("date", date);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetCostAmountsFromLastPurchase", urlQuery);
		return _client.ExecuteAsync<CostAmountsFromLastPurchase>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public MTStateForAvgPrimeCostForming GetMTCurrentStateForAvgPrimeCostForming(int mtID, DateOnly date)
	{
		return AsyncHelper.RunSync(async () => await GetMTCurrentStateForAvgPrimeCostFormingAsync(mtID, date));
	}

	public Task<MTStateForAvgPrimeCostForming> GetMTCurrentStateForAvgPrimeCostFormingAsync(int mtID, DateOnly date, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtID", mtID);
		urlQuery.Add("date", date);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetMTCurrentStateForAvgPrimeCostForming", urlQuery);
		return _client.ExecuteAsync<MTStateForAvgPrimeCostForming>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public bool ExistInHIOrSales(int mtId, string type)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtId", mtId);
		urlQuery.Add("type", type);
		StringBuilder urlBuilder = _client.CreateURLBuilder("/api/CommonServices/ExistInHIOrSales", urlQuery);
		return AsyncHelper.RunSync(() => _client.ExecuteAsync<bool>(HttpMethod.Get, urlBuilder.ToString(), CancellationToken.None));
	}

	public bool ExistInPrices(int mtId, string qntUnit, bool throwError = true)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtId", mtId);
		urlQuery.Add("qntUnit", qntUnit);
		urlQuery.Add("throwError", throwError);
		StringBuilder urlBuilder = _client.CreateURLBuilder("/api/CommonServices/ExistInPrices", urlQuery);
		return AsyncHelper.RunSync(() => _client.ExecuteAsync<bool>(HttpMethod.Get, urlBuilder.ToString(), CancellationToken.None));
	}

	public Guid GetPartyBaseRowForAverage(int mtId)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtId", mtId);
		StringBuilder urlBuilder = _client.CreateURLBuilder("/api/CommonServices/GetPartyBaseRowForAverage", urlQuery);
		return AsyncHelper.RunSync(() => _client.ExecuteAsync<Guid>(HttpMethod.Get, urlBuilder.ToString(), CancellationToken.None));
	}

	public bool ExistInMTComplects(int mtId, string qntUnit)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtId", mtId);
		urlQuery.Add("qntUnit", qntUnit);
		StringBuilder urlBuilder = _client.CreateURLBuilder("/api/CommonServices/ExistInMTComplects", urlQuery);
		return AsyncHelper.RunSync(() => _client.ExecuteAsync<bool>(HttpMethod.Get, urlBuilder.ToString(), CancellationToken.None));
	}

	public void CheckUserCardsForUnique(string userCardNumber, int userId, bool isForEdit)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("userCardNumber", userCardNumber);
		urlQuery.Add("userId", userId);
		urlQuery.Add("isForEdit", isForEdit);
		StringBuilder urlBuilder = _client.CreateURLBuilder("/api/CommonServices/CheckUserCardsForUnique", urlQuery);
		AsyncHelper.RunSync(() => _client.ExecuteAsync(HttpMethod.Get, urlBuilder.ToString(), CancellationToken.None));
	}

	public void CheckEmployeeCardForUnique(string employeeCard, int userId, bool isForEdit)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("employeeCard", employeeCard);
		urlQuery.Add("userId", userId);
		urlQuery.Add("isForEdit", isForEdit);
		StringBuilder urlBuilder = _client.CreateURLBuilder("/api/CommonServices/CheckEmployeeCardForUnique", urlQuery);
		AsyncHelper.RunSync(() => _client.ExecuteAsync(HttpMethod.Get, urlBuilder.ToString(), CancellationToken.None));
	}

	public void CheckDiscountCardForUnique(string discountCard, short partnerId, string contract, bool isForEdit)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("discountCard", discountCard);
		urlQuery.Add("partnerId", partnerId);
		urlQuery.Add("contract", contract);
		urlQuery.Add("isForEdit", isForEdit);
		StringBuilder urlBuilder = _client.CreateURLBuilder("/api/CommonServices/CheckDiscountCardForUnique", urlQuery);
		AsyncHelper.RunSync(() => _client.ExecuteAsync(HttpMethod.Get, urlBuilder.ToString(), CancellationToken.None));
	}

	public bool FavoriteMTsExists(int userId)
	{
		return AsyncHelper.RunSync(async () => await FavoriteMTsExistsAsync(userId));
	}

	public async Task<bool> FavoriteMTsExistsAsync(int userId, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("userId", userId);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/FavoriteMTsExists", urlQuery);
		return await _client.ExecuteAsync<bool>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public decimal CalculateBonusByScale(string bonusType, decimal summa)
	{
		return AsyncHelper.RunSync(async () => await CalculateBonusByScaleAsync(bonusType, summa));
	}

	public async Task<decimal> CalculateBonusByScaleAsync(string bonusType, decimal summa, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("bonusType", bonusType);
		urlQuery.Add("summa", summa);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/CalculateBonusByScale", urlQuery);
		return await _client.ExecuteAsync<decimal>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public Guid? GetNextDocumentByOrderNumber(int orderNumber, DateOnly docDate, bool direction, Guid ignoreISN)
	{
		return AsyncHelper.RunSync(async () => await GetNextDocumentByOrderNumberAsync(orderNumber, docDate, direction, ignoreISN));
	}

	public Task<Guid?> GetNextDocumentByOrderNumberAsync(int orderNumber, DateOnly docDate, bool direction, Guid ignoreISN, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("orderNumber", orderNumber);
		urlQuery.Add("docDate", docDate);
		urlQuery.Add("direction", direction);
		urlQuery.Add("ignoreISN", ignoreISN);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetNextDocumentByOrderNumber", urlQuery);
		return _client.ExecuteAsync<Guid?>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public Guid? GetDocumentByOrderNumber(int orderNumber, DateOnly docDate, Guid ignoreISN)
	{
		return AsyncHelper.RunSync(async () => await GetDocumentByOrderNumberAsync(orderNumber, docDate, ignoreISN));
	}

	public Task<Guid?> GetDocumentByOrderNumberAsync(int orderNumber, DateOnly docDate, Guid ignoreISN, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("orderNumber", orderNumber);
		urlQuery.Add("docDate", docDate);
		urlQuery.Add("ignoreISN", ignoreISN);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetDocumentByOrderNumber", urlQuery);
		return _client.ExecuteAsync<Guid?>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<Guid> GetBasedOvExpenseISNList(Guid mtIncomeISN)
	{
		return AsyncHelper.RunSync(async () => await GetBasedOvExpenseISNListAsync(mtIncomeISN));
	}

	public Task<List<Guid>> GetBasedOvExpenseISNListAsync(Guid mtIncomeISN, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtIncomeISN", mtIncomeISN);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetBasedOvExpenseISNList", urlQuery);
		return _client.ExecuteAsync<List<Guid>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<DocumentDayOrder> GetDocumentsByOrderNumbers(int orderNumber, DateOnly docDate, bool direction, Guid ignoreISN)
	{
		return AsyncHelper.RunSync(async () => await GetDocumentsByOrderNumbersAsync(orderNumber, docDate, direction, ignoreISN));
	}

	public Task<List<DocumentDayOrder>> GetDocumentsByOrderNumbersAsync(int orderNumber, DateOnly docDate, bool direction, Guid ignoreISN, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("orderNumber", orderNumber);
		urlQuery.Add("docDate", docDate);
		urlQuery.Add("direction", direction);
		urlQuery.Add("ignoreISN", ignoreISN);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetDocumentsByOrderNumbers", urlQuery);
		return _client.ExecuteAsync<List<DocumentDayOrder>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public void ChangeDocumentOrderNumber(Guid isn, int orderNumber, bool updateMaxOrderNum = true)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("isn", isn);
		urlQuery.Add("orderNumber", orderNumber);
		urlQuery.Add("updateMaxOrderNum", updateMaxOrderNum);
		StringBuilder urlBuilder = _client.CreateURLBuilder("/api/CommonServices/ChangeDocumentOrderNumber", urlQuery);
		AsyncHelper.RunSync(() => _client.ExecuteAsync(HttpMethod.Get, urlBuilder.ToString(), CancellationToken.None));
	}

	public int GetPreviousFreeOrderNumber(Guid isn, int orderNumber, DateOnly docDate)
	{
		return AsyncHelper.RunSync(async () => await GetPreviousFreeOrderNumberAsync(isn, orderNumber, docDate));
	}

	public Task<int> GetPreviousFreeOrderNumberAsync(Guid isn, int orderNumber, DateOnly docDate, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("isn", isn);
		urlQuery.Add("orderNumber", orderNumber);
		urlQuery.Add("docDate", docDate);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetPreviousFreeOrderNumber", urlQuery);
		return _client.ExecuteAsync<int>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public int GetNextFreeOrderNumber(Guid isn, int orderNumber, DateOnly docDate)
	{
		return AsyncHelper.RunSync(async () => await GetNextFreeOrderNumberAsync(isn, orderNumber, docDate));
	}

	public Task<int> GetNextFreeOrderNumberAsync(Guid isn, int orderNumber, DateOnly docDate, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("isn", isn);
		urlQuery.Add("orderNumber", orderNumber);
		urlQuery.Add("docDate", docDate);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetNextFreeOrderNumber", urlQuery);
		return _client.ExecuteAsync<int>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public void SetCompanyData(string cashDeskcode, string taxCode, string companyName, string companyAddress, string ecrCrn)
	{
		AsyncHelper.RunSync(async () =>
		{
			await SetCompanyDataAsync(cashDeskcode, taxCode, companyName, companyAddress, ecrCrn);
		});
	}

	public Task SetCompanyDataAsync(string cashDeskcode, string taxCode, string companyName, string companyAddress, string ecrCrn, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("cashDeskcode", cashDeskcode);
		urlQuery.Add("taxCode", taxCode);
		urlQuery.Add("companyName", companyName);
		urlQuery.Add("companyAddress", companyAddress);
		urlQuery.Add("ecrCrn", ecrCrn);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/SetCompanyData", urlQuery);
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken);
	}

	public List<BonusSumsComparisonDataRow> GetBonusCardsRemAndTurn(string cardNumber)
	{
		return AsyncHelper.RunSync(async () => await GetBonusCardsRemAndTurnAsync(cardNumber));
	}

	public Task<List<BonusSumsComparisonDataRow>> GetBonusCardsRemAndTurnAsync(string cardNumber, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("cardNumber", cardNumber);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetBonusCardsRemAndTurn", urlQuery);
		return _client.ExecuteAsync<List<BonusSumsComparisonDataRow>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken);
	}

	public BonusTurnRow GetPartnerBonusCardTurnByCashierShift(int partnerID, string contractCode, string cashDeskCode, string shiftNumber)
	{
		return AsyncHelper.RunSync(async () => await GetPartnerBonusCardTurnByCashierShiftAsync(partnerID, contractCode, cashDeskCode, shiftNumber));
	}

	public Task<BonusTurnRow> GetPartnerBonusCardTurnByCashierShiftAsync(int partnerID, string contractCode, string cashDeskCode, string shiftNumber, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("partnerID", partnerID);
		urlQuery.Add("contractCode", contractCode);
		urlQuery.Add("cashDeskCode", cashDeskCode);
		urlQuery.Add("shiftNumber", shiftNumber);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetPartnerBonusCardTurnByCashierShift", urlQuery);
		return _client.ExecuteAsync<BonusTurnRow>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken);
	}

	public bool EditDiscountServerData(int partID, string contractCode, bool exportedToDiscountServer)
	{
		return AsyncHelper.RunSync(async () => await EditDiscountServerDataAsync(partID, contractCode, exportedToDiscountServer));
	}

	public Task<bool> EditDiscountServerDataAsync(int partID, string contractCode, bool exportedToDiscountServer, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("partID", partID);
		urlQuery.Add("contractCode", contractCode);
		urlQuery.Add("exportedToDiscountServer", exportedToDiscountServer);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/EditDiscountServerData", urlQuery);
		return _client.ExecuteAsync<bool>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken);
	}

	public int GetUserIdByCardNumber(string cardNumber)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("cardNumber", cardNumber);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetUserIdByCardNumber", urlQuery);
		return _client.Execute<int>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string GetAvailableCashesDelimited()
	{
		return AsyncHelper.RunSync(async () => await GetAvailableCashesDelimitedAsync());
	}

	public Task<string> GetAvailableCashesDelimitedAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/AvailableCashesDelimited");
		return _client.ExecuteAsync<string>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public string GetAvailableStoragesDelimited(bool isViewAccess = true)
	{
		return AsyncHelper.RunSync(async () => await GetAvailableStoragesDelimitedAsync(isViewAccess));
	}

	public Task<string> GetAvailableStoragesDelimitedAsync(bool isViewAccess = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("isViewAccess", isViewAccess);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/AvailableStoragesDelimited", urlQuery);
		return _client.ExecuteAsync<string>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<Guid> GetDocISNByECRN(string ecrCheckNum, DateOnly endDate, short docType)
	{
		return AsyncHelper.RunSync(async () => await GetDocISNByECRNAsync(ecrCheckNum, endDate, docType));
	}

	public Task<List<Guid>> GetDocISNByECRNAsync(string ecrCheckNum, DateOnly endDate, short docType, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("ecrCheckNum", ecrCheckNum);
		urlQuery.Add("endDate", endDate);
		urlQuery.Add("docType", docType);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/GetDocISNByECRN", urlQuery);
		return _client.ExecuteAsync<List<Guid>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public bool CheckEntriesExistenceByBank(string accBankCode)
	{
		return AsyncHelper.RunSync(async () => await CheckEntriesExistenceByBankAsync(accBankCode));
	}

	public Task<bool> CheckEntriesExistenceByBankAsync(string accBankCode, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("accBankCode", accBankCode);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/CheckEntriesExistenceByBank", urlQuery);
		return _client.ExecuteAsync<bool>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public bool ExistAnyDocByType(short docType)
	{
		return AsyncHelper.RunSync(async () => await ExistAnyDocByTypeAsync(docType));
	}

	public Task<bool> ExistAnyDocByTypeAsync(short docType, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("docType", docType);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/CommonServices/ExistAnyDocByType", urlQuery);
		return _client.ExecuteAsync<bool>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}
}
