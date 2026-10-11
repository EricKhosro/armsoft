using System;

namespace ArmSoft.SME.Core.UI.Controls;

public class DocumentGridCustomUnboundColumnDataEventArgs : EventArgs
{
	public DocumentGridControl DocumentGrid { get; set; }

	public int RowHandle { get; set; }

	public DocumentGridCustomUnboundColumnDataEventArgs(DocumentGridControl documentGrid, int rowHandle)
	{
		DocumentGrid = documentGrid;
		RowHandle = rowHandle;
	}
}
