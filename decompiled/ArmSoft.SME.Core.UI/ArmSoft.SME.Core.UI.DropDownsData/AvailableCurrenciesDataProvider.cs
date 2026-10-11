using System.Collections.Generic;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Data;
using CBA.ExchangeRates;

namespace ArmSoft.SME.Core.UI.DropDownsData;

public class AvailableCurrenciesDataProvider : DataProvider<AvailableCurrenciesDataProvider.DataRow, AvailableCurrenciesDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Curr { get; set; }

		public string Caption { get; set; }
	}

	public class Param
	{
	}

	public override string Name => "AvailableCurrenciesDataProvider";

	public AvailableCurrenciesDataProvider()
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
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		using GateSoapClient gateSoapClient = ArmSoft.SME.Core.UI.Settings.CBAExchangeRatesClient;
		string[] iSOCodesResult = gateSoapClient.ISOCodes(new ISOCodesRequest()).ISOCodesResult;
		foreach (CurrenciesCacheRow item in ArmSoft.SME.Core.UI.Settings.CoreCacheService.Currency.GetList())
		{
			string[] array = iSOCodesResult;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == item.Curr)
				{
					dataProviderResult.Rows.Add(new DataRow
					{
						Curr = item.Curr,
						Caption = item.Name
					});
				}
			}
		}
		return dataProviderResult;
	}
}
