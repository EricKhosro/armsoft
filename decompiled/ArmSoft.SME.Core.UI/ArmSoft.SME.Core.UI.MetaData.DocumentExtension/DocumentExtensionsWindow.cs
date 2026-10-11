using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.MetaData.DocumentExtension;

public class DocumentExtensionsWindow : DialogWindow
{
	private readonly POTypeSE _poTypeDef;

	private readonly DropDownControl _poType;

	public readonly TextEditExt Description;

	private readonly DocumentGridControl _definitionGrid;

	private readonly ExtensionEdit _extensionEdit;

	private readonly ObservableCollection<DocumentValidatorExpressionRow> _fields = new ObservableCollection<DocumentValidatorExpressionRow>();

	public List<DocumentValidatorExpressionRow> DocumentValidators => _fields.Where((DocumentValidatorExpressionRow p) => p.IsRequired && !p.IsSystemRequired).ToList();

	public DocumentExtensionDefinition DocumentExtensionDefinition { get; private set; }

	public DocumentExtensionsWindow(DialogWindowMode mode, POTypeSE poTypeDef, DocumentExtensionModel documentExtension = null)
	{
		_poTypeDef = poTypeDef;
		Mode = mode;
		Caption = ((_poTypeDef == POTypeSE.Directory) ? ArmSoft.SME.Common.Resources.DirectoryExtension : ArmSoft.SME.Common.Resources.DocumentExtension);
		SizeToContent = SizeToContent.Manual;
		Width = 1200.0;
		Height = 900.0;
		ResizeMode = ResizeMode.CanResize;
		_poType = AddDropDownControl("POType", ArmSoft.SME.Common.Resources.TypeWord, (_poTypeDef == POTypeSE.Directory) ? new SmartEnum(typeof(DirectoryTypeSE), null, padLeft: false, '0', allSubTypes: true) : new DocumentType(), isRequired: true);
		_poType.EditValueChanged += PoType_EditValueChanged;
		if (Mode == DialogWindowMode.EditMode && documentExtension != null)
		{
			_poType.Code = documentExtension.POType.ToString();
			if (documentExtension.POTypeDef == (short)POTypeSE.Document)
			{
				_poType.Code = _poType.Code.PadLeft(2, '0');
			}
			_poType.IsReadOnly = true;
		}
		Description = AddStringControl("Description", ArmSoft.SME.Common.Resources.Description, 50);
		Description.Text = documentExtension?.Description;
		DXTabControl tc = AddTabControl(null);
		LayoutGroup container = AddTabedGroup(tc, ArmSoft.SME.Common.Resources.ExtensionFields);
		_extensionEdit = new ExtensionEdit(allowAddPages: true, this);
		AddControl(container, "_extensionEdit", null, _extensionEdit);
		if (Controls.TryGetValue(_extensionEdit.Name, out var value))
		{
			value.LayoutItem.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
			value.LayoutItem.VerticalAlignment = VerticalAlignment.Stretch;
		}
		if (Mode == DialogWindowMode.EditMode && documentExtension != null)
		{
			DocumentExtensionDefinition documentExtensionDefinition = new DocumentExtensionDefinition(documentExtension.ExtensionXml, new Environment());
			_extensionEdit.LoadExtensionGrids(documentExtensionDefinition.Tabs, documentExtensionDefinition.Elements);
			_extensionEdit.UpdateGridSources(documentExtension.POTypeDef, documentExtension.POType, updateExtendedItemsGrids: false);
		}
		LayoutGroup container2 = AddTabedGroup(tc, ArmSoft.SME.Common.Resources.SystemFields);
		_definitionGrid = AddGridControl(container2, "SystemFields");
		_definitionGrid.MinHeight = 150.0;
		_definitionGrid.AllowAdd = false;
		_definitionGrid.AllowDelete = false;
		_definitionGrid.AllowDuplicate = false;
		_definitionGrid.ShowButtons = false;
		_definitionGrid.ShowColHeaderIn2Lines = true;
		_definitionGrid.AddStringColumn("FieldCaption", ArmSoft.SME.Common.Resources.Name, 50).AllowEditing = DefaultBoolean.False;
		_definitionGrid.AddBooleanColumn("IsRequired", ArmSoft.SME.Common.Resources.IsRequired);
		_definitionGrid.AddBooleanColumn("IsSystemRequired", ArmSoft.SME.Common.Resources.System).AllowEditing = DefaultBoolean.False;
		_definitionGrid.AddDropDownColumn("UserIds", ArmSoft.SME.Common.Resources.Users, new UsersFiltersByIds(), allowAdditionButtons: true, isOwnProcessingClickDropDown: true).AllowEditing = DefaultBoolean.False;
		_definitionGrid.AddBooleanColumn("Except", ArmSoft.SME.Common.Resources.ExceptSelectedUsers).Width = 180.0;
		_definitionGrid.ClickDropDown += DefinitionGrid_ClickDropDown;
		_definitionGrid.View.ShowingEditor += View_ShowingEditor;
		_definitionGrid.View.CellValueChanged += View_CellValueChanged;
		if (Mode != DialogWindowMode.NewMode && documentExtension?.DocumentValidators != null)
		{
			LoadFields(documentExtension.DocumentValidators);
		}
	}

