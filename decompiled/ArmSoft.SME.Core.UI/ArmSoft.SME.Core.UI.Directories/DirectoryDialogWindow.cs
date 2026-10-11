using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common.Directories;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Core.UI.Directories;

public abstract class DirectoryDialogWindow<T> : DialogWindow, IDirectoryDialogWindow where T : Directory, new()
{
	private readonly HashSet<string> _dataItemNames;

	private DirectoryDefinition _definition;

	protected ButtonEditExt _code;

	public T Directory { get; set; }

	public DirectoryDefinition Definition
	{
		get
		{
			if (_definition == null)
			{
				DefinitionMappingsService.IsDirectoryWindow(((object)this).GetType(), out var dirType);
				DefinitionMappingsService.TryResolveDirectoryDefinition(dirType, out _definition);
			}
			return _definition;
		}
	}

	public int DVPId { get; set; } = -1;

	protected virtual bool ViewModeIsWithNext => false;

	public DirectoryDialogWindow(DialogWindowMode mode, Window owner = null)
		: this((T)null, mode, owner)
	{
	}

	public DirectoryDialogWindow(T directory, DialogWindowMode mode, Window owner = null)
		: base(owner)
	{
		_dataItemNames = new HashSet<string>();
		Caption = Definition.Caption;
		HelpId = Definition.HelpId;
		if (mode == DialogWindowMode.EditMode && !Settings.UserAccessService.GetDirectoryPermission(Definition.POType).AllowEdit)
		{
			Mode = DialogWindowMode.ViewMode;
		}
		else
		{
			if (ViewModeIsWithNext)
			{
				switch (mode)
				{
				case DialogWindowMode.NewMode:
					mode = DialogWindowMode.NewModeWithNext;
					break;
				case DialogWindowMode.CreateCopyMode:
					mode = DialogWindowMode.CreateCopyModeWithNext;
					break;
				}
			}
			Mode = mode;
		}
		if (directory == null)
		{
			Directory = Settings.DirectoryService.Create<T>(Definition.POType);
		}
		else
		{
			Directory = directory;
		}
		NextPressed += new SafeEventHandler<EventArgs>(OnNext).Handler;
	}

	protected virtual Control ControlByName(string name)
	{
		Controls.TryGetValue(name.ToUpper(), out var value);
		return value.Control;
	}

