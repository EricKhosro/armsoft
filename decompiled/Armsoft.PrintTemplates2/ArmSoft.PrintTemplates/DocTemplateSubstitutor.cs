using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Armsoft.PrintTemplates2;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.API.Native.Implementation;

namespace ArmSoft.PrintTemplates;

public class DocTemplateSubstitutor : DocTemplateSubstitutorBase
{
	public DocTemplateSubstitutor(IPrintTemplateSubstitution printTemplateSubstitution, string inputFormat, string outputFormat = "", string outputPassword = "", bool protect = false, bool check = false)
		: base(printTemplateSubstitution, inputFormat, outputFormat, outputPassword, protect, check)
	{
	}

	private protected override void InnerSubstitute(RichEditDocumentServer srv)
	{
		Document document = srv.Document;
		if (Protect)
		{
			document.Protect(Guid.NewGuid().ToString(), DocumentProtectionType.ReadOnly);
		}
		SubstituteAtomicsSharp(document);
		SubstituteAtomicsCircumflex(document);
		SubstituteAtomicsDollars(document);
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
				foreach (IPrintTemplateSubstitutionGrid value in PrintTemplateSubstitution.GridSubstitutions.Substitutions.Values)
				{
					string text2 = "%" + value.Name;
					DocumentRange[] array = document.FindAll(new Regex(text2 + ".\\w+\\b", RegexOptions.IgnoreCase | RegexOptions.Singleline), item.Range);
					if (array.Length != 0)
					{
						SubstituteTable(value, item, document, array[0]);
					}
				}
			}
			ValidateGrid(text, document.Text);
		}
		SubstituteImages(document);
	}

	private void SubstituteAtomicsSharp(Document doc)
	{
		string text = doc.Text;
		foreach (AtomicSubstitution sortedByLengthSubstitution in PrintTemplateSubstitution.AtomicSubstitutions.GetSortedByLengthSubstitutions())
		{
			string text2 = sortedByLengthSubstitution.Value.ToString();
			string text3 = "#" + sortedByLengthSubstitution.Code;
			DocumentRange[] array = doc.FindAll(text3, SearchOptions.None);
			foreach (DocumentRange range in array)
			{
				doc.Replace(range, text2);
			}
			SubstituteAtomicsInShapes(doc.Shapes, text3, text2);
			foreach (Section section in doc.Sections)
			{
				if (section.HasHeader(HeaderFooterType.Odd))
				{
					SubDocument subDocument = section.BeginUpdateHeader(HeaderFooterType.Odd);
					array = subDocument.FindAll(text3, SearchOptions.None);
					foreach (DocumentRange range2 in array)
					{
						subDocument.Replace(range2, text2);
					}
					section.EndUpdateHeader(subDocument);
				}
				if (section.HasFooter(HeaderFooterType.Odd))
				{
					SubDocument subDocument2 = section.BeginUpdateFooter(HeaderFooterType.Odd);
					array = subDocument2.FindAll(text3, SearchOptions.None);
					foreach (DocumentRange range3 in array)
					{
						subDocument2.Replace(range3, text2);
					}
					section.EndUpdateFooter(subDocument2);
				}
			}
		}
		ValidateAtomic(text, doc.Text, (string val) => !PrintTemplateSubstitution.ImageSubstitutions.Contains(val.Substring(1, val.Length - 1)));
	}

	private void SubstituteAtomicsCircumflex(Document doc)
	{
		SubstituteAtomics(doc, "^");
	}

	private void SubstituteAtomicsDollars(Document doc)
	{
		SubstituteAtomics(doc, "$");
	}

	private void SubstituteImages(Document doc)
	{
		foreach (PrintTemplateSubstitutionImage sortedByLengthSubstitution in PrintTemplateSubstitution.ImageSubstitutions.GetSortedByLengthSubstitutions())
		{
			string textToFind = "#" + sortedByLengthSubstitution.Name;
			DocumentRange[] array = doc.FindAll(textToFind, SearchOptions.WholeWord, doc.Range);
			foreach (DocumentRange documentRange in array)
			{
				DocumentPosition start = documentRange.Start;
				doc.Delete(documentRange);
				if (sortedByLengthSubstitution.File != null)
				{
					using MemoryStream stream = new MemoryStream(sortedByLengthSubstitution.File);
					DocumentImageSource imageSource = DocumentImageSource.FromStream(stream);
					doc.Images.Insert(start, imageSource).Size = new SizeF((float)sortedByLengthSubstitution.Width, (float)sortedByLengthSubstitution.Height);
				}
			}
		}
	}

	private void SubstituteAtomics(Document doc, string atomic)
	{
		string text = doc.Text;
		foreach (AtomicSubstitution sortedByLengthSubstitution in PrintTemplateSubstitution.AtomicSubstitutions.GetSortedByLengthSubstitutions())
		{
			string text2 = sortedByLengthSubstitution.Value.ToString();
			string text3 = atomic + sortedByLengthSubstitution.Code;
			SubstituteAtomicsInShapes(doc.Shapes, text3, text2);
			DocumentRange[] array = doc.FindAll(text3, SearchOptions.WholeWord);
			foreach (DocumentRange documentRange in array)
			{
				if (string.IsNullOrWhiteSpace(text2))
				{
					Paragraph paragraph = doc.Paragraphs.Get(documentRange.Start);
					doc.Delete(paragraph.Range);
				}
				else
				{
					doc.Replace(documentRange, text2);
				}
			}
		}
		ValidateAtomic(text, doc.Text, null, atomic);
	}

	private void SubstituteAtomicsInShapes(ShapeCollection shapes, string word, string value)
	{
		foreach (Shape shape in shapes)
		{
			if (shape.ShapeFormat != null && shape.ShapeFormat.HasText)
			{
				SubDocument document = shape.ShapeFormat.TextBox.Document;
				DocumentRange[] array = document.FindAll(word, SearchOptions.None);
				foreach (DocumentRange range in array)
				{
					document.Replace(range, value);
				}
			}
		}
	}
}
