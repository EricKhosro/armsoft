using System;

namespace ArmSoft.SME.Core.UI.Controls;

public class DocumentGridDeletingRowEventArgs : EventArgs
{
	public DocumentGridControl DocumentGrid { get; set; }

	public int RowHandle { get; set; }

	public bool Cancel { get; set; }

	public DocumentGridDeletingRowEventArgs(DocumentGridControl documentGrid, int rowHandle)
	{
		DocumentGrid = documentGrid;
		RowHandle = rowHandle;
		Cancel = false;
	}
}
