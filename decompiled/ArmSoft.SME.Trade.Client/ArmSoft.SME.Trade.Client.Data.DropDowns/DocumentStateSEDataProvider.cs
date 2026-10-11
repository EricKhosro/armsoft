using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Client.Data.DropDowns;

public class DocumentStateSEDataProvider : DataProvider<DocumentStateSEDataProvider.DataRow, DocumentStateSEDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public string ValueUI { get; set; }
	}

	public class Param
	{
	}

	public override string Name => "DocumentState";

	public DocumentStateSEDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		return new Schema
		{
			Name = Name,
			Caption = Resources.DocumentStates,
			Columns = GenerateColumns()
		};
	}

	public override Task<Schema> GetSchemaAsync(Param param, ExecutionSettings settings, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	protected static List<Column> GenerateColumns()
	{
		return new List<Column>
		{
			new Column
			{
				Name = "Code",
				Caption = Resources.Code,
				Source = "Code"
			},
			new Column
			{
				Name = "ValueUI",
				Caption = Resources.State,
				Source = "ValueUI"
			},
			new Column
			{
				Name = "Caption",
				Caption = Resources.Comment,
				Source = "Caption"
			}
		};
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		foreach (DocumentStateSE item in SmartEnum<DocumentStateSE, byte>.List)
		{
			dataProviderResult.Rows.Add(new DataRow
			{
				Code = item.Value.ToString(),
				ValueUI = item.ValueUI,
				Caption = item.Caption
			});
		}
		dataProviderResult.Rows = dataProviderResult.Rows.OrderBy((DataRow x) => x.Code).ToList();
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
