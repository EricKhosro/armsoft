namespace ArmSoft.SME.Common.Models;

public class POStoreRequest
{
	public bool WithoutValidation { get; set; }

	public bool WithoutRequiredCheck { get; set; }

	public bool WithoutAccessCheck { get; set; }

	public bool WithoutGrids { get; set; }

	public string LogComment { get; set; } = string.Empty;

	public bool IsGroupEdited { get; set; }

	public bool IsImported { get; set; }

	public bool IgnoreWarnings { get; set; }

	public bool CheckActive { get; set; }

	public bool AfterStoreInTrans { get; set; }
}
