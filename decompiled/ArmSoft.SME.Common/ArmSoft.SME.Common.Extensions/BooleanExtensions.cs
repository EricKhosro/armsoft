namespace ArmSoft.SME.Common.Extensions;

public static class BooleanExtensions
{
	public static string ToYesNo(this bool value)
	{
		if (!value)
		{
			return Resources.No;
		}
		return Resources.Yes;
	}
}
