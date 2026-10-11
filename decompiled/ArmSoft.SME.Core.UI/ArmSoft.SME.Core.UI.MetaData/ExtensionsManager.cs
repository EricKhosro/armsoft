using System;
using System.Collections.Generic;
using System.Windows.Controls;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.Controls;

namespace ArmSoft.SME.Core.UI.MetaData;

public class ExtensionsManager
{
	private readonly List<ExtensionControl> _extensionControls = new List<ExtensionControl>();

	public void AddControlToExtensionsList(Control myControl, DataItem di, string readOnlySource)
	{
		_extensionControls.Add(new ExtensionControl
		{
			Control = myControl,
			DataItem = di,
			ReadOnlySource = readOnlySource
		});
	}

	public void SetExtensionUIReadOnly(bool isReadOnly)
	{
		foreach (ExtensionControl extensionControl in _extensionControls)
		{
			if (extensionControl.DataItem is StringDataItem)
			{
				if (extensionControl.Control is TextEditExt textEditExt)
				{
					textEditExt.IsReadOnly = isReadOnly;
				}
				else if (extensionControl.Control is DropDownControl dropDownControl)
				{
					dropDownControl.IsReadOnly = isReadOnly;
				}
				else if (extensionControl.Control is DropDownCommentControl dropDownCommentControl)
				{
					dropDownCommentControl.IsReadOnly = isReadOnly;
				}
				else if (extensionControl.Control is MultiSelectDropDownControl multiSelectDropDownControl)
				{
					multiSelectDropDownControl.IsReadOnly = isReadOnly;
				}
			}
			else if (extensionControl.DataItem is TreeNodeDataItem)
			{
				if (extensionControl.Control is DropDownControl dropDownControl2)
				{
					dropDownControl2.IsReadOnly = isReadOnly;
				}
				else if (extensionControl.Control is DropDownCommentControl dropDownCommentControl2)
				{
					dropDownCommentControl2.IsReadOnly = isReadOnly;
				}
				else if (extensionControl.Control is MultiSelectDropDownControl multiSelectDropDownControl2)
				{
					multiSelectDropDownControl2.IsReadOnly = isReadOnly;
				}
			}
			else if (extensionControl.DataItem is NumberDataItem)
			{
				((PopupCalcEditExt)extensionControl.Control).IsReadOnly = isReadOnly;
			}
			else if (extensionControl.DataItem is DateDataItem)
			{
				((DateEditControl)extensionControl.Control).IsReadOnly = isReadOnly;
			}
			else if (extensionControl.DataItem is TimeDataItem)
			{
				((TimeEditControl)extensionControl.Control).IsReadOnly = isReadOnly;
			}
			else if (extensionControl.DataItem is BooleanDataItem)
			{
				((CheckEditExt)extensionControl.Control).IsReadOnly = isReadOnly;
			}
		}
	}

	public void FillExtension(ArmSoft.SME.Common.MetaData.DocumentExtension extension)
	{
		foreach (ExtensionControl extensionControl in _extensionControls)
		{
			if (extensionControl.DataItem is StringDataItem)
			{
				if (extensionControl.Control is TextEditExt textEditExt)
				{
					extension[extensionControl.DataItem.Name] = textEditExt.Text;
				}
				else if (extensionControl.Control is DropDownControl dropDownControl)
				{
					extension[extensionControl.DataItem.Name] = dropDownControl.Code;
				}
				else if (extensionControl.Control is DropDownCommentControl dropDownCommentControl)
				{
					extension[extensionControl.DataItem.Name] = dropDownCommentControl.Text;
				}
				else if (extensionControl.Control is MultiSelectDropDownControl multiSelectDropDownControl)
				{
					extension[extensionControl.DataItem.Name] = multiSelectDropDownControl.SelectedString;
				}
			}
			else if (extensionControl.DataItem is TreeNodeDataItem)
			{
				if (extensionControl.Control is DropDownControl dropDownControl2)
				{
					extension[extensionControl.DataItem.Name] = dropDownControl2.Code;
				}
				else if (extensionControl.Control is DropDownCommentControl dropDownCommentControl2)
				{
					extension[extensionControl.DataItem.Name] = dropDownCommentControl2.Text;
				}
				else if (extensionControl.Control is MultiSelectDropDownControl multiSelectDropDownControl2)
				{
					extension[extensionControl.DataItem.Name] = multiSelectDropDownControl2.SelectedString;
				}
			}
			else if (extensionControl.DataItem is NumberDataItem)
			{
				extension[extensionControl.DataItem.Name] = ((PopupCalcEditExt)extensionControl.Control).Value;
			}
			else if (extensionControl.DataItem is DateDataItem)
			{
				extension[extensionControl.DataItem.Name] = ((DateEditControl)extensionControl.Control).EditValue;
			}
			else if (extensionControl.DataItem is TimeDataItem)
			{
				extension[extensionControl.DataItem.Name] = ((TimeEditControl)extensionControl.Control).Text;
			}
			else if (extensionControl.DataItem is BooleanDataItem)
			{
				extension[extensionControl.DataItem.Name] = ((CheckEditExt)extensionControl.Control).IsChecked;
			}
		}
	}

