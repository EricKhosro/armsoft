namespace ArmSoft.SME.Common.Extensions;

public static class ShortExtensions
{
	public static bool InList(this short sValue, params short[] lValues)
	{
		return SettingsHelper.InList(sValue, lValues);
	}
}
