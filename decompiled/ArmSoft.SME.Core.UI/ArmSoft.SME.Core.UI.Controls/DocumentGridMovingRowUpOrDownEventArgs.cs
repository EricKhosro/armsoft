using System;

namespace ArmSoft.SME.Core.UI.Controls;

public class DocumentGridMovingRowUpOrDownEventArgs : EventArgs
{
	public DocumentGridControl DocumentGrid { get; set; }

	public int RowHandle { get; set; }

	public DocumentGridMovingRowUpOrDownEventArgs(DocumentGridControl documentGrid, int rowHandle)
	{
		DocumentGrid = documentGrid;
		RowHandle = rowHandle;
	}
}
