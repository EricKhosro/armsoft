using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class StandardCommentsCache : CacheDefinition<CodeCaptionPairsCacheRow, string>
{
	public override short CacheId => CacheIdSE.StandardComments.Value;

	public List<string> GetCaptionsList()
	{
		if (Settings.IsOffline)
		{
			return new List<string>();
		}
		return (from x in GetList()
			orderby x.Code
			select x.Name).ToList();
	}
}
