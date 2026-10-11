namespace ArmSoft.PrintTemplates;

public abstract class PrintTemplateSubstitution<A, G> : IPrintTemplateSubstitution where A : IAtomicSubstitutions, new() where G : IGridSubstitutions, new()
{
	public A AtomicSubstitutions { get; private set; }

	public G GridSubstitutions { get; private set; }

	public ImageSubstitutions ImageSubstitutions { get; private set; }

	IAtomicSubstitutions IPrintTemplateSubstitution.AtomicSubstitutions => AtomicSubstitutions;

	IGridSubstitutions IPrintTemplateSubstitution.GridSubstitutions => GridSubstitutions;

	public PrintTemplateSubstitution()
	{
		AtomicSubstitutions = new A();
		GridSubstitutions = new G();
		ImageSubstitutions = new ImageSubstitutions();
	}

	public void ConvertToArmenianUnicode()
	{
		AtomicSubstitutions.ConvertToArmenianUnicode();
		GridSubstitutions.ConvertToArmenianUnicode();
	}
}
public sealed class PrintTemplateSubstitution : PrintTemplateSubstitution<AtomicSubstitutions, GridSubstitutions>
{
}
