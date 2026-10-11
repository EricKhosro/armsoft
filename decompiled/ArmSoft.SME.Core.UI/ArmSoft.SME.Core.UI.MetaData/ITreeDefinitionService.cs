using System.Xml;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.MetaData.Tree;
using ArmSoft.SME.Core.UI.MetaData.Tree;

namespace ArmSoft.SME.Core.UI.MetaData;

public interface ITreeDefinitionService
{
	void Store(TreeDefinition definition);

	void Delete(string name);

	TreeDefinition Get(string name);

	TreeDefinition Parse(XmlElement node, Environment env);

	string LoadXml(string name);

	TreeExtensionDefinitionEx LoadExtensionDefinition(string name);

	void StoreExtensionXml(TreeExtensionModel model, bool updateCache = true);

	void DeleteExtension(TreeExtensionModel model);
}
