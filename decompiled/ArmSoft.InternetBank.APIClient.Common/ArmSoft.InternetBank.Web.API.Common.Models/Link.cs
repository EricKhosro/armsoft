namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class Link
{
	public string Rel { get; set; }

	public string HRef { get; set; }

	public Link(string rel, string hRef)
	{
		Rel = rel;
		HRef = hRef;
	}
}
