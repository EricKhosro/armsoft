using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Ardalis.SmartEnum;
using ArmSoft.InternetBank.APIClient;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.InternetBank;
using ArmSoft.SME.Core.UI.SmartEnums;

namespace ArmSoft.SME.Core.UI.Services;

public class InternetBankService : IInternetBankService
{
	public Dictionary<string, ArmSoft.InternetBank.APIClient.Client> BanksClients { get; } = new Dictionary<string, ArmSoft.InternetBank.APIClient.Client>();

	public Dictionary<string, string> BanksTokens { get; } = new Dictionary<string, string>();

	public void Dispose()
	{
		BanksClients.Clear();
		BanksTokens.Clear();
	}

	public string EnsureLoggedIn(string accountNumber)
	{
		if (!SmartEnum<IBBankSE, string>.TryFromValue(SettlementAccountHelper.GetBankMainCode(accountNumber), out var result))
		{
			throw new ASException(string.Format(Resources.CouldNotLoginToIBWithAccount, accountNumber));
		}
		return EnsureLoggedIn(result);
	}

	public string EnsureLoggedIn(IBBankSE bank)
	{
		bool flag = false;
		if (BanksTokens.TryGetValue(bank.Value, out var value))
		{
			InternetBankResponseTypes responseType = EnsureLoggedIn(bank.Value, value).ResponseType;
			flag = responseType == InternetBankResponseTypes.Succeeded || responseType == InternetBankResponseTypes.LoggedIn;
		}
		if (!flag)
		{
			Cursor overrideCursor = Mouse.OverrideCursor;
			Mouse.OverrideCursor = null;
			InternetBankLoginRequest request = new InternetBankLoginRequest
			{
				BankCode = bank.Value
			};
			InternetBankLoginDialog internetBankLoginDialog = new InternetBankLoginDialog(bank, request);
			if (internetBankLoginDialog.ShowDialog().Value)
			{
				Mouse.OverrideCursor = overrideCursor;
				Settings.HostingEnvironment.RefreshStatusBarBankStatuses();
				return internetBankLoginDialog.LoginToken;
			}
			return "";
		}
		return value;
	}

	private InternetBankLoginResponse EnsureLoggedIn(string bankCode, string token)
	{
		InternetBankLoginResponse internetBankLoginResponse = new InternetBankLoginResponse();
		if (string.IsNullOrWhiteSpace(token))
		{
			internetBankLoginResponse.ResponseType = InternetBankResponseTypes.NeedToLogin;
		}
		else
		{
			GetApiClient(bankCode).Login(token);
			internetBankLoginResponse.ResponseType = InternetBankResponseTypes.LoggedIn;
		}
		return internetBankLoginResponse;
	}

	public InternetBankLoginResponse Login(InternetBankLoginRequest request)
	{
		InternetBankLoginResponse internetBankLoginResponse = EnsureLoggedIn(request.BankCode, request.Token);
		if (internetBankLoginResponse.ResponseType == InternetBankResponseTypes.NeedToLogin)
		{
			try
			{
				string text = GetApiClient(request.BankCode).Login(request.User, request.Password, request.Challenge ?? "", request.SecurityKey);
				internetBankLoginResponse = new InternetBankLoginResponse
				{
					Token = text,
					ResponseType = InternetBankResponseTypes.Succeeded
				};
				BanksTokens.Add(request.BankCode, text);
			}
			catch (SecureAuthenticationRequiredException ex)
			{
				internetBankLoginResponse = new InternetBankLoginResponse
				{
					ResponseType = InternetBankResponseTypes.SecureAuthenticationRequired,
					Message = ex.Description
				};
			}
		}
		return internetBankLoginResponse;
	}

