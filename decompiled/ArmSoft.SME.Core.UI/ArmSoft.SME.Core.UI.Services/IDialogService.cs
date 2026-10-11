using System.Collections.Generic;
using ArmSoft.SME.Common.Models.Dialog;

namespace ArmSoft.SME.Core.UI.Services;

public interface IDialogService
{
	void StoreValues(string name, DialogValues values);

	DialogValues GetValues(string name);

	List<DialogValuesTemplate> GetTemplates(string group, bool forCurrentUser);

	int StoreTemplate(string group, DialogValuesTemplate template);

	void UpdateTemplates(string group, List<DialogValuesTemplate> templates);

	List<int> CopyTemplate(int id, IEnumerable<string> userNames);

	void DeleteTemplate(int id);

	void DeleteTemplates(IEnumerable<int> ids);
}
