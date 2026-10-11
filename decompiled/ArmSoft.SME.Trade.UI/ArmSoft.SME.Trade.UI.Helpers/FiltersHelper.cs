using System.Collections.Generic;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;

namespace ArmSoft.SME.Trade.UI.Helpers;

public class FiltersHelper
{
	public static List<string> SumTypeList()
	{
		return new List<string>
		{
			ArmSoft.SME.Trade.Common.Resources.AnalysSumTypeNcc,
			ArmSoft.SME.Trade.Common.Resources.AnalysSumTypeVat,
			string.Format(ArmSoft.SME.Trade.Common.Resources.AnalysSumTypeCur, Settings.ParametersService.MTCurr())
		};
	}

	public static List<string> CompareFeaturesList(bool includeProfitPercent = true, bool includeQuantity = true)
	{
		List<string> list = new List<string>();
		if (includeQuantity)
		{
			list.Add(ArmSoft.SME.Common.Resources.Quantity);
		}
		list.Add(ArmSoft.SME.Common.Resources.Income);
		list.Add(ArmSoft.SME.Common.Resources.ProfitWord);
		if (includeProfitPercent)
		{
			list.Add(ArmSoft.SME.Common.Resources.ProfitPercent);
		}
		return list;
	}

	public static List<string> PeriodList()
	{
		return new List<string>
		{
			ArmSoft.SME.Common.Resources.Dayly,
			ArmSoft.SME.Common.Resources.Monthly,
			ArmSoft.SME.Common.Resources.Yearly,
			ArmSoft.SME.Common.Resources.Weekly
		};
	}
}
