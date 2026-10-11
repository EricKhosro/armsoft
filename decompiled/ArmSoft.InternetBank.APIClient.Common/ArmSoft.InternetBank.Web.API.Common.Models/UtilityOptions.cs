using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class UtilityOptions
{
	public bool IsLocationSupported { get; set; }

	public bool IsLocationRequired { get; set; }

	public bool IsAbonentTypeSupported { get; set; }

	public bool IsIdentifierSupported { get; set; }

	public bool IsPhoneSupported { get; set; }

	public bool IsAbonentCodePhoneNumber { get; set; }

	public bool IsAmountEditable { get; set; }

	public List<UtilitySpecialSearchField> SpecialSearchFields { get; set; }
}
