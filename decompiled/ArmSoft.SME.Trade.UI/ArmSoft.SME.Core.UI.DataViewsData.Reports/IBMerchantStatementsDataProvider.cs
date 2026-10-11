using System;
using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.InternetBank;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI;

namespace ArmSoft.SME.Core.UI.DataViewsData.Reports;

public class IBMerchantStatementsDataProvider : DataProvider<IBMerchantStatementsDataProvider.DataRow, IBMerchantStatementsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public DateOnly? OperationDate { get; set; }

		public DateTime? TransactionDate { get; set; }

		public string TransactionType { get; set; }

		public string Currency { get; set; }

		public decimal Amount { get; set; }

		public decimal MerchantFee { get; set; }

		public decimal AmountWithoutMerchantFee { get; set; }

		public string MerchantName { get; set; }

		public string CardNumber { get; set; }

		public string CardType { get; set; }

		public string CardBelongingName { get; set; }

		public string TokenPan { get; set; }

		public string RRN { get; set; }

		public string TerminalId { get; set; }

		public string AuthorizationCode { get; set; }

		public Guid? CreatedDocISN { get; set; }

		public string CreatedDocDescription { get; set; }

		public byte CreatedDocState { get; set; }

		public string CreatedDocStateName { get; set; }
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public string Account { get; set; }
	}

	public override string Name => "IBMerchantStatements";

	public IBMerchantStatementsDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = DataProviderTypeSE.IBMerchantStatements.Caption
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "OperationDate",
				Caption = ArmSoft.SME.Trade.Common.Resources.PostingDate,
				Source = "OperationDate",
				Type = SupportedType.DateOnly
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
				Name = "CreatedDocDescription",
				Caption = ArmSoft.SME.Common.Resources.Matched,
				Source = "CreatedDocDescription",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "CreatedDocStateName",
				Caption = ArmSoft.SME.Common.Resources.State,
				Source = "CreatedDocStateName",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "TransactionType",
				Caption = ArmSoft.SME.Trade.Common.Resources.TransactionType,
				Source = "TransactionType"
			},
			new Column
			{
				Name = "Currency",
				Caption = ArmSoft.SME.Trade.Common.Resources.TransactionCurrency,
				Source = "Currency"
			},
			new Column
			{
				Name = "Amount",
				Caption = ArmSoft.SME.Trade.Common.Resources.TransactionAmount,
				Source = "Amount",
				Type = SupportedType.Decimal,
				FooterSum = true,
				Format = DecimalExtension.GetCurrencyFormat(16, 2)
			},
			new Column
			{
				Name = "MerchantFee",
				Caption = ArmSoft.SME.Trade.Common.Resources.Commission,
				Source = "MerchantFee",
				Type = SupportedType.Decimal,
				FooterSum = true,
				Format = DecimalExtension.GetCurrencyFormat(16, 2)
			},
			new Column
			{
				Name = "AmountWithoutMerchantFee",
				Caption = ArmSoft.SME.Trade.Common.Resources.AmountWithoutCommision,
				Source = "AmountWithoutMerchantFee",
				Type = SupportedType.Decimal,
				FooterSum = true,
				Format = DecimalExtension.GetCurrencyFormat(16, 2)
			},
			new Column
			{
				Name = "MerchantName",
				Caption = ArmSoft.SME.Trade.Common.Resources.MerchantName,
				Source = "MerchantName"
			},
			new Column
			{
				Name = "CardNumber",
				Caption = ArmSoft.SME.Trade.Common.Resources.UserCardNumber,
				Source = "CardNumber"
			},
			new Column
			{
				Name = "CardType",
				Caption = ArmSoft.SME.Trade.Common.Resources.CardType,
				Source = "CardType"
			},
			new Column
			{
				Name = "CardBelongingName",
				Caption = ArmSoft.SME.Trade.Common.Resources.CardBelongingType,
				Source = "CardBelongingName"
			},
			new Column
			{
				Name = "TokenPan",
				Caption = ArmSoft.SME.Trade.Common.Resources.TokenPan,
				Source = "TokenPan"
			},
			new Column
			{
				Name = "RRN",
				Caption = ArmSoft.SME.Trade.Common.Resources.RRN,
				Source = "RRN"
			},
			new Column
			{
				Name = "TerminalId",
				Caption = ArmSoft.SME.Trade.Common.Resources.TerminalNumber,
				Source = "TerminalId"
			},
			new Column
			{
				Name = "AuthorizationCode",
				Caption = ArmSoft.SME.Trade.Common.Resources.AuthorizationCode,
				Source = "AuthorizationCode"
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
		List<MerchantTransactionRow> merchantTransactions = ArmSoft.SME.Core.UI.Settings.InternetBankService.GetMerchantTransactions(param.StartDate.ToDateTime(), param.EndDate.ToDateTime(), param.Account);
		if (merchantTransactions != null && merchantTransactions.Count != 0)
		{
			DateOnly? startDate = merchantTransactions.Min((MerchantTransactionRow d) => d.OperationDate);
			DateOnly? endDate = merchantTransactions.Max((MerchantTransactionRow d) => d.OperationDate);
			IEnumerable<DocumentsJournalDataProvider.DataRow> source = from d in new ECRCheckDocumentsJournalDataProvider(TradeSettings.CreateTradeClient()).GetRows(new DocumentsJournalDataProvider.Param
				{
					StartDate = startDate,
					EndDate = endDate,
					ReportType = ReportTypesSE.ECRCheckDocument.Value,
					ShowPayments = true
				}, withoutAccessCheck: true)
				where d.QRPaymentAccepted
				select d;
			foreach (MerchantTransactionRow row in merchantTransactions)
			{
				DataRow dataRow = new DataRow
				{
					OperationDate = row.OperationDate,
					TransactionDate = row.TransactionDate,
					TransactionType = row.TransactionType,
					Currency = row.Currency,
					Amount = row.Amount,
					MerchantFee = row.MerchantFee,
					AmountWithoutMerchantFee = row.AmountWithoutMerchantFee,
					MerchantName = row.MerchantName,
					CardNumber = row.CardNumber,
					CardType = row.CardTypeName,
					CardBelongingName = row.CardBelongingName,
					TokenPan = row.TokenPan,
					RRN = row.RRN,
					TerminalId = row.TerminalId,
					AuthorizationCode = row.AuthorizationCode
				};
				DocumentsJournalDataProvider.DataRow dataRow2 = source.FirstOrDefault((DocumentsJournalDataProvider.DataRow p) => p.QRPaymentID.Contains(row.RRN));
				if (dataRow2 != null)
				{
					dataRow.CreatedDocISN = dataRow2.ISN;
					dataRow.CreatedDocState = dataRow2.DocState;
					dataRow.CreatedDocStateName = dataRow2.DocStateName;
					dataRow.CreatedDocDescription = string.Format(ArmSoft.SME.Common.Resources.DocDescrWithDate, SmartEnumHelper.GetSmartEnumRowCaption(typeof(DocumentTypeSE), dataRow2.DocType.ToString()), dataRow2.DocNum, dataRow2.DocDate.FormatDDMMYYYY());
				}
				dataProviderResult.Rows.Add(dataRow);
			}
		}
		return dataProviderResult;
	}
}
