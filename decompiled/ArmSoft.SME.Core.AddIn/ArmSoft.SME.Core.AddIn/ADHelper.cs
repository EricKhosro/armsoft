using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;

namespace ArmSoft.SME.Core.AddIn;

public static class ADHelper
{
	private const string _tenant = "armsoftb2c.onmicrosoft.com";

	private const string _azureADB2CHostname = "armsoftb2c.b2clogin.com";

	private const string _clientId = "87f939e5-8fd7-4daf-9284-5a42ea0bf971";

	private const string _clientIdForASAdmin = "574d7eb1-3e73-46de-88f8-2726efe16bae";

	private static string _policySignUpSignIn;

	private static string _policySignUpSignInNew;

	private const string _policyResetPassword = "b2c_1_reset";

	private const string _policyResetPasswordNew = "b2c_1_reset_new";

	private static readonly string[] _scopes;

	private static readonly string _authorityBase;

	private static readonly string _authorityForASAdmin;

	private static readonly string _cacheDir;

	private static readonly string _cacheFileName;

	private static readonly string _cacheFileNameNew;

	private static IPublicClientApplication _application;

	private static IPublicClientApplication _applicationResetPassword;

	private static bool _useNewCache;

	public static IAccount Account { get; set; }

	static ADHelper()
	{
		_policySignUpSignIn = (Settings.ASAdminLogin ? "" : "b2c_1_susi");
		_policySignUpSignInNew = (Settings.ASAdminLogin ? "" : "b2c_1_susi_new");
		_scopes = ((!Settings.ASAdminLogin) ? new string[1] { "https://armsoftb2c.onmicrosoft.com/user/read" } : new string[1] { "https://armsoft.am/Cloud/.default" });
		_authorityBase = "https://armsoftb2c.b2clogin.com/tfp/armsoftb2c.onmicrosoft.com/";
		_authorityForASAdmin = "https://login.microsoftonline.com/armsoft.am";
		_useNewCache = true;
		_cacheDir = MsalCacheHelper.UserRootDirectory;
		_cacheFileName = Settings.SystemType.ToString().ToLower() + "armsoft.bc.cache";
		_cacheFileNameNew = Settings.SystemType.ToString().ToLower() + "newarmsoft.bc.cache";
		if (!File.Exists(Path.Combine(_cacheDir, _cacheFileNameNew)))
		{
			_useNewCache = false;
		}
	}

	private static async Task InitApplicationAsync()
	{
		if (Settings.ASAdminLogin)
		{
			string authorityForASAdmin = _authorityForASAdmin;
			PublicClientApplicationBuilder publicClientApplicationBuilder = PublicClientApplicationBuilder.Create("574d7eb1-3e73-46de-88f8-2726efe16bae").WithAuthority(authorityForASAdmin);
			publicClientApplicationBuilder.WithRedirectUri("http://localhost");
			_application = publicClientApplicationBuilder.Build();
			(await MsalCacheHelper.CreateAsync(new StorageCreationPropertiesBuilder(_cacheFileNameNew, _cacheDir).Build())).RegisterCache(_application.UserTokenCache);
			return;
		}
		string authorityUri = _authorityBase + (_useNewCache ? _policySignUpSignInNew : _policySignUpSignIn);
		PublicClientApplicationBuilder publicClientApplicationBuilder2 = PublicClientApplicationBuilder.Create("87f939e5-8fd7-4daf-9284-5a42ea0bf971").WithB2CAuthority(authorityUri);
		publicClientApplicationBuilder2.WithRedirectUri("http://localhost");
		_application = publicClientApplicationBuilder2.Build();
		MsalCacheHelper msalCacheHelper = await MsalCacheHelper.CreateAsync(new StorageCreationPropertiesBuilder(_useNewCache ? _cacheFileNameNew : _cacheFileName, _cacheDir).Build());
		msalCacheHelper.RegisterCache(_application.UserTokenCache);
		authorityUri = _authorityBase + (_useNewCache ? "b2c_1_reset_new" : "b2c_1_reset");
		PublicClientApplicationBuilder publicClientApplicationBuilder3 = PublicClientApplicationBuilder.Create("87f939e5-8fd7-4daf-9284-5a42ea0bf971").WithB2CAuthority(authorityUri);
		publicClientApplicationBuilder3.WithRedirectUri("http://localhost");
		_applicationResetPassword = publicClientApplicationBuilder3.Build();
		msalCacheHelper.RegisterCache(_applicationResetPassword.UserTokenCache);
	}

