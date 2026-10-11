using System;
using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;
using ArmSoft.OpenBanking.Api.Client;
using ArmSoft.OpenBanking.Api.Common;
using ArmSoft.SME.Core.UI.InternetBank;

namespace ArmSoft.SME.Core.UI.Services;

public interface IOpenBankingService
{
	Dictionary<string, ApiClient> BanksClients { get; }

	void Dispose();

	bool Login(string bankCode);

	List<OBBankInfo> GetSupportedBanks();

	ArmSoft.OpenBanking.Api.Common.Account GetAccount(string accountNumber);

	List<AccountListItem> GetAccounts(string bankCode, bool includeBalances);

	AccountBalanceRow GetAccountBalances(string accountNumber, DateTime openingBalanceDate, DateTime? closingBalanceDate = null);

	T GetNewPayment<T>(string accountNumber) where T : DocumentForEdit<LocalPayment>, new();

	T GetPayment<T>(string accountNumber, Guid docId, bool throwNotExists = false) where T : DocumentForEdit<LocalPayment>, new();

	IBPayment GetPayment(string accountNumber, Guid docId, string receiverAccount, bool throwNotExists = false);

	T GetPaymentDetails<T>(string bankCode, Guid docId, bool throwNotExists = false) where T : DocumentDetails<LocalPayment>, new();

	void CreatePayment(IBPayment payOrderDoc);

	void CreatePayment<T>(T payment, bool ignoreWarnings = false, bool savePayment = true, bool saveTemplate = false) where T : LocalPayment, new();

	T GetNewBudgetPayment<T>(string accountNumber) where T : DocumentForEdit<BudgetaryPayment>, new();

	T GetBudgetPayment<T>(string accountNumber, Guid docId, bool throwNotExists = false) where T : DocumentForEdit<BudgetaryPayment>, new();

	T GetBudgetPaymentDetails<T>(string bankCode, Guid docId, bool throwNotExists = false) where T : DocumentDetails<BudgetaryPayment>, new();

	void CreateBudgetPayment<T>(T payment, bool ignoreWarnings = false, bool savePayment = true, bool saveTemplate = false) where T : BudgetaryPayment, new();

	List<DocumentListItem> GetPayments(DateTime startDate, DateTime endDate, string accountNumber);

	T GetNewMultiplePayment<T>(string accountNumber) where T : DocumentForEdit<MultiplePayment>, new();

	T GetMultiplePayment<T>(string accountNumber, Guid docId, bool throwNotExists = false) where T : DocumentForEdit<MultiplePayment>, new();

	IBWGPayment GetMultiplePayment(string accountNumber, Guid docId, bool throwNotExists = false);

	T GetMultiplePaymentDetails<T>(string bankCode, Guid docId, bool throwNotExists = false) where T : DocumentDetails<MultiplePayment>, new();

	void CreateMultiplePayment(IBWGPayment wgPayment);

	void CreateMultiplePayment<T>(T payment, bool ignoreWarnings = false, bool savePayment = true, bool saveTemplate = false) where T : MultiplePayment, new();

	void CreateCurrencyConversion(IBCurrConversion currConversionDoc);

	IBCurrConversion GetCurrencyConversion(string accountNumber, Guid docId, bool throwNotExists = false);

	IBCardTransfer GetCardTransfer(string accountNumber, Guid docId, bool throwNotExists = false);

	T GetCardTransfer<T>(string accountNumber, Guid docId, bool throwNotExists = false) where T : DocumentForEdit<CardTransfer>, new();

	List<Card> GetCards(string bankCode);

	List<DocumentListItem> GetApplications(string bankCode, DateTime startDate, DateTime endDate);

	List<Bond> GetBonds(string bankCode);

	ContractOnlineDataListResult<BondOnlineData> GetBondsOnlineDataList(string bankCode, List<int> ids);

	List<Credit> GetCredits(string bankCode);

	ContractOnlineDataListResult<CreditOnlineData> GetCreditsOnlineDataList(string bankCode, List<int> ids);

	List<Deposit> GetDeposits(string bankCode);

	ContractOnlineDataListResult<DepositOnlineData> GetDepositsOnlineDataList(string bankCode, List<int> ids);

	List<PurchasedSecurity> GetPurchasedSecurities(string bank);

	List<AccountTransaction> GetTransactions(DateTime startDate, DateTime endDate, string accountNumber, TransactionFilterOption transactionFilterOption = TransactionFilterOption.All);

	List<MerchantTransaction> GetMerchantTransactions(DateTime startDate, DateTime endDate, string accountNumber);

	Rate GetClientRate(string bankCode, string currency1, string currency2);

	List<DocumentHistoryRow> GetPaymentHistory(string bankCode, Guid docId);

	SignatureOptions GetSignatureOptions(string bankCode, List<Guid> paymentIds);

	SignedDocumentsResult SignPayments(string bankCode, List<Guid> paymentIds, string securityProviderChallenge, string signature);

	void RemovePayments(string bankCode, List<Guid> paymentIds);

	void CancelPayment(string bankCode, byte state, Guid targetId, string detail);

	void CancelLoacalPayment(string bankCode, Guid targetId, string detail);

	void CancelBudgetPayment(string bankCode, Guid targetId, string detail);

	void CancelMultiplePayment(string bankCode, Guid targetId, string detail);
}