	public ArmSoft.InternetBank.APIClient.Client GetApiClientWithLogin(string accountNumber)
	{
		if (!SmartEnum<IBBankSE, string>.TryFromValue(SettlementAccountHelper.GetBankMainCode(accountNumber), out var result))
		{
			throw new ASException(string.Format(Resources.CouldNotLoginToIBWithAccount, accountNumber));
		}
		string text = EnsureLoggedIn(result);
		if (string.IsNullOrWhiteSpace(text))
		{
			throw new InvisibleException(string.Format(Resources.IBNotLoggedIn, result.Caption));
		}
		InternetBankLoginResponse internetBankLoginResponse = EnsureLoggedIn(result, text);
		if (internetBankLoginResponse.ResponseType == InternetBankResponseTypes.LoggedIn)
		{
			return GetApiClient(result.Value);
		}
		if (string.IsNullOrWhiteSpace(internetBankLoginResponse.Message))
		{
			throw new ASException(internetBankLoginResponse.Message);
		}
		throw new ASException(Resources.IBAccessDenied);
	}

	private ArmSoft.InternetBank.APIClient.Client GetApiClient(string bankCode)
	{
		if (!BanksClients.TryGetValue(bankCode, out var value))
		{
			IBBankSE iBBankSE = SmartEnum<IBBankSE, string>.FromValue(bankCode);
			if (iBBankSE == IBBankSE.ArmSoftDemo777)
			{
				value = new ArmSoft.InternetBank.APIClient.Client(ConfigHelper.IBDemoSettings.Service777URL, ConfigHelper.IBDemoSettings.Credential, Settings.ProgramInnerName, 90000, LanguageService.CurrentUICultureName);
			}
			else
			{
				value = ((!(iBBankSE == IBBankSE.ArmSoftDemo888)) ? new ArmSoft.InternetBank.APIClient.Client(iBBankSE.IBBankId, GetClientSystemType(), Settings.ProgramInnerName, 90000, LanguageService.CurrentUICultureName) : new ArmSoft.InternetBank.APIClient.Client(ConfigHelper.IBDemoSettings.Service888URL, ConfigHelper.IBDemoSettings.Credential, Settings.ProgramInnerName, 90000, LanguageService.CurrentUICultureName));
			}
			BanksClients.Add(bankCode, value);
		}
		return value;
	}

	private static ClientSystemType GetClientSystemType()
	{
		return Settings.SystemType switch
		{
			SystemTypes.Accountant => ClientSystemType.ASAccountant, 
			SystemTypes.Trade => ClientSystemType.ASTrade, 
			SystemTypes.SalesManagement => ClientSystemType.ASSalesManagement, 
			_ => ClientSystemType.NotSet, 
		};
	}

	public List<InternetBankTransaction> GetTransactions(DateTime startDate, DateTime endDate, List<string> accountList, TransactionFilterOption transactionFilterOption = TransactionFilterOption.All)
	{
		List<InternetBankTransaction> list = new List<InternetBankTransaction>();
		foreach (string account in accountList)
		{
			ArmSoft.InternetBank.APIClient.Client apiClientWithLogin = GetApiClientWithLogin(account);
			List<AccountTransaction> list2 = new List<AccountTransaction>();
			try
			{
				list2 = apiClientWithLogin.AccountManager.GetAccountTransactions(account, startDate, endDate, transactionFilterOption);
			}
			catch (NotFoundException)
			{
			}
			if (list2 == null)
			{
				continue;
			}
			foreach (AccountTransaction item in list2)
			{
				list.Add(new InternetBankTransaction
				{
					Account = account,
					CreationDate = item.CreationDate,
					OperationDate = item.OperationDate,
					DebitAmountCur = item.CreditAmount,
					DebitAmount = item.CreditAmountAMD,
					CreditAmountCur = item.DebitAmount,
					CreditAmount = item.DebitAmountAMD,
					Currency = item.Currency,
					OperationType = item.OperationType,
					Comment = item.Comment,
					CorrespondentAccount = item.CorrespondentAccount,
					CorrespondentName = item.CorrespondentName,
					DocumentNumber = item.DocumentNumber,
					TransactionID = item.TransactionID,
					TransactionShortID = item.TransactionID.Substring(0, item.TransactionID.Length - 2),
					DocumentID = item.DocumentID,
					DocumentIDString = item.DocumentIDString
				});
			}
		}
		return list;
	}

