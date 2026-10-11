using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.APIClient.Core;

public class DirectoryManager : BaseManager
{
	internal DirectoryManager(Client client)
		: base(client)
	{
	}

	public List<Branch> GetBranches(bool onlyAllowedForCashWithdrawal)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["onlyAllowedForCashWithdrawal"] = onlyAllowedForCashWithdrawal };
		return client.Get<List<Branch>>("directories/branches", requireAuthorization: true, null, queryParams);
	}

	public async Task<List<Branch>> GetBranchesAsync(bool onlyAllowedForCashWithdrawal)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["onlyAllowedForCashWithdrawal"] = onlyAllowedForCashWithdrawal };
		return await client.GetAsync<List<Branch>>("directories/branches", requireAuthorization: true, null, queryParams);
	}

	public async Task<List<OpenAccountTypeConfiguration>> GetOpenAccountTypeConfigurationsAsync()
	{
		return await client.GetAsync<List<OpenAccountTypeConfiguration>>("directories/openAccountTypeConfigurations", requireAuthorization: true);
	}

	public List<CommunalUtility> GetCommunalUtilities()
	{
		return client.Get<List<CommunalUtility>>("directories/utilities", requireAuthorization: true);
	}

	public async Task<List<CommunalUtility>> GetCommunalUtilitiesAsync()
	{
		return await client.GetAsync<List<CommunalUtility>>("directories/utilities", requireAuthorization: true);
	}

	public List<CommunalUtilityLocation> GetCommunalUtilityLocations(string utilityCode)
	{
		return client.Get<List<CommunalUtilityLocation>>("directories/utilities/" + utilityCode + "/locations", requireAuthorization: true);
	}

	public async Task<List<CommunalUtilityLocation>> GetCommunalUtilityLocationsAsync(string utilityCode)
	{
		return await client.GetAsync<List<CommunalUtilityLocation>>("directories/utilities/" + utilityCode + "/locations", requireAuthorization: true);
	}

	public List<BankModel> GetBanks()
	{
		return client.Get<List<BankModel>>("directories/banks", requireAuthorization: true);
	}

	public async Task<List<BankModel>> GetBanksAsync()
	{
		return await client.GetAsync<List<BankModel>>("directories/banks", requireAuthorization: true);
	}

	public List<ForeignBank> GetForeignBanks()
	{
		return client.Get<List<ForeignBank>>("directories/ForeignBanks", requireAuthorization: true);
	}

	public async Task<List<ForeignBank>> GetForeignBanksAsync()
	{
		return await client.GetAsync<List<ForeignBank>>("directories/ForeignBanks", requireAuthorization: true);
	}

	public List<CustomizationType> GetCustomizationTypes()
	{
		return client.Get<List<CustomizationType>>("directories/CustomizationTypes", requireAuthorization: true);
	}

	public async Task<List<CustomizationType>> GetCustomizationTypesAsync()
	{
		return await client.GetAsync<List<CustomizationType>>("directories/CustomizationTypes", requireAuthorization: true);
	}

	public List<CardType> GetCardTypes()
	{
		return client.Get<List<CardType>>("directories/CardTypes", requireAuthorization: true);
	}

	public async Task<List<CardType>> GetCardTypesAsync()
	{
		return await client.GetAsync<List<CardType>>("directories/CardTypes", requireAuthorization: true);
	}

	public async Task<List<ChargeMode>> GetChargeModesAsync()
	{
		return await client.GetAsync<List<ChargeMode>>("directories/chargeModes", requireAuthorization: true);
	}

	public async Task<List<ForeignNameAndAddressCode>> GetSwiftNameAndAddressCodesAsync()
	{
		return await client.GetAsync<List<ForeignNameAndAddressCode>>("directories/swiftNameAndAddressCodes", requireAuthorization: true);
	}

	public async Task<SwiftBank> GetSwiftBankInfoAsync(string swift)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["swift"] = swift };
		return await client.GetAsync<SwiftBank>("directories/swiftBanks", requireAuthorization: true, null, queryParams);
	}

	public async Task<List<Currerncy>> GetOpenDepositUsedCurrenciesAsync()
	{
		return await client.GetAsync<List<Currerncy>>("directories/openDepositUsedCurrencies", requireAuthorization: true);
	}

	public async Task<List<Currerncy>> GetPurchaseBondUsedCurrenciesAsync()
	{
		return await client.GetAsync<List<Currerncy>>("directories/purchaseBondUsedCurrencies", requireAuthorization: true);
	}

	public async Task<List<KeyValuePair<BankQuestion, string>>> GetBankQuestionsAsync()
	{
		return await client.GetAsync<List<KeyValuePair<BankQuestion, string>>>("bankQuestions", requireAuthorization: false);
	}
}
