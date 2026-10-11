using System;
using System.Windows;
using ArmSoft.SME.Core.UI.DataViews;

namespace ArmSoft.SME.Core.UI.Controls;

public class DocumentGridDropDownEditedEventArgs : EventArgs
{
	public DocumentGridControl Grid { get; set; }

	public int RowHandle { get; set; }

	public DocumentGridColumn Column { get; set; }

	public string Value { get; set; }

	public string Comment { get; set; }

	public Window Owner { get; set; }

	public ModalBrowserWindow.ModalBrowserRowChangedDelegate RowChangedDelegate { get; set; }
}
