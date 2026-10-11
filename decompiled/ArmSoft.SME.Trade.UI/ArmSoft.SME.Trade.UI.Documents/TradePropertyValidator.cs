using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;
using DevExpress.DataAccess.Native.ExpressionEditor;
using DevExpress.XtraEditors.DXErrorProvider;

namespace ArmSoft.SME.Trade.UI.Documents;

public class TradePropertyValidator : PropertyValidator
{
	public static void ValidateMaterialCode(string value, ErrorInfo info, FieldDefinition definition)
	{
		ValidateGoodCode(value, info, definition);
	}

	public static void ValidateServiceCode(string value, ErrorInfo info, FieldDefinition definition)
	{
		ValidateGoodCode(value, info, definition, verifyingOnlyGoods: false, verifyingOnlyService: true);
	}

	public static void ValidateGoodCode(string value, ErrorInfo info, FieldDefinition definition, bool verifyingOnlyGoods = true, bool verifyingOnlyService = false)
	{
		ValidateGoodCode(value, info, definition.Caption, verifyingOnlyGoods, verifyingOnlyService, definition.IsRequired);
	}

	public static void ValidateGoodCode(string value, ErrorInfo info, string caption, bool verifyingOnlyGoods = true, bool verifyingOnlyService = false, bool isRequired = true)
	{
		if (isRequired)
		{
			PropertyValidator.EmptyStringValidate(value, info, caption);
			if (info.ErrorType == ErrorType.Critical)
			{
				return;
			}
		}
		if (!string.IsNullOrWhiteSpace(value))
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(value);
			if (item == null)
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, value);
			}
			else if (verifyingOnlyGoods && item.Type == ItemTypesSE.Services.Value)
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.NotInMaterials, value);
			}
			else if (verifyingOnlyService && item.Type == ItemTypesSE.Goods.Value)
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.NotInService, value);
			}
		}
	}

	public static void ValidateUnitCode(string value, ErrorInfo info, string caption, bool isRequired = true)
	{
		if (isRequired)
		{
			PropertyValidator.EmptyStringValidate(value, info, caption);
			if (info.ErrorType == ErrorType.Critical)
			{
				return;
			}
		}
		if (!string.IsNullOrWhiteSpace(value) && TradeSettings.TradeCacheService.QntUnit.GetItem(value) == null)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.ValueDoesNotExistInTheListWithName, ArmSoft.SME.Trade.Common.Resources.QntUnit, value, ArmSoft.SME.Common.Resources.MeasureUnits);
		}
	}

	public static void ValidateGoodUnitCode(string value, string goodCode, ErrorInfo info, FieldDefinition definition)
	{
		ValidateGoodUnitCode(value, goodCode, info, definition.Caption, definition.IsRequired);
	}

	public static void ValidateGoodUnitCode(string value, string goodCode, ErrorInfo info, string caption, bool isRequired = true)
	{
		ValidateUnitCode(value, info, caption, isRequired);
		if (info.ErrorType != ErrorType.Critical && TradeSettings.TradeCacheService.Material.GetQntUnit(goodCode, value) == null)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.InvalidMTQntUnit, goodCode, value);
		}
	}

	public static void ValidateStorage(string value, ErrorInfo info, FieldDefinition definition)
	{
		ValidateStorage(value, info, definition.Caption, definition.IsRequired);
	}

	public static void ValidateStorage(string value, ErrorInfo info, string caption, bool isRequired)
	{
		if (isRequired)
		{
			PropertyValidator.EmptyStringValidate(value, info, caption);
			if (info.ErrorType == ErrorType.Critical)
			{
				return;
			}
		}
		if (!string.IsNullOrWhiteSpace(value))
		{
			if (TradeSettings.TradeCacheService.Storage.GetItem(value) == null)
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotExist, value);
			}
			else if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(value))
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, value);
			}
		}
	}

	public static void ValidatePartner(string value, ErrorInfo info, FieldDefinition definition)
	{
		ValidatePartner(value, info, definition.Caption, definition.IsRequired);
	}

	public static void ValidatePartner(string value, ErrorInfo info, string caption, bool isRequired)
	{
		if (isRequired)
		{
			PropertyValidator.EmptyStringValidate(value, info, caption);
			if (info.ErrorType == ErrorType.Critical)
			{
				return;
			}
		}
		if (!string.IsNullOrWhiteSpace(value) && TradeSettings.TradeCacheService.Partner.GetItem(value) == null)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.PartnerCodeIsNotExist, value);
		}
	}

	public static void ValidateContract(string partCode, string contract, ErrorInfo info, FieldDefinition definition, bool isBonusCard = false)
	{
		ValidateContract(partCode, contract, info, definition.Caption, definition.IsRequired, isBonusCard);
	}

	public static void ValidateContract(string partCode, string contract, ErrorInfo info, string caption, bool isRequired, bool isBonusCard = false)
	{
		if (isRequired)
		{
			PropertyValidator.EmptyStringValidate(contract, info, caption);
			if (info.ErrorType == ErrorType.Critical)
			{
				return;
			}
		}
		if (string.IsNullOrWhiteSpace(contract))
		{
			return;
		}
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(partCode);
		if (item == null)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.PartnerCodeIsNotExist, item);
			return;
		}
		PartnerCacheRow.ContractRow contract2 = TradeSettings.TradeCacheService.Partner.GetContract(item, contract);
		if (contract2 == null)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.ErrContractNotExists, contract, item.PartCode);
		}
		else if (isBonusCard && !contract2.IsBonusCard)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.ContractIsNotBonusCard, contract, item);
		}
	}

	public static void ValidateCountry(string value, ErrorInfo info, FieldDefinition definition)
	{
		ValidateCountry(value, info, definition.Caption, definition.IsRequired);
	}

	public static void ValidateCountry(string value, ErrorInfo info, string caption, bool isRequired)
	{
		if (isRequired)
		{
			PropertyValidator.EmptyStringValidate(value, info, caption);
			if (info.ErrorType == ErrorType.Critical)
			{
				return;
			}
		}
		if (!string.IsNullOrWhiteSpace(value) && TradeSettings.TradeCacheService.Country.GetItem(value) == null)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.NotInCountry, value);
		}
	}

	public static void ValidateProducer(string value, ErrorInfo info, FieldDefinition definition)
	{
		ValidateProducer(value, info, definition.Caption, definition.IsRequired);
	}

	public static void ValidateProducer(string value, ErrorInfo info, string caption, bool isRequired)
	{
		if (isRequired)
		{
			PropertyValidator.EmptyStringValidate(value, info, caption);
			if (info.ErrorType == ErrorType.Critical)
			{
				return;
			}
		}
		if (!string.IsNullOrWhiteSpace(value) && TradeSettings.TradeCacheService.Producer.GetItem(value) == null)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.ValueDoesNotExistInTheListWithName, ArmSoft.SME.Common.Resources.Producer, value, ArmSoft.SME.Common.Resources.Producers);
		}
	}

	public static void ValidateProperty(string value, string goodsCode, bool isProperty1, ErrorInfo info, FieldDefinition definition)
	{
		ValidateProperty(value, goodsCode, isProperty1, info, definition.Caption, definition.IsRequired);
	}

	public static void ValidateProperty(string value, string goodsCode, bool isProperty1, ErrorInfo info, string caption, bool isRequired)
	{
		if (isRequired)
		{
			PropertyValidator.EmptyStringValidate(value, info, caption);
			if (info.ErrorType == ErrorType.Critical)
			{
				return;
			}
		}
		if (string.IsNullOrWhiteSpace(value))
		{
			return;
		}
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(goodsCode);
		if (item == null)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, goodsCode);
			return;
		}
		PropertyListCacheRow.RowKey key = new PropertyListCacheRow.RowKey
		{
			DestCode = (isProperty1 ? item.Property1 : item.Property2),
			Code = value
		};
		if (TradeSettings.TradeCacheService.PropertyList.GetItem(key) == null)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.ValueDoesNotExistInTheListWithName, isProperty1 ? ArmSoft.SME.Trade.Common.Resources.Attribute1 : ArmSoft.SME.Trade.Common.Resources.Attribute2, value, ArmSoft.SME.Trade.Common.Resources.PropertiesListsValues);
		}
	}

	public static void ValidatePropertyValue(string value, string goodsCode, bool isProperty1, ErrorInfo info, FieldDefinition definition)
	{
		ValidatePropertyValue(value, goodsCode, isProperty1, info, definition.Caption, definition.IsRequired);
	}

	public static void ValidatePropertyValue(string value, string goodsCode, bool isProperty1, ErrorInfo info, string caption, bool isRequired)
	{
		if (isRequired)
		{
			PropertyValidator.EmptyStringValidate(value, info, caption);
			if (info.ErrorType == ErrorType.Critical)
			{
				return;
			}
		}
		if (!string.IsNullOrWhiteSpace(value))
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(goodsCode);
			if (item == null)
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, goodsCode);
			}
			else if (TradeSettings.TradeCacheService.PropertyList.GetPropertyByCaption(value, isProperty1 ? item.Property1 : item.Property2) == null)
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.PropertyValueDoesNotExist, isProperty1 ? item.Property1 : item.Property2, value);
			}
		}
	}

	public static void ValidateCurr(string value, ErrorInfo info, FieldDefinition definition)
	{
		ValidateCurr(value, info, definition.Caption, definition.IsRequired);
	}

	public static void ValidateCurr(string value, ErrorInfo info, string caption, bool isRequired)
	{
		if (isRequired)
		{
			PropertyValidator.EmptyStringValidate(value, info, caption);
			if (info.ErrorType == ErrorType.Critical)
			{
				return;
			}
		}
		if (!string.IsNullOrWhiteSpace(value) && Settings.CoreCacheService.Currency.GetItem(value) == null)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(ArmSoft.SME.Common.Resources.ValidateEntriesRowInvalidCurrCode, value);
		}
	}

	public static void ValidatePriceType(string value, ErrorInfo info, FieldDefinition definition)
	{
		ValidatePriceType(value, info, definition.Caption, definition.IsRequired);
	}

	public static void ValidatePriceType(string value, ErrorInfo info, string caption, bool isRequired)
	{
		if (isRequired)
		{
			PropertyValidator.EmptyStringValidate(value, info, caption);
			if (info.ErrorType == ErrorType.Critical)
			{
				return;
			}
		}
		if (!string.IsNullOrWhiteSpace(value) && TradeSettings.TradeCacheService.PriceType.GetItem(value) == null)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(ArmSoft.SME.Trade.Common.Resources.ValidatePartnerRowInvalidPriceType, value);
		}
	}

	internal static void ValidateMaterialCode(string itemCode, ExpressionEditorController.ErrorInfo info, FieldDefinition fieldDefinition)
	{
		throw new NotImplementedException();
	}
}
