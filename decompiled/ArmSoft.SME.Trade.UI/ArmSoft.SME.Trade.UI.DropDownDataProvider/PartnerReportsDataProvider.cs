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

public class PartnerReportsDataProvider : DataProvider<PartnerReportsDataProvider.DataRow, PartnerReportsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public short Code { get; set; }

		public string Caption { get; set; }
	}

	public class Param
	{
	}

	public override string Name => "PartnerReports";

	public PartnerReportsDataProvider()
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
		if (ArmSoft.SME.Core.UI.Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.PartRemains).AllowView)
		{
			dataProviderResult.Rows.Add(new DataRow
			{
				Code = 10051,
				Caption = ArmSoft.SME.Trade.Common.Resources.PartnerRemains
			});
		}
		if (ArmSoft.SME.Core.UI.Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.PartnersTurnOver).AllowView)
		{
			dataProviderResult.Rows.Add(new DataRow
			{
				Code = 10066,
				Caption = ArmSoft.SME.Trade.Common.Resources.PartnerTurn
			});
		}
		if (ArmSoft.SME.Core.UI.Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.PartOpJournal).AllowView)
		{
			dataProviderResult.Rows.Add(new DataRow
			{
				Code = 10052,
				Caption = ArmSoft.SME.Trade.Common.Resources.PartnerOpJournal
			});
		}
		if (ArmSoft.SME.Core.UI.Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.PartRepCard).AllowView)
		{
			dataProviderResult.Rows.Add(new DataRow
			{
				Code = 10070,
				Caption = ArmSoft.SME.Trade.Common.Resources.PartnerCard
			});
		}
		if (ArmSoft.SME.Core.UI.Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepSalesAnalysis).AllowView)
		{
			dataProviderResult.Rows.Add(new DataRow
			{
				Code = 10057,
				Caption = ArmSoft.SME.Trade.Common.Resources.MTRepSalesAnalysis
			});
		}
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
