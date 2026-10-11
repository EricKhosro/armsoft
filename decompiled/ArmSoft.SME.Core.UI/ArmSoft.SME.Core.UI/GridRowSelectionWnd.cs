using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Utils;
using DevExpress.Utils.Localization;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI;

public class GridRowSelectionWnd : DialogWindow
{
	private static Dictionary<string, SelectCompareOperations> _dct;

	private GridControl DataViewGrid { get; set; }

	private DataViewPanel MyDvp { get; set; }

	private DocumentGridControl DialogGrid { get; set; }

	private List<SelectByConditionGridRow> SelectByConditionGridSource(GridControl grd, ref bool containsBetween, List<SelectByConditionGridRow> oldCondition = null)
	{
		List<SelectByConditionGridRow> list = new List<SelectByConditionGridRow>();
		containsBetween = false;
		for (int i = 0; i <= grd.Columns.Count - 1; i++)
		{
			if (!grd.Columns[i].Visible || !(grd.Columns[i].Name != "Selected"))
			{
				continue;
			}
			SelectByConditionGridRow selectByConditionGridRow = new SelectByConditionGridRow(grd.Columns[i].Header.ToString(), grd.Columns[i].Name, "", "", "", MyDvp.GetColumnType(grd.Columns[i].Name));
			if (oldCondition != null)
			{
				foreach (SelectByConditionGridRow item in oldCondition)
				{
					if (item.ColCode == selectByConditionGridRow.ColCode && item.OperationString != "")
					{
						selectByConditionGridRow.OperationString = item.OperationString;
						if ((selectByConditionGridRow.Operation == SelectCompareOperations.Between) | (selectByConditionGridRow.Operation == SelectCompareOperations.NotBetween))
						{
							containsBetween = true;
						}
						selectByConditionGridRow.ColValue1 = item.ColValue1;
						selectByConditionGridRow.ColValue2 = item.ColValue2;
						break;
					}
				}
			}
			list.Add(selectByConditionGridRow);
		}
		return list;
	}

