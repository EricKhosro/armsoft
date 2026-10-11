using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using ArmSoft.PrintTemplates;
using ArmSoft.PrintTemplates.Interfaces;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Models.TemplateSubstitution;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.PrintableElements;
using ArmSoft.SME.Common.Models.TemplateSubstitution;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.Printing;
using ArmSoft.SME.Core.UI.Report;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using DevExpress.XtraRichEdit;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.Services;

public class TemplateSubstitutionService : ITemplateSubstitutionService
{
	protected virtual ApiClient Client => Settings.CreateApiClient();

	public DocumentSubstituteResponseWithFile CreateAndReturnTemplate(Guid docIsn, string templateName, PrintTemplateFileFormatSE outputFileFormat, bool includePrintInfo = true)
	{
		PrintInfo printInfo = new PrintInfo();
		ArmSoft.SME.Client.Models.TemplateSubstitution.DocumentSubstituteRequest request = new ArmSoft.SME.Client.Models.TemplateSubstitution.DocumentSubstituteRequest
		{
			DocIsn = docIsn,
			TemplateName = templateName,
			OutputFileFormat = outputFileFormat,
			PrintASInfo = (includePrintInfo && printInfo.PrintASInfo),
			PrintDate = (includePrintInfo && printInfo.PrintDate)
		};
		return Client.TemplateSubstitution.CreateAndReturnTemplate(request);
	}

	public SelectedDocumentsSubstituteResponseWithFile CreateAndReturnTemplate(List<Guid> docIsns, string templateName, PrintTemplateFileFormatSE outputFileFormat, bool includePrintInfo = true)
	{
		PrintInfo printInfo = new PrintInfo();
		ArmSoft.SME.Client.Models.TemplateSubstitution.SelectedDocumentsSubstituteRequest request = new ArmSoft.SME.Client.Models.TemplateSubstitution.SelectedDocumentsSubstituteRequest
		{
			DocIsns = docIsns,
			TemplateName = templateName,
			OutputFileFormat = outputFileFormat,
			PrintASInfo = (includePrintInfo && printInfo.PrintASInfo),
			PrintDate = (includePrintInfo && printInfo.PrintDate)
		};
		return Client.TemplateSubstitution.CreateAndReturnTemplate(request);
	}

	public DocumentSubstituteResponseWithFile CreateAndReturnTemplate(List<PrintableElement> printableElements, string templateName, PrintTemplateFileFormatSE outputFileFormat, bool includePrintInfo = true)
	{
		PrintInfo printInfo = new PrintInfo();
		PrintableElementSubstituteRequestByModel request = new PrintableElementSubstituteRequestByModel
		{
			Elements = printableElements,
			TemplateName = templateName,
			OutputFileFormat = outputFileFormat,
			PrintASInfo = (includePrintInfo && printInfo.PrintASInfo),
			PrintDate = (includePrintInfo && printInfo.PrintDate)
		};
		return Client.TemplateSubstitution.CreateAndReturnTemplate(request);
	}

	public (PrintTemplateFileFormatSE FileFormat, byte[] File) CreateAndReturnTemplateLongExecute(List<PrintableElement> printableElements, string templateName, PrintTemplateFileFormatSE outputFileFormat, bool includePrintInfo = true)
	{
		PrintInfo printInfo = new PrintInfo();
		PrintableElementSubstituteRequest printableElementSubstituteRequest = new PrintableElementSubstituteRequest
		{
			OutputFileFormat = outputFileFormat,
			TemplateName = templateName,
			JSONElements = new List<string>(),
			PrintASInfo = (includePrintInfo && printInfo.PrintASInfo),
			PrintDate = (includePrintInfo && printInfo.PrintDate)
		};
		foreach (PrintableElement printableElement in printableElements)
		{
			printableElementSubstituteRequest.JSONElements.Add(JsonSerializer.Serialize(printableElement, Client.TemplateSubstitution.Options));
		}
		string item = DataProcessingRequestHelper.ExecuteThroughHandler<PrintableElementSubstituteRequest, string>(DPRType.Report, "TemplateSubstitution", printableElementSubstituteRequest).Item2;
		return (FileFormat: Path.GetExtension(item).ExtensionToTemplateFormat(), File: Client.TempFS.Download(item));
	}

