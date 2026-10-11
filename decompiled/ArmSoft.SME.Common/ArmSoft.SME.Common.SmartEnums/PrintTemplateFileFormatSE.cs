using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(4, "FileFormat")]
public class PrintTemplateFileFormatSE : ASSmartEnum<PrintTemplateFileFormatSE, string>
{
	public static readonly PrintTemplateFileFormatSE REPX = new PrintTemplateFileFormatSE("REPX", "REPX", () => Resources.InnerPrintTemplate);

	public static readonly PrintTemplateFileFormatSE XML = new PrintTemplateFileFormatSE("XML", "XML", () => Resources.InnerPrintTemplate);

	public static readonly PrintTemplateFileFormatSE DOC = new PrintTemplateFileFormatSE("DOC", "DOC", () => "Word Document");

	public static readonly PrintTemplateFileFormatSE DOCX = new PrintTemplateFileFormatSE("DOCX", "DOCX", () => "Word Document");

	public static readonly PrintTemplateFileFormatSE DOCM = new PrintTemplateFileFormatSE("DOCM", "DOCM", () => "Word Document");

	public static readonly PrintTemplateFileFormatSE XLS = new PrintTemplateFileFormatSE("XLS", "XLS", () => "Excel Worksheet");

	public static readonly PrintTemplateFileFormatSE XLSX = new PrintTemplateFileFormatSE("XLSX", "XLSX", () => "Excel Worksheet");

	public static readonly PrintTemplateFileFormatSE XLSM = new PrintTemplateFileFormatSE("XLSM", "XLSM", () => "Excel Worksheet");

	public static readonly PrintTemplateFileFormatSE XSD = new PrintTemplateFileFormatSE("XSD", "XSD", () => "XSD Schema");

	public static readonly PrintTemplateFileFormatSE PDF = new PrintTemplateFileFormatSE("PDF", "PDF", () => "Pdf document");

	public static readonly PrintTemplateFileFormatSE PRNX = new PrintTemplateFileFormatSE("PRNX", "PRNX", () => "Prnx document");

	public const string REPXConst = "REPX";

	public const string XMLConst = "XML";

	public const string DOCConst = "DOC";

	public const string DOCXConst = "DOCX";

	public const string DOCMConst = "DOCM";

	public const string XLSConst = "XLS";

	public const string XLSXConst = "XLSX";

	public const string XLSMConst = "XLSM";

	public const string XSDConst = "XSD";

	public const string PDFConst = "PDF";

	public const string PRNXConst = "PRNX";

	private PrintTemplateFileFormatSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
