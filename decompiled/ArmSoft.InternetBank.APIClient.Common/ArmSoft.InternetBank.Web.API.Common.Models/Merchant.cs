using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class Merchant
{
	public string Id { get; set; }

	public string Name { get; set; }

	public List<Terminal> Terminals { get; set; }
}
