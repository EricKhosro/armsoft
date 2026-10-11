using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;
using Armsoft.PrintTemplates2;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.API.Native.Implementation;
using DevExpress.XtraRichEdit.Export;

namespace ArmSoft.PrintTemplates;

public class DocTemplateSubstitutorEx : DocTemplateSubstitutorBase
{
	public DocTemplateSubstitutorEx(IPrintTemplateSubstitution printTemplateSubstitution, string inputFormat, string outputFormat = "", string outputPassword = "", bool protect = false, bool check = false)
		: base(printTemplateSubstitution, inputFormat, outputFormat, outputPassword, protect, check)
	{
	}

	[GeneratedRegex("#\\w+", RegexOptions.Compiled)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "10.0.14.7603")]
	private static Regex SharpRegex()
	{
		return _003CRegexGenerator_g_003EF1D9F198FAF5D9703602819DAD1F763933257B4EA9BB10A6228DBDE2DC60C992D__SharpRegex_0.Instance;
	}

	[GeneratedRegex("\\$\\w+", RegexOptions.Compiled)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "10.0.14.7603")]
	private static Regex DollarRegex()
	{
		return _003CRegexGenerator_g_003EF1D9F198FAF5D9703602819DAD1F763933257B4EA9BB10A6228DBDE2DC60C992D__DollarRegex_1.Instance;
	}

	[GeneratedRegex("\\^\\w+", RegexOptions.Compiled)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "10.0.14.7603")]
	private static Regex CircumflexRegex()
	{
		return _003CRegexGenerator_g_003EF1D9F198FAF5D9703602819DAD1F763933257B4EA9BB10A6228DBDE2DC60C992D__CircumflexRegex_2.Instance;
	}

	[GeneratedRegex("%\\w+\\.\\w+\\b", RegexOptions.Compiled | RegexOptions.Singleline)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "10.0.14.7603")]
	private static Regex TableRegex()
	{
		return _003CRegexGenerator_g_003EF1D9F198FAF5D9703602819DAD1F763933257B4EA9BB10A6228DBDE2DC60C992D__TableRegex_3.Instance;
	}

	private protected override void InnerSubstitute(RichEditDocumentServer srv)
	{
		Document document = srv.Document;
		if (Protect)
		{
			document.Protect(Guid.NewGuid().ToString(), DocumentProtectionType.ReadOnly);
		}
		List<AtomicSubstitution> sortedByLengthSubstitutions = PrintTemplateSubstitution.AtomicSubstitutions.GetSortedByLengthSubstitutions();
		SubstituteAtomicsSharp(document, sortedByLengthSubstitutions);
		SubstituteAtomicsCircumflex(document, sortedByLengthSubstitutions);
		SubstituteAtomicsDollars(document, sortedByLengthSubstitutions);
		TableCollection tables = document.Tables;
		if (tables != null)
		{
			string text = document.Text;
			foreach (NativeTable item in tables.ToList().Cast<NativeTable>())
			{
				if (!item.IsValid)
				{
					continue;
				}
				DocumentRange[] array = document.FindAll(TableRegex(), item.Range);
				if (array == null || array.Length == 0)
				{
					continue;
				}
				DocumentRange documentRange = array[0];
				string text2 = document.GetText(documentRange);
				foreach (IPrintTemplateSubstitutionGrid value in PrintTemplateSubstitution.GridSubstitutions.Substitutions.Values)
				{
					if (text2.Contains("%" + value.Name + ".", StringComparison.InvariantCultureIgnoreCase))
					{
						SubstituteTable(value, item, document, documentRange);
					}
				}
			}
			ValidateGrid(text, document.Text);
		}
		SubstituteImages(document);
	}

	private void SubstituteAtomicsSharp(Document document, List<AtomicSubstitution> atomics)
	{
		string text = document.Text;
		Regex regex = SharpRegex();
		foreach (Section section in document.Sections)
		{
			if (section.HasHeader(HeaderFooterType.Odd))
			{
				SubDocument document2 = section.BeginUpdateHeader(HeaderFooterType.Odd);
				ReplaceAtomic(document2, atomics, regex, "#", (string text2, string sharpWord) => text2.Contains(sharpWord, StringComparison.InvariantCultureIgnoreCase));
				section.EndUpdateHeader(document2);
			}
			if (section.HasFooter(HeaderFooterType.Odd))
			{
				SubDocument document3 = section.BeginUpdateFooter(HeaderFooterType.Odd);
				ReplaceAtomic(document3, atomics, regex, "#", (string text2, string sharpWord) => text2.Contains(sharpWord, StringComparison.InvariantCultureIgnoreCase));
				section.EndUpdateFooter(document3);
			}
		}
		ReplaceAtomic(document, atomics, regex, "#", (string text2, string sharpWord) => text2.Contains(sharpWord, StringComparison.InvariantCultureIgnoreCase));
		ReplaceAtomicInShapes(document.Shapes, atomics, regex, "#", (string text2, string sharpWord) => text2.Contains(sharpWord, StringComparison.InvariantCultureIgnoreCase));
		ValidateAtomic(text, document.Text, (string val) => !PrintTemplateSubstitution.ImageSubstitutions.Contains(val.Substring(1, val.Length - 1)));
	}

	private void SubstituteAtomicsCircumflex(Document document, List<AtomicSubstitution> atomics)
	{
		string text = document.Text;
		ReplaceAtomic(document, atomics, CircumflexRegex(), "^", (string text2, string circumflexWord) => text2.Equals(circumflexWord, StringComparison.InvariantCultureIgnoreCase));
		ReplaceAtomicInShapes(document.Shapes, atomics, CircumflexRegex(), "^", (string text2, string circumflexWord) => text2.Equals(circumflexWord, StringComparison.InvariantCultureIgnoreCase));
		ValidateAtomic(text, document.Text, null, "^");
	}

	private void SubstituteAtomicsDollars(Document document, List<AtomicSubstitution> atomics)
	{
		string text = document.Text;
		ReplaceAtomic(document, atomics, DollarRegex(), "$", (string text2, string dollarWord) => text2.Equals(dollarWord, StringComparison.InvariantCultureIgnoreCase));
		ReplaceAtomicInShapes(document.Shapes, atomics, DollarRegex(), "$", (string text2, string dollarWord) => text2.Equals(dollarWord, StringComparison.InvariantCultureIgnoreCase));
		ValidateAtomic(text, document.Text, null, "$");
	}

	private void SubstituteImages(Document doc)
	{
		List<PrintTemplateSubstitutionImage> sortedByLengthSubstitutions = PrintTemplateSubstitution.ImageSubstitutions.GetSortedByLengthSubstitutions();
		DocumentRange[] array = doc.FindAll(SharpRegex());
		foreach (DocumentRange documentRange in array)
		{
			string text = doc.GetText(documentRange);
			foreach (PrintTemplateSubstitutionImage item in sortedByLengthSubstitutions)
			{
				if (!text.Equals("#" + item.Name, StringComparison.InvariantCultureIgnoreCase))
				{
					continue;
				}
				DocumentPosition start = documentRange.Start;
				doc.Delete(documentRange);
				if (item.File != null)
				{
					using MemoryStream stream = new MemoryStream(item.File);
					DocumentImageSource imageSource = DocumentImageSource.FromStream(stream);
					doc.Images.Insert(start, imageSource).Size = new SizeF((float)item.Width, (float)item.Height);
				}
				break;
			}
		}
	}

	private void ReplaceAtomic(SubDocument document, List<AtomicSubstitution> atomics, Regex regex, string atomic, Func<string, string, bool> check)
	{
		DocumentRange[] array = document.FindAll(regex);
		PlainTextDocumentExporterOptions options = new PlainTextDocumentExporterOptions
		{
			ExportBulletsAndNumbering = false
		};
		DocumentRange[] array2 = array;
		foreach (DocumentRange range in array2)
		{
			string text = document.GetText(range, options);
			foreach (AtomicSubstitution atomic2 in atomics)
			{
				string text2 = atomic + atomic2.Code;
				string newValue = atomic2.Value.ToString();
				if (check(text, text2))
				{
					text = text.Replace(text2, newValue, StringComparison.InvariantCultureIgnoreCase);
				}
			}
			document.Replace(range, text);
		}
	}

	private void ReplaceAtomicInShapes(ShapeCollection shapes, List<AtomicSubstitution> atomics, Regex regex, string atomic, Func<string, string, bool> check)
	{
		foreach (Shape shape in shapes)
		{
			if (shape.ShapeFormat != null && shape.ShapeFormat.HasText)
			{
				ReplaceAtomic(shape.ShapeFormat.TextBox.Document, atomics, regex, atomic, check);
			}
		}
	}
}
