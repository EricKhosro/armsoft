using System;

namespace ArmSoft.SME.Core.UI.Controls;

public class DocumentGridAddingRowEventArgs : EventArgs
{
	public DocumentGridControl DocumentGrid { get; set; }

	public int RowNumber { get; set; }

	public bool FocusFirstColumn { get; set; }

	public int FocusColumnIndex { get; set; }

	public DocumentGridAddingRowEventArgs(DocumentGridControl documentGrid)
	{
		DocumentGrid = documentGrid;
		RowNumber = -1;
		FocusFirstColumn = true;
		FocusColumnIndex = 0;
	}
}
