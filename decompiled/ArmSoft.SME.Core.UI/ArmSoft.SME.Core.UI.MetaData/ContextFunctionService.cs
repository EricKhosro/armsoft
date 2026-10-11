using System;
using System.Collections.Generic;
using System.Xml;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Helpers;

namespace ArmSoft.SME.Core.UI.MetaData;

public class ContextFunctionService : IContextFunctionService
{
	public void Store(ContextFunctionDefinition definition, string xml)
	{
		if (SmartEnumHelper.GetValue<short>(typeof(DataProviderTypeSE), definition.Group) == 0)
		{
			throw new InvalidOperationException("View group name '" + definition.Group + "' not supported.");
		}
		Settings.CreateApiClient().ExtensionsRoutes.StoreContextFunction(definition);
	}

	public string LoadXML(string group, string name)
	{
		return Settings.CreateApiClient().ExtensionsRoutes.LoadContextFunction(group, name);
	}

	public ContextFunctionDefinition Load(string group, string name)
	{
		ContextFunctionDefinition contextFunctionDefinition = new ContextFunctionDefinition();
		Parse(contextFunctionDefinition, LoadXML(group, name), new Environment());
		return contextFunctionDefinition;
	}

	public List<string> Load(string group)
	{
		return Settings.CreateApiClient().ExtensionsRoutes.LoadContextFunctions(group);
	}

	public void Parse(ContextFunctionDefinition definition, XmlElement xmlElement, Environment env)
	{
		InnerParse(definition, xmlElement, env);
	}

	public void Parse(ContextFunctionDefinition definition, string xml, Environment environment)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlElement xmlElement = null;
		foreach (object childNode in xmlDocument.ChildNodes)
		{
			if (childNode is XmlElement { Name: "ContextFunction" } xmlElement2)
			{
				xmlElement = xmlElement2;
				break;
			}
		}
		if (xmlElement != null)
		{
			InnerParse(definition, xmlElement, environment);
		}
	}

	private static void InnerParse(ContextFunctionDefinition definition, XmlElement node, Environment environment)
	{
		definition.IsEnabled = true;
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
			case "IsEnabled":
				definition.IsEnabled = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "Order":
				definition.Order = environment.ParseShort(attribute.InnerText.Trim());
				break;
			case "ScriptFile":
				definition.Script = environment.LoadScriptFromFile(attribute.InnerText.Trim());
				break;
			}
		}
		if (!environment.IsIdentifier(definition.Name))
		{
			throw new ASException(string.Format(Resources.InvalidIdentifier, definition.Name));
		}
		if (definition.Name.Length > 20)
		{
			throw new ASException(string.Format(Resources.FieldLengthExceeded, string.Format("{0} - {1}", "ContextFunction", "Name"), definition.Name, (short)20));
		}
		if (!environment.IsIdentifier(definition.CaptionId))
		{
			throw new ASException(string.Format(Resources.InvalidIdentifier, definition.CaptionId));
		}
		definition.Caption = environment.GetStringResource(definition.CaptionId);
		if (!environment.IsIdentifier(definition.Group))
		{
			throw new ASException(string.Format(Resources.InvalidIdentifier, definition.Group));
		}
		if (definition.Order == 0)
		{
			throw new ASException(string.Format(Resources.InvalidIdentifier, definition.Order.ToString()));
		}
		foreach (object childNode in node.ChildNodes)
		{
			if (childNode is XmlElement { Name: "script" } xmlElement)
			{
				definition.Script = xmlElement.InnerText;
			}
		}
	}

	public void Delete(string group, string name)
	{
		Settings.CreateApiClient().ExtensionsRoutes.DeleteContextFunction(group, name);
	}

	public void ChangeEnabled(string group, string name, bool value)
	{
		Settings.CreateApiClient().ExtensionsRoutes.ContextFunctionChangeEnabled(group, name, value);
	}
}
