using System;
using System.Collections.Generic;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.Services;

namespace ArmSoft.SME.Trade.UI.DataViewDataProvider;

public class BonusCardsDataProvider : DataProvider<BonusCardsDataProvider.DataRow, BonusCardsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string CardNumber { get; set; } = string.Empty;

		public string Barcode { get; set; } = string.Empty;

		public string CardHolder { get; set; } = string.Empty;

		public string Gender { get; set; } = string.Empty;

		public DateOnly? BirthDate { get; set; }

		public string PhoneNumber { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public decimal BonusPercent { get; set; }

		public decimal Balance { get; set; }

		public decimal PurchaseAmount { get; set; }

		public DateOnly? ValidPeriodStart { get; set; }

		public DateOnly? ValidPeriodEnd { get; set; }

		public bool Canceled { get; set; }

		public DateTime? SyncDate { get; set; }
	}

	public class Param
	{
		public string CardNumber { get; set; }
	}

	public override string Name => "BonusCards";

	public BonusCardsDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.BonusCards
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
				Name = "Barcode",
				Caption = ArmSoft.SME.Common.Resources.BarCode,
				Source = "Barcode",
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
				Name = "Gender",
				Caption = ArmSoft.SME.Common.Resources.Sex,
				Source = "Gender",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "BirthDate",
				Caption = ArmSoft.SME.Common.Resources.DateOfBirth,
				Source = "BirthDate",
				Type = SupportedType.DateOnlyNullable
			},
			new Column
			{
				Name = "PhoneNumber",
				Caption = ArmSoft.SME.Common.Resources.PhoneNumber,
				Source = "PhoneNumber",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "Email",
				Caption = ArmSoft.SME.Common.Resources.EMailShort,
				Source = "Email",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "BonusPercent",
				Caption = ArmSoft.SME.Common.Resources.Percent,
				Source = "BonusPercent",
				Type = SupportedType.Decimal
			},
			new Column
			{
				Name = "Balance",
				Caption = ArmSoft.SME.Trade.Common.Resources.AccumulateBonus,
				Source = "Balance",
				Type = SupportedType.Decimal
			},
			new Column
			{
				Name = "PurchaseAmount",
				Caption = ArmSoft.SME.Trade.Common.Resources.BonusAccumulationBaseAccumulatedSalesAmount,
				Source = "PurchaseAmount",
				Type = SupportedType.Decimal
			},
			new Column
			{
				Name = "ValidPeriodStart",
				Caption = ArmSoft.SME.Trade.Common.Resources.BonusCardValidationPeriod + " - " + ArmSoft.SME.Common.Resources.BeginOfPeriodShort,
				Source = "ValidPeriodStart",
				Type = SupportedType.DateOnlyNullable
			},
			new Column
			{
				Name = "ValidPeriodEnd",
				Caption = ArmSoft.SME.Trade.Common.Resources.BonusCardValidationPeriod + " - " + ArmSoft.SME.Common.Resources.EndOfPeriod,
				Source = "ValidPeriodEnd",
				Type = SupportedType.DateOnlyNullable
			},
			new Column
			{
				Name = "Canceled",
				Caption = ArmSoft.SME.Trade.Common.Resources.BonusCardCanceled,
				Source = "Canceled",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "SyncDate",
				Caption = ArmSoft.SME.Trade.Common.Resources.SyncDate,
				Source = "SyncDate",
				Type = SupportedType.DateTimeNullable
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		return new DataProviderResult<DataRow>
		{
			Rows = DiscountServerService.GetBonusCardsList(param.CardNumber)
		};
	}
}
