using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.APIClient;

public class AccountManager : BaseManager
{
	internal AccountManager(Client client)
		: base(client)
	{
	}

	public List<AccountTransaction> GetAccountTransactions(string accountNumber, DateTime? dateFrom = null, DateTime? dateTo = null, TransactionFilterOption filterOption = TransactionFilterOption.All)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["DateFrom"] = dateFrom,
			["DateTo"] = dateTo,
			["Period"] = Period.Custom,
			["ShowAMD"] = true,
			["FilterOption"] = filterOption
		};
		if (Version.Parse(client.ParamManager.IBVersion).CompareTo(new Version(4, 35, 0)) < 0)
		{
			List<AccountTransaction> list = new List<AccountTransaction>();
			{
				foreach (AccountTransactionOldModel item in client.Get<List<AccountTransactionOldModel>>($"accounts/{accountNumber}/transactions", requireAuthorization: true, null, queryParams))
				{
					list.Add(new AccountTransaction
					{
						CreationDate = item.CreationDate,
						OperationDate = item.OperationDate,
						DebitAmount = item.DebitAmount,
						DebitAmountAMD = item.DebitAmountAMD,
						CreditAmount = item.CreditAmount,
						CreditAmountAMD = item.CreditAmountAMD,
						Currency = item.Currency,
						OperationType = item.OperationType,
						Comment = item.Comment,
						CorrespondentAccount = item.CorrespondentAccount,
						CorrespondentName = item.CorrespondentName,
						DocumentNumber = item.DocumentNumber,
						TransactionID = item.TransactionID,
						DocumentID = item.DocumentID
					});
				}
				return list;
			}
		}
		return client.Get<List<AccountTransaction>>($"accounts/{accountNumber}/transactions", requireAuthorization: true, null, queryParams);
	}

	public async Task<List<AccountTransaction>> GetAccountTransactionsAsync(string accountNumber, DateTime? dateFrom = null, DateTime? dateTo = null, TransactionFilterOption filterOption = TransactionFilterOption.All)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["DateFrom"] = dateFrom,
			["DateTo"] = dateTo,
			["Period"] = Period.Custom,
			["ShowAMD"] = true,
			["FilterOption"] = filterOption
		};
		if (Version.Parse(client.ParamManager.IBVersion).CompareTo(new Version(4, 35, 0)) < 0)
		{
			List<AccountTransaction> result = new List<AccountTransaction>();
			foreach (AccountTransactionOldModel item in await client.GetAsync<List<AccountTransactionOldModel>>($"accounts/{accountNumber}/transactions", requireAuthorization: true, null, queryParams))
			{
				result.Add(new AccountTransaction
				{
					CreationDate = item.CreationDate,
					OperationDate = item.OperationDate,
					DebitAmount = item.DebitAmount,
					DebitAmountAMD = item.DebitAmountAMD,
					CreditAmount = item.CreditAmount,
					CreditAmountAMD = item.CreditAmountAMD,
					Currency = item.Currency,
					OperationType = item.OperationType,
					Comment = item.Comment,
					CorrespondentAccount = item.CorrespondentAccount,
					CorrespondentName = item.CorrespondentName,
					DocumentNumber = item.DocumentNumber,
					TransactionID = item.TransactionID,
					DocumentID = item.DocumentID
				});
			}
			return result;
		}
		return await client.GetAsync<List<AccountTransaction>>($"accounts/{accountNumber}/transactions", requireAuthorization: true, null, queryParams);
	}

	public async Task<AccountLatestTransactions> GetAccountLatestTransactions(int accountId, bool showAMD, int count, TransactionFilterOption filterOption)
	{
		Dictionary<string, object> postBody = new Dictionary<string, object>
		{
			["AccountId"] = accountId,
			["ShowAMD"] = showAMD,
			["Count"] = count,
			["FilterOption"] = filterOption
		};
		return await client.PostAsync<AccountLatestTransactions>("accounts/latestTransactions", requireAuthorization: true, null, null, postBody);
	}

	public AccountBalances GetAccountBalances(string accountNumber, DateTime openingBalanceDate, DateTime? closingBalanceDate = null)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["DateFrom"] = openingBalanceDate,
			["DateTo"] = closingBalanceDate
		};
		return client.Get<AccountBalances>($"accounts/{accountNumber}/balances", requireAuthorization: true, null, queryParams);
	}

	public AccountDetails GetAccountDeatils(string accountNumber)
	{
		return client.Get<AccountDetails>($"accounts/{accountNumber}/details", requireAuthorization: true);
	}

	public async Task<AccountDetails> GetAccountDeatilsAsync(string accountNumber)
	{
		return await client.GetAsync<AccountDetails>($"accounts/{accountNumber}/details", requireAuthorization: true);
	}

	public List<Account> GetAccounts(string currency = null, int? subuserId = null, bool includeCardAccounts = true, bool includeBalances = false)
	{
		if (Version.Parse(client.ParamManager.IBVersion).CompareTo(new Version(8, 6, 0)) < 0)
		{
			Dictionary<string, object> queryParams = new Dictionary<string, object>
			{
				["Currency"] = currency,
				["IncludeLinks"] = false
			};
			return client.Get<List<Account>>("accounts", requireAuthorization: true, null, queryParams);
		}
		AccountParams postBody = new AccountParams
		{
			SubUserId = subuserId,
			Currency = currency,
			IncludeBalances = includeBalances,
			IncludeCardAccounts = includeCardAccounts
		};
		return client.Post<List<Account>>("accounts", requireAuthorization: true, null, null, postBody);
	}

	public async Task<List<Account>> GetAccountsAsync(string currency = null, int? subuserId = null, bool includeCardAccounts = true, bool includeBalances = false)
	{
		if (Version.Parse(client.ParamManager.IBVersion).CompareTo(new Version(8, 6, 0)) < 0)
		{
			Dictionary<string, object> queryParams = new Dictionary<string, object>
			{
				["Currency"] = currency,
				["IncludeLinks"] = false
			};
			return await client.GetAsync<List<Account>>("accounts", requireAuthorization: true, null, queryParams);
		}
		AccountParams postBody = new AccountParams
		{
			SubUserId = subuserId,
			Currency = currency,
			IncludeBalances = includeBalances,
			IncludeCardAccounts = includeCardAccounts
		};
		return await client.PostAsync<List<Account>>("accounts", requireAuthorization: true, null, null, postBody);
	}

	public string GetTreasuryAccountOwnerName(string accountNumber)
	{
		return client.Get("accounts/" + accountNumber + "/ownerName", requireAuthorization: true);
	}

	public async Task<string> GetTreasuryAccountOwnerNameAsync(string accountNumber)
	{
		return await client.GetAsync("accounts/" + accountNumber + "/ownerName", requireAuthorization: true);
	}

	public async Task<AccountInfo> GetAccountInfoAsync(string accountNumber)
	{
		return await client.GetAsync<AccountInfo>("accounts/" + accountNumber + "/info", requireAuthorization: true);
	}

	public void UpdateAccountNote(string accountNumber, string note)
	{
		client.Post("accounts/" + accountNumber + "/updateNote", requireAuthorization: true, null, null, note);
	}

	public Task UpdateAccountNoteAsync(string accountNumber, string note)
	{
		return client.PostAsync("accounts/" + accountNumber + "/updateNote", requireAuthorization: true, null, null, note);
	}

	public async Task<string> GetAccountCurrencyAsync(string accountNumber)
	{
		return await client.GetAsync("accounts/" + accountNumber + "/currency", requireAuthorization: true);
	}

	public async Task<List<PayerAccountData>> GetPayerAccountsAsync(string currency = null, bool includeAccountBalances = false, bool includeCardOnlineBalances = false, bool isForCharge = false, string documentType = null, bool isForPercentAccount = false, AccountAccessLevels accessLevels = AccountAccessLevels.MakeOperation)
	{
		Dictionary<string, object> postBody = new Dictionary<string, object>
		{
			["Currency"] = currency,
			["IncludeAccountBalances"] = includeAccountBalances,
			["IncludeCardOnlineBalances"] = includeCardOnlineBalances,
			["IsForCharge"] = isForCharge,
			["DocumentType"] = documentType,
			["IsForPercentAccount"] = isForPercentAccount,
			["AccessLevel"] = accessLevels
		};
		return await client.PostAsync<List<PayerAccountData>>("accounts/payerAccountDatas", requireAuthorization: true, null, null, postBody);
	}

	public async Task<List<AccountBalance>> GetAccountBalancesAsync(List<string> accountNumbers)
	{
		Dictionary<string, object> postBody = new Dictionary<string, object> { ["AccountNumbers"] = accountNumbers };
		return await client.PostAsync<List<AccountBalance>>("accounts/balances", requireAuthorization: true, null, null, postBody);
	}

	public async Task<AccountBalances> GetAccountBalancesByDateAsync(string accountNumber, DateTime dateFrom, DateTime? dateTo)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["DateFrom"] = dateFrom,
			["DateTo"] = dateTo
		};
		return await client.GetAsync<AccountBalances>(string.Format("accounts/" + accountNumber + "/balances", accountNumber), requireAuthorization: true, null, queryParams);
	}

	public async Task<List<UserAccountIndex>> UpdateIndexesAsync(AccountsIndexesParams indexesParams)
	{
		return await client.PostAsync<List<UserAccountIndex>>("accounts/updateIndexes", requireAuthorization: true, null, null, indexesParams);
	}
}