	public void ConstructUI(DataViewPanel dvp, List<SelectByConditionGridRow> oldCondition = null, bool isSelectMode = true)
	{
		Title = (isSelectMode ? ArmSoft.SME.Common.Resources.SelectByCondition : ArmSoft.SME.Common.Resources.UnSelectByCondition);
		MyDvp = dvp;
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		LayoutGroupExt layoutGroupExt = AddVerticalLayoutGroup(container);
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Stretch;
		CheckEditExt checkEditExt = AddBooleanControl(layoutGroupExt, "PreserveSelection", ArmSoft.SME.Common.Resources.PreserveSelection);
		if (!isSelectMode)
		{
			checkEditExt.Visibility = Visibility.Collapsed;
			checkEditExt.IsChecked = true;
			((LayoutItemWithRequired)(object)checkEditExt.Parent).Visibility = Visibility.Collapsed;
		}
		AddBooleanControl(layoutGroupExt, "CaseInSensitive", ArmSoft.SME.Common.Resources.IgnoreCase).IsChecked = true;
		LayoutGroupExt layoutGroupExt2 = AddVerticalLayoutGroup(container);
		layoutGroupExt2.HorizontalAlignment = HorizontalAlignment.Right;
		Button button = new Button
		{
			Height = 20.0,
			HorizontalAlignment = HorizontalAlignment.Right,
			Content = ArmSoft.SME.Common.Resources.ClearWord
		};
		button.Click += ClearFilter;
		layoutGroupExt2.Children.Add(button);
		AddLine();
		DocumentGridControl documentGridControl = (DialogGrid = AddGridControl("Columns"));
		DataViewGrid = dvp.Grid;
		documentGridControl.ShowButtons = false;
		documentGridControl.AllowAdd = false;
		documentGridControl.AllowDelete = false;
		documentGridControl.AllowDuplicate = false;
		DocumentGridColumn documentGridColumn = documentGridControl.AddStringColumn("ColCode", ArmSoft.SME.Common.Resources.Column, 8);
		documentGridColumn.FieldName = "ColCode";
		documentGridColumn.Visible = false;
		documentGridColumn.ReadOnly = true;
		documentGridColumn.AllowMoving = DefaultBoolean.False;
		documentGridColumn = documentGridControl.AddStringColumn("ColName", ArmSoft.SME.Common.Resources.Column, 10);
		documentGridColumn.FieldName = "ColName";
		documentGridColumn.Visible = true;
		documentGridColumn.ReadOnly = true;
		documentGridColumn.AllowMoving = DefaultBoolean.False;
		documentGridColumn = documentGridControl.AddStringColumn("Operation", ArmSoft.SME.Common.Resources.Operation, 12);
		documentGridColumn.FieldName = "OperationString";
		ComboBoxEditSettings comboBoxEditSettings = (ComboBoxEditSettings)(documentGridColumn.EditSettings = new ComboBoxEditSettings());
		comboBoxEditSettings.ItemsSource = PrepareOperationsList();
		documentGridControl.View.CellValueChanging += ViewCellValueChanging;
		documentGridControl.View.CellValueChanged += ViewCellValueChanged;
		documentGridColumn.AllowMoving = DefaultBoolean.False;
		documentGridColumn.ReadOnly = false;
		documentGridColumn = documentGridControl.AddStringColumn("ColValue1", ArmSoft.SME.Common.Resources.Value, 10);
		documentGridColumn.FieldName = "ColValue1";
		documentGridColumn.EditSettings = new ComboBoxEditSettings();
		documentGridColumn.AllowMoving = DefaultBoolean.False;
		documentGridColumn = documentGridControl.AddStringColumn("ColValue2", ArmSoft.SME.Common.Resources.Value, 10);
		documentGridColumn.FieldName = "ColValue2";
		documentGridColumn.EditSettings = new ComboBoxEditSettings();
		documentGridColumn.AllowMoving = DefaultBoolean.False;
		documentGridColumn.Visible = false;
		bool containsBetween = false;
		documentGridControl.ItemsSource = SelectByConditionGridSource(dvp.Grid, ref containsBetween, oldCondition);
		if (containsBetween)
		{
			documentGridColumn.Visible = true;
		}
		documentGridControl.View.ShowingEditor += SelectRowsShowEditor;
		MinHeight = 400.0;
		MaxHeight = ((Settings.HostingEnvironment.ActualHeight - 100.0 > 0.0) ? (Settings.HostingEnvironment.ActualHeight - 100.0) : 400.0);
	}

