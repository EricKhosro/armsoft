using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml;
using Ardalis.SmartEnum;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;
using ArmSoft.OnlineBanking.Common.Utility;
using ArmSoft.OpenBanking.Api.Client.Core;
using ArmSoft.OpenBanking.Api.Common;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.InternetBank;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.InternetBank;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.SmartEnums;

namespace ArmSoft.SME.Core.UI.Helpers;

public class InternetBankHelper
{
	private static List<string> _openBankingSupportedBanks;

	public static List<string> OpenBankingSupportedBanks
	{
		get
		{
			if (_openBankingSupportedBanks == null)
			{
				_openBankingSupportedBanks = (from p in Settings.OpenBankingService.GetSupportedBanks()
					select p.Id).ToList();
			}
			return _openBankingSupportedBanks;
		}
	}

	public static int BanksClientsCount()
	{
		return Settings.OpenBankingService.BanksClients.Count() + Settings.InternetBankService.BanksClients.Count();
	}

	public static IBConnectionModesSE GetBankConnectionMode(string bankCode)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (!string.IsNullOrWhiteSpace(Settings.ParametersService.BanksConnectionModes()))
		{
			dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(Settings.ParametersService.BanksConnectionModes());
		}
		if (dictionary.TryGetValue(bankCode, out var value))
		{
			if (value == IBConnectionModesSE.BankDefault.Value)
			{
				if (OpenBankingSupportedBanks.Contains(bankCode))
				{
					return IBConnectionModesSE.OpenBank;
				}
				return IBConnectionModesSE.InternetBank;
			}
			if (OpenBankingSupportedBanks.Contains(bankCode) && value == IBConnectionModesSE.OpenBank.Value)
			{
				return IBConnectionModesSE.OpenBank;
			}
			if (!OpenBankingSupportedBanks.Contains(bankCode) && value == IBConnectionModesSE.OpenBank.Value)
			{
				return IBConnectionModesSE.InternetBank;
			}
			if (value == IBConnectionModesSE.InternetBank.Value)
			{
				return IBConnectionModesSE.InternetBank;
			}
			return IBConnectionModesSE.NotConnected;
		}
		if (OpenBankingSupportedBanks.Contains(bankCode))
		{
			return IBConnectionModesSE.OpenBank;
		}
		return IBConnectionModesSE.InternetBank;
	}

	public static bool BankLogedIn(string bankCode)
	{
		if (!Settings.OpenBankingService.BanksClients.ContainsKey(bankCode))
		{
			return Settings.InternetBankService.BanksClients.ContainsKey(bankCode);
		}
		return true;
	}

	public static bool EnsureLoggedIn(HashSet<string> bankCodes)
	{
		foreach (string bankCode in bankCodes)
		{
			if (GetBankConnectionMode(bankCode) == IBConnectionModesSE.OpenBank.Value)
			{
				if (!Settings.OpenBankingService.Login(bankCode))
				{
					return false;
				}
			}
			else if (GetBankConnectionMode(bankCode) == IBConnectionModesSE.InternetBank.Value)
			{
				if (string.IsNullOrWhiteSpace(Settings.InternetBankService.EnsureLoggedIn(bankCode)))
				{
					return false;
				}
			}
			else if (GetBankConnectionMode(bankCode) == IBConnectionModesSE.NotConnected.Value)
			{
				throw new ASException(string.Format(Resources.IBConnectionModeNotSpecified, bankCode));
			}
		}
		return true;
	}

	public static List<ArmSoft.SME.Core.UI.InternetBank.Account> GetAccounts(HashSet<string> accountNumbers, bool includeBalances)
	{
		List<ArmSoft.SME.Core.UI.InternetBank.Account> list = new List<ArmSoft.SME.Core.UI.InternetBank.Account>();
		foreach (string accountNumber in accountNumbers)
		{
			string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
			if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
			{
				List<AccountListItem> accounts = Settings.OpenBankingService.GetAccounts(bankMainCode, includeBalances);
				list.AddRange(accounts.Select((AccountListItem p) => new ArmSoft.SME.Core.UI.InternetBank.Account
				{
					AccountNumber = p.Number,
					AccessLevel = p.AccessLevels.Convert(),
					Balance = p.Balance,
					Currency = p.Currency,
					DisplayAccountNumber = p.Number,
					Id = p.Id,
					InterestRate = p.InterestRate,
					IsGoalAccount = p.IsGoalAccount,
					Name = p.Name,
					Note = p.Note,
					ExpectedBalance = p.ExpectedBalance,
					DisplayType = p.DisplayType,
					GoalAmount = p.GoalAmount,
					Links = null,
					Type = p.Type.Convert()
				}));
			}
			else if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value)
			{
				List<ArmSoft.InternetBank.Web.API.Common.Models.Account> accounts2 = Settings.InternetBankService.GetAccounts(accountNumber, includeBalances);
				list.AddRange(accounts2.Select((ArmSoft.InternetBank.Web.API.Common.Models.Account p) => new ArmSoft.SME.Core.UI.InternetBank.Account
				{
					AccountNumber = p.AccountNumber,
					AccessLevel = p.AccessLevel.Convert(),
					Balance = p.Balance,
					Currency = p.Currency,
					DisplayAccountNumber = p.DisplayAccountNumber,
					Id = p.Id,
					InterestRate = p.InterestRate,
					IsGoalAccount = p.IsGoalAccount,
					Name = p.Name,
					Note = p.Note,
					ExpectedBalance = p.ExpectedBalance,
					DisplayType = p.DisplayType,
					GoalAmount = p.GoalAmount,
					Links = p.Links.Select((ArmSoft.InternetBank.Web.API.Common.Models.Link l) => new ArmSoft.SME.Core.UI.InternetBank.Link(l.Rel, l.HRef)).ToList(),
					Type = p.Type.Convert()
				}));
			}
			LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.AccountBalances);
		}
		return list;
	}

	public static ArmSoft.SME.Core.UI.InternetBank.AccountBalances GetAccountBalances(string accountNumber, DateTime? openingBalanceDate = null, DateTime? closingBalanceDate = null, bool writeLog = true)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
		{
			AccountBalanceRow accountBalances = Settings.OpenBankingService.GetAccountBalances(accountNumber, openingBalanceDate ?? DateTime.Today, closingBalanceDate ?? DateTime.Today);
			if (writeLog)
			{
				LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.AccountBalances);
			}
			return new ArmSoft.SME.Core.UI.InternetBank.AccountBalances
			{
				ClosingBalance = accountBalances.ClosingBalance,
				ClosingExpectedBalance = accountBalances.ClosingExpectedBalance,
				OpeningBalance = accountBalances.OpeningBalance,
				OpeningExpectedBalance = accountBalances.OpeningExpectedBalance
			};
		}
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value)
		{
			AccountBalanceRow accountBalances2 = Settings.InternetBankService.GetAccountBalances(accountNumber, openingBalanceDate ?? DateTime.Today, closingBalanceDate ?? DateTime.Today);
			if (writeLog)
			{
				LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.AccountBalances);
			}
			return new ArmSoft.SME.Core.UI.InternetBank.AccountBalances
			{
				ClosingBalance = accountBalances2.ClosingBalance,
				ClosingExpectedBalance = accountBalances2.ClosingExpectedBalance,
				OpeningBalance = accountBalances2.OpeningBalance,
				OpeningExpectedBalance = accountBalances2.OpeningExpectedBalance
			};
		}
		return null;
	}

	public static List<InternetBankTransaction> GetTransactions(DateOnly startDate, DateOnly endDate, List<string> accountNumbers, DebitCreditSE debitCredit = null, IBOperation ibOpType = IBOperation.TransactionsImport, bool alsoRevaluationType = false)
	{
		List<InternetBankTransaction> list = new List<InternetBankTransaction>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		foreach (string accountNumber2 in accountNumbers)
		{
			string text = GetBankConnectionMode(SettlementAccountHelper.GetBankMainCode(accountNumber2));
			if (text == IBConnectionModesSE.OpenBank.Value)
			{
				list2.Add(accountNumber2);
			}
			else if (text == IBConnectionModesSE.InternetBank.Value)
			{
				list3.Add(accountNumber2);
			}
		}
		TransactionFilterOption transactionFilterOption = TransactionFilterOption.All;
		if (debitCredit != null)
		{
			transactionFilterOption = ((!(debitCredit.Value == DebitCreditSE.Debit)) ? TransactionFilterOption.OnlyDebits : TransactionFilterOption.OnlyCredits);
		}
		foreach (string accountNumber in list2)
		{
			List<ArmSoft.OpenBanking.Api.Common.AccountTransaction> source = Settings.OpenBankingService.GetTransactions(startDate.ToDateTime(), endDate.ToDateTime(), accountNumber, transactionFilterOption);
			if (!alsoRevaluationType)
			{
				source = source.Where((ArmSoft.OpenBanking.Api.Common.AccountTransaction t) => t.OperationType != "RVL").ToList();
			}
			list.AddRange(source.Select((ArmSoft.OpenBanking.Api.Common.AccountTransaction t) => new InternetBankTransaction
			{
				Account = accountNumber,
				CreationDate = t.CreationDate,
				OperationDate = t.OperationDate,
				DebitAmountCur = t.CreditAmount,
				DebitAmount = t.CreditAmountAMD,
				CreditAmountCur = t.DebitAmount,
				CreditAmount = t.DebitAmountAMD,
				Currency = t.Currency,
				OperationType = t.OperationType,
				Comment = t.Comment,
				CorrespondentAccount = t.CorrespondentAccount,
				CorrespondentName = t.CorrespondentName,
				DocumentNumber = t.DocumentNumber,
				TransactionID = t.TransactionId,
				TransactionShortID = t.TransactionId.Substring(0, t.TransactionId.Length - 2),
				DocumentID = t.DocumentId
			}));
			LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, ibOpType);
		}
		List<InternetBankTransaction> list4 = Settings.InternetBankService.GetTransactions(startDate.ToDateTime(), endDate.ToDateTime(), list3, transactionFilterOption);
		if (!alsoRevaluationType)
		{
			list4 = list4.Where((InternetBankTransaction t) => t.OperationType != "RVL").ToList();
		}
		list.AddRange(list4);
		foreach (string item in list3)
		{
			LogIBOperation(Settings.ParametersService.TaxCode(), item, ibOpType);
		}
		return list;
	}

	public static List<DepositRow> GetDeposits(string bankCode)
	{
		List<DepositRow> list = new List<DepositRow>();
		if (GetBankConnectionMode(bankCode) == IBConnectionModesSE.OpenBank.Value)
		{
			List<ArmSoft.OpenBanking.Api.Common.Deposit> deposits = Settings.OpenBankingService.GetDeposits(bankCode);
			ContractOnlineDataListResult<DepositOnlineData> depositsDetails = Settings.OpenBankingService.GetDepositsOnlineDataList(bankCode, deposits.Select((ArmSoft.OpenBanking.Api.Common.Deposit d) => d.Id).ToList());
			list.AddRange(deposits.Select((ArmSoft.OpenBanking.Api.Common.Deposit p) => new DepositRow
			{
				DisplayContract = p.ContractNumber,
				DisplayCategory = p.DisplayCategory,
				Currency = p.Currency,
				OriginalDate = p.OriginalDate.ToDateOnly(),
				MaturityDate = depositsDetails.Contracts.FirstOrDefault((DepositOnlineData c) => c.Id == p.Id).MaturityDate.ToDateOnly(),
				OriginalPrincipal = p.OriginalPrincipal,
				InterestRate = depositsDetails.Contracts.FirstOrDefault((DepositOnlineData c) => c.Id == p.Id).InterestRate,
				OutstandingPrincipal = p.OriginalPrincipal,
				CalculatedInterest = depositsDetails.Contracts.FirstOrDefault((DepositOnlineData c) => c.Id == p.Id).CalculatedInterest,
				ClosingDate = p.ClosingDate.ToDateOnly(),
				IsReplenishable = p.IsReplenishable,
				IsCapitalizable = p.IsCapitalizable,
				IsExtendable = p.IsExtendable,
				PayPeriodName = Decode.InterestPayPeriod(p.PayPeriod)
			}));
			LogIBOperation(Settings.ParametersService.TaxCode(), bankCode, IBOperation.Deposits);
		}
		else if (GetBankConnectionMode(bankCode) == IBConnectionModesSE.InternetBank.Value)
		{
			List<ArmSoft.InternetBank.Web.API.Common.Models.Deposit> deposits2 = Settings.InternetBankService.GetDeposits(bankCode);
			list.AddRange(deposits2.Select((ArmSoft.InternetBank.Web.API.Common.Models.Deposit p) => new DepositRow
			{
				DisplayContract = p.DisplayContract,
				DisplayCategory = p.DisplayCategory,
				Currency = p.Currency,
				OriginalDate = p.OriginalDate.ToDateOnly(),
				MaturityDate = p.MaturityDate.ToDateOnly(),
				OriginalPrincipal = p.OriginalPrincipal,
				InterestRate = p.InterestRate,
				OutstandingPrincipal = p.OriginalPrincipal,
				CalculatedInterest = p.CalculatedInterest,
				ClosingDate = p.ClosingDate.ToDateOnly(),
				IsReplenishable = p.IsReplenishable,
				IsCapitalizable = p.IsCapitalizable,
				IsExtendable = p.IsExtendable,
				PayPeriodName = Decode.InterestPayPeriod(p.PayPeriod)
			}));
			LogIBOperation(Settings.ParametersService.TaxCode(), bankCode, IBOperation.Deposits);
		}
		return list;
	}

	public static List<CreditRow> GetCredits(string bankCode)
	{
		List<CreditRow> list = new List<CreditRow>();
		if (GetBankConnectionMode(bankCode) == IBConnectionModesSE.OpenBank.Value)
		{
			List<ArmSoft.OpenBanking.Api.Common.Credit> credits = Settings.OpenBankingService.GetCredits(bankCode);
			ContractOnlineDataListResult<CreditOnlineData> creditsDetails = Settings.OpenBankingService.GetCreditsOnlineDataList(bankCode, credits.Select((ArmSoft.OpenBanking.Api.Common.Credit d) => d.Id).ToList());
			list.AddRange(credits.Select((ArmSoft.OpenBanking.Api.Common.Credit p) => new CreditRow
			{
				DisplayContract = p.ContractNumber,
				CreditCode = creditsDetails.Contracts.FirstOrDefault((CreditOnlineData c) => c.Id == p.Id).CreditCode,
				DisplayCategory = p.DisplayCategory,
				OriginalDate = p.OriginalDate.ToDateOnly(),
				MaturityDate = creditsDetails.Contracts.FirstOrDefault((CreditOnlineData c) => c.Id == p.Id).MaturityDate.ToDateOnly(),
				OriginalPrincipal = p.OriginalPrincipal,
				Currency = p.Currency,
				InterestRate = creditsDetails.Contracts.FirstOrDefault((CreditOnlineData c) => c.Id == p.Id).InterestRate,
				OutstandingPrincipal = creditsDetails.Contracts.FirstOrDefault((CreditOnlineData c) => c.Id == p.Id).OutstandingPrincipal,
				CalculatedInterest = creditsDetails.Contracts.FirstOrDefault((CreditOnlineData c) => c.Id == p.Id).CalculatedInterest
			}));
			LogIBOperation(Settings.ParametersService.TaxCode(), bankCode, IBOperation.Loans);
		}
		else if (GetBankConnectionMode(bankCode) == IBConnectionModesSE.InternetBank.Value)
		{
			List<ArmSoft.InternetBank.Web.API.Common.Models.Credit> credits2 = Settings.InternetBankService.GetCredits(bankCode);
			list.AddRange(credits2.Select((ArmSoft.InternetBank.Web.API.Common.Models.Credit p) => new CreditRow
			{
				DisplayContract = p.DisplayContract,
				CreditCode = p.CreditCode,
				DisplayCategory = p.DisplayCategory,
				OriginalDate = p.OriginalDate.ToDateOnly(),
				MaturityDate = p.MaturityDate.ToDateOnly(),
				OriginalPrincipal = p.OriginalPrincipal,
				Currency = p.Currency,
				InterestRate = p.InterestRate,
				OutstandingPrincipal = p.OutstandingPrincipal,
				CalculatedInterest = p.CalculatedInterest
			}));
			LogIBOperation(Settings.ParametersService.TaxCode(), bankCode, IBOperation.Loans);
		}
		return list;
	}

	public static List<CardRow> GetCards(string bankCode)
	{
		List<CardRow> list = new List<CardRow>();
		if (GetBankConnectionMode(bankCode) == IBConnectionModesSE.OpenBank.Value)
		{
			List<ArmSoft.OpenBanking.Api.Common.Card> cards = Settings.OpenBankingService.GetCards(bankCode);
			list.AddRange(cards.Select((ArmSoft.OpenBanking.Api.Common.Card p) => new CardRow
			{
				DisplayCardNumber = p.DisplayNumber,
				DisplayAccountNumber = p.AccountNumber,
				Currency = p.Currency,
				DisplayCardType = p.DisplayType,
				Name = p.Name,
				EMail = p.Email,
				FromDate = p.ValidFrom.ToDateOnly(),
				ToDate = p.ValidThru.ToDateOnly(),
				Note = p.Note,
				MainCardNumber = p.DisplayMainCardNumber,
				AccountBalance = p.AccountBalance,
				ExpectedBalance = p.AccountExpectedBalance
			}));
			LogIBOperation(Settings.ParametersService.TaxCode(), bankCode, IBOperation.Cards);
		}
		else if (GetBankConnectionMode(bankCode) == IBConnectionModesSE.InternetBank.Value)
		{
			List<ArmSoft.InternetBank.Web.API.Common.Models.Card> cards2 = Settings.InternetBankService.GetCards(bankCode);
			list.AddRange(cards2.Select((ArmSoft.InternetBank.Web.API.Common.Models.Card p) => new CardRow
			{
				DisplayCardNumber = p.DisplayCardNumber,
				DisplayAccountNumber = p.DisplayAccountNumber,
				Currency = p.Currency,
				DisplayCardType = p.DisplayCardType,
				Name = p.Name,
				EMail = p.EMail,
				FromDate = p.FromDate.ToDateOnly(),
				ToDate = p.ToDate.ToDateOnly(),
				Note = p.Note,
				MainCardNumber = p.MainCardNumber,
				AccountBalance = p.AccountBalance,
				ExpectedBalance = p.ExpectedBalance
			}));
			LogIBOperation(Settings.ParametersService.TaxCode(), bankCode, IBOperation.Cards);
		}
		return list;
	}

	public static List<BondRow> GetBonds(string bankCode)
	{
		List<BondRow> list = new List<BondRow>();
		if (GetBankConnectionMode(bankCode) == IBConnectionModesSE.OpenBank.Value)
		{
			List<ArmSoft.OpenBanking.Api.Common.Bond> bonds = Settings.OpenBankingService.GetBonds(bankCode);
			ContractOnlineDataListResult<BondOnlineData> bondsDetails = Settings.OpenBankingService.GetBondsOnlineDataList(bankCode, bonds.Select((ArmSoft.OpenBanking.Api.Common.Bond d) => d.Id).ToList());
			list.AddRange(bonds.Select((ArmSoft.OpenBanking.Api.Common.Bond p) => new BondRow
			{
				Isin = p.Isin,
				DisplayContract = p.ContractNumber,
				OriginalDate = p.OriginalDate.ToDateOnly(),
				MaturityDate = bondsDetails.Contracts.FirstOrDefault((BondOnlineData c) => c.Id == p.Id).MaturityDate.ToDateOnly(),
				Currency = p.Currency,
				InterestRate = bondsDetails.Contracts.FirstOrDefault((BondOnlineData c) => c.Id == p.Id).InterestRate,
				ClosingDate = p.ClosingDate.ToDateOnly(),
				OutstandingPrincipal = bondsDetails.Contracts.FirstOrDefault((BondOnlineData c) => c.Id == p.Id).OutstandingPrincipal,
				CalculatedInterest = bondsDetails.Contracts.FirstOrDefault((BondOnlineData c) => c.Id == p.Id).CalculatedInterest
			}));
			LogIBOperation(Settings.ParametersService.TaxCode(), bankCode, IBOperation.Bonds);
		}
		else if (GetBankConnectionMode(bankCode) == IBConnectionModesSE.InternetBank.Value)
		{
			List<ArmSoft.InternetBank.Web.API.Common.Models.Bond> bonds2 = Settings.InternetBankService.GetBonds(bankCode);
			list.AddRange(bonds2.Select((ArmSoft.InternetBank.Web.API.Common.Models.Bond p) => new BondRow
			{
				Isin = p.Isin,
				DisplayContract = p.DisplayContract,
				OriginalDate = p.OriginalDate.ToDateOnly(),
				MaturityDate = p.MaturityDate.ToDateOnly(),
				Currency = p.Currency,
				InterestRate = p.InterestRate,
				ClosingDate = p.ClosingDate.ToDateOnly(),
				OutstandingPrincipal = p.OutstandingPrincipal,
				CalculatedInterest = p.CalculatedInterest
			}));
			LogIBOperation(Settings.ParametersService.TaxCode(), bankCode, IBOperation.Bonds);
		}
		return list;
	}

	public static List<PurchasedSecurityRow> GetPurchasedSecurities(string bankCode)
	{
		List<PurchasedSecurityRow> list = new List<PurchasedSecurityRow>();
		if (GetBankConnectionMode(bankCode) == IBConnectionModesSE.OpenBank.Value)
		{
			List<ArmSoft.OpenBanking.Api.Common.PurchasedSecurity> purchasedSecurities = Settings.OpenBankingService.GetPurchasedSecurities(bankCode);
			list.AddRange(purchasedSecurities.Select((ArmSoft.OpenBanking.Api.Common.PurchasedSecurity p) => new PurchasedSecurityRow
			{
				ContractNumber = p.ContractNumber,
				IssuanceNumber = p.IssuanceNumber,
				Type = p.Type,
				Currency = p.Currency,
				InterestRate = p.InterestRate,
				NominalValue = p.NominalValue,
				FreePartAmount = p.FreePartAmount,
				Count = p.Count,
				PledgedPartAmount = p.PledgedPartAmount,
				IssueDate = p.IssueDate.ToDateOnly(),
				PurchaseDate = p.PurchaseDate.ToDateOnly(),
				MaturityDate = p.MaturityDate.ToDateOnly(),
				PayPeriodName = Decode.InterestPayPeriod(p.PayPeriod)
			}));
			LogIBOperation(Settings.ParametersService.TaxCode(), bankCode, IBOperation.PurchasedSecurities);
		}
		else if (GetBankConnectionMode(bankCode) == IBConnectionModesSE.InternetBank.Value)
		{
			List<ArmSoft.InternetBank.Web.API.Common.Models.PurchasedSecurity> purchasedSecurities2 = Settings.InternetBankService.GetPurchasedSecurities(bankCode);
			list.AddRange(purchasedSecurities2.Select((ArmSoft.InternetBank.Web.API.Common.Models.PurchasedSecurity p) => new PurchasedSecurityRow
			{
				ContractNumber = p.ContractNumber,
				IssuanceNumber = p.IssuanceNumber,
				Type = p.Type,
				Currency = p.Currency,
				InterestRate = p.InterestRate,
				NominalValue = p.NominalValue,
				FreePartAmount = p.FreePartAmount,
				Count = p.Count,
				PledgedPartAmount = p.PledgedPartAmount,
				IssueDate = p.IssueDate.ToDateOnly(),
				PurchaseDate = p.PurchaseDate.ToDateOnly(),
				MaturityDate = p.MaturityDate.ToDateOnly(),
				PayPeriodName = Decode.InterestPayPeriod(p.PayPeriod)
			}));
			LogIBOperation(Settings.ParametersService.TaxCode(), bankCode, IBOperation.PurchasedSecurities);
		}
		return list;
	}

	public static List<ApplicationRow> GetApplications(string bankCode, DateOnly startDate, DateOnly endDate)
	{
		List<ApplicationRow> list = new List<ApplicationRow>();
		if (GetBankConnectionMode(bankCode) == IBConnectionModesSE.OpenBank.Value)
		{
			List<ArmSoft.OpenBanking.Api.Common.DocumentListItem> applications = Settings.OpenBankingService.GetApplications(bankCode, startDate.ToDateTime(), endDate.ToDateTime());
			list.AddRange(applications.Select((ArmSoft.OpenBanking.Api.Common.DocumentListItem a) => new ApplicationRow
			{
				Date = a.Date.ToDateOnly(),
				DocNum = a.Number,
				DisplayType = a.DisplayType,
				Details = a.Details,
				DisplayState = a.DisplayState,
				Comments = a.Comments,
				FirstSignature = a.AreFirstSignaturesCompleted,
				SecondSignature = a.AreSecondSignaturesCompleted
			}));
			LogIBOperation(Settings.ParametersService.TaxCode(), bankCode, IBOperation.Applications);
		}
		else if (GetBankConnectionMode(bankCode) == IBConnectionModesSE.InternetBank.Value)
		{
			List<ArmSoft.InternetBank.Web.API.Common.Models.Application> applications2 = Settings.InternetBankService.GetApplications(bankCode, startDate.ToDateTime(), endDate.ToDateTime());
			list.AddRange(applications2.Select((ArmSoft.InternetBank.Web.API.Common.Models.Application a) => new ApplicationRow
			{
				Date = a.Date.ToDateOnly(),
				DocNum = a.DocNum,
				DisplayType = a.DisplayType,
				Details = a.Details,
				DisplayState = a.DisplayState,
				Comments = a.Comments,
				FirstSignature = a.AreFirstSignaturesCompleted,
				SecondSignature = a.AreSecondSignaturesCompleted
			}));
			LogIBOperation(Settings.ParametersService.TaxCode(), bankCode, IBOperation.Applications);
		}
		return list;
	}

	public static List<MerchantTransactionRow> GetMerchantTransactions(DateOnly startDate, DateOnly endDate, string accountNumber)
	{
		List<MerchantTransactionRow> list = new List<MerchantTransactionRow>();
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
		{
			List<ArmSoft.OpenBanking.Api.Common.MerchantTransaction> merchantTransactions = Settings.OpenBankingService.GetMerchantTransactions(startDate.ToDateTime(), endDate.ToDateTime(), accountNumber);
			list.AddRange(merchantTransactions.Select((ArmSoft.OpenBanking.Api.Common.MerchantTransaction t) => new MerchantTransactionRow
			{
				OperationDate = t.OperationDate.ToDateOnly(),
				TransactionDate = (DateTime.TryParse(t.TransactionDate, out var result) ? new DateTime?(result) : ((DateTime?)null)),
				TransactionType = t.TransactionDirection,
				Currency = t.Currency,
				Amount = t.Amount,
				MerchantFee = t.MerchantFee,
				AmountWithoutMerchantFee = t.AmountWithoutMerchantFee,
				MerchantName = t.MerchantName,
				CardNumber = t.CardNumber,
				CardTypeName = t.CardTypeName,
				CardBelongingName = t.CardBelongingName,
				TokenPan = t.TokenPan,
				RRN = t.TransactionReferenceNumber,
				TerminalId = t.TerminalId,
				AuthorizationCode = t.AuthorizationCode
			}));
			LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.MerchantStatement);
		}
		else if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value)
		{
			list = Settings.InternetBankService.GetMerchantTransactions(startDate.ToDateTime(), endDate.ToDateTime(), accountNumber);
			LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.MerchantStatement);
		}
		return list;
	}

	public static RateRow GetRate(string bankCode, string currency1, string currency2)
	{
		RateRow result = new RateRow();
		if (GetBankConnectionMode(bankCode) == IBConnectionModesSE.OpenBank.Value)
		{
			ArmSoft.OpenBanking.Api.Common.Rate clientRate = Settings.OpenBankingService.GetClientRate(bankCode, currency1, currency2);
			result = new RateRow
			{
				Currency1 = clientRate.Currency1,
				Currency2 = clientRate.Currency2,
				AmountCurrency = clientRate.AmountCurrency,
				RateFor = clientRate.RateFor,
				RateLabel = clientRate.RateLabel,
				RateValue = clientRate.RateValue
			};
		}
		else if (GetBankConnectionMode(bankCode) == IBConnectionModesSE.InternetBank.Value)
		{
			ClientRate clientRate2 = Settings.InternetBankService.GetClientRate(bankCode, currency1, currency2);
			result = new RateRow
			{
				Currency1 = clientRate2.Currency1,
				Currency2 = clientRate2.Currency2,
				AmountCurrency = clientRate2.AmountCurrency,
				RateFor = clientRate2.RateFor,
				RateLabel = clientRate2.RateLabel,
				RateValue = clientRate2.OfferedRateValue
			};
		}
		return result;
	}

	public static ArmSoft.SME.Core.UI.InternetBank.AccountDetails GetAccountDetails(string accountNumber)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
		{
			ArmSoft.OpenBanking.Api.Common.Account account = Settings.OpenBankingService.GetAccount(accountNumber);
			return new ArmSoft.SME.Core.UI.InternetBank.AccountDetails
			{
				AccessLevel = account.AccessLevels.Convert(),
				AccountNumber = account.Number,
				Currency = account.Currency,
				Balance = account.CurrentBalance.GetValueOrDefault(),
				DisplayAccountNumber = account.Number,
				Id = account.Id,
				InterestRate = account.InterestRate,
				IsGoalAccount = account.IsGoalAccount,
				Name = account.Name,
				Note = account.Note,
				ExpectedBalance = account.ExpectedBalance,
				DisplayType = account.DisplayType,
				GoalAmount = account.GoalAmount,
				Links = null,
				Type = account.Type.Convert(),
				OverdraftInfo = ((account.OverdraftInfo == null) ? null : new ArmSoft.SME.Core.UI.InternetBank.AccountOverdraftInfo
				{
					ContractAmount = account.OverdraftInfo.ContractAmount,
					DebtAmount = account.OverdraftInfo.DebtAmount,
					DebtDate = account.OverdraftInfo.DebtDate,
					DebtInterestAmount = account.OverdraftInfo.DebtInterestAmount,
					RepaymentDate = account.OverdraftInfo.RepaymentDate,
					SigningDate = account.OverdraftInfo.SigningDate,
					UsedAmount = account.OverdraftInfo.UsedAmount
				})
			};
		}
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value)
		{
			ArmSoft.InternetBank.Web.API.Common.Models.AccountDetails accountDetails = Settings.InternetBankService.GetAccountDetails(accountNumber);
			return new ArmSoft.SME.Core.UI.InternetBank.AccountDetails
			{
				AccessLevel = accountDetails.AccessLevel.Convert(),
				AccountNumber = accountDetails.AccountNumber,
				Currency = accountDetails.Currency,
				Balance = accountDetails.Balance,
				DisplayAccountNumber = accountDetails.DisplayAccountNumber,
				Id = accountDetails.Id,
				InterestRate = accountDetails.InterestRate,
				IsGoalAccount = accountDetails.IsGoalAccount,
				Name = accountDetails.Name,
				Note = accountDetails.Note,
				ExpectedBalance = accountDetails.ExpectedBalance,
				DisplayType = accountDetails.DisplayType,
				GoalAmount = accountDetails.GoalAmount,
				Links = accountDetails.Links.Select((ArmSoft.InternetBank.Web.API.Common.Models.Link l) => new ArmSoft.SME.Core.UI.InternetBank.Link(l.Rel, l.HRef)).ToList(),
				Type = accountDetails.Type.Convert(),
				OverdraftInfo = ((accountDetails.OverdraftInfo == null) ? null : new ArmSoft.SME.Core.UI.InternetBank.AccountOverdraftInfo
				{
					ContractAmount = accountDetails.OverdraftInfo.ContractAmount,
					DebtAmount = accountDetails.OverdraftInfo.DebtAmount,
					DebtDate = accountDetails.OverdraftInfo.DebtDate,
					DebtInterestAmount = accountDetails.OverdraftInfo.DebtInterestAmount,
					RepaymentDate = accountDetails.OverdraftInfo.RepaymentDate,
					SigningDate = accountDetails.OverdraftInfo.SigningDate,
					UsedAmount = accountDetails.OverdraftInfo.UsedAmount
				})
			};
		}
		return null;
	}

	public static List<PaymentRow> GetPayments(DateOnly startDate, DateOnly endDate, string accountNumber)
	{
		List<PaymentRow> list = new List<PaymentRow>();
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
		{
			List<ArmSoft.OpenBanking.Api.Common.DocumentListItem> payments = Settings.OpenBankingService.GetPayments(startDate.ToDateTime(), endDate.ToDateTime(), accountNumber);
			list.AddRange(payments.Select((ArmSoft.OpenBanking.Api.Common.DocumentListItem p) => new PaymentRow
			{
				Amount = p.Amount.Value,
				CanBeSigned = p.CanBeSigned,
				Comments = p.Comments,
				Currency = p.Currency,
				Date = p.Date.ToDateOnly(),
				Details = p.Details,
				DisplayState = p.DisplayState,
				DisplaySubState = p.DisplaySubState,
				StateWithText = p.DisplayState,
				DisplayType = p.DisplayType,
				DocId = p.Id,
				DocNum = p.Number,
				PayerAccOrCard = p.PayerAccountOrCard,
				Receiver = p.Receiver,
				ReceiverAccOrCard = p.ReceiverId,
				State = Convert.ToByte(p.State),
				Type = p.Type,
				TimeStamp = p.Version,
				UtilityCode = p.UtilityCode,
				Links = null
			}));
			LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.PaymentsAndTransfers);
		}
		else if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value)
		{
			List<ArmSoft.InternetBank.Web.API.Common.Models.Payment> payments2 = Settings.InternetBankService.GetPayments(startDate.ToDateTime(), endDate.ToDateTime(), accountNumber);
			list.AddRange(payments2.Select((ArmSoft.InternetBank.Web.API.Common.Models.Payment p) => new PaymentRow
			{
				Amount = p.Amount,
				CanBeSigned = p.CanBeSigned,
				Comments = p.Comments,
				CreatedByAdmin = p.CreatedByAdmin,
				Currency = p.Currency,
				Date = p.Date.ToDateOnly(),
				Details = p.Details,
				DisplayState = p.DisplayState,
				DisplaySubState = p.DisplaySubState,
				StateWithText = p.DisplayState,
				DisplayType = p.DisplayType,
				DocId = p.DocId,
				DocIdString = p.DocIdString,
				DocNum = p.DocNum,
				PayerAccOrCard = p.PayerAccOrCard,
				Receiver = p.Receiver,
				ReceiverAccOrCard = p.ReceiverAccOrCard,
				State = Convert.ToByte(p.State),
				Template = p.Template,
				TimeStamp = p.TimeStamp,
				Type = p.Type,
				UtilityCode = p.UtilityCode,
				Links = p.Links.Select((ArmSoft.InternetBank.Web.API.Common.Models.Link l) => new ArmSoft.SME.Core.UI.InternetBank.Link(l.Rel, l.HRef)).ToList()
			}));
			LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.PaymentsAndTransfers);
		}
		return list;
	}

	public static List<ImportedIBTransactionsData> GetImportedIBTransactionsData(DateOnly startDate, DateOnly endDate, List<string> accountNumbers)
	{
		return Settings.CreateApiClient().InternetBank.GetImportedIBTransactionsData(startDate, endDate, accountNumbers);
	}

	public static List<Guid> ExportPaymentsToInternetBank(List<IBPayment> payOrdersList, EnumerateErrorsReportViewer repViewer, IBOperation ibOperationType)
	{
		List<Guid> list = new List<Guid>();
		List<IBPayment> list2 = new List<IBPayment>();
		List<IBPayment> list3 = new List<IBPayment>();
		HashSet<string> hashSet = new HashSet<string>();
		foreach (IBPayment payOrders in payOrdersList)
		{
			string bankMainCode = SettlementAccountHelper.GetBankMainCode(payOrders.PayerAcc);
			hashSet.Add(bankMainCode);
			if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
			{
				list3.Add(payOrders);
			}
			else if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value)
			{
				list2.Add(payOrders);
			}
		}
		if (EnsureLoggedIn(hashSet))
		{
			foreach (IBPayment item in list2)
			{
				try
				{
					Settings.InternetBankService.CreatePayment(item);
					list.Add(item.DocId);
				}
				catch (Exception ex)
				{
					if (!(ex is InvisibleException))
					{
						string text = ex.Message;
						if (ex is ArmSoft.InternetBank.APIClient.Core.ApiException ex2)
						{
							text = GetIBApiExceptionMsg(ex2);
						}
						string message = string.Format(Resources.DocDescrWithDate, SmartEnumHelper.GetSmartEnumRowCaption(typeof(DocumentTypeSE), item.DocType.ToString()), item.DocNum, item.Date.FormatDDMMYYYY()) + "\r\n" + text;
						repViewer.AddError(message, item.DocId);
					}
				}
				LogIBOperation(item.PayerTaxCode, item.PayerAcc, ibOperationType);
			}
			foreach (IBPayment item2 in list3)
			{
				try
				{
					Settings.OpenBankingService.CreatePayment(item2);
					list.Add(item2.DocId);
				}
				catch (Exception ex3)
				{
					if (!(ex3 is InvisibleException))
					{
						string text2 = ex3.Message;
						if (ex3 is ArmSoft.OpenBanking.Api.Client.Core.ApiException ex4)
						{
							text2 = GetOBApiExceptionMsg(ex4);
						}
						string message2 = string.Format(Resources.DocDescrWithDate, SmartEnumHelper.GetSmartEnumRowCaption(typeof(DocumentTypeSE), item2.DocType.ToString()), item2.DocNum, item2.Date.FormatDDMMYYYY()) + "\r\n" + text2;
						repViewer.AddError(message2, item2.DocId);
					}
				}
				LogIBOperation(item2.PayerTaxCode, item2.PayerAcc, ibOperationType);
			}
		}
		return list;
	}

	public static Guid ExportPaymentToInternetBank(IBPayment payOrder, IBOperation ibOperationType)
	{
		Guid result = Guid.Empty;
		bool flag = false;
		HashSet<string> hashSet = new HashSet<string>();
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(payOrder.PayerAcc);
		hashSet.Add(bankMainCode);
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
		{
			flag = true;
		}
		if (EnsureLoggedIn(hashSet))
		{
			if (!flag)
			{
				try
				{
					Settings.InternetBankService.CreatePayment(payOrder);
					result = payOrder.DocId;
				}
				catch (Exception ex)
				{
					if (!(ex is InvisibleException))
					{
						string text = ex.Message;
						if (ex is ArmSoft.InternetBank.APIClient.Core.ApiException ex2)
						{
							text = GetIBApiExceptionMsg(ex2);
						}
						throw new ASException(string.Format(Resources.DocDescrWithDate, SmartEnumHelper.GetSmartEnumRowCaption(typeof(DocumentTypeSE), payOrder.DocType.ToString()), payOrder.DocNum, payOrder.Date.FormatDDMMYYYY()) + "\r\n" + text);
					}
				}
				LogIBOperation(payOrder.PayerTaxCode, payOrder.PayerAcc, ibOperationType);
			}
			else
			{
				try
				{
					Settings.OpenBankingService.CreatePayment(payOrder);
					result = payOrder.DocId;
				}
				catch (Exception ex3)
				{
					if (!(ex3 is InvisibleException))
					{
						string text2 = ex3.Message;
						if (ex3 is ArmSoft.OpenBanking.Api.Client.Core.ApiException ex4)
						{
							text2 = GetOBApiExceptionMsg(ex4);
						}
						throw new ASException(string.Format(Resources.DocDescrWithDate, SmartEnumHelper.GetSmartEnumRowCaption(typeof(DocumentTypeSE), payOrder.DocType.ToString()), payOrder.DocNum, payOrder.Date.FormatDDMMYYYY()) + "\r\n" + text2);
					}
				}
				LogIBOperation(payOrder.PayerTaxCode, payOrder.PayerAcc, ibOperationType);
			}
		}
		return result;
	}

	public static List<Guid> ExportWGPaymentsToInternetBank(List<IBWGPayment> wgPayList, EnumerateErrorsReportViewer repViewer)
	{
		List<Guid> list = new List<Guid>();
		List<IBWGPayment> list2 = new List<IBWGPayment>();
		List<IBWGPayment> list3 = new List<IBWGPayment>();
		HashSet<string> hashSet = new HashSet<string>();
		foreach (IBWGPayment wgPay in wgPayList)
		{
			string bankMainCode = SettlementAccountHelper.GetBankMainCode(wgPay.PayerAcc);
			hashSet.Add(bankMainCode);
			if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
			{
				list3.Add(wgPay);
			}
			else if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value)
			{
				list2.Add(wgPay);
			}
		}
		if (EnsureLoggedIn(hashSet))
		{
			foreach (IBWGPayment item in list2)
			{
				try
				{
					Settings.InternetBankService.CreateMultiplePayment(item);
					list.Add(item.DocId);
				}
				catch (Exception ex)
				{
					if (!(ex is InvisibleException))
					{
						string text = ex.Message;
						if (ex is ArmSoft.InternetBank.APIClient.Core.ApiException ex2)
						{
							text = GetIBApiExceptionMsg(ex2);
						}
						string message = string.Format(Resources.DocDescrWithDate, SmartEnumHelper.GetSmartEnumRowCaption(typeof(DocumentTypeSE), item.DocType.ToString()), item.DocNum, item.Date.FormatDDMMYYYY()) + "\r\n" + text;
						repViewer.AddError(message, item.DocId);
					}
				}
				LogIBOperation(item.TaxCode, item.PayerAcc, IBOperation.PaymentsFromAccount, item.Rows.Count());
			}
			foreach (IBWGPayment item2 in list3)
			{
				try
				{
					Settings.OpenBankingService.CreateMultiplePayment(item2);
					list.Add(item2.DocId);
				}
				catch (Exception ex3)
				{
					if (!(ex3 is InvisibleException))
					{
						string text2 = ex3.Message;
						if (ex3 is ArmSoft.OpenBanking.Api.Client.Core.ApiException ex4)
						{
							text2 = GetOBApiExceptionMsg(ex4);
						}
						string message2 = string.Format(Resources.DocDescrWithDate, SmartEnumHelper.GetSmartEnumRowCaption(typeof(DocumentTypeSE), item2.DocType.ToString()), item2.DocNum, item2.Date.FormatDDMMYYYY()) + "\r\n" + text2;
						repViewer.AddError(message2, item2.DocId);
					}
				}
				LogIBOperation(item2.TaxCode, item2.PayerAcc, IBOperation.PaymentsFromAccount, item2.Rows.Count());
			}
		}
		return list;
	}

	public static List<Guid> CreateCurrencyConversion(IBCurrConversion currConversion)
	{
		List<Guid> list = new List<Guid>();
		new List<IBPayment>();
		new List<IBPayment>();
		HashSet<string> hashSet = new HashSet<string>();
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(currConversion.PayerAcc);
		hashSet.Add(bankMainCode);
		if (EnsureLoggedIn(hashSet))
		{
			if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
			{
				Settings.OpenBankingService.CreateCurrencyConversion(currConversion);
				list.Add(currConversion.DocId);
			}
			else if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value)
			{
				Settings.InternetBankService.CreateCurrencyConversion(currConversion);
				list.Add(currConversion.DocId);
			}
			LogIBOperation(Settings.ParametersService.TaxCode(), currConversion.PayerAcc, IBOperation.CurrencyConversion);
		}
		return list;
	}

	public static Guid ExportWGPaymentToInternetBank(IBWGPayment wgPay)
	{
		Guid result = Guid.Empty;
		bool flag = false;
		HashSet<string> hashSet = new HashSet<string>();
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(wgPay.PayerAcc);
		hashSet.Add(bankMainCode);
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
		{
			flag = true;
		}
		if (EnsureLoggedIn(hashSet))
		{
			if (!flag)
			{
				try
				{
					Settings.InternetBankService.CreateMultiplePayment(wgPay);
					result = wgPay.DocId;
				}
				catch (Exception ex)
				{
					if (!(ex is InvisibleException))
					{
						string text = ex.Message;
						if (ex is ArmSoft.InternetBank.APIClient.Core.ApiException ex2)
						{
							text = GetIBApiExceptionMsg(ex2);
						}
						throw new ASException(string.Format(Resources.DocDescrWithDate, SmartEnumHelper.GetSmartEnumRowCaption(typeof(DocumentTypeSE), wgPay.DocType.ToString()), wgPay.DocNum, wgPay.Date.FormatDDMMYYYY()) + "\r\n" + text);
					}
				}
				LogIBOperation(wgPay.TaxCode, wgPay.PayerAcc, IBOperation.PaymentsFromAccount, wgPay.Rows.Count());
			}
			else
			{
				try
				{
					Settings.OpenBankingService.CreateMultiplePayment(wgPay);
					result = wgPay.DocId;
				}
				catch (Exception ex3)
				{
					if (!(ex3 is InvisibleException))
					{
						string text2 = ex3.Message;
						if (ex3 is ArmSoft.OpenBanking.Api.Client.Core.ApiException ex4)
						{
							text2 = GetOBApiExceptionMsg(ex4);
						}
						throw new ASException(string.Format(Resources.DocDescrWithDate, SmartEnumHelper.GetSmartEnumRowCaption(typeof(DocumentTypeSE), wgPay.DocType.ToString()), wgPay.DocNum, wgPay.Date.FormatDDMMYYYY()) + "\r\n" + text2);
					}
				}
				LogIBOperation(wgPay.TaxCode, wgPay.PayerAcc, IBOperation.PaymentsFromAccount, wgPay.Rows.Count());
			}
		}
		return result;
	}

	public static string GetIBApiExceptionMsg(ArmSoft.InternetBank.APIClient.Core.ApiException ex)
	{
		string text = ex.Message;
		if (ex is ApiValidationException ex2)
		{
			text = ex2.Error.Message;
			for (int i = 0; i <= ex2.Error.FieldErrors.Count - 1; i++)
			{
				text += $"\r\n{i + 1}. {string.Format(ex2.Error.FieldErrors[i].DecodedMessage.Trim(), ex2.Error.FieldErrors[i].Field, ex2.Error.FieldErrors[i].Field2)}";
			}
		}
		return text;
	}

	public static string GetOBApiExceptionMsg(ArmSoft.OpenBanking.Api.Client.Core.ApiException ex)
	{
		string text = ex.Message;
		if (ex is ValidationException ex2)
		{
			text = ex2.Message;
			for (int i = 0; i <= ex2.ValidationResult.FieldErrors.Count - 1; i++)
			{
				text += $"\r\n{i + 1}. {string.Format(ex2.ValidationResult.FieldErrors[i].DecodedMessage.Trim(), ex2.ValidationResult.FieldErrors[i].Field, ex2.ValidationResult.FieldErrors[i].Field2)}";
			}
		}
		else if (ex is ActionNotSupportedException)
		{
			text = Resources.ActionNotSupported;
		}
		else if (ex is ArmSoft.OpenBanking.Api.Client.Core.UnauthorizedException)
		{
			text = Resources.IBAccessDenied;
		}
		return text;
	}

	public static void LogIBOperation(string taxCode, string account, IBOperation ibOpType, int count = 1)
	{
		try
		{
			using CloudClient cloudClient = Settings.CloudClient;
			cloudClient.Token = Settings.GetApplicationToken();
			IBCallRequest callRequest = new IBCallRequest
			{
				TaxCode = taxCode,
				BankCode = (string.IsNullOrWhiteSpace(account) ? "" : account.Substring(0, 3)),
				Op = ibOpType,
				Count = count
			};
			cloudClient.AS7X.RegisterIBCall(callRequest);
		}
		catch
		{
		}
	}

	public static List<Guid> ExportToXMLForIBank(List<IBPayment> payOrders, EnumerateErrorsReportViewer repViewer, ExportType exportType)
	{
		List<Guid> list = new List<Guid>();
		XmlDocument xmlDocument = new XmlDocument();
		XmlElement xmlElement = xmlDocument.CreateElement("As_Import-Export_File");
		XmlElement xmlElement2 = null;
		XmlElement xmlElement3;
		if (exportType == ExportType.InternetBankPassPayXML)
		{
			xmlElement3 = xmlDocument.CreateElement("PassPay");
		}
		else
		{
			xmlElement3 = xmlDocument.CreateElement("PayOrd");
			xmlElement2 = xmlDocument.CreateElement("PayBudg");
		}
		foreach (IBPayment payOrder in payOrders)
		{
			try
			{
				CheckPaymentForExport(payOrder);
				if (exportType == ExportType.InternetBankPassPayXML)
				{
					xmlElement3.AppendChild(SerializeOnePassPay(payOrder, xmlDocument));
				}
				else if (payOrder.IsThirdParty)
				{
					xmlElement2.AppendChild(SerializeOnePayOrd(payOrder, xmlDocument));
				}
				else
				{
					xmlElement3.AppendChild(SerializeOnePayOrd(payOrder, xmlDocument));
				}
				list.Add(payOrder.DocId);
			}
			catch (Exception ex)
			{
				repViewer.AddError(ex, payOrder.DocId);
			}
		}
		xmlElement.AppendChild(xmlElement3);
		if (exportType != ExportType.InternetBankPassPayXML)
		{
			xmlElement.AppendChild(xmlElement2);
		}
		xmlDocument.AppendChild(xmlElement);
		string text = ((exportType == ExportType.InternetBankPassPayXML) ? "PassPayOrder_" : "PayOrder_");
		string fileNameFromExportDialog = Settings.CommonCoreHelper.GetFileNameFromExportDialog(".xml", "XML Files (.xml)|*.xml", text + DateTime.Now.ToString("MMdd_HHmm"));
		if (string.IsNullOrWhiteSpace(fileNameFromExportDialog))
		{
			return new List<Guid>();
		}
		XMLToFile(xmlDocument, fileNameFromExportDialog);
		foreach (IBPayment payOrder2 in payOrders)
		{
			LogIBOperation(payOrder2.PayerTaxCode, payOrder2.PayerAcc, IBOperation.PaymentOrderXMLExport);
		}
		return list;
	}

	public static IBPayment GetPayment(string accountNumber, Guid isn, string receiverAccount, bool throwExists = false)
	{
		IBPayment result = null;
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
		{
			if (Settings.OpenBankingService.Login(bankMainCode))
			{
				result = Settings.OpenBankingService.GetPayment(accountNumber, isn, receiverAccount, throwExists);
			}
		}
		else if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value && !string.IsNullOrWhiteSpace(Settings.InternetBankService.EnsureLoggedIn(bankMainCode)))
		{
			result = Settings.InternetBankService.GetPayment(accountNumber, isn, receiverAccount, throwExists);
		}
		return result;
	}

	public static IBWGPayment GetWGPayment(string accountNumber, Guid isn, bool throwExists = false)
	{
		IBWGPayment result = null;
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
		{
			result = Settings.OpenBankingService.GetMultiplePayment(accountNumber, isn, throwExists);
		}
		else if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value)
		{
			result = Settings.InternetBankService.GetMultiplePayment(accountNumber, isn, throwExists);
		}
		return result;
	}

	public static IBCurrConversion GetCurrencyConversion(string accountNumber, Guid isn, bool throwExists = false)
	{
		IBCurrConversion result = null;
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
		{
			result = Settings.OpenBankingService.GetCurrencyConversion(bankMainCode, isn, throwExists);
		}
		else if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value)
		{
			result = Settings.InternetBankService.GetCurrecnyConversion(accountNumber, isn, throwExists);
		}
		return result;
	}

	public static IBCardTransfer GetCardTransfer(string accountNumber, Guid isn, bool throwExists = false)
	{
		IBCardTransfer result = null;
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
		{
			result = Settings.OpenBankingService.GetCardTransfer(accountNumber, isn, throwExists);
		}
		else if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value)
		{
			result = Settings.InternetBankService.GetCardTransfer(accountNumber, isn, throwExists);
		}
		return result;
	}

	public static List<DocumentHistoryRow> GetPaymentHistory(Guid docISN, string accountNumber)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
		{
			return Settings.OpenBankingService.GetPaymentHistory(bankMainCode, docISN);
		}
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value)
		{
			return Settings.InternetBankService.GetPaymentHistory(bankMainCode, docISN);
		}
		return null;
	}

	public static void SignPayments(string accountNumber, List<Guid> paymentIds)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
		{
			SignatureOptions signatureOptions = Settings.OpenBankingService.GetSignatureOptions(bankMainCode, paymentIds);
			IBBankSE bank = SmartEnum<IBBankSE, string>.FromValue(bankMainCode);
			InternetBankLoginRequest request = new InternetBankLoginRequest
			{
				Challenge = signatureOptions.Challenge,
				Description = signatureOptions.Description,
				IsOnlyDigiPass = true
			};
			InternetBankLoginDialog internetBankLoginDialog = new InternetBankLoginDialog(bank, request);
			string text = "";
			if (Convert.ToBoolean(internetBankLoginDialog.ShowDialog()))
			{
				text = Settings.OpenBankingService.SignPayments(bankMainCode, paymentIds, signatureOptions.Challenge, Convert.ToString(internetBankLoginDialog._securityKey.EditValue)).IgnoredDocumentsMessage;
				if (!string.IsNullOrWhiteSpace(text))
				{
					MessageBox.Show(text);
				}
				LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.SignDocument, paymentIds.Count());
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				MessageBox.Show(text);
			}
		}
		else
		{
			if (!(GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value))
			{
				return;
			}
			Challenge signatureOptions2 = Settings.InternetBankService.GetSignatureOptions(bankMainCode, paymentIds);
			IBBankSE bank2 = SmartEnum<IBBankSE, string>.FromValue(bankMainCode);
			InternetBankLoginRequest request2 = new InternetBankLoginRequest
			{
				Challenge = signatureOptions2.Challenge,
				Description = signatureOptions2.Description,
				IsOnlyDigiPass = true
			};
			InternetBankLoginDialog internetBankLoginDialog2 = new InternetBankLoginDialog(bank2, request2);
			string text2 = "";
			if (Convert.ToBoolean(internetBankLoginDialog2.ShowDialog()))
			{
				text2 = Settings.InternetBankService.SignPayments(bankMainCode, paymentIds, signatureOptions2.Challenge, Convert.ToString(internetBankLoginDialog2._securityKey.EditValue));
				if (!string.IsNullOrWhiteSpace(text2))
				{
					MessageBox.Show(text2);
				}
				LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.SignDocument, paymentIds.Count());
			}
			if (!string.IsNullOrWhiteSpace(text2))
			{
				MessageBox.Show(text2);
			}
		}
	}

	public static void RemovePayments(string accountNumber, List<Guid> paymentIds)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
		{
			Settings.OpenBankingService.RemovePayments(bankMainCode, paymentIds);
			LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.DeleteDocument, paymentIds.Count());
		}
		else if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value)
		{
			Settings.InternetBankService.RemovePayments(bankMainCode, paymentIds);
			LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.DeleteDocument, paymentIds.Count());
		}
	}

	public static void CancelLocalPayment(string accountNumber, Guid targetId, string targetNum, string targetType, DateOnly targetDate, decimal targetAmount, string detail)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
		{
			Settings.OpenBankingService.CancelLoacalPayment(bankMainCode, targetId, detail);
			LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.RevokeDocument);
		}
		else if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value)
		{
			Settings.InternetBankService.CancelPayment(bankMainCode, targetId, targetNum, targetType, targetDate.ToDateTime(), targetAmount, detail);
			LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.RevokeDocument);
		}
	}

	public static void CancelBudgetPayment(string accountNumber, Guid targetId, string targetNum, string targetType, DateOnly targetDate, decimal targetAmount, string detail)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
		{
			Settings.OpenBankingService.CancelBudgetPayment(bankMainCode, targetId, detail);
			LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.DeleteDocument);
		}
		else if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value)
		{
			Settings.InternetBankService.CancelPayment(accountNumber, targetId, targetNum, targetType, targetDate.ToDateTime(), targetAmount, detail);
			LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.DeleteDocument);
		}
	}

	public static void CancelMultiplePayment(string accountNumber, Guid targetId, string targetNum, string targetType, DateOnly targetDate, decimal targetAmount, string detail)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
		{
			Settings.OpenBankingService.CancelMultiplePayment(accountNumber, targetId, detail);
			LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.DeleteDocument);
		}
		else if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value)
		{
			Settings.InternetBankService.CancelPayment(accountNumber, targetId, targetNum, targetType, targetDate.ToDateTime(), targetAmount, detail);
			LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.DeleteDocument);
		}
	}

	public static void CancelPayment(string accountNumber, byte state, Guid targetId, string targetNum, string targetType, DateOnly targetDate, decimal targetAmount, string detail)
	{
		string bankMainCode = SettlementAccountHelper.GetBankMainCode(accountNumber);
		if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.OpenBank.Value)
		{
			Settings.OpenBankingService.CancelPayment(bankMainCode, state, targetId, detail);
			LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.DeleteDocument);
		}
		else if (GetBankConnectionMode(bankMainCode) == IBConnectionModesSE.InternetBank.Value)
		{
			Settings.InternetBankService.CancelPayment(accountNumber, targetId, targetNum, targetType, targetDate.ToDateTime(), targetAmount, detail);
			LogIBOperation(Settings.ParametersService.TaxCode(), accountNumber, IBOperation.DeleteDocument);
		}
	}

	private static XmlElement SerializeOnePayOrd(IBPayment doc, XmlDocument xmlDoc)
	{
		XmlElement xmlElement = ((!doc.IsThirdParty) ? xmlDoc.CreateElement("PayOrd") : xmlDoc.CreateElement("PayBudg"));
		xmlElement.SetAttribute("DOCNUM", doc.DocNum);
		xmlElement.SetAttribute("DOCDATE", doc.Date.FormatDDMMYY());
		xmlElement.SetAttribute("PAYERACC", doc.PayerAcc);
		xmlElement.SetAttribute("TAXCODE", doc.PayerTaxCode);
		xmlElement.SetAttribute("TAXAUTHORITYCODE", doc.TaxAuthorityCode);
		xmlElement.SetAttribute("SOCIALCARD", "");
		xmlElement.SetAttribute("BENACC", doc.ReceiverAcc);
		xmlElement.SetAttribute("BENEFICIARY", doc.ReceiverName);
		xmlElement.SetAttribute("RECEIVERLEGALSTATUS", ConvertReceiverLegalStatusForXML(doc.ReceiverLegalStatus));
		xmlElement.SetAttribute("AMOUNT", (doc.Currency == "AMD") ? doc.AmountAMD.RoundAs(1).ToString() : doc.Amount.ToString());
		xmlElement.SetAttribute("CURRENCY", doc.Currency);
		xmlElement.SetAttribute("DETAILS", doc.Details);
		if (doc.IsThirdParty)
		{
			xmlElement.SetAttribute("ISTHIRDPARTY", Convert.ToString(doc.IsThirdParty).ToLower());
			xmlElement.SetAttribute("THIRDPARTYRESIDENCETYPE", ConvertResidencyTypeForXML(doc.ThirdPartyResidenceType));
			xmlElement.SetAttribute("THIRDPARTYLEGALSTATE", ConvertLegalStatusForXML(doc.ThirdPartyLegalState));
			xmlElement.SetAttribute("TAXAUTHORITYCODE", doc.TaxAuthorityCode);
			xmlElement.SetAttribute("THIRDPARTYTAXCODE", doc.ThirdPartyTaxCode);
			xmlElement.SetAttribute("THIRDPARTYSOCIALCARD", doc.ThirdPartySocialCard);
			xmlElement.SetAttribute("THIRDPARTYNAME", doc.ThirdPartyName);
			if (!string.IsNullOrWhiteSpace(ConvertPassportTypeForXML(doc.ThirdPartyPassportType)))
			{
				xmlElement.SetAttribute("THIRDPARTYPASSPORTTYPE", ConvertPassportTypeForXML(doc.ThirdPartyPassportType));
			}
			xmlElement.SetAttribute("THIRDPARTYPASSPORTNO", doc.ThirdPartyPassportNo);
			xmlElement.SetAttribute("THIRDPARTYADDRESS", doc.ThirdPartyAddress);
		}
		return xmlElement;
	}

	private static XmlElement SerializeOnePassPay(IBPayment doc, XmlDocument xmlDoc)
	{
		XmlElement xmlElement = xmlDoc.CreateElement("PassPay");
		xmlElement.SetAttribute("DOCNUM", doc.DocNum);
		xmlElement.SetAttribute("DOCDATE", doc.Date.FormatDDMMYY());
		xmlElement.SetAttribute("PAYERACC", doc.PayerAcc);
		xmlElement.SetAttribute("BENEFICIARYNAME", doc.ReceiverName);
		xmlElement.SetAttribute("BENEFICIARYPASSPORTNUM", doc.ReceiverPassportNum);
		xmlElement.SetAttribute("RECEIVERLEGALSTATUS", ConvertReceiverLegalStatusForXML(doc.ReceiverLegalStatus));
		xmlElement.SetAttribute("AMOUNT", (doc.Currency == "AMD") ? doc.AmountAMD.RoundAs(1).ToString() : doc.Amount.ToString());
		xmlElement.SetAttribute("CURRENCY", doc.Currency);
		xmlElement.SetAttribute("DETAILS", doc.Details);
		return xmlElement;
	}

	private static void XMLToFile(XmlDocument xmlDoc, string fileName)
	{
		using StringWriter stringWriter = new StringWriter();
		XmlWriterSettings settings = new XmlWriterSettings
		{
			Encoding = Encoding.Unicode,
			NamespaceHandling = NamespaceHandling.OmitDuplicates,
			Indent = true,
			OmitXmlDeclaration = false
		};
		XmlWriter.Create(stringWriter, settings);
		XmlDeclaration newChild = xmlDoc.CreateXmlDeclaration("1.0", "UTF-16", "yes");
		xmlDoc.InsertBefore(newChild, xmlDoc.DocumentElement);
		xmlDoc.InnerXml = xmlDoc.InnerXml.ToArmenianANSI();
		xmlDoc.Save(stringWriter);
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		using StreamWriter streamWriter = new StreamWriter(fileName, append: false, Encoding.GetEncoding("Windows-1252"));
		streamWriter.Write(stringWriter.ToString());
		streamWriter.Flush();
	}

	public static ResidenceType ConvertResidencyType(string residencyCode)
	{
		if (residencyCode == ResidencyTypeSE.Resident.Value)
		{
			return ResidenceType.Resident;
		}
		if (residencyCode == ResidencyTypeSE.NotResident.Value)
		{
			return ResidenceType.NonResident;
		}
		return ResidenceType.NotSet;
	}

	public static string ConvertResidencyTypeForXML(ResidenceType residencyType)
	{
		return residencyType switch
		{
			ResidenceType.Resident => "Resident", 
			ResidenceType.NonResident => "NotResident", 
			_ => "", 
		};
	}

	public static ClientStatus ConvertLegalStatus(string residencyCode)
	{
		if (residencyCode == LegalStatusTypeSE.LegalEntity.Value)
		{
			return ClientStatus.Legal;
		}
		if (residencyCode == LegalStatusTypeSE.FizEntity.Value)
		{
			return ClientStatus.Natural;
		}
		if (residencyCode == LegalStatusTypeSE.IndividualEntity.Value)
		{
			return ClientStatus.Entrepreneur;
		}
		return ClientStatus.NotSet;
	}

	public static string ConvertLegalStatusForXML(ClientStatus residencyType)
	{
		return residencyType switch
		{
			ClientStatus.Legal => "Legal", 
			ClientStatus.Natural => "Natural", 
			ClientStatus.Entrepreneur => "Entrepreneur", 
			_ => "", 
		};
	}

	public static ReceiverLegalStatus ConvertReceiverLegalStatus(string residencyCode)
	{
		if (residencyCode == ReceiverLegalStatusTypeSE.LegalEntity.Value)
		{
			return ReceiverLegalStatus.Legal;
		}
		if (residencyCode == ReceiverLegalStatusTypeSE.FizEntity.Value)
		{
			return ReceiverLegalStatus.Natural;
		}
		return ReceiverLegalStatus.NotSet;
	}

	public static string ConvertReceiverLegalStatus(ReceiverLegalStatus residencyType)
	{
		return residencyType switch
		{
			ReceiverLegalStatus.Legal => ReceiverLegalStatusTypeSE.LegalEntity.Value, 
			ReceiverLegalStatus.Natural => ReceiverLegalStatusTypeSE.FizEntity.Value, 
			_ => "", 
		};
	}

	public static string ConvertReceiverLegalStatusForXML(ReceiverLegalStatus residencyType)
	{
		return residencyType switch
		{
			ReceiverLegalStatus.Legal => "Legal", 
			ReceiverLegalStatus.Natural => "Natural", 
			_ => "", 
		};
	}

	public static PassportType ConvertPassportType(string residencyCode)
	{
		if (residencyCode == PassportTypesSE.PassportRA.Value)
		{
			return PassportType.RA;
		}
		if (residencyCode == PassportTypesSE.PassportForeign.Value)
		{
			return PassportType.Foreign;
		}
		if (residencyCode == PassportTypesSE.DeathCertificate.Value)
		{
			return PassportType.DeathCertificate;
		}
		return PassportType.NotSet;
	}

	public static string ConvertPassportTypeForXML(PassportType residencyType)
	{
		return residencyType switch
		{
			PassportType.RA => "RA", 
			PassportType.Foreign => "Foreign", 
			PassportType.DeathCertificate => "DeathCertificate", 
			_ => "", 
		};
	}

	public static void CheckPaymentForExport(IBPayment payment)
	{
		if (string.IsNullOrWhiteSpace(payment.DocNum))
		{
			throw new ASException(string.Format(Resources.FieldIsEmpty, Resources.DocNumber));
		}
		if (string.IsNullOrWhiteSpace(payment.PayerAcc))
		{
			throw new ASException(string.Format(Resources.FieldIsEmpty, Resources.PayerSettlAcc));
		}
		if (string.IsNullOrWhiteSpace(payment.ReceiverName))
		{
			throw new ASException(string.Format(Resources.FieldIsEmpty, Resources.ReceiverName));
		}
		if (string.IsNullOrWhiteSpace(payment.ReceiverAcc))
		{
			throw new ASException(string.Format(Resources.FieldIsEmpty, Resources.ReceiverSettlAcc));
		}
		if (payment.Amount == 0m)
		{
			throw new ASException(string.Format(Resources.FieldIsEmpty, Resources.Summa));
		}
		if (string.IsNullOrWhiteSpace(payment.Details))
		{
			throw new ASException(string.Format(Resources.FieldIsEmpty, Resources.PaymentAim));
		}
		if (payment.PayerAcc == payment.ReceiverAcc)
		{
			throw new ASException(Resources.CBSameDebitAndCreditAcc);
		}
	}
}
