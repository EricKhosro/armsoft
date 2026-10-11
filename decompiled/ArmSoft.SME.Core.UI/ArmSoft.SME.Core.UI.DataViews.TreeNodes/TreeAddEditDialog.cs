using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.MetaData;
using DevExpress.Xpf.Editors;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.DataViews.TreeNodes;

internal class TreeAddEditDialog : DialogWindow
{
	private readonly bool _systemTree;

	private readonly ReadOnlyDictionary<string, short> _captionLengths;

	private readonly TextEditExt _nameEdit;

	private readonly DropDownControl _nodeCaptionId;

	private TreeDefinition _treeDefinition;

	private SpinEditExt _codeLen;

	private SpinEditExt captionLen;

	private SpinEditExt specLen;

	internal string TreeId => _nameEdit.EditText?.TrimEnd();

	internal string TreeCaption => _nodeCaptionId.Code?.TrimEnd();

	public TreeAddEditDialog(DialogWindowMode mode = DialogWindowMode.NewMode, string treeName = null)
	{
		if (!string.IsNullOrWhiteSpace(treeName) && Settings.ServiceProvider.GetRequiredService<ITreeDefinitionService>().Get(treeName) == null)
		{
			throw new RESTException(string.Format(ArmSoft.SME.Common.Resources.DataItemNotExists, treeName));
		}
		_systemTree = _treeDefinition?.System ?? false;
		Mode = mode;
		Caption = ArmSoft.SME.Common.Resources.TreeDef;
		_nameEdit = AddStringControl("Name", ArmSoft.SME.Common.Resources.Code, 8, isRequired: true);
		if (!_systemTree)
		{
			_nameEdit.MaskType = MaskType.RegEx;
			_nameEdit.Mask = "UD_.*";
			_nameEdit.Text = "UD_";
			_nameEdit.MaskPlaceHolder = ' ';
		}
		StringResources idv = new StringResources();
		DropDownControl dropDownControl = AddCustomDropDownControl("CaptionId", ArmSoft.SME.Common.Resources.DirectoryCaptionId, idv, isRequired: true);
		_nodeCaptionId = AddCustomDropDownControl("NodeCaptionId", ArmSoft.SME.Common.Resources.EditingWindowCaptionId, idv, isRequired: true);
		_codeLen = AddSpinEdit("CodeLen", ArmSoft.SME.Common.Resources.CodeLen, 1m, 20m, 60.0);
		_codeLen.EditValue = (short)20;
		captionLen = AddSpinEdit("CaptionLen", ArmSoft.SME.Common.Resources.CaptionLen, 1m, 150m, 60.0);
		captionLen.EditValue = (short)150;
		specLen = AddSpinEdit("SpecLen", ArmSoft.SME.Common.Resources.SpecLen, 0m, 255m, 60.0);
		specLen.EditValue = 0;
		DropDownControl dropDownControl2 = AddCustomDropDownControl("SpecCaptionId", ArmSoft.SME.Common.Resources.SpecFieldCaptionId, idv);
		CheckEditExt checkEditExt = AddBooleanControl("Multilevel", ArmSoft.SME.Common.Resources.Multilevel);
		CheckEditExt checkEditExt2 = AddBooleanControl("IsLogAvailable", ArmSoft.SME.Common.Resources.History);
		_nameEdit.IsReadOnly = _systemTree;
		dropDownControl.IsReadOnly = _systemTree;
		_codeLen.IsReadOnly = _systemTree;
		captionLen.IsReadOnly = _systemTree;
		_nodeCaptionId.IsReadOnly = _systemTree;
		dropDownControl2.IsReadOnly = _systemTree;
		specLen.IsReadOnly = _systemTree;
		checkEditExt.IsReadOnly = _systemTree;
		checkEditExt2.IsReadOnly = _systemTree;
		if (Mode != DialogWindowMode.NewMode && _treeDefinition != null)
		{
			_nameEdit.EditValue = _treeDefinition.Name;
			_nameEdit.IsReadOnly = true;
			dropDownControl.Code = _treeDefinition.CaptionId;
			_codeLen.EditValue = _treeDefinition.CodeLen;
			_nodeCaptionId.Code = _treeDefinition.NodeCaptionId;
			captionLen.EditValue = _treeDefinition.CaptionLen;
			dropDownControl2.Code = _treeDefinition.SpecCaptionId;
			specLen.EditValue = _treeDefinition.SpecLen;
			checkEditExt.EditValue = _treeDefinition.Multilevel;
			checkEditExt2.EditValue = _treeDefinition.IsLogAvailable;
			checkEditExt.IsReadOnly = checkEditExt.IsReadOnly || _treeDefinition.Multilevel;
			checkEditExt2.IsReadOnly = checkEditExt2.IsReadOnly || _treeDefinition.IsLogAvailable;
			Dictionary<string, short> dictionary = new Dictionary<string, short>
			{
				{
					"CodeLen".ToUpper(),
					_treeDefinition.CodeLen
				},
				{
					"CaptionLen".ToUpper(),
					_treeDefinition.CaptionLen
				},
				{
					"SpecLen".ToUpper(),
					_treeDefinition.SpecLen
				}
			};
			_captionLengths = dictionary.AsReadOnly();
			_codeLen.Validate += new SafeEventHandler<ValidationEventArgs>(CodeLen_Validate).Handler;
			captionLen.Validate += new SafeEventHandler<ValidationEventArgs>(CodeLen_Validate).Handler;
			specLen.Validate += new SafeEventHandler<ValidationEventArgs>(CodeLen_Validate).Handler;
		}
	}

	private DropDownControl AddCustomDropDownControl(string name, string caption, IDropDownView idv, bool isRequired = false)
	{
		DropDownControl dropDownControl = AddDropDownControl(name, caption, idv, isRequired, allowAdditionButtons: true);
		dropDownControl.CodeEdit.Validate += new SafeEventHandler<ValidationEventArgs>(CodeEdit_Validate).Handler;
		return dropDownControl;
	}

	private void CodeEdit_Validate(object sender, ValidationEventArgs e)
	{
		e.IsValid = true;
		e.Handled = true;
	}

	private void CodeLen_Validate(object sender, ValidationEventArgs e)
	{
		if (sender is SpinEditExt spinEditExt && _captionLengths.TryGetValue(spinEditExt.Name, out var value) && value > Convert.ToInt16(e.Value))
		{
			e.IsValid = false;
			e.ErrorContent = string.Format(ArmSoft.SME.Common.Resources.NewValueMustBeGreaterThanOld, value);
		}
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		if (!_systemTree)
		{
			base.DialogValidation(args);
			TreeDefinition definition = new TreeDefinition
			{
				Name = _nameEdit.EditText.Trim(),
				CaptionId = (string)base["CaptionId"],
				NodeCaptionId = (string)base["NodeCaptionId"],
				CaptionLen = Convert.ToInt16(base["CaptionLen"]),
				CodeLen = Convert.ToInt16(base["CodeLen"]),
				IsClosedAvailable = true,
				IsLogAvailable = (bool)base["IsLogAvailable"],
				Multilevel = (bool)base["Multilevel"],
				SpecCaptionId = (string)base["SpecCaptionId"],
				SpecLen = Convert.ToInt16(base["SpecLen"])
			};
			Settings.ServiceProvider.GetRequiredService<ITreeDefinitionService>().Store(definition);
		}
	}
}