	public List<MerchantTransactionRow> GetMerchantTransactions(DateTime startDate, DateTime endDate, string account)
	{
		List<MerchantTransactionRow> list = new List<MerchantTransactionRow>();
		ArmSoft.InternetBank.APIClient.Client apiClientWithLogin = GetApiClientWithLogin(account);
		List<string> merchantIds = new List<string>();
		List<string> terminalIds = new List<string>();
		try
		{
			foreach (MerchantTransaction transaction in apiClientWithLogin.MerchantManager.GetTransactions(startDate, endDate, merchantIds, terminalIds, account))
			{
				list.Add(new MerchantTransactionRow
				{
					OperationDate = transaction.OperationDate.ToDateOnly(),
					TransactionDate = (DateTime.TryParse(transaction.TransactionDate, out var result) ? new DateTime?(result) : ((DateTime?)null)),
					TransactionType = transaction.TransactionDirection,
					Currency = transaction.Currency,
					Amount = transaction.Amount,
					MerchantFee = transaction.MerchantFee,
					AmountWithoutMerchantFee = transaction.AmountWithoutMerchantFee,
					MerchantName = transaction.MerchantName,
					CardNumber = transaction.CardNumber,
					CardTypeName = transaction.CardTypeName,
					CardBelongingName = transaction.CardBelongingName,
					RRN = transaction.TransactionReferenceNumber,
					TokenPan = transaction.TokenPan,
					TerminalId = transaction.TerminalId,
					AuthorizationCode = transaction.AuthorizationCode
				});
			}
		}
		catch (Exception ex)
		{
			if (ex is UnauthorizedException || ex is ApiValidationException)
			{
				throw new ASException(Resources.IBAccessDenied);
			}
			if (ex is NotFoundException)
			{
				throw new ASException(Resources.MerchantAccountNotExist);
			}
			Settings.ErrorHandler(ex);
		}
		return list;
	}

	List<ArmSoft.InternetBank.Web.API.Common.Models.Account> IInternetBankService.GetAccounts(string accountNumber, bool includeBalances)
	{
		AccountManager accountManager = GetApiClientWithLogin(accountNumber).AccountManager;
		bool includeBalances2 = includeBalances;
		return accountManager.GetAccounts(null, null, includeCardAccounts: true, includeBalances2);
	}

	ArmSoft.InternetBank.Web.API.Common.Models.AccountDetails IInternetBankService.GetAccountDetails(string accountNumber)
	{
		return GetApiClientWithLogin(accountNumber).AccountManager.GetAccountDeatils(accountNumber);
	}

	AccountBalanceRow IInternetBankService.GetAccountBalances(string accountNumber, DateTime openingBalanceDate, DateTime? closingBalanceDate)
	{
		ArmSoft.InternetBank.Web.API.Common.Models.AccountBalances accountBalances = GetApiClientWithLogin(accountNumber).AccountManager.GetAccountBalances(accountNumber, openingBalanceDate, closingBalanceDate);
		return new AccountBalanceRow
		{
			OpeningBalance = accountBalances.OpeningBalance,
			ClosingBalance = accountBalances.ClosingBalance,
			OpeningExpectedBalance = accountBalances.OpeningExpectedBalance,
			ClosingExpectedBalance = accountBalances.ClosingExpectedBalance
		};
	}

