namespace ArmSoft.SME.Common.MetaData.Tree;

public class TreeExtensionModel
{
	public string Name { get; set; }

	public string Xml { get; set; }

	public TreeExtensionModel(string name, string xml)
	{
		Name = name;
		Xml = xml;
	}
}
