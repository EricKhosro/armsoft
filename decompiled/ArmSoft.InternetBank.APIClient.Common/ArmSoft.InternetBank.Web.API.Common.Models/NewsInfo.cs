using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class NewsInfo
{
	public int Id { get; set; }

	public string Date { get; set; }

	public string ExpirationDate { get; set; }

	public string LastUpdateDate { get; set; }

	public string Title { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ShortDescription { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string FullDescription { get; set; }

	public bool IsAlwaysVisible { get; set; }

	public bool AllowRewatchViewedNews { get; set; }

	public string[] MenuItems { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ImageUrl { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string LinkText { get; set; }

	public LinkType LinkType { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string LinkUrl { get; set; }

	public string LinkDocumentType { get; set; }

	public string LinkModule { get; set; }
}
