using System.Xml;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.MetaData.Tree;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Core.UI.MetaData.Tree;

namespace ArmSoft.SME.Core.UI.MetaData;

public class TreeDefinitionService : ITreeDefinitionService
{
	public void Store(TreeDefinition definition)
	{
		Settings.CreateApiClient().ExtensionsRoutes.StoreTree(definition);
		Settings.CoreCacheService.Tree.Refresh();
	}

	public void Delete(string name)
	{
		Settings.CreateApiClient().ExtensionsRoutes.DeleteTree(name);
		Settings.CoreCacheService.Tree.Refresh();
	}

	public TreeDefinition Parse(XmlElement node, Environment env)
	{
		TreeDefinition treeDefinition = new TreeDefinition();
		foreach (XmlAttribute attribute in node.Attributes)
		{
			switch (attribute.Name)
			{
			case "Name":
				treeDefinition.Name = attribute.InnerText.Trim();
				break;
			case "CaptionId":
				treeDefinition.CaptionId = attribute.InnerText.Trim();
				break;
			case "NodeCaptionId":
				treeDefinition.NodeCaptionId = attribute.InnerText.Trim();
				break;
			case "CodeCaptionId":
				treeDefinition.CodeCaptionId = attribute.InnerText.Trim();
				break;
			case "SpecCaptionId":
				treeDefinition.SpecCaptionId = attribute.InnerText.Trim();
				break;
			case "Multilevel":
				treeDefinition.Multilevel = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "PrefixBased":
				treeDefinition.PrefixBased = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "DocumentBased":
				treeDefinition.DocumentBased = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "System":
				treeDefinition.System = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "IsLogAvailable":
				treeDefinition.IsLogAvailable = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "IsClosedAvailable":
				treeDefinition.IsClosedAvailable = ArmSoft.SME.Common.MetaData.Environment.ParseBoolean(attribute.InnerText.Trim());
				break;
			case "AttachmentsTypes":
				treeDefinition.AttachmentsTypes = ArmSoft.SME.Common.MetaData.Environment.ParseAttachType(attribute.InnerText.Trim());
				break;
			case "CodeLen":
				treeDefinition.CodeLen = env.ParseShort(attribute.InnerText.Trim());
				break;
			case "CaptionLen":
				treeDefinition.CaptionLen = env.ParseShort(attribute.InnerText.Trim());
				break;
			case "SpecLen":
				treeDefinition.SpecLen = env.ParseShort(attribute.InnerText.Trim());
				break;
			case "NextCodeParamId":
				treeDefinition.NextCodeParamId = env.ParseShort(attribute.InnerText.Trim());
				break;
			case "CacheId":
				treeDefinition.CacheId = env.ParseShort(attribute.InnerText.Trim());
				break;
			}
		}
		if (!env.IsIdentifier(treeDefinition.Name))
		{
			throw new ASException(string.Format(Resources.InvalidIdentifier, treeDefinition.Name));
		}
		if (treeDefinition.Name.Length > 8)
		{
			throw new ASException(string.Format(Resources.InvalidTreeNameLength, treeDefinition.Name, 1, (short)8));
		}
		if (!env.IsIdentifier(treeDefinition.CaptionId))
		{
			throw new ASException(string.Format(Resources.InvalidIdentifier, treeDefinition.CaptionId));
		}
		if (!env.IsIdentifier(treeDefinition.NodeCaptionId))
		{
			throw new ASException(string.Format(Resources.InvalidIdentifier, treeDefinition.NodeCaptionId));
		}
		if (!string.IsNullOrWhiteSpace(treeDefinition.CodeCaptionId) && !env.IsIdentifier(treeDefinition.CodeCaptionId))
		{
			throw new ASException(string.Format(Resources.InvalidIdentifier, treeDefinition.CodeCaptionId));
		}
		if (!string.IsNullOrWhiteSpace(treeDefinition.SpecCaptionId) && !env.IsIdentifier(treeDefinition.SpecCaptionId))
		{
			throw new ASException(string.Format(Resources.InvalidIdentifier, treeDefinition.SpecCaptionId));
		}
		if (treeDefinition.CodeLen < 1 || treeDefinition.CodeLen > 20)
		{
			throw new ASException(string.Format(Resources.InvalidTreeCodeLength, treeDefinition.Name, 1, (short)20));
		}
		if (treeDefinition.CaptionLen < 1 || treeDefinition.CaptionLen > 150)
		{
			throw new ASException(string.Format(Resources.InvalidTreeCaptionLength, treeDefinition.Name, 1, (short)150));
		}
		if (!string.IsNullOrWhiteSpace(treeDefinition.SpecCaptionId) && treeDefinition.SpecLen > 255)
		{
			throw new ASException(string.Format(Resources.InvalidTreeSpecLength, treeDefinition.Name, 1, (short)255));
		}
		if (!treeDefinition.Multilevel && treeDefinition.PrefixBased)
		{
			treeDefinition.PrefixBased = false;
		}
		if (!treeDefinition.System && !treeDefinition.Name.StartsWith("UD_"))
		{
			throw new ASException("Cannot add tree without UD prefix");
		}
		return treeDefinition;
	}

	public TreeDefinition Get(string name)
	{
		return Settings.CreateApiClient().ExtensionsRoutes.GetTreeDefinition(new TreeDefinitionRequestModel
		{
			Name = name
		});
	}

	public string LoadXml(string name)
	{
		string extension = Settings.CoreCacheService.Tree.GetItem(name).Extension;
		if (!string.IsNullOrWhiteSpace(extension))
		{
			return extension;
		}
		return Settings.CreateApiClient().ExtensionsRoutes.LoadTreeXml(name);
	}

	public TreeExtensionDefinitionEx LoadExtensionDefinition(string name)
	{
		string extension = Settings.CoreCacheService.Tree.GetItem(name).Extension;
		if (!string.IsNullOrWhiteSpace(extension))
		{
			return new TreeExtensionDefinitionEx(extension, new Environment());
		}
		return null;
	}

	public void StoreExtensionXml(TreeExtensionModel model, bool updateCache = true)
	{
		Settings.CreateApiClient().ExtensionsRoutes.StoreTreeExtensionXml(model);
		if (updateCache)
		{
			Settings.CoreCacheService.Tree.Refresh();
		}
	}

	public void DeleteExtension(TreeExtensionModel model)
	{
		Settings.CreateApiClient().ExtensionsRoutes.DeleteExtension(model);
		Settings.CoreCacheService.Tree.Refresh();
	}
}
