using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class PasswordManager : BaseManager
{
	internal PasswordManager(Client client)
		: base(client)
	{
	}

	public async Task ChangePasswordAsync(ChangePassword changePassword)
	{
		await client.PostAsync("password/change", requireAuthorization: true, null, null, changePassword);
	}

	public async Task<string> GetPasswordPolicyHintAsync()
	{
		return await client.GetAsync("password/policyHint", requireAuthorization: true);
	}
}
