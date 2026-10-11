using System;
using System.Collections.Generic;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.MetaData.DocumentExtension;

public class DocumentExtensionsWindowSimple : DialogWindow
{
	private readonly POTypeSE _poTypeDef;

	private readonly DropDownControl _poType;

	private readonly ExtensionEditSimple _extensionEdit;

	public DocumentExtensionDefinition DocumentExtensionDefinition { get; private set; }

	public DocumentExtensionsWindowSimple(DialogWindowMode mode, POTypeSE poTypeDef, short poType, DocumentExtensionModel documentExtension = null, Window owner = null)
	{
		_poTypeDef = poTypeDef;
		Mode = mode;
		Caption = ArmSoft.SME.Common.Resources.CreateUserDefinedFields;
		SizeToContent = SizeToContent.Manual;
		Width = 1100.0;
		Height = 530.0;
		if (owner != null)
		{
			Owner = owner;
		}
		ResizeMode = ResizeMode.CanResize;
		_poType = AddDropDownControl("POType", ArmSoft.SME.Common.Resources.TypeWord, (_poTypeDef == POTypeSE.Directory) ? new SmartEnum(typeof(DirectoryTypeSE), null, padLeft: false, '0', allSubTypes: true) : new DocumentType(), isRequired: true);
		_poType.Code = poType.ToString();
		if (poTypeDef == POTypeSE.Document)
		{
			_poType.Code = _poType.Code.PadLeft(2, '0');
		}
		_poType.IsReadOnly = true;
		_extensionEdit = new ExtensionEditSimple(this);
		if (Controls.TryGetValue(_extensionEdit.Name, out var value))
		{
			value.LayoutItem.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
			value.LayoutItem.VerticalAlignment = VerticalAlignment.Stretch;
		}
		if (Mode == DialogWindowMode.EditMode && documentExtension != null)
		{
			DocumentExtensionDefinition documentExtensionDefinition = new DocumentExtensionDefinition(documentExtension.ExtensionXml, new Environment());
			_extensionEdit.LoadExtensionGrids(documentExtensionDefinition.Tabs, documentExtensionDefinition.Elements);
		}
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		base.DialogValidation(args);
		try
		{
			if (Mode == DialogWindowMode.NewMode && Settings.ServiceProvider.GetRequiredService<IDocumentExtensionService>().LoadExtensionDescription(new DocumentExtensionRequest(_poTypeDef, Convert.ToInt16(_poType.Code))) != null)
			{
				throw new ASException((_poTypeDef == POTypeSE.Directory) ? ArmSoft.SME.Common.Resources.DirectoryExtensionExist : ArmSoft.SME.Common.Resources.DocumentExtensionExist);
			}
			if (!_extensionEdit.Validate(out var errorMessage, out var errorControl))
			{
				args.isValid = false;
				args.ErrorContent = errorMessage;
				args.SetFocusWhenInvalidSave = errorControl;
				return;
			}
			(Dictionary<string, DataItem> DataItems, List<UITab> Tabs, List<UIElement> Elements) tuple = _extensionEdit.CreateDocumentExtension();
			Dictionary<string, DataItem> item = tuple.DataItems;
			List<UITab> item2 = tuple.Tabs;
			List<UIElement> item3 = tuple.Elements;
			DocumentExtensionDefinition = new DocumentExtensionDefinition(string.Empty, new Environment())
			{
				POTypeDef = _poTypeDef,
				POType = Convert.ToInt16(_poType.Code),
				DataItems = item,
				Tabs = item2,
				Elements = item3
			};
			MessageBox.Show(ArmSoft.SME.Common.Resources.UDFieldsChangeInfo);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
		}
	}
}
