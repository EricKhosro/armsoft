using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class GoodReportsDataProvider : DataProvider<GoodReportsDataProvider.DataRow, GoodReportsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public short Code { get; set; }

		public string Caption { get; set; }
	}

	public class Param
	{
	}

	public override string Name => "GoodReports";

	public GoodReportsDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Common.Resources.Report
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
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		if (ArmSoft.SME.Core.UI.Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepSalesAnalysis).AllowView)
		{
			dataProviderResult.Rows.Add(new DataRow
			{
				Code = 10057,
				Caption = ArmSoft.SME.Trade.Common.Resources.MTRepSalesAnalysis
			});
		}
		if (ArmSoft.SME.Core.UI.Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepAvailability).AllowView)
		{
			dataProviderResult.Rows.Add(new DataRow
			{
				Code = 10049,
				Caption = ArmSoft.SME.Trade.Common.Resources.MatRepAvail
			});
		}
		if (ArmSoft.SME.Core.UI.Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.GoodsTurnOver).AllowView)
		{
			dataProviderResult.Rows.Add(new DataRow
			{
				Code = 10041,
				Caption = ArmSoft.SME.Trade.Common.Resources.MatRepTurnOver
			});
		}
		if (ArmSoft.SME.Core.UI.Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.GoodsRepOperations).AllowView)
		{
			dataProviderResult.Rows.Add(new DataRow
			{
				Code = 10067,
				Caption = ArmSoft.SME.Trade.Common.Resources.MTOperationsJournal
			});
		}
		if (ArmSoft.SME.Core.UI.Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepTAccount).AllowView)
		{
			dataProviderResult.Rows.Add(new DataRow
			{
				Code = 10085,
				Caption = ArmSoft.SME.Trade.Common.Resources.TAccount
			});
		}
		if (ArmSoft.SME.Core.UI.Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepAvailabilityRetail).AllowView)
		{
			dataProviderResult.Rows.Add(new DataRow
			{
				Code = 10045,
				Caption = ArmSoft.SME.Trade.Common.Resources.MTRepRetailAvail
			});
		}
		if (ArmSoft.SME.Core.UI.Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepTurnOverRetail).AllowView)
		{
			dataProviderResult.Rows.Add(new DataRow
			{
				Code = 10048,
				Caption = ArmSoft.SME.Trade.Common.Resources.MTRepRetailTurn
			});
		}
		if (ArmSoft.SME.Core.UI.Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepOperationsRetail).AllowView)
		{
			dataProviderResult.Rows.Add(new DataRow
			{
				Code = 10059,
				Caption = ArmSoft.SME.Trade.Common.Resources.MTRepRetailOpJournal
			});
		}
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
