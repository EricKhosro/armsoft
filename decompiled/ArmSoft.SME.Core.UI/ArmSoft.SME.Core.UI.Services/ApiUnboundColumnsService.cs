using System.Collections.Generic;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common.Models.UnboundColumns;

namespace ArmSoft.SME.Core.UI.Services;

public class ApiUnboundColumnsService : IUnboundColumnsService
{
	public ApiClient Client => Settings.CreateApiClient();

	public UnboundColumn Get(string group, string name)
	{
		return Client.UnboundColumns.Get(group, name);
	}

	public List<UnboundColumn> Get(string group)
	{
		return Client.UnboundColumns.GetList(group);
	}

	public bool Store(UnboundColumn column)
	{
		return Client.UnboundColumns.Store(column);
	}

	public bool Delete(UnboundColumn column)
	{
		return Client.UnboundColumns.Delete(column);
	}
}
