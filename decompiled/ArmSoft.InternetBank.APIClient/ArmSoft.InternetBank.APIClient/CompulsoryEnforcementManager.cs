using System.Threading.Tasks;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class CompulsoryEnforcementManager : BaseManager
{
	internal CompulsoryEnforcementManager(Client client)
		: base(client)
	{
	}

	public async Task<CompulsoryEnforcement> GetCompulsoryEnforcementAsync(string identificationDocumentNumber)
	{
		CompulsoryEnforcementParams postBody = new CompulsoryEnforcementParams
		{
			IdentificationDocumentNumber = identificationDocumentNumber
		};
		return (await client.PostAsync<ResponseResult<CompulsoryEnforcement>>("compulsoryEnforcements", requireAuthorization: true, null, null, postBody)).Value;
	}
}
