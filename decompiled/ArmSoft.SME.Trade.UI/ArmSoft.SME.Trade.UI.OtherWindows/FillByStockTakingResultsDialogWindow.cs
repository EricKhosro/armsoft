using System;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Trade.Common;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

public class FillByStockTakingResultsDialogWindow : DialogWindow
{
	private readonly RadioButtonExt _dialogSurplus;

	public bool CreateForSurplus { get; private set; }

	public FillByStockTakingResultsDialogWindow(Window owner = null)
		: base(owner)
	{
		Caption = Settings.ProgramName;
		Name = "FillByStockTakingResults";
		AddTextBlock(ArmSoft.SME.Trade.Common.Resources.FillByStockTakingResults, TextBlockImage.Question);
		LayoutGroupExt container = AddVerticalLayoutGroup();
		_dialogSurplus = AddRadioButton(container, "Surplus", ArmSoft.SME.Common.Resources.Surplus, "CreateForSurplus");
		_dialogSurplus.IsChecked = true;
		AddRadioButton(container, "Deficit", ArmSoft.SME.Common.Resources.Deficit, "CreateForSurplus");
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		base.DialogValidation(args);
		CreateForSurplus = Convert.ToBoolean(_dialogSurplus.IsChecked);
	}
}
