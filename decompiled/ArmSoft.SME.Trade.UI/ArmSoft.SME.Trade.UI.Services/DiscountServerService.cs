using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.Discount;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Models.Other;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DataViewDataProvider;

namespace ArmSoft.SME.Trade.UI.Services;

public class DiscountServerService
{
	public static void CreateCardInfo(PartnerCacheRow partnerCacheRow, PartnerCacheRow.ContractRow contractRow)
	{
		using CloudClient cloudClient = Settings.DiscountServerClient;
		CardCreateRequest request = new CardCreateRequest
		{
			CardNumber = contractRow.DiscountCard.Trim(),
			BarCode = MCRInfo.GetInstance().Prefix + contractRow.DiscountCard.Trim() + MCRInfo.GetInstance().Suffix,
			BonusPercent = contractRow.Bonus,
			CardHolder = partnerCacheRow.Caption.Trim(),
			ValidPeriodStart = contractRow.BonusCardValidPeriodStart.ToDateTime(),
			ValidPeriodEnd = contractRow.BonusCardValidPeriodEnd.ToDateTime(),
			Canceled = contractRow.BonusCardCanceled,
			Gender = partnerCacheRow.Sex,
			PhoneNumber = partnerCacheRow.Phone.Trim(),
			Email = partnerCacheRow.Email.Trim(),
			BirthDate = (partnerCacheRow.Birthday.HasValue ? partnerCacheRow.Birthday.ToDateTime() : ((DateTime?)null))
		};
		cloudClient.Discount.CreateCardInfo(request, updateIfExists: true);
	}

	public static void CreateCardInfo(ContractsDataProvider.DataRow contractRow)
	{
		using CloudClient cloudClient = Settings.DiscountServerClient;
		CardCreateRequest cardCreateRequest = new CardCreateRequest
		{
			CardNumber = contractRow.DiscountCard.Trim(),
			BarCode = MCRInfo.GetInstance().Prefix + contractRow.DiscountCard.Trim() + MCRInfo.GetInstance().Suffix,
			BonusPercent = contractRow.Bonus,
			CardHolder = contractRow.PartnerName.Trim(),
			ValidPeriodStart = contractRow.BonusCardValidPeriodStart.ToDateTime(),
			ValidPeriodEnd = contractRow.BonusCardValidPeriodEnd.ToDateTime(),
			Canceled = contractRow.BonusCardCanceled
		};
		PartnerCacheRow itemById = TradeSettings.TradeCacheService.Partner.GetItemById(contractRow.PartnerId);
		if (itemById != null)
		{
			cardCreateRequest.Gender = itemById.Sex;
			cardCreateRequest.PhoneNumber = itemById.Phone.Trim();
			cardCreateRequest.Email = itemById.Email.Trim();
			cardCreateRequest.BirthDate = (itemById.Birthday.HasValue ? itemById.Birthday.ToDateTime() : ((DateTime?)null));
		}
		cloudClient.Discount.CreateCardInfo(cardCreateRequest, updateIfExists: true);
	}

	public static void UpdateCardInfo(CardUpdateRequest request)
	{
		if (request == null)
		{
			using (CloudClient cloudClient = Settings.DiscountServerClient)
			{
				cloudClient.Discount.UpdateCardInfo(request);
			}
		}
	}

	public static List<CardTransaction> RecordDiscountServerTransactions(List<CardTransactionRequest> request)
	{
		try
		{
			List<CardTransaction> result = new List<CardTransaction>();
			using (CloudClient cloudClient = Settings.DiscountServerClient)
			{
				result = cloudClient.Discount.CreateTransactions(request, new TimeSpan(0, 0, 0, 60, 0));
			}
			return result;
		}
		catch (OperationCanceledException ex)
		{
			Settings.ErrorHandler(ex);
			return null;
		}
	}

	public static CardTransaction RecordDiscountServerTransaction(CardTransactionRequest request)
	{
		try
		{
			CardTransaction result = new CardTransaction();
			using (CloudClient cloudClient = Settings.DiscountServerClient)
			{
				result = cloudClient.Discount.CreateTransaction(request, new TimeSpan(0, 0, 0, 60, 0));
			}
			return result;
		}
		catch (OperationCanceledException ex)
		{
			Settings.ErrorHandler(ex);
			return null;
		}
	}

	public static decimal GetCardBalance(string cardNumber)
	{
		if (string.IsNullOrEmpty(cardNumber))
		{
			return 0m;
		}
		using CloudClient cloudClient = Settings.DiscountServerClient;
		return cloudClient.Discount.GetCardInfo(cardNumber)?.Balance ?? 0m;
	}

