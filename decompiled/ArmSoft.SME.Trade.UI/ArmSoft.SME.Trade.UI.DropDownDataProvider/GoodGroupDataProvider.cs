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

public class GoodGroupDataProvider : DataProvider<GoodGroupDataProvider.DataRow, GoodGroupDataProvider.Param>
{
	public class DataRow : ExtendableRow, IFilterKey
	{
		public string Code { get; }

		public string CodeForTree { get; }

		public string Caption { get; }

		public string CaptionEn { get; }

		public string CaptionRu { get; }

		public string Path { get; }

		public string Parent { get; }

		public bool IsLeaf { get; }

		public byte[] TS { get; }

		public object FilterKey => Code;

		public object FilterPath => Path;

		public DataRow(string code, string codeForTree, string caption, string captionEn, string captionRu, string path, string parent, byte[] ts)
		{
			Code = code;
			CodeForTree = codeForTree;
			Caption = caption;
			CaptionEn = captionEn;
			CaptionRu = captionRu;
			Path = path;
			Parent = parent;
			TS = ts;
		}

		public DataRow()
		{
		}
	}

	public class Param
	{
	}

	public override string Name => "GoodGroup";

	public GoodGroupDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.MTGroup
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
			},
			new Column
			{
				Name = "CaptionEn",
				Caption = ArmSoft.SME.Trade.Common.Resources.MTNameEn,
				Source = "CaptionEn"
			},
			new Column
			{
				Name = "CaptionRu",
				Caption = ArmSoft.SME.Trade.Common.Resources.MTNameRu,
				Source = "CaptionRu"
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
			TradeSettings.TradeCacheService.MTGroup.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		foreach (MTGroupCacheRow item2 in TradeSettings.TradeCacheService.MTGroup.GetList())
		{
			DataRow item = new DataRow(item2.Code, item2.CodeForTree, item2.Caption, item2.CaptionEn, item2.CaptionRu, item2.Path, item2.Parent, item2.TS);
			dataProviderResult.Rows.Add(item);
		}
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