	public void SetExtensionDefaultValues()
	{
		foreach (ExtensionControl extensionControl in _extensionControls)
		{
			if (extensionControl.DataItem is StringDataItem)
			{
				if (extensionControl.Control is TextEditExt textEditExt)
				{
					textEditExt.Text = (string)extensionControl.DataItem.DefaultValue();
				}
				else if (extensionControl.Control is DropDownControl dropDownControl)
				{
					dropDownControl.Code = (string)extensionControl.DataItem.DefaultValue();
				}
				else if (extensionControl.Control is DropDownCommentControl dropDownCommentControl)
				{
					dropDownCommentControl.Text = (string)extensionControl.DataItem.DefaultValue();
				}
				else if (extensionControl.Control is MultiSelectDropDownControl multiSelectDropDownControl)
				{
					multiSelectDropDownControl.SelectedString = (string)extensionControl.DataItem.DefaultValue();
				}
			}
			else if (extensionControl.DataItem is TreeNodeDataItem)
			{
				if (extensionControl.Control is DropDownControl dropDownControl2)
				{
					dropDownControl2.Code = (string)extensionControl.DataItem.DefaultValue();
				}
				else if (extensionControl.Control is DropDownCommentControl dropDownCommentControl2)
				{
					dropDownCommentControl2.Text = (string)extensionControl.DataItem.DefaultValue();
				}
				else if (extensionControl.Control is MultiSelectDropDownControl multiSelectDropDownControl2)
				{
					multiSelectDropDownControl2.SelectedString = (string)extensionControl.DataItem.DefaultValue();
				}
			}
			else if (extensionControl.DataItem is NumberDataItem)
			{
				((PopupCalcEditExt)extensionControl.Control).Value = (decimal)extensionControl.DataItem.DefaultValue();
			}
			else if (extensionControl.DataItem is DateDataItem)
			{
				if (extensionControl.DataItem.DefaultValue() != null)
				{
					((DateEditControl)extensionControl.Control).EditValue = Convert.ToDateTime(extensionControl.DataItem.DefaultValue()).ToDateOnly();
				}
				else
				{
					((DateEditControl)extensionControl.Control).EditValue = null;
				}
			}
			else if (extensionControl.DataItem is TimeDataItem)
			{
				if (extensionControl.DataItem.DefaultValue() != null)
				{
					((TimeEditControl)extensionControl.Control).EditValue = (string)extensionControl.DataItem.DefaultValue();
				}
				else
				{
					((TimeEditControl)extensionControl.Control).EditValue = null;
				}
			}
			else if (extensionControl.DataItem is BooleanDataItem)
			{
				((CheckEditExt)extensionControl.Control).IsChecked = (bool)extensionControl.DataItem.DefaultValue();
			}
		}
	}

