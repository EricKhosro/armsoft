using System;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(2, "CPAClassification")]
public class CPACodesTypeSE : ASSmartEnum<CPACodesTypeSE, string>
{
	public static readonly CPACodesTypeSE CNFEA = new CPACodesTypeSE("CNFEA", ((byte)1).ToString(), () => Resources.CNFEA, () => Resources.ADGTClassification);

	public static readonly CPACodesTypeSE CPA = new CPACodesTypeSE("CPA", ((byte)2).ToString(), () => Resources.CPA, () => Resources.ADGGAAClassification);

	[JsonIgnore]
	private readonly Func<string> fullCaptionDelegate;

	public string FullCaption => fullCaptionDelegate();

	public CPACodesTypeSE(string name, string value, Func<string> caption, Func<string> fullCaptionDelegate)
		: base(name, value, caption)
	{
		this.fullCaptionDelegate = fullCaptionDelegate;
	}
}
