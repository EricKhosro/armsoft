using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "TypeWord")]
public class ApiKeyTypesSE : ASSmartEnum<ApiKeyTypesSE, short>
{
	public static readonly ApiKeyTypesSE PublicApi = new ApiKeyTypesSE("PublicApi", 1, () => Resources.PublicApi);

	public static readonly ApiKeyTypesSE ArmSoftIntegration = new ApiKeyTypesSE("ArmSoftIntegration", 2, () => Resources.ArmSoftIntegration);

	public ApiKeyTypesSE(string name, short value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
