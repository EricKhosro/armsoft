using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Directories;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Core.UI.Controls;

namespace ArmSoft.SME.Core.UI.DataViews;

public class DirectoryEditCodeDialog : DialogWindow
{
	private DirectoryDefinition _directoryDefinition;

	public readonly string OldCode;

	public readonly ButtonEditExt NewCode;

	private bool _codeIsManualChanged;

	private bool _codeMagicClickEvenTimes;

	public DirectoryEditCodeDialog(DirectoryDefinition directoryDefinition, string code, string name)
	{
		_directoryDefinition = directoryDefinition;
		Caption = ArmSoft.SME.Common.Resources.EditCodeOfDirectories;
		TextEditExt textEditExt = AddStringControl("Name", ArmSoft.SME.Common.Resources.Name, 255, isRequired: true);
		textEditExt.IsReadOnly = true;
		textEditExt.Text = name;
		TextEditExt textEditExt2 = AddStringControl("OldCode", ArmSoft.SME.Common.Resources.OldCode, ((StringFieldDefinition)directoryDefinition.Fields["Code"]).Length, isRequired: true);
		textEditExt2.IsReadOnly = true;
		textEditExt2.Text = code;
		OldCode = code;
		NewCode = AddButtonEditControl("NewCode", ArmSoft.SME.Common.Resources.NewCode, ((StringFieldDefinition)directoryDefinition.Fields["Code"]).Length, isRequired: true, 1);
		SetFocusOnActivation = NewCode;
		if (directoryDefinition.NextCodeParamId.HasValue)
		{
			NewCode.AllowMagicButton = true;
			NewCode.MagicToolTip = ArmSoft.SME.Common.Resources.CodeNextNumbers;
			NewCode.MagicClick += new SafeEventHandler<MagicClickEventArgs>(Code_MagicClick).Handler;
		}
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(DialogValidation).Handler;
	}

	protected void Code_MagicClick(object sender, MagicClickEventArgs e)
	{
		SetNextCode();
		if (NewCode.IsEditorActive)
		{
			NewCode.ClearError();
		}
		if (!NewCode.DoValidate())
		{
			NewCode.EditValue = "";
		}
		NewCode.SelectAll();
		_codeMagicClickEvenTimes = !_codeMagicClickEvenTimes;
	}

	protected void SetNextCode()
	{
		if (_directoryDefinition.NextCodeParamId.HasValue)
		{
			_codeIsManualChanged = NewCode.EditText.ToString().Trim() != NewCode.Text.Trim();
			string text = (_codeIsManualChanged ? NewCode.EditText.ToString().Trim() : "");
			NextCodeOfferSource offerSource = ((!_codeMagicClickEvenTimes && string.IsNullOrWhiteSpace(text)) ? NextCodeOfferSource.FromParam : NextCodeOfferSource.MaxCode);
			NewCode.EditValue = Settings.DirectoryService.GetNextCode(_directoryDefinition.POType, offerSource, text, NewCode.MaxLength);
		}
	}

	private void DialogValidation(object sender, DialogWindowValidationArgs args)
	{
		if (NewCode.Text.Trim() == OldCode.Trim())
		{
			args.isValid = false;
			throw new ASException(ArmSoft.SME.Common.Resources.NewCodeEqualToOldCode);
		}
	}
}