	public IBPayment GetPayment(string accountNumber, Guid docId, string receiverAccount, bool throwNotExists = false)
	{
		IBPayment result = null;
		if (SettlementAccountHelper.Is_Budget_Account(receiverAccount))
		{
			BP payment = GetPayment<BP>(accountNumber, docId, throwNotExists);
			if (payment != null)
			{
				result = new IBPayment
				{
					Amount = payment.Amount,
					Currency = payment.Currency,
					Details = payment.Details,
					Date = payment.Date,
					DocId = payment.DocId,
					DocNum = payment.DocNum,
					DocState = (byte)payment.State,
					PayerAcc = payment.PayerAcc,
					ReceiverAcc = payment.ReceiverAcc,
					PayerTaxCode = payment.TaxCode,
					TaxAuthorityCode = payment.TaxAuthorityCode,
					ReceiverName = payment.Receiver,
					IsManuallyEntered = payment.IsManuallyEntered,
					IsThirdParty = payment.IsThirdParty,
					ThirdPartyResidenceType = payment.ThirdPartyResidenceType,
					ThirdPartyLegalState = payment.ThirdPartyLegalState,
					ThirdPartyTaxCode = payment.ThirdPartyTaxCode,
					ThirdPartySocialCard = payment.ThirdPartySocialCard,
					ThirdPartyName = payment.ThirdPartyName,
					ThirdPartyPassportType = payment.ThirdPartyPassportType,
					ThirdPartyPassportNo = payment.ThirdPartyPassportNo,
					ThirdPartyAddress = payment.ThirdPartyAddress,
					ThirdPartyAdditionalInfo = payment.ThirdPartyAddInfo
				};
			}
		}
		else
		{
			PO payment2 = GetPayment<PO>(accountNumber, docId, throwNotExists);
			if (payment2 != null)
			{
				result = new IBPayment
				{
					Amount = payment2.Amount,
					Currency = payment2.Currency,
					Details = payment2.Details,
					Date = payment2.Date,
					DocId = payment2.DocId,
					DocNum = payment2.DocNum,
					DocState = (byte)payment2.State,
					PayerAcc = payment2.PayerAcc,
					ReceiverAcc = payment2.ReceiverAcc,
					PayerTaxCode = payment2.TaxCode,
					ReceiverName = payment2.Receiver,
					ReceiverLegalStatus = payment2.ReceiverLegalStatus,
					IsManuallyEntered = payment2.IsManuallyEntered
				};
			}
		}
		return result;
	}

	public IBWGPayment GetMultiplePayment(string accountNumber, Guid docId, bool throwNotExists = false)
	{
		IBWGPayment iBWGPayment = new IBWGPayment();
		MP payment = GetPayment<MP>(accountNumber, docId, throwNotExists);
		if (payment != null)
		{
			iBWGPayment.Amount = payment.Amount;
			iBWGPayment.Currency = payment.Currency;
			iBWGPayment.Details = payment.Details;
			iBWGPayment.Date = payment.Date;
			iBWGPayment.DocId = payment.DocId;
			iBWGPayment.DocNum = payment.DocNum;
			iBWGPayment.PayerAcc = payment.PayerAccOrCard;
			iBWGPayment.SocialCard = payment.SocialCard;
			iBWGPayment.TaxCode = payment.TaxCode;
			iBWGPayment.IsManuallyEntered = payment.IsManuallyEntered;
			iBWGPayment.DocState = (byte)payment.State;
			foreach (MPRow row in payment.Rows)
			{
				iBWGPayment.Rows.Add(new IBWGPaymentRow
				{
					Amount = row.Amount,
					ReceiverAcc = row.ReceiverAccount,
					Receiver = row.Receiver
				});
			}
		}
		return iBWGPayment;
	}

	public IBCurrConversion GetCurrecnyConversion(string accountNumber, Guid docId, bool throwNotExists = false)
	{
		IBCurrConversion iBCurrConversion = new IBCurrConversion();
		CC newPayment = GetNewPayment<CC>(accountNumber, docId, throwNotExists);
		if (newPayment != null)
		{
			iBCurrConversion.Amount = newPayment.Amount;
			iBCurrConversion.Currency = newPayment.Currency;
			iBCurrConversion.Details = newPayment.Details;
			iBCurrConversion.Date = newPayment.Date;
			iBCurrConversion.DocId = newPayment.DocId;
			iBCurrConversion.DocNum = newPayment.DocNum;
			iBCurrConversion.PayerAcc = newPayment.PayerAcc;
			iBCurrConversion.ReceiverAcc = newPayment.ReceiverAcc;
			iBCurrConversion.PayerTaxCode = newPayment.TaxCode;
			iBCurrConversion.ReceiverName = newPayment.Receiver;
			iBCurrConversion.RateCurrency1 = newPayment.RateCurrency1;
			iBCurrConversion.RateCurrency2 = newPayment.RateCurrency2;
			iBCurrConversion.RateFor = newPayment.RateFor;
			iBCurrConversion.RateValue = newPayment.RateValue;
			iBCurrConversion.CanBeDeleted = newPayment.CanBeDeleted;
			iBCurrConversion.CanBeSigned = newPayment.CanBeSigned;
			iBCurrConversion.FixedCurrency = newPayment.FixedCurrency;
			iBCurrConversion.AmountCredit = newPayment.AmountCredit;
			iBCurrConversion.CurrencyCredit = newPayment.CurrencyCredit;
			iBCurrConversion.IsManuallyEntered = newPayment.IsManuallyEntered;
		}
		return iBCurrConversion;
	}

