using System.Windows;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

public class ScannerMessageBoxWindow : DialogWindow
{
	public override string OkButtonText => ArmSoft.SME.Common.Resources.Yes;

	public override string CancelButtonText => ArmSoft.SME.Common.Resources.No;

	public override ImageSource OkButtonImageSource
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			return ImageService.GetImage("accept_16.svg");
		}
	}

	public override ImageSource CancelButtonImageSource
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			return ImageService.GetImage("cancel_16.svg");
		}
	}

	public string ScannerData { get; set; }

	public ScannerMessageBoxWindow(string message, Window owner)
	{
		Owner = owner;
		Caption = Settings.ProgramName;
		AddTextBlock(message, TextBlockImage.Warning);
		SetFocusOnActivation = _cancelButton;
	}

	public override void OnScanner(string scannerData)
	{
		ScannerData = MarkingHelper.CleanScannerSymbols(scannerData);
		Close();
	}
}
