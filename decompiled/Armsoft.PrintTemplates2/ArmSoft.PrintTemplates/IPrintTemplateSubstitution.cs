namespace ArmSoft.PrintTemplates;

public interface IPrintTemplateSubstitution
{
	IAtomicSubstitutions AtomicSubstitutions { get; }

	IGridSubstitutions GridSubstitutions { get; }

	ImageSubstitutions ImageSubstitutions { get; }

	void ConvertToArmenianUnicode();
}
