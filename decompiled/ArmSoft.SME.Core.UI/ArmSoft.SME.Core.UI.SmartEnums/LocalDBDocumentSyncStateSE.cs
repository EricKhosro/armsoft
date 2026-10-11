using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Core.UI.SmartEnums;

[SmartEnumProviderSupport(1, "SyncState")]
public class LocalDBDocumentSyncStateSE : ASSmartEnum<LocalDBDocumentSyncStateSE, short>
{
	public static readonly LocalDBDocumentSyncStateSE Sent = new LocalDBDocumentSyncStateSE("Sent", 1, () => Resources.Sent);

	public static readonly LocalDBDocumentSyncStateSE NotSent = new LocalDBDocumentSyncStateSE("NotSent", 2, () => Resources.NotSent);

	public static readonly LocalDBDocumentSyncStateSE Error = new LocalDBDocumentSyncStateSE("Error", 3, () => Resources.ErrorWord);

	public static readonly LocalDBDocumentSyncStateSE AutoSaved = new LocalDBDocumentSyncStateSE("AutoSaved", 4, () => Resources.AutoSaved);

	private LocalDBDocumentSyncStateSE(string name, short value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
