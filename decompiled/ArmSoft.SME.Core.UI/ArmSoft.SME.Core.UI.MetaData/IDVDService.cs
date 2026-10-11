using System.Collections.Generic;
using System.Xml;

namespace ArmSoft.SME.Core.UI.MetaData;

public interface IDVDService
{
	void Parse(DataViewDefinition definition, XmlElement xmlElement, Environment env);

	void Parse(DataViewDefinition definition, string xml, Environment env);

	void Store(DataViewDefinition definition, string xml);

	string LoadXML(string group, string name);

	DataViewDefinition Load(string group, string name);

	DataViewDefinition LoadDefault(string group);

	List<string> Load(string group);

	void Delete(string group, string name);

	void ChangeEnabled(string group, string name, bool value);

	void ChangeDefault(string group, string name, bool value);

	void UpdateLastExecDate(string name);
}
