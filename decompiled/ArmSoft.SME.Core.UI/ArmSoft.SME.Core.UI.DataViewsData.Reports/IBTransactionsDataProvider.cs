using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Ardalis.SmartEnum;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.Models.InternetBank;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.InternetBank;

namespace ArmSoft.SME.Core.UI.DataViewsData.Reports;

public class IBTransactionsDataProvider : DataProvider<IBTransactionsDataProvider.DataRow, IBTransactionsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey
	{
		public DateTime CreationDate { get; set; }

		public DateTime OperationDate { get; set; }

		public DateOnly Date { get; set; }

		public string Account { get; set; }

		public decimal DebitAmountCur { get; set; }

		public decimal DebitAmount { get; set; }

		public decimal CreditAmountCur { get; set; }

		public decimal CreditAmount { get; set; }

		public string Currency { get; set; }

		public string OperationType { get; set; }

		public string Comment { get; set; }

		public string CorrespondentAccount { get; set; }

		public string CorrespondentName { get; set; }

		public string DocumentNumber { get; set; }

		public string BankDocId { get; set; }

		public Guid? CreatedDocISN { get; set; }

		public byte CreatedDocState { get; set; }

		public string CreatedDocStateName { get; set; }

		public string CreatedDocDescription { get; set; }

		public bool Matched(object key)
		{
			IBTransactionMappingRequest iBTransactionMappingRequest = (IBTransactionMappingRequest)key;
			if (OperationDate == iBTransactionMappingRequest.Date && Account == iBTransactionMappingRequest.Account && CorrespondentAccount == iBTransactionMappingRequest.CorrespondentAccount && Comment == iBTransactionMappingRequest.Comment && BankDocId == iBTransactionMappingRequest.BankDocId && ((CreditAmount > 0m) ? CreditAmount : DebitAmount) == iBTransactionMappingRequest.Sum)
			{
				return ((CreditAmountCur > 0m) ? CreditAmountCur : DebitAmountCur) == iBTransactionMappingRequest.SumCur;
			}
			return false;
		}
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public string Account { get; set; }
	}

	public override string Name => "IBTransactions";

	public IBTransactionsDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = DataProviderTypeSE.IBTransactions.Caption
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "Date",
				Caption = Resources.DateWord,
				Source = "Date",
				Type = SupportedType.DateTime
			},
			new Column
			{
				Name = "Account",
				Caption = Resources.SettlementAccount,
				Source = "Account"
			},
			new Column
			{
				Name = "CreatedDocDescription",
				Caption = Resources.Matched,
				Source = "CreatedDocDescription"
			},
			new Column
			{
				Name = "CreatedDocStateName",
				Caption = Resources.State,
				Source = "CreatedDocStateName"
			},
			new Column
			{
				Name = "DebitAmountCur",
				Caption = Resources.Debit,
				Source = "DebitAmountCur",
				Type = SupportedType.Decimal,
				FooterSum = true,
				Format = DecimalExtension.GetCurrencyFormat(16, 2)
			},
			new Column
			{
				Name = "CreditAmountCur",
				Caption = Resources.Credit,
				Source = "CreditAmountCur",
				Type = SupportedType.Decimal,
				FooterSum = true,
				Format = DecimalExtension.GetCurrencyFormat(16, 2)
			},
			new Column
			{
				Name = "Comment",
				Caption = Resources.Comment,
				Source = "Comment"
			},
			new Column
			{
				Name = "DocumentNumber",
				Caption = Resources.DocNumber,
				Source = "DocumentNumber"
			},
			new Column
			{
				Name = "CorrespondentAccount",
				Caption = Resources.CorrAcc,
				Source = "CorrespondentAccount"
			},
			new Column
			{
				Name = "CorrespondentName",
				Caption = Resources.CorrName,
				Source = "CorrespondentName"
			},
			new Column
			{
				Name = "Currency",
				Caption = Resources.CurrShort,
				Source = "Currency"
			},
			new Column
			{
				Name = "DebitAmount",
				Caption = Resources.DebitNCC,
				Source = "DebitAmount",
				Type = SupportedType.Decimal,
				FooterSum = true,
				Format = DecimalExtension.GetCurrencyFormat(16, 2)
			},
			new Column
			{
				Name = "CreditAmount",
				Caption = Resources.CreditNCC,
				Source = "CreditAmount",
				Type = SupportedType.Decimal,
				FooterSum = true,
				Format = DecimalExtension.GetCurrencyFormat(16, 2)
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		IBTransactionMappingRequest ibKey = null;
		string text = "";
		HashSet<string> hashSet = new HashSet<string>();
		DateOnly startDate;
		DateOnly endDate;
		List<string> list;
		if (keys == null)
		{
			startDate = param.StartDate;
			endDate = param.EndDate;
			list = param.Account.SplitSelected();
		}
		else
		{
			if (keys.Count > 1)
			{
				throw new ASException(Resources.NotSupportSeveralKeys);
			}
			ibKey = (IBTransactionMappingRequest)keys[0];
			startDate = ibKey.Date.ToDateOnly();
			endDate = ibKey.Date.ToDateOnly();
			int num = 1;
			List<string> list2 = new List<string>(num);
			CollectionsMarshal.SetCount(list2, num);
			CollectionsMarshal.AsSpan(list2)[0] = ibKey.Account;
			list = list2;
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		foreach (string item in list)
		{
			text = SettlementAccountHelper.GetBankMainCode(item);
			hashSet.Add(text);
		}
		if (!InternetBankHelper.EnsureLoggedIn(hashSet))
		{
			return dataProviderResult;
		}
		List<InternetBankTransaction> list3 = InternetBankHelper.GetTransactions(startDate, endDate, list, null, IBOperation.AccountStatement, alsoRevaluationType: true);
		List<ImportedIBTransactionsData> importedIBTransactionsData = InternetBankHelper.GetImportedIBTransactionsData(startDate, endDate, list);
		if (ibKey != null)
		{
			int num = 1;
			List<InternetBankTransaction> list4 = new List<InternetBankTransaction>(num);
			CollectionsMarshal.SetCount(list4, num);
			CollectionsMarshal.AsSpan(list4)[0] = list3.FirstOrDefault((InternetBankTransaction p) => p.OperationDate == ibKey.Date && p.Account == ibKey.Account && p.CorrespondentAccount == ibKey.CorrespondentAccount && p.Comment == ibKey.Comment && ((p.CreditAmountCur > 0m) ? p.CreditAmountCur : p.DebitAmountCur) == ibKey.SumCur && p.TransactionID == ibKey.BankDocId && ((p.CreditAmount > 0m) ? p.CreditAmount : p.DebitAmount) == ibKey.Sum);
			list3 = list4;
		}
		if (list3 != null)
		{
			decimal rowCurSum;
			decimal rowSum;
			foreach (InternetBankTransaction row in list3)
			{
				DataRow dataRow = new DataRow
				{
					CreationDate = row.CreationDate,
					Comment = row.Comment,
					Account = row.Account,
					CorrespondentAccount = row.CorrespondentAccount,
					CorrespondentName = row.CorrespondentName,
					CreditAmountCur = row.CreditAmountCur,
					CreditAmount = row.CreditAmount,
					Currency = row.Currency,
					DebitAmountCur = row.DebitAmountCur,
					DebitAmount = row.DebitAmount,
					BankDocId = row.TransactionID,
					DocumentNumber = row.DocumentNumber,
					OperationDate = row.OperationDate,
					OperationType = row.OperationType,
					Date = row.OperationDate.ToDateOnly()
				};
				rowCurSum = ((row.CreditAmountCur == 0m) ? row.DebitAmountCur : row.CreditAmountCur);
				rowSum = ((row.CreditAmount == 0m) ? row.DebitAmount : row.CreditAmount);
				ImportedIBTransactionsData importedIBTransactionsData2 = importedIBTransactionsData.Find((ImportedIBTransactionsData p) => p.Date == row.OperationDate && p.Account == row.Account && p.CorrespondentAccount == row.CorrespondentAccount && p.Comment == row.Comment && p.Sum == rowSum && p.SumCur == rowCurSum && (p.BankDocId == row.TransactionID || p.BankDocId == row.TransactionShortID));
				if (importedIBTransactionsData2 != null)
				{
					dataRow.CreatedDocISN = importedIBTransactionsData2.CreatedDocISN;
					dataRow.CreatedDocState = importedIBTransactionsData2.CreatedDocState;
					dataRow.CreatedDocStateName = SmartEnum<DocumentStateSE, byte>.FromValue(dataRow.CreatedDocState).Caption;
					dataRow.CreatedDocDescription = string.Format(Resources.DocDescrWithDate, SmartEnumHelper.GetSmartEnumRowCaption(typeof(DocumentTypeSE), importedIBTransactionsData2.CreatedDocType.ToString()), importedIBTransactionsData2.CreatedDocNum, importedIBTransactionsData2.CreatedDocDate.FormatDDMMYYYY());
				}
				dataProviderResult.Rows.Add(dataRow);
			}
		}
		return dataProviderResult;
	}
}
