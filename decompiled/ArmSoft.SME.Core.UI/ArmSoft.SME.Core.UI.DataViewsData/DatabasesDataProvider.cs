using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.Edge;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Cloud;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Helpers;

namespace ArmSoft.SME.Core.UI.DataViewsData;

public class DatabasesDataProvider : DataProvider<DatabasesDataProvider.DataRow, DatabasesDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey
	{
		public int DbId { get; set; }

		public string Name { get; set; }

		public string Caption { get; set; }

		public DateTime? LastBackup { get; set; }

		public string Version { get; set; }

		public string TaxCode { get; set; }

		public string CompanyName { get; set; }

		public decimal Size { get; set; }

		public decimal FreeSpace { get; set; }

		public string ServiceAddress { get; set; }

		public bool DailyBackupEnabled { get; set; }

		public short DailyBackupCount { get; set; }

		public bool MonthlyBackupEnabled { get; set; }

		public short MonthlyBackupDay { get; set; }

		public short MonthlyBackupCount { get; set; }

		public bool AnnualBackupEnabled { get; set; }

		public short AnnualBackupDay { get; set; }

		public short AnnualBackupMonth { get; set; }

		public short AnnualBackupCount { get; set; }

		public bool CreateOnExistedLogin { get; set; }

		public string BackupPolicy { get; set; }

		public DataRow()
		{
		}

		public DataRow(DatabaseInfo cloudDatabase)
		{
			DbId = cloudDatabase.DbId;
			Name = cloudDatabase.DbName;
			Caption = cloudDatabase.Caption;
			LastBackup = cloudDatabase.LastBackup;
			Version = cloudDatabase.Version;
			TaxCode = cloudDatabase.TaxCode;
			CompanyName = cloudDatabase.CompanyName;
			Size = 0m;
			FreeSpace = 0m;
			ServiceAddress = cloudDatabase.ServiceAddress;
			DailyBackupEnabled = cloudDatabase.DailyBackupEnabled;
			DailyBackupCount = cloudDatabase.DailyBackupCount;
			MonthlyBackupEnabled = cloudDatabase.MonthlyBackupEnabled;
			MonthlyBackupDay = cloudDatabase.MonthlyBackupDay;
			MonthlyBackupCount = cloudDatabase.MonthlyBackupCount;
			AnnualBackupEnabled = cloudDatabase.AnnualBackupEnabled;
			AnnualBackupDay = cloudDatabase.AnnualBackupDay;
			AnnualBackupMonth = cloudDatabase.AnnualBackupMonth;
			AnnualBackupCount = cloudDatabase.AnnualBackupCount;
			CreateOnExistedLogin = cloudDatabase.CreateOnExistedLogin;
			BackupPolicy = GetBackupPolicyText(DailyBackupEnabled, DailyBackupCount, MonthlyBackupEnabled, MonthlyBackupDay, MonthlyBackupCount, AnnualBackupEnabled, AnnualBackupDay, AnnualBackupMonth, AnnualBackupCount, CreateOnExistedLogin);
		}

		public bool Matched(object key)
		{
			return DbId == (int)key;
		}
	}

	public class Param
	{
		public string ServiceAddress { get; set; }

		public bool ShowDatabasesSizes { get; set; }

		public bool ShowAutoBackupOptions { get; set; }
	}

	public override string Name => "CloudDatabases";

	public DatabasesDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = DataProviderTypeSE.Databases.Caption
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "Name",
				Caption = Resources.Name,
				Source = "Name"
			},
			new Column
			{
				Name = "Caption",
				Caption = Resources.Comment,
				Source = "Caption"
			},
			new Column
			{
				Name = "TaxCode",
				Caption = Resources.OptionsTaxCode,
				Source = "TaxCode"
			},
			new Column
			{
				Name = "CompanyName",
				Caption = Resources.CompanyName,
				Source = "CompanyName"
			},
			new Column
			{
				Name = "Size",
				Caption = Resources.Size + " (MB)",
				Source = "Size",
				Type = SupportedType.Decimal,
				FooterSum = true
			},
			new Column
			{
				Name = "FreeSpace",
				Caption = Resources.UnallocSpace + " (MB)",
				Source = "FreeSpace",
				Type = SupportedType.Decimal,
				FooterSum = true
			},
			new Column
			{
				Name = "LastBackup",
				Caption = Resources.LastBackupDate,
				Source = "LastBackup",
				Type = SupportedType.DateTime
			},
			new Column
			{
				Name = "BackupPolicy",
				Caption = Resources.AutoBackupOptions,
				Source = "BackupPolicy"
			}
		};
		schema.Columns = columns;
		return schema;
	}

	private static List<DataRow> CreateResultList(List<DatabaseInfo> cloudDatabases, string serviceAddress, bool showDatabasesSizes)
	{
		List<DataRow> list = new List<DataRow>();
		List<DBSize> list2 = new List<DBSize>();
		if (!ConfigHelper.ASAdminLogin & showDatabasesSizes)
		{
			using EdgeClient edgeClient = new EdgeClient(ArmSoft.SME.Core.UI.Settings.LoginService.EdgeServiceAddress, ArmSoft.SME.Core.UI.Settings.GetApplicationToken(), ADHelper.ADB2CToken);
			DBListSizeRequest dBListSizeRequest = new DBListSizeRequest
			{
				DBList = new List<DBSizeRequest>()
			};
			foreach (DatabaseInfo cloudDatabase in cloudDatabases)
			{
				DBSizeRequest item = new DBSizeRequest
				{
					ServerId = cloudDatabase.ServerId,
					Database = cloudDatabase.DbName
				};
				dBListSizeRequest.DBList.Add(item);
			}
			list2 = edgeClient.CalculateDBSizes(dBListSizeRequest);
		}
		DBSize dBSize = null;
		DataRow dataRow = null;
		foreach (DatabaseInfo row in cloudDatabases.Where((DatabaseInfo d) => !(d.PSMode ^ ConfigHelper.IsPublicSector) && (string.IsNullOrWhiteSpace(serviceAddress) || d.ServiceAddress == serviceAddress)))
		{
			dataRow = new DataRow(row);
			if (list2.Count > 0)
			{
				dBSize = list2.FirstOrDefault((DBSize d) => d.Database == row.DbName);
				if (dBSize != null)
				{
					dataRow.Size = dBSize.CurrentSizeMB;
					dataRow.FreeSpace = dBSize.FreeSpaceMB;
				}
			}
			list.Add(dataRow);
		}
		return list;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		if (keys == null)
		{
			List<DatabaseInfo> cloudDatabases = DatabaseHelper.GetCloudDatabases();
			if (cloudDatabases != null)
			{
				dataProviderResult.Rows = CreateResultList(cloudDatabases, param.ServiceAddress, param.ShowDatabasesSizes);
			}
		}
		else
		{
			if (keys.Count > 1)
			{
				throw new ASException(Resources.NotSupportSeveralKeys);
			}
			DatabaseInfo cloudDatabase = DatabaseHelper.GetCloudDatabase((int)keys[0]);
			int num = 1;
			List<DatabaseInfo> list = new List<DatabaseInfo>(num);
			CollectionsMarshal.SetCount(list, num);
			CollectionsMarshal.AsSpan(list)[0] = cloudDatabase;
			dataProviderResult.Rows = CreateResultList(list, param.ServiceAddress, param.ShowDatabasesSizes);
		}
		return dataProviderResult;
	}

	public static string GetBackupPolicyText(bool dailyBackupEnabled, short dailyBackupCount, bool monthlyBackupEnabled, short monthlyBackupDay, short monthlyBackupCount, bool annualBackupEnabled, short annualBackupDay, short annualBackupMonth, short annualBackupCount, bool createOnExistedLogin)
	{
		if (!dailyBackupEnabled && !monthlyBackupEnabled && !annualBackupEnabled)
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (dailyBackupEnabled)
		{
			stringBuilder.AppendFormat(Resources.DailyBackupsDescription, dailyBackupCount);
		}
		if (monthlyBackupEnabled)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append("; ");
			}
			stringBuilder.AppendFormat(Resources.MonthlyBackupsDescription, monthlyBackupCount, monthlyBackupDay);
		}
		if (annualBackupEnabled)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append("; ");
			}
			stringBuilder.AppendFormat(Resources.AnnualBackupsDescription, annualBackupCount, annualBackupMonth, annualBackupDay);
		}
		if (createOnExistedLogin)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append("; ");
			}
			stringBuilder.Append(Resources.CreateOnExistedLoginDescription);
		}
		return stringBuilder.ToString();
	}
}