	public static async Task<IEnumerable<IAccount>> GetAccountsAsync()
	{
		if (_application == null)
		{
			await InitApplicationAsync();
		}
		return await _application.GetAccountsAsync();
	}

	public static async Task<IAccount> GetAccountAsync(string objectId)
	{
		if (string.IsNullOrWhiteSpace(objectId))
		{
			return null;
		}
		foreach (IAccount item in await GetAccountsAsync())
		{
			string text = item.HomeAccountId.ObjectId.Split('.')[0];
			if (text.EndsWith((_useNewCache ? _policySignUpSignInNew : _policySignUpSignIn).ToLower()) && text.StartsWith(objectId))
			{
				return item;
			}
		}
		return null;
	}

	public static string GetObjectId(IAccount account)
	{
		string text = account.HomeAccountId.ObjectId.Split('.')[0];
		if (text.EndsWith((_useNewCache ? _policySignUpSignInNew : _policySignUpSignIn).ToLower()))
		{
			return text.Substring(0, text.Length - _policySignUpSignIn.Length - 1);
		}
		return string.Empty;
	}

	public static async Task<AuthenticationResult> AcquireTokenSilentAsync(IAccount account)
	{
		if (_application == null)
		{
			await InitApplicationAsync();
		}
		return await _application.AcquireTokenSilent(_scopes, account).ExecuteAsync();
	}

	public static async Task<AuthenticationResult> AcquireTokenInteractiveAsync(IAccount account, string loginHint, IntPtr wndHandle)
	{
		if (!_useNewCache)
		{
			_application = null;
			_useNewCache = true;
		}
		if (_application == null)
		{
			await InitApplicationAsync();
		}
		if (Settings.ASAdminLogin)
		{
			AcquireTokenInteractiveParameterBuilder acquireTokenInteractiveParameterBuilder = _application.AcquireTokenInteractive(_scopes);
			acquireTokenInteractiveParameterBuilder.WithExtraQueryParameters(new Dictionary<string, (string, bool)> { 
			{
				"ui_locales",
				(Settings.GetCurrentCultureName().Substring(0, 2).ToLower(), false)
			} });
			if (account != null)
			{
				acquireTokenInteractiveParameterBuilder.WithAccount(account);
				SetWebViewOptions(acquireTokenInteractiveParameterBuilder);
			}
			else
			{
				acquireTokenInteractiveParameterBuilder.WithLoginHint(loginHint);
				SetWebViewOptions(acquireTokenInteractiveParameterBuilder);
				acquireTokenInteractiveParameterBuilder.WithPrompt(Prompt.ForceLogin);
			}
			acquireTokenInteractiveParameterBuilder.WithParentActivityOrWindow(wndHandle);
			try
			{
				return await acquireTokenInteractiveParameterBuilder.ExecuteAsync();
			}
			catch (MsalServiceException ex)
			{
				if (!ex.Message.Contains("AADB2C90118"))
				{
					throw;
				}
			}
			return null;
		}
		AcquireTokenInteractiveParameterBuilder acquireTokenInteractiveParameterBuilder2 = _application.AcquireTokenInteractive(_scopes);
		acquireTokenInteractiveParameterBuilder2.WithExtraQueryParameters(new Dictionary<string, (string, bool)> { 
		{
			"ui_locales",
			(Settings.GetCurrentCultureName().Substring(0, 2).ToLower(), false)
		} });
		if (account != null)
		{
			acquireTokenInteractiveParameterBuilder2.WithAccount(account);
			SetWebViewOptions(acquireTokenInteractiveParameterBuilder2);
		}
		else
		{
			acquireTokenInteractiveParameterBuilder2.WithLoginHint(loginHint);
			SetWebViewOptions(acquireTokenInteractiveParameterBuilder2);
			acquireTokenInteractiveParameterBuilder2.WithPrompt(Prompt.ForceLogin);
		}
		acquireTokenInteractiveParameterBuilder2.WithParentActivityOrWindow(wndHandle);
		try
		{
			return await acquireTokenInteractiveParameterBuilder2.ExecuteAsync();
		}
		catch (MsalServiceException ex2)
		{
			if (!ex2.Message.Contains("AADB2C90118"))
			{
				throw;
			}
		}
		acquireTokenInteractiveParameterBuilder2 = _applicationResetPassword.AcquireTokenInteractive(_scopes);
		acquireTokenInteractiveParameterBuilder2.WithExtraQueryParameters(new Dictionary<string, (string, bool)> { 
		{
			"ui_locales",
			(Settings.GetCurrentCultureName().Substring(0, 2).ToLower(), false)
		} });
		SetWebViewOptions(acquireTokenInteractiveParameterBuilder2);
		acquireTokenInteractiveParameterBuilder2.WithParentActivityOrWindow(wndHandle);
		return await acquireTokenInteractiveParameterBuilder2.ExecuteAsync();
	}

