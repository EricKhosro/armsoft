using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Navigation;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.MetroNavigation;

public partial class MetroNavigationPage : UserControl, IComponentConnector
{
	private CommonConst.SubSystemTypes _subSystem;

	public CommonConst.SubSystemTypes SubSystem
	{
		set
		{
			try
			{
				_subSystem = value;
				AsNavigationTreeNode t = ((_subSystem == CommonConst.SubSystemTypes.UserAWP) ? Settings.HostingEnvironment.UserAccessManager.GetSubSysTree(5000) : Settings.HostingEnvironment.UserAccessManager.GetSubSysTree((short)_subSystem));
				FillGroups(t);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
		}
	}

	public MetroNavigationPage()
	{
		InitializeComponent();
		ASFontHelper.SetFontSettings(this);
	}

	private void FillSubGroups(AsNavigationTreeNode t, List<NavigationNode> arr)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		foreach (AsNavigationTreeNode children in t.ChildrenList)
		{
			if (Settings.UserAccessService.GetNavigatorNodePermission((short)_subSystem, children.Command).AllowView)
			{
				if (children.ChildrenList == null || children.ChildrenList.Count == 0)
				{
					NavigationNode item = new NavigationNode
					{
						Command = (UserAccessIdSE)(SmartEnum<UserAccessIdSE, short>)children.Command
					};
					FillNodeHeader(children, ref item);
					item.Image = ImageService.GetImage(children.Image);
					item.TreeNodeType = children.TreeNodeType;
					arr.Add(item);
				}
				else
				{
					FillSubGroups(children, arr);
				}
			}
		}
	}

