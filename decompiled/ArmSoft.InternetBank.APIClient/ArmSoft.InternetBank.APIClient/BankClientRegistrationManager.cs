using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.APIClient;

public class BankClientRegistrationManager : BaseManager
{
	internal BankClientRegistrationManager(Client client)
		: base(client)
	{
	}

	public async Task<List<KeyValuePair<BankClientRegistrationInformationOption, string>>> GetClientsBankInformationOptionsAsync(bool isRegistrationForBankLegalClient = false)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["isRegistrationForBankLegalClient"] = isRegistrationForBankLegalClient };
		return await client.GetAsync<List<KeyValuePair<BankClientRegistrationInformationOption, string>>>("clientsBankInformationOptions", requireAuthorization: false, null, queryParams);
	}

	public async Task<Guid> ValidateUserInfoAsync(UserInfoForClientRegistration userInfo)
	{
		return await client.PostAsync<Guid>("registration/validateUserInfo", requireAuthorization: false, null, null, userInfo);
	}

	public async Task<string> SendVerificationCodeToUserAsync(UserVerificationType verificationType, BankClientRegistrationInformationOption clientsBankInformationOption, string clientsBankInformation)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["verificationType"] = verificationType,
			["clientsBankInformationOption"] = clientsBankInformationOption,
			["clientsBankInformation"] = clientsBankInformation
		};
		return await client.PostAsync("registration/sendVerificationCodeForBankClientRegistration", requireAuthorization: false, null, queryParams);
	}

	public async Task<Guid> ValidateVerificationCodeAsync(UserVerificationType verificationType, Guid bankClientRegistrationInformationId, UserInfoForClientRegistration userInfo)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["verificationType"] = verificationType,
			["bankClientRegistrationInformationId"] = bankClientRegistrationInformationId
		};
		return await client.PostAsync<Guid>("registration/validateVerificationCodeForBankClientRegistration", requireAuthorization: false, null, queryParams, userInfo);
	}

	public async Task<string> RegisterUserAsync(Guid bankClientRegistrationInformationId, UserInfoForClientRegistration userInfo)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["bankClientRegistrationInformationId"] = bankClientRegistrationInformationId };
		return await client.PostAsync("registration/registerUser", requireAuthorization: false, null, queryParams, userInfo);
	}
}
