using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class PensionStatementManager : BaseManager
{
	public PensionStatementManager(Client client)
		: base(client)
	{
	}

	public async Task<PensionStatement> GetPensionStatementAsync()
	{
		return await client.GetAsync<PensionStatement>("pensionStatement", requireAuthorization: true);
	}
}
