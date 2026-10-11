using System;

namespace ArmSoft.SME.Core.UI.Controls;

public class DocumentGridInsertingRowEventArgs : EventArgs
{
	public DocumentGridControl DocumentGrid { get; set; }

	public int RowHandle { get; set; }

	public bool IsDuplicate { get; set; }

	public bool FocusFirstColumn { get; set; }

	public DocumentGridInsertingRowEventArgs(DocumentGridControl documentGrid, int rowHandle, bool isDuplicate)
	{
		DocumentGrid = documentGrid;
		RowHandle = rowHandle;
		IsDuplicate = isDuplicate;
		FocusFirstColumn = true;
	}
}
