using System.Xml;
using ArmSoft.SME.Common.MetaData;

namespace ArmSoft.SME.Core.UI.MetaData;

public interface IAWPService
{
	void Parse(AWPDefinition definition, XmlElement node, Environment env);

	void Parse(AWPDefinition definition, string node, Environment env);

	void Store(AWPDefinition definition);

	AWPDefinition Load(string name);

	string LoadXML(string name);

	void Delete(string name);
}
