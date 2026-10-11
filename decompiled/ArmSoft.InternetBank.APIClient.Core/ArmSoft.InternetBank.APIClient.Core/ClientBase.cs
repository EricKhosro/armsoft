using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using ArmSoft.InternetBank.APIClient.Core.Resources;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ArmSoft.InternetBank.APIClient.Core;

public abstract class ClientBase
{
	private readonly string baseUrl;

	private readonly string version;

	private readonly string credentials;

	private readonly RestClient restClient;

	private SystemType systemType;

	private string accessToken;

	private string language;

	public string Language
	{
		get
		{
			return language;
		}
		set
		{
			language = value;
			ArmSoft.InternetBank.APIClient.Core.Resources.Resources.Language = value;
		}
	}

	public string CurrentVersion
	{
		get
		{
			if (version != null)
			{
				return version;
			}
			return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
		}
	}

	public bool IsAuthenticated => !string.IsNullOrEmpty(accessToken);

	public ParamManager ParamManager { get; private set; }

	protected ClientBase(string baseUrl, string credentials, string appName, SystemType systemType, int timeout, string language = "en-US", string version = null, string deviceModel = null, string devicePlatform = null, string devicePlatformVersion = null)
	{
		ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
		this.baseUrl = baseUrl;
		this.credentials = credentials;
		this.systemType = systemType;
		this.version = version;
		Language = language;
		ArmSoft.InternetBank.APIClient.Core.Resources.Resources.Language = null;
		this.restClient = new RestClient(baseUrl);
		this.restClient.Timeout = timeout;
		this.restClient.AddDefaultHeader("X-Uuid", appName);
		this.restClient.AddDefaultHeader("X-Version", CurrentVersion);
		if (deviceModel != null)
		{
			this.restClient.AddDefaultHeader("X-Device-Model", deviceModel);
		}
		if (devicePlatform != null)
		{
			this.restClient.AddDefaultHeader("X-Device-Platform", devicePlatform);
		}
		if (devicePlatformVersion != null)
		{
			this.restClient.AddDefaultHeader("X-Device-Platform-Version", devicePlatformVersion);
		}
		RestClient restClient = this.restClient;
		int num = (int)systemType;
		restClient.AddDefaultHeader("X-System", num.ToString());
		this.restClient.AddDefaultHeader("User-Agent", "APIClient");
		this.restClient.AddHandler("application/json", () => NewtonsoftJsonSerializer.Default);
		this.restClient.AddHandler("text/json", () => NewtonsoftJsonSerializer.Default);
		this.restClient.AddHandler("text/x-json", () => NewtonsoftJsonSerializer.Default);
		this.restClient.AddHandler("text/plain", () => NewtonsoftJsonSerializer.Default);
		this.restClient.AddHandler("text/javascript", () => NewtonsoftJsonSerializer.Default);
		this.restClient.AddHandler("*+json", () => NewtonsoftJsonSerializer.Default);
		ParamManager = new ParamManager(this);
	}

	public void Login(string accessToken)
	{
		CheckApiVersion();
		this.accessToken = accessToken;
	}

	public Version CheckApiVersion(Version requiredVersion = null)
	{
		string iBVersion = ParamManager.IBVersion;
		if (!Version.TryParse(iBVersion, out Version result))
		{
			result = new Version(0, 0, 0, 0);
		}
		if (requiredVersion == null && !Version.TryParse("4.30.0", out requiredVersion))
		{
			requiredVersion = new Version(0, 0, 0, 0);
		}
		if (result.CompareTo(requiredVersion) < 0)
		{
			throw new ApiVersionException(iBVersion, requiredVersion.ToString(), $"The required API version is {requiredVersion}. You are trying to connect to API with version {iBVersion}");
		}
		return result;
	}

	public void Logout()
	{
		Post("logout", requireAuthorization: true);
	}

	public T Post<T>(string path, bool requireAuthorization, Dictionary<string, object> formParams = null, Dictionary<string, object> queryParams = null, object postBody = null, Dictionary<string, string> headers = null, string contentType = "application/json") where T : new()
	{
		return MakeRequest<T>(path, Method.POST, requireAuthorization, headers, formParams, queryParams, postBody, contentType);
	}

