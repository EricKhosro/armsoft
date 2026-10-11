using System;

namespace ArmSoft.SME.Core.UI.Controls;

public class DocumentGridEditingRowEventArgs : EventArgs
{
	public DocumentGridControl DocumentGrid { get; set; }

	public int RowHandle { get; set; }

	public bool AllowEdit { get; set; }

	public bool RowEdited { get; set; }

	public DocumentGridEditingRowEventArgs(DocumentGridControl documentGrid, int rowHandle, bool allowEdit)
	{
		DocumentGrid = documentGrid;
		RowHandle = rowHandle;
		AllowEdit = allowEdit;
		RowEdited = false;
	}
}
