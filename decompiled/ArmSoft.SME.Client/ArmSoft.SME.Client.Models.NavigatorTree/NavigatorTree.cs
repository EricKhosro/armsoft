using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Client.Models.NavigatorTree;

public class NavigatorTree
{
	private const string schema = "https://armsoftupdates.blob.core.windows.net/schemas/NavigatorTreeSchema.json";

	[JsonPropertyName("$schema")]
	public string Schema => "https://armsoftupdates.blob.core.windows.net/schemas/NavigatorTreeSchema.json";

	public string Name { get; set; }

	public string Caption { get; set; }

	public string Icon { get; set; }

	public bool System { get; set; }

	public List<NavigatorTreeNode> Nodes { get; set; }

	public NavigatorTreeType8X Type { get; set; }
}