	public string Post(string path, bool requireAuthorization, Dictionary<string, object> formParams = null, Dictionary<string, object> queryParams = null, object postBody = null, Dictionary<string, string> headers = null, string contentType = "application/json")
	{
		return MakeRequest(path, Method.POST, requireAuthorization, headers, formParams, queryParams, postBody, contentType);
	}

	public async Task<T> PostAsync<T>(string path, bool requireAuthorization, Dictionary<string, object> formParams = null, Dictionary<string, object> queryParams = null, object postBody = null, Dictionary<string, string> headers = null, string contentType = "application/json") where T : new()
	{
		return await MakeRequestAsync<T>(path, Method.POST, requireAuthorization, headers, formParams, queryParams, postBody, contentType);
	}

	public async Task<string> PostAsync(string path, bool requireAuthorization, Dictionary<string, object> formParams = null, Dictionary<string, object> queryParams = null, object postBody = null, Dictionary<string, string> headers = null, string contentType = "application/json")
	{
		return await MakeRequestAsync(path, Method.POST, requireAuthorization, headers, formParams, queryParams, postBody, contentType);
	}

	public string Get(string path, bool requireAuthorization, Dictionary<string, object> formParams = null, Dictionary<string, object> queryParams = null, Dictionary<string, string> headers = null, string contentType = "application/json")
	{
		return MakeRequest(path, Method.GET, requireAuthorization, headers, formParams, queryParams, null, contentType);
	}

	public T Get<T>(string path, bool requireAuthorization, Dictionary<string, object> formParams = null, Dictionary<string, object> queryParams = null, Dictionary<string, string> headers = null, string contentType = "application/json") where T : new()
	{
		return MakeRequest<T>(path, Method.GET, requireAuthorization, headers, formParams, queryParams, null, contentType);
	}

	public async Task<string> GetAsync(string path, bool requireAuthorization, Dictionary<string, object> formParams = null, Dictionary<string, object> queryParams = null, Dictionary<string, string> headers = null, string contentType = "application/json")
	{
		return await MakeRequestAsync(path, Method.GET, requireAuthorization, headers, formParams, queryParams, null, contentType);
	}

	public async Task<T> GetAsync<T>(string path, bool requireAuthorization, Dictionary<string, object> formParams = null, Dictionary<string, object> queryParams = null, Dictionary<string, string> headers = null, string contentType = "application/json") where T : new()
	{
		return await MakeRequestAsync<T>(path, Method.GET, requireAuthorization, headers, formParams, queryParams, null, contentType);
	}

	public string Login(string username, string password, string securityProviderChallenge = null, string securityProviderResponse = null, Guid? autoLoginId = null)
	{
		Version version = CheckApiVersion();
		if (systemType != SystemType.WebApiClient && version.CompareTo(new Version(8, 11, 0)) < 0)
		{
			systemType = SystemType.WebApiClient;
		}
		Dictionary<string, object> formParams = new Dictionary<string, object>
		{
			["grant_type"] = "password",
			["username"] = username,
			["password"] = password
		};
		Dictionary<string, string> dictionary = new Dictionary<string, string> { ["Authorization"] = "Basic " + credentials };
		if (securityProviderChallenge != null)
		{
			dictionary["X-Challenge"] = securityProviderChallenge;
		}
		if (securityProviderResponse != null)
		{
			dictionary["X-Token"] = securityProviderResponse;
		}
		if (autoLoginId.HasValue)
		{
			dictionary["X-AutologinId"] = autoLoginId.ToString();
		}
		JObject jObject = Post<JObject>("token", requireAuthorization: false, formParams, null, null, dictionary, "application/x-www-form-urlencoded");
		accessToken = (string?)jObject["access_token"];
		Dictionary<string, object> dictionary2 = Get<Dictionary<string, object>>("identity", requireAuthorization: true);
		if (dictionary2.ContainsKey("MustChangePassword") && (bool)dictionary2["MustChangePassword"])
		{
			throw new BadRequestException("must_change_password", ArmSoft.InternetBank.APIClient.Core.Resources.Resources.MsgMustChangePassword);
		}
		if (dictionary2.ContainsKey("MustActivateSecurityProvider") && (bool)dictionary2["MustActivateSecurityProvider"])
		{
			throw new BadRequestException("must_activate_security_provider", ArmSoft.InternetBank.APIClient.Core.Resources.Resources.MustActivateSecurityProvider);
		}
		return accessToken;
	}

