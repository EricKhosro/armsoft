using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI.Caches;

namespace ArmSoft.SME.Core.UI.DropDownsData;

public class TreeNodesDataProvider : DataProvider<TreeNodesDataProvider.DataRow, TreeNodesDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Code { get; set; }

		public string Name { get; set; }

		public string Parent { get; set; }

		public bool IsClosed { get; set; }

		public string Spec { get; set; }

		public bool IsLeaf { get; set; }

		public DataRow()
		{
		}

		public DataRow(string code, string caption, string parent, string spec, bool isLeaf, bool isClosed)
		{
			Code = code;
			Name = caption;
			Parent = parent;
			Spec = spec;
			IsLeaf = isLeaf;
			IsClosed = isClosed;
		}
	}

	public class Param
	{
		public string ParentCode { get; set; }

		public bool ShowOnlyLeafNodes { get; set; }

		public bool ShowClosed { get; set; }
	}

	private readonly TreeCacheRow _treeCacheRow;

	public override string Name => "TreeNodes";

	public TreeNodesDataProvider(string treeId)
		: base((ApiClient)null)
	{
		_treeCacheRow = ArmSoft.SME.Core.UI.Settings.CoreCacheService.Tree.GetItem(treeId);
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = _treeCacheRow.Caption
		};
		int num = 2;
		List<Column> list = new List<Column>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<Column> span = CollectionsMarshal.AsSpan(list);
		span[0] = new Column
		{
			Name = "Code",
			Caption = ((!string.IsNullOrWhiteSpace(_treeCacheRow.CodeCaption)) ? _treeCacheRow.CodeCaption : Resources.Code),
			Source = "Code"
		};
		span[1] = new Column
		{
			Name = "Name",
			Caption = Resources.Name,
			Source = "Name"
		};
		schema.Columns = list;
		Schema schema2 = schema;
		if (_treeCacheRow.Multilevel)
		{
			schema2.Columns.Add(new Column
			{
				Name = "Parent",
				Caption = Resources.Parent,
				Source = "Parent"
			});
		}
		if (_treeCacheRow.IsClosedAvailable)
		{
			schema2.Columns.Add(new Column
			{
				Name = "IsClosed",
				Caption = Resources.Closed,
				Source = "IsClosed",
				Type = SupportedType.Boolean
			});
		}
		if (!string.IsNullOrWhiteSpace(_treeCacheRow.SpecCaption))
		{
			schema2.Columns.Add(new Column
			{
				Name = "Spec",
				Caption = ((!string.IsNullOrWhiteSpace(_treeCacheRow.SpecCaption)) ? _treeCacheRow.SpecCaption : Resources.SpecField),
				Source = "Spec"
			});
		}
		return schema2;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		TreeNodesCache treeNodesCache = (TreeNodesCache)_treeCacheRow.TreeNodeCache;
		if (settings != null && settings.ReloadData)
		{
			treeNodesCache.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		Func<TreeNodesCacheRow, bool> predicate = null;
		if (!string.IsNullOrWhiteSpace(param.ParentCode) || param.ShowOnlyLeafNodes || !param.ShowClosed)
		{
			TreeNodesCacheRow parentCacheRow = treeNodesCache.GetItem(param.ParentCode);
			if (!string.IsNullOrWhiteSpace(param.ParentCode) && parentCacheRow == null)
			{
				return dataProviderResult;
			}
			predicate = (TreeNodesCacheRow p) => (string.IsNullOrWhiteSpace(param.ParentCode) || (p.Path.StartsWith(parentCacheRow?.Path.Trim()) && !(p.Code.Trim() == param.ParentCode.Trim()))) && (!param.ShowOnlyLeafNodes || p.IsLeaf) && (param.ShowClosed || !p.IsClosed);
		}
		foreach (TreeNodesCacheRow item in treeNodesCache.GetList(predicate))
		{
			dataProviderResult.Rows.Add(new DataRow(item.Code, item.Name, item.Parent, item.Spec, item.IsLeaf, item.IsClosed));
		}
		dataProviderResult.Rows = (from p in dataProviderResult.Rows
			orderby p.Parent, p.Code
			select p).ToList();
		return dataProviderResult;
	}
}
