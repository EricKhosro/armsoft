using System;
using System.Collections.Generic;
using System.Reflection;

namespace ArmSoft.SME.Core.UI.VersionManagement;

public interface IUIManager
{
	List<Version> VersionNumbers { get; }

	Version VersionInDB { get; set; }

	int ChangeNumberInDB { get; set; }

	Version ServiceMinVersion { get; }

	Version LastReleasedVersion { get; }

	int ChangeNumber { get; }

	bool CheckUpgrade();

	void LastChangeFunction();

	Version GetNextVersion(Version versionLogNumber);

	List<Type> GetChangeTypes();

	Dictionary<short, MethodInfo> GetOrderedChangesMethods(object upgradeObj);

	void ExecuteChange(object upgradeObj, MethodInfo mi);

	Version GetPreviousVersion();
}
