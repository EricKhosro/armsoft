using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "VersionCapital")]
public class BackupModeSE : ASSmartEnum<BackupModeSE, string>
{
	public static readonly BackupModeSE NewMode = new BackupModeSE("NewMode", "1", () => Resources.NewBackUpMode);

	public static readonly BackupModeSE OldMode = new BackupModeSE("OldMode", "2", () => Resources.OldBackUpMode);

	public BackupModeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
