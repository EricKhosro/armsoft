using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Common.Directories;
using ArmSoft.SME.Common.Helpers;
using ArmSoft.SME.Trade.Common.Directories.Definition;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.Helpers;

public class TradePODefinitionHelper : PODefinitionHelper
{
	public override List<short> GetExchangableDirectoryTypes()
	{
		List<short> exchangableDirectoryTypes = base.GetExchangableDirectoryTypes();
		exchangableDirectoryTypes.Add(DirectoryTypeSE.MTComplects.Value);
		return Enumerable.Except(second: new List<short>
		{
			AllDirectoryDefinitions.CPAClassifier.POType,
			TradeAllDirectoryDefinitions.ItemBarCodes.POType,
			TradeAllDirectoryDefinitions.Discount.POType,
			TradeAllDirectoryDefinitions.BonusScale.POType,
			TradeAllDirectoryDefinitions.Segment.POType,
			TradeAllDirectoryDefinitions.SubstituteItemsGroups.POType,
			TradeAllDirectoryDefinitions.GoodsBySegment.POType,
			TradeAllDirectoryDefinitions.OrderMinMax.POType,
			TradeAllDirectoryDefinitions.PartnersGood.POType,
			TradeAllDirectoryDefinitions.MTComplects.POType
		}, first: exchangableDirectoryTypes.Concat(GetExchangablePOTypes<TradeAllDirectoryDefinitions>())).ToList();
	}

	public override List<short> GetLoggablePOTypes()
	{
		List<short> loggablePOTypes = base.GetLoggablePOTypes();
		return Enumerable.Except(second: new List<short> { TradeAllDirectoryDefinitions.Contract.POType }, first: loggablePOTypes.Concat(GetLoggablePOTypes<TradeAllDirectoryDefinitions>())).ToList();
	}
}
