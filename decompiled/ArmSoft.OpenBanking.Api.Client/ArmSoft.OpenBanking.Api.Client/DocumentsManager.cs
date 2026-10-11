using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.OnlineBanking.Common.Enums;
using ArmSoft.OpenBanking.Api.Client.Core;
using ArmSoft.OpenBanking.Api.Common;

namespace ArmSoft.OpenBanking.Api.Client;

public class DocumentsManager(ApiClient client) : BaseManager(client)
{
	public async Task<List<DocumentListItem>> GetDocuments(DateTime dateFrom, DateTime dateTo, string? number = null, string? type = null, int? count = null, DocumentState? state = null, decimal? amountFrom = null, decimal? amountTo = null, string? currency = null, string? payerAccount = null, string? receiver = null, bool includeCreatorNames = false, string? category = null)
	{
		DocumentListItemParams postBody = new DocumentListItemParams
		{
			Number = number,
			Type = type,
			DateFrom = dateFrom,
			DateTo = dateTo,
			Count = count,
			State = state,
			AmountFrom = amountFrom,
			AmountTo = amountTo,
			Currency = currency,
			PayerAccount = payerAccount,
			Receiver = receiver,
			IncludeCreatorNames = includeCreatorNames,
			Category = category
		};
		return await client.Post<List<DocumentListItem>>("documents/v1/get-list", requireAuthorization: true, null, postBody);
	}

	public async Task<SignatureOptions> GetSignatureOptions(List<Guid> ids)
	{
		GetDocumentSignatureOptionsParams postBody = new GetDocumentSignatureOptionsParams
		{
			Ids = ids
		};
		return await client.Post<SignatureOptions>("documents/v1/get-signature-options", requireAuthorization: true, null, postBody);
	}

	public async Task<(string responseCode, string message)> Sign(Guid docId, string? securityProviderChallenge, string signature, byte[]? version, UserSignatureMode userSignatureMode = UserSignatureMode.NotSet)
	{
		Response<object> response = await client.PostResponse<object>("documents/v1/sign", requireAuthorization: true, null, new SignDocumentParams
		{
			Id = docId,
			SecurityProviderChallenge = securityProviderChallenge,
			Signature = signature,
			Version = version,
			UserSignatureMode = userSignatureMode
		});
		return (responseCode: response.ResponseCode, message: response.Message);
	}

	public async Task<SignedDocumentsResult> GroupSign(List<Guid> ids, string? securityProviderChallenge, string signature, UserSignatureMode userSignatureMode = UserSignatureMode.NotSet)
	{
		SignDocumentsParams postBody = new SignDocumentsParams
		{
			Ids = ids,
			Signature = signature,
			SecurityProviderChallenge = securityProviderChallenge,
			UserSignatureMode = userSignatureMode
		};
		return await client.Post<SignedDocumentsResult>("documents/v1/group-sign", requireAuthorization: true, null, postBody);
	}

	public async Task GroupDelete(List<Guid> ids)
	{
		DeleteDocumentsParams postBody = new DeleteDocumentsParams
		{
			Ids = ids
		};
		await client.Post<object>("documents/v1/group-delete", requireAuthorization: true, null, postBody);
	}

	public async Task<IEnumerable<DocumentHistory>> GetHistory(Guid id)
	{
		GetDocumentHistoryParams postBody = new GetDocumentHistoryParams
		{
			Id = id
		};
		return await client.Post<IEnumerable<DocumentHistory>>("documents/v1/get-history", requireAuthorization: true, null, postBody);
	}
}
