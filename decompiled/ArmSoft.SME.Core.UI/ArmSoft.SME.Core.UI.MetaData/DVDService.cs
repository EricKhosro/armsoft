using System;
using System.Collections.Generic;
using System.Xml;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Helpers;

namespace ArmSoft.SME.Core.UI.MetaData;

public class DVDService : IDVDService
{
	public void Store(DataViewDefinition definition, string xml)
	{
		if (SmartEnumHelper.GetValue<short>(typeof(DataProviderTypeSE), definition.Group) == 0)
		{
			throw new InvalidOperationException("View group name '" + definition.Group + "' not supported.");
		}
		Settings.CreateApiClient().ExtensionsRoutes.StoreDVD(new ArmSoft.SME.Common.MetaData.DataViewDefinition
		{
			CaptionId = definition.CaptionId,
			Group = definition.Group,
			IsEnabled = definition.IsEnabled,
			Name = definition.Name,
			Xml = xml,
			System = definition.System
		});
	}

	public string LoadXML(string group, string name)
	{
		return Settings.CreateApiClient().ExtensionsRoutes.LoadDVD(group, name);
	}

	public DataViewDefinition Load(string group, string name)
	{
		DataViewDefinition dataViewDefinition = null;
		string text = Settings.CreateApiClient().ExtensionsRoutes.LoadDVD(group, name);
		if (!string.IsNullOrWhiteSpace(text))
		{
			dataViewDefinition = new DataViewDefinition();
			Parse(dataViewDefinition, text, new Environment());
		}
		return dataViewDefinition;
	}

	public DataViewDefinition LoadDefault(string group)
	{
		DataViewDefinition dataViewDefinition = new DataViewDefinition();
		Parse(dataViewDefinition, Settings.CreateApiClient().ExtensionsRoutes.LoadDefaultDVD(group), new Environment());
		return dataViewDefinition;
	}

	public List<string> Load(string group)
	{
		return Settings.CreateApiClient().ExtensionsRoutes.LoadDVDs(group);
	}

	public void Parse(DataViewDefinition definition, XmlElement xmlElement, Environment env)
	{
		InnerParse(definition, xmlElement, env);
	}

	public void Parse(DataViewDefinition definition, string xml, Environment env)
	{
		XmlDocument xmlDocument = new XmlDocument();
		XmlElement xmlElement = null;
		xmlDocument.LoadXml(xml);
		foreach (XmlElement childNode in xmlDocument.ChildNodes)
		{
			if (childNode != null)
			{
				XmlElement xmlElement3 = childNode;
				if (xmlElement3.Name == "View")
				{
					xmlElement = xmlElement3;
					break;
				}
			}
		}
		if (xmlElement != null)
		{
			InnerParse(definition, xmlElement, env);
		}
	}

	private static void InnerParse(DataViewDefinition definition, XmlElement node, Environment env)
	{
		definition.System = false;
		definition.IsEnabled = true;
		definition.PreserveColumns = true;
		definition.Indication = ViewLoadIndication.Default;
		definition.CalculatedColumns = new Dictionary<string, CalculatedColumn>();
		definition.ViewColumns = new List<ViewColumn>();
		definition.SortOrder = new List<ColumnOrder>();
		foreach (XmlAttribute attribute in node.Attributes)
		{
			switch (attribute.Name)
			{
			case "Name":
				definition.Name = attribute.InnerText.Trim();
				break;
			case "CaptionId":
				definition.CaptionId = attribute.InnerText.Trim();
				break;
			case "Group":
				definition.Group = attribute.InnerText.Trim();
				break;
			case "System":
				definition.System = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "IsEnabled":
				definition.IsEnabled = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "PreserveColumns":
				definition.PreserveColumns = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "Indication":
				definition.Indication = (ViewLoadIndication)Enum.Parse(typeof(ViewLoadIndication), attribute.InnerText.Trim());
				break;
			case "ScriptFile":
				definition.Script = env.LoadScriptFromFile(attribute.InnerText.Trim());
				break;
			}
		}
		if (!env.IsIdentifier(definition.Name))
		{
			throw new ASException(string.Format(Resources.InvalidIdentifier, definition.Name));
		}
		if (definition.Name.Length > 30)
		{
			throw new ASException(string.Format(Resources.FieldLengthExceeded, string.Format("{0} - {1}", "View", "Name"), definition.Name, (short)30));
		}
		if (!env.IsIdentifier(definition.CaptionId))
		{
			throw new ASException(string.Format(Resources.InvalidIdentifier, definition.CaptionId));
		}
		definition.Caption = env.GetStringResource(definition.CaptionId);
		if (!env.IsIdentifier(definition.Group))
		{
			throw new ASException(string.Format(Resources.InvalidIdentifier, definition.Group));
		}
		foreach (object childNode in node.ChildNodes)
		{
			if (childNode is XmlElement xmlElement)
			{
				switch (xmlElement.Name)
				{
				case "ViewColumns":
					ParseViewColumns(definition, xmlElement, env);
					break;
				case "SortOrder":
					ParseSortOrder(xmlElement, definition.SortOrder);
					break;
				case "CalculatedColumns":
					ParseCalculatedColumns(xmlElement, env, definition.CalculatedColumns);
					break;
				case "DialogExtension":
					ParseDialogExtension(definition, xmlElement, env);
					break;
				case "script":
					definition.Script = xmlElement.InnerText;
					break;
				}
			}
		}
	}

