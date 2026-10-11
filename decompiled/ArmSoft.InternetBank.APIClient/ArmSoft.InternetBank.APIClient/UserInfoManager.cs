using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class UserInfoManager : BaseManager
{
	internal UserInfoManager(Client client)
		: base(client)
	{
	}

	public async Task<UserInfo> GetUserInfoAsync()
	{
		return await client.GetAsync<UserInfo>("userInfo", requireAuthorization: true);
	}

	public async Task SaveUserPhotoAsync(UserPhoto userPhoto)
	{
		await client.PostAsync("userInfo/savePhoto", requireAuthorization: true, null, null, userPhoto);
	}

	public async Task<UserPhoto> GetUserPhotoAsync()
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["PhotoVersion"] = -1 };
		return await client.GetAsync<UserPhoto>("userInfo/photo", requireAuthorization: true, null, queryParams);
	}

	public async Task RemovePhotoAsync()
	{
		await client.PostAsync("userInfo/photo/delete", requireAuthorization: true);
	}
}
