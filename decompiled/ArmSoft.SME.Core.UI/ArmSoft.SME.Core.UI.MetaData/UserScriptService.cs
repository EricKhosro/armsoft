using System.Xml;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;

namespace ArmSoft.SME.Core.UI.MetaData;

public class UserScriptService : IUserScriptService
{
	public void Store(UserScriptDefinition definition)
	{
		Settings.CreateApiClient().ExtensionsRoutes.StoreUserScript(definition);
	}

	public string LoadXML(string name)
	{
		return Settings.CreateApiClient().ExtensionsRoutes.LoadUserScript(name);
	}

	public UserScriptDefinition Load(string name)
	{
		UserScriptDefinition userScriptDefinition = new UserScriptDefinition();
		Parse(userScriptDefinition, Settings.CreateApiClient().ExtensionsRoutes.LoadUserScript(name), new Environment());
		return userScriptDefinition;
	}

	public void Parse(UserScriptDefinition definition, XmlElement xmlElement, Environment env)
	{
		InnerParse(definition, xmlElement, env);
	}

	public void Parse(UserScriptDefinition definition, string xml, Environment env)
	{
		XmlDocument xmlDocument = new XmlDocument();
		XmlElement xmlElement = null;
		xmlDocument.LoadXml(xml);
		if (xmlDocument.ChildNodes == null || xmlDocument.ChildNodes.Count <= 0)
		{
			return;
		}
		XmlNodeList xmlNodeList = ((!(xmlDocument.ChildNodes[0].Name == "Extensions")) ? xmlDocument.ChildNodes : xmlDocument.ChildNodes[0].ChildNodes);
		foreach (XmlElement item in xmlNodeList)
		{
			if (item != null)
			{
				XmlElement xmlElement3 = item;
				if (xmlElement3.Name == "UserScript")
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

	private static void InnerParse(UserScriptDefinition definition, XmlElement node, Environment env)
	{
		foreach (XmlAttribute attribute in node.Attributes)
		{
			switch (attribute.Name)
			{
			case "Name":
				definition.Name = attribute.InnerText.Trim();
				break;
			case "Caption":
				definition.Caption = attribute.InnerText.Trim();
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
		if (definition.Name.Length > 32)
		{
			throw new ASException(string.Format(Resources.FieldLengthExceeded, string.Format("{0} - {1}", "UserScript", "Name"), definition.Name, (short)32));
		}
		if (string.IsNullOrWhiteSpace(definition.Caption))
		{
			throw new ASException(string.Format(Resources.FieldIsEmpty, "Caption"));
		}
		if (definition.Caption.Length > 150)
		{
			throw new ASException(string.Format(Resources.FieldLengthExceeded, string.Format("{0} - {1}", "UserScript", "Caption"), definition.Caption, (short)150));
		}
		foreach (object childNode in node.ChildNodes)
		{
			if (childNode is XmlElement { Name: "script" } xmlElement)
			{
				definition.Script = xmlElement.InnerText;
			}
		}
	}

	public void Delete(string name)
	{
		Settings.CreateApiClient().ExtensionsRoutes.DeleteUserScript(name);
	}
}
