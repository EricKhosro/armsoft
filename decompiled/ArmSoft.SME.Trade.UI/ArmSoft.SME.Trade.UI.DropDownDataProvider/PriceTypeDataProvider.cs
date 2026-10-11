using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class PriceTypeDataProvider : DataProvider<PriceTypeDataProvider.DataRow, PriceTypeDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public string CurrCode { get; set; }

		public string Round { get; set; }

		public bool VAT { get; set; }

		public byte[] TS { get; set; }
	}

	public class Param
	{
		public string CurrCode { get; set; }
	}

	public override string Name => "PriceType";

	public PriceTypeDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.PriceType
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
				Name = "Caption",
				Caption = ArmSoft.SME.Common.Resources.Name,
				Source = "Caption"
			},
			new Column
			{
				Name = "CurrCode",
				Caption = ArmSoft.SME.Common.Resources.Curr,
				Source = "CurrCode"
			},
			new Column
			{
				Name = "Round",
				Caption = ArmSoft.SME.Common.Resources.DegreeOfRound,
				Source = "Round"
			},
			new Column
			{
				Name = "VAT",
				Caption = ArmSoft.SME.Common.Resources.CostWithInVAT,
				Source = "VAT",
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
		if (settings != null && settings.ReloadData)
		{
			TradeSettings.TradeCacheService.PriceType.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		IEnumerable<PriceTypeCacheRow> enumerable = TradeSettings.TradeCacheService.PriceType.GetList();
		if (!string.IsNullOrWhiteSpace(param.CurrCode))
		{
			enumerable = enumerable.Where((PriceTypeCacheRow p) => param.CurrCode.Contains(p.CurrCode, StringComparison.CurrentCultureIgnoreCase));
		}
		foreach (PriceTypeCacheRow item2 in enumerable)
		{
			DataRow item = new DataRow
			{
				Code = item2.Code,
				Caption = item2.Caption,
				CurrCode = item2.CurrCode,
				Round = item2.Round,
				VAT = item2.VAT,
				TS = item2.TS
			};
			dataProviderResult.Rows.Add(item);
		}
		dataProviderResult.Rows = dataProviderResult.Rows.OrderBy((DataRow x) => x.Code).ToList();
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
