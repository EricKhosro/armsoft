using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Extensions;

public static class DocumentStateExtension
{
	public static bool IsRefused(this DocumentState state)
	{
		return state == DocumentState.Refused;
	}

	public static bool IsInProcess(this DocumentState state)
	{
		if (state != DocumentState.Editable && state != DocumentState.Confirmed && state != DocumentState.NotSet)
		{
			return state != DocumentState.Refused;
		}
		return false;
	}
}
