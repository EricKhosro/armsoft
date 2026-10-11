using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class CommissionManager : BaseManager
{
	internal CommissionManager(Client client)
		: base(client)
	{
	}

	public async Task<Commission> GetInnerPaymentCommissionAsync(InnerPaymentCommissionParams commissionParams)
	{
		return (await client.PostAsync<ResponseResult<Commission>>("v2/commission/innerPayment", requireAuthorization: true, null, null, commissionParams)).Value;
	}

	public Commission GetOuterPaymentCommission(OuterTransferCommissionParams commissionParams)
	{
		return client.Post<ResponseResult<Commission>>("v2/commission/outerPayment", requireAuthorization: true, null, null, commissionParams).Value;
	}

	public async Task<Commission> GetOuterPaymentCommissionAsync(OuterTransferCommissionParams commissionParams)
	{
		return (await client.PostAsync<ResponseResult<Commission>>("v2/commission/outerPayment", requireAuthorization: true, null, null, commissionParams)).Value;
	}

	public async Task<Commission> GetCurrencyConversionCommissionAsync(CommissionParams commissionParams)
	{
		return await client.PostAsync<Commission>("commission/currencyConversion", requireAuthorization: true, null, null, commissionParams);
	}

	public async Task<Commission> GetCashOutByPhoneCommissionAsync(CashOutCommissionParams commissionParams)
	{
		return (await client.PostAsync<ResponseResult<Commission>>("v2/commission/cashOutByPhone", requireAuthorization: true, null, null, commissionParams)).Value;
	}

	public async Task<Commission> GetCardTransferCommissionAsync(string payerAccount, string receiverCardNumber, decimal amount, string currency)
	{
		CardTransferCommissionParams postBody = new CardTransferCommissionParams
		{
			PayerAccount = payerAccount,
			ReceiverCardNumber = receiverCardNumber,
			Amount = amount,
			Currency = currency
		};
		return (await client.PostAsync<ResponseResult<Commission>>("v2/commission/cardTransfer", requireAuthorization: true, null, null, postBody)).Value;
	}

	public async Task<Commission> GetUtilityPaymentCommissionAsync(UtilityPaymentCommissionParams commissionParams)
	{
		return (await client.PostAsync<ResponseResult<Commission>>("commission/utilityPayment", requireAuthorization: true, null, null, commissionParams)).Value;
	}

	public MultipleUtilityCommission GetMultipleUtilityPaymentCommission(List<UtilityPaymentCommissionParams> utilityAmounts)
	{
		MultipleUtilityPaymentCommissionParams postBody = new MultipleUtilityPaymentCommissionParams
		{
			UtilityAmounts = utilityAmounts
		};
		return client.Post<ResponseResult<MultipleUtilityCommission>>("commission/multipleUtilityPayment", requireAuthorization: true, null, null, postBody).Value;
	}

	public async Task<MultipleUtilityCommission> GetMultipleUtilityPaymentCommissionAsync(List<UtilityPaymentCommissionParams> utilityAmounts)
	{
		MultipleUtilityPaymentCommissionParams postBody = new MultipleUtilityPaymentCommissionParams
		{
			UtilityAmounts = utilityAmounts
		};
		return (await client.PostAsync<ResponseResult<MultipleUtilityCommission>>("commission/multipleUtilityPayment", requireAuthorization: true, null, null, postBody)).Value;
	}

	public async Task<Commission> GetArcaPayTransferCommissionAsync(ArCaP2PTransferCommissionParams commissionParams)
	{
		return (await client.PostAsync<ResponseResult<Commission>>("commission/arcaP2PTransfer", requireAuthorization: true, null, null, commissionParams)).Value;
	}

	public async Task<Commission> GetCashWithdrawalCommissionAsync(CashWithdrawalCommissionParams commissionParams)
	{
		return (await client.PostAsync<ResponseResult<Commission>>("commission/cashWithdrawal", requireAuthorization: true, null, null, commissionParams)).Value;
	}
}
