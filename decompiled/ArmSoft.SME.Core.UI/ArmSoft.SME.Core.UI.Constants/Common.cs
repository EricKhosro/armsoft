using ArmSoft.SME.Common;

namespace ArmSoft.SME.Core.UI.Constants;

public class Common
{
	public const string StartTime = "00:00";

	public const double LenBetweenDateAndDocNum = 10.0;

	public const string SingleElementsDataProviderName = "SingleElements";

	public const string AutoHideLeftName = "AutoHideLeft";

	public static short LenDialogName
	{
		get
		{
			if (Settings.SystemType == SystemTypes.Trade || Settings.SystemType == SystemTypes.Accountant || Settings.SystemType == SystemTypes.SalesManagement)
			{
				return 35;
			}
			return 20;
		}
	}
}