	private void ViewCellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		TableView tableView = (TableView)sender;
		if (!(tableView.Grid.CurrentColumn.Name == "Operation"))
		{
			tableView.CommitEditing();
			tableView.HideEditor();
			e.Handled = true;
		}
	}

	public void SelectRowsShowEditor(object sender, ShowingEditorEventArgs e)
	{
		SelectByConditionGridRow selectByConditionGridRow = (SelectByConditionGridRow)DialogGrid.Grid.GetRow(e.RowHandle);
		string colCode = selectByConditionGridRow.ColCode;
		GridColumn columnBase = DataViewGrid.Columns[colCode];
		ColumnBase currentColumn = ((TableView)sender).Grid.CurrentColumn;
		if (!((currentColumn.Name != "ColValue1") & (currentColumn.Name != "ColValue2")))
		{
			SelectCompareOperations operation = selectByConditionGridRow.Operation;
			if (currentColumn.Name == "ColValue2" && operation != SelectCompareOperations.Between && operation != SelectCompareOperations.NotBetween && Convert.ToString(DialogGrid.Grid.GetCellValue(e.RowHandle, "ColValue2")) == "")
			{
				e.Cancel = true;
			}
			else if (currentColumn.Name == "ColValue1" && ((operation == SelectCompareOperations.ValueIsNullOrEmpty) | (operation == SelectCompareOperations.ValueIsNotNullAndNotEmpty)) && Convert.ToString(DialogGrid.Grid.GetCellValue(e.RowHandle, "ColValue1")) == "")
			{
				e.Cancel = true;
			}
			else if (currentColumn.Name == "ColValue2" && ((operation == SelectCompareOperations.ValueIsNullOrEmpty) | (operation == SelectCompareOperations.ValueIsNotNullAndNotEmpty)) && Convert.ToString(DialogGrid.Grid.GetCellValue(e.RowHandle, "ColValue2")) == "")
			{
				e.Cancel = true;
			}
			else if (currentColumn.EditSettings is ComboBoxEditSettings comboBoxEditSettings)
			{
				List<string> itemsSource = GetUniqueValues(DataViewGrid, columnBase).Values.ToList();
				comboBoxEditSettings.ItemsSource = itemsSource;
			}
		}
	}

	private static List<string> PrepareOperationsList()
	{
		return GetCompareOperations().Keys.ToList();
	}

	public static Dictionary<string, SelectCompareOperations> GetCompareOperations()
	{
		if (_dct != null)
		{
			return _dct;
		}
		_dct = new Dictionary<string, SelectCompareOperations>();
		XtraLocalizer<EditorStringId> active = EditorLocalizer.Active;
		_dct.Add("", SelectCompareOperations.NoOperation);
		_dct.Add("=", SelectCompareOperations.Equal);
		_dct.Add("<>", SelectCompareOperations.NotEqual);
		_dct.Add(">", SelectCompareOperations.Greater);
		_dct.Add(">=", SelectCompareOperations.GreaterE);
		_dct.Add("<", SelectCompareOperations.Less);
		_dct.Add("<=", SelectCompareOperations.LessE);
		_dct.Add(active.GetLocalizedString(EditorStringId.FilterClauseBetween), SelectCompareOperations.Between);
		_dct.Add(active.GetLocalizedString(EditorStringId.FilterClauseNotBetween), SelectCompareOperations.NotBetween);
		_dct.Add(active.GetLocalizedString(EditorStringId.FilterCriteriaToStringFunctionStartsWith), SelectCompareOperations.StartsWith);
		_dct.Add(active.GetLocalizedString(EditorStringId.FilterCriteriaToStringFunctionEndsWith), SelectCompareOperations.EndsWith);
		_dct.Add(active.GetLocalizedString(EditorStringId.FilterClauseLike), SelectCompareOperations.LikeMask);
		_dct.Add(active.GetLocalizedString(EditorStringId.FilterClauseNotLike), SelectCompareOperations.NotLikeMask);
		_dct.Add(active.GetLocalizedString(EditorStringId.FilterClauseAnyOf), SelectCompareOperations.InOperator);
		_dct.Add(active.GetLocalizedString(EditorStringId.FilterClauseNoneOf), SelectCompareOperations.NotIn);
		_dct.Add(active.GetLocalizedString(EditorStringId.FilterClauseIsNullOrEmpty), SelectCompareOperations.ValueIsNullOrEmpty);
		_dct.Add(active.GetLocalizedString(EditorStringId.FilterClauseIsNotNullOrEmpty), SelectCompareOperations.ValueIsNotNullAndNotEmpty);
		return _dct;
	}

	public static SortedDictionary<object, string> GetUniqueValues(GridControl grd, ColumnBase columnBase)
	{
		SortedDictionary<object, string> sortedDictionary = new SortedDictionary<object, string>();
		BaseEditSettings editSettings = columnBase.EditSettings;
		for (int i = 0; i <= grd.VisibleRowCount - 1; i++)
		{
			int rowHandleByVisibleIndex = grd.GetRowHandleByVisibleIndex(i);
			if (rowHandleByVisibleIndex < 0)
			{
				continue;
			}
			object cellValue = grd.GetCellValue(rowHandleByVisibleIndex, columnBase.Name);
			if (cellValue != null && !sortedDictionary.ContainsKey(cellValue))
			{
				if (editSettings != null && editSettings.DisplayFormat != null && editSettings.DisplayFormat != "")
				{
					sortedDictionary.Add(cellValue, string.Format(editSettings.DisplayFormat, cellValue));
				}
				else
				{
					sortedDictionary.Add(cellValue, cellValue.ToString());
				}
			}
		}
		return sortedDictionary;
	}

	private static string ExpressionValue(string formattedValue, DataColumnType colType, bool caseSensitive)
	{
		switch (colType)
		{
		case DataColumnType.Integer:
			if (formattedValue == "EMPTY")
			{
				return "Nothing";
			}
			return Convert.ToString(Convert.ToInt32(formattedValue));
		case DataColumnType.Decimal:
			if (formattedValue == "EMPTY")
			{
				return "Nothing";
			}
			return Convert.ToString(Convert.ToDecimal(formattedValue));
		case DataColumnType.DateTime:
			if (formattedValue == "EMPTY")
			{
				return "Nothing";
			}
			return "#" + Convert.ToDateTime(formattedValue).ToString("d", DateTimeFormatInfo.InvariantInfo) + "#";
		case DataColumnType.DateOnly:
			if (formattedValue == "EMPTY")
			{
				return "Nothing";
			}
			return "#" + Convert.ToDateTime(formattedValue).ToDateOnly().ToString("d", DateTimeFormatInfo.InvariantInfo) + "#";
		case DataColumnType.Boolean:
			if (formattedValue == "EMPTY")
			{
				return "Nothing";
			}
			return Convert.ToString(Convert.ToBoolean(formattedValue));
		default:
			if (formattedValue == "EMPTY")
			{
				return "";
			}
			if (!caseSensitive & (colType == DataColumnType.String))
			{
				formattedValue = formattedValue.ToUpper();
			}
			formattedValue = formattedValue.Replace("'", "''");
			return "'" + Convert.ToString(formattedValue) + "'";
		}
	}

	public static string GetEnglishNameForOperation(SelectCompareOperations op)
	{
		return op switch
		{
			SelectCompareOperations.NoOperation => "", 
			SelectCompareOperations.Equal => "=", 
			SelectCompareOperations.Greater => ">", 
			SelectCompareOperations.GreaterE => ">=", 
			SelectCompareOperations.Less => "<", 
			SelectCompareOperations.LessE => "<=", 
			SelectCompareOperations.NotEqual => "<>", 
			SelectCompareOperations.LikeMask => "LIKE", 
			SelectCompareOperations.NotLikeMask => "NOT LIKE", 
			SelectCompareOperations.ValueIsNullOrEmpty => "IS NULL", 
			SelectCompareOperations.ValueIsNotNullAndNotEmpty => "IS NOT NULL", 
			SelectCompareOperations.Between => "BETWEEN", 
			SelectCompareOperations.NotBetween => "Not between", 
			SelectCompareOperations.StartsWith => "Starts with", 
			SelectCompareOperations.EndsWith => "Ends with", 
			SelectCompareOperations.InOperator => "In", 
			SelectCompareOperations.NotIn => "Not In", 
			_ => "", 
		};
	}

	public CriteriaOperator GenerateFilterString2(ref bool preservePreviousSelection, ref bool caseSensitive)
	{
		CriteriaOperator criteriaOperator = null;
		CriteriaOperator[] array = null;
		int num = -1;
		for (int i = 0; i <= DialogGrid.Grid.VisibleRowCount - 1; i++)
		{
			int rowHandleByVisibleIndex = DialogGrid.Grid.GetRowHandleByVisibleIndex(i);
			if (rowHandleByVisibleIndex < 0)
			{
				continue;
			}
			SelectByConditionGridRow selectByConditionGridRow = (SelectByConditionGridRow)DialogGrid.Grid.GetRow(rowHandleByVisibleIndex);
			string colCode = selectByConditionGridRow.ColCode;
			SelectCompareOperations selectCompareOperations = selectByConditionGridRow.Operation;
			if (selectCompareOperations == SelectCompareOperations.NoOperation)
			{
				continue;
			}
			string text = selectByConditionGridRow.ColValue1;
			string text2 = selectByConditionGridRow.ColValue2;
			if (!caseSensitive & (selectByConditionGridRow.ColumnType == DataColumnType.String))
			{
				text = text.ToUpper();
				text2 = text2.ToUpper();
			}
			if ((selectCompareOperations == SelectCompareOperations.LikeMask) | (selectCompareOperations == SelectCompareOperations.NotLikeMask))
			{
				text = "%" + text.Trim() + "%";
			}
			else
			{
				switch (selectCompareOperations)
				{
				case SelectCompareOperations.StartsWith:
					text = text.Trim() + "%";
					selectCompareOperations = SelectCompareOperations.LikeMask;
					break;
				case SelectCompareOperations.EndsWith:
					text = "%" + text.Trim();
					selectCompareOperations = SelectCompareOperations.LikeMask;
					break;
				}
			}
			string englishNameForOperation = GetEnglishNameForOperation(selectCompareOperations);
			switch (selectCompareOperations)
			{
			case SelectCompareOperations.Between:
			{
				text = ExpressionValue(text.Trim(), selectByConditionGridRow.ColumnType, caseSensitive);
				text2 = ExpressionValue(text2.Trim(), selectByConditionGridRow.ColumnType, caseSensitive);
				string criteria;
				if ((text == "") | (text2 == ""))
				{
					criteria = "";
				}
				else if (selectByConditionGridRow.ColumnType != DataColumnType.String)
				{
					criteria = "([" + colCode + "] >= " + text + " And [" + colCode + "] <= " + text2 + ")";
				}
				else
				{
					criteria = (caseSensitive ? ("(Trim([" + colCode + "]) >= " + text + " And Trim([" + colCode + "]) <= " + text2 + ")") : ("(upper(trim([" + colCode + "])) >= " + text + " And upper(trim([" + colCode + "])) <= " + text2 + ")"));
				}
				criteriaOperator = CriteriaOperator.Parse(criteria);
				break;
			}
			case SelectCompareOperations.NotBetween:
			{
				text = ExpressionValue(text.Trim(), selectByConditionGridRow.ColumnType, caseSensitive);
				text2 = ExpressionValue(text2.Trim(), selectByConditionGridRow.ColumnType, caseSensitive);
				string criteria;
				if ((text == "") | (text2 == ""))
				{
					criteria = "";
				}
				else if (selectByConditionGridRow.ColumnType != DataColumnType.String)
				{
					criteria = "([" + colCode + "] < " + text + " Or [" + colCode + "] > " + text2 + ")";
				}
				else
				{
					criteria = (caseSensitive ? ("(Trim([" + colCode + "]) < " + text + " Or Trim([" + colCode + "]) > " + text2 + ")") : ("(upper(trim([" + colCode + "])) < " + text + " Or upper(trim([" + colCode + "])) > " + text2 + ")"));
				}
				criteriaOperator = CriteriaOperator.Parse(criteria);
				break;
			}
			case SelectCompareOperations.ValueIsNullOrEmpty:
			case SelectCompareOperations.ValueIsNotNullAndNotEmpty:
			{
				text = ExpressionValue("EMPTY", selectByConditionGridRow.ColumnType, caseSensitive);
				string criteria;
				if (text == "Nothing")
				{
					criteria = "([" + colCode + "] " + englishNameForOperation + ")";
				}
				else
				{
					criteria = ((selectCompareOperations != SelectCompareOperations.ValueIsNullOrEmpty) ? ("(Trim([" + colCode + "]) <> '' )") : ("(Trim([" + colCode + "]) = '' )"));
				}
				criteriaOperator = CriteriaOperator.Parse(criteria);
				break;
			}
			case SelectCompareOperations.InOperator:
			case SelectCompareOperations.NotIn:
				text = selectByConditionGridRow.ColValue1;
				if (text != "")
				{
					string[] array2 = text.Split(",");
					string criteria;
					if (selectByConditionGridRow.ColumnType != DataColumnType.String)
					{
						criteria = "([" + colCode + "] IN (";
					}
					else
					{
						criteria = (caseSensitive ? ("(Trim([" + colCode + "]) IN (") : ("(upper(trim([" + colCode + "])) IN ("));
					}
					for (int j = 0; j <= array2.GetUpperBound(0); j++)
					{
						array2[j] = ExpressionValue(array2[j], selectByConditionGridRow.ColumnType, caseSensitive);
						criteria += array2[j];
						criteria = ((j == array2.GetUpperBound(0)) ? (criteria + ")") : (criteria + ","));
					}
					criteria += ")";
					criteriaOperator = ((selectCompareOperations != SelectCompareOperations.InOperator) ? new NotOperator(CriteriaOperator.Parse(criteria)) : CriteriaOperator.Parse(criteria));
				}
				break;
			default:
			{
				text = ExpressionValue(text.Trim(), selectByConditionGridRow.ColumnType, caseSensitive);
				string criteria;
				if (selectByConditionGridRow.ColumnType != DataColumnType.String)
				{
					criteria = "([" + colCode + "] " + englishNameForOperation + " " + text + ")";
				}
				else
				{
					criteria = ((!caseSensitive) ? ("(upper(trim([" + colCode + "])) " + englishNameForOperation + " " + text + ")") : ("(Trim([" + colCode + "]) " + englishNameForOperation + " " + text + ")"));
				}
				criteriaOperator = CriteriaOperator.Parse(criteria);
				break;
			}
			}
			if (!criteriaOperator.ReferenceEqualsNull())
			{
				CriteriaOperator[] array3 = array;
				array = new CriteriaOperator[num + 1 + 1];
				if (array3 != null)
				{
					Array.Copy(array3, array, Math.Min(num + 1 + 1, array3.Length));
				}
				num++;
				array[num] = criteriaOperator;
			}
		}
		if (num > -1)
		{
			CriteriaOperator result = CriteriaOperator.And(array);
			preservePreviousSelection = Convert.ToBoolean(base["PreserveSelection"]);
			caseSensitive = !Convert.ToBoolean(base["CaseInSensitive"]);
			return result;
		}
		return null;
	}

	public List<SelectByConditionGridRow> ConditionsList()
	{
		return (List<SelectByConditionGridRow>)DialogGrid.ItemsSource;
	}

	private void ClearFilter(object sender, RoutedEventArgs e)
	{
		DialogGrid.Grid.BeginDataUpdate();
		for (int i = 0; i <= DialogGrid.Grid.VisibleRowCount - 1; i++)
		{
			int rowHandleByVisibleIndex = DialogGrid.Grid.GetRowHandleByVisibleIndex(i);
			if (rowHandleByVisibleIndex >= 0)
			{
				DialogGrid.Grid.SetCellValue(rowHandleByVisibleIndex, "OperationString", "");
				DialogGrid.Grid.SetCellValue(rowHandleByVisibleIndex, "ColValue1", "");
				DialogGrid.Grid.SetCellValue(rowHandleByVisibleIndex, "ColValue2", "");
			}
		}
		DialogGrid.Grid.EndDataUpdate();
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		base.DialogValidation(args);
		try
		{
			args.isValid = !DialogGrid.CalculateHasValidationError();
		}
		catch (Exception)
		{
			args.isValid = false;
		}
	}

	private void ViewCellValueChanging(object sender, CellValueChangedEventArgs e)
	{
		TableView tableView = (TableView)sender;
		ColumnBase currentColumn = tableView.Grid.CurrentColumn;
		XtraLocalizer<EditorStringId> active = EditorLocalizer.Active;
		string localizedString = active.GetLocalizedString(EditorStringId.FilterClauseBetween);
		string localizedString2 = active.GetLocalizedString(EditorStringId.FilterClauseNotBetween);
		if (currentColumn.Name != "Operation")
		{
			return;
		}
		if ((e.Value ?? "").ToString() == localizedString || (e.Value ?? "").ToString() == localizedString2)
		{
			tableView.Grid.Columns["ColValue2"].Visible = true;
			return;
		}
		bool flag = false;
		for (int i = 0; i <= DialogGrid.Grid.VisibleRowCount - 1; i++)
		{
			int rowHandleByVisibleIndex = DialogGrid.Grid.GetRowHandleByVisibleIndex(i);
			if (rowHandleByVisibleIndex >= 0)
			{
				string text = Convert.ToString(DialogGrid.Grid.GetCellValue(rowHandleByVisibleIndex, "OperationString"));
				if ((((text == localizedString) | (text == localizedString2)) && e.RowHandle != rowHandleByVisibleIndex) || (e.RowHandle == rowHandleByVisibleIndex && (((e.Value ?? "").ToString() == localizedString) | ((e.Value ?? "").ToString() == localizedString2))))
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			tableView.Grid.Columns["ColValue2"].Visible = false;
		}
	}
}
