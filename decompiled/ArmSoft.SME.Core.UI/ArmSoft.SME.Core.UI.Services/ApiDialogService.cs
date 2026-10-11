using System.Collections.Generic;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common.Models.Dialog;

namespace ArmSoft.SME.Core.UI.Services;

public class ApiDialogService : IDialogService
{
	private static ApiClient Client => Settings.CreateApiClient();

	public void StoreValues(string name, DialogValues values)
	{
		Client.Dialog.StoreValues(name, values);
	}

	public DialogValues GetValues(string name)
	{
		return Client.Dialog.GetValues(name);
	}

	public List<DialogValuesTemplate> GetTemplates(string group, bool forCurrentUser)
	{
		return Client.Dialog.GetTemplates(group, forCurrentUser);
	}

	public int StoreTemplate(string group, DialogValuesTemplate template)
	{
		return Client.Dialog.StoreTemplate(new DialogValuesTemplateStoreRequest
		{
			Group = group,
			Template = template
		});
	}

	public void UpdateTemplates(string group, List<DialogValuesTemplate> templates)
	{
		Client.Dialog.UpdateTemplates(new DialogValuesTemplatesUpdateRequest
		{
			Group = group,
			Templates = templates
		});
	}

	public List<int> CopyTemplate(int id, IEnumerable<string> userNames)
	{
		return Client.Dialog.CopyTemplate(id, userNames);
	}

	public void DeleteTemplate(int id)
	{
		Client.Dialog.DeleteTemplate(id);
	}

	public void DeleteTemplates(IEnumerable<int> ids)
	{
		Client.Dialog.DeleteTemplates(ids);
	}
}
