using System.Windows.Media;
using ArmSoft.SME.Core.UI.StandartWindow;
using DevExpress.Mvvm;

namespace ArmSoft.SME.Core.UI.BackgroundWorks;

public class ToastNotificationViewModel : ViewModelBase
{
	public string Caption { get; set; }

	public string Text { get; set; }

	public ImageSource Image { get; set; }

	public HistoryItem HistoryItem { get; set; }

	public ToastNotificationViewModel()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		Caption = "Caption";
		Text = "Text";
		Image = ImageService.GetImage("software_update_available_32.svg");
	}

	public ToastNotificationViewModel(string caption, string text, string image)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		Text = text;
		Caption = caption;
		Image = ImageService.GetImage(image);
		HistoryItem = null;
	}
}
