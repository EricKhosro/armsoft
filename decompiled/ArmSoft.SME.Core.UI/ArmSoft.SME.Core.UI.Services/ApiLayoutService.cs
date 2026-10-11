using System.Collections.Generic;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common.Models.Layout;

namespace ArmSoft.SME.Core.UI.Services;

public class ApiLayoutService : ILayoutService
{
	private static ApiClient Client => Settings.CreateApiClient();

	public bool Store(LayoutRow layout)
	{
		return Client.Layout.Store(layout);
	}

	public List<LayoutRow> GetList(string name)
	{
		return Client.Layout.GetList(name);
	}

	public bool Delete(LayoutRow layout)
	{
		return Client.Layout.Delete(layout);
	}

	public bool EditCaption(LayoutRow layout, string newCaption)
	{
		return Client.Layout.EditCaption(layout, newCaption);
	}

	public string GetBody(LayoutRow layout)
	{
		return Client.Layout.GetBody(layout);
	}

	public bool IsDeleteLayoutRow(string name, string caption)
	{
		return Client.Layout.IsDeleteLayoutRow(name, caption);
	}

	public bool ExistsLayoutRow(string name, string caption)
	{
		return Client.Layout.ExistsLayoutRow(name, caption);
	}
}
