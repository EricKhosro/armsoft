using System;

namespace ArmSoft.SME.Common.SmartEnums;

public class DirectoryLogOpSE : ASSmartEnum<DirectoryLogOpSE, byte>
{
	public static readonly DirectoryLogOpSE Created = new DirectoryLogOpSE("Created", 1, () => Resources.Created);

	public static readonly DirectoryLogOpSE Edited = new DirectoryLogOpSE("Edited", 2, () => Resources.Edited);

	public static readonly DirectoryLogOpSE Imported = new DirectoryLogOpSE("Imported", 4, () => Resources.DocImported);

	public static readonly DirectoryLogOpSE Converted = new DirectoryLogOpSE("Converted", 5, () => Resources.Converted);

	public static readonly DirectoryLogOpSE EditedOnlyCodeOfDirectories = new DirectoryLogOpSE("EditedOnlyCodeOfDirectories", 10, () => Resources.EditedCodeOfDirectories);

	public static readonly DirectoryLogOpSE VersionUpgraded = new DirectoryLogOpSE("VersionUpgraded", 12, () => Resources.VersionUpgraded);

	public static readonly DirectoryLogOpSE Deleted = new DirectoryLogOpSE("Deleted", 15, () => Resources.Deleted);

	protected DirectoryLogOpSE(string name, byte value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
