using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Client.Job;

internal class ProgressHelper
{
	public static int GetDelayTimeForProgress(ref int iteration)
	{
		if (iteration >= 20)
		{
			return 1000;
		}
		int result = (int)((decimal)iteration / 2m).RoundAs(0) * 100;
		iteration++;
		return result;
	}
}
