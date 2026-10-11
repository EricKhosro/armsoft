using System.Collections.Generic;
using System.Linq;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Common.Models.Authentication;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Cloud;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Directories;

namespace ArmSoft.SME.Core.UI.Helpers;

public class DatabaseHelper
{
	public static List<ArmSoft.SME.Core.UI.Directories.Database.UserRoles> GetDatabaseUserRoles(int dbId)
	{
		using CloudClient cloudClient = Settings.CloudClient;
		cloudClient.Token = Settings.GetApplicationToken();
		List<UserRoles> usersRoles = GetUsersRoles(dbId);
		if (usersRoles == null)
		{
			return new List<ArmSoft.SME.Core.UI.Directories.Database.UserRoles>();
		}
		Dictionary<int, UserRoles> dictionary = new Dictionary<int, UserRoles>();
		foreach (UserRoles item in usersRoles)
		{
			dictionary.Add(item.UserId, item);
		}
		List<ArmSoft.SME.Core.UI.Directories.Database.UserRoles> list = new List<ArmSoft.SME.Core.UI.Directories.Database.UserRoles>();
		foreach (UsersCacheRow item2 in Settings.CoreCacheService.User.GetList())
		{
			ArmSoft.SME.Core.UI.Directories.Database.UserRoles userRoles = new ArmSoft.SME.Core.UI.Directories.Database.UserRoles
			{
				UserId = item2.CloudUserId,
				UserName = item2.Name,
				UserCaption = item2.Description,
				UserLoginEnabled = item2.Login,
				UserIsAdmin = item2.Admin,
				Roles = new HashSet<short>()
			};
			if (dictionary.TryGetValue(item2.CloudUserId, out var value))
			{
				foreach (ArmSoft.SME.Common.Models.Authentication.Role role in value.Roles)
				{
					if (role.Allow)
					{
						userRoles.Roles.Add(role.RoleId);
					}
				}
			}
			else if (item2.CloudUserId == 27335)
			{
				ArmSoft.SME.Core.UI.Directories.Database.UserRoles userRoles2 = userRoles;
				HashSet<short> hashSet = new HashSet<short>();
				foreach (short item3 in from v in SmartEnumHelper.GetPartialSmartEnumValues(typeof(SubsystemTypesSE))
					select (short)(short.TryParse(v, out var result) ? result : 0))
				{
					hashSet.Add(item3);
				}
				userRoles2.Roles = hashSet;
			}
			list.Add(userRoles);
		}
		return list;
	}

	public static List<ArmSoft.SME.Core.UI.Directories.Database.UserRoles> GetNewDatabaseUserRoles()
	{
		List<ArmSoft.SME.Core.UI.Directories.Database.UserRoles> list = new List<ArmSoft.SME.Core.UI.Directories.Database.UserRoles>();
		foreach (UsersCacheRow item in Settings.CoreCacheService.User.GetList())
		{
			ArmSoft.SME.Core.UI.Directories.Database.UserRoles userRoles = new ArmSoft.SME.Core.UI.Directories.Database.UserRoles
			{
				UserId = item.CloudUserId,
				UserName = item.Name,
				UserCaption = item.Description,
				UserLoginEnabled = item.Login,
				UserIsAdmin = item.Admin,
				Roles = new HashSet<short>()
			};
			if (item.UserId == Settings.LoginService.UserId)
			{
				userRoles.SelectAll = true;
			}
			list.Add(userRoles);
		}
		return list;
	}

	public static void UpdateDatabaseInfo(int dbId, string caption)
	{
		using CloudClient cloudClient = Settings.CloudClient;
		cloudClient.Token = Settings.GetApplicationToken();
		cloudClient.AS7X.UpdateCloudDatabase(dbId, new CloudDatabaseUpdateRequest
		{
			Caption = caption
		});
	}

	public static void UpdateUsersRoles(int dbId, IEnumerable<ArmSoft.SME.Core.UI.Directories.Database.UserRoles> usersRoles)
	{
		Dictionary<short, string> subsystems = Settings.HostingEnvironment.Subsystems;
		List<CloudUserRoles> list = new List<CloudUserRoles>();
		foreach (ArmSoft.SME.Core.UI.Directories.Database.UserRoles usersRole in usersRoles)
		{
			CloudUserRoles cloudUserRoles = new CloudUserRoles
			{
				UserId = usersRole.UserId,
				Roles = new List<ArmSoft.Cloud.Client.AS7X.Role>()
			};
			foreach (short key in subsystems.Keys)
			{
				cloudUserRoles.Roles.Add(new ArmSoft.Cloud.Client.AS7X.Role
				{
					RoleId = key,
					Allow = usersRole.Roles.Contains(key)
				});
			}
			list.Add(cloudUserRoles);
		}
		if (ConfigHelper.ASAdminLogin)
		{
			Settings.CreateApiClient().AdminCloud.EditUsersRoles(ADHelper.ASAdminB2CToken, dbId, ConvertCloudTypeToSystem(list));
			return;
		}
		using CloudClient cloudClient = Settings.CloudClient;
		cloudClient.Token = Settings.GetApplicationToken();
		cloudClient.AS7X.EditUsersRoles(dbId, list);
	}

