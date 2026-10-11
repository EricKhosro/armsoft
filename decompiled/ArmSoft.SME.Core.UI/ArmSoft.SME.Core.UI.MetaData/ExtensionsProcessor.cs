using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.MetaData.Document;
using ArmSoft.SME.Common.MetaData.Tree;
using ArmSoft.SME.Core.UI.MetaData.Tree;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.MetaData;

public class ExtensionsProcessor
{
	private readonly List<AWPDefinition> _awpDefinitions;

	private readonly List<UserScriptDefinition> _userScriptsDefinitions;

	private readonly Dictionary<DataViewDefinition, string> _dataViewDefinitions;

	private readonly Dictionary<string, TreeDefinition> _treeDefinitions;

	private readonly Dictionary<string, string> _treeExtensionDefinitions;

	private readonly Dictionary<string, StringResource> _stringResources;

	private readonly Dictionary<DocumentExtensionKey, string> _documentExtensions;

	private readonly Dictionary<ContextFunctionDefinition, string> _contextFunctionsDefinitions;

	internal ExtensionsProcessor(XmlDocument xmlDoc, Environment env)
	{
		_awpDefinitions = new List<AWPDefinition>();
		_userScriptsDefinitions = new List<UserScriptDefinition>();
		_dataViewDefinitions = new Dictionary<DataViewDefinition, string>();
		_treeDefinitions = new Dictionary<string, TreeDefinition>();
		_treeExtensionDefinitions = new Dictionary<string, string>();
		_stringResources = new Dictionary<string, StringResource>();
		_documentExtensions = new Dictionary<DocumentExtensionKey, string>();
		_contextFunctionsDefinitions = new Dictionary<ContextFunctionDefinition, string>();
		foreach (object childNode in xmlDoc.ChildNodes)
		{
			if (!(childNode is XmlElement { Name: "Extensions" } xmlElement))
			{
				continue;
			}
			foreach (object childNode2 in xmlElement.ChildNodes)
			{
				if (!(childNode2 is XmlElement xmlElement2))
				{
					continue;
				}
				switch (xmlElement2.Name)
				{
				case "Resources":
					ParseResources(xmlElement2, env);
					break;
				case "Tree":
				{
					TreeDefinition treeDefinition = Settings.ServiceProvider.GetRequiredService<ITreeDefinitionService>().Parse(xmlElement2, env);
					if (_treeDefinitions.ContainsKey(treeDefinition.Name))
					{
						throw new ASException(string.Format(Resources.TreeHasMultipleDefinitions, treeDefinition.Name));
					}
					_treeDefinitions.Add(treeDefinition.Name, treeDefinition);
					break;
				}
				case "DocumentExtension":
				{
					DocumentExtensionDefinition documentExtensionDefinition = new DocumentExtensionDefinition(xmlElement2, env);
					_documentExtensions.Add(new DocumentExtensionKey(documentExtensionDefinition.POTypeDef, documentExtensionDefinition.POType), ConvertXmlElementToString(xmlElement2));
					break;
				}
				case "AWP":
				{
					AWPDefinition aWPDefinition = new AWPDefinition();
					Settings.ServiceProvider.GetRequiredService<IAWPService>().Parse(aWPDefinition, xmlElement2, env);
					_awpDefinitions.Add(aWPDefinition);
					break;
				}
				case "View":
				{
					DataViewDefinition dataViewDefinition = new DataViewDefinition();
					Settings.ServiceProvider.GetRequiredService<IDVDService>().Parse(dataViewDefinition, xmlElement2, env);
					_dataViewDefinitions.Add(dataViewDefinition, ConvertXmlElementToString(xmlElement2));
					break;
				}
				case "TreeExtension":
				{
					TreeExtensionDefinitionEx treeExtensionDefinitionEx = new TreeExtensionDefinitionEx(xmlElement2, env);
					_treeExtensionDefinitions.Add(treeExtensionDefinitionEx.Name, ConvertXmlElementToString(xmlElement2));
					break;
				}
				case "UserScript":
				{
					UserScriptDefinition userScriptDefinition = new UserScriptDefinition();
					Settings.ServiceProvider.GetRequiredService<IUserScriptService>().Parse(userScriptDefinition, xmlElement2, env);
					_userScriptsDefinitions.Add(userScriptDefinition);
					break;
				}
				case "ContextFunction":
				{
					ContextFunctionDefinition contextFunctionDefinition = new ContextFunctionDefinition();
					Settings.ServiceProvider.GetRequiredService<IContextFunctionService>().Parse(contextFunctionDefinition, xmlElement2, env);
					_contextFunctionsDefinitions.Add(contextFunctionDefinition, ConvertXmlElementToString(xmlElement2));
					break;
				}
				}
			}
		}
	}

	public static ExtensionsProcessor Load(string filename, Environment environment)
	{
		XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
		XmlDocument xmlDocument = null;
		ValidationEventArgs validationErr = null;
		using (Stream stream = GetXmlSchemaStream())
		{
			XmlSchema schema = XmlSchema.Read(stream, (object? sender, ValidationEventArgs e) =>
			{
				validationErr = e;
			});
			xmlReaderSettings.Schemas.Add(schema);
		}
		xmlReaderSettings.ValidationType = ValidationType.Schema;
		using (XmlReader reader = XmlReader.Create(filename, xmlReaderSettings))
		{
			xmlDocument = new XmlDocument();
			xmlDocument.Load(reader);
		}
		if (validationErr == null)
		{
			environment.CurrentDirectory = Path.GetDirectoryName(filename);
			return new ExtensionsProcessor(xmlDocument, environment);
		}
		throw validationErr.Exception;
	}

