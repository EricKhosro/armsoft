using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(6)]
public class DiscountGroupDialogWindow : DirectoryDialogWindow<DiscountGroup>
{
	private TextEditExt _name;

	private DropDownControl _type;

	private SpinEditExt _priority;

	public DiscountGroupDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public DiscountGroupDialogWindow(DiscountGroup directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
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
		_type = AddSmartEnumDropDownControl(Definition.Fields["Type"]);
		_priority = AddSpinEditControl(Definition.Fields["Priority"]);
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

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		if (Mode == DialogWindowMode.NewMode)
		{
			_type.Code = DiscountsMergeMethodsSE.Summation.Value;
			_priority.EditValue = (byte)0;
		}
		SpinEditExt priority = _priority;
		if (priority.EditValue == null)
		{
			object obj = (priority.EditValue = (byte)0);
		}
	}
}
