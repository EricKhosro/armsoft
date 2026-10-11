using System;
using System.Collections.Generic;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Documents;
using ArmSoft.SME.Common.Documents.Definition;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Trade.Common.PersistableObject;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.Documents.Definition;

public static class TradeDefinitionCreation
{
	public static DocDefinition CreateBankIn()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankIn)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 121,
			HelpId = "Purchases_Documents_SupportingDocuments_BankIn",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PaymentOrdersJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BankOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BankTAccount.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BankDocumentsJournal.Value
			},
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankOut.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.InvoiceForPay.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankOut.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("OperType", () => Resources.OpType, typeof(BankInOpTypeSE), isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SecondAcc", () => Resources.OutputAccount, 8));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("AccountInBankCode", () => ArmSoft.SME.Common.Resources.Account, 8, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.Summa, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRateShort, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummNCC", () => Resources.SummAMD, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Partner, isSupplier: false, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract),
			SummaPartFieldDefinition = new SumFieldDefinition("SummaPart", () => ArmSoft.SME.Common.Resources.Summa)
		});
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		return tradeDocDefinition;
	}

	public static DocDefinition CreateBankOut()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankOut)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 122,
			HelpId = "Purchases_Documents_SupportingDocuments_BankOut",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PaymentOrdersJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BankOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BankTAccount.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BankDocumentsJournal.Value
			},
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankIn.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.ReceivedService.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTOverheadExp.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankIn.Value
			},
			WeakChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankIn.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("OperType", () => Resources.OpType, typeof(BankOutOpTypeSE), isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SecondAcc", () => Resources.InputAccount, 8));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("AccountInBankCode", () => ArmSoft.SME.Common.Resources.Account, 8, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.Summa, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRateShort, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummNCC", () => Resources.SummAMD, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Partner, isSupplier: false, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract),
			SummaPartFieldDefinition = new SumFieldDefinition("SummaPart", () => ArmSoft.SME.Common.Resources.Summa)
		});
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		return tradeDocDefinition;
	}

	public static DocDefinition BonusGroupCorrect()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BonusGroupCorrect)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 359,
			HelpId = "Purchases_Documents_SupportingDocuments_BonusGroupEdit",
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BonusOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BonusTAccount.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CashDesk", () => ArmSoft.SME.Common.Resources.Cash, 8));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CashierShiftNumber", () => Resources.CashierShiftNum, 12));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRateShort, isRequired: true));
		GridDefinition gridDefinition = new GridDefinition("PartRemList", () => Resources.PartnerSepContractList, isRequired: true);
		PartnerBonusGroupCorrectBase.AddDefToPartBonusGroupCorrectGrid(gridDefinition);
		gridDefinition.AddFieldDefinition(new ContractFieldDefinition("Contract", () => ArmSoft.SME.Common.Resources.Contract, isBonusCard: true, isRequired: true));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Summa", () => string.Format(Resources.Bonus, ArmSoft.SME.Common.Resources.Unit) + " (+/-)", isRequired: true, isNonNegative: false));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		return tradeDocDefinition;
	}

	public static DocDefinition CashIn()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = true,
			NextDocNumParamId = 67,
			HelpId = "Sales_Documents_CashIn",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail
			},
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.InvoiceForPay,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankOut,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill
			},
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashDeskTAccount.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ECRCheckDocumentsJournal.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("OpType", () => Resources.OpType, typeof(CashInOpTypesSE), isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CashDesk", () => ArmSoft.SME.Common.Resources.Cash, 8, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CashOutDesk", () => Resources.CashOutDesk, 8));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CashierShiftNumber", () => Resources.CashierShiftNum, 12));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => Resources.DocCurr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRateShort, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("ECRCheckNum", () => ArmSoft.SME.Common.Resources.ECRCheckNum, 8));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BaseISN", () => ArmSoft.SME.Common.Resources.BaseDoc, 50));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.Summa + " " + Resources.WithCurr, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummNCC", () => ArmSoft.SME.Common.Resources.Summa + " " + Resources.NCOnly, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("AccountInBankCode", () => ArmSoft.SME.Common.Resources.Account, 8));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Employee", () => ArmSoft.SME.Common.Resources.Employee, 12));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Partner, isSupplier: false, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract),
			SummaPartFieldDefinition = new SumFieldDefinition("SummaPart", () => ArmSoft.SME.Common.Resources.Summa)
		});
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("BankAcquiringPartnerGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => Resources.POSTerminalPart, isSupplier: false, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => Resources.POSTerminalContract)
		});
		tradeDocDefinition.AddFieldDefinition(new BoolFieldDefinition("PrintPartnerTinOnCheck", () => ArmSoft.SME.Common.Resources.PrintExpenseJustifyingCheck));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PaymentId", () => ArmSoft.SME.Common.Resources.PaymentId, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PayerName", () => ArmSoft.SME.Common.Resources.PayerBut, 255, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Base", () => ArmSoft.SME.Common.Resources.Base, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Appendix", () => Resources.Appendix, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("ChiefAccountant", () => ArmSoft.SME.Common.Resources.ChAccPost1, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Cashier", () => ArmSoft.SME.Common.Resources.Cashier, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("PartnerRem", () => Resources.PartnerRem, isRequired: false, isNonNegative: false));
		return tradeDocDefinition;
	}

	public static DocDefinition CashOut()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = true,
			NextDocNumParamId = 68,
			HelpId = "Purchases_Documents_SupportingDocuments_CashOut",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankIn,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.ReceivedService,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTOverheadExp,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer
			},
			ChildDocTypes = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn },
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashDeskTAccount.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ECRCheckDocumentsJournal.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("OpType", () => Resources.OpType, typeof(CashOutOpTypesSE), isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CashDesk", () => ArmSoft.SME.Common.Resources.Cash, 8, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CashInDesk", () => Resources.CashInDesk, 8));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CashierShiftNumber", () => Resources.CashierShiftNum, 12));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => Resources.DocCurr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRateShort, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BaseISN", () => ArmSoft.SME.Common.Resources.BaseDoc, 50));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.Summa + " " + Resources.WithCurr, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummNCC", () => ArmSoft.SME.Common.Resources.Summa + " " + Resources.NCOnly, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("AccountInBankCode", () => ArmSoft.SME.Common.Resources.Account, 8));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Employee", () => ArmSoft.SME.Common.Resources.Employee, 12));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Partner, isSupplier: false, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract),
			SummaPartFieldDefinition = new SumFieldDefinition("SummaPart", () => ArmSoft.SME.Common.Resources.Summa)
		});
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("BankAcquiringPartnerGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => Resources.POSTerminalPart, isSupplier: false, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => Resources.POSTerminalContract)
		});
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("ECRReturnCheckNum", () => ArmSoft.SME.Common.Resources.ECRCheckNum, 8));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Base", () => ArmSoft.SME.Common.Resources.Base, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Appendix", () => Resources.Appendix, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("NPassport", () => Resources.NPassport, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("ReceiverName", () => ArmSoft.SME.Common.Resources.RecTO, 50, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("PartnerRem", () => Resources.PartnerRem, isRequired: false, isNonNegative: false));
		return tradeDocDefinition;
	}

	public static DocDefinition CreateEMarkReceipt()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.EMarkReceipt)
		{
			ChangingActualnessDate = false,
			AllowStoreAsDraft = true,
			NextDocNumParamId = 10199,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			HelpId = "Deregistration_Of_Stamps_From_Circulation",
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.EMarkReceipListJournal.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("EMarkReceiptNumber", () => Resources.EMarkReceiptNumber, 20));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => ArmSoft.SME.Common.Resources.ItemList, isRequired: true);
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 200));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("RowId", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int)));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateInvoiceForPay()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.InvoiceForPay)
		{
			ChangingActualnessDate = false,
			AllowStoreAsDraft = true,
			IsDocNumberUnique = true,
			NextDocNumParamId = 10001,
			AttachmentsTypes = (AttachType.File | AttachType.Link),
			IsExchangeable = true,
			HelpId = "Sales_Documents_TransferInvoice",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.WrittenOffInvoicesForPay.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankIn.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRate, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => Resources.PartnerContracts)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Buyer, isSupplier: false, isCustomer: true),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract)
		});
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("ContractDate", () => ArmSoft.SME.Common.Resources.ContractDate));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("VATType", () => ArmSoft.SME.Common.Resources.VATCalculationType, 1, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PriceListType", () => Resources.PriceType, 2));
		tradeDocDefinition.AddFieldDefinition(new PercentFieldDefinition("Discount", () => Resources.DiscountPercentSymbol, isRequired: false, isNonNegative: false));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummVAT", () => ArmSoft.SME.Common.Resources.SummaVAT));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.TotalSum, isRequired: true));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => ArmSoft.SME.Common.Resources.ItemList, isRequired: true);
		InvoiceBase.AddDefinitionsToMTInvoiceGrid(gridDefinition);
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("Price", () => ArmSoft.SME.Common.Resources.Price, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierName", () => ArmSoft.SME.Common.Resources.Name, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierLegalAddress", () => ArmSoft.SME.Common.Resources.LegalAddresses, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierBusinessAddress", () => ArmSoft.SME.Common.Resources.BusinessAddresses, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierSettlAcc", () => ArmSoft.SME.Common.Resources.SettlementAccount, 22));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierTaxCode", () => ArmSoft.SME.Common.Resources.OptionsTaxCode, 20));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierMngPost", () => ArmSoft.SME.Common.Resources.ManagerPost, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierMngName", () => ArmSoft.SME.Common.Resources.ManagerFIO, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierChiefAccPost", () => ArmSoft.SME.Common.Resources.AccountantPost, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierChiefAccName", () => ArmSoft.SME.Common.Resources.ChAccFIO, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerName", () => ArmSoft.SME.Common.Resources.Name, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerLegalAddress", () => ArmSoft.SME.Common.Resources.LegalAddresses, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerBusinessAddress", () => ArmSoft.SME.Common.Resources.BusinessAddresses, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerSettlAcc", () => ArmSoft.SME.Common.Resources.SettlementAccount, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerTaxCode", () => ArmSoft.SME.Common.Resources.OptionsTaxCode, 20));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerMngPost", () => ArmSoft.SME.Common.Resources.ManagerPost, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerMngName", () => ArmSoft.SME.Common.Resources.ManagerFIO, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerChiefAccPost", () => ArmSoft.SME.Common.Resources.AccountantPost, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerChiefAccName", () => ArmSoft.SME.Common.Resources.ChAccFIO, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerStateRegNum", () => Resources.PartStateRegister, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerPassport", () => ArmSoft.SME.Common.Resources.PassportNum, 32));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTBill()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = true,
			SetDuplicateDocNumAutomatic = true,
			NextDocNumParamId = 161,
			AttachmentsTypes = (AttachType.File | AttachType.Link),
			HelpId = "Sales_Documents_PosSale",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepTAccount.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashDeskTAccount.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.TaxInvoicesJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.SaleDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ECRCheckDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ECRCheckDocumentsListJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepSalesAnalysis.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BonusOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BonusTAccount.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankIn.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value
			},
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.InvoiceForPay.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true, logChanges: true, null, "Date"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true, logChanges: true, null, "DocumentNumber"));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Buyer, isSupplier: false, isCustomer: true),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract)
		});
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummaChange", () => Resources.SummaChange, isRequired: false, isNonNegative: false, logChanges: true, null, "ChangeAmount"));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("BankAcquiring", () => ArmSoft.SME.Common.Resources.Bank)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => Resources.POSTerminalPart, isSupplier: false, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => Resources.POSTerminalContract)
		});
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummaCash", () => Resources.CashPaymentForPrint, isRequired: false, isNonNegative: true, logChanges: true, null, "CashAmount"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummaPlCard", () => Resources.PlasticCartPaymentDialog, isRequired: false, isNonNegative: true, logChanges: true, null, "NonCashAmount"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummaCredit", () => Resources.CreditSale, isRequired: false, isNonNegative: true, logChanges: true, null, "CreditAmount"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummaPayByBonus", () => Resources.PayByBonus, isRequired: false, isNonNegative: true, logChanges: true, null, "BonusAmount"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummaAccumulatedBonus", () => Resources.AccumulatedBonusShort, isRequired: false, isNonNegative: true, logChanges: true, null, "AccumulatedBonus"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SumAddedBonus", () => Resources.BonusAddShort, isRequired: false, isNonNegative: true, logChanges: true, null, "AddedBonus"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummaPrePayment", () => Resources.PrePayment, isRequired: false, isNonNegative: true, logChanges: true, null, "PrepaymentAmount"));
		tradeDocDefinition.AddFieldDefinition(new BoolFieldDefinition("PrintPartnerTinOnCheck", () => ArmSoft.SME.Common.Resources.PrintExpenseJustifyingCheck, isRequired: false, logChanges: true, null, "PrintExpenseConfirmingCheck"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("POSTerminalType", () => Resources.PluggedDevice, 1, isRequired: false, logChanges: true, null, "ConnectedDevice"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("ECRCheckNum", () => ArmSoft.SME.Common.Resources.ECRCheckNum, 8, isRequired: false, logChanges: true, null, "ECRCheckNumber"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PrePaymentECRCheckNum", () => Resources.PrePaymentECRCheckNum, 8, isRequired: false, logChanges: true, null, "PrepaymentECRCheckNumber"));
		tradeDocDefinition.AddFieldDefinition(new NumberFieldDefinition("NonCashPaymentMethod", () => ArmSoft.SME.Common.Resources.PaymentSystem, 2, 0, isNonNegative: true, isRequired: false, null, logChanges: true, null, "PaymentSystem"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PriceListType", () => Resources.PriceType, 2, isRequired: false, logChanges: true, null, "PriceType"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DiscountPriceListType", () => Resources.DiscountPriceType, 2, isRequired: false, logChanges: true, null, "DiscountPriceType"));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PrepaymentGroup", () => ArmSoft.SME.Common.Resources.Bank)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Buyer, isSupplier: false, isCustomer: true),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract)
		});
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PaymentId", () => ArmSoft.SME.Common.Resources.PaymentId, 100));
		tradeDocDefinition.AddFieldDefinition(new BoolFieldDefinition("PaymentAccepted", () => ArmSoft.SME.Common.Resources.State));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255, isRequired: false, logChanges: true, null, "Comment"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50, isRequired: false, logChanges: true, null, "Owner"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRateShort, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("VATType", () => ArmSoft.SME.Common.Resources.VATCalculationType, 1, isRequired: true, logChanges: true, null, "VATCalculationType"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummVAT", () => ArmSoft.SME.Common.Resources.SummaVAT, isRequired: false, isNonNegative: true, logChanges: true, null, "VATAmount"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.Total, isRequired: false, isNonNegative: true, logChanges: true, null, "TotalAmount"));
		tradeDocDefinition.AddFieldDefinition(new PercentFieldDefinition("Discount", () => Resources.DiscountPercentSymbol, isRequired: false, isNonNegative: false, logChanges: true, null, "DiscountPercent"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SalesConsultant", () => Resources.SalesConsultant, 12, isRequired: false, logChanges: true, null, "Seller"));
		tradeDocDefinition.AddFieldDefinition(new BoolFieldDefinition("IsCheckLimitAnswerYes", () => Resources.ReceivablesConfirmExceed));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("TaxInvoiceSeries", () => ArmSoft.SME.Common.Resources.Serial, 1));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("TaxInvoiceNumber", () => ArmSoft.SME.Common.Resources.Number, 10));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("InvoiceDate", () => ArmSoft.SME.Common.Resources.SubmissionDate));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("ExportedType", () => ArmSoft.SME.Common.Resources.TaxInvoiceExportForm, 1));
		tradeDocDefinition.AddFieldDefinition(new BoolFieldDefinition("IsDeferred", () => Resources.DeferShort));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5, isRequired: false, logChanges: true, null, "Storage"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CashDesk", () => ArmSoft.SME.Common.Resources.Cash, 8, isRequired: true, logChanges: true, null, "CashDesk"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CashierShiftNumber", () => Resources.CashierShiftNum, 12, isRequired: false, logChanges: true, null, "CashierShiftNumber"));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => ArmSoft.SME.Common.Resources.ItemList, isRequired: true, logChanges: true, null, "ItemList");
		InvoiceBase.AddDefinitionsToMTInvoiceGrid(gridDefinition);
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: false, isNonNegative: true, logChanges: true, null, "Quantity"));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("Price", () => ArmSoft.SME.Common.Resources.Price, isRequired: false, logChanges: true, null, "SalePrice"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5, isRequired: false, logChanges: true, null, "Storage"));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("UsedBonus", () => Resources.BonusUse, isRequired: false, isNonNegative: true, logChanges: true, null, "UsedBonus"));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("IsGift", () => Resources.Gift, isRequired: false, logChanges: true, null, "Gift"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("BarCode", () => ArmSoft.SME.Common.Resources.BarCode, 20, isRequired: false, logChanges: true, null, "Barcode"));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("DiscountSumma", () => Resources.DiscountSumm, isRequired: false, isNonNegative: false, logChanges: true, null, "DiscountAmount"));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("IsRowMarkable", () => ArmSoft.SME.Common.Resources.Markable));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MarkingStatus", () => ArmSoft.SME.Common.Resources.Marking, 19));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("AccumulatedBonus", () => Resources.AccumulateBonus, isRequired: false, isNonNegative: true, logChanges: true, null, "AccumulatedBonus"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("RowDescription", () => ArmSoft.SME.Common.Resources.Description, 50, isRequired: false, logChanges: true, null, "Description"));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("IsDeleted", () => Resources.DeletedShort, isRequired: false, logChanges: true, null, "Deleted"));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTBillReturn()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = true,
			SetDuplicateDocNumAutomatic = true,
			NextDocNumParamId = 10124,
			HelpId = "Sales_Documents_PosReturn",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepTAccount.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashDeskTAccount.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.SaleDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ECRCheckDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ECRCheckDocumentsListJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepSalesAnalysis.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BonusOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BonusTAccount.Value
			},
			BaseDocTypes = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value }
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true, logChanges: true, null, "Date"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true, logChanges: true, null, "DocumentNumber"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BaseISN", () => ArmSoft.SME.Common.Resources.BaseDoc, 50, isRequired: true, logChanges: true, null, "BaseDocumentISN"));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Buyer, isSupplier: false, isCustomer: true),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract)
		});
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummaChange", () => Resources.SummaChange, isRequired: false, isNonNegative: false, logChanges: true, null, "ChangeAmount"));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("BankAcquiring", () => ArmSoft.SME.Common.Resources.Bank)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => Resources.POSTerminalPart, isSupplier: false, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => Resources.POSTerminalContract)
		});
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("ECRReturnByCardSum", () => Resources.PlasticCartPaymentDialog, isRequired: false, isNonNegative: true, logChanges: true, null, "NonCashAmount"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("ECRReturnByCashSum", () => Resources.CashPaymentForPrint, isRequired: false, isNonNegative: false, logChanges: true, null, "CashAmount"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("ECRReturnByPrePaymentSum", () => Resources.PrePayment, isRequired: false, isNonNegative: true, logChanges: true, null, "PrepaymentAmount"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummaCredit", () => Resources.CreditSale, isRequired: false, isNonNegative: true, logChanges: true, null, "CreditAmount"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummaPayByBonus", () => Resources.PayByBonus, isRequired: false, isNonNegative: true, logChanges: true, null, "BonusAmount"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("ECRCheckNum", () => ArmSoft.SME.Common.Resources.ECRCheckNum, 8, isRequired: false, logChanges: true, null, "ECRReturnCheckNumber"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PrePaymentECRCheckNum", () => Resources.PrePaymentECRCheckNum, 8, isRequired: false, logChanges: true, null, "PrepaymentECRCheckNumber"));
		tradeDocDefinition.AddFieldDefinition(new NumberFieldDefinition("NonCashPaymentMethod", () => ArmSoft.SME.Common.Resources.PaymentSystem, 2, 0, isNonNegative: true, isRequired: false, null, logChanges: true, null, "PaymentSystem"));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PrepaymentGroup", () => ArmSoft.SME.Common.Resources.Bank)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Buyer, isSupplier: false, isCustomer: true),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract)
		});
		tradeDocDefinition.AddFieldDefinition(new BoolFieldDefinition("PaymentRefundAccepted", () => ArmSoft.SME.Common.Resources.State));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255, isRequired: false, logChanges: true, null, "Comment"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50, isRequired: false, logChanges: true, null, "Owner"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRateShort, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("VATType", () => ArmSoft.SME.Common.Resources.VATCalculationType, 1, isRequired: true, logChanges: true, null, "VATCalculationType"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummVAT", () => ArmSoft.SME.Common.Resources.SummaVAT, isRequired: false, isNonNegative: true, logChanges: true, null, "VATAmount"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.Total, isRequired: false, isNonNegative: true, logChanges: true, null, "TotalAmount"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SalesConsultant", () => Resources.SalesConsultant, 12, isRequired: false, logChanges: true, null, "Seller"));
		tradeDocDefinition.AddFieldDefinition(new BoolFieldDefinition("IsDeferred", () => Resources.DeferShort));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5, isRequired: false, logChanges: true, null, "Storage"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CashDesk", () => ArmSoft.SME.Common.Resources.Cash, 8, isRequired: true, logChanges: true, null, "CashDesk"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CashierShiftNumber", () => Resources.CashierShiftNum, 12, isRequired: false, logChanges: true, null, "CashierShiftNumber"));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => ArmSoft.SME.Common.Resources.ItemList, isRequired: true, logChanges: true, null, "ItemList");
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5, isRequired: false, logChanges: true, null, "Storage"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("BarCode", () => ArmSoft.SME.Common.Resources.BarCode, 20, isRequired: false, logChanges: true, null, "Barcode"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ItemCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true, logChanges: true, null, "Code"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ItemName", () => ArmSoft.SME.Common.Resources.Name, 200, isRequired: false, logChanges: true, null, "Name"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true, logChanges: true, null, "Unit"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6, isRequired: false, logChanges: true, null, "UnitAbbreviation"));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true, isNonNegative: true, logChanges: true, null, "Quantity"));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("BeginPrice", () => ArmSoft.SME.Common.Resources.MTTAccSalePrice, isRequired: true, logChanges: true, null, "SalePrice"));
		gridDefinition.AddFieldDefinition(new PercentFieldDefinition("Discount", () => ArmSoft.SME.Common.Resources.DiscountPercent, isRequired: false, isNonNegative: false, logChanges: true, null, "DiscountPercent"));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("DiscountPrice", () => ArmSoft.SME.Common.Resources.DiscountedPrice, isRequired: false, logChanges: true, null, "DiscountedPrice"));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Summa", () => ArmSoft.SME.Common.Resources.Summa, isRequired: false, isNonNegative: true, logChanges: true, null, "DiscountedAmount"));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("DiscountSumma", () => Resources.DiscountSumm, isRequired: false, isNonNegative: false, logChanges: true, null, "DiscountAmount"));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("UsedBonus", () => Resources.BonusUse, isRequired: false, isNonNegative: true, logChanges: true, null, "UsedBonus"));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("IsRowMarkable", () => ArmSoft.SME.Common.Resources.Markable));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("VAT", () => ArmSoft.SME.Common.Resources.OptionsVAT, isRequired: false, logChanges: true, null, "VAT"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("RowDescription", () => ArmSoft.SME.Common.Resources.Description, 50, isRequired: false, logChanges: true, null, "Description"));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("AccumulatedBonus", () => Resources.AccumulateBonus, isRequired: false, isNonNegative: true, logChanges: true, null, "AccumulatedBonus"));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("RowID", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int), logChanges: true, null, "RowId"));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("IsDeleted", () => Resources.DeletedShort, isRequired: false, logChanges: true, null, "Deleted"));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition MTComplectation()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 124,
			HelpId = "Warehouse_Documents_Complectation",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.WarehouseDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepTAccount.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => Resources.DocumentDate, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("StorageExpense", () => Resources.OutStorage, 5));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("StorageIncome", () => Resources.InStorage, 5, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("MTComplectCode", () => ArmSoft.SME.Common.Resources.Complect, 20, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("QuantityUnit", () => ArmSoft.SME.Common.Resources.Unit, 13, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("UseLife", () => Resources.UseLife));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Country", () => ArmSoft.SME.Common.Resources.Country, 3));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Producer", () => ArmSoft.SME.Common.Resources.Producer, 4));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Property1", () => Resources.Attribute1, 20));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Property2", () => Resources.Attribute2, 20));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		GridDefinition gridDefinition = new GridDefinition("ComplectPartsList", () => Resources.MTComplectsParts, isRequired: true);
		gridDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("ItemType", () => ArmSoft.SME.Common.Resources.TypeWord, typeof(GoodsTypesSE)));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ItemCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ItemName", () => ArmSoft.SME.Common.Resources.Name, 200));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Summa", () => ArmSoft.SME.Common.Resources.Summa, isRequired: false, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("RowID", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int)));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition MTDecomplectation()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTDeComplectation)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 125,
			HelpId = "Warehouse_Documents_UnComplectation",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.WarehouseDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepTAccount.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("StorageExpense", () => Resources.OutStorage, 5, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("StorageIncome", () => Resources.InStorage, 5, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("MTComplectCode", () => ArmSoft.SME.Common.Resources.Complect, 20, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("QuantityUnit", () => ArmSoft.SME.Common.Resources.Unit, 13, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		GridDefinition gridDefinition = new GridDefinition("ComplectPartsList", () => Resources.MTComplectsParts, isRequired: true);
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ItemCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ItemName", () => ArmSoft.SME.Common.Resources.Name, 200));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("PricePart", () => Resources.SummaPart, 16, 3, isNonNegative: true, isRequired: true));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("SummaPart", () => Resources.SummPart, 16, 3, isNonNegative: true, isRequired: true));
		gridDefinition.AddFieldDefinition(new DateFieldDefinition("UseLife", () => Resources.UseLife));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Country", () => ArmSoft.SME.Common.Resources.Country, 3));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Producer", () => ArmSoft.SME.Common.Resources.Producer, 4));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property1", () => Resources.Attribute1, 20));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property1Caption", () => Resources.Attribute1, 50));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property2", () => Resources.Attribute2, 20));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property2Caption", () => Resources.Attribute2, 50));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("RowID", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int)));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTExpense()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTExpense)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 71,
			HelpId = "Warehouse_Documents_WriteOff",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.WarehouseDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepTAccount.Value
			},
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTaking,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTakingRetail
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Trust", () => ArmSoft.SME.Common.Resources.Trust, 6));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Mediator", () => ArmSoft.SME.Common.Resources.Mediator, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Machine", () => ArmSoft.SME.Common.Resources.Machine, 9));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Chief", () => ArmSoft.SME.Common.Resources.ChAccPost1, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Allower", () => ArmSoft.SME.Common.Resources.Allower, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Demand", () => ArmSoft.SME.Common.Resources.Demand, 255));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => Resources.MTLIST, isRequired: true);
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("RowID", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int)));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTName", () => ArmSoft.SME.Common.Resources.Name, 200));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("SalePrice", () => ArmSoft.SME.Common.Resources.MTTAccSalePrice));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Summa", () => ArmSoft.SME.Common.Resources.Summa, isRequired: false, isNonNegative: false));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition MTIncome()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 70,
			HelpId = "Purchases_Documents_Income",
			AttachmentsTypes = (AttachType.File | AttachType.Link),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTaking.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankOut.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTOverheadExp.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder.Value
			},
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PurchaseDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepTAccount.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.WarehouseDocumentsJournal.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true, logChanges: true, null, "Date"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true, logChanges: true, null, "DocumentNumber"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5, isRequired: true, logChanges: true, null, "Storage"));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Supplier, isSupplier: true, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract),
			SummaPartFieldDefinition = new SumFieldDefinition("SummaPart", () => ArmSoft.SME.Common.Resources.Summa)
		});
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("InvoiceNumber", () => Resources.InvoiceNumber, 20, isRequired: false, logChanges: true, null, "PurchaseDocumentNumber"));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("InvoiceDate", () => ArmSoft.SME.Common.Resources.DateShort, isRequired: false, logChanges: true, null, "PurchaseDocumentDate"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255, isRequired: false, logChanges: true, null, "Comment"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50, isRequired: false, logChanges: true, null, "Owner"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true, logChanges: true, null, "Currency"));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRate, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("VATType", () => ArmSoft.SME.Common.Resources.VATCalculationType, 1, isRequired: true, logChanges: true, null, "VATCalculationType"));
		tradeDocDefinition.AddFieldDefinition(new BoolFieldDefinition("IncludeVATInCost", () => Resources.IncludeVATInCost, isRequired: false, logChanges: true, null, "IncludeVATInCost"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PriceType", () => ArmSoft.SME.Common.Resources.PriceListType, 2, isRequired: false, logChanges: true, null, "PriceType"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Trust", () => ArmSoft.SME.Common.Resources.Trust, 6, isRequired: false, logChanges: true, null, "LetterOfAttorney"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Mediator", () => ArmSoft.SME.Common.Resources.Mediator, 12, isRequired: false, logChanges: true, null, "Mediator"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Machine", () => ArmSoft.SME.Common.Resources.Machine, 9, isRequired: false, logChanges: true, null, "Vehicle"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Chief", () => ArmSoft.SME.Common.Resources.ChAccPost1, 100, isRequired: false, logChanges: true, null, "ChiefAccount"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Allower", () => ArmSoft.SME.Common.Resources.Allower, 50, isRequired: false, logChanges: true, null, "Allowed"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Response", () => ArmSoft.SME.Common.Resources.Respons, 50, isRequired: false, logChanges: true, null, "Accepted"));
		tradeDocDefinition.AddFieldDefinition(new PercentFieldDefinition("Discount", () => ArmSoft.SME.Common.Resources.DiscountPercent, isRequired: false, isNonNegative: true, logChanges: true, null, "DiscountPercent"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummVAT", () => ArmSoft.SME.Common.Resources.SummaVAT, isRequired: false, isNonNegative: true, logChanges: true, null, "VATAmount"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.TotalSum, isRequired: false, isNonNegative: true, logChanges: true, null, "TotalAmount"));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => Resources.MTLIST, isRequired: true, logChanges: true, null, "ProductsList");
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true, logChanges: true, null, "Code"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTName", () => ArmSoft.SME.Common.Resources.Name, 200, isRequired: false, logChanges: true, null, "Name"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true, logChanges: true, null, "Unit"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6, isRequired: false, logChanges: true, null, "UnitAbbreviation"));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true, isNonNegative: true, logChanges: true, null, "Quantity"));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("LastPurchasePrice", () => Resources.LastPurchasePrice));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("VariancePercent", () => Resources.VariancePercent, 6, 2, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("Price", () => ArmSoft.SME.Common.Resources.Price, isRequired: false, logChanges: true, null, "Price"));
		gridDefinition.AddFieldDefinition(new PercentFieldDefinition("Discount", () => ArmSoft.SME.Common.Resources.DiscountPercent, isRequired: false, isNonNegative: false, logChanges: true, null, "DiscountPercent"));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("DiscountPrice", () => ArmSoft.SME.Common.Resources.DiscountedPrice, isRequired: false, logChanges: true, null, "DiscountedPrice"));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Summa", () => ArmSoft.SME.Common.Resources.Summa, isRequired: false, isNonNegative: true, logChanges: true, null, "Amount"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("RowDescription", () => ArmSoft.SME.Common.Resources.Description, 50, isRequired: false, logChanges: true, null, "Description"));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("VAT", () => ArmSoft.SME.Common.Resources.OptionsVAT, isRequired: false, logChanges: true, null, "VAT"));
		gridDefinition.AddFieldDefinition(new DateFieldDefinition("UseLife", () => Resources.UseLife, isRequired: false, logChanges: true, null, "ExpirationDate"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Country", () => ArmSoft.SME.Common.Resources.Country, 3, isRequired: false, logChanges: true, null, "Country"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Producer", () => ArmSoft.SME.Common.Resources.Producer, 4, isRequired: false, logChanges: true, null, "Producer"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property1", () => Resources.Attribute1, 20, isRequired: false, logChanges: true, null, "Property1"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property1Caption", () => Resources.Attribute1, 50));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property2", () => Resources.Attribute1, 20, isRequired: false, logChanges: true, null, "Property2"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property2Caption", () => Resources.Attribute2, 50));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("RowID", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int), logChanges: true, null, "RowId"));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTIncomeImport()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeImport)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			AttachmentsTypes = (AttachType.File | AttachType.Link),
			NextDocNumParamId = 168,
			HelpId = "Purchases_Documents_IncomeImport",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PurchaseDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.WarehouseDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepTAccount.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsRetail.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTOverheadExp.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Supplier, isSupplier: true, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract),
			SummaPartFieldDefinition = new SumFieldDefinition("SummaPart", () => ArmSoft.SME.Common.Resources.Summa)
		});
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("CustomContractContainer", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => Resources.CustomHouse, isSupplier: false, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract),
			SummaPartFieldDefinition = new SumFieldDefinition("SummaPart", () => ArmSoft.SME.Common.Resources.Summa)
		});
		tradeDocDefinition.AddFieldDefinition(new PriceFieldDefinition("CustomFee", () => Resources.CustomFee));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("InvoiceNumber", () => Resources.InvoiceNumber, 20));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRateShort, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PriceType", () => Resources.PriceType, 2));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Trust", () => ArmSoft.SME.Common.Resources.Trust, 6));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Mediator", () => ArmSoft.SME.Common.Resources.Mediator, 12));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Machine", () => ArmSoft.SME.Common.Resources.Machine, 9));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Chief", () => ArmSoft.SME.Common.Resources.ChAccPost1, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Allower", () => ArmSoft.SME.Common.Resources.Allower, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Response", () => ArmSoft.SME.Common.Resources.Respons, 50));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummVAT", () => ArmSoft.SME.Common.Resources.SummaVAT + " (AMD)"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.Summa + " (AMD)", isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("CustomDuty", () => Resources.CustomDuty + " (AMD)"));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => Resources.MTLIST, isRequired: true);
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("LastPurchasePrice", () => Resources.LastPurchasePrice));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("VariancePercent", () => Resources.VariancePercent, 6, 2, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Price", () => ArmSoft.SME.Common.Resources.Price));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Summa", () => ArmSoft.SME.Common.Resources.Summa, isRequired: true));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("CustomSumma", () => Resources.CustomSumma, isRequired: true));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("CustomPercent", () => ArmSoft.SME.Common.Resources.Percent, 5, 2));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("CustomDuty", () => Resources.CustomDuty));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("VAT", () => ArmSoft.SME.Common.Resources.OptionsVAT));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("VATSumma", () => ArmSoft.SME.Common.Resources.SummaVAT));
		gridDefinition.AddFieldDefinition(new DateFieldDefinition("UseLife", () => Resources.UseLife));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Country", () => ArmSoft.SME.Common.Resources.Country, 3));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Producer", () => ArmSoft.SME.Common.Resources.Producer, 4));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property1", () => Resources.Attribute1, 20));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property1Caption", () => Resources.Attribute1, 50));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property2", () => Resources.Attribute1, 20));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property2Caption", () => Resources.Attribute2, 50));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTName", () => ArmSoft.SME.Common.Resources.Name, 200));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("RowDescription", () => ArmSoft.SME.Common.Resources.Description, 50));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("RowID", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int)));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTIncomeRem()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRem)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = true,
			NextDocNumParamId = 174,
			HelpId = "Purchases_Documents_SupportingDocuments_productsbalancesaccess",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.WarehouseDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PurchaseDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepTAccount.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsRetail.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Supplier, isSupplier: true, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract)
		});
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRateShort, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummVAT", () => ArmSoft.SME.Common.Resources.TotalSum + Resources.WithNDS));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.TotalSum + Resources.WithoutNDS));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummaCurr", () => ArmSoft.SME.Common.Resources.TotalSum));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => Resources.MTLIST, isRequired: true);
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTName", () => ArmSoft.SME.Common.Resources.Name, 200));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("Price", () => ArmSoft.SME.Common.Resources.Price + Resources.WithoutNDS));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Summa", () => ArmSoft.SME.Common.Resources.Summa + Resources.WithoutNDS));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("VAT", () => ArmSoft.SME.Common.Resources.OptionsVAT));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("VATPrice", () => ArmSoft.SME.Common.Resources.Price + Resources.WithNDS));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("VATSumma", () => ArmSoft.SME.Common.Resources.Summa + Resources.WithNDS));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("CurrPrice", () => ArmSoft.SME.Common.Resources.Price));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("CurrSumma", () => ArmSoft.SME.Common.Resources.NewPrice));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("SalePrice", () => Resources.NCCSalePrice));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("SaleSumma", () => Resources.MatRepSaleTotalSum));
		gridDefinition.AddFieldDefinition(new DateFieldDefinition("UseLife", () => Resources.UseLife));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Country", () => ArmSoft.SME.Common.Resources.Country, 3));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Producer", () => ArmSoft.SME.Common.Resources.Producer, 4));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property1", () => Resources.Attribute1, 20));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property1Caption", () => Resources.Attribute1, 50));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property2", () => Resources.Attribute2, 20));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property2Caption", () => Resources.Attribute2, 50));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("RowID", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int)));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition MTIncomeRetail()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 170,
			HelpId = "Purchases_Documents_IncomeRetail",
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTaking.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTakingRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankOut.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTOverheadExp.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTRevaluationRetail.Value
			},
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PurchaseDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepTAccount.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.WarehouseDocumentsJournal.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Supplier, isSupplier: true, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract),
			SummaPartFieldDefinition = new SumFieldDefinition("SummaPart", () => ArmSoft.SME.Common.Resources.Summa)
		});
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("InvoiceNumber", () => Resources.InvoiceNumber, 20));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("InvoiceDate", () => ArmSoft.SME.Common.Resources.DateShort));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRate, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("VATType", () => ArmSoft.SME.Common.Resources.VATCalculationType, 1, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new BoolFieldDefinition("IncludeVATInCost", () => Resources.IncludeVATInCost));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PriceType", () => ArmSoft.SME.Common.Resources.PriceListType, 2));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Trust", () => ArmSoft.SME.Common.Resources.Trust, 6));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Mediator", () => ArmSoft.SME.Common.Resources.Mediator, 12));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Machine", () => ArmSoft.SME.Common.Resources.Machine, 9));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Chief", () => ArmSoft.SME.Common.Resources.ChAccPost1, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Allower", () => ArmSoft.SME.Common.Resources.Allower, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Response", () => ArmSoft.SME.Common.Resources.Respons, 50));
		tradeDocDefinition.AddFieldDefinition(new PercentFieldDefinition("Discount", () => ArmSoft.SME.Common.Resources.DiscountPercent));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummVAT", () => ArmSoft.SME.Common.Resources.SummaVAT));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.TotalSum));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => Resources.MTLIST, isRequired: true);
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTName", () => ArmSoft.SME.Common.Resources.Name, 200));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("LastPurchasePrice", () => Resources.LastPurchasePrice));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("VariancePercent", () => Resources.VariancePercent, 6, 2, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("Price", () => ArmSoft.SME.Common.Resources.Price));
		gridDefinition.AddFieldDefinition(new PercentFieldDefinition("Discount", () => ArmSoft.SME.Common.Resources.DiscountPercent, isRequired: false, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("DiscountPrice", () => ArmSoft.SME.Common.Resources.DiscountedPrice));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Summa", () => ArmSoft.SME.Common.Resources.Summa));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("RowDescription", () => ArmSoft.SME.Common.Resources.Description, 50));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("ExtraPercent", () => Resources.Extra, 6, 2, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("SalePrice", () => ArmSoft.SME.Common.Resources.MTTAccSalePrice));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("SaleSumma", () => Resources.SaleSum));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("VAT", () => ArmSoft.SME.Common.Resources.OptionsVAT));
		gridDefinition.AddFieldDefinition(new DateFieldDefinition("UseLife", () => Resources.UseLife));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Country", () => ArmSoft.SME.Common.Resources.Country, 3));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Producer", () => ArmSoft.SME.Common.Resources.Producer, 4));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property1", () => Resources.Attribute1, 20));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property1Caption", () => Resources.Attribute1, 50));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property2", () => Resources.Attribute1, 20));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property2Caption", () => Resources.Attribute2, 50));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("RowID", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int)));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTInvoice()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = true,
			NextDocNumParamId = 69,
			HelpId = "Sales_Documents_InvoiceSale",
			IsExchangeable = true,
			AttachmentsTypes = (AttachType.File | AttachType.Link),
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankIn.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTaking.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.InvoiceForPay.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankIn.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer.Value
			},
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.SaleDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.TaxInvoicesJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepSalesAnalysis.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepTAccount.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true, logChanges: true, null, "Date"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true, logChanges: true, null, "DocumentNumber"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BaseISN", () => ArmSoft.SME.Common.Resources.BaseDoc, 50, isRequired: false, logChanges: true, null, "BaseDocumentISN"));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => Resources.PartnerContracts)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Buyer, isSupplier: false, isCustomer: true),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract),
			SummaPartFieldDefinition = new SumFieldDefinition("SummaPart", () => ArmSoft.SME.Common.Resources.Summa)
		});
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BookNum", () => ArmSoft.SME.Common.Resources.SupBookN, 10, isRequired: false, logChanges: true, null, "BookNumber"));
		tradeDocDefinition.AddFieldDefinition(new NumberFieldDefinition("BookPage", () => ArmSoft.SME.Common.Resources.PageN, 6, 0, isNonNegative: true, isRequired: false, null, logChanges: true, null, "BookPage"));
		tradeDocDefinition.AddFieldDefinition(new NumberFieldDefinition("BookLine", () => ArmSoft.SME.Common.Resources.RowN, 6, 0, isNonNegative: true, isRequired: false, null, logChanges: true, null, "BookLine"));
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("ExportedType", () => ArmSoft.SME.Common.Resources.TaxInvoiceExportForm, typeof(TaxInvoicesWriteTypesSE), isRequired: false, logChanges: true, ValueMember.Code, null, null, "TaxExportType"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("TaxInvoiceSeries", () => ArmSoft.SME.Common.Resources.Serial, 1, isRequired: false, logChanges: true, null, "TaxInvoiceSeries"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("TaxInvoiceNumber", () => ArmSoft.SME.Common.Resources.Number, 10, isRequired: false, logChanges: true, null, "TaxInvoiceNumber"));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("InvoiceDate", () => ArmSoft.SME.Common.Resources.SubmissionDate, isRequired: false, logChanges: true, null, "SubmissionDate"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SalesConsultant", () => Resources.SalesConsultant, 12, isRequired: false, logChanges: true, null, "Seller"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255, isRequired: false, logChanges: true, null, "Comment"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50, isRequired: false, logChanges: true, null, "Owner"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true, logChanges: true, null, "Currency"));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRate, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("VATType", () => ArmSoft.SME.Common.Resources.VATCalculationType, 1, isRequired: true, logChanges: true, null, "VATCalculationType"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DealType", () => ArmSoft.SME.Common.Resources.DealType, 1, isRequired: false, logChanges: true, null, "DealType"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PriceListType", () => Resources.PriceType, 2, isRequired: false, logChanges: true, null, "PriceType"));
		tradeDocDefinition.AddFieldDefinition(new PercentFieldDefinition("Discount", () => Resources.DiscountPercentSymbol, isRequired: false, isNonNegative: false, logChanges: true, null, "DiscountPercent"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5, isRequired: false, logChanges: true, null, "Storage"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummVAT", () => ArmSoft.SME.Common.Resources.SummaVAT, isRequired: false, isNonNegative: true, logChanges: true, null, "VATAmount"));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.TotalSum, isRequired: false, isNonNegative: true, logChanges: true, null, "TotalAmount"));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => ArmSoft.SME.Common.Resources.ItemList, isRequired: true, logChanges: true, null, "ItemList");
		InvoiceBase.AddDefinitionsToMTInvoiceGrid(gridDefinition);
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true, isNonNegative: true, logChanges: true, null, "Quantity"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("DealType", () => ArmSoft.SME.Common.Resources.DealType, 1, isRequired: false, logChanges: true, null, "DealType"));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("Price", () => ArmSoft.SME.Common.Resources.Price, isRequired: true, logChanges: true, null, "SalePrice"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5, isRequired: false, logChanges: true, null, "Storage"));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("EnvFeePercent", () => ArmSoft.SME.Common.Resources.EnvFeePercent, 5, 2, isNonNegative: true, isRequired: false, null, logChanges: true, null, "EnvironmentalFeePercent"));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("EnvFeeSumma", () => ArmSoft.SME.Common.Resources.EnvFeeSumma, isRequired: false, isNonNegative: true, logChanges: true, null, "EnvironmentalFeeAmount"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("RowDescription", () => ArmSoft.SME.Common.Resources.Description, 50, isRequired: false, logChanges: true, null, "Description"));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("IsRowMarkable", () => ArmSoft.SME.Common.Resources.Markable));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MarkingStatus", () => ArmSoft.SME.Common.Resources.Marking, 19));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierName", () => ArmSoft.SME.Common.Resources.Name, 100, isRequired: false, logChanges: true, null, "SupplierName"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierLegalAddress", () => ArmSoft.SME.Common.Resources.LegalAddresses, 255, isRequired: false, logChanges: true, null, "SupplierLegalAddress"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierBusinessAddress", () => ArmSoft.SME.Common.Resources.BusinessAddresses, 255, isRequired: false, logChanges: true, null, "SupplierBusinessAddress"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierSettlAcc", () => ArmSoft.SME.Common.Resources.SettlementAccount, 22, isRequired: false, logChanges: true, null, "SupplierSettlementAccount"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierTaxCode", () => ArmSoft.SME.Common.Resources.OptionsTaxCode, 20, isRequired: false, logChanges: true, null, "SupplierTaxCode"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierMngPost", () => ArmSoft.SME.Common.Resources.ManagerPost, 100, isRequired: false, logChanges: true, null, "SupplierManagerPosition"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierMngName", () => ArmSoft.SME.Common.Resources.ManagerFIO, 100, isRequired: false, logChanges: true, null, "SupplierManagerName"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierChiefAccPost", () => ArmSoft.SME.Common.Resources.AccountantPost, 100, isRequired: false, logChanges: true, null, "SupplierAccountantPosition"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierChiefAccName", () => ArmSoft.SME.Common.Resources.ChAccFIO, 100, isRequired: false, logChanges: true, null, "SupplierAccountantName"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierAdditionalData", () => ArmSoft.SME.Common.Resources.AdditionalData, 255, isRequired: false, logChanges: true, null, "SupplierAdditionalData"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerName", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: false, logChanges: true, null, "CustomerName"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerLegalAddress", () => ArmSoft.SME.Common.Resources.LegalAddresses, 255, isRequired: false, logChanges: true, null, "CustomerLegalAddress"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerBusinessAddress", () => ArmSoft.SME.Common.Resources.BusinessAddresses, 255, isRequired: false, logChanges: true, null, "CustomerBusinessAddress"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerSettlAcc", () => ArmSoft.SME.Common.Resources.SettlementAccount, 50, isRequired: false, logChanges: true, null, "CustomerSettlementAccount"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerTaxCode", () => ArmSoft.SME.Common.Resources.OptionsTaxCode, 20, isRequired: false, logChanges: true, null, "CustomerTaxCode"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerMngPost", () => ArmSoft.SME.Common.Resources.ManagerPost, 50, isRequired: false, logChanges: true, null, "CustomerManagerPosition"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerMngName", () => ArmSoft.SME.Common.Resources.ManagerFIO, 50, isRequired: false, logChanges: true, null, "CustomerManagerName"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerChiefAccPost", () => ArmSoft.SME.Common.Resources.AccountantPost, 50, isRequired: false, logChanges: true, null, "CustomerAccountantPosition"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerChiefAccName", () => ArmSoft.SME.Common.Resources.ChAccFIO, 50, isRequired: false, logChanges: true, null, "CustomerAccountantName"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerStateRegNum", () => Resources.PartStateRegister, 50, isRequired: false, logChanges: true, null, "StateRegisterNumber"));
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("BuyerIDDocType", () => Resources.IDDocType, typeof(IdentityTypeSE), isRequired: false, logChanges: true, ValueMember.Code, null, null, "IdDocumentType"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerPassport", () => ArmSoft.SME.Common.Resources.PassportNum, 32, isRequired: false, logChanges: true, null, "PassportNumber"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerAdditionalData", () => ArmSoft.SME.Common.Resources.AdditionalData, 255, isRequired: false, logChanges: true, null, "CustomerAdditionalData"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerShippingMethod", () => ArmSoft.SME.Common.Resources.ShippingMethod, 255, isRequired: false, logChanges: true, null, "ShippingMethod"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("ByWhomFio", () => ArmSoft.SME.Common.Resources.FIO, 255, isRequired: false, logChanges: true, null, "AdditionalFullName"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("AuthorizationNumber", () => ArmSoft.SME.Common.Resources.CredentialNumber, 255, isRequired: false, logChanges: true, null, "CredentialNumber"));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("AuthorizationDate", () => ArmSoft.SME.Common.Resources.CredentialDate, isRequired: false, logChanges: true, null, "CredentialDate"));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTInvoiceRetail()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = true,
			NextDocNumParamId = 172,
			HelpId = "Sales_Documents_RetailSale",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankIn.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTakingRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value
			},
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.WrittenOffInvoicesForPay.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.SaleDocumentsJournal.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankIn.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BaseISN", () => ArmSoft.SME.Common.Resources.BaseDoc, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => Resources.PartnerContracts)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Buyer, isSupplier: false, isCustomer: true),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract),
			SummaPartFieldDefinition = new SumFieldDefinition("SummaPart", () => ArmSoft.SME.Common.Resources.Summa)
		});
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("ExportedType", () => ArmSoft.SME.Common.Resources.TaxInvoiceExportForm, typeof(TaxInvoicesWriteTypesSE)));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("TaxInvoiceSeries", () => ArmSoft.SME.Common.Resources.Serial, 1));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("TaxInvoiceNumber", () => ArmSoft.SME.Common.Resources.Number, 10));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("InvoiceDate", () => ArmSoft.SME.Common.Resources.SubmissionDate));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SalesConsultant", () => Resources.SalesConsultant, 12));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRateShort));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("VATType", () => ArmSoft.SME.Common.Resources.VATCalculationType, 1, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DealType", () => ArmSoft.SME.Common.Resources.DealType, 1));
		tradeDocDefinition.AddFieldDefinition(new PercentFieldDefinition("Discount", () => Resources.DiscountPercentSymbol, isRequired: false, isNonNegative: false));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummVAT", () => ArmSoft.SME.Common.Resources.SummaVAT));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.TotalSum));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => ArmSoft.SME.Common.Resources.ItemList, isRequired: true);
		InvoiceBase.AddDefinitionsToMTInvoiceGrid(gridDefinition);
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("DealType", () => ArmSoft.SME.Common.Resources.DealType, 1));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("Price", () => ArmSoft.SME.Common.Resources.Price, isRequired: true));
		gridDefinition.AddFieldDefinition(new PercentFieldDefinition("EnvFeePercent", () => ArmSoft.SME.Common.Resources.EnvFeePercent));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("EnvFeeSumma", () => ArmSoft.SME.Common.Resources.EnvFeeSumma));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("RowDescription", () => ArmSoft.SME.Common.Resources.Description, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierName", () => ArmSoft.SME.Common.Resources.Name, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierLegalAddress", () => ArmSoft.SME.Common.Resources.LegalAddresses, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierBusinessAddress", () => ArmSoft.SME.Common.Resources.BusinessAddresses, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierSettlAcc", () => ArmSoft.SME.Common.Resources.SettlementAccount, 22));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierTaxCode", () => ArmSoft.SME.Common.Resources.OptionsTaxCode, 20));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierMngPost", () => ArmSoft.SME.Common.Resources.ManagerPost, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierMngName", () => ArmSoft.SME.Common.Resources.ManagerFIO, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierChiefAccPost", () => ArmSoft.SME.Common.Resources.AccountantPost, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierChiefAccName", () => ArmSoft.SME.Common.Resources.ChAccFIO, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierAdditionalData", () => ArmSoft.SME.Common.Resources.AdditionalData, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerName", () => ArmSoft.SME.Common.Resources.Name, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerLegalAddress", () => ArmSoft.SME.Common.Resources.LegalAddresses, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerBusinessAddress", () => ArmSoft.SME.Common.Resources.BusinessAddresses, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerSettlAcc", () => ArmSoft.SME.Common.Resources.SettlementAccount, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerTaxCode", () => ArmSoft.SME.Common.Resources.OptionsTaxCode, 20));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerMngPost", () => ArmSoft.SME.Common.Resources.ManagerPost, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerMngName", () => ArmSoft.SME.Common.Resources.ManagerFIO, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerChiefAccPost", () => ArmSoft.SME.Common.Resources.AccountantPost, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerChiefAccName", () => ArmSoft.SME.Common.Resources.ChAccFIO, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerStateRegNum", () => Resources.PartStateRegister, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerIDDocType", () => Resources.IDDocType, 1));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerPassport", () => ArmSoft.SME.Common.Resources.PassportNum, 32));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerAdditionalData", () => ArmSoft.SME.Common.Resources.AdditionalData, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerShippingMethod", () => ArmSoft.SME.Common.Resources.ShippingMethod, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("ByWhomFio", () => ArmSoft.SME.Common.Resources.FIO, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("AuthorizationNumber", () => ArmSoft.SME.Common.Resources.CredentialNumber, 50));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("AuthorizationDate", () => ArmSoft.SME.Common.Resources.CredentialDate));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTMove()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 72,
			HelpId = "Warehouse_Documents_Movement",
			IsExchangeable = true,
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeImport.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveOrder.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value
			},
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.WarehouseDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.TransportationDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepTAccount.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsRetail.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTRevaluation.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTRevaluationRetail.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true, logChanges: true, null, "Date"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true, logChanges: true, null, "DocumentNumber"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BaseISN", () => ArmSoft.SME.Common.Resources.BaseDoc, 50, isRequired: false, logChanges: true, null, "BaseDocumentISN"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("StorageExpense", () => Resources.OutStorage, 5, isRequired: true, logChanges: true, null, "StorageOutput"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("StorageIncome", () => Resources.InStorage, 5, isRequired: true, logChanges: true, null, "StorageInput"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PriceType", () => Resources.PriceType, 2, isRequired: false, logChanges: true, null, "PriceType"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255, isRequired: false, logChanges: true, null, "Comment"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50, isRequired: false, logChanges: true, null, "Owner"));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => Resources.MTLIST, isRequired: true, logChanges: true, null, "ProductsList");
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("RowID", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int), logChanges: true, null, "RowId"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true, logChanges: true, null, "Code"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTName", () => ArmSoft.SME.Common.Resources.Name, 200, isRequired: false, logChanges: true, null, "Name"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true, logChanges: true, null, "Unit"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6, isRequired: false, logChanges: true, null, "UnitAbbreviation"));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true, isNonNegative: true, logChanges: true, null, "Quantity"));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Price", () => ArmSoft.SME.Common.Resources.MTTAccSalePrice, isRequired: false, isNonNegative: true, logChanges: true, null, "SalePrice"));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Summa", () => Resources.SaleSum, isRequired: false, isNonNegative: true, logChanges: true, null, "SaleAmount"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("RowDescription", () => ArmSoft.SME.Common.Resources.Description, 50, isRequired: false, logChanges: true, null, "Description"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Mediator", () => ArmSoft.SME.Common.Resources.Mediator, 50, isRequired: false, logChanges: true, null, "Mediator"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Chief", () => ArmSoft.SME.Common.Resources.ChAccPost1, 100, isRequired: false, logChanges: true, null, "ChiefAccount"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Allower", () => ArmSoft.SME.Common.Resources.Allower, 50, isRequired: false, logChanges: true, null, "Allowed"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BookNum", () => ArmSoft.SME.Common.Resources.SupBookN, 10, isRequired: false, logChanges: true, null, "BookNumber"));
		tradeDocDefinition.AddFieldDefinition(new NumberFieldDefinition("BookPage", () => ArmSoft.SME.Common.Resources.PageN, 6, 0, isNonNegative: true, isRequired: false, null, logChanges: true, null, "BookPage"));
		tradeDocDefinition.AddFieldDefinition(new NumberFieldDefinition("BookLine", () => ArmSoft.SME.Common.Resources.RowN, 6, 0, isNonNegative: true, isRequired: false, null, logChanges: true, null, "BookLine"));
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("ExportedType", () => ArmSoft.SME.Common.Resources.TaxInvoiceExportForm, typeof(TaxInvoicesWriteTypesSE), isRequired: false, logChanges: true, ValueMember.Code, null, null, "TaxExportType"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("TaxInvoiceSeries", () => ArmSoft.SME.Common.Resources.Serial, 1, isRequired: false, logChanges: true, null, "TaxInvoiceSeries"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("TaxInvoiceNumber", () => ArmSoft.SME.Common.Resources.Number, 10, isRequired: false, logChanges: true, null, "TaxInvoiceNumber"));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("InvoiceDate", () => ArmSoft.SME.Common.Resources.SubmissionDate, isRequired: false, logChanges: true, null, "SubmissionDate"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("TransportationMethod", () => ArmSoft.SME.Common.Resources.TransportationMethod, 255, isRequired: false, logChanges: true, null, "TransportationMethod"));
		tradeDocDefinition.AddFieldDefinition(new BoolFieldDefinition("MotorVehicle", () => ArmSoft.SME.Common.Resources.MotorVehicle, isRequired: false, logChanges: true, null, "Vehicle"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Brand", () => Resources.Brand, 256, isRequired: false, logChanges: true, null, "Brand"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("LicensePlate", () => ArmSoft.SME.Common.Resources.LicensePlate, 20, isRequired: false, logChanges: true, null, "LicensePlate"));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTMoveOrder()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveOrder)
		{
			ChangingActualnessDate = false,
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 10080,
			HelpId = "Warehouse_Documents_GoodsTransferOrder",
			IsExchangeable = true,
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			BaseDocTypes = new HashSet<short>(),
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.WarehouseDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.TransportationDocumentsJournal.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("SupplyDate", () => ArmSoft.SME.Common.Resources.SupplyDate, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BaseISN", () => ArmSoft.SME.Common.Resources.BaseDoc, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("StorageExpense", () => Resources.OutStorage, 5, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("StorageIncome", () => Resources.InStorage, 5, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		tradeDocDefinition.AddFieldDefinition(new BoolFieldDefinition("IsMade", () => Resources.IsDone));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => Resources.MTLIST, isRequired: true);
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("RowID", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int)));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTName", () => ArmSoft.SME.Common.Resources.Name, 200));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("RowDescription", () => ArmSoft.SME.Common.Resources.Description, 50));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTMoveWithConfirmation()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 10212,
			HelpId = "Warehouse_Documents_MovementWithConfirmation",
			IsExchangeable = true,
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeImport.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveOrder.Value
			},
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.WarehouseDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.TransportationDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepTAccount.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsRetail.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTRevaluation.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTRevaluationRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true, logChanges: true, null, "Date"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true, logChanges: true, null, "DocumentNumber"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BaseISN", () => ArmSoft.SME.Common.Resources.BaseDoc, 50, isRequired: false, logChanges: true, null, "BaseDocumentISN"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("StorageExpense", () => Resources.OutStorage, 5, isRequired: true, logChanges: true, null, "StorageOutput"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("StorageIncome", () => Resources.InStorage, 5, isRequired: true, logChanges: true, null, "StorageInput"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PriceType", () => Resources.PriceType, 2, isRequired: false, logChanges: true, null, "PriceType"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255, isRequired: false, logChanges: true, null, "Comment"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50, isRequired: false, logChanges: true, null, "Owner"));
		tradeDocDefinition.AddFieldDefinition(new BoolFieldDefinition("Confirmed", () => Resources.EntryConfirmation, isRequired: false, logChanges: true, null, "Confirmed"));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => Resources.MTLIST, isRequired: true, logChanges: true, null, "ProductsList");
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("RowID", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int), logChanges: true, null, "RowId"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true, logChanges: true, null, "Code"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTName", () => ArmSoft.SME.Common.Resources.Name, 200, isRequired: false, logChanges: true, null, "Name"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true, logChanges: true, null, "Unit"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6, isRequired: false, logChanges: true, null, "UnitAbbreviation"));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true, isNonNegative: true, logChanges: true, null, "Quantity"));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("ActualQuantity", () => ArmSoft.SME.Common.Resources.ActualQTY, isRequired: false, isNonNegative: true, logChanges: true, null, "ActualQuantity"));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Price", () => ArmSoft.SME.Common.Resources.MTTAccSalePrice, isRequired: false, isNonNegative: true, logChanges: true, null, "SalePrice"));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Summa", () => Resources.SaleSum, isRequired: false, isNonNegative: true, logChanges: true, null, "SaleAmount"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("RowDescription", () => ArmSoft.SME.Common.Resources.Description, 50, isRequired: false, logChanges: true, null, "Description"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Mediator", () => ArmSoft.SME.Common.Resources.Mediator, 50, isRequired: false, logChanges: true, null, "Mediator"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Chief", () => ArmSoft.SME.Common.Resources.ChAccPost1, 100, isRequired: false, logChanges: true, null, "ChiefAccount"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Allower", () => ArmSoft.SME.Common.Resources.Allower, 50, isRequired: false, logChanges: true, null, "Allowed"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BookNum", () => ArmSoft.SME.Common.Resources.SupBookN, 10, isRequired: false, logChanges: true, null, "BookNumber"));
		tradeDocDefinition.AddFieldDefinition(new NumberFieldDefinition("BookPage", () => ArmSoft.SME.Common.Resources.PageN, 6, 0, isNonNegative: true, isRequired: false, null, logChanges: true, null, "BookPage"));
		tradeDocDefinition.AddFieldDefinition(new NumberFieldDefinition("BookLine", () => ArmSoft.SME.Common.Resources.RowN, 6, 0, isNonNegative: true, isRequired: false, null, logChanges: true, null, "BookLine"));
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("ExportedType", () => ArmSoft.SME.Common.Resources.TaxInvoiceExportForm, typeof(TaxInvoicesWriteTypesSE), isRequired: false, logChanges: true, ValueMember.Code, null, null, "TaxExportType"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("TaxInvoiceSeries", () => ArmSoft.SME.Common.Resources.Serial, 1, isRequired: false, logChanges: true, null, "TaxInvoiceSeries"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("TaxInvoiceNumber", () => ArmSoft.SME.Common.Resources.Number, 10, isRequired: false, logChanges: true, null, "TaxInvoiceNumber"));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("InvoiceDate", () => ArmSoft.SME.Common.Resources.SubmissionDate, isRequired: false, logChanges: true, null, "SubmissionDate"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("TransportationMethod", () => ArmSoft.SME.Common.Resources.TransportationMethod, 255, isRequired: false, logChanges: true, null, "TransportationMethod"));
		tradeDocDefinition.AddFieldDefinition(new BoolFieldDefinition("MotorVehicle", () => ArmSoft.SME.Common.Resources.MotorVehicle, isRequired: false, logChanges: true, null, "Vehicle"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Brand", () => Resources.Brand, 256, isRequired: false, logChanges: true, null, "Brand"));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("LicensePlate", () => ArmSoft.SME.Common.Resources.LicensePlate, 20, isRequired: false, logChanges: true, null, "LicensePlate"));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTOverheadExp()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTOverheadExp)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 126,
			HelpId = "Purchases_Documents_OverheadExpenses",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PurchaseDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepTAccount.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value
			},
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeImport.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankOut.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BaseISN", () => ArmSoft.SME.Common.Resources.BaseDoc, 50, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => Resources.PartnerContracts)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Supplier, isSupplier: true, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract),
			SummaPartFieldDefinition = new SumFieldDefinition("SummaPart", () => ArmSoft.SME.Common.Resources.Summa)
		});
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("InvoiceNumber", () => Resources.InvoiceNumber, 20));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRate, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummWithoutVAT", () => Resources.DistributionSumm, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.TotalSum));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummVAT", () => ArmSoft.SME.Common.Resources.SummaVAT));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("VATType", () => ArmSoft.SME.Common.Resources.VATCalculationType, 1, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new BoolFieldDefinition("AllowMTListEdit", () => ArmSoft.SME.Common.Resources.AllowListEdit));
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("DistrType", () => ArmSoft.SME.Common.Resources.DistributionType, typeof(DistrTypeSE), isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("DistrSumBy", () => ArmSoft.SME.Common.Resources.DistributionBase, typeof(DistrSumByTypeSE), isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("GridViewColsType", () => Resources.DisplayableCols, typeof(MTOverheadExpGridColsViewSE)));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => Resources.MTLIST, isRequired: true);
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTName", () => ArmSoft.SME.Common.Resources.Name, 200));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 200));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Unit, 6));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("DistributedSumma", () => Resources.DistributedSumma, isRequired: false, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("DistributedVATSumma", () => Resources.DistributedVATSumma, isRequired: false, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("DistributedCurrSumma", () => Resources.DistributedCurrSumma, isRequired: false, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("Coeff", () => ArmSoft.SME.Common.Resources.OverHeadExpCoeff, 7, 3));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("RowID", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int)));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTPurchaseOrder()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder)
		{
			ChangingActualnessDate = false,
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 608,
			HelpId = "Purchases_Documents_OrdertoSupplier",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PurchaseOrdersDocsJournal.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value
			},
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveOrder.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Supplier, isSupplier: true, isCustomer: false)
		});
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierEMail", () => ArmSoft.SME.Common.Resources.Email, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerPassport", () => ArmSoft.SME.Common.Resources.Passport, 32));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => Resources.MTLIST);
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTName", () => ArmSoft.SME.Common.Resources.Name, 200));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("RowID", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int)));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTReturnFromCustomer()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = true,
			SetDuplicateDocNumAutomatic = true,
			NextDocNumParamId = 135,
			HelpId = "Sales_Documents_Returnfrombuyer",
			AttachmentsTypes = (AttachType.File | AttachType.Link),
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankOut.Value
			},
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.SaleDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.TaxInvoicesJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ECRCheckDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepSalesAnalysis.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BonusOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashOpJournal.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BaseISN", () => ArmSoft.SME.Common.Resources.BaseDoc, 50));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => Resources.PartnerContracts)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Buyer, isSupplier: false, isCustomer: true),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract),
			SummaPartFieldDefinition = new SumFieldDefinition("SummaPart", () => ArmSoft.SME.Common.Resources.Summa)
		});
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BaseDocTaxInvoiceSeries", () => ArmSoft.SME.Common.Resources.BaseSerial, 1));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BaseDocTaxInvoiceNumber", () => ArmSoft.SME.Common.Resources.Number, 10));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("BaseDocTaxInvoiceDate", () => ArmSoft.SME.Common.Resources.BaseSubmissionDate));
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("ExportedType", () => ArmSoft.SME.Common.Resources.TaxInvoiceExportForm, typeof(TaxInvoicesWriteTypesSE)));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("TaxInvoiceSeries", () => ArmSoft.SME.Common.Resources.Serial, 1));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("TaxInvoiceNumber", () => ArmSoft.SME.Common.Resources.Number, 10));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("InvoiceDate", () => ArmSoft.SME.Common.Resources.SubmissionDate));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SalesConsultant", () => Resources.SalesConsultant, 12));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRate, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("VATType", () => ArmSoft.SME.Common.Resources.VATCalculationType, 1, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DealType", () => ArmSoft.SME.Common.Resources.DealType, 1));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PriceType", () => Resources.PriceType, 2));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummVAT", () => ArmSoft.SME.Common.Resources.SummaVAT));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.TotalSum));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => Resources.MTLIST, isRequired: true);
		gridDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("ItemType", () => ArmSoft.SME.Common.Resources.TypeWord, typeof(GoodsTypesSE), isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ItemCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ItemName", () => ArmSoft.SME.Common.Resources.Name, 200));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("Price", () => ArmSoft.SME.Common.Resources.Price));
		gridDefinition.AddFieldDefinition(new PercentFieldDefinition("Discount", () => ArmSoft.SME.Common.Resources.DiscountPercent, isRequired: false, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("DiscountPrice", () => ArmSoft.SME.Common.Resources.DiscountedPrice));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Summa", () => ArmSoft.SME.Common.Resources.Summa));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("UsedBonus", () => Resources.BonusUse));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("AccumulatedBonus", () => Resources.AccumulatedBonus));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("VAT", () => ArmSoft.SME.Common.Resources.OptionsVAT));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("DealType", () => ArmSoft.SME.Common.Resources.DealType, 1));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("RowDescription", () => ArmSoft.SME.Common.Resources.Description, 50));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("CostPrice", () => Resources.CostSummaNCC + " " + Resources.WithoutNDS));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("CostPriceVAT", () => Resources.CostSummaNCC + " " + Resources.WithNDS));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("CurrCostPrice", () => Resources.CostSummaCurr));
		gridDefinition.AddFieldDefinition(new DateFieldDefinition("UseLife", () => Resources.UseLife));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Country", () => ArmSoft.SME.Common.Resources.Country, 3));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Producer", () => ArmSoft.SME.Common.Resources.Producer, 4));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ProducerCaption", () => ArmSoft.SME.Common.Resources.Producer, 50));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property1", () => Resources.Attribute1, 20));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property1Caption", () => Resources.Attribute1, 50));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property2", () => Resources.Attribute2, 20));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property2Caption", () => Resources.Attribute2, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierName", () => ArmSoft.SME.Common.Resources.Name, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierLegalAddress", () => ArmSoft.SME.Common.Resources.LegalAddresses, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierBusinessAddress", () => ArmSoft.SME.Common.Resources.BusinessAddresses, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierSettlAcc", () => ArmSoft.SME.Common.Resources.SettlementAccount, 22));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierTaxCode", () => ArmSoft.SME.Common.Resources.OptionsTaxCode, 20));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierMngPost", () => ArmSoft.SME.Common.Resources.ManagerPost, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierMngName", () => ArmSoft.SME.Common.Resources.ManagerFIO, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierChiefAccPost", () => ArmSoft.SME.Common.Resources.AccountantPost, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierChiefAccName", () => ArmSoft.SME.Common.Resources.ChAccFIO, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerName", () => ArmSoft.SME.Common.Resources.Name, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerLegalAddress", () => ArmSoft.SME.Common.Resources.LegalAddresses, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerBusinessAddress", () => ArmSoft.SME.Common.Resources.BusinessAddresses, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerSettlAcc", () => ArmSoft.SME.Common.Resources.SettlementAccount, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerTaxCode", () => ArmSoft.SME.Common.Resources.OptionsTaxCode, 20));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerMngPost", () => ArmSoft.SME.Common.Resources.ManagerPost, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerMngName", () => ArmSoft.SME.Common.Resources.ManagerFIO, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerChiefAccPost", () => ArmSoft.SME.Common.Resources.AccountantPost, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerChiefAccName", () => ArmSoft.SME.Common.Resources.ChAccFIO, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerStateRegNum", () => Resources.PartStateRegister, 50));
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("BuyerIDDocType", () => Resources.IDDocType, typeof(IdentityTypeSE)));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerPassport", () => ArmSoft.SME.Common.Resources.PassportNum, 32));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerAdditionalData", () => ArmSoft.SME.Common.Resources.AdditionalData, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerShippingMethod", () => ArmSoft.SME.Common.Resources.ShippingMethod, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("ByWhomFio", () => ArmSoft.SME.Common.Resources.FIO, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("AuthorizationNumber", () => ArmSoft.SME.Common.Resources.CredentialNumber, 50));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("AuthorizationDate", () => ArmSoft.SME.Common.Resources.CredentialDate));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Trust", () => ArmSoft.SME.Common.Resources.Trust, 6));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Mediator", () => ArmSoft.SME.Common.Resources.Mediator, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Machine", () => ArmSoft.SME.Common.Resources.Machine, 9));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Chief", () => ArmSoft.SME.Common.Resources.ChAccPost1, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Allower", () => ArmSoft.SME.Common.Resources.Allower, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Response", () => ArmSoft.SME.Common.Resources.Respons, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CashDesk", () => ArmSoft.SME.Common.Resources.Cash, 8));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CashierShiftNumber", () => Resources.CashierShiftNum, 12));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("BankAcquiring", () => Resources.PartnerContracts)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Buyer, isSupplier: false, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract),
			SummaPartFieldDefinition = new SumFieldDefinition("SummaPart", () => ArmSoft.SME.Common.Resources.Summa)
		});
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("ECRCheckNum", () => ArmSoft.SME.Common.Resources.ECRCheckNum, 8));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("ECRReturnCheckNum", () => Resources.ECRReturnCheckNum, 8));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("ECRReturnByCashSum", () => Resources.CashPaymentForPrint));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("ECRReturnByCardSum", () => Resources.PlasticCartPaymentDialog));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("ECRReturnByPrePaymentSum", () => Resources.PrePayment));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("ECRBaseCheckNum", () => Resources.ECRBaseCheckNum, 8));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("BaseECRByCashSum", () => Resources.CashPaymentForPrint));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("BaseECRByCardSum", () => Resources.PlasticCartPaymentDialog));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("BaseECRByPrePaymentSum", () => Resources.PrePayment));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTReturnToSupplier()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = true,
			NextDocNumParamId = 127,
			HelpId = "Purchases_Documents_Returntovendor",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeImport.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value
			},
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PurchaseDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BaseDocInvoiceSeries", () => ArmSoft.SME.Common.Resources.BaseSerial, 1));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BaseDocInvoiceNumber", () => ArmSoft.SME.Common.Resources.Number, 10));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("BaseDocInvoiceDate", () => ArmSoft.SME.Common.Resources.BaseSubmissionDate));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("TaxInvoiceSeries", () => ArmSoft.SME.Common.Resources.Serial, 1));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("TaxInvoiceNumber", () => ArmSoft.SME.Common.Resources.Number, 10));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		tradeDocDefinition.AddFieldDefinition(new PercentFieldDefinition("Discount", () => ArmSoft.SME.Common.Resources.DiscountPercent));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRate, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PriceListType", () => Resources.PriceType, 2));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummVAT", () => ArmSoft.SME.Common.Resources.SummaVAT));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.Summa));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BookNum", () => ArmSoft.SME.Common.Resources.SupBookN, 10));
		tradeDocDefinition.AddFieldDefinition(new NumberFieldDefinition("BookPage", () => ArmSoft.SME.Common.Resources.PageN, 6, 0));
		tradeDocDefinition.AddFieldDefinition(new NumberFieldDefinition("BookLine", () => ArmSoft.SME.Common.Resources.RowN, 6, 0));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SalesConsultant", () => Resources.SalesConsultant, 12));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerName", () => ArmSoft.SME.Common.Resources.Name, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerLegalAddress", () => ArmSoft.SME.Common.Resources.LegalAddresses, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerBusinessAddress", () => ArmSoft.SME.Common.Resources.BusinessAddresses, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerSettlAcc", () => ArmSoft.SME.Common.Resources.SettlementAccount, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerTaxCode", () => ArmSoft.SME.Common.Resources.OptionsTaxCode, 20));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerMngPost", () => ArmSoft.SME.Common.Resources.ManagerPost, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerMngName", () => ArmSoft.SME.Common.Resources.ManagerFIO, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerChiefAccPost", () => ArmSoft.SME.Common.Resources.AccountantPost, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerChiefAccName", () => ArmSoft.SME.Common.Resources.ChAccFIO, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerStateRegNum", () => Resources.PartStateRegister, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BuyerPassport", () => ArmSoft.SME.Common.Resources.PassportNum, 32));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierName", () => ArmSoft.SME.Common.Resources.Name, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierLegalAddress", () => ArmSoft.SME.Common.Resources.LegalAddresses, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierBusinessAddress", () => ArmSoft.SME.Common.Resources.BusinessAddresses, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierSettlAcc", () => ArmSoft.SME.Common.Resources.SettlementAccount, 22));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierTaxCode", () => ArmSoft.SME.Common.Resources.OptionsTaxCode, 20));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierMngPost", () => ArmSoft.SME.Common.Resources.ManagerPost, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierMngName", () => ArmSoft.SME.Common.Resources.ManagerFIO, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierChiefAccPost", () => ArmSoft.SME.Common.Resources.AccountantPost, 100));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("SupplierChiefAccName", () => ArmSoft.SME.Common.Resources.ChAccFIO, 100));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => ArmSoft.SME.Common.Resources.Supplier)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Supplier, isSupplier: true, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract),
			SummaPartFieldDefinition = new SumFieldDefinition("SummaPart", () => ArmSoft.SME.Common.Resources.Summa)
		});
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("VATType", () => ArmSoft.SME.Common.Resources.VATCalculationType, 1, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => ArmSoft.SME.Common.Resources.List, isRequired: true);
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTName", () => ArmSoft.SME.Common.Resources.Name, 200));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		gridDefinition.AddFieldDefinition(new PercentFieldDefinition("Discount", () => ArmSoft.SME.Common.Resources.DiscountPercent, isRequired: false, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("DiscountPrice", () => ArmSoft.SME.Common.Resources.DiscountedPrice));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("Price", () => ArmSoft.SME.Common.Resources.Price));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Summa", () => ArmSoft.SME.Common.Resources.Summa));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("SalePrice", () => ArmSoft.SME.Common.Resources.MTTAccSalePrice));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("SaleSumma", () => Resources.SaleSum));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("VAT", () => ArmSoft.SME.Common.Resources.OptionsVAT));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("RowID", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int)));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTRevaluation()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTRevaluation)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			IsDocNumRequired = false,
			NextDocNumParamId = 10029,
			HelpId = "Warehouse_Documents_Revaluation",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			BaseDocTypes = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove },
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.WarehouseDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepTAccount.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BaseISN", () => ArmSoft.SME.Common.Resources.BaseDoc, 50));
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("PriceFormationOpType", () => Resources.NewPriceFormationMethod, typeof(MTRvlNewPriceFormationTypesSE), isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("GridViewColsType", () => Resources.DisplayableCols, typeof(MTRvlGridColsViewMethodsSE)));
		tradeDocDefinition.AddFieldDefinition(new NumberFieldDefinition("Coefficient", () => ArmSoft.SME.Common.Resources.Coefficient, 8, 4));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("DistrSum", () => ArmSoft.SME.Common.Resources.Summa));
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("DistrSumBy", () => ArmSoft.SME.Common.Resources.DistributionBase, typeof(MTRvlDistrSumByTypesSE)));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PriceType", () => Resources.PriceType, 2));
		tradeDocDefinition.AddFieldDefinition(new NumberFieldDefinition("Price", () => ArmSoft.SME.Common.Resources.Price, 16, 2));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => Resources.MTLIST, isRequired: true);
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTName", () => ArmSoft.SME.Common.Resources.Name, 200));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("Cost", () => Resources.MatRepCurrentCost + Resources.WithoutNDS));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("CostWithVAT", () => Resources.MatRepCurrentCost + Resources.WithNDS));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("CurCostWithVAT", () => Resources.CurrPrice));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("NewCost", () => ArmSoft.SME.Common.Resources.NewPrice + Resources.WithoutNDS, isRequired: true));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("NewCostWithVAT", () => ArmSoft.SME.Common.Resources.NewPrice + Resources.WithNDS));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("NewCurCostWithVAT", () => ArmSoft.SME.Common.Resources.NewPrice + Resources.WithCurr));
		gridDefinition.AddFieldDefinition(new DateFieldDefinition("IncomeDate", () => ArmSoft.SME.Common.Resources.IncomeDate));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("IncomeDocDescr", () => ArmSoft.SME.Common.Resources.IncomeDoc, 50));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("PartnerName", () => ArmSoft.SME.Common.Resources.Partner, 50));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ContractName", () => ArmSoft.SME.Common.Resources.Contract, 50));
		gridDefinition.AddFieldDefinition(new DateFieldDefinition("UseLife", () => Resources.UseLife));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("CountryName", () => ArmSoft.SME.Common.Resources.Country, 50));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ProducerName", () => ArmSoft.SME.Common.Resources.Producer, 50));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property1Value", () => Resources.Attribute1, 50));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Property2Value", () => Resources.Attribute2, 50));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("RowID", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int)));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTRevaluationRetail()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTRevaluationRetail)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 173,
			HelpId = "Warehouse_Documents_RevaluationRetail",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value
			},
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.WarehouseDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepOperationsRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MatRepTAccount.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("BaseISN", () => ArmSoft.SME.Common.Resources.BaseDoc, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PriceType", () => Resources.PriceType, 2));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => Resources.MTLIST, isRequired: true);
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTName", () => ArmSoft.SME.Common.Resources.Name, 200));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Unit, 6));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("Cost", () => Resources.OldCost));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("NewCost", () => ArmSoft.SME.Common.Resources.NewPrice, isRequired: true));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("RowID", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int)));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTStockTaking()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTaking)
		{
			ChangingActualnessDate = false,
			AllowStoreAsDraft = true,
			NextDocNumParamId = 128,
			HelpId = "Warehouse_Documents_Inventory",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.WarehouseDocumentsJournal.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTExpense.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("GoodGroup", () => Resources.MTGroup, 5));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRateShort, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PriceType", () => ArmSoft.SME.Common.Resources.PriceListType, 2));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => ArmSoft.SME.Common.Resources.ItemList, isRequired: true);
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTName", () => ArmSoft.SME.Common.Resources.Name, 200));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("AccQuantity", () => ArmSoft.SME.Common.Resources.AccQTY, isRequired: false, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("ActualQuantity", () => ArmSoft.SME.Common.Resources.ActualQTY));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("VarianceQuantity", () => ArmSoft.SME.Common.Resources.VarianceQty, isRequired: false, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("AccSum", () => ArmSoft.SME.Common.Resources.AccSumm, isRequired: false, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("ActualSum", () => ArmSoft.SME.Common.Resources.ActualSumm));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("Price", () => ArmSoft.SME.Common.Resources.Price));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("SalePrice", () => ArmSoft.SME.Common.Resources.MTTAccSalePrice));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("SaleSum", () => Resources.SaleSum, isRequired: false, isNonNegative: false));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateMTStockTakingRetail()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTakingRetail)
		{
			ChangingActualnessDate = false,
			AllowStoreAsDraft = true,
			IsDocNumberUnique = true,
			NextDocNumParamId = 172,
			HelpId = "Warehouse_Documents_InventoryRetail",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.WarehouseDocumentsJournal.Value
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTExpense.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("GoodGroup", () => Resources.MTGroup, 5));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRateShort, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PriceType", () => ArmSoft.SME.Common.Resources.PriceListType, 2));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		GridDefinition gridDefinition = new GridDefinition("MTList", () => ArmSoft.SME.Common.Resources.ItemList, isRequired: true);
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("MTName", () => ArmSoft.SME.Common.Resources.Name, 200));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("AccQuantity", () => ArmSoft.SME.Common.Resources.AccQTY, isRequired: false, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("ActualQuantity", () => ArmSoft.SME.Common.Resources.ActualQTY));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("VarianceQuantity", () => ArmSoft.SME.Common.Resources.VarianceQty, isRequired: false, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("AccSum", () => ArmSoft.SME.Common.Resources.AccSumm, isRequired: false, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("ActualSum", () => ArmSoft.SME.Common.Resources.ActualSumm));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("Price", () => ArmSoft.SME.Common.Resources.MTTAccSalePrice));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition PartnerGroupCorrect()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PartnerRemGroupCorrect)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 343,
			HelpId = "Purchases_Documents_SupportingDocuments_PartnerDebtRevisionGroup",
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRateShort, isRequired: true));
		GridDefinition gridDefinition = new GridDefinition("PartRemList", () => Resources.PartnerSepContractList, isRequired: true);
		PartnerBonusGroupCorrectBase.AddDefToPartBonusGroupCorrectGrid(gridDefinition);
		gridDefinition.AddFieldDefinition(new ContractFieldDefinition("Contract", () => ArmSoft.SME.Common.Resources.Contract, isBonusCard: false, isRequired: true));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Summa", () => ArmSoft.SME.Common.Resources.Summa + " (+/-)", isRequired: true, isNonNegative: false));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("SummaNCC", () => Resources.SumNCC + " (+/-)", isRequired: true, isNonNegative: false));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		return tradeDocDefinition;
	}

	public static DocDefinition PartnerRemCorrect()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PartnerRemCorrect)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 123,
			HelpId = "Purchases_Documents_SupportingDocuments_Partnerdebtrevision",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRateShort, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Partner, isSupplier: false, isCustomer: false, isRequired: true),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract, isBonusCard: false, isRequired: true)
		});
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.Summa, isRequired: true, isNonNegative: false));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummNCC", () => Resources.SumNCC, isRequired: true, isNonNegative: false));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		return tradeDocDefinition;
	}

	public static DocDefinition PartnerRemMove()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PartnerRemMove)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 341,
			HelpId = "Purchases_Documents_SupportingDocuments_PartnerDebtMove",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRateShort, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Partner, isSupplier: false, isCustomer: false, isRequired: true),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract)
		});
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("CorrPartnerContractGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Partner, isSupplier: false, isCustomer: false, isRequired: true),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract)
		});
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.Summa, isRequired: true, isNonNegative: false));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummNCC", () => Resources.SumNCC, isRequired: true, isNonNegative: false));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CorrCurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		return tradeDocDefinition;
	}

	public static DocDefinition CreatePartnersGroupRevaluation()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PartnersGroupRevaluation)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 202,
			HelpId = "Purchases_Documents_SupportingDocuments_parntersGroupRevaluation",
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		GridDefinition gridDefinition = new GridDefinition("RevaluationList", () => Resources.PartnerSepContractList, isRequired: true);
		gridDefinition.AddFieldDefinition(new PartnerFieldDefinition("PartCode", () => Resources.PartnerCode, isSupplier: false, isCustomer: false, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("PartName", () => ArmSoft.SME.Common.Resources.Name, 50));
		gridDefinition.AddFieldDefinition(new ContractFieldDefinition("Contract", () => ArmSoft.SME.Common.Resources.Contract, isBonusCard: false, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Summa", () => ArmSoft.SME.Common.Resources.Summa, isRequired: false, isNonNegative: false));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition PayOrder()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd)
		{
			AllowStoreAsDraft = true,
			NextDocNumParamId = 66,
			IsDocNumberUnique = true,
			HelpId = "Purchases_Documents_SupportingDocuments_PaymentOrder",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			BaseDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.ReceivedService,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTOverheadExp,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer
			},
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PaymentOrdersJournal,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BankOpJournal,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BankTAccount,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard
			},
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankIn,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankOut
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRateShort, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Partner, isSupplier: false, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract),
			SummaPartFieldDefinition = new SumFieldDefinition("SummaPart", () => ArmSoft.SME.Common.Resources.Summa)
		});
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PayerName", () => ArmSoft.SME.Common.Resources.PayerBut, 255, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PayerSettlAcc", () => Resources.AccountN, 22, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PayerTaxCode", () => ArmSoft.SME.Common.Resources.PayerTaxCode, 20));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("ReceiverName", () => ArmSoft.SME.Common.Resources.ReceiverBut, 50, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("ReceiverSettlAcc", () => Resources.AccountN, 50, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("ReceiverTaxCode", () => ArmSoft.SME.Common.Resources.OptionsTaxCode, 20));
		tradeDocDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("ReceiverLegalStatus", () => Resources.LegalStatus, typeof(ReceiverLegalStatusTypeSE)));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PaymentAim", () => ArmSoft.SME.Common.Resources.PaymentAim, 255, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.Summa + " " + Resources.WithCurr, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummNCC", () => ArmSoft.SME.Common.Resources.Summa + " " + Resources.NCOnly, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("PartnerRem", () => Resources.PartnerRem, isRequired: false, isNonNegative: false));
		return tradeDocDefinition;
	}

	public static DocDefinition ReceivedService()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.ReceivedService)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 346,
			HelpId = "Purchases_Documents_SupportingDocuments_ReceivedServices",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			IsExchangeable = true,
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.BankOut.Value
			},
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ReceivedServicesJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PurchaseDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartRepCard.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Supplier, isSupplier: true, isCustomer: false),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract),
			SummaPartFieldDefinition = new SumFieldDefinition("SummaPart", () => ArmSoft.SME.Common.Resources.Summa)
		});
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("InvoiceNumber", () => Resources.InvoiceNumber, 20));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new ExchangeRateFieldDefinition("ExchangeRate", () => ArmSoft.SME.Common.Resources.ExchangeRate, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("VATType", () => ArmSoft.SME.Common.Resources.VATCalculationType, 1, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("PriceType", () => Resources.PriceType, 2));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("SummVAT", () => ArmSoft.SME.Common.Resources.SummaVAT));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("Summ", () => ArmSoft.SME.Common.Resources.TotalSum));
		GridDefinition gridDefinition = new GridDefinition("ReceivedServiceList", () => ArmSoft.SME.Common.Resources.Services, isRequired: true);
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ServCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ServName", () => ArmSoft.SME.Common.Resources.Name, 200, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, isRequired: true));
		gridDefinition.AddFieldDefinition(new PriceFieldDefinition("Price", () => ArmSoft.SME.Common.Resources.Price, isRequired: true));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Summa", () => ArmSoft.SME.Common.Resources.Summa, isRequired: true));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("VAT", () => ArmSoft.SME.Common.Resources.OptionsVAT, isRequired: true));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition CreateReceivedTaxInvoice()
	{
		return DefinitionCreation.CreateDocDefinition(new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.ReceivedTaxInvoice)
		{
			ChangingActualnessDate = false,
			AllowStoreAsDraft = false,
			IsDocNumberUnique = false,
			HelpId = "",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			ChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.ReceivedService.Value
			},
			WeakChildDocTypes = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.ReceivedService.Value
			},
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ReceivedInvoicesJournal.Value
			}
		});
	}

	public static DocDefinition CreateReconcilationStatement()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.Reconciliation)
		{
			ChangingActualnessDate = false,
			AllowStoreAsDraft = false,
			IsDocNumberUnique = false,
			NextDocNumParamId = 10007,
			HelpId = "Purchases_Documents_SupportingDocuments_ReconciliationStatement",
			IsExchangeable = true,
			AttachmentsTypes = (AttachType.File | AttachType.Link),
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ReconciliationStatementsJournal.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new PartnerContractGroupDefinition("PartnerContractGroup", () => ArmSoft.SME.Common.Resources.Partner)
		{
			PartnerFieldDefinition = new PartnerFieldDefinition("PartnerCode", () => ArmSoft.SME.Common.Resources.Partner, isSupplier: false, isCustomer: false, isRequired: true),
			ContractFieldDefinition = new ContractFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract)
		});
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("PeriodStart", () => ArmSoft.SME.Common.Resources.DatePeriod, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("PeriodEnd", () => "", isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		GridDefinition gridDefinition = new GridDefinition("OurCalcsList", () => ArmSoft.SME.Common.Resources.OurCalculations);
		gridDefinition.AddFieldDefinition(new DateFieldDefinition("Date", () => ArmSoft.SME.Common.Resources.DateShort));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ContractCode", () => ArmSoft.SME.Common.Resources.Contract, 3));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("BaseDocDescr", () => ArmSoft.SME.Common.Resources.BaseDoc, 255));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("TransactionComment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("SummIn", () => Resources.DebtIncrease));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("SummOut", () => Resources.DebtDecrease));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("OurStartRem", () => ArmSoft.SME.Common.Resources.InitRem, isRequired: false, isNonNegative: false));
		tradeDocDefinition.AddFieldDefinition(new SumFieldDefinition("OurEndRem", () => ArmSoft.SME.Common.Resources.EndRem, isRequired: false, isNonNegative: false));
		tradeDocDefinition.AddGridDefinition(gridDefinition);
		return tradeDocDefinition;
	}

	public static DocDefinition Revaluation()
	{
		TradeDocDefinition tradeDocDefinition = new TradeDocDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.Revaluation)
		{
			AllowStoreAsDraft = true,
			IsDocNumberUnique = false,
			NextDocNumParamId = 129,
			HelpId = "Purchases_Documents_SupportingDocuments_revoluation",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BankOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartOpJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BankDocumentsJournal.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashOpJournal.Value
			}
		};
		tradeDocDefinition.AddFieldDefinition(new DateFieldDefinition("DocDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("DocNum", () => ArmSoft.SME.Common.Resources.DocNumber, 12, isRequired: true));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => Resources.DocCurr, 3));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		tradeDocDefinition.AddFieldDefinition(new StringFieldDefinition("Owner", () => ArmSoft.SME.Common.Resources.DocOwner, 50));
		tradeDocDefinition.AddFieldDefinition(new BoolFieldDefinition("IsRevalCashes", () => Resources.RevalCashes));
		tradeDocDefinition.AddFieldDefinition(new BoolFieldDefinition("IsRevalBankAccs", () => Resources.RevalBankAccs));
		tradeDocDefinition.AddFieldDefinition(new BoolFieldDefinition("IsRevalPartners", () => Resources.RevalPartners));
		return tradeDocDefinition;
	}
}
