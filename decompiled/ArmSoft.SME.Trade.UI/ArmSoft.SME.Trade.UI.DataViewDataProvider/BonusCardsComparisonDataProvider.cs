using System.Collections.Generic;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.Services;

namespace ArmSoft.SME.Trade.UI.DataViewDataProvider;

public class BonusCardsComparisonDataProvider : DataProvider<BonusCardsComparisonDataProvider.DataRow, BonusCardsComparisonDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey
	{
		public string CardNumber { get; set; }

		public string CardHolder { get; set; }

		public CommonConst.b2bComparisonResult ComparisonResult { get; set; }

		public string ComparisonDescription { get; set; }

		public bool Matched(object key)
		{
			return (string)key == CardNumber;
		}
	}

	public class Param
	{
		public string CardNumbers { get; set; }
	}

	private readonly string _prefix;

	private readonly string _suffix;

	public override string Name => "BonusCardsComparison";

	public BonusCardsComparisonDataProvider()
		: base((ApiClient)null)
	{
		_prefix = MCRInfo.GetInstance().Prefix;
		_suffix = MCRInfo.GetInstance().Suffix;
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.CompareWithDiscountServer
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "CardNumber",
				Caption = ArmSoft.SME.Trade.Common.Resources.DiscountCardCode,
				Source = "CardNumber",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "CardHolder",
				Caption = ArmSoft.SME.Trade.Common.Resources.CardHolder,
				Source = "CardHolder",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "ComparisonDescription",
				Caption = ArmSoft.SME.Common.Resources.Description,
				Source = "ComparisonDescription",
				Type = SupportedType.String
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		List<BonusCardsDataProvider.DataRow> bonusCardsList = DiscountServerService.GetBonusCardsList();
		string[] array = param.CardNumbers.Split(',');
		foreach (string cardNumber in array)
		{
			BonusCardsDataProvider.DataRow bonusCardRow = bonusCardsList.Find((BonusCardsDataProvider.DataRow x) => x.CardNumber == cardNumber);
			PartnerCacheRow.ContractRow contractByDiscountCard = TradeSettings.TradeCacheService.Partner.GetContractByDiscountCard(cardNumber, out var partnerCacheRow);
			if (contractByDiscountCard != null)
			{
				dataProviderResult.Rows.Add(GetComparisonResultRow(bonusCardRow, partnerCacheRow, contractByDiscountCard));
			}
		}
		return dataProviderResult;
	}

	private DataRow GetComparisonResultRow(BonusCardsDataProvider.DataRow bonusCardRow, PartnerCacheRow partnerCacheRow, PartnerCacheRow.ContractRow partnerContractRow)
	{
		DataRow dataRow = new DataRow
		{
			CardNumber = partnerContractRow.DiscountCard,
			CardHolder = partnerCacheRow.Caption
		};
		if (bonusCardRow == null)
		{
			dataRow.ComparisonResult = CommonConst.b2bComparisonResult.NotFoundInB2B;
			dataRow.ComparisonDescription = ArmSoft.SME.Trade.Common.Resources.BonusCardNotFoundInDiscountServer;
		}
		else if (bonusCardRow.Barcode != _prefix + partnerContractRow.DiscountCard + _suffix)
		{
			dataRow.ComparisonResult = CommonConst.b2bComparisonResult.NotEqual;
			dataRow.ComparisonDescription = string.Format(ArmSoft.SME.Common.Resources.b2bProductComparisonNotEqual, ArmSoft.SME.Common.Resources.BarCode);
		}
		else if (bonusCardRow.BonusPercent != partnerContractRow.Bonus)
		{
			dataRow.ComparisonResult = CommonConst.b2bComparisonResult.NotEqual;
			dataRow.ComparisonDescription = string.Format(ArmSoft.SME.Common.Resources.b2bProductComparisonNotEqual, ArmSoft.SME.Common.Resources.Percent);
		}
		else if (bonusCardRow.CardHolder != partnerCacheRow.Caption)
		{
			dataRow.ComparisonResult = CommonConst.b2bComparisonResult.NotEqual;
			dataRow.ComparisonDescription = string.Format(ArmSoft.SME.Common.Resources.b2bProductComparisonNotEqual, ArmSoft.SME.Trade.Common.Resources.CardHolder);
		}
		else if (bonusCardRow.Canceled != partnerContractRow.BonusCardCanceled)
		{
			dataRow.ComparisonResult = CommonConst.b2bComparisonResult.NotEqual;
			dataRow.ComparisonDescription = string.Format(ArmSoft.SME.Common.Resources.b2bProductComparisonNotEqual, ArmSoft.SME.Trade.Common.Resources.BonusCardCanceled);
		}
		else if ((bonusCardRow.ValidPeriodStart.HasValue && !partnerContractRow.BonusCardValidPeriodStart.HasValue) || (!bonusCardRow.ValidPeriodStart.HasValue && partnerContractRow.BonusCardValidPeriodStart.HasValue) || (bonusCardRow.ValidPeriodStart.HasValue && partnerContractRow.BonusCardValidPeriodStart.HasValue && bonusCardRow.ValidPeriodStart.Value != partnerContractRow.BonusCardValidPeriodStart.Value))
		{
			dataRow.ComparisonResult = CommonConst.b2bComparisonResult.NotEqual;
			dataRow.ComparisonDescription = string.Format(ArmSoft.SME.Common.Resources.b2bProductComparisonNotEqual, ArmSoft.SME.Trade.Common.Resources.BonusCardValidationPeriod + " - " + ArmSoft.SME.Common.Resources.BeginOfPeriodShort);
		}
		else if ((bonusCardRow.ValidPeriodEnd.HasValue && !partnerContractRow.BonusCardValidPeriodEnd.HasValue) || (!bonusCardRow.ValidPeriodEnd.HasValue && partnerContractRow.BonusCardValidPeriodEnd.HasValue) || (bonusCardRow.ValidPeriodEnd.HasValue && partnerContractRow.BonusCardValidPeriodEnd.HasValue && bonusCardRow.ValidPeriodEnd.Value != partnerContractRow.BonusCardValidPeriodEnd.Value))
		{
			dataRow.ComparisonResult = CommonConst.b2bComparisonResult.NotEqual;
			dataRow.ComparisonDescription = string.Format(ArmSoft.SME.Common.Resources.b2bProductComparisonNotEqual, ArmSoft.SME.Trade.Common.Resources.BonusCardValidationPeriod + " - " + ArmSoft.SME.Common.Resources.EndOfPeriodShort);
		}
		else
		{
			PartnerCacheRow itemById = TradeSettings.TradeCacheService.Partner.GetItemById(partnerCacheRow.Id);
			if (itemById != null)
			{
				if (bonusCardRow.Gender != itemById.Sex)
				{
					dataRow.ComparisonResult = CommonConst.b2bComparisonResult.NotEqual;
					dataRow.ComparisonDescription = string.Format(ArmSoft.SME.Common.Resources.b2bProductComparisonNotEqual, ArmSoft.SME.Common.Resources.Sex);
				}
				else if (bonusCardRow.PhoneNumber != itemById.Phone)
				{
					dataRow.ComparisonResult = CommonConst.b2bComparisonResult.NotEqual;
					dataRow.ComparisonDescription = string.Format(ArmSoft.SME.Common.Resources.b2bProductComparisonNotEqual, ArmSoft.SME.Common.Resources.PhoneNumber);
				}
				else if (bonusCardRow.Email != itemById.Email)
				{
					dataRow.ComparisonResult = CommonConst.b2bComparisonResult.NotEqual;
					dataRow.ComparisonDescription = string.Format(ArmSoft.SME.Common.Resources.b2bProductComparisonNotEqual, ArmSoft.SME.Common.Resources.EMailShort);
				}
				else if ((bonusCardRow.BirthDate.HasValue && !itemById.Birthday.HasValue) || (!bonusCardRow.BirthDate.HasValue && itemById.Birthday.HasValue) || (bonusCardRow.BirthDate.HasValue && itemById.Birthday.HasValue && bonusCardRow.BirthDate.Value != itemById.Birthday.Value))
				{
					dataRow.ComparisonResult = CommonConst.b2bComparisonResult.NotEqual;
					dataRow.ComparisonDescription = string.Format(ArmSoft.SME.Common.Resources.b2bProductComparisonNotEqual, ArmSoft.SME.Common.Resources.DateOfBirth);
				}
				else
				{
					dataRow.ComparisonResult = CommonConst.b2bComparisonResult.Equal;
					dataRow.ComparisonDescription = ArmSoft.SME.Common.Resources.Matched;
				}
			}
			else
			{
				dataRow.ComparisonResult = CommonConst.b2bComparisonResult.NotEqual;
				dataRow.ComparisonDescription = string.Format(ArmSoft.SME.Common.Resources.b2bProductComparisonNotEqual, ArmSoft.SME.Common.Resources.Partner);
			}
		}
		return dataRow;
	}
}
