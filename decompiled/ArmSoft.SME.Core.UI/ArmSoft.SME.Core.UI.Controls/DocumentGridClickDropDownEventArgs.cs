using System;
using System.Windows;
using ArmSoft.SME.Core.UI.DataViews;

namespace ArmSoft.SME.Core.UI.Controls;

public class DocumentGridClickDropDownEventArgs : EventArgs
{
	public DocumentGridControl Grid { get; set; }

	public int RowHandle { get; set; }

	public DocumentGridColumn Column { get; set; }

	public double Left { get; set; }

	public double Top { get; set; }

	public string Value { get; set; }

	public string NewValue { get; set; }

	public Window Owner { get; set; }

	public ModalBrowserWindow.ModalBrowserRowChangedDelegate RowChangedDelegate { get; set; }

	public bool UpClick { get; set; }
}
