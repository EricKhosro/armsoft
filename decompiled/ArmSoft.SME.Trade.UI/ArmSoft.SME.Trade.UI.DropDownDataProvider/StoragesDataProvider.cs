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

public class StoragesDataProvider : DataProvider<StoragesDataProvider.DataRow, StoragesDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public string Response { get; set; }

		public string Address { get; set; }

		public bool Retail { get; set; }

		public bool IsDefault { get; set; }

		public DataRow(string code, string caption, string response, string address, bool retail, bool isDefault)
		{
			Code = code;
			Caption = caption;
			Response = response;
			Address = address;
			Retail = retail;
			IsDefault = isDefault;
		}

		public DataRow()
		{
		}
	}

	public class Param
	{
		public bool? IsRetail { get; set; }

		public bool? CheckViewOrOperationAccess { get; set; }
	}

	public override string Name => "Storages";

	public StoragesDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Common.Resources.StoragesAndShops
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
				Name = "Caption",
				Caption = ArmSoft.SME.Common.Resources.Name,
				Source = "Caption"
			},
			new Column
			{
				Name = "Response",
				Caption = ArmSoft.SME.Common.Resources.Stockkeeper,
				Source = "Response"
			},
			new Column
			{
				Name = "Address",
				Caption = ArmSoft.SME.Common.Resources.Address,
				Source = "Address"
			},
			new Column
			{
				Name = "Retail",
				Caption = ArmSoft.SME.Trade.Common.Resources.RetailWord,
				Source = "Retail",
				Type = SupportedType.Boolean
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
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		if (settings != null && settings.ReloadData)
		{
			TradeSettings.TradeCacheService.Storage.Refresh();
		}
		IEnumerable<StoragesCacheRow> enumerable = TradeSettings.TradeCacheService.Storage.GetList();
		if (param.IsRetail.HasValue)
		{
			enumerable = enumerable.Where((StoragesCacheRow p) => p.Retail == param.IsRetail.Value);
		}
		if (param.CheckViewOrOperationAccess.HasValue)
		{
			string availableStoragesDelimited = TradeSettings.CreateTradeClient().CommonServices.GetAvailableStoragesDelimited(param.CheckViewOrOperationAccess.Value);
			if (!string.IsNullOrWhiteSpace(availableStoragesDelimited))
			{
				string[] storages = availableStoragesDelimited.Split(',');
				enumerable = enumerable.Where((StoragesCacheRow x) => MemoryExtensions.Contains<string>((ReadOnlySpan<string>)storages, x.Code));
			}
		}
		foreach (StoragesCacheRow item2 in enumerable)
		{
			DataRow item = new DataRow(item2.Code, item2.Caption, item2.Response, item2.Address, item2.Retail, item2.IsDefault);
			dataProviderResult.Rows.Add(item);
		}
		dataProviderResult.Rows = dataProviderResult.Rows.OrderBy((DataRow x) => x.Code).ToList();
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