	public IBCardTransfer GetCardTransfer(string accountNumber, Guid docId, bool throwNotExists = false)
	{
		IBCardTransfer iBCardTransfer = null;
		TC newPayment = GetNewPayment<TC>(accountNumber, docId, throwNotExists);
		if (newPayment != null)
		{
			iBCardTransfer.Amount = newPayment.Amount;
			iBCardTransfer.Currency = newPayment.Currency;
			iBCardTransfer.Details = newPayment.Details;
			iBCardTransfer.Date = newPayment.Date;
			iBCardTransfer.DocId = newPayment.DocId;
			iBCardTransfer.DocNum = newPayment.DocNum;
			iBCardTransfer.PayerAcc = newPayment.PayerAcc;
			iBCardTransfer.ReceiverCardNumber = newPayment.ReceiverCardNumber;
			iBCardTransfer.ReceiverCountryCode = newPayment.ReceiverCountryCode;
			iBCardTransfer.CommissionAmount = newPayment.CommissionAmount;
			iBCardTransfer.ChargeAcc = newPayment.ChargeAcc;
		}
		return iBCardTransfer;
	}

	public void CreatePayment(IBPayment payOrderDoc)
	{
		if (SettlementAccountHelper.Is_Budget_Account(payOrderDoc.ReceiverAcc))
		{
			CreateIBBudgetPayment(payOrderDoc);
		}
		else
		{
			CreateIBPayOrder(payOrderDoc);
		}
	}

	public void CreateCurrencyConversion(IBCurrConversion currConversionDoc)
	{
		CreateIBCurrencyConversion(currConversionDoc);
	}

	private void CreateIBPayOrder(IBPayment payOrderDoc)
	{
		InternetBankHelper.CheckPaymentForExport(payOrderDoc);
		PO newPayment = GetNewPayment<PO>(payOrderDoc.PayerAcc);
		newPayment.Amount = payOrderDoc.Amount;
		newPayment.Currency = payOrderDoc.Currency;
		newPayment.Details = payOrderDoc.Details;
		newPayment.Date = payOrderDoc.Date;
		newPayment.DocId = payOrderDoc.DocId;
		newPayment.DocNum = payOrderDoc.DocNum;
		newPayment.PayerAcc = payOrderDoc.PayerAcc;
		newPayment.ReceiverAcc = payOrderDoc.ReceiverAcc;
		newPayment.ReceiverLegalStatus = payOrderDoc.ReceiverLegalStatus;
		newPayment.TaxCode = payOrderDoc.PayerTaxCode;
		newPayment.Receiver = payOrderDoc.ReceiverName;
		newPayment.IsManuallyEntered = payOrderDoc.IsManuallyEntered;
		CreatePayment(newPayment, ignoreWarnings: true);
	}

	private void CreateIBBudgetPayment(IBPayment payOrderDoc)
	{
		InternetBankHelper.CheckPaymentForExport(payOrderDoc);
		BP newPayment = GetNewPayment<BP>(payOrderDoc.PayerAcc);
		newPayment.Amount = payOrderDoc.Amount;
		newPayment.Currency = payOrderDoc.Currency;
		newPayment.Details = payOrderDoc.Details;
		newPayment.Date = payOrderDoc.Date;
		newPayment.DocId = payOrderDoc.DocId;
		newPayment.DocNum = payOrderDoc.DocNum;
		newPayment.PayerAcc = payOrderDoc.PayerAcc;
		newPayment.ReceiverAcc = payOrderDoc.ReceiverAcc;
		newPayment.TaxCode = payOrderDoc.PayerTaxCode;
		newPayment.TaxAuthorityCode = payOrderDoc.TaxAuthorityCode;
		newPayment.Receiver = payOrderDoc.ReceiverName;
		newPayment.IsManuallyEntered = payOrderDoc.IsManuallyEntered;
		newPayment.IsThirdParty = payOrderDoc.IsThirdParty;
		newPayment.ThirdPartyResidenceType = payOrderDoc.ThirdPartyResidenceType;
		newPayment.ThirdPartyLegalState = payOrderDoc.ThirdPartyLegalState;
		newPayment.TaxAuthorityCode = payOrderDoc.TaxAuthorityCode;
		newPayment.ThirdPartyTaxCode = payOrderDoc.ThirdPartyTaxCode;
		newPayment.ThirdPartySocialCard = payOrderDoc.ThirdPartySocialCard;
		newPayment.ThirdPartyName = payOrderDoc.ThirdPartyName;
		newPayment.ThirdPartyPassportType = payOrderDoc.ThirdPartyPassportType;
		newPayment.ThirdPartyPassportNo = payOrderDoc.ThirdPartyPassportNo;
		newPayment.ThirdPartyAddress = payOrderDoc.ThirdPartyAddress;
		newPayment.ThirdPartyAddInfo = payOrderDoc.ThirdPartyAdditionalInfo;
		CreatePayment(newPayment, ignoreWarnings: true);
	}

