using System.Collections.Generic;
using System.Xml;
using ArmSoft.SME.Common.MetaData;

namespace ArmSoft.SME.Core.UI.MetaData;

public interface IContextFunctionService
{
	void Parse(ContextFunctionDefinition definition, XmlElement node, Environment environment);

	void Parse(ContextFunctionDefinition definition, string xml, Environment environment);

	void Store(ContextFunctionDefinition definition, string xml);

	string LoadXML(string group, string name);

	ContextFunctionDefinition Load(string group, string name);

	List<string> Load(string group);

	void Delete(string group, string name);

	void ChangeEnabled(string group, string name, bool value);
}
