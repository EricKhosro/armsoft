using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Armsoft.PrintTemplates2.Resources;
using DevExpress.Spreadsheet;
using DevExpress.XtraPrinting;
using Microsoft.VisualBasic;

namespace ArmSoft.PrintTemplates;

public abstract class SpreadsheetTemplateSubstitutorBase : TemplateSubstitutor
{
	protected readonly Regex TableRegEx = new Regex("%(?<TableName>[\\w]+)\\.(?<ColumnName>[\\w]+)", RegexOptions.Compiled);

	protected readonly Regex TableRegExRow = new Regex("^.*?(%(?<TableName>\\w+)\\.(?<ColumnName>\\w+))+.*?$", RegexOptions.Multiline | RegexOptions.Compiled);

	protected DocumentFormat InputDocumentFormat { get; set; }

	protected DocumentFormat OutputDocumentFormat { get; set; }

	private bool ExportToPdf { get; set; }

	private PdfExportOptions PdfExportOptions { get; set; } = new PdfExportOptions();

	protected string OutputDocumentPassword { get; set; }

	public SpreadsheetTemplateSubstitutorBase(IPrintTemplateSubstitution printTemplateSubstitution, string inputFormat, string outputFormat = "", string outputPassword = "", bool check = false)
		: base(printTemplateSubstitution, check)
	{
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
			InputDocumentFormat = GetFormat(inputFormat);
			OutputDocumentFormat = (string.IsNullOrWhiteSpace(outputFormat) ? InputDocumentFormat : GetFormat(outputFormat));
			OutputDocumentPassword = outputPassword;
		}
	}

	public override void SaveToFile(string file)
	{
		using Workbook workbook = new Workbook();
		DoSubstitute(workbook, file);
		if (ExportToPdf)
		{
			workbook.ExportToPdf(file, PdfExportOptions);
			return;
		}
		if (!string.IsNullOrWhiteSpace(OutputDocumentPassword))
		{
			workbook.DocumentSettings.Encryption.Password = OutputDocumentPassword;
		}
		workbook.SaveDocument(file, OutputDocumentFormat);
	}

	public override Stream GetStream(byte[] file)
	{
		using Workbook workbook = new Workbook();
		DoSubstitute(workbook, file);
		MemoryStream memoryStream;
		if (ExportToPdf)
		{
			memoryStream = new MemoryStream();
			workbook.ExportToPdf(memoryStream, PdfExportOptions);
			memoryStream.Seek(0L, SeekOrigin.Begin);
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(OutputDocumentPassword))
			{
				workbook.DocumentSettings.Encryption.Password = OutputDocumentPassword;
			}
			memoryStream = new MemoryStream(workbook.SaveDocument(OutputDocumentFormat));
		}
		return memoryStream;
	}

	public override async Task SaveToFileAsync(string file)
	{
		using Workbook workbook = new Workbook();
		DoSubstitute(workbook, file);
		if (ExportToPdf)
		{
			await workbook.ExportToPdfAsync(file, PdfExportOptions);
			return;
		}
		if (!string.IsNullOrWhiteSpace(OutputDocumentPassword))
		{
			workbook.DocumentSettings.Encryption.Password = OutputDocumentPassword;
		}
		await workbook.SaveDocumentAsync(file, OutputDocumentFormat);
	}

	public override async Task<Stream> GetStreamAsync(byte[] file)
	{
		using Workbook workbook = new Workbook();
		DoSubstitute(workbook, file);
		MemoryStream stream;
		if (ExportToPdf)
		{
			stream = new MemoryStream();
			await workbook.ExportToPdfAsync(stream, PdfExportOptions);
			stream.Seek(0L, SeekOrigin.Begin);
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(OutputDocumentPassword))
			{
				workbook.DocumentSettings.Encryption.Password = OutputDocumentPassword;
			}
			stream = new MemoryStream(await workbook.SaveDocumentAsync(OutputDocumentFormat));
		}
		return stream;
	}

	private void DoSubstitute(Workbook workbook, string file)
	{
		if (workbook.LoadDocument(file, InputDocumentFormat))
		{
			InnerSubstitute(workbook);
			return;
		}
		throw new Exception(Resource.WrongFileExtension);
	}

	private void DoSubstitute(Workbook workbook, byte[] file)
	{
		if (workbook.LoadDocument(file, InputDocumentFormat))
		{
			InnerSubstitute(workbook);
			return;
		}
		throw new Exception(Resource.WrongFileExtension);
	}

	private DocumentFormat GetFormat(string format)
	{
		return format switch
		{
			"XLSM" => DocumentFormat.Xlsm, 
			"XLSX" => DocumentFormat.Xlsx, 
			"TEXT" => DocumentFormat.Text, 
			"CSV" => DocumentFormat.Csv, 
			_ => throw new Exception(format + " format is not supported"), 
		};
	}

	private void InnerSubstitute(Workbook workbook)
	{
		workbook.BeginUpdate();
		try
		{
			SubstituteAtomicsSharp(workbook);
			SubstituteAtomicsCircumflex(workbook);
			SubstituteTables(workbook);
			SubstituteImages(workbook);
		}
		finally
		{
			workbook.EndUpdate();
		}
	}

	protected abstract void SubstituteTables(Workbook workbook);

	private void SubstituteAtomicsSharp(Workbook workbook)
	{
		List<AtomicSubstitution> sortedByLengthSubstitutions = PrintTemplateSubstitution.AtomicSubstitutions.GetSortedByLengthSubstitutions();
		foreach (Worksheet item in workbook.Sheets.Cast<Worksheet>())
		{
			foreach (Cell item2 in item.Search("#"))
			{
				if (!item2.Value.IsText)
				{
					continue;
				}
				if (item2.HasRichText)
				{
					RichTextString richTextString = new RichTextString();
					foreach (RichTextRun run in item2.GetRichText().Runs)
					{
						string text = run.Text;
						string text2 = run.Text;
						if (run.Text.Contains('#'))
						{
							foreach (AtomicSubstitution item3 in sortedByLengthSubstitutions)
							{
								string text3 = "#" + item3.Code;
								string text4 = item3.Value.ToString();
								if (Strings.InStr(text, text3, CompareMethod.Text) > 0)
								{
									text = Strings.Replace(text, text3, (text4 == string.Empty) ? " " : text4, 1, -1, CompareMethod.Text);
								}
							}
							ValidateAtomic(text2, text, (string val) => !PrintTemplateSubstitution.ImageSubstitutions.Contains(val.Substring(1, val.Length - 1)));
						}
						richTextString.AddTextRun(text, run.Font);
					}
					item2.SetRichText(richTextString);
					continue;
				}
				string displayText = item2.DisplayText;
				foreach (AtomicSubstitution item4 in sortedByLengthSubstitutions)
				{
					string text5 = "#" + item4.Code;
					string text6 = item4.Value.ToString();
					if (item2.Value.TextValue.Trim().Equals(text5, StringComparison.InvariantCultureIgnoreCase))
					{
						item2.SetValue(item4.Value);
						break;
					}
					if (Strings.InStr(item2.Value.TextValue, text5, CompareMethod.Text) > 0)
					{
						item2.Value = Strings.Replace(item2.Value.TextValue, text5, (text6 == string.Empty) ? " " : text6, 1, -1, CompareMethod.Text);
					}
				}
				ValidateAtomic(displayText, item2.DisplayText, (string val) => !PrintTemplateSubstitution.ImageSubstitutions.Contains(val.Substring(1, val.Length - 1)));
			}
		}
	}

	private void SubstituteAtomicsCircumflex(Workbook workbook)
	{
		List<AtomicSubstitution> sortedByLengthSubstitutions = PrintTemplateSubstitution.AtomicSubstitutions.GetSortedByLengthSubstitutions();
		foreach (Worksheet item in workbook.Sheets.Cast<Worksheet>())
		{
			foreach (Cell item2 in item.Search("^"))
			{
				if (!item2.Value.IsText)
				{
					continue;
				}
				if (item2.HasRichText)
				{
					RichTextString richTextString = new RichTextString();
					foreach (RichTextRun run in item2.GetRichText().Runs)
					{
						string text = run.Text;
						if (run.Text.Contains('^'))
						{
							string text2 = run.Text;
							foreach (AtomicSubstitution item3 in sortedByLengthSubstitutions)
							{
								string text3 = "^" + item3.Code;
								string text4 = item3.Value.ToString();
								if (Strings.InStr(text, text3, CompareMethod.Text) > 0)
								{
									if (Strings.Trim(text4) == string.Empty)
									{
										break;
									}
									text = Strings.Replace(text, text3, text4, 1, -1, CompareMethod.Text);
								}
							}
							ValidateAtomic(text2, text, (string val) => !PrintTemplateSubstitution.ImageSubstitutions.Contains(val.Substring(1, val.Length - 1)), "^");
						}
						richTextString.AddTextRun(text, run.Font);
					}
					item2.SetRichText(richTextString);
					continue;
				}
				string displayText = item2.DisplayText;
				foreach (AtomicSubstitution item4 in sortedByLengthSubstitutions)
				{
					string text5 = "^" + item4.Code;
					if (item2.Value.TextValue.Trim().Equals(text5, StringComparison.InvariantCultureIgnoreCase))
					{
						item2.SetValue(item4.Value);
						break;
					}
					if (Strings.InStr(item2.Value.TextValue, text5, CompareMethod.Text) > 0)
					{
						item2.Value = Strings.Replace(item2.Value.TextValue, text5, item4.Value.ToString(), 1, -1, CompareMethod.Text);
					}
				}
				ValidateAtomic(displayText, item2.DisplayText, (string val) => !PrintTemplateSubstitution.ImageSubstitutions.Contains(val.Substring(1, val.Length - 1)), "^");
			}
		}
	}

	private void SubstituteImages(Workbook workbook)
	{
		if (PrintTemplateSubstitution.ImageSubstitutions.Count() == 0)
		{
			return;
		}
		List<PrintTemplateSubstitutionImage> sortedByLengthSubstitutions = PrintTemplateSubstitution.ImageSubstitutions.GetSortedByLengthSubstitutions();
		foreach (Worksheet item in workbook.Sheets.Cast<Worksheet>())
		{
			foreach (Cell item2 in item.Search("#"))
			{
				if (!item2.Value.IsText)
				{
					continue;
				}
				_ = item2.RowIndex;
				_ = item2.ColumnIndex;
				foreach (PrintTemplateSubstitutionImage item3 in sortedByLengthSubstitutions)
				{
					if (string.Compare(item2.Value.TextValue, "#" + item3.Name, ignoreCase: true) == 0)
					{
						using MemoryStream stream = new MemoryStream(item3.File);
						SpreadsheetImageSource imageSource = SpreadsheetImageSource.FromStream(stream);
						Picture picture = item.Pictures.AddPicture(imageSource, item2);
						picture.Height = (float)item3.Height;
						picture.Width = (float)item3.Width;
						picture.Name = item3.Name;
						picture.LockAspectRatio = true;
						item2.Value = "";
					}
				}
			}
		}
	}
}
