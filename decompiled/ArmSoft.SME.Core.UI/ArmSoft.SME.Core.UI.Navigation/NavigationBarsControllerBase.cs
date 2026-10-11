using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DataViews.TreeNodes;
using ArmSoft.SME.Core.UI.Exchange;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Core.UI.OtherWindows;
using ArmSoft.SME.Core.UI.StandartWindow;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.NavBar;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI.Navigation;

public abstract class NavigationBarsControllerBase
{
	public class SolutionNodeImageSelector : TreeListNodeImageSelector
	{
		private static readonly Dictionary<string, ImageSource> _imageCache;

		static SolutionNodeImageSelector()
		{
			_imageCache = new Dictionary<string, ImageSource>();
		}

		public override ImageSource Select(TreeListRowData rowData)
		{
			if (rowData.Row is AsNavigationTreeNode asNavigationTreeNode)
			{
				return GetImageByTypeNode(asNavigationTreeNode.Image);
			}
			return null;
		}

		public static ImageSource GetImageByTypeNode(string typeNode)
		{
			if (_imageCache.TryGetValue(typeNode, out var value))
			{
				return value;
			}
			ImageSource imagePathByTypeNode = GetImagePathByTypeNode(typeNode);
			_imageCache.Add(typeNode, imagePathByTypeNode);
			return imagePathByTypeNode;
		}

