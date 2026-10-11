using System;
using System.Linq;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Services;

namespace ArmSoft.SME.Core.UI.Caches;

public class UserCache : CacheDefinition<UsersCacheRow, int>
{
	public override short CacheId => CacheIdSE.Users.Value;

	public override bool StoreInLocalStorage => true;

	public UsersCacheRow GetItemByName(string name)
	{
		return GetList().FirstOrDefault((UsersCacheRow p) => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
	}

	public string GetNameById(int id)
	{
		UsersCacheRow item = GetItem(id);
		if (item != null)
		{
			return item.Name;
		}
		return $"{id} user";
	}
}
