using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ArmSoft.OnlineBanking.Common.Enums;
using ArmSoft.OpenBanking.Api.Client;
using ArmSoft.OpenBanking.Api.Client.Core;
using ArmSoft.OpenBanking.Api.Common;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.InternetBank;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace ArmSoft.SME.Core.UI.Services;

public class OpenBankingService : IOpenBankingService
{
	private readonly string _state = "";

	private string _codeVerifier;

	private const string _redirectUri = "http://localhost";

	public Dictionary<string, ArmSoft.OpenBanking.Api.Client.ApiClient> BanksClients { get; } = new Dictionary<string, ArmSoft.OpenBanking.Api.Client.ApiClient>();

	public OpenBankingService()
	{
		_state = Guid.NewGuid().ToString("N");
	}

	public bool Login(string bankCode)
	{
		if (BanksClients.TryGetValue(bankCode, out var client) && client.IsLoggedIn)
		{
			return true;
		}
		(string, ArmSoft.OpenBanking.Api.Client.ApiClient) loginUrl = GetLoginUrl(bankCode);
		string item = loginUrl.Item1;
		ArmSoft.OpenBanking.Api.Client.ApiClient item2 = loginUrl.Item2;
		client = item2;
		WebView2 browser = new WebView2();
		InitializeWebViewAsync(browser, item);
		DialogWindow dialog = new DialogWindow
		{
			Name = "OpenBankingLoginDialog",
			MinWidth = 800.0,
			MinHeight = 850.0
		};
		bool loginComplete = false;
		browser.NavigationCompleted += async (object? s, CoreWebView2NavigationCompletedEventArgs e) =>
		{
			Uri source = browser.Source;
			if (source.AbsoluteUri.StartsWith("http://localhost") && !loginComplete)
			{
				NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(source.Query);
				string code = nameValueCollection["code"];
				if (nameValueCollection["state"] == _state)
				{
					await CompleteLogin(code, "http://localhost", client);
					loginComplete = true;
					dialog.DialogResult = true;
				}
				else
				{
					dialog.DialogResult = false;
				}
			}
		};
		dialog.Content = browser;
		if (dialog.ShowDialog().Value)
		{
			Settings.HostingEnvironment.RefreshStatusBarBankStatuses();
			return true;
		}
		return false;
	}

	private static async void InitializeWebViewAsync(WebView2 browser, string authUrl)
	{
		_ = 1;
		try
		{
			await browser.EnsureCoreWebView2Async(await CoreWebView2Environment.CreateAsync(null, Settings.GetTempPath(), new CoreWebView2EnvironmentOptions()));
			browser.Source = new Uri(authUrl);
			browser.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
			browser.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
			browser.CoreWebView2.Settings.AreDevToolsEnabled = false;
			browser.CoreWebView2.Settings.IsZoomControlEnabled = false;
			browser.CoreWebView2.Settings.IsStatusBarEnabled = false;
		}
		catch (Exception)
		{
		}
	}

	public List<OBBankInfo> GetSupportedBanks()
	{
		ArmSoft.OpenBanking.Api.Client.ApiClient client = new ArmSoft.OpenBanking.Api.Client.ApiClient(ConfigHelper.AOBSettings.AuthURL, ConfigHelper.AOBSettings.ApiURL, "", "");
		return (from b in AsyncHelper.RunSync(async () => await client.BankManager.GetBanks())
			where b.Status == BankStatus.Active
			select new OBBankInfo
			{
				Id = b.Id,
				Name = b.Name,
				IconUrl = b.IconUrl
			}).ToList();
	}

	private (string, ArmSoft.OpenBanking.Api.Client.ApiClient) GetLoginUrl(string bankCode)
	{
		return AsyncHelper.RunSync(() => GetLoginUrlAsync(bankCode));
	}

	private async Task<(string, ArmSoft.OpenBanking.Api.Client.ApiClient)> GetLoginUrlAsync(string bankCode)
	{
		string clientId = Settings.LoginService.Key;
		string token = Settings.B2BClient.Token;
		_codeVerifier = Settings.CommonCoreHelper.GetRandomString(6);
		ArmSoft.OpenBanking.Api.Client.ApiClient client = new ArmSoft.OpenBanking.Api.Client.ApiClient(ConfigHelper.AOBSettings.AuthURL, ConfigHelper.AOBSettings.ApiURL, clientId, token, null, LanguageService.CurrentUICultureName);
		string loginCode = await client.AuthenticationManager.Verify(_state);
		BanksClients[bankCode] = client;
		return (client.AuthenticationManager.BuildAuthorizeUrl(clientId, "http://localhost", _state, loginCode, _codeVerifier, bankCode), client);
	}

	private async Task CompleteLogin(string code, string redirectUri, ArmSoft.OpenBanking.Api.Client.ApiClient client)
	{
		try
		{
			await client.AuthenticationManager.SetTokens(code, redirectUri, _codeVerifier);
		}
		catch
		{
		}
	}

	public void Dispose()
	{
		BanksClients.Clear();
	}

