using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Caches;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.PersistableObject;
using DevExpress.XtraEditors.DXErrorProvider;

namespace ArmSoft.SME.Core.UI.Documents;

public class PropertyValidator
{
	public static void RemoveDocumentGridValidators(PODefinition definition, IPOWindowInfo windowInfo)
	{
		if (definition.Grids == null || definition.Grids.Count == 0)
		{
			return;
		}
		foreach (GridDefinition value2 in definition.Grids.Values)
		{
			Type gridRowType = windowInfo.GetGridRowType(value2.Name);
			if (DocumentGridRow.Validators.TryGetValue(gridRowType, out var value))
			{
				DocumentGridRow.Validators.Remove(gridRowType);
				value.Validators = new Dictionary<string, DocumentGridRow.ValidatorIndexPair>();
			}
		}
	}

	public static void AddDocumentGridStandardValidators(PODefinition definition, IPOWindowInfo windowInfo)
	{
		if (definition.Grids == null || definition.Grids.Count == 0)
		{
			return;
		}
		foreach (GridDefinition value4 in definition.Grids.Values)
		{
			Type gridRowType = windowInfo.GetGridRowType(value4.Name);
			if (gridRowType == null || (DocumentGridRow.Validators.TryGetValue(gridRowType, out var value) && value.IsCoreAdded))
			{
				break;
			}
			DocumentGridRow.ValidatorsDescription validatorsDescription = new DocumentGridRow.ValidatorsDescription
			{
				IsCoreAdded = true,
				CacheValidationResults = true,
				Validators = new Dictionary<string, DocumentGridRow.ValidatorIndexPair>()
			};
			int num = 0;
			foreach (FieldDefinition value5 in value4.Columns.Values)
			{
				FieldDefinition column = value5;
				Dictionary<string, DocumentGridRow.ValidatorIndexPair> validators = validatorsDescription.Validators;
				string name = column.Name;
				DocumentGridRow.ValidatorIndexPair validatorIndexPair = new DocumentGridRow.ValidatorIndexPair
				{
					Index = num++
				};
				int num2 = 1;
				List<Action<DocumentGridRow, ErrorInfo>> list = new List<Action<DocumentGridRow, ErrorInfo>>(num2);
				CollectionsMarshal.SetCount(list, num2);
				CollectionsMarshal.AsSpan(list)[0] = action;
				validatorIndexPair.Actions = list;
				validators.Add(name, validatorIndexPair);
				void action(DocumentGridRow row, ErrorInfo info)
				{
					GetDefaultValidator(column, row, info);
				}
			}
			num = 0;
			if (DocumentGridRow.Validators.TryGetValue(gridRowType, out var value2))
			{
				if (value2.Validators == null)
				{
					value2.Validators = validatorsDescription.Validators;
				}
				else
				{
					num = value2.Validators.Count;
					foreach (KeyValuePair<string, DocumentGridRow.ValidatorIndexPair> validator in validatorsDescription.Validators)
					{
						if (value2.Validators.TryGetValue(validator.Key, out var value3))
						{
							value3.Actions.AddRange(validator.Value.Actions);
							continue;
						}
						value2.Validators.Add(validator.Key, new DocumentGridRow.ValidatorIndexPair
						{
							Index = num++,
							Actions = validator.Value.Actions
						});
					}
				}
				value2.IsCoreAdded = true;
			}
			else
			{
				DocumentGridRow.Validators.Add(gridRowType, validatorsDescription);
			}
		}
	}

	private static void GetDefaultValidator(FieldDefinition columnDefinition, DocumentGridRow row, ErrorInfo info)
	{
		if (columnDefinition is TreeNodeFieldDefinition definition)
		{
			ValidateTreeNode(Convert.ToString(row.GetType().GetProperty(columnDefinition.Name).GetValue(row)), info, definition);
		}
		else if (columnDefinition is SmartEnumFieldDefinition definition2)
		{
			ValidateSmartEnumValue(Convert.ToString(row.GetType().GetProperty(columnDefinition.Name).GetValue(row)), info, definition2);
		}
		if (columnDefinition is StringFieldDefinition)
		{
			if (columnDefinition.IsRequired)
			{
				EmptyStringValidate((string)row.GetType().GetProperty(columnDefinition.Name).GetValue(row), info, columnDefinition);
			}
		}
		else if (columnDefinition is DateFieldDefinition)
		{
			if (columnDefinition.IsRequired)
			{
				EmptyDateValidate((DateOnly?)row.GetType().GetProperty(columnDefinition.Name).GetValue(row), info, columnDefinition.Caption);
			}
		}
		else if (columnDefinition is PercentFieldDefinition percentFieldDefinition)
		{
			ValidatePercent(Convert.ToDecimal(row.GetType().GetProperty(percentFieldDefinition.Name).GetValue(row)), info, percentFieldDefinition);
		}
		else if (columnDefinition is NumberFieldDefinition numberFieldDefinition && (columnDefinition.IsRequired || numberFieldDefinition.IsNonNegative))
		{
			ValidateNumber(Convert.ToDecimal(row.GetType().GetProperty(columnDefinition.Name).GetValue(row)), info, numberFieldDefinition);
		}
	}