	private void CreateIBCurrencyConversion(IBCurrConversion currConversionDoc)
	{
		CC newPayment = GetNewPayment<CC>(currConversionDoc.PayerAcc);
		newPayment.PaymentMode = PaymentMode.MyAccount;
		newPayment.Amount = currConversionDoc.Amount;
		newPayment.Currency = currConversionDoc.Currency;
		newPayment.Details = currConversionDoc.Details;
		newPayment.Date = currConversionDoc.Date;
		newPayment.DocId = currConversionDoc.DocId;
		newPayment.DocNum = currConversionDoc.DocNum;
		newPayment.PayerAcc = currConversionDoc.PayerAcc;
		newPayment.ReceiverAcc = currConversionDoc.ReceiverAcc;
		newPayment.TaxCode = currConversionDoc.PayerTaxCode;
		newPayment.Receiver = currConversionDoc.ReceiverName;
		newPayment.RateCurrency1 = currConversionDoc.RateCurrency1;
		newPayment.RateCurrency2 = currConversionDoc.RateCurrency2;
		newPayment.RateFor = currConversionDoc.RateFor;
		newPayment.RateValue = currConversionDoc.RateValue;
		newPayment.CanBeDeleted = currConversionDoc.CanBeDeleted;
		newPayment.CanBeSigned = currConversionDoc.CanBeSigned;
		newPayment.FixedCurrency = currConversionDoc.FixedCurrency;
		newPayment.AmountCredit = currConversionDoc.AmountCredit;
		newPayment.CurrencyCredit = currConversionDoc.CurrencyCredit;
		newPayment.IsManuallyEntered = currConversionDoc.IsManuallyEntered;
		CreatePayment(newPayment, ignoreWarnings: true);
	}

	public void CreateMultiplePayment(IBWGPayment wgPaymentDoc)
	{
		MP newPayment = GetNewPayment<MP>(wgPaymentDoc.PayerAcc, null, throwExceptionNotExits: true);
		newPayment.Amount = wgPaymentDoc.Amount;
		newPayment.Currency = wgPaymentDoc.Currency;
		newPayment.Details = wgPaymentDoc.Details;
		newPayment.Date = wgPaymentDoc.Date;
		newPayment.DocId = wgPaymentDoc.DocId;
		newPayment.DocNum = wgPaymentDoc.DocNum;
		newPayment.PayerAcc = wgPaymentDoc.PayerAcc;
		newPayment.SocialCard = wgPaymentDoc.SocialCard;
		newPayment.TaxCode = wgPaymentDoc.TaxCode;
		newPayment.IsManuallyEntered = wgPaymentDoc.IsManuallyEntered;
		foreach (IBWGPaymentRow row in wgPaymentDoc.Rows)
		{
			newPayment.Rows.Add(new MPRow
			{
				Amount = row.Amount,
				ReceiverAccount = row.ReceiverAcc,
				Receiver = row.Receiver,
				ReceiverLegalStatus = ReceiverLegalStatus.Natural
			});
		}
		CreatePayment(newPayment, ignoreWarnings: true);
	}

