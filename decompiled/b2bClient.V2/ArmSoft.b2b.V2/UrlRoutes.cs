using System.Collections.Generic;
using System.Text;

namespace ArmSoft.b2b.V2;

public class UrlRoutes
{
	private B2BClient client;

	internal UrlRoutes(B2BClient client)
	{
		this.client = client;
	}

	public Dictionary<UrlType, string> GetStaticUrls()
	{
		try
		{
			StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Urls");
			return client.Send<Dictionary<UrlType, string>>("GET", stringBuilder.ToString());
		}
		catch
		{
			return null;
		}
	}

	public string Registration(int actorId, string appKeyCode)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Urls/Registration");
		stringBuilder.AddQueryParameter(new { actorId, appKeyCode });
		return client.Send<string>("GET", stringBuilder.ToString());
	}

	public string MapMarkers(MapRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Urls/MapMarkers");
		return client.Send<string>("POST", stringBuilder.ToString(), request);
	}

	public string ActorProducts(int actorId, bool isUrlInPatverArmsoft)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Urls/ActorProducts");
		stringBuilder.AddQueryParameter(new { actorId, isUrlInPatverArmsoft });
		return client.Send<string>("GET", stringBuilder.ToString());
	}

	public string SMTermsAgreement()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Urls/TermsAgreement/SM");
		return client.Send<string>("GET", stringBuilder.ToString());
	}
}
