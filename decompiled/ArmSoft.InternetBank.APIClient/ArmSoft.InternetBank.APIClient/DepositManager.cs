using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.APIClient;

public class DepositManager : BaseManager
{
	internal DepositManager(Client client)
		: base(client)
	{
	}

	public List<Deposit> GetDeposits(DepositAccessLevels desiredAccess = DepositAccessLevels.View)
	{
		DepositsParams postBody = new DepositsParams
		{
			DesiredAccess = desiredAccess
		};
		return client.Post<List<Deposit>>("deposits", requireAuthorization: true, null, null, postBody);
	}

	public async Task<List<Deposit>> GetDepositsAsync(DepositAccessLevels desiredAccess = DepositAccessLevels.View)
	{
		DepositsParams postBody = new DepositsParams
		{
			DesiredAccess = desiredAccess
		};
		return await client.PostAsync<List<Deposit>>("deposits", requireAuthorization: true, null, null, postBody);
	}

	public Deposit GetDepositDetails(string contractNumber)
	{
		DepositDetailsParams postBody = new DepositDetailsParams
		{
			ContractNumber = contractNumber
		};
		return client.Post<Deposit>("deposits/details", requireAuthorization: true, null, null, postBody);
	}

	public async Task<Deposit> GetDepositDetailsAsync(string contractNumber)
	{
		DepositDetailsParams postBody = new DepositDetailsParams
		{
			ContractNumber = contractNumber
		};
		return await client.PostAsync<Deposit>("deposits/details", requireAuthorization: true, null, null, postBody);
	}

	public List<DepositTransaction> GetDepositTransactions(string contractNumber, Period? period = Period.CurrentMonth, DateTime? dateFrom = null, DateTime? dateTo = null, bool showAmd = false, int? count = null, TransactionFilterOption filterOption = TransactionFilterOption.All)
	{
		DepositTransactionsParams postBody = new DepositTransactionsParams
		{
			ContractNumber = contractNumber,
			Period = period,
			DateFrom = dateFrom,
			DateTo = dateTo,
			ShowAMD = showAmd,
			Count = count,
			FilterOption = filterOption
		};
		return client.Post<List<DepositTransaction>>("deposits/transactions", requireAuthorization: true, null, null, postBody);
	}

	public async Task<List<DepositTransaction>> GetDepositTransactionsAsync(string contractNumber, Period? period = Period.CurrentMonth, DateTime? dateFrom = null, DateTime? dateTo = null, bool showAmd = false, int? count = null, TransactionFilterOption filterOption = TransactionFilterOption.All)
	{
		DepositTransactionsParams postBody = new DepositTransactionsParams
		{
			ContractNumber = contractNumber,
			Period = period,
			DateFrom = dateFrom,
			DateTo = dateTo,
			ShowAMD = showAmd,
			Count = count,
			FilterOption = filterOption
		};
		return await client.PostAsync<List<DepositTransaction>>("deposits/transactions", requireAuthorization: true, null, null, postBody);
	}

	public List<DepositTemplate> GetDepositTemplates()
	{
		return client.Get<List<DepositTemplate>>("depositTemplates", requireAuthorization: true);
	}

	public async Task<List<DepositTemplate>> GetDepositTemplatesAsync()
	{
		return await client.GetAsync<List<DepositTemplate>>("depositTemplates", requireAuthorization: true);
	}

	public List<DepositSchedule> GetDepositSchedule(string contractNumber, int? count = null)
	{
		DepositScheduleParams postBody = new DepositScheduleParams
		{
			ContractNumber = contractNumber,
			Count = count
		};
		return client.Post<List<DepositSchedule>>("deposits/schedule", requireAuthorization: true, null, null, postBody);
	}

	public async Task<List<DepositSchedule>> GetDepositScheduleAsync(string contractNumber, int? count = null)
	{
		DepositScheduleParams postBody = new DepositScheduleParams
		{
			ContractNumber = contractNumber,
			Count = count
		};
		return await client.PostAsync<List<DepositSchedule>>("deposits/schedule", requireAuthorization: true, null, null, postBody);
	}

	public DepositCalculatedInterestAmount GetDepositCalculatedInterestAmount(DateTime dateBegin, DateTime dateEnd, decimal amount, string currency, bool capitalized, string floatingRateCode, InterestPayPeriod payPeriod, string passOverDirection, string passOverType, int rateDiv, decimal rate)
	{
		DepositCalculatedInterestAmountParams postBody = new DepositCalculatedInterestAmountParams
		{
			DateFrom = dateBegin,
			DateTo = dateEnd,
			Amount = amount,
			Currency = currency,
			Capitalize = capitalized,
			FloatingRateCode = floatingRateCode,
			PayPeriod = payPeriod,
			PassOverDirection = passOverDirection,
			PassOverType = passOverType,
			RateDivisor = rateDiv,
			Rate = rate
		};
		return client.Post<DepositCalculatedInterestAmount>("depositCalculatedInterestAmount", requireAuthorization: true, null, null, postBody);
	}

	public async Task<DepositCalculatedInterestAmount> GetDepositCalculatedInterestAmountAsync(DateTime dateBegin, DateTime dateEnd, decimal amount, string currency, bool capitalized, string floatingRateCode, InterestPayPeriod payPeriod, string passOverDirection, string passOverType, int rateDiv, decimal rate)
	{
		DepositCalculatedInterestAmountParams postBody = new DepositCalculatedInterestAmountParams
		{
			DateFrom = dateBegin,
			DateTo = dateEnd,
			Amount = amount,
			Currency = currency,
			Capitalize = capitalized,
			FloatingRateCode = floatingRateCode,
			PayPeriod = payPeriod,
			PassOverDirection = passOverDirection,
			PassOverType = passOverType,
			RateDivisor = rateDiv,
			Rate = rate
		};
		return await client.PostAsync<DepositCalculatedInterestAmount>("depositCalculatedInterestAmount", requireAuthorization: true, null, null, postBody);
	}
}
