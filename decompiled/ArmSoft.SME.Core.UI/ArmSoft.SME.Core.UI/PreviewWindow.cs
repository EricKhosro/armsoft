using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;

namespace ArmSoft.SME.Core.UI;

public class PreviewWindow : ThemedWindow
{
	public PreviewWindow(Window owner, object documentSource, string printerName, bool restorePageSettings = false)
	{
		Owner = owner;
		WindowState = WindowState.Maximized;
		Grid grid = new Grid();
		DocumentPreviewControl documentPreviewControl = new DocumentPreviewControl
		{
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
			DocumentSource = documentSource
		};
		if (restorePageSettings)
		{
			documentPreviewControl.Document.PageSettings.RestoreFromXml($"{documentSource}.xml");
		}
		if (!string.IsNullOrWhiteSpace(printerName))
		{
			documentPreviewControl.Document.PrintingSystem.PageSettings.PrinterName = printerName;
		}
		grid.Children.Add(documentPreviewControl);
		Content = grid;
	}

	public static void Print(string fileName)
	{
		DocumentPreviewControl documentPreviewControl = new DocumentPreviewControl();
		documentPreviewControl.DocumentSource = fileName;
		documentPreviewControl.Print();
	}

	public static void PrintDirect(string fileName, string printerName = null, bool restorePageSettings = false)
	{
		DocumentPreviewControl documentPreviewControl = new DocumentPreviewControl
		{
			DocumentSource = fileName
		};
		if (restorePageSettings)
		{
			documentPreviewControl.Document.PageSettings.RestoreFromXml(fileName + ".xml");
		}
		documentPreviewControl.PrintDirect(printerName);
	}
}
