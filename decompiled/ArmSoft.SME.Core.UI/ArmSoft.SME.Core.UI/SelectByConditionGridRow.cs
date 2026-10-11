using System;
using System.Collections.Generic;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI.Controls;
using DevExpress.XtraEditors.DXErrorProvider;

namespace ArmSoft.SME.Core.UI;

public class SelectByConditionGridRow : DocumentGridRow
{
	public string ColName { get; set; }

	public string ColCode { get; set; }

	public string OperationString
	{
		get
		{
			Dictionary<string, SelectCompareOperations> compareOperations = GridRowSelectionWnd.GetCompareOperations();
			foreach (string key in compareOperations.Keys)
			{
				if (compareOperations[key] == Operation)
				{
					return key;
				}
			}
			return "";
		}
		set
		{
			Dictionary<string, SelectCompareOperations> compareOperations = GridRowSelectionWnd.GetCompareOperations();
			if (value == "" || !compareOperations.ContainsKey(value.Trim()))
			{
				Operation = SelectCompareOperations.NoOperation;
			}
			else
			{
				Operation = compareOperations[value.Trim()];
			}
		}
	}

	public SelectCompareOperations Operation { get; set; }

	public string ColValue1 { get; set; }

	public string ColValue2 { get; set; }

	public DataColumnType ColumnType { get; set; }

	public SelectByConditionGridRow(string colName, string colCode, string operation, string colValue1, string colValue2, DataColumnType dvColumnType)
	{
		ColName = colName;
		ColCode = colCode;
		Dictionary<string, SelectCompareOperations> compareOperations = GridRowSelectionWnd.GetCompareOperations();
		if (operation.Trim() == "" || !compareOperations.ContainsKey(operation.Trim()))
		{
			Operation = SelectCompareOperations.NoOperation;
		}
		else
		{
			Operation = compareOperations[operation.Trim()];
		}
		OperationString = operation;
		ColValue1 = colValue1;
		ColValue2 = colValue2;
		ColumnType = dvColumnType;
	}

	protected override void OnValidate(ErrorInfo info)
	{
		base.OnValidate(info);
		if (Operation == SelectCompareOperations.NoOperation)
		{
			ColValue1 = "";
			ColValue2 = "";
		}
		else
		{
			if (Operation == SelectCompareOperations.NoOperation)
			{
				return;
			}
			if ((Operation == SelectCompareOperations.Between || Operation == SelectCompareOperations.NotBetween) && (ColValue1 == "" || ColValue2 == ""))
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = Resources.CompareOpInvalidCondition;
				return;
			}
			if (Operation != SelectCompareOperations.Between && Operation != SelectCompareOperations.NotBetween && ColValue2 != "")
			{
				ColValue2 = "";
				return;
			}
			if ((Operation == SelectCompareOperations.ValueIsNotNullAndNotEmpty) | (Operation == SelectCompareOperations.ValueIsNullOrEmpty))
			{
				ColValue1 = "";
				ColValue2 = "";
			}
			else if (Operation != SelectCompareOperations.Between && Operation != SelectCompareOperations.NotBetween && ((ColValue2 != "") | (ColValue1 == "")))
			{
				ColValue2 = "";
			}
			if ((Operation != SelectCompareOperations.ValueIsNotNullAndNotEmpty && Operation != SelectCompareOperations.ValueIsNullOrEmpty) & (ColValue1 == ""))
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = Resources.CompareOpInvalidCondition;
				return;
			}
			if (((Operation == SelectCompareOperations.LikeMask) | (Operation == SelectCompareOperations.NotLikeMask) | (Operation == SelectCompareOperations.StartsWith) | (Operation == SelectCompareOperations.EndsWith)) && ColumnType != DataColumnType.String)
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = Resources.CompareOpInvalidCondition;
				return;
			}
			string[] array;
			if (ColValue1 != "")
			{
				array = ColValue1.Split(",");
				foreach (string value in array)
				{
					try
					{
						if (ColumnType == DataColumnType.Integer)
						{
							Convert.ToInt32(value);
						}
						else if (ColumnType == DataColumnType.Decimal)
						{
							Convert.ToDecimal(value);
						}
						else if (ColumnType == DataColumnType.DateTime)
						{
							Convert.ToDateTime(value);
						}
						else if (ColumnType == DataColumnType.DateOnly)
						{
							Convert.ToDateTime(value).ToDateOnly();
						}
						else if (ColumnType == DataColumnType.Boolean)
						{
							Convert.ToBoolean(value);
						}
					}
					catch (Exception)
					{
						info.ErrorType = ErrorType.Critical;
						info.ErrorText = Resources.CompareOpInvalidCondition;
					}
				}
			}
			if (!(ColValue2 != ""))
			{
				return;
			}
			array = ColValue2.Split(",");
			foreach (string value2 in array)
			{
				try
				{
					if (ColumnType == DataColumnType.Integer)
					{
						Convert.ToInt32(value2);
					}
					else if (ColumnType == DataColumnType.Decimal)
					{
						Convert.ToDecimal(value2);
					}
					else if (ColumnType == DataColumnType.DateTime)
					{
						Convert.ToDateTime(value2);
					}
					else if (ColumnType == DataColumnType.DateOnly)
					{
						Convert.ToDateTime(value2).ToDateOnly();
					}
					else if (ColumnType == DataColumnType.Boolean)
					{
						Convert.ToBoolean(value2);
					}
				}
				catch (Exception)
				{
					info.ErrorType = ErrorType.Critical;
					info.ErrorText = Resources.CompareOpInvalidCondition;
				}
			}
		}
	}
}
