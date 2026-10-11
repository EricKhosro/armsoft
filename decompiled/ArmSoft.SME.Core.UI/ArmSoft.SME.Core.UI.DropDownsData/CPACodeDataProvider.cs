using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Core.UI.DropDownsData;

public class CPACodeDataProvider : DataProvider<CPACodeDataProvider.DataRow, CPACodeDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public string Type { get; set; }

		public string TypeName { get; set; }

		public DateOnly? MarkInitDate { get; set; }
	}

	public class Param
	{
		public string Type { get; set; }
	}

	public override string Name => "CPACode";

	public CPACodeDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = Resources.CPA
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
				Name = "TypeName",
				Caption = Resources.TypeWord,
				Source = "TypeName"
			},
			new Column
			{
				Name = "MarkInitDate",
				Caption = Resources.MarkableFrom,
				Source = "MarkInitDate"
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
			ArmSoft.SME.Core.UI.Settings.CoreCacheService.CPACode.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		IReadOnlyList<CPACodesCacheRow> readOnlyList = ArmSoft.SME.Core.UI.Settings.CoreCacheService.CPACode.GetList();
		if (!string.IsNullOrWhiteSpace(param.Type))
		{
			List<CPACodesCacheRow> list = new List<CPACodesCacheRow>();
			list.AddRange(readOnlyList.Where((CPACodesCacheRow p) => param.Type.Contains(p.Type, StringComparison.CurrentCulture)));
			readOnlyList = new _003C_003Ez__ReadOnlyList<CPACodesCacheRow>(list);
		}
		foreach (CPACodesCacheRow item2 in readOnlyList)
		{
			DataRow item = new DataRow
			{
				Code = item2.Code,
				Caption = item2.Name,
				Type = item2.Type,
				TypeName = item2.TypeName,
				MarkInitDate = item2.MarkInitDate
			};
			dataProviderResult.Rows.Add(item);
		}
		dataProviderResult.Rows = (from x in dataProviderResult.Rows
			orderby x.TypeName, x.Code
			select x).ToList();
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
