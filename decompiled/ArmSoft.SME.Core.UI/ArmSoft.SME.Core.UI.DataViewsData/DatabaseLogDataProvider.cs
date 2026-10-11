using System;
using System.Collections.Generic;
using System.Linq;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Core.UI.DataViewsData;

public class DatabaseLogDataProvider : DataProvider<DatabaseLogDataProvider.DataRow, DatabaseLogDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey
	{
		public int LogId { get; set; }

		public DateTime Date { get; set; }

		public string DBName { get; set; }

		public string DBCaption { get; set; }

		public string UserName { get; set; }

		public string Key { get; set; }

		public string Description { get; set; }

		public int ProcessId { get; set; }

		public string Station { get; set; }

		public string AppType { get; set; }

		public string IPAddress { get; set; }

		public bool Matched(object key)
		{
			return LogId == (int)key;
		}
	}

	public class Param
	{
		public DateTime? DateBegin { get; set; }

		public DateTime? DateEnd { get; set; }

		public string DatabaseName { get; set; }

		public int UserId { get; set; }
	}

	public override string Name => "CloudDBLog";

	public DatabaseLogDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = DataProviderTypeSE.DatabaseLog.Caption
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "DBName",
				Caption = Resources.Database,
				Source = "DBName"
			},
			new Column
			{
				Name = "DBCaption",
				Caption = Resources.Name,
				Source = "DBCaption"
			},
			new Column
			{
				Name = "UserName",
				Caption = Resources.UserName,
				Source = "UserName"
			},
			new Column
			{
				Name = "Station",
				Caption = Resources.CompName,
				Source = "Station"
			},
			new Column
			{
				Name = "Date",
				Caption = Resources.DateWord,
				Source = "Date",
				Type = SupportedType.DateTime
			},
			new Column
			{
				Name = "Key",
				Caption = Resources.Key,
				Source = "Key"
			},
			new Column
			{
				Name = "Description",
				Caption = Resources.Description,
				Source = "Description"
			},
			new Column
			{
				Name = "AppType",
				Caption = Resources.ApplicationType,
				Source = "AppType"
			},
			new Column
			{
				Name = "ProcessId",
				Caption = Resources.ConnectionID,
				Source = "ProcessId"
			},
			new Column
			{
				Name = "IPAddress",
				Caption = Resources.IPAddress,
				Source = "IPAddress"
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
		using CloudClient cloudClient = ArmSoft.SME.Core.UI.Settings.CloudClient;
		cloudClient.Token = ArmSoft.SME.Core.UI.Settings.GetApplicationToken();
		foreach (CloudDBLogInfo item2 in from r in cloudClient.AS7X.GetCloudDBLogInfo(param.DateBegin, param.DateEnd)
			where (param.UserId == -1 || r.UserId == param.UserId) && (string.IsNullOrWhiteSpace(param.DatabaseName) || r.DBName == param.DatabaseName)
			select r)
		{
			DataRow dataRow = new DataRow();
			dataRow.LogId = item2.LogId;
			dataRow.Date = item2.Date;
			dataRow.DBName = item2.DBName;
			dataRow.DBCaption = item2.DBCaption;
			dataRow.Key = item2.KeyCode;
			dataRow.UserName = item2.UserName;
			dataRow.IPAddress = item2.IPAddress;
			dataRow.ProcessId = item2.ProcessId;
			dataRow.Station = item2.Station;
			DataRow dataRow2 = dataRow;
			dataRow2.Description = item2.Action switch
			{
				0 => Resources.Login, 
				1 => Resources.Login, 
				2 => Resources.Logout, 
				_ => Resources.Unknown, 
			};
			DataRow dataRow3 = dataRow;
			dataRow3.AppType = item2.Description switch
			{
				"Mobile" => Resources.MobileApp, 
				"Web" => Resources.WebClient, 
				"Other" => Resources.Other, 
				"Desktop" => "Desktop", 
				_ => "", 
			};
			DataRow item = dataRow;
			dataProviderResult.Rows.Add(item);
		}
		return dataProviderResult;
	}
}