	public static List<CardInfo> GetCardList(string cardNumber = "")
	{
		using CloudClient cloudClient = Settings.DiscountServerClient;
		if (string.IsNullOrWhiteSpace(cardNumber))
		{
			return cloudClient.Discount.GetCards();
		}
		CardInfo cardInfo = cloudClient.Discount.GetCardInfo(cardNumber);
		if (cardInfo != null)
		{
			int num = 1;
			List<CardInfo> list = new List<CardInfo>(num);
			CollectionsMarshal.SetCount(list, num);
			CollectionsMarshal.AsSpan(list)[0] = cardInfo;
			return list;
		}
		return new List<CardInfo>();
	}

	public static CardInfo GetCardInfo(string cardNumber)
	{
		if (string.IsNullOrEmpty(cardNumber))
		{
			return null;
		}
		using CloudClient cloudClient = Settings.DiscountServerClient;
		return cloudClient.Discount.GetCardInfo(cardNumber);
	}

	public static List<CardTransaction> GetTransactionsList(string cardNumber, DateOnly? startDate, DateOnly? endDate, bool fillDocInfo, bool showDeleted)
	{
		using CloudClient cloudClient = Settings.DiscountServerClient;
		DateTime? periodEnd = (endDate.HasValue ? new DateTime?(endDate.Value.ToDateTime().AddDays(1.0).AddMilliseconds(-1.0)) : ((DateTime?)null));
		return cloudClient.Discount.GetTransactions(cardNumber, startDate.ToDateTime(), periodEnd, string.Empty, string.Empty, fillDocInfo, showDeleted);
	}

	public static List<BonusTransactionsDataProvider.DataRow> TransformToLocalType(List<CardTransaction> discountServerTransactionList)
	{
		List<BonusTransactionsDataProvider.DataRow> list = new List<BonusTransactionsDataProvider.DataRow>();
		if (discountServerTransactionList != null && discountServerTransactionList.Count > 0)
		{
			foreach (CardTransaction discountServerTransaction in discountServerTransactionList)
			{
				if (discountServerTransaction.Rows != null)
				{
					foreach (CardTransactionRow row in discountServerTransaction.Rows)
					{
						list.Add(new BonusTransactionsDataProvider.DataRow
						{
							IsDeleted = discountServerTransaction.Deleted,
							CardNumber = discountServerTransaction.CardNumber,
							TransactionDate = discountServerTransaction.Date,
							DocDate = discountServerTransaction.DocInfo.Date.ToDateOnly(),
							DocSum = discountServerTransaction.DocInfo.Amount,
							DocTypeCode = DecodeBonusDocType((byte)discountServerTransaction.DocInfo.Type).Value,
							Sum = row.Amount,
							DocNumber = discountServerTransaction.DocInfo.Number,
							OpTypeCode = (byte)row.Op,
							OpType = DecodeBonusOpType((byte)row.Op),
							TransactionISN = discountServerTransaction.ISN,
							User = discountServerTransaction.DocInfo.User,
							FingerPrint = discountServerTransaction.DocInfo.FingerPrint,
							CashCode = discountServerTransaction.DocInfo.CashCode,
							ShiftNumber = discountServerTransaction.DocInfo.ShiftNumber
						});
					}
				}
				else
				{
					list.Add(new BonusTransactionsDataProvider.DataRow
					{
						IsDeleted = discountServerTransaction.Deleted,
						CardNumber = discountServerTransaction.CardNumber,
						TransactionDate = discountServerTransaction.Date,
						DocDate = discountServerTransaction.DocInfo.Date.ToDateOnly(),
						DocSum = discountServerTransaction.DocInfo.Amount,
						DocTypeCode = DecodeBonusDocType((byte)discountServerTransaction.DocInfo.Type).Value,
						Sum = 0m,
						DocNumber = discountServerTransaction.DocInfo.Number,
						TransactionISN = discountServerTransaction.ISN,
						User = discountServerTransaction.DocInfo.User,
						FingerPrint = discountServerTransaction.DocInfo.FingerPrint
					});
				}
			}
		}
		return list;
	}

	public static List<BonusTransactionRow> GetTransactionListForComparison(string cardNumber, DateOnly? startDate, DateOnly? endDate, string cashDesk, string shiftNumber)
	{
		List<CardTransaction> list = new List<CardTransaction>();
		DateTime? periodEnd = (endDate.HasValue ? new DateTime?(endDate.Value.ToDateTime().AddDays(1.0).AddMilliseconds(-1.0)) : ((DateTime?)null));
		using (CloudClient cloudClient = Settings.DiscountServerClient)
		{
			list = cloudClient.Discount.GetTransactions(cardNumber, startDate.ToDateTime(), periodEnd, cashDesk, shiftNumber, fillDocInfo: true, showDeleted: false);
		}
		List<BonusTransactionRow> list2 = new List<BonusTransactionRow>();
		if (list != null && list.Count > 0)
		{
			foreach (CardTransaction item in list)
			{
				if (item.Rows == null)
				{
					continue;
				}
				foreach (CardTransactionRow row in item.Rows)
				{
					if (row.Amount >= 0m)
					{
						list2.Add(new BonusTransactionRow
						{
							CardNumber = item.CardNumber,
							TransactionDate = item.Date,
							Sum = row.Amount,
							OpTypeCode = (byte)row.Op,
							DBCR = true
						});
					}
					else
					{
						list2.Add(new BonusTransactionRow
						{
							CardNumber = item.CardNumber,
							TransactionDate = item.Date,
							Sum = row.Amount,
							OpTypeCode = (byte)row.Op,
							DBCR = false
						});
					}
				}
			}
		}
		return list2;
	}

