using System.Collections.Generic;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.Web.API.Common;

public class CommunalDebtParams
{
	public string Utility { get; set; }

	public string Location { get; set; }

	public string AbonentCode { get; set; }

	public string AbonentId { get; set; }

	public string AbonentPhone { get; set; }

	public bool AbonentType { get; set; }

	public List<UtilitySpecialSearchFieldValue> UtilitySpecialSearchFields { get; set; }
}