	public bool Store()
	{
		if (_stringResources.Count > 0)
		{
			Settings.StringResourceService.Store(_stringResources.Values.ToList());
		}
		if (_treeDefinitions.Count > 0)
		{
			foreach (KeyValuePair<string, TreeDefinition> treeDefinition in _treeDefinitions)
			{
				Settings.ServiceProvider.GetRequiredService<ITreeDefinitionService>().Store(treeDefinition.Value);
			}
			Settings.CoreCacheService.Tree.Refresh();
		}
		if (_documentExtensions.Count > 0)
		{
			foreach (KeyValuePair<DocumentExtensionKey, string> documentExtension in _documentExtensions)
			{
				Settings.ServiceProvider.GetRequiredService<IDocumentExtensionDefinitionService>().Store(documentExtension.Key, documentExtension.Value, updateCache: false);
			}
			Settings.CoreCacheService.DocumentExtensions.Refresh(checkChanging: false);
		}
		if (_treeExtensionDefinitions.Count > 0)
		{
			foreach (KeyValuePair<string, string> treeExtensionDefinition in _treeExtensionDefinitions)
			{
				Settings.ServiceProvider.GetRequiredService<ITreeDefinitionService>().StoreExtensionXml(new TreeExtensionModel(treeExtensionDefinition.Key, treeExtensionDefinition.Value), updateCache: false);
			}
			Settings.CoreCacheService.Tree.Refresh(checkChanging: false);
		}
		foreach (AWPDefinition awpDefinition in _awpDefinitions)
		{
			Settings.ServiceProvider.GetRequiredService<IAWPService>().Store(awpDefinition);
		}
		foreach (UserScriptDefinition userScriptsDefinition in _userScriptsDefinitions)
		{
			Settings.ServiceProvider.GetRequiredService<IUserScriptService>().Store(userScriptsDefinition);
		}
		foreach (KeyValuePair<DataViewDefinition, string> dataViewDefinition in _dataViewDefinitions)
		{
			Settings.ServiceProvider.GetRequiredService<IDVDService>().Store(dataViewDefinition.Key, dataViewDefinition.Value);
		}
		foreach (KeyValuePair<ContextFunctionDefinition, string> contextFunctionsDefinition in _contextFunctionsDefinitions)
		{
			Settings.ServiceProvider.GetRequiredService<IContextFunctionService>().Store(contextFunctionsDefinition.Key, contextFunctionsDefinition.Value);
		}
		if (_stringResources.Count == 0 && _treeDefinitions.Count == 0 && _awpDefinitions.Count == 0 && _userScriptsDefinitions.Count == 0 && _dataViewDefinitions.Count == 0 && _documentExtensions.Count == 0)
		{
			return _contextFunctionsDefinitions.Count != 0;
		}
		return true;
	}

	internal static Stream GetXmlSchemaStream()
	{
		return Assembly.GetExecutingAssembly().GetManifestResourceStream("ArmSoft.SME.Core.UI.MetaData.SysDefs.xsd");
	}

	private static string ConvertXmlElementToString(XmlElement xmlElement)
	{
		if (xmlElement == null)
		{
			throw new ArgumentNullException("xmlElement", "XmlElement cannot be null.");
		}
		using StringWriter stringWriter = new StringWriter();
		using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
		xmlTextWriter.Formatting = Formatting.Indented;
		xmlElement.WriteTo(xmlTextWriter);
		return stringWriter.ToString();
	}

	private void ParseResources(XmlElement rootNode, Environment env)
	{
		foreach (object childNode in rootNode.ChildNodes)
		{
			if (!(childNode is XmlElement xmlElement))
			{
				continue;
			}
			string name = xmlElement.Name;
			if (!(name == "Constant"))
			{
				if (!(name == "StringResource"))
				{
					continue;
				}
				StringResource stringResource = new StringResource
				{
					Name = xmlElement.Attributes["Name"].InnerText.Trim()
				};
				if (!env.IsIdentifier(stringResource.Name))
				{
					throw new ASException(string.Format(Resources.InvalidIdentifier, stringResource.Name));
				}
				foreach (object childNode2 in xmlElement.ChildNodes)
				{
					if (childNode2 is XmlElement xmlElement2)
					{
						stringResource.Values.Add(xmlElement2.Name.Trim(), xmlElement2.InnerText);
					}
				}
				if (_stringResources.ContainsKey(stringResource.Name))
				{
					throw new ASException(string.Format(Resources.StringHasMultipleDefinitions, stringResource.Name));
				}
				_stringResources.Add(stringResource.Name, stringResource);
			}
			else
			{
				string text = xmlElement.Attributes["Name"].InnerText.Trim();
				if (!env.IsIdentifier(text))
				{
					throw new ASException(string.Format(Resources.InvalidIdentifier, text));
				}
				if (env.Constants.ContainsKey(text))
				{
					throw new ASException(string.Format(Resources.ConstantHasMultipleDefinitions, text));
				}
				env.Constants.Add(text, xmlElement.Attributes["Value"].InnerText);
			}
		}
	}
}
