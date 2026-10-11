using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArmSoft.SME.Client.JsonSerialization;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;

namespace ArmSoft.SME.Trade.Client.JsonSerialization;

public class TradePrintableElementTypeResolver : PrintableElementTypeResolver
{
	protected override List<Type> GetInheritedClasses(Type baseType)
	{
		List<Type> inheritedClasses = base.GetInheritedClasses(baseType);
		inheritedClasses.AddRange((from m in Assembly.GetAssembly(typeof(IDCard)).GetTypes()
			where m.IsClass && !m.IsAbstract && m.IsSubclassOf(baseType)
			select m).ToList());
		return inheritedClasses;
	}
}