	public static void ValidateString(string value, ErrorInfo info, string caption, int length, bool isRequired = false)
	{
		ValidateString(caption, value, info, length, isRequired);
	}

	private static void ValidateString(string caption, string value, ErrorInfo info, int length, bool isRequired = false)
	{
		if (isRequired)
		{
			EmptyStringValidate(value, info, caption);
		}
		if (info.ErrorType != ErrorType.Critical && !string.IsNullOrWhiteSpace(value) && value.Length > length)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(Resources.FieldLengthExceeded, caption, value, length);
		}
	}

	public static void EmptyStringValidate(string value, ErrorInfo info, FieldDefinition definition)
	{
		if (definition.IsRequired)
		{
			EmptyStringValidate(value, info, definition.Caption);
		}
	}

	public static void EmptyStringValidate(string value, ErrorInfo info, string caption)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(Resources.FieldIsEmpty, caption);
		}
	}

	public static void ValidateNumber(decimal value, ErrorInfo info, NumberFieldDefinition definition)
	{
		if (definition.PrecisionDefinedInParam)
		{
			ValidateNumber(value, info, definition.Caption, definition.LengthFromParam(Settings.ParametersService), definition.PrecisionFromParam(Settings.ParametersService), definition.IsRequired, definition.IsNonNegative);
		}
		else
		{
			ValidateNumber(value, info, definition.Caption, definition.Length, definition.Precision, definition.IsRequired, definition.IsNonNegative);
		}
	}

	public static void ValidateNumber(decimal value, ErrorInfo info, FieldDefinition definition)
	{
		if (definition is NumberFieldDefinition numberFieldDefinition)
		{
			if (numberFieldDefinition.PrecisionDefinedInParam)
			{
				ValidateNumber(value, info, definition.Caption, numberFieldDefinition.LengthFromParam(Settings.ParametersService), numberFieldDefinition.PrecisionFromParam(Settings.ParametersService), definition.IsRequired, numberFieldDefinition.IsNonNegative);
			}
			else
			{
				ValidateNumber(value, info, definition.Caption, numberFieldDefinition.Length, numberFieldDefinition.Precision, definition.IsRequired, numberFieldDefinition.IsNonNegative);
			}
		}
	}

	public static void ValidateNumber(decimal value, ErrorInfo info, string caption, short length, short precision, bool isRequired = false, bool isNonNegative = false)
	{
		if (isRequired)
		{
			NonZeroValidate(value, info, caption);
		}
		if (isNonNegative && info.ErrorType != ErrorType.Critical)
		{
			NonNegativeValidate(value, info, caption);
		}
		if (info.ErrorType != ErrorType.Critical)
		{
			if (value - value.RoundAs(precision) != 0m)
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = string.Format(Resources.InvalidNumberPrecision, caption, 0, precision);
			}
			else if (Convert.ToString(Math.Truncate(Math.Abs(value))).Length + ((precision > 0) ? (precision + 1) : 0) > length)
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = string.Format(Resources.InvalidNumberLength, caption, 0, length);
			}
		}
	}

	public static void PositiveValidate(decimal value, ErrorInfo info, string caption)
	{
		if (value == 0m)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(Resources.FieldIsEmpty, caption);
		}
		else if (value < 0m)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(Resources.ValueIsNegativeWithName, caption);
		}
	}

	public static void NonNegativeValidate(decimal value, ErrorInfo info, string caption)
	{
		if (value < 0m)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(Resources.ValueIsNegativeWithName, caption);
		}
	}

	public static void NonZeroValidate(decimal value, ErrorInfo info, string caption)
	{
		if (value == 0m)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(Resources.FieldIsEmpty, caption);
		}
	}

	public static void EmptyDateValidate(DateOnly? value, ErrorInfo info, string caption)
	{
		if (!value.HasValue)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(Resources.FieldIsEmpty, caption);
		}
	}

	public static void ValidatePercent(decimal value, ErrorInfo info, PercentFieldDefinition definition)
	{
		if (info.ErrorType != ErrorType.Critical && value > 100m)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = Resources.PercentLessHundred;
		}
		else if (info.ErrorType != ErrorType.Critical)
		{
			ValidateNumber(value, info, definition.Caption, definition.Length, definition.Precision, definition.IsRequired, definition.IsNonNegative);
		}
	}

	public static void ValidateSmartEnumValue(string value, ErrorInfo info, SmartEnumFieldDefinition definition)
	{
		if (definition.PadLeftInUI)
		{
			value = value.TrimStart('0');
		}
		if (definition.Values != null && definition.Values.Count > 0 && !string.IsNullOrWhiteSpace(value) && !definition.Values.Contains(value))
		{
			SmartEnumProviderSupportAttribute smartEnumProviderSupportAttribute = (SmartEnumProviderSupportAttribute)Attribute.GetCustomAttribute(definition.SmartEnumType, typeof(SmartEnumProviderSupportAttribute));
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(Resources.ElemWithCodeNotExists, value, Settings.StringResourceService.GetStringResource(smartEnumProviderSupportAttribute.CaptionResourceId));
		}
		else
		{
			ValidateSmartEnumValue(info, definition.Name, definition.Caption, definition.SmartEnumType, value, definition.IsRequired);
		}
	}

	public static void ValidateSmartEnumValue(string value, ErrorInfo info, FieldDefinition definition)
	{
		if (definition is SmartEnumFieldDefinition smartEnumFieldDefinition)
		{
			ValidateSmartEnumValue(info, smartEnumFieldDefinition.Name, smartEnumFieldDefinition.Caption, smartEnumFieldDefinition.SmartEnumType, value);
		}
	}

	public static void ValidateSmartEnumValue(ErrorInfo info, string name, string caption, Type smartEnumType, string value, bool isRequired = false)
	{
		if (isRequired)
		{
			EmptyStringValidate(value, info, caption);
		}
		if (!string.IsNullOrWhiteSpace(value))
		{
			int num = 1;
			List<string> list = new List<string>(num);
			CollectionsMarshal.SetCount(list, num);
			CollectionsMarshal.AsSpan(list)[0] = value;
			List<CodeName> smartEnumCodeCaption = SmartEnumHelper.GetSmartEnumCodeCaption(smartEnumType, list);
			if (smartEnumCodeCaption == null || smartEnumCodeCaption.Count == 0)
			{
				SmartEnumProviderSupportAttribute smartEnumProviderSupportAttribute = (SmartEnumProviderSupportAttribute)Attribute.GetCustomAttribute(smartEnumType, typeof(SmartEnumProviderSupportAttribute));
				ValidateString(value, info, caption, smartEnumProviderSupportAttribute.ValueMemberLength, isRequired);
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = string.Format(Resources.ElemWithCodeNotExists, value, Settings.StringResourceService.GetStringResource(smartEnumProviderSupportAttribute.CaptionResourceId));
			}
		}
	}

	public static void ValidateTreeNode(string value, ErrorInfo info, TreeNodeFieldDefinition definition, bool checkActive = true)
	{
		ValidateTreeNode(value, info, definition.Caption, definition.TreeId, definition.IsRequired, definition.OnlyLeaf, checkActive);
	}

	public static void ValidateTreeNode(string value, ErrorInfo info, FieldDefinition definition, bool checkActive = true)
	{
		if (definition is TreeNodeFieldDefinition treeNodeFieldDefinition)
		{
			ValidateTreeNode(value, info, definition.Caption, treeNodeFieldDefinition.TreeId, definition.IsRequired, treeNodeFieldDefinition.OnlyLeaf, checkActive);
		}
	}

	public static void ValidateTreeNode(string value, ErrorInfo info, string caption, string treeId, bool isRequired = false, bool checkNotLeaf = true, bool checkActive = true)
	{
		if (isRequired)
		{
			EmptyStringValidate(value, info, caption);
		}
		if (info.ErrorType == ErrorType.Critical || string.IsNullOrWhiteSpace(value))
		{
			return;
		}
		TreeCacheRow item = Settings.CoreCacheService.Tree.GetItem(treeId);
		if (item == null)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(Resources.TreeNotExists, treeId);
			return;
		}
		TreeNodesCacheRow item2 = ((TreeNodesCache)item.TreeNodeCache).GetItem(value, throwExceptionNotExits: true);
		if ((item.Multilevel & checkNotLeaf) && !item2.IsLeaf)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(Resources.CodeIsNorLeaf, value, item.Caption);
		}
		else if ((item.IsClosedAvailable & checkActive) && item2.IsClosed)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = string.Format(Resources.TreeNodeIsClosed, value, item.Caption);
		}
	}
}
