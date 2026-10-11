using System;

namespace ArmSoft.SME.Core.UI.Controls;

public class DocumentGridDuplicatingRowEventArgs : EventArgs
{
	public DocumentGridControl DocumentGrid { get; set; }

	public int RowHandle { get; set; }

	public int NewRowNumber { get; set; }

	public DocumentGridDuplicatingRowEventArgs(DocumentGridControl documentGrid, int rowHandle)
	{
		DocumentGrid = documentGrid;
		RowHandle = rowHandle;
		NewRowNumber = -1;
	}
}
