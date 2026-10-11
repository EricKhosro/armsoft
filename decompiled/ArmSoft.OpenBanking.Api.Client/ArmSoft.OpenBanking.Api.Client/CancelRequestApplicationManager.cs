using System.Threading.Tasks;
using ArmSoft.OpenBanking.Api.Common;

namespace ArmSoft.OpenBanking.Api.Client;

public class CancelRequestApplicationManager(ApiClient client) : DocumentManager<CancelRequestApplication, CancelRequestApplicationDetails, CancelRequestApplicationForEdit>(client)
{
	public async Task<CancelRequestApplicationForEdit> GetNew(GetNewCancelRequestApplicationParams requestParams)
	{
		return await client.Post<CancelRequestApplicationForEdit>(GetControllerName() + "/v2/get-new", requireAuthorization: true, null, new GetNewCancelRequestApplicationParams
		{
			TemplateId = requestParams.TemplateId,
			TargetId = requestParams.TargetId
		});
	}
}
