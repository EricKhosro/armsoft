using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Common.Models.Directories;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(27)]
public class QntUnitDialogWindow : DirectoryDialogWindow<QntUnits>
{
	private TextEditExt _name;

	private TextEditExt _brief;

	public QntUnitDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public QntUnitDialogWindow(QntUnits directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		_code = AddButtonEditControl(Definition.Fields["Code"], 1);
		_code.AllowMagicButton = true;
		_code.MagicToolTip = ArmSoft.SME.Common.Resources.CodeNextNumbers;
		_code.MagicClick += new SafeEventHandler<MagicClickEventArgs>(Code_MagicClick).Handler;
		_name = AddStringControl(Definition.Fields["Name"]);
		_brief = AddStringControl(Definition.Fields["Brief"]);
		if (Mode == DialogWindowMode.EditMode)
		{
			SetFocusOnActivation = _name;
		}
		base.ConstructUI();
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_code.IsReadOnly = Mode == DialogWindowMode.EditMode;
	}
}
