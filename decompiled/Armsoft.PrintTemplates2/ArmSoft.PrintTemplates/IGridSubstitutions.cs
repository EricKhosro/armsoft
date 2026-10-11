using System.Collections.Generic;

namespace ArmSoft.PrintTemplates;

public interface IGridSubstitutions
{
	Dictionary<string, IPrintTemplateSubstitutionGrid> Substitutions { get; }

	bool Contains(string name);

	int Count();

	IPrintTemplateSubstitutionGrid Value(string name);

	IPrintTemplateSubstitutionGrid Add(string name);

	internal void ConvertToArmenianUnicode();
}
