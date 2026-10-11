using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.OnlineBanking.Common.Enums;
using ArmSoft.OpenBanking.Api.Common;

namespace ArmSoft.OpenBanking.Api.Client;

public class AccountManager(ApiClient client) : BaseManager(client)
{
	public async Task<IEnumerable<AccountListItem>> GetAccountListItems(bool includeBalances, bool includeCardAccounts, string? currency, bool onlyCardAccounts = false, bool onlyActive = true, AccountAccessLevels desiredAccess = AccountAccessLevels.View, DateTime? balanceDate = null)
	{
		AccountListItemParams postBody = new AccountListItemParams
		{
			OnlyActive = onlyActive,
			IncludeBalances = includeBalances,
			IncludeCardAccounts = includeCardAccounts,
			OnlyCardAccounts = onlyCardAccounts,
			Currency = currency,
			DesiredAccess = desiredAccess,
			BalanceDate = balanceDate
		};
		return await client.Post<IEnumerable<AccountListItem>>("accounts/v1/get-list", requireAuthorization: true, null, postBody);
	}

	public async Task<IEnumerable<AccountTransaction>> GetAccountTransactions(string accountNumber, bool showAMD, int? count, TransactionFilterOption filterOption, DateTime dateFrom, DateTime dateTo)
	{
		AccountTransactionByNumberParams postBody = new AccountTransactionByNumberParams
		{
			AccountNumber = accountNumber,
			ShowAMD = showAMD,
			Count = count,
			FilterOption = filterOption,
			DateFrom = dateFrom,
			DateTo = dateTo
		};
		return await client.Post<IEnumerable<AccountTransaction>>("accounts/v1/get-transactions-by-number", requireAuthorization: true, null, postBody);
	}

	public async Task<AccountLatestTransactions> GetAccountLatestTransactions(string accountNumber, bool showAMD, int count, TransactionFilterOption filterOption)
	{
		AccountLatestTransactionsByNumberParams postBody = new AccountLatestTransactionsByNumberParams
		{
			AccountNumber = accountNumber,
			ShowAMD = showAMD,
			Count = count,
			FilterOption = filterOption
		};
		return await client.Post<AccountLatestTransactions>("accounts/v1/get-latest-transactions-by-number", requireAuthorization: true, null, postBody);
	}

	public async Task<Account> GetAccount(string number)
	{
		AccountByNumberParams postBody = new AccountByNumberParams
		{
			Number = number
		};
		return await client.Post<Account>("accounts/v1/get-by-number", requireAuthorization: true, null, postBody);
	}

	public async Task<AccountBalance> GetAccountBalancesByDate(string accountNumber, DateTime dateFrom, DateTime? dateTo = null)
	{
		GetAccountBalancesByDateParams postBody = new GetAccountBalancesByDateParams
		{
			Number = accountNumber,
			DateFrom = dateFrom,
			DateTo = dateTo
		};
		return await client.Post<AccountBalance>("accounts/v1/get-balance-by-date", requireAuthorization: true, null, postBody);
	}

	public async Task UpdateNote(int id, string note)
	{
		UpdateAccountNoteParams postBody = new UpdateAccountNoteParams
		{
			Id = id,
			Note = note
		};
		await client.Post<object>("accounts/v1/update-note", requireAuthorization: true, null, postBody);
	}
}
