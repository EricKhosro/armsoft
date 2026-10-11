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

public class DiscountGroupDataProvider : DataProvider<DiscountGroupDataProvider.DataRow, DiscountGroupDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public string Type { get; set; }

		public string TypeCaption { get; set; }

		public byte Priority { get; set; }
	}

	public class Param
	{
	}

	public override string Name => "DiscountGroup";

	public DiscountGroupDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.DiscountGroup
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
				Name = "Type",
				Caption = ArmSoft.SME.Trade.Common.Resources.DiscountsMergeMethod,
				Source = "Type"
			},
			new Column
			{
				Name = "TypeCaption",
				Caption = ArmSoft.SME.Trade.Common.Resources.MethodName,
				Source = "TypeCaption"
			},
			new Column
			{
				Name = "Priority",
				Caption = ArmSoft.SME.Trade.Common.Resources.Priority,
				Source = "Priority",
				Type = SupportedType.Byte
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
			TradeSettings.TradeCacheService.DiscountGroup.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		foreach (DiscountGroupCacheRow item2 in TradeSettings.TradeCacheService.DiscountGroup.GetList())
		{
			DataRow item = new DataRow
			{
				Code = item2.Code,
				Caption = item2.Caption,
				Type = item2.Type,
				TypeCaption = item2.TypeCaption,
				Priority = item2.Priority
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
