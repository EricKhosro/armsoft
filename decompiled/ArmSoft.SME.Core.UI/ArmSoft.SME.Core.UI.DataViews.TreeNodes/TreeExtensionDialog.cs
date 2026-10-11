using System;
using System.Collections.Generic;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Core.UI.MetaData.Tree;

namespace ArmSoft.SME.Core.UI.DataViews.TreeNodes;

public class TreeExtensionDialog : DialogWindow
{
	private readonly TextEditExt _treeNameControl;

	private readonly ExtensionEdit _extensionEdit;

	internal TreeExtensionDefinitionEx TreeExtension { get; private set; }

	public TreeExtensionDialog(string treeName, DialogWindowMode mode, TreeExtensionDefinitionEx treeExtension = null, Window owner = null)
		: base(owner)
	{
		Mode = mode;
		Caption = ArmSoft.SME.Common.Resources.Extension;
		SizeToContent = SizeToContent.WidthAndHeight;
		_treeNameControl = AddStringControl("_treeNameControl", ArmSoft.SME.Common.Resources.TreeDef, 8);
		_treeNameControl.EditValue = treeName;
		_treeNameControl.IsReadOnly = true;
		_extensionEdit = new ExtensionEdit(allowAddPages: true, this);
		AddControl("_extensionEdit", null, _extensionEdit);
		if (mode == DialogWindowMode.EditMode && treeExtension != null)
		{
			_extensionEdit.LoadExtensionGrids(null, treeExtension.Elements);
		}
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		base.DialogValidation(args);
		if (!_extensionEdit.Validate(out var errorMessage, out var errorControl))
		{
			args.isValid = false;
			args.ErrorContent = errorMessage;
			args.SetFocusWhenInvalidSave = errorControl;
			return;
		}
		try
		{
			(Dictionary<string, DataItem> DataItems, List<UITab> Tabs, List<ArmSoft.SME.Core.UI.MetaData.UIElement> Elements) tuple = _extensionEdit.CreateDocumentExtension();
			Dictionary<string, DataItem> item = tuple.DataItems;
			List<ArmSoft.SME.Core.UI.MetaData.UIElement> item2 = tuple.Elements;
			TreeExtension = new TreeExtensionDefinitionEx(string.Empty, new ArmSoft.SME.Core.UI.MetaData.Environment())
			{
				Name = _treeNameControl.EditText?.Trim(),
				DataItems = item,
				Elements = item2
			};
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
		}
	}
}
