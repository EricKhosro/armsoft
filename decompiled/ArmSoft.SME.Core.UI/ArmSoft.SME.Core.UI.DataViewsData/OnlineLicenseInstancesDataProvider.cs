using System;
using System.Collections.Generic;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Core.UI.DataViewsData;

public class OnlineLicenseInstancesDataProvider : DataProvider<OnlineLicenseInstancesDataProvider.DataRow, OnlineLicenseInstancesDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int ProcessId { get; set; }

		public string ServerName { get; set; }

		public string DBName { get; set; }

		public string User { get; set; }

		public string Version { get; set; }

		public DateTime Renewed { get; set; }
	}

	public class Param
	{
		public Guid LicenseId { get; set; }
	}

	public override string Name => "OnlineLicensesInstances";

	public OnlineLicenseInstancesDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = Resources.OnlineLicenses
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "ProcessId",
				Caption = Resources.ProcessId,
				Source = "ProcessId"
			},
			new Column
			{
				Name = "DBName",
				Caption = Resources.Database,
				Source = "DBName"
			},
			new Column
			{
				Name = "User",
				Caption = Resources.UserName,
				Source = "User"
			},
			new Column
			{
				Name = "Renewed",
				Caption = Resources.LastRenew,
				Source = "Renewed"
			},
			new Column
			{
				Name = "ServerName",
				Caption = Resources.Server,
				Source = "ServerName"
			},
			new Column
			{
				Name = "Version",
				Caption = Resources.VersionCapital,
				Source = "Version"
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
		List<OnlineLicenseInstance> list = null;
		using (CloudClient cloudClient = ArmSoft.SME.Core.UI.Settings.CloudClient)
		{
			cloudClient.Token = ArmSoft.SME.Core.UI.Settings.GetApplicationToken();
			list = cloudClient.AS7X.GetOnlineLicenseInstances(param.LicenseId);
		}
		if (list != null)
		{
			foreach (OnlineLicenseInstance item in list)
			{
				dataProviderResult.Rows.Add(CreateRow(item));
			}
		}
		return dataProviderResult;
	}

	private static DataRow CreateRow(OnlineLicenseInstance row)
	{
		return new DataRow
		{
			DBName = row.DBName,
			User = row.User,
			ProcessId = row.ProcessId,
			Renewed = row.Renewed,
			ServerName = row.ServerName,
			Version = row.Version
		};
	}
}
