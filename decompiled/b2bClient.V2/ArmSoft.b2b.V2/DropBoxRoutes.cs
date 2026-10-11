using System;
using System.Text;

namespace ArmSoft.b2b.V2;

public class DropBoxRoutes
{
	private readonly B2BClient client;

	internal DropBoxRoutes(B2BClient client)
	{
		this.client = client;
	}

	public Uri GetAuthorizationUri()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Urls/DropBoxAuthorization");
		return client.Send<Uri>("GET", stringBuilder.ToString());
	}

	public Uri GetRedirectUri(string oauth2State)
	{
		if (string.IsNullOrEmpty(oauth2State))
		{
			throw new ArgumentNullException("oauth2State");
		}
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Account/GetRedirectedUri/{oauth2State}");
		stringBuilder.AddPathParameterValue("{oauth2State}", oauth2State);
		return client.Send<Uri>("GET", stringBuilder.ToString());
	}
}
