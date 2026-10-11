namespace ArmSoft.PrintTemplates;

public interface IContentProvider
{
	string GetContent(string file);

	string GetContent(byte[] file);
}
