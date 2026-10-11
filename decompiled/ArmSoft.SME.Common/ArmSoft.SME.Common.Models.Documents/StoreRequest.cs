using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Common.Models.Documents;

public class StoreRequest : POStoreRequest
{
	public byte State { get; set; }

	public bool IsDataChanged { get; set; }

	public bool RegenerateEntries { get; set; }

	public bool DeleteEntries { get; set; }

	public StoreMode StoreMode { get; set; }

	public DocNumOfferSource DocNumOfferSource { get; set; } = DocNumOfferSource.FromParam;

	public StoreRequest()
	{
		State = DocumentStateSE.Processed.Value;
		IsDataChanged = true;
		IsGroupEdited = false;
		RegenerateEntries = false;
		DeleteEntries = false;
		StoreMode = StoreMode.All;
		LogComment = "";
		CheckActive = true;
	}
}
