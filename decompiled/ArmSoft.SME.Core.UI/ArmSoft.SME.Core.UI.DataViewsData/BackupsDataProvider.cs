using System;
using System.Collections.Generic;
using System.Linq;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.Edge;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Cloud;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Helpers;

namespace ArmSoft.SME.Core.UI.DataViewsData;

public class BackupsDataProvider : DataProvider<BackupsDataProvider.DataRow, BackupsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey
	{
		public int DbId { get; set; }

		public string DbName { get; set; } = "";

		public string DbCaption { get; set; } = "";

		public string DbVersion { get; set; } = "";

		public string SQLServerVersion { get; set; } = "";

		public decimal CompressedSize { get; set; }

		public decimal DbSize { get; set; }

		public decimal Size { get; set; }

		public string Comment { get; set; }

		public DateTime DateCreated { get; set; }

		public int UserId { get; set; }

		public string UserName { get; set; } = "";

		public string FileName { get; set; } = "";

		public bool IsAutoBackup { get; set; }

		public bool IsDailyBackup { get; set; }

		public bool IsMonthlyBackup { get; set; }

		public bool IsAnnualBackup { get; set; }

		public bool CreatedByASUser { get; set; }

		public bool Matched(object key)
		{
			return (string)key == FileName;
		}
	}

	public class Param
	{
		public string ServiceAddress { get; set; }

		public string Mode { get; set; }
	}

	public override string Name => "CloudBackups";

	public BackupsDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = DataProviderTypeSE.Backups.Caption
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "DbName",
				Caption = Resources.Database,
				Source = "DbName"
			},
			new Column
			{
				Name = "DbCaption",
				Caption = Resources.Name,
				Source = "DbCaption"
			},
			new Column
			{
				Name = "Comment",
				Caption = Resources.Comment,
				Source = "Comment"
			},
			new Column
			{
				Name = "DbSize",
				Caption = Resources.DBSize,
				Source = "DbSize",
				Type = SupportedType.Decimal,
				Format = DecimalExtension.GetCurrencyFormat(16, 2)
			},
			new Column
			{
				Name = "Size",
				Caption = Resources.SizeMB,
				Source = "Size",
				Type = SupportedType.Decimal,
				Format = DecimalExtension.GetCurrencyFormat(16, 2)
			},
			new Column
			{
				Name = "CompressedSize",
				Caption = Resources.CompressedSizeMB,
				Source = "CompressedSize",
				Type = SupportedType.Decimal,
				Format = DecimalExtension.GetCurrencyFormat(16, 2)
			},
			new Column
			{
				Name = "DateCreated",
				Caption = Resources.DateWord,
				Source = "DateCreated",
				Type = SupportedType.DateTime
			},
			new Column
			{
				Name = "UserName",
				Caption = Resources.UserName,
				Source = "UserName"
			},
			new Column
			{
				Name = "IsAutoBackup",
				Caption = Resources.System,
				Source = "IsAutoBackup",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "IsDailyBackup",
				Caption = Resources.Daily,
				Source = "IsDailyBackup",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "IsMonthlyBackup",
				Caption = Resources.PeriodicityMonthly,
				Source = "IsMonthlyBackup",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "IsAnnualBackup",
				Caption = Resources.Annual,
				Source = "IsAnnualBackup",
				Type = SupportedType.Boolean
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
		List<BackupInfo> list = null;
		List<BackupInfoEx> list2 = null;
		if (keys != null && keys.Count > 1)
		{
			throw new ASException(Resources.NotSupportSeveralKeys);
		}
		string fileName = ((keys != null) ? ((string)keys[0]) : "");
		if (ConfigHelper.ASAdminLogin)
		{
			using EdgeAdminClient edgeAdminClient = new EdgeAdminClient(ArmSoft.SME.Core.UI.Settings.LoginService.EdgeServiceAddress, ADHelper.ASAdminB2CToken);
			if (param.Mode == BackupModeSE.OldMode.Value)
			{
				list = edgeAdminClient.GetBackupsInfo(ArmSoft.SME.Core.UI.Settings.BackupExtension, ArmSoft.SME.Core.UI.Settings.LoginService.Key, fileName);
			}
			else
			{
				int serverId = DatabaseHelper.GetCloudDatabase(ArmSoft.SME.Core.UI.Settings.LoginService.DatabaseId).ServerId;
				list2 = edgeAdminClient.GetBackupsInfoEx(serverId, ArmSoft.SME.Core.UI.Settings.LoginService.Key, fileName);
			}
		}
		else
		{
			using EdgeClient edgeClient = new EdgeClient(ArmSoft.SME.Core.UI.Settings.LoginService.EdgeServiceAddress, ArmSoft.SME.Core.UI.Settings.GetApplicationToken(), ADHelper.ADB2CToken);
			if (param.Mode == BackupModeSE.OldMode.Value)
			{
				list = edgeClient.GetBackupsInfo(ArmSoft.SME.Core.UI.Settings.BackupExtension, fileName);
			}
			else
			{
				int serverId2 = DatabaseHelper.GetCloudDatabase(ArmSoft.SME.Core.UI.Settings.LoginService.DatabaseId).ServerId;
				list2 = edgeClient.GetBackupsInfoEx(serverId2, fileName);
			}
		}
		if (list != null)
		{
			List<DatabaseInfo> cloudDatabases = DatabaseHelper.GetCloudDatabases();
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			if (cloudDatabases != null)
			{
				dictionary = cloudDatabases.ToDictionary((DatabaseInfo d) => d.DbId, (DatabaseInfo d) => d.Caption);
			}
			foreach (BackupInfo item in list)
			{
				dataProviderResult.Rows.Add(new DataRow
				{
					DbId = item.DbId,
					DbName = item.DbName,
					DbVersion = item.DbVersion,
					DbCaption = ((item.DbId != -1 && dictionary.TryGetValue(item.DbId, out var value)) ? value : ""),
					CompressedSize = ((item.CompressedSize != 0L) ? DecimalExtension.RoundAs(item.CompressedSize / 1048576, 2) : 0m),
					Size = ((item.UncompressedSize != 0L) ? DecimalExtension.RoundAs(item.UncompressedSize / 1048576, 2) : 0m),
					Comment = ((item.DbId != -1) ? item.Comment : item.Comment),
					DateCreated = item.DateCreated,
					UserId = item.UserId,
					UserName = ((item.UserId == 0) ? "ArmSoft Admin" : ((item.UserId != -1) ? ArmSoft.SME.Core.UI.Settings.CoreCacheService.User.GetNameById(item.UserId) : item.UserName)),
					FileName = item.FileName
				});
			}
		}
		if (list2 != null)
		{
			List<DatabaseInfo> cloudDatabases2 = DatabaseHelper.GetCloudDatabases();
			Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
			if (cloudDatabases2 != null)
			{
				dictionary2 = cloudDatabases2.ToDictionary((DatabaseInfo d) => d.DbId, (DatabaseInfo d) => d.Caption);
			}
			foreach (BackupInfoEx item2 in list2.Where((BackupInfoEx r) => r.PSMode == ConfigHelper.IsPublicSector))
			{
				dataProviderResult.Rows.Add(new DataRow
				{
					DbId = item2.DbId,
					DbName = item2.DbName,
					DbVersion = item2.DbVersion,
					DbCaption = ((item2.DbId != -1 && dictionary2.TryGetValue(item2.DbId, out var value2)) ? value2 : ""),
					DbSize = ((item2.DbSize != 0L) ? DecimalExtension.RoundAs(item2.DbSize / 1048576, 2) : 0m),
					Size = ((item2.FileLength != 0m) ? (item2.FileLength / 1048576m).RoundAs(2) : 0m),
					Comment = ((item2.DbId != -1) ? item2.Comment : item2.Comment),
					DateCreated = item2.DateCreated,
					IsAutoBackup = item2.IsAutoBackup,
					UserName = item2.Creator,
					CreatedByASUser = item2.CreatedByASUser,
					FileName = item2.FileName,
					IsDailyBackup = item2.IsDailyBackup,
					IsMonthlyBackup = item2.IsMonthlyBackup,
					IsAnnualBackup = item2.IsAnnualBackup
				});
			}
		}
		return dataProviderResult;
	}
}
