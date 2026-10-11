using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArmSoft.SME.Client.JsonSerialization;
using ArmSoft.SME.Trade.Client.Models.Exchange;

namespace ArmSoft.SME.Trade.Client.JsonSerialization;

public class TradeExchangeTypeResolver : ExchangeTypeResolver
{
	protected override List<Type> GetInheritedClasses(Type baseType)
	{
		List<Type> inheritedClasses = base.GetInheritedClasses(baseType);
		inheritedClasses.AddRange((from m in Assembly.GetAssembly(typeof(TradeDocumentExchangeBase)).GetTypes()
			where m.IsClass && !m.IsAbstract && m.IsSubclassOf(baseType)
			select m).ToList());
		return inheritedClasses;
	}
}
