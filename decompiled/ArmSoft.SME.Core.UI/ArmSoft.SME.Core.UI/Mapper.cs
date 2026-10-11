using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI.DataViews;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI;

public class Mapper
{
	public static MyGridColumn Convert(ViewColumn sourceColumn, Func<string, DataColumnType> getColumnType)
	{
		MyGridColumn myGridColumn = new MyGridColumn
		{
			Name = sourceColumn.Name,
			FieldName = sourceColumn.Source,
			Header = sourceColumn.Caption,
			Visible = sourceColumn.Visible,
			AllowGrouping = DefaultBoolean.False,
			AllowEditing = DefaultBoolean.True,
			ReadOnly = true,
			AllowColumnFiltering = DefaultBoolean.True,
			FilterPopupMode = FilterPopupMode.CheckedList
		};
		if (sourceColumn.Converter != null)
		{
			Binding binding = new Binding(sourceColumn.Name)
			{
				Converter = sourceColumn.Converter,
				Mode = BindingMode.TwoWay
			};
			myGridColumn.Binding = binding;
			if (sourceColumn.Format == "Image")
			{
				myGridColumn.CellTemplate = Settings.GenericResources["ColumnToImage"] as DataTemplate;
				myGridColumn.UnboundType = UnboundColumnType.Object;
				myGridColumn.AllowEditing = DefaultBoolean.False;
			}
		}
		else
		{
			switch (getColumnType(sourceColumn.Source))
			{
			case DataColumnType.String:
				if (sourceColumn.IsDynamic)
				{
					myGridColumn.UnboundType = UnboundColumnType.String;
				}
				break;
			case DataColumnType.DateTime:
				if (sourceColumn.IsDynamic)
				{
					myGridColumn.UnboundType = UnboundColumnType.DateTime;
				}
				myGridColumn.EditSettings = new DateEditSettings();
				if (string.IsNullOrWhiteSpace(sourceColumn.Format))
				{
					myGridColumn.EditSettings.DisplayFormat = "dd/MM/yy HH:mm:ss";
				}
				else
				{
					myGridColumn.EditSettings.DisplayFormat = sourceColumn.Format;
				}
				break;
			case DataColumnType.DateOnly:
				if (sourceColumn.IsDynamic)
				{
					myGridColumn.UnboundType = UnboundColumnType.DateTime;
				}
				myGridColumn.EditSettings = new DateEditSettings();
				if (string.IsNullOrWhiteSpace(sourceColumn.Format))
				{
					myGridColumn.EditSettings.DisplayFormat = "dd/MM/yy";
				}
				else
				{
					myGridColumn.EditSettings.DisplayFormat = sourceColumn.Format;
				}
				break;
			case DataColumnType.Decimal:
				if (sourceColumn.IsDynamic)
				{
					myGridColumn.UnboundType = UnboundColumnType.Decimal;
				}
				myGridColumn.EditSettings = new TextEditSettings();
				myGridColumn.DisplayTemplate = (ControlTemplate)Settings.GenericResources["ColumnDisplayTemplate"];
				if (string.IsNullOrWhiteSpace(sourceColumn.Format))
				{
					myGridColumn.EditSettings.DisplayFormat = DecimalExtension.GetCurrencyFormat(16, 2);
				}
				else
				{
					myGridColumn.EditSettings.DisplayFormat = sourceColumn.Format;
				}
				break;
			case DataColumnType.Integer:
				if (sourceColumn.IsDynamic)
				{
					myGridColumn.UnboundType = UnboundColumnType.Integer;
				}
				if (!string.IsNullOrWhiteSpace(sourceColumn.Format))
				{
					myGridColumn.EditSettings = new TextEditSettings();
					myGridColumn.DisplayTemplate = (ControlTemplate)Settings.GenericResources["ColumnDisplayTemplate"];
					myGridColumn.EditSettings.DisplayFormat = sourceColumn.Format;
				}
				break;
			case DataColumnType.Boolean:
				if (sourceColumn.IsDynamic)
				{
					myGridColumn.UnboundType = UnboundColumnType.Boolean;
				}
				break;
			}
		}
		return myGridColumn;
	}
}
