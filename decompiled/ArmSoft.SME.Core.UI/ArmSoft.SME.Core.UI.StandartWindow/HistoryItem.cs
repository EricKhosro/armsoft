using System;

namespace ArmSoft.SME.Core.UI.StandartWindow;

public class HistoryItem
{
	public enum ItemCategory
	{
		Document = 1,
		Notification
	}

	public enum RemoveMode
	{
		None,
		RemoveOld,
		RemoveLast
	}

	private readonly ItemCategory _category;

	public string Id { get; set; }

	public int Type { get; set; }

	public string Details { get; set; }

	public string Description { get; set; }

	public string Icon { get; set; }

	public string ActionName { get; set; }

	public RemoveMode Mode { get; set; }

	public DateTime CreationDate { get; set; }

	public bool IsNewItem { get; set; }

	public short NavigationCommandId { get; set; }

	public ItemCategory Category => _category;

	public HistoryItem(ItemCategory itemCategory)
	{
		_category = itemCategory;
		Id = "";
		Description = "";
		Details = "";
		Type = 0;
		Icon = "";
		Mode = RemoveMode.RemoveOld;
		CreationDate = DateTime.Now;
		NavigationCommandId = 0;
	}
}
