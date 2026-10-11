using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class EmployeeDataProvider : DataProvider<EmployeeDataProvider.DataRow, EmployeeDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public byte[] TS = TimeStamp.NewTS();

		public int EmployeeID { get; set; }

		public string EmployeeCode { get; set; }

		public string Caption { get; set; }

		public string Address { get; set; }

		public string Phone { get; set; }

		public string Passport { get; set; }

		public string EmployeeCard { get; set; }

		public bool IsSalesConsultant { get; set; }
	}

	public class Param
	{
		public bool IsOnlySalesConsultant { get; set; }
	}

	public override string Name => "Employee";

	public EmployeeDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Common.Resources.Employee
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "EmployeeCode",
				Caption = ArmSoft.SME.Common.Resources.Code,
				Source = "EmployeeCode"
			},
			new Column
			{
				Name = "Caption",
				Caption = ArmSoft.SME.Common.Resources.FIO,
				Source = "Caption"
			},
			new Column
			{
				Name = "Address",
				Caption = ArmSoft.SME.Common.Resources.Address,
				Source = "Address"
			},
			new Column
			{
				Name = "Phone",
				Caption = ArmSoft.SME.Common.Resources.Phone,
				Source = "Phone"
			},
			new Column
			{
				Name = "Passport",
				Caption = ArmSoft.SME.Common.Resources.Passport,
				Source = "Passport"
			},
			new Column
			{
				Name = "EmployeeCard",
				Caption = ArmSoft.SME.Trade.Common.Resources.DiscountCardCode,
				Source = "EmployeeCard"
			},
			new Column
			{
				Name = "IsSalesConsultant",
				Caption = ArmSoft.SME.Trade.Common.Resources.SalesConsultant,
				Source = "IsSalesConsultant",
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
			TradeSettings.TradeCacheService.Employee.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		IEnumerable<EmployeeCacheRow> enumerable = TradeSettings.TradeCacheService.Employee.GetList();
		if (param.IsOnlySalesConsultant)
		{
			enumerable = enumerable.Where((EmployeeCacheRow p) => p.IsSalesConsultant);
		}
		foreach (EmployeeCacheRow item2 in enumerable)
		{
			DataRow item = new DataRow
			{
				EmployeeID = item2.Id,
				EmployeeCode = item2.EmployeeCode,
				Caption = item2.Caption,
				Address = item2.Address,
				EmployeeCard = item2.EmployeeCard,
				Phone = item2.Phone,
				Passport = item2.Passport,
				IsSalesConsultant = item2.IsSalesConsultant,
				TS = item2.TS
			};
			dataProviderResult.Rows.Add(item);
		}
		dataProviderResult.Rows = dataProviderResult.Rows.OrderBy((DataRow x) => x.EmployeeCode).ToList();
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
