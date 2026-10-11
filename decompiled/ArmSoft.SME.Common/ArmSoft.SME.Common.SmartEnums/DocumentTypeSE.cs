using System;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(2, "DocumentType")]
[SmartEnumPartial(SmartEnumType.Base)]
public class DocumentTypeSE : ASSmartEnum<DocumentTypeSE, short>
{
	public static readonly DocumentTypeSE TaxDocument = new DocumentTypeSE("TaxDocument", 231, () => Resources.SettlementDocument, () => Resources.SettlementDocument, 0);

	public const short TaxDocumentConst = 231;

	[JsonIgnore]
	public readonly Func<string> FullCaptionDelegate;

	public readonly short DataProviderType;

	public string FullCaption => FullCaptionDelegate();

	public DocumentTypeSE(string name, short value, Func<string> caption, Func<string> fullCaption = null, short dataProviderType = 0)
		: base(name, value, caption)
	{
		FullCaptionDelegate = ((fullCaption == null) ? caption : fullCaption);
		DataProviderType = dataProviderType;
	}
}
