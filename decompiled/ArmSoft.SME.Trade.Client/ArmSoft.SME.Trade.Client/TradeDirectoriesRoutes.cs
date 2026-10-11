using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Trade.Client.Models;

namespace ArmSoft.SME.Trade.Client;

public class TradeDirectoriesRoutes
{
	private readonly TradeClient client;

	internal TradeDirectoriesRoutes(TradeClient client)
	{
		this.client = client;
	}

	public List<Currency> GetCurrencies()
	{
		return AsyncHelper.RunSync(async () => await GetCurrenciesAsync());
	}

	public Task<List<Currency>> GetCurrenciesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/TradeDirectory/Currencies");
		return client.ExecuteAsync<List<Currency>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<PartnerGroup> GetPartnerGroups()
	{
		return AsyncHelper.RunSync(async () => await GetPartnerGroupsAsync());
	}

	public Task<List<PartnerGroup>> GetPartnerGroupsAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/PartnerGroups");
		return client.ExecuteAsync<List<PartnerGroup>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<Partner> GetPartners(string partnerCode = "", string partnerType = "")
	{
		return AsyncHelper.RunSync(async () => await GetPartnersAsync(partnerCode, partnerType));
	}

	public Task<List<Partner>> GetPartnersAsync(string partnerCode = "", string partnerType = "", CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("partnerCode", partnerCode);
		urlQuery.Add("partnerType", partnerType);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/Partners", urlQuery);
		return client.ExecuteAsync<List<Partner>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<QntUnit> GetQntUnits()
	{
		return AsyncHelper.RunSync(async () => await GetQntUnitsAsync());
	}

	public Task<List<QntUnit>> GetQntUnitsAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/QntUnits");
		return client.ExecuteAsync<List<QntUnit>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<Storage> GetStorages(string storageCode = "")
	{
		return AsyncHelper.RunSync(async () => await GetStoragesAsync(storageCode));
	}

	public Task<List<Storage>> GetStoragesAsync(string storageCode = "", bool checkUserAccess = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("storageCode", storageCode);
		urlQuery.Add("checkUserAccess", checkUserAccess);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/Storages", urlQuery);
		return client.ExecuteAsync<List<Storage>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public Task<List<MaterialShort>> GetMaterialsShortAsync(CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/MaterialsShort");
		return client.ExecuteAsync<List<MaterialShort>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public Task<List<Material>> GetMaterialsAsync(string mtCode = "", CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtCode", mtCode);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/Materials", urlQuery);
		return client.ExecuteAsync<List<Material>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<Material> GetMaterials(string mtCode = "")
	{
		return AsyncHelper.RunSync(async () => await GetMaterialsAsync(mtCode));
	}

	public Task<List<MTGroupShort>> GetMTGroupsShortAsync(CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/MTGroupsShort");
		return client.ExecuteAsync<List<MTGroupShort>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<MTGroup> GetMTGroups()
	{
		return AsyncHelper.RunSync(async () => await GetMTGroupsAsync());
	}

	public Task<List<MTGroup>> GetMTGroupsAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/MTGroups");
		return client.ExecuteAsync<List<MTGroup>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public Task<List<MTBarCodesShort>> GetMTBarCodesShortAsync(CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/MTBarCodesShort");
		return client.ExecuteAsync<List<MTBarCodesShort>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public Task<List<PartnerShort>> GetPartnersShortAsync(CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/PartnersShort");
		return client.ExecuteAsync<List<PartnerShort>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public Task<List<PartnerGroupShort>> GetPartnerGroupsShortAsync(CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/PartnerGroupsShort");
		return client.ExecuteAsync<List<PartnerGroupShort>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<Cashes> GetCashes(bool checkUserAccess)
	{
		return AsyncHelper.RunSync(async () => await GetCashesAsync(checkUserAccess));
	}

	public Task<List<Cashes>> GetCashesAsync(bool checkUserAccess, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("checkUserAccess", checkUserAccess);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/Cashes", urlQuery);
		return client.ExecuteAsync<List<Cashes>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<NonCashInfo> GetNonCashInfoListByCashCode(bool checkUserAccess, string code)
	{
		return AsyncHelper.RunSync(async () => await GetNonCashInfoListByCashCodeAsync(checkUserAccess, code));
	}

	public Task<List<NonCashInfo>> GetNonCashInfoListByCashCodeAsync(bool checkUserAccess, string code, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("checkUserAccess", checkUserAccess);
		urlQuery.Add("code", code);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/NonCashInfoList", urlQuery);
		return client.ExecuteAsync<List<NonCashInfo>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public Task<List<CashesShort>> GetCashesShortAsync(bool checkUserAccess, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("checkUserAccess", checkUserAccess);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/CashesShort", urlQuery);
		return client.ExecuteAsync<List<CashesShort>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<PriceTypes> GetPriceTypes()
	{
		return AsyncHelper.RunSync(async () => await GetPriceTypesAsync());
	}

	public Task<List<PriceTypes>> GetPriceTypesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/PriceTypes");
		return client.ExecuteAsync<List<PriceTypes>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<PartnersContract> GetPartnersContract(string partnerCode = "")
	{
		return AsyncHelper.RunSync(async () => await GetPartnersContractAsync(partnerCode));
	}

	public Task<List<PartnersContract>> GetPartnersContractAsync(string partnerCode = "", CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("partnerCode", partnerCode);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/PartnersContracts", urlQuery);
		return client.ExecuteAsync<List<PartnersContract>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<AccountInBanks> GetAccountInBanks()
	{
		return AsyncHelper.RunSync(async () => await GetAccountInBanksAsync());
	}

	public Task<List<AccountInBanks>> GetAccountInBanksAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/AccountInBanks");
		return client.ExecuteAsync<List<AccountInBanks>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<Employee> GetEmployees(string EmplCode = "")
	{
		return AsyncHelper.RunSync(async () => await GetEmployeesAsync(EmplCode));
	}

	public Task<List<Employee>> GetEmployeesAsync(string emplCode = "", CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("emplCode", emplCode);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/Employees", urlQuery);
		return client.ExecuteAsync<List<Employee>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<Banks> GetBanks()
	{
		return AsyncHelper.RunSync(async () => await GetBanksAsync());
	}

	public Task<List<Banks>> GetBanksAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/TradeDirectory/Banks");
		return client.ExecuteAsync<List<Banks>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<User> GetUsers()
	{
		return AsyncHelper.RunSync(async () => await GetUsersAsync());
	}

	public Task<List<User>> GetUsersAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/Users");
		return client.ExecuteAsync<List<User>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<CodeName> GetStandardComments()
	{
		return AsyncHelper.RunSync(async () => await GetStandardCommentsAsync());
	}

	public Task<List<CodeName>> GetStandardCommentsAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/StandardComments");
		return client.ExecuteAsync<List<CodeName>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<CashierShiftCache> GetCashierShifts()
	{
		return AsyncHelper.RunSync(async () => await GetCashierShiftsAsync());
	}

	public Task<List<CashierShiftCache>> GetCashierShiftsAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/CashierShifts");
		return client.ExecuteAsync<List<CashierShiftCache>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<CodeName> GetCountries()
	{
		return AsyncHelper.RunSync(async () => await GetCountriesAsync());
	}

	public Task<List<CodeName>> GetCountriesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/Countries");
		return client.ExecuteAsync<List<CodeName>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<CodeName> GetProducers()
	{
		return AsyncHelper.RunSync(async () => await GetProducersAsync());
	}

	public Task<List<CodeName>> GetProducersAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/Producers");
		return client.ExecuteAsync<List<CodeName>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<CodeName> GetSubstituteItemsGroups()
	{
		return AsyncHelper.RunSync(async () => await GetSubstituteItemsGroupsAsync());
	}

	public Task<List<CodeName>> GetSubstituteItemsGroupsAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/SubstituteItemsGroups");
		return client.ExecuteAsync<List<CodeName>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<CodeName> GetSegments()
	{
		return AsyncHelper.RunSync(async () => await GetSegmentsAsync());
	}

	public Task<List<CodeName>> GetSegmentsAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/Segments");
		return client.ExecuteAsync<List<CodeName>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<Properties> GetProperties()
	{
		return AsyncHelper.RunSync(async () => await GetPropertiesAsync());
	}

	public Task<List<Properties>> GetPropertiesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/Properties");
		return client.ExecuteAsync<List<Properties>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<PropertiesList> GetPropertiesList()
	{
		return AsyncHelper.RunSync(async () => await GetPropertiesListAsync());
	}

	public Task<List<PropertiesList>> GetPropertiesListAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/PropertiesList");
		return client.ExecuteAsync<List<PropertiesList>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<Discount> GetDiscounts()
	{
		return AsyncHelper.RunSync(async () => await GetDiscountAsync());
	}

	public Task<List<Discount>> GetDiscountAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/Discounts");
		return client.ExecuteAsync<List<Discount>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<DiscountGroup> GetDiscountGroups()
	{
		return AsyncHelper.RunSync(async () => await GetDiscountGroupsAsync());
	}

	public Task<List<DiscountGroup>> GetDiscountGroupsAsync(string code = "", CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("code", code);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Directories/DiscountGroups", urlQuery);
		return client.ExecuteAsync<List<DiscountGroup>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}
}
