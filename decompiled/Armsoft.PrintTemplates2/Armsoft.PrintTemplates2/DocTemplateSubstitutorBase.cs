using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ArmSoft.PrintTemplates;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;

namespace Armsoft.PrintTemplates2;

public abstract class DocTemplateSubstitutorBase : TemplateSubstitutor, ITemplateMerger
{
	private DocumentFormat InputDocumentFormat { get; set; }

	private DocumentFormat OutputDocumentFormat { get; set; }

	private bool ExportToPdf { get; set; }

	protected bool Protect { get; set; }

	private PdfExportOptions PdfExportOptions { get; set; } = new PdfExportOptions();

	public DocTemplateSubstitutorBase(IPrintTemplateSubstitution printTemplateSubstitution, string inputFormat, string outputFormat = "", string outputPassword = "", bool protect = false, bool check = false)
		: base(printTemplateSubstitution, check)
	{
		InputDocumentFormat = GetFormat(inputFormat);
		Protect = protect;
		if (outputFormat == "PDF")
		{
			ExportToPdf = true;
			if (!string.IsNullOrWhiteSpace(outputPassword))
			{
				PdfExportOptions.PasswordSecurityOptions.OpenPassword = outputPassword;
			}
		}
		else
		{
			OutputDocumentFormat = (string.IsNullOrWhiteSpace(outputFormat) ? InputDocumentFormat : GetFormat(outputFormat));
		}
	}

	public override void SaveToFile(string file)
	{
		using RichEditDocumentServer richEditDocumentServer = new RichEditDocumentServer();
		DoSubstitute(richEditDocumentServer, file);
		if (ExportToPdf)
		{
			richEditDocumentServer.ExportToPdf(file, PdfExportOptions);
		}
		else
		{
			richEditDocumentServer.SaveDocument(file, OutputDocumentFormat);
		}
	}

	public override Stream GetStream(byte[] file)
	{
		using RichEditDocumentServer richEditDocumentServer = new RichEditDocumentServer();
		DoSubstitute(richEditDocumentServer, file);
		Stream stream;
		if (ExportToPdf)
		{
			stream = new MemoryStream();
			richEditDocumentServer.ExportToPdf(stream, PdfExportOptions);
			stream.Seek(0L, SeekOrigin.Begin);
		}
		else
		{
			stream = new MemoryStream(richEditDocumentServer.SaveDocument(OutputDocumentFormat));
		}
		return stream;
	}

	public override async Task SaveToFileAsync(string file)
	{
		using RichEditDocumentServer srv = new RichEditDocumentServer();
		DoSubstitute(srv, file);
		if (ExportToPdf)
		{
			await srv.ExportToPdfAsync(file, PdfExportOptions);
		}
		else
		{
			await srv.SaveDocumentAsync(file, OutputDocumentFormat);
		}
	}

	public override async Task<Stream> GetStreamAsync(byte[] file)
	{
		using RichEditDocumentServer srv = new RichEditDocumentServer();
		DoSubstitute(srv, file);
		Stream stream;
		if (ExportToPdf)
		{
			stream = new MemoryStream();
			await srv.ExportToPdfAsync(stream, PdfExportOptions);
			stream.Seek(0L, SeekOrigin.Begin);
		}
		else
		{
			stream = new MemoryStream(await srv.SaveDocumentAsync(OutputDocumentFormat));
		}
		return stream;
	}

	private void DoSubstitute(RichEditDocumentServer srv, string file)
	{
		srv.LoadDocument(file, InputDocumentFormat);
		InnerDoSubstitute(srv);
	}

	private void DoSubstitute(RichEditDocumentServer srv, byte[] file)
	{
		srv.LoadDocument(file, InputDocumentFormat);
		InnerDoSubstitute(srv);
	}

	private void InnerDoSubstitute(RichEditDocumentServer srv)
	{
		try
		{
			srv.BeginUpdate();
			InnerSubstitute(srv);
		}
		finally
		{
			srv.EndUpdate();
		}
	}

	public void MergeFile(string targetFile, string sourceFile, bool insertPageBreak)
	{
		using RichEditDocumentServer richEditDocumentServer = new RichEditDocumentServer();
		richEditDocumentServer.LoadDocument(targetFile, InputDocumentFormat);
		if (insertPageBreak)
		{
			richEditDocumentServer.Document.AppendText(new string('\f', 1));
		}
		richEditDocumentServer.Document.AppendDocumentContent(sourceFile);
		richEditDocumentServer.SaveDocument(targetFile, OutputDocumentFormat);
	}

