using ArmSoft.SME.Core.UI.InternetBank;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.Helpers;

public class ConfigHelper
{
	private static bool? _callsRecordingIsAvailable;

	private static bool? _asAdminLogin;

	private static string _asAdminLoginBaseURL;

	private static string _baseUrl;

	private static bool? _qATestMode;

	private static bool? _eInvoicingConnectToTest;

	private static bool? _isPublicSector;

	private static IBDemoSettings _ibDemoSettings;

	private static AOBSettings _aobSettings;

	public static bool CallsRecordingIsAvailable
	{
		get
		{
			if (!_callsRecordingIsAvailable.HasValue)
			{
				if (bool.TryParse(Settings.ServiceProvider.GetRequiredService<IConfiguration>()["CallsRecordingIsAvailable"], out var result))
				{
					_callsRecordingIsAvailable = result;
				}
				else
				{
					_callsRecordingIsAvailable = false;
				}
			}
			return _callsRecordingIsAvailable.Value;
		}
	}

	public static bool ASAdminLogin
	{
		get
		{
			if (!_asAdminLogin.HasValue)
			{
				if (bool.TryParse(Settings.ServiceProvider.GetRequiredService<IConfiguration>()["ASAdminLogin"], out var result))
				{
					_asAdminLogin = result;
				}
				else
				{
					_asAdminLogin = false;
				}
			}
			return _asAdminLogin.Value;
		}
	}

	public static string ASAdminLoginBaseURL
	{
		get
		{
			if (_asAdminLoginBaseURL == null)
			{
				_asAdminLoginBaseURL = Settings.ServiceProvider.GetRequiredService<IConfiguration>()["ASAdminLoginBaseURL"];
			}
			return _asAdminLoginBaseURL;
		}
	}

	public static string BaseUrl
	{
		get
		{
			if (_baseUrl == null)
			{
				_baseUrl = Settings.ServiceProvider.GetRequiredService<IConfiguration>()["BaseURL"];
			}
			return _baseUrl;
		}
	}

	public static bool QATestMode
	{
		get
		{
			if (!_qATestMode.HasValue)
			{
				if (bool.TryParse(Settings.ServiceProvider.GetRequiredService<IConfiguration>()["QATestMode"], out var result))
				{
					_qATestMode = result;
				}
				else
				{
					_qATestMode = false;
				}
			}
			return _qATestMode.Value;
		}
	}

	public static bool EInvoicingConnectToTest
	{
		get
		{
			if (!_eInvoicingConnectToTest.HasValue)
			{
				if (bool.TryParse(Settings.ServiceProvider.GetRequiredService<IConfiguration>()["EInvoicingConnectToTest"], out var result))
				{
					_eInvoicingConnectToTest = result;
				}
				else
				{
					_eInvoicingConnectToTest = false;
				}
			}
			return _eInvoicingConnectToTest.Value;
		}
	}

	public static bool IsPublicSector
	{
		get
		{
			if (!_isPublicSector.HasValue)
			{
				if (bool.TryParse(Settings.ServiceProvider.GetRequiredService<IConfiguration>()["IsPublicSector"], out var result))
				{
					_isPublicSector = result;
				}
				else
				{
					_isPublicSector = false;
				}
			}
			return _isPublicSector.Value;
		}
	}

	public static IBDemoSettings IBDemoSettings
	{
		get
		{
			if (_ibDemoSettings == null)
			{
				IConfiguration requiredService = Settings.ServiceProvider.GetRequiredService<IConfiguration>();
				_ibDemoSettings = new IBDemoSettings();
				IConfigurationSection section = requiredService.GetSection("IBDemoSettings");
				if (section.Exists())
				{
					section.Bind(_ibDemoSettings);
				}
			}
			return _ibDemoSettings;
		}
	}

	public static AOBSettings AOBSettings
	{
		get
		{
			if (_aobSettings == null)
			{
				IConfiguration requiredService = Settings.ServiceProvider.GetRequiredService<IConfiguration>();
				_aobSettings = new AOBSettings();
				IConfigurationSection section = requiredService.GetSection("AOBSettings");
				if (section.Exists())
				{
					section.Bind(_aobSettings);
				}
			}
			return _aobSettings;
		}
	}
}
