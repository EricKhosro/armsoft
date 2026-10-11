using System;
using System.Collections.Generic;
using System.Windows.Threading;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.DataViews;

public class MyTreeView : TreeListView, IPanelView
{
	public int TreeLevel
	{
		get
		{
			int maxLevel = 1;
			foreach (TreeListNode node in Nodes)
			{
				if (node.Level > maxLevel)
				{
					maxLevel = Convert.ToInt32(node.Level);
				}
				foreach (TreeListNode node2 in node.Nodes)
				{
					CompareLevel(node2, ref maxLevel);
				}
			}
			return maxLevel;
		}
	}

	int IPanelView.GroupingLevel => TreeLevel;

	public bool ShowFilterRow
	{
		get
		{
			return ShowAutoFilterRow;
		}
		set
		{
			ShowAutoFilterRow = value;
			if (!value)
			{
				return;
			}
			GridColumn col = null;
			if (VisibleColumns.Count > 1)
			{
				col = (GridColumn)VisibleColumns[1];
			}
			for (int i = 0; i <= VisibleColumns.Count - 1; i++)
			{
				if (VisibleColumns[i].SortIndex == 0)
				{
					col = (GridColumn)VisibleColumns[i];
					break;
				}
			}
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
			{
				FocusedRowHandle = -2147483645;
				((GridControl)(object)Parent).CurrentColumn = col;
				ShowEditor();
			}), Array.Empty<object>());
		}
	}

	public static void CompareLevel(TreeListNode node, ref int maxLevel)
	{
		if (node.Level > maxLevel)
		{
			maxLevel = node.Level;
		}
		foreach (TreeListNode node2 in node.Nodes)
		{
			CompareLevel(node2, ref maxLevel);
		}
	}

	public void CollapseGroups()
	{
		CollapseAllNodes();
	}

	public void ExpandGroups()
	{
		ExpandAllNodes();
	}

	public void ExpandGroupLevel(int level)
	{
		CollapseAllNodes();
		level--;
		foreach (TreeListNode node in Nodes)
		{
			if (node.Level <= level)
			{
				node.IsExpanded = true;
			}
			if (node.Level > level)
			{
				continue;
			}
			foreach (TreeListNode node2 in node.Nodes)
			{
				InnerExpandNodes(node2, level);
			}
		}
	}

	private static void InnerExpandNodes(TreeListNode node, int level)
	{
		if (level == 0)
		{
			return;
		}
		if (node.Level <= level)
		{
			node.IsExpanded = true;
		}
		if (node.Level > level)
		{
			return;
		}
		foreach (TreeListNode node2 in node.Nodes)
		{
			InnerExpandNodes(node2, level);
		}
	}

	public IEnumerable<ColumnBase> GetVisibleColumns()
	{
		return VisibleColumns;
	}

	public void FitColumn(GridColumn col)
	{
		BestFitColumn(col);
	}

	void IPanelView.BestFitColumn(GridColumn col)
	{
		FitColumn(col);
	}
}
