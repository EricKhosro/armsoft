using System.Collections.Generic;
using ArmSoft.SME.Common.MetaData;

namespace ArmSoft.SME.Core.UI.Services;

public interface IStringResourceService
{
	string GetStringResource(string captionId);

	List<KeyValuePair<string, string>> GetSystemStringResources();

	StringResource Load(string captionId);

	void Store(List<StringResource> resources);

	void Update(StringResource resource);

	void Delete(string name);
}
