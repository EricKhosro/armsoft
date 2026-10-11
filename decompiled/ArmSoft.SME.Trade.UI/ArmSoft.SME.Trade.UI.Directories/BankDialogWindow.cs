using System.Windows;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Common.Models.Directories;

namespace ArmSoft.SME.Trade.UI.Directories;

[DirectoryWindow(231)]
public class BankDialogWindow : BankBaseDialogWindow<Bank>
{
	public BankDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public BankDialogWindow(Bank directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}
}
