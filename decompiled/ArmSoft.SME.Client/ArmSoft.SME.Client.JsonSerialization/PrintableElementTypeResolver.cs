using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using ArmSoft.SME.Common.Models.PrintableElements;

namespace ArmSoft.SME.Client.JsonSerialization;

public class PrintableElementTypeResolver : DefaultJsonTypeInfoResolver
{
	public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
	{
		JsonTypeInfo typeInfo = base.GetTypeInfo(type, options);
		Type typeFromHandle = typeof(PrintableElement);
		if (typeInfo.Type == typeFromHandle)
		{
			typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
			{
				TypeDiscriminatorPropertyName = "$printelementmodeltype",
				IgnoreUnrecognizedTypeDiscriminators = true,
				UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization
			};
			foreach (Type inheritedClass in GetInheritedClasses(typeFromHandle))
			{
				typeInfo.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(inheritedClass, inheritedClass.Name));
			}
		}
		return typeInfo;
	}

	protected virtual List<Type> GetInheritedClasses(Type baseType)
	{
		return (from m in Assembly.GetAssembly(baseType).GetTypes()
			where m.IsClass && !m.IsAbstract && m.IsSubclassOf(baseType)
			select m).ToList();
	}
}