	private void View_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		DocumentValidatorExpressionRow documentValidatorExpressionRow = (DocumentValidatorExpressionRow)e.Row;
		if (e.Column.Name == "IsRequired" && !(bool)e.Value)
		{
			documentValidatorExpressionRow.UserIds = "";
			documentValidatorExpressionRow.Except = false;
			_definitionGrid.Grid.RefreshRow(e.RowHandle);
		}
	}

	private void View_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		object row = e.Row;
		DocumentValidatorExpressionRow row2 = row as DocumentValidatorExpressionRow;
		if (row2 != null)
		{
			e.Cancel = _fields.Any((DocumentValidatorExpressionRow p) => p.FieldName == row2.FieldName && p.IsRequired && p.IsSystemRequired);
			if (!e.Cancel && e.Column.FieldName == "Except")
			{
				e.Cancel = !row2.IsRequired || row2.IsSystemRequired;
			}
		}
	}

	private void DefinitionGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		DocumentValidatorExpressionRow documentValidatorExpressionRow = (DocumentValidatorExpressionRow)e.Grid.Grid.GetRow(e.RowHandle);
		if (!documentValidatorExpressionRow.IsRequired || documentValidatorExpressionRow.IsSystemRequired || !(e.Column.Name == "UserIds"))
		{
			return;
		}
		UsersFiltersByIds usersFiltersByIds = new UsersFiltersByIds();
		ModalBrowser mb = new ModalBrowser(usersFiltersByIds)
		{
			MultiSelect = true,
			SelectedRows = GetSelectedList(documentValidatorExpressionRow.UserIds, out var _, usersFiltersByIds),
			Left = e.Left,
			Top = e.Top,
			Owner = e.Owner
		};
		if (mb.Show())
		{
			e.NewValue = string.Join(",", from p in mb.SelectedRows
				select mb.DataView.ValueMember(p) into p
				orderby p
				select p);
		}
	}

	private static List<int> GetSelectedList(string stringRepresentation, out bool errorExists, IDropDownView idvClickDropDown = null)
	{
		List<int> list = new List<int>();
		errorExists = false;
		List<string> list2 = idvClickDropDown.ValuesList();
		for (int i = 0; i <= list2.Count - 1; i++)
		{
			list2[i] = list2[i].Trim();
		}
		string[] array = stringRepresentation?.Split(",") ?? Array.Empty<string>();
		for (int j = 0; j <= array.Length - 1; j++)
		{
			string text = array[j].Trim();
			if (string.IsNullOrWhiteSpace(text))
			{
				continue;
			}
			bool flag = false;
			for (int k = 0; k <= list2.Count - 1; k++)
			{
				if (text == list2[k])
				{
					list.Add(k);
					flag = true;
					break;
				}
			}
			errorExists = errorExists || !flag;
		}
		return list;
	}

	private void PoType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (string.IsNullOrWhiteSpace(_poType.Code))
		{
			return;
		}
		LoadFields();
		if (_extensionEdit != null)
		{
			if (Mode == DialogWindowMode.NewMode)
			{
				DocumentExtensionDefinition documentExtensionDefinition = Settings.ServiceProvider.GetRequiredService<IDocumentExtensionDefinitionService>().Load(_poTypeDef, Convert.ToInt16(_poType.Code));
				if (documentExtensionDefinition != null)
				{
					_extensionEdit.LoadExtensionGrids(documentExtensionDefinition.Tabs, documentExtensionDefinition.Elements);
				}
				_extensionEdit.IsReadOnly = false;
			}
			else
			{
				_extensionEdit.IsReadOnly = true;
			}
		}
		if (_extensionEdit != null && !string.IsNullOrWhiteSpace(_poType.Code))
		{
			_extensionEdit.UpdateGridSources(_poTypeDef, short.Parse(_poType.Code));
		}
	}

	private void LoadFields(List<DocumentValidatorExpressionRow> rows = null)
	{
		if (_definitionGrid == null || string.IsNullOrWhiteSpace(_poType.Code))
		{
			return;
		}
		short num = Convert.ToInt16(_poType.Code);
		_fields.Clear();
		if (_poTypeDef == POTypeSE.Directory)
		{
			if (DefinitionMappingsService.TryResolveDirectoryDefinition(num, out var definition))
			{
				LoadTypeFields(definition.Fields, rows);
			}
		}
		else if (_poTypeDef == POTypeSE.Document)
		{
			if (DefinitionMappingsService.TryResolveDocumentDefinition(num, out var definition2))
			{
				LoadTypeFields(definition2.Fields, rows);
			}
		}
		else
		{
			_fields.Clear();
			_definitionGrid.ItemsSource = _fields;
		}
		_definitionGrid.ItemsSource = _fields;
	}

	private void LoadTypeFields(Dictionary<string, FieldDefinition> fields, List<DocumentValidatorExpressionRow> rows = null)
	{
		foreach (KeyValuePair<string, FieldDefinition> field in fields)
		{
			if (!(field.Value is BoolFieldDefinition))
			{
				DocumentValidatorExpressionRow documentValidatorExpressionRow = rows?.FirstOrDefault((DocumentValidatorExpressionRow p) => p.FieldName == field.Key);
				DocumentValidatorExpressionRow documentValidatorExpressionRow2 = new DocumentValidatorExpressionRow
				{
					FieldName = field.Key,
					FieldCaption = field.Value.Caption,
					IsRequired = field.Value.IsRequired,
					IsSystemRequired = field.Value.IsRequired
				};
				if (documentValidatorExpressionRow != null)
				{
					documentValidatorExpressionRow2.IsRequired = documentValidatorExpressionRow.IsRequired;
					documentValidatorExpressionRow2.UserIds = documentValidatorExpressionRow.UserIds;
					documentValidatorExpressionRow2.Except = documentValidatorExpressionRow.Except;
				}
				_fields.Add(documentValidatorExpressionRow2);
			}
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
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
		}
	}
}
