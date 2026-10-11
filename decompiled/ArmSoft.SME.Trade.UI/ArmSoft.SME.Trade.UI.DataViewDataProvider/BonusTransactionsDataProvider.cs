using System;
using System.Collections.Generic;
using System.Linq;
using ArmSoft.Cloud.Client.Discount;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.Services;

namespace ArmSoft.SME.Trade.UI.DataViewDataProvider;

public class BonusTransactionsDataProvider : DataProvider<BonusTransactionsDataProvider.DataRow, BonusTransactionsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey
	{
		public string CardNumber { get; set; } = "";

		public DateTime TransactionDate { get; set; }

		public decimal Sum { get; set; }

		public byte OpTypeCode { get; set; }

		public string OpType { get; set; }

		public bool IsDeleted { get; set; }

		public DateOnly DocDate { get; set; }

		public short? DocTypeCode { get; set; }

		public string DocTypeCaption { get; set; }

		public string DocNumber { get; set; } = "";

		public decimal DocSum { get; set; }

		public Guid TransactionISN { get; set; }

		public string User { get; set; }

		public string FingerPrint { get; set; }

		public string CashCode { get; set; }

		public string ShiftNumber { get; set; }

		bool IMatchedUpdateKey.Matched(object key)
		{
			return (Guid)key == TransactionISN;
		}
	}

	public class Param
	{
		public DateOnly? StartDate { get; set; }

		public DateOnly? EndDate { get; set; }

		public string CardNumber { get; set; }

		public bool ShowDeleted { get; set; }
	}

	public override string Name => "BonusTransactions";

	public BonusTransactionsDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.BonusTransactions
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
				Name = "TransactionDate",
				Caption = ArmSoft.SME.Trade.Common.Resources.TransactionDate,
				Source = "TransactionDate",
				Type = SupportedType.DateTime
			},
			new Column
			{
				Name = "Sum",
				Caption = ArmSoft.SME.Common.Resources.Summa,
				Source = "Sum",
				Type = SupportedType.Decimal,
				FooterSum = true
			},
			new Column
			{
				Name = "OpType",
				Caption = ArmSoft.SME.Common.Resources.Operation,
				Source = "OpType",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "IsDeleted",
				Caption = ArmSoft.SME.Common.Resources.Deleted,
				Source = "IsDeleted",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "DocDate",
				Caption = ArmSoft.SME.Trade.Common.Resources.DocumentDate,
				Source = "DocDate",
				Type = SupportedType.DateOnly
			},
			new Column
			{
				Name = "DocTypeCaption",
				Caption = ArmSoft.SME.Common.Resources.TypeWord,
				Source = "DocTypeCaption",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "DocNumber",
				Caption = ArmSoft.SME.Common.Resources.DocNumber,
				Source = "DocNumber",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "DocSum",
				Caption = ArmSoft.SME.Trade.Common.Resources.DocumentSum,
				Source = "DocSum",
				Type = SupportedType.Decimal
			},
			new Column
			{
				Name = "User",
				Caption = ArmSoft.SME.Trade.Common.Resources.ASTradeUser,
				Source = "User",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "FingerPrint",
				Caption = ArmSoft.SME.Trade.Common.Resources.FingerPrint,
				Source = "FingerPrint",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "CashCode",
				Caption = ArmSoft.SME.Common.Resources.Cash,
				Source = "CashCode",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "ShiftNumber",
				Caption = ArmSoft.SME.Trade.Common.Resources.CashierShiftNum,
				Source = "ShiftNumber",
				Type = SupportedType.String
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		List<CardTransaction> list = DiscountServerService.GetTransactionsList(param.CardNumber, param.StartDate, param.EndDate, fillDocInfo: true, param.ShowDeleted);
		if (keys != null)
		{
			list = list.Where((CardTransaction t) => keys.Contains(t.ISN)).ToList();
		}
		dataProviderResult.Rows = DiscountServerService.TransformToLocalType(list);
		return dataProviderResult;
	}
}
