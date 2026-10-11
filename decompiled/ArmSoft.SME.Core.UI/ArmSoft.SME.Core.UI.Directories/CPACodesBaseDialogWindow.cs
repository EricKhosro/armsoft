using System.Windows;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;

namespace ArmSoft.SME.Core.UI.Directories;

public class CPACodesBaseDialogWindow<T> : DirectoryDialogWindow<T> where T : CPACodesBase, new()
{
	private new TextEditExt _code;

	private TextEditExt _name;

	private DropDownControl _type;

	private DateEditControl _date;

	public CPACodesBaseDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public CPACodesBaseDialogWindow(T directory, DialogWindowMode mode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		_code = AddStringControl(Definition.Fields["Code"]);
		_name = AddStringControl(Definition.Fields["Name"]);
		_type = AddSmartEnumDropDownControl(Definition.Fields["Type"]);
		_date = AddDateControl(Definition.Fields["MarkInitDate"]);
		if (Mode == DialogWindowMode.EditMode)
		{
			SetFocusOnActivation = _name;
		}
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		if (Mode == DialogWindowMode.NewMode)
		{
			_type.Code = CPACodesTypeSE.CNFEA.Value;
		}
		if (Mode == DialogWindowMode.CreateCopyMode)
		{
			_code.EditValue = "";
		}
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_code.IsReadOnly = Mode == DialogWindowMode.EditMode;
		_type.IsReadOnly = Mode == DialogWindowMode.EditMode;
	}
}
