using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Data;
using ArmSoft.SME.Common.Models.Data;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.DataViews;

public class GroupByDataView : DataView<ExtendableRow, object>
{
	private bool _mIsDialogSupported;

	private ExtendableRow.RowSchema _rowSchema;

	private readonly DataViewPanel _dvp;

	private readonly BindingList<GroupingColumn> _groupingInfo;

	private readonly bool _withQty;

	private readonly List<ExtendableRow> _extendableRows;

	private readonly Dictionary<string, List<int>> _dctKeyValues;

	public override string Caption => _dvp.Caption.ToString();

	public override bool RefreshSupported => false;

	public override string Name => _dvp.Name + "GRP";

	public override bool DialogSupported
	{
		get
		{
			_mIsDialogSupported = false;
			return _mIsDialogSupported;
		}
	}

	public GroupByDataView(DataViewPanel sourceDVP, BindingList<GroupingColumn> colGroupingInfo, bool showQty)
	{
		_dvp = sourceDVP;
		_groupingInfo = colGroupingInfo;
		_withQty = showQty;
		CreateSchemaColumns();
		_extendableRows = new List<ExtendableRow>();
		_dctKeyValues = new Dictionary<string, List<int>>();
		CalcGroupedRows();
		DataProvider = new GroupByColumnsDataProvider(_rowSchema, _extendableRows);
	}

	private void CreateSchemaColumns()
	{
		List<Column> list = new List<Column>();
		foreach (GroupingColumn item in _groupingInfo)
		{
			bool visible = ((MyGridColumn)(_dvp.Grid.Columns[item.Name] ?? throw new ASException(string.Format(Resources.ColumnNotFound, item.Caption)))).Visible;
			for (int i = 0; i <= 4; i++)
			{
				switch (i)
				{
				case 0:
					if (item.GroupBy)
					{
						ViewColumn viewColumn3 = Columns.AddDynamic("GBY_" + item.Name, item.Caption, item.Name, item.TypeGroup);
						viewColumn3.Visible = visible;
						viewColumn3.Source = viewColumn3.Name;
						list.Add(new Column
						{
							Caption = viewColumn3.Caption,
							Type = viewColumn3.Type.CastToDataColumnType(),
							SourceType = ColumnSourceType.Dynamic,
							Name = viewColumn3.Name,
							FooterSum = viewColumn3.FooterSum
						});
					}
					break;
				case 1:
					if (item.CalcSum)
					{
						ViewColumn viewColumn4 = Columns.AddDynamic("SUM_" + item.Name, "(SUM)" + item.Caption, item.Name, item.TypeGroup);
						viewColumn4.Visible = visible;
						viewColumn4.Source = viewColumn4.Name;
						viewColumn4.FooterSum = true;
						list.Add(new Column
						{
							Caption = viewColumn4.Caption,
							Type = viewColumn4.Type.CastToDataColumnType(),
							SourceType = ColumnSourceType.Dynamic,
							Name = viewColumn4.Name,
							FooterSum = viewColumn4.FooterSum
						});
					}
					break;
				case 2:
					if (item.CalcAvg)
					{
						ViewColumn viewColumn2 = Columns.AddDynamic("AVG_" + item.Name, "(AVG)" + item.Caption, item.Name, item.TypeGroup);
						viewColumn2.Visible = visible;
						viewColumn2.Source = viewColumn2.Name;
						viewColumn2.FooterSum = true;
						list.Add(new Column
						{
							Caption = viewColumn2.Caption,
							Type = viewColumn2.Type.CastToDataColumnType(),
							SourceType = ColumnSourceType.Dynamic,
							Name = viewColumn2.Name,
							FooterSum = viewColumn2.FooterSum
						});
					}
					break;
				case 3:
					if (item.CalcMax)
					{
						ViewColumn viewColumn5 = Columns.AddDynamic("MAX_" + item.Name, "(MAX)" + item.Caption, item.Name, item.TypeGroup);
						viewColumn5.Visible = visible;
						viewColumn5.Source = viewColumn5.Name;
						if (item.TypeGroup == DataColumnType.Decimal || item.TypeGroup == DataColumnType.Integer)
						{
							viewColumn5.FooterSum = true;
							list.Add(new Column
							{
								Caption = viewColumn5.Caption,
								Type = viewColumn5.Type.CastToDataColumnType(),
								SourceType = ColumnSourceType.Dynamic,
								Name = viewColumn5.Name,
								FooterSum = viewColumn5.FooterSum
							});
						}
					}
					break;
				case 4:
					if (item.CalcMin)
					{
						ViewColumn viewColumn = Columns.AddDynamic("MIN_" + item.Name, "(MIN)" + item.Caption, item.Name, item.TypeGroup);
						viewColumn.Visible = visible;
						viewColumn.Source = viewColumn.Name;
						if (item.TypeGroup == DataColumnType.Decimal || item.TypeGroup == DataColumnType.Integer)
						{
							viewColumn.FooterSum = true;
							list.Add(new Column
							{
								Caption = viewColumn.Caption,
								Type = viewColumn.Type.CastToDataColumnType(),
								SourceType = ColumnSourceType.Dynamic,
								Name = viewColumn.Name,
								FooterSum = viewColumn.FooterSum
							});
						}
					}
					break;
				}
			}
		}
		if (_withQty)
		{
			ViewColumn viewColumn6 = Columns.AddDynamic("_GroupQuantity", Resources.Quantity, "_GroupQuantity", DataColumnType.Integer);
			viewColumn6.Visible = true;
			viewColumn6.FooterSum = true;
			list.Add(new Column
			{
				Caption = viewColumn6.Caption,
				Type = viewColumn6.Type.CastToDataColumnType(),
				SourceType = ColumnSourceType.Dynamic,
				Name = viewColumn6.Name,
				FooterSum = viewColumn6.FooterSum
			});
		}
		_rowSchema = new ExtendableRow.RowSchema(typeof(ExtendableRow), list, null);
	}

