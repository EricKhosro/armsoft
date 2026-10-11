using System;
using System.Collections.Generic;
using System.Windows.Data;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.MetaData;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.DataViews;

public class ViewColumn : IColumn
{
	public bool Visible { get; set; }

	public bool FooterSum { get; set; }

	public bool IsDynamic { get; set; }

	public bool IsReference { get; set; }

	public bool IsExtended { get; set; }

	public bool IsSystem { get; set; }

	public string Name { get; set; }

	public string Caption { get; set; }

	public string Source { get; set; }

	public string MethodInfoSource { get; set; }

	public string Format { get; set; }

	public DataColumnType Type { get; set; }

	public string Group { get; set; }

	public string Before { get; set; }

	public string After { get; set; }

	public bool IsUnbound { get; set; }

	public string UnboundExpression { get; set; }

	public virtual IValueConverter Converter { get; set; }

	public ViewColumn(string name, string caption, string source)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new InvalidOperationException("Column name is Empty.");
		}
		Name = name.Trim();
		Caption = ((caption == null) ? string.Empty : caption);
		Source = ((source == null) ? string.Empty : source);
		Type = DataColumnType.Unknown;
		Group = string.Empty;
		IsDynamic = false;
		Visible = true;
		FooterSum = false;
		IsSystem = false;
	}

	public ViewColumn(string name, string caption, string source, DataColumnType type, string group, string cachePrefix)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new InvalidOperationException("Column name is Empty.");
		}
		Name = cachePrefix + name.Trim();
		Caption = ((caption == null) ? string.Empty : caption);
		Source = ((source == null) ? string.Empty : (cachePrefix + source));
		Type = type;
		Group = group;
		IsDynamic = false;
		Visible = true;
		FooterSum = false;
		IsSystem = false;
		IsReference = true;
	}

	public static List<ViewColumn> AddXMLColumns(short poTypeDef, short poType, string group, string prefix, bool addGroupToCaption = true, string captionPrefix = null)
	{
		List<ViewColumn> list = new List<ViewColumn>();
		ArmSoft.SME.Core.UI.MetaData.DocumentExtensionDefinition documentExtensionDefinition = Settings.ServiceProvider.GetRequiredService<IDocumentExtensionDefinitionService>().Load(poTypeDef, poType);
		if (addGroupToCaption)
		{
			captionPrefix = group;
		}
		if (documentExtensionDefinition != null)
		{
			foreach (DataItem value in documentExtensionDefinition.DataItems.Values)
			{
				string text = prefix + value.Name;
				ViewColumn viewColumn = new ViewColumn(text, (!string.IsNullOrWhiteSpace(captionPrefix)) ? string.Format(Resources.ColumnNameWithParts, captionPrefix, value.Caption) : value.Caption, text)
				{
					IsReference = true,
					Group = group,
					Visible = true,
					IsSystem = false
				};
				if (value is NumberDataItem numberDataItem)
				{
					viewColumn.Type = DataColumnType.Decimal;
					viewColumn.FooterSum = true;
					if (numberDataItem.UseCurrencyFormat)
					{
						viewColumn.Format = DecimalExtension.GetCurrencyFormat(numberDataItem.Length, numberDataItem.Precision);
					}
					else
					{
						viewColumn.Format = DecimalExtension.GetNumberFormat(numberDataItem.Length, numberDataItem.Precision);
					}
				}
				else if (value is BooleanDataItem)
				{
					viewColumn.Type = DataColumnType.Boolean;
					viewColumn.Format = "";
				}
				else if (value is DateDataItem dateDataItem)
				{
					if (dateDataItem.UseLongDate)
					{
						viewColumn.Format = "dd/MM/yyyy";
						viewColumn.Type = DataColumnType.DateTime;
					}
					else
					{
						viewColumn.Format = "dd/MM/yy";
						viewColumn.Type = DataColumnType.DateOnly;
					}
				}
				else
				{
					viewColumn.Type = DataColumnType.String;
					viewColumn.Format = "";
				}
				list.Add(viewColumn);
				if (value is StringDataItem stringDataItem)
				{
					UIField uIField = documentExtensionDefinition.GetUIField(stringDataItem.Name);
					if (uIField.DropDownItem != null)
					{
						short? dropDownCacheId = SmartEnumHelper.GetDropDownCacheId(uIField.DropDownItem.Name);
						if (dropDownCacheId.HasValue)
						{
							viewColumn.Caption = ((!string.IsNullOrWhiteSpace(captionPrefix)) ? string.Format(Resources.ColumnNameWithParts, captionPrefix, string.Format(Resources.ColumnNameWithParts, value.Caption, Resources.Code)) : string.Format(Resources.ColumnNameWithParts, value.Caption, Resources.Code));
							text = $"{viewColumn.Name}{"_DropDownName_"}{dropDownCacheId.Value}";
							ViewColumn item = new ViewColumn(text, (!string.IsNullOrWhiteSpace(captionPrefix)) ? string.Format(Resources.ColumnNameWithParts, captionPrefix, string.Format(Resources.ColumnNameWithParts, value.Caption, Resources.Name)) : string.Format(Resources.ColumnNameWithParts, value.Caption, Resources.Name), text)
							{
								IsReference = true,
								Group = group,
								Visible = true,
								IsSystem = false,
								Type = DataColumnType.String,
								Format = ""
							};
							list.Add(item);
						}
					}
				}
				if (value is TreeNodeDataItem treeNodeDataItem)
				{
					viewColumn.Caption = ((!string.IsNullOrWhiteSpace(captionPrefix)) ? string.Format(Resources.ColumnNameWithParts, captionPrefix, string.Format(Resources.ColumnNameWithParts, value.Caption, Resources.Code)) : string.Format(Resources.ColumnNameWithParts, value.Caption, Resources.Code));
					text = viewColumn.Name + "_TreeNodeName_" + treeNodeDataItem.TreeName;
					ViewColumn item2 = new ViewColumn(text, (!string.IsNullOrWhiteSpace(captionPrefix)) ? string.Format(Resources.ColumnNameWithParts, captionPrefix, string.Format(Resources.ColumnNameWithParts, value.Caption, Resources.Name)) : string.Format(Resources.ColumnNameWithParts, value.Caption, Resources.Name), text)
					{
						IsReference = true,
						Group = group,
						Visible = true,
						IsSystem = false,
						Type = DataColumnType.String,
						Format = ""
					};
					list.Add(item2);
				}
			}
		}
		return list;
	}

	public ViewColumn Clone()
	{
		return new ViewColumn(Name, Caption, Source)
		{
			Visible = Visible,
			Format = Format,
			FooterSum = FooterSum,
			Type = Type,
			IsDynamic = IsDynamic,
			IsSystem = IsSystem,
			Group = Group
		};
	}
}
