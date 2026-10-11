using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class AccountInBankDataProvider : DataProvider<AccountInBankDataProvider.DataRow, AccountInBankDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Code { get; }

		public string Caption { get; }

		public string Curr { get; }

		public string SettlAccount { get; }

		public bool IsDefault { get; }

		public string NextNum { get; }

		public string BankingModeCode { get; }

		public string BankingModeCaption { get; }

		public byte[] TS { get; }

		public DataRow(string code, string caption, string curr, string settlAccount, bool isDefault, string nextNum, string bankingModeCode, string bankingModeCaption, byte[] ts)
		{
			Code = code;
			Caption = caption;
			Curr = curr;
			SettlAccount = settlAccount;
			IsDefault = isDefault;
			NextNum = nextNum;
			BankingModeCode = bankingModeCode;
			BankingModeCaption = bankingModeCaption;
			TS = ts;
		}

		public DataRow()
		{
		}
	}

	public class Param
	{
		public string CurrCode { get; set; }

		public string BankingMode { get; set; }
	}

	private readonly bool _isKeySettlAccount;

	public override string Name => "AccountInBank";

	public AccountInBankDataProvider(bool isKeySettlAccount)
		: base((ApiClient)null)
	{
		_isKeySettlAccount = isKeySettlAccount;
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Common.Resources.AccountInBank
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "Code",
				Caption = ArmSoft.SME.Common.Resources.Code,
				Source = "Code"
			},
			new Column
			{
				Name = "SettlAccount",
				Caption = ArmSoft.SME.Common.Resources.SettlementAccount,
				Source = "SettlAccount"
			},
			new Column
			{
				Name = "Caption",
				Caption = ArmSoft.SME.Common.Resources.Name,
				Source = "Caption"
			},
			new Column
			{
				Name = "Curr",
				Caption = ArmSoft.SME.Common.Resources.Curr,
				Source = "Curr"
			},
			new Column
			{
				Name = "NextNum",
				Caption = ArmSoft.SME.Trade.Common.Resources.NextPONum,
				Source = "NextNum"
			},
			new Column
			{
				Name = "IsDefault",
				Caption = ArmSoft.SME.Common.Resources.Default,
				Source = "IsDefault",
				Type = SupportedType.Boolean
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
		if (settings != null && settings.ReloadData)
		{
			TradeSettings.TradeCacheService.AccountInBank.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		IEnumerable<AccountInBankCacheRow> enumerable = TradeSettings.TradeCacheService.AccountInBank.GetList();
		if (!string.IsNullOrWhiteSpace(param.CurrCode))
		{
			enumerable = enumerable.Where((AccountInBankCacheRow p) => p.Curr == param.CurrCode);
		}
		if (!string.IsNullOrWhiteSpace(param.BankingMode))
		{
			enumerable = enumerable.Where((AccountInBankCacheRow p) => p.BankingModeCode == param.BankingMode);
		}
		foreach (AccountInBankCacheRow item2 in enumerable)
		{
			DataRow item = new DataRow(item2.Code, item2.Caption, item2.Curr, item2.SettlAccount, item2.IsDefault, item2.NextNum, item2.BankingModeCode, item2.BankingModeCaption, item2.TS);
			dataProviderResult.Rows.Add(item);
		}
		if (_isKeySettlAccount)
		{
			dataProviderResult.Rows = (from x in dataProviderResult.Rows
				orderby x.IsDefault descending, x.SettlAccount
				select x).ToList();
		}
		else
		{
			dataProviderResult.Rows = (from x in dataProviderResult.Rows
				orderby x.IsDefault descending, x.Code
				select x).ToList();
		}
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
