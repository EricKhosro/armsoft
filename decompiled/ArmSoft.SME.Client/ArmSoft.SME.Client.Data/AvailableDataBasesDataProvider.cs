using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Cloud;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Client.Data;

public class AvailableDataBasesDataProvider : DataProvider<AvailableDataBasesDataProvider.DataRow, AvailableDataBasesDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Name { get; set; }

		public string Caption { get; set; }

		public bool IsBlocked { get; set; }

		public bool DisableOldUI { get; set; }

		public string ServerName { get; set; }

		public string ComboView => Name + " (" + Caption + ")";
	}

	public class Param
	{
	}

	private readonly List<Database> _dataBaseslist;

	public override string Name => "AvailableDataBases";

	public AvailableDataBasesDataProvider(List<Database> source)
		: base((ApiClient)null)
	{
		_dataBaseslist = source;
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = "AvailableDataBases",
			Caption = "AvailableDataBases"
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "Name",
				Caption = Resources.Name,
				Source = "Name"
			},
			new Column
			{
				Name = "Caption",
				Caption = Resources.Comment,
				Source = "Caption"
			},
			new Column
			{
				Name = "IsBlocked",
				Caption = Resources.IsBlocked,
				Source = "IsBlocked",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "DisableOldUI",
				Caption = Resources.OldUIDisabled,
				Source = "DisableOldUI",
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
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		foreach (Database item2 in _dataBaseslist)
		{
			DataRow item = new DataRow
			{
				Name = item2.Name,
				Caption = item2.Caption,
				IsBlocked = item2.IsBlocked,
				DisableOldUI = item2.DisableOldUI
			};
			dataProviderResult.Rows.Add(item);
		}
		dataProviderResult.Rows.OrderBy((DataRow p) => p.Name);
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
