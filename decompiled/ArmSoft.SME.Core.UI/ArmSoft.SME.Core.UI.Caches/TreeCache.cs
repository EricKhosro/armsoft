using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Services;

namespace ArmSoft.SME.Core.UI.Caches;

public class TreeCache : CacheDefinition<TreeCacheRow, string>
{
	public override short CacheId => CacheIdSE.Tree.Value;

	protected override bool InnerRefresh(bool checkChanging, bool ignoreDeleted, out List<string> updatedKeys)
	{
		base.InnerRefresh(checkChanging, ignoreDeleted, out var updatedKeysBase);
		updatedKeys = updatedKeysBase;
		foreach (TreeCacheRow item in ItemsWithKeys.Values.Where((TreeCacheRow p) => updatedKeysBase.Contains(p.Id)))
		{
			if (item.TreeNodeCache == null)
			{
				if (!Settings.CoreCacheService.TreeNodes.TryGetValue(item.CacheId, out var value))
				{
					value = new TreeNodesCache(item.CacheId, item.NodeCaption);
					Settings.CoreCacheService.TreeNodes.Add(item.CacheId, value);
				}
				item.TreeNodeCache = value;
			}
			((TreeNodesCache)item.TreeNodeCache).Refresh(checkChanging: false);
		}
		return true;
	}

	public List<string> GetTreeLeaves(string treeId, string nodes)
	{
		IReadOnlyList<TreeNodesCacheRow> list = ((TreeNodesCache)GetItem(treeId).TreeNodeCache).GetList();
		List<string> list2 = nodes.Split(',').ToList();
		List<string> list3 = new List<string>();
		foreach (TreeNodesCacheRow item in list)
		{
			foreach (TreeNodesCacheRow elInner in list)
			{
				if (item.Path.Left(elInner.Path.Length) == elInner.Path && item.IsLeaf && list2.Exists((string s) => s == elInner.Code))
				{
					list3.Add(item.Code);
				}
			}
		}
		return list3;
	}

	public TreeNodesCache GetTreeNodesCache(string treeId)
	{
		return (TreeNodesCache)GetItem(treeId).TreeNodeCache;
	}
}
