using System;
using System.Collections.Generic;
using System.Windows;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.Data;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.DataViews;

public abstract class DropDownView<R, P> : View<R, P>, IDropDownView, IView where R : ExtendableRow, new() where P : class, new()
{
	private short? _dropDownType;

	private string _dataViewGroup;

	public override bool WithoutAccessCheck => true;

	public short DropDownType
	{
		get
		{
			if (!_dropDownType.HasValue)
			{
				DefinitionMappingsService.IsDropDown(GetType(), out var dropDownType);
				_dropDownType = dropDownType;
			}
			return _dropDownType.Value;
		}
	}

	public override string DataViewGroup
	{
		get
		{
			if (_dataViewGroup == null)
			{
				_dataViewGroup = ((DropDownType != 0) ? SmartEnumHelper.GetName(typeof(DropDownTypeSE), DropDownType) : base.DataViewGroup);
			}
			return _dataViewGroup;
		}
	}

	public override string Name => DataViewGroup;

	public virtual bool AddNewSupported => false;

	public virtual bool EditSupported => false;

	public virtual bool FilteredSearchSupported => ClosedFilterSupported;

	public virtual bool AddNewEnabled => AddNewSupported;

	public virtual bool EditEnabled => EditSupported;

	public virtual bool FilteredSearchEnabled => FilteredSearchSupported;

	public virtual bool KeyMemberSupported => false;

	public virtual bool DialogSupported => false;

	public virtual ReloadMode ReloadMode => ReloadMode.None;

	public List<AsFormatting> ConditionalFormattingList { get; }

	public virtual string ValueDoesNotExistInTheListMessage => Resources.ValueDoesNotExistInTheList;

	public virtual bool ClosedFilterSupported => false;

	public virtual string ClosedColumnName
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public DropDownView()
	{
		ConditionalFormattingList = new List<AsFormatting>();
		AddConditionalFormatting();
	}

	public virtual bool AddNew(ref string value, ref string comment, Window owner, double top, double left)
	{
		throw new NotImplementedException();
	}

	public virtual bool Edit(ref string value, ref string comment, Window owner, double top, double left, object selectedRow)
	{
		throw new NotImplementedException();
	}

	public virtual bool FilteredSearch(ref string value, ref string comment, Window owner, double top, double left)
	{
		throw new NotImplementedException();
	}

	public bool ShowDialog()
	{
		if (DialogSupported)
		{
			return CreateDialog().ShowDialog();
		}
		return false;
	}

	public virtual DropDownViewParametersDialogWindow CreateDialog()
	{
		return null;
	}

	public virtual bool Validate(string value, ref string comment, ref string realValue)
	{
		value = value.TrimEnd();
		if (!DataLoadedAtLeastOnce)
		{
			LoadData();
		}
		for (int i = 0; i <= Data.Count - 1; i++)
		{
			string text = ValueMember(i);
			if (text?.TrimEnd() == value)
			{
				comment = CommentMember(i);
				realValue = text;
				return true;
			}
		}
		return false;
	}

	object IDropDownView.GetRowByValue(string value)
	{
		return GetRowByValue(value);
	}

	public virtual R GetRowByValue(string value)
	{
		value = value.TrimEnd();
		if (!DataLoadedAtLeastOnce)
		{
			LoadData();
		}
		int num = Data.Count - 1;
		for (int i = 0; i <= num; i++)
		{
			if (ValueMember(i).TrimEnd() == value)
			{
				return Data[i];
			}
		}
		return null;
	}

	public virtual List<string> ValuesList()
	{
		List<string> list = new List<string>();
		if (!DataLoadedAtLeastOnce)
		{
			LoadData();
		}
		int num = Data.Count - 1;
		for (int i = 0; i <= num; i++)
		{
			list.Add(ValueMember(i));
		}
		return list;
	}

	public virtual string ValueMember(int index)
	{
		throw new NotImplementedException();
	}

	public virtual short ValueMemberLength()
	{
		throw new NotImplementedException();
	}

	public virtual string CommentMember(int index)
	{
		throw new NotImplementedException();
	}

	public virtual object KeyMember(int index)
	{
		throw new NotImplementedException();
	}

	public void ReloadRows(LoadSource source = LoadSource.None)
	{
		DataLoadedAtLeastOnce = false;
		LoadData(source);
		DataLoadedAtLeastOnce = true;
	}

	public virtual void BeforeLoadData(GridControl grid)
	{
		if (Columns.SortOrder().Count <= 0)
		{
			return;
		}
		foreach (ViewColumnSorting item in Columns.SortOrder())
		{
			SetColumnSortOrder(grid, item.Column.Name, (!Convert.ToBoolean(item.Descending)) ? ColumnSortOrder.Ascending : ColumnSortOrder.Descending);
		}
	}

	public virtual void AfterLoadData(GridControl grid)
	{
	}

	protected void SetColumnSortOrder(GridControl grid, string columnName, ColumnSortOrder sortOrder)
	{
		foreach (GridColumn column in grid.Columns)
		{
			if (column.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
			{
				column.SortOrder = sortOrder;
				break;
			}
		}
	}

	public void ApplyDefaultDataViewDefinition()
	{
		throw new NotImplementedException();
	}

	public void ApplyDataViewDefinition(DataViewDefinition dvd)
	{
		throw new NotImplementedException();
	}

	public virtual ExtendableRow GetSelectedRow(int rowIndex)
	{
		return Data[rowIndex];
	}

	protected void AddConditionalFormatting()
	{
		if (ClosedFilterSupported)
		{
			ConditionalFormattingList.Add(new AsCondititionFormat
			{
				Expression = "[" + ClosedColumnName + "]",
				ApplyToRow = true,
				FormatName = "Custom",
				CustomFormat = new UserDefinedFormat
				{
					ForegroundColor = "#B3B3B3"
				},
				IsFixed = true
			});
		}
	}
}
