using System.Collections.Generic;
using System.Runtime.InteropServices;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Core.UI.DropDownsData;

public class SingleElementsDataProvider : DataProvider<SingleElementsDataProvider.DataRow, ExtendableParameter>
{
	public class DataRow : ExtendableRow
	{
		public string Name { get; set; }
	}

	private readonly string _caption;

	private readonly IEnumerable<string> _items;

	public override string Name => "SingleElements";

	public SingleElementsDataProvider(string caption, IEnumerable<string> items)
		: base((ApiClient)null)
	{
		_caption = caption;
		_items = items;
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = _caption
		};
		int num = 1;
		List<Column> list = new List<Column>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = new Column
		{
			Name = "Name",
			Caption = Resources.Code,
			Source = "Name"
		};
		schema.Columns = list;
		return schema;
	}

	public override DataProviderResult<DataRow> Execute(ExtendableParameter param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		foreach (string item in _items)
		{
			dataProviderResult.Rows.Add(new DataRow
			{
				Name = item
			});
		}
		return dataProviderResult;
	}
}
