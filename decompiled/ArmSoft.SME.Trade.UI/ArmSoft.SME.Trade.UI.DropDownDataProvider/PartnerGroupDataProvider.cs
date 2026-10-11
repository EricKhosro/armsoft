using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Client.Data;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class PartnerGroupDataProvider : DataProvider<PartnerGroupDataProvider.DataRow, PartnerGroupDataProvider.Param>
{
	public class DataRow : ExtendableRow, IFilterKey
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public string CodeForTree { get; set; }

		public string Path { get; set; }

		public string Parent { get; set; }

		public string Body { get; set; }

		public object FilterKey => Code;

		public object FilterPath => Path;

		public DataRow(string code, string codeForTree, string caption, string path, string parent)
		{
			Code = code;
			CodeForTree = codeForTree;
			Caption = caption;
			Path = path;
			Parent = parent;
		}

		public DataRow()
		{
		}
	}

	public class Param : ExtendableRow
	{
	}

	public override string Name => "PartnerGroup";

	public PartnerGroupDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.PartnerGroup
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
				Name = "CodeForTree",
				Caption = ArmSoft.SME.Common.Resources.Code,
				Source = "CodeForTree"
			},
			new Column
			{
				Name = "Caption",
				Caption = ArmSoft.SME.Common.Resources.Name,
				Source = "Caption"
			},
			new Column
			{
				Name = "Parent",
				Caption = ArmSoft.SME.Common.Resources.Parent,
				Source = "Parent"
			},
			new Column
			{
				Name = "Path",
				Caption = "Path",
				Source = "Path"
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
			TradeSettings.TradeCacheService.PartnerGood.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		foreach (PartnerGroupCacheRow item2 in TradeSettings.TradeCacheService.PartnerGroup.GetList())
		{
			DataRow item = new DataRow(item2.Code, item2.CodeForTree, item2.Caption, item2.Path, item2.Parent);
			dataProviderResult.Rows.Add(item);
		}
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
