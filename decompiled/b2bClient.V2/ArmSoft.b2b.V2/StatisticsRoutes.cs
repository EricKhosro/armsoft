using System.Collections.Generic;
using System.Text;

namespace ArmSoft.b2b.V2;

public class StatisticsRoutes
{
	private B2BClient client;

	internal StatisticsRoutes(B2BClient client)
	{
		this.client = client;
	}

	public CommonStatistics GetCommonStatistics()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Statistics/Common");
		return client.Send<CommonStatistics>("GET", stringBuilder.ToString());
	}

	public List<OrganizationStatistics> GetOrganizationStatistics(List<string> taxCodes)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Statistics/Organizations");
		return client.Send<List<OrganizationStatistics>>("POST", stringBuilder.ToString(), new
		{
			codes = taxCodes
		});
	}

	public int GetVersion(string resourceName)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Statistics/{resourceName}/VersionNumber");
		stringBuilder.AddPathParameterValue("{resourceName}", resourceName);
		return client.Send<int>("GET", stringBuilder.ToString());
	}
}
