namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class ApplicationType
{
	public string Id { get; set; }

	public string Title { get; set; }

	public string ImageUrl { get; set; }

	public ApplicationType(string id, string title, string imageUrl)
	{
		Id = id;
		Title = title;
		ImageUrl = imageUrl;
	}
}
