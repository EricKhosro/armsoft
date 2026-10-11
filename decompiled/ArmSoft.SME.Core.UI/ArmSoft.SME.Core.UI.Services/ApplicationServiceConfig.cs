using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Authentication;
using ArmSoft.SME.Core.UI.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

namespace ArmSoft.SME.Core.UI.Services;

public class ApplicationServiceConfig
{
	private static string _token;

	private static int? _exp;

	public static string Token
	{
		get
		{
			if (string.IsNullOrWhiteSpace(_token))
			{
				GetToken();
				return _token;
			}
			if (!_exp.HasValue)
			{
				_exp = ADHelper.GetTokenExpiration(_token);
			}
			if (ADHelper.CheckExpiration(_exp.Value, new TimeSpan(0, 15, 0)))
			{
				return _token;
			}
			try
			{
				GetToken();
				_exp = null;
				return _token;
			}
			catch (MsalUiRequiredException ex)
			{
				throw new ASException(ex.Message, ex);
			}
		}
		set
		{
			_token = value;
			_exp = null;
		}
	}

	public static string GetTokenForOtherDb(int databaseId)
	{
		ApiClient apiClient = new ApiClient(Settings.LoginService.ServiceAddress, null, new HttpClient(), null);
		AuthenticateResponse authenticateResponse;
		if (ConfigHelper.ASAdminLogin)
		{
			authenticateResponse = apiClient.Authentication.AuthenticateACS(ADHelper.ASAdminB2CToken, databaseId, Environment.MachineName, AppType.Desktop);
		}
		else
		{
			string osVersion = $"{Environment.OSVersion.VersionString.Trim()}, ({(Environment.Is64BitOperatingSystem ? 64 : 32)}-bit)";
			authenticateResponse = apiClient.Authentication.AuthenticateB2C(ADHelper.ADB2CToken, databaseId, AppType.Desktop, Settings.AppVersionString, Environment.MachineName, Settings.ProcessId(), Settings.GetMachineGuid(), AppDomain.CurrentDomain.BaseDirectory, osVersion, RuntimeInformation.FrameworkDescription, Settings.LoginService.DemoMode);
		}
		if (authenticateResponse != null)
		{
			return authenticateResponse.AccessToken;
		}
		return "";
	}

	private static void GetToken()
	{
		try
		{
			ApiClient apiClient = new ApiClient(Settings.LoginService.ServiceAddress, null, new HttpClient(), null);
			AuthenticateResponse authenticateResponse;
			if (ConfigHelper.ASAdminLogin)
			{
				authenticateResponse = apiClient.Authentication.AuthenticateACS(ADHelper.ASAdminB2CToken, Settings.LoginService.DatabaseId, Environment.MachineName, AppType.Desktop);
			}
			else
			{
				string osVersion = $"{Environment.OSVersion.VersionString.Trim()}, ({(Environment.Is64BitOperatingSystem ? 64 : 32)}-bit)";
				authenticateResponse = apiClient.Authentication.AuthenticateB2C(ADHelper.ADB2CToken, Settings.LoginService.DatabaseId, AppType.Desktop, Settings.AppVersionString, Environment.MachineName, Settings.ProcessId(), Settings.GetMachineGuid(), AppDomain.CurrentDomain.BaseDirectory, osVersion, RuntimeInformation.FrameworkDescription, Settings.LoginService.DemoMode, Settings.GetDisplayResolution(), Settings.GetDisplayeScale());
			}
			if (authenticateResponse != null)
			{
				if (ConfigHelper.ASAdminLogin)
				{
					Settings.LoginService.SetUserInfoForASAdmin(authenticateResponse);
				}
				_token = authenticateResponse.AccessToken;
				Settings.LoginService.Login(authenticateResponse);
			}
			else
			{
				_token = "";
				Settings.LoginService.Logout();
			}
		}
		catch (ApiException ex) when (ex.Code == 20006)
		{
			Settings.ServiceProvider.GetRequiredService<IAppUpdateService>().TryUpdateAppFiles(updateRequired: true);
		}
		catch
		{
			Settings.LoginService.Logout();
			throw;
		}
	}

	public static void ClearToken()
	{
		Token = "";
		_exp = null;
	}
}
