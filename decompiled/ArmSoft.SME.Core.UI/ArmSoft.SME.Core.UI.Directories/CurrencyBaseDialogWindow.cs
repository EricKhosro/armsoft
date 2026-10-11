using System.Windows;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI.Controls;

namespace ArmSoft.SME.Core.UI.Directories;

[DirectoryWindow(232)]
public class CurrencyBaseDialogWindow<T> : DirectoryDialogWindow<T> where T : CurrencyBase, new()
{
	private TextEditExt _curr;

	private TextEditExt _name;

	private TextEditExt _unitName;

	private TextEditExt _centName;

	public CurrencyBaseDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public CurrencyBaseDialogWindow(T directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		_curr = AddStringControl(Definition.Fields["Code"]);
		_name = AddStringControl(Definition.Fields["Name"]);
		_unitName = AddStringControl(Definition.Fields["UnitName"]);
		_centName = AddStringControl(Definition.Fields["CentName"]);
		if (Mode == DialogWindowMode.EditMode)
		{
			SetFocusOnActivation = _name;
		}
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_curr.IsReadOnly = Mode == DialogWindowMode.EditMode;
	}
}
