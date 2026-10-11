using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Data;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Client.Data.Reports;

public class GroupByColumnsDataProvider : DataProvider<ExtendableRow, object>
{
	private List<ExtendableRow> _sourceRows;

	private readonly ExtendableRow.RowSchema _rowScheme;

	public override string Name
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public GroupByColumnsDataProvider(ExtendableRow.RowSchema schema, List<ExtendableRow> source)
		: base((ApiClient)null)
	{
		_sourceRows = source;
		_rowScheme = schema;
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema();
		List<Column> list = new List<Column>();
		foreach (ExtendableRow.Column value in _rowScheme.DynamicColumns.Values)
		{
			Column item = new Column
			{
				Name = value.Name,
				Caption = value.Caption,
				Source = value.Name,
				Type = value.Type,
				SourceType = ColumnSourceType.Dynamic
			};
			list.Add(item);
		}
		schema.Columns = list;
		return schema;
	}

	public override DataProviderResult<ExtendableRow> Execute(object param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		return new DataProviderResult<ExtendableRow>
		{
			Rows = _sourceRows
		};
	}

	public override Task<DataProviderResult<ExtendableRow>> ExecuteAsync(object param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
