using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.MetaData;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.Navigation;

public class UserAccessManagerBase
{
	protected const string _docUri = "doc_16.svg";

	protected const string _newDocUri = "doc_new_16.svg";

	protected string _docUriNew = (LanguageService.IsArmenian ? "doc_new_16_new.svg" : "doc_new_16_new_en.svg");

	protected const string _newDocInvoiceUri = "invoice_new_16.svg";

	protected string _newReportUri = (LanguageService.IsArmenian ? "report_16_new.svg" : "report_16_new_en.svg");

	protected const string _reportUri = "report_16.svg";

	protected string _newJournalUri = (LanguageService.IsArmenian ? "journal_16_new.svg" : "journal_16_new_en.svg");

	protected const string _journalUri = "journal_16.svg";

	protected const string _folderUri = "table_16.svg";

	protected string _newFolderUri = (LanguageService.IsArmenian ? "table_16_new.svg" : "table_16_new_en.svg");

	protected const string _redFolderUri = "table_red_16.svg";

	protected string _newRedFolderUri = (LanguageService.IsArmenian ? "table_red_16_new.svg" : "table_red_16_new_en.svg");

	protected const string _cloudRedUri = "cloud_16_red.svg";

	protected const string _cloudUpdateUri = "cloud_update_16.svg";

	protected const string _excelUri = "doc_export_excel_16.svg";

	protected const string _wordUri = "doc_export_word_16.svg";

	protected const string _dashBoardUri = "Analysis_16.svg";

	protected const string _othersUri = "action_16.svg";

	protected const string _importDataUri = "Import_16.svg";

	protected string _newOthersUri = (LanguageService.IsArmenian ? "action_16_new.svg" : "action_16_new_en.svg");

	protected const string _calendarAlertUri = "calendar_alert_16.svg";

	protected const string _cloudUri = "table_cloud_16.svg";

	protected const string _userAWPUri32 = "user_32.svg";

	protected const string _xMLUri = "doc_export_xml_16.svg";

	protected const string _layOutUri = "layout_icon16.svg";

	private const short _userAWPSubSystemId = 100;

	protected readonly bool _checkAccess;

	protected Dictionary<short, AsNavigationTreeNode> SubSystemRoots { get; set; }

	protected Dictionary<short, AsNavigationTreeNode> MainPageSubSysRoots { get; set; }

	public Dictionary<short, AsNavigationTreeNode> AccessTree => SubSystemRoots;

	public Dictionary<short, AsNavigationTreeNode> MainPageAccessTree => MainPageSubSysRoots;

	public UserAccessManagerBase(bool checkAccess = true)
	{
		_checkAccess = checkAccess;
		SubSystemRoots = new Dictionary<short, AsNavigationTreeNode>();
		MainPageSubSysRoots = new Dictionary<short, AsNavigationTreeNode>();
	}

	public AsNavigationTreeNode GetSubSysTree(short cmd)
	{
		return AccessTree.Values.FirstOrDefault((AsNavigationTreeNode p) => p.Command == cmd);
	}

	public virtual string GetCommandName(short command)
	{
		throw new NotImplementedException();
	}

	public virtual short GetCommandByName(string name)
	{
		throw new NotImplementedException();
	}

	protected bool IsSubsystemAccessible(short subsystemId, out bool checkNodeAccess)
	{
		if (_checkAccess)
		{
			return Settings.UserAccessService.IsSubsystemAccessible(subsystemId, out checkNodeAccess);
		}
		checkNodeAccess = false;
		return true;
	}

	public void BuildUserAWP(string name)
	{
		AWPDefinition aWPDefinition = Settings.ServiceProvider.GetRequiredService<IAWPService>().Load(name);
		if (aWPDefinition != null)
		{
			AsNavigationTreeNode asNavigationTreeNode = new AsNavigationTreeNode(5000, Resources.UserAWP, "user_32.svg", isViewChangeable: false, isOperChangeable: false);
			BuildUserAWPInner(aWPDefinition.AWPNodes[0].AWPTreeNodes, asNavigationTreeNode);
			SubSystemRoots.Add(100, asNavigationTreeNode);
		}
	}

