namespace ArmSoft.SME.Core.UI.Extensions;

public static class ObjectExtensions
{
	public static bool TryParseToString(this object input, out string result, bool checkNull = true)
	{
		result = null;
		if (checkNull && input == null)
		{
			return false;
		}
		if (input is string text)
		{
			result = text ?? ((!checkNull) ? "" : null);
			return true;
		}
		return false;
	}
}
