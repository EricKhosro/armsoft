using System.Text.Json;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Common.Models.Caches;

public class TemplatesCacheRow : CacheRowBase<string>
{
	public string Name { get; set; }

	public string Caption { get; set; }

	public short DocType { get; set; }

	public PrintTemplateFileFormatSE FileFormat { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Name);

	public TemplatesCacheRow()
	{
	}

	public TemplatesCacheRow(string name, string caption, short docType, PrintTemplateFileFormatSE fileFormat, byte[] ts)
	{
		Name = name;
		Caption = caption;
		DocType = docType;
		FileFormat = fileFormat;
		TS = ts;
	}
}
