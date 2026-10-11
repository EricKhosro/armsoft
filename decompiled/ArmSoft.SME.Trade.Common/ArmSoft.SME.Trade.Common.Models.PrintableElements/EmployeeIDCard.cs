using System.Collections.Generic;
using ArmSoft.SME.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.PrintableElements;

public class EmployeeIDCard : PrintableElement
{
	public string Caption { get; set; }

	public List<EmployeeCacheRow> EmployeeList { get; set; }

	public string Prefix { get; set; }

	public string Suffix { get; set; }
}