	public static PrintTemplateFileFormatSE ConvertPrintModeToFileFormat(PrintMode mode)
	{
		return mode switch
		{
			PrintMode.ExportToExcel => PrintTemplateFileFormatSE.XLSX, 
			PrintMode.ExportToWord => PrintTemplateFileFormatSE.DOCX, 
			PrintMode.ExportToPdf => PrintTemplateFileFormatSE.PDF, 
			PrintMode.PrintPreview => PrintTemplateFileFormatSE.PRNX, 
			PrintMode.Print => PrintTemplateFileFormatSE.PRNX, 
			PrintMode.PrintDirect => PrintTemplateFileFormatSE.PRNX, 
			_ => throw new RESTException(Resources.InvalidFileFormat), 
		};
	}

	public static PrintMode ConvertFileFormatToPrintMode(PrintTemplateFileFormatSE fileFormat)
	{
		return fileFormat.Value switch
		{
			"XLSX" => PrintMode.ExportToExcel, 
			"XLS" => PrintMode.ExportToExcel, 
			"XLSM" => PrintMode.ExportToExcel, 
			"DOC" => PrintMode.ExportToWord, 
			"DOCX" => PrintMode.ExportToWord, 
			"DOCM" => PrintMode.ExportToWord, 
			"PDF" => PrintMode.ExportToPdf, 
			"XML" => PrintMode.PrintPreview, 
			_ => throw new RESTException(Resources.InvalidFileFormat), 
		};
	}

	public string Export(Guid docIsn, PrintMode mode, string templateName = null, bool includePrintInfo = true)
	{
		PrintTemplateFileFormatSE outputFileFormat = ConvertPrintModeToFileFormat(mode);
		DocumentSubstituteResponseWithFile documentSubstituteResponseWithFile = CreateAndReturnTemplate(docIsn, templateName, outputFileFormat, includePrintInfo);
		if (documentSubstituteResponseWithFile.File.Length != 0)
		{
			string tempFileName = Settings.GetTempFileName("." + documentSubstituteResponseWithFile.FileFormat);
			using FileStream fileStream = File.Create(tempFileName);
			fileStream.Write(documentSubstituteResponseWithFile.File);
			fileStream.Close();
			return tempFileName;
		}
		return null;
	}

	public void PrintPreview(Guid docIsn, PrintMode mode, string templateName = null, string printerName = null, int count = 1, bool includePrintInfo = true, string fileNamePrephix = "")
	{
		PrintTemplateFileFormatSE outputFileFormat = ConvertPrintModeToFileFormat(mode);
		DocumentSubstituteResponseWithFile documentSubstituteResponseWithFile = CreateAndReturnTemplate(docIsn, templateName, outputFileFormat, includePrintInfo);
		ShowPrintingFile(documentSubstituteResponseWithFile.File, documentSubstituteResponseWithFile.FileFormat, mode, documentSubstituteResponseWithFile.PageSettings, printerName, count, fileNamePrephix);
	}

	public void PrintPreview(List<Guid> docIsns, PrintMode mode, string templateName = null, bool insertPageBreaks = true, string printerName = null, EnumerateErrorsReportViewer errorRepViewer = null, int count = 1, bool includePrintInfo = true, string fileNamePrephix = "")
	{
		if (docIsns.Count == 1)
		{
			PrintPreview(docIsns[0], mode, templateName, printerName, count, includePrintInfo, fileNamePrephix);
			return;
		}
		PrintTemplateFileFormatSE outputFileFormat = ConvertPrintModeToFileFormat(mode);
		SelectedDocumentsSubstituteResponseWithFile selectedDocumentsSubstituteResponseWithFile = CreateAndReturnTemplate(docIsns, templateName, outputFileFormat, includePrintInfo);
		if (errorRepViewer != null && selectedDocumentsSubstituteResponseWithFile.Exceptions != null)
		{
			foreach (DocumentSubstituteResponse exception in selectedDocumentsSubstituteResponseWithFile.Exceptions)
			{
				errorRepViewer.AddError(exception.Exception.Message, exception.DocISN);
			}
		}
		if (!string.IsNullOrWhiteSpace(selectedDocumentsSubstituteResponseWithFile.FileFormat))
		{
			ShowPrintingFile(selectedDocumentsSubstituteResponseWithFile.File, selectedDocumentsSubstituteResponseWithFile.FileFormat, mode, null, printerName, count, fileNamePrephix);
		}
	}

