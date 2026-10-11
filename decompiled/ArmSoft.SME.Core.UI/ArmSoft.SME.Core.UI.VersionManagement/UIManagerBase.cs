using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.VersionManagement;

public abstract class UIManagerBase : IUIManager
{
	public class Warning
	{
		public string Message;

		public Guid SourceGuid;

		public Warning(string message)
		{
			Message = message;
		}

		public Warning(string message, Guid sourceGuid)
		{
			Message = message;
			SourceGuid = sourceGuid;
		}
	}

	private readonly List<Warning> _warnings;

	private readonly IParametersService _parametersService;

	public abstract Version ServiceMinVersion { get; }

	public abstract Version LastReleasedVersion { get; }

	public abstract int ChangeNumber { get; }

	public abstract List<Version> VersionNumbers { get; }

	public Version VersionInDB { get; set; }

	public int ChangeNumberInDB { get; set; }

	public UIManagerBase(IServiceProvider serviceProvider)
	{
		_warnings = new List<Warning>();
		_parametersService = serviceProvider.GetRequiredService<IParametersService>();
		VersionInDB = GetPreviousVersion();
		if (RegistryHelper.ReadDictionaryFromRegistry("UIVersionChangeNums").TryGetValue(Settings.LoginService.Database, out var value))
		{
			ChangeNumberInDB = Convert.ToInt32(value);
		}
		else
		{
			ChangeNumberInDB = 0;
		}
	}

	public Version GetPreviousVersion()
	{
		if (RegistryHelper.ReadDictionaryFromRegistry("UIVersions").TryGetValue(Settings.LoginService.Database, out var value))
		{
			return new Version(value);
		}
		return new Version();
	}

	public virtual bool CheckUpgrade()
	{
		if (!(LastReleasedVersion > VersionInDB))
		{
			if (LastReleasedVersion == VersionInDB)
			{
				return ChangeNumber > ChangeNumberInDB;
			}
			return false;
		}
		return true;
	}

	public Version GetNextVersion(Version versionLogNumber)
	{
		int num = VersionNumbers.IndexOf(versionLogNumber);
		if (num + 1 >= VersionNumbers.Count)
		{
			return VersionNumbers.Last();
		}
		return VersionNumbers[num + 1];
	}

	public abstract List<Type> GetChangeTypes();

	public Dictionary<short, MethodInfo> GetOrderedChangesMethods(object upgradeObj)
	{
		short num = 1;
		Dictionary<short, MethodInfo> dictionary = new Dictionary<short, MethodInfo>();
		if (VersionInDB == LastReleasedVersion && RegistryHelper.ReadDictionaryFromRegistry("UIVersionChangeNums").TryGetValue(Settings.LoginService.Database, out var value))
		{
			num = Convert.ToInt16(value);
			num++;
		}
		foreach (MethodInfo item in from p in upgradeObj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
			where p.GetCustomAttributes(typeof(ChangeAttribute), inherit: false).Length != 0
			select p)
		{
			short number = ((ChangeAttribute)item.GetCustomAttributes(typeof(ChangeAttribute), inherit: false)[0]).Number;
			if (number >= num)
			{
				dictionary.Add(number, item);
			}
		}
		return dictionary = dictionary.OrderBy((KeyValuePair<short, MethodInfo> p) => p.Key).ToDictionary((KeyValuePair<short, MethodInfo> p) => p.Key, (KeyValuePair<short, MethodInfo> p) => p.Value);
	}

	public void ExecuteChange(object upgradeObj, MethodInfo mi)
	{
		object obj = mi.Invoke(upgradeObj, Array.Empty<object>());
		if (obj != null)
		{
			if (obj is Warning item)
			{
				_warnings.Add(item);
			}
			else if (obj is IEnumerable<Warning> collection)
			{
				_warnings.AddRange(collection);
			}
		}
	}

	public virtual void LastChangeFunction()
	{
		SQLiteStorageService requiredService = Settings.ServiceProvider.GetRequiredService<SQLiteStorageService>();
		requiredService.StoreInUserBasedLocalStorage = true;
		requiredService.DeleteCachesTables();
		requiredService.CreateTablesIfNotExists();
		requiredService.StoreInUserBasedLocalStorage = false;
		requiredService.DeleteDatabase();
	}
}
