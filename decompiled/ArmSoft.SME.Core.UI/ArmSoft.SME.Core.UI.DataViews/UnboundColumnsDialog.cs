using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.UnboundColumns;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using DevExpress.Mvvm.Native;
using DevExpress.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Core.UI.DataViews;

public class UnboundColumnsDialog : DialogWindow
{
	private readonly DataViewPanel _dvp;

	private DocumentGridControl _unboundColumnsGrid;

	private readonly List<MyGridColumn> _dvpUnboundColumns;

	private readonly ObservableCollection<UnboundColumn> _unboundColumns;

	private readonly List<UnboundColumn> _oldUnboundColumns;

	private ButtonEditExt _caption;

	private Button _expressionButton;

	private SpinEditExt _precision;

	private ComboBoxEditExt _columnType;

	private bool _save;

	private bool _focusedRowChanged;

	public UnboundColumnsDialog(DataViewPanel dvp)
	{
		_dvp = dvp;
		_unboundColumns = Settings.UnboundColumnsService.Get(_dvp.IDV.DataViewGroup).ToObservableCollection();
		_oldUnboundColumns = _unboundColumns.ToList();
		_dvpUnboundColumns = dvp.Grid.Columns.Where((GridColumn p) => _unboundColumns.Any((UnboundColumn c) => c.Name == p.Name)).Cast<MyGridColumn>().ToList();
		ConstructUI();
	}

