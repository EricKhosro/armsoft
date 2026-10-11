using System.Collections.Generic;
using ArmSoft.SME.Common.MetaData.Document;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Services;

namespace ArmSoft.SME.Core.UI.Caches;

public class DocumentExtensionsCache : CacheDefinition<DocumentExtensionCacheRow, DocumentExtensionKey>
{
	public override short CacheId => CacheIdSE.DocumentExtensions.Value;

	public static List<ViewColumn> GetDataViewColumns(short poTypeDef, short poType)
	{
		return ViewColumn.AddXMLColumns(poTypeDef, poType, "", "DocumentExtension", addGroupToCaption: false);
	}
}
