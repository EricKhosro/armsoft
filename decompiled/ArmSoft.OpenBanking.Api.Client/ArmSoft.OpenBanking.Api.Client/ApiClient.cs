using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ArmSoft.OnlineBanking.Common.Enums;
using ArmSoft.OpenBanking.Api.Client.Core;
using ArmSoft.OpenBanking.Api.Common;

namespace ArmSoft.OpenBanking.Api.Client;

public class ApiClient
{
	internal readonly string baseAuthenticationApiUrl;

	private readonly string baseUrl;

	private bool disposedValue;

	internal string ClientId;

	internal string Secret;

	private readonly SemaphoreSlim refreshSemaphore = new SemaphoreSlim(1, 1);

	public string Language { get; set; }

	protected virtual bool AreCredentialsRequiredForRefresh => true;

	public string? AccessToken { get; set; }

	public string? RefreshToken { get; set; }

	public AuthenticationManager AuthenticationManager { get; private set; }

	public BankManager BankManager { get; private set; }

	public CancelRequestApplicationManager CancelRequestApplicationManager { get; private set; }

	public DocumentsManager DocumentsManager { get; private set; }

	public DocumentManager<BudgetaryPayment> BudgetaryPaymentManager { get; private set; }

	public RateManager RateManager { get; private set; }

	public AccountManager AccountManager { get; private set; }

	public DocumentManager<LocalPayment> LocalPaymentManager { get; private set; }

	public DocumentManager<MultiplePayment> MultiplePaymentManager { get; private set; }

	public CardManager CardManager { get; private set; }

	public DocumentManager<CardTransfer> CardTransferManager { get; private set; }

	public BondManager BondManager { get; private set; }

	public CreditManager CreditManager { get; private set; }

	public DepositManager DepositManager { get; private set; }

	public MerchantManager MerchantManager { get; private set; }

	public PurchasedSecurityManager PurchasedSecurityManager { get; private set; }

	internal HttpClient HttpClient { get; set; }

	public bool IsLoggedIn
	{
		get
		{
			if (!disposedValue)
			{
				return !string.IsNullOrEmpty(RefreshToken);
			}
			return false;
		}
	}

	public event Action<string, string>? TokenRefreshed;

	public ApiClient(string baseAuthenticationApiUrl, string baseApiUrl, string clientId, string secret, HttpClient? httpClient = null, string language = "en-US")
	{
		HttpClient = httpClient ?? new HttpClient();
		HttpClient.BaseAddress = new Uri(baseAuthenticationApiUrl);
		this.baseAuthenticationApiUrl = baseAuthenticationApiUrl;
		baseUrl = baseApiUrl;
		ClientId = clientId;
		Secret = secret;
		Language = language;
		HttpClient.DefaultRequestHeaders.Add("X-Language", Language);
		AuthenticationManager = new AuthenticationManager(this);
		BankManager = new BankManager(this);
		CancelRequestApplicationManager = new CancelRequestApplicationManager(this);
		DocumentsManager = new DocumentsManager(this);
		BudgetaryPaymentManager = new DocumentManager<BudgetaryPayment>(this);
		RateManager = new RateManager(this);
		AccountManager = new AccountManager(this);
		LocalPaymentManager = new DocumentManager<LocalPayment>(this);
		MultiplePaymentManager = new DocumentManager<MultiplePayment>(this);
		CardTransferManager = new DocumentManager<CardTransfer>(this);
		BondManager = new BondManager(this);
		CreditManager = new CreditManager(this);
		DepositManager = new DepositManager(this);
		MerchantManager = new MerchantManager(this);
		PurchasedSecurityManager = new PurchasedSecurityManager(this);
		CardManager = new CardManager(this);
	}

