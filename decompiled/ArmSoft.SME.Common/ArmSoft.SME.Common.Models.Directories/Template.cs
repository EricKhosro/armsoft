namespace ArmSoft.SME.Common.Models.Directories;

public class Template
{
	public string Name { get; set; }

	public string Caption { get; set; }

	public short DocType { get; set; }

	public bool Enabled { get; set; }

	public bool IsSystem { get; set; }

	public byte[] TS { get; set; }

	public bool StoredInDb { get; set; }

	public string TemplateType { get; set; }
}
