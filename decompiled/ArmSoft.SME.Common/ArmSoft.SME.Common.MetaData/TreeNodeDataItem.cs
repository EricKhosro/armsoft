using System.Xml;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Common.MetaData;

public class TreeNodeDataItem : DataItem
{
	public bool LeafsOnly { get; set; }

	public string TreeName { get; set; }

	public string OverriddenDefaultValue { get; set; }

	public TreeCacheRow Tree { get; set; }

	public bool AllowAdditionButtons { get; set; }

	public override string Type => "TreeNode";

	public TreeNodeDataItem()
	{
		Required = false;
		LeafsOnly = true;
		OverriddenDefaultValue = "";
		AllowAdditionButtons = false;
	}

	public static TreeNodeDataItem Parse(XmlElement node, Environment environment)
	{
		TreeNodeDataItem treeNodeDataItem = new TreeNodeDataItem();
		treeNodeDataItem.InnerParse(node, environment);
		foreach (XmlAttribute attribute in node.Attributes)
		{
			switch (attribute.Name)
			{
			case "Tree":
				treeNodeDataItem.TreeName = attribute.InnerText.Trim();
				break;
			case "Required":
				treeNodeDataItem.Required = Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "LeafsOnly":
				treeNodeDataItem.LeafsOnly = Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "DefaultValue":
				treeNodeDataItem.OverriddenDefaultValue = attribute.InnerText;
				break;
			case "AllowAdditionButtons":
				treeNodeDataItem.AllowAdditionButtons = Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			}
		}
		if (!environment.IsIdentifier(treeNodeDataItem.TreeName))
		{
			throw new RESTException(string.Format(Resources.InvalidIdentifier, treeNodeDataItem.Name));
		}
		treeNodeDataItem.Tree = environment.Tree(treeNodeDataItem.TreeName);
		return treeNodeDataItem;
	}

	public override object FromBody(string value)
	{
		return Format(value);
	}

	public string Format(string value)
	{
		value = ((value != null) ? value.TrimStart() : string.Empty);
		if (value.Length > Tree.CodeLen)
		{
			value = value.Left(Tree.CodeLen);
		}
		return value;
	}

	public override object DefaultValue()
	{
		return OverriddenDefaultValue;
	}

	public override object Assign(object value)
	{
		if (value == null)
		{
			return string.Empty;
		}
		return Format(value.ToString());
	}

	public override string ToBody(object value)
	{
		if (value == null)
		{
			return null;
		}
		return ((string)value).TrimEnd();
	}

	public override XmlElement ToXml(XmlDocument xmlDoc)
	{
		XmlElement xmlElement = xmlDoc.CreateElement("TreeNode");
		InnerFill(xmlDoc, xmlElement);
		xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Tree")).InnerText = TreeName;
		if (Required)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Required")).InnerText = "true";
		}
		if (!LeafsOnly)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("LeafsOnly")).InnerText = "false";
		}
		if (!string.IsNullOrWhiteSpace(OverriddenDefaultValue))
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("DefaultValue")).InnerText = OverriddenDefaultValue;
		}
		if (AllowAdditionButtons)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("AllowAdditionButtons")).InnerText = "true";
		}
		return xmlElement;
	}

	public override bool Validate(object value, out string msg)
	{
		string text = (string)value;
		msg = string.Empty;
		if (string.IsNullOrWhiteSpace(text))
		{
			if (Required)
			{
				msg = string.Format(Resources.FieldIsEmpty, Caption);
				return false;
			}
		}
		else
		{
			dynamic val = Tree.TreeNodeCache.GetType().GetMethod("GetItem").Invoke(Tree.TreeNodeCache, new object[1] { text });
			if (val == null)
			{
				msg = string.Format(Tree.Multilevel ? Resources.NodeNotExistTree : Resources.CodeNotExistDir, text, Tree.Caption);
			}
			else if (LeafsOnly && !val.IsLeaf)
			{
				msg = string.Format(Resources.CodeIsNorLeaf, text, Tree.Caption);
			}
		}
		return true;
	}

	public override string Print(object value)
	{
		value = Assign(value);
		return value.ToString();
	}
}