	private void CalcGroupedRows()
	{
		List<object> list = new List<object>();
		Dictionary<int, string> colDetailsId = new Dictionary<int, string>();
		for (int i = 0; i <= _dvp.Grid.VisibleRowCount - 1; i++)
		{
			int rowHandleByVisibleIndex = _dvp.Grid.GetRowHandleByVisibleIndex(i);
			if (rowHandleByVisibleIndex >= 0)
			{
				list.Add(rowHandleByVisibleIndex);
				string text = CalculateDetailsId(rowHandleByVisibleIndex);
				colDetailsId.Add(rowHandleByVisibleIndex, text);
				if (!_dctKeyValues.TryGetValue(text, out var value))
				{
					value = new List<int>();
					_dctKeyValues.Add(text, value);
				}
				value.Add(rowHandleByVisibleIndex);
			}
		}
		IGrouping<string, object>[] array = (from a in list
			group a by colDetailsId[(int)a]).ToArray();
		foreach (IGrouping<string, object> r in array)
		{
			FillGroupRows(r);
		}
	}

	public void FillGroupRows(IGrouping<string, object> r)
	{
		ExtendableRow extendableRow = new ExtendableRow(_rowSchema);
		foreach (ExtendableRow.Column gCol in _rowSchema.DynamicColumns.Values)
		{
			object[] array = r.ToArray();
			if (gCol.Name == "_GroupQuantity")
			{
				if (_dctKeyValues.ContainsKey(r.Key))
				{
					extendableRow["_GroupQuantity"] = array.Length;
				}
				else
				{
					extendableRow["_GroupQuantity"] = 0;
				}
			}
			else if (gCol.Name == "DetailsID")
			{
				extendableRow["DetailsID"] = r.Key;
			}
			else if (gCol.Name == "Selected")
			{
				extendableRow["Selected"] = false;
			}
			else
			{
				if (!(gCol.Name != "Details"))
				{
					continue;
				}
				if (gCol.Name.StartsWith("GBY_"))
				{
					extendableRow[gCol.Name] = _dvp.Grid.GetCellValue(Convert.ToInt32(array[0]), gCol.Name.Substring(4));
				}
				else if (gCol.Name.StartsWith("SUM_"))
				{
					extendableRow[gCol.Name] = r.Sum((object f) => Convert.ToDecimal(_dvp.Grid.GetCellValue(Convert.ToInt32(f), gCol.Name.Substring(4))));
				}
				else if (gCol.Name.StartsWith("AVG_"))
				{
					extendableRow[gCol.Name] = r.Average((object f) => Convert.ToDecimal(_dvp.Grid.GetCellValue(Convert.ToInt32(f), gCol.Name.Substring(4))));
				}
				else if (gCol.Name.StartsWith("MAX_"))
				{
					extendableRow[gCol.Name] = r.Max((object f) => _dvp.Grid.GetCellValue(Convert.ToInt32(f), gCol.Name.Substring(4)));
				}
				else if (gCol.Name.StartsWith("MIN_"))
				{
					extendableRow[gCol.Name] = r.Min((object f) => _dvp.Grid.GetCellValue(Convert.ToInt32(f), gCol.Name.Substring(4)));
				}
			}
		}
		_extendableRows.Add(extendableRow);
	}

	private string CalculateDetailsId(int rowHandle)
	{
		object obj = 1;
		for (int i = 0; i <= _groupingInfo.Count - 1; i++)
		{
			if (_groupingInfo[i].GroupBy)
			{
				GridColumn gridColumn = _dvp.Grid.Columns[_groupingInfo[i].Name];
				if (obj == null)
				{
					obj = Tuple.Create(_dvp.Grid.GetCellValue(rowHandle, gridColumn.Name));
				}
				obj = Tuple.Create(obj, _dvp.Grid.GetCellValue(rowHandle, gridColumn.Name));
			}
		}
		return obj.ToString();
	}
}
