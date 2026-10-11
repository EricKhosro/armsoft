using System;

namespace ArmSoft.PrintTemplates;

public class PrintTemplateSubstitutionImage : IComparable<PrintTemplateSubstitutionImage>
{
	public string Name { get; }

	public decimal Width { get; set; }

	public decimal Height { get; set; }

	public byte[] File { get; set; }

	public string FileName { get; set; }

	public PrintTemplateSubstitutionImage(string name, string fileName = "", byte[] file = null, decimal width = 0m, decimal height = 0m)
	{
		Name = name;
		FileName = fileName;
		File = file;
		Width = width;
		Height = height;
	}

	public int CompareTo(PrintTemplateSubstitutionImage other)
	{
		if (other == null || other.Name == null)
		{
			return -1;
		}
		int num = -Name.Length.CompareTo(other.Name.Length);
		if (num != 0)
		{
			return num;
		}
		return Name.CompareTo(other.Name);
	}
}
