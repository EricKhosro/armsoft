using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.MetaData.DocumentExtension;

public class DropDownItemDialogWindow : DialogWindow
{
	private const short _columnLength = 100;

	private readonly TextEditExt _nameControl;

	private readonly DropDown _dropDown;

	public DropDownItemDialogWindow(DropDown dropDown, Window owner)
		: base(owner)
	{
		Caption = ArmSoft.SME.Common.Resources.DropDown;
		_dropDown = dropDown;
		if (!_dropDown.IsFileDialog)
		{
			AddDropDownControl("Group", ArmSoft.SME.Common.Resources.Group, new DropDownType(onlyUsedInExtensions: true), isRequired: true).EditValueChanged += GroupControl_EditValueChanged;
		}
		else
		{
			AddStringControl("Group", ArmSoft.SME.Common.Resources.Group, 100);
		}
		base["Group"] = dropDown.Group ?? "";
		_nameControl = AddStringControl("Name", ArmSoft.SME.Common.Resources.Code, 100, isRequired: true);
		_nameControl.EditValue = dropDown.Name;
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		base.DialogValidation(args);
		if (args.isValid)
		{
			_dropDown.Name = _nameControl.EditText.Trim();
			_dropDown.Group = base["Group"]?.ToString()?.Trim();
		}
	}

	private void GroupControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null)
		{
			string text = e.NewValue.ToString();
			if (!text.Equals(e.OldValue?.ToString()) && _nameControl != null)
			{
				_nameControl.EditValue = text;
			}
		}
	}
}
