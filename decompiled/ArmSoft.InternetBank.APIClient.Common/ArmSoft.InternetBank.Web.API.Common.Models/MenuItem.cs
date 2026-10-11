using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class MenuItem
{
	public string Id { get; set; }

	public string Title { get; set; }

	public int Level { get; set; }

	public string Parent { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Badge { get; set; }

	public MenuItem()
		: this(string.Empty, string.Empty, -1, string.Empty, null)
	{
	}

	public MenuItem(string id, string title, int level, string parent)
		: this(id, title, level, parent, null)
	{
	}

	public MenuItem(string id, string title, int level, string parent, string badge)
	{
		Id = id;
		Title = title;
		Level = level;
		Parent = parent;
		Badge = badge;
	}
}
