using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class ProducerCacheRow : CodeCaptionPairsCacheRow
{
	public const string ProducerCache = "ProducerCache";

	public object GetReferenceValue(string name)
	{
		if (!(name == "ProducerCacheCode"))
		{
			if (name == "ProducerCacheName")
			{
				return Name;
			}
			return null;
		}
		return Code;
	}
}