	public static string DecodeBonusOpType(byte bonusType)
	{
		return bonusType switch
		{
			1 => Resources.BonusOperationAccumulate, 
			3 => Resources.BonusOperationCorrection, 
			2 => Resources.BonusOperationUse, 
			_ => string.Empty, 
		};
	}

	public static DocumentTypeSE DecodeBonusDocType(byte bonusDocType)
	{
		return bonusDocType switch
		{
			1 => DocumentTypeSE.MTBill, 
			2 => DocumentTypeSE.MTReturnFromCustomer, 
			3 => DocumentTypeSE.BonusGroupCorrect, 
			_ => null, 
		};
	}

	public static void DeleteTransactionsByISN(Guid transactionISN)
	{
		using CloudClient cloudClient = Settings.DiscountServerClient;
		cloudClient.Discount.DeleteTransaction(transactionISN);
	}

	public static void DeleteTransactionsByDocISN(Guid docISN)
	{
		using CloudClient cloudClient = Settings.DiscountServerClient;
		cloudClient.Discount.DeleteTransactions(docISN);
	}

	public static List<BonusCardsDataProvider.DataRow> GetBonusCardsList(string cardNumber = "")
	{
		List<BonusCardsDataProvider.DataRow> list = new List<BonusCardsDataProvider.DataRow>();
		List<CardInfo> cardList = GetCardList(cardNumber);
		if (cardList != null && cardList.Count > 0)
		{
			foreach (CardInfo item in cardList)
			{
				list.Add(GetBonusCardRow(item));
			}
		}
		return list;
	}

	public static BonusCardsDataProvider.DataRow GetBonusCardRow(CardInfo cardInfo)
	{
		return new BonusCardsDataProvider.DataRow
		{
			CardNumber = cardInfo.CardNumber,
			Barcode = cardInfo.BarCode,
			CardHolder = cardInfo.CardHolder,
			Gender = cardInfo.Gender,
			BirthDate = cardInfo.BirthDate.ToDateOnly(),
			PhoneNumber = cardInfo.PhoneNumber,
			Email = cardInfo.Email,
			BonusPercent = cardInfo.BonusPercent,
			Balance = cardInfo.Balance,
			PurchaseAmount = cardInfo.PurchaseAmount,
			ValidPeriodStart = cardInfo.ValidPeriodStart.ToDateOnly(),
			ValidPeriodEnd = cardInfo.ValidPeriodEnd.ToDateOnly(),
			Canceled = cardInfo.Canceled,
			SyncDate = cardInfo.SyncDate
		};
	}

	public static void ImportCardInfo(CardInfo cardInfo, ref List<string> updatedCards)
	{
		PartnerCacheRow.ContractRow contractRow = TradeSettings.TradeCacheService.Partner.GetContractByDiscountCard(cardInfo.CardNumber, out var partnerCacheRow);
		if (contractRow != null)
		{
			Partner partner = (Partner)Settings.DirectoryService.Load(DirectoryTypeSE.Partner, partnerCacheRow.Id);
			partner.Name = cardInfo.CardHolder;
			partner.FullCaption = cardInfo.CardHolder;
			partner.Sex = cardInfo.Gender;
			partner.Phone = cardInfo.PhoneNumber;
			partner.Email = cardInfo.Email;
			partner.Birthday = cardInfo.BirthDate.ToDateOnly();
			Partner.PartnerContractRow partnerContractRow = partner.PartnerContracts.Find((Partner.PartnerContractRow row) => row.Contract == contractRow.Contract);
			if (partnerContractRow != null)
			{
				partnerContractRow.Bonus = cardInfo.BonusPercent;
				partnerContractRow.BonusCardValidPeriodStart = cardInfo.ValidPeriodStart.ToDateOnly();
				partnerContractRow.BonusCardValidPeriodEnd = cardInfo.ValidPeriodEnd.ToDateOnly();
				partnerContractRow.BonusCardCanceled = cardInfo.Canceled;
			}
			Settings.DirectoryService.Store(partner, new DirStoreRequest
			{
				LogComment = Resources.ImportFromDiscountServer
			});
			if (updatedCards != null)
			{
				updatedCards.Add(cardInfo.CardNumber);
			}
		}
	}
}
