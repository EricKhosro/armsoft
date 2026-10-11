using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.CustomTypes;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Helpers;

public class DiscountsCalculator
{
	public delegate void ShowMessageOnDiscountEventHandler(string msg);

	public delegate void AddGiftGoodsToDocEventHandler(List<DiscountCacheRow> docDiscounts, List<MTComplectsPartsDataProvider.DataRow> docGiftSets);

	public delegate void SelectAndAddGiftGoodsToDocEventHandler(List<DiscountCacheRow> docDiscounts, Dictionary<string, List<MTComplectsPartsDataProvider.DataRow>> docGiftSets);

	private readonly DocumentTypeSE _documentType;

	private readonly string _paramDiscountUnionMethod = Settings.ParametersService.DiscountsMergeMethod();

	private Dictionary<int, List<DiscountCacheRow>> _rowsMatchingDiscounts;

	private Dictionary<int, List<DiscountCacheRow>> _rowsMatchingGiftDiscounts;

	private List<DiscountCacheRow> _matchingDiscountsForDoc;

	private List<DiscountCacheRow> _matchingGiftDiscountsForDoc;

	private Dictionary<string, decimal> _partnerAccumulatedAmountsByDiscount;

	private List<MTComplectsPartsDataProvider.DataRow> _matchingGiftIDsForDoc;

	private Dictionary<string, List<MTComplectsPartsDataProvider.DataRow>> _matchingGiftRowsWithSelectForDoc;

	private List<DiscountCacheRow> _matchingNewGiftDiscountsForDoc;

	private const decimal MaximumAllowedDiscountPercent = 100m;

	private int docPartID = -1;

	private List<GoodsBySegmentDataProvider.DataRow> goodsBySegmentRows;

	public List<DiscountLogRow> Log { get; set; }

	public DocumentInfo DocInfo { get; set; }

	public event ShowMessageOnDiscountEventHandler ShowMessageOnDiscount;

	public event AddGiftGoodsToDocEventHandler AddGiftGoodsToDoc;

	public event SelectAndAddGiftGoodsToDocEventHandler SelectAndAddGiftGoodsToDoc;

	public DiscountsCalculator(DocumentTypeSE documentType)
	{
		_documentType = documentType;
	}

	public void InitializeAndCalculate()
	{
		ValidateAndResetInnerState();
		FindMatchingDiscounts();
		Calculate();
		RaiseMessageEvents();
		AddGiftGoodsRows();
		SelectAndAddGiftGoodsRows();
	}

	public void ValidateAndResetInnerState()
	{
		if (DocInfo == null)
		{
			throw new InvalidOperationException("DocInfo property is not set");
		}
		goodsBySegmentRows = new GoodsBySegmentDataProvider(TradeSettings.CreateTradeClient()).GetRows(new GoodsBySegmentDataProvider.Param(), withoutAccessCheck: true);
		Log = new List<DiscountLogRow>();
		_matchingDiscountsForDoc = new List<DiscountCacheRow>();
		_matchingGiftDiscountsForDoc = new List<DiscountCacheRow>();
		_matchingNewGiftDiscountsForDoc = new List<DiscountCacheRow>();
		_matchingGiftIDsForDoc = new List<MTComplectsPartsDataProvider.DataRow>();
		_matchingGiftRowsWithSelectForDoc = new Dictionary<string, List<MTComplectsPartsDataProvider.DataRow>>();
		_rowsMatchingDiscounts = new Dictionary<int, List<DiscountCacheRow>>();
		_rowsMatchingGiftDiscounts = new Dictionary<int, List<DiscountCacheRow>>();
		_partnerAccumulatedAmountsByDiscount = new Dictionary<string, decimal>();
	}

	private void FindMatchingDiscounts()
	{
		if (!string.IsNullOrWhiteSpace(DocInfo.PartnerCode))
		{
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(DocInfo.PartnerCode, DocInfo.PartnerContract);
			if (contract != null)
			{
				DocInfo.PartnerContractPercent = contract.Percent;
			}
		}
		FindMatchingDiscountsForDocument();
		foreach (ISaleRow row in DocInfo.Rows)
		{
			if (row.IsDeleted || row.ItemID == -1)
			{
				continue;
			}
			List<DiscountCacheRow> list = new List<DiscountCacheRow>();
			List<DiscountCacheRow> list2 = new List<DiscountCacheRow>();
			foreach (DiscountCacheRow item in _matchingDiscountsForDoc.Where((DiscountCacheRow d) => d.UseType != UsageMethodsSE.ShowMessage.Value))
			{
				if (item.IsGoodInMySegment(row.ItemID, goodsBySegmentRows) && (string.IsNullOrWhiteSpace(item.Storage) || row.ItemType == ItemTypesSE.Services.Value || row.Storage == item.Storage))
				{
					list.Add(item);
				}
			}
			foreach (DiscountCacheRow item2 in _matchingGiftDiscountsForDoc)
			{
				if (item2.IsGoodInMySegment(row.ItemID, goodsBySegmentRows) && (string.IsNullOrWhiteSpace(item2.Storage) || row.ItemType == ItemTypesSE.Services.Value || row.Storage == item2.Storage))
				{
					list2.Add(item2);
				}
			}
			foreach (DiscountCacheRow discount in GetMatchingDiscountsForRow(DocInfo.PartnerCode, DocInfo.DocDate, row))
			{
				if (discount.UseType == UsageMethodsSE.ShowMessage.Value)
				{
					if (!_matchingDiscountsForDoc.Any((DiscountCacheRow d) => d.Code == discount.Code))
					{
						_matchingDiscountsForDoc.Add(discount);
					}
				}
				else if (discount.UseType == UsageMethodsSE.Gift.Value || discount.UseType == UsageMethodsSE.GiftSet.Value)
				{
					if (!_matchingDiscountsForDoc.Any((DiscountCacheRow d) => d.Code == discount.Code))
					{
						list2.Add(discount);
					}
				}
				else
				{
					if (!discount.IsGoodInMySegment(row.ItemID, goodsBySegmentRows) || (!string.IsNullOrWhiteSpace(discount.Storage) && !(row.ItemType == ItemTypesSE.Services.Value) && !(row.Storage == discount.Storage)))
					{
						continue;
					}
					if (discount.DiscountCondition.Type == DiscountConditionTypesSE.OneTimeSaleRowQuantity.Value && discount.UseType == UsageMethodsSE.GiftWithLowestPrice.Value)
					{
						discount.Amount = 0m;
						decimal giftQuantity = discount.GiftQuantity;
						foreach (ISaleRow item3 in from r in DocInfo.Rows
							where !r.IsDeleted && r.ItemID != -1 && r.Price > 0m && r.Quantity >= discount.DiscountCondition.Limit && (string.IsNullOrWhiteSpace(discount.DiscountCondition.Segment) || SegmentsHelpers.IsGoodInSegment(r.ItemID, discount.DiscountCondition.Segment, discount.DiscountCondition.SegmentNegation, goodsBySegmentRows))
							orderby r.Price
							select r)
						{
							if (giftQuantity > 0m)
							{
								if (item3.Quantity >= giftQuantity)
								{
									discount.Amount += (giftQuantity * item3.Price * discount.GiftDiscountPercent / 100m).RoundAs(2);
									break;
								}
								discount.Amount += (item3.Quantity * item3.Price * discount.GiftDiscountPercent / 100m).RoundAs(2);
								giftQuantity -= item3.Quantity;
								continue;
							}
							break;
						}
					}
					list.Add(discount);
				}
			}
			if (list.Count > 0)
			{
				_rowsMatchingDiscounts.Add(row.RowID, list);
			}
			if (list2.Count > 0)
			{
				_rowsMatchingGiftDiscounts.Add(row.RowID, list2);
			}
		}
	}

