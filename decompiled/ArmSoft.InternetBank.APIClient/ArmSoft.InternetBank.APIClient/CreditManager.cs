using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.APIClient;

public class CreditManager : BaseManager
{
	internal CreditManager(Client client)
		: base(client)
	{
	}

	public List<Credit> GetCredits(CreditAccessLevels desiredAccess = CreditAccessLevels.View)
	{
		CreditsParams postBody = new CreditsParams
		{
			DesiredAccess = desiredAccess
		};
		return client.Post<List<Credit>>("credits", requireAuthorization: true, null, null, postBody);
	}

	public async Task<List<Credit>> GetCreditsAsync(CreditAccessLevels desiredAccess = CreditAccessLevels.View)
	{
		CreditsParams postBody = new CreditsParams
		{
			DesiredAccess = desiredAccess
		};
		return await client.PostAsync<List<Credit>>("credits", requireAuthorization: true, null, null, postBody);
	}

	public Credit GetCreditDetails(string contractNumber, RepaymentType repaymentType, decimal amount, bool isFullRepayment)
	{
		CreditDetailsParams postBody = new CreditDetailsParams
		{
			ContractNumber = contractNumber,
			RepaymentType = repaymentType,
			Amount = amount,
			IsFullRepayment = isFullRepayment
		};
		return client.Post<Credit>("v2/credits/details", requireAuthorization: true, null, null, postBody);
	}

	public Task<Credit> GetCreditDetailsAsync(string contractNumber, RepaymentType repaymentType, decimal amount, bool isFullRepayment, bool includePrematureRepaymentFee)
	{
		CreditDetailsParams postBody = new CreditDetailsParams
		{
			ContractNumber = contractNumber,
			RepaymentType = repaymentType,
			Amount = amount,
			IsFullRepayment = isFullRepayment,
			IncludePrematureRepaymentFee = includePrematureRepaymentFee
		};
		return client.PostAsync<Credit>("v2/credits/details", requireAuthorization: true, null, null, postBody);
	}

	public List<CreditSchedule> GetCreditSchedule(string contractNumber, int? count = null)
	{
		CreditScheduleParams postBody = new CreditScheduleParams
		{
			ContractNumber = contractNumber,
			Count = count
		};
		return client.Post<List<CreditSchedule>>("credits/schedule", requireAuthorization: true, null, null, postBody);
	}

	public Task<List<CreditSchedule>> GetCreditScheduleAsync(string contractNumber, int? count = null)
	{
		CreditScheduleParams postBody = new CreditScheduleParams
		{
			ContractNumber = contractNumber,
			Count = count
		};
		return client.PostAsync<List<CreditSchedule>>("credits/schedule", requireAuthorization: true, null, null, postBody);
	}

	public List<CreditTransaction> GetCreditTransactions(string contractNumber, Period? period = Period.CurrentMonth, DateTime? dateFrom = null, DateTime? dateTo = null, bool showAmd = false, int? count = null, TransactionFilterOption filterOption = TransactionFilterOption.All)
	{
		CreditTransactionsParams postBody = new CreditTransactionsParams
		{
			ContractNumber = contractNumber,
			Period = period,
			DateFrom = dateFrom,
			DateTo = dateTo,
			ShowAMD = showAmd,
			Count = count,
			FilterOption = filterOption
		};
		return client.Post<List<CreditTransaction>>("credits/transactions", requireAuthorization: true, null, null, postBody);
	}

	public Task<List<CreditTransaction>> GetCreditTransactionsAsync(string contractNumber, Period? period = null, DateTime? dateFrom = null, DateTime? dateTo = null, bool showAmd = false, int? count = null, TransactionFilterOption filterOption = TransactionFilterOption.All)
	{
		CreditTransactionsParams postBody = new CreditTransactionsParams
		{
			ContractNumber = contractNumber,
			Period = period,
			DateFrom = dateFrom,
			DateTo = dateTo,
			ShowAMD = showAmd,
			Count = count,
			FilterOption = filterOption
		};
		return client.PostAsync<List<CreditTransaction>>("credits/transactions", requireAuthorization: true, null, null, postBody);
	}

	public async Task<List<DocumentInfo>> GetDocumentInfoListForCreditRepaymentAsync(string documentType, string contractNo, RepaymentType repaymentType)
	{
		CreditRepaymentDocumentInfoParams postBody = new CreditRepaymentDocumentInfoParams
		{
			DocumentType = documentType,
			ContractNo = contractNo,
			RepaymentType = repaymentType
		};
		return (await client.PostAsync<ResponseResult<List<DocumentInfo>>>("v2/creditRepaymentDocumentInfos", requireAuthorization: true, null, null, postBody)).Value;
	}
}
