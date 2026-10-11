using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Common.Models.Directories;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(25)]
public class PropertyDialogWindow : DirectoryDialogWindow<Property>
{
	private TextEditExt _name;

	public PropertyDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public PropertyDialogWindow(Property directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
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
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_code.IsReadOnly = Mode == DialogWindowMode.EditMode;
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
			Directory.NextProperty = StringExtensions.NextDocNum(new string('0', 4), 4);
		}
	}
}