	private static void SetWebViewOptions(AcquireTokenInteractiveParameterBuilder builder)
	{
		SystemWebViewOptions options = new SystemWebViewOptions
		{
			HtmlMessageSuccess = "<!DOCTYPE html><html><head><title>Authentication complete</title ><meta charset=\"utf-16\"/></head><body><p>Նույնականացումն ավարտված է: Կարող եք վերադառնալ ծրագիր և փակել վեբ-բրաուզերի այս ներդիրը:</p><p>Authentication complete. You can return to the application. Feel free to close this browser tab.</p></body></html>"
		};
		builder.WithUseEmbeddedWebView(useEmbeddedWebView: false);
		builder.WithSystemWebViewOptions(options);
	}

	public static async Task ChangePasswordAsync(string loginHint, IntPtr wndHandle)
	{
		if (_application == null)
		{
			await InitApplicationAsync();
		}
		AcquireTokenInteractiveParameterBuilder acquireTokenInteractiveParameterBuilder = _applicationResetPassword.AcquireTokenInteractive(_scopes);
		acquireTokenInteractiveParameterBuilder.WithExtraQueryParameters(new Dictionary<string, (string, bool)> { 
		{
			"ui_locales",
			(Settings.GetCurrentCultureName().Substring(0, 2).ToLower(), false)
		} });
		acquireTokenInteractiveParameterBuilder.WithLoginHint(loginHint);
		SetWebViewOptions(acquireTokenInteractiveParameterBuilder);
		acquireTokenInteractiveParameterBuilder.WithParentActivityOrWindow(wndHandle);
		try
		{
			await acquireTokenInteractiveParameterBuilder.ExecuteAsync();
		}
		catch (MsalClientException ex)
		{
			if (ex.ErrorCode != "authentication_canceled")
			{
				throw;
			}
		}
		catch (MsalServiceException ex2)
		{
			if (ex2.ErrorCode != "access_denied")
			{
				throw;
			}
		}
	}

	public static AuthenticationResult RefreshToken()
	{
		if (_application == null)
		{
			Task.Run(async () =>
			{
				await InitApplicationAsync();
			}).GetAwaiter().GetResult();
		}
		return Task.Run(async () => await _application.AcquireTokenSilent(_scopes, Account).ExecuteAsync()).GetAwaiter().GetResult();
	}

	public static async Task ClearCacheAsync()
	{
		if (_application == null)
		{
			await InitApplicationAsync();
		}
		List<IAccount> list = (await _application.GetAccountsAsync()).ToList();
		while (list.Count != 0)
		{
			await _application.RemoveAsync(list.First());
			list = (await _application.GetAccountsAsync()).ToList();
		}
		Account = null;
	}
}
