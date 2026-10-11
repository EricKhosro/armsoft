using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class OpenAccountTypeConfiguration
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string Type { get; set; }

	public List<Currerncy> Currencies { get; set; }

	public string ShortDescription { get; set; }

	public string Description { get; set; }
}
