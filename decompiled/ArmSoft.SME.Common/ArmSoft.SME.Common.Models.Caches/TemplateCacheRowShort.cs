namespace ArmSoft.SME.Common.Models.Caches;

public class TemplateCacheRowShort
{
	public string Name { get; set; }

	public string Caption { get; set; }

	public TemplateCacheRowShort()
	{
	}

	public TemplateCacheRowShort(string name, string caption)
	{
		Name = name;
		Caption = caption;
	}
}
