using System;

namespace ArmSoft.SME.Common.SmartEnums;

public class DocumentLogOpSE : ASSmartEnum<DocumentLogOpSE, byte>
{
	public static readonly DocumentLogOpSE Created = new DocumentLogOpSE("Created", 1, () => Resources.Created);

	public static readonly DocumentLogOpSE Edited = new DocumentLogOpSE("Edited", 2, () => Resources.Edited);

	public static readonly DocumentLogOpSE ExportedToBank = new DocumentLogOpSE("ExportedToBank", 3, () => Resources.ExportedToBank);

	public static readonly DocumentLogOpSE Imported = new DocumentLogOpSE("Imported", 4, () => Resources.Imported);

	public static readonly DocumentLogOpSE Converted = new DocumentLogOpSE("Converted", 5, () => Resources.Converted);

	public static readonly DocumentLogOpSE MadeDraft = new DocumentLogOpSE("MadeDraft", 6, () => Resources.MadeDraft);

	public static readonly DocumentLogOpSE Processed = new DocumentLogOpSE("Processed", 7, () => Resources.MadeProcessed);

	public static readonly DocumentLogOpSE EntriesDeleted = new DocumentLogOpSE("EntriesDeleted", 8, () => Resources.EntriesDeleted);

	public static readonly DocumentLogOpSE SystemEntriesGenerated = new DocumentLogOpSE("SystemEntriesGenerated", 9, () => Resources.SystemEntriesGenerated);

	public static readonly DocumentLogOpSE EditedOnlyCodeOfDirectories = new DocumentLogOpSE("EditedOnlyCodeOfDirectories", 10, () => Resources.EditedCodeOfDirectories);

	public static readonly DocumentLogOpSE OrderNumberChanged = new DocumentLogOpSE("OrderNumberChanged", 11, () => Resources.DayOrderNumberChanged);

	public static readonly DocumentLogOpSE VersionUpgraded = new DocumentLogOpSE("VersionUpgraded", 12, () => Resources.VersionUpgraded);

	public static readonly DocumentLogOpSE ExportedToXML = new DocumentLogOpSE("ExportedToXML", 13, () => Resources.ExportedToXML);

	public static readonly DocumentLogOpSE DateChanged = new DocumentLogOpSE("DateChanged", 14, () => Resources.DateChanged);

	public static readonly DocumentLogOpSE Deleted = new DocumentLogOpSE("Deleted", 15, () => Resources.Deleted);

	protected DocumentLogOpSE(string name, byte value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