	public static List<DatabaseInfo> GetCloudDatabases()
	{
		if (ConfigHelper.ASAdminLogin)
		{
			return Settings.CreateApiClient().AdminCloud.GetCloudDatabases(ADHelper.ASAdminB2CToken);
		}
		List<DatabaseInfo> list = new List<DatabaseInfo>();
		using CloudClient cloudClient = Settings.CloudClient;
		cloudClient.Token = Settings.GetApplicationToken();
		foreach (CloudDatabaseInfo cloudDatabase in cloudClient.AS7X.GetCloudDatabases())
		{
			list.Add(ConvertCloudTypeToSystem(cloudDatabase));
		}
		return list;
	}

	public static List<UserRoles> GetUsersRoles(int databaseId)
	{
		if (ConfigHelper.ASAdminLogin)
		{
			return Settings.CreateApiClient().AdminCloud.GetUsersRoles(ADHelper.ASAdminB2CToken, databaseId);
		}
		using CloudClient cloudClient = Settings.CloudClient;
		cloudClient.Token = Settings.GetApplicationToken();
		return ConvertCloudTypeToSystem(cloudClient.AS7X.GetUsersRoles(databaseId));
	}

	public static DatabaseInfo GetCloudDatabase(int databaseId)
	{
		if (ConfigHelper.ASAdminLogin)
		{
			return Settings.CreateApiClient().AdminCloud.GetCloudDatabases(ADHelper.ASAdminB2CToken, databaseId)[0];
		}
		using CloudClient cloudClient = Settings.CloudClient;
		cloudClient.Token = Settings.GetApplicationToken();
		return ConvertCloudTypeToSystem(cloudClient.AS7X.GetCloudDatabase(databaseId));
	}

	private static DatabaseInfo ConvertCloudTypeToSystem(CloudDatabaseInfo cloudDatabase)
	{
		return new DatabaseInfo
		{
			DbId = cloudDatabase.DbId,
			DbName = cloudDatabase.DbName,
			Caption = cloudDatabase.Caption,
			CompanyName = cloudDatabase.CompanyName,
			ServerName = cloudDatabase.ServerName,
			KeyCode = cloudDatabase.KeyCode,
			KeyId = cloudDatabase.KeyId,
			TaxCode = cloudDatabase.TaxCode,
			Version = cloudDatabase.Version,
			ServerId = cloudDatabase.ServerId,
			Host = cloudDatabase.Host,
			PSMode = cloudDatabase.PSMode,
			LastLogin = cloudDatabase.LastLogin,
			ServiceAddress = cloudDatabase.ServiceAddress,
			LastBackup = cloudDatabase.LastBackup,
			DailyBackupEnabled = cloudDatabase.DailyBackupEnabled,
			DailyBackupCount = cloudDatabase.DailyBackupCount,
			MonthlyBackupEnabled = cloudDatabase.MonthlyBackupEnabled,
			MonthlyBackupDay = cloudDatabase.MonthlyBackupDay,
			MonthlyBackupCount = cloudDatabase.MonthlyBackupCount,
			AnnualBackupEnabled = cloudDatabase.AnnualBackupEnabled,
			AnnualBackupDay = cloudDatabase.AnnualBackupDay,
			AnnualBackupMonth = cloudDatabase.AnnualBackupMonth,
			AnnualBackupCount = cloudDatabase.AnnualBackupCount,
			CreateOnExistedLogin = cloudDatabase.CreateOnExistedLogin
		};
	}

	private static List<UserRoles> ConvertCloudTypeToSystem(List<CloudUserRoles> cloudUserRoles)
	{
		List<UserRoles> list = null;
		if (cloudUserRoles != null)
		{
			list = new List<UserRoles>();
			foreach (CloudUserRoles cloudUserRole in cloudUserRoles)
			{
				list.Add(new UserRoles
				{
					UserId = cloudUserRole.UserId,
					Roles = ConvertCloudTypeToSystem(cloudUserRole.Roles)
				});
			}
		}
		return list;
	}

	private static List<ArmSoft.SME.Common.Models.Authentication.Role> ConvertCloudTypeToSystem(List<ArmSoft.Cloud.Client.AS7X.Role> cloudUserRoles)
	{
		List<ArmSoft.SME.Common.Models.Authentication.Role> list = null;
		if (cloudUserRoles != null)
		{
			list = new List<ArmSoft.SME.Common.Models.Authentication.Role>();
			foreach (ArmSoft.Cloud.Client.AS7X.Role cloudUserRole in cloudUserRoles)
			{
				list.Add(new ArmSoft.SME.Common.Models.Authentication.Role
				{
					RoleId = cloudUserRole.RoleId,
					Allow = cloudUserRole.Allow
				});
			}
		}
		return list;
	}
}
