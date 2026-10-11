namespace ArmSoft.SME.Common.Models;

public class PODeleteRequest
{
	public bool WithoutValidation { get; set; }

	public string LogComment { get; set; } = string.Empty;

	public bool IsGroupDelete { get; set; }

	public bool IsFullDelete { get; set; }

	public bool IgnoreWarnings { get; set; }

	public bool FromImport { get; set; }
}