	public DocumentManager<TModel> GetManager<TModel>() where TModel : Document
	{
		PropertyInfo[] properties = GetType().GetProperties();
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (typeof(IDocumentManager<TModel>).IsAssignableFrom(propertyInfo.PropertyType))
			{
				object value = propertyInfo.GetValue(this, null);
				if (value != null)
				{
					return (DocumentManager<TModel>)value;
				}
			}
		}
		throw new NotSupportedException();
	}

	public async Task<T> Post<T>(string path, bool requireAuthorization, Dictionary<string, object>? queryParams = null, object? postBody = null, Dictionary<string, string>? headers = null, string contentType = "application/json")
	{
		return (await PostResponse<T>(path, requireAuthorization, queryParams, postBody, headers, contentType)).Value;
	}

	public async Task<Response<T>> PostResponse<T>(string path, bool requireAuthorization, Dictionary<string, object>? queryParams = null, object? postBody = null, Dictionary<string, string>? headers = null, string contentType = "application/json")
	{
		string accessToken = null;
		try
		{
			if (requireAuthorization)
			{
				accessToken = AccessToken;
			}
			return await PostInner<T>(path, accessToken, queryParams, postBody, headers, contentType);
		}
		catch (ApiException ex) when ((ex.StatusCode == HttpStatusCode.Unauthorized) & requireAuthorization)
		{
			await Refresh(RefreshToken ?? string.Empty, accessToken ?? string.Empty);
			accessToken = AccessToken;
			return await PostInner<T>(path, accessToken, queryParams, postBody, headers, contentType);
		}
	}

	public async Task Refresh(string refreshToken, string accessToken, EasyLoginMode easyLoginMode = EasyLoginMode.NotSet)
	{
		if (!(accessToken == AccessToken) && AccessToken != null)
		{
			return;
		}
		try
		{
			await refreshSemaphore.WaitAsync();
			if (accessToken == AccessToken || AccessToken == null)
			{
				AuthenticationResponse authenticationResponse = await AuthenticationManager.Refresh(refreshToken);
				RefreshToken = authenticationResponse.RefreshToken;
				AccessToken = authenticationResponse.AccessToken;
				TokenRefreshed?.Invoke(AccessToken, RefreshToken);
			}
		}
		finally
		{
			refreshSemaphore.Release();
		}
	}

	private async Task<Response<T>> PostInner<T>(string path, string? accessToken = null, Dictionary<string, object>? queryParams = null, object? postBody = null, Dictionary<string, string>? headers = null, string contentType = "application/json", string? baseUrl = null)
	{
		using HttpResponseMessage response = await GetPostResponse(path, accessToken, queryParams, postBody, headers, contentType, baseUrl);
		return await GetContent<T>(response);
	}

	public async Task CheckAuthentication()
	{
		await Post<object>("authentication/check", requireAuthorization: true);
	}

	private async Task<Response<T>> GetContent<T>(HttpResponseMessage response)
	{
		Response<T> content;
		try
		{
			content = await response.Content.ReadFromJsonAsync<Response<T>>();
		}
		catch (Exception innerException)
		{
			string content2 = await response.Content.ReadAsStringAsync();
			throw new ApiException(response.StatusCode, content2, string.Empty, $"Response Status code is {response.StatusCode}", innerException);
		}
		await ValidateContent(response, content);
		return content;
	}

	private async Task ValidateContent<T>(HttpResponseMessage response, Response<T>? content)
	{
		if (content == null)
		{
			string content2 = await response.Content.ReadAsStringAsync();
			throw new UnexpectedException(response.StatusCode, content2);
		}
		if (response.IsSuccessStatusCode)
		{
			return;
		}
		string text = await response.Content.ReadAsStringAsync();
		switch (response.StatusCode)
		{
		case HttpStatusCode.BadRequest:
			if (content.ResponseCode == "validation_failed" && content.Value != null)
			{
				ValidationResult validationResult = null;
				try
				{
					validationResult = JsonSerializer.Deserialize<Response<ValidationResult>>(text, new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true
					}).Value;
				}
				catch
				{
				}
				if (validationResult != null)
				{
					foreach (FieldError fieldError in validationResult.FieldErrors)
					{
						if (fieldError.Collection != null && fieldError.Collection.Length > 0)
						{
							fieldError.Collection = char.ToUpper(fieldError.Collection[0]) + fieldError.Collection.Substring(1);
						}
						if (fieldError.Field != null && fieldError.Field.Length > 0)
						{
							fieldError.Field = char.ToUpper(fieldError.Field[0]) + fieldError.Field.Substring(1);
						}
						if (fieldError.Field2 != null && fieldError.Field2.Length > 0)
						{
							fieldError.Field2 = char.ToUpper(fieldError.Field2[0]) + fieldError.Field2.Substring(1);
						}
					}
					throw new ValidationException(validationResult, content.ResponseCode, text, content.Message);
				}
			}
			throw new BadRequestException(content.ResponseCode, text, content.Message);
		case HttpStatusCode.NotFound:
			if (content.ResponseCode == "not_supported")
			{
				throw new ActionNotSupportedException(content.ResponseCode, text, content.Message);
			}
			throw new NotFoundException(content.ResponseCode, text, content.Message);
		case HttpStatusCode.Unauthorized:
			throw new UnauthorizedException(content.ResponseCode, text, content.Message);
		case HttpStatusCode.Forbidden:
			if (content.ResponseCode == "unsupported_version")
			{
				throw new ClientVersionException(text);
			}
			if (content.ResponseCode == "action_not_supported")
			{
				throw new ActionNotSupportedException(text);
			}
			throw new ForbiddenException(content.ResponseCode, text, content.Message);
		default:
			throw new UnexpectedException(response.StatusCode, text);
		}
	}

	private async Task<HttpResponseMessage> GetPostResponse(string path, string? accessToken, Dictionary<string, object>? queryParams = null, object? postBody = null, Dictionary<string, string>? headers = null, string contentType = "application/json", string? baseUrl = null)
	{
		HttpContent content = JsonContent.Create(postBody, new MediaTypeHeaderValue(contentType));
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, BuildUri(path, queryParams, baseUrl));
		httpRequestMessage.Headers.Add("X-Language", Language);
		httpRequestMessage.Content = content;
		if (accessToken != null)
		{
			httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
		}
		else
		{
			httpRequestMessage.Headers.Authorization = null;
		}
		if (headers != null)
		{
			foreach (KeyValuePair<string, string> header in headers)
			{
				httpRequestMessage.Headers.Add(header.Key, header.Value);
			}
		}
		try
		{
			return await HttpClient.SendAsync(httpRequestMessage);
		}
		catch (TaskCanceledException ex)
		{
			throw new TimeoutException(ex.Message, ex);
		}
	}

	private string BuildUri(string path, Dictionary<string, object>? queryParams, string? baseUrl)
	{
		UriBuilder uriBuilder = new UriBuilder(CombineUrls(baseUrl ?? this.baseUrl, path));
		if (queryParams != null)
		{
			NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(uriBuilder.Query);
			foreach (KeyValuePair<string, object> queryParam in queryParams)
			{
				if (queryParam.Value is ICollection)
				{
					foreach (object item in (IEnumerable)queryParam.Value)
					{
						nameValueCollection[queryParam.Key] = FormatQueryParam(item);
					}
				}
				else
				{
					nameValueCollection[queryParam.Key] = FormatQueryParam(queryParam.Value);
				}
			}
			uriBuilder.Query = nameValueCollection.ToString();
		}
		return uriBuilder.ToString();
	}

	private string CombineUrls(string baseUrl, string relativeUrl)
	{
		return baseUrl.TrimEnd('/') + "/" + relativeUrl.TrimStart('/');
	}

	private string? FormatQueryParam(object value)
	{
		if (value is DateTime dateTime)
		{
			return dateTime.ToString("yyyy-MM-dd");
		}
		return value?.ToString();
	}
}
