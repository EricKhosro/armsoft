using System.Collections.Generic;
using System.ComponentModel;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.SmartEnums;

namespace ArmSoft.SME.Core.UI.Navigation;

public class AsNavigationTreeNode : INotifyPropertyChanged
{
	private bool _hasNotification;

	private readonly List<AsNavigationTreeNode> _children;

	public short Command { get; set; }

	public short Subsystem { get; set; }

	public string Header { get; set; }

	public string Image { get; set; }

	public bool IsExpanded { get; set; }

	public bool IsViewChangeable { get; set; }

	public bool IsOperChangeable { get; set; }

	public string FullHeader { get; set; }

	public AWPCommandType CommandType { get; set; }

	public string CommandTag { get; set; }

	public bool IsInFirstColumnInHomePage { get; set; }

	public bool IsInSecondColumnInHomePage { get; set; }

	public FeatureStatusTypeSE TreeNodeType { get; set; }

	public bool HasNotification
	{
		get
		{
			return _hasNotification;
		}
		set
		{
			if (_hasNotification != value)
			{
				_hasNotification = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasNotification"));
			}
		}
	}

	public List<AsNavigationTreeNode> ChildrenList => _children;

	public event PropertyChangedEventHandler PropertyChanged;

	public AsNavigationTreeNode(short command, string header, string image = "", bool isViewChangeable = true, bool isOperChangeable = true, bool isExpanded = true, string fullHeader = "", AWPCommandType commandType = AWPCommandType.SystemCall, string commandTag = "", bool isInFirstColumnInHomePage = false, FeatureStatusTypeSE treeNodeType = null, bool isInSecondColumnInHomePage = false)
	{
		_children = new List<AsNavigationTreeNode>();
		Command = command;
		Header = header;
		Image = image;
		IsExpanded = isExpanded;
		IsViewChangeable = isViewChangeable;
		IsOperChangeable = isOperChangeable;
		if (string.IsNullOrWhiteSpace(fullHeader))
		{
			FullHeader = Header;
		}
		else
		{
			FullHeader = fullHeader;
		}
		CommandType = commandType;
		CommandTag = commandTag;
		IsInFirstColumnInHomePage = isInFirstColumnInHomePage;
		IsInSecondColumnInHomePage = isInSecondColumnInHomePage;
		TreeNodeType = treeNodeType ?? FeatureStatusTypeSE.Regular;
	}

	public AsNavigationTreeNode AddChild(AsNavigationTreeNode node)
	{
		_children.Add(node);
		return node;
	}

	public AsNavigationTreeNode AddChild(AsNavigationTreeNode node, short subSystemId, bool checkAccess = false, bool addIfHasChild = false)
	{
		bool flag = true;
		if (checkAccess)
		{
			flag = Settings.UserAccessService.GetNavigatorNodePermission(subSystemId, node.Command).AllowView;
		}
		if ((!addIfHasChild || node._children.Count != 0) & flag)
		{
			_children.Add(node);
			return node;
		}
		return null;
	}

	public void RemoveWithoutChildNode(AsNavigationTreeNode node)
	{
		if (node._children.Count == 0)
		{
			_children.Remove(node);
		}
	}
}
