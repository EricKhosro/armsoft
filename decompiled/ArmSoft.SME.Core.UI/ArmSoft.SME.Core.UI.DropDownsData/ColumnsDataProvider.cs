using System.Collections.Generic;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Core.UI.DropDownsData;

public class ColumnsDataProvider : CodeCaptionPairsDataProvider
{
	private readonly Dictionary<string, string> _columns;

	public override string Name => "Columns";

	public ColumnsDataProvider(Dictionary<string, string> columns)
		: base(null)
	{
		_columns = columns;
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = Resources.Columns
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
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		foreach (string key in _columns.Keys)
		{
			dataProviderResult.Rows.Add(new DataRow
			{
				Code = key,
				Caption = _columns[key]
			});
		}
		return dataProviderResult;
	}
}
