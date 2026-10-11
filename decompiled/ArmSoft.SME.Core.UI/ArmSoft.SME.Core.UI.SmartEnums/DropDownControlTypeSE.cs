using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Core.UI.SmartEnums;

[SmartEnumProviderSupport(10, "DropDownTypes")]
public class DropDownControlTypeSE : ASSmartEnum<DropDownControlTypeSE, string>
{
	public const string DropDown = "DropDown";

	public const string DropDownComment = "DropDownComment";

	public const string MultiSelect = "MultiSelect";

	public const string FileDialog = "FileDialog";

	public static readonly DropDownControlTypeSE DropDownType = new DropDownControlTypeSE("DropDown", "DropDown", () => Resources.DropDown);

	public static readonly DropDownControlTypeSE DropDownCommentType = new DropDownControlTypeSE("DropDownComment", "DropDownComment", () => Resources.DropDownComment);

	public static readonly DropDownControlTypeSE MultiSelectType = new DropDownControlTypeSE("MultiSelect", "MultiSelect", () => Resources.MultiSelectDropDown);

	public static readonly DropDownControlTypeSE FileDialogType = new DropDownControlTypeSE("FileDialog", "FileDialog", () => Resources.FileDialog);

	public DropDownControlTypeSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