	public T GetPayment<T>(string accountNumber, Guid docId, bool throwExceptionNotExits = false) where T : Payment, new()
	{
		ArmSoft.InternetBank.APIClient.Client apiClientWithLogin = GetApiClientWithLogin(accountNumber);
		try
		{
			return apiClientWithLogin.PaymentManager.GetPayment<T>(docId);
		}
		catch (NotFoundException)
		{
			if (throwExceptionNotExits)
			{
				throw;
			}
			return null;
		}
	}

	public List<Payment> GetPayments(DateTime startDate, DateTime endDate, string accountNumber)
	{
		ArmSoft.InternetBank.APIClient.Client apiClientWithLogin = GetApiClientWithLogin(accountNumber);
		List<Payment> result = null;
		try
		{
			result = apiClientWithLogin.PaymentManager.GetPayments(startDate, endDate, null, null, null, DocumentState.NotSet, null, null, null, accountNumber);
		}
		catch (NotFoundException)
		{
		}
		return result;
	}

	public void CreatePayment<T>(T payment, bool ignoreWarnings = false, bool savePayment = true, bool saveTemplate = false, bool savePayee = false) where T : AccountPayment, new()
	{
		GetApiClientWithLogin(payment.PayerAcc).PaymentManager.CreatePayment(payment, ignoreWarnings, savePayment, saveTemplate, savePayee);
	}

	public T GetNewPayment<T>(string accountNumber, Guid? docId = null, bool throwExceptionNotExits = false) where T : Payment, new()
	{
		ArmSoft.InternetBank.APIClient.Client apiClientWithLogin = GetApiClientWithLogin(accountNumber);
		try
		{
			return apiClientWithLogin.PaymentManager.GetNewPayment<T>(docId);
		}
		catch (NotFoundException ex)
		{
			if (throwExceptionNotExits)
			{
				Settings.ErrorHandler(ex);
			}
			return null;
		}
	}

	public List<Card> GetCards(string bank)
	{
		ArmSoft.InternetBank.APIClient.Client apiClientWithLogin = GetApiClientWithLogin(bank);
		List<Card> result = null;
		try
		{
			result = apiClientWithLogin.CardManager.GetCards();
		}
		catch (NotFoundException)
		{
		}
		return result;
	}

	public List<Application> GetApplications(string bank, DateTime startDate, DateTime endDate)
	{
		ArmSoft.InternetBank.APIClient.Client apiClientWithLogin = GetApiClientWithLogin(bank);
		List<Application> result = null;
		try
		{
			result = apiClientWithLogin.ApplicationManager.GetApplications(startDate, endDate);
		}
		catch (NotFoundException)
		{
		}
		return result;
	}

	public List<Bond> GetBonds(string bank)
	{
		ArmSoft.InternetBank.APIClient.Client apiClientWithLogin = GetApiClientWithLogin(bank);
		List<Bond> result = null;
		try
		{
			result = apiClientWithLogin.BondManager.GetBonds();
		}
		catch (NotFoundException)
		{
		}
		return result;
	}

	public List<Credit> GetCredits(string bank)
	{
		ArmSoft.InternetBank.APIClient.Client apiClientWithLogin = GetApiClientWithLogin(bank);
		List<Credit> result = null;
		try
		{
			result = apiClientWithLogin.CreditManager.GetCredits();
		}
		catch (NotFoundException)
		{
		}
		return result;
	}

	public List<Deposit> GetDeposits(string bank)
	{
		ArmSoft.InternetBank.APIClient.Client apiClientWithLogin = GetApiClientWithLogin(bank);
		List<Deposit> result = null;
		try
		{
			result = apiClientWithLogin.DepositManager.GetDeposits();
		}
		catch (NotFoundException)
		{
		}
		return result;
	}

	public List<PurchasedSecurity> GetPurchasedSecurities(string bankCode)
	{
		ArmSoft.InternetBank.APIClient.Client apiClientWithLogin = GetApiClientWithLogin(bankCode);
		List<PurchasedSecurity> result = null;
		try
		{
			result = apiClientWithLogin.PurchasedSecurityManager.GetPurchasedSecurities();
		}
		catch (NotFoundException)
		{
		}
		return result;
	}

	public Challenge GetSignatureOptions(string bankCode, List<Guid> paymentIds)
	{
		ArmSoft.InternetBank.APIClient.Client apiClientWithLogin = GetApiClientWithLogin(bankCode);
		try
		{
			return apiClientWithLogin.PaymentManager.GetSignatureOptions(paymentIds);
		}
		catch (Exception ex)
		{
			throw new ASException(ex.Message);
		}
	}

