using System.Windows;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Common.Models.Directories;

namespace ArmSoft.SME.Trade.UI.Directories;

[DirectoryWindow(234)]
public class CPACodesWindow : CPACodesBaseDialogWindow<CPACodes>
{
	public CPACodesWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public CPACodesWindow(CPACodes directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}
}
