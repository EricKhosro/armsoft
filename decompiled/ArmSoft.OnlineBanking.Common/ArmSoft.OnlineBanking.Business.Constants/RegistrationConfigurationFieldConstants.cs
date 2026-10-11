using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ArmSoft.OnlineBanking.Business.Constants;

public static class RegistrationConfigurationFieldConstants
{
	public const string Address = "RegistrationFieldAddress";

	public const string Email = "RegistrationFieldEmail";

	public const string PhoneNumber = "RegistrationFieldPhoneNumber";

	public const string StatementDeliveryMethod = "RegistrationFieldStatementDeliveryMethod";

	public static IEnumerable<string> GetAllFieldNames()
	{
		return from x in typeof(RegistrationConfigurationFieldConstants).GetFields(BindingFlags.Static | BindingFlags.Public)
			select (string)x.GetValue(null);
	}
}