	private RestRequest PrepareRequest(string path, Method method, bool requireAuthorization, Dictionary<string, string> headers, Dictionary<string, object> formParams, Dictionary<string, object> queryParams, object postBody, string contentType)
	{
		RestRequest restRequest = new RestRequest(path, method);
		if (headers == null)
		{
			headers = new Dictionary<string, string>();
		}
		headers["Content-Type"] = contentType;
		headers["Accept"] = "application/json";
		headers["X-Language"] = Language;
		Dictionary<string, string> dictionary = headers;
		int num = (int)systemType;
		dictionary["X-System"] = num.ToString();
		if (requireAuthorization)
		{
			if (string.IsNullOrEmpty(accessToken))
			{
				throw new UnauthorizedException(ArmSoft.InternetBank.APIClient.Core.Resources.Resources.MsgAccessTokenIsEmpty);
			}
			headers["Authorization"] = "Bearer " + accessToken;
		}
		foreach (KeyValuePair<string, string> header in headers)
		{
			restRequest.AddHeader(header.Key, header.Value);
		}
		if (formParams != null)
		{
			foreach (KeyValuePair<string, object> formParam in formParams)
			{
				restRequest.AddParameter(formParam.Key, formParam.Value);
			}
		}
		if (queryParams != null)
		{
			foreach (KeyValuePair<string, object> queryParam in queryParams)
			{
				if (queryParam.Value is ICollection)
				{
					foreach (object item in (IEnumerable)queryParam.Value)
					{
						AddQueryParam(restRequest, queryParam.Key, item);
					}
				}
				else
				{
					AddQueryParam(restRequest, queryParam.Key, queryParam.Value);
				}
			}
		}
		if (postBody != null)
		{
			restRequest.AddParameter(contentType, JsonConvert.SerializeObject(postBody), ParameterType.RequestBody);
		}
		restRequest.JsonSerializer = NewtonsoftJsonSerializer.Default;
		return restRequest;
	}

	private void AddQueryParam(RestRequest request, string key, object value)
	{
		if (value is DateTime dateTime)
		{
			value = dateTime.ToString("yyyy-MM-dd");
		}
		request.AddParameter(key, value, ParameterType.QueryString);
	}

	private T MakeRequest<T>(string path, Method method, bool requireAuthorization, Dictionary<string, string> headers, Dictionary<string, object> formParams, Dictionary<string, object> queryParams, object postBody, string contentType) where T : new()
	{
		RestRequest request = PrepareRequest(path, method, requireAuthorization, headers, formParams, queryParams, postBody, contentType);
		IRestResponse<T> restResponse = restClient.Execute<T>(request);
		ValidateStatusCode(restResponse);
		return restResponse.Data;
	}

	private string MakeRequest(string path, Method method, bool requireAuthorization, Dictionary<string, string> headers, Dictionary<string, object> formParams, Dictionary<string, object> queryParams, object postBody, string contentType)
	{
		RestRequest request = PrepareRequest(path, method, requireAuthorization, headers, formParams, queryParams, postBody, contentType);
		IRestResponse restResponse = restClient.Execute(request);
		ValidateStatusCode(restResponse);
		return restResponse.Content.Trim('"');
	}

	private async Task<T> MakeRequestAsync<T>(string path, Method method, bool requireAuthorization, Dictionary<string, string> headers, Dictionary<string, object> formParams, Dictionary<string, object> queryParams, object postBody, string contentType) where T : new()
	{
		RestRequest request = PrepareRequest(path, method, requireAuthorization, headers, formParams, queryParams, postBody, contentType);
		IRestResponse<T> restResponse = await restClient.ExecuteAsync<T>(request);
		ValidateStatusCode(restResponse);
		return restResponse.Data;
	}

	private async Task<string> MakeRequestAsync(string path, Method method, bool requireAuthorization, Dictionary<string, string> headers, Dictionary<string, object> formParams, Dictionary<string, object> queryParams, object postBody, string contentType)
	{
		RestRequest request = PrepareRequest(path, method, requireAuthorization, headers, formParams, queryParams, postBody, contentType);
		IRestResponse restResponse = await restClient.ExecuteAsync(request);
		ValidateStatusCode(restResponse);
		return restResponse.Content.Trim('"');
	}

