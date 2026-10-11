using System;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.Controls;

namespace ArmSoft.SME.Core.UI.MetaData.DocumentExtension;

public abstract class DataItemDialogWindow : DialogWindow
{
	private const short _codeLength = 100;

	private const short _expressionLength = 500;

	private const short _sourceLength = 200;

	protected TextEditExt _nameControl;

	protected TextEditExt _expressionControl;

	protected CheckEditExt _saveValueOnCopyControl;

	protected TextEditExt _sourceControl;

	protected DataItem DataItem { get; set; }

	protected DataItemDialogWindow(DataItem dataItem, Window owner = null)
		: base(owner)
	{
		DataItem = dataItem ?? throw new ArgumentNullException("dataItem");
		Caption = string.Format(ArmSoft.SME.Common.Resources.EditFieldTypeAttributes, dataItem.Type);
		Mode = DialogWindowMode.EditMode;
		AddControls();
	}

	protected void AddControls()
	{
		_nameControl = AddStringControl("Name", ArmSoft.SME.Common.Resources.Code, 100, isRequired: true);
		_nameControl.Text = DataItem.Name ?? string.Empty;
		_nameControl.IsReadOnly = true;
		AddSpecificControls();
		_saveValueOnCopyControl = AddBooleanControl("SaveValueOnCopy", ArmSoft.SME.Common.Resources.SaveValueOnCopy);
		_saveValueOnCopyControl.IsChecked = DataItem.SaveValueOnCopy;
		_expressionControl = AddStringControl("Expression", ArmSoft.SME.Common.Resources.StandartComment, 500);
		_expressionControl.Text = DataItem.Expression ?? string.Empty;
		_sourceControl = AddStringControl("Source", ArmSoft.SME.Common.Resources.Source, 200);
		_sourceControl.Text = DataItem.Source ?? string.Empty;
	}

	protected abstract void AddSpecificControls();

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		base.DialogValidation(args);
		if (args.isValid)
		{
			DataItem.Name = _nameControl.Text?.Trim() ?? string.Empty;
			DataItem.Expression = (string.IsNullOrWhiteSpace(_expressionControl.Text) ? null : _expressionControl.Text.Trim());
			DataItem.SaveValueOnCopy = _saveValueOnCopyControl.IsChecked == true;
			DataItem.Source = (string.IsNullOrWhiteSpace(_sourceControl.Text) ? null : _sourceControl.Text.Trim());
			UpdateSpecificProperties();
			if (string.IsNullOrWhiteSpace(DataItem.Name))
			{
				args.isValid = false;
				args.ErrorContent = ArmSoft.SME.Common.Resources.IsRequired;
				args.SetFocusWhenInvalidSave = _nameControl;
			}
		}
	}

	protected abstract void UpdateSpecificProperties();
}
