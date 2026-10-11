using System;

namespace ArmSoft.PrintTemplates;

internal class MergedRange
{
	protected const char ColonCharacter = ':';

	protected const int RowNumberLimit = 1048576;

	protected string _reference;

	protected string _startColumn;

	protected int _startRow;

	protected string _endColumn;

	protected int _endRow;

	protected bool _isReferenceValid;

	public string Reference
	{
		get
		{
			if (!_isReferenceValid)
			{
				_reference = ToString();
				_isReferenceValid = true;
			}
			return _reference;
		}
		set
		{
			DecomposeReference(value);
			_reference = value;
			_isReferenceValid = true;
		}
	}

	public string StartColumn
	{
		get
		{
			return _startColumn;
		}
		set
		{
			_isReferenceValid = false;
			_startColumn = value;
		}
	}

	public int StartRow
	{
		get
		{
			return _startRow;
		}
		set
		{
			_startRow = value;
		}
	}

	public string EndColumn
	{
		get
		{
			return _endColumn;
		}
		set
		{
			_isReferenceValid = false;
			_endColumn = value;
		}
	}

	public int EndRow
	{
		get
		{
			return _endRow;
		}
		set
		{
			_isReferenceValid = false;
			_endRow = value;
		}
	}

	public MergedRange(string reference)
	{
		Reference = reference;
	}

	public MergedRange(string startColumn, int startRow, string endColumn, int endRow)
	{
		_startColumn = startColumn;
		_startRow = startRow;
		_endColumn = endColumn;
		_endRow = endRow;
		_isReferenceValid = false;
	}

	protected void DecomposeReference(string rangeReference)
	{
		string[] array = rangeReference.Split(':');
		if (array.Length != 2)
		{
			throw new ArgumentException("Passed range reference {0} is not valid. It does not contain splitter character(:)", rangeReference);
		}
		string column = StartColumn;
		int row = StartRow;
		DecomposeCellReference(array[0], ref column, ref row);
		StartColumn = column;
		StartRow = row;
		string column2 = EndColumn;
		int row2 = EndRow;
		DecomposeCellReference(array[1], ref column2, ref row2);
		EndColumn = column2;
		EndRow = row2;
	}

	protected static void DecomposeCellReference(string cellReference, ref string column, ref int row)
	{
		int num = 0;
		int i = 0;
		for (int num2 = cellReference.Length - 1; i <= num2; i++)
		{
			if (char.IsDigit(cellReference, i))
			{
				num = i;
				break;
			}
		}
		if (num == 0)
		{
			throw new ArgumentException("Passed cell reference {0} is not valid.", cellReference);
		}
		column = cellReference.Substring(0, num);
		int num3 = num;
		row = Convert.ToInt32(cellReference.Substring(num3, cellReference.Length - num3));
	}

	public MergedRange ShiftDown(int rowCount)
	{
		if (EndRow + rowCount <= 1048576)
		{
			StartRow += rowCount;
			EndRow += rowCount;
		}
		return this;
	}

	public MergedRange ShiftDownEnd(int rowCount)
	{
		if (EndRow + rowCount <= 1048576)
		{
			EndRow += rowCount;
		}
		return this;
	}

	public MergedRange ShiftUp(int rowCount)
	{
		StartRow -= rowCount;
		EndRow -= rowCount;
		return this;
	}

	public MergedRange ShiftUpEnd(int rowCount)
	{
		EndRow -= rowCount;
		return this;
	}

	public MergedRange ReflectRowInserts(int insertStartRow, int rowCount)
	{
		if (insertStartRow < StartRow)
		{
			ShiftDown(rowCount);
		}
		else if ((insertStartRow >= StartRow) & (insertStartRow <= EndRow) & (StartRow != EndRow))
		{
			ShiftDownEnd(rowCount);
		}
		return this;
	}

	public MergedRange ReflectRowDelete(int deleteStartRow)
	{
		if (deleteStartRow < StartRow)
		{
			ShiftUp(1);
		}
		else if ((deleteStartRow >= StartRow) & (deleteStartRow <= EndRow) & (StartRow != EndRow))
		{
			ShiftUpEnd(1);
		}
		return this;
	}

	public override string ToString()
	{
		if (_isReferenceValid)
		{
			return _reference;
		}
		_003C_003Ey__InlineArray4<object> buffer = default;
		buffer[0] = _startColumn;
		buffer[1] = _startRow;
		buffer[2] = _endColumn;
		buffer[3] = _endRow;
		return string.Format("{0}{1}:{2}{3}", (ReadOnlySpan<object?>)buffer);
	}
}
