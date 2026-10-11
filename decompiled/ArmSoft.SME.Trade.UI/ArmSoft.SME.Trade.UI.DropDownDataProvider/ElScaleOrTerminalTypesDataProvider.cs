using System.Collections.Generic;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Common;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class ElScaleOrTerminalTypesDataProvider : DataProvider<ElScaleOrTerminalTypesDataProvider.DataRow, ElScaleOrTerminalTypesDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Code { get; set; }

		public string Caption { get; set; }
	}

	public class Param
	{
		public Dictionary<string, string> Source;
	}

	public override string Name => "ElScaleTypes";

	public ElScaleOrTerminalTypesDataProvider(ApiClient client)
		: base(client)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.DeviceModel
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "Code",
				Caption = ArmSoft.SME.Common.Resources.Code,
				Source = "Code",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "Caption",
				Caption = ArmSoft.SME.Common.Resources.Name,
				Source = "Caption",
				Type = SupportedType.String
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		foreach (KeyValuePair<string, string> item2 in param.Source)
		{
			DataRow item = new DataRow
			{
				Code = item2.Key,
				Caption = item2.Value
			};
			dataProviderResult.Rows.Add(item);
		}
		return dataProviderResult;
	}
}
