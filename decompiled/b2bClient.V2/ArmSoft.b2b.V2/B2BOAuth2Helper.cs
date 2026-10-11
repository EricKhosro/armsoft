using System;

namespace ArmSoft.b2b.V2;

public static class B2BOAuth2Helper
{
	public static string GetBaseAddress(string wcfBaseAddress)
	{
		if (wcfBaseAddress.StartsWith("https://services.armsoft.am:4443/") || wcfBaseAddress.StartsWith("https://b2badmin.armsoft.am:4443/"))
		{
			return B2BClient.DefaultBaseAddress();
		}
		if (wcfBaseAddress.StartsWith("https://hartak:5443/"))
		{
			return "https://hartak:4443/";
		}
		if (wcfBaseAddress.StartsWith("https://hartak:9443/"))
		{
			return "https://hartak:7443/";
		}
		if (wcfBaseAddress.StartsWith("https://localhost:44308/"))
		{
			return "https://localhost:44364/";
		}
		throw new ArgumentException($"{wcfBaseAddress} invalid address", "wcfBaseAddress");
	}
}
