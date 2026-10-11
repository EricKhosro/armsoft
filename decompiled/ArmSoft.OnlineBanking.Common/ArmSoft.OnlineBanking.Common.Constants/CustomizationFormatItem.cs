using System.Collections.Generic;

namespace ArmSoft.OnlineBanking.Common.Constants;

public static class CustomizationFormatItem
{
	public const string FullName = "[NAME]";

	public const string Mobile = "[MOBILE]";

	public const string Email = "[EMAIL]";

	public const string ClientName = "[CLIENTNAME]";

	public const string ClientGroup = "[CLIENTGROUP]";

	public const string ClientDepartment = "[CLIENTDEPARTMENT]";

	public const string TaxCode = "[TAXCODE]";

	public const string SocCard = "[SOCCARD]";

	public const string Passport = "[PASSPORT]";

	public static List<string> UiExtensionFormatItems => new List<string> { "[NAME]", "[MOBILE]", "[EMAIL]", "[CLIENTNAME]", "[CLIENTGROUP]", "[TAXCODE]", "[SOCCARD]", "[PASSPORT]" };
}
