using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CAField : CAFieldBase
{
	public Dictionary<string, object> Properties { get; set; }

	public string DisplayValue { get; set; }
}
