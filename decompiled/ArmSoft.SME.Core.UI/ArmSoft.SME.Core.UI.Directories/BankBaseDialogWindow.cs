using System.Windows;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI.Controls;

namespace ArmSoft.SME.Core.UI.Directories;

public class BankBaseDialogWindow<T> : DirectoryDialogWindow<T> where T : BankBase, new()
{
	private new TextEditExt _code;

	private TextEditExt _name;

	private TextEditExt _swiftCode;

	public BankBaseDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public BankBaseDialogWindow(T directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		_code = AddStringControl(Definition.Fields["Code"]);
		_name = AddStringControl(Definition.Fields["Name"]);
		_swiftCode = AddStringControl(Definition.Fields["SWIFTCODE"]);
		if (Mode == DialogWindowMode.EditMode)
		{
			SetFocusOnActivation = _name;
		}
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_code.IsReadOnly = Mode == DialogWindowMode.EditMode;
	}
}
