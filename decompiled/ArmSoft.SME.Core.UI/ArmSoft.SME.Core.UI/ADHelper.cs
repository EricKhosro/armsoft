using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Helpers;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Desktop;
using Microsoft.Identity.Client.Extensions.Msal;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ArmSoft.SME.Core.UI;

public static class ADHelper
{
	private const string _tenant = "armsoftb2c.onmicrosoft.com";

	private const string _azureADB2CHostname = "armsoftb2c.b2clogin.com";

	private const string _clientId = "87f939e5-8fd7-4daf-9284-5a42ea0bf971";

	private const string _clientIdForASAdmin = "574d7eb1-3e73-46de-88f8-2726efe16bae";

	private static readonly string _policySignUpSignIn;

	private static readonly string _policySignUpSignInNew;

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

	private static string _adB2CToken;

	private static int? _adB2CExp;

	private static bool _useNewCache;

	public static string ADB2CToken
	{
		get
		{
			if (ConfigHelper.ASAdminLogin)
			{
				_adB2CToken = "";
				return _adB2CToken;
			}
			if (!_adB2CExp.HasValue)
			{
				_adB2CExp = GetTokenExpiration(_adB2CToken);
			}
			if (CheckExpiration(_adB2CExp.Value, new TimeSpan(0, 15, 0)))
			{
				return _adB2CToken;
			}
			try
			{
				_adB2CToken = AsyncHelper.RunSync(async () => await AcquireTokenSilentAsync(Account))?.AccessToken;
				_adB2CExp = null;
				return _adB2CToken;
			}
			catch (MsalUiRequiredException ex)
			{
				throw new ASException(ex.Message, ex);
			}
		}
		set
		{
			_adB2CToken = value;
		}
	}

	public static string ASAdminB2CToken { get; set; }

	public static IAccount Account { get; set; }

