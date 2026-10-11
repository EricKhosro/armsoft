using System.Reflection;
using System.Threading;

namespace ArmSoft.InternetBank.APIClient.Core.Resources;

internal static class Resources
{
	private static string language = string.Empty;

	internal static string Language
	{
		get
		{
			if (!string.IsNullOrEmpty(language))
			{
				return language;
			}
			return Thread.CurrentThread.CurrentUICulture.Name;
		}
		set
		{
			language = value;
		}
	}

	internal static string MsgAccessTokenIsEmpty => GetMsgAccessTokenIsEmpty(Language);

	internal static string MsgMustChangePassword => GetMsgMustChangePassword(Language);

	internal static string MustActivateSecurityProvider => GetMustActivateSecurityProvider(Language);

	internal static string GetMsgAccessTokenIsEmpty(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Access Token пустой.";
			}
			return "The Access Token is empty.";
		}
		return "Access Token-ը դատարկ է";
	}

	internal static string GetMsgMustChangePassword(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Вы должны сменить Ваш пароль.";
			}
			return "You must change your password.";
		}
		return "Դուք պետք է փոխեք Ձեր գաղտնաբառը";
	}

	internal static string GetMustActivateSecurityProvider(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Вы должны активировать средство безопасности.";
			}
			return "You must activate security provider.";
		}
		return "Դուք պետք է ակտիվացնեք անվտանգության միջոց";
	}

	internal static string Get(string resourceName, string language = null)
	{
		if (string.IsNullOrEmpty(language))
		{
			language = Language;
		}
		MethodInfo method = typeof(Resources).GetMethod("Get" + resourceName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		if (method == null)
		{
			return string.Empty;
		}
		return (string)method.Invoke(null, new object[1] { language });
	}
}
