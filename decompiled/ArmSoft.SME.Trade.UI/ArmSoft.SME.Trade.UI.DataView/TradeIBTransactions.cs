using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.InternetBank;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DataViews.Reports;
using ArmSoft.SME.Core.UI.DataViewsData.Reports;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Documents;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(21)]
public class TradeIBTransactions : IBTransactions
{
	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		IBTransactionsDataProvider.DataRow dataRow = (IBTransactionsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (((AllowOperation && Settings.LoginService.IsAdmin) || Settings.UserAccessService.IsSubsystemAccessible(4)) && dataRow != null)
		{
			if (!dataRow.CreatedDocISN.HasValue || Settings.HostingEnvironment.CurrentViewPanel.GetSelectedAny())
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "EstablishCorrespondence", ArmSoft.SME.Common.Resources.EstablishCorrespondenceToExistingDoc, EstablishCorrespondence);
			}
			if (dataRow.CreatedDocISN.HasValue || Settings.HostingEnvironment.CurrentViewPanel.GetSelectedAny())
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "BreakCorrespondence", ArmSoft.SME.Common.Resources.BreakCorrespondenceToDoc, BreakCorrespondence);
			}
			if (dataRow.CreatedDocISN.HasValue)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "OpenCorrespondentDoc", ArmSoft.SME.Common.Resources.OpenCorrespondentDoc, OpenCorrespondentDoc);
			}
			if (Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.Partner.Value).AllowAdd)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "AddPartner", ArmSoft.SME.Common.Resources.AddPartner, AddPartner);
			}
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "CreateDoc", ArmSoft.SME.Common.Resources.CreateDoc, CreateDoc, "doc_new_16.svg");
		}
	}

	private void EstablishCorrespondence()
	{
		List<IBTransactionsDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<IBTransactionsDataProvider.DataRow>();
		if (selectedOrFocusedRows.Count == 0)
		{
			return;
		}
		DateOnly date = selectedOrFocusedRows[0].Date;
		DateOnly date2 = selectedOrFocusedRows[0].Date;
		foreach (IBTransactionsDataProvider.DataRow item in selectedOrFocusedRows)
		{
			if (item.Date < date)
			{
				date = item.Date;
			}
			if (item.Date < date2)
			{
				date2 = item.Date;
			}
			if (item.CreatedDocISN.HasValue)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.IBSelectedLinesHaveCorrespondent);
			}
		}
		DocumentsJournal documentsJournal = new DocumentsJournal(Settings.HostingEnvironment, ReportTypesSE.IBCorrespondent, new HashSet<short>
		{
			DocumentTypeSE.BankIn.Value,
			DocumentTypeSE.BankOut.Value,
			DocumentTypeSE.CashIn.Value,
			DocumentTypeSE.CashOut.Value,
			DocumentTypeSE.PayOrd.Value
		})
		{
			StartDate = date,
			EndDate = date2,
			DocState = DocumentStateSE.DraftOrProcessed.ValueUI
		};
		if (!documentsJournal.ShowDialog())
		{
			return;
		}
		ModalBrowser modalBrowser = new ModalBrowser(documentsJournal);
		modalBrowser.WindowStartupLocation = WindowStartupLocation.CenterOwner;
		modalBrowser.Owner = Settings.HostingEnvironment;
		if (!modalBrowser.Show())
		{
			return;
		}
		Guid iSN = documentsJournal.GetRowByIndex(modalBrowser.SelectedRowIndex).ISN;
		EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(ArmSoft.SME.Common.Resources.EstablishCorrespondenceToExistingDoc, "EstablishCorrespondence");
		foreach (IBTransactionsDataProvider.DataRow item2 in selectedOrFocusedRows)
		{
			try
			{
				IBTransactionMappingRequest iBTransactionMappingRequest = new IBTransactionMappingRequest
				{
					Date = item2.OperationDate,
					Account = item2.Account,
					CorrespondentAccount = item2.CorrespondentAccount,
					Comment = item2.Comment,
					BankDocId = item2.BankDocId
				};
				if (item2.CreditAmount != 0m || item2.CreditAmountCur != 0m)
				{
					iBTransactionMappingRequest.Sum = item2.CreditAmount;
					iBTransactionMappingRequest.SumCur = item2.CreditAmountCur;
					iBTransactionMappingRequest.IsCredit = true;
				}
				else
				{
					iBTransactionMappingRequest.Sum = item2.DebitAmount;
					iBTransactionMappingRequest.SumCur = item2.DebitAmountCur;
					iBTransactionMappingRequest.IsCredit = false;
				}
				iBTransactionMappingRequest.MatchedDocISN = iSN;
				Settings.CreateApiClient().InternetBank.AddMapping(iBTransactionMappingRequest);
			}
			catch (Exception ex)
			{
				enumerateErrorsReportViewer.AddError(ex);
			}
		}
		foreach (IBTransactionsDataProvider.DataRow item3 in selectedOrFocusedRows)
		{
			IBTransactionMappingRequest iBTransactionMappingRequest2 = new IBTransactionMappingRequest
			{
				Date = item3.OperationDate,
				Account = item3.Account,
				CorrespondentAccount = item3.CorrespondentAccount,
				Comment = item3.Comment,
				BankDocId = item3.BankDocId
			};
			if (item3.CreditAmount != 0m || item3.CreditAmountCur != 0m)
			{
				iBTransactionMappingRequest2.Sum = item3.CreditAmount;
				iBTransactionMappingRequest2.SumCur = item3.CreditAmountCur;
				iBTransactionMappingRequest2.IsCredit = true;
			}
			else
			{
				iBTransactionMappingRequest2.Sum = item3.DebitAmount;
				iBTransactionMappingRequest2.SumCur = item3.DebitAmountCur;
				iBTransactionMappingRequest2.IsCredit = false;
			}
			Settings.HostingEnvironment.CurrentViewPanel.Update(iBTransactionMappingRequest2);
		}
		InternetBankHelper.LogIBOperation(Settings.ParametersService.TaxCode(), "", IBOperation.Matching);
		if (enumerateErrorsReportViewer.HasErrors)
		{
			enumerateErrorsReportViewer.Show();
		}
	}

	private void BreakCorrespondence()
	{
		List<IBTransactionsDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<IBTransactionsDataProvider.DataRow>();
		if (ArmSoft.SME.Core.UI.MessageBox.Show((selectedOrFocusedRows.Count == 1) ? ArmSoft.SME.Common.Resources.ConfBreakCorrespondence : ArmSoft.SME.Common.Resources.ConfBreakCorrespondenceSel, "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
		{
			return;
		}
		EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(ArmSoft.SME.Common.Resources.BreakCorrespondenceToDoc, "BreakCorrespondence");
		foreach (IBTransactionsDataProvider.DataRow item in selectedOrFocusedRows)
		{
			try
			{
				IBTransactionMappingRequest iBTransactionMappingRequest = new IBTransactionMappingRequest
				{
					Date = item.OperationDate,
					Account = item.Account,
					CorrespondentAccount = item.CorrespondentAccount,
					Comment = item.Comment,
					BankDocId = item.BankDocId
				};
				if (item.CreditAmount != 0m || item.CreditAmountCur != 0m)
				{
					iBTransactionMappingRequest.Sum = item.CreditAmount;
					iBTransactionMappingRequest.SumCur = item.CreditAmountCur;
					iBTransactionMappingRequest.IsCredit = true;
				}
				else
				{
					iBTransactionMappingRequest.Sum = item.DebitAmount;
					iBTransactionMappingRequest.SumCur = item.DebitAmountCur;
					iBTransactionMappingRequest.IsCredit = false;
				}
				Settings.CreateApiClient().InternetBank.RemoveMapping(iBTransactionMappingRequest);
			}
			catch (Exception ex)
			{
				enumerateErrorsReportViewer.AddError(ex);
			}
		}
		foreach (IBTransactionsDataProvider.DataRow item2 in selectedOrFocusedRows)
		{
			IBTransactionMappingRequest iBTransactionMappingRequest2 = new IBTransactionMappingRequest
			{
				Date = item2.OperationDate,
				Account = item2.Account,
				CorrespondentAccount = item2.CorrespondentAccount,
				Comment = item2.Comment,
				BankDocId = item2.BankDocId
			};
			if (item2.CreditAmount != 0m || item2.CreditAmountCur != 0m)
			{
				iBTransactionMappingRequest2.Sum = item2.CreditAmount;
				iBTransactionMappingRequest2.SumCur = item2.CreditAmountCur;
				iBTransactionMappingRequest2.IsCredit = true;
			}
			else
			{
				iBTransactionMappingRequest2.Sum = item2.DebitAmount;
				iBTransactionMappingRequest2.SumCur = item2.DebitAmountCur;
				iBTransactionMappingRequest2.IsCredit = false;
			}
			Settings.HostingEnvironment.CurrentViewPanel.Update(iBTransactionMappingRequest2);
		}
		if (enumerateErrorsReportViewer.HasErrors)
		{
			enumerateErrorsReportViewer.Show();
		}
	}

	private void OpenCorrespondentDoc()
	{
		IBTransactionsDataProvider.DataRow dataRow = (IBTransactionsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		Settings.DocumentService.ShowWindow(dataRow.CreatedDocISN.Value, WindowViewMode.Edit, Settings.HostingEnvironment.CurrentViewPanel.Id);
	}

	public override string GetDefaultFunction()
	{
		return "OpenCorrespondentDoc";
	}

	private void AddPartner()
	{
		try
		{
			IBTransactionsDataProvider.DataRow dataRow = (IBTransactionsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			Partner partner = Settings.DirectoryService.Create<Partner>(DirectoryTypeSE.Partner.Value);
			partner.Name = dataRow.CorrespondentName;
			partner.PartnerContracts.Add(new Partner.PartnerContractRow
			{
				Contract = "001",
				CurrCode = dataRow.Currency,
				Caption = ArmSoft.SME.Trade.Common.Resources.DefaultContract,
				IsDefault = true
			});
			string caption = Settings.CoreCacheService.Bank.GetItem(dataRow.CorrespondentAccount.Mid(1, 5))?.Name ?? "";
			partner.SettlementAccounts.Add(new Partner.PartnerSettlementAccount
			{
				SettlAccount = dataRow.CorrespondentAccount,
				Caption = caption,
				IsDefault = true
			});
			Settings.DirectoryService.ShowWindow(partner, isPOWindow: true, WindowViewMode.NewCreate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CreateDoc()
	{
		DocumentTypeSE documentTypeSE;
		if (!(((IBTransactionsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow()).CreditAmount == 0m))
		{
			int num = 2;
			List<short> list = new List<short>(num);
			CollectionsMarshal.SetCount(list, num);
			Span<short> span = CollectionsMarshal.AsSpan(list);
			span[0] = DocumentTypeSE.BankOut.Value;
			span[1] = DocumentTypeSE.CashIn.Value;
			documentTypeSE = InitContextFunctionsHelper.ChooseDocTypeFromMenu(list);
		}
		else
		{
			int num = 2;
			List<short> list2 = new List<short>(num);
			CollectionsMarshal.SetCount(list2, num);
			Span<short> span2 = CollectionsMarshal.AsSpan(list2);
			span2[0] = DocumentTypeSE.BankIn.Value;
			span2[1] = DocumentTypeSE.CashOut.Value;
			documentTypeSE = InitContextFunctionsHelper.ChooseDocTypeFromMenu(list2);
		}
		DocumentTypeSE documentTypeSE2 = documentTypeSE;
		foreach (IBTransactionsDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<IBTransactionsDataProvider.DataRow>())
		{
			bool flag = selectedOrFocusedRow.CreditAmount == 0m;
			if ((!flag || ((short)documentTypeSE2 != DocumentTypeSE.BankOut.Value && (short)documentTypeSE2 != DocumentTypeSE.CashIn.Value)) && (flag || ((short)documentTypeSE2 != DocumentTypeSE.BankIn.Value && (short)documentTypeSE2 != DocumentTypeSE.CashOut.Value)))
			{
				switch (documentTypeSE2)
				{
				case 4:
					CreateBankIn(selectedOrFocusedRow);
					break;
				case 5:
					CreateBankOut(selectedOrFocusedRow);
					break;
				case 2:
					CreateCashIn(selectedOrFocusedRow);
					break;
				case 3:
					CreateCashOut(selectedOrFocusedRow);
					break;
				}
			}
		}
	}

	private static void CreateBankIn(IBTransactionsDataProvider.DataRow row)
	{
		try
		{
			BankIn bankIn = Settings.DocumentService.Create<BankIn>(DocumentTypeSE.BankIn.Value);
			AccountInBankCacheRow accountInBankBySettlementAccount = TradeSettings.TradeCacheService.AccountInBank.GetAccountInBankBySettlementAccount(row.Account);
			if (accountInBankBySettlementAccount != null)
			{
				bankIn.AccountInBankCode = accountInBankBySettlementAccount.Code;
				bankIn.CurrCode = accountInBankBySettlementAccount.Curr;
			}
			bankIn.Summ = row.DebitAmountCur;
			bankIn.SummNCC = row.DebitAmount;
			bankIn.DocDate = row.OperationDate.ToDateOnly();
			bankIn.DocNum = row.DocumentNumber;
			PartnerCacheRow itemBySettlAccount = TradeSettings.TradeCacheService.Partner.GetItemBySettlAccount(row.CorrespondentAccount);
			if (itemBySettlAccount == null)
			{
				string settlAccount = row.CorrespondentAccount.Left(5) + "-" + row.CorrespondentAccount.Mid(6);
				itemBySettlAccount = TradeSettings.TradeCacheService.Partner.GetItemBySettlAccount(settlAccount);
			}
			if (itemBySettlAccount != null)
			{
				bankIn.PartnerContractGroup.PartnerCode = itemBySettlAccount.PartCode;
				PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(itemBySettlAccount.PartCode);
				if (defaultContract != null)
				{
					bankIn.PartnerContractGroup.ContractCode = defaultContract.Contract;
				}
				bankIn.Summ = row.DebitAmountCur;
				if (defaultContract.CurrCode != bankIn.CurrCode)
				{
					ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(defaultContract.CurrCode, bankIn.DocDate);
					bankIn.Summ = (bankIn.Summ.Value * exchangeRates.Base / exchangeRates.Rate).RoundAs(2);
				}
			}
			bankIn.Comment = row.Comment;
			IBTransactionMappingRequest iBTransactionMappingRequest = new IBTransactionMappingRequest
			{
				Date = row.OperationDate,
				Account = row.Account,
				CorrespondentAccount = row.CorrespondentAccount,
				Comment = row.Comment,
				BankDocId = row.BankDocId
			};
			if (row.CreditAmount != 0m || row.CreditAmountCur != 0m)
			{
				iBTransactionMappingRequest.Sum = row.CreditAmount;
				iBTransactionMappingRequest.SumCur = row.CreditAmountCur;
				iBTransactionMappingRequest.IsCredit = true;
			}
			else
			{
				iBTransactionMappingRequest.Sum = row.DebitAmount;
				iBTransactionMappingRequest.SumCur = row.DebitAmountCur;
				iBTransactionMappingRequest.IsCredit = false;
			}
			bankIn.IBKey = iBTransactionMappingRequest;
			BankInWindow bankInWindow = new BankInWindow(bankIn);
			bankInWindow.DVPID = Settings.HostingEnvironment.CurrentViewPanel.Id;
			bankInWindow.Show(WindowViewMode.NewCreate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static void CreateBankOut(IBTransactionsDataProvider.DataRow row)
	{
		try
		{
			BankOut bankOut = Settings.DocumentService.Create<BankOut>(DocumentTypeSE.BankOut.Value);
			AccountInBankCacheRow accountInBankBySettlementAccount = TradeSettings.TradeCacheService.AccountInBank.GetAccountInBankBySettlementAccount(row.Account);
			if (accountInBankBySettlementAccount != null)
			{
				bankOut.AccountInBankCode = accountInBankBySettlementAccount.Code;
				bankOut.CurrCode = accountInBankBySettlementAccount.Curr;
			}
			bankOut.Summ = row.CreditAmountCur;
			bankOut.SummNCC = row.CreditAmount;
			bankOut.DocDate = row.OperationDate.ToDateOnly();
			bankOut.DocNum = row.DocumentNumber;
			PartnerCacheRow itemBySettlAccount = TradeSettings.TradeCacheService.Partner.GetItemBySettlAccount(row.CorrespondentAccount);
			if (itemBySettlAccount == null)
			{
				string settlAccount = row.CorrespondentAccount.Left(5) + "-" + row.CorrespondentAccount.Mid(6);
				itemBySettlAccount = TradeSettings.TradeCacheService.Partner.GetItemBySettlAccount(settlAccount);
			}
			if (itemBySettlAccount != null)
			{
				bankOut.PartnerContractGroup.PartnerCode = itemBySettlAccount.PartCode;
				PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(itemBySettlAccount.PartCode);
				if (defaultContract != null)
				{
					bankOut.PartnerContractGroup.ContractCode = defaultContract.Contract;
				}
				bankOut.Summ = row.CreditAmountCur;
				if (defaultContract.CurrCode != bankOut.CurrCode)
				{
					ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(defaultContract.CurrCode, bankOut.DocDate);
					bankOut.Summ = (bankOut.Summ.Value * exchangeRates.Base / exchangeRates.Rate).RoundAs(2);
				}
			}
			bankOut.Comment = row.Comment;
			IBTransactionMappingRequest iBTransactionMappingRequest = new IBTransactionMappingRequest
			{
				Date = row.OperationDate,
				Account = row.Account,
				CorrespondentAccount = row.CorrespondentAccount,
				Comment = row.Comment,
				BankDocId = row.BankDocId
			};
			if (row.CreditAmount != 0m || row.CreditAmountCur != 0m)
			{
				iBTransactionMappingRequest.Sum = row.CreditAmount;
				iBTransactionMappingRequest.SumCur = row.CreditAmountCur;
				iBTransactionMappingRequest.IsCredit = true;
			}
			else
			{
				iBTransactionMappingRequest.Sum = row.DebitAmount;
				iBTransactionMappingRequest.SumCur = row.DebitAmountCur;
				iBTransactionMappingRequest.IsCredit = false;
			}
			bankOut.IBKey = iBTransactionMappingRequest;
			BankOutWindow bankOutWindow = new BankOutWindow(bankOut);
			bankOutWindow.DVPID = Settings.HostingEnvironment.CurrentViewPanel.Id;
			bankOutWindow.Show(WindowViewMode.NewCreate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static void CreateCashIn(IBTransactionsDataProvider.DataRow row)
	{
		try
		{
			CashIn cashIn = Settings.DocumentService.Create<CashIn>(DocumentTypeSE.CashIn.Value);
			cashIn.OpType = CashInOpTypesSE.FromSettlementAcc.Value;
			IReadOnlyList<CashCacheRow> list = TradeSettings.TradeCacheService.Cash.GetList();
			cashIn.CashDesk = ((list.Count > 0) ? list[0].Code : "");
			AccountInBankCacheRow accountInBankBySettlementAccount = TradeSettings.TradeCacheService.AccountInBank.GetAccountInBankBySettlementAccount(row.Account);
			if (accountInBankBySettlementAccount != null)
			{
				cashIn.AccountInBankCode = accountInBankBySettlementAccount.Code;
				cashIn.CurrCode = accountInBankBySettlementAccount.Curr;
			}
			cashIn.Summ = row.CreditAmountCur;
			cashIn.SummNCC = row.CreditAmount;
			cashIn.DocDate = row.OperationDate.ToDateOnly();
			cashIn.PayerName = row.Comment;
			IBTransactionMappingRequest iBTransactionMappingRequest = new IBTransactionMappingRequest
			{
				Date = row.OperationDate,
				Account = row.Account,
				CorrespondentAccount = row.CorrespondentAccount,
				Comment = row.Comment,
				BankDocId = row.BankDocId
			};
			if (row.CreditAmount != 0m || row.CreditAmountCur != 0m)
			{
				iBTransactionMappingRequest.Sum = row.CreditAmount;
				iBTransactionMappingRequest.SumCur = row.CreditAmountCur;
				iBTransactionMappingRequest.IsCredit = true;
			}
			else
			{
				iBTransactionMappingRequest.Sum = row.DebitAmount;
				iBTransactionMappingRequest.SumCur = row.DebitAmountCur;
				iBTransactionMappingRequest.IsCredit = false;
			}
			cashIn.IBKey = iBTransactionMappingRequest;
			CashInWindow cashInWindow = new CashInWindow(cashIn);
			cashInWindow.DVPID = Settings.HostingEnvironment.CurrentViewPanel.Id;
			cashInWindow.Show(WindowViewMode.NewCreate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static void CreateCashOut(IBTransactionsDataProvider.DataRow row)
	{
		try
		{
			CashOut cashOut = Settings.DocumentService.Create<CashOut>(DocumentTypeSE.CashOut.Value);
			cashOut.OpType = CashOutOpTypesSE.ToSettlementAcc.Value;
			IReadOnlyList<CashCacheRow> list = TradeSettings.TradeCacheService.Cash.GetList();
			cashOut.CashDesk = ((list.Count > 0) ? list[0].Code : "");
			AccountInBankCacheRow accountInBankBySettlementAccount = TradeSettings.TradeCacheService.AccountInBank.GetAccountInBankBySettlementAccount(row.Account);
			if (accountInBankBySettlementAccount != null)
			{
				cashOut.AccountInBankCode = accountInBankBySettlementAccount.Code;
				cashOut.CurrCode = accountInBankBySettlementAccount.Curr;
			}
			cashOut.Summ = row.DebitAmountCur;
			cashOut.SummNCC = row.DebitAmount;
			cashOut.DocDate = row.OperationDate.ToDateOnly();
			cashOut.ReceiverName = row.Comment;
			IBTransactionMappingRequest iBTransactionMappingRequest = new IBTransactionMappingRequest
			{
				Date = row.OperationDate,
				Account = row.Account,
				CorrespondentAccount = row.CorrespondentAccount,
				Comment = row.Comment,
				BankDocId = row.BankDocId
			};
			if (row.CreditAmount != 0m || row.CreditAmountCur != 0m)
			{
				iBTransactionMappingRequest.Sum = row.CreditAmount;
				iBTransactionMappingRequest.SumCur = row.CreditAmountCur;
				iBTransactionMappingRequest.IsCredit = true;
			}
			else
			{
				iBTransactionMappingRequest.Sum = row.DebitAmount;
				iBTransactionMappingRequest.SumCur = row.DebitAmountCur;
				iBTransactionMappingRequest.IsCredit = false;
			}
			cashOut.IBKey = iBTransactionMappingRequest;
			CashOutWindow cashOutWindow = new CashOutWindow(cashOut);
			cashOutWindow.DVPID = Settings.HostingEnvironment.CurrentViewPanel.Id;
			cashOutWindow.Show(WindowViewMode.NewCreate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