	static ADHelper()
	{
		_policySignUpSignIn = (ConfigHelper.ASAdminLogin ? "" : "b2c_1_susi");
		_policySignUpSignInNew = (ConfigHelper.ASAdminLogin ? "" : "b2c_1_susi_new");
		_scopes = ((!ConfigHelper.ASAdminLogin) ? new string[1] { "https://armsoftb2c.onmicrosoft.com/user/read" } : new string[1] { "https://armsoft.am/Cloud/.default" });
		_authorityBase = "https://armsoftb2c.b2clogin.com/tfp/armsoftb2c.onmicrosoft.com/";
		_authorityForASAdmin = "https://login.microsoftonline.com/armsoft.am";
		_adB2CToken = "";
		_adB2CExp = null;
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
		if (ConfigHelper.ASAdminLogin)
		{
			string authorityForASAdmin = _authorityForASAdmin;
			PublicClientApplicationBuilder publicClientApplicationBuilder = PublicClientApplicationBuilder.Create("574d7eb1-3e73-46de-88f8-2726efe16bae").WithAuthority(authorityForASAdmin).WithWindowsEmbeddedBrowserSupport();
			publicClientApplicationBuilder.WithRedirectUri("http://localhost");
			_application = publicClientApplicationBuilder.Build();
			(await MsalCacheHelper.CreateAsync(new StorageCreationPropertiesBuilder(_cacheFileNameNew, _cacheDir).Build())).RegisterCache(_application.UserTokenCache);
			return;
		}
		string authorityUri = _authorityBase + (_useNewCache ? _policySignUpSignInNew : _policySignUpSignIn);
		PublicClientApplicationBuilder publicClientApplicationBuilder2 = PublicClientApplicationBuilder.Create("87f939e5-8fd7-4daf-9284-5a42ea0bf971").WithB2CAuthority(authorityUri).WithWindowsEmbeddedBrowserSupport();
		if (!Settings.B2CUsesEmbeddedWebView)
		{
			publicClientApplicationBuilder2.WithRedirectUri("http://localhost");
		}
		_application = publicClientApplicationBuilder2.Build();
		MsalCacheHelper msalCacheHelper = await MsalCacheHelper.CreateAsync(new StorageCreationPropertiesBuilder(_useNewCache ? _cacheFileNameNew : _cacheFileName, _cacheDir).Build());
		msalCacheHelper.RegisterCache(_application.UserTokenCache);
		authorityUri = _authorityBase + (_useNewCache ? "b2c_1_reset_new" : "b2c_1_reset");
		publicClientApplicationBuilder2 = PublicClientApplicationBuilder.Create("87f939e5-8fd7-4daf-9284-5a42ea0bf971").WithB2CAuthority(authorityUri).WithWindowsEmbeddedBrowserSupport();
		if (!Settings.B2CUsesEmbeddedWebView)
		{
			publicClientApplicationBuilder2.WithRedirectUri("http://localhost");
		}
		_applicationResetPassword = publicClientApplicationBuilder2.Build();
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
			return text.Substring(0, text.Length - (_useNewCache ? _policySignUpSignInNew : _policySignUpSignIn).Length - 1);
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

	public static async Task<AuthenticationResult> AcquireTokenInteractiveAsync(IAccount account, string loginHint, nint wndHandle)
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
		if (ConfigHelper.ASAdminLogin)
		{
			AcquireTokenInteractiveParameterBuilder acquireTokenInteractiveParameterBuilder = _application.AcquireTokenInteractive(_scopes);
			acquireTokenInteractiveParameterBuilder.WithExtraQueryParameters(new Dictionary<string, (string, bool)> { 
			{
				"ui_locales",
				(LanguageService.CurrentUICultureName.Substring(0, 2).ToLower(), false)
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
			(LanguageService.CurrentUICultureName.Substring(0, 2).ToLower(), false)
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
			(LanguageService.CurrentUICultureName.Substring(0, 2).ToLower(), false)
		} });
		SetWebViewOptions(acquireTokenInteractiveParameterBuilder2);
		acquireTokenInteractiveParameterBuilder2.WithParentActivityOrWindow(wndHandle);
		return await acquireTokenInteractiveParameterBuilder2.ExecuteAsync();
	}

	private static void SetWebViewOptions(AcquireTokenInteractiveParameterBuilder builder)
	{
		if (!Settings.B2CUsesEmbeddedWebView)
		{
			SystemWebViewOptions options = new SystemWebViewOptions
			{
				HtmlMessageSuccess = $"<!DOCTYPE html><html><head><title>{Resources.ArmsoftB2CLoginTitleSuccess}</title ><meta charset=\"utf-16\"/></head><body><p>{Resources.ArmsoftB2CLoginMessageSuccess}</p></body></html>"
			};
			builder.WithUseEmbeddedWebView(useEmbeddedWebView: false);
			builder.WithSystemWebViewOptions(options);
		}
	}

	public static async Task ChangePasswordAsync(string loginHint, nint wndHandle)
	{
		if (_application == null)
		{
			await InitApplicationAsync();
		}
		AcquireTokenInteractiveParameterBuilder acquireTokenInteractiveParameterBuilder = _applicationResetPassword.AcquireTokenInteractive(_scopes);
		acquireTokenInteractiveParameterBuilder.WithExtraQueryParameters(new Dictionary<string, (string, bool)> { 
		{
			"ui_locales",
			(LanguageService.CurrentUICultureName.Substring(0, 2).ToLower(), false)
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
		return AsyncHelper.RunSync(async () => await _application.AcquireTokenSilent(_scopes, Account).ExecuteAsync());
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

	public static int GetTokenExpiration(string token)
	{
		return Convert.ToInt32(new JsonWebToken(token).GetClaim("exp").Value);
	}

	public static bool CheckExpiration(int exp, TimeSpan offset)
	{
		DateTime dateTime = new DateTime(1970, 1, 1);
		int num = (int)new TimeSpan(DateTime.UtcNow.Add(offset).Ticks - dateTime.Ticks).TotalSeconds;
		return exp > num;
	}
}
