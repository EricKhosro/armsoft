namespace ArmSoft.OnlineBanking.Common.Constants;

public static class DevicePlatform
{
	private const string Android = "Android";

	private const string Ios = "iOS";

	private const string IPad = "iPadOS";

	public static bool IsIOS(string platform)
	{
		bool result = false;
		if (platform != null)
		{
			string text = platform.Split()[0].ToLower();
			result = text == "iOS".ToLower() || text == "iPadOS".ToLower();
		}
		return result;
	}

	public static bool IsAndroid(string platform)
	{
		bool result = false;
		if (platform != null)
		{
			result = platform.Split()[0].ToLower() == "Android".ToLower();
		}
		return result;
	}
}
