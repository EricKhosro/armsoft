using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.APIClient;

public class CancelRequestManager : BaseManager
{
	internal CancelRequestManager(Client client)
		: base(client)
	{
	}

	public CancelRequest GetNewCancelRequest(Guid targetId)
	{
		return client.Get<CancelRequest>($"cancelRequests/{targetId}/create", requireAuthorization: true);
	}

	public async Task<CancelRequest> GetNewCancelRequestAsync(Guid targetId)
	{
		return await client.GetAsync<CancelRequest>($"cancelRequests/{targetId}/create", requireAuthorization: true);
	}

	public void CreateCancelRequest(CancelRequest cancelRequest, bool ignoreWarnings = false)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["forceUpdate"] = ignoreWarnings };
		client.Post("cancelRequests/create", requireAuthorization: true, null, queryParams, cancelRequest);
	}

	public async Task CreateCancelRequestAsync(CancelRequest cancelRequest, bool ignoreWarnings = false)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["forceUpdate"] = ignoreWarnings };
		await client.PostAsync("cancelRequests/create", requireAuthorization: true, null, queryParams, cancelRequest);
	}

	public List<CancelRequest> GetCancelRequests(DateTime? dateFrom = null, DateTime? dateTo = null, string number = null, DocumentState state = DocumentState.NotSet)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["Number"] = number,
			["DateFrom"] = dateFrom,
			["DateTo"] = dateTo,
			["Period"] = Period.Custom,
			["State"] = state
		};
		return client.Get<List<CancelRequest>>("cancelRequests", requireAuthorization: true, null, queryParams);
	}

	public async Task<List<CancelRequest>> GetCancelRequestsAsync(DateTime? dateFrom = null, DateTime? dateTo = null, string number = null, DocumentState state = DocumentState.NotSet)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["Number"] = number,
			["DateFrom"] = dateFrom,
			["DateTo"] = dateTo,
			["Period"] = Period.Custom,
			["State"] = state
		};
		return await client.GetAsync<List<CancelRequest>>("cancelRequests", requireAuthorization: true, null, queryParams);
	}

	public CancelRequest GetCancelRequest(Guid id)
	{
		return client.Get<CancelRequest>($"cancelRequests/{id}/details", requireAuthorization: true);
	}

	public async Task<CancelRequest> GetCancelRequestAsync(Guid id)
	{
		return await client.GetAsync<CancelRequest>($"cancelRequests/{id}/details", requireAuthorization: true);
	}

	public void UpdateCancelRequest(CancelRequest cancelRequest, bool ignoreWarnings = false)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["forceUpdate"] = ignoreWarnings };
		Dictionary<string, string> headers = new Dictionary<string, string> { ["X-TimeStamp"] = Convert.ToBase64String(cancelRequest.TimeStamp) };
		client.Post($"cancelRequests/{cancelRequest.DocId}/update", requireAuthorization: true, null, queryParams, cancelRequest, headers);
	}

	public async Task UpdateCancelRequestAsync(CancelRequest cancelRequest, bool ignoreWarnings = false)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["forceUpdate"] = ignoreWarnings };
		Dictionary<string, string> headers = new Dictionary<string, string> { ["X-TimeStamp"] = Convert.ToBase64String(cancelRequest.TimeStamp) };
		await client.PostAsync($"cancelRequests/{cancelRequest.DocId}/update", requireAuthorization: true, null, queryParams, cancelRequest, headers);
	}

	public Challenge GetSignatureOptions(List<Guid> cancelRequestIds)
	{
		return client.Post<Challenge>("cancelRequests/signatureoptions", requireAuthorization: true, null, null, cancelRequestIds);
	}

	public async Task<Challenge> GetSignatureOptionsAsync(List<Guid> cancelRequestIds)
	{
		return await client.PostAsync<Challenge>("cancelRequests/signatureoptions", requireAuthorization: true, null, null, cancelRequestIds);
	}

	public void SignCancelRequest(Guid cancelRequestId, byte[] timestamp, UserSignatureMode userSignatureMode = UserSignatureMode.NotSet, string securityProviderChallenge = null, string securityProviderResponse = null)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (!string.IsNullOrEmpty(securityProviderChallenge))
		{
			dictionary["X-Challenge"] = securityProviderChallenge;
		}
		if (!string.IsNullOrEmpty(securityProviderResponse))
		{
			dictionary["X-Token"] = securityProviderResponse;
		}
		dictionary["X-TimeStamp"] = Convert.ToBase64String(timestamp);
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["userSignatureMode"] = userSignatureMode };
		client.Post($"cancelRequests/{cancelRequestId}/sign", requireAuthorization: true, null, queryParams, null, dictionary);
	}

	public async Task SignCancelRequestAsync(Guid cancelRequestId, byte[] timestamp, string securityProviderChallenge = null, string securityProviderResponse = null)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (!string.IsNullOrEmpty(securityProviderChallenge))
		{
			dictionary["X-Challenge"] = securityProviderChallenge;
		}
		if (!string.IsNullOrEmpty(securityProviderResponse))
		{
			dictionary["X-Token"] = securityProviderResponse;
		}
		dictionary["X-TimeStamp"] = Convert.ToBase64String(timestamp);
		await client.PostAsync($"cancelRequests/{cancelRequestId}/sign", requireAuthorization: true, null, null, null, dictionary);
	}

	public void SignCancelRequests(List<Guid> cancelRequestId, string securityProviderChallenge = null, string securityProviderResponse = null)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (!string.IsNullOrEmpty(securityProviderChallenge))
		{
			dictionary["X-Challenge"] = securityProviderChallenge;
		}
		if (!string.IsNullOrEmpty(securityProviderResponse))
		{
			dictionary["X-Token"] = securityProviderResponse;
		}
		client.Post("cancelRequests/groupsign", requireAuthorization: true, null, null, cancelRequestId, dictionary);
	}

	public async Task SignCancelRequestsAsync(List<Guid> cancelRequestId, string securityProviderChallenge = null, string securityProviderResponse = null)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (!string.IsNullOrEmpty(securityProviderChallenge))
		{
			dictionary["X-Challenge"] = securityProviderChallenge;
		}
		if (!string.IsNullOrEmpty(securityProviderResponse))
		{
			dictionary["X-Token"] = securityProviderResponse;
		}
		await client.PostAsync("cancelRequests/groupsign", requireAuthorization: true, null, null, cancelRequestId, dictionary);
	}

	public void RemoveCancelRequest(Guid id, byte[] timestamp)
	{
		Dictionary<string, string> headers = new Dictionary<string, string> { ["X-TimeStamp"] = Convert.ToBase64String(timestamp) };
		client.Post($"cancelRequests/{id}/delete", requireAuthorization: true, null, null, null, headers);
	}

	public async Task RemoveCancelRequestAsync(Guid id, byte[] timestamp)
	{
		Dictionary<string, string> headers = new Dictionary<string, string> { ["X-TimeStamp"] = Convert.ToBase64String(timestamp) };
		await client.PostAsync($"cancelRequests/{id}/delete", requireAuthorization: true, null, null, null, headers);
	}

	public void RemoveCancelRequests(List<Guid> ids)
	{
		client.Post("cancelRequests/groupdelete", requireAuthorization: true, null, null, ids);
	}

	public async Task RemoveCancelRequestsAsync(List<Guid> ids)
	{
		await client.PostAsync("cancelRequests/groupdelete", requireAuthorization: true, null, null, ids);
	}
}