	private void BuildUserAWPInner(List<AWPTreeNode> AWPNodes, AsNavigationTreeNode navTreeNode)
	{
		foreach (AWPTreeNode AWPNode in AWPNodes)
		{
			if (AWPNode.AWPTreeNodes.Count > 0)
			{
				AWPNode treeNode = AWPNode.TreeNode;
				AsNavigationTreeNode asNavigationTreeNode = new AsNavigationTreeNode((short)(5000 + treeNode.Id), treeNode.Caption, treeNode.Icon, isViewChangeable: false, isOperChangeable: false, isExpanded: true, treeNode.Tooltip);
				navTreeNode.AddChild(asNavigationTreeNode);
				BuildUserAWPInner(AWPNode.AWPTreeNodes, asNavigationTreeNode);
				continue;
			}
			AWPNode treeNode2 = AWPNode.TreeNode;
			if (treeNode2.CommandType == AWPCommandType.SystemCall)
			{
				if (!short.TryParse(treeNode2.Command, out var result))
				{
					result = GetCommandByName(treeNode2.Command);
				}
				if (result != 0)
				{
					navTreeNode.AddChild(new AsNavigationTreeNode(result, treeNode2.Caption, treeNode2.Icon, isViewChangeable: false, isOperChangeable: false, isExpanded: true, treeNode2.Tooltip));
				}
			}
			else
			{
				navTreeNode.AddChild(new AsNavigationTreeNode(0, treeNode2.Caption, treeNode2.Icon, isViewChangeable: false, isOperChangeable: false, isExpanded: true, treeNode2.Tooltip, treeNode2.CommandType, treeNode2.Command));
			}
		}
	}

	public virtual void ExportAPWsMetadata(XmlWriter writer, Dictionary<string, string> stringResourcesDictionary)
	{
		throw new ASException("Export APW Not Implemented");
	}

	protected void ExportAWPMetaData(XmlWriter writer, Dictionary<string, string> stringResourcesDictionary, string name, AsNavigationTreeNode rootNavTreeNode)
	{
		writer.WriteStartElement("AWP");
		writer.WriteAttributeString("Name", name);
		if (stringResourcesDictionary.TryGetValue(rootNavTreeNode.Header, out var value))
		{
			writer.WriteAttributeString("CaptionId", value);
			writer.WriteAttributeString("System", "true");
			writer.WriteStartElement("Nodes");
			BuildAWPFromNavigationTreeNode(writer, stringResourcesDictionary, rootNavTreeNode.ChildrenList);
			writer.WriteEndElement();
			writer.WriteEndElement();
			return;
		}
		throw new ASException(string.Format(Resources.InvalidIdentifier, rootNavTreeNode.Header));
	}

	private void BuildAWPFromNavigationTreeNode(XmlWriter writer, Dictionary<string, string> stringResourcesDictionary, List<AsNavigationTreeNode> navTreeNodes)
	{
		foreach (AsNavigationTreeNode navTreeNode in navTreeNodes)
		{
			if (navTreeNode.ChildrenList.Count == 0)
			{
				writer.WriteStartElement("AWPLeaf");
				if (navTreeNode.CommandType == AWPCommandType.UserReport)
				{
					writer.WriteAttributeString("CommandType", "UserReport");
					writer.WriteAttributeString("Command", navTreeNode.CommandTag);
				}
				else if (navTreeNode.CommandType == AWPCommandType.Tree)
				{
					writer.WriteAttributeString("CommandType", "Tree");
					writer.WriteAttributeString("Command", navTreeNode.CommandTag);
				}
				else
				{
					writer.WriteAttributeString("Command", GetCommandName(navTreeNode.Command));
				}
				BuildCommonAWPNode(writer, stringResourcesDictionary, navTreeNode);
				writer.WriteEndElement();
			}
			else
			{
				BuildAWPFromNavigationTreeNode(writer, stringResourcesDictionary, navTreeNode.ChildrenList);
			}
		}
	}

	private static void BuildCommonAWPNode(XmlWriter writer, Dictionary<string, string> stringResourcesDictionary, AsNavigationTreeNode node)
	{
		if (stringResourcesDictionary.TryGetValue(node.Header, out var value))
		{
			writer.WriteAttributeString("CaptionId", value);
		}
		else
		{
			writer.WriteAttributeString("Caption", node.Header);
		}
		if (!(node.Header == node.FullHeader))
		{
			if (stringResourcesDictionary.TryGetValue(node.FullHeader, out value))
			{
				writer.WriteAttributeString("TooltipId", value);
			}
			else
			{
				writer.WriteAttributeString("Tooltip", node.FullHeader);
			}
		}
		if (!string.IsNullOrWhiteSpace(node.Image))
		{
			writer.WriteAttributeString("Icon", node.Image);
		}
	}
}