	public new bool? ShowDialog()
	{
		ConstructUI();
		LoadDataToControls();
		if (Mode == DialogWindowMode.NewMode || Mode == DialogWindowMode.NewModeWithNext || Mode == DialogWindowMode.CreateCopyMode)
		{
			SetNextCode();
		}
		SetControlsReadOnly();
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordDirectoryDefinition))
		{
			CallsRecordingHelper.ExportDirectoryWindowDefinition(Directory.POName, Caption, isDialogWindow: true, GetControlsInfoForRecord(withoutValues: true));
		}
		return base.ShowDialog();
	}

	protected virtual void OnNext(object sender, EventArgs e)
	{
		Directory = Settings.DirectoryService.Create<T>(Definition.POType);
		SetNextCode();
		OnNewAfterSave();
	}

	protected virtual void OnNewAfterSave()
	{
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		try
		{
			if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordDirectory))
			{
				CallsRecordingHelper.ExportDirectoryWindow(Directory.POName, GetControlsInfoForRecord());
			}
			FillDocument();
			Validate(args);
			DirStoreRequest sr = new DirStoreRequest();
			Directory = Settings.DirectoryService.StoreAndLoad(Directory, sr);
			AfterStore(sr);
			DataViewPanel dataViewPanel = Settings.HostingEnvironment.FindDataViewPanel(DVPId);
			if (dataViewPanel != null && dataViewPanel.Id != -1)
			{
				UpdateDataViewPanel(dataViewPanel);
			}
		}
		catch (ASException ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
			if (!string.IsNullOrWhiteSpace(ex.FieldName))
			{
				args.SetFocusWhenInvalidSave = ControlByName(ex.FieldName);
			}
		}
		catch (Exception ex2)
		{
			Settings.ErrorHandler(ex2);
			args.isValid = false;
		}
	}

	protected virtual void Validate(DialogWindowValidationArgs eventArg)
	{
	}

	protected virtual void ConstructUI()
	{
	}

	protected virtual void SetControlsReadOnlyInner()
	{
	}

	private void SetControlsReadOnly()
	{
		if (Mode == DialogWindowMode.ViewMode)
		{
			SetControlsReadonly(value: true);
		}
		else
		{
			SetControlsReadOnlyInner();
		}
	}

	protected virtual void FillDocument()
	{
		foreach (ControlDescriptor value in Controls.Values)
		{
			if (Definition.Fields.ContainsKey(value.Name))
			{
				SetPropertyValue(Directory, value.Name, ControlsHelper.GetControlValue(value.Control));
			}
		}
	}

	protected virtual void LoadDataToControls()
	{
		foreach (ControlDescriptor value in Controls.Values)
		{
			if (Definition.Fields.ContainsKey(value.Name))
			{
				ControlsHelper.SetControlValue(value.Control, GetPropertyValue(Directory, value.Name));
			}
		}
	}

	public static object GetPropertyValue(object src, string propName)
	{
		return src.GetType().GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public).GetValue(src, null);
	}

	public static void SetPropertyValue(object src, string propName, object value)
	{
		src.GetType().GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public).SetValue(src, value);
	}

	protected virtual void AfterStore(DirStoreRequest sr)
	{
	}

	protected virtual void UpdateDataViewPanel(DataViewPanel dvp)
	{
		if (Directory.Id > 0)
		{
			dvp.Update(Directory.Id);
		}
		else
		{
			dvp.Update(Directory.Code);
		}
	}

	public override CheckEditExt AddBooleanControl(LayoutGroup container, FieldDefinition definition, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		return AddBooleanControl(container, definition.Name, definition.Caption, storeValue: false, isThreeState: false, "", isNew, helpText);
	}

	public override CheckEditExt AddBooleanControl(FieldDefinition definition, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		return base.AddBooleanControl(definition, isNew, helpText);
	}

	public ButtonEditExt AddButtonEditControl(FieldDefinition definition, short estimatedButtonsCount = 1, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)definition;
		return AddButtonEditControl(stringFieldDefinition.Name, stringFieldDefinition.Caption, stringFieldDefinition.Length, stringFieldDefinition.IsRequired, estimatedButtonsCount, storeValue: false, "", isNew, helpText);
	}

	public ButtonEditExt AddButtonEditControl(LayoutGroup container, FieldDefinition definition, short estimatedButtonsCount = 1, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)definition;
		return AddButtonEditControl(container, stringFieldDefinition.Name, stringFieldDefinition.Caption, stringFieldDefinition.Length, stringFieldDefinition.IsRequired, estimatedButtonsCount, storeValue: false, "", LayoutItemLabelPosition.Left, isNew, helpText);
	}

	public ComboBoxEditExt AddComboBoxControl(FieldDefinition definition, IEnumerable<string> itemsSource, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)definition;
		return AddComboBoxControl(stringFieldDefinition.Name, stringFieldDefinition.Caption, stringFieldDefinition.Length, itemsSource, stringFieldDefinition.IsRequired, storeValue: false, horizontalAlignment, "", isNew, helpText);
	}

	public ComboBoxEditExt AddComboBoxControl(LayoutGroup container, FieldDefinition definition, IEnumerable<string> itemsSource, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)definition;
		return AddComboBoxControl(container, stringFieldDefinition.Name, stringFieldDefinition.Caption, stringFieldDefinition.Length, itemsSource, stringFieldDefinition.IsRequired, storeValue: false, horizontalAlignment, "", isNew, helpText);
	}

	public override DateEditControl AddDateControl(FieldDefinition definition, bool useLongDate = false, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		return base.AddDateControl(definition, useLongDate, isNew, helpText);
	}

	public DateEditControl AddDateControl(LayoutGroup container, FieldDefinition definition, bool useLongDate = false, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		return AddDateControl(container, definition.Name, definition.Caption, useLongDate, definition.IsRequired, storeValue: false, "", isNew, helpText);
	}

	public DropDownCommentControl AddDropDownCommentControl(FieldDefinition definition, IDropDownView idv, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)definition;
		return AddDropDownCommentControl(stringFieldDefinition.Name, stringFieldDefinition.Caption, idv, stringFieldDefinition.IsRequired, storeValue: false, stringFieldDefinition.Length, horizontalAlignment, "", isNew, helpText);
	}

	public DropDownCommentControl AddDropDownCommentControl(LayoutGroup container, FieldDefinition definition, IDropDownView idv, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)definition;
		return AddDropDownCommentControl(container, stringFieldDefinition.Name, stringFieldDefinition.Caption, idv, stringFieldDefinition.IsRequired, storeValue: false, stringFieldDefinition.Length, horizontalAlignment, "", LayoutItemLabelPosition.Left, isNew, helpText);
	}

	public DropDownControl AddSmartEnumDropDownControl(FieldDefinition fieldDefinition, bool allowAdditionButtons = false, bool isShowComment = true, int codeWidth = -1, bool isNew = false, string helpText = "")
	{
		SmartEnumFieldDefinition smartEnumFieldDefinition = (SmartEnumFieldDefinition)fieldDefinition;
		return AddDropDownControl(fieldDefinition, new SmartEnum(smartEnumFieldDefinition.SmartEnumType, smartEnumFieldDefinition.Values, padLeft: false, '0', allSubTypes: false, smartEnumFieldDefinition.ValueMember), allowAdditionButtons, isShowComment, codeWidth, isNew, helpText);
	}

	public DropDownControl AddSmartEnumDropDownControl(LayoutGroup container, FieldDefinition fieldDefinition, bool allowAdditionButtons = false, bool isShowComment = true, int codeWidth = -1, bool isNew = false, string helpText = "")
	{
		SmartEnumFieldDefinition smartEnumFieldDefinition = (SmartEnumFieldDefinition)fieldDefinition;
		return AddDropDownControl(container, fieldDefinition, new SmartEnum(smartEnumFieldDefinition.SmartEnumType, smartEnumFieldDefinition.Values, padLeft: false, '0', allSubTypes: false, smartEnumFieldDefinition.ValueMember), allowAdditionButtons, isShowComment, codeWidth, isNew, helpText);
	}

	public override DropDownControl AddDropDownControl(FieldDefinition definition, IDropDownView idv, bool allowAdditionButtons = false, bool isShowComment = true, int codeWidth = -1, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		return AddDropDownControl(definition.Name, definition.Caption, idv, definition.IsRequired, allowAdditionButtons, storeValue: false, isShowComment, codeWidth, isNew, helpText);
	}

	public override DropDownControl AddDropDownControl(LayoutGroup container, FieldDefinition definition, IDropDownView idv, bool allowAdditionButtons = false, bool showComment = true, int codeWidth = -1, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		return AddDropDownControl(container, definition.Name, definition.Caption, idv, definition.IsRequired, allowAdditionButtons, storeValue: false, showComment, codeWidth, "", LayoutItemLabelPosition.Left, isNew, helpText);
	}

	public MemoEditControl AddMemoControl(FieldDefinition definition, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)definition;
		return AddMemoControl(stringFieldDefinition.Name, stringFieldDefinition.Caption, stringFieldDefinition.Length, stringFieldDefinition.IsRequired, storeValue: false, horizontalAlignment, "", isNew, helpText);
	}

	public MemoEditControl AddMemoControl(LayoutGroup container, FieldDefinition definition, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)definition;
		return AddMemoControl(container, stringFieldDefinition.Name, stringFieldDefinition.Caption, stringFieldDefinition.Length, stringFieldDefinition.IsRequired, storeValue: false, horizontalAlignment, "", isNew, helpText);
	}

	public MonthYearControl AddMonthYearControl(FieldDefinition definition, bool isFirstMonthDay, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		return AddMonthYearControl(definition.Name, definition.Caption, isFirstMonthDay, definition.IsRequired, storeValue: false, "", isNew, helpText);
	}

	public MonthYearControl AddMonthYearControl(LayoutGroup container, FieldDefinition definition, bool isFirstMonthDay, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		return AddMonthYearControl(container, definition.Name, definition.Caption, isFirstMonthDay, definition.IsRequired, storeValue: false, "", isNew, helpText);
	}

	public MultiSelectDropDownControl AddMultiSelectDropDownControl(FieldDefinition definition, IDropDownView idv, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		return AddMultiSelectDropDownControl(definition.Name, definition.Caption, idv, definition.IsRequired, storeValue: false, "", isNew, helpText);
	}

	public MultiSelectDropDownControl AddMultiSelectDropDownControl(LayoutGroup container, FieldDefinition definition, IDropDownView idv, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		return AddMultiSelectDropDownControl(container, definition.Name, definition.Caption, idv, definition.IsRequired, storeValue: false, "", isNew, helpText);
	}

	public override PopupCalcEditExt AddNumericControl(FieldDefinition definition, bool useCurrencyFormat = true, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		return base.AddNumericControl(definition, useCurrencyFormat, isNew, helpText);
	}

	public override PopupCalcEditExt AddNumericControl(LayoutGroup container, FieldDefinition definition, bool useCurrencyFormat = true, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		return base.AddNumericControl(container, definition, useCurrencyFormat, isNew, helpText);
	}

	public RadioButtonExt AddRadioButton(FieldDefinition definition, string groupName)
	{
		_dataItemNames.Add(definition.Name);
		return AddRadioButton(definition.Name, definition.Caption, groupName);
	}

	public RadioButtonExt AddRadioButton(LayoutGroup container, FieldDefinition definition, string groupName)
	{
		_dataItemNames.Add(definition.Name);
		return AddRadioButton(container, definition.Name, definition.Caption, groupName);
	}

	public override TextEditExt AddStringControl(FieldDefinition definition, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		return base.AddStringControl(definition, isNew, helpText);
	}

	public TextEditExt AddStringControl(LayoutGroup container, FieldDefinition definition, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)definition;
		return AddStringControl(container, stringFieldDefinition.Name, stringFieldDefinition.Caption, stringFieldDefinition.Length, stringFieldDefinition.IsRequired, storeValue: false, "", horizontalAlignment, LayoutItemLabelPosition.Left, isNew, helpText);
	}

	public SpinEditExt AddSpinEditControl(FieldDefinition definition, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		SpinFieldDefinition spinFieldDefinition = (SpinFieldDefinition)definition;
		return AddSpinEdit(spinFieldDefinition.Name, spinFieldDefinition.Caption, spinFieldDefinition.MinValue, spinFieldDefinition.MaxValue, 50.0, storeValue: false, "", spinFieldDefinition.Type, isNew, helpText);
	}

	public SpinEditExt AddSpinEditControl(LayoutGroup container, FieldDefinition definition, bool isNew = false, string helpText = "")
	{
		_dataItemNames.Add(definition.Name);
		SpinFieldDefinition spinFieldDefinition = (SpinFieldDefinition)definition;
		return AddSpinEdit(container, spinFieldDefinition.Name, spinFieldDefinition.Caption, spinFieldDefinition.MinValue, spinFieldDefinition.MaxValue, 50.0, storeValue: false, "", null, LayoutItemLabelPosition.Left, isNew, helpText);
	}

	protected void Code_MagicClick(object sender, MagicClickEventArgs e)
	{
		if (_code.IsEditorActive)
		{
			_code.ClearError();
		}
		SetNextCode();
		_code.DoValidate();
		_code.SelectAll();
	}

	protected virtual void SetNextCode()
	{
		if (_definition.NextCodeParamId.HasValue)
		{
			_code.EditValue = Settings.ParametersService.GetGeneralValue(_definition.NextCodeParamId.Value);
		}
	}
}
