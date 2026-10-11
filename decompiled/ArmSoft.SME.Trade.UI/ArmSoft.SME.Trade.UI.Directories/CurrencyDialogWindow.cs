using System.Windows;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Common.Models.Directories;

namespace ArmSoft.SME.Trade.UI.Directories;

[DirectoryWindow(232)]
public class CurrencyDialogWindow : CurrencyBaseDialogWindow<Currency>
{
	public CurrencyDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public CurrencyDialogWindow(Currency directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}
}
