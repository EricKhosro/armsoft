using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.APIClient.Core;

public class ParamManager
{
	private readonly ClientBase client;

	public UserVerificationOptions VerifyUserCreate => client.Get<UserVerificationOptions>("params/verifyUserCreate", requireAuthorization: true);

	public UserVerificationOptions VerifyUserEdit => client.Get<UserVerificationOptions>("params/verifyUserEdit", requireAuthorization: true);

	public string IBVersion
	{
		get
		{
			try
			{
				return client.Get<object>("params/version", requireAuthorization: false).ToString();
			}
			catch (NotFoundException)
			{
				return "0.0.0";
			}
		}
	}

	public Task<int> DocumentAttachmentMaxSize => client.GetAsync<int>("params/documentMaxAttachmentSize", requireAuthorization: true);

	public Task<bool> AllowIntrabankCurrencyConversion => client.GetAsync<bool>("params/allowIntrabankCurrencyConversion", requireAuthorization: true);

	public Task<bool> AllowCardTransferToOtherProcessingCards => client.GetAsync<bool>("params/allowTransfersToOtherProcessingCards", requireAuthorization: true);

	public Task<bool> ShowCommission => client.GetAsync<bool>("params/showCommission", requireAuthorization: true);

	public Task<List<string>> GetCommissionEnabledDocumentTypes => client.GetAsync<List<string>>("params/getCommissionEnabledDocumentTypes", requireAuthorization: true);

	public Task<ParamsForUrgentTransfer> GetParamsForUrgentTransfer => client.GetAsync<ParamsForUrgentTransfer>("params/urgentTransfer", requireAuthorization: true);

	public Task<InitialParams> GetInitialParams => client.GetAsync<InitialParams>("params/initialParams", requireAuthorization: false);

	public Task<bool> AllowLoanRepaymentFromCardAccounts => client.GetAsync<bool>("params/allowCreditRepaymentFromCardAccounts", requireAuthorization: true);

	public Task<bool> GetOfferReceiverNameInBudgetaryPayments => client.GetAsync<bool>("params/offerReceiverNameInBudgetaryPayments", requireAuthorization: true);

	public Task<bool> GetAttachedCardsAvailablity => client.GetAsync<bool>("params/attachedCardsAvailable", requireAuthorization: true);

	public Task<decimal> GetAttachCardTransactionAmount => client.GetAsync<decimal>("params/attachCardTransactionAmount", requireAuthorization: true);

	public Task<List<string>> GetArcaMerchantTransactionAllowedCurrencies => client.GetAsync<List<string>>("params/arcaMerchantTransactionAllowedCurrencies", requireAuthorization: true);

	public Task<ParamsForCardSettings> GetCardSettingsParamsAsync => client.GetAsync<ParamsForCardSettings>("params/cardSettingsParams", requireAuthorization: true);

	public Task<object> GetNewMobileAppNameAsync => client.GetAsync<object>("params/newMobileAppName", requireAuthorization: false);

	public Task<ParamsForRegistrationButton> GetRegistrationParamsAsync => client.GetAsync<ParamsForRegistrationButton>("params/registrationButtonParams", requireAuthorization: false);

	public Task<ParamsForBankClientRegistration> GetParamsForBankClientRegistration => client.GetAsync<ParamsForBankClientRegistration>("params/bankClientRegistrationParams", requireAuthorization: false);

	public Task<ParamsForBankClientRegistration> GetParamsForBankLegalClientRegistration => client.GetAsync<ParamsForBankClientRegistration>("params/bankLegalClientRegistrationParams", requireAuthorization: false);

	public Task<List<DigitalCardType>> GetDigitalCardTypesAsync => client.GetAsync<List<DigitalCardType>>("params/digitalCardTypes", requireAuthorization: true);

	public Task<bool> GetShowBalancesInAccountsListAsync => client.GetAsync<bool>("params/showBalancesInAccountsList", requireAuthorization: true);

	public Task<bool> GetArCaP2PSettingsAvailabilityAsync => client.GetAsync<bool>("params/enableArCaP2PSettings", requireAuthorization: true);

	public Task<ParamsForPersonalDataModifications> GetParamsForPersonalDataModificationsAsync => client.GetAsync<ParamsForPersonalDataModifications>("params/personalDataModificationParams", requireAuthorization: true);

	public Task<int> ImportedDocumentsMaxCount => client.GetAsync<int>("params/importedDocumentsMaxCount", requireAuthorization: true);

	public Task<string> GetCashOutByPhoneInformationAsync => client.GetAsync("params/cashOutByPhoneInformation", requireAuthorization: true);

	internal ParamManager(ClientBase client)
	{
		this.client = client;
	}

	public async Task<bool> GetDocumentCommissionEnabled(string docType)
	{
		GetDocumentCommissionEnabledParams postBody = new GetDocumentCommissionEnabledParams
		{
			DocumentType = docType
		};
		return await client.PostAsync<bool>("params/isDocumentCommissionEnabled", requireAuthorization: true, null, null, postBody);
	}
}
