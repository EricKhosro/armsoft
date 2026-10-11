using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Documents;

namespace ArmSoft.SME.Trade.UI.Helpers;

public class PriceHelper
{
	public static decimal GetPriceForUnit(string oldUnit, string newUnit, decimal oldPrice, string mtCode)
	{
		decimal mTQntUnitCoeff = GetMTQntUnitCoeff(mtCode, oldUnit, showError: false);
		decimal mTQntUnitCoeff2 = GetMTQntUnitCoeff(mtCode, newUnit);
		return (oldPrice / mTQntUnitCoeff * mTQntUnitCoeff2).RoundAs(2);
	}

	public static decimal GetMTQntUnitCoeff(string mtCode, string qntUnit, bool showError = true)
	{
		int id = TradeSettings.TradeCacheService.Material.GetItem(mtCode).Id;
		MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(id, qntUnit);
		if (qntUnit2 == null)
		{
			if (showError)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.InvalidMTQntUnit, mtCode, qntUnit));
			}
			return 1m;
		}
		return qntUnit2.Coef;
	}

	public static bool GetPriceListRowVATType(string priceListType, out string priceListRowVATType, out string priceListRowCurr)
	{
		priceListRowVATType = string.Empty;
		priceListRowCurr = string.Empty;
		PriceTypeCacheRow item = TradeSettings.TradeCacheService.PriceType.GetItem(priceListType);
		if (item == null)
		{
			return false;
		}
		priceListRowVATType = (item.VAT ? "2" : "1");
		priceListRowCurr = item.CurrCode;
		return true;
	}

	public static decimal CalcVATAmountByType(decimal sum, string vatType, bool itemVAT)
	{
		return ArmSoft.SME.Trade.Common.Helpers.PriceHelper.CalcVATAmountByType(sum, vatType, itemVAT, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
	}

	public static decimal CalcSumDependOnVATType(decimal sum, string vatType, string rowVatType, bool itemVAT)
	{
		return ArmSoft.SME.Trade.Common.Helpers.PriceHelper.CalcSumDependOnVATType(sum, vatType, rowVatType, itemVAT, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
	}

	public static decimal GetPriceCalcForUnitByPriceList(string priceListType, string vatType, string mtCode, string unit, DateOnly date, string curr)
	{
		decimal result = 0m;
		if (!string.IsNullOrWhiteSpace(priceListType))
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
			if (item != null)
			{
				PricesDataProvider.DataRow priceCalculatedForGivenOrBaseUnit = GetPriceCalculatedForGivenOrBaseUnit(priceListType, mtCode, unit, date, curr);
				if (priceCalculatedForGivenOrBaseUnit != null)
				{
					if (priceCalculatedForGivenOrBaseUnit.QntUnit != unit)
					{
						priceCalculatedForGivenOrBaseUnit.Price = GetPriceForUnit(priceCalculatedForGivenOrBaseUnit.QntUnit, unit, priceCalculatedForGivenOrBaseUnit.Price, mtCode);
						priceCalculatedForGivenOrBaseUnit.QntUnit = unit;
					}
					GetPriceListRowVATType(priceListType, out var priceListRowVATType, out var _);
					return CalcSumDependOnVATType(priceCalculatedForGivenOrBaseUnit.Price, vatType, priceListRowVATType, item.VAT);
				}
			}
		}
		return result;
	}

	public static PricesDataProvider.DataRow GetPriceCalculatedForGivenOrBaseUnit(string priceType, string mtCode, string unit, DateOnly date, string curr = "")
	{
		PricesDataProvider.DataRow result = null;
		List<PricesDataProvider.DataRow> list = new List<PricesDataProvider.DataRow>();
		decimal num = 1m;
		decimal num2 = 1m;
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
		if (item == null)
		{
			return result;
		}
		PricesDataProvider.Param param = new PricesDataProvider.Param
		{
			EndDate = date,
			MTCode = mtCode,
			PriceType = priceType
		};
		List<PricesDataProvider.DataRow> rows = new PricesDataProvider(TradeSettings.CreateTradeClient()).GetRows(param, withoutAccessCheck: true);
		if (rows.Count == 0)
		{
			return result;
		}
		DateOnly maxDate = rows.Max((PricesDataProvider.DataRow dataRow) => dataRow.Date);
		list = rows.Where((PricesDataProvider.DataRow dataRow) => dataRow.Date == maxDate).ToList();
		if (list.Count == 0)
		{
			return result;
		}
		if (list.Count == 1)
		{
			result = list[0];
		}
		else
		{
			result = list.FirstOrDefault((PricesDataProvider.DataRow dataRow) => dataRow.QntUnit == unit);
			if (result == null)
			{
				foreach (PricesDataProvider.DataRow item2 in list)
				{
					if (item2.QntUnit == item.Unit)
					{
						result = item2;
						break;
					}
					if (item2.QntUnit == item.AltUnit)
					{
						result = item2;
					}
				}
			}
		}
		if (result != null && !string.IsNullOrWhiteSpace(curr) && curr != result.CurrCode)
		{
			if (curr == "AMD")
			{
				if (result.CurrCode != "AMD")
				{
					ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(result.CurrCode, date);
					num = exchangeRates.Rate;
					num2 = exchangeRates.Base;
					if (num2 == 0m)
					{
						throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ValidateCurCount} \r\n {result.CurrCode} - {date.FormatDDMMYY()}");
					}
					result.Price = (num * result.Price / num2).RoundAs(2);
				}
			}
			else
			{
				decimal num3 = 1m;
				decimal num4 = 1m;
				ExchangeRate exchangeRates2 = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(result.CurrCode, date);
				num3 = exchangeRates2.Rate;
				num4 = exchangeRates2.Base;
				if (num4 == 0m)
				{
					throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ValidateCurCount} \r\n {curr} - {date.FormatDDMMYY()}");
				}
				if (result.CurrCode != "AMD")
				{
					ExchangeRate exchangeRates3 = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(result.CurrCode, date);
					num = exchangeRates3.Rate;
					num2 = exchangeRates3.Base;
					if (num2 == 0m)
					{
						throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ValidateCurCount} \r\n {result.CurrCode} - {date.FormatDDMMYY()}");
					}
					result.Price = (num * result.Price / num2 / (num3 * num4)).RoundAs(2);
				}
				else
				{
					result.Price = (result.Price / (num3 * num4)).RoundAs(2);
				}
			}
		}
		return result;
	}

	public static decimal GetPriceForGivenPriceListAndVATType(string priceListType, string vatType, int mtId, string unit, DateOnly date, string currency)
	{
		return TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(priceListType, vatType, mtId, unit, date, currency);
	}

	public static decimal GetPriceForGivenPriceListAndVATTypeFromCache(string priceListType, string vatType, string mtCode, string unit)
	{
		decimal number = 0m;
		PriceTypeCacheRow item = TradeSettings.TradeCacheService.PriceType.GetItem(priceListType);
		if (item != null)
		{
			MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
			if (item2 != null)
			{
				string rowVatType = (item.VAT ? "2" : "1");
				CurrentPricesCacheRow item3 = TradeSettings.TradeCacheService.CurrentPrices.GetItem(new CurrentPricesCacheRow.RowKey
				{
					MTID = item2.Id,
					Unit = unit,
					PriceType = priceListType
				});
				if (item3 != null)
				{
					number = CalcSumDependOnVATType(item3.Price, vatType, rowVatType, item2.VAT);
				}
			}
		}
		return number.RoundAs(2);
	}

	public static decimal GetPriceCalcForUnitByPriceListFromCache(string priceListType, string vatType, string mtCode, string unit, DateOnly date, string curr)
	{
		decimal result = 0m;
		if (!string.IsNullOrWhiteSpace(priceListType))
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
			if (item != null)
			{
				CurrentPricesCacheRow priceCalculatedForGivenOrBaseUnitFromCache = GetPriceCalculatedForGivenOrBaseUnitFromCache(priceListType, mtCode, unit, date, curr);
				if (priceCalculatedForGivenOrBaseUnitFromCache != null)
				{
					if (priceCalculatedForGivenOrBaseUnitFromCache.Unit != unit)
					{
						priceCalculatedForGivenOrBaseUnitFromCache.Price = GetPriceForUnit(priceCalculatedForGivenOrBaseUnitFromCache.Unit, unit, priceCalculatedForGivenOrBaseUnitFromCache.Price, mtCode);
						priceCalculatedForGivenOrBaseUnitFromCache.Unit = unit;
					}
					GetPriceListRowVATType(priceListType, out var priceListRowVATType, out var _);
					return CalcSumDependOnVATType(priceCalculatedForGivenOrBaseUnitFromCache.Price, vatType, priceListRowVATType, item.VAT);
				}
			}
		}
		return result;
	}

	public static CurrentPricesCacheRow GetPriceCalculatedForGivenOrBaseUnitFromCache(string priceType, string mtCode, string unit, DateOnly date, string curr = "")
	{
		CurrentPricesCacheRow result = null;
		decimal num = 1m;
		decimal num2 = 1m;
		MaterialCacheRow materialCacheRow = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
		PriceTypeCacheRow item = TradeSettings.TradeCacheService.PriceType.GetItem(priceType);
		if (materialCacheRow == null || item == null)
		{
			return result;
		}
		IReadOnlyList<CurrentPricesCacheRow> list = TradeSettings.TradeCacheService.CurrentPrices.GetList((CurrentPricesCacheRow p) => p.MTID == materialCacheRow.Id && p.PriceType == priceType);
		if (list.Count == 0)
		{
			return result;
		}
		if (list.Count == 1)
		{
			result = list[0];
		}
		else
		{
			result = list.FirstOrDefault((CurrentPricesCacheRow currentPricesCacheRow) => currentPricesCacheRow.Unit == unit);
			if (result == null)
			{
				foreach (CurrentPricesCacheRow item2 in list)
				{
					if (item2.Unit == materialCacheRow.Unit)
					{
						result = item2;
						break;
					}
					if (item2.Unit == materialCacheRow.AltUnit)
					{
						result = item2;
					}
				}
			}
		}
		if (result != null && !string.IsNullOrWhiteSpace(curr) && curr != item.CurrCode)
		{
			if (curr == "AMD")
			{
				if (item.CurrCode != "AMD")
				{
					if (Settings.IsOffline)
					{
						CurrentExchangeRatesCacheRow currentExchangeRatesCacheRow = Settings.CoreCacheService.CurrentExchangeRates.GetItem(item.CurrCode) ?? throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ValidateCurCount} \r\n {item.CurrCode} - {date.FormatDDMMYY()}");
						num = currentExchangeRatesCacheRow.Rate;
						num2 = currentExchangeRatesCacheRow.Base;
					}
					else
					{
						ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(item.CurrCode, date);
						num = exchangeRates.Rate;
						num2 = exchangeRates.Base;
						if (num2 == 0m)
						{
							throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ValidateCurCount} \r\n {item.CurrCode} - {date.FormatDDMMYY()}");
						}
					}
					result.Price = (num * result.Price / num2).RoundAs(2);
				}
			}
			else
			{
				if (Settings.IsOffline)
				{
					CurrentExchangeRatesCacheRow currentExchangeRatesCacheRow2 = Settings.CoreCacheService.CurrentExchangeRates.GetItem(item.CurrCode) ?? throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ValidateCurCount} \r\n {item.CurrCode} - {date.FormatDDMMYY()}");
					num = currentExchangeRatesCacheRow2.Rate;
					num2 = currentExchangeRatesCacheRow2.Base;
				}
				else
				{
					ExchangeRate exchangeRates2 = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(item.CurrCode, date);
					num = exchangeRates2.Rate;
					num2 = exchangeRates2.Base;
					if (num2 == 0m)
					{
						throw new ASException($"{ArmSoft.SME.Trade.Common.Resources.ValidateCurCount} \r\n {item.CurrCode} - {date.FormatDDMMYY()}");
					}
				}
				if (item.CurrCode != "AMD")
				{
					result.Price = (num * result.Price / num2 / (num * num2)).RoundAs(2);
				}
				else
				{
					result.Price = (result.Price / (num * num2)).RoundAs(2);
				}
			}
		}
		return result;
	}

	public static Dictionary<string, string> GetPriceTypesForPriceListColumns(string currCode = "")
	{
		string empty = string.Empty;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		IEnumerable<PriceTypesDataProvider.DataRow> enumerable = from row in new PriceTypesDataProvider(TradeSettings.CreateTradeClient()).GetRows(new PriceTypesDataProvider.Param())
			select (row);
		if (enumerable.Any())
		{
			foreach (PriceTypesDataProvider.DataRow item in enumerable)
			{
				empty = $"{item.Caption} ({item.CurrCode}) {(item.VAT ? ArmSoft.SME.Common.Resources.CostWithInVAT : ArmSoft.SME.Common.Resources.CostWithoutVAT)}";
				dictionary.Add(item.Code, empty);
			}
		}
		return dictionary;
	}

	public static decimal CalculateExtraPercent(decimal price, decimal salePrice, bool rowVAT, string vatType, string currCode, ExchangeRate currExChngRate)
	{
		decimal num = (rowVAT ? Settings.ParametersService.VAT() : 0m);
		if (price == 0m)
		{
			return 999.99m;
		}
		if (currCode != "AMD" && currExChngRate.Base != 0m)
		{
			price = (price * currExChngRate.Rate / currExChngRate.Base).RoundAs(2);
		}
		if (vatType == "1")
		{
			price += (price * num / 100m).RoundAs(2);
		}
		decimal num2 = 0m;
		if (price != 0m)
		{
			num2 = ((salePrice - price) * 100m / price).RoundAs(2);
		}
		if (num2 > 999.99m)
		{
			num2 = 999.99m;
		}
		return num2;
	}

	public static void ProcessPriceAdding(MTIncomeBaseWindow<ArmSoft.SME.Trade.Common.Models.Documents.MTIncomeRetail>.MTIncomeSpecificationRow row, DateOnly date, string priceType, string priceTypeVATType, string currCode, string comment, decimal constVAT, string vatType)
	{
		try
		{
			Price price = Settings.DirectoryService.Create<Price>(DirectoryTypeSE.Price.Value);
			price.Code = row.MTCode;
			price.Id = row.MTID;
			price.QntUnit = row.UnitCode;
			price.Curr = currCode;
			price.PriceType = priceType;
			price.PriceTypeOld = priceType;
			price.Comment = comment;
			price.ItemPrice = FormPriceToSave(row, priceTypeVATType, constVAT, vatType);
			price.PriceDate = date;
			Settings.DirectoryService.Store(price, new DirStoreRequest
			{
				WithoutValidation = true
			});
		}
		catch
		{
			throw;
		}
	}

	public static void ProcessPriceEditing(MTIncomeBaseWindow<ArmSoft.SME.Trade.Common.Models.Documents.MTIncomeRetail>.MTIncomeSpecificationRow row, DateOnly date, string priceType, string priceTypeVATType, string comment, decimal constVAT, string vatType)
	{
		try
		{
			PricesDataProvider.DataRow.PricesRowKey value = new PricesDataProvider.DataRow.PricesRowKey
			{
				MTID = row.MTID,
				PriceType = priceType,
				QntUnit = row.UnitCode,
				Date = date
			};
			Price price = Settings.DirectoryService.Load<Price>(DirectoryTypeSE.Price.Value, JsonSerializer.Serialize(value));
			price.Code = row.MTCode;
			price.GoodCodeOld = row.MTCode;
			price.QntUnit = row.UnitCode;
			price.UnitCodeOld = row.UnitCode;
			price.PriceType = priceType;
			price.PriceTypeOld = priceType;
			price.Comment = comment;
			price.ItemPrice = FormPriceToSave(row, priceTypeVATType, constVAT, vatType);
			price.PriceDate = date;
			price.PriceDateOld = date;
			Settings.DirectoryService.Store(price, new DirStoreRequest
			{
				WithoutValidation = false
			});
		}
		catch
		{
			throw;
		}
	}

	public static decimal FormPriceToSave(MTIncomeBaseWindow<ArmSoft.SME.Trade.Common.Models.Documents.MTIncomeRetail>.MTIncomeSpecificationRow row, string _priceTypeVATType, decimal constVAT, string vatType)
	{
		if (!(vatType == "1") && !(vatType == "2") && _priceTypeVATType != "2" && row.VAT)
		{
			decimal num = (row.SalePrice * constVAT / (100m + constVAT)).RoundAs(2);
			return row.SalePrice - num;
		}
		return row.SalePrice;
	}

	public static bool PriceAlreadyExists(MTIncomeBaseWindow<ArmSoft.SME.Trade.Common.Models.Documents.MTIncomeRetail>.MTIncomeSpecificationRow row, List<PricesDataProvider.DataRow> prices)
	{
		if (prices.FirstOrDefault((PricesDataProvider.DataRow x) => x.MTID == row.MTID && x.QntUnit == row.UnitCode) != null)
		{
			return true;
		}
		return false;
	}

	public static decimal CalculateDiscountPrice(decimal price, decimal discount)
	{
		return (price - price * discount / 100m).RoundAs(2);
	}

	public static decimal CalculateSum(decimal price, decimal quantity)
	{
		return (price * quantity).RoundAs(2);
	}

	public static decimal CalculatePrice(decimal summ, decimal quantity)
	{
		return (summ / quantity).RoundAs(2);
	}
}
