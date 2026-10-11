using System.Net;

namespace ArmSoft.OpenBanking.Api.Common.Utility;

public static class ClientIpAddressHelper
{
	public static string Normalize(IPAddress? ipAddress)
	{
		if (ipAddress == null)
		{
			return string.Empty;
		}
		if (ipAddress.IsIPv4MappedToIPv6)
		{
			ipAddress = ipAddress.MapToIPv4();
		}
		return ipAddress.ToString();
	}
}
