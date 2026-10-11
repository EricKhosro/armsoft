using System;
using System.Collections.Generic;
using System.Reflection;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.StringResources;
using ArmSoft.SME.Core.UI.Caches;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.Services;

public class StringResourceService : IStringResourceService
{
	public virtual string GetStringResource(string captionId)
	{
		if (string.IsNullOrWhiteSpace(captionId))
		{
			return "";
		}
		string resource = (string)typeof(Resources).GetProperty(captionId)?.GetValue(captionId);
		if (resource == null && TryGetUserDefinedResource(captionId, out resource))
		{
			return resource;
		}
		return resource ?? captionId;
	}

	private static bool TryGetUserDefinedResource(string captionId, out string resource)
	{
		resource = null;
		ResourcesCacheRow item = Settings.ServiceProvider.GetRequiredService<CoreCacheService>().StringResources.GetItem(captionId);
		if (item != null)
		{
			resource = item.Caption;
			return true;
		}
		return false;
	}

	public virtual List<KeyValuePair<string, string>> GetSystemStringResources()
	{
		return GetSystemStringResources(typeof(Resources));
	}

	public StringResource Load(string captionId)
	{
		return Settings.CreateApiClient().ExtensionsRoutes.LoadResource(new StringResourceRequest
		{
			CaptionId = captionId
		});
	}

	public void Store(List<StringResource> resources)
	{
		Settings.CreateApiClient().ExtensionsRoutes.StoreResources(resources);
		Settings.CoreCacheService.StringResources.Refresh();
	}

	public void Update(StringResource resource)
	{
		Settings.CreateApiClient().ExtensionsRoutes.UpdateResource(resource);
		Settings.CoreCacheService.StringResources.Refresh();
	}

	public void Delete(string name)
	{
		Settings.CreateApiClient().ExtensionsRoutes.DeleteResource(name);
		Settings.CoreCacheService.StringResources.Refresh();
	}

	protected List<KeyValuePair<string, string>> GetSystemStringResources(Type type)
	{
		List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
		PropertyInfo[] properties = type.GetProperties(BindingFlags.Static | BindingFlags.Public);
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (propertyInfo.PropertyType == typeof(string))
			{
				list.Add(new KeyValuePair<string, string>(propertyInfo.Name, propertyInfo.GetValue(null, null) as string));
			}
		}
		return list;
	}
}
