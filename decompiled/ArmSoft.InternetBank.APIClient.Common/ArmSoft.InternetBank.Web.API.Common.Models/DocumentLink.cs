namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class DocumentLink
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Url { get; set; }

	public string CreditTypes { get; set; }

	public bool ShowAcceptTerms { get; set; }

	public bool Accepted { get; set; }

	public DocumentLink(string id, string name, string url, bool showAcceptTerms, string creditTypes)
	{
		Accepted = false;
		Id = id;
		Name = name;
		Url = url;
		ShowAcceptTerms = showAcceptTerms;
		CreditTypes = creditTypes;
	}
}