	public void SetExtensionValues(ArmSoft.SME.Common.MetaData.DocumentExtension extension)
	{
		foreach (ExtensionControl extensionControl in _extensionControls)
		{
			if (extensionControl.DataItem is StringDataItem)
			{
				if (extensionControl.Control is TextEditExt textEditExt)
				{
					textEditExt.Text = (extensionControl.DataItem.SaveValueOnCopy ? ((string)extension[extensionControl.DataItem.Name]) : ((string)extensionControl.DataItem.DefaultValue()));
				}
				else if (extensionControl.Control is DropDownControl dropDownControl)
				{
					dropDownControl.Code = (extensionControl.DataItem.SaveValueOnCopy ? ((string)extension[extensionControl.DataItem.Name]) : ((string)extensionControl.DataItem.DefaultValue()));
				}
				else if (extensionControl.Control is DropDownCommentControl dropDownCommentControl)
				{
					dropDownCommentControl.Text = (extensionControl.DataItem.SaveValueOnCopy ? ((string)extension[extensionControl.DataItem.Name]) : ((string)extensionControl.DataItem.DefaultValue()));
				}
				else if (extensionControl.Control is MultiSelectDropDownControl multiSelectDropDownControl)
				{
					multiSelectDropDownControl.SelectedString = (extensionControl.DataItem.SaveValueOnCopy ? ((string)extension[extensionControl.DataItem.Name]) : ((string)extensionControl.DataItem.DefaultValue()));
				}
			}
			else if (extensionControl.DataItem is TreeNodeDataItem)
			{
				if (extensionControl.Control is DropDownControl dropDownControl2)
				{
					dropDownControl2.Code = (extensionControl.DataItem.SaveValueOnCopy ? ((string)extension[extensionControl.DataItem.Name]) : ((string)extensionControl.DataItem.DefaultValue()));
				}
				else if (extensionControl.Control is DropDownCommentControl dropDownCommentControl2)
				{
					dropDownCommentControl2.Text = (extensionControl.DataItem.SaveValueOnCopy ? ((string)extension[extensionControl.DataItem.Name]) : ((string)extensionControl.DataItem.DefaultValue()));
				}
				else if (extensionControl.Control is MultiSelectDropDownControl multiSelectDropDownControl2)
				{
					multiSelectDropDownControl2.SelectedString = (extensionControl.DataItem.SaveValueOnCopy ? ((string)extension[extensionControl.DataItem.Name]) : ((string)extensionControl.DataItem.DefaultValue()));
				}
			}
			else if (extensionControl.DataItem is NumberDataItem)
			{
				((PopupCalcEditExt)extensionControl.Control).Value = (extensionControl.DataItem.SaveValueOnCopy ? ((decimal)extension[extensionControl.DataItem.Name]) : ((decimal)extensionControl.DataItem.DefaultValue()));
			}
			else if (extensionControl.DataItem is DateDataItem)
			{
				if (extensionControl.DataItem.DefaultValue() != null)
				{
					((DateEditControl)extensionControl.Control).EditValue = ((extensionControl.DataItem.SaveValueOnCopy && extension[extensionControl.DataItem.Name] != null) ? Convert.ToDateTime(extension[extensionControl.DataItem.Name]).ToDateOnly() : Convert.ToDateTime(extensionControl.DataItem.DefaultValue()).ToDateOnly());
				}
				else
				{
					((DateEditControl)extensionControl.Control).EditValue = ((extensionControl.DataItem.SaveValueOnCopy && extension[extensionControl.DataItem.Name] != null) ? new DateOnly?(Convert.ToDateTime(extension[extensionControl.DataItem.Name]).ToDateOnly()) : ((DateOnly?)null));
				}
			}
			else if (extensionControl.DataItem is TimeDataItem)
			{
				if (extensionControl.DataItem.DefaultValue() != null)
				{
					((TimeEditControl)extensionControl.Control).EditValue = (extensionControl.DataItem.SaveValueOnCopy ? ((string)extension[extensionControl.DataItem.Name]) : ((string)extensionControl.DataItem.DefaultValue()));
				}
				else
				{
					((TimeEditControl)extensionControl.Control).EditValue = (extensionControl.DataItem.SaveValueOnCopy ? ((string)extension[extensionControl.DataItem.Name]) : null);
				}
			}
			else if (extensionControl.DataItem is BooleanDataItem)
			{
				((CheckEditExt)extensionControl.Control).IsChecked = (extensionControl.DataItem.SaveValueOnCopy ? ((bool)extension[extensionControl.DataItem.Name]) : ((bool)extensionControl.DataItem.DefaultValue()));
			}
		}
	}