	private List<DiscountCacheRow> GetMatchingDiscountsForRow(string docPartCode, DateOnly docDate, ISaleRow row)
	{
		List<DiscountCacheRow> list = new List<DiscountCacheRow>();
		string text = string.Empty;
		string text2 = string.Empty;
		if (!string.IsNullOrWhiteSpace(docPartCode))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(docPartCode);
			if (item != null)
			{
				text = item.GroupRow?.Code;
				if (!string.IsNullOrWhiteSpace(text))
				{
					PartnerGroupCacheRow item2 = TradeSettings.TradeCacheService.PartnerGroup.GetItem(text);
					if (item2 != null)
					{
						text2 = item2.Path;
					}
				}
			}
		}
		foreach (DiscountCacheRow item3 in TradeSettings.TradeCacheService.Discount.GetList((DiscountCacheRow d) => d.IsActive && d.StartDate <= docDate && (!d.EndDate.HasValue || docDate <= d.EndDate.Value) && d.DiscountCondition.Scope == DiscountConditionScopeSE.Row.Value))
		{
			PartnerGroupCacheRow partnerGroupCacheRow = null;
			if (!string.IsNullOrWhiteSpace(item3.DiscountCondition.PartnerGroup))
			{
				partnerGroupCacheRow = TradeSettings.TradeCacheService.PartnerGroup.GetItem(item3.DiscountCondition.PartnerGroup);
			}
			if ((string.IsNullOrWhiteSpace(item3.DiscountCondition.PartnerGroup) || item3.DiscountCondition.PartnerGroup == text || (!string.IsNullOrWhiteSpace(text2) && partnerGroupCacheRow != null && text2.StartsWith(partnerGroupCacheRow.Path + "!"))) && item3.DiscountCondition.Type == DiscountConditionTypesSE.OneTimeSaleRowQuantity.Value && row.Quantity >= item3.DiscountCondition.Limit && (string.IsNullOrWhiteSpace(item3.DiscountCondition.Segment) || SegmentsHelpers.IsGoodInSegment(row.ItemID, item3.DiscountCondition.Segment, item3.DiscountCondition.SegmentNegation, goodsBySegmentRows)))
			{
				list.Add(item3);
			}
		}
		return list;
	}

	private void FindMatchingDiscountsForDocument()
	{
		string text = string.Empty;
		string text2 = string.Empty;
		bool flag = string.IsNullOrWhiteSpace(DocInfo.PartnerCode);
		PartnerCacheRow partnerCacheRow = null;
		if (!flag)
		{
			partnerCacheRow = TradeSettings.TradeCacheService.Partner.GetItem(DocInfo.PartnerCode);
			if (partnerCacheRow != null)
			{
				docPartID = partnerCacheRow.Id;
				text = partnerCacheRow.Type;
				if (!string.IsNullOrWhiteSpace(text))
				{
					PartnerGroupCacheRow item = TradeSettings.TradeCacheService.PartnerGroup.GetItem(text);
					if (item != null)
					{
						text2 = item.Path;
					}
				}
			}
		}
		else
		{
			docPartID = -1;
		}
		foreach (DiscountCacheRow discountObject in TradeSettings.TradeCacheService.Discount.GetClonedList((DiscountCacheRow p) => p.DiscountCondition.Scope == DiscountConditionScopeSE.Document.Value && (p.DiscountDocType.Trim() == string.Empty || MapDiscountDocType(p.DiscountDocType) == _documentType.Value)))
		{
			if (!discountObject.IsActive || discountObject.StartDate > DocInfo.DocDate || (discountObject.EndDate.HasValue && DocInfo.DocDate > discountObject.EndDate.Value))
			{
				continue;
			}
			DateTime serverDate = TradeSettings.CreateTradeClient().CommonServices.GetServerDate();
			bool flag2 = false;
			bool flag3 = false;
			if (discountObject.DiscountCondition.UseExtension && !string.IsNullOrWhiteSpace(discountObject.DiscountCondition.UserScript))
			{
				flag3 = true;
			}
			int key = (int)((serverDate.DayOfWeek == DayOfWeek.Sunday) ? ((DayOfWeek)7) : serverDate.DayOfWeek);
			int num;
			if (discountObject.DiscountCondition.DaysHours[key].FirstStartTime.HasValue && discountObject.DiscountCondition.DaysHours[key].FirstEndTime.HasValue)
			{
				if (discountObject.DiscountCondition.DaysHours[key].Active)
				{
					TimeSpan timeOfDay = serverDate.TimeOfDay;
					TimeSpan? firstStartTime = discountObject.DiscountCondition.DaysHours[key].FirstStartTime;
					if (timeOfDay >= firstStartTime)
					{
						timeOfDay = serverDate.TimeOfDay;
						firstStartTime = discountObject.DiscountCondition.DaysHours[key].FirstEndTime;
						num = ((timeOfDay <= firstStartTime) ? 1 : 0);
						goto IL_0291;
					}
				}
				num = 0;
				goto IL_0291;
			}
			goto IL_0293;
			IL_0291:
			flag2 = (byte)num != 0;
			goto IL_0293;
			IL_0293:
			if (!(!discountObject.UseDayHours | flag2))
			{
				continue;
			}
			if ((discountObject.UseDayHours & flag2) && DocInfo.DocDate != serverDate.Date.ToDateOnly())
			{
				throw new RESTException(ArmSoft.SME.Common.Resources.DocDateNotCurrent + " " + ArmSoft.SME.Common.Resources.DiscountNotCalculated);
			}
			bool flag4 = false;
			if (flag3 && !(flag3 & flag4))
			{
				continue;
			}
			PartnerGroupCacheRow partnerGroupCacheRow = null;
			if (!string.IsNullOrWhiteSpace(discountObject.DiscountCondition.PartnerGroup))
			{
				partnerGroupCacheRow = TradeSettings.TradeCacheService.PartnerGroup.GetItem(discountObject.DiscountCondition.PartnerGroup);
			}
			bool flag5 = DateTime.IsLeapYear(DocInfo.DocDate.Year);
			bool flag6 = false;
			int num2 = 0;
			int num3 = 0;
			if (flag5)
			{
				num2 = 1;
			}
			if (partnerCacheRow != null && partnerCacheRow.Birthday.HasValue)
			{
				flag6 = DateTime.IsLeapYear(partnerCacheRow.Birthday.Value.Year);
			}
			if (flag6)
			{
				num3 = 1;
			}
			if ((discountObject.DiscountCondition.IsBirthDayDiscount && (partnerCacheRow == null || !discountObject.DiscountCondition.IsBirthDayDiscount || !partnerCacheRow.Birthday.HasValue || DocInfo.DocDate.DayOfYear - num2 < partnerCacheRow.Birthday.Value.DayOfYear - num3 - discountObject.DiscountCondition.BeforBDay || DocInfo.DocDate.DayOfYear - num2 > partnerCacheRow.Birthday.Value.DayOfYear - num3 + discountObject.DiscountCondition.AfterBDay)) || (!string.IsNullOrWhiteSpace(discountObject.DiscountCondition.PartnerGroup) && !(discountObject.DiscountCondition.PartnerGroup == text) && (string.IsNullOrWhiteSpace(text2) || partnerGroupCacheRow == null || !text2.StartsWith(partnerGroupCacheRow.Path + "!"))))
			{
				continue;
			}
			decimal num4 = 0m;
			decimal num5 = 0m;
			foreach (ISaleRow row in DocInfo.Rows)
			{
				if (!row.IsDeleted && row.ItemID != -1 && (string.IsNullOrWhiteSpace(discountObject.DiscountCondition.Segment) || SegmentsHelpers.IsGoodInSegment(row.ItemID, discountObject.DiscountCondition.Segment, discountObject.DiscountCondition.SegmentNegation, goodsBySegmentRows)))
				{
					num4 += (row.Quantity * row.Price).RoundAs(2);
					num5 += row.Quantity;
				}
			}
			num4 = (num4 * DocInfo.CurrRate / DocInfo.CurrBase).RoundAs(2);
			if (discountObject.IsMultiple)
			{
				decimal num6 = 1m;
				decimal num7 = discountObject.DiscountCondition.Limit;
				if (discountObject.DiscountCondition.Type == DiscountConditionTypesSE.OneTimeSaleSumma.Value)
				{
					num7 = num4;
				}
				else if (discountObject.DiscountCondition.Type == DiscountConditionTypesSE.OneTimeSaleTotalQuantity.Value)
				{
					num7 = num5;
				}
				num6 = decimal.Truncate(num7 / discountObject.DiscountCondition.Limit);
				if (num6 < 1m)
				{
					num6 = 1m;
				}
				else if (num6 > 1m && num7 < num6 * discountObject.DiscountCondition.Limit)
				{
					num6 = -1m;
				}
				if (discountObject.UseType == UsageMethodsSE.FixedDiscountAmount.Value)
				{
					discountObject.Amount = (num6 * discountObject.Amount).RoundAs(2);
				}
				else if (discountObject.UseType.InList(UsageMethodsSE.GiftWithLowestPrice.Value, UsageMethodsSE.Gift.Value, UsageMethodsSE.GiftSet.Value))
				{
					discountObject.GiftQuantity = (num6 * discountObject.GiftQuantity).RoundAs(3);
				}
			}
			if (discountObject.UseType == UsageMethodsSE.GiftWithLowestPrice.Value)
			{
				discountObject.Amount = 0m;
				decimal giftQuantity = discountObject.GiftQuantity;
				foreach (ISaleRow item2 in from x in DocInfo.Rows
					where !x.IsDeleted && x.ItemID != -1 && x.Price > 0m && (string.IsNullOrWhiteSpace(discountObject.SegmentCode) || SegmentsHelpers.IsGoodInSegment(x.ItemID, discountObject.SegmentCode, discountObject.SegmentNegation, goodsBySegmentRows))
					orderby x.Price
					select x)
				{
					if (giftQuantity <= 0m)
					{
						break;
					}
					if (item2.Quantity >= giftQuantity)
					{
						discountObject.Amount += (giftQuantity * item2.Price * discountObject.GiftDiscountPercent / 100m).RoundAs(2);
						break;
					}
					discountObject.Amount += (item2.Quantity * item2.Price * discountObject.GiftDiscountPercent / 100m).RoundAs(2);
					giftQuantity -= item2.Quantity;
				}
			}
			if (discountObject.UseType == UsageMethodsSE.Gift.Value)
			{
				discountObject.Amount = 0m;
				decimal giftQuantity2 = discountObject.GiftQuantity;
				MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(discountObject.GiftGoodID);
				if (itemById != null)
				{
					discountObject.GiftPrice = Methods.RoundPrice(TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(DocInfo.PriceType, DocInfo.VATType, itemById.Id, itemById.Unit, DocInfo.DocDate, DocInfo.CurrCode), DocInfo.PriceListTypeRoundDgr);
					discountObject.Amount = (discountObject.GiftPrice * giftQuantity2).RoundAs(2);
				}
			}
			List<MTComplectsPartsDataProvider.DataRow> list = new List<MTComplectsPartsDataProvider.DataRow>();
			Dictionary<string, List<MTComplectsPartsDataProvider.DataRow>> dictionary = new Dictionary<string, List<MTComplectsPartsDataProvider.DataRow>>();
			if (discountObject.UseType == UsageMethodsSE.GiftSet.Value)
			{
				MaterialCacheRow itemById2 = TradeSettings.TradeCacheService.Material.GetItemById(discountObject.GiftGoodID);
				if (itemById2 != null)
				{
					discountObject.Amount = 0m;
					decimal giftQuantity3 = discountObject.GiftQuantity;
					decimal giftQuantity4 = discountObject.GiftQuantity;
					List<MTComplectsPartsDataProvider.DataRow> rows = new MTComplectsPartsDataProvider(TradeSettings.CreateTradeClient()).GetRows(new MTComplectsPartsDataProvider.Param
					{
						ComplectCode = itemById2.MTCode
					}, withoutAccessCheck: true);
					if (rows.Count > 0)
					{
						decimal num8 = 0m;
						List<MTComplectsPartsDataProvider.DataRow> list2 = new List<MTComplectsPartsDataProvider.DataRow>();
						foreach (MTComplectsPartsDataProvider.DataRow item3 in rows)
						{
							if (item3 != null)
							{
								giftQuantity4 = (item3.QTY = ((!(item3.ComplectQTY != 0m)) ? (giftQuantity3 * item3.QTY) : (giftQuantity3 * item3.QTY / item3.ComplectQTY)));
								num8 = (item3.Summ = Methods.RoundPrice(TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(DocInfo.PriceType, DocInfo.VATType, item3.MTSourceID, item3.QntUnit, DocInfo.DocDate, DocInfo.CurrCode), DocInfo.PriceListTypeRoundDgr));
								if (discountObject.SelectGift)
								{
									list2.Add(item3);
									continue;
								}
								list.Add(item3);
								discountObject.Amount += (num8 * giftQuantity4).RoundAs(2);
							}
						}
						dictionary.Add(discountObject.Code, list2);
					}
				}
			}
			if (discountObject.DiscountCondition.Type == DiscountConditionTypesSE.OneTimeSaleSumma.Value)
			{
				if (!(num4 >= discountObject.DiscountCondition.Limit))
				{
					continue;
				}
				if (discountObject.UseType == UsageMethodsSE.Gift.Value || discountObject.UseType == UsageMethodsSE.GiftSet.Value)
				{
					_matchingGiftDiscountsForDoc.Add(discountObject);
					foreach (MTComplectsPartsDataProvider.DataRow item4 in list)
					{
						_matchingGiftIDsForDoc.Add(item4);
					}
					foreach (KeyValuePair<string, List<MTComplectsPartsDataProvider.DataRow>> item5 in dictionary)
					{
						_matchingGiftRowsWithSelectForDoc.Add(item5.Key, item5.Value);
					}
				}
				else
				{
					_matchingDiscountsForDoc.Add(discountObject);
				}
			}
			else if (discountObject.DiscountCondition.Type == DiscountConditionTypesSE.OneTimeSaleTotalQuantity.Value)
			{
				if (!(num5 >= discountObject.DiscountCondition.Limit))
				{
					continue;
				}
				if (discountObject.UseType == UsageMethodsSE.Gift.Value || discountObject.UseType == UsageMethodsSE.GiftSet.Value)
				{
					_matchingGiftDiscountsForDoc.Add(discountObject);
					foreach (MTComplectsPartsDataProvider.DataRow item6 in list)
					{
						_matchingGiftIDsForDoc.Add(item6);
					}
					foreach (KeyValuePair<string, List<MTComplectsPartsDataProvider.DataRow>> item7 in dictionary)
					{
						_matchingGiftRowsWithSelectForDoc.Add(item7.Key, item7.Value);
					}
				}
				else
				{
					_matchingDiscountsForDoc.Add(discountObject);
				}
			}
			else
			{
				if (!(discountObject.DiscountCondition.Type == DiscountConditionTypesSE.AccumulativeSaleSumma.Value) || docPartID == -1)
				{
					continue;
				}
				DateOnly startDate = DateOnly.FromDateTime(ArmSoft.SME.Common.Constants.Common.minOPDate);
				DateOnly endDate = DateOnly.FromDateTime(ArmSoft.SME.Common.Constants.Common.maxOPDate);
				if (discountObject.DiscountCondition.ConditionStart.HasValue)
				{
					startDate = discountObject.DiscountCondition.ConditionStart.Value;
				}
				if (discountObject.DiscountCondition.ConditionEnd.HasValue)
				{
					endDate = discountObject.DiscountCondition.ConditionEnd.Value;
				}
				decimal mTSaleSumByPartner = TradeSettings.CreateTradeClient().CommonServices.GetMTSaleSumByPartner(getQty: false, CommonConst.SummaTypes.NCCSummWithVAT, CommonConst.SaleSumTypes.SaleSum, -1, string.Empty, startDate, endDate, docPartID, getByDescr: false, string.Empty, discountObject.DiscountCondition.Segment, discountObject.DiscountCondition.SegmentNegation);
				if (discountObject.DiscountCondition.IncludeCurrentSale)
				{
					mTSaleSumByPartner += num4;
				}
				if (mTSaleSumByPartner >= discountObject.DiscountCondition.Limit)
				{
					if (discountObject.UseType == UsageMethodsSE.Gift.Value || discountObject.UseType == UsageMethodsSE.GiftSet.Value)
					{
						_matchingDiscountsForDoc.Add(discountObject);
						foreach (MTComplectsPartsDataProvider.DataRow item8 in list)
						{
							_matchingGiftIDsForDoc.Add(item8);
						}
						foreach (KeyValuePair<string, List<MTComplectsPartsDataProvider.DataRow>> item9 in dictionary)
						{
							_matchingGiftRowsWithSelectForDoc.Add(item9.Key, item9.Value);
						}
					}
					else
					{
						_matchingDiscountsForDoc.Add(discountObject);
					}
				}
				_partnerAccumulatedAmountsByDiscount.Add(discountObject.Code, mTSaleSumByPartner);
			}
		}
	}

	public void GenerateNewGiftDiscountsForDocument(Dictionary<string, int> selectedGiftsDiscounts)
	{
		new List<int>();
		foreach (DiscountCacheRow item in _matchingGiftDiscountsForDoc)
		{
			if (!selectedGiftsDiscounts.TryGetValue(item.Code, out var value))
			{
				continue;
			}
			item.Amount = 0m;
			_ = item.GiftQuantity;
			MaterialCacheRow goodCacheRow = TradeSettings.TradeCacheService.Material.GetItemById(value);
			List<MTComplectsPartsDataProvider.DataRow> source = _matchingGiftRowsWithSelectForDoc[item.Code];
			if (goodCacheRow != null)
			{
				decimal num = (from p in source
					where p.MTSourceID == goodCacheRow.Id
					select p.QTY).DefaultIfEmpty(0m).Sum();
				item.GiftPrice = Methods.RoundPrice(TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(DocInfo.PriceType, DocInfo.VATType, goodCacheRow.Id, goodCacheRow.Unit, DocInfo.DocDate, DocInfo.CurrCode), DocInfo.PriceListTypeRoundDgr);
				item.Amount = (item.GiftPrice * num).RoundAs(2);
			}
			_matchingNewGiftDiscountsForDoc.Add(item);
		}
	}

	public void CalculateNewGeneratedGiftDiscount(Dictionary<string, int> selectedGiftsDiscounts)
	{
		short num = 1;
		foreach (ISaleRow row in DocInfo.Rows)
		{
			if (!row.IsDeleted && row.ItemID != -1)
			{
				Dictionary<string, List<DiscountLogInfo>> discountDictionaryAndCalculateNewGeneratedGiftDiscountedSums = GetDiscountDictionaryAndCalculateNewGeneratedGiftDiscountedSums(row, selectedGiftsDiscounts);
				List<DiscountGroupResult> list = new List<DiscountGroupResult>();
				foreach (string key in discountDictionaryAndCalculateNewGeneratedGiftDiscountedSums.Keys)
				{
					DiscountGroupCacheRow discountGroupCacheRow = null;
					decimal discountedSum = 0m;
					if (!string.IsNullOrWhiteSpace(key))
					{
						discountGroupCacheRow = TradeSettings.TradeCacheService.DiscountGroup.GetItem(key);
					}
					string groupMergeMethod;
					byte groupPriority;
					if (discountGroupCacheRow == null)
					{
						groupMergeMethod = _paramDiscountUnionMethod;
						groupPriority = 0;
					}
					else
					{
						groupMergeMethod = discountGroupCacheRow.Type;
						groupPriority = discountGroupCacheRow.Priority;
					}
					foreach (DiscountLogInfo item in discountDictionaryAndCalculateNewGeneratedGiftDiscountedSums[key])
					{
						discountedSum += item.DiscountedSum;
						WriteToLog(row, num, key, groupPriority, groupMergeMethod, item);
					}
					list.Add(new DiscountGroupResult
					{
						GroupPriority = groupPriority,
						GroupCode = key,
						DiscountCode = string.Empty,
						DiscountPriority = 0,
						DiscountedSum = discountedSum
					});
				}
				row.DiscountSumma += CalculateDiscountedSumForRow(row.Quantity * row.Price, list);
				row.Summa = (row.Quantity * row.Price - row.DiscountSumma).RoundAs(2);
				if (row.Summa < 0m)
				{
					row.Summa = 0m;
					row.DiscountSumma = (row.Quantity * row.Price).RoundAs(2);
				}
				if (row.Quantity == 0m)
				{
					row.DiscountPrice = row.Price;
				}
				else
				{
					row.DiscountPrice = (row.Summa / row.Quantity).RoundAs(2);
				}
				if (row.Price != 0m)
				{
					row.Discount = (100m * (row.Price - row.DiscountPrice) / row.Price).RoundAs(2);
					if (row.Discount > 100m)
					{
						row.Discount = 100m;
						row.DiscountPrice = (row.Price * row.Discount / 100m).RoundAs(2);
						row.Summa = (row.Quantity * row.DiscountPrice).RoundAs(2);
						row.DiscountSumma = row.BeginSumma - row.Summa;
					}
				}
				row.DiscountSumma = row.DiscountSumma.RoundAs(2);
			}
			num++;
		}
	}

	private Dictionary<string, List<DiscountLogInfo>> GetDiscountDictionaryAndCalculateNewGeneratedGiftDiscountedSums(ISaleRow row, Dictionary<string, int> selectedGiftsDiscounts)
	{
		Dictionary<string, List<DiscountLogInfo>> dictionary = new Dictionary<string, List<DiscountLogInfo>>();
		if (_rowsMatchingGiftDiscounts.ContainsKey(row.RowID))
		{
			foreach (DiscountCacheRow discount in _rowsMatchingGiftDiscounts[row.RowID])
			{
				if (!selectedGiftsDiscounts.ContainsKey(discount.Code))
				{
					continue;
				}
				decimal num = 0m;
				decimal num2 = 0m;
				decimal discountedSum = 0m;
				decimal num3 = (row.Quantity * row.Price).RoundAs(2);
				num2 = -1m;
				IEnumerable<KeyValuePair<int, List<DiscountCacheRow>>> enumerable = _rowsMatchingGiftDiscounts.Where((KeyValuePair<int, List<DiscountCacheRow>> k) => k.Value.Any((DiscountCacheRow d) => d.Code == discount.Code));
				if (!(discount.UseType == UsageMethodsSE.GiftSet.Value) || !discount.SelectGift)
				{
					continue;
				}
				if (row.RowID == enumerable.Last().Key)
				{
					foreach (DiscountCacheRow item2 in _matchingNewGiftDiscountsForDoc.Where((DiscountCacheRow k) => k.Code == discount.Code))
					{
						discountedSum = item2.Amount - (from l in Log
							where l.DiscountGroupCode == discount.DiscountGroupCode && l.DiscountCode == discount.Code
							select l.DiscountedSum).DefaultIfEmpty(0m).Sum();
					}
				}
				else
				{
					decimal num4 = 0m;
					foreach (KeyValuePair<int, List<DiscountCacheRow>> pair in enumerable)
					{
						if (pair.Key == row.RowID)
						{
							num4 += num3;
							continue;
						}
						ISaleRow saleRow = DocInfo.Rows.FirstOrDefault((ISaleRow r) => r.RowID == pair.Key);
						if (saleRow != null)
						{
							num4 += (saleRow.Price * saleRow.Quantity).RoundAs(2);
						}
					}
					discountedSum = ((!(num4 == 0m)) ? (discount.Amount / num4 * num3).RoundAs(2) : discount.Amount);
				}
				num = -1m;
				DiscountLogInfo item = new DiscountLogInfo
				{
					DiscountCode = discount.Code,
					Priority = discount.Priority,
					Percent = num,
					DiscountedSum = discountedSum,
					DiscountPrice = num2
				};
				if (dictionary.ContainsKey(discount.DiscountGroupCode))
				{
					dictionary[discount.DiscountGroupCode].Add(item);
					continue;
				}
				dictionary.Add(discount.DiscountGroupCode, new List<DiscountLogInfo> { item });
			}
		}
		return dictionary;
	}

	private short MapDiscountDocType(string discountDocType)
	{
		if (discountDocType == "1")
		{
			return DocumentTypeSE.MTInvoice.Value;
		}
		return DocumentTypeSE.MTBill.Value;
	}

	public void Calculate()
	{
		int num = 1;
		foreach (ISaleRow row in DocInfo.Rows)
		{
			CalculateDiscount(row, (short)num);
			CalculateGiftDiscount(row, (short)num);
			num++;
		}
	}

	private void RaiseMessageEvents()
	{
		foreach (DiscountCacheRow item in from d in _matchingDiscountsForDoc
			where d.UseType == UsageMethodsSE.ShowMessage.Value && (string.IsNullOrWhiteSpace(d.Storage) || DocInfo.Rows.Any((ISaleRow r) => r.Storage == d.Storage))
			orderby d.Priority, d.Code
			select d)
		{
			ShowMessageOnDiscount?.Invoke(item.Message);
		}
	}

	private void CalculateGiftDiscount(ISaleRow row, short rowNum)
	{
		if (Convert.ToBoolean(row.IsDeleted) || row.ItemID == -1)
		{
			return;
		}
		Dictionary<string, List<DiscountLogInfo>> discountDictionaryByGroupAndCalculateGiftDiscountedSums = GetDiscountDictionaryByGroupAndCalculateGiftDiscountedSums(row);
		List<DiscountGroupResult> list = new List<DiscountGroupResult>();
		foreach (string key in discountDictionaryByGroupAndCalculateGiftDiscountedSums.Keys)
		{
			DiscountGroupCacheRow discountGroupCacheRow = null;
			string empty = string.Empty;
			decimal discountedSum = 0m;
			if (!string.IsNullOrWhiteSpace(key))
			{
				discountGroupCacheRow = TradeSettings.TradeCacheService.DiscountGroup.GetItem(key);
			}
			byte groupPriority;
			if (discountGroupCacheRow == null)
			{
				empty = _paramDiscountUnionMethod;
				groupPriority = 0;
			}
			else
			{
				empty = discountGroupCacheRow.Type;
				groupPriority = discountGroupCacheRow.Priority;
			}
			foreach (DiscountLogInfo item in discountDictionaryByGroupAndCalculateGiftDiscountedSums[key])
			{
				discountedSum += item.DiscountedSum;
				WriteToLog(row, rowNum, key, groupPriority, empty, item);
			}
			list.Add(new DiscountGroupResult
			{
				GroupPriority = groupPriority,
				GroupCode = key,
				DiscountCode = string.Empty,
				DiscountPriority = 0,
				DiscountedSum = discountedSum
			});
		}
		row.DiscountSumma += CalculateDiscountedSumForRow(row.Quantity * row.Price, list);
		row.Summa = (row.Quantity * row.Price - row.DiscountSumma).RoundAs(2);
		if (row.Summa < 0m)
		{
			row.Summa = 0m;
			row.DiscountSumma = (row.Quantity * row.Price).RoundAs(2);
		}
		if (row.Quantity == 0m)
		{
			row.DiscountPrice = row.Price;
		}
		else
		{
			row.DiscountPrice = (row.Summa / row.Quantity).RoundAs(2);
		}
		if (row.Price != 0m)
		{
			row.Discount = (100m * (row.Price - row.DiscountPrice) / row.Price).RoundAs(2);
			if (row.Discount > 100m)
			{
				row.Discount = 100m;
				row.DiscountPrice = (row.Price * row.Discount / 100m).RoundAs(2);
				row.Summa = (row.Quantity * row.DiscountPrice).RoundAs(2);
				row.DiscountSumma = row.BeginSumma - row.Summa;
			}
		}
		row.DiscountSumma = row.DiscountSumma.RoundAs(2);
	}

	public void CalculateDiscount(ISaleRow row, short rowNum)
	{
		if (row.IsDeleted || row.ItemID == -1)
		{
			return;
		}
		Dictionary<string, List<DiscountLogInfo>> discountDictionaryByGroupAndCalculateDiscountedSums = GetDiscountDictionaryByGroupAndCalculateDiscountedSums(row);
		List<DiscountGroupResult> list = new List<DiscountGroupResult>();
		foreach (string key in discountDictionaryByGroupAndCalculateDiscountedSums.Keys)
		{
			DiscountGroupCacheRow discountGroupCacheRow = null;
			string empty = string.Empty;
			decimal num = 0m;
			if (!string.IsNullOrWhiteSpace(key))
			{
				discountGroupCacheRow = TradeSettings.TradeCacheService.DiscountGroup.GetItem(key);
			}
			byte groupPriority;
			if (discountGroupCacheRow == null)
			{
				empty = _paramDiscountUnionMethod;
				groupPriority = 0;
			}
			else
			{
				empty = discountGroupCacheRow.Type;
				groupPriority = discountGroupCacheRow.Priority;
			}
			if (empty == DiscountsMergeMethodsSE.Summation.Value.ToString())
			{
				foreach (DiscountLogInfo item in discountDictionaryByGroupAndCalculateDiscountedSums[key])
				{
					num += item.DiscountedSum;
					WriteToLog(row, rowNum, key, groupPriority, empty, item);
				}
			}
			else if (empty == DiscountsMergeMethodsSE.SequentialUsage.Value.ToString())
			{
				decimal num2 = 0m;
				decimal num3 = 0m;
				decimal num4 = 0m;
				bool flag = true;
				foreach (DiscountLogInfo item2 in from r in discountDictionaryByGroupAndCalculateDiscountedSums[key]
					orderby r.Priority, r.DiscountCode
					select r)
				{
					if (flag)
					{
						num3 = item2.DiscountedSum;
						if (row.Quantity != 0m)
						{
							num4 = (row.Price * row.Quantity - num3) / row.Quantity;
						}
						num2 = row.Price * row.Quantity - num3;
						WriteToLog(row, rowNum, key, groupPriority, empty, item2);
						num += num3;
						flag = false;
						continue;
					}
					if (item2.DiscountPrice != -1m)
					{
						num3 = num4 * row.Quantity - item2.DiscountPrice * row.Quantity;
						num4 = item2.DiscountPrice;
						num2 = num4 * row.Quantity;
					}
					else if (item2.Percent != -1m)
					{
						num4 *= 1m - item2.Percent / 100m;
						num3 = num2 - num4 * row.Quantity;
						num2 = num4 * row.Quantity;
					}
					else
					{
						num3 = item2.DiscountedSum;
						if (row.Quantity != 0m)
						{
							num4 = (row.Price * row.Quantity - num - num3) / row.Quantity;
						}
					}
					num += num3;
					item2.DiscountedSum = num3;
					WriteToLog(row, rowNum, key, groupPriority, empty, item2);
				}
			}
			else if (empty == DiscountsMergeMethodsSE.MaxPercent.Value.ToString())
			{
				num = decimal.MinValue;
				foreach (DiscountLogInfo item3 in discountDictionaryByGroupAndCalculateDiscountedSums[key])
				{
					if (item3.DiscountedSum > num)
					{
						num = item3.DiscountedSum;
					}
					WriteToLog(row, rowNum, key, groupPriority, empty, item3);
				}
			}
			else if (empty == DiscountsMergeMethodsSE.Priority.Value.ToString())
			{
				List<DiscountLogInfo> list2 = (from r in discountDictionaryByGroupAndCalculateDiscountedSums[key]
					orderby r.Priority, r.DiscountCode
					select r).ToList();
				num = list2.First().DiscountedSum;
				foreach (DiscountLogInfo item4 in list2)
				{
					WriteToLog(row, rowNum, key, groupPriority, empty, item4);
				}
			}
			list.Add(new DiscountGroupResult
			{
				GroupPriority = groupPriority,
				GroupCode = key,
				DiscountCode = string.Empty,
				DiscountPriority = 0,
				DiscountedSum = num
			});
		}
		row.DiscountSumma = CalculateDiscountedSumForRow(row.Quantity * row.Price, list);
		row.Summa = (row.Quantity * row.Price - row.DiscountSumma).RoundAs(2);
		if (row.Summa < 0m)
		{
			row.Summa = 0m;
			row.DiscountSumma = (row.Quantity * row.Price).RoundAs(2);
		}
		if (row.Quantity == 0m)
		{
			row.DiscountPrice = row.Price;
		}
		else
		{
			row.DiscountPrice = (row.Summa / row.Quantity).RoundAs(2);
		}
		if (row.Price != 0m)
		{
			row.Discount = (100m * (row.Price - row.DiscountPrice) / row.Price).RoundAs(2);
			if (row.Discount > 100m)
			{
				row.Discount = 100m;
				row.DiscountPrice = (row.Price * row.Discount / 100m).RoundAs(2);
				row.Summa = (row.Quantity * row.DiscountPrice).RoundAs(2);
				row.DiscountSumma = row.BeginSumma - row.Summa;
			}
		}
		row.DiscountSumma = row.DiscountSumma.RoundAs(2);
	}

	private Dictionary<string, List<DiscountLogInfo>> GetDiscountDictionaryByGroupAndCalculateDiscountedSums(ISaleRow row)
	{
		Dictionary<string, List<DiscountLogInfo>> dictionary = new Dictionary<string, List<DiscountLogInfo>>();
		if (_rowsMatchingDiscounts.ContainsKey(row.RowID))
		{
			foreach (DiscountCacheRow discount in _rowsMatchingDiscounts[row.RowID])
			{
				decimal num = 0m;
				decimal discountPrice = 0m;
				decimal discountedSum = 0m;
				decimal num2 = (row.Quantity * row.Price).RoundAs(2);
				bool flag = true;
				if (discount.UseType == UsageMethodsSE.GoodDiscountPercent.Value)
				{
					num = TradeSettings.TradeCacheService.Material.GetItemById(row.ItemID).Discount;
					discountedSum = num2 - row.Price * (1m - num / 100m) * row.Quantity;
					discountPrice = -1m;
				}
				else if (discount.UseType == UsageMethodsSE.Scale.Value)
				{
					if (discount.DiscountCondition.Type == DiscountConditionTypesSE.OneTimeSaleSumma.Value || discount.DiscountCondition.Type == DiscountConditionTypesSE.OneTimeSaleTotalQuantity.Value || discount.DiscountCondition.Type == DiscountConditionTypesSE.OneTimeSaleRowQuantity.Value)
					{
						num = TradeSettings.CreateTradeClient().CommonServices.CalculateDiscountByScale(DocInfo.TotalAmount, discount.Code);
						if (num == -1m)
						{
							num = 0m;
						}
					}
					else
					{
						decimal summa = 0m;
						if (_partnerAccumulatedAmountsByDiscount.ContainsKey(discount.Code))
						{
							summa = _partnerAccumulatedAmountsByDiscount[discount.Code];
						}
						num = TradeSettings.CreateTradeClient().CommonServices.CalculateDiscountByScale(summa, discount.Code);
						if (num == -1m)
						{
							num = 0m;
						}
					}
					discountPrice = -1m;
					discountedSum = num2 - row.Price * (1m - num / 100m) * row.Quantity;
				}
				else if (discount.UseType == UsageMethodsSE.PartnerCard.Value)
				{
					discountPrice = -1m;
					if (DocInfo.PartnerContractPercent.HasValue && DocInfo.PartnerContractPercent.HasValue)
					{
						num = DocInfo.PartnerContractPercent.Value;
						discountedSum = num2 - row.Price * (1m - num / 100m) * row.Quantity;
					}
				}
				else if (discount.UseType == UsageMethodsSE.DiscountPriceType.Value)
				{
					string priceType = discount.PriceType;
					if (GetGridRowDiscountPriceBasedOnDiscountPriceList(ref discountPrice, row, priceType))
					{
						discountedSum = num2 - discountPrice * row.Quantity;
						num = -1m;
					}
					else
					{
						flag = false;
					}
				}
				else if (discount.UseType == UsageMethodsSE.DiscountPercent.Value)
				{
					discountPrice = -1m;
					num = discount.Percent;
					discountedSum = num2 - row.Price * (1m - num / 100m) * row.Quantity;
				}
				else if (discount.UseType == UsageMethodsSE.FixedDiscountAmount.Value || discount.UseType == UsageMethodsSE.GiftWithLowestPrice.Value)
				{
					discountPrice = -1m;
					IEnumerable<KeyValuePair<int, List<DiscountCacheRow>>> enumerable = _rowsMatchingDiscounts.Where((KeyValuePair<int, List<DiscountCacheRow>> k) => k.Value.Any((DiscountCacheRow d) => d.Code == discount.Code));
					if (discount.IsDistrInProportion || discount.UseType == UsageMethodsSE.FixedDiscountAmount.Value)
					{
						if (row.RowID == enumerable.Last().Key)
						{
							discountedSum = discount.Amount - (from l in Log
								where l.DiscountGroupCode == discount.DiscountGroupCode && l.DiscountCode == discount.Code
								select l.DiscountedSum).DefaultIfEmpty(0m).Sum();
						}
						else
						{
							decimal num3 = 0m;
							foreach (KeyValuePair<int, List<DiscountCacheRow>> pair in enumerable)
							{
								if (pair.Key == row.RowID)
								{
									num3 += num2;
									continue;
								}
								ISaleRow saleRow = DocInfo.Rows.FirstOrDefault((ISaleRow r) => r.RowID == pair.Key);
								if (saleRow != null)
								{
									num3 += (saleRow.Price * saleRow.Quantity).RoundAs(2);
								}
							}
							discountedSum = ((!(num3 == 0m)) ? (discount.Amount / num3 * num2).RoundAs(2) : discount.Amount);
						}
					}
					else
					{
						decimal giftQuantity = discount.GiftQuantity;
						foreach (ISaleRow item2 in from x in DocInfo.Rows
							where !x.IsDeleted && x.ItemID != -1 && x.Price > 0m && (string.IsNullOrWhiteSpace(discount.SegmentCode) || SegmentsHelpers.IsGoodInSegment(x.ItemID, discount.SegmentCode, discount.SegmentNegation, goodsBySegmentRows))
							orderby x.Price
							select x)
						{
							if (!(giftQuantity > 0m))
							{
								break;
							}
							if (item2.Quantity >= giftQuantity)
							{
								item2.DiscountSumma = (giftQuantity * item2.Price * discount.GiftDiscountPercent / 100m).RoundAs(2);
								if (item2.RowID == row.RowID)
								{
									discountedSum = item2.DiscountSumma;
								}
								break;
							}
							item2.DiscountSumma = (item2.Quantity * item2.Price * discount.GiftDiscountPercent / 100m).RoundAs(2);
							giftQuantity -= item2.Quantity;
							if (item2.RowID == row.RowID)
							{
								discountedSum = item2.DiscountSumma;
							}
						}
					}
					num = -1m;
				}
				else if (discount.UseType == UsageMethodsSE.ShowMessage.Value)
				{
					flag = false;
				}
				if (flag)
				{
					DiscountLogInfo item = new DiscountLogInfo
					{
						DiscountCode = discount.Code,
						Priority = discount.Priority,
						Percent = num,
						DiscountedSum = discountedSum,
						DiscountPrice = discountPrice
					};
					if (dictionary.ContainsKey(discount.DiscountGroupCode))
					{
						dictionary[discount.DiscountGroupCode].Add(item);
						continue;
					}
					dictionary.Add(discount.DiscountGroupCode, new List<DiscountLogInfo> { item });
				}
			}
		}
		return dictionary;
	}

	private bool GetGridRowDiscountPriceBasedOnDiscountPriceList(ref decimal discountPrice, ISaleRow row, string discountPriceListType)
	{
		discountPrice = 0m;
		if (row.Price != 0m)
		{
			string priceTypeRoungDgr = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(discountPriceListType);
			decimal num = 1m;
			decimal num2 = 1m;
			string priceListRowCurr = string.Empty;
			string priceListRowVATType = string.Empty;
			PriceHelper.GetPriceListRowVATType(discountPriceListType, out priceListRowVATType, out priceListRowCurr);
			if (priceListRowCurr != DocInfo.CurrCode && priceListRowCurr != "AMD")
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(priceListRowCurr, DocInfo.DocDate);
				num = exchangeRates.Rate;
				num2 = exchangeRates.Base;
			}
			PricesDataProvider.DataRow dataRow = null;
			if (!string.IsNullOrWhiteSpace(discountPriceListType))
			{
				dataRow = TradeSettings.CreateTradeClient().CommonServices.GetPrice(discountPriceListType, row.ItemID, row.UnitCode, DocInfo.DocDate);
			}
			if (string.IsNullOrWhiteSpace(dataRow.PriceType))
			{
				return false;
			}
			if (dataRow != null && dataRow.Price != 0m)
			{
				if (priceListRowCurr == DocInfo.CurrCode)
				{
					discountPrice = dataRow.Price;
				}
				else if (priceListRowCurr == "AMD")
				{
					discountPrice = (dataRow.Price / DocInfo.CurrRate * DocInfo.CurrBase).RoundAs(2);
				}
				else
				{
					discountPrice = (dataRow.Price * num / num2 / DocInfo.CurrRate * DocInfo.CurrBase).RoundAs(2);
				}
				if (priceListRowVATType != DocInfo.VATType)
				{
					discountPrice = PriceHelper.CalcSumDependOnVATType(discountPrice, DocInfo.VATType, priceListRowVATType, Convert.ToBoolean(row.VAT.HasValue && row.VAT.Value));
				}
				discountPrice = Methods.RoundPrice(discountPrice, priceTypeRoungDgr);
			}
		}
		return true;
	}

	private decimal CalculateDiscountedSumForRow(decimal amount, List<DiscountGroupResult> totalDiscountedSumsByGroups)
	{
		if (amount == 0m)
		{
			return 0m;
		}
		decimal number = 0m;
		if (_paramDiscountUnionMethod == DiscountsMergeMethodsSE.Summation.Value.ToString())
		{
			foreach (DiscountGroupResult totalDiscountedSumsByGroup in totalDiscountedSumsByGroups)
			{
				number += totalDiscountedSumsByGroup.DiscountedSum;
			}
		}
		else if (_paramDiscountUnionMethod == DiscountsMergeMethodsSE.SequentialUsage.Value.ToString())
		{
			decimal num = amount;
			decimal num2 = 0m;
			for (int i = 0; i < totalDiscountedSumsByGroups.Count; i++)
			{
				num2 = ((i != 0) ? (num * (totalDiscountedSumsByGroups[i].DiscountedSum * 100m / amount) / 100m) : totalDiscountedSumsByGroups[i].DiscountedSum);
				num -= num2;
			}
			number = amount - num;
		}
		else if (_paramDiscountUnionMethod == DiscountsMergeMethodsSE.MaxPercent.Value.ToString())
		{
			if (totalDiscountedSumsByGroups.Count > 0)
			{
				number = totalDiscountedSumsByGroups.Max((DiscountGroupResult r) => r.DiscountedSum);
			}
		}
		else if (_paramDiscountUnionMethod == DiscountsMergeMethodsSE.Priority.Value.ToString() && totalDiscountedSumsByGroups.Count > 0)
		{
			number = (from r in totalDiscountedSumsByGroups
				orderby r.GroupPriority, r.GroupCode, r.DiscountPriority, r.DiscountCode
				select r).First().DiscountedSum;
		}
		return number.RoundAs(2);
	}

	private void WriteToLog(ISaleRow row, short rowNum, string groupCode, byte groupPriority, string groupMergeMethod, DiscountLogInfo logInfo)
	{
		DiscountCacheRow item = TradeSettings.TradeCacheService.Discount.GetItem(logInfo.DiscountCode);
		string discountCaption = string.Empty;
		if (item != null)
		{
			discountCaption = item.Caption;
		}
		DiscountLogRow discountLogRow = new DiscountLogRow
		{
			GoodId = row.ItemID,
			RowNumber = rowNum,
			DiscountGroupCode = groupCode,
			GroupMergeMethod = groupMergeMethod,
			GroupMergeMethodCaption = SmartEnum<DiscountsMergeMethodsSE, string>.FromValue(groupMergeMethod).Caption,
			GoodCode = row.ItemCode,
			GoodCaption = row.ItemName,
			DiscountCode = logInfo.DiscountCode,
			DiscountCaption = discountCaption,
			DiscountPriority = logInfo.Priority,
			DiscountedSum = logInfo.DiscountedSum
		};
		if (logInfo.DiscountedSum == 0m)
		{
			discountLogRow.Percent = 0m;
		}
		else if (logInfo.Percent == -1m)
		{
			if (logInfo.DiscountPrice == -1m)
			{
				decimal num = (row.Price * row.Quantity).RoundAs(2);
				if (num != 0m)
				{
					discountLogRow.Percent = (logInfo.DiscountedSum * 100m / num).RoundAs(2);
				}
			}
			else if (row.Price != 0m)
			{
				discountLogRow.Percent = (100m * (row.Price - logInfo.DiscountPrice) / row.Price).RoundAs(2);
			}
		}
		else
		{
			discountLogRow.Percent = logInfo.Percent;
		}
		Log.Add(discountLogRow);
	}

	private Dictionary<string, List<DiscountLogInfo>> GetDiscountDictionaryByGroupAndCalculateGiftDiscountedSums(ISaleRow row)
	{
		Dictionary<string, List<DiscountLogInfo>> dictionary = new Dictionary<string, List<DiscountLogInfo>>();
		if (_rowsMatchingGiftDiscounts.ContainsKey(row.RowID))
		{
			foreach (DiscountCacheRow discount in _rowsMatchingGiftDiscounts[row.RowID])
			{
				decimal num = 0m;
				decimal num2 = 0m;
				decimal num3 = 0m;
				decimal num4 = (row.Quantity * row.Price).RoundAs(2);
				num2 = -1m;
				IEnumerable<KeyValuePair<int, List<DiscountCacheRow>>> enumerable = _rowsMatchingGiftDiscounts.Where((KeyValuePair<int, List<DiscountCacheRow>> k) => k.Value.Any((DiscountCacheRow d) => d.Code == discount.Code));
				if (!(discount.UseType == UsageMethodsSE.Gift.Value) && (!(discount.UseType == UsageMethodsSE.GiftSet.Value) || discount.SelectGift))
				{
					continue;
				}
				if (row.RowID == enumerable.Last().Key)
				{
					num3 = discount.Amount - (from l in Log
						where l.DiscountGroupCode == discount.DiscountGroupCode && l.DiscountCode == discount.Code
						select l.DiscountedSum).DefaultIfEmpty(0m).Sum();
				}
				else
				{
					decimal num5 = 0m;
					foreach (KeyValuePair<int, List<DiscountCacheRow>> pair in enumerable)
					{
						if (pair.Key == row.RowID)
						{
							num5 += num4;
							continue;
						}
						ISaleRow saleRow = DocInfo.Rows.FirstOrDefault((ISaleRow r) => r.RowID == pair.Key);
						if (saleRow != null)
						{
							num5 += (saleRow.Price * saleRow.Quantity).RoundAs(2);
						}
					}
					num3 = ((!(num5 == 0m)) ? (discount.Amount / num5 * num4).RoundAs(2) : discount.Amount);
				}
				num = -1m;
				DiscountLogInfo item = new DiscountLogInfo
				{
					DiscountCode = discount.Code,
					Priority = discount.Priority,
					Percent = num,
					DiscountedSum = num3,
					DiscountPrice = num2
				};
				if (dictionary.ContainsKey(discount.DiscountGroupCode))
				{
					dictionary[discount.DiscountGroupCode].Add(item);
					continue;
				}
				dictionary.Add(discount.DiscountGroupCode, new List<DiscountLogInfo> { item });
			}
		}
		return dictionary;
	}

	private void AddGiftGoodsRows()
	{
		AddGiftGoodsToDoc?.Invoke(_matchingGiftDiscountsForDoc, _matchingGiftIDsForDoc);
	}

	private void SelectAndAddGiftGoodsRows()
	{
		SelectAndAddGiftGoodsToDoc?.Invoke(_matchingGiftDiscountsForDoc, _matchingGiftRowsWithSelectForDoc);
	}
}
