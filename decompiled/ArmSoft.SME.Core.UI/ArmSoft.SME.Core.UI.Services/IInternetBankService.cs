using System;
using System.Collections.Generic;
using ArmSoft.InternetBank.APIClient;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;
using ArmSoft.SME.Core.UI.InternetBank;
using ArmSoft.SME.Core.UI.SmartEnums;

namespace ArmSoft.SME.Core.UI.Services;

public interface IInternetBankService
{
	Dictionary<string, string> BanksTokens { get; }

	Dictionary<string, ArmSoft.InternetBank.APIClient.Client> BanksClients { get; }

	string EnsureLoggedIn(string accountNumber);

	string EnsureLoggedIn(IBBankSE bank);

	InternetBankLoginResponse Login(InternetBankLoginRequest request);

	ArmSoft.InternetBank.APIClient.Client GetApiClientWithLogin(string accountNumber);

	List<InternetBankTransaction> GetTransactions(DateTime startDate, DateTime endDate, List<string> accountList, TransactionFilterOption transactionFilterOption = TransactionFilterOption.All);

	List<MerchantTransactionRow> GetMerchantTransactions(DateTime startDate, DateTime endDate, string account);

	List<ArmSoft.InternetBank.Web.API.Common.Models.Account> GetAccounts(string accountNumber, bool includeBalances);

	ArmSoft.InternetBank.Web.API.Common.Models.AccountDetails GetAccountDetails(string accountNumber);

	AccountBalanceRow GetAccountBalances(string accountNumber, DateTime openingBalanceDate, DateTime? closingBalanceDate = null);

	void CreatePayment(IBPayment payOrderDoc);

	void CreateCurrencyConversion(IBCurrConversion currConversionDoc);

	void CreateMultiplePayment(IBWGPayment wgPaymentDoc);

	IBPayment GetPayment(string accountNumber, Guid docId, string receiverAccount, bool throwNotExists = false);

	IBWGPayment GetMultiplePayment(string accountNumber, Guid docId, bool throwNotExists = false);

	IBCurrConversion GetCurrecnyConversion(string accountNumber, Guid docId, bool throwNotExists = false);

	IBCardTransfer GetCardTransfer(string accountNumber, Guid docId, bool throwNotExists = false);

	T GetPayment<T>(string accountNumber, Guid docId, bool throwNotExists = false) where T : Payment, new();

	List<Payment> GetPayments(DateTime startDate, DateTime endDate, string accountNumber);

	void CreatePayment<T>(T payment, bool ignoreWarnings = false, bool savePayment = true, bool saveTemplate = false, bool savePayee = false) where T : AccountPayment, new();

	T GetNewPayment<T>(string accountNumber, Guid? docId = null, bool throwNotExists = false) where T : Payment, new();

	List<Card> GetCards(string bank);

	List<Application> GetApplications(string bank, DateTime startDate, DateTime endDate);

	List<Bond> GetBonds(string bank);

	List<Credit> GetCredits(string bank);

	List<Deposit> GetDeposits(string bank);

	List<PurchasedSecurity> GetPurchasedSecurities(string bankCode);

	Challenge GetSignatureOptions(string bankCode, List<Guid> paymentIds);

	string SignPayments(string bankCode, List<Guid> paymentIds, string securityProviderChallenge = null, string securityProviderResponse = null);

	void RemovePayments(string bankCode, List<Guid> paymentIds);

	void CancelPayment(string bankCode, Guid targetId, string targetNum, string targetType, DateTime targetDate, decimal targetAmount, string detail);

	List<DocumentHistoryRow> GetPaymentHistory(string bankCode, Guid docISN);

	ClientRate GetClientRate(string account, string currency1, string currency2);

	void Dispose();
}
