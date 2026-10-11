using System.Collections.Generic;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class ProducerCache : CacheDefinition<ProducerCacheRow, string>
{
	public override short CacheId => CacheIdSE.Producers.Value;

	public static List<ViewColumn> GetDataViewColumns(string group, string namePrefix, string captionPrefix)
	{
		return new List<ViewColumn>
		{
			new ViewColumn(namePrefix + "Name", captionPrefix + Resources.Name, "Name", DataColumnType.String, group, "ProducerCache")
		};
	}
}
