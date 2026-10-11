namespace ArmSoft.SME.Common.Models.UnboundColumns;

public class UnboundColumn
{
	public string Group { get; set; }

	public string Name { get; set; }

	public string Caption { get; set; }

	public string CaptionEn { get; set; }

	public string CaptionRu { get; set; }

	public short Type { get; set; }

	public short Precision { get; set; }

	public bool IsEnabled { get; set; }

	public string Expression { get; set; }

	public int UserId { get; set; }

	public bool IsStored { get; set; }

	public short RowNum { get; set; }
}
