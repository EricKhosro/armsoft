using System;
using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Other;
using ArmSoft.SME.Trade.UI.Services;

namespace ArmSoft.SME.Trade.UI.DataViewDataProvider;

public class BonusSumsComparisonDataProvider : DataProvider<BonusSumsComparisonDataProvider.DataRow, BonusSumsComparisonDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string BonusCardNumber { get; set; } = "";

		public decimal DatabaseAccumulatedBonusSum { get; set; }

		public decimal DiscountServerAccumulatedBonusSum { get; set; }

		public decimal AccumulatedBonusDifference
		{
			get
			{
				return DatabaseAccumulatedBonusSum - DiscountServerAccumulatedBonusSum;
			}
			set
			{
			}
		}

		public decimal DatabaseUsedBonusSum { get; set; }

		public decimal DiscountServerUsedBonusSum { get; set; }

		public decimal UsedBonusDifference
		{
			get
			{
				return DatabaseUsedBonusSum - DiscountServerUsedBonusSum;
			}
			set
			{
			}
		}

		public decimal DatabaseInitialBonusRemain { get; set; }

		public decimal DatabaseFinalBonusRemain { get; set; }

		public decimal DiscountServerInitialBonusRemain { get; set; }

		public decimal DiscountServerFinalBonusRemain { get; set; }

		public int PartnerID { get; set; }

		public string PartnerCode { get; set; } = "";

		public string PartnerName { get; set; } = "";

		public string Phone { get; set; } = "";

		public string Address { get; set; } = "";

		public bool IsBonusContract { get; set; }

		public string ContractCode { get; set; } = "";
	}

	public class Param
	{
		public DateOnly? StartDate { get; set; }

		public DateOnly? EndDate { get; set; }

		public string CardNumber { get; set; } = "";

		public string CashDesk { get; set; } = "";

		public string ShiftNumber { get; set; } = "";
	}

	public override string Name => "BonusCardsComparison";

	public BonusSumsComparisonDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.DiscountServerBonusSumComparison
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "BonusCardNumber",
				Caption = ArmSoft.SME.Trade.Common.Resources.DiscountCardCode,
				Source = "BonusCardNumber",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "DatabaseInitialBonusRemain",
				Caption = ArmSoft.SME.Trade.Common.Resources.DatabaseShort + " - " + ArmSoft.SME.Trade.Common.Resources.InitRem,
				Source = "DatabaseInitialBonusRemain",
				Type = SupportedType.Decimal,
				FooterSum = true
			},
			new Column
			{
				Name = "DiscountServerInitialBonusRemain",
				Caption = ArmSoft.SME.Trade.Common.Resources.DiscountServer + " - " + ArmSoft.SME.Trade.Common.Resources.InitRem,
				Source = "DiscountServerInitialBonusRemain",
				Type = SupportedType.Decimal,
				FooterSum = true
			},
			new Column
			{
				Name = "DatabaseAccumulatedBonusSum",
				Caption = ArmSoft.SME.Trade.Common.Resources.DatabaseShort + " - " + ArmSoft.SME.Trade.Common.Resources.IncreasedBonus,
				Source = "DatabaseAccumulatedBonusSum",
				Type = SupportedType.Decimal,
				FooterSum = true
			},
			new Column
			{
				Name = "DiscountServerAccumulatedBonusSum",
				Caption = ArmSoft.SME.Trade.Common.Resources.DiscountServer + " - " + ArmSoft.SME.Trade.Common.Resources.IncreasedBonus,
				Source = "DiscountServerAccumulatedBonusSum",
				Type = SupportedType.Decimal,
				FooterSum = true
			},
			new Column
			{
				Name = "AccumulatedBonusDifference",
				Caption = ArmSoft.SME.Trade.Common.Resources.Difference,
				Source = "AccumulatedBonusDifference",
				Type = SupportedType.Decimal,
				FooterSum = true
			},
			new Column
			{
				Name = "DatabaseUsedBonusSum",
				Caption = ArmSoft.SME.Trade.Common.Resources.DatabaseShort + " - " + ArmSoft.SME.Trade.Common.Resources.DecreasedBonus,
				Source = "DatabaseUsedBonusSum",
				Type = SupportedType.Decimal,
				FooterSum = true
			},
			new Column
			{
				Name = "DiscountServerUsedBonusSum",
				Caption = ArmSoft.SME.Trade.Common.Resources.DiscountServer + " - " + ArmSoft.SME.Trade.Common.Resources.DecreasedBonus,
				Source = "DiscountServerUsedBonusSum",
				Type = SupportedType.Decimal,
				FooterSum = true
			},
			new Column
			{
				Name = "UsedBonusDifference",
				Caption = ArmSoft.SME.Trade.Common.Resources.Difference,
				Source = "UsedBonusDifference",
				Type = SupportedType.Decimal,
				FooterSum = true
			},
			new Column
			{
				Name = "DatabaseFinalBonusRemain",
				Caption = ArmSoft.SME.Trade.Common.Resources.DatabaseShort + " - " + ArmSoft.SME.Trade.Common.Resources.EndRem,
				Source = "DatabaseFinalBonusRemain",
				Type = SupportedType.Decimal,
				FooterSum = true
			},
			new Column
			{
				Name = "DiscountServerFinalBonusRemain",
				Caption = ArmSoft.SME.Trade.Common.Resources.DiscountServer + " - " + ArmSoft.SME.Trade.Common.Resources.EndRem,
				Source = "DiscountServerFinalBonusRemain",
				Type = SupportedType.Decimal,
				FooterSum = true
			},
			new Column
			{
				Name = "PartnerCode",
				Caption = ArmSoft.SME.Common.Resources.Partner,
				Source = "PartnerCode",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "ContractCode",
				Caption = ArmSoft.SME.Common.Resources.Contract,
				Source = "ContractCode",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "IsBonusContract",
				Caption = ArmSoft.SME.Trade.Common.Resources.BonusCard,
				Source = "IsBonusContract",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "PartnerName",
				Caption = ArmSoft.SME.Common.Resources.Name,
				Source = "PartnerName",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "Phone",
				Caption = ArmSoft.SME.Common.Resources.PhoneNumber,
				Source = "Phone",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "Address",
				Caption = ArmSoft.SME.Common.Resources.Address,
				Source = "Address",
				Type = SupportedType.String
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		List<BonusTransactionRow> transactionListForComparison = DiscountServerService.GetTransactionListForComparison(param.CardNumber, null, param.EndDate, param.CashDesk, param.ShiftNumber);
		DateOnly dateOnly = ArmSoft.SME.Common.Constants.Common.maxOPDate.ToDateOnly();
		DateOnly startDate = ArmSoft.SME.Common.Constants.Common.minOPDate.ToDateOnly();
		if (param.StartDate.HasValue)
		{
			startDate = param.StartDate.Value;
		}
		if (param.EndDate.HasValue)
		{
			dateOnly = param.EndDate.Value;
		}
		Dictionary<string, BonusRemAndTurnRow> dictionary = new Dictionary<string, BonusRemAndTurnRow>();
		foreach (string item in transactionListForComparison.Select((BonusTransactionRow x) => x.CardNumber).Distinct())
		{
			dictionary.Add(item, new BonusRemAndTurnRow
			{
				CardNumber = item
			});
		}
		if (string.IsNullOrWhiteSpace(param.CashDesk) || string.IsNullOrWhiteSpace(param.ShiftNumber))
		{
			foreach (var item2 in from transaction in transactionListForComparison
				where transaction.TransactionDate < startDate.ToDateTime()
				group transaction by transaction.CardNumber into @group
				select new
				{
					CardNumber = @group.Key,
					Sum = @group.Sum((BonusTransactionRow transaction) => transaction.Sum)
				})
			{
				if (dictionary.TryGetValue(item2.CardNumber, out var value))
				{
					value.InitialRem = item2.Sum;
					value.EndRem = item2.Sum;
				}
			}
		}
		foreach (var item3 in from transaction in transactionListForComparison
			where transaction.TransactionDate >= startDate.ToDateTime()
			group transaction by new { transaction.DBCR, transaction.CardNumber } into @group
			select new
			{
				DBCR = @group.Key.DBCR,
				CardNumber = @group.Key.CardNumber,
				Sum = @group.Sum((BonusTransactionRow transaction) => transaction.Sum)
			})
		{
			if (dictionary.ContainsKey(item3.CardNumber))
			{
				if (item3.DBCR)
				{
					dictionary[item3.CardNumber].DBSum += item3.Sum;
					dictionary[item3.CardNumber].EndRem += item3.Sum;
				}
				else
				{
					dictionary[item3.CardNumber].CRSum += item3.Sum;
					dictionary[item3.CardNumber].EndRem += item3.Sum;
				}
			}
		}
		List<BonusSumsComparisonDataRow> bonusCardsRemAndTurn = TradeSettings.CreateTradeClient().CommonServices.GetBonusCardsRemAndTurn(param.CardNumber);
		if (string.IsNullOrWhiteSpace(param.CashDesk) || string.IsNullOrWhiteSpace(param.ShiftNumber))
		{
			foreach (BonusSumsComparisonDataRow item4 in bonusCardsRemAndTurn)
			{
				DataRow dataRow = new DataRow
				{
					PartnerID = item4.PartnerID,
					PartnerCode = item4.PartnerCode,
					PartnerName = item4.PartnerName,
					IsBonusContract = item4.IsBonusContract,
					BonusCardNumber = item4.BonusCardNumber,
					ContractCode = item4.ContractCode,
					Phone = item4.Phone,
					Address = item4.Address
				};
				SummCurrSummNccPair accRem = TradeSettings.CreateTradeClient().CommonServices.GetAccRem(CommonConst.AccountingsTypes.Bonus, item4.ContractCode, item4.PartnerID, "AMD", dateOnly);
				dataRow.DatabaseFinalBonusRemain = accRem.SummCurr;
				TurnSumms accTurn = TradeSettings.CreateTradeClient().CommonServices.GetAccTurn(CommonConst.AccountingsTypes.Bonus, item4.ContractCode, item4.PartnerID, "AMD", startDate, dateOnly);
				dataRow.DatabaseAccumulatedBonusSum = accTurn.DbTurnCurr;
				dataRow.DatabaseUsedBonusSum = -accTurn.CrTurnCurr;
				dataRow.DatabaseInitialBonusRemain = dataRow.DatabaseFinalBonusRemain + Math.Abs(accTurn.CrTurnCurr) - accTurn.DbTurnCurr;
				if (dictionary.TryGetValue(dataRow.BonusCardNumber, out var value2))
				{
					dataRow.DiscountServerAccumulatedBonusSum = value2.DBSum;
					dataRow.DiscountServerUsedBonusSum = value2.CRSum;
					dataRow.DiscountServerInitialBonusRemain = value2.InitialRem;
					dataRow.DiscountServerFinalBonusRemain = value2.EndRem;
					dictionary.Remove(dataRow.BonusCardNumber);
				}
				dataProviderResult.Rows.Add(dataRow);
			}
		}
		else
		{
			foreach (BonusSumsComparisonDataRow item5 in bonusCardsRemAndTurn)
			{
				DataRow dataRow2 = new DataRow
				{
					PartnerID = item5.PartnerID,
					PartnerCode = item5.PartnerCode,
					PartnerName = item5.PartnerName,
					IsBonusContract = item5.IsBonusContract,
					BonusCardNumber = item5.BonusCardNumber,
					ContractCode = item5.ContractCode,
					Phone = item5.Phone,
					Address = item5.Address
				};
				BonusTurnRow partnerBonusCardTurnByCashierShift = TradeSettings.CreateTradeClient().CommonServices.GetPartnerBonusCardTurnByCashierShift(item5.PartnerID, item5.ContractCode, param.CashDesk, param.ShiftNumber);
				dataRow2.DatabaseAccumulatedBonusSum = partnerBonusCardTurnByCashierShift.DBSum;
				dataRow2.DatabaseUsedBonusSum = -partnerBonusCardTurnByCashierShift.CRSum;
				if (dictionary.TryGetValue(dataRow2.BonusCardNumber, out var value3))
				{
					dataRow2.DiscountServerAccumulatedBonusSum = value3.DBSum;
					dataRow2.DiscountServerUsedBonusSum = value3.CRSum;
					dictionary.Remove(dataRow2.BonusCardNumber);
				}
				dataProviderResult.Rows.Add(dataRow2);
			}
		}
		foreach (KeyValuePair<string, BonusRemAndTurnRow> item6 in dictionary)
		{
			if (string.IsNullOrWhiteSpace(param.CashDesk) || string.IsNullOrWhiteSpace(param.ShiftNumber))
			{
				dataProviderResult.Rows.Add(new DataRow
				{
					DiscountServerAccumulatedBonusSum = item6.Value.DBSum,
					DiscountServerUsedBonusSum = item6.Value.CRSum,
					BonusCardNumber = item6.Key
				});
			}
			else
			{
				dataProviderResult.Rows.Add(new DataRow
				{
					DiscountServerAccumulatedBonusSum = item6.Value.DBSum,
					DiscountServerUsedBonusSum = item6.Value.CRSum,
					DiscountServerInitialBonusRemain = item6.Value.InitialRem,
					DiscountServerFinalBonusRemain = item6.Value.EndRem,
					BonusCardNumber = item6.Key
				});
			}
		}
		return dataProviderResult;
	}
}
