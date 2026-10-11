using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ArmSoft.OnlineBanking.Common.Constants;

public static class AdvancedLocalPaymentTypes
{
	public const string CreditPayment = "ALCP";

	public const string SoldierPayment = "ALSP";

	public const string GeneralPayment = "ALGP";

	public static IEnumerable<string> GetAllTypes()
	{
		return from x in typeof(AdvancedLocalPaymentTypes).GetFields(BindingFlags.Static | BindingFlags.Public)
			select (string)x.GetValue(null);
	}

	public static bool Contains(string value)
	{
		return GetAllTypes().Contains(value);
	}
}
