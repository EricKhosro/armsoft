using System;
using System.ComponentModel;
using System.IO;
using ArmSoft.PrintTemplates;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.SmartEnums;
using DevExpress.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace ArmSoft.SME.Core.UI.Printing;

internal class XtraReportTemplateSubstitutor : TemplateSubstitutor
{
	private readonly bool _printDate;

	private readonly bool _printASInfo;

	private DocumentPrintWrapper PrintWrapper { get; }

	private PrintTemplateFileFormatSE OutputFileFormat { get; }

	private PdfExportOptions PdfExportOptions { get; set; } = new PdfExportOptions();

	public XtraReportTemplateSubstitutor(DocumentPrintWrapper printWrapper, PrintTemplateFileFormatSE outputFileFormat, string outputPassword = "", bool printASInfo = false, bool printDate = false)
		: base(printWrapper.GetTemplateSubstitution().PrintTemplateSubstitution)
	{
		PrintWrapper = printWrapper;
		OutputFileFormat = outputFileFormat;
		_printASInfo = printASInfo;
		_printDate = printDate;
		if (OutputFileFormat == PrintTemplateFileFormatSE.PDF && !string.IsNullOrWhiteSpace(outputPassword))
		{
			PdfExportOptions.PasswordSecurityOptions.OpenPassword = outputPassword;
		}
	}

	public override void SaveToFile(string file)
	{
		using XtraReport xtraReport = new XtraReport();
		DoSubstitute(xtraReport, file);
		string text = OutputFileFormat;
		if (text != null)
		{
			int length = text.Length;
			if (length != 3)
			{
				if (length == 4)
				{
					char c = text[0];
					if (c != 'D')
					{
						if (c != 'P')
						{
							if (c == 'X' && (text == "XLSX" || text == "XLSM"))
							{
								goto IL_0123;
							}
						}
						else if (text == "PRNX")
						{
							xtraReport.SaveDocument(file);
							return;
						}
					}
					else if (text == "DOCX" || text == "DOCM")
					{
						goto IL_0105;
					}
				}
			}
			else
			{
				char c = text[0];
				if (c != 'D')
				{
					if (c != 'P')
					{
						if (c == 'X' && text == "XLS")
						{
							goto IL_0123;
						}
					}
					else if (text == "PDF")
					{
						xtraReport.ExportToPdf(file, PdfExportOptions);
						return;
					}
				}
				else if (text == "DOC")
				{
					goto IL_0105;
				}
			}
		}
		throw new Exception($"Wrong print mode - {OutputFileFormat}");
		IL_0123:
		xtraReport.ExportToXlsx(file);
		return;
		IL_0105:
		DocxExportOptions options = new DocxExportOptions
		{
			TableLayout = true,
			KeepRowHeight = true
		};
		xtraReport.ExportToDocx(file, options);
	}

	public override Stream GetStream(byte[] file)
	{
		using XtraReport xtraReport = new XtraReport();
		DoSubstitute(xtraReport, file);
		MemoryStream memoryStream = new MemoryStream();
		string text = OutputFileFormat;
		if (text != null)
		{
			int length = text.Length;
			if (length != 3)
			{
				if (length == 4)
				{
					char c = text[0];
					if (c != 'D')
					{
						if (c == 'X' && (text == "XLSX" || text == "XLSM"))
						{
							goto IL_0125;
						}
					}
					else if (text == "DOCX" || text == "DOCM")
					{
						goto IL_011b;
					}
				}
			}
			else
			{
				switch (text[1])
				{
				case 'M':
					break;
				case 'D':
					goto IL_0097;
				case 'O':
					goto IL_00a9;
				case 'L':
					goto IL_00bb;
				default:
					goto IL_0143;
				}
				if (text == "XML")
				{
					xtraReport.SaveDocument(memoryStream);
					goto IL_0173;
				}
			}
		}
		goto IL_0143;
		IL_00a9:
		if (text == "DOC")
		{
			goto IL_011b;
		}
		goto IL_0143;
		IL_011b:
		xtraReport.ExportToDocx(memoryStream);
		goto IL_0173;
		IL_0097:
		if (!(text == "PDF"))
		{
			goto IL_0143;
		}
		xtraReport.ExportToPdf(memoryStream, PdfExportOptions);
		goto IL_0173;
		IL_0143:
		throw new Exception($"Wrong print mode - {OutputFileFormat}");
		IL_00bb:
		if (text == "XLS")
		{
			goto IL_0125;
		}
		goto IL_0143;
		IL_0125:
		DocxExportOptions options = new DocxExportOptions
		{
			TableLayout = true,
			KeepRowHeight = true
		};
		xtraReport.ExportToDocx(memoryStream, options);
		goto IL_0173;
		IL_0173:
		memoryStream.Seek(0L, SeekOrigin.Begin);
		return memoryStream;
	}

	public XtraReport CreateReport(string file)
	{
		XtraReport xtraReport = new XtraReport();
		DoSubstitute(xtraReport, file);
		return xtraReport;
	}

	private void DoSubstitute(XtraReport rep, string file)
	{
		rep.LoadLayoutFromXml(file);
		InnerSubstitute(rep);
	}

	public void DoSubstitute(XtraReport rep, byte[] fileData)
	{
		using MemoryStream stream = new MemoryStream(fileData);
		rep.LoadLayoutFromXml(stream);
		InnerSubstitute(rep);
	}

	private void InnerSubstitute(XtraReport rep)
	{
		rep.BeforePrint += Rep_BeforePrint;
		object dataSource = null;
		rep.ScriptsSource = "";
		rep.Bookmark = PrintWrapper.InitXtraReportDataSource(ref dataSource);
		rep.DataSource = dataSource;
		rep.DisplayName = PrintWrapper.GetTemplate().Caption;
		rep.CreateDocument();
	}

	private void Rep_BeforePrint(object sender, CancelEventArgs e)
	{
		if (_printASInfo)
		{
			PrintAsInfo(((XtraReport)sender).Bands.GetBandByType(typeof(TopMarginBand)));
		}
		if (_printDate)
		{
			PrintDate(((XtraReport)sender).Bands.GetBandByType(typeof(BottomMarginBand)));
		}
	}

	private static void PrintAsInfo(Band reportBand)
	{
		float num = 0f;
		int count = reportBand.Controls.Count;
		float topF = ((count != 0) ? (reportBand.Controls[count - 1].BottomF + 5f) : 15f);
		XRLabel xRLabel = new XRLabel
		{
			Text = string.Format(Resources.PrintedByAsSystem, Settings.ProgramName)
		};
		xRLabel.Text += Resources.PrintedByAsSystemWebPage;
		xRLabel.TopF = topF;
		xRLabel.Left = (int)Math.Round(num);
		xRLabel.Font = new DXFont("Tahoma", 8f);
		xRLabel.WidthF = 600f;
		reportBand.Controls.Add(xRLabel);
	}

	private static void PrintDate(Band reportBand)
	{
		XRLabel child = new XRLabel
		{
			Text = DateTime.Now.FormatDDMMYY(),
			Font = new DXFont("Tahoma", 8f),
			TextAlignment = TextAlignment.BottomLeft
		};
		reportBand.Controls.Add(child);
	}
}