	public void PrintPreview(PrintableElement printableElement, PrintMode mode, string templateName = null, string printerName = null, int count = 1, bool includePrintInfo = true, string fileNamePrephix = "")
	{
		if (Settings.IsOffline)
		{
			PrintPreviewOffline(printableElement, mode, templateName, printerName, count, includePrintInfo: false, fileNamePrephix);
			return;
		}
		PrintTemplateFileFormatSE outputFileFormat = ConvertPrintModeToFileFormat(mode);
		int num = 1;
		List<PrintableElement> list = new List<PrintableElement>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = printableElement;
		DocumentSubstituteResponseWithFile documentSubstituteResponseWithFile = CreateAndReturnTemplate(list, templateName, outputFileFormat, includePrintInfo);
		if (!string.IsNullOrWhiteSpace(documentSubstituteResponseWithFile.FileFormat))
		{
			ShowPrintingFile(documentSubstituteResponseWithFile.File, documentSubstituteResponseWithFile.FileFormat, mode, documentSubstituteResponseWithFile.PageSettings, printerName, count, fileNamePrephix);
		}
	}

	public void PrintPreview(List<PrintableElement> printableElements, PrintMode mode, string templateName = null, bool insertPageBreaks = true, string printerName = null, EnumerateErrorsReportViewer errorRepViewer = null, int count = 1, bool includePrintInfo = true, string fileNamePrephix = "")
	{
		if (printableElements.Count == 1)
		{
			PrintPreview(printableElements[0], mode, templateName, printerName, count, includePrintInfo, fileNamePrephix);
			return;
		}
		PrintTemplateFileFormatSE outputFileFormat = ConvertPrintModeToFileFormat(mode);
		DocumentSubstituteResponseWithFile documentSubstituteResponseWithFile = CreateAndReturnTemplate(printableElements, templateName, outputFileFormat, includePrintInfo);
		if (errorRepViewer != null && documentSubstituteResponseWithFile.Exceptions != null)
		{
			foreach (ExceptionModel exception in documentSubstituteResponseWithFile.Exceptions)
			{
				errorRepViewer.AddError(exception.Message);
			}
		}
		if (!string.IsNullOrWhiteSpace(documentSubstituteResponseWithFile.FileFormat))
		{
			ShowPrintingFile(documentSubstituteResponseWithFile.File, documentSubstituteResponseWithFile.FileFormat, mode, null, printerName, count, fileNamePrephix);
		}
	}

	public void PrintPreviewLongExecute(PrintableElement printableElement, PrintMode mode, string templateName = null, string printerName = null, int count = 1, bool includePrintInfo = true, string fileNamePrephix = "")
	{
		int num = 1;
		List<PrintableElement> list = new List<PrintableElement>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = printableElement;
		PrintPreviewLongExecute(list, mode, templateName, insertPageBreaks: true, printerName, count, includePrintInfo, fileNamePrephix);
	}

	public void PrintPreviewLongExecute(List<PrintableElement> printableElements, PrintMode mode, string templateName = null, bool insertPageBreaks = true, string printerName = null, int count = 1, bool includePrintInfo = true, string fileNamePrephix = "")
	{
		PrintTemplateFileFormatSE outputFileFormat = ConvertPrintModeToFileFormat(mode);
		(PrintTemplateFileFormatSE, byte[]) tuple = CreateAndReturnTemplateLongExecute(printableElements, templateName, outputFileFormat, includePrintInfo);
		ShowPrintingFile(tuple.Item2, tuple.Item1, mode, null, printerName, count, fileNamePrephix);
	}

