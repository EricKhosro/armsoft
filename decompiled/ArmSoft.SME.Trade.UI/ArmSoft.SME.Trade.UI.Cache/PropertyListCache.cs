using System.Linq;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class PropertyListCache : CacheDefinition<PropertyListCacheRow, PropertyListCacheRow.RowKey>
{
	public override short CacheId => CacheIdSE.PropertiesList.Value;

	public PropertyListCacheRow GetPropertyByCaption(string caption, string destCode = "")
	{
		return (from p in GetList((PropertyListCacheRow p) => p.Caption == caption && p.DestCode == destCode)
			orderby p.Code
			select p).FirstOrDefault();
	}
}