		public static ImageSource GetImagePathByTypeNode(string typeNode)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return ImageService.GetImage(typeNode);
		}
	}

	public NavigationBarsControllerBase()
	{
	}

	protected abstract object GetUserAccessDefinition(short accessId);

	public void ExecuteCommand(NavigationCommand nc)
	{
		if (Settings.ParametersService.KeepUICacheUpToDate())
		{
			Settings.CacheFunctionsService.RefreshCaches();
		}
		ExecuteCommand(nc.Command, nc.Subsystem);
	}

	public virtual void ExecuteCommand(short command, short? subsystem = null)
	{
		try
		{
			dynamic userAccessDefinition = GetUserAccessDefinition(command);
			if (command == UserAccessIdSE.RefreshServiceCaches.Value)
			{
				if (MessageBox.Show(Resources.RefreshCacheInService, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
				{
					Mouse.OverrideCursor = Cursors.Wait;
					Settings.CacheFunctionsService.ReloadServiceCaches();
				}
			}
			else if (command == UserAccessIdSE.ImportData.Value)
			{
				new ImportWindow().Show();
			}
			else if (command == UserAccessIdSE.ExportData.Value)
			{
				new ExportWindow().ShowDialog();
			}
			else if (command == UserAccessIdSE.ImportExtensions.Value)
			{
				OpenFileDialog openFileDialog = new OpenFileDialog
				{
					DefaultExt = ".xml",
					Filter = "XML documents (.xml)|*.xml"
				};
				if (openFileDialog.ShowDialog().Value)
				{
					ArmSoft.SME.Core.UI.MetaData.Environment environment = new ArmSoft.SME.Core.UI.MetaData.Environment();
					if (ExtensionsProcessor.Load(openFileDialog.FileName, environment).Store())
					{
						MessageBox.Show(Resources.ExtensionsSuccessfullyImported);
					}
					else
					{
						MessageBox.Show(string.Format(Resources.ImportCompletedWithErrors, 0, 0));
					}
				}
			}
			else if (command == UserAccessIdSE.Databases.Value)
			{
				if (!Settings.LoginService.IsAdmin)
				{
					throw new ASException(string.Format(Resources.DontHavePermissionToExecuteData, Resources.Databases), 20008);
				}
				new Databases().Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
			}
			else if (command == UserAccessIdSE.DebugChangeDBVersion.Value)
			{
				new ChangeVersionsWindow();
			}
			else if (command == UserAccessIdSE.DebugExpPrintSubstitutions.Value)
			{
				new ExportTemplatesSubstitutionsWindow();
			}
			else
			{
				if (!((userAccessDefinition != null && userAccessDefinition.RefObjectType != null && userAccessDefinition.RefObjectId > 0) ? true : false))
				{
					return;
				}
				if (userAccessDefinition.RefObjectType == NavigationCommandRefObjectType.Document)
				{
					dynamic val = Settings.DocumentService.CreateWindow(userAccessDefinition.RefObjectId);
					val.Show();
				}
				else if (userAccessDefinition.RefObjectType == NavigationCommandRefObjectType.DataProvider)
				{
					dynamic val2 = Settings.DataViewService.CreateDataView(userAccessDefinition.RefObjectId);
					if (subsystem.HasValue && subsystem != 100)
					{
						val2.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
					}
					val2.Show(openAsChild: false);
				}
				else if (userAccessDefinition.RefObjectType == NavigationCommandRefObjectType.TreeNodesDataProvider)
				{
					TreeNodes treeNodes = new TreeNodes(((string)SmartEnumHelper.GetName(typeof(DataProviderTypeSE), userAccessDefinition.RefObjectId)).Substring("Tree_".Length));
					if (subsystem.HasValue && subsystem != 100)
					{
						treeNodes.AllowOperation = Settings.UserAccessService.GetNavigatorNodePermission(subsystem.Value, command).AllowOperation;
					}
					treeNodes.Show(useParamValues: false, openAsChild: false);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			Mouse.OverrideCursor = null;
		}
	}

	public virtual void Fill(NavBarControl navBar, UserAccessManagerBase userAccessManager)
	{
		throw new NotImplementedException();
	}

	public virtual bool CheckUserAccess(short command)
	{
		throw new NotImplementedException();
	}

	public static AsNavigationTreeNode GetTreeNodeByCommand(short cmd, UserAccessManagerBase userAccessManager, ref short subSystem)
	{
		AsNavigationTreeNode asNavigationTreeNode = null;
		foreach (KeyValuePair<short, AsNavigationTreeNode> item in userAccessManager.AccessTree)
		{
			asNavigationTreeNode = GetTreeNodeByCommand(cmd, item.Value.ChildrenList);
			if (asNavigationTreeNode != null)
			{
				subSystem = item.Key;
				return asNavigationTreeNode;
			}
		}
		return asNavigationTreeNode;
	}

	private static AsNavigationTreeNode GetTreeNodeByCommand(short cmd, List<AsNavigationTreeNode> list)
	{
		foreach (AsNavigationTreeNode item in list)
		{
			if (item.Command == cmd)
			{
				return item;
			}
			if (item.ChildrenList.Count != 0)
			{
				AsNavigationTreeNode treeNodeByCommand = GetTreeNodeByCommand(cmd, item.ChildrenList);
				if (treeNodeByCommand != null)
				{
					return treeNodeByCommand;
				}
			}
		}
		return null;
	}

	protected static void AddSubTree(TreeViewControl treeView, List<AsNavigationTreeNode> list, short subSystem)
	{
		foreach (AsNavigationTreeNode item in list)
		{
			SetSubsystemRecursive(item, subSystem);
		}
		treeView.ItemsSource = list;
	}

	private static void SetSubsystemRecursive(AsNavigationTreeNode node, short subSystem)
	{
		node.Subsystem = subSystem;
		foreach (AsNavigationTreeNode children in node.ChildrenList)
		{
			SetSubsystemRecursive(children, subSystem);
		}
	}

	protected TreeViewControl AddGroup(NavBarControl navBar, ref NavBarGroup group, string name, string header, string imageUri)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		group.Name = ((name == ((short)5000).ToString()) ? "UserAPW" : name);
		group.Header = header;
		ImageSource imageSource = null;
		if (!string.IsNullOrWhiteSpace(imageUri))
		{
			imageSource = ImageService.GetImage(imageUri);
		}
		group.Style = (Style)Settings.GenericResources["NavBarGroupStyle"];
		group.ImageSource = imageSource;
		group.DisplaySource = DisplaySource.Content;
		TreeViewControl treeViewControl = new TreeViewControl
		{
			Background = Settings.GetBackgroundBrush(),
			UseLayoutRounding = true,
			DataAreaBackground = Settings.GetBackgroundBrush(),
			FontFamily = Settings.FontFamily,
			FontSize = Settings.GetFontSize(),
			TreeViewFieldName = "Header",
			ChildNodesPath = "ChildrenList",
			ImageFieldName = "Image",
			ShowNodeImages = true,
			NodeImageSelector = new SolutionNodeImageSelector(),
			Margin = new Thickness(0.0, 0.0, 5.0, 0.0),
			BorderBrush = Brushes.Transparent,
			BorderThickness = new Thickness(0.0),
			VerticalAlignment = VerticalAlignment.Stretch,
			ExpandStateFieldName = "IsExpanded",
			FilteringMode = TreeListFilteringMode.Nodes,
			ExpandNodesOnFiltering = true,
			SearchPanelFindFilter = FilterCondition.Contains,
			NodeIndent = 18.0,
			TreeDerivationMode = TreeDerivationMode.ChildNodesSelector,
			MaxHeight = SystemParameters.PrimaryScreenHeight,
			AllowInitiallyFocusedRow = true,
			NodeContentDisplayTemplate = (DataTemplate)Settings.GenericResources["NavigationTreeNodeTemplate"]
		};
		ScrollBarExtensions.SetScrollBarMode((DependencyObject)(object)treeViewControl, ScrollBarMode.TouchOverlap);
		treeViewControl.NodeDoubleClick += new SafeEventHandler<NodeDoubleClickEventArgs>(TreeView_NodeDoubleClick).Handler;
		treeViewControl.KeyDown += new SafeEventHandler<KeyEventArgs>(KeyDown).Handler;
		treeViewControl.ContextMenuOpening += new SafeEventHandler<ContextMenuEventArgs>(TreeView_ContextMenuOpening).Handler;
		treeViewControl.StartRecordDrag += new SafeEventHandler<StartRecordDragEventArgs>(TreeView_StartRecordDrag).Handler;
		treeViewControl.CompleteRecordDragDrop += new SafeEventHandler<CompleteRecordDragDropEventArgs>(TreeView_CompleteRecordDragDrop).Handler;
		treeViewControl.AllowDragDrop = true;
		treeViewControl.ShowDragDropHint = false;
		group.Content = treeViewControl;
		group.GroupScrollMode = DevExpress.Xpf.NavBar.ScrollMode.None;
		navBar.Groups.Add(group);
		return treeViewControl;
	}

	private void TreeView_StartRecordDrag(object sender, StartRecordDragEventArgs e)
	{
		TreeViewControl treeViewControl = sender as TreeViewControl;
		TreeListNode treeListNode = treeViewControl?.CurrentNode;
		if (treeListNode != null)
		{
			DataObject data = new DataObject("MyTreeViewItem", treeListNode);
			DragDrop.DoDragDrop((DependencyObject)(object)treeViewControl, data, DragDropEffects.Copy);
		}
		e.AllowedEffects = DragDropEffects.None;
		e.AllowDrag = false;
	}

	private void TreeView_CompleteRecordDragDrop(object sender, CompleteRecordDragDropEventArgs e)
	{
		e.Effects = DragDropEffects.None;
	}

	private void TreeView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
	{
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		TreeViewControl treeViewControl = (TreeViewControl)sender;
		TreeViewControlView treeViewControlView = (TreeViewControlView)e.Source;
		FrameworkElement rowElementByRowHandle = treeViewControlView.GetRowElementByRowHandle(treeViewControlView.FocusedRowHandle);
		AsNavigationTreeNode asNavigationTreeNode = treeViewControl.CurrentItem as AsNavigationTreeNode;
		TreeListNode currentNode = treeViewControl.CurrentNode;
		PopupMenu popupMenu = new PopupMenu();
		if (currentNode.HasChildren || asNavigationTreeNode.Command == 0)
		{
			e.Handled = true;
			return;
		}
		treeViewControl.SelectItem(currentNode);
		BarButtonItem barButtonItem = new BarButtonItem
		{
			Content = asNavigationTreeNode.Header,
			Tag = new NavigationCommand
			{
				Command = asNavigationTreeNode.Command,
				Subsystem = asNavigationTreeNode.Subsystem,
				CommandType = asNavigationTreeNode.CommandType,
				CommandTag = asNavigationTreeNode.CommandTag
			},
			Name = Settings.HostingEnvironment.UserAccessManager.GetCommandName(asNavigationTreeNode.Command),
			Glyph = ImageService.GetImage(asNavigationTreeNode.Image)
		};
		if (MainFunctionsHelper.FindBarItemByName(Settings.HostingEnvironment.MainRibbonPanel.BarManager, barButtonItem.Name) == null)
		{
			Settings.HostingEnvironment.MainRibbonPanel.BarManager.Items.Add(barButtonItem);
		}
		barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButton_Click).Handler;
		popupMenu.ItemLinks.Add(barButtonItem.CreateLink());
		popupMenu.ItemLinks[0].IsVisible = false;
		popupMenu.Opening += new SafeEventHandler<CancelEventArgs>(AdjustMenu).Handler;
		BarManager.SetDXContextMenu(rowElementByRowHandle, popupMenu);
		BarPopupBase.ShowElementContextMenu(asNavigationTreeNode);
		popupMenu.ShowPopup(rowElementByRowHandle);
	}

	private void BarButton_Click(object sender, ItemClickEventArgs e)
	{
		NavigationCommand navigationCommand = (NavigationCommand)((BarButtonItem)sender).Tag;
		if (navigationCommand.CommandType == AWPCommandType.SystemCall)
		{
			Settings.HostingEnvironment.NavigationBarsController.ExecuteCommand(navigationCommand);
		}
	}

	private void AdjustMenu(object sender, CancelEventArgs e)
	{
		PopupMenu popupMenu = (PopupMenu)sender;
		BarButtonItem barButtonItem = (BarButtonItem)popupMenu.ItemLinks[0].Item;
		if (popupMenu.ItemLinks.Count > 1)
		{
			popupMenu.ItemLinks.RemoveAt(1);
		}
		BarButtonItemLink barButtonItemLink = MainFunctionsHelper.FindBarItemByNameInFavorites(barButtonItem.Name);
		if (barButtonItemLink == null)
		{
			if (((NavigationCommand)barButtonItem.Tag).CommandType == AWPCommandType.SystemCall)
			{
				Settings.HostingEnvironment.MainRibbonPanel.AddToFavoritesBarButtonItem.Tag = barButtonItem.Tag;
				popupMenu.ItemLinks.Add(Settings.HostingEnvironment.MainRibbonPanel.AddToFavoritesBarButtonItem.CreateLink());
			}
		}
		else
		{
			Settings.HostingEnvironment.MainRibbonPanel.RemoveFromFavoritesBarButtonItem.Tag = barButtonItemLink;
			popupMenu.ItemLinks.Add(Settings.HostingEnvironment.MainRibbonPanel.RemoveFromFavoritesBarButtonItem.CreateLink());
		}
	}

	private void TreeView_NodeDoubleClick(object sender, NodeDoubleClickEventArgs e)
	{
		e.Handled = true;
		if (!(e.HitInfo is TreeListViewHitInfo { HitTest: TreeListViewHitTest.ExpandButton }) && e.HitInfo.InRow)
		{
			TreeViewControl treeViewControl = (TreeViewControl)sender;
			AsNavigationTreeNode asNavigationTreeNode = (AsNavigationTreeNode)treeViewControl.GetRow(e.HitInfo.RowHandle);
			if (asNavigationTreeNode.ChildrenList.Count > 0)
			{
				TreeListNode nodeByRowHandle = treeViewControl.GetNodeByRowHandle(e.HitInfo.RowHandle);
				nodeByRowHandle.IsExpanded = !nodeByRowHandle.IsExpanded;
			}
			else
			{
				ExecuteCommandInner(asNavigationTreeNode);
			}
		}
	}

	private void KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Invalid comparison between Unknown and I4
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Invalid comparison between Unknown and I4
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Invalid comparison between Unknown and I4
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Invalid comparison between Unknown and I4
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected I4, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Invalid comparison between Unknown and I4
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected I4, but got Unknown
		if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 0)
		{
			TreeViewControl treeViewControl = (TreeViewControl)sender;
			AsNavigationTreeNode asNavigationTreeNode = (AsNavigationTreeNode)treeViewControl.SelectedItem;
			if (asNavigationTreeNode.ChildrenList.Count > 0)
			{
				TreeListNode nodeByContent = treeViewControl.GetNodeByContent(asNavigationTreeNode);
				nodeByContent.IsExpanded = !nodeByContent.IsExpanded;
			}
			else
			{
				ExecuteCommandInner(asNavigationTreeNode);
			}
			e.Handled = true;
		}
		else if ((((int)e.Key >= 75 && (int)e.Key <= 83) || ((int)e.Key >= 35 && (int)e.Key <= 43)) && (int)Keyboard.Modifiers == 2)
		{
			FocusNavBarGroup(navBarGroupRowNumber: ((int)e.Key < 75 || (int)e.Key > 83) ? (e.Key - 35) : (e.Key - 75), navBar: NavigatorPanel.NavPanel.NavBarControl);
			e.Handled = true;
		}
		else
		{
			e.Handled = false;
		}
	}

	private void ExecuteCommandInner(AsNavigationTreeNode nbc)
	{
		NavigationCommand navigationCommand = new NavigationCommand
		{
			Command = nbc.Command,
			Subsystem = nbc.Subsystem,
			CommandType = nbc.CommandType,
			CommandTag = nbc.CommandTag
		};
		switch (navigationCommand.CommandType)
		{
		case AWPCommandType.SystemCall:
			ExecuteCommand(navigationCommand);
			break;
		case AWPCommandType.UserReport:
		case AWPCommandType.Tree:
			break;
		}
	}

	private static void FocusNavBarGroup(NavBarControl navBar, int navBarGroupRowNumber)
	{
		NavBarGroupCollection groups = navBar.Groups;
		if (groups.Count > navBarGroupRowNumber)
		{
			NavBarGroup activeGroup = groups[navBarGroupRowNumber];
			navBar.ActiveGroup = activeGroup;
			navBar.ActiveGroup.Focus();
			FocusActiveBarFirstRow(navBar);
		}
	}

	private void KeyDownShort(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected I4, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected I4, but got Unknown
		if ((((int)e.Key >= 75 && (int)e.Key <= 83) || ((int)e.Key >= 35 && (int)e.Key <= 43)) && (int)Keyboard.Modifiers == 2)
		{
			FocusNavBarGroup(navBarGroupRowNumber: ((int)e.Key < 75 || (int)e.Key > 83) ? (e.Key - 35) : (e.Key - 75), navBar: NavigatorPanel.NavPanel.NavBarControl);
			e.Handled = true;
		}
	}

	private static void FocusActiveBarFirstRow(NavBarControl navBar)
	{
		TreeViewControl treeViewControl = (TreeViewControl)navBar.ActiveGroup.Content;
		AsNavigationTreeNode selectedItem = (AsNavigationTreeNode)treeViewControl.VisibleItems[0];
		treeViewControl.SelectedItem = selectedItem;
		Settings.HostingEnvironment.LayoutManager.Activate(NavigatorPanel.NavPanel);
	}

	public static void SelectGroup(string groupName)
	{
		if (NavigatorPanel.NavPanel == null || NavigatorPanel.NavPanel.NavBarControl == null)
		{
			throw new ASException("Navigator Panel not Activated");
		}
		foreach (NavBarGroup group in NavigatorPanel.NavPanel.NavBarControl.Groups)
		{
			if (group.Header.ToString() == groupName)
			{
				NavigatorPanel.NavPanel.NavBarControl.ActiveGroup = group;
				break;
			}
		}
	}

	public static void UpdateNavigatorPanelNotificationState(short commandId, bool hasNotification)
	{
		if (NavigatorPanel.NavPanel == null)
		{
			return;
		}
		foreach (NavBarGroup group in NavigatorPanel.NavPanel.NavBarControl.Groups)
		{
			if (group.Content is TreeViewControl treeViewControl)
			{
				int num = UpdateTreeViewNotificationState(treeViewControl, treeViewControl.Nodes, commandId, hasNotification);
				if (num != -1)
				{
					treeViewControl.RefreshRow(num);
				}
			}
		}
	}

	private static int UpdateTreeViewNotificationState(TreeViewControl treeView, TreeListNodeCollection treeViewCollection, short commandId, bool hasNotification)
	{
		int result = -1;
		foreach (TreeListNode item in treeViewCollection)
		{
			if (!(item is TreeListNode treeListNode) || treeListNode == null)
			{
				continue;
			}
			if (treeView.GetRow(treeListNode.RowHandle) is AsNavigationTreeNode asNavigationTreeNode && asNavigationTreeNode != null && asNavigationTreeNode.Command == commandId)
			{
				if (asNavigationTreeNode.HasNotification && !hasNotification)
				{
					HistoryController recentsController = Settings.HostingEnvironment.RecentsController;
					List<HistoryItem> list = recentsController.ItemsList.Where((HistoryItem e) => e.IsNewItem).ToList();
					HistoryItem historyItem = list.FirstOrDefault((HistoryItem el) => el.NavigationCommandId == commandId);
					if (historyItem != null)
					{
						historyItem.IsNewItem = false;
						if (list.Count == 1)
						{
							recentsController.NewItemsMode = false;
						}
						if (Settings.HostingEnvironment.RecentsPanel != null)
						{
							Settings.HostingEnvironment.RecentsPanel.RefreshGrid();
						}
					}
				}
				asNavigationTreeNode.HasNotification = hasNotification;
				return treeListNode.RowHandle;
			}
			if (treeListNode.HasChildren)
			{
				int num = UpdateTreeViewNotificationState(treeView, treeListNode.Nodes, commandId, hasNotification);
				if (num != -1)
				{
					result = num;
				}
			}
		}
		return result;
	}
}
