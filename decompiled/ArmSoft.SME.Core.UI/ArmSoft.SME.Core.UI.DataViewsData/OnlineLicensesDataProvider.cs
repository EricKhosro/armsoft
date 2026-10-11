using System;
using System.Collections.Generic;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Core.UI.DataViewsData;

public class OnlineLicensesDataProvider : DataProvider<OnlineLicensesDataProvider.DataRow, OnlineLicensesDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey
	{
		public string Station { get; set; }

		public string IPAddress { get; set; }

		public int TerminalId { get; set; }

		public string LicenseId { get; set; }

		public int ProcessCount { get; set; }

		public DateTime LastRenewed { get; set; }

		public string Info { get; set; }

		public bool Matched(object key)
		{
			return (string)key == LicenseId;
		}
	}

	public class Param
	{
	}

	public override string Name => "OnlineLicenses";

	public OnlineLicensesDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = DataProviderTypeSE.OnlineLicenses.Caption
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "LicenseId",
				Caption = Resources.License,
				Source = "LicenseId"
			},
			new Column
			{
				Name = "Station",
				Caption = Resources.Workstation,
				Source = "Station"
			},
			new Column
			{
				Name = "TerminalId",
				Caption = Resources.Terminal,
				Source = "TerminalId"
			},
			new Column
			{
				Name = "ProcessCount",
				Caption = Resources.RegisteredProcessesCount,
				Source = "ProcessCount"
			},
			new Column
			{
				Name = "LastRenewed",
				Caption = Resources.RenewDate,
				Source = "LastRenewed"
			},
			new Column
			{
				Name = "IPAddress",
				Caption = Resources.IPAddress,
				Source = "IPAddress"
			},
			new Column
			{
				Name = "Info",
				Caption = Resources.AdditionalInfo,
				Source = "Info"
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
		if (keys == null)
		{
			List<OnlineLicenseInfo> list = null;
			using (CloudClient cloudClient = ArmSoft.SME.Core.UI.Settings.CloudClient)
			{
				cloudClient.Token = ArmSoft.SME.Core.UI.Settings.GetApplicationToken();
				list = cloudClient.AS7X.GetOnlineLicenses((Guid?)null);
			}
			if (list != null)
			{
				foreach (OnlineLicenseInfo item in list)
				{
					dataProviderResult.Rows.Add(CreateRow(item));
				}
			}
		}
		else
		{
			List<OnlineLicenseInfo> list2 = null;
			using (CloudClient cloudClient2 = ArmSoft.SME.Core.UI.Settings.CloudClient)
			{
				cloudClient2.Token = ArmSoft.SME.Core.UI.Settings.GetApplicationToken();
				list2 = cloudClient2.AS7X.GetOnlineLicenses(Guid.Parse(keys[0].ToString()));
			}
			if (list2 != null && list2.Count > 0)
			{
				dataProviderResult.Rows.Add(CreateRow(list2[0]));
			}
		}
		return dataProviderResult;
	}

	private static DataRow CreateRow(OnlineLicenseInfo row)
	{
		return new DataRow
		{
			LicenseId = row.LicenseId.ToString(),
			Station = row.Station,
			IPAddress = row.IPAddress,
			Info = row.Info,
			LastRenewed = row.LastRenewed,
			TerminalId = row.TerminalId,
			ProcessCount = row.ProcessCount
		};
	}
}