	private static void ShowPrintingFile(byte[] fileContent, PrintTemplateFileFormatSE fileFormat, PrintMode mode, byte[] pageSettings = null, string printerName = null, int count = 1, string fileNamePrephix = "")
	{
		if (fileContent.Length == 0)
		{
			return;
		}
		string text = ((!string.IsNullOrWhiteSpace(fileNamePrephix)) ? Settings.GetTempFileNameByPrephix(fileNamePrephix, "." + fileFormat) : Settings.GetTempFileName("." + fileFormat));
		using (FileStream fileStream = File.Create(text))
		{
			fileStream.Write(fileContent);
			fileStream.Close();
		}
		if (pageSettings != null)
		{
			using FileStream fileStream2 = File.Create(text + ".xml");
			fileStream2.Write(pageSettings);
			fileStream2.Close();
		}
		if (mode == PrintMode.Print || mode == PrintMode.PrintDirect)
		{
			while (count != 0)
			{
				if (fileFormat == PrintTemplateFileFormatSE.PRNX)
				{
					if (mode == PrintMode.Print)
					{
						PreviewWindow.Print(text);
					}
					else
					{
						PreviewWindow.PrintDirect(text, printerName, pageSettings != null);
					}
				}
				else
				{
					RichEditDocumentServer richEditDocumentServer = new RichEditDocumentServer();
					if (!richEditDocumentServer.IsPrintingAvailable)
					{
						Process.Start(new ProcessStartInfo
						{
							FileName = text,
							UseShellExecute = true
						});
						break;
					}
					richEditDocumentServer.LoadDocument(text);
					using PrintingSystemBase printingSystemBase = new PrintingSystemBase();
					using PrintableComponentLinkBase printableComponentLinkBase = new PrintableComponentLinkBase(printingSystemBase);
					printableComponentLinkBase.Component = richEditDocumentServer;
					printingSystemBase.ShowMarginsWarning = false;
					printingSystemBase.ShowPrintStatusDialog = false;
					printableComponentLinkBase.CreateDocument();
					new PrintToolBase(printingSystemBase).Print();
				}
				count--;
			}
		}
		else if (mode == PrintMode.PrintPreview && fileFormat == PrintTemplateFileFormatSE.PRNX)
		{
			new PreviewWindow(null, text, printerName, pageSettings != null).Show();
		}
		else
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = text,
				UseShellExecute = true
			});
		}
	}

	public (PrintTemplateFileFormatSE FileFormat, byte[] File) CreateAndReturnTemplateOffline(DocumentPrintWrapper printWrapper, PrintTemplateFileFormatSE outputFileFormat, bool printASInfo, bool printDate)
	{
		try
		{
			if (printWrapper == null)
			{
				return (FileFormat: null, File: null);
			}
			(PrintTemplateFileFormatSE, byte[]) tuple = Settings.ServiceProvider.GetRequiredService<ITemplateService>().LoadTemplateFileOffline(printWrapper.GetTemplate().Name);
			if (tuple.Item1 != PrintTemplateFileFormatSE.XML && outputFileFormat != PrintTemplateFileFormatSE.PDF)
			{
				(outputFileFormat, _) = tuple;
			}
			string tempFileName = Settings.GetTempFileName("." + outputFileFormat);
			using (FileStream fileStream = File.Create(tempFileName))
			{
				fileStream.Write(tuple.Item2);
				fileStream.Close();
			}
			InitTemplateSubstitutor(printWrapper, tuple.Item1, outputFileFormat, printASInfo, printDate).SaveToFile(tempFileName);
			return (FileFormat: outputFileFormat, File: File.ReadAllBytes(tempFileName));
		}
		catch (InvalidTemplateException ex)
		{
			throw new RESTException(ex.Message, ex);
		}
	}

	private static ITemplateSubstitutor InitTemplateSubstitutor(DocumentPrintWrapper printWrapper, PrintTemplateFileFormatSE inputFileFormat, PrintTemplateFileFormatSE outputFileFormat, bool printASInfo = false, bool printDate = false)
	{
		PrintTemplateSubstitution printTemplateSubstitution = printWrapper.GetTemplateSubstitution().PrintTemplateSubstitution;
		return inputFileFormat.ToString() switch
		{
			"DOC" => (ITemplateSubstitutor)new DocTemplateSubstitutor(printTemplateSubstitution, inputFileFormat, outputFileFormat), 
			"DOCX" => new DocTemplateSubstitutor(printTemplateSubstitution, inputFileFormat, outputFileFormat), 
			"XLSM" => new SpreadsheetTemplateSubstitutorXlsm(printTemplateSubstitution, inputFileFormat), 
			"XLSX" => new SpreadsheetTemplateSubstitutorXlsx(printTemplateSubstitution, inputFileFormat), 
			"XML" => new XtraReportTemplateSubstitutor(printWrapper, outputFileFormat, "", printASInfo, printDate), 
			_ => throw new RESTException(Resources.InvalidFileFormat), 
		};
	}

	public virtual DocumentPrintWrapper GetDocumentPrintWrapperOffline(PrintableElement printableElement, TemplatesCacheRow template)
	{
		throw new NotImplementedException();
	}

	public void PrintPreviewOffline(PrintableElement printableElement, PrintMode mode, string templateName = null, string printerName = null, int count = 1, bool includePrintInfo = true, string fileNamePrephix = "")
	{
		PrintInfo printInfo = new PrintInfo();
		PrintTemplateFileFormatSE outputFileFormat = ConvertPrintModeToFileFormat(mode);
		(PrintTemplateFileFormatSE, byte[]) tuple = CreateAndReturnTemplateOffline(GetDocumentPrintWrapperOffline(printableElement, Settings.CoreCacheService.Template.GetItem(templateName)), outputFileFormat, includePrintInfo && printInfo.PrintASInfo, includePrintInfo && printInfo.PrintDate);
		ShowPrintingFile(tuple.Item2, tuple.Item1, mode, null, printerName, count, fileNamePrephix);
	}
}
