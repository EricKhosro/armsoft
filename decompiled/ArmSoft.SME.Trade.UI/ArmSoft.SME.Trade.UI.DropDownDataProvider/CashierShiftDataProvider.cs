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

public class CashierShiftDataProvider : DataProvider<CashierShiftDataProvider.DataRow, CashierShiftDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string CashDesk { get; set; }

		public string ShiftNumber { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public decimal InitialRemain { get; set; }
	}

	public class Param
	{
		public string CashDesk { get; set; }
	}

	public override string Name => "CashierShift";

	public CashierShiftDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.CashierShifts
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "CashDesk",
				Caption = ArmSoft.SME.Common.Resources.Cash,
				Source = "CashDesk"
			},
			new Column
			{
				Name = "ShiftNumber",
				Caption = ArmSoft.SME.Trade.Common.Resources.CashierShiftNum,
				Source = "ShiftNumber"
			},
			new Column
			{
				Name = "StartDate",
				Caption = ArmSoft.SME.Trade.Common.Resources.CashierShiftStart,
				Source = "StartDate",
				Type = SupportedType.DateTime
			},
			new Column
			{
				Name = "EndDate",
				Caption = ArmSoft.SME.Trade.Common.Resources.CashierShiftEnd,
				Source = "EndDate",
				Type = SupportedType.DateTimeNullable
			},
			new Column
			{
				Name = "InitialRemain",
				Caption = ArmSoft.SME.Trade.Common.Resources.InitRem,
				Source = "InitialRemain",
				Type = SupportedType.Decimal
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
			TradeSettings.TradeCacheService.CashierShift.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		IEnumerable<CashierShiftCacheRow> enumerable = TradeSettings.TradeCacheService.CashierShift.GetList();
		if (!string.IsNullOrWhiteSpace(param.CashDesk))
		{
			enumerable = enumerable.Where((CashierShiftCacheRow x) => x.CashDesk == param.CashDesk);
		}
		foreach (CashierShiftCacheRow item2 in enumerable)
		{
			DataRow item = new DataRow
			{
				CashDesk = item2.CashDesk,
				ShiftNumber = item2.ShiftNumber,
				StartDate = item2.StartDate,
				EndDate = item2.EndDate,
				InitialRemain = item2.InitialRemain
			};
			dataProviderResult.Rows.Add(item);
		}
		dataProviderResult.Rows.OrderBy((DataRow x) => x.ShiftNumber);
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
