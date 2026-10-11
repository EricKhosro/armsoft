using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class ECRChecksListInsidePOSDataProvider : DataProvider<ECRChecksListInsidePOSDataProvider.DataRow, ECRChecksListInsidePOSDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public Guid ISN { get; set; }

		public string DocNum { get; set; }

		public DateOnly DocDate { get; set; }

		public short DocType { get; set; }

		public string DocTypeName { get; set; }

		public decimal Summ { get; set; }

		public string ECRCheckNum { get; set; } = "";

		public decimal SummaCash { get; set; }

		public decimal SummaBank { get; set; }

		public decimal SummaPrePay { get; set; }

		public decimal SummaBonus { get; set; }

		public decimal SummaCredit { get; set; }

		public decimal SummaryOfOtherSums => SummaPrePay + SummaBonus + SummaCredit;

		public Guid GetDocISN()
		{
			return ISN;
		}

		public virtual short GetDocType()
		{
			return DocType;
		}

		public bool Matched(object key)
		{
			return (Guid)key == ISN;
		}
	}

	public class Param
	{
		public DateOnly? StartDate { get; set; }

		public DateOnly? EndDate { get; set; }

		public string CurrCode { get; set; } = "";

		public string CashCode { get; set; } = "";

		public string ShiftNumber { get; set; } = "";

		public string DocState { get; set; } = "";
	}

	public override string Name => "ECRChecksListInsidePOSDataProvider";

	public ECRChecksListInsidePOSDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = Resources.ECRChecksJournal
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "DocDate",
				Caption = Resources.DateWord,
				Source = "DocDate"
			},
			new Column
			{
				Name = "DocNum",
				Caption = Resources.DocNumber,
				Source = "DocNum"
			},
			new Column
			{
				Name = "ECRCheckNum",
				Caption = Resources.ECRCheckNum,
				Source = "ECRCheckNum"
			},
			new Column
			{
				Name = "DocTypeName",
				Caption = Resources.TypeWord,
				Source = "DocTypeName"
			},
			new Column
			{
				Name = "DocType",
				Caption = Resources.TypeWord,
				Source = "DocType"
			},
			new Column
			{
				Name = "Summ",
				Caption = Resources.Summa,
				Source = "Summ",
				Format = DecimalExtension.GetCurrencyFormat(16, 2),
				Type = SupportedType.Decimal,
				FooterSum = true
			},
			new Column
			{
				Name = "SummaCash",
				Caption = Resources.CashSum,
				Source = "SummaCash",
				Format = DecimalExtension.GetCurrencyFormat(16, 2),
				Type = SupportedType.Decimal,
				FooterSum = true
			},
			new Column
			{
				Name = "SummaBank",
				Caption = Resources.PlCardSum,
				Source = "SummaBank",
				Format = DecimalExtension.GetCurrencyFormat(16, 2),
				Type = SupportedType.Decimal,
				FooterSum = true
			},
			new Column
			{
				Name = "SummaryOfOtherSums",
				Caption = Resources.OtherPayment,
				Source = "SummaryOfOtherSums",
				Format = DecimalExtension.GetCurrencyFormat(16, 2),
				Type = SupportedType.Decimal,
				FooterSum = true
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override Task<Schema> GetSchemaAsync(Param param, ExecutionSettings settings, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		foreach (DocumentsJournalDataProvider.DataRow row in new ECRCheckDocumentsJournalDataProvider(TradeSettings.CreateTradeClient()).GetRows(new DocumentsJournalDataProvider.Param
		{
			StartDate = param.StartDate,
			EndDate = param.EndDate,
			CurrCode = param.CurrCode,
			CashCode = param.CashCode,
			ShiftNumber = param.ShiftNumber,
			DocState = param.DocState,
			ShowPayments = true,
			ReportType = ReportTypesSE.ECRCheckDocument.Value,
			DocType = string.Join(",", new HashSet<short>
			{
				DocumentTypeSE.MTBill,
				DocumentTypeSE.MTBillReturn,
				DocumentTypeSE.MTReturnFromCustomer
			})
		}))
		{
			DataRow dataRow = new DataRow
			{
				ISN = row.ISN,
				DocNum = row.DocNum,
				DocDate = row.DocDate,
				DocType = row.DocType,
				DocTypeName = row.DocTypeName,
				ECRCheckNum = row.ECRCheckNum
			};
			if (dataRow.DocType == 38 || dataRow.DocType == 15)
			{
				dataRow.Summ = -row.Summ;
				dataRow.SummaCash = -row.SummaCash;
				dataRow.SummaBank = -row.SummaBank;
				dataRow.SummaPrePay = -row.SummaPrePay;
				dataRow.SummaBonus = -row.SummaBonus;
				dataRow.SummaCredit = -row.SummaCredit;
			}
			else
			{
				dataRow.Summ = row.Summ;
				dataRow.SummaCash = row.SummaCash;
				dataRow.SummaBank = row.SummaBank;
				dataRow.SummaPrePay = row.SummaPrePay;
				dataRow.SummaBonus = row.SummaBonus;
				dataRow.SummaCredit = row.SummaCredit;
			}
			dataProviderResult.Rows.Add(dataRow);
		}
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
