using System;
using System.Text;
using System.Threading.Tasks;
using ArmSoft.OnlineBanking.Common.Enums;
using ArmSoft.OpenBanking.Api.Common;

namespace ArmSoft.OpenBanking.Api.Client;

public class DocumentManager<TModel, TDetails, TForEdit>(ApiClient client) : DocumentManager<TModel>(client) where TModel : Document where TDetails : DocumentDetails<TModel> where TForEdit : DocumentForEdit<TModel>
{
	public new async Task<TDetails> GetDetails(Guid id)
	{
		return (TDetails)(await GetDetailsInner(id));
	}

	public new async Task<TForEdit> GetForEdit(Guid id)
	{
		return (TForEdit)(await GetForEditInner(id));
	}

	protected override async Task<DocumentDetails<TModel>> GetDetailsInner(Guid id)
	{
		return await client.Post<TDetails>(GetControllerName() + "/v1/get-details", requireAuthorization: true, null, new GetSignableObjectParams
		{
			Id = id
		});
	}

	protected override async Task<DocumentForEdit<TModel>> GetForEditInner(Guid id)
	{
		return await client.Post<TForEdit>(GetControllerName() + "/v1/get-for-edit", requireAuthorization: true, null, new GetSignableObjectParams
		{
			Id = id
		});
	}
}
public class DocumentManager<TModel>(ApiClient client) : BaseManager(client), IDocumentManager<TModel> where TModel : Document
{
	public async Task<DocumentForEdit<TModel>> GetNew(Guid? templateId = null, bool isCurrencyConversion = false)
	{
		return await client.Post<DocumentForEdit<TModel>>(GetControllerName() + "/v2/get-new", requireAuthorization: true, null, new GetNewLocalPaymentParams
		{
			TemplateId = templateId,
			IsForCurrencyConversion = isCurrencyConversion
		});
	}

	public async Task<DocumentForEdit<TModel>> GetForEdit(Guid id)
	{
		return await GetForEditInner(id);
	}

	public async Task<DocumentDetails<TModel>> GetDetails(Guid id)
	{
		return await GetDetailsInner(id);
	}

	protected virtual async Task<DocumentDetails<TModel>> GetDetailsInner(Guid id)
	{
		return await client.Post<DocumentDetails<TModel>>(GetControllerName() + "/v1/get-details", requireAuthorization: true, null, new GetSignableObjectParams
		{
			Id = id
		});
	}

	protected virtual async Task<DocumentForEdit<TModel>> GetForEditInner(Guid id)
	{
		return await client.Post<DocumentForEdit<TModel>>(GetControllerName() + "/v1/get-for-edit", requireAuthorization: true, null, new GetSignableObjectParams
		{
			Id = id
		});
	}

	public async Task Create(TModel model, bool saveDocument = true, bool saveTemplate = false, bool ignoreWarnings = false, byte[]? attachment = null, AttachmentAction attachmentAction = AttachmentAction.IgnoreModification)
	{
		CreateDocumentParams<TModel> postBody = new CreateDocumentParams<TModel>
		{
			Model = model,
			SaveDocument = saveDocument,
			SaveTemplate = saveTemplate,
			IgnoreWarnings = ignoreWarnings,
			AttachmentAction = attachmentAction,
			Attachment = attachment
		};
		await client.Post<object>(GetControllerName() + "/v2/create", requireAuthorization: true, null, postBody);
	}

	public async Task Update(TModel model, bool ignoreWarnings = false, bool saveDocument = true, bool saveTemplate = false, byte[]? attachment = null, AttachmentAction? attachmentAction = null)
	{
		UpdateDocumentParams<TModel> postBody = new UpdateDocumentParams<TModel>
		{
			Model = model,
			IgnoreWarnings = ignoreWarnings,
			SaveDocument = saveDocument,
			SaveTemplate = saveTemplate,
			AttachmentAction = attachmentAction,
			Attachment = attachment
		};
		await client.Post<object>(GetControllerName() + "/v2/update", requireAuthorization: true, null, postBody);
	}

	public async Task Delete(Guid id, byte[] version)
	{
		await client.Post<object>(GetControllerName() + "/v1/delete", requireAuthorization: true, null, new DeleteSignableObjectParams
		{
			Id = id,
			Version = version
		});
	}

	protected string GetControllerName()
	{
		string name = typeof(TModel).Name;
		StringBuilder stringBuilder = new StringBuilder(name.Length);
		stringBuilder.Append(char.ToLower(name[0]));
		for (int i = 1; i < name.Length; i++)
		{
			if (char.IsUpper(name[i]))
			{
				stringBuilder.Append("-");
				stringBuilder.Append(char.ToLower(name[i]));
			}
			else
			{
				stringBuilder.Append(name[i]);
			}
		}
		return stringBuilder.ToString();
	}
}
