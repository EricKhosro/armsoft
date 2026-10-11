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

public class PropertyListDataProvider : DataProvider<PropertyListDataProvider.DataRow, PropertyListDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public class Key
		{
			public string DestCode { get; set; }

			public string Code { get; set; }
		}

		public string DestCode { get; set; }

		public string Code { get; set; }

		public string Caption { get; set; }

		public byte[] TS { get; set; }
	}

	public class Param
	{
		public string MyDestCode { get; set; }
	}

	public override string Name => "PropertyList";

	public PropertyListDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.PropertiesListsValues
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "DestCode",
				Caption = ArmSoft.SME.Trade.Common.Resources.AttributeType,
				Source = "DestCode"
			},
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
			TradeSettings.TradeCacheService.PropertyList.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		IEnumerable<PropertyListCacheRow> enumerable = TradeSettings.TradeCacheService.PropertyList.GetList();
		if (!string.IsNullOrWhiteSpace(param.MyDestCode))
		{
			enumerable = enumerable.Where((PropertyListCacheRow p) => p.DestCode == param.MyDestCode);
		}
		foreach (PropertyListCacheRow item2 in enumerable)
		{
			DataRow item = new DataRow
			{
				Code = item2.Code,
				Caption = item2.Caption,
				DestCode = item2.DestCode,
				TS = item2.TS
			};
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
