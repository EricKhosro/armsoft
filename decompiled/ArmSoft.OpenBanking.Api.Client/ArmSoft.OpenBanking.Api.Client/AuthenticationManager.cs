using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ArmSoft.OpenBanking.Api.Common;

namespace ArmSoft.OpenBanking.Api.Client;

public class AuthenticationManager(ApiClient client) : BaseManager(client)
{
	public async Task<string> Verify(string session)
	{
		VerifyIdentityParams body = new VerifyIdentityParams
		{
			ClientId = Client.ClientId,
			Secret = Client.Secret,
			State = session
		};
		return (await PostAsync<VerifyIdentityResponse>("verify-identity", body)).LoginCode;
	}

	public async Task<AuthenticationResponse> Authenticate(string code)
	{
		AuthenticationParams body = new AuthenticationParams
		{
			ClientId = Client.ClientId,
			Secret = Client.Secret,
			AuthenticationCode = code
		};
		return await PostAsync<AuthenticationResponse>("authenticate", body);
	}

	public async Task SetTokens(string authorizationCode, string redirectUri, string pkceVerifier)
	{
		AuthenticationResponse authenticationResponse = await Token(authorizationCode, redirectUri, pkceVerifier);
		Client.AccessToken = authenticationResponse.AccessToken;
		Client.RefreshToken = authenticationResponse.RefreshToken;
	}

	public async Task<AuthenticationResponse> Token(string authorizationCode, string redirectUri, string pkceVerifier)
	{
		Dictionary<string, string> nameValueCollection = new Dictionary<string, string>
		{
			["grant_type"] = "authorization_code",
			["code"] = authorizationCode,
			["redirect_uri"] = redirectUri,
			["client_id"] = Client.ClientId,
			["client_secret"] = Client.Secret,
			["code_verifier"] = pkceVerifier
		};
		using FormUrlEncodedContent content = new FormUrlEncodedContent(nameValueCollection);
		return await PostAsync<AuthenticationResponse>("token", content);
	}

	public async Task<AuthenticationResponse> Refresh(string refreshToken)
	{
		Dictionary<string, string> nameValueCollection = new Dictionary<string, string>
		{
			["grant_type"] = "refresh_token",
			["refresh_token"] = refreshToken,
			["client_id"] = Client.ClientId,
			["client_secret"] = Client.Secret
		};
		using FormUrlEncodedContent content = new FormUrlEncodedContent(nameValueCollection);
		return await PostAsync<AuthenticationResponse>("token", content);
	}

	public string BuildAuthorizeUrl(string clientId, string redirectUri, string state, string loginCode, string codeVerifier, string? bankCode = null)
	{
		if (string.IsNullOrWhiteSpace(clientId))
		{
			throw new ArgumentException("clientId is required", "clientId");
		}
		if (string.IsNullOrWhiteSpace(redirectUri))
		{
			throw new ArgumentException("redirectUri is required", "redirectUri");
		}
		if (string.IsNullOrWhiteSpace(state))
		{
			throw new ArgumentException("state is required", "state");
		}
		if (string.IsNullOrWhiteSpace(loginCode))
		{
			throw new ArgumentException("loginCode is required", "loginCode");
		}
		if (string.IsNullOrWhiteSpace(codeVerifier))
		{
			throw new ArgumentException("codeVerifier is required", "codeVerifier");
		}
		string value = ComputeCodeChallenge(codeVerifier);
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
		nameValueCollection["response_type"] = "code";
		nameValueCollection["scope"] = "openid";
		nameValueCollection["code_challenge_method"] = "S256";
		nameValueCollection["language"] = client.Language;
		nameValueCollection["client_id"] = clientId;
		nameValueCollection["redirect_uri"] = Uri.EscapeDataString(redirectUri);
		nameValueCollection["state"] = state;
		nameValueCollection["login_code"] = loginCode;
		nameValueCollection["code_challenge"] = value;
		if (!string.IsNullOrWhiteSpace(bankCode))
		{
			nameValueCollection["bank_code"] = bankCode;
		}
		return $"{client.baseAuthenticationApiUrl.TrimEnd('/')}/authorize?{nameValueCollection}";
	}

	private static string ComputeCodeChallenge(string verifier)
	{
		using SHA256 sHA = SHA256.Create();
		return Convert.ToBase64String(sHA.ComputeHash(Encoding.ASCII.GetBytes(verifier))).TrimEnd('=').Replace('+', '-')
			.Replace('/', '_');
	}
}