	private static void ParseDialogExtension(DataViewDefinition definition, XmlElement node, Environment environment)
	{
		definition.DialogExtension = new DataViewDialogExtension
		{
			DataItems = new Dictionary<string, DataItem>(),
			Elements = new List<UIElement>()
		};
		foreach (object childNode in node.ChildNodes)
		{
			if (!(childNode is XmlElement xmlElement))
			{
				continue;
			}
			string name = xmlElement.Name;
			if (!(name == "Data"))
			{
				if (name == "UI")
				{
					ExtensionUIHelper.ParseUI(xmlElement, environment, definition.DialogExtension.DataItems, definition.DialogExtension.Elements);
				}
			}
			else
			{
				ExtensionDefinitionBase.ParseData(xmlElement, environment, definition.DialogExtension.DataItems);
			}
		}
	}

	private static void ParseCalculatedColumns(XmlElement node, Environment environment, Dictionary<string, CalculatedColumn> calculatedColumns)
	{
		foreach (object childNode in node.ChildNodes)
		{
			if (childNode is XmlElement { Name: "CalculatedColumn" } xmlElement)
			{
				CalculatedColumn calculatedColumn = CalculatedColumn.Parse(xmlElement, environment);
				calculatedColumns.Add(calculatedColumn.Name, calculatedColumn);
			}
		}
	}

	private static void ParseSortOrder(XmlElement node, List<ColumnOrder> sortOrder)
	{
		foreach (object childNode in node.ChildNodes)
		{
			if (childNode is XmlElement { Name: "ColumnOrder" } xmlElement)
			{
				ParseColumnOrder(xmlElement, sortOrder);
			}
		}
	}

	private static void ParseColumnOrder(XmlElement node, List<ColumnOrder> sortOrder)
	{
		ColumnOrder columnOrder = new ColumnOrder();
		foreach (XmlAttribute attribute in node.Attributes)
		{
			string name = attribute.Name;
			if (!(name == "Name"))
			{
				if (name == "Descending")
				{
					columnOrder.Descending = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				}
			}
			else
			{
				columnOrder.Name = attribute.InnerText.Trim();
			}
		}
		sortOrder.Add(columnOrder);
	}

	private static void ParseViewColumns(DataViewDefinition definition, XmlElement node, Environment environment)
	{
		foreach (object childNode in node.ChildNodes)
		{
			if (childNode is XmlElement xmlElement)
			{
				switch (xmlElement.Name)
				{
				case "AddColumn":
					ParseColumn(definition, xmlElement, ViewColumnAction.Add, environment);
					break;
				case "ModifyColumn":
					ParseColumn(definition, xmlElement, ViewColumnAction.Modify, environment);
					break;
				case "RemoveColumn":
					RemoveColumn(definition, xmlElement);
					break;
				}
			}
		}
	}

	private static void RemoveColumn(DataViewDefinition definition, XmlElement node)
	{
		ViewColumn viewColumn = new ViewColumn
		{
			Action = ViewColumnAction.Remove
		};
		foreach (XmlAttribute attribute in node.Attributes)
		{
			if (attribute.Name == "Name")
			{
				viewColumn.Name = attribute.InnerText.Trim();
			}
		}
		definition.ViewColumns.Add(viewColumn);
	}

	private static void ParseColumn(DataViewDefinition definition, XmlElement node, ViewColumnAction actionType, Environment environment)
	{
		ViewColumn viewColumn = new ViewColumn
		{
			Action = actionType
		};
		foreach (XmlAttribute attribute in node.Attributes)
		{
			switch (attribute.Name)
			{
			case "Name":
				viewColumn.Name = attribute.InnerText.Trim();
				break;
			case "CaptionId":
				viewColumn.CaptionId = attribute.InnerText.Trim();
				break;
			case "Caption":
				viewColumn.Caption = attribute.InnerText.Trim();
				break;
			case "FooterSum":
				viewColumn.FooterSum = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "Visible":
				viewColumn.Visible = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "After":
				viewColumn.After = attribute.InnerText.Trim();
				break;
			case "Before":
				viewColumn.Before = attribute.InnerText.Trim();
				break;
			case "Format":
				viewColumn.Format = attribute.InnerText.Trim();
				break;
			}
		}
		if (!string.IsNullOrWhiteSpace(viewColumn.CaptionId))
		{
			if (!environment.IsIdentifier(viewColumn.CaptionId))
			{
				throw new ASException(string.Format(Resources.InvalidIdentifier, viewColumn.CaptionId));
			}
			viewColumn.Caption = environment.GetStringResource(viewColumn.CaptionId);
		}
		definition.ViewColumns.Add(viewColumn);
	}

	public void Delete(string group, string name)
	{
		Settings.CreateApiClient().ExtensionsRoutes.DeleteDVD(group, name);
	}

	public void ChangeEnabled(string group, string name, bool value)
	{
		Settings.CreateApiClient().ExtensionsRoutes.DVDChangeEnabled(group, name, value);
	}

	public void ChangeDefault(string group, string name, bool value)
	{
		Settings.CreateApiClient().ExtensionsRoutes.DVDChangeDefault(group, name, value);
	}

	public void UpdateLastExecDate(string name)
	{
		Settings.CreateApiClient().ExtensionsRoutes.DVDUpdateLastExecDate(name);
	}
}
