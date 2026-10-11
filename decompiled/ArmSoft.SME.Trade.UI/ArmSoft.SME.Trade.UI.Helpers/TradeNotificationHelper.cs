using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.StandartWindow;

namespace ArmSoft.SME.Trade.UI.Helpers;

public class TradeNotificationHelper : BaseNotificationHelper
{
	public static void ShowTradeNotification(string caption, string imageUri, string text, int historyItemType, string historyItemDescription, string historyItemDetails, string historyItemIconUri, string historyItemId = "")
	{
		BaseNotificationHelper.ShowBaseNotification(caption, imageUri, text, HistoryItem.ItemCategory.Notification, historyItemType, historyItemDescription, historyItemDetails, historyItemIconUri, historyItemId, 0);
	}

	public static void ShowTradeDefaultNotification(string caption, string text, string historyItemId = "")
	{
		BaseNotificationHelper.ShowBaseNotification(caption, "accept_32.svg", text, HistoryItem.ItemCategory.Notification, 101, caption, text, "accept_16.svg", historyItemId, 0);
	}
}
