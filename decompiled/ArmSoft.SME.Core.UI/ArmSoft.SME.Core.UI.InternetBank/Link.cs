namespace ArmSoft.SME.Core.UI.InternetBank;

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
