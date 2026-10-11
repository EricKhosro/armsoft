using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient.Core;

public class RubTransferManager : BaseManager
{
	internal RubTransferManager(Client client)
		: base(client)
	{
	}

	public async Task<PR> TransliterateFromRussianToEnglish(PR document)
	{
		return await client.PostAsync<PR>("payments/pr/transliterateFromRussianToEnglish", requireAuthorization: true, null, null, document);
	}

	public async Task<PR> TransliterateFromEnglishToRussian(PR document)
	{
		return await client.PostAsync<PR>("payments/pr/transliterateFromEnglishToRussian", requireAuthorization: true, null, null, document);
	}
}
