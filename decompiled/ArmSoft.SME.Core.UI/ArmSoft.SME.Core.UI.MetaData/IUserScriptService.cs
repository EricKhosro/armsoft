using System.Xml;
using ArmSoft.SME.Common.MetaData;

namespace ArmSoft.SME.Core.UI.MetaData;

public interface IUserScriptService
{
	void Parse(UserScriptDefinition definition, XmlElement node, Environment env);

	void Parse(UserScriptDefinition definition, string node, Environment env);

	void Store(UserScriptDefinition definition);

	UserScriptDefinition Load(string name);

	string LoadXML(string name);

	void Delete(string name);
}
