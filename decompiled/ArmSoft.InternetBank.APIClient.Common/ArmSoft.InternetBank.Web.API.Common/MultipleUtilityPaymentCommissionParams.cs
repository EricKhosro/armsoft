using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common;

public class MultipleUtilityPaymentCommissionParams
{
	public List<UtilityPaymentCommissionParams> UtilityAmounts { get; set; }
}
