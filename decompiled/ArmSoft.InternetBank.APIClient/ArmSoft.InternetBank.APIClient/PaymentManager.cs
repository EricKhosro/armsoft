using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.APIClient;

public class PaymentManager : BaseManager
{
	internal PaymentManager(Client client)
		: base(client)
	{
	}

	public List<Payment> GetPayments(DateTime? dateFrom = null, DateTime? dateTo = null, string number = null, string type = null, int? count = null, DocumentState state = DocumentState.NotSet, decimal? amountFrom = null, decimal? amountTo = null, string currency = null, string account = null, string receiver = null)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["Number"] = number,
			["Type"] = type,
			["DateFrom"] = dateFrom,
			["DateTo"] = dateTo,
			["Period"] = Period.Custom,
			["State"] = state,
			["AmountFrom"] = amountFrom,
			["AmountTo"] = amountTo,
			["Currency"] = currency,
			["Account"] = account,
			["Receiver"] = receiver,
			["Count"] = count
		};
		return client.Get<List<Payment>>("payments", requireAuthorization: true, null, queryParams);
	}

	public async Task<List<Payment>> GetPaymentsAsync(DateTime? dateFrom = null, DateTime? dateTo = null, string number = null, string type = null, int? count = null, DocumentState state = DocumentState.NotSet, decimal? amountFrom = null, decimal? amountTo = null, string currency = null, string account = null, string receiver = null)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["Number"] = number,
			["Type"] = type,
			["DateFrom"] = dateFrom,
			["DateTo"] = dateTo,
			["Period"] = Period.Custom,
			["State"] = state,
			["AmountFrom"] = amountFrom,
			["AmountTo"] = amountTo,
			["Currency"] = currency,
			["Account"] = account,
			["Receiver"] = receiver,
			["Count"] = count
		};
		return await client.GetAsync<List<Payment>>("payments", requireAuthorization: true, null, queryParams);
	}

	public List<DocumentListItem> GetDocumentsUserCanSign()
	{
		return client.Get<List<DocumentListItem>>("documentsUserCanSign", requireAuthorization: true);
	}

	public async Task<List<DocumentListItem>> GetDocumentsUserCanSignAsync()
	{
		return await client.GetAsync<List<DocumentListItem>>("documentsUserCanSign", requireAuthorization: true);
	}

	public Challenge GetSignatureOptions(List<Guid> paymentIds)
	{
		return client.Post<Challenge>("payments/signatureoptions", requireAuthorization: true, null, null, paymentIds);
	}

	public async Task<Challenge> GetSignatureOptionsAsync(List<Guid> paymentIds)
	{
		return await client.PostAsync<Challenge>("payments/signatureoptions", requireAuthorization: true, null, null, paymentIds);
	}

	public void SignPayment(Guid paymentId, byte[] timestamp, string securityProviderChallenge = null, string securityProviderResponse = null, UserSignatureMode userSignatureMode = UserSignatureMode.NotSet)
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
		client.Post($"payments/{paymentId}/sign", requireAuthorization: true, null, queryParams, null, dictionary);
	}

	public async Task<(string responseCode, string message)> SignPaymentAsync(Guid paymentId, byte[] timestamp, string securityProviderChallenge = null, string securityProviderResponse = null)
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
		ResponseResult<string> responseResult = await client.PostAsync<ResponseResult<string>>($"payments/{paymentId}/sign", requireAuthorization: true, null, null, null, dictionary);
		return (responseCode: responseResult.ResponseCode, message: responseResult.Message);
	}

	public async Task<PaymentState> SignPaymentAndConfirmCommissionChangeAsync(Guid paymentId, byte[] timestamp, string securityProviderChallenge = null, string securityProviderResponse = null)
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
		return (await client.PostAsync<ResponseResult<PaymentState>>($"payments/{paymentId}/signAndConfirmCommissionChange", requireAuthorization: true, null, null, null, dictionary)).Value;
	}

	public string SignPayments(List<Guid> paymentIds, string securityProviderChallenge = null, string securityProviderResponse = null)
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
		return client.Post<ResponseResult<object>>("payments/groupsign", requireAuthorization: true, null, null, paymentIds, dictionary).Message;
	}

	public async Task<string> SignPaymentsAsync(List<Guid> paymentIds, string securityProviderChallenge = null, string securityProviderResponse = null)
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
		return (await client.PostAsync<ResponseResult<object>>("payments/groupsign", requireAuthorization: true, null, null, paymentIds, dictionary)).Message;
	}

	public T GetNewPayment<T>(Guid? templateId = null) where T : Payment, new()
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["templateId"] = templateId };
		return client.Get<T>($"payments/{typeof(T).Name}/create", requireAuthorization: true, null, queryParams);
	}

	public async Task<T> GetNewPaymentAsync<T>(Guid? templateId = null, Dictionary<string, string> options = null) where T : Payment, new()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object> { ["templateId"] = templateId };
		if (options != null)
		{
			foreach (KeyValuePair<string, string> option in options)
			{
				dictionary.Add(option.Key, option.Value);
			}
		}
		return await client.GetAsync<T>($"payments/{typeof(T).Name}/create", requireAuthorization: true, null, dictionary);
	}

	public void CreatePayment<T>(T payment, bool ignoreWarnings = false, bool savePayment = true, bool saveTemplate = false, bool savePayee = false) where T : Payment
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["forceUpdate"] = ignoreWarnings,
			["savePayment"] = savePayment,
			["saveTemplate"] = saveTemplate,
			["savePayee"] = savePayee
		};
		client.Post($"payments/{typeof(T).Name}/create", requireAuthorization: true, null, queryParams, payment);
	}

	public async Task CreatePaymentAsync<T>(T payment, bool ignoreWarnings = false, bool savePayment = true, bool saveTemplate = false, bool savePayee = false) where T : Payment
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["forceUpdate"] = ignoreWarnings,
			["savePayment"] = savePayment,
			["saveTemplate"] = saveTemplate,
			["savePayee"] = savePayee
		};
		await client.PostAsync($"payments/{typeof(T).Name}/create", requireAuthorization: true, null, queryParams, payment);
	}

	public T GetPayment<T>(Guid docId) where T : Payment, new()
	{
		return client.Get<T>($"payments/{typeof(T).Name}/{docId}/details", requireAuthorization: true);
	}

	public async Task<T> GetPaymentAsync<T>(Guid docId) where T : Payment, new()
	{
		return await client.GetAsync<T>($"payments/{typeof(T).Name}/{docId}/details", requireAuthorization: true);
	}

	public void UpdatePayment<T>(T payment, bool ignoreWarnings = false) where T : Payment
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["forceUpdate"] = ignoreWarnings };
		Dictionary<string, string> headers = new Dictionary<string, string> { ["X-TimeStamp"] = Convert.ToBase64String(payment.TimeStamp) };
		client.Post($"payments/{typeof(T).Name}/{payment.DocId}/update", requireAuthorization: true, null, queryParams, payment, headers);
	}

	public async Task UpdatePaymentAsync<T>(T payment, bool ignoreWarnings = false) where T : Payment
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["forceUpdate"] = ignoreWarnings };
		Dictionary<string, string> headers = new Dictionary<string, string> { ["X-TimeStamp"] = Convert.ToBase64String(payment.TimeStamp) };
		await client.PostAsync($"payments/{typeof(T).Name}/{payment.DocId}/update", requireAuthorization: true, null, queryParams, payment, headers);
	}

	public void RemovePayment(Guid id, byte[] timestamp)
	{
		Dictionary<string, string> headers = new Dictionary<string, string> { ["X-TimeStamp"] = Convert.ToBase64String(timestamp) };
		client.Post($"payments/{id}/delete", requireAuthorization: true, null, null, null, headers);
	}

	public async Task RemovePaymentAsync(Guid id, byte[] timestamp)
	{
		Dictionary<string, string> headers = new Dictionary<string, string> { ["X-TimeStamp"] = Convert.ToBase64String(timestamp) };
		await client.PostAsync($"payments/{id}/delete", requireAuthorization: true, null, null, null, headers);
	}

	public void RemovePayments(List<Guid> ids)
	{
		client.Post("payments/groupdelete", requireAuthorization: true, null, null, ids);
	}

	public async Task RemovePaymentsAsync(List<Guid> ids)
	{
		await client.PostAsync("payments/groupdelete", requireAuthorization: true, null, null, ids);
	}

	public List<Link> GetPaymentLinks(Guid id)
	{
		return client.Get<List<Link>>($"payments/{id}/links", requireAuthorization: true);
	}

	public async Task<List<Link>> GetPaymentLinksAsync(Guid id)
	{
		return await client.GetAsync<List<Link>>($"payments/{id}/links", requireAuthorization: true);
	}

	public List<PaymentType> GetPaymentTypes(string accountNo = null)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["accountNo"] = accountNo };
		return client.Get<List<PaymentType>>("payments/usableTypes", requireAuthorization: true, null, queryParams);
	}

	public async Task<List<PaymentType>> GetPaymentTypesAsync(string accountNo = null)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["accountNo"] = accountNo };
		return await client.GetAsync<List<PaymentType>>("payments/usableTypes", requireAuthorization: true, null, queryParams);
	}

	public List<ArmSoft.InternetBank.Web.API.Common.Models.PaymentGroup> GetPaymentGroups()
	{
		return client.Get<List<ArmSoft.InternetBank.Web.API.Common.Models.PaymentGroup>>("identity/paymentGroups", requireAuthorization: true);
	}

	public async Task<List<ArmSoft.InternetBank.Web.API.Common.Models.PaymentGroup>> GetPaymentGroupsAsync()
	{
		return await client.GetAsync<List<ArmSoft.InternetBank.Web.API.Common.Models.PaymentGroup>>("v4/identity/paymentGroups", requireAuthorization: true);
	}

	public async Task<PaymentTypeResponse> GetPaymentTypeAsync(Guid id)
	{
		PaymentTypeParams postBody = new PaymentTypeParams
		{
			Id = id
		};
		return (await client.PostAsync<ResponseResult<PaymentTypeResponse>>("payments/type", requireAuthorization: true, null, null, postBody)).Value;
	}

	public async Task<List<DocumentHistory>> GetPaymentHistoryAsync(Guid id)
	{
		return await client.GetAsync<List<DocumentHistory>>($"payments/{id}/history", requireAuthorization: true);
	}

	public async Task<List<RequiredSignaturesGroup>> GetRequiredSignaturesForPaymentAsync(Guid id)
	{
		return await client.GetAsync<List<RequiredSignaturesGroup>>($"payments/{id}/requiredSignatures", requireAuthorization: true);
	}

	public async Task<List<DocumentListItem>> GetSignableDocumentsAsync()
	{
		return await client.GetAsync<List<DocumentListItem>>("documentsUserCanSign", requireAuthorization: true);
	}

	public async Task<Challenge> GetDocumentsSignatureOptionsAsync(List<Guid> documentIds)
	{
		return await client.PostAsync<Challenge>("documentsUserCanSign/signatureoptions", requireAuthorization: true, null, null, documentIds);
	}

	public async Task<string> SignDocumentsAsync(List<Guid> documentIds, string securityProviderChallenge = null, string securityProviderResponse = null)
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
		return (await client.PostAsync<ResponseResult<object>>("documentsUserCanSign/groupsign", requireAuthorization: true, null, null, documentIds, dictionary)).Message;
	}

	public async Task SendPushNotificationForDocumentSignAsync(List<Guid> documentIds)
	{
		PushNotificationForDocumentSignParams postBody = new PushNotificationForDocumentSignParams
		{
			DocumentIds = documentIds
		};
		await client.PostAsync<object>("notifications/send-document-sign-push", requireAuthorization: true, null, null, postBody);
	}

	public async Task SendToEditAsync(Guid documentId)
	{
		await client.PostAsync<object>("document/sendToEdit", requireAuthorization: true, null, null, documentId);
	}
}
