using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.PrintableElements;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Printing;
using ArmSoft.SME.Core.UI.Report;

namespace ArmSoft.SME.Core.UI.Services;

public interface ITemplateSubstitutionService
{
	string Export(Guid docIsn, PrintMode mode, string templateName = null, bool includePrintInfo = true);

	void PrintPreview(Guid docIsn, PrintMode mode, string templateName = null, string printerName = null, int count = 1, bool includePrintInfo = true, string fileNamePrephix = "");

	void PrintPreview(List<Guid> docIsn, PrintMode mode, string templateName = null, bool insertPageBreaks = true, string printerName = null, EnumerateErrorsReportViewer errorRepViewer = null, int count = 1, bool includePrintInfo = true, string fileNamePrephix = "");

	void PrintPreview(PrintableElement printableElement, PrintMode mode, string templateName = null, string printerName = null, int count = 1, bool includePrintInfo = true, string fileNamePrephix = "");

	void PrintPreview(List<PrintableElement> printableElements, PrintMode mode, string templateName = null, bool insertPageBreaks = true, string printerName = null, EnumerateErrorsReportViewer errorRepViewer = null, int count = 1, bool includePrintInfo = true, string fileNamePrephix = "");

	void PrintPreviewLongExecute(PrintableElement printableElement, PrintMode mode, string templateName = null, string printerName = null, int count = 1, bool includePrintInfo = true, string fileNamePrephix = "");

	void PrintPreviewLongExecute(List<PrintableElement> printableElements, PrintMode mode, string templateName = null, bool insertPageBreaks = true, string printerName = null, int count = 1, bool includePrintInfo = true, string fileNamePrephix = "");

	void PrintPreviewOffline(PrintableElement printableElement, PrintMode mode, string templateName = null, string printerName = null, int count = 1, bool includePrintInfo = true, string fileNamePrephix = "");

	(PrintTemplateFileFormatSE FileFormat, byte[] File) CreateAndReturnTemplateOffline(DocumentPrintWrapper printWrapper, PrintTemplateFileFormatSE outputFileFormat, bool printASInfo, bool printDate);

	DocumentPrintWrapper GetDocumentPrintWrapperOffline(PrintableElement printableElement, TemplatesCacheRow template);
}
