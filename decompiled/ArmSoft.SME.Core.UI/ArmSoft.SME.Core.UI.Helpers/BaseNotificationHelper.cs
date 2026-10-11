using ArmSoft.SME.Core.UI.BackgroundWorks;
using ArmSoft.SME.Core.UI.StandartWindow;

namespace ArmSoft.SME.Core.UI.Helpers;

public class BaseNotificationHelper
{
	public static void ShowBaseNotification(string caption, string imageUri, string text, HistoryItem.ItemCategory historyItemCategory, int historyItemType, string historyItemDescription, string historyItemDetails, string historyItemIconUri, string historyItemId = "", short commandId = 0)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		ToastNotificationViewModel viewModel = new ToastNotificationViewModel
		{
			Caption = caption,
			Image = ImageService.GetImage(imageUri),
			Text = text,
			HistoryItem = new HistoryItem(historyItemCategory)
			{
				Type = historyItemType,
				Description = historyItemDescription,
				Details = historyItemDetails,
				Icon = historyItemIconUri,
				Id = historyItemId,
				NavigationCommandId = commandId
			}
		};
		Settings.HostingEnvironment.ShowNotificationAsync(viewModel);
	}
}
