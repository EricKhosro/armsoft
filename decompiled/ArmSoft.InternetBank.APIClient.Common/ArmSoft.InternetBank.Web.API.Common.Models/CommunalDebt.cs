using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CommunalDebt
{
	public string Utility { get; set; }

	public UtilityType UtilityType { get; set; }

	public string UtilityName { get; set; }

	public string Location { get; set; }

	public string LocationName { get; set; }

	public string AbonentCode { get; set; }

	public string AbonentID { get; set; }

	public string Name { get; set; }

	public string Address { get; set; }

	public decimal Debt { get; set; }

	public string ComDetails { get; set; }

	public string Special { get; set; }

	public bool IsAbonentTypeNatural { get; set; }

	public string DebtDate { get; set; }

	public List<UtilitySpecialSearchFieldValue> SpecialSearchFields { get; set; }

	public AbonentSearchResult SearchResult { get; set; }
}
