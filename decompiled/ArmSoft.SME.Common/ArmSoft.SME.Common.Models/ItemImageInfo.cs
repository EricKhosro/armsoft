using System.Text.Json.Serialization;

namespace ArmSoft.SME.Common.Models;

public class ItemImageInfo
{
	public string ItemKey { get; }

	public string ImageKey { get; }

	public ItemImageInfo()
	{
	}

	[JsonConstructor]
	public ItemImageInfo(string itemKey, string imageKey)
	{
		ItemKey = itemKey;
		ImageKey = imageKey;
	}
}
