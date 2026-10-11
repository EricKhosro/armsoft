using System.ComponentModel;

namespace ArmSoft.SME.Core.UI.Report;

public class ReportActiveRow : INotifyPropertyChanged
{
	private int _rowNum;

	private int _oldRowNum;

	private int _selectedRows;

	private int _cellNum;

	private int _oldCellNum;

	public int SelectedCount
	{
		get
		{
			return _selectedRows;
		}
		set
		{
			_selectedRows = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedCountChanged"));
		}
	}

	public int OldRowValue => _oldRowNum;

	public int OldCellValue => _oldCellNum;

	public int ActiveRow
	{
		get
		{
			return _rowNum;
		}
		set
		{
			_oldRowNum = _rowNum;
			_rowNum = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActiveRowChanged"));
		}
	}

	public int ActiveCell
	{
		get
		{
			return _cellNum;
		}
		set
		{
			_oldCellNum = _cellNum;
			_cellNum = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActiveCellChanged"));
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public ReportActiveRow()
	{
		_rowNum = -1;
		_oldRowNum = -1;
		_cellNum = -1;
		_oldCellNum = -1;
		_selectedRows = 0;
	}
}