	public async Task MergeFileAsync(string targetFile, string sourceFile, bool insertPageBreak)
	{
		using RichEditDocumentServer srv = new RichEditDocumentServer();
		srv.LoadDocument(targetFile, InputDocumentFormat);
		if (insertPageBreak)
		{
			srv.Document.AppendText(new string('\f', 1));
		}
		srv.Document.AppendDocumentContent(sourceFile);
		await srv.SaveDocumentAsync(targetFile, OutputDocumentFormat);
	}

	public Stream MergeFile(byte[] targetFile, byte[] sourceFile, bool insertPageBreak)
	{
		using RichEditDocumentServer richEditDocumentServer = new RichEditDocumentServer();
		richEditDocumentServer.LoadDocument(targetFile, InputDocumentFormat);
		if (insertPageBreak)
		{
			richEditDocumentServer.Document.AppendText(new string('\f', 1));
		}
		using MemoryStream stream = new MemoryStream(sourceFile);
		richEditDocumentServer.Document.AppendDocumentContent(stream);
		return new MemoryStream(richEditDocumentServer.SaveDocument(OutputDocumentFormat));
	}

	public async Task<Stream> MergeFileAsync(byte[] targetFile, byte[] sourceFile, bool insertPageBreak)
	{
		using RichEditDocumentServer srv = new RichEditDocumentServer();
		srv.LoadDocument(targetFile, InputDocumentFormat);
		if (insertPageBreak)
		{
			srv.Document.AppendText(new string('\f', 1));
		}
		using MemoryStream memoryStream = new MemoryStream(sourceFile);
		srv.Document.AppendDocumentContent(memoryStream);
		return new MemoryStream(await srv.SaveDocumentAsync(OutputDocumentFormat));
	}

	private DocumentFormat GetFormat(string format)
	{
		return format switch
		{
			"DOCM" => DocumentFormat.Docm, 
			"DOCX" => DocumentFormat.OpenXml, 
			"TEXT" => DocumentFormat.PlainText, 
			"DOC" => DocumentFormat.Doc, 
			"DOT" => DocumentFormat.Dot, 
			_ => throw new Exception(format + " format is not supported"), 
		};
	}

	private protected abstract void InnerSubstitute(RichEditDocumentServer srv);

	private protected bool SubstituteTable(IPrintTemplateSubstitutionGrid grid, Table table, DevExpress.XtraRichEdit.API.Native.Document doc, DocumentRange documentRange)
	{
		int num = -1;
		foreach (DevExpress.XtraRichEdit.API.Native.TableRow row in table.Rows)
		{
			if (row.Range.Contains(documentRange.Start))
			{
				num = row.Index;
				break;
			}
		}
		if (num != -1)
		{
			if (grid.RowCount > 0)
			{
				List<GridColumn> sortedByLengthColums = grid.GetSortedByLengthColums();
				for (int i = 1; i < grid.RowCount; i++)
				{
					table.Rows.InsertAfter(num + i - 1);
					foreach (TableCell cell in table.Rows[num].Cells)
					{
						doc.InsertDocumentContent(table[num + i, cell.Index].ContentRange.Start, table[num, cell.Index].ContentRange);
					}
					SubstituteRowsReplace(doc, grid, sortedByLengthColums, i, table.Rows[num + i].Range);
				}
				SubstituteRowsReplace(doc, grid, sortedByLengthColums, 0, table.Rows[num].Range);
			}
			else
			{
				ClearRow(doc, table, grid);
			}
		}
		return false;
	}

	private void SubstituteRowsReplace(DevExpress.XtraRichEdit.API.Native.Document doc, IPrintTemplateSubstitutionGrid grid, List<GridColumn> cols, int gridRowIndex, DocumentRange range)
	{
		foreach (GridColumn col in cols)
		{
			string textToFind = "%" + grid.Name + "." + col.Name;
			string replaceWith = grid.GetValueConsideringFormatter(gridRowIndex, col.Name)?.ToString() ?? string.Empty;
			doc.ReplaceAll(textToFind, replaceWith, SearchOptions.None, range);
		}
	}

	private void ClearRow(DevExpress.XtraRichEdit.API.Native.Document doc, Table table, IPrintTemplateSubstitutionGrid grid)
	{
		foreach (GridColumn sortedByLengthColum in grid.GetSortedByLengthColums())
		{
			string textToFind = "%" + grid.Name + "." + sortedByLengthColum.Name;
			DocumentRange[] array = doc.FindAll(textToFind, SearchOptions.None, table.Range);
			foreach (DocumentRange range in array)
			{
				doc.Replace(range, "");
			}
		}
	}
}
