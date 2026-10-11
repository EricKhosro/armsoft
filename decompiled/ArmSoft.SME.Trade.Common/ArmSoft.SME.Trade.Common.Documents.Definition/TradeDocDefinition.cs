using ArmSoft.SME.Common.Documents;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.Documents.Definition;

public class TradeDocDefinition : DocDefinition
{
	public bool ChangingActualnessDate { get; set; } = true;

	public TradeDocDefinition(DocumentTypeSE docType)
	{
		POType = docType.Value;
		CaptionDelegate = docType.CaptionDelegate;
		FullCaptionDelegate = docType.FullCaptionDelegate;
	}
}