	ArmSoft.OpenBanking.Api.Common.Account IOpenBankingService.GetAccount(string accountNumber)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		Login(bankMainCode);
		return AsyncHelper.RunSync(() => GetAccountAsync(accountNumber));
	}

	private async Task<ArmSoft.OpenBanking.Api.Common.Account> GetAccountAsync(string accountNumber)
	{
		ArmSoft.OpenBanking.Api.Client.ApiClient value;
		return (!BanksClients.TryGetValue(SettlementAccountHelper.GetBankMainCode(accountNumber), out value)) ? null : (await value.AccountManager.GetAccount(accountNumber));
	}

	List<AccountListItem> IOpenBankingService.GetAccounts(string bankCode, bool includeBalances)
	{
		Login(bankCode);
		return AsyncHelper.RunSync(() => GetAccountsAsync(bankCode, includeBalances));
	}

	private async Task<List<AccountListItem>> GetAccountsAsync(string bankCode, bool includeBalances)
	{
		IEnumerable<AccountListItem> enumerable = ((!BanksClients.TryGetValue(bankCode, out var value)) ? null : (await value.AccountManager.GetAccountListItems(includeBalances, includeCardAccounts: true, null)));
		return (List<AccountListItem>)enumerable;
	}

	AccountBalanceRow IOpenBankingService.GetAccountBalances(string accountNumber, DateTime openingBalanceDate, DateTime? closingBalanceDate)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		Login(bankMainCode);
		return AsyncHelper.RunSync(() => GetAccountBalancesAsync(accountNumber, openingBalanceDate, closingBalanceDate));
	}

	private async Task<AccountBalanceRow> GetAccountBalancesAsync(string accountNumber, DateTime openingBalanceDate, DateTime? closingBalanceDate)
	{
		AccountBalance accountBalance = ((!BanksClients.TryGetValue(SettlementAccountHelper.GetBankMainCode(accountNumber), out var value)) ? null : (await value.AccountManager.GetAccountBalancesByDate(accountNumber, openingBalanceDate, closingBalanceDate)));
		AccountBalance accountBalance2 = accountBalance;
		return new AccountBalanceRow
		{
			OpeningBalance = accountBalance2?.OpeningBalance,
			ClosingBalance = accountBalance2?.ClosingBalance,
			OpeningExpectedBalance = accountBalance2?.OpeningExpectedBalance,
			ClosingExpectedBalance = accountBalance2?.ClosingExpectedBalance
		};
	}

	T IOpenBankingService.GetNewPayment<T>(string accountNumber)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		Login(bankMainCode);
		return AsyncHelper.RunSync(() => GetNewPaymentAsync<T>(accountNumber));
	}

	private async Task<T> GetNewPaymentAsync<T>(string accountNumber) where T : DocumentForEdit<LocalPayment>, new()
	{
		if (!BanksClients.TryGetValue(SettlementAccountHelper.GetBankMainCode(accountNumber), out var value))
		{
			return null;
		}
		return (await value.LocalPaymentManager.GetNew()) as T;
	}

	IBPayment IOpenBankingService.GetPayment(string accountNumber, Guid docId, string receiverAccount, bool throwNotExists)
	{
		if (SettlementAccountHelper.Is_Budget_Account(receiverAccount))
		{
			DocumentForEdit<BudgetaryPayment> documentForEdit = AsyncHelper.RunSync(() => GetBudgetPaymentAsync<DocumentForEdit<BudgetaryPayment>>(accountNumber, docId, throwNotExists));
			if (documentForEdit == null)
			{
				return null;
			}
			DocumentDetails<BudgetaryPayment> documentDetails = AsyncHelper.RunSync(() => GetBudgetPaymentDetailsAsync<DocumentDetails<BudgetaryPayment>>(SettlementAccountHelper.GetBankMainCode(accountNumber), docId, throwNotExists));
			return new IBPayment
			{
				Amount = documentForEdit.Model.Amount,
				Currency = documentForEdit.Model.Currency,
				Details = documentForEdit.Model.Details,
				Date = documentForEdit.Model.Date,
				DocId = documentForEdit.Model.Id,
				DocNum = documentForEdit.Model.Number,
				DocState = Convert.ToByte(documentDetails?.State),
				PayerAcc = documentForEdit.Model.PayerAccount,
				ReceiverAcc = documentForEdit.Model.ReceiverAccount,
				PayerTaxCode = documentForEdit.Model.TaxCode,
				TaxAuthorityCode = documentForEdit.Model.TaxAuthorityCode,
				ReceiverName = documentForEdit.Model.Receiver,
				IsManuallyEntered = documentForEdit.Model.IsManuallyEntered,
				IsThirdParty = documentForEdit.Model.IsThirdParty,
				ThirdPartyResidenceType = documentForEdit.Model.ThirdPartyResidenceType,
				ThirdPartyLegalState = documentForEdit.Model.ThirdPartyLegalState,
				ThirdPartyTaxCode = documentForEdit.Model.ThirdPartyTaxCode,
				ThirdPartySocialCard = documentForEdit.Model.ThirdPartySocialCard,
				ThirdPartyName = documentForEdit.Model.ThirdPartyName,
				ThirdPartyPassportType = documentForEdit.Model.ThirdPartyPassportType,
				ThirdPartyPassportNo = documentForEdit.Model.ThirdPartyPassportNumber,
				ThirdPartyAddress = documentForEdit.Model.ThirdPartyAddress,
				ThirdPartyAdditionalInfo = documentForEdit.Model.ThirdPartyAdditionalInfo
			};
		}
		DocumentForEdit<LocalPayment> documentForEdit2 = AsyncHelper.RunSync(() => GetPaymentAsync<DocumentForEdit<LocalPayment>>(accountNumber, docId, throwNotExists));
		if (documentForEdit2 == null)
		{
			return null;
		}
		DocumentDetails<LocalPayment> documentDetails2 = AsyncHelper.RunSync(() => GetPaymentDetailsAsync<DocumentDetails<LocalPayment>>(SettlementAccountHelper.GetBankMainCode(accountNumber), docId, throwNotExists));
		return new IBPayment
		{
			Amount = documentForEdit2.Model.Amount,
			Currency = documentForEdit2.Model.Currency,
			Details = documentForEdit2.Model.Details,
			Date = documentForEdit2.Model.Date,
			DocId = documentForEdit2.Model.Id,
			DocNum = documentForEdit2.Model.Number,
			DocState = (byte)(documentDetails2?.State).Value,
			PayerAcc = documentForEdit2.Model.PayerAccount,
			ReceiverAcc = documentForEdit2.Model.ReceiverAccount,
			ReceiverLegalStatus = documentForEdit2.Model.ReceiverLegalStatus,
			ReceiverName = documentForEdit2.Model.Receiver,
			PayerTaxCode = documentForEdit2.Model.TaxCode
		};
	}

	T IOpenBankingService.GetPayment<T>(string accountNumber, Guid docId, bool throwNotExists)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		Login(bankMainCode);
		return AsyncHelper.RunSync(() => GetPaymentAsync<T>(accountNumber, docId, throwNotExists));
	}

	private async Task<T> GetPaymentAsync<T>(string accountNumber, Guid docId, bool throwNotExists = false) where T : DocumentForEdit<LocalPayment>, new()
	{
		try
		{
			if (!BanksClients.TryGetValue(SettlementAccountHelper.GetBankMainCode(accountNumber), out var client))
			{
				return null;
			}
			DocumentForEdit<LocalPayment> documentForEdit = await client.LocalPaymentManager.GetForEdit(docId);
			client.LocalPaymentManager.GetDetails(docId);
			return documentForEdit as T;
		}
		catch (NotFoundException)
		{
			if (throwNotExists)
			{
				throw;
			}
			return null;
		}
	}

	T IOpenBankingService.GetPaymentDetails<T>(string bankCode, Guid docId, bool throwNotExists)
	{
		Login(bankCode);
		return AsyncHelper.RunSync(() => GetPaymentDetailsAsync<T>(bankCode, docId, throwNotExists));
	}

	private async Task<T> GetPaymentDetailsAsync<T>(string bankCode, Guid docId, bool throwNotExists) where T : DocumentDetails<LocalPayment>, new()
	{
		try
		{
			if (!BanksClients.TryGetValue(bankCode, out var value))
			{
				return null;
			}
			return (await value.LocalPaymentManager.GetDetails(docId)) as T;
		}
		catch (NotFoundException)
		{
			if (throwNotExists)
			{
				throw;
			}
			return null;
		}
	}

	T IOpenBankingService.GetMultiplePaymentDetails<T>(string bankCode, Guid docId, bool throwNotExists)
	{
		Login(bankCode);
		return AsyncHelper.RunSync(() => GetMultiplePaymentDetailsAsync<T>(bankCode, docId, throwNotExists));
	}

	private async Task<T> GetMultiplePaymentDetailsAsync<T>(string bankCode, Guid docId, bool throwNotExists) where T : DocumentDetails<MultiplePayment>, new()
	{
		try
		{
			if (!BanksClients.TryGetValue(bankCode, out var value))
			{
				return null;
			}
			return (await value.MultiplePaymentManager.GetDetails(docId)) as T;
		}
		catch (NotFoundException)
		{
			if (throwNotExists)
			{
				throw;
			}
			return null;
		}
	}

	public async Task CreatePayment(IBPayment payOrderDoc)
	{
		if (SettlementAccountHelper.Is_Budget_Account(payOrderDoc.ReceiverAcc))
		{
			await CreateIBBudgetPayOrder(payOrderDoc);
		}
		else
		{
			await CreateIBPayOrder(payOrderDoc);
		}
	}

	private async Task CreateIBPayOrder(IBPayment payOrderDoc)
	{
		InternetBankHelper.CheckPaymentForExport(payOrderDoc);
		DocumentForEdit<LocalPayment> documentForEdit = await GetNewPaymentAsync<DocumentForEdit<LocalPayment>>(SettlementAccountHelper.GetBankMainCode(payOrderDoc.PayerAcc));
		if (documentForEdit != null)
		{
			LocalPayment model = documentForEdit.Model;
			model.Amount = payOrderDoc.Amount;
			model.Currency = payOrderDoc.Currency;
			model.Details = payOrderDoc.Details;
			model.Date = payOrderDoc.Date;
			model.Id = payOrderDoc.DocId;
			model.Number = payOrderDoc.DocNum;
			model.PayerAccount = payOrderDoc.PayerAcc;
			model.ReceiverAccount = payOrderDoc.ReceiverAcc;
			model.Receiver = payOrderDoc.ReceiverName;
			model.ReceiverLegalStatus = payOrderDoc.ReceiverLegalStatus;
			model.TaxCode = payOrderDoc.PayerTaxCode;
			model.IsManuallyEntered = payOrderDoc.IsManuallyEntered;
			await CreatePaymentAsync(model, ignoreWarnings: true);
		}
	}

	void IOpenBankingService.CreatePayment(IBPayment payOrderDoc)
	{
		AsyncHelper.RunSync(() => CreatePayment(payOrderDoc));
	}

	void IOpenBankingService.CreatePayment<T>(T payment, bool ignoreWarnings, bool savePayment, bool saveTemplate)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(payment.PayerAccount);
		Login(bankMainCode);
		AsyncHelper.RunSync(() => CreatePaymentAsync(payment, savePayment, saveTemplate, ignoreWarnings));
	}

	private async Task CreatePaymentAsync<T>(T payment, bool ignoreWarnings = false, bool savePayment = true, bool saveTemplate = false) where T : LocalPayment, new()
	{
		BanksClients.TryGetValue(SettlementAccountHelper.GetBankMainCode(payment.PayerAccount), out var value);
		await value.LocalPaymentManager.Create(payment, savePayment, saveTemplate, ignoreWarnings);
	}

	void IOpenBankingService.CreateCurrencyConversion(IBCurrConversion currConversionDoc)
	{
		AsyncHelper.RunSync(() => CreateCurrencyConversionAsync(currConversionDoc));
	}

	public async Task CreateCurrencyConversionAsync(IBCurrConversion currConversionDoc)
	{
		await CreateCurrencyIBConversion(currConversionDoc);
	}

	private async Task CreateCurrencyIBConversion(IBCurrConversion currConversionDoc)
	{
		DocumentForEdit<LocalPayment> documentForEdit = await GetNewPaymentAsync<DocumentForEdit<LocalPayment>>(SettlementAccountHelper.GetBankMainCode(currConversionDoc.PayerAcc));
		if (documentForEdit != null)
		{
			LocalPayment model = documentForEdit.Model;
			model.PaymentMode = PaymentMode.MyAccount;
			model.Amount = currConversionDoc.Amount;
			model.Currency = currConversionDoc.Currency;
			model.Details = currConversionDoc.Details;
			model.Date = currConversionDoc.Date;
			model.Number = currConversionDoc.DocNum;
			model.PayerAccount = currConversionDoc.PayerAcc;
			model.ReceiverAccount = currConversionDoc.ReceiverAcc;
			model.TaxCode = currConversionDoc.PayerTaxCode;
			model.Receiver = currConversionDoc.ReceiverName;
			model.RateCurrency1 = currConversionDoc.RateCurrency1;
			model.RateCurrency2 = currConversionDoc.RateCurrency2;
			model.RateFor = currConversionDoc.RateFor;
			model.RateValue = currConversionDoc.RateValue;
			model.FixedCurrency = currConversionDoc.FixedCurrency;
			model.AmountCredit = currConversionDoc.AmountCredit;
			model.CurrencyCredit = currConversionDoc.CurrencyCredit;
			model.IsManuallyEntered = currConversionDoc.IsManuallyEntered;
			await CreatePaymentAsync(model, ignoreWarnings: true);
		}
	}

	IBCurrConversion IOpenBankingService.GetCurrencyConversion(string bankCode, Guid docId, bool throwNotExists)
	{
		DocumentForEdit<LocalPayment> documentForEdit = AsyncHelper.RunSync(() => GetPaymentAsync<DocumentForEdit<LocalPayment>>(bankCode, docId, throwNotExists));
		if (documentForEdit == null)
		{
			return null;
		}
		_ = documentForEdit.Model;
		IBCurrConversion iBCurrConversion = new IBCurrConversion
		{
			Amount = documentForEdit.Model.Amount,
			Currency = documentForEdit.Model.Currency,
			Details = documentForEdit.Model.Details,
			Date = documentForEdit.Model.Date,
			DocId = documentForEdit.Model.Id,
			DocNum = documentForEdit.Model.Number,
			PayerAcc = documentForEdit.Model.PayerAccount,
			ReceiverAcc = documentForEdit.Model.Receiver,
			RateCurrency1 = documentForEdit.Model.RateCurrency1,
			RateCurrency2 = documentForEdit.Model.RateCurrency2,
			RateFor = documentForEdit.Model.RateFor.Value,
			RateValue = documentForEdit.Model.RateValue.Value,
			FixedCurrency = documentForEdit.Model.FixedCurrency,
			AmountCredit = documentForEdit.Model.AmountCredit,
			CurrencyCredit = documentForEdit.Model.CurrencyCredit,
			IsManuallyEntered = documentForEdit.Model.IsManuallyEntered
		};
		Rate rate = AsyncHelper.RunSync(() => GetClientRateAsync(bankCode, iBCurrConversion.RateCurrency1, iBCurrConversion.RateCurrency2));
		iBCurrConversion.RateLabel = rate.RateLabel;
		return iBCurrConversion;
	}

	public IBCardTransfer GetCardTransfer(string accountNumber, Guid docId, bool throwNotExists = false)
	{
		DocumentForEdit<CardTransfer> documentForEdit = AsyncHelper.RunSync(() => GetCardTransferAsync<DocumentForEdit<CardTransfer>>(accountNumber, docId, throwNotExists));
		if (documentForEdit == null)
		{
			return null;
		}
		return new IBCardTransfer
		{
			Amount = documentForEdit.Model.Amount,
			Currency = documentForEdit.Model.Currency,
			Details = documentForEdit.Model.Details,
			Date = documentForEdit.Model.Date,
			DocId = documentForEdit.Model.Id,
			DocNum = documentForEdit.Model.Number,
			PayerAcc = documentForEdit.Model.PayerAccount,
			ReceiverCardNumber = documentForEdit.Model.ReceiverCardNumber,
			ReceiverCountryCode = documentForEdit.Model.ReceiverCountryCode,
			CommissionAmount = documentForEdit.Model.CommissionAmount,
			ChargeAcc = documentForEdit.Model.ChargeAccount
		};
	}

	T IOpenBankingService.GetCardTransfer<T>(string accountNumber, Guid docId, bool throwNotExists)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		Login(bankMainCode);
		return AsyncHelper.RunSync(() => GetCardTransferAsync<T>(accountNumber, docId, throwNotExists));
	}

	private async Task<T> GetCardTransferAsync<T>(string accountNumber, Guid docId, bool throwNotExists = false) where T : DocumentForEdit<CardTransfer>, new()
	{
		try
		{
			if (!BanksClients.TryGetValue(SettlementAccountHelper.GetBankMainCode(accountNumber), out var value))
			{
				return null;
			}
			return value.CardTransferManager.GetDetails(docId) as T;
		}
		catch (NotFoundException)
		{
			if (throwNotExists)
			{
				throw;
			}
			return null;
		}
	}

	T IOpenBankingService.GetNewBudgetPayment<T>(string accountNumber)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		Login(bankMainCode);
		return AsyncHelper.RunSync(() => GetNewBudgetPaymentAsync<T>(accountNumber));
	}

	private async Task<T> GetNewBudgetPaymentAsync<T>(string accountNumber) where T : DocumentForEdit<BudgetaryPayment>, new()
	{
		if (!BanksClients.TryGetValue(SettlementAccountHelper.GetBankMainCode(accountNumber), out var value))
		{
			return null;
		}
		return (await value.BudgetaryPaymentManager.GetNew()) as T;
	}

	T IOpenBankingService.GetBudgetPayment<T>(string accountNumber, Guid docId, bool throwNotExists)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		Login(bankMainCode);
		return AsyncHelper.RunSync(() => GetBudgetPaymentAsync<T>(accountNumber, docId, throwNotExists));
	}

	private async Task<T> GetBudgetPaymentAsync<T>(string accountNumber, Guid docId, bool throwNotExists = false) where T : DocumentForEdit<BudgetaryPayment>, new()
	{
		try
		{
			if (!BanksClients.TryGetValue(SettlementAccountHelper.GetBankMainCode(accountNumber), out var value))
			{
				return null;
			}
			return (await value.BudgetaryPaymentManager.GetForEdit(docId)) as T;
		}
		catch (NotFoundException)
		{
			if (throwNotExists)
			{
				throw;
			}
			return null;
		}
	}

	T IOpenBankingService.GetBudgetPaymentDetails<T>(string bankCode, Guid docId, bool throwNotExists)
	{
		Login(bankCode);
		return AsyncHelper.RunSync(() => GetBudgetPaymentDetailsAsync<T>(bankCode, docId, throwNotExists));
	}

	private async Task<T> GetBudgetPaymentDetailsAsync<T>(string bankCode, Guid docId, bool throwNotExists) where T : DocumentDetails<BudgetaryPayment>, new()
	{
		try
		{
			if (!BanksClients.TryGetValue(bankCode, out var value))
			{
				return null;
			}
			return (await value.BudgetaryPaymentManager.GetDetails(docId)) as T;
		}
		catch (NotFoundException)
		{
			if (throwNotExists)
			{
				throw;
			}
			return null;
		}
	}

	private async Task CreateIBBudgetPayOrder(IBPayment payOrderDoc)
	{
		InternetBankHelper.CheckPaymentForExport(payOrderDoc);
		DocumentForEdit<BudgetaryPayment> documentForEdit = await GetNewBudgetPaymentAsync<DocumentForEdit<BudgetaryPayment>>(SettlementAccountHelper.GetBankMainCode(payOrderDoc.PayerAcc));
		if (documentForEdit != null)
		{
			BudgetaryPayment model = documentForEdit.Model;
			model.Amount = payOrderDoc.Amount;
			model.Currency = payOrderDoc.Currency;
			model.Details = payOrderDoc.Details;
			model.Date = payOrderDoc.Date;
			model.Id = payOrderDoc.DocId;
			model.Number = payOrderDoc.DocNum;
			model.PayerAccount = payOrderDoc.PayerAcc;
			model.ReceiverAccount = payOrderDoc.ReceiverAcc;
			model.TaxCode = payOrderDoc.PayerTaxCode;
			model.TaxAuthorityCode = payOrderDoc.TaxAuthorityCode;
			model.Receiver = payOrderDoc.ReceiverName;
			model.IsManuallyEntered = payOrderDoc.IsManuallyEntered;
			model.IsThirdParty = payOrderDoc.IsThirdParty;
			if (model.IsThirdParty)
			{
				model.ThirdPartyResidenceType = payOrderDoc.ThirdPartyResidenceType;
				model.ThirdPartyLegalState = payOrderDoc.ThirdPartyLegalState;
				model.TaxAuthorityCode = payOrderDoc.TaxAuthorityCode;
				model.ThirdPartyTaxCode = payOrderDoc.ThirdPartyTaxCode;
				model.ThirdPartySocialCard = payOrderDoc.ThirdPartySocialCard;
				model.ThirdPartyName = payOrderDoc.ThirdPartyName;
				model.ThirdPartyPassportType = payOrderDoc.ThirdPartyPassportType;
				model.ThirdPartyPassportNumber = payOrderDoc.ThirdPartyPassportNo;
				model.ThirdPartyAddress = payOrderDoc.ThirdPartyAddress;
				model.ThirdPartyAdditionalInfo = payOrderDoc.ThirdPartyAdditionalInfo;
			}
			await CreateBudgetPaymentAsync(model, ignoreWarnings: true);
		}
	}

	void IOpenBankingService.CreateBudgetPayment<T>(T payment, bool ignoreWarnings, bool savePayment, bool saveTemplate)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(payment.PayerAccount);
		Login(bankMainCode);
		AsyncHelper.RunSync(() => CreateBudgetPaymentAsync(payment, savePayment, saveTemplate, ignoreWarnings));
	}

	private async Task CreateBudgetPaymentAsync<T>(T payment, bool ignoreWarnings = false, bool savePayment = true, bool saveTemplate = false) where T : BudgetaryPayment, new()
	{
		BanksClients.TryGetValue(SettlementAccountHelper.GetBankMainCode(payment.PayerAccount), out var value);
		await value.BudgetaryPaymentManager.Create(payment, savePayment, saveTemplate, ignoreWarnings);
	}

	T IOpenBankingService.GetNewMultiplePayment<T>(string accountNumber)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		Login(bankMainCode);
		return AsyncHelper.RunSync(() => GetNewMultiplePaymentAsync<T>(accountNumber));
	}

	private async Task<T> GetNewMultiplePaymentAsync<T>(string accountNumber) where T : DocumentForEdit<MultiplePayment>, new()
	{
		if (!BanksClients.TryGetValue(SettlementAccountHelper.GetBankMainCode(accountNumber), out var value))
		{
			return null;
		}
		return (await value.MultiplePaymentManager.GetNew()) as T;
	}

	T IOpenBankingService.GetMultiplePayment<T>(string accountNumber, Guid docId, bool throwNotExists)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		Login(bankMainCode);
		return AsyncHelper.RunSync(() => GetMultiplePaymentAsync<T>(accountNumber, docId, throwNotExists));
	}

	private async Task<T> GetMultiplePaymentAsync<T>(string accountNumber, Guid docId, bool throwNotExists = false) where T : DocumentForEdit<MultiplePayment>, new()
	{
		try
		{
			if (!BanksClients.TryGetValue(SettlementAccountHelper.GetBankMainCode(accountNumber), out var value))
			{
				return null;
			}
			return (await value.MultiplePaymentManager.GetForEdit(docId)) as T;
		}
		catch (NotFoundException)
		{
			if (throwNotExists)
			{
				throw;
			}
			return null;
		}
	}

	IBWGPayment IOpenBankingService.GetMultiplePayment(string accountNumber, Guid docId, bool throwNotExists)
	{
		DocumentForEdit<MultiplePayment> documentForEdit = AsyncHelper.RunSync(() => GetMultiplePaymentAsync<DocumentForEdit<MultiplePayment>>(accountNumber, docId, throwNotExists));
		if (documentForEdit == null)
		{
			return null;
		}
		DocumentDetails<MultiplePayment> documentDetails = AsyncHelper.RunSync(() => GetMultiplePaymentDetailsAsync<DocumentDetails<MultiplePayment>>(SettlementAccountHelper.GetBankMainCode(accountNumber), docId, throwNotExists));
		return new IBWGPayment
		{
			Amount = documentForEdit.Model.Amount,
			Currency = documentForEdit.Model.Currency,
			Details = documentForEdit.Model.Details,
			Date = documentForEdit.Model.Date,
			DocId = documentForEdit.Model.Id,
			DocNum = documentForEdit.Model.Number,
			DocState = Convert.ToByte(documentDetails?.State),
			PayerAcc = documentForEdit.Model.PayerAccount,
			IsManuallyEntered = documentForEdit.Model.IsManuallyEntered,
			TaxCode = documentForEdit.Model.TaxCode,
			SocialCard = documentForEdit.Model.SocialCard,
			Rows = documentForEdit.Model.Rows.Select((MultiplePaymentRow r) => new IBWGPaymentRow
			{
				Amount = r.Amount,
				ReceiverAcc = r.ReceiverAcc,
				Receiver = r.Receiver
			}).ToList()
		};
	}

	public async Task CreateMultiplePayment(IBWGPayment wgPayDoc)
	{
		DocumentForEdit<MultiplePayment> documentForEdit = await GetNewMultiplePaymentAsync<DocumentForEdit<MultiplePayment>>(SettlementAccountHelper.GetBankMainCode(wgPayDoc.PayerAcc));
		if (documentForEdit == null)
		{
			return;
		}
		MultiplePayment model = documentForEdit.Model;
		model.Amount = wgPayDoc.Amount;
		model.Currency = wgPayDoc.Currency;
		model.Details = wgPayDoc.Details;
		model.Date = wgPayDoc.Date;
		model.Id = wgPayDoc.DocId;
		model.Number = wgPayDoc.DocNum;
		model.PayerAccount = wgPayDoc.PayerAcc;
		model.TaxCode = wgPayDoc.TaxCode;
		model.SocialCard = wgPayDoc.SocialCard;
		model.IsManuallyEntered = wgPayDoc.IsManuallyEntered;
		foreach (IBWGPaymentRow row in wgPayDoc.Rows)
		{
			model.Rows.Add(new MultiplePaymentRow
			{
				Amount = row.Amount,
				ReceiverAcc = row.ReceiverAcc,
				Receiver = row.Receiver,
				ReceiverLegalStatus = ReceiverLegalStatus.Natural
			});
		}
		await CreateMultiplePaymentAsync(model, ignoreWarnings: true);
	}

	void IOpenBankingService.CreateMultiplePayment(IBWGPayment wgPaymentDoc)
	{
		AsyncHelper.RunSync(() => CreateMultiplePayment(wgPaymentDoc));
	}

	void IOpenBankingService.CreateMultiplePayment<T>(T payment, bool ignoreWarnings, bool savePayment, bool saveTemplate)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(payment.PayerAccount);
		Login(bankMainCode);
		AsyncHelper.RunSync(() => CreateMultiplePaymentAsync(payment, savePayment, saveTemplate, ignoreWarnings));
	}

	private async Task CreateMultiplePaymentAsync<T>(T payment, bool ignoreWarnings = false, bool savePayment = true, bool saveTemplate = false) where T : MultiplePayment, new()
	{
		BanksClients.TryGetValue(SettlementAccountHelper.GetBankMainCode(payment.PayerAccount), out var value);
		await value.MultiplePaymentManager.Create(payment, savePayment, saveTemplate, ignoreWarnings);
	}

	List<DocumentListItem> IOpenBankingService.GetPayments(DateTime startDate, DateTime endDate, string accountNumber)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		Login(bankMainCode);
		return AsyncHelper.RunSync(() => GetPaymentsAsync(startDate, endDate, accountNumber));
	}

	public async Task<List<DocumentListItem>> GetPaymentsAsync(DateTime startDate, DateTime endDate, string accountNumber)
	{
		BanksClients.TryGetValue(SettlementAccountHelper.GetBankMainCode(accountNumber), out var value);
		return await value.DocumentsManager.GetDocuments(startDate, endDate, null, null, null, null, null, null, null, accountNumber, null, includeCreatorNames: false, "P");
	}

	List<MerchantTransaction> IOpenBankingService.GetMerchantTransactions(DateTime startDate, DateTime endDate, string accountNumber)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		Login(bankMainCode);
		return AsyncHelper.RunSync(() => GetMerchantTransactionsAsync(startDate, endDate, accountNumber));
	}

	public async Task<List<MerchantTransaction>> GetMerchantTransactionsAsync(DateTime startDate, DateTime endDate, string accountNumber)
	{
		try
		{
			BanksClients.TryGetValue(SettlementAccountHelper.GetBankMainCode(accountNumber), out var value);
			return await value.MerchantManager.GetMerchantTransactions(startDate, endDate, null, null, accountNumber);
		}
		catch (NotFoundException)
		{
			throw new ASException(Resources.MerchantAccountNotExist);
		}
	}

	List<AccountTransaction> IOpenBankingService.GetTransactions(DateTime startDate, DateTime endDate, string accountNumber, TransactionFilterOption transactionFilterOption = TransactionFilterOption.All)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		Login(bankMainCode);
		return AsyncHelper.RunSync(() => GetTransactionsAsync(startDate, endDate, accountNumber, transactionFilterOption));
	}

	public async Task<List<AccountTransaction>> GetTransactionsAsync(DateTime startDate, DateTime endDate, string accountNumber, TransactionFilterOption transactionFilterOption = TransactionFilterOption.All)
	{
		List<AccountTransaction> result = new List<AccountTransaction>();
		try
		{
			BanksClients.TryGetValue(SettlementAccountHelper.GetBankMainCode(accountNumber), out var value);
			result = (List<AccountTransaction>)(await value.AccountManager.GetAccountTransactions(accountNumber, showAMD: true, null, transactionFilterOption, startDate, endDate));
		}
		catch (NotFoundException)
		{
		}
		return result;
	}

	List<DocumentListItem> IOpenBankingService.GetApplications(string bankCode, DateTime startDate, DateTime endDate)
	{
		Login(bankCode);
		return AsyncHelper.RunSync(() => GetApplicationsAsync(bankCode, startDate, endDate));
	}

	public async Task<List<DocumentListItem>> GetApplicationsAsync(string bankCode, DateTime startDate, DateTime endDate)
	{
		List<DocumentListItem> result = new List<DocumentListItem>();
		try
		{
			BanksClients.TryGetValue(bankCode, out var value);
			result = await value.DocumentsManager.GetDocuments(startDate, endDate, null, null, null, null, null, null, null, null, null, includeCreatorNames: false, "A");
		}
		catch (NotFoundException)
		{
		}
		return result;
	}

	List<Card> IOpenBankingService.GetCards(string bankCode)
	{
		Login(bankCode);
		return AsyncHelper.RunSync(() => GetCardsAsync(bankCode));
	}

	public async Task<List<Card>> GetCardsAsync(string bankCode)
	{
		List<Card> result = new List<Card>();
		try
		{
			BanksClients.TryGetValue(bankCode, out var value);
			result = (List<Card>)(await value.CardManager.GetCards(includeAccountBalances: true));
		}
		catch (NotFoundException)
		{
		}
		return result;
	}

	List<Bond> IOpenBankingService.GetBonds(string bankCode)
	{
		Login(bankCode);
		return AsyncHelper.RunSync(() => GetBondsAsync(bankCode));
	}

	public async Task<List<Bond>> GetBondsAsync(string bankCode)
	{
		List<Bond> result = new List<Bond>();
		try
		{
			BanksClients.TryGetValue(bankCode, out var value);
			result = (List<Bond>)(await value.BondManager.GetBonds(BondAccessLevels.View));
		}
		catch (NotFoundException)
		{
		}
		return result;
	}

	ContractOnlineDataListResult<BondOnlineData> IOpenBankingService.GetBondsOnlineDataList(string bankCode, List<int> ids)
	{
		Login(bankCode);
		return AsyncHelper.RunSync(() => GetBondsOnlineDataListAsync(bankCode, ids));
	}

	public async Task<ContractOnlineDataListResult<BondOnlineData>> GetBondsOnlineDataListAsync(string bankCode, List<int> ids)
	{
		BanksClients.TryGetValue(bankCode, out var value);
		return await value.BondManager.GetBondsOnlineDataList(ids, BondAccessLevels.View, includeSummary: true);
	}

	List<Credit> IOpenBankingService.GetCredits(string bankCode)
	{
		Login(bankCode);
		return AsyncHelper.RunSync(() => GetCreditsAsync(bankCode));
	}

	public async Task<List<Credit>> GetCreditsAsync(string bankCode)
	{
		BanksClients.TryGetValue(bankCode, out var value);
		return (List<Credit>)(await value.CreditManager.GetCredits(CreditAccessLevels.View));
	}

	ContractOnlineDataListResult<CreditOnlineData> IOpenBankingService.GetCreditsOnlineDataList(string bankCode, List<int> ids)
	{
		Login(bankCode);
		return AsyncHelper.RunSync(() => GetCreditsOnlineDataListAsync(bankCode, ids));
	}

	public async Task<ContractOnlineDataListResult<CreditOnlineData>> GetCreditsOnlineDataListAsync(string bankCode, List<int> ids)
	{
		BanksClients.TryGetValue(bankCode, out var value);
		return await value.CreditManager.GetCreditsOnlineDataList(ids, CreditAccessLevels.View, includeSummary: true);
	}

	List<Deposit> IOpenBankingService.GetDeposits(string bankCode)
	{
		Login(bankCode);
		return AsyncHelper.RunSync(() => GetDepositsAsync(bankCode));
	}

	public async Task<List<Deposit>> GetDepositsAsync(string bankCode)
	{
		BanksClients.TryGetValue(bankCode, out var value);
		return (List<Deposit>)(await value.DepositManager.GetDeposits());
	}

	ContractOnlineDataListResult<DepositOnlineData> IOpenBankingService.GetDepositsOnlineDataList(string bankCode, List<int> ids)
	{
		Login(bankCode);
		return AsyncHelper.RunSync(() => GetDepositsOnlineDataListAsync(bankCode, ids));
	}

	public async Task<ContractOnlineDataListResult<DepositOnlineData>> GetDepositsOnlineDataListAsync(string bankCode, List<int> ids)
	{
		BanksClients.TryGetValue(bankCode, out var value);
		return await value.DepositManager.GetDepositsOnlineDataList(ids, DepositAccessLevels.View, includeSummary: true);
	}

	List<PurchasedSecurity> IOpenBankingService.GetPurchasedSecurities(string bankCode)
	{
		Login(bankCode);
		return AsyncHelper.RunSync(() => GetPurchasedSecuritiesAsync(bankCode));
	}

	public async Task<List<PurchasedSecurity>> GetPurchasedSecuritiesAsync(string bankCode)
	{
		BanksClients.TryGetValue(bankCode, out var value);
		return await value.PurchasedSecurityManager.GetPurchasedSecurities();
	}

	List<DocumentHistoryRow> IOpenBankingService.GetPaymentHistory(string bankCode, Guid docId)
	{
		Login(bankCode);
		return AsyncHelper.RunSync(() => GetPaymentHistoryAsync(bankCode, docId));
	}

	public async Task<List<DocumentHistoryRow>> GetPaymentHistoryAsync(string bankCode, Guid docId)
	{
		BanksClients.TryGetValue(bankCode, out var value);
		List<DocumentHistoryRow> result = new List<DocumentHistoryRow>();
		foreach (DocumentHistory item in (List<DocumentHistory>)(await value.DocumentsManager.GetHistory(docId)))
		{
			result.Add(new DocumentHistoryRow
			{
				Id = item.Id,
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

	Rate IOpenBankingService.GetClientRate(string bankCode, string currency1, string currency2)
	{
		Login(bankCode);
		return AsyncHelper.RunSync(() => GetClientRateAsync(bankCode, currency1, currency2));
	}

	public async Task<Rate> GetClientRateAsync(string bankCode, string currency1, string currency2)
	{
		BanksClients.TryGetValue(bankCode, out var value);
		return await value.RateManager.GetRate(currency1, currency2);
	}

	SignatureOptions IOpenBankingService.GetSignatureOptions(string bankCode, List<Guid> paymentIds)
	{
		Login(bankCode);
		return AsyncHelper.RunSync(() => GetSignatureOptionsAsync(bankCode, paymentIds));
	}

	public async Task<SignatureOptions> GetSignatureOptionsAsync(string bankCode, List<Guid> paymentIds)
	{
		BanksClients.TryGetValue(bankCode, out var value);
		return await value.DocumentsManager.GetSignatureOptions(paymentIds);
	}

	SignedDocumentsResult IOpenBankingService.SignPayments(string bankCode, List<Guid> paymentIds, string securityProviderChallenge, string signature)
	{
		Login(bankCode);
		return AsyncHelper.RunSync(() => SignPaymentsAsync(bankCode, paymentIds, securityProviderChallenge, signature));
	}

	public async Task<SignedDocumentsResult> SignPaymentsAsync(string bankCode, List<Guid> paymentIds, string securityProviderChallenge, string signature)
	{
		BanksClients.TryGetValue(bankCode, out var value);
		return await value.DocumentsManager.GroupSign(paymentIds, securityProviderChallenge, signature);
	}

	void IOpenBankingService.RemovePayments(string bankCode, List<Guid> paymentIds)
	{
		Login(bankCode);
		AsyncHelper.RunSync(() => RemovePaymentsAsync(bankCode, paymentIds));
	}

	public async Task RemovePaymentsAsync(string bankCode, List<Guid> paymentIds)
	{
		try
		{
			BanksClients.TryGetValue(bankCode, out var value);
			await value.DocumentsManager.GroupDelete(paymentIds);
		}
		catch (ForbiddenException)
		{
			throw new ASException(Resources.PayDocsCantBeRemoved);
		}
	}

	void IOpenBankingService.CancelPayment(string bankCode, byte state, Guid targetId, string detail)
	{
		Login(bankCode);
		AsyncHelper.RunSync(() => CancelPaymentAsync(bankCode, state, targetId, detail));
	}

	public async Task CancelPaymentAsync(string bankCode, byte state, Guid targetId, string detail)
	{
		BanksClients.TryGetValue(bankCode, out var client);
		bool flag = false;
		if (state == 2 || state == 3 || state == 4 || state == 5)
		{
			flag = true;
		}
		if (!flag)
		{
			throw new ASException(Resources.PayDocCantBeCanceled);
		}
		DocumentForEdit<CancelRequestApplication> documentForEdit = await client.CancelRequestApplicationManager.GetNew(new GetNewCancelRequestApplicationParams
		{
			TargetId = targetId
		});
		documentForEdit.Model.Date = DateTime.Now;
		documentForEdit.Model.Details = detail;
		documentForEdit.Model.TargetId = targetId;
		await client.CancelRequestApplicationManager.Create(documentForEdit.Model);
	}

	void IOpenBankingService.CancelLoacalPayment(string bankCode, Guid targetId, string detail)
	{
		Login(bankCode);
		AsyncHelper.RunSync(() => CancelLoacalPaymentAsync(bankCode, targetId, detail));
	}

	public async Task CancelLoacalPaymentAsync(string bankCode, Guid targetId, string detail)
	{
		BanksClients.TryGetValue(bankCode, out var client);
		bool canBeCanceled = false;
		DocumentDetails<LocalPayment> documentDetails = await client.LocalPaymentManager.GetDetails(targetId);
		if (documentDetails != null && (documentDetails.State == DocumentState.PartiallySigned || documentDetails.State == DocumentState.ReadyToBeSent || documentDetails.State == DocumentState.Sent || documentDetails.State == DocumentState.InProcessBank))
		{
			canBeCanceled = true;
		}
		if (!canBeCanceled)
		{
			throw new ASException(Resources.PayDocCantBeCanceled);
		}
		DocumentForEdit<CancelRequestApplication> documentForEdit = await client.CancelRequestApplicationManager.GetNew(new GetNewCancelRequestApplicationParams
		{
			TargetId = targetId
		});
		documentForEdit.Model.Date = DateTime.Now;
		documentForEdit.Model.Details = detail;
		documentForEdit.Model.TargetId = targetId;
		await client.CancelRequestApplicationManager.Create(documentForEdit.Model);
	}

	void IOpenBankingService.CancelBudgetPayment(string bankCode, Guid targetId, string detail)
	{
		Login(bankCode);
		AsyncHelper.RunSync(() => CancelBudgetPaymentAsync(bankCode, targetId, detail));
	}

	public async Task CancelBudgetPaymentAsync(string bankCode, Guid targetId, string detail)
	{
		BanksClients.TryGetValue(bankCode, out var client);
		bool canBeCanceled = false;
		DocumentDetails<BudgetaryPayment> documentDetails = await client.BudgetaryPaymentManager.GetDetails(targetId);
		if (documentDetails != null && (documentDetails.State == DocumentState.PartiallySigned || documentDetails.State == DocumentState.ReadyToBeSent || documentDetails.State == DocumentState.Sent || documentDetails.State == DocumentState.InProcessBank))
		{
			canBeCanceled = true;
		}
		if (!canBeCanceled)
		{
			throw new ASException(Resources.PayDocCantBeCanceled);
		}
		DocumentForEdit<CancelRequestApplication> documentForEdit = await client.CancelRequestApplicationManager.GetNew(targetId);
		documentForEdit.Model.Date = DateTime.Now;
		documentForEdit.Model.Details = detail;
		documentForEdit.Model.TargetId = targetId;
		await client.CancelRequestApplicationManager.Create(documentForEdit.Model);
	}

	void IOpenBankingService.CancelMultiplePayment(string bankCode, Guid targetId, string detail)
	{
		Login(bankCode);
		AsyncHelper.RunSync(() => CancelMultiplePaymentAsync(bankCode, targetId, detail));
	}

	public async Task CancelMultiplePaymentAsync(string bankCode, Guid targetId, string detail)
	{
		BanksClients.TryGetValue(bankCode, out var client);
		bool canBeCanceled = false;
		DocumentDetails<MultiplePayment> documentDetails = await client.MultiplePaymentManager.GetDetails(targetId);
		if (documentDetails != null && (documentDetails.State == DocumentState.PartiallySigned || documentDetails.State == DocumentState.ReadyToBeSent || documentDetails.State == DocumentState.Sent || documentDetails.State == DocumentState.InProcessBank))
		{
			canBeCanceled = true;
		}
		if (!canBeCanceled)
		{
			throw new ASException(Resources.PayDocCantBeCanceled);
		}
		DocumentForEdit<CancelRequestApplication> documentForEdit = await client.CancelRequestApplicationManager.GetNew(targetId);
		documentForEdit.Model.Date = DateTime.Now;
		documentForEdit.Model.Details = detail;
		documentForEdit.Model.TargetId = targetId;
		await client.CancelRequestApplicationManager.Create(documentForEdit.Model);
	}
}
