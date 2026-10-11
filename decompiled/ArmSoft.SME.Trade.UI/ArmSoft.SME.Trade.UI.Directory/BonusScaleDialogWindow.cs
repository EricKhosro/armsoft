using System.Windows;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(12)]
public class BonusScaleDialogWindow : DirectoryDialogWindow<BonusScale>
{
	private PopupCalcEditExt _beginSum;

	private PopupCalcEditExt _endSum;

	private PopupCalcEditExt _pointOrPercent;

	public BonusScaleDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public BonusScaleDialogWindow(BonusScale directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		string text = Settings.ParametersService.BonusType();
		_beginSum = AddNumericControl(Definition.Fields["BeginSumma"]);
		_endSum = AddNumericControl(Definition.Fields["EndSumma"]);
		if (text == BonusTypeSE.BySum.Value)
		{
			_pointOrPercent = AddNumericControl(Definition.Fields["Point"]);
		}
		else
		{
			_pointOrPercent = AddNumericControl(Definition.Fields["Percent"]);
		}
		base.ConstructUI();
	}
}