	public void LoadDataToExtensionControls(ArmSoft.SME.Common.MetaData.DocumentExtension extension)
	{
		foreach (ExtensionControl extensionControl in _extensionControls)
		{
			bool isReadOnly = false;
			if (!string.IsNullOrWhiteSpace(extensionControl.ReadOnlySource))
			{
				isReadOnly = extension.EvaluateExtensionUIReadOnly(extensionControl.ReadOnlySource);
			}
			if (extensionControl.DataItem is StringDataItem)
			{
				if (extensionControl.Control is TextEditExt textEditExt)
				{
					textEditExt.Text = (string)extension[extensionControl.DataItem.Name];
					textEditExt.IsReadOnly = isReadOnly;
				}
				else if (extensionControl.Control is DropDownControl dropDownControl)
				{
					dropDownControl.Code = (string)extension[extensionControl.DataItem.Name];
					dropDownControl.IsReadOnly = isReadOnly;
				}
				else if (extensionControl.Control is DropDownCommentControl dropDownCommentControl)
				{
					dropDownCommentControl.Text = (string)extension[extensionControl.DataItem.Name];
					dropDownCommentControl.IsReadOnly = isReadOnly;
				}
				else if (extensionControl.Control is MultiSelectDropDownControl multiSelectDropDownControl)
				{
					multiSelectDropDownControl.SelectedString = (string)extension[extensionControl.DataItem.Name];
					multiSelectDropDownControl.IsReadOnly = isReadOnly;
				}
			}
			else if (extensionControl.DataItem is TreeNodeDataItem)
			{
				if (extensionControl.Control is DropDownControl dropDownControl2)
				{
					dropDownControl2.Code = (string)extension[extensionControl.DataItem.Name];
					dropDownControl2.IsReadOnly = isReadOnly;
				}
				else if (extensionControl.Control is DropDownCommentControl dropDownCommentControl2)
				{
					dropDownCommentControl2.Text = (string)extension[extensionControl.DataItem.Name];
					dropDownCommentControl2.IsReadOnly = isReadOnly;
				}
				else if (extensionControl.Control is MultiSelectDropDownControl multiSelectDropDownControl2)
				{
					multiSelectDropDownControl2.SelectedString = (string)extension[extensionControl.DataItem.Name];
					multiSelectDropDownControl2.IsReadOnly = isReadOnly;
				}
			}
			else if (extensionControl.DataItem is NumberDataItem)
			{
				PopupCalcEditExt popupCalcEditExt = (PopupCalcEditExt)extensionControl.Control;
				popupCalcEditExt.Value = (decimal)extension[extensionControl.DataItem.Name];
				popupCalcEditExt.IsReadOnly = isReadOnly;
			}
			else if (extensionControl.DataItem is DateDataItem)
			{
				DateEditControl dateEditControl = (DateEditControl)extensionControl.Control;
				if (extension[extensionControl.DataItem.Name] != null)
				{
					if (DateOnly.TryParse(extension[extensionControl.DataItem.Name].ToString(), out var result))
					{
						dateEditControl.EditValue = result;
					}
				}
				else
				{
					dateEditControl.EditValue = null;
				}
				dateEditControl.IsReadOnly = isReadOnly;
			}
			else if (extensionControl.DataItem is TimeDataItem)
			{
				TimeEditControl timeEditControl = (TimeEditControl)extensionControl.Control;
				if (extension[extensionControl.DataItem.Name] != null)
				{
					timeEditControl.EditValue = (string)extension[extensionControl.DataItem.Name];
				}
				else
				{
					timeEditControl.EditValue = null;
				}
				timeEditControl.IsReadOnly = isReadOnly;
			}
			else if (extensionControl.DataItem is BooleanDataItem)
			{
				CheckEditExt checkEditExt = (CheckEditExt)extensionControl.Control;
				checkEditExt.IsChecked = (bool)extension[extensionControl.DataItem.Name];
				checkEditExt.IsReadOnly = isReadOnly;
			}
		}
	}
}
