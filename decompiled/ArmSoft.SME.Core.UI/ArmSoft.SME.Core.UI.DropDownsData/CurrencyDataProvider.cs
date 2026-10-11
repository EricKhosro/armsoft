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

public class CurrencyDataProvider : DataProvider<CurrencyDataProvider.DataRow, CurrencyDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Curr { get; set; }

		public string Caption { get; set; }

		public string UnitName { get; set; }

		public string CentName { get; set; }
	}

	public class Param
	{
		public bool WithOutNCC { get; set; }

		public string Currency { get; set; }
	}

	public override string Name => "Currency";

	public CurrencyDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = Resources.Currencies
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "Curr",
				Caption = Resources.Code,
				Source = "Curr"
			},
			new Column
			{
				Name = "Caption",
				Caption = Resources.Name,
				Source = "Caption"
			},
			new Column
			{
				Name = "UnitName",
				Caption = Resources.CurrencyUnit,
				Source = "UnitName"
			},
			new Column
			{
				Name = "CentName",
				Caption = Resources.SmallChange,
				Source = "CentName"
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
			ArmSoft.SME.Core.UI.Settings.CoreCacheService.Currency.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		IReadOnlyList<CurrenciesCacheRow> readOnlyList = ArmSoft.SME.Core.UI.Settings.CoreCacheService.Currency.GetList();
		if (param.WithOutNCC)
		{
			List<CurrenciesCacheRow> list = new List<CurrenciesCacheRow>();
			list.AddRange(readOnlyList.Where((CurrenciesCacheRow p) => p.Curr != "AMD"));
			readOnlyList = new _003C_003Ez__ReadOnlyList<CurrenciesCacheRow>(list);
		}
		if (!string.IsNullOrWhiteSpace(param.Currency))
		{
			List<CurrenciesCacheRow> list2 = new List<CurrenciesCacheRow>();
			list2.AddRange(readOnlyList.Where((CurrenciesCacheRow p) => param.Currency.Contains(p.Curr, StringComparison.CurrentCulture)));
			readOnlyList = new _003C_003Ez__ReadOnlyList<CurrenciesCacheRow>(list2);
		}
		foreach (CurrenciesCacheRow item2 in readOnlyList)
		{
			DataRow item = new DataRow
			{
				Curr = item2.Curr,
				Caption = item2.Name,
				UnitName = item2.UnitName,
				CentName = item2.CentName
			};
			dataProviderResult.Rows.Add(item);
		}
		dataProviderResult.Rows = dataProviderResult.Rows.OrderBy((DataRow x) => x.Curr).ToList();
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
