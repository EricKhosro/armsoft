using System;
using System.Collections.Generic;
using System.Xml;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;

namespace ArmSoft.SME.Core.UI.MetaData;

public class UIField : UIElement
{
	public string Name { get; set; }

	public DataItem DataItem { get; set; }

	public DropDown DropDownItem { get; set; }

	public string ReadOnlySource { get; set; }

	public HorizontalAlignment HorizontalAlignment { get; set; }

	public bool HideCaption { get; set; }

	public bool StoreValue { get; set; }

	public bool ShowInMobile { get; set; }

	public UIField()
	{
		HorizontalAlignment = HorizontalAlignment.Left;
		HideCaption = false;
		StoreValue = true;
	}

	public static UIField Parse(XmlElement node, ArmSoft.SME.Common.MetaData.Environment environment, Dictionary<string, DataItem> dataItems)
	{
		UIField uIField = new UIField();
		foreach (XmlAttribute attribute in node.Attributes)
		{
			switch (attribute.Name)
			{
			case "DataItem":
				uIField.Name = attribute.InnerText.Trim();
				break;
			case "CaptionId":
				uIField.CaptionId = attribute.InnerText.Trim();
				break;
			case "Caption":
				uIField.Caption = attribute.InnerText.Trim();
				break;
			case "ReadOnlySource":
				uIField.ReadOnlySource = attribute.InnerText.Trim();
				break;
			case "HideCaption":
				uIField.HideCaption = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "StoreValue":
				uIField.StoreValue = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "ShowInMobile":
				uIField.ShowInMobile = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "HorizontalAlignment":
				uIField.HorizontalAlignment = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), attribute.InnerText.Trim());
				break;
			}
		}
		foreach (object childNode in node.ChildNodes)
		{
			if (!(childNode is XmlElement xmlElement))
			{
				continue;
			}
			switch (xmlElement.Name)
			{
			case "ReadOnly":
				throw new ASException(string.Format(Resources.InvalidIdentifier, "ReadOnly"));
			case "DropDown":
			case "DropDownComment":
			case "MultiSelect":
			case "FileDialog":
				if (uIField.DropDownItem != null)
				{
					throw new ASException(string.Format(Resources.DataItemHasMultipleDefinitions, xmlElement.Name));
				}
				uIField.DropDownItem = ParseDropDown(xmlElement, environment);
				break;
			}
		}
		if (!environment.IsIdentifier(uIField.Name))
		{
			throw new ASException(string.Format(Resources.InvalidIdentifier, uIField.Name));
		}
		if (dataItems.TryGetValue(uIField.Name, out var value))
		{
			uIField.DataItem = value;
			if (uIField.DataItem.IsCalculated)
			{
				throw new ASException(string.Format(Resources.DataItemNotForUI, uIField.Name));
			}
			if (!string.IsNullOrWhiteSpace(uIField.CaptionId))
			{
				uIField.Caption = environment.GetStringResource(uIField.CaptionId);
			}
			return uIField;
		}
		throw new ASException(string.Format(Resources.DataItemNotExists, uIField.Name));
	}

	private static DropDown ParseDropDown(XmlElement node, ArmSoft.SME.Common.MetaData.Environment environment)
	{
		DropDown dropDown = new DropDown(node.Name);
		if (node.Name == "DropDownComment")
		{
			dropDown.IsDropDownComment = true;
		}
		else if (node.Name == "MultiSelect")
		{
			dropDown.IsMultiSelect = true;
		}
		else if (node.Name == "FileDialog")
		{
			dropDown.IsFileDialog = true;
		}
		foreach (XmlAttribute attribute in node.Attributes)
		{
			switch (attribute.Name)
			{
			case "Name":
				dropDown.Name = attribute.InnerText.Trim();
				break;
			case "Group":
				dropDown.Group = attribute.InnerText.Trim();
				break;
			case "CheckValue":
				dropDown.CheckValue = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "ShowComment":
				dropDown.ShowComment = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "AllowAdditionButtons":
				dropDown.AllowAdditionButtons = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "IsTextEditable":
				dropDown.IsTextEditable = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			}
		}
		if (!environment.IsIdentifier(dropDown.Name))
		{
			throw new ASException(string.Format(Resources.InvalidIdentifier, dropDown.Name));
		}
		if (string.IsNullOrWhiteSpace(dropDown.Group))
		{
			dropDown.Group = dropDown.Name;
		}
		return dropDown;
	}

	public override XmlElement ToXml(XmlDocument xmlDoc)
	{
		XmlElement xmlElement = xmlDoc.CreateElement("UIField");
		xmlElement.Attributes.Append(xmlDoc.CreateAttribute("DataItem")).InnerText = Name;
		if (!string.IsNullOrWhiteSpace(CaptionId))
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("CaptionId")).InnerText = CaptionId;
		}
		else if (!string.IsNullOrWhiteSpace(Caption))
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("Caption")).InnerText = Caption;
		}
		if (HideCaption)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("HideCaption")).InnerText = "true";
		}
		if (!StoreValue)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("StoreValue")).InnerText = "false";
		}
		if (ShowInMobile)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("ShowInMobile")).InnerText = "true";
		}
		if (HorizontalAlignment != HorizontalAlignment.Left)
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("HorizontalAlignment")).InnerText = HorizontalAlignment.ToString();
		}
		if (!string.IsNullOrWhiteSpace(ReadOnlySource))
		{
			xmlElement.Attributes.Append(xmlDoc.CreateAttribute("ReadOnlySource")).InnerText = ReadOnlySource;
		}
		if (DropDownItem != null)
		{
			XmlElement xmlElement2;
			if (DropDownItem.IsDropDownComment)
			{
				xmlElement2 = xmlDoc.CreateElement("DropDownComment");
			}
			else if (DropDownItem.IsMultiSelect)
			{
				xmlElement2 = xmlDoc.CreateElement("MultiSelect");
			}
			else
			{
				xmlElement2 = ((!DropDownItem.IsFileDialog) ? xmlDoc.CreateElement("DropDown") : xmlDoc.CreateElement("FileDialog"));
			}
			xmlElement2.Attributes.Append(xmlDoc.CreateAttribute("Name")).InnerText = DropDownItem.Name;
			if (!string.IsNullOrWhiteSpace(DropDownItem.Group) && !(DropDownItem.Name == DropDownItem.Group))
			{
				xmlElement2.Attributes.Append(xmlDoc.CreateAttribute("Group")).InnerText = DropDownItem.Group;
			}
			if (!DropDownItem.CheckValue)
			{
				xmlElement2.Attributes.Append(xmlDoc.CreateAttribute("CheckValue")).InnerText = "false";
			}
			if (!DropDownItem.ShowComment)
			{
				xmlElement2.Attributes.Append(xmlDoc.CreateAttribute("ShowComment")).InnerText = "false";
			}
			if (DropDownItem.AllowAdditionButtons)
			{
				xmlElement2.Attributes.Append(xmlDoc.CreateAttribute("AllowAdditionButtons")).InnerText = "true";
			}
			if (!DropDownItem.IsTextEditable)
			{
				xmlElement2.Attributes.Append(xmlDoc.CreateAttribute("IsTextEditable")).InnerText = "false";
			}
			xmlElement.AppendChild(xmlElement2);
		}
		return xmlElement;
	}
}
