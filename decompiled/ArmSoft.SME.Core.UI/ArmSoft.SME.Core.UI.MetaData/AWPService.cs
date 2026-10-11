using System;
using System.Collections.Generic;
using System.Xml;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;

namespace ArmSoft.SME.Core.UI.MetaData;

public class AWPService : IAWPService
{
	public void Store(AWPDefinition definition)
	{
		Settings.CreateApiClient().ExtensionsRoutes.StoreAWP(definition);
	}

	public string LoadXML(string name)
	{
		return Settings.CreateApiClient().ExtensionsRoutes.LoadAWP(name);
	}

	public AWPDefinition Load(string name)
	{
		AWPDefinition aWPDefinition = new AWPDefinition();
		Parse(aWPDefinition, Settings.CreateApiClient().ExtensionsRoutes.LoadAWP(name), new Environment());
		return aWPDefinition;
	}

	public void Delete(string name)
	{
		Settings.CreateApiClient().ExtensionsRoutes.DeleteAWP(name);
	}

	public void Parse(AWPDefinition definition, XmlElement xmlElement, Environment env)
	{
		InnerParse(definition, xmlElement, env);
	}

	public void Parse(AWPDefinition definition, string xml, Environment env)
	{
		XmlDocument xmlDocument = new XmlDocument();
		XmlElement xmlElement = null;
		xmlDocument.LoadXml(xml);
		if (xmlDocument.ChildNodes == null || xmlDocument.ChildNodes.Count <= 0)
		{
			return;
		}
		XmlNodeList xmlNodeList = ((!(xmlDocument.ChildNodes[0].Name == "Extensions")) ? xmlDocument.ChildNodes : xmlDocument.ChildNodes[0].ChildNodes);
		foreach (object item in xmlNodeList)
		{
			if (item is XmlElement { Name: "AWP" } xmlElement2)
			{
				xmlElement = xmlElement2;
				break;
			}
		}
		if (xmlElement != null)
		{
			InnerParse(definition, xmlElement, env);
		}
	}

	private static void InnerParse(AWPDefinition definition, XmlElement xmlElement, Environment env)
	{
		foreach (XmlAttribute attribute in xmlElement.Attributes)
		{
			switch (attribute.Name)
			{
			case "Name":
				definition.Name = attribute.InnerText.Trim();
				break;
			case "CaptionId":
				definition.CaptionId = attribute.InnerText.Trim();
				break;
			case "System":
				definition.System = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			}
		}
		if (!env.IsIdentifier(definition.Name))
		{
			throw new ASException(string.Format(Resources.InvalidIdentifier, definition.Name));
		}
		if (definition.Name.Length > 20)
		{
			throw new ASException(string.Format(Resources.FieldLengthExceeded, string.Format("{0} - {1}", "AWP", "Name"), definition.Name, (short)20));
		}
		if (!env.IsIdentifier(definition.CaptionId))
		{
			throw new ASException(string.Format(Resources.InvalidIdentifier, definition.CaptionId));
		}
		definition.Caption = env.GetStringResource(definition.CaptionId);
		int num = 0;
		short id = 0;
		definition.AWPNodes = new List<AWPTreeNode>();
		AWPTreeNode aWPTreeNode = new AWPTreeNode();
		definition.AWPNodes.Add(aWPTreeNode);
		foreach (object childNode in xmlElement.ChildNodes)
		{
			if (!(childNode is XmlElement xmlElement2))
			{
				continue;
			}
			string name = xmlElement2.Name;
			if (!(name == "AWPNode"))
			{
				if (name == "AWPLeaf")
				{
					num++;
					Parse(xmlElement2, env, definition.AWPNodes, aWPTreeNode, num, isLeaf: true, ref id);
				}
			}
			else
			{
				num++;
				id++;
				Parse(xmlElement2, env, definition.AWPNodes, aWPTreeNode, num, isLeaf: false, ref id);
			}
		}
	}

	public static AWPNode Parse(XmlElement xmlElement, Environment environment, List<AWPTreeNode> nodes, AWPTreeNode ownAPWTreeNode, int order, bool isLeaf, ref short id)
	{
		AWPNode aWPNode = new AWPNode();
		foreach (XmlAttribute attribute in xmlElement.Attributes)
		{
			switch (attribute.Name)
			{
			case "CaptionId":
				aWPNode.CaptionId = attribute.InnerText.Trim();
				break;
			case "Caption":
				aWPNode.Caption = attribute.InnerText.Trim();
				break;
			case "TooltipId":
				aWPNode.TooltipId = attribute.InnerText.Trim();
				break;
			case "Tooltip":
				aWPNode.Tooltip = attribute.InnerText.Trim();
				break;
			case "Icon":
				aWPNode.Icon = attribute.InnerText.Trim();
				break;
			case "CommandType":
				aWPNode.CommandType = (AWPCommandType)Enum.Parse(typeof(AWPCommandType), attribute.InnerText.Trim());
				break;
			case "Command":
				aWPNode.Command = attribute.InnerText.Trim();
				break;
			}
		}
		if (!string.IsNullOrWhiteSpace(aWPNode.CaptionId))
		{
			if (!environment.IsIdentifier(aWPNode.CaptionId))
			{
				throw new ASException(string.Format(Resources.InvalidIdentifier, aWPNode.CaptionId));
			}
			aWPNode.Caption = environment.GetStringResource(aWPNode.CaptionId);
		}
		if (string.IsNullOrWhiteSpace(aWPNode.Caption))
		{
			throw new ASException(string.Format(Resources.FieldIsEmpty, "Caption"));
		}
		if (string.IsNullOrWhiteSpace(aWPNode.TooltipId))
		{
			if (string.IsNullOrWhiteSpace(aWPNode.Tooltip))
			{
				aWPNode.Tooltip = aWPNode.Caption;
			}
		}
		else
		{
			if (!environment.IsIdentifier(aWPNode.TooltipId))
			{
				throw new ASException(string.Format(Resources.InvalidIdentifier, aWPNode.TooltipId));
			}
			aWPNode.Tooltip = environment.GetStringResource(aWPNode.TooltipId);
		}
		if (!isLeaf && aWPNode.Id == 0)
		{
			aWPNode.Id = id;
		}
		AWPTreeNode aWPTreeNode2;
		if (isLeaf)
		{
			AWPTreeNode aWPTreeNode = new AWPTreeNode
			{
				TreeNode = aWPNode
			};
			ownAPWTreeNode.AWPTreeNodes.Add(aWPTreeNode);
			aWPTreeNode.Order = order;
			aWPTreeNode2 = ownAPWTreeNode;
		}
		else
		{
			aWPTreeNode2 = new AWPTreeNode
			{
				TreeNode = aWPNode
			};
			ownAPWTreeNode.AWPTreeNodes.Add(aWPTreeNode2);
			aWPTreeNode2.Order = order;
		}
		foreach (object childNode in xmlElement.ChildNodes)
		{
			if (!(childNode is XmlElement xmlElement2))
			{
				continue;
			}
			string name = xmlElement2.Name;
			if (!(name == "AWPNode"))
			{
				if (name == "AWPLeaf")
				{
					order++;
					Parse(xmlElement2, environment, nodes, aWPTreeNode2, order, isLeaf: true, ref id);
				}
			}
			else
			{
				order++;
				id++;
				Parse(xmlElement2, environment, nodes, aWPTreeNode2, order, isLeaf: false, ref id);
			}
		}
		return aWPNode;
	}
}
