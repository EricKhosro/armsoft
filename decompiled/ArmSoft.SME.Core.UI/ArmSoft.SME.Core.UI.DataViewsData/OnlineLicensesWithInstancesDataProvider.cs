using System;
using System.Collections.Generic;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Core.UI.DataViewsData;

public class OnlineLicensesWithInstancesDataProvider : DataProvider<OnlineLicensesWithInstancesDataProvider.DataRow, OnlineLicensesWithInstancesDataProvider.Param>
{
	public class Instance
	{
		public int ProcessId { get; set; }

		public string ServerName { get; set; }

		public string DBName { get; set; }

		public string User { get; set; }

		public string Version { get; set; }

		public DateTime Renewed { get; set; }

		public Instance(OnlineLicenseInstance row)
		{
			ProcessId = row.ProcessId;
			ServerName = row.ServerName;
			DBName = row.DBName;
			User = row.User;
			Version = row.Version;
			Renewed = row.Renewed;
		}
	}

	public class DataRow : ExtendableRow
	{
		public string Station { get; set; }

		public string IPAddress { get; set; }

		public int TerminalId { get; set; }

		public Guid LicenseId { get; set; }

		public int ProcessCount { get; set; }

		public DateTime LastRenewed { get; set; }

		public string User { get; set; }

		public string Info { get; set; }

		public string Remove { get; set; }

		public List<Instance> Instances { get; set; }
	}

	public class Param
	{
		public string Key { get; set; }

		public string UserToken { get; set; }
	}

	public override string Name => "OnlineLicensesWithInstances";

	public OnlineLicensesWithInstancesDataProvider()
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
				Name = "Station",
				Caption = Resources.Workstation,
				Source = "Station"
			},
			new Column
			{
				Name = "IPAddress",
				Caption = Resources.IPAddress,
				Source = "IPAddress"
			},
			new Column
			{
				Name = "LastRenewed",
				Caption = Resources.RenewDate,
				Source = "LastRenewed"
			},
			new Column
			{
				Name = "User",
				Caption = Resources.UserName,
				Source = "User"
			},
			new Column
			{
				Name = "Remove",
				Caption = Resources.Delete,
				Source = "Remove"
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
		List<OnlineLicenseInfoWithInstances> list = null;
		using (CloudClient cloudClient = new CloudClient("", ArmSoft.SME.Core.UI.Settings.B2BCloudBaseAddress))
		{
			cloudClient.UserToken = param.UserToken;
			list = cloudClient.AS7X.GetOnlineLicensesWithInstances(param.Key);
		}
		if (list != null)
		{
			foreach (OnlineLicenseInfoWithInstances item in list)
			{
				dataProviderResult.Rows.Add(CreateRow(item));
			}
		}
		return dataProviderResult;
	}

	private static DataRow CreateRow(OnlineLicenseInfoWithInstances row)
	{
		DataRow dataRow = new DataRow
		{
			Station = row.Station,
			IPAddress = row.IPAddress,
			TerminalId = row.TerminalId,
			LicenseId = row.LicenseId,
			ProcessCount = row.ProcessCount,
			LastRenewed = row.LastRenewed,
			Info = row.Info,
			Remove = Resources.Delete,
			Instances = new List<Instance>()
		};
		foreach (OnlineLicenseInstance instance in row.Instances)
		{
			dataRow.Instances.Add(new Instance(instance));
		}
		bool flag = true;
		if (dataRow.Instances.Count > 1 && dataRow.Instances.Count != 0)
		{
			for (int i = 0; i < dataRow.Instances.Count - 2; i++)
			{
				if (dataRow.Instances[i].User != dataRow.Instances[i + 1].User)
				{
					flag = false;
				}
			}
			if (flag)
			{
				dataRow.User = dataRow.Instances[0].User;
			}
		}
		else if (dataRow.Instances.Count == 1)
		{
			dataRow.User = dataRow.Instances[0].User;
		}
		return dataRow;
	}
}
