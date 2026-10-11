using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "Print")]
public class PrintPreviewSelectedModeSE : ASSmartEnum<PrintPreviewSelectedModeSE, byte>
{
	public static readonly PrintPreviewSelectedModeSE CurrentRow = new PrintPreviewSelectedModeSE("CurrentRow", 1, () => Resources.CurrentRowMode);

	public static readonly PrintPreviewSelectedModeSE SelectedRows = new PrintPreviewSelectedModeSE("SelectedRows", 2, () => Resources.SelectedRowsMode);

	private PrintPreviewSelectedModeSE(string name, byte value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