	public void ConstructUI()
	{
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		MinHeight = 400.0;
		Caption = ArmSoft.SME.Common.Resources.WorkWithUnboundColumns;
		Closed += new SafeEventHandler<EventArgs>(UnboundColumnsDialog_Closed).Handler;
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		LayoutGroupExt container2 = AddVerticalLayoutGroup(container);
		_unboundColumnsGrid = AddGridControl(container2, "Columns");
		_unboundColumnsGrid.Width = 336.0;
		_unboundColumnsGrid.AllowDuplicate = false;
		_unboundColumnsGrid.IsVisibleDuplicateButton = false;
		_unboundColumnsGrid.AddStringColumn("Caption", ArmSoft.SME.Common.Resources.Name, 50).AllowEditing = DefaultBoolean.False;
		_unboundColumnsGrid.ItemsSource = _unboundColumns;
		_unboundColumnsGrid.View.BestFitColumns();
		_unboundColumnsGrid.View.AllowResizing = false;
		_unboundColumnsGrid.View.FocusedRowHandleChanged += new SafeEventHandler<FocusedRowHandleChangedEventArgs>(View_FocusedRowHandleChanged).Handler;
		_unboundColumnsGrid.AddingRow += new SafeEventHandler<DocumentGridAddingRowEventArgs>(UnboundColumnsGrid_AddingRow).Handler;
		_unboundColumns.CollectionChanged += new SafeEventHandler<NotifyCollectionChangedEventArgs>(UnboundColumns_CollectionChanged).Handler;
		LayoutGroupExt layoutGroupExt = AddVerticalLayoutGroup(container, LayoutGroupView.Group, ArmSoft.SME.Common.Resources.Column);
		layoutGroupExt.Margin = new Thickness(24.0, 24.0, 0.0, 0.0);
		layoutGroupExt.VerticalAlignment = VerticalAlignment.Stretch;
		_caption = AddButtonEditControl(layoutGroupExt, "Caption", ArmSoft.SME.Common.Resources.Name, 50, isRequired: true, 1, storeValue: false, "", LayoutItemLabelPosition.Top);
		_caption.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Caption_EditValueChanged).Handler;
		DXImage content = new DXImage
		{
			Source = ImageService.GetImage("language_14.svg"),
			Width = 16.0,
			Height = 16.0,
			Margin = new Thickness(2.0, 0.0, 2.0, 0.0),
			ToolTip = ArmSoft.SME.Common.Resources.Names
		};
		ButtonInfo buttonInfo = new ButtonInfo
		{
			GlyphKind = GlyphKind.Custom,
			Content = content
		};
		_caption.Buttons.Add(buttonInfo);
		buttonInfo.Click += new SafeEventHandler<RoutedEventArgs>(ButtonInfo_Click).Handler;
		_columnType = AddComboBoxControl(layoutGroupExt, "Type", ArmSoft.SME.Common.Resources.Type, 50, SmartEnum<UnboundColumnTypeSE, short>.List.OrderBy((UnboundColumnTypeSE p) => p.Order).ToList(), "Caption", "Value", isRequired: true, storeValue: false, HorizontalAlignment.Left, "", LayoutItemLabelPosition.Top);
		_columnType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ColumnType_EditValueChanged).Handler;
		_precision = AddSpinEdit(layoutGroupExt, "Precision", ArmSoft.SME.Common.Resources.Accuracy, 0m, 5m, 50.0, storeValue: false, "", typeof(short), LayoutItemLabelPosition.Top);
		_precision.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Precision_EditValueChanged).Handler;
		_precision.IsTextEditable = false;
		AddLine(layoutGroupExt);
		_expressionButton = AddButton(layoutGroupExt, "Expression", "functions_16.svg", ArmSoft.SME.Common.Resources.Formula);
		_expressionButton.Click += new SafeEventHandler<RoutedEventArgs>(ExpressionButton_Click).Handler;
		FieldsAreReadOnly();
		FocusRow();
	}

	private void ButtonInfo_Click(object sender, RoutedEventArgs e)
	{
		int focusedRowHandle = _unboundColumnsGrid.View.FocusedRowHandle;
		UnboundColumn unboundColumn = (UnboundColumn)_unboundColumnsGrid.Grid.GetRow(focusedRowHandle);
		if (unboundColumn != null)
		{
			DialogWindow dialogWindow = new DialogWindow(this)
			{
				Caption = ArmSoft.SME.Common.Resources.Names
			};
			LayoutGroupExt container = dialogWindow.AddVerticalLayoutGroup();
			TextEditExt textEditExt = dialogWindow.AddStringControl(container, "CaptionEn", ArmSoft.SME.Common.Resources.NameEn, 50, isRequired: false, storeValue: false, "", HorizontalAlignment.Left, LayoutItemLabelPosition.Top);
			textEditExt.EditValue = unboundColumn.CaptionEn;
			TextEditExt textEditExt2 = dialogWindow.AddStringControl(container, "CaptionRu", ArmSoft.SME.Common.Resources.NameRU, 50, isRequired: false, storeValue: false, "", HorizontalAlignment.Left, LayoutItemLabelPosition.Top);
			textEditExt2.EditValue = unboundColumn.CaptionRu;
			if (dialogWindow.ShowDialog().Value)
			{
				unboundColumn.CaptionEn = textEditExt.EditValue?.ToString() ?? "";
				unboundColumn.CaptionRu = textEditExt2.EditValue?.ToString() ?? "";
			}
			_unboundColumnsGrid.Grid.RefreshRow(focusedRowHandle);
		}
	}

	private void UnboundColumns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		FieldsAreReadOnly();
	}

	private void FieldsAreReadOnly()
	{
		bool flag = _unboundColumns.Count == 0;
		_caption.IsReadOnly = flag;
		_columnType.IsReadOnly = flag;
		_precision.IsReadOnly = flag;
		_expressionButton.IsEnabled = !flag;
		if (flag)
		{
			_caption.EditValue = "";
			_precision.EditValue = (short)0;
		}
	}

	private void ColumnType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_focusedRowChanged || _columnType.EditValue == null)
		{
			return;
		}
		int focusedRowHandle = _unboundColumnsGrid.View.FocusedRowHandle;
		UnboundColumn unboundColumn = (UnboundColumn)_unboundColumnsGrid.Grid.GetRow(focusedRowHandle);
		if (unboundColumn != null)
		{
			short num = (short)_columnType.EditValue;
			if (num == (short)UnboundColumnTypeSE.Decimal)
			{
				_precision.IsReadOnly = false;
			}
			else
			{
				_precision.EditValue = (short)0;
				_precision.IsReadOnly = true;
			}
			unboundColumn.Type = num;
		}
		_unboundColumnsGrid.Grid.RefreshRow(focusedRowHandle);
	}

	private void Precision_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_focusedRowChanged)
		{
			int focusedRowHandle = _unboundColumnsGrid.View.FocusedRowHandle;
			UnboundColumn unboundColumn = (UnboundColumn)_unboundColumnsGrid.Grid.GetRow(focusedRowHandle);
			if (unboundColumn != null)
			{
				unboundColumn.Precision = (short)_precision.EditValue;
			}
			_unboundColumnsGrid.Grid.RefreshRow(focusedRowHandle);
		}
	}

	private void Caption_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_focusedRowChanged)
		{
			int focusedRowHandle = _unboundColumnsGrid.View.FocusedRowHandle;
			UnboundColumn unboundColumn = (UnboundColumn)_unboundColumnsGrid.Grid.GetRow(focusedRowHandle);
			if (unboundColumn != null)
			{
				unboundColumn.Caption = _caption.EditValue?.ToString() ?? "";
			}
			_unboundColumnsGrid.Grid.RefreshRow(focusedRowHandle);
		}
	}

	private void UnboundColumnsDialog_Closed(object sender, EventArgs e)
	{
		if (_save)
		{
			return;
		}
		int i;
		for (i = _dvp.Grid.Columns.Count - 1; i >= 0; i--)
		{
			if (_unboundColumns.Any((UnboundColumn p) => p.Name == _dvp.Grid.Columns[i].Name) && !_oldUnboundColumns.Any((UnboundColumn p) => p.Name == _dvp.Grid.Columns[i].Name))
			{
				_dvp.Grid.Columns.RemoveAt(i);
			}
		}
	}

	private void ExpressionButton_Click(object sender, RoutedEventArgs e)
	{
		if (_columnType.EditValue == null)
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.Type));
		}
		int focusedRowHandle = _unboundColumnsGrid.View.FocusedRowHandle;
		UnboundColumn focusedColumn = (UnboundColumn)_unboundColumnsGrid.Grid.GetRow(focusedRowHandle);
		if (focusedColumn != null)
		{
			MyGridColumn myGridColumn = _dvpUnboundColumns.FirstOrDefault((MyGridColumn p) => p.Name == focusedColumn.Name);
			if (myGridColumn == null)
			{
				myGridColumn = _dvp.PopulateColumn(focusedColumn);
				_dvpUnboundColumns.Add(myGridColumn);
			}
			myGridColumn.Tag = true;
			_dvp.GridView.ShowUnboundExpressionEditor(myGridColumn);
			myGridColumn.Tag = null;
			focusedColumn.Expression = myGridColumn.UnboundExpression;
			_unboundColumnsGrid.Grid.RefreshRow(focusedRowHandle);
		}
	}

	private void UnboundColumnsGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		string name = "UnboundColumn_" + Settings.CommonCoreHelper.GetRandomString(6);
		string caption = $"{ArmSoft.SME.Common.Resources.UnboundColumn} {_unboundColumns.Count + 1}";
		UnboundColumn unboundColumn = new UnboundColumn
		{
			Name = name,
			Caption = caption,
			CaptionEn = "",
			CaptionRu = "",
			Group = _dvp.IDV.DataViewGroup,
			Type = UnboundColumnTypeSE.String.Value,
			Expression = ""
		};
		_unboundColumns.Add(unboundColumn);
		_unboundColumnsGrid.Grid.CurrentItem = unboundColumn;
		_unboundColumnsGrid.Grid.SelectedItem = null;
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		base.DialogValidation(args);
		_save = true;
		List<UnboundColumn> list = _oldUnboundColumns.Except(_unboundColumns).ToList();
		short num = 0;
		int num2 = _dvp.Grid.Columns.Count + 1;
		foreach (UnboundColumn unboundColumn in _unboundColumns)
		{
			if (string.IsNullOrWhiteSpace(unboundColumn.Caption))
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.Name));
			}
			if (string.IsNullOrWhiteSpace(unboundColumn.Expression))
			{
				throw new ASException(unboundColumn.Caption + Environment.NewLine + string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.Formula));
			}
			GridColumn columnByName = _dvp.Grid.Columns.GetColumnByName(unboundColumn.Name);
			if (columnByName != null)
			{
				columnByName.VisibleIndex = num2;
				((MyGridColumn)columnByName).ColumnVisibleIndex = num2;
				string text = unboundColumn.Caption;
				if (LanguageService.IsEnglish)
				{
					text = unboundColumn.CaptionEn;
				}
				else if (LanguageService.IsRussian)
				{
					text = unboundColumn.CaptionRu;
				}
				if (string.IsNullOrWhiteSpace(text))
				{
					text = unboundColumn.Caption;
				}
				if (columnByName.Header.ToString() != text)
				{
					columnByName.Header = text;
				}
				if (unboundColumn.Type == (short)UnboundColumnTypeSE.Decimal)
				{
					columnByName.EditSettings.DisplayFormat = DecimalExtension.GetCurrencyFormat(16, unboundColumn.Precision);
				}
			}
			unboundColumn.RowNum = num;
			Settings.UnboundColumnsService.Store(unboundColumn);
			num++;
			num2++;
		}
		_dvp.Grid.Columns.BeginUpdate();
		int i;
		for (i = _dvp.Grid.Columns.Count - 1; i >= 0; i--)
		{
			if (!_unboundColumns.Any((UnboundColumn p) => p.Name == _dvp.Grid.Columns[i].Name) && _oldUnboundColumns.Any((UnboundColumn p) => p.Name == _dvp.Grid.Columns[i].Name))
			{
				_dvp.Grid.Columns.RemoveAt(i);
			}
			else if (_unboundColumns.Any((UnboundColumn p) => p.Name == _dvp.Grid.Columns[i].Name) && !_dvp.Grid.Columns[i].Visible)
			{
				_dvp.Grid.Columns[i].Visible = true;
			}
		}
		_dvp.Grid.Columns.EndUpdate();
		foreach (UnboundColumn item in list)
		{
			Settings.UnboundColumnsService.Delete(item);
		}
		_dvp.CollectUnboundExpressions();
	}

	private void View_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		FocusRow();
	}

	private void FocusRow()
	{
		int focusedRowHandle = _unboundColumnsGrid.View.FocusedRowHandle;
		UnboundColumn unboundColumn = (UnboundColumn)_unboundColumnsGrid.Grid.GetRow(focusedRowHandle);
		if (unboundColumn != null)
		{
			_focusedRowChanged = true;
			_caption.EditValue = unboundColumn.Caption;
			_columnType.SelectedItem = SmartEnum<UnboundColumnTypeSE, short>.FromValue(unboundColumn.Type);
			_columnType.IsReadOnly = unboundColumn.IsStored;
			_precision.EditValue = unboundColumn.Precision;
			_precision.IsReadOnly = unboundColumn.Type != (short)UnboundColumnTypeSE.Decimal;
			_focusedRowChanged = false;
		}
	}
}