	private void FillSubGroupsWithoutAccessCheck(AsNavigationTreeNode t, List<NavigationNode> arr)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		foreach (AsNavigationTreeNode children in t.ChildrenList)
		{
			if (children.ChildrenList == null || children.ChildrenList.Count == 0)
			{
				NavigationNode item = new NavigationNode
				{
					Command = (UserAccessIdSE)(SmartEnum<UserAccessIdSE, short>)children.Command,
					Header = children.Header,
					Image = ImageService.GetImage(children.Image),
					TreeNodeType = children.TreeNodeType
				};
				arr.Add(item);
			}
			else
			{
				FillSubGroups(children, arr);
			}
		}
	}

	private static void FillNodeHeader(AsNavigationTreeNode node, ref NavigationNode item)
	{
		short command = node.Command;
		if (command == UserAccessIdSE.TaxServiceTaxInvoicesSubmitted.Value)
		{
			item.Header = ArmSoft.SME.Common.Resources.EInvoicingSubmittedInvoices;
		}
		else if (command == UserAccessIdSE.TaxServiceTaxInvoicesReceived.Value)
		{
			item.Header = ArmSoft.SME.Common.Resources.EInvoicingReceivedInvoices;
		}
		else if (command == UserAccessIdSE.TaxServiceTaxInvoicesDraft.Value)
		{
			item.Header = ArmSoft.SME.Common.Resources.EInvoicingDraftInvoices;
		}
		else if (command == UserAccessIdSE.TaxServiceAccDocsSubmitted.Value)
		{
			item.Header = ArmSoft.SME.Common.Resources.EInvoicingSubmittedAccDocs;
		}
		else if (command == UserAccessIdSE.TaxServiceAccDocsReceived.Value)
		{
			item.Header = ArmSoft.SME.Common.Resources.EInvoicingReceivedAccDocs;
		}
		else if (command == UserAccessIdSE.TaxServiceAccDocsDraft.Value)
		{
			item.Header = ArmSoft.SME.Common.Resources.EInvoicingDraftAccDocs;
		}
		else
		{
			item.Header = node.Header;
		}
	}

	public void FillGroups(AsNavigationTreeNode t)
	{
		FlowList.Children.Clear();
		FlowLayoutControl flowLayoutControl = new FlowLayoutControl
		{
			Orientation = Orientation.Vertical,
			BreakFlowToFit = false,
			Padding = new Thickness(0.0, 0.0, 0.0, 0.0)
		};
		FlowLayoutControl flowLayoutControl2 = new FlowLayoutControl
		{
			Orientation = Orientation.Vertical,
			BreakFlowToFit = false,
			Padding = new Thickness(0.0, 0.0, 0.0, 0.0)
		};
		FlowList.Children.Add(flowLayoutControl);
		FlowList.Children.Add(flowLayoutControl2);
		foreach (AsNavigationTreeNode item2 in t.ChildrenList.OrderBy((AsNavigationTreeNode x) => x.Command))
		{
			StackPanel stackPanel = new StackPanel
			{
				Orientation = Orientation.Vertical
			};
			List<NavigationNode> list = new List<NavigationNode>();
			if (Settings.LoginService.IsAdmin)
			{
				FillSubGroupsWithoutAccessCheck(item2, list);
			}
			else
			{
				FillSubGroups(item2, list);
			}
			if (list.Count <= 0)
			{
				continue;
			}
			foreach (NavigationNode item3 in list)
			{
				Hyperlink hyperlink = new Hyperlink();
				TextBlock textBlock = new TextBlock
				{
					VerticalAlignment = VerticalAlignment.Center
				};
				DXImage dXImage = new DXImage
				{
					Width = 16.0,
					Height = 16.0,
					Source = item3.Image,
					Stretch = Stretch.Fill,
					Margin = new Thickness(0.0, 2.0, 5.0, -5.0)
				};
				RenderOptions.SetBitmapScalingMode((DependencyObject)(object)dXImage, BitmapScalingMode.NearestNeighbor);
				Run item = new Run(item3.Header);
				hyperlink.TextDecorations = null;
				hyperlink.Inlines.Add(item);
				textBlock.Inlines.Add(dXImage);
				textBlock.Inlines.Add(hyperlink);
				hyperlink.NavigateUri = new Uri("http://www.armsoft.am");
				hyperlink.Tag = item3.Command;
				textBlock.Tag = item3;
				hyperlink.Foreground = Settings.GetForegroundBrush();
				hyperlink.FontSize = Settings.GetFontSize();
				textBlock.ContextMenuOpening += new SafeEventHandler<ContextMenuEventArgs>(CommandContextMenuOpening).Handler;
				hyperlink.RequestNavigate += new SafeEventHandler<RequestNavigateEventArgs>(NavigateToCommand).Handler;
				textBlock.Margin = new Thickness(5.0);
				Border border = new Border
				{
					Margin = new Thickness(8.0, 0.0, 0.0, -3.0),
					Padding = new Thickness(4.0, 2.0, 4.0, 2.0),
					VerticalAlignment = VerticalAlignment.Center,
					Background = item3.TreeNodeType.BadgeColor,
					CornerRadius = new CornerRadius(6.0)
				};
				TextBlock child = new TextBlock
				{
					FontSize = 9.0,
					FontFamily = Settings.FontFamily,
					Foreground = Brushes.White,
					FontWeight = FontWeights.Regular,
					Text = item3.TreeNodeType.Caption
				};
				border.Child = child;
				textBlock.Inlines.Add(border);
				stackPanel.Children.Add(textBlock);
			}
			Expander expander = new Expander
			{
				Header = item2.Header,
				Foreground = Brushes.DarkGray,
				ExpandDirection = ExpandDirection.Down,
				IsExpanded = true,
				Margin = new Thickness(0.0, 0.0, 0.0, 5.0),
				Content = stackPanel
			};
			expander.FontSize += 2.0;
			RenderOptions.SetBitmapScalingMode((DependencyObject)(object)expander, BitmapScalingMode.HighQuality);
			if (item2.IsInFirstColumnInHomePage)
			{
				flowLayoutControl.Children.Add(expander);
			}
			else if (item2.IsInSecondColumnInHomePage)
			{
				flowLayoutControl2.Children.Add(expander);
			}
			else
			{
				FlowList.Children.Add(expander);
			}
		}
	}

	private void CommandContextMenuOpening(object sender, ContextMenuEventArgs e)
	{
		PopupMenu popupMenu = new PopupMenu();
		TextBlock obj = sender as TextBlock;
		NavigationNode navigationNode = obj.Tag as NavigationNode;
		BarButtonItem barButtonItem = new BarButtonItem
		{
			Content = navigationNode.Header,
			Tag = new NavigationCommand
			{
				Command = navigationNode.Command,
				Subsystem = (short)_subSystem,
				CommandType = AWPCommandType.SystemCall,
				CommandTag = navigationNode.Header
			},
			Name = Settings.HostingEnvironment.UserAccessManager.GetCommandName(navigationNode.Command),
			Glyph = navigationNode.Image
		};
		obj.FontFamily = Settings.FontFamily;
		obj.FontSize = Settings.GetFontSize();
		if (MainFunctionsHelper.FindBarItemByName(Settings.HostingEnvironment.MainRibbonPanel.BarManager, barButtonItem.Name) == null)
		{
			Settings.HostingEnvironment.MainRibbonPanel.BarManager.Items.Add(barButtonItem);
		}
		barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(Btn_click).Handler;
		popupMenu.ItemLinks.Add(barButtonItem.CreateLink());
		popupMenu.ItemLinks[0].IsVisible = false;
		popupMenu.Opening += AdjustMenu;
		BarManager.SetDXContextMenu(obj, popupMenu);
		BarPopupBase.ShowElementContextMenu(obj);
	}

	private void Btn_click(object sender, ItemClickEventArgs e)
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
		BarButtonItem barButtonItem = (BarButtonItem)((BarButtonItemLink)popupMenu.ItemLinks[0]).Item;
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

	private void ExecuteNavCommand(object sender, MouseButtonEventArgs e)
	{
		ListBoxEdit listBoxEdit = (ListBoxEdit)sender;
		if (listBoxEdit.SelectedItem != null)
		{
			NavigationNode navigationNode = (NavigationNode)listBoxEdit.SelectedItem;
			if (navigationNode.Command != null)
			{
				NavigationCommand nc = new NavigationCommand
				{
					Command = navigationNode.Command,
					Subsystem = (short)_subSystem
				};
				Settings.HostingEnvironment.NavigationBarsController.ExecuteCommand(nc);
			}
		}
	}

	private void NavigateToCommand(object sender, RequestNavigateEventArgs e)
	{
		Hyperlink hyperlink = (Hyperlink)sender;
		NavigationCommand nc = new NavigationCommand
		{
			Command = ((UserAccessIdSE)hyperlink.Tag).Value,
			Subsystem = (short)_subSystem
		};
		Settings.HostingEnvironment.NavigationBarsController.ExecuteCommand(nc);
		e.Handled = true;
	}

	private void SetHyperlinkColor(object sender, MouseEventArgs e)
	{
		((Hyperlink)sender).Foreground = Brushes.Red;
	}

	private void UnSetHyperlinkColor(object sender, MouseEventArgs e)
	{
		((Hyperlink)sender).Foreground = Brushes.Orange;
	}
}
