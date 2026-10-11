using System;
using System.Collections.Generic;

namespace ArmSoft.PrintTemplates;

public class ImageSubstitutions
{
	private readonly Dictionary<string, PrintTemplateSubstitutionImage> mSubstitutions;

	public Dictionary<string, PrintTemplateSubstitutionImage> Substitutions => mSubstitutions;

	public ImageSubstitutions()
	{
		mSubstitutions = new Dictionary<string, PrintTemplateSubstitutionImage>(StringComparer.OrdinalIgnoreCase);
	}

	public int Count()
	{
		return mSubstitutions.Count;
	}

	public bool Contains(string name)
	{
		return mSubstitutions.ContainsKey(name);
	}

	public PrintTemplateSubstitutionImage Value(string name)
	{
		return mSubstitutions[name];
	}

	public PrintTemplateSubstitutionImage Add(string name, string fileName = "", byte[] file = null, decimal width = 0m, decimal height = 0m)
	{
		PrintTemplateSubstitutionImage printTemplateSubstitutionImage = new PrintTemplateSubstitutionImage(name, fileName, file, width, height);
		mSubstitutions.Add(name, printTemplateSubstitutionImage);
		return printTemplateSubstitutionImage;
	}

	public PrintTemplateSubstitutionImage Add(string name)
	{
		PrintTemplateSubstitutionImage printTemplateSubstitutionImage = new PrintTemplateSubstitutionImage(name);
		mSubstitutions.Add(name, printTemplateSubstitutionImage);
		return printTemplateSubstitutionImage;
	}

	internal List<PrintTemplateSubstitutionImage> GetSortedByLengthSubstitutions()
	{
		List<PrintTemplateSubstitutionImage> list = new List<PrintTemplateSubstitutionImage>();
		if (mSubstitutions.Count > 0)
		{
			PrintTemplateSubstitutionImage[] array = new PrintTemplateSubstitutionImage[mSubstitutions.Count];
			mSubstitutions.Values.CopyTo(array, 0);
			Array.Sort(array);
			list.AddRange(array);
		}
		return list;
	}
}
