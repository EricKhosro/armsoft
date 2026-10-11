using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.EMarkReceiptClient.Configuration;
using ArmSoft.EMarkReceiptClient.Exceptions;
using ArmSoft.EMarkReceiptClient.Helpers;
using ArmSoft.EMarkReceiptClient.Models;

namespace ArmSoft.EMarkReceiptClient.Services;

public class EMarkApiClient : IEMarkApiClient
{
	private readonly HttpClient _httpClient;

	private readonly bool _disposeHttpClient;

	private readonly EMarkOptions _options;

	private static JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		PropertyNameCaseInsensitive = true
	};

	public EMarkApiClient(EMarkOptions options)
		: this(new HttpClient(), options, disposeHttpClient: true)
	{
	}

	public EMarkApiClient(HttpClient httpClient, EMarkOptions options, bool disposeHttpClient = false)
	{
		_httpClient = httpClient ?? throw new ArgumentNullException("httpClient");
		_options = options ?? throw new ArgumentNullException("options");
		_disposeHttpClient = disposeHttpClient;
		if (_httpClient.BaseAddress == null)
		{
			_httpClient.BaseAddress = new Uri(_options.BaseUrl.TrimEnd('/') + "/");
		}
	}

	public void Dispose()
	{
		if (_disposeHttpClient)
		{
			_httpClient.Dispose();
		}
	}

	public DatamatrixResponse GetDatamatrix(CancellationToken cancellationToken = default(CancellationToken))
	{
		return AsyncHelper.RunSync(async () => await GetDatamatrixAsync());
	}

	public async Task<DatamatrixResponse> GetDatamatrixAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "ext_app/api/v1/datamatrix");
		using HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		await EnsureSuccessOrThrowAsync(response).ConfigureAwait(continueOnCapturedContext: false);
		byte[] imagebytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(continueOnCapturedContext: false);
		Guid result = Guid.Empty;
		if (response.Headers.TryGetValues("datamatrix-file-id", out IEnumerable<string> values))
		{
			string text = values.FirstOrDefault();
			if (!string.IsNullOrWhiteSpace(text))
			{
				Guid.TryParse(text, out result);
			}
		}
		_options.Datamatrix_file_id = result;
		return new DatamatrixResponse
		{
			imagebytes = imagebytes,
			datamatrix_file_id = result
		};
	}

	public ActivationStatusResponse CheckActivation(bool updateTokens = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		return AsyncHelper.RunSync(async () => await CheckActivationAsync(updateTokens, cancellationToken));
	}

	public async Task<ActivationStatusResponse> CheckActivationAsync(bool updateTokens = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		string requestUri = "ext_app/api/v1/check-activation?id=" + Uri.EscapeDataString(_options.Datamatrix_file_id.ToString());
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
		using HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		await EnsureSuccessOrThrowAsync(response).ConfigureAwait(continueOnCapturedContext: false);
		ActivationStatusResponse activationStatusResponse = JsonSerializer.Deserialize<ActivationStatusResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false), JsonSerializerOptions);
		if (updateTokens && activationStatusResponse.activated && !string.IsNullOrWhiteSpace(activationStatusResponse.token))
		{
			_options.Token = activationStatusResponse.token;
			_options.RefreshToken = activationStatusResponse.refresh_token;
			_options.CRN = activationStatusResponse.crn;
		}
		return activationStatusResponse;
	}

	public RefreshTokenResponse GetRefreshToken(CancellationToken cancellationToken = default(CancellationToken))
	{
		return AsyncHelper.RunSync(async () => await GetRefreshTokenAsync(cancellationToken));
	}

	public async Task<RefreshTokenResponse> GetRefreshTokenAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		if (string.IsNullOrWhiteSpace(_options.RefreshToken))
		{
			throw new InvalidOperationException("RefreshToken is not set in EMarkOptions. Activation is required before calling GetRefreshTokenAsync.");
		}
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "ext_app/api/v1/refresh-token");
		httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.RefreshToken);
		using HttpResponseMessage response = await _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		await EnsureSuccessOrThrowAsync(response).ConfigureAwait(continueOnCapturedContext: false);
		RefreshTokenResponse refreshTokenResponse = JsonSerializer.Deserialize<RefreshTokenResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false), JsonSerializerOptions);
		_options.Token = refreshTokenResponse.token;
		_options.RefreshToken = refreshTokenResponse.refresh_token;
		return refreshTokenResponse;
	}

	public bool UploadMetadata(Guid datamatrixFileId, string name, byte[] iconBytes, string iconName, CancellationToken cancellationToken = default(CancellationToken))
	{
		return AsyncHelper.RunSync(async () => await UploadMetadataAsync(datamatrixFileId, name, iconBytes, iconName, cancellationToken));
	}

	public async Task<bool> UploadMetadataAsync(Guid datamatrixFileId, string name, byte[] iconBytes, string iconName, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (datamatrixFileId == Guid.Empty)
		{
			throw new ArgumentException("datamatrixFileId must not be empty.", "datamatrixFileId");
		}
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("name must not be null or empty.", "name");
		}
		if (iconBytes == null || iconBytes.Length == 0)
		{
			throw new ArgumentException("iconBytes must not be null or empty.", "iconBytes");
		}
		if (string.IsNullOrWhiteSpace(_options.Token))
		{
			throw new InvalidOperationException("Token is not set. Call CheckActivationAsync and ensure activation is completed before UploadMetadataAsync.");
		}
		string requestUri = $"ext_app/api/v1/{datamatrixFileId}/metadata";
		using MultipartFormDataContent content = new MultipartFormDataContent();
		content.Add(new StringContent(name, Encoding.UTF8), "name");
		ByteArrayContent byteArrayContent = new ByteArrayContent(iconBytes);
		byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
		content.Add(byteArrayContent, "icon", iconName);
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri)
		{
			Content = content
		};
		await AddAuthorizationHeaderAsync(request).ConfigureAwait(continueOnCapturedContext: false);
		using HttpResponseMessage response = await SendWithRefreshIfNeededAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		await EnsureSuccessOrThrowAsync(response).ConfigureAwait(continueOnCapturedContext: false);
		return true;
	}

	public PrintResponse Print(PrintRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		return AsyncHelper.RunSync(async () => await PrintAsync(request, cancellationToken));
	}

	public async Task<PrintResponse> PrintAsync(PrintRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (request == null)
		{
			throw new ArgumentNullException("request");
		}
		if (string.IsNullOrWhiteSpace(_options.Token))
		{
			throw new InvalidOperationException("Token is not set. You must call CheckActivationAsync and wait for successful activation before calling PrintAsync.");
		}
		string requestUri = "ext_app/api/v1/print";
		string content = JsonSerializer.Serialize(request, JsonSerializerOptions);
		using StringContent content2 = new StringContent(content, Encoding.UTF8, "application/json");
		using HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri)
		{
			Content = content2
		};
		await AddAuthorizationHeaderRefreshTokenAsync(httpRequest).ConfigureAwait(continueOnCapturedContext: false);
		using HttpResponseMessage response = await SendWithRefreshIfNeededAsync(httpRequest, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		await EnsureSuccessOrThrowAsync(response).ConfigureAwait(continueOnCapturedContext: false);
		PrintResponse printResponse = JsonSerializer.Deserialize<PrintResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
		if (!printResponse.succeed)
		{
			ThrowIfNotSuccess(printResponse, request.receipt);
		}
		return printResponse ?? throw new InvalidOperationException("Invalid /print response (empty or malformed JSON).");
	}

	public GetReceiptResponse GetReceipt(GetReceiptRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		return AsyncHelper.RunSync(async () => await GetReceiptAsync(request, cancellationToken));
	}

	public async Task<GetReceiptResponse> GetReceiptAsync(GetReceiptRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (request == null)
		{
			throw new ArgumentNullException("request");
		}
		if (request.receipt_id <= 0)
		{
			throw new ArgumentException("receipt_id must be greater than zero.", "request");
		}
		if (string.IsNullOrWhiteSpace(request.crn))
		{
			throw new ArgumentException("crn must not be null or empty.", "request");
		}
		if (string.IsNullOrWhiteSpace(_options.Token))
		{
			throw new InvalidOperationException("Token is not set. You must complete activation before calling GetReceiptAsync.");
		}
		string content = JsonSerializer.Serialize(request, JsonSerializerOptions);
		using StringContent content2 = new StringContent(content, Encoding.UTF8, "application/json");
		using HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, "ext_app/api/v1/get_receipt")
		{
			Content = content2
		};
		await AddAuthorizationHeaderAsync(httpRequest).ConfigureAwait(continueOnCapturedContext: false);
		using HttpResponseMessage response = await SendWithRefreshIfNeededAsync(httpRequest, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		await EnsureSuccessOrThrowAsync(response).ConfigureAwait(continueOnCapturedContext: false);
		return JsonSerializer.Deserialize<GetReceiptResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false), JsonSerializerOptions) ?? throw new InvalidOperationException("Invalid /get_receipt response (empty or malformed JSON).");
	}

	public FindReceiptByCodeResponse FindReceiptByCode(FindReceiptByCodeRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		return AsyncHelper.RunSync(async () => await FindReceiptByCodeAsync(request, cancellationToken));
	}

	public async Task<FindReceiptByCodeResponse> FindReceiptByCodeAsync(FindReceiptByCodeRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (request == null)
		{
			throw new ArgumentNullException("request");
		}
		if (string.IsNullOrWhiteSpace(request.code))
		{
			throw new ArgumentException("code must not be null or empty.", "request");
		}
		if (string.IsNullOrWhiteSpace(_options.Token))
		{
			throw new InvalidOperationException("Token is not set. You must complete activation before calling FindReceiptByCodeAsync.");
		}
		string content = JsonSerializer.Serialize(request, JsonSerializerOptions);
		using StringContent content2 = new StringContent(content, Encoding.UTF8, "application/json");
		using HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, "ext_app/api/v1/find_receipt_by_code")
		{
			Content = content2
		};
		await AddAuthorizationHeaderAsync(httpRequest).ConfigureAwait(continueOnCapturedContext: false);
		using HttpResponseMessage response = await SendWithRefreshIfNeededAsync(httpRequest, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		await EnsureSuccessOrThrowAsync(response).ConfigureAwait(continueOnCapturedContext: false);
		FindReceiptByCodeResponse findReceiptByCodeResponse = JsonSerializer.Deserialize<FindReceiptByCodeResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false), JsonSerializerOptions);
		if (!findReceiptByCodeResponse.succeed)
		{
			string datamatrix = findReceiptByCodeResponse.error_details?.invalid_codes?.FirstOrDefault();
			EMarkApiException.ThrowReceiptError(findReceiptByCodeResponse.error_code, "", datamatrix);
		}
		return findReceiptByCodeResponse ?? throw new InvalidOperationException("Invalid /find_receipt_by_code response (empty or malformed JSON).");
	}

	public ReturnReceiptResponse ReturnReceipt(ReturnReceiptRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		return AsyncHelper.RunSync(async () => await ReturnReceiptAsync(request, cancellationToken));
	}

	public async Task<ReturnReceiptResponse> ReturnReceiptAsync(ReturnReceiptRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (request == null)
		{
			throw new ArgumentNullException("request");
		}
		if (string.IsNullOrWhiteSpace(request.crn))
		{
			throw new ArgumentException("crn must not be null or empty.", "request");
		}
		if (request.receipt_id <= 0)
		{
			throw new ArgumentException("receipt_id must be greater than zero.", "request");
		}
		if (request.receipt == null || request.receipt.Count == 0)
		{
			throw new ArgumentException("receipt collection must contain at least one item.", "request");
		}
		if (string.IsNullOrWhiteSpace(_options.Token))
		{
			throw new InvalidOperationException("Token is not set. You must complete activation before calling ReturnReceiptAsync.");
		}
		string content = JsonSerializer.Serialize(request, JsonSerializerOptions);
		using StringContent content2 = new StringContent(content, Encoding.UTF8, "application/json");
		using HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, "ext_app/api/v1/return_receipt")
		{
			Content = content2
		};
		await AddAuthorizationHeaderAsync(httpRequest).ConfigureAwait(continueOnCapturedContext: false);
		using HttpResponseMessage response = await SendWithRefreshIfNeededAsync(httpRequest, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		await EnsureSuccessOrThrowAsync(response).ConfigureAwait(continueOnCapturedContext: false);
		ReturnReceiptResponse returnReceiptResponse = JsonSerializer.Deserialize<ReturnReceiptResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
		if (!returnReceiptResponse.succeed)
		{
			ThrowIfNotSuccess(returnReceiptResponse, request.receipt);
		}
		return returnReceiptResponse ?? throw new InvalidOperationException("Invalid /return_receipt response (empty or malformed JSON).");
	}

	private Task AddAuthorizationHeaderAsync(HttpRequestMessage request)
	{
		if (string.IsNullOrWhiteSpace(_options.Token))
		{
			throw new InvalidOperationException("Token is not set. Make sure activation is completed and tokens are available.");
		}
		request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.Token);
		return Task.CompletedTask;
	}

	private Task AddAuthorizationHeaderRefreshTokenAsync(HttpRequestMessage request)
	{
		if (string.IsNullOrWhiteSpace(_options.RefreshToken))
		{
			throw new InvalidOperationException("Token is not set. Make sure activation is completed and tokens are available.");
		}
		request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.RefreshToken);
		return Task.CompletedTask;
	}

	private async Task<HttpResponseMessage> SendWithRefreshIfNeededAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		HttpRequestMessage retryRequest = await CloneRequestAsync(request).ConfigureAwait(continueOnCapturedContext: false);
		HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized && !string.IsNullOrWhiteSpace(_options.RefreshToken))
		{
			httpResponseMessage.Dispose();
			await GetRefreshTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			await AddAuthorizationHeaderRefreshTokenAsync(retryRequest).ConfigureAwait(continueOnCapturedContext: false);
			httpResponseMessage = await _httpClient.SendAsync(retryRequest, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		return httpResponseMessage;
	}

	private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request)
	{
		HttpRequestMessage clone = new HttpRequestMessage(request.Method, request.RequestUri);
		foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
		{
			clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
		}
		if (request.Content != null)
		{
			MemoryStream ms = new MemoryStream();
			await request.Content.CopyToAsync(ms).ConfigureAwait(continueOnCapturedContext: false);
			ms.Position = 0L;
			StreamContent streamContent = new StreamContent(ms);
			if (request.Content.Headers != null)
			{
				foreach (KeyValuePair<string, IEnumerable<string>> header2 in request.Content.Headers)
				{
					streamContent.Headers.TryAddWithoutValidation(header2.Key, header2.Value);
				}
			}
			clone.Content = streamContent;
		}
		return clone;
	}

	private static async Task EnsureSuccessOrThrowAsync(HttpResponseMessage response)
	{
		if (response.IsSuccessStatusCode)
		{
			return;
		}
		string responseContent = string.Empty;
		if (response.Content != null)
		{
			responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		throw new EMarkApiException(response.StatusCode, responseContent);
	}

	private static void ThrowIfNotSuccess(PrintReceiptBaseResponse response, List<EMarkReceiptItem> receiptItems)
	{
		string datamatrix = response.error_details?.invalid_codes?.FirstOrDefault();
		string item = "";
		if (!string.IsNullOrWhiteSpace(datamatrix))
		{
			EMarkReceiptItem eMarkReceiptItem = receiptItems.FirstOrDefault((EMarkReceiptItem r) => r.emarks.Any((string e) => e == datamatrix));
			if (eMarkReceiptItem != null)
			{
				item = eMarkReceiptItem.product_name + " - " + eMarkReceiptItem.measurement_unit;
			}
		}
		EMarkApiException.ThrowReceiptError(response.error_code, item, datamatrix);
	}
}