	private void ValidateStatusCode(IRestResponse response)
	{
		if (response.IsSuccessful)
		{
			return;
		}
		switch (response.StatusCode)
		{
		case HttpStatusCode.BadRequest:
		{
			dynamic val = JsonConvert.DeserializeObject(response.Content);
			if (val["FieldErrors"] != null)
			{
				throw new ApiValidationException(JsonConvert.DeserializeObject<Error>(response.Content), response.ErrorMessage, response.ErrorException);
			}
			string text2;
			string message;
			if (!string.IsNullOrEmpty((string)val.error))
			{
				text2 = (string)val.error;
				message = (string)val.error_description;
			}
			else
			{
				text2 = (string)val.ErrorCode;
				if (string.IsNullOrEmpty(text2))
				{
					text2 = (string)val.ResponseCode;
					if (text2 == null)
					{
						text2 = string.Empty;
					}
				}
				message = (string)val.Message;
			}
			if (string.IsNullOrEmpty(message))
			{
				message = response.ErrorMessage;
			}
			if (text2 == "secure_authentication_required")
			{
				string[] array = message.Split(':');
				string description = array[0];
				string challenge = null;
				if (array.Length > 1)
				{
					challenge = array[1];
				}
				string authenticationMode = null;
				string securityProvider = null;
				bool hasDeviceWithIntegratedOtp = false;
				string tokenSerial = null;
				string text3 = HttpUtility.UrlDecode((string)val["error_uri"]);
				if (!string.IsNullOrEmpty(text3))
				{
					int startIndex = text3.IndexOf('/') + 1;
					text3 = text3.Substring(startIndex);
					string[] array2 = text3.Split('&');
					for (int i = 0; i < array2.Length; i++)
					{
						string[] array3 = array2[i].Split('=');
						switch (array3[0])
						{
						case "AuthenticationMode":
							authenticationMode = array3[1];
							break;
						case "SecurityProvider":
							securityProvider = array3[1];
							break;
						case "HasDeviceWithIntegratedOtp":
							hasDeviceWithIntegratedOtp = bool.Parse(array3[1]);
							break;
						case "TokenSerial":
							tokenSerial = array3[1];
							break;
						}
					}
				}
				else
				{
					ResponseResult<object> responseResult = JsonConvert.DeserializeObject<ResponseResult<object>>(response.Content);
					if (responseResult != null)
					{
						dynamic value = responseResult.Value;
						if (value != null)
						{
							try
							{
								securityProvider = (string)value.SecurityProvider;
								challenge = (string)value.Challenge;
								tokenSerial = (string)value.TokenSerial;
								hasDeviceWithIntegratedOtp = (bool)value.HasDeviceWithIntegratedOtp;
								authenticationMode = (string)value.AuthenticationMode;
							}
							catch
							{
								securityProvider = string.Empty;
								challenge = string.Empty;
								tokenSerial = string.Empty;
								hasDeviceWithIntegratedOtp = false;
								authenticationMode = string.Empty;
							}
						}
					}
				}
				throw new SecureAuthenticationRequiredException(description, challenge, authenticationMode, securityProvider, hasDeviceWithIntegratedOtp, tokenSerial);
			}
			if (text2 == "unsupported_version")
			{
				throw new ClientVersionException("The installed version of the client is outdated");
			}
			throw new BadRequestException(text2, response.Content, message, response.ErrorException);
		}
		case HttpStatusCode.NotFound:
		{
			dynamic val = null;
			try
			{
				val = JsonConvert.DeserializeObject(response.Content);
			}
			catch (Exception)
			{
			}
			string message = response.ErrorMessage;
			string text = string.Empty;
			if (val != null)
			{
				if (!string.IsNullOrEmpty((string)val.Message))
				{
					message = (string)val.Message;
				}
				if (!string.IsNullOrEmpty((string)val.responseCode))
				{
					text = (string)val.responseCode;
				}
			}
			if (text == "not_supported")
			{
				throw new EndpointNotSupportedException(message, response.Content, response.ErrorException);
			}
			throw new NotFoundException(message, response.Content, response.ErrorException);
		}
		case HttpStatusCode.Unauthorized:
			throw new UnauthorizedException(response.ErrorMessage, response.Content, response.ErrorException);
		case HttpStatusCode.Forbidden:
			throw new ForbiddenException(response.ErrorMessage, response.Content, response.ErrorException);
		default:
			throw new UnexpectedException(response.StatusCode, response.Content, response.ErrorMessage, response.ErrorException);
		}
	}
}
