using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class ComplectDataProvider : DataProvider<ComplectDataProvider.DataRow, ComplectDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string MTCode { get; set; }

		public string Caption { get; set; }

		public string UnitBrief { get; set; }

		public string Description { get; set; }

		public DataRow(string mtCode, string caption, string unitBrief, string description)
		{
			MTCode = mtCode;
			Caption = caption;
			UnitBrief = unitBrief;
			Description = description;
		}

		public DataRow()
		{
		}
	}

	public class Param
	{
		public string Group { get; set; }
	}

	public override string Name => "Complect";

	public ComplectDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = Resources.Complect
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "MTCode",
				Caption = Resources.Code,
				Source = "MTCode"
			},
			new Column
			{
				Name = "Caption",
				Caption = Resources.Name,
				Source = "Caption"
			},
			new Column
			{
				Name = "UnitBrief",
				Caption = Resources.MeasureUnit,
				Source = "UnitBrief"
			},
			new Column
			{
				Name = "Description",
				Caption = Resources.Specification,
				Source = "Description"
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
			TradeSettings.TradeCacheService.Material.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		IEnumerable<MaterialCacheRow> enumerable = (string.IsNullOrWhiteSpace(param.Group) ? TradeSettings.TradeCacheService.Material.GetList((MaterialCacheRow p) => p.IsComplect) : (from p in TradeSettings.TradeCacheService.Material.GetItems(param.Group)
			where p.IsComplect
			select p));
		foreach (MaterialCacheRow item2 in enumerable)
		{
			DataRow item = new DataRow(item2.MTCode, item2.Caption, item2.UnitRow.Brief, item2.Description);
			dataProviderResult.Rows.Add(item);
		}
		dataProviderResult.Rows = dataProviderResult.Rows.OrderBy((DataRow x) => x.MTCode).ToList();
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
