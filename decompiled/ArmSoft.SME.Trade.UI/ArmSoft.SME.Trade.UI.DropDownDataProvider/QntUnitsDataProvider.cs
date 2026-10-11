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

public class QntUnitsDataProvider : DataProvider<QntUnitsDataProvider.DataRow, QntUnitsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public string Brief { get; set; }

		public DataRow(string code, string caption, string brief)
		{
			Code = code;
			Caption = caption;
			Brief = brief;
		}

		public DataRow()
		{
		}
	}

	public class Param
	{
	}

	public override string Name => "QntUnits";

	public QntUnitsDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = Resources.MeasureUnits
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "Code",
				Caption = Resources.Code,
				Source = "Code"
			},
			new Column
			{
				Name = "Caption",
				Caption = Resources.Name,
				Source = "Caption"
			},
			new Column
			{
				Name = "Brief",
				Caption = Resources.Brief,
				Source = "Brief"
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		if (settings != null && settings.ReloadData)
		{
			TradeSettings.TradeCacheService.QntUnit.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		foreach (QntUnitCacheRow item2 in TradeSettings.TradeCacheService.QntUnit.GetList())
		{
			DataRow item = new DataRow
			{
				Code = item2.Code,
				Caption = item2.Caption,
				Brief = item2.Brief
			};
			dataProviderResult.Rows.Add(item);
		}
		dataProviderResult.Rows = dataProviderResult.Rows.OrderBy((DataRow x) => x.Code).ToList();
		return dataProviderResult;
	}

	public override Task<Schema> GetSchemaAsync(Param param, ExecutionSettings settings, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
