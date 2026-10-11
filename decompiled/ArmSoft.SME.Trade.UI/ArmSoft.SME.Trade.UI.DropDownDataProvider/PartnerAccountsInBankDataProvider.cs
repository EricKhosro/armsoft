using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class PartnerAccountsInBankDataProvider : DataProvider<PartnerAccountsInBankDataProvider.DataRow, PartnerAccountsInBankDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int PartID { get; set; }

		public string PartCode { get; set; }

		public string PartCaption { get; set; }

		public string SettlAccount { get; set; }

		public string Caption { get; set; }

		public bool IsDefault { get; set; }
	}

	public class Param
	{
		public string PartnerCode { get; set; }
	}

	public override string Name => "PartnerAccountsInBank";

	public PartnerAccountsInBankDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.PartnersAccountsInBank
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "PartCode",
				Caption = ArmSoft.SME.Common.Resources.Partner,
				Source = "PartCode"
			},
			new Column
			{
				Name = "PartCaption",
				Caption = ArmSoft.SME.Common.Resources.PartnerName,
				Source = "PartCaption"
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
				Name = "IsDefault",
				Caption = ArmSoft.SME.Common.Resources.Default,
				Source = "IsDefault"
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
			TradeSettings.TradeCacheService.Partner.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		IEnumerable<PartnerCacheRow> enumerable = (string.IsNullOrWhiteSpace(param.PartnerCode) ? TradeSettings.TradeCacheService.Partner.GetList() : new _003C_003Ez__ReadOnlySingleElementList<PartnerCacheRow>(TradeSettings.TradeCacheService.Partner.GetItem(param.PartnerCode)));
		foreach (PartnerCacheRow item2 in enumerable)
		{
			foreach (PartnerCacheRow.SettlementAccountRow settlementAccount in item2.SettlementAccounts)
			{
				DataRow item = new DataRow
				{
					PartID = item2.Id,
					SettlAccount = settlementAccount.SettlAccount,
					Caption = settlementAccount.Caption,
					IsDefault = settlementAccount.IsDefault,
					PartCode = item2.PartCode,
					PartCaption = item2.Caption
				};
				dataProviderResult.Rows.Add(item);
			}
		}
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
