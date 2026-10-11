using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI.Constants;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Layout.Core;
using DevExpress.Xpf.NavBar;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI.Navigation;

public class NavigatorPanel : LayoutPanel
{
	private readonly NavBarControl _navBarControl;

	private readonly NavigationPaneView _navPanelView;

	private readonly ExplorerBarView _explorerBarView;

	private readonly ButtonEdit _searchBox;

	private bool _isExpandedAll;

	private const string _stateH = "H";

	private const string _stateC = "C";

	private const string _stateA = "A";

	private const string _stateE = "E";

	public static NavigatorPanel NavPanel { get; set; }

	public static bool FirstCall { get; set; } = true;

	public NavBarControl NavBarControl => _navBarControl;

	public static void Restore(bool considerPanelState)
	{
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey, writable: false);
		if (registryKey == null)
		{
			NavPanel = new NavigatorPanel();
			NavPanel.RestoreSettings(registryKey);
		}
		else if (!considerPanelState || Convert.ToString(registryKey.GetValue("NavPanelState", "0")) != "C")
		{
			NavPanel = new NavigatorPanel();
			NavPanel.RestoreSettings(registryKey);
		}
	}

	public static void Close()
	{
		if (NavPanel != null)
		{
			Settings.HostingEnvironment.LayoutManager.DockController.Close(NavPanel);
			NavPanel = null;
		}
	}

	public static void SaveSettings()
	{
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey);
		if (NavPanel != null)
		{
			NavPanel.SaveSettings(registryKey);
		}
		else
		{
			registryKey.SetValue("NavPanelState", "C");
		}
	}

	private NavigatorPanel()
	{
		ItemWidth = new GridLength(220.0);
		Caption = ArmSoft.SME.Common.Resources.Explorer;
		KeyDown += new SafeEventHandler<KeyEventArgs>(NavigatorPanel_KeyDown).Handler;
		Grid grid = new Grid
		{
			RowDefinitions = 
			{
				new System.Windows.Controls.RowDefinition
				{
					Height = GridLength.Auto
				},
				new System.Windows.Controls.RowDefinition
				{
					Height = new GridLength(1.0, GridUnitType.Star)
				}
			}
		};
		_searchBox = new ButtonEdit
		{
			Margin = new Thickness(5.0),
			NullText = ArmSoft.SME.Common.Resources.Find,
			EditMode = EditMode.InplaceActive,
			EditValuePostDelay = 500,
			EditValuePostMode = PostMode.Delayed,
			AllowDefaultButton = false
		};
		_searchBox.Buttons.Add(new ButtonInfo
		{
			GlyphKind = GlyphKind.Search,
			IsLeft = true,
			IsEnabled = false
		});
		ButtonInfo buttonInfo = new ButtonInfo
		{
			GlyphKind = GlyphKind.Cancel,
			IsLeft = false,
			IsEnabled = false
		};
		buttonInfo.Click += (object s, RoutedEventArgs e) =>
		{
			_searchBox.Text = "";
		};
		_searchBox.Buttons.Add(buttonInfo);
		_searchBox.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(SearchBox_EditValueChanged).Handler;
		_searchBox.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(SearchBox_KeyDown).Handler;
		Grid.SetRow(_searchBox, 0);
		grid.Children.Add(_searchBox);
		_navBarControl = new NavBarControl
		{
			Margin = new Thickness(0.0),
			UseLayoutRounding = true
		};
		_navPanelView = new NavigationPaneView
		{
			MaxVisibleGroupCount = 4,
			IsExpandButtonVisible = false,
			OverflowGroupTemplate = (DataTemplate)Settings.GenericResources["OverFlowGroupHeaderButtonTemplate"]
		};
		_navPanelView.Click += new SafeEventHandler<RoutedEventArgs>(NavPanelView_Click).Handler;
		_explorerBarView = new ExplorerBarView
		{
			Orientation = Orientation.Vertical
		};
		_navPanelView.NavPaneExpandedChanging += new SafeEventHandler<NavPaneExpandedChangingEventArgs>(PreventCollapsing).Handler;
		_navBarControl.View = _navPanelView;
		_navPanelView.ActiveGroupChanged += new SafeEventHandler<NavBarActiveGroupChangedEventArgs>(NavPanelView_ActiveGroupChanged).Handler;
		Grid.SetRow(_navBarControl, 1);
		grid.Children.Add(_navBarControl);
		Content = grid;
		AllowFloat = false;
		AllowMove = false;
	}

	private void NavPanelView_Click(object sender, RoutedEventArgs e)
	{
		if (e.OriginalSource is NavBarGroupHeader)
		{
			if (_isExpandedAll)
			{
				((TreeViewControl)NavPanel.NavBarControl.ActiveGroup.Content).CollapseAllNodes();
				_isExpandedAll = false;
			}
			else
			{
				((TreeViewControl)NavPanel.NavBarControl.ActiveGroup.Content).ExpandAllNodes();
				_isExpandedAll = true;
			}
		}
		else
		{
			_isExpandedAll = false;
		}
	}

	private void SearchBox_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.Key != 26)
		{
			return;
		}
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
		{
			TreeViewControl treeViewControl = (TreeViewControl)_navBarControl.ActiveGroup.Content;
			if (treeViewControl.VisibleNodeCount > 0)
			{
				treeViewControl.CurrentItem = treeViewControl.VisibleItems[0];
				treeViewControl.Focus();
			}
		}), (DispatcherPriority)5, Array.Empty<object>());
	}

	private void NavPanelView_ActiveGroupChanged(object sender, NavBarActiveGroupChangedEventArgs e)
	{
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
		{
			if (e?.Group?.Content is TreeViewControl treeViewControl)
			{
				treeViewControl.Focus();
			}
		}), (DispatcherPriority)5, Array.Empty<object>());
	}

	private void NavigatorPanel_KeyDown(object sender, KeyEventArgs e)
	{
		if (HotKeys.WithModifier.Control_F.Matches(this, e))
		{
			_searchBox.Focus();
		}
	}

	private void SearchBox_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (string.IsNullOrWhiteSpace(_searchBox.Text))
		{
			_searchBox.RightButtons.First().IsEnabled = false;
			VisibleGroups();
			_navBarControl.View = _navPanelView;
			return;
		}
		_searchBox.RightButtons.First().IsEnabled = true;
		if (!(_navBarControl.View is ExplorerBarView))
		{
			_navBarControl.View = _explorerBarView;
		}
		ApplyGlobalSearch(_searchBox.Text);
	}

	private void ApplyGlobalSearch(string search)
	{
		foreach (NavBarGroup group in _navBarControl.Groups)
		{
			group.IsVisible = true;
			bool flag = false;
			if (group.Content is TreeViewControl treeViewControl)
			{
				treeViewControl.SearchString = search;
				treeViewControl.SelectedItem = null;
				flag = HasVisibleNodes(treeViewControl);
			}
			group.IsExpanded = flag;
			if (!flag)
			{
				group.IsVisible = false;
			}
		}
		if (_navBarControl.Groups.Count((NavBarGroup p) => p.ActualIsVisible) == 1)
		{
			_navBarControl.ActiveGroup = _navBarControl.Groups[0];
		}
	}

	private void VisibleGroups()
	{
		foreach (NavBarGroup group in _navBarControl.Groups)
		{
			group.IsVisible = true;
			if (group.Content is TreeViewControl treeViewControl)
			{
				treeViewControl.SearchString = "";
			}
		}
	}

	private static bool HasVisibleNodes(TreeViewControl tree)
	{
		foreach (TreeListNode node in tree.Nodes)
		{
			if (NodeIsVisible(node))
			{
				return true;
			}
		}
		return false;
	}

	private static bool NodeIsVisible(TreeListNode node)
	{
		if (!node.IsFiltered)
		{
			return true;
		}
		foreach (TreeListNode node2 in node.Nodes)
		{
			if (!node2.IsFiltered)
			{
				return true;
			}
			if (node2.HasChildren)
			{
				return NodeIsVisible(node2);
			}
		}
		return false;
	}

	private void SaveSettings(RegistryKey rk)
	{
		if (rk == null)
		{
			return;
		}
		if (ItemWidth.IsAbsolute)
		{
			if (IsAutoHidden)
			{
				rk.SetValue("NavPanelWidth", _navPanelView.ActualWidth);
			}
			else
			{
				rk.SetValue("NavPanelWidth", Settings.HostingEnvironment.NavigatorGroup.ActualWidth);
			}
		}
		if (IsAutoHidden)
		{
			rk.SetValue("NavPanelState", "H");
		}
		else if (Parent == null)
		{
			rk.SetValue("NavPanelState", "C");
		}
		else
		{
			rk.SetValue("NavPanelState", "A");
		}
		if (_navBarControl.ActiveGroup != null)
		{
			rk.SetValue("NavPanelCurrent", _navBarControl.ActiveGroup.Name);
		}
		rk.SetValue("VisibleGroupCount", _navPanelView.MaxVisibleGroupCount);
		foreach (NavBarGroup group in _navBarControl.Groups)
		{
			rk.SetValue(group.Name, group.ActualIsVisible ? "1" : "0");
		}
	}

	private void RestoreSettings(RegistryKey rk)
	{
		string text = "";
		NavBarGroup navBarGroup = null;
		double result;
		string text2;
		int maxVisibleGroupCount;
		if (rk == null)
		{
			result = 0.0;
			text2 = "A";
			maxVisibleGroupCount = 4;
			foreach (NavBarGroup group in _navBarControl.Groups)
			{
				group.IsVisible = true;
			}
		}
		else
		{
			if (!double.TryParse((string)rk.GetValue("NavPanelWidth", "0"), out result))
			{
				result = 0.0;
			}
			maxVisibleGroupCount = (int)rk.GetValue("VisibleGroupCount", 4);
			text2 = Convert.ToString(rk.GetValue("NavPanelState", "0"));
			text = (string)rk.GetValue("NavPanelCurrent", "");
			foreach (NavBarGroup group2 in _navBarControl.Groups)
			{
				string text3 = Convert.ToString(rk.GetValue(group2.Name, "1"));
				group2.IsVisible = text3 == "1";
			}
		}
		_navBarControl.BorderBrush = Brushes.Transparent;
		_navBarControl.Margin = new Thickness(5.0, 0.0, 0.0, 0.0);
		if (result == 0.0)
		{
			result = 300.0;
		}
		ItemWidth = new GridLength(result);
		Settings.HostingEnvironment.NavigationBarsController.Fill(_navBarControl, Settings.HostingEnvironment.UserAccessManager);
		_navPanelView.MaxVisibleGroupCount = maxVisibleGroupCount;
		foreach (NavBarGroup group3 in _navBarControl.Groups)
		{
			if (group3.Name == text)
			{
				navBarGroup = group3;
				break;
			}
		}
		if (navBarGroup == null)
		{
			if (_navBarControl.Groups.Count <= 0)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.DontExistUserViewAccess);
			}
			navBarGroup = _navBarControl.Groups[0];
		}
		Visibility = Visibility.Visible;
		if (Parent == null && (text2 != "C" || !FirstCall))
		{
			if (Settings.HostingEnvironment.RecentsPanel != null && Settings.HostingEnvironment.RecentsPanel.Parent != null)
			{
				Settings.HostingEnvironment.LayoutManager.DockController.Dock(NavPanel, Settings.HostingEnvironment.RecentsPanel, DockType.Fill);
			}
			else if (text2 == "H")
			{
				Settings.HostingEnvironment.LayoutManager.DockController.Dock(this, Settings.HostingEnvironment.AutoHideLeft, DockType.Left);
			}
			else
			{
				Settings.HostingEnvironment.NavigatorGroup.Items.Insert(0, this);
				Settings.HostingEnvironment.NavigatorGroup.ItemWidth = ItemWidth;
			}
		}
		FirstCall = false;
		Settings.HostingEnvironment.LayoutManager.Activate(this);
		_navBarControl.ActiveGroup = navBarGroup;
		navBarGroup.Focus();
	}

	private void PreventCollapsing(object sender, NavPaneExpandedChangingEventArgs e)
	{
		if (!e.IsExpanded)
		{
			e.Cancel = true;
		}
	}

	public static string CollectNodesState()
	{
		if (NavPanel == null)
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		foreach (NavBarGroup group in NavPanel.NavBarControl.Groups)
		{
			foreach (AsNavigationTreeNode item in ((TreeViewControl)group.Content).ItemsSource as List<AsNavigationTreeNode>)
			{
				if (item.ChildrenList.Count > 0)
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						stringBuilder.Append(',');
					}
					if (item.IsExpanded)
					{
						stringBuilder.Append("E"[0]);
					}
					else
					{
						stringBuilder.Append("C"[0]);
					}
					AsNavigationTreeNode asNavigationTreeNode = item;
					stringBuilder.Append(asNavigationTreeNode.Command);
					CollectChildsNodesState(item, stringBuilder);
				}
			}
		}
		return stringBuilder.ToString();
	}

	private static void CollectChildsNodesState(AsNavigationTreeNode root, StringBuilder sb)
	{
		foreach (AsNavigationTreeNode children in root.ChildrenList)
		{
			if (children.ChildrenList.Count > 0)
			{
				sb.Append(',');
				if (children.IsExpanded)
				{
					sb.Append("E"[0]);
				}
				else
				{
					sb.Append("C"[0]);
				}
				AsNavigationTreeNode asNavigationTreeNode = children;
				sb.Append(asNavigationTreeNode.Command);
				CollectChildsNodesState(children, sb);
			}
		}
	}

	public static void LoadNavigatorNodes()
	{
		string text = Settings.ParametersService.NavigatorState();
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		string[] array = text.Split(",");
		Dictionary<short, bool> dictionary = new Dictionary<short, bool>();
		for (int i = 0; i <= array.Length - 1; i++)
		{
			if (!string.IsNullOrWhiteSpace(array[i].Mid(2)))
			{
				short key = Convert.ToInt16(array[i].Mid(2));
				bool value = array[i].Mid(1, 1) == "E";
				dictionary.TryAdd(key, value);
			}
		}
		foreach (NavBarGroup group in NavPanel.NavBarControl.Groups)
		{
			TreeViewControl treeViewControl = (TreeViewControl)group.Content;
			treeViewControl.FontFamily = Settings.FontFamily;
			treeViewControl.FontSize = Settings.GetFontSize();
			for (int j = 0; j < treeViewControl.VisibleNodeCount; j++)
			{
				int rowHandleByVisibleIndex = treeViewControl.GetRowHandleByVisibleIndex(j);
				AsNavigationTreeNode asNavigationTreeNode = (AsNavigationTreeNode)treeViewControl.GetRow(rowHandleByVisibleIndex);
				TreeListNode nodeByVisibleIndex = treeViewControl.GetNodeByVisibleIndex(j);
				if (dictionary.TryGetValue(asNavigationTreeNode.Command, out var value2))
				{
					asNavigationTreeNode.IsExpanded = value2;
					nodeByVisibleIndex.IsExpanded = value2;
				}
			}
		}
	}
}
