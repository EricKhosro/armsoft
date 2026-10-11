using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Core.UI.DropDownsData;

public class StringResourcesDataProvider : DataProvider<UserResourcesDataProvider.DataRow, ExtendableParameter>
{
	public override string Name => CacheIdSE.StringResources.Name;

	public StringResourcesDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = Resources.Columns
		};
		int num = 3;
		List<Column> list = new List<Column>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<Column> span = CollectionsMarshal.AsSpan(list);
		span[0] = new Column
		{
			Name = "Name",
			Caption = Resources.Code,
			Source = "Name"
		};
		span[1] = new Column
		{
			Name = "Caption",
			Caption = Resources.Name,
			Source = "Caption"
		};
		span[2] = new Column
		{
			Name = "System",
			Caption = Resources.System,
			Source = "System",
			Type = SupportedType.Boolean
		};
		schema.Columns = list;
		return schema;
	}

	public override DataProviderResult<UserResourcesDataProvider.DataRow> Execute(ExtendableParameter param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		if (settings != null && settings.ReloadData)
		{
			ArmSoft.SME.Core.UI.Settings.CoreCacheService.StringResources.Refresh();
		}
		DataProviderResult<UserResourcesDataProvider.DataRow> dataProviderResult = new DataProviderResult<UserResourcesDataProvider.DataRow>();
		List<KeyValuePair<string, string>> systemStringResources = ArmSoft.SME.Core.UI.Settings.StringResourceService.GetSystemStringResources();
		IReadOnlyList<ResourcesCacheRow> list = ArmSoft.SME.Core.UI.Settings.CoreCacheService.StringResources.GetList();
		foreach (KeyValuePair<string, string> item in systemStringResources.OrderBy((KeyValuePair<string, string> el) => el.Key))
		{
			dataProviderResult.Rows.Add(new UserResourcesDataProvider.DataRow
			{
				Name = item.Key,
				Caption = item.Value,
				System = true
			});
		}
		foreach (ResourcesCacheRow item2 in list.OrderBy((ResourcesCacheRow el) => el.Name))
		{
			dataProviderResult.Rows.Add(new UserResourcesDataProvider.DataRow
			{
				Name = item2.Name,
				Caption = item2.Caption
			});
		}
		return dataProviderResult;
	}
}