	public string SignPayments(string bankCode, List<Guid> paymentIds, string securityProviderChallenge = null, string securityProviderResponse = null)
	{
		return GetApiClientWithLogin(bankCode).PaymentManager.SignPayments(paymentIds, securityProviderChallenge, securityProviderResponse);
	}

	public void RemovePayments(string bankCode, List<Guid> paymentIds)
	{
		ArmSoft.InternetBank.APIClient.Client apiClientWithLogin = GetApiClientWithLogin(bankCode);
		try
		{
			apiClientWithLogin.PaymentManager.RemovePayments(paymentIds);
		}
		catch (ForbiddenException)
		{
			throw new ASException(Resources.PayDocsCantBeRemoved);
		}
		catch (Exception ex2)
		{
			throw new ASException(ex2.Message);
		}
	}

	public void CancelPayment(string bankCode, Guid targetId, string targetNum, string targetType, DateTime targetDate, decimal targetAmount, string detail)
	{
		ArmSoft.InternetBank.APIClient.Client apiClientWithLogin = GetApiClientWithLogin(bankCode);
		try
		{
			List<ArmSoft.InternetBank.Web.API.Common.Models.Link> paymentLinks = apiClientWithLogin.PaymentManager.GetPaymentLinks(targetId);
			bool flag = false;
			foreach (ArmSoft.InternetBank.Web.API.Common.Models.Link item in paymentLinks)
			{
				if (item.Rel == "CancelDocument")
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				throw new ASException(Resources.PayDocCantBeCanceled);
			}
			CancelRequest newCancelRequest = apiClientWithLogin.CancelRequestManager.GetNewCancelRequest(targetId);
			newCancelRequest.Date = DateTime.Now;
			newCancelRequest.Details = detail;
			newCancelRequest.TargetAmount = Convert.ToString(targetAmount);
			newCancelRequest.TargetDate = Convert.ToString(targetDate);
			newCancelRequest.TargetId = targetId;
			newCancelRequest.TargetType = targetType;
			newCancelRequest.TargetNum = targetNum;
			apiClientWithLogin.CancelRequestManager.CreateCancelRequest(newCancelRequest, ignoreWarnings: true);
		}
		catch (Exception ex)
		{
			throw new ASException(ex.Message);
		}
	}

	public static async Task<List<DocumentHistoryRow>> GetPaymentHistoryAsync(Guid docISN, string bankCode, ArmSoft.InternetBank.APIClient.Client client)
	{
		List<DocumentHistoryRow> result = new List<DocumentHistoryRow>();
		foreach (DocumentHistory item in await client.PaymentManager.GetPaymentHistoryAsync(docISN))
		{
			result.Add(new DocumentHistoryRow
			{
				Id = item.DocId,
				OperationDate = item.OperationDate,
				OperationType = (short)item.OperationType,
				DisplayOperationType = item.DisplayOperationType,
				UserName = item.UserName,
				SystemType = (short)item.SystemType,
				DisplaySystemType = item.DisplaySystemType,
				HostAddress = item.HostAddress,
				Location = item.Location,
				DeviceModel = item.DeviceModel,
				DevicePlatform = item.DevicePlatform,
				ClientVersion = item.ClientVersion
			});
		}
		return result;
	}

	public List<DocumentHistoryRow> GetPaymentHistory(string bankCode, Guid docISN)
	{
		ArmSoft.InternetBank.APIClient.Client client = GetApiClientWithLogin(bankCode);
		return Task.Run(async () => await GetPaymentHistoryAsync(docISN, bankCode, client)).GetAwaiter().GetResult();
	}

	public ClientRate GetClientRate(string account, string currency1, string currency2)
	{
		ArmSoft.InternetBank.APIClient.Client apiClientWithLogin = GetApiClientWithLogin(SettlementAccountHelper.GetBankMainCode(account));
		try
		{
			return apiClientWithLogin.RateManager.GetClientRate(currency1, currency2);
		}
		catch (Exception ex)
		{
			throw new ASException(ex.Message);
		}
	}
}
