using System.Collections.Generic;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class PropertyCache : CacheDefinition<PropertyCacheRow, string>
{
	public override short CacheId => CacheIdSE.Properties.Value;

	public PropertyCacheRow GetPropertyByCaption(string caption, string destCode = "")
	{
		IReadOnlyList<PropertyCacheRow> list = GetList((PropertyCacheRow p) => p.Caption == caption && p.Code == destCode);
		if (list.Count > 0)
		{
			return list[0];
		}
		return null;
	}
}
